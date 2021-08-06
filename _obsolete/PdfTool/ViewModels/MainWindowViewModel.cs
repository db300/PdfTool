using System;
using System.Collections.Generic;
using System.Text;

namespace PdfTool.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            var fileName = @"C:\Users\�仳��\Downloads\���Ǵ��½������.pdf";
            var text1 = PdfHelper.PdfTextExtractor.GetText(fileName);
            System.Diagnostics.Debug.WriteLine(text1);
            var text = PdfSharpTextExtractor.Extractor.PdfToText(fileName);
            System.Diagnostics.Debug.WriteLine(text);
        }

        public string Greeting => "Welcome to Avalonia!";
    }
}
