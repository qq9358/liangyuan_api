using Egoal.Members;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Members
{
    public class UserWechatMap : IEntityTypeConfiguration<UserWechat>
    {
        public void Configure(EntityTypeBuilder<UserWechat> entity)
        {
            entity.Property(e => e.Nickname)
                .HasMaxLength(32);

            entity.Property(e => e.HeadImageUrl)
                .HasMaxLength(256);

            entity.Property(e => e.UnionId)
                .IsUnicode(false)
                .HasMaxLength(48);

            entity.Property(e => e.OffiaccountOpenId)
                .IsUnicode(false)
                .HasMaxLength(48);

            entity.Property(e => e.MiniProgramOpenId)
                .IsUnicode(false)
                .HasMaxLength(48);

            entity.HasOne(e => e.Member)
                .WithOne(e => e.UserWechat)
                .HasForeignKey<UserWechat>(e => e.UserId)
                .IsRequired();

            entity.ToTable("MM_UserWechat");
        }
    }
}
