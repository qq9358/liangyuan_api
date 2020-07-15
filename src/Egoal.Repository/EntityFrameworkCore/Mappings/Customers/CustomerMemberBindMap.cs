using Egoal.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Customers
{
    public class CustomerMemberBindMap : IEntityTypeConfiguration<CustomerMemberBind>
    {
        public void Configure(EntityTypeBuilder<CustomerMemberBind> entity)
        {
            entity.ToTable("CM_CustomerMemberBind");
        }
    }
}
