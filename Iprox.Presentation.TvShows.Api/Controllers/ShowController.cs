using Iprox.Application.Common.Dtos;
using Iprox.Application.TvShowsApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Iprox.Presentation.TvShows.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShowController : ControllerBase
{
    private readonly IShowApiService _showApiService;

    public ShowController(IShowApiService showApiService)
    {
        _showApiService = showApiService;
    }

    // GET: api/<ShowController>
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] int? page = null,
        [FromQuery] int? pageSize = null,
        [FromQuery] string? search = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool? descending = null)
    {
        try
        {
            if (page.HasValue || pageSize.HasValue || !string.IsNullOrWhiteSpace(search) || !string.IsNullOrWhiteSpace(sortBy) || descending == true)
            {
                var pagedResult = await _showApiService.GetGridDataAsync(page, pageSize, search, sortBy, descending);
                return Ok(pagedResult);
            }
            else
            {
                IEnumerable<TvShowDto> tvShows = await _showApiService.GetAllAsync();
                return Ok(tvShows);
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while fetching TV shows.");
        }
    }

    // GET api/<ShowController>/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            TvShowDto? tvShow = await _showApiService.GetByIdAsync(id);
            if (tvShow == null)
            {
                return NotFound($"TV Show with ID {id} not found.");
            }
            return Ok(tvShow);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while fetching the TV show.");
        }
    }

    // POST api/<ShowController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateTvShowDto tvShowDto)
    {
        try
        {
            if (tvShowDto == null)
            {
                return BadRequest("TV show data is required.");
            }

            TvShowDto? createdTvShow = await _showApiService.CreateAsync(tvShowDto);
            return CreatedAtAction(nameof(Get), new { id = createdTvShow?.Id }, createdTvShow);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while creating the TV show.");
        }
    }

    // PUT api/<ShowController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] TvShowDto tvShowDto)
    {
        try
        {
            if (tvShowDto == null)
            {
                return BadRequest("TV show data is required.");
            }

            bool updated = await _showApiService.UpdateAsync(id, tvShowDto);
            if (!updated)
            {
                return NotFound($"TV Show with ID {id} not found.");
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while updating the TV show.");
        }
    }

    // PATCH api/<ShowController>/5
    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(int id, [FromBody] PatchTvShowDto patchDto)
    {
        try
        {
            if (patchDto == null)
            {
                return BadRequest("Patch data is required.");
            }

            bool patched = await _showApiService.PatchAsync(id, patchDto);
            if (!patched)
            {
                return NotFound($"TV Show with ID {id} not found.");
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while applying the patch.");
        }
    }

    // DELETE api/<ShowController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            bool deleted = await _showApiService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound($"TV Show with ID {id} not found.");
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while deleting the TV show.");
        }
    }
}
