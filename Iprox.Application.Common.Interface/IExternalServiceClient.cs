using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iprox.Application.Common.Dtos;

namespace Iprox.Application.Common.Interface
{
    public interface IExternalServiceClient
    {
        Task<List<TvMazeResponseDto>?> GetTvShowsByPageAsync(int pageNo);
    }
}
