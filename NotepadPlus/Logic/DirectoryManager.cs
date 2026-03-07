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
        
        public static void CopyDirectory(string sourceDir, string destinationDir)
        {
         
            Directory.CreateDirectory(destinationDir);

       
            foreach (var file in Directory.GetFiles(sourceDir))
            {
                string destFile = Path.Combine(destinationDir, Path.GetFileName(file));
                File.Copy(file, destFile, true);
            }

            foreach (var dir in Directory.GetDirectories(sourceDir))
            {
                string destDir = Path.Combine(destinationDir, Path.GetFileName(dir));
                CopyDirectory(dir, destDir);
            }
        }
    }
}