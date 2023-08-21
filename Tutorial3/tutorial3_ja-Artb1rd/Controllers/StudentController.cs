using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Zadanie3.Model;

namespace Zadanie3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStudents()
        {
            return Ok(FileHandler.Read());
        }

        [HttpGet("indexNumber")]
        public IActionResult GetStudentsByIndex(string indexNumber)
        {
            if (FileHandler.ReadByIndex(indexNumber) == null)
            {
                return BadRequest("The indexNumber is invalid");
            }

            return Ok(FileHandler.ReadByIndex(indexNumber));
        }
        // public IActionResult GetResponse(RequestStatus status)
        // {
        //     switch (status)
        //     {
        //         case RequestStatus.ERROR_EXISTS:
        //             return BadRequest("Error");
        //         case RequestStatus.ERROR_NOT_EXISTS:
        //             return BadRequest("Not exists");
        //         case RequestStatus.ERROR_PROVIDED_DATA:
        //             return BadRequest("Error_Provided_Dara");
        //     }
        //
        //     return Ok("Success");
        // }
     
        
        [HttpPost]
        public IActionResult PostStudent(StudentModel student)
        {
            return Ok(FileHandler.Insert(student));
            // return GetResponse(FileHandler.Insert(student));
        }

        [HttpPut("index")]
        public IActionResult PutStudent(string index, StudentModel student)
        {
            return Ok(FileHandler.Update(index, student));
        }

        [HttpDelete("index")]
        public IActionResult DeleteStudent(string index)
        {
            return Ok(FileHandler.Delete(index));
        }
    }
}