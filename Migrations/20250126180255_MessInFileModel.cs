﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoteSait.Migrations
{
    /// <inheritdoc />
    public partial class MessInFileModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Files",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "Files");
        }
    }
}
