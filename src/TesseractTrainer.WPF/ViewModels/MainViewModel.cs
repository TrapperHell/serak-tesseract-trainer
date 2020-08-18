using System.Windows;
using System.Windows.Input;

namespace TesseractTrainer.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ICommand ExitApplicationCommand { get; set; }
        public ICommand DirectoryBrowserCommand { get; set; }

        string _projectLocation;

        public string ProjectLocation
        {
            get { return _projectLocation; }
            set
            {
                _projectLocation = value;
                RaisePropertyChanged("ProjectLocation");
            }
        }

        public MainViewModel()
        {
            ExitApplicationCommand = new DelegateCommand((o) =>
            {
                (o as Window).Close();
            });

            DirectoryBrowserCommand = new DelegateCommand((o) =>
            {
                var fbd = new System.Windows.Forms.FolderBrowserDialog();

                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    ProjectLocation = fbd.SelectedPath;
                else
                    ProjectLocation = null;
            });
        }
    }
}
