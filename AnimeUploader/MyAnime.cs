namespace AnimeUploader
{
    internal class MyAnime
    {
        public int ID { get; set; }
        public int Status { get; set; }
        public int Episodes { get; set; }
        public int Score { get; set; }
        public int AnimeID { get; set; }

        public int GetStatus(string status)
        {
            var myStatus = 0;
            switch (status)
            {
                case "Completed":
                    myStatus = 1;
                    break;
                case "Watching":
                    myStatus = 2;
                    break;
                case "Plan to Watch":
                    myStatus = 3;
                    break;
                default:
                    myStatus = 4;
                    break;
            }
            return myStatus;
        }
    }
}