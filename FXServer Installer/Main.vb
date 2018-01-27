Imports MaterialSkin
Imports HtmlAgilityPack
Imports System.IO
Imports System.Threading
Imports Ionic.Zip
Imports System.ComponentModel
Imports Link.AppShortcut
Imports Link.InternetShortcut

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms

Public Class Main
    Dim remoteUri As String
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MaterialTabControl1.TabPages.Remove(TabPage2)
        Dim SkinManager As MaterialSkinManager = MaterialSkinManager.Instance
        SkinManager.AddFormToManage(Me)
        SkinManager.Theme = MaterialSkinManager.Themes.LIGHT
        SkinManager.ColorScheme = New ColorScheme(Primary.Green400, Primary.Green500, Primary.Green500, Accent.LightGreen200, TextShade.WHITE)
        Dim webClient As New System.Net.WebClient
        Dim result As String = webClient.DownloadString("https://runtime.fivem.net/artifacts/fivem/build_server_windows/master/")
        Dim doc As New HtmlAgilityPack.HtmlDocument()
        doc.LoadHtml(result)
        Dim htmlNodes = doc.DocumentNode.SelectNodes("//pre/a")
        For i As Integer = htmlNodes.Count - 1 To 0 Step -1
            If htmlNodes(i).Attributes("href").Value = "../" Then
                Continue For
            Else
                ComboBox1.Items.Add(htmlNodes(i).Attributes("href").Value.Split("-")(0))

            End If
        Next i
        ComboBox1.CheckForIllegalCrossThreadCalls = False
        TextBox1.CheckForIllegalCrossThreadCalls = False
        TextBox2.CheckForIllegalCrossThreadCalls = False
        TextBox3.CheckForIllegalCrossThreadCalls = False
        TextBox4.CheckForIllegalCrossThreadCalls = False
    End Sub

    Private Sub TabPage1_Click(sender As Object, e As EventArgs) Handles TabPage1.Click

    End Sub

    Private Sub MaterialLabel1_Click(sender As Object, e As EventArgs) Handles MaterialLabel1.Click

    End Sub
    Sub CreateShortCut()
        Dim objShell, strDesktopPath, objLink
        objShell = CreateObject("WScript.Shell")
        strDesktopPath = objShell.SpecialFolders("Desktop")
        objLink = objShell.CreateShortcut(strDesktopPath & "\mylink.lnk")
        objLink.Arguments = "+exec server.cfg"
        objLink.Description = "Start Fivem Server!"
        objLink.TargetPath = Path.Combine(Application.StartupPath(), "fivemserver\run.cmd")
        objLink.WindowStyle = 1
        objLink.WorkingDirectory = Path.Combine(Application.StartupPath(), "fivemserver")
        objLink.Save
    End Sub
    Private Sub MaterialRaisedButton1_Click(sender As Object, e As EventArgs) Handles MaterialRaisedButton1.Click
        MaterialRaisedButton1.Enabled = False
        log.Show()
        BackgroundWorker1.RunWorkerAsync()

    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As DoWorkEventArgs) Handles BackgroundWorker1.DoWork

        MsgBox("The program will now install FXServer onto your machine.")
        Dim webClient As New System.Net.WebClient
        Dim result As String = webClient.DownloadString("https://runtime.fivem.net/artifacts/fivem/build_server_windows/master/")
        Dim doc As New HtmlAgilityPack.HtmlDocument()
        Invoke(Sub() log.TextBox1.AppendText("searching for fxserver version " + ComboBox1.Text + vbCrLf))
        doc.LoadHtml(result)
        Dim htmlNodes = doc.DocumentNode.SelectNodes("//pre/a")
        For i As Integer = htmlNodes.Count - 1 To 0 Step -1
            If htmlNodes(i).Attributes("href").Value.IndexOf(ComboBox1.Text + "-") = -1 Then
                Continue For
            Else

                remoteUri = "https://runtime.fivem.net/artifacts/fivem/build_server_windows/master/" + htmlNodes(i).Attributes("href").Value + "server.zip"
                Exit For

            End If
        Next i
        Invoke(Sub() log.TextBox1.AppendText("download link generated: " + remoteUri + vbCrLf))
        Invoke(Sub() log.TextBox1.AppendText("downloading resources..." + vbCrLf))
        webClient.DownloadFile("https://github.com/citizenfx/cfx-server-data/archive/master.zip", "resources.zip")
        Invoke(Sub() log.TextBox1.AppendText("done downloading resources!" + vbCrLf))
        Invoke(Sub() log.TextBox1.AppendText("downloading vcredist installer..." + vbCrLf))
        webClient.DownloadFile("https://download.visualstudio.microsoft.com/download/pr/11100230/15ccb3f02745c7b206ad10373cbca89b/VC_redist.x64.exe", "v.exe")
        Invoke(Sub() log.TextBox1.AppendText("done downloading vcredist!" + vbCrLf))
        Invoke(Sub() log.TextBox1.AppendText(remoteUri))
        Dim fileName As String = "server.zip"
        Dim myStringWebResource As String = Nothing
        Invoke(Sub() log.TextBox1.AppendText("downloading FXServer zip package..." + vbCrLf))
        webClient.DownloadFile(remoteUri, fileName)
        Invoke(Sub() log.TextBox1.AppendText("done downloading FXServer!" + vbCrLf))
        Invoke(Sub() log.TextBox1.AppendText("checking if vcredist is already installed..." + vbCrLf))
        Dim w As String = Environment.GetEnvironmentVariable("windir")
        If File.Exists(Path.Combine(w, "SysWOW64\msvcp140.dll")) Then
            Invoke(Sub() log.TextBox1.AppendText("vcredist was found on your system!" + vbCrLf))
        Else
            Invoke(Sub() log.TextBox1.AppendText("vcredist not found! starting the installer..." + vbCrLf))
            Process.Start("v.exe")
        End If
        While Process.GetProcessesByName("v.exe").Count > 0
            Thread.Sleep(1)
        End While
        Invoke(Sub() log.TextBox1.AppendText("vcredist was installed, continuing." + vbCrLf))
        Try
            Invoke(Sub() log.TextBox1.AppendText("extracting FXServer..." + vbCrLf))
            Using zip As ZipFile =
                ZipFile.Read("server.zip")
                ' Loop through the archive's files.
                For Each zip_entry As ZipEntry In zip
                    zip_entry.Extract(Path.Combine(Application.StartupPath(), "fivemserver"))
                Next zip_entry
            End Using

            log.TextBox1.AppendText(("done!" + vbCrLf))
        Catch ex As Exception
            MessageBox.Show("Error extracting archive.\n" + ex.Message)
            Invoke(Sub() log.TextBox1.AppendText("error extracting archive!" + ex.Message + vbCrLf))
        End Try
        Try
            Invoke(Sub() log.TextBox1.AppendText("extracting resources..." + vbCrLf))
            Using zip As ZipFile =
                ZipFile.Read("resources.zip")
                ' Loop through the archive's files.
                For Each zip_entry As ZipEntry In zip
                    zip_entry.Extract(Path.Combine(Application.StartupPath(), "r"))
                Next zip_entry
            End Using

            Invoke(Sub() log.TextBox1.AppendText("done!" + vbCrLf))
        Catch ex As Exception
            MessageBox.Show("Error extracting archive.\n" + ex.Message)
        End Try
        Dim r As String = Path.Combine(Application.StartupPath, "r", "cfx-server-data-master", "resources")
        Try
            Invoke(Sub() log.TextBox1.AppendText("moving resources to the right directory!" + vbCrLf))
            Directory.Move(r, Path.Combine(Application.StartupPath, "fivemserver\resources"))

        Catch ex As DirectoryNotFoundException
            Invoke(Sub() log.TextBox1.AppendText("failed! the directory wansn't found... :/" + vbCrLf))
            MsgBox("the program had an unexpected error, the resources directory wasn't found :/")
            End
        End Try
        Invoke(Sub() log.TextBox1.AppendText("writing server.cfg..." + vbCrLf))
        Dim cfg As String = "# you probably don't want to change these!
