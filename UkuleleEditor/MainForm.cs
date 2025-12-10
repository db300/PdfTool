using System.Reflection;

namespace UkuleleEditor
{
    public partial class MainForm : Form
    {
        #region constructor
        public MainForm()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;
            // 优先显示 AssemblyVersion (通常是 1.0.0.0)，若不可用则回退到 Application.ProductVersion
            var asmVersion = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? Application.ProductVersion;
            Text = $"尤克里里编辑器 Ukulele Editor v{asmVersion}";

            // 在 MainForm.cs 中使用
            var bitmap = SheetMusicRenderer.RenderSheetMusic("tests\\test.json");
            bitmap.Save("output.jpg");
        }
        #endregion
    }
}
