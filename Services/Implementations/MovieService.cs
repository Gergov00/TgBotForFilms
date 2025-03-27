using System.Net.Http.Json;
using System.Text.Json;
using Data.Entities;
using Microsoft.Extensions.Configuration;
using Services.Interfaces;

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
    
    public async Task<MovieInfoSimplified> GetRandom()
    {
        var url = $"https://api.kinopoisk.dev/v1.4/movie/random";
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
