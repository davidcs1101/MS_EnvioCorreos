using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECO.Dominio.Entidades;

namespace ECO.DataAccess.EntidadesConfig
{
    public class ECO_CorreoAdjuntoConfig : IEntityTypeConfiguration<ECO_CorreoAdjunto>
    {
        public void Configure(EntityTypeBuilder<ECO_CorreoAdjunto> builder) 
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nombre)
                .HasColumnType("varchar(255)")
                .HasComment("Nombre del archivo adjunto sin extensión.");

            builder.Property(x => x.Extension)
                .HasColumnType("varchar(20)")
                .HasComment("Extensión del archivo adjunto.");

            builder.Property(x => x.TipoContenido)
                .HasColumnType("varchar(150)")
                .HasComment("Tipo MIME del archivo adjunto.");

            builder.Property(x => x.TamanoBytes)
                .HasComment("Tamaño del archivo en bytes.");

            builder.Property(x => x.ContenidoArchivo)
                .HasColumnType("longblob")
                .HasComment("Contenido binario del archivo adjunto.");

            builder.Property(x => x.FechaCreado).HasColumnType("datetime");

            builder.HasIndex(x => x.CorreoId);
        }
    }
}
