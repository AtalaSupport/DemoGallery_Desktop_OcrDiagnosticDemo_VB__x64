Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Drawing.Text
Imports Atalasoft.Ocr

Namespace OcrDiagnostic
	''' <summary>
	''' Summary description for ResolutionFontBuilder.
	''' </summary>
	Public Class ResolutionFontBuilder
		Inherits BasicFontBuilder
		Private _scale As Double

		Public Sub New()
			MyBase.New()
			_scale = 1.0
		End Sub

		Public Overrides Function BuildFont(ByVal family As FontFamily, ByVal size As Single, ByVal style As FontStyle) As Font
			size *= CSng(_scale)
			Return MyBase.BuildFont(family, size, style)
		End Function

		Public Property Scale() As Double
			Get
				Return _scale
			End Get
			Set
				_scale = Value
			End Set
		End Property
	End Class
End Namespace
