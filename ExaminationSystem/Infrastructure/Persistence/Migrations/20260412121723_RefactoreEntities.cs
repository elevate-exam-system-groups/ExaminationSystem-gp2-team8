using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExaminationSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RefactoreEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attempts_AspNetUsers_UserId",
                table: "Attempts");

            migrationBuilder.DropForeignKey(
                name: "FK_Attempts_Quizzes_QuizId",
                table: "Attempts");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_AspNetUsers_UserId",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Diplomas_DiplomaId",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Options_Questions_QuestionId",
                table: "Options");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_AspNetUsers_CreatedByUserId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Diplomas_DiplomaId",
                table: "Quizzes");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAnswers_AspNetUsers_UserId",
                table: "StudentAnswers");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Quizzes",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Quizzes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Questions",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Questions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Diplomas",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Diplomas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "score",
                table: "Attempts",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuizId",
                table: "Questions",
                column: "QuizId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attempts_AspNetUsers_UserId",
                table: "Attempts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attempts_Quizzes_QuizId",
                table: "Attempts",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_AspNetUsers_UserId",
                table: "Enrollments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Diplomas_DiplomaId",
                table: "Enrollments",
                column: "DiplomaId",
                principalTable: "Diplomas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Questions_QuestionId",
                table: "Options",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_AspNetUsers_CreatedByUserId",
                table: "Questions",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Quizzes_QuizId",
                table: "Questions",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Diplomas_DiplomaId",
                table: "Quizzes",
                column: "DiplomaId",
                principalTable: "Diplomas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAnswers_AspNetUsers_UserId",
                table: "StudentAnswers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attempts_AspNetUsers_UserId",
                table: "Attempts");

            migrationBuilder.DropForeignKey(
                name: "FK_Attempts_Quizzes_QuizId",
                table: "Attempts");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_AspNetUsers_UserId",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Diplomas_DiplomaId",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Options_Questions_QuestionId",
                table: "Options");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_AspNetUsers_CreatedByUserId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Quizzes_QuizId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Diplomas_DiplomaId",
                table: "Quizzes");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAnswers_AspNetUsers_UserId",
                table: "StudentAnswers");

            migrationBuilder.DropIndex(
                name: "IX_Questions_QuizId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Diplomas");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Quizzes",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Questions",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Diplomas",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "score",
                table: "Attempts",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddForeignKey(
                name: "FK_Attempts_AspNetUsers_UserId",
                table: "Attempts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attempts_Quizzes_QuizId",
                table: "Attempts",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_AspNetUsers_UserId",
                table: "Enrollments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Diplomas_DiplomaId",
                table: "Enrollments",
                column: "DiplomaId",
                principalTable: "Diplomas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Questions_QuestionId",
                table: "Options",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_AspNetUsers_CreatedByUserId",
                table: "Questions",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Diplomas_DiplomaId",
                table: "Quizzes",
                column: "DiplomaId",
                principalTable: "Diplomas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAnswers_AspNetUsers_UserId",
                table: "StudentAnswers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
