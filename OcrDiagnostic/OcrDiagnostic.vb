Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data
Imports System.Reflection
Imports System.IO
Imports Atalasoft.Imaging
Imports Atalasoft.Imaging.WinControls
Imports Atalasoft.Ocr
Imports Atalasoft.Ocr.GlyphReader
Imports Atalasoft.Ocr.Tesseract
Imports System.Globalization
Imports Atalasoft.Imaging.Codec
Imports Atalasoft.Ocr.OmniPage
Imports WinDemoHelperMethods.WinDemoHelperMethods

Namespace OcrDiagnostic
    ''' <summary>
    ''' Summary description for Form1.
    ''' </summary>
    Public Class Form1 : Inherits System.Windows.Forms.Form
        Protected Enum InfoLocation
            ul
            ur
            bl
            br
        End Enum

        Protected Class ClickableItem
            Private _bounds As Rectangle
            Private _thing As Object
            Public Sub New(ByVal theBounds As Rectangle, ByVal theThing As Object)
                _bounds = theBounds
                _thing = theThing
            End Sub
            Public ReadOnly Property Bounds() As Rectangle
                Get
                    Return _bounds
                End Get
            End Property
            Public ReadOnly Property Thing() As Object
                Get
                    Return _thing
                End Get
            End Property
        End Class

        Private mainMenu1 As System.Windows.Forms.MainMenu
        Private File As System.Windows.Forms.MenuItem
        Private WithEvents Recognize As System.Windows.Forms.MenuItem
        'TODO: INSTANT VB TODO TASK: Exit is a keyword in VB.NET. Change the name or use square brackets to override it:
        Private WithEvents Exit_Renamed As System.Windows.Forms.MenuItem
        Private Options As System.Windows.Forms.MenuItem
        Private View As System.Windows.Forms.MenuItem
        Private WithEvents AutoRotate As System.Windows.Forms.MenuItem
        Private WithEvents Deskew As System.Windows.Forms.MenuItem
        Private WithEvents Despeckle As System.Windows.Forms.MenuItem
        Private WithEvents Flip As System.Windows.Forms.MenuItem
        Private WithEvents Invert As System.Windows.Forms.MenuItem
        Private WithEvents Binarize As System.Windows.Forms.MenuItem
        Private components As System.ComponentModel.IContainer
        ''' <summary>
        ''' Required designer variable.
        ''' </summary>

        Private _glyphReaderEngine As GlyphReaderEngine
        Private _tesseract5Engine As Tesseract5Engine
        Private _omniPageEngine As OmniPageEngine
        Private _omniPageLoader As OmniPageLoader
        Private _engine As OcrEngine
        Private WithEvents OcrPane As System.Windows.Forms.Panel
        Private _theDoc As OcrDocument = Nothing
        Private QuestionUp, QuestionDown As Icon
        Private PicturePen, WordBoundingBoxPen, BaselinePen, GlobalBaselinePen, LineBoundingBoxPen, GlyphBoundingBoxPen As Pen
        Private builder As ResolutionFontBuilder
        Private _clickables As ArrayList
        Private _clicked As ClickableItem
        Private WithEvents ShowWordBaselines As System.Windows.Forms.MenuItem
        Private WithEvents ShowLineBoundingBoxes As System.Windows.Forms.MenuItem
        Private WithEvents ShowWordBoundingBoxes As System.Windows.Forms.MenuItem
        Private WithEvents ShowGlyphBoundingBoxes As System.Windows.Forms.MenuItem
        Private WithEvents ShowLineBaselines As System.Windows.Forms.MenuItem
        Private WithEvents ShowFontNames As System.Windows.Forms.MenuItem
        Private _wasInClicked As Boolean
        Private FontNameFont As Font
        Private menuItem1 As System.Windows.Forms.MenuItem
        Private WithEvents AboutButton As System.Windows.Forms.MenuItem
        Private menuItem2 As System.Windows.Forms.MenuItem
        Private menuLanguage As System.Windows.Forms.MenuItem
        Private WithEvents menuGlyphReader As System.Windows.Forms.MenuItem
        Private statusBar1 As System.Windows.Forms.StatusBar
        Private progressBar1 As System.Windows.Forms.ProgressBar
        Private FontBrush As Brush
        Private _validLicense, _hasEV, _hasGR As Boolean
        Friend WithEvents MenuTesseract5 As System.Windows.Forms.MenuItem
        Friend WithEvents MenuOmniPage As System.Windows.Forms.MenuItem

        Public Sub New()
            CheckLicenseFile()

            ' PLEASE NOTE: this is the default location for when using our SDK
            ' you will need to ensure when using OmniPageEngine in yhour application that you 
            ' specify the resources directory wher the OmniPageResources live
            ' Please see https://www.atalasoft.com/KB2/KB/50396/INFO-OmniPageEngine-Overview
            Dim omniPageyOcrResourcesDirectory As String = "C:\Program Files (x86)\Atalasoft\DotImage 2026.2\bin\OCRResources\OmniPage"

            If Not Directory.Exists(omniPageyOcrResourcesDirectory) Then
                MessageBox.Show("You need to ensure you ahve downloaded the OmniPageEngine OCR Resources to the OmniPage resource directory" & vbCrLf & "see https://www.atalasoft.com/KB2/KB/50396/INFO-OmniPageEngine-Overview")
            End If
            _omniPageLoader = New OmniPageLoader(omniPageyOcrResourcesDirectory)

            HelperMethods.PopulateDecoders(RegisteredDecoders.Decoders)

            If Me._validLicense Then
                '
                ' Required for Windows Form Designer support
                '
                InitializeComponent()


                ' Pick a licensed engine to start with.
                If _hasGR Then
                    _glyphReaderEngine = New GlyphReaderEngine
                    _glyphReaderEngine.DefaultFontName = "Times New Roman"
                    Me.menuGlyphReader.Enabled = True
                Else
                    _tesseract5Engine = New Tesseract5Engine
                End If

                If _hasGR Then
                    Me.menuGlyphReader.Checked = True
                    _engine = _glyphReaderEngine
                Else
                    Me.MenuTesseract5.Checked = True
                    _engine = _tesseract5Engine
                End If

                _engine.Initialize()

                CreateLanguageMenu()
                MapNativeOptionsToMenus()
                QuestionUp = GetResourceIcon("qmarkup.ico")
                QuestionDown = GetResourceIcon("qmarkdown.ico")
                PicturePen = New Pen(Color.MediumVioletRed)
                PicturePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
                WordBoundingBoxPen = New Pen(Color.IndianRed)
                LineBoundingBoxPen = New Pen(Color.ForestGreen)
                GlyphBoundingBoxPen = New Pen(Color.MediumBlue)
                BaselinePen = New Pen(Color.DodgerBlue)
                BaselinePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot
                GlobalBaselinePen = New Pen(Color.SeaGreen)
                GlobalBaselinePen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot
                builder = New ResolutionFontBuilder
                _clickables = New ArrayList

                ShowWordBaselines.Checked = True
                ShowLineBoundingBoxes.Checked = True
                ShowWordBoundingBoxes.Checked = True
                ShowGlyphBoundingBoxes.Checked = True
                ShowLineBaselines.Checked = True
                ShowFontNames.Checked = True
                FontNameFont = New Font("Verdana", 6)
                FontBrush = Brushes.LightBlue

                'hook in OCR events
                AddHandler _engine.DocumentProgress, AddressOf _engine_DocumentProgress
            End If
        End Sub

        Private Function GetResourceIcon(ByVal resourceName As String) As Icon
            Dim asm As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
            Dim res As System.IO.Stream = asm.GetManifestResourceStream(resourceName)
            If Not res Is Nothing Then
                Dim ico As Icon = New Icon(res)
                res.Close()
                Return ico
            End If

            Return Nothing
        End Function

