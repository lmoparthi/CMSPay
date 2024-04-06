<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Public Class FindDocuments
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FindDocuments))
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtSearchBatch = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtSearchMaximID = New System.Windows.Forms.TextBox()
        Me.txtSearchDate = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.CancelActionButton = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtSearchFamilyID = New ExTextBox()
        Me.ClearButton = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtSearchDocID = New ExTextBox()
        Me.txtSearchProviderTaxID = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.SearchButton = New System.Windows.Forms.Button()
        Me.txtSearchMemSSN = New ExTextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.UfcwDocsControl1 = New UFCWDocsControl()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.txtSearchBatch)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.txtSearchMaximID)
        Me.GroupBox1.Controls.Add(Me.txtSearchDate)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.CancelActionButton)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.txtSearchFamilyID)
        Me.GroupBox1.Controls.Add(Me.ClearButton)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.txtSearchDocID)
        Me.GroupBox1.Controls.Add(Me.txtSearchProviderTaxID)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.SearchButton)
        Me.GroupBox1.Controls.Add(Me.txtSearchMemSSN)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(9, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(606, 112)
        Me.GroupBox1.TabIndex = 15
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Search"
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(193, 81)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(326, 26)
        Me.Label6.TabIndex = 33
        Me.Label6.Text = "Received Date can be used alone or in conjunction with Primary search values on l" &
    "eft hand side"
        '
        'txtSearchBatch
        '
        Me.txtSearchBatch.Location = New System.Drawing.Point(308, 35)
        Me.txtSearchBatch.MaxLength = 10
        Me.txtSearchBatch.Name = "txtSearchBatch"
        Me.txtSearchBatch.Size = New System.Drawing.Size(100, 20)
        Me.txtSearchBatch.TabIndex = 32
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(219, 37)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(38, 13)
        Me.Label8.TabIndex = 31
        Me.Label8.Text = "Batch:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(219, 62)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(54, 13)
        Me.Label7.TabIndex = 30
        Me.Label7.Text = "Maxim ID:"
        '
        'txtSearchMaximID
        '
        Me.txtSearchMaximID.Location = New System.Drawing.Point(308, 59)
        Me.txtSearchMaximID.MaxLength = 26
        Me.txtSearchMaximID.Name = "txtSearchMaximID"
        Me.txtSearchMaximID.Size = New System.Drawing.Size(172, 20)
        Me.txtSearchMaximID.TabIndex = 29
        '
        'txtSearchDate
        '
        Me.txtSearchDate.Location = New System.Drawing.Point(308, 11)
        Me.txtSearchDate.MaxLength = 10
        Me.txtSearchDate.Name = "txtSearchDate"
        Me.txtSearchDate.Size = New System.Drawing.Size(100, 20)
        Me.txtSearchDate.TabIndex = 27
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(219, 14)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(82, 13)
        Me.Label5.TabIndex = 26
        Me.Label5.Text = "Received Date:"
        '
        'CancelButton
        '
        Me.CancelActionButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelActionButton.Location = New System.Drawing.Point(525, 78)
        Me.CancelActionButton.Name = "CancelButton"
        Me.CancelActionButton.Size = New System.Drawing.Size(75, 23)
        Me.CancelActionButton.TabIndex = 25
        Me.CancelActionButton.Text = "Exit"
        Me.CancelActionButton.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(30, 62)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(50, 13)
        Me.Label4.TabIndex = 24
        Me.Label4.Text = "FamilyID:"
        '
        'txtSearchFamilyID
        '
        Me.txtSearchFamilyID.Location = New System.Drawing.Point(87, 59)
        Me.txtSearchFamilyID.MaxLength = 8
        Me.txtSearchFamilyID.Name = "txtSearchFamilyID"
        Me.txtSearchFamilyID.Size = New System.Drawing.Size(100, 20)
        Me.txtSearchFamilyID.TabIndex = 23
        '
        'ClearButton
        '
        Me.ClearButton.Location = New System.Drawing.Point(525, 46)
        Me.ClearButton.Name = "ClearButton"
        Me.ClearButton.Size = New System.Drawing.Size(75, 23)
        Me.ClearButton.TabIndex = 22
        Me.ClearButton.Text = "Clear"
        Me.ClearButton.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(10, 88)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(70, 13)
        Me.Label3.TabIndex = 21
        Me.Label3.Text = "Provider TIN:"
        '
        'txtSearchDocID
        '
        Me.txtSearchDocID.Location = New System.Drawing.Point(86, 35)
        Me.txtSearchDocID.MaxLength = 8
        Me.txtSearchDocID.Name = "txtSearchDocID"
        Me.txtSearchDocID.Size = New System.Drawing.Size(100, 20)
        Me.txtSearchDocID.TabIndex = 20
        '
        'txtSearchProvider
        '
        Me.txtSearchProviderTaxID.Location = New System.Drawing.Point(86, 83)
        Me.txtSearchProviderTaxID.MaxLength = 10
        Me.txtSearchProviderTaxID.Name = "txtSearchProvider"
        Me.txtSearchProviderTaxID.Size = New System.Drawing.Size(100, 20)
        Me.txtSearchProviderTaxID.TabIndex = 19
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(10, 37)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(70, 13)
        Me.Label2.TabIndex = 18
        Me.Label2.Text = "DocumentID:"
        '
        'SearchButton
        '
        Me.SearchButton.Location = New System.Drawing.Point(525, 14)
        Me.SearchButton.Name = "SearchButton"
        Me.SearchButton.Size = New System.Drawing.Size(75, 23)
        Me.SearchButton.TabIndex = 17
        Me.SearchButton.Text = "Search"
        Me.SearchButton.UseVisualStyleBackColor = True
        '
        'txtSearchMemSSN
        '
        Me.txtSearchMemSSN.Location = New System.Drawing.Point(87, 11)
        Me.txtSearchMemSSN.MaxLength = 11
        Me.txtSearchMemSSN.Name = "txtSearchMemSSN"
        Me.txtSearchMemSSN.Size = New System.Drawing.Size(100, 20)
        Me.txtSearchMemSSN.TabIndex = 16
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(48, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(32, 13)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "SSN:"
        '
        'UfcwDocsControl1
        '
        Me.UfcwDocsControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UfcwDocsControl1.AppKey = "UFCW\Claims\"
        Me.UfcwDocsControl1.Location = New System.Drawing.Point(9, 151)
        Me.UfcwDocsControl1.Mode = SearchMode.SearchDB2AndSQL
        Me.UfcwDocsControl1.Name = "UfcwDocsControl1"
        Me.UfcwDocsControl1.Size = New System.Drawing.Size(608, 333)
        Me.UfcwDocsControl1.TabIndex = 9
        '
        'FindDocuments
        '
        Me.AcceptButton = Me.SearchButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(627, 496)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.UfcwDocsControl1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FindDocuments"
        Me.Text = "Find Document"
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents UfcwDocsControl1 As UFCWDocsControl
    Friend WithEvents ErrorProvider1 As ErrorProvider
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents txtSearchDate As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents CancelActionButton As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents txtSearchFamilyID As ExTextBox
    Friend WithEvents ClearButton As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents txtSearchDocID As ExTextBox
    Friend WithEvents txtSearchProviderTaxID As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents SearchButton As Button
    Friend WithEvents txtSearchMemSSN As ExTextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents txtSearchMaximID As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents txtSearchBatch As TextBox
    Friend WithEvents Label8 As Label
End Class
