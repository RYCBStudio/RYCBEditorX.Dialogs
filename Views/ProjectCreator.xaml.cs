using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RYCBEditorX.Dialogs.Views;
/// <summary>
/// ProjectCreator.xaml 的交互逻辑
/// </summary>
public partial class ProjectCreator : Window
{
    public ProjectCreator()
    {
        InitializeComponent();
        if (GlobalConfig.Skin == "dark")
        {
            MainGrid.Background = (Brush)Application.Current.Resources["DarkBackGround"];
        }
        else
        {
            MainGrid.Background = (Brush)Application.Current.Resources["LightBackGround"];
        }
    }

    private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ContentPresenter.Content = new ProjectCreatingStep1();
    }
}
