using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicAndReaction.Server.Migrations
{
    /// <inheritdoc />
    public partial class modes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Mode",
                table: "UserReactions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mode",
                table: "UserReactions");
        }
    }
}
