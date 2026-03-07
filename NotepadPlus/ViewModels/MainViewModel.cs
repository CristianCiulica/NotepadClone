using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NotepadPlus.Logic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotepadPlus.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private FileManager _fileManager;
        public ObservableCollection<TabViewModel> Tabs { get; set; }
        private TabViewModel _selectedTab;
        public TabViewModel SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                _selectedTab = value;
                OnPropertyChanged();
            }
        }
        public ICommand OpenFileFromTreeCommand { get; set; }
        public MainViewModel()
        {
            _fileManager = new FileManager();
            Tabs = new ObservableCollection<TabViewModel>();

            DrivesAndFolders = new ObservableCollection<FileItemViewModel>();
            foreach (var drive in DirectoryManager.GetLogicalDrives())
            {
                DrivesAndFolders.Add(new FileItemViewModel(drive, true));
            }

            NewFileCommand = new RelayCommand(o => AddNewTab());
            CloseFileCommand = new RelayCommand(o => CloseTab((TabViewModel)o));
            CloseAllFilesCommand = new RelayCommand(o => CloseAllTabs());
            OpenFileFromTreeCommand = new RelayCommand(o => OpenFileFromTree((FileItemViewModel)o));
            OpenFileCommand = new RelayCommand(o => OpenFile());
            SaveFileCommand = new RelayCommand(o => SaveFile(SelectedTab));
            SaveAsCommand = new RelayCommand(o => SaveFileAs(SelectedTab));

            AddNewTab();
        }

        public void CloseTab(TabViewModel tabToClose)
        {
            if (tabToClose != null)
            {
                Tabs.Remove(tabToClose);
            }
        }

        public void CloseAllTabs()
        {
            Tabs.Clear();
        }

        public void OpenFileFromTree(FileItemViewModel item)
        {
            if (item == null) return;
            if (item.IsDirectory) return;
            string content = _fileManager.ReadFile(item.FullPath);

            if (content != null)
            {
                var newTab = new TabViewModel(item.Name)
                {
                    TextContent = content,
                    FilePath = item.FullPath,
                    IsSaved = true
                };

                Tabs.Add(newTab);
                SelectedTab = newTab;
            }
        }
        public void AddNewTab()
        {
            int nextFileNumber = Tabs.Count + 1;
            var newTab = new TabViewModel($"File {nextFileNumber}");

            Tabs.Add(newTab);
            SelectedTab = newTab;
        }

        public void OpenFile()
        {
            string filePath;
            string content = _fileManager.OpenFile(out filePath);

            if (content != null && filePath != null)
            {
              
                string fileName = System.IO.Path.GetFileName(filePath);

                var newTab = new TabViewModel(fileName)
                {
                    TextContent = content,
                    FilePath = filePath,
                    IsSaved = true
                };

                Tabs.Add(newTab);
                SelectedTab = newTab; 
            }
        }

        public void SaveFile(TabViewModel tab)
        {
            if (tab == null) return;

            if (string.IsNullOrEmpty(tab.FilePath))
            {
              
                SaveFileAs(tab);
            }
            else
            {
              
                _fileManager.SaveFile(tab.FilePath, tab.TextContent);
                tab.IsSaved = true;
            }
        }
        public ObservableCollection<FileItemViewModel> DrivesAndFolders { get; set; }
        public void SaveFileAs(TabViewModel tab)
        {
            if (tab == null) return;

            string newPath = _fileManager.SaveFileAs(tab.TextContent);
            if (newPath != null)
            {
                tab.FilePath = newPath;
                tab.FileName = System.IO.Path.GetFileName(newPath); 
                tab.IsSaved = true;
            }
        }
        public ICommand NewFileCommand { get; set; }
        public ICommand CloseFileCommand { get; set; } 
        public ICommand CloseAllFilesCommand { get; set; }

        public ICommand OpenFileCommand { get; set; }
        public ICommand SaveFileCommand { get; set; }
        public ICommand SaveAsCommand { get; set; }

    }
}
