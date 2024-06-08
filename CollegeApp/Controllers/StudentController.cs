using AutoMapper;
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
        private readonly IMapper _mapper;
        public StudentController(ILogger<StudentController> logger, CollegeDBContext dBContext, IMapper mapper)
        {
            _logger = logger;
            _dBContext = dBContext;
            _mapper = mapper;
        }

        [HttpGet("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudentsAsync()
        {
            _logger.LogInformation($"{nameof(GetStudentsAsync)} method executed");
            var students = await _dBContext.StudentsDbTable.ToListAsync();

            var studentsDTO = _mapper.Map<StudentDTO>(students);

            return Ok(students);
        }

        [HttpGet("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO>> GetStudentById(int id)
        {
            if (id <= 0)
            {
                _logger.LogError($"from {nameof(GetStudentById)} - \"id\" can not be null or negative");
                return BadRequest();
            }


            var students = await _dBContext.StudentsDbTable.FirstOrDefaultAsync(s => s.Id == id);
            if (students == null)
            {
                _logger.LogError($"from {nameof(GetStudentById)} - \"id\" not found");
                return NotFound($"{id} not found");
            }

            var studentDTO = _mapper.Map<StudentDTO>(students);

            return Ok(studentDTO);
        }

        [HttpGet("{name:alpha}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO>> GetStudentByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest();

            var students = await _dBContext.StudentsDbTable.FirstOrDefaultAsync(s => s.Name.ToLower() == name.ToLower().Trim());
            if (students == null)
                return NotFound($"{name} not found");
            var studentDTO = _mapper.Map<StudentDTO>(students);
            return Ok(studentDTO);
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO>> CreateStudent([FromBody] StudentDTO model)
        {
            if (model == null)
                return BadRequest();

            //if (model.AdmissionDate < DateTime.Now)
            //{
            //    ModelState.AddModelError("AdmissionDate Error", "AdmissionDate must be greater than or equal to the present date");
            //    return BadRequest(ModelState);
            //}

            Student student = _mapper.Map<Student>(model);

            await _dBContext.StudentsDbTable.AddAsync(student);
            await _dBContext.SaveChangesAsync();

            model.Id = student.Id;


            return CreatedAtRoute("GetStudentById", new { id = model.Id }, model);

        }

        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateStudent([FromBody] StudentDTO model)
        {
            if (model == null || model.Id <= 0) return BadRequest();
            var existingStudent = await _dBContext.StudentsDbTable.AsNoTracking().Where(s => s.Id == model.Id).FirstOrDefaultAsync();
            if (existingStudent == null) return NotFound();

            var newRecord = _mapper.Map<Student>(model);

            _dBContext.StudentsDbTable.Update(newRecord);

            //existingStudent.Email = model.Email;
            //existingStudent.Name = model.Name;
            //existingStudent.Address = model.Address;
            //existingStudent.DOB = Convert.ToDateTime(model.DOB);

            await _dBContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id:int}/UpdatePartial")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateStudentPartial(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocumnet)
        {
            if (patchDocumnet == null || id <= 0)
                return BadRequest();

            var existingStudent = await _dBContext.StudentsDbTable.Where(s => s.Id == id).FirstOrDefaultAsync();

            if (existingStudent == null)
                return NotFound();

            var studentDTO = _mapper.Map<StudentDTO>(existingStudent);

            patchDocumnet.ApplyTo(studentDTO, ModelState);

            if (!ModelState.IsValid)
                return BadRequest();

            existingStudent = _mapper.Map<Student>(studentDTO);
            _dBContext.Update(existingStudent);

            await _dBContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int:min(1):max(100)}", Name = "DeleteStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteStudent(int id)
        {
            if (id <= 0)
                return BadRequest();

            var stu = await _dBContext.StudentsDbTable.FirstOrDefaultAsync(s => s.Id == id);

            if (stu == null)
                return NotFound($"{id} not found");

            _dBContext.StudentsDbTable.Remove(stu);
            await _dBContext.SaveChangesAsync();

            return Ok(true);
        }
    }
}
