using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerOver.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFieldForTriadGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "GroupEmblemBaseId",
                table: "exvs2ob_triad_partner",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "GroupEmblemEffectId",
                table: "exvs2ob_triad_partner",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "GroupEmblemPartsId",
                table: "exvs2ob_triad_partner",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "GroupPlateId",
                table: "exvs2ob_triad_partner",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "MsSkill3",
                table: "exvs2ob_triad_partner",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupEmblemBaseId",
                table: "exvs2ob_triad_partner");

            migrationBuilder.DropColumn(
                name: "GroupEmblemEffectId",
                table: "exvs2ob_triad_partner");

            migrationBuilder.DropColumn(
                name: "GroupEmblemPartsId",
                table: "exvs2ob_triad_partner");

            migrationBuilder.DropColumn(
                name: "GroupPlateId",
                table: "exvs2ob_triad_partner");

            migrationBuilder.DropColumn(
                name: "MsSkill3",
                table: "exvs2ob_triad_partner");
        }
    }
}
