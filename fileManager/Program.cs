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
            Console.CursorVisible = false;

            var view = new ListView(10, 2, 20);
          

            view.ColumnsWidth = new List<int> { Console.WindowWidth - 30, 10, 10 };
            view.Items = GetItems(@"C:\");
                

            view.Selected += View_Selected;

            while (true)
            {
                view.Render();
                var key = Console.ReadKey();
                view.Update(key);
                view.Render();
            }

        }

        private static List<ListViewItem>  GetItems(string path)
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
            if (info is FileInfo)
                Process.Start((info as FileInfo).FullName);
            else if (info is DirectoryInfo)
            {
                view.Clean();
                view.Items = GetItems((info as DirectoryInfo).FullName);
            }

        }
    }
}
