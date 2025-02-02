using Iprox.Application.Common.Dtos;
using Iprox.Application.TvShowsApi.Interfaces;

namespace Iprox.Presentation.TvShows.Minimal.Api.Endpoints.V1.V0;

public static class ShowEndpoint
{
    private const string ShowTag = "Show";

    public static void MapShowEndpointV1V0(this IEndpointRouteBuilder app)
    {
        var apiGroupV1 = app.MapGroup("api/v1.0")
                            .DisableAntiforgery()
                            .WithOpenApi()
                            .WithTags(ShowTag)
                            .WithGroupName("v1.0");

        var docsGroupV1 = apiGroupV1.MapGroup("Show");

        // GET: api/v1.0/Show
        docsGroupV1.MapGet("", async (int? page, int? pageSize, string? search, string? sortBy, bool? descending, IShowApiService showApiService) =>
        {
            try
            {
                if (page.HasValue || pageSize.HasValue || !string.IsNullOrWhiteSpace(search) || !string.IsNullOrWhiteSpace(sortBy) || descending == true)
                {
                    var pagedResult = await showApiService.GetGridDataAsync(page, pageSize, search, sortBy, descending);
                    return Results.Ok(pagedResult);
                }
                else
                {
                    var tvShows = await showApiService.GetAllAsync();
                    return Results.Ok(tvShows);
                }
            }
            catch (Exception)
            {
                return Results.Problem("An error occurred while fetching TV shows.", statusCode: 500);
            }
        });

        // GET: api/v1.0/Show/{id}
        docsGroupV1.MapGet("{id}", async (int id, IShowApiService showApiService) =>
        {
            try
            {
                var tvShow = await showApiService.GetByIdAsync(id);
                return tvShow is not null ? Results.Ok(tvShow) : Results.NotFound($"TV Show with ID {id} not found.");
            }
            catch (Exception)
            {
                return Results.Problem("An error occurred while fetching the TV show.", statusCode: 500);
            }
        });

        // POST: api/v1.0/Show
        docsGroupV1.MapPost("", async (CreateTvShowDto tvShowDto, IShowApiService showApiService) =>
        {
            try
            {
                if (tvShowDto is null)
                    return Results.BadRequest("TV show data is required.");

                var createdTvShow = await showApiService.CreateAsync(tvShowDto);
                return Results.Created($"api/v1.0/Show/{createdTvShow?.Id}", createdTvShow);
            }
            catch (Exception)
            {
                return Results.Problem("An error occurred while creating the TV show.", statusCode: 500);
            }
        });

        // PUT: api/v1.0/Show/{id}
        docsGroupV1.MapPut("{id}", async (int id, TvShowDto tvShowDto, IShowApiService showApiService) =>
        {
            try
            {
                if (tvShowDto is null)
                    return Results.BadRequest("TV show data is required.");

                var updated = await showApiService.UpdateAsync(id, tvShowDto);
                return updated ? Results.NoContent() : Results.NotFound($"TV Show with ID {id} not found.");
            }
            catch (Exception)
            {
                return Results.Problem("An error occurred while updating the TV show.", statusCode: 500);
            }
        });

        // PATCH: api/v1.0/Show/{id}
        docsGroupV1.MapPatch("{id}", async (int id, PatchTvShowDto patchDto, IShowApiService showApiService) =>
        {
            try
            {
                if (patchDto is null)
                    return Results.BadRequest("Patch data is required.");

                var patched = await showApiService.PatchAsync(id, patchDto);
                return patched ? Results.NoContent() : Results.NotFound($"TV Show with ID {id} not found.");
            }
            catch (Exception)
            {
                return Results.Problem("An error occurred while applying the patch.", statusCode: 500);
            }
        });

        // DELETE: api/v1.0/Show/{id}
        docsGroupV1.MapDelete("{id}", async (int id, IShowApiService showApiService) =>
        {
            try
            {
                var deleted = await showApiService.DeleteAsync(id);
                return deleted ? Results.NoContent() : Results.NotFound($"TV Show with ID {id} not found.");
            }
            catch (Exception)
            {
                return Results.Problem("An error occurred while deleting the TV show.", statusCode: 500);
            }
        });
    }
}
