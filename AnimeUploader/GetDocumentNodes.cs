#region

using HtmlAgilityPack;

#endregion

namespace AnimeUploader
{
    internal sealed class GetDocumentNodes
    {
        private readonly HtmlDocument _document;
        private readonly Nodes _nodes;

        public GetDocumentNodes(HtmlDocument documentToParse)
        {
            _document = documentToParse;
            _nodes = JsonLoader.LoadNodeSettings();
        }

        public string getType()
        {
            return _document.DocumentNode.SelectSingleNode(_nodes.Type).InnerText.Replace("Type:", "").Trim();
        }

        public string GetEpisode()
        {
            return _document.DocumentNode.SelectSingleNode(_nodes.Episode).InnerText.Replace("Episodes:", "").Trim();
        }

        public string GetStatus()
        {
            return _document.DocumentNode.SelectSingleNode(_nodes.Status).InnerText.Replace("Status:", "").Trim();
        }

        public string GetAired()
        {
            return _document.DocumentNode.SelectSingleNode(_nodes.Aired).InnerText.Replace("Aired:", "").Trim();
        }

        public string GetDuration()
        {
            return _document.DocumentNode.SelectSingleNode(_nodes.Duration).InnerText.Replace("Duration:", "").Trim();
        }

        public string GetRating()
        {
            return _document.DocumentNode.SelectSingleNode(_nodes.Rating).InnerText.Replace("Rating:", "").Trim();
        }

        public string GetSynopsis()
        {
            return _document.DocumentNode.SelectSingleNode(_nodes.Synopsis)
                .GetAttributeValue("content", "")
                .Replace("'", "''")
                .Trim();
        }

        public string GetGenres()
        {
            return _document.DocumentNode.SelectSingleNode(_nodes.Genres)
                .InnerText.Replace("Genres:", "")
                .Replace(", ", ",")
                .Trim();
        }

        public string GetTitles()
        {
            return _document.DocumentNode.SelectSingleNode(_nodes.Title)
                .GetAttributeValue("content", "")
                .Replace("'", "''")
                .Trim();
        }

        public HtmlNode GetPrequelId()
        {
            return _document.DocumentNode.SelectSingleNode(_nodes.PrequelId);
        }

        public HtmlNode GetPrequel()
        {
            return _document.DocumentNode.SelectSingleNode(_nodes.Prequel);
        }

        public HtmlNode GetSequelId()
        {
            return _document.DocumentNode.SelectSingleNode(_nodes.SequelId);
        }

        public HtmlNode GetSequel()
        {
            return _document.DocumentNode.SelectSingleNode(_nodes.Sequel);
        }
    }
}