#Region "Check for license code"

        Private Sub CheckGRLicense()
            Try
                _hasGR = True
                Dim gr As GlyphReaderEngine = New GlyphReaderEngine   ' does not throw
                gr.Initialize() ' will throw on no license
                gr.Dispose()
            Catch e1 As AtalasoftLicenseException
                _hasGR = False
            End Try
        End Sub

        Private Sub CheckLicenseFile()
            ' Make sure a license for DotImage and Advanced DocClean exist.
            Try
                Dim img As AtalaImage = New AtalaImage
                img.Dispose()
            Catch e1 As Atalasoft.Imaging.AtalasoftLicenseException
                LicenseCheckFailure("This demo requires a DotImage license and an OCR license.")
                Return
            End Try

            If AtalaImage.Edition <> LicenseEdition.Document Then
                LicenseCheckFailure("This demo requires a Document Imaging License." & Constants.vbCrLf & "Your current license is for '" & AtalaImage.Edition.ToString() & "'.")
                Return
            End If

            Try
                Dim t As TranslatorCollection = New TranslatorCollection
            Catch e1 As AtalasoftLicenseException
                LicenseCheckFailure("This demo requires an OCR license.")
                Return
            End Try

            CheckGRLicense()
            Me._validLicense = True

        End Sub

        Private Sub LicenseCheckFailure(ByVal message As String)
            AddHandler Load, AddressOf Form1_Load
            If MessageBox.Show(Me, message & Constants.vbCrLf & Constants.vbCrLf & "Would you like to request an evaluation license?", "License Required", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = DialogResult.Yes Then
                ' Locate the activation utility.
                Dim path As String = ""
                Dim key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\Atalasoft\dotImage\5.0")
                If Not key Is Nothing Then
                    path = Convert.ToString(key.GetValue("AssemblyBasePath"))
                    If Not path Is Nothing AndAlso path.Length > 5 Then
                        path = path.Substring(0, path.Length - 3) & "AtalasoftToolkitActivation.exe"
                    Else
                        path = System.IO.Path.GetFullPath("..\..\..\..\..\AtalasoftToolkitActivation.exe")
                    End If

                    key.Close()
                End If

                If System.IO.File.Exists(path) Then
                    System.Diagnostics.Process.Start(path)
                Else
                    MessageBox.Show(Me, "We were unable to location the DotImage activation utility." & Constants.vbCrLf & "Please run it from the Start menu shortcut.", "File Not Found")
                End If
            End If
        End Sub

        Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs)
            ' close the demo if there is no valid license
            If (Not Me._validLicense) Then
                Application.Exit()
            End If
        End Sub

