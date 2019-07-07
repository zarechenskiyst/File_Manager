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

        public ListView(int x, int y, int height)
        {
            this.x = x;
            this.y = y;
            this.height = height;
            directory = new PathView();

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

        public void Render()
        {
            directory.Show(x, ColumnsWidth.Sum());
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
            else if (key.Key == ConsoleKey.Enter)
            {
                if (Items.Count == 0)
                {
                    this.Error("There is no items to choose.");
                }
                else
                {
                    directory.Go(Items[selectedIndex].ReturnItemName());
                    Selected(this, EventArgs.Empty);
                }
                scroll = 0;
            }
            else if (key.Key == ConsoleKey.Backspace)
            {
                directory.GoUp();
                Up(this, EventArgs.Empty);
                scroll = 0;
            }
            else if (key.Key == ConsoleKey.F4)
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
                wasPainted = false;
            }
            else if (key.Key == ConsoleKey.F1)
            {
                buffer = new MyBuffer(directory.ReturnStringPath(), Items[selectedIndex].ReturnItemName());
            }
            else if (key.Key == ConsoleKey.F2)
            {
                string fullPath = directory.ReturnStringPath() + "\\" + Items[selectedIndex].ReturnItemName();
                Directory.CreateDirectory("temp");
                buffer = new MyBuffer("temp", Items[selectedIndex].ReturnItemName());
                if (Items[selectedIndex].ReturnItemName().Contains("."))
                {
                    buffer.FileCopy(fullPath, buffer.BuffPath);
                    File.Delete(fullPath);
                }
                else
                {
                    buffer.DirectoryCopy(fullPath, buffer.BuffPath);
                    buffer.DeleteDirectory(fullPath);
                }
                buffer = new MyBuffer(buffer.BuffPath , Items[selectedIndex].ReturnItemName());

            }
            else if (key.Key == ConsoleKey.F3)
            {
                if (buffer.NameFile.Contains("."))
                    buffer.FileCopy(buffer.BuffPath,directory.ReturnStringPath());
                else
                    buffer.DirectoryCopy(buffer.BuffPath, directory.ReturnStringPath());
                wasPainted = false;
                if (Directory.Exists("temp"))
                    buffer.DeleteDirectory("temp");
            }
            else if (key.Key == ConsoleKey.F6)
            {
                Properties(Items[selectedIndex]);

            }
        }

        private void Properties(ListViewItem view)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.BackgroundColor = ConsoleColor.White;


            Console.CursorLeft = 0;
            Console.CursorTop =height/2;
            var info = view.State;
            if (info is FileInfo)
            {
                var file=info as FileInfo;

                Console.WriteLine("=".PadRight(Console.WindowWidth - 1, '='));
                Console.WriteLine(String.Format("{0,-30}{1,-50}", "Name: ", file.Name).PadRight(Console.WindowWidth-1, ' '));
                Console.WriteLine(String.Format("{0,-30}{1,-80}", "Parent Directory: " , file.Directory).PadRight(Console.WindowWidth-1, ' '));
                Console.WriteLine(String.Format("{0,-30}{1,-80}", "Root Directory: " , file.DirectoryName).PadRight(Console.WindowWidth - 1, ' '));
                Console.WriteLine(String.Format("{0,-30}{1,-80}", "Is read only: " , file.IsReadOnly).PadRight(Console.WindowWidth - 1, ' '));
                Console.WriteLine(String.Format("{0,-30}{1,-80}", "Last Read Time: " , file.LastAccessTime).PadRight(Console.WindowWidth - 1, ' '));
                Console.WriteLine(String.Format("{0,-30}{1,-80}", "Last Write Time: " , file.LastWriteTime).PadRight(Console.WindowWidth - 1, ' '));
                Console.WriteLine(String.Format("{0,-30}{1,-80}", "Size: ", file.Length).PadRight(Console.WindowWidth - 1, ' '));
                Console.WriteLine("=".PadRight(Console.WindowWidth - 1, '='));
                Console.ReadKey();
            }
            else
            {
                var dir = info as DirectoryInfo;

                Console.WriteLine("=".PadRight(Console.WindowWidth-1, '='));
                Console.WriteLine(String.Format("{0,-30}{1,-50}", "Name: ", dir.Name).PadRight(Console.WindowWidth - 1, ' '));
                Console.WriteLine(String.Format("{0,-30}{1,-80}", "Root Directory: ", dir.FullName).PadRight(Console.WindowWidth - 1, ' '));
                Console.WriteLine(String.Format("{0,-30}{1,-80}", "Parent Directory: ", dir.Parent.Name).PadRight(Console.WindowWidth - 1, ' '));
                Console.WriteLine(String.Format("{0,-30}{1,-80}", "Last Read Time: ",  dir.LastAccessTime).PadRight(Console.WindowWidth - 1, ' '));
                Console.WriteLine(String.Format("{0,-30}{1,-80}", "Last Write Time: ", dir.LastWriteTime).PadRight(Console.WindowWidth - 1, ' '));
                Console.WriteLine(String.Format("{0,-30}{1,-80}", "Size: ", dir.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length)).PadRight(Console.WindowWidth - 1, ' '));
                Console.WriteLine(String.Format("{0,-30}{1,-80}", "Files: ", dir.GetFiles().Length).PadRight(Console.WindowWidth - 1, ' '));
                Console.WriteLine(String.Format("{0,-30}{1,-80}", "Folders: ", dir.GetDirectories().Length).PadRight(Console.WindowWidth - 1, ' '));
                Console.WriteLine("=".PadRight(Console.WindowWidth - 1, '='));
                Console.ReadKey();
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            wasPainted = false;

        }

        private static List<ListViewItem> GetItems(string path)
        {
            return new DirectoryInfo(path).GetFileSystemInfos().
                Select(f =>
                new ListViewItem(
                    f,
                    f.Name,
                    f is DirectoryInfo ? "<directory>" : f.Extension,
                    f is FileInfo ? (f as FileInfo).Length.ToString() : ""
                )).ToList();

        }

        public void Error(string message)
        {
            Console.Clear();
            Console.WriteLine(message);
            Console.WriteLine();
            Console.WriteLine("Press something and we continue ;)");
            Console.ReadLine();
            Console.Clear();
            this.Render();
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
                    view.Error("You can't open this file(");
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
                    view.Error("You can't open this directoryectory(");
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
                view.Error("You can't open this directoryectory(");
            }

        }

        public event EventHandler Selected;
        public event EventHandler Up;
    }
}
