using App.Domain.Entities.Process;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Persistence.Configurations.Process
{
    public class InvFundsCustomerSupplierConfiguration : IEntityTypeConfiguration<InvFundsCustomerSupplier>
    {
        public void Configure(EntityTypeBuilder<InvFundsCustomerSupplier> builder)
        {
            builder.ToTable("InvFundsCustomerSupplier");
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.Person)
            //.WithMany(a => a.FundsCustomerSuppliers) //Commented by Muhammad Salem and replaced with the with one statement
            .WithOne(a => a.FundsCustomerSuppliers);
            //.HasForeignKey(p => p.PersonId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
