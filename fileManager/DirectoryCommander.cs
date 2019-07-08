using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace fileManager
{
    class DirectoryectoryCommander
    {
        private ListView[] windows;

        private int onFocusedElement;

        private int windowHeight;

        private ConsoleColor activeColor, nonActiveColor;

        public DirectoryectoryCommander(int windowHeight)
        {
            this.windowHeight = windowHeight;
            windows = new ListView[] {
                new ListView(5, 2, windowHeight),
                new ListView(Console.WindowWidth/2 +5, 2, windowHeight)
                };
            onFocusedElement = 0;

            activeColor = ConsoleColor.DarkGray;
            nonActiveColor = ConsoleColor.White;
        }

        public void StartProgram()
        {
            RenderItems();
            while (true)
            {
                var key = Console.ReadKey();
                Update(key, ref onFocusedElement);
                for (int i = 0; i < windows.Length; i++)
                {
                    if (i == onFocusedElement)
                    { 
                        windows[i].Update(key);
                        windows[i].Render(activeColor);
                    }

                }
            }
        }

        private void RenderItems()
        {
            Console.Clear();
            for (int i = 0; i < windows.Length; i++)
            {
                if (i == onFocusedElement)
                    windows[i].Render(activeColor, painAll: true);
                else
                    windows[i].Render(nonActiveColor, true);
            }
        }
        private void Update(ConsoleKeyInfo key, ref int item)
        {
            if (key.Key == ConsoleKey.RightArrow)
            {
                item++;
                RenderItems();
            }
            else if (key.Key == ConsoleKey.LeftArrow)
            {
                item--;
                RenderItems();
            }
            if (item >= windows.Length)
                item = 0;
            else if (item < 0)
                item = windows.Length - 1;
            /*if (key.Key == ConsoleKey.F1)
            {
                buffer = new MyBuffer(windows[i].Items.directory.ReturnStringPath() + "\\" + Items[selectedIndex].ReturnItemName());
            }*/
        }

        


    }
}
