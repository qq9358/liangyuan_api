using Egoal.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Tickets
{
    public class InvoiceInfoMap : IEntityTypeConfiguration<InvoiceInfo>
    {
        public void Configure(EntityTypeBuilder<InvoiceInfo> entity)
        {
            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.ListNo)
                .IsUnicode(false)
                .HasMaxLength(50);

            entity.Property(e => e.GMFType)
                .HasColumnName("Style");

            entity.Property(e => e.FPDM)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);

            entity.Property(e => e.FPHM)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);

            entity.Property(e => e.KPR)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(20);

            entity.Property(e => e.GMF_MC)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.GMF_NSRSBH)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);

            entity.Property(e => e.GMF_DZDH)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);

            entity.Property(e => e.GMF_YHZH)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);

            entity.Property(e => e.GMF_Email)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);

            entity.Property(e => e.Channel)
                .HasColumnName("ChannelID");

            entity.Property(e => e.CreateTime)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);

            entity.ToTable("TM_Invoice");
        }
    }
}
