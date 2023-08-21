using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Zadanie4.DI;
using Zadanie4.DTOs;
using Zadanie4.Utils;

namespace Zadanie4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly List<string> filters = new List<string>(new string[]
            { "name", "idAnimal", "description", "area", "category" });

        private IDBService _dbService;

        public AnimalsController(IDBService idbService)
        {
            _dbService = idbService;
        }

        [HttpGet]
        public ActionResult Get([FromQuery] string param = "name")
        {
            if (!filters.Contains(param))
                return BadRequest("Wrong parameter");
            return Ok(_dbService.GetRequest(param));
        }

        [HttpPost]
        public ActionResult Post(AnimalDTO animal)
        {
            var result = _dbService.PostRequest(animal);
            if (result == RequestStatus.SUCCESS)
                return Ok("Success");
            return BadRequest(_dbService.getResponse(result));
        }

        [HttpDelete]
        public ActionResult Delete(int idAnimal)
        {
            var result = _dbService.DeleteRequest(idAnimal);
            if (result == RequestStatus.SUCCESS)
                return Ok("Success");
            return BadRequest(_dbService.getResponse(result));
        }

        [HttpPut]
        public ActionResult Update(int idAnimal, AnimalDTO animal)
        {
            var result = _dbService.UpdateRequest(idAnimal, animal);
            if (result == RequestStatus.SUCCESS)
                return Ok("Success");
            return BadRequest(_dbService.getResponse(result));
        }
    }
}