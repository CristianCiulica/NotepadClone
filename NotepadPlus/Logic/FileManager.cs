using Microsoft.Win32;
using System.IO;

namespace NotepadPlus.Logic
{
    public class FileManager
    {
        public string OpenFile(out string filePath)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                return File.ReadAllText(filePath); 
            }

            filePath = null;
            return null;
        }

        public string ReadFile(string path)
        {
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            return null;
        }
        public string SaveFileAs(string content)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, content);
                return saveFileDialog.FileName; 
            }

            return null;
        }
        public void SaveFile(string path, string content)
        {
            File.WriteAllText(path, content);
        }
    }
}