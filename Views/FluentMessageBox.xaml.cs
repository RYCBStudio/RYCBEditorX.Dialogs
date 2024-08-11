using System.Windows;
using System.Windows.Media;

namespace RYCBEditorX.Dialogs.Views;
/// <summary>
/// FluentMessageBox.xaml 的交互逻辑
/// </summary>
public partial class FluentMessageBox : Window
{

    public static FluentMessageBox Instance
    {
        get; private set;
    }

    public static string Theme
    {
        get; set;
    }
    public FluentMessageBox()
    {
        InitializeComponent();
        Instance = this;
        TitleBar.Background = Theme switch
        {
            "Info" => (Brush)Resources["InfoColor"],
            "Warn" => (Brush)Resources["WarnColor"],
            "Error" => (Brush)Resources["ErrorColor"],
            _ => (Brush)Resources["InfoColor"],
        };
        OKBtn.Background = Theme switch
        {
            "Info" => (Brush)Resources["InfoColor"],
            "Warn" => (Brush)Resources["WarnColor"],
            "Error" => (Brush)Resources["ErrorColor"],
            _ => (Brush)Resources["InfoColor"],
        };
    }
}
