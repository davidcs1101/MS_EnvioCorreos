using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECO.Dominio.Entidades;

namespace ECO.DataAccess.EntidadesConfig
{
    public class ECO_PlantillaConfig : IEntityTypeConfiguration<ECO_Plantilla>
    {
        public void Configure(EntityTypeBuilder<ECO_Plantilla> builder) 
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.EmpresaId)
                .HasComment("Empresa propietaria de la plantilla de correo.");

            builder.Property(x => x.Codigo)
                .HasColumnType("varchar(30)")
                .HasComment("Código de la plantilla de correo de la empresa.");

            builder.Property(x => x.Nombre)
                .HasColumnType("varchar(250)")
                .HasComment("Nombre de la plantilla de correo.");

            builder.Property(x => x.Asunto)
                .HasColumnType("varchar(250)")
                .HasComment("Asunto de la plantilla de correo.");

            builder.Property(x => x.Html)
                .HasColumnType("TEXT")
                .HasComment("Contenido HTML de la plantilla de correo.");

            builder.Property(x => x.EstadoActivo)
                .HasComment("Indica si la plantilla de correo se encuentra activa.");

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
