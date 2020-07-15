using Egoal.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Common
{
    public class ApiLogMap : IEntityTypeConfiguration<ApiLog>
    {
        public void Configure(EntityTypeBuilder<ApiLog> entity)
        {
            entity.ToTable("SM_ApiLog");

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.ClientIpAddress)
                .HasMaxLength(64);

            entity.Property(e => e.ClientName)
                .HasMaxLength(128);

            entity.Property(e => e.Ctime)
                .HasColumnName("CTime")
                .HasColumnType("datetime");

            entity.Property(e => e.RequestContent)
                .IsRequired();

            entity.Property(e => e.ResponseContent)
                .IsRequired();

            entity.Property(e => e.ServiceName)
                .IsRequired()
                .HasMaxLength(256);
        }
    }
}
