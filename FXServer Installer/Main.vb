Imports MaterialSkin
Imports HtmlAgilityPack
Imports System.IO
Imports System.Threading
Imports Ionic.Zip

Public Class Main
    Dim remoteUri As String
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim SkinManager As MaterialSkinManager = MaterialSkinManager.Instance
        SkinManager.AddFormToManage(Me)
        SkinManager.Theme = MaterialSkinManager.Themes.LIGHT
        SkinManager.ColorScheme = New ColorScheme(Primary.Green400, Primary.Green500, Primary.Green500, Accent.LightGreen200, TextShade.WHITE)
        Dim webClient As New System.Net.WebClient
        Dim result As String = webClient.DownloadString("https://runtime.fivem.net/artifacts/fivem/build_server_windows/master/")
        Dim doc As New HtmlDocument()
        doc.LoadHtml(result)
        Dim htmlNodes = doc.DocumentNode.SelectNodes("//pre/a")
        For i As Integer = htmlNodes.Count - 1 To 0 Step -1
            If htmlNodes(i).Attributes("href").Value = "../" Then
                Continue For
            Else
                ComboBox1.Items.Add(htmlNodes(i).Attributes("href").Value.Split("-")(0))

            End If
        Next i
    End Sub

    Private Sub TabPage1_Click(sender As Object, e As EventArgs) Handles TabPage1.Click

    End Sub

    Private Sub MaterialLabel1_Click(sender As Object, e As EventArgs) Handles MaterialLabel1.Click

    End Sub

    Private Sub MaterialRaisedButton1_Click(sender As Object, e As EventArgs) Handles MaterialRaisedButton1.Click
        Dim webClient As New System.Net.WebClient
        Dim result As String = webClient.DownloadString("https://runtime.fivem.net/artifacts/fivem/build_server_windows/master/")
        Dim doc As New HtmlDocument()
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
        webClient.DownloadFile("https://github.com/citizenfx/cfx-server-data/archive/master.zip", "resources.zip")
        webClient.DownloadFile("https://download.visualstudio.microsoft.com/download/pr/11100230/15ccb3f02745c7b206ad10373cbca89b/VC_redist.x64.exe", "v.exe")

        Console.WriteLine(remoteUri)
        Dim fileName As String = "server.zip"
        Dim myStringWebResource As String = Nothing
        webClient.DownloadFile(remoteUri, fileName)
        Dim w As String = Environment.GetEnvironmentVariable("windir")
        If File.Exists(Path.Combine(w, "SysWOW64\msvcp140.dll")) Then
        Else
            Process.Start("v.exe")
        End If
        While Process.GetProcessesByName("v.exe").Count > 0
            Thread.Sleep(1)
        End While
        Try
            Using zip As ZipFile =
                ZipFile.Read("server.zip")
                ' Loop through the archive's files.
                For Each zip_entry As ZipEntry In zip
                    zip_entry.Extract(Path.Combine(Application.StartupPath(), "fivemserver"))
                Next zip_entry
            End Using

            MessageBox.Show("Done")
        Catch ex As Exception
            MessageBox.Show("Error extracting archive.\n" + ex.Message)
        End Try
        Try
            Using zip As ZipFile =
                ZipFile.Read("resources.zip")
                ' Loop through the archive's files.
                For Each zip_entry As ZipEntry In zip
                    zip_entry.Extract(Path.Combine(Application.StartupPath(), "r"))
                Next zip_entry
            End Using

            MessageBox.Show("Done")
        Catch ex As Exception
            MessageBox.Show("Error extracting archive.\n" + ex.Message)
        End Try
        Dim r As String = Path.Combine(Application.StartupPath, "r", "cfx-server-data-master", "resources")
        Try
            Directory.Move(r, Path.Combine(Application.StartupPath, "fivemserver\resources"))

        Catch ex As DirectoryNotFoundException
            MsgBox("the program had an unexpected error, the resources directory wasn't found :/")
            End
        End Try
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
    End Sub
End Class