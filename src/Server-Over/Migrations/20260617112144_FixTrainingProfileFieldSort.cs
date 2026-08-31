using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerOver.Migrations
{
    /// <inheritdoc />
    public partial class FixTrainingProfileFieldSort : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "exvs2ob_training_profile",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT")
                .Annotation("Relational:ColumnOrder", 11);

            migrationBuilder.AlterColumn<uint>(
                name: "QuickFill",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "INTEGER")
                .Annotation("Relational:ColumnOrder", 15);

            migrationBuilder.AlterColumn<uint>(
                name: "Player1HpMode",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "INTEGER")
                .Annotation("Relational:ColumnOrder", 12);

            migrationBuilder.AlterColumn<uint>(
                name: "MstMobileSuitId",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "INTEGER")
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<uint>(
                name: "ExOverLimit",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "INTEGER")
                .Annotation("Relational:ColumnOrder", 14);

            migrationBuilder.AlterColumn<uint>(
                name: "ExBurstGauge",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "INTEGER")
                .Annotation("Relational:ColumnOrder", 6);

            migrationBuilder.AlterColumn<bool>(
                name: "DamageDisplay",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER")
                .Annotation("Relational:ColumnOrder", 7);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "exvs2ob_training_profile",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT")
                .Annotation("Relational:ColumnOrder", 10);

            migrationBuilder.AlterColumn<uint>(
                name: "CpuLevel",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "INTEGER")
                .Annotation("Relational:ColumnOrder", 5);

            migrationBuilder.AlterColumn<uint>(
                name: "CpuHpAutoRegen",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "INTEGER")
                .Annotation("Relational:ColumnOrder", 13);

            migrationBuilder.AlterColumn<bool>(
                name: "CpuAutoGuard",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER")
                .Annotation("Relational:ColumnOrder", 8);

            migrationBuilder.AlterColumn<uint>(
                name: "CommandGuideDisplay",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "INTEGER")
                .Annotation("Relational:ColumnOrder", 9);

            migrationBuilder.AlterColumn<int>(
                name: "CardId",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<uint>(
                name: "BurstType",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "INTEGER")
                .Annotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Relational:ColumnOrder", 1)
                .Annotation("Sqlite:Autoincrement", true)
                .OldAnnotation("Sqlite:Autoincrement", true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "exvs2ob_training_profile",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT")
                .OldAnnotation("Relational:ColumnOrder", 11);

            migrationBuilder.AlterColumn<uint>(
                name: "QuickFill",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "INTEGER")
                .OldAnnotation("Relational:ColumnOrder", 15);

            migrationBuilder.AlterColumn<uint>(
                name: "Player1HpMode",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "INTEGER")
                .OldAnnotation("Relational:ColumnOrder", 12);

            migrationBuilder.AlterColumn<uint>(
                name: "MstMobileSuitId",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "INTEGER")
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<uint>(
                name: "ExOverLimit",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "INTEGER")
                .OldAnnotation("Relational:ColumnOrder", 14);

            migrationBuilder.AlterColumn<uint>(
                name: "ExBurstGauge",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "INTEGER")
                .OldAnnotation("Relational:ColumnOrder", 6);

            migrationBuilder.AlterColumn<bool>(
                name: "DamageDisplay",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER")
                .OldAnnotation("Relational:ColumnOrder", 7);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "exvs2ob_training_profile",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT")
                .OldAnnotation("Relational:ColumnOrder", 10);

            migrationBuilder.AlterColumn<uint>(
                name: "CpuLevel",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "INTEGER")
                .OldAnnotation("Relational:ColumnOrder", 5);

            migrationBuilder.AlterColumn<uint>(
                name: "CpuHpAutoRegen",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "INTEGER")
                .OldAnnotation("Relational:ColumnOrder", 13);

            migrationBuilder.AlterColumn<bool>(
                name: "CpuAutoGuard",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER")
                .OldAnnotation("Relational:ColumnOrder", 8);

            migrationBuilder.AlterColumn<uint>(
                name: "CommandGuideDisplay",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "INTEGER")
                .OldAnnotation("Relational:ColumnOrder", 9);

            migrationBuilder.AlterColumn<int>(
                name: "CardId",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<uint>(
                name: "BurstType",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "INTEGER")
                .OldAnnotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "exvs2ob_training_profile",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true)
                .OldAnnotation("Relational:ColumnOrder", 1)
                .OldAnnotation("Sqlite:Autoincrement", true);
        }
    }
}
