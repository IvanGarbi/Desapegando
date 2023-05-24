using Desapegando.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Desapegando.Data.Mappings
{
    public class CampanhaImagemMapping : IEntityTypeConfiguration<CampanhaImagem>
    {
        public void Configure(EntityTypeBuilder<CampanhaImagem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.FileName)
                .IsRequired()
                .HasColumnType("NVARCHAR")
                .HasMaxLength(150);

            builder.Property(x => x.CampanhaId)
                .IsRequired()
                .HasColumnType("UNIQUEIDENTIFIER");

            builder.ToTable("CampanhaImagem");
        }
    }
}
