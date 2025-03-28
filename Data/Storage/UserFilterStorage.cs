using Data.Entities;

namespace Data.Sorage;

public class UserFilterStorage
{
    public Dictionary<long, UserFilter> Filters = new();
}
