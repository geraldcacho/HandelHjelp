using HandleHjelp.Data.Models;
using HandleHjelp.Helpers;
using HandleHjelp.Services;
using HandleHjelp.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace HandleHjelp.Controllers;

[Route("api/orders")]
[ApiController]
[Authorize]
public class OrderApiController(ILogger<OrderApiController> logger, DataServices database) : ControllerBase
{
    private readonly DataServices _database = database;
    private readonly ILogger<OrderApiController> _logger = logger;

    // GET: api/orders
    //[HttpGet()]
    //public async Task<ActionResult<List<ListStoreModel>>> GetOrdersAsync()
    //{
    //    var stores = await _database.GetStoresAsync();

    //    return Ok(stores);
    //}

    // GET: api/orders/supportedtypes
    [HttpGet("supportedtypes")]
    [AllowAnonymous]
    public async Task<ActionResult<List<SupportedOrderType>>> GetFeaturesAsync()
    {
        //var tags = await _database.GetFeaturesAsync();

        var categories = await _database.GetSupportedOrderTypeAsync();

        return Ok(categories);
    }

    // GET api/orders/search?query={query}&limit={limit}&country={country}&tags={tag1,tag2,tag3}&latitude={latitude}&longitude={longitude}&rangeInKm={rangeInKm}
    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<ActionResult<List<OrderWithDistance>>> GetOrdersBySearchAsync(
        [FromQuery] string? query,
        [FromQuery] int? limit,
        [FromQuery] string? country,
        [FromQuery] string? tags,
        [FromQuery] double? latitude,
        [FromQuery] double? longitude,
        [FromQuery] double? rangeInKm)
    {
        if (!string.IsNullOrEmpty(query))
        {
            if (query.Length < 3 || query.Length > 64)
            {
                return BadRequest("Invalid input value for query parameter.");
            }
        }

        if (!string.IsNullOrEmpty(country))
        {
            if (country.Length != 2)
            {
                return BadRequest("Invalid input value for country parameter.");
            }
        }

        if (longitude.HasValue && latitude.HasValue && rangeInKm.HasValue)
        {
            if (!GeospatialHelper.IsValidLatitude(latitude.Value) || !GeospatialHelper.IsValidLongitude(longitude.Value) || rangeInKm.Value <= 0 || rangeInKm.Value > 100)
            {
                return BadRequest("Invalid latitude or longitude values or range is invalid.");
            }
        }

        //var stores = await _database.GetStoresBySearchAsync(query, limit, country, tags, latitude, longitude, rangeInKm);
        var orders = await _database.GetOrdersAsync(query, limit, country, tags, latitude, longitude, rangeInKm); ;

        return Ok(orders);
    }

    // GET api/orders/defaultlocation/{id}
    [HttpGet("defaultlocation")]
    [Authorize]
    public async Task<IActionResult> GetCountryDefaultLocation(string id)
    {
        if (string.IsNullOrWhiteSpace(id) || id.Length > 64)
        {
            return BadRequest("Invalid input value.");
        }

        var country = await _database.GetCountryByIdAsync(id);

        return Ok(country);
    }

    // GET api/orders/{id}
    //[HttpGet("{id}")]
    //[AllowAnonymous]
    //public async Task<IActionResult> GetOrderByIdAsync(string id)
    //{
    //    if (string.IsNullOrWhiteSpace(id) || id.Length > 64)
    //    {
    //        return BadRequest("Invalid input value.");
    //    }

    //    var store = await _database.GetStoreByIdAsync(id);

    //    if (store != null)
    //    {
    //        return Ok(store);
    //    }

    //    return NotFound();
    //}

    // DELETE: api/orders/{id}
    //[HttpDelete("{id}")]
    //public async Task<ActionResult> DeleteOrderAsync(string id)
    //{
    //    if (string.IsNullOrWhiteSpace(id) || id.Length > 64)
    //    {
    //        return BadRequest("Invalid input value.");
    //    }

    //    var store = await _database.GetStoreByIdAsync(id);

    //    if (store == null)
    //    {
    //        return NotFound();
    //    }

    //    try
    //    {
    //        await _database.DeleteStoreAsync(store);

    //        return Ok();
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}

    // Create a new order
    // POST: api/orders/
    [HttpPost(Name ="create")]
    public async Task<ActionResult> CreateOrderAsync([FromBody] Order order)
    {
        try
        {
            await _database.UpdateOrderAsync(order);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Update an existing store
    // PUT: api/stores/
    //[HttpPut()]
    //public async Task<ActionResult> UpdateOrderAsync([FromBody] Store store)
    //{
    //    try
    //    {
    //        await _database.UpdateStoreAsync(store);

    //        return Ok();
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}
}