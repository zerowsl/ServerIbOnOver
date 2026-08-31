using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerOver.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldBattleGradeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "GradeId",
                table: "exvs2ob_battle_target",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "GradeId",
                table: "exvs2ob_battle_self",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "GradeId",
                table: "exvs2ob_battle_ally",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GradeId",
                table: "exvs2ob_battle_target");

            migrationBuilder.DropColumn(
                name: "GradeId",
                table: "exvs2ob_battle_self");

            migrationBuilder.DropColumn(
                name: "GradeId",
                table: "exvs2ob_battle_ally");
        }
    }
}
