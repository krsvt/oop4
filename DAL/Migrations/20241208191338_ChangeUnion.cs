using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUnion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChildrenIds",
                table: "Unions");

            migrationBuilder.AddColumn<int>(
                name: "ChildId",
                table: "Unions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChildId",
                table: "Unions");

            migrationBuilder.AddColumn<List<int>>(
                name: "ChildrenIds",
                table: "Unions",
                type: "integer[]",
                nullable: false);
        }
    }
}
