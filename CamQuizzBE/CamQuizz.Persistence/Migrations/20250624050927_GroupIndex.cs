using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CamQuizz.Migrations
{
    /// <inheritdoc />
    public partial class GroupIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Groups_OwnerId",
                table: "Groups");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_OwnerId_Name",
                table: "Groups",
                columns: new[] { "OwnerId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Groups_OwnerId_Name",
                table: "Groups");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_OwnerId",
                table: "Groups",
                column: "OwnerId");
        }
    }
}
