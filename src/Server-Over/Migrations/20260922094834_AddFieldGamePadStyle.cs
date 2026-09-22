using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerOver.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldGamePadStyle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "CommandDispConfig",
                table: "exvs2ob_player_profile",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "GamePadStyle",
                table: "exvs2ob_player_profile",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommandDispConfig",
                table: "exvs2ob_player_profile");

            migrationBuilder.DropColumn(
                name: "GamePadStyle",
                table: "exvs2ob_player_profile");
        }
    }
}
