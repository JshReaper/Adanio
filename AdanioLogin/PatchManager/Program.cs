using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace PatchManager
{
    class Program
    {
        static void Main(string[] args)
        {
            string username = "";
            string pass = "";

            Console.WriteLine("please enter github username/email");
            username = Console.ReadLine();
            Console.WriteLine("please enter github password");

            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                // Backspace Should Not Work
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Substring(0, (pass.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);

            ReleaseChecker checker = new ReleaseChecker("Adanio", "JshReaper", "Adanio",username,pass);
            if (checker.IsNewRelease)
            {
                checker.StartDownload();
                Console.WriteLine("Downloading new release");

                int ticks = 0;
                int dotTick = 0;
                while (!checker.DownloadStatus)
                {
                    ticks++;
                    dotTick++;
                    if (dotTick > 100)
                    {
                        Console.Write(".");
                        dotTick = 0;

                    }
                    if (ticks > 1000)
                    {
                        Console.Clear();
                        Console.WriteLine("Downloading new release");
                        ticks = 0;
                    }
                }
                Console.WriteLine("Download completed");
            }
            else
            {
                Console.WriteLine("Version up to date");
            }
            
        }


    }
}
