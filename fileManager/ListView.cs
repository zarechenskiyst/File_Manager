﻿using System;
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

                int elementIndex =i+scroll;
                

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
                    directory.Go(Items[selectedIndex].ReturnItemDir());
                    Selected(this, EventArgs.Empty);
                }
            }
            else if (key.Key == ConsoleKey.Backspace)
            {
                directory.GoUp();
                Up(this, EventArgs.Empty);
            }
            else if (key.Key == ConsoleKey.F4)
            {
                Items.Clear();
                Console.Clear();
                directory.Clear();
                string[] discs = Environment.GetLogicalDrives();
                for (int i = 0; i < discs.Length; i++)
                {
                    DirectoryInfo dc = (new DirectoryInfo(discs[i]));
                    Items.Add(new ListViewItem((object)dc, dc.ToString()));
                }
                wasPainted = false;
            }

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
