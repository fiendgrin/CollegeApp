
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data.Repositories
{
    public class StudentReposiory : IStudentRepository
    {
        private readonly CollegeDBContext _dBContext;

        public StudentReposiory(CollegeDBContext dBContext)
        {
            _dBContext = dBContext;

        }
        public async Task<int> Create(Student student)
        {
            _dBContext.StudentsDbTable.Add(student);
            await _dBContext.SaveChangesAsync();
            return student.Id;
        }

        public async Task<bool> Delete(Student studentIn)
        {
            var studToDelete = await _dBContext.StudentsDbTable.FirstOrDefaultAsync(student => student.Id == studentIn.Id);

            if (studToDelete != null)
            {
                throw new ArgumentNullException(nameof(studToDelete));
            }

            _dBContext.StudentsDbTable.Remove(studToDelete);
            await _dBContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Student>> GetAllAsync()
        {
           return await _dBContext.StudentsDbTable.ToListAsync();
        }

        public async Task<Student> GetByIdAsync(int id)
        {
            return await _dBContext.StudentsDbTable.FirstOrDefaultAsync(student => student.Id == id);

        }

        public async Task<Student> GetByNameAsync(string name)
        {
            return await _dBContext.StudentsDbTable.FirstOrDefaultAsync(student => student.Name.ToLower().Equals(name.ToLower()));
        }

        public async Task<int> Update(Student studentIn)
        {
            var studToUp = await _dBContext.StudentsDbTable.FirstOrDefaultAsync(student => student.Id == studentIn.Id);

            if (studToUp != null)
            {
               throw new ArgumentNullException(nameof(studToUp));
            }
            studToUp.Name = studentIn.Name;
            studToUp.Email = studentIn.Email;
            studToUp.Address = studentIn.Address;
            studToUp.Id = studentIn.Id;
            studToUp.DOB = studentIn.DOB;

            await _dBContext.SaveChangesAsync();
            return studentIn.Id;
        }
    }
}
