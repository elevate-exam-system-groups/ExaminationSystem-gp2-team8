using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExaminationSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class checks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "AnsweredAt",
                table: "StudentAnswers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "StudentAnswers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CorrectCount",
                table: "Attempts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Passed",
                table: "Attempts",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScorePct",
                table: "Attempts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalQuestions",
                table: "Attempts",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AnsweredAt",
                table: "StudentAnswers");

            migrationBuilder.DropColumn(
                name: "IsCorrect",
                table: "StudentAnswers");

            migrationBuilder.DropColumn(
                name: "CorrectCount",
                table: "Attempts");

            migrationBuilder.DropColumn(
                name: "Passed",
                table: "Attempts");

            migrationBuilder.DropColumn(
                name: "ScorePct",
                table: "Attempts");

            migrationBuilder.DropColumn(
                name: "TotalQuestions",
                table: "Attempts");
        }
    }
}
