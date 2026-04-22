Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms

Namespace AtalaDemos.AboutBox
	''' <summary>
	''' Summary description for About.
	''' </summary>
	Public Class About
		Inherits System.Windows.Forms.Form
		Private label1 As System.Windows.Forms.Label
		Private label2 As System.Windows.Forms.Label
        Private WithEvents downloadHelpLinkLabel As System.Windows.Forms.LinkLabel
        Private WithEvents button1 As System.Windows.Forms.Button
        Private WithEvents demoGalleryLinkLabel As System.Windows.Forms.LinkLabel
        Private WithEvents downloadDotImageLinkLabel As System.Windows.Forms.LinkLabel
        Private label4 As System.Windows.Forms.Label
        Private WithEvents pictureBox1 As System.Windows.Forms.PictureBox
        Private label5 As System.Windows.Forms.Label
        Private groupBox1 As System.Windows.Forms.GroupBox
        Private txtAssemblies As System.Windows.Forms.TextBox

        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.Container = Nothing

        Public Sub New(ByVal windowTitle As String, ByVal progName As String)
            '
            ' Required for Windows Form Designer support
            '
            InitializeComponent()

            '
            ' TODO: Add any constructor code after InitializeComponent call
            '
            Me.Text = windowTitle
            Me.label5.Text = progName
            ' Load assembly versions.
            Dim asm As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
            Dim refs As System.Reflection.AssemblyName() = asm.GetReferencedAssemblies()
            Dim msg As System.Text.StringBuilder = New System.Text.StringBuilder()

            For Each name As System.Reflection.AssemblyName In refs
                If name.Name.StartsWith("Atalasoft") Then
                    If msg.Length <> 0 Then
                        msg.Append(Constants.vbCrLf)
                    End If
                    msg.Append(name.Name & " - " & Name.Version.ToString())
                End If
            Next name

            Me.txtAssemblies.Text = msg.ToString()

        End Sub

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If Not components Is Nothing Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

#Region "Windows Form Designer generated code"
        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(About))
            Me.label1 = New System.Windows.Forms.Label
            Me.label2 = New System.Windows.Forms.Label
            Me.downloadHelpLinkLabel = New System.Windows.Forms.LinkLabel
            Me.button1 = New System.Windows.Forms.Button
            Me.demoGalleryLinkLabel = New System.Windows.Forms.LinkLabel
            Me.downloadDotImageLinkLabel = New System.Windows.Forms.LinkLabel
            Me.label4 = New System.Windows.Forms.Label
            Me.pictureBox1 = New System.Windows.Forms.PictureBox
            Me.label5 = New System.Windows.Forms.Label
            Me.groupBox1 = New System.Windows.Forms.GroupBox
            Me.txtAssemblies = New System.Windows.Forms.TextBox
            CType(Me.pictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.groupBox1.SuspendLayout()
            Me.SuspendLayout()
            '
            'label1
            '
            Me.label1.Location = New System.Drawing.Point(24, 64)
            Me.label1.Name = "label1"
            Me.label1.Size = New System.Drawing.Size(432, 144)
            Me.label1.TabIndex = 1
            Me.label1.Text = "This demo showcases most of the basic functions that can be performed with DotIma" & _
                "ge.  This is a good starting place for learning about the SDK."
            '
            'label2
            '
            Me.label2.Location = New System.Drawing.Point(56, 216)
            Me.label2.Name = "label2"
            Me.label2.Size = New System.Drawing.Size(336, 16)
            Me.label2.TabIndex = 2
            Me.label2.Text = "Please Check the following for help programming with DotImage:"
            '
            'downloadHelpLinkLabel
            '
            Me.downloadHelpLinkLabel.LinkArea = New System.Windows.Forms.LinkArea(0, 36)
            Me.downloadHelpLinkLabel.Location = New System.Drawing.Point(120, 272)
            Me.downloadHelpLinkLabel.Name = "downloadHelpLinkLabel"
            Me.downloadHelpLinkLabel.Size = New System.Drawing.Size(200, 16)
            Me.downloadHelpLinkLabel.TabIndex = 4
            Me.downloadHelpLinkLabel.TabStop = True
            Me.downloadHelpLinkLabel.Text = "DotImage Documentation"
            Me.downloadHelpLinkLabel.UseCompatibleTextRendering = True
            '
            'button1
            '
            Me.button1.BackColor = System.Drawing.SystemColors.Control
            Me.button1.Location = New System.Drawing.Point(328, 560)
            Me.button1.Name = "button1"
            Me.button1.Size = New System.Drawing.Size(104, 24)
            Me.button1.TabIndex = 5
            Me.button1.Text = "OK"
            Me.button1.UseVisualStyleBackColor = False
            '
            'demoGalleryLinkLabel
            '
            Me.demoGalleryLinkLabel.Location = New System.Drawing.Point(120, 296)
            Me.demoGalleryLinkLabel.Name = "demoGalleryLinkLabel"
            Me.demoGalleryLinkLabel.Size = New System.Drawing.Size(192, 16)
            Me.demoGalleryLinkLabel.TabIndex = 6
            Me.demoGalleryLinkLabel.TabStop = True
            Me.demoGalleryLinkLabel.Text = "OCR Diagnostic Demo Home"
            '
            'downloadDotImageLinkLabel
            '
            Me.downloadDotImageLinkLabel.Location = New System.Drawing.Point(120, 249)
            Me.downloadDotImageLinkLabel.Name = "downloadDotImageLinkLabel"
            Me.downloadDotImageLinkLabel.Size = New System.Drawing.Size(184, 23)
            Me.downloadDotImageLinkLabel.TabIndex = 7
            Me.downloadDotImageLinkLabel.TabStop = True
            Me.downloadDotImageLinkLabel.Text = "Download the Latest DotImage"
            '
            'label4
            '
            Me.label4.Location = New System.Drawing.Point(120, 352)
            Me.label4.Name = "label4"
            Me.label4.Size = New System.Drawing.Size(264, 23)
            Me.label4.TabIndex = 8
            Me.label4.Text = "Sales / Support:  +1 781.743.2119"
            '
            'pictureBox1
            '
            Me.pictureBox1.Image = CType(resources.GetObject("pictureBox1.Image"), System.Drawing.Image)
            Me.pictureBox1.Location = New System.Drawing.Point(40, 496)
            Me.pictureBox1.Name = "pictureBox1"
            Me.pictureBox1.Size = New System.Drawing.Size(264, 88)
            Me.pictureBox1.TabIndex = 9
            Me.pictureBox1.TabStop = False
            '
            'label5
            '
            Me.label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.label5.ForeColor = System.Drawing.Color.DarkOrange
            Me.label5.Location = New System.Drawing.Point(8, 16)
            Me.label5.Name = "label5"
            Me.label5.Size = New System.Drawing.Size(408, 32)
            Me.label5.TabIndex = 10
            Me.label5.Text = "[demo title here]"
            Me.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'groupBox1
            '
            Me.groupBox1.Controls.Add(Me.txtAssemblies)
            Me.groupBox1.Location = New System.Drawing.Point(88, 384)
            Me.groupBox1.Name = "groupBox1"
            Me.groupBox1.Size = New System.Drawing.Size(296, 103)
            Me.groupBox1.TabIndex = 11
            Me.groupBox1.TabStop = False
            Me.groupBox1.Text = "Assemblies"
            '
            'txtAssemblies
            '
            Me.txtAssemblies.BackColor = System.Drawing.Color.White
            Me.txtAssemblies.BorderStyle = System.Windows.Forms.BorderStyle.None
            Me.txtAssemblies.Location = New System.Drawing.Point(8, 16)
            Me.txtAssemblies.Multiline = True
            Me.txtAssemblies.Name = "txtAssemblies"
            Me.txtAssemblies.Size = New System.Drawing.Size(275, 74)
            Me.txtAssemblies.TabIndex = 0
            '
            'About
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.BackColor = System.Drawing.Color.White
            Me.ClientSize = New System.Drawing.Size(470, 588)
            Me.Controls.Add(Me.groupBox1)
            Me.Controls.Add(Me.label5)
            Me.Controls.Add(Me.pictureBox1)
            Me.Controls.Add(Me.label4)
            Me.Controls.Add(Me.downloadDotImageLinkLabel)
            Me.Controls.Add(Me.demoGalleryLinkLabel)
            Me.Controls.Add(Me.button1)
            Me.Controls.Add(Me.downloadHelpLinkLabel)
            Me.Controls.Add(Me.label2)
            Me.Controls.Add(Me.label1)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "About"
            Me.ShowInTaskbar = False
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "[about title here]"
            CType(Me.pictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.groupBox1.ResumeLayout(False)
            Me.groupBox1.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
#End Region

        Private Sub button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles button1.Click
            Me.Dispose()
        End Sub

        Private Sub apiReferenceLinkLabel_LinkClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles downloadHelpLinkLabel.LinkClicked
            System.Diagnostics.Process.Start("https://www.atalasoft.com/Support/APIs-Dev-Guides")
        End Sub

        Private Sub demoGalleryLinkLabel_LinkClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles demoGalleryLinkLabel.LinkClicked
            System.Diagnostics.Process.Start("https://www.atalasoft.com/KB2/KB/50086/")
        End Sub

        Private Sub downloadDotImageLinkLabel_LinkClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles downloadDotImageLinkLabel.LinkClicked
            System.Diagnostics.Process.Start("https://www.atalasoft.com/BeginDownload")
        End Sub

        Private Sub pictureBox1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles pictureBox1.Click
            System.Diagnostics.Process.Start("https://www.atalasoft.com")
        End Sub

		Private Overloads Sub OnMouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles pictureBox1.MouseEnter
			Me.Cursor = Cursors.Hand
		End Sub

		Private Overloads Sub OnMouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles pictureBox1.MouseLeave
			Me.Cursor = Cursors.Default
		End Sub

		Private _description As String = ""

		Private Sub About_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
			Me.label1.Text = _description
		End Sub

		Public Property Description() As String
			Get
				Return _description
			End Get
			Set
				_description = Value
			End Set
		End Property


	End Class
End Namespace
