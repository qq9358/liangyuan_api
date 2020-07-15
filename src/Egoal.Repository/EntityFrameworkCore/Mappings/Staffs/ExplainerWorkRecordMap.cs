using Egoal.Staffs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Staffs
{
    public class ExplainerWorkRecordMap : IEntityTypeConfiguration<ExplainerWorkRecord>
    {
        public void Configure(EntityTypeBuilder<ExplainerWorkRecord> entity)
        {
            entity.Property(p => p.ListNo)
                .IsRequired()
                .HasMaxLength(50);

            entity.ToTable("RM_ExplainerWorkRecord");
        }
    }
}
