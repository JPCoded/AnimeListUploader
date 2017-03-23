namespace AnimeUploader
{
    public abstract class Nodes
    {
        public readonly string Type;
        public readonly string Episode;
        public readonly string Status ;
        public readonly string Aired ;
        public readonly string Duration;
        public readonly string Rating;
        public readonly string Synopsis;
        public readonly string Genres;
        public readonly string PrequelId;
        public readonly string Prequel;
        public readonly string SequelId;
        public readonly string Sequel;
        public readonly string Title;

        public Nodes(string type, string episode, string status, string aired, string duration, string rating, string synopsis, string genres, string prequelId, string prequel, string sequelId, string sequelId1, string sequel, string title)
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
            SequelId = sequelId1;
            Sequel = sequel;
            Title = title;
        }
    }
}
