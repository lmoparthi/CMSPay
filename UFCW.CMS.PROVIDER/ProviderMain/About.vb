Imports System.IO

Public Class AboutMain
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Version As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Pic As System.Windows.Forms.PictureBox
    Friend WithEvents OK As System.Windows.Forms.Button
    Friend WithEvents Versions As System.Windows.Forms.ListBox
    Friend WithEvents cbUpdateProvider As System.Windows.Forms.CheckBox
    Friend WithEvents cbAddProvider As System.Windows.Forms.CheckBox
    Friend WithEvents cbAdministratorProvider As System.Windows.Forms.CheckBox
    Friend WithEvents cbDeleteProvider As System.Windows.Forms.CheckBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AboutMain))
        Me.OK = New System.Windows.Forms.Button
        Me.Pic = New System.Windows.Forms.PictureBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Version = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Versions = New System.Windows.Forms.ListBox
        Me.cbUpdateProvider = New System.Windows.Forms.CheckBox
        Me.cbAddProvider = New System.Windows.Forms.CheckBox
        Me.cbDeleteProvider = New System.Windows.Forms.CheckBox
        Me.cbAdministratorProvider = New System.Windows.Forms.CheckBox
        CType(Me.Pic, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'OK
        '
        Me.OK.Location = New System.Drawing.Point(424, 88)
        Me.OK.Name = "OK"
        Me.OK.Size = New System.Drawing.Size(75, 23)
        Me.OK.TabIndex = 0
        Me.OK.Text = "OK"
        '
        'Pic
        '
        Me.Pic.Image = CType(resources.GetObject("Pic.Image"), System.Drawing.Image)
        Me.Pic.Location = New System.Drawing.Point(8, 8)
        Me.Pic.Name = "Pic"
        Me.Pic.Size = New System.Drawing.Size(64, 48)
        Me.Pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Pic.TabIndex = 1
        Me.Pic.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(88, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(223, 17)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Provider Management System"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(88, 24)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(16, 17)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "v"
        '
        'Version
        '
        Me.Version.AutoSize = True
        Me.Version.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Version.Location = New System.Drawing.Point(104, 24)
        Me.Version.Name = "Version"
        Me.Version.Size = New System.Drawing.Size(17, 17)
        Me.Version.TabIndex = 5
        Me.Version.Text = "0"
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(8, 72)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(232, 32)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "This Product Was Designed For Use By: So. Cal. UFCW Trust Fund"
        '
        'Versions
        '
        Me.Versions.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Versions.HorizontalScrollbar = True
        Me.Versions.Location = New System.Drawing.Point(8, 124)
        Me.Versions.Name = "Versions"
        Me.Versions.SelectionMode = System.Windows.Forms.SelectionMode.None
        Me.Versions.Size = New System.Drawing.Size(506, 277)
        Me.Versions.Sorted = True
        Me.Versions.TabIndex = 8
        '
        'cbUpdateProvider
        '
        Me.cbUpdateProvider.Enabled = False
        Me.cbUpdateProvider.Location = New System.Drawing.Point(328, 24)
        Me.cbUpdateProvider.Name = "cbUpdateProvider"
        Me.cbUpdateProvider.Size = New System.Drawing.Size(116, 16)
        Me.cbUpdateProvider.TabIndex = 11
        Me.cbUpdateProvider.Text = "Update Provider"
        '
        'cbAddProvider
        '
        Me.cbAddProvider.Enabled = False
        Me.cbAddProvider.Location = New System.Drawing.Point(328, 8)
        Me.cbAddProvider.Name = "cbAddProvider"
        Me.cbAddProvider.Size = New System.Drawing.Size(120, 16)
        Me.cbAddProvider.TabIndex = 10
        Me.cbAddProvider.Text = "Add Provider"
        '
        'cbDeleteProvider
        '
        Me.cbDeleteProvider.Enabled = False
        Me.cbDeleteProvider.Location = New System.Drawing.Point(328, 43)
        Me.cbDeleteProvider.Name = "cbDeleteProvider"
        Me.cbDeleteProvider.Size = New System.Drawing.Size(116, 16)
        Me.cbDeleteProvider.TabIndex = 12
        Me.cbDeleteProvider.Text = "Delete Provider"
        '
        'cbAdministratorProvider
        '
        Me.cbAdministratorProvider.Enabled = False
        Me.cbAdministratorProvider.Location = New System.Drawing.Point(328, 62)
        Me.cbAdministratorProvider.Name = "cbAdministratorProvider"
        Me.cbAdministratorProvider.Size = New System.Drawing.Size(116, 16)
        Me.cbAdministratorProvider.TabIndex = 13
        Me.cbAdministratorProvider.Text = "Administrator"
        '
        'AboutMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(514, 412)
        Me.Controls.Add(Me.cbAdministratorProvider)
        Me.Controls.Add(Me.cbDeleteProvider)
        Me.Controls.Add(Me.cbUpdateProvider)
        Me.Controls.Add(Me.cbAddProvider)
        Me.Controls.Add(Me.Versions)
        Me.Controls.Add(Me.Version)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Pic)
        Me.Controls.Add(Me.OK)
        Me.Controls.Add(Me.Label3)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AboutMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "About UFCW CMS"
        CType(Me.Pic, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub About_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Version.Text = Application.ProductVersion

        Call CollectVersions()

        Me.cbAddProvider.Checked = UFCWGeneralAD.CMSCanAddProvider
        Me.cbUpdateProvider.Checked = UFCWGeneralAD.CMSCanModifyProvider
        Me.cbDeleteProvider.Checked = UFCWGeneralAD.CMSCanDeleteProvider
        Me.cbAdministratorProvider.Checked = UFCWGeneralAD.CMSAdministrators

    End Sub

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        Me.Close()
    End Sub

    Private Sub CollectVersions()
        Dim fInfo As FileInfo = New FileInfo(Application.ExecutablePath)
        Dim dInfo As DirectoryInfo = New DirectoryInfo(fInfo.DirectoryName)

        Dim dllFileArray() As FileInfo = dInfo.GetFiles("*.dll")
        Dim exeFileArray() As FileInfo = dInfo.GetFiles("*.exe")

        Versions.BeginUpdate()
        For Each fi As FileInfo In dllFileArray
            If fi.Name.IndexOf("UFCW.CMS") >= 0 Then
                With System.Diagnostics.FileVersionInfo.GetVersionInfo(fi.FullName)
                    Versions.Items.Add(fi.FullName & "(" & .FileMajorPart & "." & .FileMinorPart & "." & .FileBuildPart & "." & .FilePrivatePart & ")")
                End With
            End If
        Next
        For Each fi As FileInfo In exeFileArray
            With System.Diagnostics.FileVersionInfo.GetVersionInfo(fi.FullName)
                Versions.Items.Add(fi.FullName & "(" & .FileMajorPart & "." & .FileMinorPart & "." & .FileBuildPart & "." & .FilePrivatePart & ")")
            End With
        Next
        Versions.EndUpdate()

    End Sub

End Class
