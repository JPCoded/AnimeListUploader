#region

using HtmlAgilityPack;

#endregion

namespace AnimeUploader
{
    internal class GetDocumentNodes
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

        public string getEpisode()
        {
            return _document.DocumentNode.SelectSingleNode(_nodes.Episode).InnerText.Replace("Episodes:", "").Trim();
        }

        public string getStatus()
        {
            return _document.DocumentNode.SelectSingleNode(_nodes.Status).InnerText.Replace("Status:", "").Trim();
        }

        public string getAired()
        {
            return _document.DocumentNode.SelectSingleNode(_nodes.Aired).InnerText.Replace("Aired:", "").Trim();
        }

        public string getDuration()
        {
            return _document.DocumentNode.SelectSingleNode(_nodes.Duration).InnerText.Replace("Duration:", "").Trim();
        }

        public string getRating()
        {
            return _document.DocumentNode.SelectSingleNode(_nodes.Rating).InnerText.Replace("Rating:", "").Trim();
        }

        public string getSynopsis()
        {
            return _document.DocumentNode.SelectSingleNode(_nodes.Synopsis)
                .GetAttributeValue("content", "")
                .Replace("'", "''")
                .Trim();
        }

        public string getGenres()
        {
            return _document.DocumentNode.SelectSingleNode(_nodes.Genres)
                .InnerText.Replace("Genres:", "")
                .Replace(", ", ",")
                .Trim();
        }

        public string getTitles()
        {
            return _document.DocumentNode.SelectSingleNode(_nodes.Title)
                .GetAttributeValue("content", "")
                .Replace("'", "''")
                .Trim();
        }

        public HtmlNode getPrequelId()
        {
            return _document.DocumentNode.SelectSingleNode(_nodes.PrequelId);
        }

        public HtmlNode getPrequel()
        {
            return _document.DocumentNode.SelectSingleNode(_nodes.Prequel);
        }

        public HtmlNode getSequelId()
        {
            return _document.DocumentNode.SelectSingleNode(_nodes.SequelId);
        }

        public HtmlNode getSequel()
        {
            return _document.DocumentNode.SelectSingleNode(_nodes.Sequel);
        }
    }
}