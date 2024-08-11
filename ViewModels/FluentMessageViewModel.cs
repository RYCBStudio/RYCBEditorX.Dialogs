using Prism.Commands;
using Prism.Mvvm;
using RYCBEditorX.Dialogs.Views;

namespace RYCBEditorX.Dialogs.ViewModels
{
    public class FluentMessageBoxViewModel : BindableBase
    {
        private string _title;
        private string _message;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public DelegateCommand OkCommand
        {
            get; private set;
        }
        public DelegateCommand CancelCommand
        {
            get; private set;
        }

        public FluentMessageBoxViewModel()
        {
            OkCommand = new DelegateCommand(OnOk);
            CancelCommand = new DelegateCommand(OnCancel);
        }

        private void OnOk()
        {
            // 处理确定按钮的点击事件
            FluentMessageBox.Instance.DialogResult = true;
        }

        private void OnCancel()
        {
            // 处理取消按钮的点击事件
            FluentMessageBox.Instance.DialogResult = false;
        }
    }
}