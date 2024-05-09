using CollegeApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<StudentDTO>> GetStudents()
        {

            //var students = new List<StudentDTO>();

            //foreach (Student student in CollegeRepository.Students)
            //{
            //    StudentDTO studentDTO = new StudentDTO() 
            //    {
            //        Id = student.Id,
            //        Name = student.Name,
            //        Address = student.Address,
            //        Email = student.Email
            //    };
            //    students.Add(studentDTO);
            //}
            var students = CollegeRepository.Students.Select(x => new StudentDTO()
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                Email = x.Email
            });
            return Ok(students);
        }

        [HttpGet("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> GetStudentById(int id)
        {
            if (id <= 0)
                return BadRequest();

            var studen = CollegeRepository.Students.FirstOrDefault(s => s.Id == id);
            if (studen == null)
                return NotFound($"{id} not found");

            var studentDTO = new StudentDTO()
            {
                Id = studen.Id,
                Name = studen.Name,
                Address = studen.Address,
                Email = studen.Email
            };

            return Ok(studentDTO);
        }

        [HttpGet("{name:alpha}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> GetStudentByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest();

            var studen = CollegeRepository.Students.FirstOrDefault(s => s.Name.ToLower() == name.ToLower().Trim());
            if (studen == null)
                return NotFound($"{name} not found");
            var studentDTO = new StudentDTO()
            {
                Id = studen.Id,
                Name = studen.Name,
                Address = studen.Address,
                Email = studen.Email
            };
            return Ok(studentDTO);
        }

        [HttpDelete("{id:int:min(1):max(100)}", Name = "DeleteStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<bool> DeleteStudent(int id)
        {
            if (id <= 0)
                return BadRequest();

            var stu = CollegeRepository.Students.FirstOrDefault(s => s.Id == id);

            if (stu == null)
                return NotFound($"{id} not found");

            CollegeRepository.Students.Remove(stu);

            return Ok(true);
        }
    }
}
