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
        public string Getdirectoryectory()
        {
            return currentPath;
        }

        public void Show(int x, int maxWidth, ConsoleColor color)
        {
            Console.BackgroundColor = color;
            Console.CursorTop = 0;
            Console.CursorLeft = x;

            if (currentPath.Length < maxWidth)
                Console.WriteLine(currentPath.PadRight(maxWidth, ' '));
            else
                Console.WriteLine("[...]"+ currentPath.Substring(currentPath.Length- maxWidth +5, maxWidth - 5));
        }

        public string ReturnStringPath()
        {
            return currentPath;
        }

        public void Clear()
        {
            currentPath = "";
        }

        public void Go(string directoryName)
        {
            var newPath = Path.Combine(currentPath, directoryName);
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
