using Data.Entities;

namespace Services.Interfaces;

public interface IMovieService
{
    Task<MovieInfoSimplified> GetById(int id);
    Task<MovieInfoSimplified> GetFavourites(int id);
    Task<MovieInfoSimplified> GetRandomByFilter(UserFilter filter);
    Task<IEnumerable<MovieInfoSimplified>> GetByTitle(string title);
}