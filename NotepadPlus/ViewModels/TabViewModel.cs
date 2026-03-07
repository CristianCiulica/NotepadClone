using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotepadPlus.ViewModels
{

    public class TabViewModel : ViewModelBase
    {
        private string _fileName;
        private string _textContent;
        private bool _isSaved;

        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                OnPropertyChanged();
            }
        }
        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; OnPropertyChanged(); }
        }

        public string TextContent
        {
            get { return _textContent; }
            set
            {
               _textContent = value; OnPropertyChanged();
            }
        }
        public bool IsSaved
        {
            get { return _isSaved; }
            set { _isSaved = value; OnPropertyChanged(); }
        }
        public TabViewModel(string initialName)
        {
            FileName = initialName;
            TextContent = "";
            IsSaved = false;
        }
    }
}
