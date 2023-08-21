using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zadanie5.DTOs;
using Zadanie5.Services;
using Zadanie5.Utils;

namespace Zadanie5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;

        public WarehousesController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpPost]
        public ActionResult AddProduct(ProductDTO product)
        {
            var result = _warehouseService.AddProduct(product);
            if (result.Result < 0)
            {
                var errorBody = new { code = 404, errorBody = _warehouseService.getResponse(result.Result) };
                return BadRequest(errorBody);
            }
            return Ok(result);
        }
    }
}
