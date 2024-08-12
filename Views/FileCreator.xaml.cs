using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Microsoft.TeamFoundation.Common;

namespace RYCBEditorX.Dialogs.Views;
/// <summary>
/// FileCreator.xaml 的交互逻辑
/// </summary>
public partial class FileCreator : Window
{

    public string FileName
    {
        get; private set;
    }

    public FileCreator()
    {
        InitializeComponent();
        if (GlobalConfig.Skin == "dark")
        {
            MainPanel.Background = (Brush)Application.Current.Resources["DarkBackGround"];
        }
        else
        {
            MainPanel.Background = (Brush)Application.Current.Resources["LightBackGround"];
        }
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

    private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (Filename.Text.IsNullOrEmpty()) {
                Filename.Style = (Style)Resources["InvalidStyle"];
                ElasticAnimation(tt, 3);
                Filename.Focus();
                return;
            }
            FileName = Filename.Text;
            DialogResult = true;
        }
    }

    private void Filename_TextChanged(object sender, TextChangedEventArgs e)
    {

        if (Filename.Style is not null)
        {
            Filename.Style = (Style)Application.Current.Resources["TextBoxBaseStyle"];
        }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        ElasticAnimation(tt, 3);
        Filename.Focus();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }

    private void OK_Click(object sender, RoutedEventArgs e)
    {
        if (Filename.Text.IsNullOrEmpty())
        {
            Filename.Style = (Style)Resources["InvalidStyle"];
            ElasticAnimation(tt, 3);
            Filename.Focus();
            return;
        }
        FileName = Filename.Text;
        DialogResult = true;
    }
}
