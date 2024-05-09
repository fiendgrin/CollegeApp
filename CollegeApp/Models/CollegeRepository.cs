namespace CollegeApp.Models
{
    public static class CollegeRepository
    {
        public static List<Student> Students { get; set; } = new List<Student>() 
        {
             new Student
                {
                    Id = 1,
                    Name = "Yunis",
                    Email = "yunis@mail.ru",
                    Address = "Baku, AZE"

                },
                 new Student
                {
                    Id = 2,
                    Name = "Ali",
                    Email = "ali@mail.ru",
                    Address = "Moscow, RU"

                }
        };
    }
}
