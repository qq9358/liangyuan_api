using Egoal.TicketTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.TicketTypes
{
    public class TicketTypeMap : IEntityTypeConfiguration<TicketType>
    {
        public void Configure(EntityTypeBuilder<TicketType> entity)
        {
            entity.Property(e => e.Id)
                .HasColumnName("ID")
                .ValueGeneratedNever();

            entity.Property(e => e.AllowSecondIn)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.ApplyToSystemTypeId)
                .HasColumnName("ApplyToSystemTypeID");

            entity.Property(e => e.AuthorizationTypeId)
                .HasColumnName("AuthorizationTypeID");

            entity.Property(e => e.BookEdate)
                .HasColumnName("BookEDate")
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.BookSdate)
                .HasColumnName("BookSDate")
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.CheckTypeId)
                .HasColumnName("CheckTypeID");

            entity.Property(e => e.CID)
                .HasColumnName("CID");

            entity.Property(e => e.Code)
                .HasMaxLength(50);

            entity.Property(e => e.CTime)
                .HasColumnName("CTime")
                .HasColumnType("datetime");

            entity.Property(e => e.DefaultPayTypeId)
                .HasColumnName("DefaultPayTypeID");

            entity.Property(e => e.DelayTypeId)
                .HasColumnName("DelayTypeID");

            entity.Property(e => e.Etime)
                .HasColumnName("ETime")
                .HasMaxLength(5)
                .IsUnicode(false);

            entity.Property(e => e.ExchangeTicketTypeId)
                .HasColumnName("ExchangeTicketTypeID");

            entity.Property(e => e.FingerBindTypeId)
                .HasColumnName("FingerBindTypeID");

            entity.Property(e => e.GlkGoTypeId)
                .HasColumnName("GlkGoTypeID");

            entity.Property(e => e.GnkGoTypeId)
                .HasColumnName("GnkGoTypeID");

            entity.Property(e => e.GnkTypeId)
                .HasColumnName("GnkTypeID");

            entity.Property(e => e.ImgPath)
                .HasMaxLength(100);

            entity.Property(e => e.LastBookTime)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.LcdId)
                .HasColumnName("LcdID");

            entity.Property(e => e.LcdName)
                .HasMaxLength(8);

            entity.Property(e => e.LedId)
                .HasColumnName("LedID");

            entity.Property(e => e.MID)
                .HasColumnName("MID");

            entity.Property(e => e.MTime)
                .HasColumnName("MTime")
                .HasColumnType("datetime");

            entity.Property(e => e.Name)
                .HasMaxLength(50);

            entity.Property(e => e.FullName)
                .HasMaxLength(50);

            entity.Property(e => e.EnglishFullName)
                .IsUnicode(false)
                .HasMaxLength(200);

            entity.Property(e => e.NeedCertFlag)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.ParkId)
                .HasColumnName("ParkID");

            entity.Property(e => e.PrinterId)
                .HasColumnName("PrinterID");

            entity.Property(e => e.SaleSiteId)
                .HasColumnName("SaleSiteID");

            entity.Property(e => e.SortCode)
                .HasMaxLength(50);

            entity.Property(e => e.StatTicketCheckFlag)
                .HasDefaultValueSql("((1))");

            entity.Property(e => e.Stime)
                .HasColumnName("STime")
                .HasMaxLength(5)
                .IsUnicode(false);

            entity.Property(e => e.SvalidDate)
                .HasColumnName("SValidDate")
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.TheTicketDate)
                .HasMaxLength(50);

            entity.Property(e => e.TicketBindTypeId)
                .HasColumnName("TicketBindTypeID");

            entity.Property(e => e.TicketTypeGoRuleTypeId)
                .HasColumnName("TicketTypeGoRuleTypeID");

            entity.Property(e => e.TicketTypeProjectId)
                .HasColumnName("TicketTypeProjectID");

            entity.Property(e => e.TicketTypeStatTypeId)
                .HasColumnName("TicketTypeStatTypeID");

            entity.Property(e => e.TicketTypeTypeId)
                .HasColumnName("TicketTypeTypeID");

            entity.Property(e => e.Tkid)
                .HasColumnName("TKID");

            entity.Property(e => e.Ttid)
                .HasColumnName("TTID");

            entity.Property(e => e.ValidDate)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.VoiceId)
                .HasColumnName("VoiceID");

            entity.Property(e => e.XsTypeId)
                .HasColumnName("XsTypeID");

            entity.Property(e => e.Zjf)
                .HasMaxLength(50);

            entity.Property(e => e.StatGroupId)
                .HasColumnName("StatGroupID");

            entity.Property(e => e.UsageMethod)
                .HasMaxLength(100);

            entity.ToTable("TM_TicketType");

            entity.HasIndex(e => e.Code)
                .IsUnique();

            entity.HasOne(e => e.TicketTypeDescription)
                .WithOne(d => d.TicketType)
                .HasForeignKey<TicketTypeDescription>(d => d.TicketTypeId);
        }
    }
}
