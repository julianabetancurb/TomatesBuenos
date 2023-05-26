namespace TomatesBuenos.Models
{
    public class TVshow
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string ImageURL { get; set; }
        public string? AudienceRating { get; set; }
        public string? CriticsRating { get; set; }
        public string? AvaliablePlatforms { get; set; }
        public string? Synopsis { get; set; }
        public string? Genre { get; set; }
        public string? Premiere { get; set; }
        public string? Creators { get; set; }
        public string? Starring { get; set; }
    }
}
