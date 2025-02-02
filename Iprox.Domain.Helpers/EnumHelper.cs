using Iprox.Domain.Entities;
using Iprox.Domain.Enums;

namespace Iprox.Domain.Helpers;

public static class EnumHelper
{
    public static Genre GetGenre(string genreName)
    {
        if (Enum.TryParse<GenreType>(genreName, true, out var genre))
        {
            return new Genre
            {
                Id = (int)genre,
                Name = genre.ToString()
            };
        }

        return new Genre
        {
            Id = (int)GenreType.Other,
            Name = genre.ToString()
        };
    }
}
