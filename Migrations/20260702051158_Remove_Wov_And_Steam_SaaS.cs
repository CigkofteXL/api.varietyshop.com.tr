using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.varietyshop.com.tr.Migrations
{
    /// <inheritdoc />
    public partial class Remove_Wov_And_Steam_SaaS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SteamBoostNodes");

            migrationBuilder.DropTable(
                name: "SteamLicenseSubscriptions");

            migrationBuilder.DropTable(
                name: "WovAccountNodes");

            migrationBuilder.DropTable(
                name: "WovLicenseSubscriptions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SteamBoostNodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AccountName = table.Column<string>(type: "TEXT", nullable: false),
                    AppIds = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CustomTitle = table.Column<string>(type: "TEXT", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    SharedSecret = table.Column<string>(type: "TEXT", nullable: false),
                    SlotIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    TotalBoostedHours = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SteamBoostNodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SteamLicenseSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AllowedSlots = table.Column<int>(type: "INTEGER", nullable: false),
                    LimitHours = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UsedHours = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SteamLicenseSubscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WovAccountNodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AutoAvatarSlot = table.Column<bool>(type: "INTEGER", nullable: false),
                    AutoSkipQuest = table.Column<bool>(type: "INTEGER", nullable: false),
                    AutoStartInactiveHours = table.Column<int>(type: "INTEGER", nullable: false),
                    AutoStopXp = table.Column<long>(type: "INTEGER", nullable: false),
                    AutoTalisman = table.Column<bool>(type: "INTEGER", nullable: false),
                    AutoXpBoost = table.Column<bool>(type: "INTEGER", nullable: false),
                    ClanGemAmount = table.Column<int>(type: "INTEGER", nullable: false),
                    ClanGemDayOfMonth = table.Column<int>(type: "INTEGER", nullable: false),
                    ClanGemDayOfWeek = table.Column<int>(type: "INTEGER", nullable: false),
                    ClanGemDayOfYear = table.Column<int>(type: "INTEGER", nullable: false),
                    ClanGemMonthOfYear = table.Column<int>(type: "INTEGER", nullable: false),
                    ClanGemScheduleType = table.Column<string>(type: "TEXT", nullable: false),
                    ClanGoldAmount = table.Column<int>(type: "INTEGER", nullable: false),
                    ClanGoldDayOfMonth = table.Column<int>(type: "INTEGER", nullable: false),
                    ClanGoldDayOfWeek = table.Column<int>(type: "INTEGER", nullable: false),
                    ClanGoldDayOfYear = table.Column<int>(type: "INTEGER", nullable: false),
                    ClanGoldMonthOfYear = table.Column<int>(type: "INTEGER", nullable: false),
                    ClanGoldScheduleType = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DailyXp = table.Column<long>(type: "INTEGER", nullable: false),
                    ExchangeNetwork = table.Column<bool>(type: "INTEGER", nullable: false),
                    GameCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Gems = table.Column<int>(type: "INTEGER", nullable: false),
                    Gold = table.Column<int>(type: "INTEGER", nullable: false),
                    HourlyXp = table.Column<long>(type: "INTEGER", nullable: false),
                    Lootboxes = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PlayingRoomName = table.Column<string>(type: "TEXT", nullable: false),
                    RandomEmoteSender = table.Column<bool>(type: "INTEGER", nullable: false),
                    RoleCardOptimizer = table.Column<bool>(type: "INTEGER", nullable: false),
                    Roses = table.Column<int>(type: "INTEGER", nullable: false),
                    SessionXp = table.Column<long>(type: "INTEGER", nullable: false),
                    SlotIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    VillagerRate = table.Column<int>(type: "INTEGER", nullable: false),
                    WolfRate = table.Column<int>(type: "INTEGER", nullable: false),
                    WovEmail = table.Column<string>(type: "TEXT", nullable: false),
                    WovPasswordHash = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WovAccountNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WovAccountNodes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WovLicenseSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AllowedSlots = table.Column<int>(type: "INTEGER", nullable: false),
                    LicenseEmail = table.Column<string>(type: "TEXT", nullable: false),
                    LimitXp = table.Column<long>(type: "INTEGER", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UsedXp = table.Column<long>(type: "INTEGER", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WovLicenseSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WovLicenseSubscriptions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WovAccountNodes_UserId",
                table: "WovAccountNodes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WovLicenseSubscriptions_UserId",
                table: "WovLicenseSubscriptions",
                column: "UserId",
                unique: true);
        }
    }
}
