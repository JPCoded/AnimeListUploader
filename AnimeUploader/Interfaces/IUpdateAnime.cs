namespace AnimeUploader
{
    internal interface IUpdateAnime
    {
        string Aired { get; set; }
        string Duration { get; set; }
        int? Episodes { get; set; }
        int ID { get; set; }
        string Prequel { get; set; }
        int? Rating { get; set; }
        string Sequel { get; set; }
        int? Status { get; set; }
    }
}