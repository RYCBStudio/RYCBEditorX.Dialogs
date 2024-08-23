using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HandyControl.Tools.Extension;
using RYCBEditorX.Dialogs.Models;
using RYCBEditorX.Utils;
using Sunny.UI;

namespace RYCBEditorX.Dialogs.Views;
/// <summary>
/// PythonPackageManager.xaml 的交互逻辑
/// </summary>
public partial class PythonPackageManager : Window
{
    public int currentPage = 1, totalpages;
    private readonly string baseAdd = "https://pypi.org/search/?q={0}&page={1}";
    public List<GeneralPackageInfo> resLst = [];
    private readonly List<string> LocalPackageNames = [];
    private List<PythonPackageInfo> LocalPythonPackages = [];
    public string query = "";
    public static PythonPackageManager Instance
    {
        get; private set;
    }

    public PythonPackageManager()
    {
        InitializeComponent();
        Instance = this;
        DataContext = new PythonPackageManagerViewModel();
        GlobalWindows.ActivatingWindows.Add(this);
    }
    private void GetAllPackages()
    {
        LocalPackageNames.Clear();
        Process pip = new()
        {
            StartInfo = new ProcessStartInfo()
            {
                Arguments = $"list --format=freeze",
                FileName = "pip",
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            }
        };
        pip.Start();
        var output = pip.StandardOutput.ReadToEnd();
        foreach (var item in output.Split(["\r", "\n", "\r\n"], StringSplitOptions.RemoveEmptyEntries))
        {
            LocalPackageNames.Add($"{item.Split('=')[0]}");
        }
        pip.Dispose();
    }

    private string GetPackageStatus(string name)
    {
        Process pip = new();
        var filename = GlobalConfig.CommonTempFilePath;
        pip.StartInfo = new ProcessStartInfo()
        {
            Arguments = $"show {name} > {filename}",
            FileName = "pip",
            CreateNoWindow = true,
            UseShellExecute = true,
            WindowStyle = ProcessWindowStyle.Hidden
        };
        pip.Start();
        if (System.IO.File.ReadAllText(filename).Contains("not found"))
        {
            return "nf";
        }
        else
        {
            return "i";
        }
    }

    public List<PythonPackageInfo> GetInstalledPackages()
    {
        var packages = new List<PythonPackageInfo>();

        var startInfo = new ProcessStartInfo
        {
            FileName = "python", // 你的 Python 可执行文件的路径
            Arguments = "Tools\\get_packages.py", // 你的脚本路径
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = Process.Start(startInfo))
        {
            using var reader = process.StandardOutput;
            var result = reader.ReadToEnd();
            packages = JsonSerializer.Deserialize<List<PythonPackageInfo>>(result);
        }

        return packages;
    }

    /// <summary>
    /// 控件抖动
    /// </summary>
    /// <param name="translate"></param>
    /// <param name="power">抖动第一下偏移量</param>
    /// <param name="range">减弱幅度（小于等于power，大于0）</param>
    /// <param name="speed">持续系数(大于0)，越大时间越长，</param>
    public void ElasticAnimation(TranslateTransform translate, int power, int range = 1, double speed = 1)
    {
        var animation1 = new DoubleAnimationUsingKeyFrames();
        for (int i = power, j = 1; i >= 0; i -= range)
        {
            animation1.KeyFrames.Add(new LinearDoubleKeyFrame(-i, TimeSpan.FromMilliseconds(j++ * 100 * speed)));
            animation1.KeyFrames.Add(new LinearDoubleKeyFrame(i, TimeSpan.FromMilliseconds(j++ * 100 * speed)));
        }
        translate.BeginAnimation(TranslateTransform.YProperty, animation1);
        var animation2 = new DoubleAnimationUsingKeyFrames();
        for (int i = power, j = 1; i >= 0; i -= range)
        {
            animation2.KeyFrames.Add(new LinearDoubleKeyFrame(-i, TimeSpan.FromMilliseconds(j++ * 100 * speed)));
            animation2.KeyFrames.Add(new LinearDoubleKeyFrame(i, TimeSpan.FromMilliseconds(j++ * 100 * speed)));
        }
        translate.BeginAnimation(TranslateTransform.XProperty, animation2);
    }

