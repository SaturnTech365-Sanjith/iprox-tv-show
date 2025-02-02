using System.ComponentModel.DataAnnotations;

namespace Iprox.Domain.Entities;

public class TvShow : Base
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string? Language { get; set; }

    public DateOnly? Premiered { get; set; }

    public List<Genre>? Genres { get; set; }

    public string? Summary { get; set; }

    public int? TvMazeId { get; set; }
}