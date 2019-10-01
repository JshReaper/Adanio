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
            ReleaseChecker checker = new ReleaseChecker("Adanio", "JshReaper", "Adanio");
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
                    if(dotTick> 100)
                    {
                        Console.Write(".");
                        dotTick = 0;
                        
                    }
                    if(ticks> 1000)
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
            
            Console.ReadKey();
        }
        
       
    }
}