#End Region

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If Not _engine Is Nothing Then
                    _engine.ShutDown()
                End If
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
            Me.components = New System.ComponentModel.Container()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
            Me.mainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
            Me.File = New System.Windows.Forms.MenuItem()
            Me.Recognize = New System.Windows.Forms.MenuItem()
            Me.Exit_Renamed = New System.Windows.Forms.MenuItem()
            Me.Options = New System.Windows.Forms.MenuItem()
            Me.AutoRotate = New System.Windows.Forms.MenuItem()
            Me.Deskew = New System.Windows.Forms.MenuItem()
            Me.Despeckle = New System.Windows.Forms.MenuItem()
            Me.Flip = New System.Windows.Forms.MenuItem()
            Me.Invert = New System.Windows.Forms.MenuItem()
            Me.Binarize = New System.Windows.Forms.MenuItem()
            Me.View = New System.Windows.Forms.MenuItem()
            Me.ShowWordBaselines = New System.Windows.Forms.MenuItem()
            Me.ShowLineBaselines = New System.Windows.Forms.MenuItem()
            Me.ShowLineBoundingBoxes = New System.Windows.Forms.MenuItem()
            Me.ShowWordBoundingBoxes = New System.Windows.Forms.MenuItem()
            Me.ShowGlyphBoundingBoxes = New System.Windows.Forms.MenuItem()
            Me.ShowFontNames = New System.Windows.Forms.MenuItem()
            Me.menuItem2 = New System.Windows.Forms.MenuItem()
            Me.menuGlyphReader = New System.Windows.Forms.MenuItem()
            Me.MenuOmniPage = New System.Windows.Forms.MenuItem()
            Me.MenuTesseract5 = New System.Windows.Forms.MenuItem()
            Me.menuLanguage = New System.Windows.Forms.MenuItem()
            Me.menuItem1 = New System.Windows.Forms.MenuItem()
            Me.AboutButton = New System.Windows.Forms.MenuItem()
            Me.OcrPane = New System.Windows.Forms.Panel()
            Me.statusBar1 = New System.Windows.Forms.StatusBar()
            Me.progressBar1 = New System.Windows.Forms.ProgressBar()
            Me.SuspendLayout()
            '
            'mainMenu1
            '
            Me.mainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.File, Me.Options, Me.View, Me.menuItem2, Me.menuLanguage, Me.menuItem1})
            '
            'File
            '
            Me.File.Index = 0
            Me.File.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.Recognize, Me.Exit_Renamed})
            Me.File.Text = "File"
            '
            'Recognize
            '
            Me.Recognize.Index = 0
            Me.Recognize.Text = "Recognize..."
            '
            'Exit_Renamed
            '
            Me.Exit_Renamed.Index = 1
            Me.Exit_Renamed.Text = "Exit"
            '
            'Options
            '
            Me.Options.Index = 1
            Me.Options.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.AutoRotate, Me.Deskew, Me.Despeckle, Me.Flip, Me.Invert, Me.Binarize})
            Me.Options.Text = "Options"
            '
            'AutoRotate
            '
            Me.AutoRotate.Index = 0
            Me.AutoRotate.Text = "Auto Rotate"
            '
            'Deskew
            '
            Me.Deskew.Index = 1
            Me.Deskew.Text = "Deskew"
            '
            'Despeckle
            '
            Me.Despeckle.Index = 2
            Me.Despeckle.Text = "Despeckle"
            '
            'Flip
            '
            Me.Flip.Index = 3
            Me.Flip.Text = "Flip Left/Right"
            '
            'Invert
            '
            Me.Invert.Index = 4
            Me.Invert.Text = "Invert"
            '
            'Binarize
            '
            Me.Binarize.Index = 5
            Me.Binarize.Text = "Convert to Bilevel"
            '
            'View
            '
            Me.View.Index = 2
            Me.View.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.ShowWordBaselines, Me.ShowLineBaselines, Me.ShowLineBoundingBoxes, Me.ShowWordBoundingBoxes, Me.ShowGlyphBoundingBoxes, Me.ShowFontNames})
            Me.View.Text = "View"
            '
            'ShowWordBaselines
            '
            Me.ShowWordBaselines.Index = 0
            Me.ShowWordBaselines.Text = "Word Baselines"
            '
            'ShowLineBaselines
            '
            Me.ShowLineBaselines.Index = 1
            Me.ShowLineBaselines.Text = "Line Baselines"
            '
            'ShowLineBoundingBoxes
            '
            Me.ShowLineBoundingBoxes.Index = 2
            Me.ShowLineBoundingBoxes.Text = "Line Bounding Boxes"
            '
            'ShowWordBoundingBoxes
            '
            Me.ShowWordBoundingBoxes.Index = 3
            Me.ShowWordBoundingBoxes.Text = "Word Bounding Boxes"
            '
            'ShowGlyphBoundingBoxes
            '
            Me.ShowGlyphBoundingBoxes.Index = 4
            Me.ShowGlyphBoundingBoxes.Text = "Glyph Bounding Boxes"
            '
            'ShowFontNames
            '
            Me.ShowFontNames.Index = 5
            Me.ShowFontNames.Text = "Font Names"
            '
            'menuItem2
            '
            Me.menuItem2.Index = 3
            Me.menuItem2.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuGlyphReader, Me.MenuOmniPage, Me.MenuTesseract5})
            Me.menuItem2.Text = "Engine"
            '
            'menuGlyphReader
            '
            Me.menuGlyphReader.Index = 0
            Me.menuGlyphReader.Text = "GlyphReader"
            '
            'MenuOmniPage
            '
            Me.MenuOmniPage.Index = 1
            Me.MenuOmniPage.Text = "OmniPage"
            '
            'MenuTesseract5
            '
            Me.MenuTesseract5.Index = 2
            Me.MenuTesseract5.Text = "Tesseract 5"
            '
            'menuLanguage
            '
            Me.menuLanguage.Index = 4
            Me.menuLanguage.Text = "Language"
            '
            'menuItem1
            '
            Me.menuItem1.Index = 5
            Me.menuItem1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.AboutButton})
            Me.menuItem1.Text = "Help"
            '
            'AboutButton
            '
            Me.AboutButton.Index = 0
            Me.AboutButton.Text = "About ..."
            '
            'OcrPane
            '
            Me.OcrPane.AutoScroll = True
            Me.OcrPane.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OcrPane.Location = New System.Drawing.Point(0, 0)
            Me.OcrPane.Name = "OcrPane"
            Me.OcrPane.Size = New System.Drawing.Size(608, 563)
            Me.OcrPane.TabIndex = 0
            '
            'statusBar1
            '
            Me.statusBar1.Location = New System.Drawing.Point(0, 563)
            Me.statusBar1.Name = "statusBar1"
            Me.statusBar1.Size = New System.Drawing.Size(608, 22)
            Me.statusBar1.TabIndex = 1
            '
            'progressBar1
            '
            Me.progressBar1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.progressBar1.Location = New System.Drawing.Point(214, 566)
            Me.progressBar1.Name = "progressBar1"
            Me.progressBar1.Size = New System.Drawing.Size(376, 16)
            Me.progressBar1.TabIndex = 2
            '
            'Form1
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(608, 585)
            Me.Controls.Add(Me.progressBar1)
            Me.Controls.Add(Me.OcrPane)
            Me.Controls.Add(Me.statusBar1)
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.Menu = Me.mainMenu1
            Me.Name = "Form1"
            Me.Text = "Ocr Diagnostic"
            Me.ResumeLayout(False)

        End Sub
