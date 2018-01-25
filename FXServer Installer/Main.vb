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
            If htmlNodes(i).Attributes("href").Value.IndexOf(ComboBox1.SelectedText + "-") = -1 Then
                Continue For
            Else

                remoteUri = "https://runtime.fivem.net/artifacts/fivem/build_server_windows/master/" + htmlNodes(i).Attributes("href").Value + "server.zip"


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
    End Sub
End Class