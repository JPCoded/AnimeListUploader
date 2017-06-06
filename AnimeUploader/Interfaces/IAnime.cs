namespace AnimeUploader
{
    internal interface IAnime
    {
        int ID { set; }
        string Title { set; }
        int? Type { set; }
        int? Status { set; }
        int? Episodes { set; }
        string Aired { set; }
        string Duration { set; }
        int? Rating { set; }
        string Description { set; }
        int? SequelID { set; }
        string Sequel { set; }
        int? PrequelID { set; }
        string Prequel { set; }
        string Genre { set; }
        int GetRating(string rating);
        int GetType(string type);
        int GetStatus(string status);
    }
}