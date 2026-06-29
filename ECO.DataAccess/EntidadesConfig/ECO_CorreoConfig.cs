using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECO.Dominio.Entidades;

namespace ECO.DataAccess.EntidadesConfig
{
    public class ECO_CorreoConfig : IEntityTypeConfiguration<ECO_Correo>
    {
        public void Configure(EntityTypeBuilder<ECO_Correo> builder) 
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Asunto)
                .HasColumnType("varchar(250)")
                .HasComment("Asunto del correo electrónico");

            builder.Property(x => x.Cuerpo)
                .HasColumnType("longtext")
                .HasComment("Contenido del correo electrónico.");

            builder.Property(x => x.EsCuerpoHtml)
                .HasComment("Indica si el cuerpo del correo está en formato HTML.");

            builder.Property(x => x.CorreoRespuesta)
                .HasColumnType("varchar(150)")
                .HasComment("Correo de respuesta (Reply-To).");

            builder.Property(x => x.Estado)
                .HasComment("Estado actual del correo (Pendiente = 0, Enviado = 1, Fallido = 2).");

            builder.Property(x => x.ErrorMensaje)
                .HasColumnType("text")
                .HasComment("Descripción del último error presentado durante el envío.");

            builder.Property(x => x.FechaEnvio).HasColumnType("datetime")
                .HasComment("Fecha y hora en que el correo fue enviado exitosamente.");

            builder.Property(x => x.EmpresaId)
                .HasComment("Empresa desde la cual se solicitó el envío de correo");

            builder.Property(x => x.FechaCreado).HasColumnType("datetime");

            builder.Property(x => x.Codigo)
                .HasComment("Código único utilizado para consultar el correo.");

            builder.Property(x => x.PlantillaId)
                .HasComment("Plantilla utilizada para generar el correo.");


            builder.HasMany(x => x.CorreosDestinatarios)
                .WithOne(x => x.Correo)
                .HasForeignKey(x => x.CorreoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.CorreosAdjuntos)
                .WithOne(x => x.Correo)
                .HasForeignKey(x => x.CorreoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CorreoEml)
                .WithOne(x => x.Correo)
                .HasForeignKey<ECO_CorreoEml>(x => x.CorreoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Plantilla)
                .WithMany(x => x.Correos)
                .HasForeignKey(x => x.PlantillaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.Estado);
            builder.HasIndex(x => x.FechaCreado);
            builder.HasIndex(x => x.FechaEnvio);
            builder.HasIndex(x => x.Codigo).IsUnique();
            builder.HasIndex(x => x.PlantillaId);
        }
    }
}
