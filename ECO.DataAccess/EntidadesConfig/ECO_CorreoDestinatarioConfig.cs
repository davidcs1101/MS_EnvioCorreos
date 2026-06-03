using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECO.Dominio.Entidades;

namespace ECO.DataAccess.EntidadesConfig
{
    public class ECO_CorreoDestinatarioConfig : IEntityTypeConfiguration<ECO_CorreoDestinatario>
    {
        public void Configure(EntityTypeBuilder<ECO_CorreoDestinatario> builder) 
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Destinatario)
                .HasColumnType("varchar(150)")
                .IsRequired()
                .HasComment("Correo electrónico destinatario.");

            builder.Property(x => x.Tipo)
                .IsRequired()
                .HasComment("Tipo de destinatario (Para = 0, CC = 1, CCO = 2).");

            builder.Property(x => x.FechaCreado).HasColumnType("datetime");

            builder.HasIndex(x => x.CorreoId);
            builder.HasIndex(x => x.Tipo);
            builder.HasIndex(x => x.Destinatario);
        }
    }
}
