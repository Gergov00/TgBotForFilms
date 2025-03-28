using System.Text.Json.Serialization;

namespace Data.Entities;

public class MovieSearchResult
{
    [JsonPropertyName("docs")]
    public IEnumerable<MovieInfoSimplified> Docs { get; set; } = new List<MovieInfoSimplified>();
    [JsonPropertyName("total")]
    public int Total { get; set; }
    [JsonPropertyName("limit")]
    public int Limit { get; set; }
    [JsonPropertyName("page")]
    public int Page { get; set; }
    [JsonPropertyName("pages")]
    public int Pages { get; set; }
}