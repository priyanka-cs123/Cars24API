using System;
using System.Collections.Generic;

namespace Cars24API.Models
{
    public class Car
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Emi { get; set; }
        public string Fuel { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Location { get; set; } = string.Empty;
        public int Popularity { get; set; } = 0;

        public Specs Specs { get; set; } = new Specs();
        public List<string> Images { get; set; } = new();

        public int Views { get; set; } = 0;
     
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        public int Score { get; set; } = 0;
    }
}
