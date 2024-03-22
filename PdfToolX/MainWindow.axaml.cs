using Avalonia.Controls;

namespace PdfToolX;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        Title = $"PDF工具集 {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";
    }
}