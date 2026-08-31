using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerOver.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayerBadge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "djyx_ib_battle_player_badge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<uint>(type: "INTEGER", nullable: false),
                    BadgeId = table.Column<uint>(type: "INTEGER", nullable: false),
                    BadgeExp = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_djyx_ib_battle_player_badge", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_djyx_ib_battle_player_badge_CardId",
                table: "djyx_ib_battle_player_badge",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_djyx_ib_battle_player_badge_Id",
                table: "djyx_ib_battle_player_badge",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "djyx_ib_battle_player_badge");
        }
    }
}
