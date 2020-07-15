using Egoal.Scenics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Scenics
{
    public class GateMap : IEntityTypeConfiguration<Gate>
    {
        public void Configure(EntityTypeBuilder<Gate> entity)
        {
            entity.ToTable("VM_Gate");

            entity.HasIndex(e => e.GateGroupId);

            entity.Property(e => e.Id)
                .HasColumnName("ID")
                .ValueGeneratedNever();

            entity.Property(e => e.DeviceId)
                .HasColumnName("DeviceID")
                .HasMaxLength(50);

            entity.Property(e => e.GateGroupId)
                .HasColumnName("GateGroupID");

            entity.Property(e => e.GateTypeId)
                .HasColumnName("GateTypeID");

            entity.Property(e => e.GroundId)
                .HasColumnName("GroundID");

            entity.Property(e => e.IdentifyCode)
                .HasMaxLength(50);

            entity.Property(e => e.LastStatusUpdateTime)
                .HasColumnType("datetime");

            entity.Property(e => e.MachineId)
                .HasColumnName("MachineID");

            entity.Property(e => e.Name)
                .HasMaxLength(50);

            entity.Property(e => e.Pcid)
                .HasColumnName("PCID");

            entity.Property(e => e.PdaTypeId)
                .HasColumnName("PdaTypeID");

            entity.Property(e => e.SortCode)
                .HasMaxLength(50);

            entity.Property(e => e.SpPort)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.TcpGateway)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.TcpIp)
                .HasColumnName("TcpIP")
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.TcpMac)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.TcpMask)
                .HasMaxLength(50)
                .IsUnicode(false);
        }
    }
}
