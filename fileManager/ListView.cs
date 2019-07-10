using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileManager
{
    class ListView
    {
        public PathView directory;
        private int selectedPrevIndex;
        private int selectedIndex;
        private bool wasPainted;

        private int x, y, height;

        private int scroll;

        private MyBuffer buffer;

        public List<int> ColumnsWidth { get; set; }
        public List<ListViewItem> Items { get; set; }

        public ListView(int x, int y, int height, MyBuffer buffer)
        {
            this.x = x;
            this.y = y;
            this.height = height;
            directory = new PathView();

            this.buffer = buffer;

            ColumnsWidth = new List<int> { Console.WindowWidth / 2 - 30, 13, 13 };
            Items = GetItems(directory.Getdirectoryectory());

            Selected += View_Selected;
            Up += UpDirectory;

        }

        public void Clean()
        {
            selectedIndex = selectedPrevIndex = 0;
            wasPainted = false;
            for (int i = 0; i < Math.Min(height, Items.Count); i++)
            {
                Console.CursorLeft = x;
                Console.CursorTop = i + y;
                Items[i].Clean(ColumnsWidth, i, x, y);
            }
        }

        public ListViewItem SelectedItem => Items[selectedIndex];

        public bool Focused { get; set; }

        public void Render(ConsoleColor color, bool painAll = false)
        {
            if (painAll)
                wasPainted = false;

            if (!wasPainted)
                directory.Show(x, ColumnsWidth.Sum(), color);
            Console.BackgroundColor = color;
            for (int i = 0; i < Math.Min(height, Items.Count); i++)
            {
                int elementIndex = i + scroll;

                if (wasPainted)
                {
                    if (elementIndex != selectedIndex && elementIndex != selectedPrevIndex)
                        continue;
                }

                var item = Items[elementIndex];
                var saveForeground = Console.ForegroundColor;
                var saveBackground = Console.BackgroundColor;
                if (elementIndex == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }

                Console.CursorLeft = x;
                Console.CursorTop = i + y;
                item.Render(ColumnsWidth, i, x, y);

                Console.ForegroundColor = saveForeground;
                Console.BackgroundColor = saveBackground;
            }
            wasPainted = true;
        }
        public void Update(ConsoleKeyInfo key)
        {
            selectedPrevIndex = selectedIndex;
            if (key.Key == ConsoleKey.DownArrow && selectedIndex + 1 < Items.Count)
                selectedIndex++;
            else if (key.Key == ConsoleKey.UpArrow && selectedIndex - 1 >= 0)
                selectedIndex--;

            if (selectedIndex >= height + scroll)
            {
                scroll++;
                wasPainted = false;
            }
            else if (selectedIndex < scroll)
            {
                scroll--;
                wasPainted = false;
            }

            else if (key.Key == ConsoleKey.Backspace)
            {
                directory.GoUp();
                Up(this, EventArgs.Empty);
                scroll = 0;
            }
        }

        internal void GetRoot()
        {
            var info = Path.GetPathRoot(directory.ReturnStringPath());
            Clean();
            directory = new PathView(info);
            Items = GetItems(info);
            wasPainted = false;
        }

        internal void Rename(string name)
        {
            try
            {
                var info = Items[selectedIndex].State;
                if (info is FileInfo)
                {
                    var file = info as FileInfo;
                    string newPath = Path.Combine(file.Directory.ToString(), name);
                    File.Move(file.FullName, newPath);
                }
                else if (info is DirectoryInfo)
                {
                    var dir = info as DirectoryInfo;
                    string newPath = Path.Combine(dir.Parent.FullName, name);
                    Directory.Move(dir.FullName, newPath);
                }
                Items = GetItems(directory.ReturnStringPath());
            }
            catch
            {
                throw new InvalidOperationException("File with the same name already exist");
            }
        }

        internal void Find(string name)
        {
            
            try
            {
                string[] allFoundFiles = Directory.GetFiles(directory.ReturnStringPath(), name, SearchOption.AllDirectories);
                Clean();
                Items.Clear();
                for (int i = 0; i < allFoundFiles.Length; i++)
                {
                    object dc = new object();
                    if (name.Contains("."))
                    Items.Add(new ListViewItem(dc, dc.ToString()));
                }
            }
            catch
            {
                throw new InvalidOperationException("This directory have folders without access");
            }
        }

        internal void CreateDirectory(string name)
        {
            string path = Path.Combine(directory.ReturnStringPath(), name);
            Directory.CreateDirectory(path);
            Items = GetItems(directory.ReturnStringPath());
        }

        public void Enter()
        {
            if (Items.Count == 0)
            {
                throw new InvalidOperationException("There is no items to choose.");
            }
            else
            {
                directory.Go(Items[selectedIndex].ReturnItemName());
                Selected(this, EventArgs.Empty);
            }
            scroll = 0;
        }

        public void PasteItem()
        {
            try
            {
                buffer.Upload(directory.ReturnStringPath());
                buffer.ClearBuffer();
            }
            catch
            {
                throw new InvalidOperationException("Buffer is missing");
            }

            Items = GetItems(directory.ReturnStringPath());
            wasPainted = false;
        }

        public void CutItem(bool isCut)
        {
            try
            {
                string fullPath = directory.ReturnStringPath() + "\\" + Items[selectedIndex].ReturnItemName();
                if (Items[selectedIndex].ReturnItemName().Contains("."))
                {
                    buffer.CopyToBuffFile(directory.ReturnStringPath(), Items[selectedIndex].ReturnItemName());
                    if (isCut)
                        File.Delete(fullPath);
                }
                else
                {
                    buffer.CopyToBuffFolder(directory.ReturnStringPath(), Items[selectedIndex].ReturnItemName());
                    if (isCut)
                        buffer.DeleteDirectory(fullPath);
                }
                Clean();
                Items = GetItems(directory.ReturnStringPath());
                wasPainted = false;
            }
            catch
            {
                throw new InvalidOperationException("There is no item to choose");
            }

        }

        public void GetDirectories()
        {
            Clean();
            Items.Clear();
            directory.Clear();
            string[] discs = Environment.GetLogicalDrives();
            for (int i = 0; i < discs.Length; i++)
            {
                DirectoryInfo dc = (new DirectoryInfo(discs[i]));
                Items.Add(new ListViewItem((object)dc, dc.ToString()));
            }
        }

        public void Properties()
        {
            if (Items.Count == 0)
                throw new InvalidOperationException("There is no item to choose");
            else {
                var saveForeground = Console.ForegroundColor;
                var saveBackground = Console.BackgroundColor;

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.BackgroundColor = ConsoleColor.White;

                Console.CursorLeft = 0;
                Console.CursorTop = height / 2;

                var info = Items[selectedIndex].State;

                try
                {
                    if (info is FileInfo)
                    {
                        FileProperty(info);
                    }
                    else
                    {
                        FolderProperty(info);
                    }
                }
                finally
                {
                    Console.ForegroundColor = saveForeground;
                    Console.BackgroundColor = saveBackground;
                }
            }

        }

        private void FileProperty(object info)
        {
            var file = info as FileInfo;

            Console.WriteLine("=".PadRight(Console.WindowWidth - 1, '='));
            Console.WriteLine(String.Format("{0,-30}{1,-50}", "Name: ", file.Name).PadRight(Console.WindowWidth - 1, ' '));
            Console.WriteLine(String.Format("{0,-30}{1,-80}", "Parent Directory: ", file.DirectoryName).PadRight(Console.WindowWidth - 1, ' '));
            Console.WriteLine(String.Format("{0,-30}{1,-80}", "Root Directory: ", file.Directory.Root).PadRight(Console.WindowWidth - 1, ' '));
            Console.WriteLine(String.Format("{0,-30}{1,-80}", "Is read only: ", file.IsReadOnly).PadRight(Console.WindowWidth - 1, ' '));
            Console.WriteLine(String.Format("{0,-30}{1,-80}", "Last Read Time: ", file.LastAccessTime).PadRight(Console.WindowWidth - 1, ' '));
            Console.WriteLine(String.Format("{0,-30}{1,-80}", "Last Write Time: ", file.LastWriteTime).PadRight(Console.WindowWidth - 1, ' '));
            Console.WriteLine(String.Format("{0,-30}{1,-80}", "Size: ", file.Length).PadRight(Console.WindowWidth - 1, ' '));
            Console.WriteLine("=".PadRight(Console.WindowWidth - 1, '='));
            Console.ReadKey();
        }

        private void FolderProperty(object info)
        {

            var dir = info as DirectoryInfo;

            Console.WriteLine("=".PadRight(Console.WindowWidth - 1, '='));
            Console.WriteLine(String.Format("{0,-30}{1,-50}", "Name: ", dir.Name).PadRight(Console.WindowWidth - 1, ' '));
            Console.WriteLine(String.Format("{0,-30}{1,-80}", "Root Directory: ", dir.Root).PadRight(Console.WindowWidth - 1, ' '));
            Console.WriteLine(String.Format("{0,-30}{1,-80}", "Parent Directory: ", dir.Parent.FullName).PadRight(Console.WindowWidth - 1, ' '));
            Console.WriteLine(String.Format("{0,-30}{1,-80}", "Last Read Time: ", dir.LastAccessTime).PadRight(Console.WindowWidth - 1, ' '));
            Console.WriteLine(String.Format("{0,-30}{1,-80}", "Last Write Time: ", dir.LastWriteTime).PadRight(Console.WindowWidth - 1, ' '));
            try
            {
                Console.WriteLine(String.Format("{0,-30}{1,-80}", "Size: ", dir.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length)).PadRight(Console.WindowWidth - 1, ' '));
                Console.WriteLine(String.Format("{0,-30}{1,-80}", "Files: ", dir.GetFiles().Length).PadRight(Console.WindowWidth - 1, ' '));
                Console.WriteLine(String.Format("{0,-30}{1,-80}", "Folders: ", dir.GetDirectories().Length).PadRight(Console.WindowWidth - 1, ' '));
            }
            catch
            {
                Console.WriteLine(String.Format("{0,-30}{1,-80}", "Size: ", "Information not available".PadRight(Console.WindowWidth - 1, ' ')));
                Console.WriteLine(String.Format("{0,-30}{1,-80}", "Files: ", "Information not available".PadRight(Console.WindowWidth - 1, ' ')));
                Console.WriteLine(String.Format("{0,-30}{1,-80}", "Folders: ", "Information not available".PadRight(Console.WindowWidth - 1, ' ')));
            }
            Console.WriteLine("=".PadRight(Console.WindowWidth - 1, '='));
            Console.ReadKey();
        }

        private static List<ListViewItem> GetItems(string path)
        {
            return new DirectoryInfo(path).GetFileSystemInfos().
                Select(f =>
                new ListViewItem(
                    f,
                    f.Name,
                    f is DirectoryInfo ? "<directory>" : f.Extension,
                    f is FileInfo ? BytesAsString((f as FileInfo).Length) : ""
                )).ToList();

        }

        private static void View_Selected(object sender, EventArgs e)
        {
            var view = (ListView)sender;
            var info = view.SelectedItem.State;
            view.Clean();
            if (info is FileInfo)
            {
                try
                {
                    Process.Start((info as FileInfo).FullName);
                }
                catch
                {
                    throw new InvalidOperationException("You can't open this file(");
                }
            }
            else if (info is DirectoryInfo)
            {
                try
                {
                    view.Items = GetItems((info as DirectoryInfo).FullName);
                }
                catch
                {
                    throw new InvalidOperationException("You can't open this directoryectory(");
                }
            }
        }
        private static void UpDirectory(object sender, EventArgs e)
        {
            var view = (ListView)sender;
            object info = new object();
            if (view.Items.Count != 0)
                info = view.SelectedItem.State;
            view.Clean();
            try
            {
                view.Items = GetItems(view.directory.Getdirectoryectory());
            }
            catch
            {
                throw new InvalidOperationException("You can't open this directoryectory(");
            }
        }

        private static string BytesAsString(float bytes)
        {
            string[] suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            double doubleBytes = bytes;

            
                for (i = 0; (int)(bytes / 1024) > 0; i++, bytes /= 1024)
                {
                    doubleBytes = bytes / 1024.0;
                }
            
            return string.Format("{0:0.00} {1}", doubleBytes, suffix[i]);
        }

        public event EventHandler Selected;
        public event EventHandler Up;
    }
}
