using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerOver.Migrations
{
    /// <inheritdoc />
    public partial class AddVsRouteBattleStageHighScoreInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "djyx_ib_VsRouteBattleStageDataInfo");

            migrationBuilder.CreateTable(
                name: "djyx_ib_VsRouteBattleStageHighScoreInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    Difficulty = table.Column<uint>(type: "INTEGER", nullable: false),
                    ClearTime1 = table.Column<uint>(type: "INTEGER", nullable: false),
                    ClearScore1 = table.Column<uint>(type: "INTEGER", nullable: false),
                    ClearTime2 = table.Column<uint>(type: "INTEGER", nullable: false),
                    ClearScore2 = table.Column<uint>(type: "INTEGER", nullable: false),
                    ClearTime3 = table.Column<uint>(type: "INTEGER", nullable: false),
                    ClearScore3 = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalClearTime = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalClearScore = table.Column<uint>(type: "INTEGER", nullable: false),
                    IsRecord = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_djyx_ib_VsRouteBattleStageHighScoreInfo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_djyx_ib_VsRouteBattleStageHighScoreInfo_CardId",
                table: "djyx_ib_VsRouteBattleStageHighScoreInfo",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_djyx_ib_VsRouteBattleStageHighScoreInfo_Id",
                table: "djyx_ib_VsRouteBattleStageHighScoreInfo",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "djyx_ib_VsRouteBattleStageHighScoreInfo");

            migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                table: "djyx_ib_VsRouteBattleStageDataInfo",
                type: "BLOB",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 13);
        }
    }
}
