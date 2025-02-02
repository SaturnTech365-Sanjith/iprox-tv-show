using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iprox.Application.Common.Dtos
{
    public class TvMazeResponseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Language { get; set; }
        public string? Premiered { get; set; }
        public List<string>? Genres { get; set; }
        public string? Summary { get; set; }
    }
}
