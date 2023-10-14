using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Desapegando.Business.Models;

namespace Desapegando.Data.Mappings
{
    public class CompraMapping : IEntityTypeConfiguration<Compra>
    {
        public void Configure(EntityTypeBuilder<Compra> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CondominoId)
                   .IsRequired()
                   .HasColumnType("UNIQUEIDENTIFIER");

            builder.Property(x => x.ProdutoId)
                   .IsRequired()
                   .HasColumnType("UNIQUEIDENTIFIER");

            builder.Property(x => x.DataVenda)
                   .IsRequired(false)
                   .HasColumnType("DATETIME");

            builder.ToTable("Compras");
        }
    }
}
