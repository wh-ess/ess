using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ESS.Domain.JXC.Models.Mapping
{
    public class CustomerMap : EntityTypeConfiguration<Customer>
    {
        public CustomerMap()
        {
            // Primary Key
            this.HasKey(t => t.CustomerId);

            // Properties
            this.Property(t => t.CustomerAttribName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CustomerName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.InvoiceNo)
                .HasMaxLength(50);

            this.Property(t => t.Owner)
                .HasMaxLength(50);

            this.Property(t => t.RocId)
                .HasMaxLength(50);

            this.Property(t => t.ContactMan1)
                .HasMaxLength(50);

            this.Property(t => t.ContactMan2)
                .HasMaxLength(50);

            this.Property(t => t.ContactPhone1)
                .HasMaxLength(50);

            this.Property(t => t.ContactPhone2)
                .HasMaxLength(50);

            this.Property(t => t.Fax)
                .HasMaxLength(50);

            this.Property(t => t.CustomerAddress)
                .HasMaxLength(100);

            this.Property(t => t.DeliveryAddress)
                .HasMaxLength(100);

            this.Property(t => t.InvoiceAddress)
                .HasMaxLength(100);

            this.Property(t => t.CarNo)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Customer", "Common");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.CustomerCode).HasColumnName("CustomerCode");
            this.Property(t => t.CustomerAttribName).HasColumnName("CustomerAttribName");
            this.Property(t => t.CustomerName).HasColumnName("CustomerName");
            this.Property(t => t.InvoiceNo).HasColumnName("InvoiceNo");
            this.Property(t => t.Owner).HasColumnName("Owner");
            this.Property(t => t.RocId).HasColumnName("RocId");
            this.Property(t => t.ContactMan1).HasColumnName("ContactMan1");
            this.Property(t => t.ContactMan2).HasColumnName("ContactMan2");
            this.Property(t => t.ContactPhone1).HasColumnName("ContactPhone1");
            this.Property(t => t.ContactPhone2).HasColumnName("ContactPhone2");
            this.Property(t => t.Fax).HasColumnName("Fax");
            this.Property(t => t.CustomerAddress).HasColumnName("CustomerAddress");
            this.Property(t => t.DeliveryAddress).HasColumnName("DeliveryAddress");
            this.Property(t => t.InvoiceAddress).HasColumnName("InvoiceAddress");
            this.Property(t => t.PayDays).HasColumnName("PayDays");
            this.Property(t => t.CreditLine).HasColumnName("CreditLine");
            this.Property(t => t.CreditBalance).HasColumnName("CreditBalance");
            this.Property(t => t.LastDeliveryDate).HasColumnName("LastDeliveryDate");
            this.Property(t => t.Advance).HasColumnName("Advance");
            this.Property(t => t.CarNo).HasColumnName("CarNo");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.CreateById).HasColumnName("CreateById");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.ModifyById).HasColumnName("ModifyById");
            this.Property(t => t.ModifyDate).HasColumnName("ModifyDate");
            this.Property(t => t.SalesMan_SalesManId).HasColumnName("SalesMan_SalesManId");

            // Relationships
            this.HasOptional(t => t.SalesMan)
                .WithMany(t => t.Customers)
                .HasForeignKey(d => d.SalesMan_SalesManId);

        }
    }
}
