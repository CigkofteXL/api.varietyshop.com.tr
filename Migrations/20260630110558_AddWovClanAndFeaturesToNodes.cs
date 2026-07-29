using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.varietyshop.com.tr.Migrations
{
    /// <inheritdoc />
    public partial class AddWovClanAndFeaturesToNodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AutoAvatarSlot",
                table: "WovAccountNodes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AutoSkipQuest",
                table: "WovAccountNodes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AutoTalisman",
                table: "WovAccountNodes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AutoXpBoost",
                table: "WovAccountNodes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ClanGemAmount",
                table: "WovAccountNodes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClanGemDayOfMonth",
                table: "WovAccountNodes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClanGemDayOfWeek",
                table: "WovAccountNodes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClanGemDayOfYear",
                table: "WovAccountNodes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ClanGemMonthOfYear",
                table: "WovAccountNodes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClanGemScheduleType",
                table: "WovAccountNodes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ClanGoldAmount",
                table: "WovAccountNodes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClanGoldDayOfMonth",
                table: "WovAccountNodes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClanGoldDayOfWeek",
                table: "WovAccountNodes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClanGoldDayOfYear",
                table: "WovAccountNodes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ClanGoldMonthOfYear",
                table: "WovAccountNodes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClanGoldScheduleType",
                table: "WovAccountNodes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "ExchangeNetwork",
                table: "WovAccountNodes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RandomEmoteSender",
                table: "WovAccountNodes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RoleCardOptimizer",
                table: "WovAccountNodes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoAvatarSlot",
                table: "WovAccountNodes");

            migrationBuilder.DropColumn(
                name: "AutoSkipQuest",
                table: "WovAccountNodes");

            migrationBuilder.DropColumn(
                name: "AutoTalisman",
                table: "WovAccountNodes");

            migrationBuilder.DropColumn(
                name: "AutoXpBoost",
                table: "WovAccountNodes");

            migrationBuilder.DropColumn(
                name: "ClanGemAmount",
                table: "WovAccountNodes");

            migrationBuilder.DropColumn(
                name: "ClanGemDayOfMonth",
                table: "WovAccountNodes");

            migrationBuilder.DropColumn(
                name: "ClanGemDayOfWeek",
                table: "WovAccountNodes");

            migrationBuilder.DropColumn(
                name: "ClanGemDayOfYear",
                table: "WovAccountNodes");

            migrationBuilder.DropColumn(
                name: "ClanGemMonthOfYear",
                table: "WovAccountNodes");

            migrationBuilder.DropColumn(
                name: "ClanGemScheduleType",
                table: "WovAccountNodes");

            migrationBuilder.DropColumn(
                name: "ClanGoldAmount",
                table: "WovAccountNodes");

            migrationBuilder.DropColumn(
                name: "ClanGoldDayOfMonth",
                table: "WovAccountNodes");

            migrationBuilder.DropColumn(
                name: "ClanGoldDayOfWeek",
                table: "WovAccountNodes");

            migrationBuilder.DropColumn(
                name: "ClanGoldDayOfYear",
                table: "WovAccountNodes");

            migrationBuilder.DropColumn(
                name: "ClanGoldMonthOfYear",
                table: "WovAccountNodes");

            migrationBuilder.DropColumn(
                name: "ClanGoldScheduleType",
                table: "WovAccountNodes");

            migrationBuilder.DropColumn(
                name: "ExchangeNetwork",
                table: "WovAccountNodes");

            migrationBuilder.DropColumn(
                name: "RandomEmoteSender",
                table: "WovAccountNodes");

            migrationBuilder.DropColumn(
                name: "RoleCardOptimizer",
                table: "WovAccountNodes");
        }
    }
}
