using Newtonsoft.Json;
using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PatchManager
{
    public class ReleaseChecker
    {
        private readonly string owner;
        private readonly string reponame;
        GitHubClient client;
        Release latestrelease;
        string path;
        public bool DownloadStatus { get; private set; }
        public bool CheckStatus { get; private set; }
        public ReleaseChecker(string appname, string owner, string reponame)
        {
            client = new GitHubClient(new ProductHeaderValue(appname+"PatchManager"));
            path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\"+appname;
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
        async Task<bool> CheckIfNewRelease()
        {
            CheckStatus = false;
            latestrelease = await client.Repository.Release.GetLatest(owner, reponame);

            Release current = null;
            try
            {
                current = JsonConvert.DeserializeObject<Release>(File.ReadAllText(path+"\\version.json"));
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
            File.WriteAllText(path + "\\version.json", JsonConvert.SerializeObject(latestrelease));

            DownloadStatus = true;
            System.Diagnostics.Process.Start(path);
        }
    }
}
