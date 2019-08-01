using Prism.Mvvm;

namespace Jaxx.Net.Cobaka.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Cobaka";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
