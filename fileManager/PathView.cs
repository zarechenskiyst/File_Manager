using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileManager
{
    class PathView
    {
        private string currentPath;

        public PathView(string basePath = @"C:\")
        {

            if (!Path.IsPathRooted(basePath))
                throw new ArgumentException("Invalid Path", nameof(basePath));

            currentPath = basePath;
        }
        public string GetDirectory()
        {
            return currentPath;
        }

        public void Show(int x)
        {
            Console.CursorTop = 0;
            Console.CursorLeft = x;
            Console.Write(currentPath.PadRight(Console.WindowWidth));
        }

        public void Clear()
        {
            currentPath = "";
        }

        public void Go(string dirName)
        {
            var newPath = Path.Combine(currentPath, dirName);
            newPath = Path.GetFullPath(newPath);
            if (File.Exists(newPath) || Directory.Exists(newPath))
                currentPath = newPath;
        }

        public void GoUp()
        {
            if (currentPath != "")
            {
                var newPath = Path.Combine(currentPath, "..");
                newPath = Path.GetFullPath(newPath);
                if (File.Exists(newPath) || Directory.Exists(newPath))
                    currentPath = newPath;
            }
        }
    }
}
