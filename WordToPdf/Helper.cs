using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordToPdf
{
    internal static class Helper
    {
        // Convert method
        internal static void ConvertToPdf(string input, string output, WdSaveFormat format = WdSaveFormat.wdFormatPDF)
        {
            // Create an instance of Word.exe
            var oWord = new Application
            {
                // Make this instance of word invisible (Can still see it in the taskmgr).
                Visible = false
            };

            // Interop requires objects.
            object oMissing = System.Reflection.Missing.Value;
            object isVisible = true;
            object readOnly = true;     // Does not cause any word dialog to show up
            //object readOnly = false;  // Causes a word object dialog to show at the end of the conversion
            object oInput = input;
            object oOutput = output;
            object oFormat = format;

            // Load a document into our instance of word.exe
            _Document oDoc = oWord.Documents.Open(
                ref oInput, ref oMissing, ref readOnly, ref oMissing, ref oMissing,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                ref oMissing, ref isVisible, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                );

            // Make this document the active document.
            oDoc.Activate();

            // Save this document using Word
            oDoc.SaveAs(ref oOutput, ref oFormat, ref oMissing, ref oMissing,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                );

            // Always close Word.exe.
            oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
        }

        internal static void ConvertToPdf(List<string> inputFileList, LogDelegate logHandler)
        {
            var background = new BackgroundWorker { WorkerReportsProgress = true };
            background.DoWork += (ww, ee) =>
            {
                // Create an instance of Word.exe
                var oWord = new Application { Visible = false };
                // Interop requires objects.
                object oMissing = System.Reflection.Missing.Value;
                object isVisible = true;
                object readOnly = true;     // Does not cause any word dialog to show up
                object oFormat = WdSaveFormat.wdFormatPDF;
                foreach (var fileName in inputFileList)
                {
                    object oInput = fileName;
                    object oOutput = Path.Combine(Path.GetDirectoryName(fileName), $"{Path.GetFileNameWithoutExtension(fileName)}.pdf");
                    // Load a document into our instance of word.exe
                    var oDoc = oWord.Documents.Open(
                        ref oInput, ref oMissing, ref readOnly, ref oMissing, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                        ref oMissing, ref isVisible, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                    // Make this document the active document.
                    oDoc.Activate();
                    // Save this document using Word
                    oDoc.SaveAs(ref oOutput, ref oFormat, ref oMissing, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                    background.ReportProgress(0, $"{fileName} 转换完成");
                }
                // Always close Word.exe.
                oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
            };
            background.ProgressChanged += (ww, ee) =>
            {
                if (ee.UserState is string msg) logHandler(msg);
            };
            background.RunWorkerCompleted += (ww, ee) =>
            {
                logHandler("转换完成");
            };
            background.RunWorkerAsync();
        }

        internal delegate void LogDelegate(string msg);
    }
}
