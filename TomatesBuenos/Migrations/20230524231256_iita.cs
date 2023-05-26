using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TomatesBuenos.Migrations
{
    /// <inheritdoc />
    public partial class iita : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AudienceRating = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CriticsRating = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvaliablePlatfomrms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Synopsis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Clasification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Genre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleaseDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DirectionTeam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainActors = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AudienceComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CriticsComments = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movie");
        }
    }
}
