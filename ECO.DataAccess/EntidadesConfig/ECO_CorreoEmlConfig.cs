using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECO.Dominio.Entidades;

namespace ECO.DataAccess.EntidadesConfig
{
    public class ECO_CorreoEmlConfig : IEntityTypeConfiguration<ECO_CorreoEml>
    {
        public void Configure(EntityTypeBuilder<ECO_CorreoEml> builder) 
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Nombre)
                .HasColumnType("varchar(250)")
                .HasComment("Nombre del archivo EML");

            builder.Property(x => x.TamanoBytes)
                .HasComment("Tamaño del archivo EML en bytes");

            builder.Property(x => x.ContenidoArchivo)
                .HasColumnType("longblob")
                .HasComment("Contenido binario del archivo EML");

            builder.Property(x => x.FechaCreado)
                .HasColumnType("datetime");

            builder.HasOne(x => x.Correo)
                .WithOne(x => x.CorreoEml)
                .HasForeignKey<ECO_CorreoEml>(x => x.CorreoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.CorreoId).IsUnique();
        }
    }
}