# only change them if you're using a server with multiple network interfaces
endpoint_add_tcp " & ControlChars.Quote & "0.0.0.0:" & TextBox1.Text & ControlChars.Quote & "
endpoint_add_udp  " & ControlChars.Quote & "0.0.0.0:" & TextBox1.Text & ControlChars.Quote & "

start mapmanager
start chat
start spawnmanager
start sessionmanager
start fivem
start hardcap
start rconlog
start scoreboard
start playernames

sv_scriptHookAllowed 1

# change this
#rcon_password " & TextBox3.Text & "

sv_hostname " & ControlChars.Quote & TextBox2.Text & ControlChars.Quote & "

# nested configs!
#exec server_internal.cfg

# loading a server icon (96x96 PNG file)
#load_server_icon myLogo.png

# convars for use from script
set temp_convar " & ControlChars.Quote & "hey world!" & ControlChars.Quote & "

# disable announcing? clear out the master by uncommenting this
#sv_master1 " & ControlChars.Quote & ControlChars.Quote & "

# want to only allow players authenticated with a third-party provider like Steam?
#sv_authMaxVariance 1
#sv_authMinTrust 5

# add system admins
add_ace group.admin command allow # allow all commands
add_ace group.admin command.quit deny # but don't allow quit
add_principal identifier.steam:110000112345678 group.admin # add the admin to the group

