using Egoal.Members;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Members
{
    public class MemberGuiderMap : IEntityTypeConfiguration<MemberGuider>
    {
        public void Configure(EntityTypeBuilder<MemberGuider> entity)
        {
            entity.ToTable("MM_MemberGuider");
        }
    }
}
