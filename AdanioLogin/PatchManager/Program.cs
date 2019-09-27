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
            }
            while (!checker.DownloadStatus)
            {
                Console.WriteLine("Downloading new release");
                Console.Clear();
            }
            Console.WriteLine("Download completed");
            Console.ReadKey();
        }
        
       
    }
}
