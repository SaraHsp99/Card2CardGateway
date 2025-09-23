using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Card2CardGateway.Domain.Entities;

namespace Card2CardGateway.Infrastructure.Persistence.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.RequestTraceId)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(e => e.SourceCardNumber)
                   .IsRequired()
                   .HasMaxLength(16);

            builder.Property(e => e.DestinationCardNumber)
                   .IsRequired()
                   .HasMaxLength(16);

            builder.Property(e => e.BankName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(e => e.Amount)
                   .HasColumnType("decimal(18,2)");

            builder.Property(e => e.Status)
                   .IsRequired();

            builder.Property(e => e.TransactionReferenceCode)
                   .HasMaxLength(100);

            builder.Property(e => e.ErrorCode)
                   .HasMaxLength(50);

            builder.Property(e => e.RawRequest)
                   .HasColumnType("nvarchar(max)");

            builder.Property(e => e.RawResponse)
                   .HasColumnType("nvarchar(max)");

            builder.Property(e => e.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.HasIndex(e => e.RequestTraceId)
                   .IsUnique();
        }
    }
}
