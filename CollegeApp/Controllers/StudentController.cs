using CollegeApp.Data;
using CollegeApp.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly CollegeDBContext _dBContext;
        public StudentController(ILogger<StudentController> logger, CollegeDBContext dBContext)
        {
            _logger = logger;
            _dBContext = dBContext;
        }

        [HttpGet("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<StudentDTO>> GetStudents()
        {
            _logger.LogInformation($"{nameof(GetStudents)} method executed");
            //var students = _dBContext.StudentsDbTable.ToList();
            var students = _dBContext.StudentsDbTable.Select(x => new StudentDTO()
            {
                Name = x.Name,
                Address = x.Address,
                DOB = x.DOB.ToShortDateString(),
                Email = x.Email,
                Id = x.Id,
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
            {
                _logger.LogError($"from {nameof(GetStudentById)} - \"id\" can not be null or negative");
                return BadRequest();
            }


            var studen = _dBContext.StudentsDbTable.FirstOrDefault(s => s.Id == id);
            if (studen == null)
            {
                _logger.LogError($"from {nameof(GetStudentById)} - \"id\" not found");
                return NotFound($"{id} not found");
            }

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

            var studen = _dBContext.StudentsDbTable.FirstOrDefault(s => s.Name.ToLower() == name.ToLower().Trim());
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

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> CreateStudent([FromBody] StudentDTO model)
        {
            if (model == null)
                return BadRequest();

            //if (model.AdmissionDate < DateTime.Now)
            //{
            //    ModelState.AddModelError("AdmissionDate Error", "AdmissionDate must be greater than or equal to the present date");
            //    return BadRequest(ModelState);
            //}

            Student student = new Student
            {
                Name = model.Name,
                Address = model.Address,
                Email = model.Email
            };

            _dBContext.StudentsDbTable.Add(student);
            _dBContext.SaveChanges();

            model.Id = student.Id;


            return CreatedAtRoute("GetStudentById", new { id = model.Id }, model);

        }

        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateStudent([FromBody] StudentDTO model)
        {
            if (model == null || model.Id <= 0) return BadRequest();
            var existingStudent = _dBContext.StudentsDbTable.Where(s => s.Id == model.Id).FirstOrDefault();
            if (existingStudent == null) return NotFound();

            existingStudent.Email = model.Email;
            existingStudent.Name = model.Name;
            existingStudent.Address = model.Address;
            existingStudent.DOB = Convert.ToDateTime(model.DOB);
            _dBContext.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id:int}/UpdatePartial")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateStudentPartial(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocumnet)
        {
            if (patchDocumnet == null || id <= 0)
                return BadRequest();

            var existingStudent = _dBContext.StudentsDbTable.Where(s => s.Id == id).FirstOrDefault();

            if (existingStudent == null)
                return NotFound();

            var studentDTO = new StudentDTO()
            {
                Id = existingStudent.Id,
                Email = existingStudent.Email,
                Name = existingStudent.Name,
                Address = existingStudent.Address
            };

            patchDocumnet.ApplyTo(studentDTO, ModelState);

            if (!ModelState.IsValid)
                return BadRequest();

            existingStudent.Name = studentDTO.Name;
            existingStudent.Address = studentDTO.Address;
            existingStudent.Email = studentDTO.Email;
            _dBContext.SaveChanges();

            return NoContent();
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

            var stu = _dBContext.StudentsDbTable.FirstOrDefault(s => s.Id == id);

            if (stu == null)
                return NotFound($"{id} not found");

            _dBContext.StudentsDbTable.Remove(stu);
            _dBContext.SaveChanges();

            return Ok(true);
        }
    }
}
