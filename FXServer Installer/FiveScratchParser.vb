'WORK IN PROGRESS

'Imports System.IO

'Public Class FiveScratchParser
'    Public Sub Parse(f As String, d As String)
'        'Loop trough all the lines
'        For Each Line As String In File.ReadLines(f)
'            'check the command
'            Select Case True
'                Case Line.StartsWith("ADD")
'                    Try
'                        Dim s As String() = Line.Split(" ")

'                        Dim filename As String = Path.Combine(d, s(1))
'                        Dim l = Integer.Parse(s(2))
'                        Dim p1 As Integer = Integer.Parse(s(3)) - 1
'                        Dim d1 = File.ReadAllLines(filename)
'                        Dim d2 = d1(p1).Concat(vbCrLf + Line.Replace("ADD" + filename + l + p1, ""))
'                        d1(p1) = d2
'                        For Each x In d1
'                            If File.Exists(filename) Then
'                                File.Delete(filename)
'                            End If

'                            File.AppendAllText(filename, x + vbCrLf))

'                        Next
'                    Catch ex As FileNotFoundException
'                        MsgBox("FXInstaller has  thrown a file not found exception. You should check to see if any file is missing. For debug purposes, here's the whole error: " + ex.ToString())
'                        End
'                    Catch ex As UnauthorizedAccessException
'                        MsgBox("The installer couldn't access one or more files, are you in a admin-only folder? :/")
'                        End
'                    End Try

'                Case Line.StartsWith("REPLACE")
'                    Try
'                        Dim s As String() = Line.Split(" ")

'                        Dim filename As String = Path.Combine(d, s(1))
'                        Dim l = Integer.Parse(s(2))
'                        Dim p1 As Integer = Integer.Parse(s(3)) + 1
'                        Dim d1 = File.ReadAllLines(filename)
'                        Dim d2 = Line.Replace("REPLACE" + filename + l + p1, "")
'                        d1(p1) = d2
'                        For Each x In d1
'                            If File.Exists(filename) Then
'                                File.Delete(filename)
'                            End If

'                            File.AppendAllText(filename, x + vbCrLf))

'                        Next
'                    Catch ex As FileNotFoundException
'                        MsgBox("FXInstaller has  thrown a file not found exception. You should check to see if any file is missing. For debug purposes, here's the whole error: " + ex.ToString())
'                        End
'                    Catch ex As UnauthorizedAccessException
'                        MsgBox("The installer couldn't access one or more files, are you in a admin-only folder? :/")
'                        End
'                    End Try
'                Case Else

'            End Select
'        Next
'    End Sub
'End Class
