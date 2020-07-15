using Egoal.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Settings
{
    public class SettingMap : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> builder)
        {
            builder.Property(p => p.Id)
                .HasMaxLength(100);

            builder.Property(p => p.Value)
                .HasMaxLength(4000);

            builder.Property(p => p.Caption)
                .HasMaxLength(100);

            builder.Property(p => p.Id).HasColumnName("Name");
            builder.ToTable("SM_SysPara");
        }
    }
}
