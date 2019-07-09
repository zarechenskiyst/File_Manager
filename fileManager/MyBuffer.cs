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
        public string BuffPath;

        private string name;
        public MyBuffer(string path, string name)
        {
            BuffPath = path;
            this.name = name;
        }

        public MyBuffer(string path)
        {
            BuffPath = path;
            Directory.CreateDirectory(path);
        }

        private void DirectoryCopy(string sourceDirName, string destDirName)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName + "\\" + name);

            string destination = destDirName + "\\" + name;

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

        private void FileCopy(string path, string destDirName)
        {
            try
            {
                FileInfo file = new FileInfo(path + "//" + name);
                string destination = destDirName + "//" + name;
                file.CopyTo(destination, true);
            }
            catch
            {
                ModuleWindow.ErrorMessage("File copied from this directory!");
            }
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

        public void CopyToBuffFile(string path, string name)
        {
            this.name = name;
            FileCopy(path, BuffPath);
        }

        public void CopyToBuffFolder(string path, string name)
        {
            this.name = name;
            DirectoryCopy(path, BuffPath);
        }

        public void Upload(string path)
        {
            if (name.Contains('.'))
                FileCopy(BuffPath, path);
            else
                DirectoryCopy(BuffPath, path);
        }

        public void ClearBuffer()
        {
            DeleteDirectory(BuffPath);
            Directory.CreateDirectory("temp");
        }
    }
}
