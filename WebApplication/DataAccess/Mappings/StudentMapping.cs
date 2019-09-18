using System;
using DaceloRex.WebApplication.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DaceloRex.WebApplication.DataAccess.Mappings
{
    public class StudentMapping : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable(name: "student");
            builder.Property<Guid>(nameof(Student.Id)).HasColumnName("studentId");
            builder.Property<Guid>(nameof(Student.TenantId)).HasColumnName("tenantId");
            builder.HasKey(x => new { x.Id, x.TenantId });

            builder.Property<string>(nameof(Student.Firstname)).HasColumnName("firstname").HasMaxLength(250).IsRequired();
            builder.Property<string>(nameof(Student.Surname)).HasColumnName("surname").HasMaxLength(250).IsRequired();
            builder.Property<DateTime>(nameof(Student.Birthdate)).HasColumnName("birthdate").IsRequired();


            builder.HasMany<Grade>(x=>x.Grades)
            .WithOne()
            .HasForeignKey(g => new { g.StudentId, g.TenantId })
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
