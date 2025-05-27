using System.Collections.Generic;

namespace ExcelTool
{
    internal class Common
    {
    }

    internal interface IExcelHandler
    {
        void OpenExcels(List<string> files);
    }
}
