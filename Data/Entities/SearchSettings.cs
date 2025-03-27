namespace Data.Entities
{
    public class SearchSettings
    {
        public List<string> Genres { get; private set; } = new List<string>();
        public int? YearFrom { get; private set; }
        public int? YearTo { get; private set; }
        public string? Country { get; private set; }

        public void ClearGenres() => Genres.Clear();

        public void AddGenre(string genre)
        {
            if (!Genres.Contains(genre, StringComparer.OrdinalIgnoreCase))
                Genres.Add(genre);
        }

        public void RemoveGenre(string genre)
        {
            Genres.RemoveAll(g => string.Equals(g, genre, StringComparison.OrdinalIgnoreCase));
        }

        public void SetYearRange(int? from, int? to)
        {
            YearFrom = from;
            YearTo = to;
        }

        public void SetCountry(string? country)
        {
            Country = country;
        }

        public void ClearCountry() => Country = null;
    }
}