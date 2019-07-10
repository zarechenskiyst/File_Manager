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

        private MyBuffer buffer;

        public DirectoryectoryCommander(int windowHeight)
        {
            buffer = new MyBuffer("temp");

            this.windowHeight = windowHeight;
            windows = new ListView[] {
                new ListView(5, 2, windowHeight, buffer),
                new ListView(Console.WindowWidth/2 +5, 2, windowHeight, buffer)
                };
            onFocusedElement = 0;

            activeColor = ConsoleColor.Black;
            nonActiveColor = ConsoleColor.DarkGray;
        }

        public void StartProgram()
        {
            RenderItems();
            while (true)
            {
                Console.BackgroundColor = nonActiveColor;
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
                {
                    windows[i].Render(activeColor, painAll: true);
                }
                else
                {
                    windows[i].Render(nonActiveColor, true);
                }
                
            }
            RenderButtons();
        }
        private void Update(ConsoleKeyInfo key, ref int item)
        {
            try
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
                {
                    item = 0;
                    RenderItems();
                }

                else if (item < 0)
                {
                    item = windows.Length - 1;
                    RenderItems();
                }

                if (key.Key == ConsoleKey.F1)
                {
                    windows[onFocusedElement].CutItem(false);
                }
                else if (key.Key == ConsoleKey.F2)
                {
                    windows[onFocusedElement].CutItem(true);
                }

                else if (key.Key == ConsoleKey.F3)
                {
                    windows[onFocusedElement].PasteItem();
                }

                else if (key.Key == ConsoleKey.F4)
                {
                    windows[onFocusedElement].GetRoot();
                }

                else if (key.Key == ConsoleKey.F5)
                {
                    windows[onFocusedElement].GetDirectories();
                }

                else if (key.Key == ConsoleKey.F6)
                {
                    windows[onFocusedElement].Properties();
                    RenderItems();
                }
                else if (key.Key == ConsoleKey.F7)
                {
                    if (windows[onFocusedElement].Items.Count == 0)
                        throw new InvalidOperationException("There is no items");
                    var newName = ModuleWindow.ReadMessage("Enter new Name:");
                    windows[onFocusedElement].Rename(newName);
                    RenderItems();
                }
                else if (key.Key == ConsoleKey.F8)
                {
                    if (windows[onFocusedElement].Items.Count == 0)
                        throw new InvalidOperationException("There is no items in this directory");
                    var name = ModuleWindow.ReadMessage("Enter file name:");
                    windows[onFocusedElement].Find(name);
                    if (windows[onFocusedElement].Items.Count == 0)
                        throw new InvalidOperationException("File not found(");
                    RenderItems();
                }
                else if (key.Key == ConsoleKey.F9)
                {
                    var name = ModuleWindow.ReadMessage("Enter new direction name:");
                    windows[onFocusedElement].CreateDirectory(name);
                    RenderItems();
                }

                else if (key.Key == ConsoleKey.Enter)
                {
                    windows[onFocusedElement].Enter();
                }
            }
            catch (Exception ex)
            {
                ModuleWindow.ErrorMessage(ex.Message);
                RenderItems();
            }
        }

        private void RenderButtons()
        {
            var saveForeground = Console.ForegroundColor;
            var saveBackground = Console.BackgroundColor;
            Console.CursorLeft = 0;
            for (int i = 0; i < 9; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.CursorLeft = Console.CursorLeft + 2;
                Console.CursorTop = windowHeight + 5;
                string butName = "";
                switch (i)
                {
                    case 0:
                        butName = "F1-Copy";
                        break;
                    case 1:
                        butName = "F2-Cut";
                        break;
                    case 2:
                        butName = "F3-Paste";
                        break;
                    case 3:
                        butName = "F4-Root";
                        break;
                    case 4:
                        butName = "F5-List of discs";
                        break;
                    case 5:
                        butName = "F6-Property";
                        break;
                    case 6:
                        butName = "F7-Rename";
                        break;
                    case 7:
                        butName = "F8-Find";
                        break;
                    case 8:
                        butName = "F9-New Folder";
                        break;
                }
                Console.Write(string.Format("{0,-10}", butName));


            }
            Console.ForegroundColor = saveForeground;
            Console.BackgroundColor = saveBackground;
        }
    }
}
