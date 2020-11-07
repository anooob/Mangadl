using HtmlAgilityPack;
using MangaDl.Model.Json.MangaLife;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MangaDl
{



    class MangaMangaLife : MangaBase
    {
        private const string m_baseUrl = "https://manga4life.com";

        public override string ListName
        {
            get { return m_name + " - MangaLife"; }
        }

        public MangaMangaLife(string url)
            : base(url)
        {
            ParseUrl();
            Site = MangaSite.MANGALIFE;
            m_path = Path.Combine(Config.SavePath, "Mangalife", m_name);
        }

        protected override void ParseUrl()
        {
            var tokens = m_url.Split('/').ToList();
            tokens.RemoveAll(s => s == "");
            m_name = tokens.Last().Split('=').Last();
        }

        public override void GetChapters(object param)
        {
            m_isGettingChapters = true;
            m_chapters.Clear();

            HtmlDocument document = new HtmlDocument();
            try
            {
                using (var webClient = new WebClientGZ())
                {
                    var site = webClient.DownloadString(m_url);
                    document.LoadHtml(site);
                }
            }
            catch (Exception e)
            {
                Log.WriteLine(e.Message);
                Log.WriteLine(e.StackTrace);
                m_isGettingChapters = false;
            }
            try
            {
                var ex = new Regex("vm.Chapters = (?<text>.+);");
                var match = ex.Match(document.ParsedText);
                var raw = match.Groups["text"].Value;

                var chapters = JsonConvert.DeserializeObject<List<ChapterJson>>(raw);

                foreach (var chapter in chapters)
                {
                    var url = $"{m_baseUrl}/read-online/{m_name}-chapter-{chapter.Chapter.Substring(2, 3)}-index-{chapter.Chapter[0]}";
                    m_chapters.Add(new ChapterDownloaderMangaLife(url, "", m_name, m_path));
                }
            }
            catch (Exception e)
            {
                Log.WriteLine(e.Message);
                Log.WriteLine(e.StackTrace);
            }
            finally
            {
                m_isGettingChapters = false;
            }
        }
    }
}
