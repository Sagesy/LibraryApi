using System;
using LibraryApi.Application.Service;
using LibraryApi.Domain.Request;
using LibraryApi.Domain.Response;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private readonly BookService _bookService;
    private readonly ILogger<BookController> _logger;

    public BookController(ILogger<BookController> logger, BookService bookService)
    {
        _bookService = bookService;
        _logger = logger;
    }

    [HttpGet("GetAll")]
    [ProducesResponseType(typeof(IEnumerable<BookResponse>), 200)]
    public IActionResult GetAll()
    {
        _logger.LogInformation($"Execute Book/GetAll at {DateTime.UtcNow}");

        var response = _bookService.GetAllData();

        return Ok(response);
    }

    [HttpGet("Get/{id}")]
    [ProducesResponseType(typeof(BookResponse), 200)]
    public IActionResult GetById(string id)
    {
        _logger.LogInformation($"Execute Book/GetAll at {DateTime.UtcNow}");

        var response =  _bookService.GetDataById(id);

        return Ok(response);
    }

    [HttpPost("Register")]
    [ProducesResponseType(typeof(BookResponse), 201)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    public IActionResult Register([FromForm] BookRequest request)
    {
        _logger.LogInformation($"Execute /Book/GetAll at {DateTime.UtcNow}");

        string result = string.Empty;
        try
        {
            result = _bookService.Insert(request);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ex.Message));
        }

        return Ok(result);
    }

    [HttpPost("Modify")]
    [ProducesResponseType(typeof(BookResponse), 201)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    public IActionResult Modify([FromForm] ModifyBookRequest request)
    {
        _logger.LogInformation($"Execute /Book/GetAll at {DateTime.UtcNow}");
        string result = string.Empty;
        try
        {
            result = _bookService.Update(request);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ex.Message));
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(BookResponse), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    public IActionResult Remove(string id)
    {
        _logger.LogInformation($"Execute /Book/GetAll at {DateTime.UtcNow}");

        string result = string.Empty;
        try
        {
            result = _bookService.Delete(id);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ex.Message));
        }

        return Ok(result);
    }
}
