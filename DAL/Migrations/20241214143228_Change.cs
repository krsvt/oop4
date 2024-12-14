using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Unions",
                table: "Unions");

            migrationBuilder.DropIndex(
                name: "IX_Unions_Partner1Id",
                table: "Unions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Unions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Unions",
                table: "Unions",
                columns: new[] { "Partner1Id", "Partner2Id", "ChildId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Unions",
                table: "Unions");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Unions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Unions",
                table: "Unions",
                columns: new[] { "Id", "ChildId" });

            migrationBuilder.CreateIndex(
                name: "IX_Unions_Partner1Id",
                table: "Unions",
                column: "Partner1Id");
        }
    }
}
