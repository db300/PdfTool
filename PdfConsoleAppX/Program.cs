namespace PdfConsoleAppX
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            //测试骑缝章方法
            const string inputFileName = @"骑缝章测试文件.pdf";
            const string stampFileName = @"骑缝章文件.png";
            PdfHelperLibrary.SealHelper.CrossPageSeal(stampFileName, inputFileName);
            return;
        }
    }
}
