namespace TomatesBuenos.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string ImageURL { get; set; }
        public string? AudienceRating { get; set; }
        public string? CriticsRating { get; set; } //tomatometer
        public string? AvaliablePlatfomrms { get; set; }
        public string? Synopsis { get; set; }
        public string? Clasification { get; set; }
        public string? Genre { get; set; }
        public string? ReleaseDate { get; set; }
        public string? Duration { get; set; }
        public string? DirectionTeam { get; set; }
        public string? MainActors { get; set; }
        public string? AudienceComments { get; set; }
        public string? CriticsComments { get; set; }


    }
}
