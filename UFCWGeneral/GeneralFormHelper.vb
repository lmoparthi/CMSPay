
Option Infer On
Option Strict Off

Imports System.Configuration
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.Win32

Partial Class UFCWGeneral

    Public Shared Function SetFormPosition(ByRef frm As Form, Optional appKEY As String = "") As Boolean

        Dim FName As String
        Dim FSize As Single
        Dim FStyle As FontStyle
        Dim FUnit As GraphicsUnit
        Dim FCharset As Byte
        Dim InRegistry As Boolean

        Try
            If appKEY = "" Then appKEY = If(ConfigurationManager.AppSettings("AppKey"), "")

            Using key As RegistryKey = Registry.CurrentUser.OpenSubKey($"SOFTWARE\VB and VBA Program Settings\{appKEY}")

                If key IsNot Nothing Then
                    For Each valueName As String In key.GetSubKeyNames()
                        InRegistry = True

                        Debug.Print($"Key: {valueName}, Value: {key.GetValue(valueName)}")

                        Exit For
                    Next
                Else
                    Debug.Print($"The specified registry path '{appKEY}' does not exist.")
                End If
            End Using

            If appKEY.Length > 0 AndAlso InRegistry Then

                frm.Top = If(CInt(GetSetting(appKEY, frm.Name & "\Settings", "Top", frm.Top.ToString)) < 0, 0, CInt(GetSetting(appKEY, frm.Name & "\Settings", "Top", frm.Top.ToString)))
                frm.Height = CInt(GetSetting(appKEY, frm.Name & "\Settings", "Height", frm.Height.ToString))
                frm.Left = If(CInt(GetSetting(appKEY, frm.Name & "\Settings", "Left", frm.Left.ToString)) < 0, 0, CInt(GetSetting(appKEY, frm.Name & "\Settings", "Left", frm.Left.ToString)))
                frm.Width = CInt(GetSetting(appKEY, frm.Name & "\Settings", "Width", frm.Width.ToString))
                frm.WindowState = CType(GetSetting(appKEY, frm.Name & "\Settings", "WindowState", CInt(frm.WindowState).ToString), FormWindowState)

                FStyle = New FontStyle
                FUnit = New GraphicsUnit

                FName = GetSetting(appKEY, frm.Name & "\Settings", "FontName", frm.Font.Name)
                FSize = CSng(GetSetting(appKEY, frm.Name & "\Settings", "FontSize", CStr(frm.Font.Size)))

                FStyle = CType(GetSetting(appKEY, frm.Name & "\Settings", "FontStyle", CStr(frm.Font.Style)), FontStyle)
                FUnit = CType(GetSetting(appKEY, frm.Name & "\Settings", "FontUnit", CStr(frm.Font.Unit)), GraphicsUnit)
                FCharset = CByte(GetSetting(appKEY, frm.Name & "\Settings", "FontCharset", CStr(frm.Font.GdiCharSet)))

                frm.Font = New Font(FName, FSize, FStyle, FUnit, FCharset)

            End If

            Return IsOnScreen(frm)

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Sub SaveFormPosition(frm As Form, Optional appKEY As String = "")

        Try

            If appKEY = "" Then appKEY = If(ConfigurationManager.AppSettings("AppKey"), "")

            frm.WindowState = FormWindowState.Normal
            SaveSetting(appKEY, frm.Name & "\Settings", "Top", CStr(frm.Top))
            SaveSetting(appKEY, frm.Name & "\Settings", "Height", CStr(frm.Height))
            SaveSetting(appKEY, frm.Name & "\Settings", "Left", CStr(frm.Left))
            SaveSetting(appKEY, frm.Name & "\Settings", "Width", CStr(frm.Width))

            SaveSetting(appKEY, frm.Name & "\Settings", "FontName", frm.Font.Name)
            SaveSetting(appKEY, frm.Name & "\Settings", "FontSize", CStr(frm.Font.Size))
            SaveSetting(appKEY, frm.Name & "\Settings", "FontStyle", CStr(frm.Font.Style))
            SaveSetting(appKEY, frm.Name & "\Settings", "FontUnit", CStr(frm.Font.Unit))
            SaveSetting(appKEY, frm.Name & "\Settings", "FontCharset", CStr(frm.Font.GdiCharSet))

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Shared Function IsOnScreen(ByVal form As Form) As Boolean
        ' Create rectangle
        Dim formRectangle As New Rectangle(form.Left, form.Top, form.Width, form.Height)

        ' Test
        Return Screen.AllScreens.Any(Function(s) s.WorkingArea.IntersectsWith(formRectangle))
    End Function

End Class