    internal void InitPkg()
    {
        var si = ListBoxLocalPackages.SelectedIndex;
        GetAllPackages();
        ListBoxLocalPackages.ItemsSource = null;
        if (si == -1)
        {
            si = 0;
        }
        LocalPythonPackages = GetInstalledPackages();
        ListBoxLocalPackages.ItemsSource = LocalPythonPackages;
        ListBoxLocalPackages.SelectedIndex = si;
    }
    private void Uninstall(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show("Comfirm to uninstall?", "", MessageBoxButton.YesNo) == MessageBoxResult.OK)
        {
            //try
            //{
            //    pw.Update(0, 100, true, _I18nFile.Localize("text.ppmm.pkg.uninst"));
            //    pw.uiProcessBar1.Hide();
            //    var handler = new Action(() => pw.ShowDialog());
            //    Task.Run(handler.Invoke);
            //}
            //catch
            //{
            //    var pw = new FrmPackageManagerProgressWindow();
            //    pw.Update(0, 100, true, _I18nFile.Localize("text.ppmm.pkg.uninst"));
            //    pw.uiProcessBar1.Hide();
            //    var handler = new Action(() => pw.ShowDialog());
            //    Task.Run(handler.Invoke);
            //}
            //pw.progressBar1.Show();
            var pip = new Process
            {
                StartInfo = new()
                {
                    Arguments = $"uninstall {TBlkNameLocal.Text}",
                    FileName = "pip",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                },
            };
            pip.Start();
            pip.StandardInput.WriteLine("y");
            if (pip.StandardOutput.ReadToEnd().Contains("Successfully uninstalled"))
            {
                MessageBox.Show(string.Format("Successfully uninstalled {0}", ListBoxLocalPackages.SelectedItem));
            }
            //pw.Hide();
            InitPkg();
        }
    }

    private void SearchOnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            Search(sender, e);
        }
    }

    public async void Search(object sender, RoutedEventArgs e)
    {
        if (((FrameworkElement)sender).Tag is null)
        {
            if (((TextBox)sender).Text.IsNullOrEmpty())
            {
                ElasticAnimation(Tt, 2);
                return;
            }
            Mask.Show();
            Loading.Show();
            //var dialog = this.Dispatcher.InvokeAsync(pw.Show);
            ResLst.ItemsSource = null;
            var _res = PypiHelper.ParseLinkWithContent(await PypiHelper.GetHtml(string.Format(baseAdd, SearchBox.Text, currentPage.ToString())));
            var res = PypiHelper.ConvertToClass(_res.Item1);
            resLst = res;
            ResLst.ItemsSource = resLst;
            CurrentPage.Text = $"{currentPage}/{_res.Item2}";
            totalpages = Convert.ToInt32(_res.Item2);
            Mask.Hide();
            Loading.Hide();
        }
        else
        {
            if (((FrameworkElement)sender).Tag.ToString() == "Local")
            {

                foreach (var item in (List<PythonPackageInfo>)ListBoxLocalPackages.ItemsSource)
                {
                    if (item.Name.Contains(SearchBox.Text)
                        || item.Version.Contains(SearchBox.Text)
                        || item.Description.Contains(SearchBox.Text)
                        || item.Summary.Contains(SearchBox.Text)
                        || item.Author.Contains(SearchBox.Text)
                        || item.RequiredBy.Contains(SearchBox.Text)
                        || item.Requires.Contains(SearchBox.Text))
                    {
                        ListBoxLocalPackages.SelectedItem = item;
                        ListBoxLocalPackages.Focus();
                        var color = System.Drawing.ColorTranslator.FromHtml("#FFABADB3");
                        SearchBox.BorderBrush = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
                        return;
                    }
                }
                SearchBox.BorderBrush = (SolidColorBrush)Resources["NotFound"];
                ElasticAnimation(TtLocal, 2);
            }
        }
    }

    private static readonly string[] separator = ["\r", "\n", "\r\n"];

    private void GetPkgInfo(object sender, SelectionChangedEventArgs e)
    {

        if (((ListBox)sender).SelectedIndex < 0) { return; }
        if (((FrameworkElement)sender).Tag is not null)
        {
            if (((FrameworkElement)sender).Tag.ToString() == "Local")
            {
                TBlkNameLocal.Text = LocalPythonPackages[ListBoxLocalPackages.SelectedIndex].Name;
                TBlkVerLocal.Text = LocalPythonPackages[ListBoxLocalPackages.SelectedIndex].Version;
                TBlkDescLocal.Text = LocalPythonPackages[ListBoxLocalPackages.SelectedIndex].Summary;
                TBlkAuthorLocal.Text = LocalPythonPackages[ListBoxLocalPackages.SelectedIndex].Author;
                TBlkRqdLocal.Text = LocalPythonPackages[ListBoxLocalPackages.SelectedIndex].RequiredBy;
                TBlkRqsLocal.Text = LocalPythonPackages[ListBoxLocalPackages.SelectedIndex].Requires;
            }
        }
        else
        {
            TBlkName.Text = resLst[ResLst.SelectedIndex].Name;
            TBlkVer.Text = resLst[ResLst.SelectedIndex].Version;
            TBlkDesc.Text = resLst[ResLst.SelectedIndex].Description;
        }
    }

    private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (MainTabCtrl.SelectedIndex == 1)
        {
            //InitPkg();
            //ListBoxLocalPackages.ItemsSource = LocalPythonPackages;
        }
    }

    private void Local_Initialized(object sender, EventArgs e)
    {
        InitPkg();
    }

    public void Install()
    {
        var pip = new Process()
        {
            StartInfo = new()
            {
                Arguments = "install " + TBlkName.Text,
                FileName = "pip",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
            }
        };
        pip.Start();
        var output = pip.StandardOutput.ReadToEnd();
        var error = pip.StandardError.ReadToEnd();
        if (error.IsNullOrEmpty() & (output.Contains("Successfully installed") || output.Contains("Requirement already satisfied")))
        {
            MessageBox.Show(string.Format("Successfully installed {0}", TBlkName.Text), "tip", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
            MessageBox.Show($"Fail to install: \n{error}", "tip", MessageBoxButton.OK, MessageBoxImage.Stop);
        }
        pip.Close();
    }
}
