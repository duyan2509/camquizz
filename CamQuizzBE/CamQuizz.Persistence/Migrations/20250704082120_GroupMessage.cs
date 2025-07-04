using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CamQuizz.Migrations
{
    /// <inheritdoc />
    public partial class GroupMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LastReadMessageId",
                table: "UserGroups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GroupMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupMessages_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GroupMessages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_LastReadMessageId",
                table: "UserGroups",
                column: "LastReadMessageId",
                unique: true,
                filter: "[LastReadMessageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessages_GroupId",
                table: "GroupMessages",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessages_UserId",
                table: "GroupMessages",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_GroupMessages_LastReadMessageId",
                table: "UserGroups",
                column: "LastReadMessageId",
                principalTable: "GroupMessages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_GroupMessages_LastReadMessageId",
                table: "UserGroups");

            migrationBuilder.DropTable(
                name: "GroupMessages");

            migrationBuilder.DropIndex(
                name: "IX_UserGroups_LastReadMessageId",
                table: "UserGroups");

            migrationBuilder.DropColumn(
                name: "LastReadMessageId",
                table: "UserGroups");
        }
    }
}
