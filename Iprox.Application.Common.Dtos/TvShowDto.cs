using System.ComponentModel.DataAnnotations;

namespace Iprox.Application.Common.Dtos;

public class TvShowDto : BaseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Language { get; set; }
    public DateOnly? Premiered { get; set; }
    public List<GenreDto>? Genres { get; set; }
    public string? Summary { get; set; }
}
