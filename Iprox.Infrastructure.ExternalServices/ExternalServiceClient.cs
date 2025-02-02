using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Iprox.Application.Common.Dtos;
using Iprox.Application.Common.Interface;
using Newtonsoft.Json;

namespace Iprox.Infrastructure.ExternalServices;

public class ExternalServiceClient : IExternalServiceClient
{
    private readonly HttpClient _httpClient;

    public ExternalServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<TvMazeResponseDto>?> GetTvShowsByPageAsync(int pageNo)
    {
        try
        {
            var url = $"https://api.tvmaze.com/shows?page={pageNo}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var tvShows = JsonConvert.DeserializeObject<List<TvMazeResponseDto>>(content);

            return tvShows;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
