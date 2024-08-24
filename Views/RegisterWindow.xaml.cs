using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HandyControl.Tools.Extension;
using Mono.Unix.Native;
using MySqlX.XDevAPI.Common;
using RYCBEditorX.MySQL;
using RYCBEditorX.Utils;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace RYCBEditorX.Dialogs.Views;
/// <summary>
/// RegisterWindow.xaml 的交互逻辑
/// </summary>
public partial class RegisterWindow : HandyControl.Controls.Window
{

    private readonly ISQLConnectionUtils _sqlConnectionUtils;

    public RegisterWindow(Window parent)
    {
        InitializeComponent();
        Owner = parent;
        _sqlConnectionUtils = MySQLModule.ConnectionUtils;
    }

    public RegisterWindow()
    {
        InitializeComponent();
        _sqlConnectionUtils = MySQLModule.ConnectionUtils;
    }
    private void Register(object sender, RoutedEventArgs re)
    {

        if (!IsPasswordMatch(PwdBox.Password, ConfirmPwdBox.Password))
        {
            FailTip.Text = GlobalConfig.Resources["Register.UnmatchedPwd"].ToString();
            FailTip.Show();
        }
        if (!IsPasswordValid(PwdBox.Password))
        {
            FailTip.Text = GlobalConfig.Resources["Register.IllegalPwd"].ToString();
            FailTip.Show();
        }
        if (!IsPasswordMatch(PwdBox.Password, ConfirmPwdBox.Password) || !IsPasswordValid(PwdBox.Password))
        {
            return;
        }

        var result = _sqlConnectionUtils?.Select("users", order_by: SQL_ORDER_BY_KEYWORDS.DESC, order_by_field: "uid");

        if (result is null) { return; }
        foreach (var user in result)
        {
            if (user["name"].ToString().Equals(NameBox.Text, StringComparison.CurrentCultureIgnoreCase) && user["password"].ToString() == PwdBox.Password)
            {
                FailTip.Show();
                GlobalConfig.CurrentLogger.Log($"用户{user["name"]}已存在，uid为:{user["uid"]}");
                return;
            }
        }
        FailTip.Hide();
        var latest_uid = int.Parse(result[0]["uid"].ToString()) + 1;

        // 定义表名
        var tableName = "users";

        // 定义字段列表
        var fieldsList = "uid, name, password, create_time, signature";

        // 定义值列表，并对特殊字符进行转义（例如引号）
        var valuesList = $"{latest_uid}, '{NameBox.Text}', '{PwdBox.Password}', '{DateTime.Now:yyyy-MM-dd HH:mm:ss}', ''";

        // 执行插入操作
        var success = _sqlConnectionUtils.Insert(tableName, fieldsList, valuesList);

        if (success)
        {
            GlobalConfig.CurrentLogger.Log($"成功创建新的用户，UID为'{latest_uid}'。", module: EnumLogModule.SQL);
            LoginWindow.Instance.UserName = NameBox.Text;
            LoginWindow.Instance.Password = PwdBox.Password;
            LoginWindow.Instance.Login(true, null);
            DialogResult = true;
        }
        else
        {
            GlobalConfig.CurrentLogger.Log($"创建新的用户失败，UID为'{latest_uid}'。", module: EnumLogModule.SQL);
        }
    }

    /// <summary>
    /// 判断密码是否匹配
    /// </summary>
    /// <param name="password"></param>
    /// <param name="confirmPassword"></param>
    /// <returns></returns>
    private static bool IsPasswordMatch(string password, string confirmPassword)
    {
        return password == confirmPassword;
    }

    /// <summary>
    /// 判断密码是否合法
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    private static bool IsPasswordValid(string password)
    {
        var suc = false;
        //第一个逻辑：密码长度在8~20位之间
        suc = password.Length >= 8 & password.Length <= 20;
        //第二个逻辑：密码中包含至少一个小写字母
        suc &= password.Any(char.IsLower);
        //第三个逻辑：密码中包含至少一个数字
        suc &= password.Any(char.IsDigit);
        return suc;
    }

    private void Register(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter) { 
            Register(sender, re:e);
        }
    }

    private void Close(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}
