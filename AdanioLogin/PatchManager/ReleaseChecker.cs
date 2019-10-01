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
    public class RealRelease : Release
    {

        public new string TarballUrl
        {
            get
            {
                return base.TarballUrl;
            }
            set
            {
                base.TarballUrl = value;
            }
        }
        public new Author Author { get { return base.Author; }  set { base.Author = value; } }
        public new DateTimeOffset? PublishedAt { get { return base.PublishedAt; }  set { base.PublishedAt = value; } }
        public new DateTimeOffset CreatedAt { get { return base.CreatedAt; }  set { base.CreatedAt = value; } }
        public new bool Prerelease { get { return base.Prerelease; }  set { base.Prerelease = value; } }
        public new bool Draft { get { return base.Draft; }  set { base.Draft = value; } }
        public new string Body { get { return base.Body; }  set { base.Body = value; } }
        public new string Name { get { return base.Name; }  set { base.Name = value; } }
        public new string TagName { get { return base.TagName; }  set { base.TagName = value; } }
        public new string ZipballUrl { get { return base.ZipballUrl; }  set { base.ZipballUrl = value; } }
        //
        // Summary:
        //     GraphQL Node Id
        public new string NodeId { get { return base.NodeId; }  set { base.NodeId = value; } }
        public new int Id { get { return base.Id; }  set { base.Id = value; } }
        public new string UploadUrl { get { return base.UploadUrl; }  set { base.UploadUrl = value; } }
        public new string AssetsUrl { get { return base.AssetsUrl; }  set { base.AssetsUrl = value; } }
        public new string HtmlUrl { get { return base.HtmlUrl; }  set { base.HtmlUrl = value; } }
        public new string Url { get { return base.Url; }  set { base.Url = value; } }
        public new string TargetCommitish { get { return base.TargetCommitish; }  set { base.TargetCommitish = value; } }
        public new IReadOnlyList<ReleaseAsset> Assets { get { return base.Assets; }  set { base.Assets = value; } }

    }

    public class ReleaseChecker
    {
        private readonly string owner;
        private readonly string reponame;
        private GitHubClient client;
        private Release latestrelease;
        private string path;
        public bool DownloadStatus { get; private set; }
        public bool CheckStatus { get; private set; }

        public ReleaseChecker(string appname, string owner, string reponame)
        {
            client = new GitHubClient(new ProductHeaderValue(appname + "PatchManager"));
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

        private async Task<bool> CheckIfNewRelease()
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
            if (current == null)
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

        private void DownloadRelease()
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
            File.WriteAllText(path + "\\version.json", JsonConvert.SerializeObject(latestrelease,Formatting.Indented));

            DownloadStatus = true;
            System.Diagnostics.Process.Start(path);
        }
    }
}