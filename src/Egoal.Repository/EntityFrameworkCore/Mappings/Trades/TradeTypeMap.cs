using Egoal.Trades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Trades
{
    public class TradeTypeMap : IEntityTypeConfiguration<TradeType>
    {
        public void Configure(EntityTypeBuilder<TradeType> entity)
        {
            entity.ToTable("TM_TradeType");

            entity.Property(e => e.Id)
                .HasColumnName("ID")
                .ValueGeneratedNever();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.PrintInvoiceFlag)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.SaleFlag)
                .HasDefaultValueSql("((1))");

            entity.Property(e => e.SortCode)
                .HasMaxLength(50);

            entity.Property(e => e.StatFlag)
                .HasDefaultValueSql("((1))");

            entity.Property(e => e.TicPrice)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.TradeTypeTypeId)
                .HasColumnName("TradeTypeTypeID");
        }
    }
}
