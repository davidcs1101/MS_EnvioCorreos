using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECO.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ECO_Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ECO_ColaSolicitudes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Tipo = table.Column<string>(type: "varchar(250)", nullable: false, comment: "Tipo de solicitud a realizar.")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Payload = table.Column<string>(type: "Text", nullable: false, comment: "Payload para la solicitud.")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Estado = table.Column<short>(type: "smallint", nullable: false, comment: "Estado del proceso de la solicitud. (0: Pendiente, 1: Procesando, 2: Exitosa, 3: Fallida)."),
                    Intentos = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Intentos del proceso"),
                    FechaCreado = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    FechaUltimoIntento = table.Column<DateTime>(type: "datetime", nullable: true),
                    ErrorMensaje = table.Column<string>(type: "Text", nullable: true, comment: "Detalle de error de procasado de la solicitud.")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECO_ColaSolicitudes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ECO_Configuraciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmpresaId = table.Column<int>(type: "int", nullable: false, comment: "Empresa propietaria de la configuración de correo electrónico."),
                    Codigo = table.Column<string>(type: "varchar(30)", nullable: false, comment: "Código del proceso de la empresa.")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "varchar(250)", nullable: false, comment: "Nombre descriptivo de la configuración de correo.")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Usuario = table.Column<string>(type: "varchar(150)", nullable: false, comment: "Usuario o cuenta de correo utilizada para el envío.")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Clave = table.Column<string>(type: "varchar(500)", nullable: false, comment: "Clave o secreto utilizado para autenticación.")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Host = table.Column<string>(type: "varchar(250)", nullable: false, comment: "Servidor de correo.")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Puerto = table.Column<int>(type: "int", nullable: false, comment: "Puerto utilizado para la conexión"),
                    UsaSsl = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "Indica si la conexión utiliza SSL/TLS."),
                    UsaCredencialPorDefecto = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "Indica si se utilizan las credenciales predeterminadas del servidor."),
                    CorreoRespuesta = table.Column<string>(type: "varchar(150)", nullable: false, comment: "Nombre o correo mostrado como remitente de respuesta.")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Estado = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "Indica si la configuración se encuentra activa."),
                    UsuarioCreadorId = table.Column<int>(type: "int", nullable: false),
                    FechaCreado = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECO_Configuraciones", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ECO_Plantillas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmpresaId = table.Column<int>(type: "int", nullable: false, comment: "Empresa propietaria de la plantilla de correo."),
                    Codigo = table.Column<string>(type: "varchar(30)", nullable: false, comment: "Código de la plantilla de correo de la empresa.")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "varchar(250)", nullable: false, comment: "Nombre de la plantilla de correo.")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Asunto = table.Column<string>(type: "varchar(250)", nullable: false, comment: "Asunto de la plantilla de correo.")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Html = table.Column<string>(type: "TEXT", nullable: false, comment: "Contenido HTML de la plantilla de correo.")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Estado = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "Indica si la plantilla de correo se encuentra activa."),
                    UsuarioCreadorId = table.Column<int>(type: "int", nullable: false),
                    FechaCreado = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECO_Plantillas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ECO_Correos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Codigo = table.Column<Guid>(type: "char(36)", nullable: false, comment: "Código único utilizado para consultar el correo.", collation: "ascii_general_ci"),
                    Asunto = table.Column<string>(type: "varchar(250)", nullable: false, comment: "Asunto del correo electrónico")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cuerpo = table.Column<string>(type: "longtext", nullable: false, comment: "Contenido del correo electrónico.")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EsCuerpoHtml = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "Indica si el cuerpo del correo está en formato HTML."),
                    CorreoRespuesta = table.Column<string>(type: "varchar(150)", nullable: true, comment: "Correo de respuesta (Reply-To).")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Estado = table.Column<int>(type: "int", nullable: false, comment: "Estado actual del correo (Pendiente = 0, Enviado = 1, Fallido = 2)."),
                    ErrorMensaje = table.Column<string>(type: "text", nullable: true, comment: "Descripción del último error presentado durante el envío.")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaEnvio = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Fecha y hora en que el correo fue enviado exitosamente."),
                    EmpresaId = table.Column<int>(type: "int", nullable: true, comment: "Empresa desde la cual se solicitó el envío de correo"),
                    PlantillaId = table.Column<int>(type: "int", nullable: true, comment: "Plantilla utilizada para generar el correo."),
                    UsuarioCreadorId = table.Column<int>(type: "int", nullable: false),
                    FechaCreado = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECO_Correos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ECO_Correos_ECO_Plantillas_PlantillaId",
                        column: x => x.PlantillaId,
                        principalTable: "ECO_Plantillas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ECO_CorreosAdjuntos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CorreoId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(255)", nullable: false, comment: "Nombre del archivo adjunto sin extensión.")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Extension = table.Column<string>(type: "varchar(20)", nullable: false, comment: "Extensión del archivo adjunto.")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoContenido = table.Column<string>(type: "varchar(150)", nullable: true, comment: "Tipo MIME del archivo adjunto.")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TamanoBytes = table.Column<long>(type: "bigint", nullable: false, comment: "Tamaño del archivo en bytes."),
                    ContenidoArchivo = table.Column<byte[]>(type: "longblob", nullable: false, comment: "Contenido binario del archivo adjunto."),
                    UsuarioCreadorId = table.Column<int>(type: "int", nullable: false),
                    FechaCreado = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECO_CorreosAdjuntos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ECO_CorreosAdjuntos_ECO_Correos_CorreoId",
                        column: x => x.CorreoId,
                        principalTable: "ECO_Correos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ECO_CorreosDestinatarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CorreoId = table.Column<int>(type: "int", nullable: false),
                    Destinatario = table.Column<string>(type: "varchar(150)", nullable: false, comment: "Correo electrónico destinatario.")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tipo = table.Column<int>(type: "int", nullable: false, comment: "Tipo de destinatario (Para = 0, CC = 1, CCO = 2)."),
                    UsuarioCreadorId = table.Column<int>(type: "int", nullable: false),
                    FechaCreado = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECO_CorreosDestinatarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ECO_CorreosDestinatarios_ECO_Correos_CorreoId",
                        column: x => x.CorreoId,
                        principalTable: "ECO_Correos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ECO_CorreosEml",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CorreoId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(250)", nullable: false, comment: "Nombre del archivo EML")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TamanoBytes = table.Column<long>(type: "bigint", nullable: false, comment: "Tamaño del archivo EML en bytes"),
                    ContenidoArchivo = table.Column<byte[]>(type: "longblob", nullable: false, comment: "Contenido binario del archivo EML"),
                    UsuarioCreadorId = table.Column<int>(type: "int", nullable: false),
                    FechaCreado = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECO_CorreosEml", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ECO_CorreosEml_ECO_Correos_CorreoId",
                        column: x => x.CorreoId,
                        principalTable: "ECO_Correos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ECO_ColaSolicitudes_Estado",
                table: "ECO_ColaSolicitudes",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_ECO_ColaSolicitudes_Tipo",
                table: "ECO_ColaSolicitudes",
                column: "Tipo");

            migrationBuilder.CreateIndex(
                name: "IX_ECO_Configuraciones_EmpresaId",
                table: "ECO_Configuraciones",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_ECO_Configuraciones_EmpresaId_Codigo",
                table: "ECO_Configuraciones",
                columns: new[] { "EmpresaId", "Codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ECO_Correos_Codigo",
                table: "ECO_Correos",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ECO_Correos_Estado",
                table: "ECO_Correos",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_ECO_Correos_FechaCreado",
                table: "ECO_Correos",
                column: "FechaCreado");

            migrationBuilder.CreateIndex(
                name: "IX_ECO_Correos_FechaEnvio",
                table: "ECO_Correos",
                column: "FechaEnvio");

            migrationBuilder.CreateIndex(
                name: "IX_ECO_Correos_PlantillaId",
                table: "ECO_Correos",
                column: "PlantillaId");

            migrationBuilder.CreateIndex(
                name: "IX_ECO_CorreosAdjuntos_CorreoId",
                table: "ECO_CorreosAdjuntos",
                column: "CorreoId");

            migrationBuilder.CreateIndex(
                name: "IX_ECO_CorreosDestinatarios_CorreoId",
                table: "ECO_CorreosDestinatarios",
                column: "CorreoId");

            migrationBuilder.CreateIndex(
                name: "IX_ECO_CorreosDestinatarios_Destinatario",
                table: "ECO_CorreosDestinatarios",
                column: "Destinatario");

            migrationBuilder.CreateIndex(
                name: "IX_ECO_CorreosDestinatarios_Tipo",
                table: "ECO_CorreosDestinatarios",
                column: "Tipo");

            migrationBuilder.CreateIndex(
                name: "IX_ECO_CorreosEml_CorreoId",
                table: "ECO_CorreosEml",
                column: "CorreoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ECO_Plantillas_EmpresaId",
                table: "ECO_Plantillas",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_ECO_Plantillas_EmpresaId_Codigo",
                table: "ECO_Plantillas",
                columns: new[] { "EmpresaId", "Codigo" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ECO_ColaSolicitudes");

            migrationBuilder.DropTable(
                name: "ECO_Configuraciones");

            migrationBuilder.DropTable(
                name: "ECO_CorreosAdjuntos");

            migrationBuilder.DropTable(
                name: "ECO_CorreosDestinatarios");

            migrationBuilder.DropTable(
                name: "ECO_CorreosEml");

            migrationBuilder.DropTable(
                name: "ECO_Correos");

            migrationBuilder.DropTable(
                name: "ECO_Plantillas");
        }
    }
}
