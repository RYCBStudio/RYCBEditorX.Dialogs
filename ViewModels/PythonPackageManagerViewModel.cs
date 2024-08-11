using Prism.Commands;
using Prism.Mvvm;
using RYCBEditorX.Dialogs.Views;

namespace RYCBEditorX.Dialogs.Models
{
    public class PythonPackageManagerViewModel : BindableBase
    {
        public DelegateCommand NextPageCmd
        {
            get; set;
        }
        public DelegateCommand PreviousPageCmd
        {
            get; set;
        }
        public DelegateCommand StartPageCmd
        {
            get; set;
        }
        public DelegateCommand EndPageCmd
        {
            get; set;
        }
        public DelegateCommand DownloadCmd
        {
            get; set;
        }

        public PythonPackageManagerViewModel()
        {
            NextPageCmd = new(NxtPg);
            PreviousPageCmd = new(PrePg);
            StartPageCmd = new(St);
            EndPageCmd = new(Ed);
            DownloadCmd = new(Download);
        }

        internal void Download()
        {
            PythonPackageManager.Instance.Install();
        }

        private void NxtPg()
        {
            PythonPackageManager.Instance.currentPage++;
            if (PythonPackageManager.Instance.currentPage >= PythonPackageManager.Instance.totalpages)
            {
                PythonPackageManager.Instance.currentPage = PythonPackageManager.Instance.totalpages;
            }
            PythonPackageManager.Instance.Search(PythonPackageManager.Instance.SearchBox, new());
        }

        private void PrePg()
        {
            PythonPackageManager.Instance.currentPage--;
            PythonPackageManager.Instance.Search(PythonPackageManager.Instance.SearchBox, new());
        }

        private void St()
        {
            PythonPackageManager.Instance.currentPage = 1;
            PythonPackageManager.Instance.Search(PythonPackageManager.Instance.SearchBox, new());
        }

        private void Ed()
        {
            PythonPackageManager.Instance.currentPage = PythonPackageManager.Instance.totalpages;
            PythonPackageManager.Instance.Search(PythonPackageManager.Instance.SearchBox, new());
        }
    }
}