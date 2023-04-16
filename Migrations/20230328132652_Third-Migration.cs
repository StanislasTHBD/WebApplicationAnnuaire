using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplicationAnnuaire.Migrations
{
    /// <inheritdoc />
    public partial class ThirdMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "Employes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "Employes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employes_ServiceId",
                table: "Employes",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Employes_SiteId",
                table: "Employes",
                column: "SiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employes_Service_ServiceId",
                table: "Employes",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employes_Site_SiteId",
                table: "Employes",
                column: "SiteId",
                principalTable: "Site",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employes_Service_ServiceId",
                table: "Employes");

            migrationBuilder.DropForeignKey(
                name: "FK_Employes_Site_SiteId",
                table: "Employes");

            migrationBuilder.DropIndex(
                name: "IX_Employes_ServiceId",
                table: "Employes");

            migrationBuilder.DropIndex(
                name: "IX_Employes_SiteId",
                table: "Employes");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "Employes");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "Employes");
        }
    }
}
