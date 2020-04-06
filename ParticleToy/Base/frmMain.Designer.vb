﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.ScreenImgBox = New System.Windows.Forms.PictureBox()
        Me.Ticker = New System.Windows.Forms.Timer(Me.components)
        Me.SaveDirDialog = New System.Windows.Forms.SaveFileDialog()
        Me.CommandTxb = New System.Windows.Forms.TextBox()
        CType(Me.ScreenImgBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ScreenImgBox
        '
        Me.ScreenImgBox.BackColor = System.Drawing.Color.Black
        Me.ScreenImgBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ScreenImgBox.Location = New System.Drawing.Point(0, 0)
        Me.ScreenImgBox.Name = "ScreenImgBox"
        Me.ScreenImgBox.Size = New System.Drawing.Size(800, 600)
        Me.ScreenImgBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.ScreenImgBox.TabIndex = 0
        Me.ScreenImgBox.TabStop = False
        '
        'Ticker
        '
        Me.Ticker.Interval = 16
        '
        'SaveDirDialog
        '
        Me.SaveDirDialog.AddExtension = False
        Me.SaveDirDialog.FileName = "UnknownFrameFolder"
        Me.SaveDirDialog.OverwritePrompt = False
        Me.SaveDirDialog.Title = "Choose target folder"
        '
        'CommandTxb
        '
        Me.CommandTxb.AcceptsReturn = True
        Me.CommandTxb.AcceptsTab = True
        Me.CommandTxb.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CommandTxb.BackColor = System.Drawing.Color.Black
        Me.CommandTxb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.CommandTxb.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CommandTxb.ForeColor = System.Drawing.Color.Yellow
        Me.CommandTxb.HideSelection = False
        Me.CommandTxb.Location = New System.Drawing.Point(0, 580)
        Me.CommandTxb.Multiline = True
        Me.CommandTxb.Name = "CommandTxb"
        Me.CommandTxb.Size = New System.Drawing.Size(800, 20)
        Me.CommandTxb.TabIndex = 1
        Me.CommandTxb.Visible = False
        Me.CommandTxb.WordWrap = False
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.DimGray
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.Controls.Add(Me.CommandTxb)
        Me.Controls.Add(Me.ScreenImgBox)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Particle Toy"
        CType(Me.ScreenImgBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ScreenImgBox As System.Windows.Forms.PictureBox
    Friend WithEvents Ticker As System.Windows.Forms.Timer
    Friend WithEvents SaveDirDialog As SaveFileDialog
    Friend WithEvents CommandTxb As TextBox
End Class
