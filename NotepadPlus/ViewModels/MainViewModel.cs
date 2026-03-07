using NotepadPlus.Logic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace NotepadPlus.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private FileManager _fileManager;

        public ObservableCollection<TabViewModel> Tabs { get; set; }
        public ObservableCollection<FileItemViewModel> DrivesAndFolders { get; set; }

        private TabViewModel _selectedTab;
        public TabViewModel SelectedTab
        {
            get { return _selectedTab; }
            set { _selectedTab = value; OnPropertyChanged(); }
        }
        private bool _isFolderExplorerVisible = true;
        public bool IsFolderExplorerVisible
        {
            get { return _isFolderExplorerVisible; }
            set { _isFolderExplorerVisible = value; OnPropertyChanged(); }
        }

        private string _searchText;
        public string SearchText { get { return _searchText; } set { _searchText = value; OnPropertyChanged(); } }

        private string _replacementText;
        public string ReplacementText { get { return _replacementText; } set { _replacementText = value; OnPropertyChanged(); } }

        private bool _searchAllTabs;
        public bool SearchAllTabs { get { return _searchAllTabs; } set { _searchAllTabs = value; OnPropertyChanged(); } }

        public ICommand NewFileCommand { get; set; }
        public ICommand OpenFileCommand { get; set; }
        public ICommand SaveFileCommand { get; set; }
        public ICommand SaveAsCommand { get; set; }
        public ICommand CloseFileCommand { get; set; }
        public ICommand CloseAllFilesCommand { get; set; }
        public ICommand OpenFileFromTreeCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand ShowSearchCommand { get; set; }
        public ICommand FindCommand { get; set; }
        public ICommand ReplaceCommand { get; set; }
        public ICommand ReplaceAllCommand { get; set; }

        public ICommand ViewStandardCommand { get; set; }
        public ICommand ViewFolderExplorerCommand { get; set; }
        public ICommand ShowAboutCommand { get; set; }

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
            OpenFileCommand = new RelayCommand(o => OpenFile());
            SaveFileCommand = new RelayCommand(o => SaveFile(SelectedTab));
            SaveAsCommand = new RelayCommand(o => SaveFileAs(SelectedTab));
            CloseFileCommand = new RelayCommand(o => CloseTab((TabViewModel)o));
            CloseAllFilesCommand = new RelayCommand(o => CloseAllTabs());
            OpenFileFromTreeCommand = new RelayCommand(o => OpenFileFromTree((FileItemViewModel)o));
            ExitCommand = new RelayCommand(o => { CloseAllTabs(); if (Tabs.Count == 0) System.Windows.Application.Current.Shutdown(); });

 
            ViewStandardCommand = new RelayCommand(o => IsFolderExplorerVisible = false);
            ViewFolderExplorerCommand = new RelayCommand(o => IsFolderExplorerVisible = true);
            ShowAboutCommand = new RelayCommand(o => { new AboutWindow().ShowDialog(); });

            ShowSearchCommand = new RelayCommand(o => { SearchWindow w = new SearchWindow(); w.DataContext = this; w.Show(); });
            FindCommand = new RelayCommand(o => FindText());
            ReplaceCommand = new RelayCommand(o => ReplaceTextAction(false));
            ReplaceAllCommand = new RelayCommand(o => ReplaceTextAction(true));

            AddNewTab();
        }
        public void AddNewTab()
        {
            int nextFileNumber = Tabs.Count + 1;
            var newTab = new TabViewModel($"File {nextFileNumber}");
            Tabs.Add(newTab);
            SelectedTab = newTab;
        }

     
        public bool CloseTab(TabViewModel tabToClose)
        {
            if (tabToClose == null) return true;

            if (!tabToClose.IsSaved)
            {
                SelectedTab = tabToClose; 
                var result = System.Windows.MessageBox.Show(
                    $"Vrei să salvezi modificările din '{tabToClose.FileName}'?",
                    "Notepad++ Clone",
                    System.Windows.MessageBoxButton.YesNoCancel,
                    System.Windows.MessageBoxImage.Warning);

                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    SaveFile(tabToClose);
                    if (!tabToClose.IsSaved) return false; 
                }
                else if (result == System.Windows.MessageBoxResult.Cancel)
                {
                    return false; 
                }
            }

            Tabs.Remove(tabToClose);
            return true;
        }

        public void CloseAllTabs()
        {
            var tabsToList = Tabs.ToList();
            foreach (var tab in tabsToList)
            {
                bool closed = CloseTab(tab);
                if (!closed) return;
            }
        }

        public void OpenFile()
        {
            string filePath;
            string content = _fileManager.OpenFile(out filePath);

            if (content != null && filePath != null)
            {
                var newTab = new TabViewModel(System.IO.Path.GetFileName(filePath))
                {
                    TextContent = content,
                    FilePath = filePath,
                    IsSaved = true
                };
                Tabs.Add(newTab);
                SelectedTab = newTab;
            }
        }

        public void OpenFileFromTree(FileItemViewModel item)
        {
            if (item == null || item.IsDirectory) return;
            string content = _fileManager.ReadFile(item.FullPath);
            if (content != null)
            {
                var newTab = new TabViewModel(item.Name) { TextContent = content, FilePath = item.FullPath, IsSaved = true };
                Tabs.Add(newTab);
                SelectedTab = newTab;
            }
        }

        public void SaveFile(TabViewModel tab)
        {
            if (tab == null) return;
            if (string.IsNullOrEmpty(tab.FilePath)) SaveFileAs(tab);
            else { _fileManager.SaveFile(tab.FilePath, tab.TextContent); tab.IsSaved = true; }
        }

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

        private void FindText()
        {
            if (string.IsNullOrEmpty(SearchText)) return;
            int count = 0;
            if (SearchAllTabs) foreach (var tab in Tabs) count += CountOccurrences(tab.TextContent, SearchText);
            else if (SelectedTab != null) count = CountOccurrences(SelectedTab.TextContent, SearchText);
            System.Windows.MessageBox.Show($"Am găsit textul '{SearchText}' de {count} ori.", "Find Result");
        }

        private int CountOccurrences(string text, string search)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(search)) return 0;
            return (text.Length - text.Replace(search, "").Length) / search.Length;
        }

        private void ReplaceTextAction(bool replaceAll)
        {
            if (string.IsNullOrEmpty(SearchText)) return;
            if (SearchAllTabs)
            {
                foreach (var tab in Tabs) tab.TextContent = DoReplace(tab.TextContent, SearchText, ReplacementText, replaceAll);
            }
            else if (SelectedTab != null) SelectedTab.TextContent = DoReplace(SelectedTab.TextContent, SearchText, ReplacementText, replaceAll);
        }

        private string DoReplace(string original, string search, string replace, bool replaceAll)
        {
            if (string.IsNullOrEmpty(original)) return original;
            replace = replace ?? "";
            if (replaceAll) return original.Replace(search, replace);
            else
            {
                int index = original.IndexOf(search);
                if (index < 0) return original;
                return original.Substring(0, index) + replace + original.Substring(index + search.Length);
            }
        }
    }
}