using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerOver.Migrations
{
    /// <inheritdoc />
    public partial class FixTraining : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "CpuHpAutoRegen",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "ExOverLimit",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "Player1HpMode",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "QuickFill",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CpuHpAutoRegen",
                table: "exvs2ob_training_profile");

            migrationBuilder.DropColumn(
                name: "ExOverLimit",
                table: "exvs2ob_training_profile");

            migrationBuilder.DropColumn(
                name: "Player1HpMode",
                table: "exvs2ob_training_profile");

            migrationBuilder.DropColumn(
                name: "QuickFill",
                table: "exvs2ob_training_profile");
        }
    }
}
