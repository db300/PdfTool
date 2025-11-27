using System.Collections.Generic;

namespace AudioTool
{
    internal class Common
    {
    }

    internal interface IMp3Handler
    {
        void OpenMp3s(List<string> files);
    }
}
