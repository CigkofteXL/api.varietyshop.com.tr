using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.varietyshop.com.tr.Migrations
{
    /// <inheritdoc />
    public partial class WOVDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WovAccountNodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SlotIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    WovEmail = table.Column<string>(type: "TEXT", nullable: false),
                    WovPasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    PlayingRoomName = table.Column<string>(type: "TEXT", nullable: false),
                    HourlyXp = table.Column<long>(type: "INTEGER", nullable: false),
                    DailyXp = table.Column<long>(type: "INTEGER", nullable: false),
                    SessionXp = table.Column<long>(type: "INTEGER", nullable: false),
                    GameCount = table.Column<int>(type: "INTEGER", nullable: false),
                    VillagerRate = table.Column<int>(type: "INTEGER", nullable: false),
                    WolfRate = table.Column<int>(type: "INTEGER", nullable: false),
                    AutoStopXp = table.Column<long>(type: "INTEGER", nullable: false),
                    AutoStartInactiveHours = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
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
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UsedXp = table.Column<long>(type: "INTEGER", nullable: false),
                    LimitXp = table.Column<long>(type: "INTEGER", nullable: false),
                    AllowedSlots = table.Column<int>(type: "INTEGER", nullable: false),
                    LicenseEmail = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WovAccountNodes");

            migrationBuilder.DropTable(
                name: "WovLicenseSubscriptions");
        }
    }
}
