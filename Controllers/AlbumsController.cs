using Microsoft.AspNetCore.Mvc;
using MusicApi.Dtos;
using MusicApi.Services;

namespace MusicApi.Controllers;

[ApiController]
[Route("api/albums")]
public class AlbumsController(IAlbumService albumService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<List<AlbumDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AlbumDto>>> GetAll() =>
        Ok(await albumService.GetAllAsync());

    [HttpGet("{id:int}")]
    [ProducesResponseType<AlbumDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AlbumDto>> GetById(int id)
    {
        var album = await albumService.GetByIdAsync(id);
        return album is null ? NotFound() : Ok(album);
    }

    [HttpGet("search")]
    [ProducesResponseType<List<AlbumDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<AlbumDto>>> Search([FromQuery] AlbumFilter filter)
    {
        try
        {
            return Ok(await albumService.SearchAsync(filter));
        }
        catch (AlbumValidationException ex)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "Некоректний фільтр", detail: ex.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType<AlbumDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AlbumDto>> Create(CreateAlbumDto dto)
    {
        try
        {
            var album = await albumService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = album.Id }, album);
        }
        catch (AlbumValidationException ex)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "Некоректні дані альбому", detail: ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id) =>
        await albumService.DeleteAsync(id) ? NoContent() : NotFound();
}
