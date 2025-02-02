using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Iprox.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TvShow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Language = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Premiered = table.Column<DateOnly>(type: "date", nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TvMazeId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TvShow", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TvShowGenres",
                columns: table => new
                {
                    GenresId = table.Column<int>(type: "int", nullable: false),
                    TvShowId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TvShowGenres", x => new { x.GenresId, x.TvShowId });
                    table.ForeignKey(
                        name: "FK_TvShowGenres_Genre_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Genre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TvShowGenres_TvShow_TvShowId",
                        column: x => x.TvShowId,
                        principalTable: "TvShow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Genre",
                columns: new[] { "Id", "CreatedOn", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 1, 9, 34, 3, 794, DateTimeKind.Utc).AddTicks(208), "Drama" },
                    { 2, new DateTime(2025, 2, 1, 9, 34, 3, 794, DateTimeKind.Utc).AddTicks(219), "ScienceFiction" },
                    { 3, new DateTime(2025, 2, 1, 9, 34, 3, 794, DateTimeKind.Utc).AddTicks(276), "Thriller" },
                    { 4, new DateTime(2025, 2, 1, 9, 34, 3, 794, DateTimeKind.Utc).AddTicks(278), "Action" },
                    { 5, new DateTime(2025, 2, 1, 9, 34, 3, 794, DateTimeKind.Utc).AddTicks(279), "Crime" },
                    { 6, new DateTime(2025, 2, 1, 9, 34, 3, 794, DateTimeKind.Utc).AddTicks(280), "Horror" },
                    { 7, new DateTime(2025, 2, 1, 9, 34, 3, 794, DateTimeKind.Utc).AddTicks(281), "Romance" },
                    { 8, new DateTime(2025, 2, 1, 9, 34, 3, 794, DateTimeKind.Utc).AddTicks(282), "Adventure" },
                    { 9, new DateTime(2025, 2, 1, 9, 34, 3, 794, DateTimeKind.Utc).AddTicks(282), "Other" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TvShow_TvMazeId",
                table: "TvShow",
                column: "TvMazeId",
                unique: true,
                filter: "[TvMazeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TvShowGenres_TvShowId",
                table: "TvShowGenres",
                column: "TvShowId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TvShowGenres");

            migrationBuilder.DropTable(
                name: "Genre");

            migrationBuilder.DropTable(
                name: "TvShow");
        }
    }
}
