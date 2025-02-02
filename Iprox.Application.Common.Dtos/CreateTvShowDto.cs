namespace Iprox.Application.Common.Dtos;

public class CreateTvShowDto
{
    public required string Name { get; set; }
    public string? Language { get; set; }
    public DateOnly? Premiered { get; set; }
    public List<GenreDto>? Genres { get; set; }
    public string? Summary { get; set; }
}
