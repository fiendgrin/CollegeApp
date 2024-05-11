using CollegeApp.Models;
using Microsoft.AspNetCore.JsonPatch;
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

            int newId = CollegeRepository.Students.LastOrDefault().Id + 1;

            Student student = new Student
            {
                Id = newId,
                Name = model.Name,
                Address = model.Address,
                Email = model.Email
            };

            CollegeRepository.Students.Add(student);

            model.Id = newId;

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
            var existingStudent = CollegeRepository.Students.Where(s => s.Id == model.Id).FirstOrDefault();
            if (existingStudent == null) return NotFound();

            existingStudent.Email = model.Email;
            existingStudent.Name = model.Name;
            existingStudent.Address = model.Address;
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

            var existingStudent = CollegeRepository.Students.Where(s => s.Id == id).FirstOrDefault();

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

            var stu = CollegeRepository.Students.FirstOrDefault(s => s.Id == id);

            if (stu == null)
                return NotFound($"{id} not found");

            CollegeRepository.Students.Remove(stu);

            return Ok(true);
        }
    }
}
