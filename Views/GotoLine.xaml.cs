using System;
using System.Windows.Input;
using HandyControl.Controls;
using Sunny.UI;

namespace RYCBEditorX.Dialogs.Views;
/// <summary>
/// GotoLineWindow.xaml 的交互逻辑
/// </summary>
public partial class GotoLineWindow : Window
{
    private int totalLines;

    public bool isCompleted;
    public static GotoLineWindow Instance { get; private set; }

    public static int Line
    {
        get; set;
    }

    public GotoLineWindow(int totalLines)
    {
        InitializeComponent();
        Instance = this;
        this.totalLines = totalLines;
        TitleBar.Text += $"(0~{totalLines})";
    }

    private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        if (TextBox.Text.Length > 0)
        {
            if (TextBox.Text.IsNumber())
            {
                Line = Convert.ToInt32(TextBox.Text);
                isCompleted = true;
                DialogResult = true;
            }
        }
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter) {
            if (TextBox.Text.Length > 0)
            {
                Line = Convert.ToInt32(TextBox.Text);
                isCompleted = true;
                DialogResult = true;
            }
        }else if (e.Key == Key.Escape)
        {
            isCompleted = false;
            DialogResult = false;
        }
    }
}
