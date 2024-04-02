
using App.Domain.Entities.Process;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Org.BouncyCastle.Crypto.Tls;

namespace App.Infrastructure.Persistence.Configurations.Process
{
    public class EmployeesConfigurations : IEntityTypeConfiguration<InvEmployees>
    {
        public EmployeesConfigurations()
        {
            //this.HasOptional(emp => emp.ApplicationUser)
            //    .WithOptionalPrincipal(user => user.Employee);
        }
        public void Configure(EntityTypeBuilder<InvEmployees> builder)
        {
            builder.ToTable("InvEmployees");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.ArabicName).IsRequired();
            //builder.HasOne(e => e.branch)
            //       .WithMany(a => a.employees)
            //       .HasForeignKey(p => p.branch_Id).OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(a => a.Code).IsUnique();
            builder.Ignore(e => e.Image);

            builder.Property(c=> c.gLBranchId).HasDefaultValue(1).IsRequired();
            builder.Property(c => c.JobId).IsRequired(false);

            builder.HasOne(e => e.Job).WithMany(a => a.Employees).HasForeignKey(p => p.JobId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(c => c.GLBranch).WithMany(c => c.Employees).HasForeignKey(c => c.gLBranchId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(c => c.Sections).WithMany(c => c.invEmployeesSection).HasForeignKey(c => c.SectionsId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(c => c.Departments).WithMany(c => c.invEmployeesDepartment).HasForeignKey(c => c.DepartmentsId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(c => c.missions).WithMany(c => c.employees).HasForeignKey(c => c.missionsId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(c => c.projects).WithMany(c => c.InvEmployees).HasForeignKey(c => c.projectsId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(c => c.employeesGroup).WithMany(c => c.InvEmployees).HasForeignKey(c => c.employeesGroupId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(c => c.shiftsMaster).WithMany(c => c.InvEmployees).HasForeignKey(c => c.shiftsMasterId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(c => c.FirstLogmachine).WithMany(c => c.InvEmployees).HasForeignKey(c => c.FirstLogmachineId).OnDelete(DeleteBehavior.NoAction);
           
        }
    }
}
