using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hanamilegal.Web.ApplicationsApi.Migrations
{
    /// <inheritdoc />
    public partial class AddTechApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Applications",
                newName: "LegalApplications");

            migrationBuilder.Sql(
                """
                ALTER TABLE "LegalApplications"
                RENAME CONSTRAINT "PK_Applications" TO "PK_LegalApplications";
                """);

            migrationBuilder.CreateTable(
                name: "TechApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    SenderName = table.Column<string>(type: "text", nullable: false),
                    Organization = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechApplications", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "TechApplications");

            migrationBuilder.Sql(
                """
                ALTER TABLE "LegalApplications"
                RENAME CONSTRAINT "PK_LegalApplications" TO "PK_Applications";
                """);

            migrationBuilder.RenameTable(
                name: "LegalApplications",
                newName: "Applications");
        }
    }
}
