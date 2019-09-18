using DaceloRex.WebApplication.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaceloRex.WebApplication.DataAccess.Mappings
{
    public class GradeMapping : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.ToTable(name: "grade");
            builder.Property<Guid>(nameof(Grade.Id)).HasColumnName("gradeId");
            builder.Property<Guid>(nameof(Grade.TenantId)).HasColumnName("tenantId");
            builder.HasKey(x => new { x.Id, x.TenantId });

            builder.Property<int>(nameof(Grade.Value)).HasColumnName("value").IsRequired();

        }
    }
}
