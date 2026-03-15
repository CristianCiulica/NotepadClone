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
        public string Icon => IsDirectory ? "🖿" : "📄";
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
            ContextNewFileCommand = new RelayCommand(o => CreateNewFile(), o => IsDirectory);
            ContextCopyPathCommand = new RelayCommand(o => System.Windows.Clipboard.SetText(FullPath));
            ContextCopyFolderCommand = new RelayCommand(o =>
            {
                CopiedFolderPath = FullPath;
                System.Windows.Input.CommandManager.InvalidateRequerySuggested();
            }, o => IsDirectory);

            ContextPasteFolderCommand = new RelayCommand(o => PasteCopiedFolder(), o => IsDirectory && !string.IsNullOrEmpty(CopiedFolderPath));
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
        private void CreateNewFile()
        {
            string newFilePath = Path.Combine(FullPath, "NewFile.txt");
            int counter = 1;

     
            while (File.Exists(newFilePath))
            {
                newFilePath = Path.Combine(FullPath, $"NewFile ({counter}).txt");
                counter++;
            }

            File.WriteAllText(newFilePath, ""); 

            if (IsExpanded) LoadChildren(); 
        }

        private void PasteCopiedFolder()
        {
            string destPath = Path.Combine(FullPath, Path.GetFileName(CopiedFolderPath));

            if (!Directory.Exists(destPath))
            {
                DirectoryManager.CopyDirectory(CopiedFolderPath, destPath);
                if (IsExpanded) LoadChildren(); 
            }
        }

        public static string CopiedFolderPath { get; set; }
        public System.Windows.Input.ICommand ContextNewFileCommand { get; set; }
        public System.Windows.Input.ICommand ContextCopyPathCommand { get; set; }
        public System.Windows.Input.ICommand ContextCopyFolderCommand { get; set; }
        public System.Windows.Input.ICommand ContextPasteFolderCommand { get; set; }
    }
}