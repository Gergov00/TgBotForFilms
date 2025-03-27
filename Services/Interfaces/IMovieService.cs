using Data.Entities;

namespace Services.Interfaces;

public interface IMovieService
{
    Task<MovieInfoSimplified> GetFavourites(int id);
    Task<MovieInfoSimplified> GetRandomByFilter(UserFilter filter);
}