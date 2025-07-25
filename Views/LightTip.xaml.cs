﻿using System;
using System.Windows;
using System.Windows.Media;
using RYCBEditorX.Dialogs.ViewModels;

namespace RYCBEditorX.Dialogs.Views;

/// <summary>
/// LightTip.xaml 的交互逻辑
/// </summary>
public partial class LightTip : Window
{
    private readonly Window _Parent;
    private readonly System.Timers.Timer _Timer;

    public static LightTip Instance
    {
        get; private set;
    }
    public static LightTipViewModel ViewModelInstance
    {
        get; private set;
    }

    public LightTip(Window parent)
    {
        InitializeComponent();
        _Timer = new(5000);
        _Timer.Elapsed += AutoClose;
        _Parent = parent;
        Topmost = true;
        Instance = this;
        ViewModelInstance = new LightTipViewModel();
        DataContext = ViewModelInstance;
        ContentMd.Markdown = ViewModelInstance.Content;
        if (GlobalConfig.Skin == "dark")
        {
            MainPanel.Background = (Brush)Application.Current.Resources["DarkBackGround"];
            ContentMd.Theme = "dark";
        }
        else
        {
            MainPanel.Background = (Brush)Application.Current.Resources["LightBackGround"];
            ContentMd.Theme = "light";
        }
    }

    private void AutoClose(object sender, System.Timers.ElapsedEventArgs e)
    {
        _Timer.Stop();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Hide();
    }

    private void Window_ContentRendered(object sender, EventArgs e)
    {
        _Timer.Start();
        ContentMd.Markdown = ViewModelInstance.Content;
        var xpos = _Parent.Left + _Parent.ActualWidth - ActualWidth - 10;
        var ypos = _Parent.Top + _Parent.ActualHeight - ActualHeight - 10;
        if (_Parent.WindowState == WindowState.Maximized)
        {
            xpos = 0 + _Parent.ActualWidth - ActualWidth - 20;
            ypos = 0 + _Parent.ActualHeight - ActualHeight - 30;
        }
        Left = xpos;
        Top = ypos;
    }

    private void OpenHyperlink(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
    {
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
        {
            FileName = e.Parameter.ToString(),
            UseShellExecute = true,
        });
    }

    private void ClickOnImage(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
    {

    }
}
