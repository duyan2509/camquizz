using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CamQuizz.Migrations
{
    /// <inheritdoc />
    public partial class UserQuiz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AuthorId",
                table: "Quizzes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_AuthorId",
                table: "Quizzes",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Users_AuthorId",
                table: "Quizzes",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Users_AuthorId",
                table: "Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_Quizzes_AuthorId",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Quizzes");
        }
    }
}
