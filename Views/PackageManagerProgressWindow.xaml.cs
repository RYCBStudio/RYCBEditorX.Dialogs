using System.Windows;
using RYCBEditorX.Utils;

namespace RYCBEditorX.Dialogs.Views
{

    /// <summary>
    /// PackageManagerProgressWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PackageManagerProgressWindow : Window
    {
        public PackageManagerProgressWindow Instance { get; private set; }
        public PackageManagerProgressWindow()
        {
            InitializeComponent();
            Instance = this;
            GlobalWindows.ActivatingWindows.Add(this);
        }
    }
}
