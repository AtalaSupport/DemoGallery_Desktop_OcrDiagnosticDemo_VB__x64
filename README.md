# OCR Diagnostic Demo
The purpose of this demo is to show what the OCR engine recognizes in a document using the various engines supplied with DotImage OCR.  When translating an image, all the areas that are recognized are shown, along with the resulting text.  

This program is useful for diagnosing OCR, comparing results with different engines, and to demonstrate some more advanced features using the OcrDocument class, which can be used to traverse a recognized page. 

This is the VB.NET version. We have a [C# version](https://github.com/AtalaSupport/DemoGallery_Desktop_OcrDiagnosticDemo_CS_x64) available.


## Licensing
This application requires a license for DotImage Document Imaging as well as our OCR addon (either on its own to enable just Tesseract5Engine) or with our GlyphReader or OmniPage addon. You may also request a 30 day evaulation if youre evaluating if DotImage / our OCR is right for you


## SDK Dependencies
This app was built based on 2026.2.0.0. It targets .NET Framework 4.6.2 and was created in Visual Studio 2022. You must have our SDK installed (and licesed per above)

[Download DotImage](https://www.atalasoft.com/BeginDownload/DotImageDownloadPage)


### OmniPage
Our OmniPageEngine has additional OCR resource requirements not included in our main SDK download (they would nearly double the size of the SDK download so we make them available separately to those who wish to use OmniPageEngine). They cna be found here: [INFO: OmniPageEngine Overview](https://www.atalasoft.com/kb2/KB/50396/INFO-OmniPageEngine-Overview)


### Using NuGet for SDK Dependencies
We do publish our SDK components to NuGet. We have chosen to base the demo on local installed SDK because this leads to much smaller applications (NuGet packages add a lot of overhead due to the way they're packaged and deployed, and many of our demos -- including this one -- are often used to reproduce issues that need to be submitted to support. Apps that use NuGet are often significantly larger and run up against our maximum support case upload size)

Still, if you wish to use NuGet for the dependencies instead of relying on locally installed SDK, you can.

- Take note of each of the references we've included:
    - Atalasoft.DotImage.dll
    - Atalasoft.DotImage.Dicom.dll
    - Atalasoft.DotImage.Dwg.dll
    - Atalasoft.DotImage.Heif.dll
    - Atalasoft.DotImage.Jbig2.dll
    - Atalasoft.DotImage.Jpeg2000.dll
    - Atalasoft.DotImage.Lib.dll
    - Atalasoft.DotImage.Ocr.GlyphReader.dll
    - Atalasoft.DotImage.Ocr.OmniPage.dll
    - Atalasoft.DotImage.Ocr.Tesseract5.dll
    - Atalasoft.DotImage.Pdf.dll
    - Atalasoft.DotImage.PdfDoc.Bridge.dll
    - Atalasoft.DotImage.PdfReader.dll
    - Atalasoft.DotImage.Raw.dll
    - Atalasoft.DotImage.WinControls.dll
    - Atalasoft.PdfDoc.dll
    - Atalasoft.Shared.dll
- Remove those referneces
- Open the NuGet Package Manger from `Tools -> NuGet Package Manager -> Manage NuGet Packages for this Solution`
- Browse for and install  Atalasoft.DotImage.WinControls.x64 - It will pull in DotImage Document Imaging (the base SDK) and our windows controls and shared dll
- Browse for and install Atalasoft.Ocr.x64 - Required to bring in the base OCR and Tesseract dependencies
- Browse for and install Atalasoft.Ocr.Tesseractt5.x64 - Required to bring in Tesseract5Engine
- Browse for and install Atalasoft.Ocr.Tesseract5.Resources - Required to bring in the Tesseract5 OCR Resources
- Browse for and install Atalasoft.Ocr.GlyphReader.x64 -  Required if you wish to use GlyphReaderEngine
- Browse for and install Atalasoft.Ocr.GlyphReader.Resources -  Required if you wish to use GlyphReaderEngine
- Browse for and install Atalasoft.Ocr.OmniPage.x64. Required if you wish to use OmniPageEngine
- Browse for and install Atalasoft.Dwg.x64. (optional if you wish to have support for DWG and other CADD files)
- Browse for and install Atalasoft.Dicom.x64. (optional if you wish to have support for Dicom files)
- Browse for and install Atalasoft.Jbig2.x64. (optional if you wish to have support for Jbig2 files)
- Browse for and install Atalasoft.Jpeg2000.x64. (optional if you wish to have support for Jpeg2000 files)
- Browse for and install Atalasoft.Pdf.x64  to bring in the PdfEncoder
- Browse for and install Atalasoft.PdfReader.x64. (optional if you wish to have support for PDF files)
- Browse for and install Atalasoft.Raw.x64. (optional if you wish to have support for RAW files)


## Downloading source
The sources can be downloaded for [c#](https://github.com/AtalaSupport/DemoGallery_Desktop_OcrDiagnosticDemo_CS_x64/archive/refs/heads/main.zip) and [VB.NET](https://github.com/AtalaSupport/DemoGallery_Desktop_OcrDiagnosticDemo_VB_x64/archive/refs/heads/main.zip)


## Cloning
We recommend the following to ensure you clone with the required submodule

Example: git for windows
```bash
git clone https://github.com/AtalaSupport/DemoGallery_Desktop_OcrDiagnosticDemo_VB__x64.git OcrDiagnosticDemo --recursive
cd OcrDiagnosticDemo
git submodule init
git submodule update
```

## Related documentation
In addition to this README, the Atalasoft documentation set includes the following:  
- [AtalaSupport Github](https://github.com/AtalaSupport/) For an extensive set of sample apps.  
- [Atalasoft's APIs & Developer Guides page](https://www.atalasoft.com/Support/APIs-Dev-Guides) for our Developers guide and API references.  
- [Atalasoft Support](http://www.atalasoft.com/support/) for our main support portal.
- [Atalasoft Knowledgebase](http://www.atalasoft.com/kb2) where you can find answers to common questions / issues.  


## Getting Help for Atalasoft products
Atalasoft regularly updates our support [Knowledgebase](http://www.atalasoft.com/kb2) with the latest information about our products. To access some resources, you must have a valid Support Agreement with an authorized Atalasoft Reseller/Partner or with Atalasoft directly. Use the tools that Atalasoft provides for researching and identifying issues. 

Customers with an active evaluation, or those with active support / maintenance may [create a support case](https://www.atalasoft.com/Support/my-portal/Cases/Create-Case) 24/7, or call in to support ([+1 949 236-6510](tel:19492366510) ) during our normal support hours (Monday - Friday 8:00am to 5:00PM Eastern (New York) time).  

Customers who are unable to create a case or call in may [email our Sales Team](mailto:sales@atalasoft.com).  
