using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECO.Dominio.Entidades;

namespace ECO.DataAccess.EntidadesConfig
{
    public class ECO_ConfiguracionConfig : IEntityTypeConfiguration<ECO_Configuracion>
    {
        public void Configure(EntityTypeBuilder<ECO_Configuracion> builder) 
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Codigo)
                .HasColumnType("varchar(30)")
                .HasComment("Código del proceso de la empresa.");

            builder.Property(x => x.Descripcion)
                .HasColumnType("varchar(250)")
                .HasComment("Nombre descriptivo de la configuración de correo.");

            builder.Property(x => x.Usuario)
                .HasColumnType("varchar(150)")
                .HasComment("Usuario o cuenta de correo utilizada para el envío.");

            builder.Property(x => x.Clave)
                .HasColumnType("varchar(500)")
                .HasComment("Clave o secreto utilizado para autenticación.");

            builder.Property(x => x.Host)
                .HasColumnType("varchar(250)")
                .HasComment("Servidor de correo.");

            builder.Property(x => x.Puerto)
                .HasComment("Puerto utilizado para la conexión");

            builder.Property(x => x.UsaSsl)
                .HasComment("Indica si la conexión utiliza SSL/TLS.");

            builder.Property(x => x.UsaCredencialPorDefecto)
                .HasComment("Indica si se utilizan las credenciales predeterminadas del servidor.");

            builder.Property(x => x.CorreoRespuesta)
                .HasColumnType("varchar(150)")
                .HasComment("Nombre o correo mostrado como remitente de respuesta.");

            builder.Property(x => x.EmpresaId)
                .HasComment("Empresa propietaria de la configuración de correo electrónico.");

            builder.Property(x => x.EstadoActivo)
                .HasComment("Indica si la configuración se encuentra activa.");

            builder.Property(x => x.FechaCreado)
                .HasColumnType("datetime");

            builder.HasIndex(x => x.EmpresaId);
            builder.HasIndex(x => new
            {
                x.EmpresaId,
                x.Codigo
            }).IsUnique();
        }
    }
}
