using Egoal.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Tickets
{
    public class ReceiveInvoiceEmailDomainBlacklistMap : IEntityTypeConfiguration<ReceiveInvoiceEmailDomainBlacklist>
    {
        public void Configure(EntityTypeBuilder<ReceiveInvoiceEmailDomainBlacklist> entity)
        {
            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.EmailDomain)
                .HasMaxLength(100);

            entity.Property(e => e.SortCode)
                .HasMaxLength(20);

            entity.ToTable("SM_ReceiveInvoiceEmailDomainBlacklist");
        }
    }
}
