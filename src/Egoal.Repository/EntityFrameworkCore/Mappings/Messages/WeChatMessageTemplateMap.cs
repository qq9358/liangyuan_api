using Egoal.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Messages
{
    public class WeChatMessageTemplateMap : IEntityTypeConfiguration<WeChatMessageTemplate>
    {
        public void Configure(EntityTypeBuilder<WeChatMessageTemplate> entity)
        {
            entity.Property(p => p.ShortTemplateId)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(p => p.TemplateId)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.ToTable("SM_WeChatMessageTemplate");
        }
    }
}
