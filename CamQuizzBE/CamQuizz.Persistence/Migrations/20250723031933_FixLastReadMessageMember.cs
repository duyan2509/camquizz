using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CamQuizz.Migrations
{
    /// <inheritdoc />
    public partial class FixLastReadMessageMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserGroups_LastReadMessageId",
                table: "UserGroups");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_LastReadMessageId",
                table: "UserGroups",
                column: "LastReadMessageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserGroups_LastReadMessageId",
                table: "UserGroups");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_LastReadMessageId",
                table: "UserGroups",
                column: "LastReadMessageId",
                unique: true,
                filter: "[LastReadMessageId] IS NOT NULL");
        }
    }
}
