using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerOver.Migrations
{
    /// <inheritdoc />
    public partial class AddIbSoloTeamClassRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_djyx_ib_QuickStartInfoProfile",
                table: "djyx_ib_QuickStartInfoProfile");

            migrationBuilder.RenameTable(
                name: "djyx_ib_QuickStartInfoProfile",
                newName: "djyx_ib_quick_start_info");

            migrationBuilder.RenameIndex(
                name: "IX_djyx_ib_QuickStartInfoProfile_Id",
                table: "djyx_ib_quick_start_info",
                newName: "IX_djyx_ib_quick_start_info_Id");

            migrationBuilder.RenameIndex(
                name: "IX_djyx_ib_QuickStartInfoProfile_CardId",
                table: "djyx_ib_quick_start_info",
                newName: "IX_djyx_ib_quick_start_info_CardId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_djyx_ib_quick_start_info",
                table: "djyx_ib_quick_start_info",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "djyx_ib_battle_solo_class_record",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClassId = table.Column<uint>(type: "INTEGER", nullable: false),
                    GradeId = table.Column<uint>(type: "INTEGER", nullable: false),
                    Rate = table.Column<float>(type: "REAL", nullable: false),
                    TopPointRank = table.Column<uint>(type: "INTEGER", nullable: false),
                    TopPointRankEntryCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    Grade_1 = table.Column<uint>(type: "INTEGER", nullable: false),
                    ClassChangeStatus = table.Column<uint>(type: "INTEGER", nullable: false),
                    WeeklyTotalBattleCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    WeeklyTotalWinCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    MonthlyTotalBattleCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    MonthlyTotalWinCount = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_djyx_ib_battle_solo_class_record", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "djyx_ib_battle_team_class_record",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClassId = table.Column<uint>(type: "INTEGER", nullable: false),
                    GradeId = table.Column<uint>(type: "INTEGER", nullable: false),
                    Rate = table.Column<float>(type: "REAL", nullable: false),
                    TopPointRank = table.Column<uint>(type: "INTEGER", nullable: false),
                    TopPointRankEntryCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    Grade_1 = table.Column<uint>(type: "INTEGER", nullable: false),
                    ClassChangeStatus = table.Column<uint>(type: "INTEGER", nullable: false),
                    WeeklyTotalBattleCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    WeeklyTotalWinCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    MonthlyTotalBattleCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    MonthlyTotalWinCount = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_djyx_ib_battle_team_class_record", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_djyx_ib_battle_solo_class_record_CardId",
                table: "djyx_ib_battle_solo_class_record",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_djyx_ib_battle_solo_class_record_Id",
                table: "djyx_ib_battle_solo_class_record",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_djyx_ib_battle_team_class_record_CardId",
                table: "djyx_ib_battle_team_class_record",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_djyx_ib_battle_team_class_record_Id",
                table: "djyx_ib_battle_team_class_record",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "djyx_ib_battle_solo_class_record");

            migrationBuilder.DropTable(
                name: "djyx_ib_battle_team_class_record");

            migrationBuilder.DropPrimaryKey(
                name: "PK_djyx_ib_quick_start_info",
                table: "djyx_ib_quick_start_info");

            migrationBuilder.RenameTable(
                name: "djyx_ib_quick_start_info",
                newName: "djyx_ib_QuickStartInfoProfile");

            migrationBuilder.RenameIndex(
                name: "IX_djyx_ib_quick_start_info_Id",
                table: "djyx_ib_QuickStartInfoProfile",
                newName: "IX_djyx_ib_QuickStartInfoProfile_Id");

            migrationBuilder.RenameIndex(
                name: "IX_djyx_ib_quick_start_info_CardId",
                table: "djyx_ib_QuickStartInfoProfile",
                newName: "IX_djyx_ib_QuickStartInfoProfile_CardId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_djyx_ib_QuickStartInfoProfile",
                table: "djyx_ib_QuickStartInfoProfile",
                column: "Id");
        }
    }
}
