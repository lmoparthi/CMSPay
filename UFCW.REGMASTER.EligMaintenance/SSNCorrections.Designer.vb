﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SSNCorrections
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SSNCorrections))
        Me.SsnChangeControl = New SSNChangeControl()
        Me.SuspendLayout()
        '
        'SsnChangeControl
        '
        Me.SsnChangeControl.AppKey = "UFCW\RegMaster\"
        Me.SsnChangeControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SsnChangeControl.Location = New System.Drawing.Point(0, 0)
        Me.SsnChangeControl.MinimumSize = New System.Drawing.Size(961, 711)
        Me.SsnChangeControl.Name = "SsnChangeControl"
        Me.SsnChangeControl.Size = New System.Drawing.Size(965, 720)
        Me.SsnChangeControl.TabIndex = 0
        '
        'SSNCorrections
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(965, 720)
        Me.Controls.Add(Me.SsnChangeControl)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "SSNCorrections"
        Me.Text = "Social Security Corrections"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SsnChangeControl As SSNChangeControl
End Class
