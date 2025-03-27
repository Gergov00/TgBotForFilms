using Data.Entities;

namespace Services.Interfaces;

public interface IMovieService
{
    Task<MovieInfoSimplified> GetRandom();
    Task<MovieInfoSimplified> GetFavourites(int id);
}