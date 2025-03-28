using System.Text.Json.Serialization;

namespace Data.Entities;

public class MovieInfoSimplified
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("alternativeName")]
    public string? AlternativeName { get; set; }

    [JsonIgnore]
    public string DisplayName => !string.IsNullOrEmpty(Name) ? Name : AlternativeName ?? "Неизвестно";

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("year")]
    public int? Year { get; set; }

    [JsonPropertyName("genres")]
    public List<Genre>? Genres { get; set; }

    [JsonPropertyName("countries")]
    public List<Country>? Countries { get; set; }

    // Добавляем свойство для постера
    [JsonPropertyName("poster")]
    public Poster? Poster { get; set; }
}

public class Genre
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}

public class Country
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

// Класс для постера
public class Poster
{
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("previewUrl")]
    public string? PreviewUrl { get; set; }
}