namespace AnimeUploader
{
    internal sealed class UpdateAnime : IUpdateAnime
    {
        public int ID { get; set; }
        public int? Status { get; set; }
        public int? Episodes { get; set; }
        public string Aired { get; set; }
        public string Duration { get; set; }
        public int? Rating { get; set; }
        public string Sequel { get; set; }
        public string Prequel { get; set; }
    }
}