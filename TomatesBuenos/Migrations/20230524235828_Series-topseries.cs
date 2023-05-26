using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TomatesBuenos.Migrations
{
    /// <inheritdoc />
    public partial class Seriestopseries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TVshow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AudienceRating = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CriticsRating = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvaliablePlatforms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Synopsis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Genre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Premiere = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Creators = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Starring = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TVshow", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TVshow");
        }
    }
}
