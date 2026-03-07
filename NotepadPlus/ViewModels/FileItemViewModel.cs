using NotepadPlus.Logic;
using System.Collections.ObjectModel;
using System.IO;

namespace NotepadPlus.ViewModels
{
    public class FileItemViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public bool IsDirectory { get; set; }
        public string Icon => IsDirectory ? "📁" : "📄";
        public ObservableCollection<FileItemViewModel> Children { get; set; }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (_isExpanded != value)
                {
                    _isExpanded = value;
                    OnPropertyChanged();

                    if (_isExpanded)
                    {
                        LoadChildren();
                    }
                }
            }
        }

        public FileItemViewModel(string fullPath, bool isDirectory)
        {
            FullPath = fullPath;
            Name = isDirectory ? new DirectoryInfo(fullPath).Name : Path.GetFileName(fullPath);

            if (string.IsNullOrEmpty(Name)) Name = fullPath;

            IsDirectory = isDirectory;
            Children = new ObservableCollection<FileItemViewModel>();

            
            if (IsDirectory)
            {
                Children.Add(new FileItemViewModel("DUMMY", false));
            }
        }

        private void LoadChildren()
        {
            Children.Clear();

            foreach (var dirPath in DirectoryManager.GetDirectories(FullPath))
            {
                Children.Add(new FileItemViewModel(dirPath, true));
            }
            foreach (var filePath in DirectoryManager.GetFiles(FullPath))
            {
                Children.Add(new FileItemViewModel(filePath, false));
            }
        }
    }
}