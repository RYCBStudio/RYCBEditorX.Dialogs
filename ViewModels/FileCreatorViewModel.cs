using Prism.Mvvm;

namespace RYCBEditorX.Dialogs.Models
{
    public class FileCreatorViewModel : BindableBase
    {
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}