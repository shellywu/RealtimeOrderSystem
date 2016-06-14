using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity.ModelConfiguration;
using OrderSystem.Core.Models;

namespace OrderSystem.Core.Models.ModelsMap
{
    public class ProductConfiguration:EntityTypeConfiguration<Product>
    {
        public ProductConfiguration()
        {
            HasKey(p=>p.Id);
            Property(p => p.ProductName).IsRequired();
            Property(p => p.ProductCode).IsRequired();
            Property(p => p.ValidStartDate).HasColumnType("datetime2");
            Property(p => p.ValidEndDate).HasColumnType("datetime2");
            Property(p => p.ProductCode).HasMaxLength(20);
            HasRequired(p => p.Partner).WithMany(par => par.Products).HasForeignKey(p=>p.PartnerId);
        }        
    }

    public class PartnerConfiguration:EntityTypeConfiguration<Partner>
    {
        public PartnerConfiguration()
        {
            HasKey(par=>par.Id);
            Property(par => par.Name).IsRequired().HasMaxLength(128);
            Property(par => par.ParnerPhone).HasMaxLength(20);
            HasMany(par=>par.Products);
        }
    }
}