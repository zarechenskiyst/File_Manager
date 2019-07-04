using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileManager
{
    class ListView
    {
        public PathView dir;
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
            dir = new PathView();
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
            dir.Show(x);
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
                    Console.Clear();
                    Console.WriteLine("There is no items to choose. Press something and cats will love you!");
                    Console.ReadLine();
                    Console.Clear();
                    this.Render();
                }
                else {
                    dir.Go(Items[selectedIndex].ReturnItemName());
                    Selected(this, EventArgs.Empty);
                }
            }
            else if (key.Key == ConsoleKey.Backspace)
            {
                dir.GoUp();
                Up(this, EventArgs.Empty);
            }
            else if (key.Key == ConsoleKey.F4)
            {
                Items.Clear();
                Console.Clear();
                dir.Clear();
                string[] discs = Environment.GetLogicalDrives();
                for (int i = 0; i < discs.Length; i++)
                {
                    DirectoryInfo dc = (new DirectoryInfo(discs[i]));
                    Items.Add(new ListViewItem((object)dc, dc.ToString()));
                }
                wasPainted = false;
            }

        }

        public event EventHandler Selected;
        public event EventHandler Up;
    }
}