# remove the # to hide player endpoints in external log output
#sv_endpointprivacy true

# server slots limit (must be between 1 and 31)
sv_maxclients 30

# license key for server (https://keymaster.fivem.net)
sv_licenseKey " & TextBox4.Text
        Dim parts As String() = cfg.Split(New String() {Environment.NewLine}, StringSplitOptions.None)
        For Each part In parts
            File.AppendAllText(Path.Combine(Application.StartupPath(), "fivemserver\server.cfg"), part + vbCrLf)

        Next
        Invoke(Sub() log.TextBox1.AppendText("done!" + vbCrLf))
        Invoke(Sub() log.TextBox1.AppendText("creating shortcut..." + vbCrLf))
        Call CreateShortCut()
        Invoke(Sub() log.TextBox1.AppendText("FXserver was installed correctly onto your machine." + vbCrLf))
        MaterialRaisedButton1.Enabled = True
    End Sub
    'WORK IN PROGRESS!
    'Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
    '    OpenFileDialog1.ShowDialog()
    'End Sub

    'Private Sub OpenFileDialog1_FileOk(sender As Object, e As CancelEventArgs) Handles OpenFileDialog1.FileOk
    '    TextBox7.Text = OpenFileDialog1.FileName
    'End Sub

    'Private Sub MaterialRaisedButton3_Click(sender As Object, e As EventArgs) Handles MaterialRaisedButton3.Click
    '    If Directory.Exists(Path.Combine(Application.StartupPath, "fivemserver/cache")) Then
    '        Directory.Delete((Path.Combine(Application.StartupPath, "fivemserver/cache")))
    '    End If
    'End Sub

    'Private Sub MaterialRaisedButton6_Click(sender As Object, e As EventArgs) Handles MaterialRaisedButton6.Click
    '    Process.Start(Path.Combine(Application.StartupPath, "fivemserver\run.cmd +exec server.cfg"))
    'End Sub

    'Private Sub MaterialRaisedButton2_Click(sender As Object, e As EventArgs) Handles MaterialRaisedButton2.Click
    '    Dim name As String = TextBox4.Text
    '    Dim dl As String = TextBox5.Text
    '    Dim ins As String = TextBox6.Text
    '    Dim n As Integer = CInt(Math.Ceiling(Rnd() * 200000)) + 1
    '    Dim w As System.Net.WebClient = New System.Net.WebClient
    '    w.DownloadFile(dl, n.ToString() + ".zip")
    '    Try
    '        Using zip As ZipFile =
    '            ZipFile.Read("server.zip")
    '            ' Loop through the archive's files.
    '            For Each zip_entry As ZipEntry In zip
    '                If zip_entry.IsDirectory Then
    '                    If zip_entry.FileName() = name OrElse zip_entry.FileName = name + "-master" Then

    '                    End If

    '                End If
    '                    zip_entry.Extract(Path.Combine(Application.StartupPath(), "fivemserver"))
    '            Next zip_entry
    '        End Using

    '        MessageBox.Show("Done")
    '    Catch ex As Exception
    '        MessageBox.Show("Error extracting archive.\n" + ex.Message)
    '    End Try
    'End Sub
End Class