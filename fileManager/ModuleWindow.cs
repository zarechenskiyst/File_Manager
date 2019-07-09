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
            height = Console.WindowHeight/2;
        }

        public static void ErrorMessage(string message)
        {
            var saveForeground = Console.ForegroundColor;
            var saveBackground = Console.BackgroundColor;

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.BackgroundColor = ConsoleColor.DarkCyan;

            Console.CursorTop = height ;

            Console.WriteLine("=".PadRight(Console.WindowWidth - 3, '='));
            Console.WriteLine(string.Format("{0, -50}{1,-45}", " ", message.PadRight(Console.WindowWidth - 61, ' ')));
            Console.WriteLine("=".PadRight(Console.WindowWidth - 3, '='));
            Console.WriteLine(string.Format("{0, -60}{1,-45}", " ", "OK".PadRight(Console.WindowWidth - 61, ' ')));

            Console.ForegroundColor = saveForeground;
            Console.BackgroundColor = saveBackground;
            Console.ReadKey();
            Console.Clear();
        }

        public static string ReadMessage()
        {
            var saveForeground = Console.ForegroundColor;
            var saveBackground = Console.BackgroundColor;

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.BackgroundColor = ConsoleColor.DarkCyan;

            Console.CursorTop = height;

            Console.WriteLine("=".PadRight(Console.WindowWidth - 3, '='));
            Console.WriteLine(string.Format("{0, -50}{1,-45}", " ", "Enter name:"));

            Console.ForegroundColor = saveForeground;
            Console.BackgroundColor = saveBackground;

            return Console.ReadLine();
        }


    }
}
