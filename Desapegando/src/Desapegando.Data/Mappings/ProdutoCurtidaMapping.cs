using Desapegando.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Desapegando.Data.Mappings
{
    public class ProdutoCurtidaMapping : IEntityTypeConfiguration<ProdutoCurtida>
    {
        public void Configure(EntityTypeBuilder<ProdutoCurtida> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CondominoId)
                   .IsRequired()
                   .HasColumnType("UNIQUEIDENTIFIER");

            builder.Property(x => x.ProdutoId)
                   .IsRequired()
                   .HasColumnType("UNIQUEIDENTIFIER");
            
            builder.ToTable("ProdutoCurtida");
        }
    }
}
