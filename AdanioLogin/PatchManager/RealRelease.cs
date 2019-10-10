using Octokit;
using System;
using System.Collections.Generic;

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
}
