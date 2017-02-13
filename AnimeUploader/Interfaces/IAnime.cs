

namespace AnimeUploader
{
    interface IAnime
    {
       int ID { get; set; }
        string Title { get; set; }
         int? Type { get; set; }
         int? Status { get; set; }
         int? Episodes { get; set; }
        string Aired { get; set; }
        string Duration { get; set; }
        int? Rating { get; set; }
        string Description { get; set; }
        int? SequelID { get; set; }
        string Sequel { get; set; }
        int? PrequelID { get; set; }
        string Prequel { get; set; }
        string Genre { get; set; }

        int GetRating(string rating);
        int GetType(string type);
        int GetStatus(string status);
    }
}
