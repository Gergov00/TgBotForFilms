namespace Data.Entities;

public class UserFilter
{
    public List<string> Genres { get; set; } = new();
    public List<string> Countries { get; set; } = new();
    public int? Year { get; set; }

    public bool IsEmpty => !Genres.Any() && !Countries.Any() && Year == null;
}
