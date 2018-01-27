Imports MaterialSkin
Imports System.Net

Public Class log
    Private Sub log_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim SkinManager As MaterialSkinManager = MaterialSkinManager.Instance
        SkinManager.AddFormToManage(Me)
        SkinManager.Theme = MaterialSkinManager.Themes.LIGHT
        SkinManager.ColorScheme = New ColorScheme(Primary.Green400, Primary.Green500, Primary.Green500, Accent.LightGreen200, TextShade.WHITE)

    End Sub
End Class