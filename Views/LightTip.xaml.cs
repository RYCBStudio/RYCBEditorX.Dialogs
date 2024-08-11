using System;
using System.Windows;
using System.Windows.Media;
using RYCBEditorX.Dialogs.ViewModels;

namespace RYCBEditorX.Dialogs.Views;
/// <summary>
/// LightTip.xaml 的交互逻辑
/// </summary>
public partial class LightTip : Window
{
    private Window _Parent;
    private System.Timers.Timer _Timer;

    public static LightTip Instance
    {
        get; private set;
    }
    public static LightTipViewModel ViewModelInstance
    {
        get; private set;
    }

    public const string INFO = "\xe60e";
    public const string WARN = "\xe6d1";
    public const string ERROR = "\xeb37";

    public LightTip(Window parent)
    {
        InitializeComponent();
        _Timer = new(5000);
        _Timer.Elapsed += AutoClose;
        _Parent = parent;
        this.Topmost = true;
        Instance = this;
        ViewModelInstance = new LightTipViewModel();
        this.DataContext = ViewModelInstance;
        if (GlobalConfig.Skin == "dark")
        {
            MainPanel.Background = (Brush)Application.Current.Resources["DarkBackGround"];
        }
        else
        {
            MainPanel.Background = (Brush)Application.Current.Resources["LightBackGround"];
        }
    }

    private void AutoClose(object sender, System.Timers.ElapsedEventArgs e)
    {
        _Timer.Stop();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        this.Hide();
    }

    private void Window_ContentRendered(object sender, EventArgs e)
    {
        _Timer.Start();
        var xpos = _Parent.Left + _Parent.ActualWidth - this.ActualWidth - 10;
        var ypos = _Parent.Top + _Parent.ActualHeight - this.ActualHeight - 10;
        if (_Parent.WindowState == WindowState.Maximized)
        {
            xpos = 0 + _Parent.ActualWidth - this.ActualWidth - 20;
            ypos = 0 + _Parent.ActualHeight - this.ActualHeight - 30;
        }
        this.Left = xpos;
        this.Top = ypos;
    }
}
