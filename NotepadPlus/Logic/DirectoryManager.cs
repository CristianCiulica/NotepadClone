using System.Collections.Generic;
using System.IO;

namespace NotepadPlus.Logic
{
    public class DirectoryManager
    {
        public static List<string> GetLogicalDrives()
        {
            return new List<string>(Directory.GetLogicalDrives());
        }
        public static List<string> GetDirectories(string path)
        {
            try { return new List<string>(Directory.GetDirectories(path)); }
            catch { return new List<string>(); }
        }
        public static List<string> GetFiles(string path)
        {
            try { return new List<string>(Directory.GetFiles(path)); }
            catch { return new List<string>(); }
        }
    }
}