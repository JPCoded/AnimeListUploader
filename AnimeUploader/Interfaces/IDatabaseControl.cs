namespace AnimeUploader
{
    internal interface IDatabaseControl
    {
        void Dispose();
        bool GenreExist(int animeId, int genreId);
        System.Collections.Generic.List<GetAnime> GetAllMyListId();
    }
}