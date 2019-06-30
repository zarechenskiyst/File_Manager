using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileManager
{
    class ListView
    {
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
        }

        public void Clean()
        {
            selectedIndex = selectedPrevIndex = 0;
            wasPainted = false;
            for (int i = 0; i < Items.Count; i++)
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
            for (int i = 0; i <Math.Min(height, Items.Count); i++)
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

            if (selectedIndex >= height+scroll)
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
                Selected(this, EventArgs.Empty);
        }

        public event EventHandler Selected;
    }
}
