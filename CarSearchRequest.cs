namespace Cars24API.Models
{
    public class CarSearchRequest
    {
        public string? Keyword { get; set; }
        public string? Fuel { get; set; }
        public string? Transmission { get; set; }
        public int? MinYear { get; set; }
        public int? MaxYear { get; set; }
    }
}
