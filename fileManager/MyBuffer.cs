using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileManager
{
    class MyBuffer
    {
        public string buffPath;

        public MyBuffer(string path)
        {
            buffPath = path;
        }

        public void DirectoryCopy(string sourceDirName, string destDirName)
        {

            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            string destination = destDirName + "\\" + GetName(sourceDirName);

            Directory.CreateDirectory(destination);

            DirectoryInfo[] dirs = dir.GetDirectories();

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destination, file.Name);
                file.CopyTo(temppath, true);
            }
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destination, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath);
            }
        }

        public void FileCopy(string name, string destDirName)
        {
            FileInfo file = new FileInfo(name);
            string destination = destDirName + "\\" + GetName(name);
            file.CopyTo(destination, true);
        }

        private string GetName(string path)
        {
            string[] getItemName = path.Split('\\');
            return  getItemName[getItemName.Length - 1];
        }

        public void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }
    }
}
