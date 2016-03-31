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
                case MyStatus.Completed:
                    myStatus = 1;
                    break;
                case MyStatus.Watching:
                    myStatus = 2;
                    break;
                case MyStatus.PlanToWatch:
                    myStatus = 3;
                    break;
                case MyStatus.OnHold:
                    myStatus = 4;
                    break;
            }
            return myStatus;
        }

        }

    internal class MyStatus
    {
        public const string Completed = "Completed";
        public const string Watching = "Watching";
        public const string PlanToWatch = "Plan to Watch";
        public const string OnHold = "On-Hold";
    }
}