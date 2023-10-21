using Desapegando.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Desapegando.Data.Mappings;

public class ProdutoMapping : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Ativo)
            .IsRequired()
            .HasColumnType("BIT");

        builder.Property(x => x.Desistencia)
            .IsRequired()
            .HasColumnType("BIT");

        builder.Property(x => x.Categoria)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.Curtida)
            .IsRequired()
            .HasColumnType("INT");

        builder.Property(x => x.Quantidade)
            .IsRequired()
            .HasColumnType("INT");

        builder.Property(x => x.Preco)
            .IsRequired()
            .HasColumnType("DECIMAL");

        builder.Property(x => x.DataPublicacao)
            .IsRequired()
            .HasColumnType("DATETIME");

        //builder.Property(x => x.DataVenda)
        //    .IsRequired(false)
        //    .HasColumnType("DATETIME");

        builder.Property(x => x.DataDesistencia)
              .IsRequired(false)
              .HasColumnType("DATETIME");

        builder.Property(x => x.Descricao)
            .IsRequired()
            .HasColumnType("NVARCHAR(MAX)");

        builder.Property(x => x.EstadoProduto)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.Nome)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(150);

        builder.Property(x => x.CondominoId)
            .IsRequired()
            .HasColumnType("UNIQUEIDENTIFIER");

        builder.HasMany(x => x.ProdutoImagens)
            .WithOne(a => a.Produto)
            .HasForeignKey(k => k.ProdutoId);

        builder.HasMany(x => x.ProdutoCurtidas)
            .WithOne(a => a.Produto)
            .HasForeignKey(k => k.ProdutoId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Compras)
            .WithOne(a => a.Produto)
            .HasForeignKey(k => k.ProdutoId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.ToTable("Produto");
    }
}