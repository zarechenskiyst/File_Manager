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

        private bool isCut;
        private string name;
        public MyBuffer(string path, string name, bool cut = false)
        {
            BuffPath = path;
            this.name = name;
            isCut = cut;
        }

        public MyBuffer()
        {
            BuffPath = null;
            name = null;
        }

        public void DirectoryCopy(string sourceDirName, string destDirName)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            string destination = destDirName;

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
            if (isCut)
                DeleteDirectory(sourceDirName);
        }

        public void FileCopy(string path, string destDirName)
        {
            try
            {
                FileInfo file = new FileInfo(path);
                string destination = destDirName;
                file.CopyTo(destination, true);
                if (isCut)
                    File.Delete(path);
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

        public void Paste(string destination)
        {
            try
            {
                if (name.Contains("."))
                    FileCopy(Path.Combine(BuffPath, name), Path.Combine(destination, name));
                else
                    DirectoryCopy(Path.Combine(BuffPath, name), Path.Combine(destination, name));
            }
            catch
            {
                throw new InvalidOperationException("Buffer is missing");
            }
        }
    }
}
