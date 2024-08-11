using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using HandyControl.Controls;
using RYCBEditorX.Utils;
using Sunny.UI;

namespace RYCBEditorX.Dialogs.Views;
/// <summary>
/// RunnerProfileConfig.xaml 的交互逻辑
/// </summary>
public partial class RunnerProfileConfig : Window
{
#pragma warning disable IDE0059 // 不需要赋值
    public List<ProfileInfo> ProfileList
    {
        get; set;
    }
    public List<string> ProfilePathsList
    {
        get; set;
    }

    public RunnerProfileConfig(List<ProfileInfo> profileInfos, List<string> profilePathsList)
    {
        InitializeComponent();
        this.ProfileList = profileInfos;
        ProfilesListBox.ItemsSource = ProfileList;
        ProfilePathsList = profilePathsList;
        RefreshItems(this, new());
    }

    private void ProfilesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var item = ProfilePathsList[ProfilesListBox.SelectedIndex];
        var icbfp = new ICBFileProcessor(item);
        PName.Text = icbfp.GetInfo(ICBFileProcessor.InfoType.name);
        Loca.Text = icbfp.GetInfo(ICBFileProcessor.InfoType.script);
        ItprArgs.Text = icbfp.GetInfo(ICBFileProcessor.InfoType.itpr_args);
        Itpr.Text = icbfp.GetInfo(ICBFileProcessor.InfoType.itpr);
        ScriptArgs.Text = icbfp.GetInfo(ICBFileProcessor.InfoType.script_args);
        UseBPSR.IsChecked = bool.Parse(icbfp.GetInfo(ICBFileProcessor.InfoType.use_bpsr));
        icbfp = null;
    }

    private void DeleteSelectedItem(object sender, System.Windows.RoutedEventArgs e)
    {
        if (ProfilesListBox.SelectedItem != null)
        {
            File.Delete(ProfilePathsList[ProfilesListBox.SelectedIndex]);
            ProfilePathsList.RemoveAt(ProfilesListBox.SelectedIndex);
            ProfileList.RemoveAt(ProfilesListBox.SelectedIndex);
            ProfilesListBox.Items.Refresh();
        }
    }

    private void SaveItem(object sender, System.Windows.RoutedEventArgs e)
    {
        if (ProfilesListBox.SelectedItem != null)
        {
            var item = ProfilePathsList[ProfilesListBox.SelectedIndex];
            var icbfp = new ICBFileProcessor(item);
            icbfp.SetInfo(ICBFileProcessor.InfoType.itpr, PName.Text);
            icbfp.SetInfo(ICBFileProcessor.InfoType.script, Loca.Text);
            icbfp.SetInfo(ICBFileProcessor.InfoType.itpr_args, ItprArgs.Text);
            icbfp.SetInfo(ICBFileProcessor.InfoType.itpr, Itpr.Text);
            icbfp.SetInfo(ICBFileProcessor.InfoType.script_args, ScriptArgs.Text);
            icbfp.SetInfo(ICBFileProcessor.InfoType.use_bpsr, UseBPSR.IsChecked);
            icbfp = null;
        }
    }

    private void ImportProfile(object sender, System.Windows.RoutedEventArgs e)
    {
        var ofd = new OpenFileDialog() { Filter = "|*.icbconfig" };
        if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            foreach (var item in ProfilePathsList)
            {
                if (File.ReadAllText(item) == File.ReadAllText(ofd.FileName))
                {
                    ProfilesListBox.SelectedIndex = ProfilePathsList.IndexOf(item);
                    return;
                }
            }
            File.Copy(ofd.FileName, Path.GetDirectoryName(ProfilePathsList[0]) + Path.GetFileName(ofd.FileName), true);
            ProfilePathsList.Add(ofd.FileName);
            ProfileList.Add(new()
            {
                Name = Path.GetFileName(ofd.FileName),
            });
            ProfilesListBox.Items.Refresh();
        }
    }

    private void RefreshItems(object sender, System.Windows.RoutedEventArgs e)
    {
        GlobalConfig.CurrentProfiles.Clear();
        foreach (var item in ProfilePathsList) {
            var icbfp = new ICBFileProcessor(item);
            GlobalConfig.RunProfile rp = new()
            {
                Name = icbfp.GetInfo(ICBFileProcessor.InfoType.name),
                ScriptPath = icbfp.GetInfo(ICBFileProcessor.InfoType.script),
                InterpreterArgs = icbfp.GetInfo(ICBFileProcessor.InfoType.itpr_args),
                Interpreter = icbfp.GetInfo(ICBFileProcessor.InfoType.itpr),
                ScriptArgs = icbfp.GetInfo(ICBFileProcessor.InfoType.script_args),
                UseBPSR = bool.Parse(icbfp.GetInfo(ICBFileProcessor.InfoType.use_bpsr)),
            };
            icbfp = null;
            GlobalConfig.CurrentProfiles.Add(rp);
        }
    }
}

public class ICBFileProcessor
{
#pragma warning disable IDE0044 // 添加只读修饰符
    private string _path;
    private IniFile _file;

    public ICBFileProcessor(string path)
    {
        _path = path;
        _file = new(_path);
    }

    public string GetInfo(InfoType infoType)
    {
        var ret = "";
        switch (infoType)
        {
            case InfoType.name:
                ret = _file.Read("itprconf", "name", "");
                break;
            case InfoType.script:
                ret = _file.Read("itprconf", "script", "");
                break;
            case InfoType.itpr:
                ret = _file.Read("itprconf", "itpr", "");
                break;
            case InfoType.itpr_args:
                ret = _file.Read("itprconf", "itpr_args", "");
                break;
            case InfoType.script_args:
                ret = _file.Read("itprconf", "script_args", "");
                break;
            case InfoType.use_bpsr:
                ret = _file.Read("itprconf", "use_bpsr", "");
                break;
            default:
                break;
        }
        return ret;
    }

    public void SetInfo(InfoType infoType, object value)
    {
        switch (infoType)
        {
            case InfoType.name:
                _file.Write("itprconf", "name", (string)value);
                break;
            case InfoType.script:
                _file.Write("itprconf", "script", (string)value);
                break;
            case InfoType.script_args:
                _file.Write("itprconf", "script_args", (string)value);
                break;
            case InfoType.itpr:
                _file.Write("itprconf", "itpr", (string)value);
                break;
            case InfoType.itpr_args:
                _file.Write("itprconf", "itpr_args", (string)value);
                break;
            case InfoType.use_bpsr:
                _file.Write("itprconf", "use_bpsr", (bool)value);
                break;
            default:
                break;
        }
        return;
    }

    public enum InfoType
    {
        name,
        script,
        script_args,
        itpr,
        itpr_args,
        use_bpsr,
    }
}
