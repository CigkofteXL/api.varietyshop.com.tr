using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.varietyshop.com.tr.Migrations
{
    /// <inheritdoc />
    public partial class CompleteWovSaaSSchemaFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ClanGoldMonthOfYear",
                table: "WovAccountNodes",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "ClanGemMonthOfYear",
                table: "WovAccountNodes",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "Gems",
                table: "WovAccountNodes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Gold",
                table: "WovAccountNodes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Lootboxes",
                table: "WovAccountNodes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Roses",
                table: "WovAccountNodes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gems",
                table: "WovAccountNodes");

            migrationBuilder.DropColumn(
                name: "Gold",
                table: "WovAccountNodes");

            migrationBuilder.DropColumn(
                name: "Lootboxes",
                table: "WovAccountNodes");

            migrationBuilder.DropColumn(
                name: "Roses",
                table: "WovAccountNodes");

            migrationBuilder.AlterColumn<string>(
                name: "ClanGoldMonthOfYear",
                table: "WovAccountNodes",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "ClanGemMonthOfYear",
                table: "WovAccountNodes",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
