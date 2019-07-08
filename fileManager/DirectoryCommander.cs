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

        private MyBuffer buffer;

        private int windowHeight;

        public DirectoryectoryCommander(int windowHeight)
        {
            this.windowHeight = windowHeight;
            windows = new ListView[] {
                new ListView(5, 2, windowHeight),
                new ListView(Console.WindowWidth/2 +5, 2, windowHeight)
                };
            onFocusedElement = 1;
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
                        windows[i].Render();
                        ErrorMessage("Something goes wrong");
                    }

                }
            }
            

        }

        private void RenderItems()
        {
            foreach (var item in windows)
            {
                item.Render();
            }
        }
        private void Update(ConsoleKeyInfo key, ref int item)
        {
            if (key.Key == ConsoleKey.RightArrow)
                item++;
            else if (key.Key == ConsoleKey.LeftArrow)
                item--;

            if (item >= windows.Length)
                item = 0;
            else if (item < 0)
                item = windows.Length - 1;

            /*if (key.Key == ConsoleKey.F1)
            {
                buffer = new MyBuffer(directory.ReturnStringPath() + "\\" + Items[selectedIndex].ReturnItemName());
            }*/
        }

        private void ErrorMessage(string message)
        {

            var saveForeground = Console.ForegroundColor;
            var saveBackground = Console.BackgroundColor;
 
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.BackgroundColor = ConsoleColor.DarkCyan;

            
            Console.CursorTop = windowHeight / 2;

            Console.WriteLine("=".PadRight(Console.WindowWidth - 2, '='));          
            Console.WriteLine(string.Format("{0, -50}{1,-45}", " ", message.PadRight(Console.WindowWidth-51, ' ')));
            Console.WriteLine("=".PadRight(Console.WindowWidth - 1, '='));
            Console.WriteLine(string.Format("{0, -60}{1,-45}", " ", "OK".PadRight(Console.WindowWidth - 61, ' ')));

            Console.ForegroundColor = saveForeground;
            Console.BackgroundColor = saveBackground;
            Console.ReadKey();
            Console.Clear();
            RenderItems();
        }


    }
}
