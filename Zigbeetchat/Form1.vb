Imports System.IO.Ports
Public Class Form1
    Dim baud As String
    Dim port As String
    Dim str As String
    Dim atcmd As String
    Public WithEvents comPort As SerialPort
    Public Event ScanDataRecieved(ByVal data As String)

    Private Sub accueil_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Button1.Enabled = True
        Button9.Enabled = False
        Me.AcceptButton = Button2
    End Sub

#Region "Connexion"
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Button1.Enabled = False
        Button9.Enabled = True
        ComboBox1.Enabled = False
        ComboBox2.Enabled = False
        baud = ComboBox1.Text
        port = ComboBox2.Text
        Connect()
    End Sub

    Public Sub connect()
        Try
            comPort = My.Computer.Ports.OpenSerialPort(port, baud)
        Catch
            RichTextBox1.Text = "Impossible de se connecter"
        End Try

    End Sub
#End Region

#Region "Déconnexion"
    Private Sub RadButtonElement3_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Button1.Enabled = True
        Button9.Enabled = False
        ComboBox1.Enabled = True
        ComboBox2.Enabled = True
        Disconnect()
    End Sub

    Public Sub Disconnect()
        Try
            comPort.Close()
        Catch ex As Exception
            RichTextBox1.Text = RichTextBox1.Text & vbCrLf & "Impossible de se déconnecter"
        End Try
    End Sub
#End Region

#Region "Reception des données"
    Private Sub comPort_DataReceived(ByVal sender As Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) Handles comPort.DataReceived
        Try
            If e.EventType = SerialData.Chars Then
                Do
                    Try
                        Dim message As String = comPort.ReadLine
                        updateStatus("Reception : " & message)
                    Catch ex As TimeoutException
                        updateStatus(ex.ToString)
                    End Try
                Loop
            End If
            RaiseEvent ScanDataRecieved(str)
        Catch ex As Exception
            MsgBox("impossible de lire le flux entrant", MsgBoxStyle.Critical)
        End Try
    End Sub

    Public Delegate Sub updateStatusDelegate(ByVal newStatus As String)
    Public Sub updateStatus(ByVal newStatus As String)
        If Me.InvokeRequired Then
            Dim upbd As New updateStatusDelegate(AddressOf updateStatus)
            Me.Invoke(upbd, New Object() {newStatus})
        Else
            RichTextBox1.Text = newStatus & vbCrLf & vbCrLf & RichTextBox1.Text
        End If
    End Sub
#End Region

#Region "Envoie de données"
    Private Sub Send_btn_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            atcmd = RichTextBox2.Text
            RichTextBox2.Text = ""
            writecomport()
        Catch ex As Exception
            RichTextBox1.Text = "Impossible d'envoyer votre commande" & vbCrLf & RichTextBox1.Text
        End Try
    End Sub

    Public Sub writecomport()
        comPort.Write(atcmd & vbCrLf)
    End Sub
#End Region

#Region "commande bar"

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        RichTextBox2.Text = "AT"
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        RichTextBox2.Text = "AT+SN"
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        RichTextBox2.Text = "AT+ESCAN"
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        RichTextBox2.Text = "AT+BCAST,00="
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        RichTextBox2.Text = "ATI"
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        RichTextBox2.Text = "S1A = ATS1A?"
    End Sub
#End Region

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        RichTextBox1.Text = ""
    End Sub
End Class
