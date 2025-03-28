using System.Net.Http.Json;
using System.Text.Json;
using System.Web;
using Data.Entities;
using Microsoft.Extensions.Configuration;
using Services.Interfaces;
using Services.Utils;



namespace Services.Implementations;

public class MovieService : IMovieService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    
    public MovieService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["MovieApi:ApiKey"];
        
        _httpClient.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);
    }
    
    public async Task<MovieInfoSimplified> GetRandomByFilter(UserFilter filter)
    {
        var baseUrl = $"https://api.kinopoisk.dev/v1.4/movie/random";
        var url = filter == null ?  baseUrl + "?year=2000-2024" : MovieUrlBuilder.BuildUrl(baseUrl, filter);

        var response = await _httpClient.GetAsync(url);
        
        if (response.IsSuccessStatusCode)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var jsonString = await response.Content.ReadAsStringAsync();

            var movieInfo = JsonSerializer.Deserialize<MovieInfoSimplified>(jsonString, options);
            return movieInfo;
        }
        else
        {
            throw new Exception($"Ошибка запроса к API: {response.StatusCode}");
        }
    }

    public async Task<IEnumerable<MovieInfoSimplified>> GetByTitle(string title)
    {
        var baseUrl = "https://api.kinopoisk.dev/v1.4/movie/search?page=1&limit=10";
        var encodedFilmTitle = HttpUtility.UrlEncode(title); 
        var finalUrl = $"{baseUrl}&query={encodedFilmTitle}";
        

        var response = await _httpClient.GetAsync(finalUrl);
        if (response.IsSuccessStatusCode)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var jsonString = await response.Content.ReadAsStringAsync();

            // Если в теле запроса прямо массив фильмов:
            var movies = JsonSerializer.Deserialize<MovieSearchResult>(jsonString, options);
            return movies.Docs;
        }
        else
        {
            throw new Exception($"Ошибка запроса к API: {response.StatusCode}");
        }

    }

    public async Task<MovieInfoSimplified> GetById(int id)
    {
        var url = $"https://api.kinopoisk.dev/v1.4/movie/{id}";

        var response = await _httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var jsonString = await response.Content.ReadAsStringAsync();

            var movieInfo = JsonSerializer.Deserialize<MovieInfoSimplified>(jsonString, options);
            return movieInfo;
        }
        else
        {
            throw new Exception($"Ошибка запроса к API: {response.StatusCode}");
        }
        
    }

    public Task<MovieInfoSimplified> GetFavourites(int id)
    {
        throw new NotImplementedException();
    }
}
