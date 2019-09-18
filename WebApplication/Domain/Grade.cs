using DaceloRex.WebApplication.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaceloRex.WebApplication.Domain
{
    public class Grade : Entity, IAuditableEntity, ISoftDeletableEntity, ITenancyEntity
    {
        public int Value { get; private set; }
        public Guid StudentId { get; private set; }

        public Grade(int value)
        {
            Id = Guid.NewGuid();
            Value = value;
        }
    }
}
