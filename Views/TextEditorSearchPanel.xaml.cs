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
using ICSharpCode.AvalonEdit;
using RYCBEditorX.Dialogs.ViewModels;

namespace RYCBEditorX.Dialogs.Views;
/// <summary>
/// TextEditorSearchPanel.xaml 的交互逻辑
/// </summary>
public partial class TextEditorSearchPanel : HandyControl.Controls.Window
{
    public TextEditorSearchPanel()
    {
        InitializeComponent();
    }

    private void ReplaceBtn_Click(object sender, RoutedEventArgs e)
    {
        PerformReplace();
    }

    private void ReplaceAllBtn_Click(object sender, RoutedEventArgs e)
    {
        PerformReplaceAll();
    }

    private void PerformReplace()
    {
        if (DataContext is SearchPanelViewModel viewModel)
        {
            // Get the search and replace text
            var searchText = SearchBar.Text;
            var replaceText = ReplaceBar.Text;

            // Access the TextEditor instance
            var editor = viewModel.Editor;

            // Perform replace logic
            if (editor != null)
            {
                var options = viewModel.GetSearchOptions();
                editor.Text = editor.Text.Replace(searchText, replaceText); // Simple replace logic
            }
        }
    }

    private void PerformReplaceAll()
    {
        if (DataContext is SearchPanelViewModel viewModel)
        {
            // Get the search and replace text
            var searchText = SearchBar.Text;
            var replaceText = ReplaceBar.Text;

            // Access the TextEditor instance
            var editor = viewModel.Editor;

            // Perform replace all logic
            if (editor != null)
            {
                var options = viewModel.GetSearchOptions();
                editor.Text = editor.Text.Replace(searchText, replaceText); // Simple replace all logic
            }
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}
