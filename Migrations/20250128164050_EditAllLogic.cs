using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoteSait.Migrations
{
    /// <inheritdoc />
    public partial class EditAllLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_AspNetUsers_IdUser",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_IdUser",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "IdUser",
                table: "Files");

            migrationBuilder.AddColumn<string>(
                name: "AlbumId",
                table: "Files",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AlbumModel",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlbumModel_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_AlbumId",
                table: "Files",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumModel_UserId",
                table: "AlbumModel",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_AlbumModel_AlbumId",
                table: "Files",
                column: "AlbumId",
                principalTable: "AlbumModel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_AlbumModel_AlbumId",
                table: "Files");

            migrationBuilder.DropTable(
                name: "AlbumModel");

            migrationBuilder.DropIndex(
                name: "IX_Files_AlbumId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "AlbumId",
                table: "Files");

            migrationBuilder.AddColumn<string>(
                name: "IdUser",
                table: "Files",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Files_IdUser",
                table: "Files",
                column: "IdUser");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_AspNetUsers_IdUser",
                table: "Files",
                column: "IdUser",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
