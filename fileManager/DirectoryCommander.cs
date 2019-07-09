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
                        ModuleWindow.ReadMessage();
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
        }
        private void Update(ConsoleKeyInfo key, ref int item)
        {
            try
            {
                if (key.Key == ConsoleKey.RightArrow)
                    item++;

                else if (key.Key == ConsoleKey.LeftArrow)
                    item--;

                if (item >= windows.Length)
                    item = 0;

                else if (item < 0)
                    item = windows.Length - 1;

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
                    try
                    {
                        windows[onFocusedElement].PasteItem();
                    }
                    catch
                    {
                        throw new InvalidOperationException("Buffer is missing");
                    }
                }

                else if (key.Key == ConsoleKey.F4)
                {
                    windows[onFocusedElement].GetDirectories();
                }

                else if (key.Key == ConsoleKey.F6)
                {
                    windows[onFocusedElement].Properties();
                    RenderItems();
                }

                else if (key.Key == ConsoleKey.Enter)
                {
                    try
                    {
                        windows[onFocusedElement].Enter();
                    }
                    catch (Exception ex)
                    {
                        ModuleWindow.ErrorMessage(ex.Message);
                        RenderItems();
                    }
                }
            }
            catch(Exception ex)
            {
                ModuleWindow.ErrorMessage(ex.Message);
            }

            RenderItems();
        }




    }
}
