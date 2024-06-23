namespace CollegeApp.Data.Repositories
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllAsync();
        Task<Student> GetByIdAsync(int id);
        Task<Student> GetByNameAsync(string name);
        Task<int> Create(Student student);
        Task<int> Update(Student studentIn);
        Task<bool> Delete(Student studentIn);
    }
}
