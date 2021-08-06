using System;
using System.Collections.Generic;
using System.Text;

namespace PdfTool.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            var fileName = @"C:\Users\冷怀晶\Downloads\汉仪创新解决方案.pdf";
            var text1 = PdfHelper.PdfTextExtractor.GetText(fileName);
            System.Diagnostics.Debug.WriteLine(text1);
            var text = PdfSharpTextExtractor.Extractor.PdfToText(fileName);
            System.Diagnostics.Debug.WriteLine(text);
        }

        public string Greeting => "Welcome to Avalonia!";
    }
}
