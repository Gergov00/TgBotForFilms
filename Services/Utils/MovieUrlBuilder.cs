using System.Web;
using Data.Entities;

namespace Services.Utils;

public static class MovieUrlBuilder
{
    public static string BuildUrl(string baseUrl, UserFilter filter)
    {
        var uriBuilder = new UriBuilder(baseUrl);
        var query = HttpUtility.ParseQueryString(string.Empty);

        filter.Genres.ForEach(g => query.Add("genres.name", g));
        filter.Countries.ForEach(c => query.Add("countries.name", c));
        if (filter.Year.HasValue)
            query.Add("year", filter.Year.Value.ToString());

        uriBuilder.Query = query.ToString();
        return uriBuilder.ToString();
    }
}