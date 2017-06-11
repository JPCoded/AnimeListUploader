namespace AnimeUploader
{
    internal interface IMyAnime
    {
        int AnimeID { get; set; }
        int ID { get; set; }
        int Score { get; set; }
        string Status { get; set; }
        int WatchedEpisodes { get; set; }
    }
}