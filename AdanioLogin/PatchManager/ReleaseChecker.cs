using Newtonsoft.Json;
using Octokit;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Web.Security;

namespace PatchManager
{

    public class ReleaseChecker
    {
        private readonly string owner;
        private readonly string reponame;
        GitHubClient client;
        Release latestrelease;
        public string path { get; private set; }
        public bool DownloadStatus { get; private set; }
        public bool CheckStatus { get; private set; }

        public ReleaseChecker(string appname, string owner, string reponame,string username, string pass)
        {

            client = new GitHubClient(new ProductHeaderValue(appname));
            var basicAuth = new Credentials(username, pass); // NOTE: not real credentials
            client.Credentials = basicAuth;

            path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + appname;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            this.owner = owner;
            this.reponame = reponame;
        }
        
        public bool IsNewRelease
        {
            get
            {
                bool toreturn = CheckIfNewRelease().Result;
                while (!CheckStatus)
                {

                }
                return toreturn;
            }
        }
        public async Task<bool> CheckIfNewRelease()
        {
            CheckStatus = false;
            latestrelease = await client.Repository.Release.GetLatest(owner, reponame);

            Release current = null;
            try
            {
                current = JsonConvert.DeserializeObject<RealRelease>(File.ReadAllText(path+"\\version.json")) as Release;
            }
            catch (Exception e)
            {
                string ex = e.Message;
                File.Create(path + "\\version.json");
            }
            if(current == null)
            {
                CheckStatus = true;
                return true;
            }
            if (latestrelease.TagName != current.TagName)
            {
                CheckStatus = true;
                return true;
            }
            else
            {
                CheckStatus = true;
                DownloadStatus = true;
                return false;
            }
        }
        public void StartDownload()
        {
            DownloadStatus = false;
            Thread th = new Thread(DownloadRelease);
            th.Start();
        }
        void DownloadRelease()
        {
            Task t = DownLoadReleaseAsync();
            t.Wait();
        }
        private void clearFolder(string FolderName)
        {
            DirectoryInfo dir = new DirectoryInfo(FolderName);

            foreach (FileInfo fi in dir.GetFiles())
            {
                fi.Delete();
            }

            foreach (DirectoryInfo di in dir.GetDirectories())
            {
                clearFolder(di.FullName);
                di.Delete();
            }
        }
        async Task DownLoadReleaseAsync()
        {
            if(latestrelease == null) {
                latestrelease = await client.Repository.Release.GetLatest(owner, reponame);
            }
            //Gets the assets for the latest relase
            var assets = await client.Repository.Release.GetAllAssets(owner, reponame, latestrelease.Id);
            foreach (var asset in assets)
            {
                //Download the release
                var wc = new WebClient();
                var filename = Path.Combine(path, asset.Name);

                await wc.DownloadFileTaskAsync(asset.BrowserDownloadUrl, filename);
            }
            File.WriteAllText(path + "\\version.json", JsonConvert.SerializeObject(latestrelease,Formatting.Indented));

            DownloadStatus = true;

            if (Directory.Exists(path + "\\game"))
            {
                clearFolder(path + "\\game");
                Directory.CreateDirectory(path + "\\game");
            }
            else
            {
                Directory.CreateDirectory(path + "\\game");
            }

            ZipFile.ExtractToDirectory(path + "\\Adanio.zip",path +"\\game");

            File.Delete(path + "\\Adanio.zip");

            System.Diagnostics.Process.Start(path+ "\\game\\Adanio.exe");
        }
    }
}
