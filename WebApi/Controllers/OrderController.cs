using LibraryApi.Application.Service;
using LibraryApi.Domain.Request;
using LibraryApi.Domain.Response;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;
    private readonly ILogger<OrderController> _logger;

    public OrderController(ILogger<OrderController> logger, OrderService orderService)
    {
        _orderService = orderService;
        _logger = logger;
    }

    [HttpGet("GetAll")]
    [ProducesResponseType(typeof(IEnumerable<OrderResponse>), 200)]
    public IActionResult GetAll()
    {
        var response = _orderService.GetAllData();

        return Ok(response);
    }

    [HttpGet("Get/{id}")]
    [ProducesResponseType(typeof(OrderResponse), 200)]
    public IActionResult GetById(string id)
    {
        var response =  _orderService.GetDataById(id);

        return Ok(response);
    }

    [HttpPost("Register")]
    [ProducesResponseType(typeof(OrderResponse), 201)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    public IActionResult Register(OrderRequest request)
    {
        string result = string.Empty;
        try
        {
            result = _orderService.Insert(request);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ex.Message));
        }

        return Ok(result);
    }

    [HttpPost("Modify")]
    [ProducesResponseType(typeof(OrderResponse), 201)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    public IActionResult Modify(ModifyOrderRequest request)
    {
        string result = string.Empty;
        try
        {
            result = _orderService.Update(request);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ex.Message));
        }

        return Ok(result);
    }

    [HttpPost("UpdateStatus/{id}")]
    [ProducesResponseType(typeof(OrderResponse), 201)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    public IActionResult UpdateStatus(string id, UpdateStatusOrderRequest request)
    {
        string result = string.Empty;
        try
        {
            result = _orderService.UpdateStatus(id, request);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ex.Message));
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(OrderResponse), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    public IActionResult Remove(string id)
    {
        string result = string.Empty;
        try
        {
            result = _orderService.Delete(id);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ex.Message));
        }

        return Ok(result);
    }

    [HttpPost("SearchTitle/{searchKey}")]
    [ProducesResponseType(typeof(OrderResponse), 201)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    public IActionResult SearchBookByTitle(string searchKey)
    {
        var result = new List<BookOrderResponse>();
        try
        {
            result = _orderService.SearchBookByTitle(searchKey);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ex.Message));
        }

        return Ok(result);
    }
}
