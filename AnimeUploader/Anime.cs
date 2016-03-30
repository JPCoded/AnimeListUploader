namespace AnimeUploader
{
    internal class Anime
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
        public int Episodes { get; set; }
        public string Aired { get; set; }
        public string Duration { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }
        public string Sequel { get; set; }
        public string Prequel { get; set; }

        public int GetRating(string rating)
        {
            var ratingNumber = 0;

            switch (rating)
            {
                case "G - All Ages":
                    ratingNumber = 1;
                    break;
                case "PG - Children":
                    ratingNumber = 2;
                    break;
                case "PG-13 - Teens 13 or older":
                    ratingNumber = 3;
                    break;
                case "R - 17+ (violence &amp; profanity)":
                    ratingNumber = 4;
                    break;
                case "R+ - Mild Nudity":
                    ratingNumber = 5;
                    break;
                default:
                    ratingNumber = 6;
                    break;
            }
            return ratingNumber;
        }

        public int GetType(string type)
        {
            var typeNumber = 0;

            switch (type)
            {
                case "TV":
                    typeNumber = 1;
                    break;
                case "OVA":
                    typeNumber = 2;
                    break;
                case "ONA":
                    typeNumber = 3;
                    break;
                case "Special":
                    typeNumber = 4;
                    break;
                case "Movie":
                    typeNumber = 5;
                    break;
            }
            return typeNumber;
        }

        public int GetStatus(string status)
        {
            var statusNumber = 0;

            switch (status)
            {
                case "Not yet aired":
                    statusNumber = 1;
                    break;
                case "Currently Airing":
                    statusNumber = 2;
                    break;
                case "Finished Airing":
                    statusNumber = 3;
                    break;
            }
            return statusNumber;
        }
    }
}