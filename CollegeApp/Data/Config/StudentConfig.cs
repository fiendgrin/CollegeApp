using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeApp.Data.Config
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder) 
        { 
            builder.ToTable(nameof(Student));
            builder.HasKey(x => x.Id);
            builder.Property(x=>x.Id).UseIdentityColumn();
            builder.Property(n => n.Name).IsRequired().HasMaxLength(250);
            builder.Property(n => n.Email).IsRequired().HasMaxLength(500);
            builder.Property(n => n.Address).IsRequired(false).HasMaxLength(250);
            builder.HasData(new List<Student>()
            {
                new Student()
                {
                    Id=1,
                    Name="Yunis",
                    Address="Baku",
                    Email = "yunis@mail.ru",
                    DOB = new DateTime(2024,1,1),
                },
                new Student()
                {
                    Id=2,
                    Name="Ali",
                    Address="Dubai",
                    Email = "ali@mail.ru",
                    DOB = new DateTime(2024,2,3),
                },
            });
        }
    }
}
