using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaceloRex.DataAccess;
using DaceloRex.WebApplication.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly DaceloRexContext dbContext;

        public StudentController(DaceloRexContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentsAsync()
        {
            var students = await dbContext.Students.ToListAsync();
            return Ok(students);
        }

        [HttpGet]
        [Route("adults")]
        public async Task<IActionResult> GetAdultStudentsAsync()
        {
            var students = await dbContext.Students.Where(x => (DateTime.Today.Year - x.Birthdate.Year) > 18)
            .ToListAsync();
            return Ok(students);
        }

        [HttpGet]
        [Route("{id:guid}", Name = "GetStudent")]
        public async Task<IActionResult> GetStudentAsync(Guid id)
        {
            var student = await dbContext.Students.Where(x => x.Id == id)
                .Include(x => x.Grades)
                .SingleOrDefaultAsync();

            if (student == null)
                return NotFound();

            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync()
        {
            var random = new Random((int)DateTime.Now.Ticks);
            var std = new Student("John", "Moore", new DateTime(random.Next(1980, 2010), 2, 10));

            dbContext.Students.Add(std);
            await dbContext.SaveChangesAsync();

            return CreatedAtRoute("GetStudent", new { id = std.Id }, new { std.Id });
        }

        [HttpPatch]
        [Route("{id:guid}")]
        public async Task<IActionResult> ChangeStudentNameAsync(Guid id)
        {
            var student = await dbContext.Students.Where(x => x.Id == id)
                .SingleOrDefaultAsync();
            if (student == null)
                return NotFound();

            student.ChangeFirstname("Jack");

            dbContext.Update(student);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteStudentAsync(Guid id)
        {
            var student = await dbContext.Students.Where(x => x.Id == id)
                .Include(x=>x.Grades)
                .SingleOrDefaultAsync();
            if (student == null)
                return NoContent();

            dbContext.Remove(student);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        [Route("{id:guid}/add-grade")]
        public async Task<IActionResult> AddNewGrade(Guid id)
        {
            var student = await dbContext.Students.Where(x => x.Id == id)
                .Include(x => x.Grades)
                .SingleOrDefaultAsync();
            if (student == null)
                return NotFound();

            var random = new Random((int)DateTime.Now.Ticks);

            student.AddNewGrade(random.Next(5) + 1);

            dbContext.Update(student);
            await dbContext.SaveChangesAsync();

            return CreatedAtRoute("GetStudent", new { id = student.Id }, new { student.Id });
        }

        [HttpGet]
        [Route("{id:guid}/recent-grades")]
        public async Task<IActionResult> GetStudentRecentGradesAsync(Guid id)
        {
            var grades = await dbContext.Grades.Where(x => x.StudentId == id)
                .Where(x=> EF.Property<DateTime>(x, "createdDate") > DateTime.Now.AddDays(-30))
                .ToListAsync();

            var query = from grade in dbContext.Grades
                        where grade.StudentId == id
                        && EF.Property<DateTime>(grade, "createdDate") > DateTime.Now.AddDays(-30)
                        select grade;

            grades = await query.ToListAsync();

            return Ok(grades);
        }
    }
}
