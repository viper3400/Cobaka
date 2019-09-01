using Prism.Mvvm;
using System.Diagnostics;

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
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            Title = $"{Title} - {version}";
        }
    }
}
