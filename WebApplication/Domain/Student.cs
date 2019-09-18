using DaceloRex.WebApplication.Domain;
using DaceloRex.WebApplication.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace DaceloRex.WebApplication.Domain
{
   public class Student : Entity, IAuditableEntity, ITenancyEntity, ISoftDeletableEntity
    {
        public string Firstname { get; private set; }

        public string Surname { get; private set; }

        public DateTime Birthdate { get; private set; }
        
        private readonly List<Grade> _grades;

        public IReadOnlyCollection<Grade> Grades => _grades;

        public Student()
        {
            _grades = new List<Grade>();
        }

        public Student(string firstname, string surname, DateTime birthdate) : this()
        {
            Id = Guid.NewGuid();
            
            Firstname = firstname;
            Surname = surname;
            Birthdate = birthdate;
        }

        internal void ChangeFirstname(string firstname)
        {
            Firstname = firstname;
        }

        public void AddNewGrade(int value)
        {
            var grade = new Grade(value);
            _grades.Add(grade);
        }
    }
}
