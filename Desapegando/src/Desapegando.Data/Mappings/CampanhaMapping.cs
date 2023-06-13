using Desapegando.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Desapegando.Data.Mappings
{
    public class CampanhaMapping : IEntityTypeConfiguration<Campanha>
    {
        public void Configure(EntityTypeBuilder<Campanha> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nome)
                .IsRequired()
                .HasColumnType("VARCHAR(100)");

            builder.Property(x => x.NomeInstituicao)
                .IsRequired()
                .HasColumnType("VARCHAR(100)");

            builder.Property(x => x.EmailResponsavel)
                .IsRequired()
                .HasColumnType("VARCHAR(100)");

            builder.Property(x => x.Descricao)
                .IsRequired()
                .HasColumnType("VARCHAR(MAX)");

            builder.Property(x => x.LocalDeEncontro)
                .IsRequired()
                .HasColumnType("VARCHAR(50)");

            builder.Property(x => x.NomeResponsavel)
                .IsRequired()
                .HasColumnType("VARCHAR(100)");

            builder.Property(x => x.TelefoneResponsavel)
                .IsRequired()
                .HasColumnType("VARCHAR(20)");

            builder.Property(x => x.DataInicio)
                .IsRequired()
                .HasColumnType("DATE");

            builder.Property(x => x.DataFinal)
                .IsRequired()
                .HasColumnType("DATE");

            builder.Property(x => x.Ativo)
                .IsRequired()
                .HasColumnType("BIT");

            builder.Property(x => x.CondominoId)
                .IsRequired()
                .HasColumnType("UNIQUEIDENTIFIER");

            builder.HasMany(x => x.CampanhaImagens)
                .WithOne(a => a.Campanha)
                .HasForeignKey(k => k.CampanhaId);

            builder.ToTable("Campanha");
        }
    }
}
