using Egoal.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Settings
{
    public class ConstantMap : IEntityTypeConfiguration<Constant>
    {
        public void Configure(EntityTypeBuilder<Constant> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasMaxLength(100)
                .HasColumnName("Name")
                .ValueGeneratedNever();

            entity.Property(e => e.Caption)
                .HasMaxLength(100);

            entity.ToTable("SM_Constant");
        }
    }
}
