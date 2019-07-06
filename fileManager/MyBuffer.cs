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

            DirectoryInfo[] dirs = dir.GetDirectories();

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath);
            }
        }

        public void FileCopy(string name,string destDirName)
        {
            FileInfo file = new FileInfo(name);
            file.CopyTo(destDirName, true);
        }
    }
}
