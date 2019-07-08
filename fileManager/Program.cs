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
            Console.WindowWidth = 120;
            Console.WindowHeight = 35;
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.DarkGray;

            var window = new DirectoryectoryCommander(20);
            window.StartProgram();

            
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


        }



    }

}

