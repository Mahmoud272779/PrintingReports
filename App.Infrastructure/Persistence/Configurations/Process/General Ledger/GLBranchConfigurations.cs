﻿using App.Domain.Entities.Process;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Persistence.Configurations.Process
{
    public class GLBranchConfigurations : IEntityTypeConfiguration<GLBranch>
    {
        public void Configure(EntityTypeBuilder<GLBranch> builder)
        {
            builder.ToTable("GLBranch");
            builder.HasKey(e=>e.Id);
            builder.Property(e => e.ArabicName).HasColumnName("ArabicName");
            builder.Property(e => e.LatinName).HasColumnName("LatinName");
         //   builder.HasMany(e => e.Stores).WithOne(s => s.Branch).HasForeignKey(d => d.BranchId);
        }
    }
}
