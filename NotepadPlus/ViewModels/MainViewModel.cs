using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotepadPlus.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
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
        public MainViewModel()
        {
            Tabs = new ObservableCollection<TabViewModel>();
            AddNewTab();
        }

        public void AddNewTab()
        {
            int nextFileNumber = Tabs.Count + 1;
            var newTab = new TabViewModel($"File {nextFileNumber}");

            Tabs.Add(newTab);
            SelectedTab = newTab;
        }
    }
}
