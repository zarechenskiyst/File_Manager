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

        private int x, y;
        public List<object> Items { get; set; }

        public ListView(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void Clean()
        {
            wasPainted = false;
            for (int i = 0; i < Items.Count; i++)
            {
                Console.CursorLeft = x;
                Console.CursorTop = i+y;
                Console.Write(new string(' ', Items[i].ToString().Length));
            }
        }

        public object Item { get; set; }

        public bool Focused { get; set; }

        public void Render()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (wasPainted)
                {
                    if (i != selectedIndex && i != selectedPrevIndex)
                        continue;
                }

                var item = Items[i];
                var saveForeground = Console.ForegroundColor;
                var saveBackground = Console.BackgroundColor;
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }

                Console.CursorLeft = x;
                Console.CursorTop = i+y;
                Console.Write(item);

                Console.ForegroundColor = saveForeground;
                Console.BackgroundColor = saveBackground;
            }
            wasPainted = true;
        }
        public void Update(ConsoleKeyInfo key)
        {
            selectedPrevIndex = selectedIndex;
            if (key.Key == ConsoleKey.DownArrow && selectedIndex+1<Items.Count)
                selectedIndex++;
            else if (key.Key == ConsoleKey.UpArrow && selectedIndex -1 >= 0)
                selectedIndex--;
        }
    }
}
