using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileManager
{
    class ModuleWindow
    {
        private static int height;

        static ModuleWindow()
        {
            height = Console.WindowHeight / 2;
        }

        public static void ErrorMessage(string message)
        {
            var saveForeground = Console.ForegroundColor;
            var saveBackground = Console.BackgroundColor;

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.BackgroundColor = ConsoleColor.DarkRed;

            Console.CursorLeft = 0;
            Console.CursorTop = height - 5;

            Console.WriteLine("=".PadRight(Console.WindowWidth - 1, '='));
            Console.WriteLine(string.Empty.PadRight(Console.WindowWidth - 1));
            Console.WriteLine(string.Format("{0, -45}{1,-65}", " ", message.PadRight(Console.WindowWidth - 46, ' ')));
            Console.WriteLine(string.Empty.PadRight(Console.WindowWidth - 1));
            Console.WriteLine("=".PadRight(Console.WindowWidth - 1, '='));
            Console.WriteLine(string.Format("{0, -60}{1,-45}", " ", "OK".PadRight(Console.WindowWidth - 61, ' ')));

            Console.ForegroundColor = saveForeground;
            Console.BackgroundColor = saveBackground;
            Console.ReadKey();
            Console.Clear();
        }

        public static string ReadMessage(string message)
        {
            var saveForeground = Console.ForegroundColor;
            var saveBackground = Console.BackgroundColor;

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.BackgroundColor = ConsoleColor.DarkCyan;

            Console.CursorTop = height;
            Console.CursorLeft = 0;

            Console.WriteLine("=".PadRight(Console.WindowWidth - 1, '='));
            Console.WriteLine(string.Format("{0, -50}{1,-69}", " ", message));
            Console.WriteLine(string.Empty.PadRight(Console.WindowWidth - 1));
            Console.WriteLine(string.Empty.PadRight(Console.WindowWidth - 1));
            Console.WriteLine(string.Empty.PadRight(Console.WindowWidth - 1));

            Console.CursorTop = Console.CursorTop - 2;
            Console.CursorLeft = Console.WindowWidth / 2 - 10;

            var returnString = Console.ReadLine();

            Console.WriteLine("=".PadRight(Console.WindowWidth - 3, '='));
            Console.WriteLine(string.Format("{0, -60}{1,-45}", " ", "OK".PadRight(Console.WindowWidth - 61, ' ')));

            Console.ReadKey();

            Console.ForegroundColor = saveForeground;
            Console.BackgroundColor = saveBackground;

            return returnString;
        }
    }
}
