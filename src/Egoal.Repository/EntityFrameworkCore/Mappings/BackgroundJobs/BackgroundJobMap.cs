using Egoal.BackgroundJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.BackgroundJobs
{
    public class BackgroundJobMap : IEntityTypeConfiguration<BackgroundJobInfo>
    {
        public void Configure(EntityTypeBuilder<BackgroundJobInfo> entity)
        {
            entity.Property(p => p.JobType)
                .IsRequired()
                .HasMaxLength(BackgroundJobInfo.MaxJobTypeLength);

            entity.Property(p => p.JobArgs)
                .IsRequired();

            entity.ToTable("BackgroundJob");
        }
    }
}
