using DaceloRex.WebApplication.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaceloRex.WebApplication.DataAccess
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder ApplyShadowProperties(this ModelBuilder modelBuilder)
        {
            foreach (var type in modelBuilder.Model.GetEntityTypes().Select(x=>x.ClrType))
            {
                if (typeof(IAuditableEntity).IsAssignableFrom(type))
                {
                    modelBuilder.Entity(type).Property<DateTime>("createdDate").IsRequired();
                    modelBuilder.Entity(type).Property<Guid>("createdBy").IsRequired();
                    modelBuilder.Entity(type).Property<DateTime?>("modifiedDate");
                    modelBuilder.Entity(type).Property<Guid?>("modifiedBy");
                }

                if (typeof(ISoftDeletableEntity).IsAssignableFrom(type))
                {
                    modelBuilder.Entity(type).Property<bool>("isDeleted").IsRequired();
                }
            }

            return modelBuilder;
        }
    }
}
