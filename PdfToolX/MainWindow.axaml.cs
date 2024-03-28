using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System.Diagnostics;

namespace PdfToolX;

public partial class MainWindow : Window
{
    #region constructor
    public MainWindow()
    {
        InitializeComponent();

        Title = $"PDF工具集 {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";
    }
    #endregion

    #region property
    private const string Url4Appreciate = "https://www.yuque.com/docs/share/4d2ad434-a4fe-40a1-b530-c61811d5226e?# 《打赏说明》";
    private const string Url4Feedback = "https://www.yuque.com/lengda/eq8cm6/ezwik4?singleDoc# 《需求记录》";
    #endregion

    #region event handler
    private void TxtLink1_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var psi = new ProcessStartInfo
        {
            FileName = Url4Appreciate,
            UseShellExecute = true
        };
        Process.Start(psi);
    }

    private void TxtLink2_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var psi = new ProcessStartInfo
        {
            FileName = Url4Feedback,
            UseShellExecute = true
        };
        Process.Start(psi);
    }
    #endregion
}