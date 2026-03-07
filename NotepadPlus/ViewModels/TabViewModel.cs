namespace NotepadPlus.ViewModels
{
    public class TabViewModel : ViewModelBase
    {
        private string _fileName;
        private string _textContent;
        private bool _isSaved;
        private string _filePath;

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; OnPropertyChanged(); OnPropertyChanged("DisplayName"); }
        }

        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; OnPropertyChanged(); }
        }

        public string DisplayName => IsSaved ? FileName : FileName + "*";

        public string TextContent
        {
            get { return _textContent; }
            set
            {
               
                if (_textContent != value)
                {
                    _textContent = value;
                    IsSaved = false;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsSaved
        {
            get { return _isSaved; }
            set
            {
                _isSaved = value;
                OnPropertyChanged();
                OnPropertyChanged("DisplayName"); 
            }
        }

        public TabViewModel(string initialName)
        {
            FileName = initialName;
            _textContent = "";
            IsSaved = true; 
        }
    }
}