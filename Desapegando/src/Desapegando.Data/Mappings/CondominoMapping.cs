using Desapegando.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Desapegando.Data.Mappings;

public class CondominoMapping : IEntityTypeConfiguration<Condomino>
{
    public void Configure(EntityTypeBuilder<Condomino> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Apartamento)
            .IsRequired()
            .HasColumnType("VARCHAR(20)");

        builder.Property(x => x.Cpf)
            .IsRequired()
            .HasColumnType("VARCHAR(11)");

        builder.Property(x => x.Email)
            .IsRequired()
            .HasColumnType("VARCHAR(100)");

        builder.Property(x => x.Nome)
            .IsRequired()
            .HasColumnType("VARCHAR(100)");

        builder.Property(x => x.Sexo)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.Sobrenome)
            .IsRequired()
            .HasColumnType("VARCHAR(100)");

        builder.Property(x => x.Telefone)
            .IsRequired()
            .HasColumnType("VARCHAR(20)");

        builder.Property(x => x.Idade)
            .IsRequired()
            .HasColumnType("INT");

        builder.Property(x => x.Administrador)
            .IsRequired()
            .HasColumnType("BIT");

        builder.ToTable("Condomino");
    }
}