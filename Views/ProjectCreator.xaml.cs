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
            MainGrid.Background = (Brush)Application.Current.Resources["DarkBackgroud"];
        }
        else
        {
            MainGrid.Background = (Brush)Application.Current.Resources["LightBackgroud"];
        }
    }

    private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        contentPresenter.Content = new ProjectCreatingStep1();
    }
}
