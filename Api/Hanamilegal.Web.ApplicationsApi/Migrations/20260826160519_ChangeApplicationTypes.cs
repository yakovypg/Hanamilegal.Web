using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hanamilegal.Web.ApplicationsApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangeApplicationTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE "Applications"
                SET "Type" = 7
                WHERE "Type" = 1
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE "Applications"
                SET "Type" = 1
                WHERE "Type" = 7
            """);
        }
    }
}
