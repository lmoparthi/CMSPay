Public Class DentalHistory

    Private Const _APPKEY As String = "UFCW\Dental"

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If String.IsNullOrEmpty(FamilyIDTextBox.Text) And SSNTextBox.Text <> "" Then
            DentalControl1.LoadDentalControl(CDate("2000-01-01"), CDate("2021-06-28"), CType(SSNTextBox.Text, Integer))
        Else
            DentalControl1.FamilyID = CType(FamilyIDTextBox.Text, Integer)
            If String.IsNullOrEmpty(RelationIDTextBox.Text) Then
                DentalControl1.LoadDentalControl(CDate("1998-01-01"), CDate("2021-06-28"), CType(FamilyIDTextBox.Text, Integer))
                DentalControl1.LoadPENDDentalControl(CType(FamilyIDTextBox.Text, Integer), Nothing)
                DentalControl1.LoadPREAuthDentalControl(CType(FamilyIDTextBox.Text, Integer), Nothing)

            Else
                DentalControl1.LoadDentalControl(CDate("2000-01-01"), CDate("2021-06-28"), CType(FamilyIDTextBox.Text, Integer), CType(RelationIDTextBox.Text, Short), Nothing)
                DentalControl1.LoadPENDDentalControl(CType(FamilyIDTextBox.Text, Integer), CType(RelationIDTextBox.Text, Short))
                DentalControl1.LoadPREAuthDentalControl(CType(FamilyIDTextBox.Text, Integer), CType(RelationIDTextBox.Text, Short))

            End If
        End If

    End Sub

    Private Sub DentalHistory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SSNTextBox.Enabled = False

        If Not UFCWGeneral.SetFormPosition(Me, _APPKEY) Then Me.CenterToScreen()

        'SetSettings()

        Me.DentalControl1.AppKey = _APPKEY

        Me.MainStatusBar.Location = New Point(0, 720)
        Me.MainStatusBar.Size = New Size(546, 12)

        Me.DomainUserStatusBarPanel.Text = SystemInformation.UserName
        Me.DataStatusBarPanel.Text = "Server=" & CMSDALCommon.GetServerName(Nothing) & ";DB=" & CMSDALCommon.GetDatabaseName(Nothing)
        Me.DateStatusBarPanel.Text = Format(UFCWGeneral.NowDate, "MM-dd-yyyy")

        Me.InfoStatusBarPanel.Text = "Enter Search Criteria and press Enter to continue"
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        FamilyIDTextBox.Text = ""
        RelationIDTextBox.Text = ""
        SSNTextBox.Text = ""
        DentalControl1.ClearAll()
    End Sub

    Private Sub SSNCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles SSNCheckBox.CheckedChanged
        If SSNCheckBox.Checked Then
            FamilyIDTextBox.Enabled = False
            RelationIDTextBox.Enabled = False
            FamilyIDTextBox.Text = ""
            RelationIDTextBox.Text = ""
            SSNTextBox.Enabled = True
            SSNTextBox.Select()
        Else
            FamilyIDTextBox.Enabled = True
            RelationIDTextBox.Enabled = True
            SSNTextBox.Enabled = False
            SSNTextBox.Text = ""
            FamilyIDTextBox.Select()
        End If
    End Sub
    Private Sub SetSettings()
        Dim FName As String = ""
        Dim FSize As Single
        Dim FStyle As FontStyle
        Dim FUnit As GraphicsUnit
        Dim FCharset As Byte

        Try

            FStyle = New FontStyle
            FUnit = New GraphicsUnit

            Me.Visible = False

            Me.Top = If(CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)))
            Me.Height = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString))
            Me.Left = If(CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)))
            Me.Width = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString))
            Me.WindowState = CType(GetSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)

            FName = GetSetting(_APPKEY, Me.Name & "\Settings", "FontName", Me.Font.Name)
            FSize = CSng(GetSetting(_APPKEY, Me.Name & "\Settings", "FontSize", CStr(Me.Font.Size)))
            FStyle = CType(GetSetting(_APPKEY, Me.Name & "\Settings", "FontStyle", CStr(Me.Font.Style)), FontStyle)
            FUnit = CType(GetSetting(_APPKEY, Me.Name & "\Settings", "FontUnit", CStr(Me.Font.Unit)), GraphicsUnit)
            FCharset = CByte(GetSetting(_APPKEY, Me.Name & "\Settings", "FontCharset", CStr(Me.Font.GdiCharSet)))

            Me.Font = New Font(FName, FSize, FStyle, FUnit, FCharset)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DentalHistory_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        UFCWGeneral.SaveFormPosition(Me, _APPKEY)

    End Sub
End Class