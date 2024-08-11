using System.Windows.Shapes;
using System;
using Prism.Commands;
using Prism.Mvvm;
using RYCBEditorX.Dialogs.Views;

namespace RYCBEditorX.Dialogs.Models
{
    public class GotoLineViewModel : BindableBase
    {

        public DelegateCommand Close
        {
            get; set;
        }
        public DelegateCommand Enter
        {
            get; set;
        }
        public GotoLineViewModel()
        {
            Close = new(() =>
            {
                GotoLineWindow.Instance.isCompleted = false;
                GotoLineWindow.Instance.Close();
            });
            Enter = new(() =>
            {
                if (GotoLineWindow.Instance.TextBox.Text.Length > 0)
                {
                    GotoLineWindow.Line = Convert.ToInt32(GotoLineWindow.Instance.TextBox.Text);
                    GotoLineWindow.Instance.DialogResult = true;
                }
            });
        }
    }
}