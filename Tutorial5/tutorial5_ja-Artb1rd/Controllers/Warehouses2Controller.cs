using Microsoft.AspNetCore.Mvc;
using Zadanie5.DTOs;
using Zadanie5.Services;


namespace Zadanie5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Warehouses2Controller : Controller
    {
        private readonly IWarehouseService _warehouseService;

        public Warehouses2Controller(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpPost]
        public ActionResult AddProduct(ProductDTO product)
        {
            _warehouseService.Post(product);
            return Ok(_warehouseService.getResultId(product));
        }
    }
}