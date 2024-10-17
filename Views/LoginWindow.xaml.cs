using System;
using System.Windows;
using System.Windows.Input;
using HandyControl.Tools.Extension;
using RYCBEditorX.MySQL;

namespace RYCBEditorX.Dialogs.Views;
/// <summary>
/// LoginWindow.xaml 的交互逻辑
/// </summary>
public partial class LoginWindow : HandyControl.Controls.Window
{

    private ISQLConnectionUtils SQLConnectionUtils { get; set; }
    private int attempts = 0;

    public static LoginWindow Instance { get; private set; }
    public string UserName { get; set; }
    public string UsrName { get; set; }
    public string Password { get; set; }

    public LoginWindow()
    {
        InitializeComponent();
        SQLConnectionUtils = MySQLModule.ConnectionUtils;
        Instance = this;
    }

    public void Login_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            Login(sender, e);
        }
    }

    public void Login(object sender, RoutedEventArgs e)
    {
        var isAuto = false;
        try
        {
            isAuto = (bool)sender;
        }
        catch { }
        if (isAuto) {
            attempts = 0;
        }
        else
        {
            attempts++;
            var result = SQLConnectionUtils?.Select("users");
            if (result is null) { return; }
            foreach (var user in result)
            {
                if (user["name"].ToString().Equals(NameBox.Text, StringComparison.CurrentCultureIgnoreCase) && user["password"].ToString() == PwdBox.Password)
                {
                    UserName = user["name"].ToString();
                    Password = user["password"].ToString();
                    UsrName = user["username"].ToString();
                    attempts = 0;
                    DialogResult = true;
                    return;
                }
            }
            if (attempts > 5)
            {
                DialogResult = false;
            }
            ErrorText.Show();
        }
    }

    private void Register(object sender, RoutedEventArgs e)
    {
        new RegisterWindow(this).ShowDialog();
    }
}
