using Egoal.Scenics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Scenics
{
    public class GroundMap : IEntityTypeConfiguration<Ground>
    {
        public void Configure(EntityTypeBuilder<Ground> entity)
        {
            entity.ToTable("VM_Ground");

            entity.HasIndex(e => e.ParkId);

            entity.Property(e => e.Id)
                .HasColumnName("ID")
                .ValueGeneratedNever();

            entity.Property(e => e.CzkPayMode)
                .HasDefaultValueSql("((1))");

            entity.Property(e => e.DefaultTicketTypeId)
                .HasColumnName("DefaultTicketTypeID");

            entity.Property(e => e.DeptId)
                .HasColumnName("DeptID");

            entity.Property(e => e.GroundPlayTypeId)
                .HasColumnName("GroundPlayTypeID");

            entity.Property(e => e.GroundTypeId)
                .HasColumnName("GroundTypeID");

            entity.Property(e => e.LastGroundId)
                .HasColumnName("LastGroundID");

            entity.Property(e => e.Name)
                .HasMaxLength(50);

            entity.Property(e => e.OnlineCheck)
                .HasDefaultValueSql("((1))");

            entity.Property(e => e.ParkId)
                .HasColumnName("ParkID");

            entity.Property(e => e.SortCode)
                .HasMaxLength(50);

            entity.Property(e => e.StadiumId)
                .HasColumnName("StadiumID");
        }
    }
}
