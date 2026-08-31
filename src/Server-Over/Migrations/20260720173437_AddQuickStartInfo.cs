using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerOver.Migrations
{
    /// <inheritdoc />
    public partial class AddQuickStartInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "djyx_ib_QuickStartInfoProfile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<uint>(type: "INTEGER", nullable: false),
                    SaveMode = table.Column<int>(type: "INTEGER", nullable: false),
                    GameMode = table.Column<uint>(type: "INTEGER", nullable: false),
                    RuleType = table.Column<uint>(type: "INTEGER", nullable: false),
                    TeamType = table.Column<uint>(type: "INTEGER", nullable: false),
                    MstMobileSuitId = table.Column<uint>(type: "INTEGER", nullable: false),
                    BurstType = table.Column<uint>(type: "INTEGER", nullable: false),
                    PartnerMobileSuitId = table.Column<uint>(type: "INTEGER", nullable: false),
                    PartnerBurstType = table.Column<uint>(type: "INTEGER", nullable: false),
                    BattleStageId = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_djyx_ib_QuickStartInfoProfile", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_djyx_ib_QuickStartInfoProfile_CardId",
                table: "djyx_ib_QuickStartInfoProfile",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_djyx_ib_QuickStartInfoProfile_Id",
                table: "djyx_ib_QuickStartInfoProfile",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "djyx_ib_QuickStartInfoProfile");
        }
    }
}
