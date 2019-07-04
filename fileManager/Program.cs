using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileManager
{
   
    class Program
    {
        static void Main(string[] args)
        {
            var view = new ListView(10, 2, 20);

            view.ColumnsWidth = new List<int> { Console.WindowWidth - 30, 10, 10 };
            view.Items = GetItems(view.dir.GetDirectory());

            view.Selected += View_Selected;
            view.Up += Up;

           /* for (int i = 0; i < 10; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.CursorLeft = i * ((Console.WindowWidth / 9));
                Console.CursorTop = 25;
                string butName = "";
                switch(i)
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

                Console.WriteLine(butName.PadRight((Console.WindowWidth / 9) - 1));
            }*/
            Console.BackgroundColor = ConsoleColor.Black;
            while (true)
            {
                view.Render();
                var key = Console.ReadKey();
                view.Update(key);
                view.Render();
            }

        }

        private static List<ListViewItem> GetItems(string path)
        {
                return new DirectoryInfo(path).GetFileSystemInfos().
                    Select(f =>
                    new ListViewItem(
                        f,
                        f.Name,
                        f is DirectoryInfo ? "<dir>" : f.Extension,
                        f is FileInfo ? (f as FileInfo).Length.ToString() : ""
                    )).ToList();
         
        }

        private static void View_Selected(object sender, EventArgs e)
        {
            var view = (ListView)sender;
            var info = view.SelectedItem.State;
            view.Clean();
            if (info is FileInfo)
            {
                try {
                    Process.Start((info as FileInfo).FullName);
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("You can't open this file(");
                    Console.WriteLine();
                    Console.WriteLine("Press something and we continue ;)");
                    Console.ReadLine();
                    Console.Clear();
                    view.Render();
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
                    Console.Clear();
                    Console.WriteLine("You can't open this directory(");
                    Console.WriteLine();
                    Console.WriteLine("Press something and we continue ;)");
                    Console.ReadLine();
                    Console.Clear();
                    view.Render();
                }
                
            }

        }

        private static void Up(object sender, EventArgs e)
        {
            var view = (ListView)sender;
            var info = view.SelectedItem.State;
            
                view.Clean();
                view.Items = GetItems(view.dir.GetDirectory());
            }

        }

    }