#End Region

        ''' <summary>
        ''' The main entry point for the application.
        ''' </summary>
        <STAThread()>
        Shared Sub Main()
            Dim loader As OcrResourceLoader = New OcrResourceLoader
            Application.Run(New Form1)
        End Sub

        Private Sub Exit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Exit_Renamed.Click
            Application.Exit()
        End Sub

#Region "OptionMenu Handling"

        Private Sub MapNativeOptionsToMenus()
            AutoRotate.Checked = _engine.PreprocessingOptions.AutoRotate
            Deskew.Checked = _engine.PreprocessingOptions.Deskew
            Despeckle.Checked = _engine.PreprocessingOptions.Despeckle
            Flip.Checked = _engine.PreprocessingOptions.FlipLeftRight
            Invert.Checked = _engine.PreprocessingOptions.Invert
            Binarize.Checked = _engine.PreprocessingOptions.ToBilevel
        End Sub

        Private Sub AutoRotate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AutoRotate.Click
            _engine.PreprocessingOptions.AutoRotate = Not _engine.PreprocessingOptions.AutoRotate
            AutoRotate.Checked = _engine.PreprocessingOptions.AutoRotate
        End Sub

        Private Sub Deskew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Deskew.Click
            _engine.PreprocessingOptions.Deskew = Not _engine.PreprocessingOptions.Deskew
            Deskew.Checked = _engine.PreprocessingOptions.Deskew
        End Sub

        Private Sub Despeckle_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Despeckle.Click
            _engine.PreprocessingOptions.Despeckle = Not _engine.PreprocessingOptions.Despeckle
            Despeckle.Checked = _engine.PreprocessingOptions.Despeckle
        End Sub

        Private Sub Flip_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Flip.Click
            _engine.PreprocessingOptions.FlipLeftRight = Not _engine.PreprocessingOptions.FlipLeftRight
            Flip.Checked = _engine.PreprocessingOptions.FlipLeftRight
        End Sub

        Private Sub Invert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Invert.Click
            _engine.PreprocessingOptions.Invert = Not _engine.PreprocessingOptions.Invert
            Invert.Checked = _engine.PreprocessingOptions.Invert
        End Sub

        Private Sub Binarize_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Binarize.Click
            _engine.PreprocessingOptions.ToBilevel = Not _engine.PreprocessingOptions.ToBilevel
            Invert.Checked = _engine.PreprocessingOptions.ToBilevel
        End Sub
#End Region

        Private Sub Recognize_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Recognize.Click
            Dim oif As OpenImageFileDialog = New OpenImageFileDialog
            oif.Filter = HelperMethods.CreateDialogFilter(True)

            ' try to locate images folder
            Dim imagesFolder As String = Application.ExecutablePath
            ' we assume we are running under the DotImage install folder
            Dim pos As Integer = imagesFolder.IndexOf("DotImage ")
            If pos <> -1 Then
                imagesFolder = imagesFolder.Substring(0, imagesFolder.IndexOf("\", pos)) & "\Images\OCR"
            End If

            'use this folder as starting point			
            oif.InitialDirectory = imagesFolder

            If oif.ShowDialog(Me) <> DialogResult.OK Then
                Return
            End If

            Dim paths As String() = New String(0) {}
            paths(0) = oif.FileName
            oif.Dispose()
            Dim source As FileSystemImageSource = New FileSystemImageSource(paths, False)

            Try
                _theDoc = _engine.Recognize(source)
            Catch err As Exception
                MessageBox.Show("Recognition Failed: " & err.Message)
                _theDoc = Nothing
            Finally
                DocChanged()
            End Try
        End Sub

        Private Sub DocChanged()
            Dim theSize As Size
            If _theDoc Is Nothing Then
                theSize = New Size(OcrPane.Width, OcrPane.Height)
                OcrPane.AutoScrollMinSize = theSize
                _clickables.Clear()
            Else
                Dim page As OcrPage = _theDoc.Pages(0)
                theSize = New Size(page.Width, page.Height)
                OcrPane.AutoScrollMinSize = theSize
                BuildClickables(page)
                OcrPane.AutoScrollPosition = New Point(0, 0)
            End If
            OcrPane.Invalidate()
        End Sub

        Private Sub BuildClickables(ByVal page As OcrPage)
            Dim r As Rectangle
            _clickables.Clear()
            For Each region As OcrRegion In page.Regions
                If TypeOf region Is OcrTextRegion Then
                    Dim textRegion As OcrTextRegion = CType(region, OcrTextRegion)
                    For Each line As OcrLine In textRegion.Lines
                        If Me.ShowLineBoundingBoxes.Checked Then
                            r = GetInfoBounds(line.Bounds, InfoLocation.bl, QuestionUp)
                            _clickables.Add(New ClickableItem(r, line))
                        End If
                        For Each word As OcrWord In line.Words
                            If Me.ShowWordBoundingBoxes.Checked Then
                                r = GetInfoBounds(word.Bounds, InfoLocation.ul, QuestionUp)
                                _clickables.Add(New ClickableItem(r, word))
                            End If
                            If Me.ShowGlyphBoundingBoxes.Checked Then
                                For Each glyph As OcrGlyph In word.Glyphs
                                    r = GetInfoBounds(glyph.Bounds, InfoLocation.ur, QuestionUp)
                                    _clickables.Add(New ClickableItem(r, glyph))
                                Next glyph
                            End If
                        Next word
                    Next line
                ElseIf TypeOf region Is OcrImageRegion Then
                    r = GetInfoBounds(region.Bounds, InfoLocation.ul, QuestionUp)
                    _clickables.Add(New ClickableItem(r, region))
                End If
            Next region
        End Sub

        Private Sub OcrPane_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles OcrPane.Paint
            Dim g As Graphics = e.Graphics
            If _theDoc Is Nothing Then
                Dim clipRect As Rectangle = e.ClipRectangle
                g.FillRectangle(Brushes.AliceBlue, clipRect)
            Else
                Dim page As OcrPage = _theDoc.Pages(0)

                Dim clipRect As Rectangle = e.ClipRectangle
                g.FillRectangle(Brushes.White, clipRect)

                clipRect.X -= OcrPane.AutoScrollPosition.X
                clipRect.Y -= OcrPane.AutoScrollPosition.Y

                g.TranslateTransform(OcrPane.AutoScrollPosition.X, OcrPane.AutoScrollPosition.Y)

                For Each region As OcrRegion In page.Regions
                    If TypeOf region Is OcrTextRegion Then
                        Dim textRegion As OcrTextRegion = CType(region, OcrTextRegion)
                        For Each line As OcrLine In textRegion.Lines
                            DrawLine(g, line, page.Resolution)
                        Next line
                    ElseIf TypeOf region Is OcrImageRegion Then
                        Dim destRect As Rectangle = region.Bounds
                        destRect.Offset(OcrPane.AutoScrollPosition.X, OcrPane.AutoScrollPosition.Y)
                        Dim imageRegion As OcrImageRegion = CType(region, OcrImageRegion)
                        imageRegion.Image.Draw(g, destRect)
                        g.DrawRectangle(PicturePen, imageRegion.Bounds)
                        DrawInfoIcon(g, imageRegion.Bounds, InfoLocation.ul, QuestionUp)
                    End If
                Next region
            End If
        End Sub

        Private Function GetInfoBounds(ByVal bounds As Rectangle, ByVal loc As InfoLocation, ByVal icon As Icon) As Rectangle
            Dim x, y As Integer
            Select Case loc
                Case InfoLocation.bl
                    x = bounds.Left
                    y = bounds.Bottom - icon.Height
                Case InfoLocation.br
                    x = bounds.Right - icon.Width
                    y = bounds.Bottom - icon.Height
                Case InfoLocation.ul
                    x = bounds.Left
                    y = bounds.Top
                Case Else
                    'Case InfoLocation.ur
                    x = bounds.Right - icon.Width
                    y = bounds.Top
            End Select
            Return New Rectangle(x, y, icon.Width, icon.Height)
        End Function

        Private Sub DrawInfoIcon(ByVal g As Graphics, ByVal bounds As Rectangle, ByVal icon As Icon)
            g.DrawIcon(icon, bounds)
        End Sub

        Private Sub DrawInfoIcon(ByVal g As Graphics, ByVal bounds As Rectangle, ByVal loc As InfoLocation, ByVal icon As Icon)
            Dim r As Rectangle = GetInfoBounds(bounds, loc, icon)
            g.DrawIcon(icon, r)
        End Sub

        Private Sub DrawLine(ByVal g As Graphics, ByVal line As OcrLine, ByVal imageResolution As Dpi)
            builder.Scale = imageResolution.X / g.DpiX
            Dim mapper As IFontMapper = _engine.FontMapper
            Dim text As String = line.Text
            Dim minX, maxX As Integer
            minX = -1
            maxX = 0
            For Each word As OcrWord In line.Words
                If word.Glyphs.Count > 0 Then
                    Dim bounds As Rectangle = line.Bounds
                    If word.StyleIsUniform(mapper, builder) Then
                        Dim confidence As Double = word.Confidence
                        Dim level As Integer = CInt(255.0 * (1.0 - (confidence * confidence)))
                        Dim cbrush As Brush = New SolidBrush(Color.FromArgb(level, level, level))
                        Dim font As Font = word.GetFontAt(imageResolution, mapper, builder, 0)
                        Dim size As Single = font.Size
                        Dim style As FontStyle = font.Style
                        Dim family As FontFamily = word.GetFontFamilyAt(mapper, 0)
                        Dim emheight As Single = family.GetEmHeight(style)
                        Dim conversionToPixels As Single = size / emheight
                        Dim spacing As Single = family.GetLineSpacing(style)
                        Dim correction As Integer = CInt(spacing * conversionToPixels)

                        g.DrawString(word.Text, font, cbrush, word.Bounds.X, line.Baseline - correction, StringFormat.GenericTypographic)
                        If Me.ShowWordBoundingBoxes.Checked Then
                            g.DrawRectangle(WordBoundingBoxPen, word.Bounds)
                            DrawInfoIcon(g, word.Bounds, InfoLocation.ul, QuestionUp)
                        End If
                        If Me.ShowWordBaselines.Checked Then
                            g.DrawLine(BaselinePen, word.Bounds.X, word.Baseline, word.Bounds.Right, word.Baseline)
                        End If
                        cbrush.Dispose()
                        If minX < 0 Then
                            minX = word.Bounds.X
                        ElseIf word.Bounds.X < minX Then
                            minX = word.Bounds.X
                        End If
                        If word.Bounds.Right > maxX Then
                            maxX = word.Bounds.Right
                        End If
                    End If
                    If ShowGlyphBoundingBoxes.Checked Then
                        For Each glyph As OcrGlyph In word.Glyphs
                            g.DrawRectangle(GlyphBoundingBoxPen, glyph.Bounds)
                            DrawInfoIcon(g, glyph.Bounds, InfoLocation.ur, QuestionUp)
                        Next glyph
                    End If
                End If
            Next word
            If Me.ShowLineBaselines.Checked Then
                g.DrawLine(GlobalBaselinePen, minX, line.Baseline, maxX, line.Baseline)
            End If
            If Me.ShowLineBoundingBoxes.Checked Then
                g.DrawRectangle(LineBoundingBoxPen, line.Bounds)
                DrawInfoIcon(g, line.Bounds, InfoLocation.bl, QuestionUp)
            End If
            If Me.ShowFontNames.Checked Then
                Dim fontName As String = line.GetFontNameAt(mapper, 0)
                Dim size As SizeF = g.MeasureString(fontName, FontNameFont)
                Dim r As Rectangle = New Rectangle(line.Bounds.Left + 18, line.Bounds.Bottom - (CInt(size.Height)) - 2, (CInt(size.Width)) + 3, (CInt(size.Height)) + 2)
                g.FillRectangle(FontBrush, r)
                g.DrawRectangle(Pens.Black, r)
                g.DrawString(fontName, FontNameFont, Brushes.Black, CSng(r.X + 1), CSng(r.Y + 1))
            End If
        End Sub

        Private Sub OcrPane_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles OcrPane.MouseDown
            Dim X As Integer = e.X - OcrPane.AutoScrollPosition.X
            Dim Y As Integer = e.Y - OcrPane.AutoScrollPosition.Y
            _clicked = Nothing
            If e.Button = MouseButtons.Left Then
                For Each o As Object In _clickables
                    Dim item As ClickableItem = CType(o, ClickableItem)
                    If item.Bounds.Contains(X, Y) Then
                        Dim g As Graphics = OcrPane.CreateGraphics()
                        g.TranslateTransform(OcrPane.AutoScrollPosition.X, OcrPane.AutoScrollPosition.Y)
                        DrawInfoIcon(g, item.Bounds, QuestionDown)
                        g.Dispose()
                        _clicked = item
                        _wasInClicked = True
                    End If
                Next o
            End If
        End Sub

        Private Sub OcrPane_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles OcrPane.MouseUp
            Dim X As Integer = e.X - OcrPane.AutoScrollPosition.X
            Dim Y As Integer = e.Y - OcrPane.AutoScrollPosition.Y
            If Not _clicked Is Nothing Then
                If _clicked.Bounds.Contains(X, Y) Then
                    Dim g As Graphics = OcrPane.CreateGraphics()
                    g.TranslateTransform(OcrPane.AutoScrollPosition.X, OcrPane.AutoScrollPosition.Y)
                    DrawInfoIcon(g, _clicked.Bounds, QuestionUp)
                    g.Dispose()
                    Dim o As Object = _clicked.Thing
                    DisplayInfo(o)
                End If
                _clicked = Nothing
            End If
        End Sub

        Private Sub DisplayInfo(ByVal o As Object)
            Dim typename As String = "unknown"
            If TypeOf o Is OcrImageRegion Then
                typename = "Image"
            ElseIf TypeOf o Is OcrTextRegion Then
                typename = "Text Region"
            ElseIf TypeOf o Is OcrLine Then
                typename = "Line"
            ElseIf TypeOf o Is OcrWord Then
                typename = "Word"
            ElseIf TypeOf o Is OcrGlyph Then
                typename = "Glyph"
            End If
            Dim myParameters As Parameters = New Parameters(typename, o)
            myParameters.ShowDialog(Me)
        End Sub

        Private Sub OcrPane_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles OcrPane.MouseMove
            Dim X As Integer = e.X - OcrPane.AutoScrollPosition.X
            Dim Y As Integer = e.Y - OcrPane.AutoScrollPosition.Y
            If Not _clicked Is Nothing Then
                Dim inClicked As Boolean = _clicked.Bounds.Contains(X, Y)
                If inClicked <> _wasInClicked Then
                    Dim g As Graphics = OcrPane.CreateGraphics()
                    g.TranslateTransform(OcrPane.AutoScrollPosition.X, OcrPane.AutoScrollPosition.Y)
                    If inClicked Then
                        DrawInfoIcon(g, _clicked.Bounds, QuestionDown)
                    Else
                        DrawInfoIcon(g, _clicked.Bounds, QuestionUp)
                    End If
                    g.Dispose()
                    _wasInClicked = inClicked
                End If
            End If
        End Sub

        Private Sub ShowWordBaselines_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ShowWordBaselines.Click
            ShowWordBaselines.Checked = Not ShowWordBaselines.Checked
            OcrPane.Invalidate()
            If Not _theDoc Is Nothing Then
                BuildClickables(_theDoc.Pages(0))
            End If
        End Sub

        Private Sub ShowLineBaselines_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ShowLineBaselines.Click
            ShowLineBaselines.Checked = Not ShowLineBaselines.Checked
            OcrPane.Invalidate()
            If Not _theDoc Is Nothing Then
                BuildClickables(_theDoc.Pages(0))
            End If
        End Sub

        Private Sub ShowLineBoundingBoxes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ShowLineBoundingBoxes.Click
            ShowLineBoundingBoxes.Checked = Not ShowLineBoundingBoxes.Checked
            OcrPane.Invalidate()
            If Not _theDoc Is Nothing Then
                BuildClickables(_theDoc.Pages(0))
            End If
        End Sub

        Private Sub ShowWordBoundingBoxes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ShowWordBoundingBoxes.Click
            ShowWordBoundingBoxes.Checked = Not ShowWordBoundingBoxes.Checked
            OcrPane.Invalidate()
            If Not _theDoc Is Nothing Then
                BuildClickables(_theDoc.Pages(0))
            End If
        End Sub

        Private Sub ShowGlyphBoundingBoxes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ShowGlyphBoundingBoxes.Click
            ShowGlyphBoundingBoxes.Checked = Not ShowGlyphBoundingBoxes.Checked
            OcrPane.Invalidate()
            If Not _theDoc Is Nothing Then
                BuildClickables(_theDoc.Pages(0))
            End If
        End Sub

        Private Sub ShowFontNames_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ShowFontNames.Click
            ShowFontNames.Checked = Not ShowFontNames.Checked
            OcrPane.Invalidate()
            If Not _theDoc Is Nothing Then
                BuildClickables(_theDoc.Pages(0))
            End If
        End Sub

        Private Sub AboutButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AboutButton.Click
            Dim aboutBox As AtalaDemos.AboutBox.About = New AtalaDemos.AboutBox.About("About Atalasoft DotImage OCR Diagnostic Demo", "DotImage OCR Diagnostic Demo")
            aboutBox.Description = "The purpose of this demo is to show what the OCR engine recognizes in a document using the various engines supplied with DotImage OCR.  When translating an image, all the areas that are recognized are shown, along with the resulting text.  This program is useful for diagnosing OCR, comparing results with different engines, and to demonstrate some more advanced features using the OcrDocument class, which can be used to traverse a recognized page.  Requires DotImage, DotImage OCR, and a license for at least one OCR Engine."
            aboutBox.ShowDialog()
        End Sub


        Private Sub MenuTesseract5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuTesseract5.Click

            If MenuTesseract5.Checked AndAlso _engine Is _tesseract5Engine Then
                Return
            End If
            If _tesseract5Engine Is Nothing Then
                Try
                    _tesseract5Engine = New Tesseract5Engine
                Catch err As Exception
                    MessageBox.Show(Me, "Unable to Create Tesseract 3 Engine: " & err.Message)
                    _tesseract5Engine = Nothing
                End Try
            End If

            If _tesseract5Engine Is Nothing Then
                Return
            End If

            menuGlyphReader.Checked = False
            MenuTesseract5.Checked = True
            MenuOmniPage.Checked = False


            SwapEngine(_tesseract5Engine)
            CreateLanguageMenu()
        End Sub

        Private Sub MenuOmniPage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuOmniPage.Click
            If MenuOmniPage.Checked AndAlso _engine Is _omniPageEngine Then
                Return
            End If
            If _omniPageEngine Is Nothing Then
                Try
                    _omniPageEngine = New OmniPageEngine
                Catch err As Exception
                    MessageBox.Show(Me, "Unable to Create OmniPage Engine: " & err.Message)
                    _omniPageEngine = Nothing
                End Try
            End If


            If _omniPageEngine Is Nothing Then
                Return
            End If

            menuGlyphReader.Checked = False
            MenuTesseract5.Checked = False
            MenuOmniPage.Checked = True


            SwapEngine(_omniPageEngine)
            CreateLanguageMenu()

        End Sub

        Private Sub SwapEngine(ByVal newEngine As OcrEngine)
            _engine.ShutDown()
            _engine = newEngine
            _engine.Initialize()
            MapNativeOptionsToMenus()
        End Sub

        Private Sub language_Click(ByVal sender As Object, ByVal e As EventArgs)
            For Each item As MenuItem In Me.menuLanguage.MenuItems
                item.Checked = False
            Next item
            Dim selecteditem As MenuItem = CType(sender, MenuItem)
            selecteditem.Checked = True
            Dim cultures As CultureInfo() = _engine.GetSupportedRecognitionCultures()
            For Each info As CultureInfo In cultures
                If info.DisplayName = selecteditem.Text Then
                    _engine.RecognitionCulture = info
                End If
            Next info
        End Sub

        Private Sub CreateLanguageMenu()
            'build culture menu
            Me.menuLanguage.MenuItems.Clear()
            Dim cultures As CultureInfo() = _engine.GetSupportedRecognitionCultures()
            Dim ev As EventHandler = New EventHandler(AddressOf language_Click)
            For Each info As CultureInfo In cultures
                Dim mi As MenuItem = New MenuItem(info.DisplayName, ev)
                Me.menuLanguage.MenuItems.Add(mi)
                If _engine.RecognitionCulture.Name = info.Name Then
                    mi.Checked = True
                End If
            Next info
        End Sub

        Private Sub menuGlyphReader_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuGlyphReader.Click
            If menuGlyphReader.Checked AndAlso _engine Is _glyphReaderEngine Then
                Return
            End If
            menuGlyphReader.Checked = True
            MenuTesseract5.Checked = False
            MenuOmniPage.Checked = False
            SwapEngine(_glyphReaderEngine)
            CreateLanguageMenu()
        End Sub

        Private Sub _engine_DocumentProgress(ByVal sender As Object, ByVal e As OcrDocumentProgressEventArgs)
            Me.statusBar1.Text = EnglishStringFromOcrStage(e.Stage)
            Me.progressBar1.Value = e.Progress
            Me.statusBar1.Refresh()
            Me.progressBar1.Refresh()
        End Sub

        Private Function EnglishStringFromOcrStage(ByVal stage As OcrDocumentStage) As String
            Select Case stage
                Case OcrDocumentStage.BeginDocument
                    Return "Recognizing Document..."
                Case OcrDocumentStage.BeginPage
                    Return "Recognizing Page..."
                Case OcrDocumentStage.EndPage
                    Return "End Recognizing Page..."
                Case OcrDocumentStage.EndDocument
                    Return "Done Recognizing"
                Case Else
                    Return ""


            End Select
        End Function


    End Class
End Namespace
