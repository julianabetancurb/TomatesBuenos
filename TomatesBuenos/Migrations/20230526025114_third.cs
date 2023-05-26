using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TomatesBuenos.Migrations
{
    /// <inheritdoc />
    public partial class third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TopTVshow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TVshowId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopTVshow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopTVshow_TVshow_TVshowId",
                        column: x => x.TVshowId,
                        principalTable: "TVshow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TopTVshow_TVshowId",
                table: "TopTVshow",
                column: "TVshowId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TopTVshow");
        }
    }
}
