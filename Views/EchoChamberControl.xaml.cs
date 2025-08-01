using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace RYCBEditorX.Dialogs.Views;
/// <summary>
/// EchoChamberControl.xaml 的交互逻辑
/// </summary>
public partial class EchoChamberControl : UserControl
{
    private bool _isAnimating;

    // 颜色定义
    private readonly Color _lightBgColor = Color.FromArgb(255, 245, 245, 245);
    private readonly Color _darkBgColor = Color.FromArgb(255, 30, 30, 30);
    private readonly Color _lightTextColor = Colors.Black;
    private readonly Color _darkTextColor = Colors.White;
    private readonly Color _lightShadowColor = Colors.Purple;
    private readonly Color _darkShadowColor = Color.FromArgb(255, 150, 0, 150); // 深紫色

    public EchoChamberControl()
    {
        InitializeComponent();

        // 初始化颜色
        UpdateSkinColors();

        Loaded += (s, e) => InitializeEchoChamber();
        MouseDown += async (s, e) => await OnEchoChamberClicked();
    }

    private void UpdateSkinColors()
    {
        bool isDarkMode = GlobalConfig.Skin == "dark";

        // 设置控件颜色
        ContentTextBlock.Foreground = new SolidColorBrush(isDarkMode ? _darkTextColor : _lightTextColor);
        MainBorder.Background = new SolidColorBrush(isDarkMode ? _darkBgColor : _lightBgColor);
        ShadowEffect.Color = isDarkMode ? _darkShadowColor : _lightShadowColor;
    }

    private void InitializeEchoChamber()
    {
        try
        {
            LoadAndDisplayMessage();
        }
        catch (Exception ex)
        {
            ContentTextBlock.Text = $"Echo failed: {ex.Message}";
        }
    }

    private async Task OnEchoChamberClicked()
    {
        if (_isAnimating) return;

        try
        {
            _isAnimating = true;
            await PlayClickAnimation();
            LoadAndDisplayMessage();
        }
        finally
        {
            _isAnimating = false;
        }
    }

    private void LoadAndDisplayMessage()
    {
        var message = GetMessageFromDatabase();
        ContentTextBlock.Text = message;
        ContentViewbox.Opacity = 0;
        ContentViewbox.RenderTransform = null;

        PlayEntranceAnimation();
    }

    private string GetMessageFromDatabase()
    {
        if (!MySQL.MySQLModule.ConnectionUtils.ConnectionOpened) return ""; 
        return MySQL.MySQLModule.EchoChambers[Random.Shared.Next(0, MySQL.MySQLModule.EchoChambers.Count - 1)]["content"]?.ToString() ?? "Echo... echo... echo...";
    }

    private void PlayEntranceAnimation()
    {
        bool isDarkMode = GlobalConfig.Skin == "dark";

        // 背景动画 - 使用当前皮肤的主色调
        var bgColor = isDarkMode ? _darkBgColor : _lightBgColor;
        var highlightColor = isDarkMode ?
            Color.FromArgb(255, 80, 0, 80) :
            Color.FromArgb(255, 200, 150, 255);

        var bgColorAnimation = new ColorAnimation
        {
            From = bgColor,
            To = highlightColor,
            Duration = TimeSpan.FromSeconds(0.3),
            AutoReverse = true
        };

        var bgBrush = new SolidColorBrush(bgColor);
        MainBorder.Background = bgBrush;
        bgBrush.BeginAnimation(SolidColorBrush.ColorProperty, bgColorAnimation);

        // 文字弹跳进入
        var bounceAnimation = new DoubleAnimation
        {
            From = 0,
            To = 1,
            Duration = TimeSpan.FromSeconds(0.7),
            EasingFunction = new ElasticEase { Oscillations = 3, Springiness = 10 }
        };

        ContentViewbox.BeginAnimation(OpacityProperty, bounceAnimation);

        var rotateTransform = new RotateTransform();
        ContentViewbox.RenderTransform = rotateTransform;
        ContentViewbox.RenderTransformOrigin = new Point(0.5, 0.5);

        var rotateAnimation = new DoubleAnimation
        {
            From = -15,
            To = 15,
            Duration = TimeSpan.FromSeconds(1.5),
            AutoReverse = true,
            RepeatBehavior = RepeatBehavior.Forever
        };

        rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);

        // 文字颜色动画 - 使用当前皮肤的对比色
        var textColor = isDarkMode ? _darkTextColor : _lightTextColor;
        var accentColor = isDarkMode ?
            Color.FromArgb(255, 255, 105, 180) :
            Color.FromArgb(255, 200, 0, 150);

        var colorAnimation = new ColorAnimation
        {
            From = textColor,
            To = accentColor,
            Duration = TimeSpan.FromSeconds(2),
            AutoReverse = true,
            RepeatBehavior = RepeatBehavior.Forever
        };

        ContentTextBlock.Foreground = new SolidColorBrush(textColor);
        ContentTextBlock.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
    }

    private async Task PlayClickAnimation()
    {
        bool isDarkMode = GlobalConfig.Skin == "dark";
        var accentColor = isDarkMode ? Colors.Cyan : Colors.DodgerBlue;

        // 阴影颜色变化
        var shadowColorAnimation = new ColorAnimation
        {
            To = accentColor,
            Duration = TimeSpan.FromSeconds(0.3),
            AutoReverse = true
        };

        ShadowEffect.BeginAnimation(DropShadowEffect.ColorProperty, shadowColorAnimation);

        // 其余点击动画保持不变...
        var scaleAnimation = new DoubleAnimation
        {
            From = 1,
            To = 0.7,
            Duration = TimeSpan.FromSeconds(0.2),
            AutoReverse = true
        };

        var scaleTransform = new ScaleTransform(1, 1);
        RenderTransform = scaleTransform;
        RenderTransformOrigin = new Point(0.5, 0.5);
        scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
        scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);

        var spinAnimation = new DoubleAnimation
        {
            From = 0,
            To = 360,
            Duration = TimeSpan.FromSeconds(0.5)
        };

        var rotateTransform = new RotateTransform();
        ContentViewbox.RenderTransform = rotateTransform;
        rotateTransform.BeginAnimation(RotateTransform.AngleProperty, spinAnimation);

        await Task.Delay(500);
    }

    public async Task CloseEchoChamber()
    {
        var exitAnimation = new DoubleAnimation
        {
            From = 1,
            To = 0,
            Duration = TimeSpan.FromSeconds(0.5),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };

        BeginAnimation(OpacityProperty, exitAnimation);
        await Task.Delay(500);

        if (Parent is Panel parent)
        {
            parent.Children.Remove(this);
        }
    }
}
