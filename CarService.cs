using Cars24API.Models;

namespace Cars24API.Services
{
    public class CarService
    {
        private readonly List<Car> cars = new();

        // ✅ Get all cars
        public Task<List<Car>> GetAllAsync()
        {
            return Task.FromResult(cars.ToList());
        }

        // ✅ Get by Id
        public Task<Car?> GetByIdAsync(string id)
        {
            var car = cars.FirstOrDefault(c => c.Id == id);
            return Task.FromResult(car);
        }

        // ✅ Create
        public Task CreateAsync(Car car)
        {
            car.Id = Guid.NewGuid().ToString();
            car.CreatedDate = DateTime.UtcNow;   // For recency ranking
            car.Popularity = 0;                  // Default popularity
            cars.Add(car);
            return Task.CompletedTask;
        }

        // 🔥 ADVANCED SEARCH WITH RANKING
        public Task<List<Car>> AdvancedSearchAsync(
            string? keyword,
            string? fuel,
            string? transmission,
            int? minYear,
            int? maxYear,
            int? minMileage,
            int? maxMileage)
        {
            var query = cars.AsQueryable();

            // Apply Filters
            if (!string.IsNullOrWhiteSpace(fuel))
                query = query.Where(c =>
                    c.Specs.Fuel.Equals(fuel, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(transmission))
                query = query.Where(c =>
                    c.Specs.Transmission.Equals(transmission, StringComparison.OrdinalIgnoreCase));

            if (minYear.HasValue)
                query = query.Where(c => c.Specs.Year >= minYear.Value);

            if (maxYear.HasValue)
                query = query.Where(c => c.Specs.Year <= maxYear.Value);

            if (minMileage.HasValue)
                query = query.Where(c => c.Specs.Mileage >= minMileage.Value);

            if (maxMileage.HasValue)
                query = query.Where(c => c.Specs.Mileage <= maxMileage.Value);

            var results = query.ToList();

            // 🎯 Apply Scoring (Relevance Ranking)
            foreach (var car in results)
            {
                int score = 0;

                // Keyword Match Score
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    if (car.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                        score += 10;

                    // Basic Fuzzy Match
                    if (LevenshteinDistance(car.Title.ToLower(), keyword.ToLower()) <= 2)
                        score += 5;
                }

                // Popularity Score
                score += car.Popularity;

                // Recency Score (new cars get more score)
                score += (int)(DateTime.UtcNow - car.CreatedDate).TotalDays < 30 ? 5 : 0;

                car.Score = score;
            }

            // Sort by Score Descending
            results = results
                .OrderByDescending(c => c.Score)
                .ThenByDescending(c => c.CreatedDate)
                .ToList();

            return Task.FromResult(results);
        }

        // 💡 Auto Suggestions (Improved)
        public Task<List<string>> GetSuggestionsAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return Task.FromResult(new List<string>());

            var suggestions = cars
                .Where(c =>
                    c.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    LevenshteinDistance(c.Title.ToLower(), keyword.ToLower()) <= 2)
                .Select(c => c.Title)
                .Distinct()
                .Take(5)
                .ToList();

            return Task.FromResult(suggestions);
        }

        // 🧠 Fuzzy Matching Logic (Levenshtein Distance)
        private int LevenshteinDistance(string s, string t)
        {
            int[,] d = new int[s.Length + 1, t.Length + 1];

            for (int i = 0; i <= s.Length; i++)
                d[i, 0] = i;

            for (int j = 0; j <= t.Length; j++)
                d[0, j] = j;

            for (int i = 1; i <= s.Length; i++)
            {
                for (int j = 1; j <= t.Length; j++)
                {
                    int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;

                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            return d[s.Length, t.Length];
        }
    }
}