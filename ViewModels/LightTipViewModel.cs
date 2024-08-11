using System.Windows.Media;
using Prism.Mvvm;

namespace RYCBEditorX.Dialogs.ViewModels
{
    public class LightTipViewModel : BindableBase
    {
        private string _content;
        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        private string _icon;
        public string Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        private Brush _icon_brush;
        public Brush IconBrush
        {
            get => _icon_brush;
            set => SetProperty(ref _icon_brush, value);
        }
    }
}