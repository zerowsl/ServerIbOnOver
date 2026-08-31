using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerOver.Migrations
{
    /// <inheritdoc />
    public partial class AddVsRouteBattleStageDataInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "djyx_ib_VsRouteBattleStageDataInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<uint>(type: "INTEGER", nullable: false),
                    PatternId = table.Column<uint>(type: "INTEGER", nullable: false),
                    Difficulty = table.Column<uint>(type: "INTEGER", nullable: false),
                    StageId = table.Column<uint>(type: "INTEGER", nullable: false),
                    TeamCostMax = table.Column<uint>(type: "INTEGER", nullable: false),
                    TeamCostCurr = table.Column<uint>(type: "INTEGER", nullable: false),
                    MobileSuitId = table.Column<uint>(type: "INTEGER", nullable: false),
                    Chips = table.Column<string>(type: "TEXT", nullable: true),
                    BossIds = table.Column<string>(type: "TEXT", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_djyx_ib_VsRouteBattleStageDataInfo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_djyx_ib_VsRouteBattleStageDataInfo_CardId",
                table: "djyx_ib_VsRouteBattleStageDataInfo",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_djyx_ib_VsRouteBattleStageDataInfo_Id",
                table: "djyx_ib_VsRouteBattleStageDataInfo",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "djyx_ib_VsRouteBattleStageDataInfo");
        }
    }
}
