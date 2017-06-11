namespace AnimeUploader
{
    internal abstract class Nodes
    {
        public readonly string Aired;
        public readonly string Duration;
        public readonly string Episode;
        public readonly string Genres;
        public readonly string Prequel;
        public readonly string PrequelId;
        public readonly string Rating;
        public readonly string Sequel;
        public readonly string SequelId;
        public readonly string Status;
        public readonly string Synopsis;
        public readonly string Title;
        public readonly string Type;

        protected Nodes(string type, string episode, string status, string aired, string duration, string rating,
            string synopsis, string genres, string prequelId, string prequel, string sequelId,
            string sequel, string title)
        {
            Type = type;
            Episode = episode;
            Status = status;
            Aired = aired;
            Duration = duration;
            Rating = rating;
            Synopsis = synopsis;
            Genres = genres;
            PrequelId = prequelId;
            Prequel = prequel;
            SequelId = sequelId;
            Sequel = sequel;
            Title = title;
        }
    }
}