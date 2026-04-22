Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms

Namespace OcrDiagnostic
	''' <summary>
	''' Summary description for Parameters.
	''' </summary>
	Public Class Parameters
		Inherits System.Windows.Forms.Form
		Private WithEvents btnOk As System.Windows.Forms.Button
		Private btnCancel As System.Windows.Forms.Button
		Private Grid As System.Windows.Forms.PropertyGrid
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.Container = Nothing

		Public Sub New()
			'
			' Required for Windows Form Designer support
			'
			InitializeComponent()
		End Sub

		Public Sub New(ByVal text As String, ByVal selectedObject As Object)
			InitializeComponent()

			Me.Text = text
			Me.Grid.SelectedObject = selectedObject
		End Sub

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		Protected Overrides Overloads Sub Dispose(ByVal disposing As Boolean)
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
			Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Parameters))
			Me.Grid = New System.Windows.Forms.PropertyGrid()
			Me.btnOk = New System.Windows.Forms.Button()
			Me.btnCancel = New System.Windows.Forms.Button()
			Me.SuspendLayout()
			' 
			' Grid
			' 
			Me.Grid.Anchor = (CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
			Me.Grid.CommandsVisibleIfAvailable = True
			Me.Grid.LargeButtons = False
			Me.Grid.LineColor = System.Drawing.SystemColors.ScrollBar
			Me.Grid.Location = New System.Drawing.Point(0, 0)
			Me.Grid.Name = "Grid"
			Me.Grid.PropertySort = System.Windows.Forms.PropertySort.Alphabetical
			Me.Grid.Size = New System.Drawing.Size(408, 208)
			Me.Grid.TabIndex = 0
			Me.Grid.Text = "propertyGrid1"
			Me.Grid.ToolbarVisible = False
			Me.Grid.ViewBackColor = System.Drawing.SystemColors.Window
			Me.Grid.ViewForeColor = System.Drawing.SystemColors.WindowText
			' 
			' btnOk
			' 
			Me.btnOk.Anchor = (CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles))
			Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
			Me.btnOk.Location = New System.Drawing.Point(49, 216)
			Me.btnOk.Name = "btnOk"
			Me.btnOk.Size = New System.Drawing.Size(80, 24)
			Me.btnOk.TabIndex = 1
			Me.btnOk.Text = "&OK"
'			Me.btnOk.Click += New System.EventHandler(Me.btnOk_Click);
			' 
			' btnCancel
			' 
			Me.btnCancel.Anchor = (CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles))
			Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
			Me.btnCancel.Location = New System.Drawing.Point(161, 216)
			Me.btnCancel.Name = "btnCancel"
			Me.btnCancel.Size = New System.Drawing.Size(80, 24)
			Me.btnCancel.TabIndex = 2
			Me.btnCancel.Text = "&Cancel"
			' 
			' Parameters
			' 
			Me.AcceptButton = Me.btnOk
			Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
			Me.CancelButton = Me.btnCancel
			Me.ClientSize = New System.Drawing.Size(408, 248)
			Me.Controls.Add(Me.btnCancel)
			Me.Controls.Add(Me.btnOk)
			Me.Controls.Add(Me.Grid)
			Me.Icon = (CType(resources.GetObject("$this.Icon"), System.Drawing.Icon))
			Me.MaximizeBox = False
			Me.MinimizeBox = False
			Me.Name = "Parameters"
			Me.ShowInTaskbar = False
			Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
			Me.Text = "Parameters"
'			Me.Load += New System.EventHandler(Me.Parameters_Load);
			Me.ResumeLayout(False)

		End Sub
		#End Region

		Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click

		End Sub

		Private Sub Parameters_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

		End Sub
	End Class
End Namespace
