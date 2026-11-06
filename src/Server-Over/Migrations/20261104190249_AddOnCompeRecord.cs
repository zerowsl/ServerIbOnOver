using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerOver.Migrations
{
    /// <inheritdoc />
    public partial class AddOnCompeRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "djyx_ib_battle_solo_oncompe_record",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    RoomId = table.Column<uint>(type: "INTEGER", nullable: false),
                    Point = table.Column<uint>(type: "INTEGER", nullable: false),
                    Rank = table.Column<uint>(type: "INTEGER", nullable: false),
                    IsOutdated = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_djyx_ib_battle_solo_oncompe_record", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "djyx_ib_battle_team_oncompe_record",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    RoomId = table.Column<uint>(type: "INTEGER", nullable: false),
                    Point = table.Column<uint>(type: "INTEGER", nullable: false),
                    Rank = table.Column<uint>(type: "INTEGER", nullable: false),
                    IsOutdated = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_djyx_ib_battle_team_oncompe_record", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_djyx_ib_battle_solo_oncompe_record_CardId",
                table: "djyx_ib_battle_solo_oncompe_record",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_djyx_ib_battle_solo_oncompe_record_Id",
                table: "djyx_ib_battle_solo_oncompe_record",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_djyx_ib_battle_team_oncompe_record_CardId",
                table: "djyx_ib_battle_team_oncompe_record",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_djyx_ib_battle_team_oncompe_record_Id",
                table: "djyx_ib_battle_team_oncompe_record",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "djyx_ib_battle_solo_oncompe_record");

            migrationBuilder.DropTable(
                name: "djyx_ib_battle_team_oncompe_record");
        }
    }
}
