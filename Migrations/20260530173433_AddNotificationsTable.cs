using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.varietyshop.com.tr.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BioVerifications");

            migrationBuilder.DropTable(
                name: "BoruClans");

            migrationBuilder.DropTable(
                name: "BoruLicenseKeys");

            migrationBuilder.DropTable(
                name: "BoruMatchRecords");

            migrationBuilder.DropTable(
                name: "BoruUsers");

            migrationBuilder.DropTable(
                name: "WovAccounts");

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    IsRead = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.CreateTable(
                name: "BioVerifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    TargetWovUsername = table.Column<string>(type: "TEXT", nullable: false),
                    VerificationCode = table.Column<string>(type: "TEXT", nullable: false),
                    WebAccountId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BioVerifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BoruClans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ExpireDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    WovId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoruClans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BoruDownloadLinks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedByWebAccountId = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    MaxUses = table.Column<int>(type: "INTEGER", nullable: false),
                    Token = table.Column<string>(type: "TEXT", nullable: false),
                    UsedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoruDownloadLinks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BoruLicenseKeys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsUsed = table.Column<bool>(type: "INTEGER", nullable: false),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    KeyType = table.Column<string>(type: "TEXT", nullable: false),
                    UsedByAccountId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Value = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoruLicenseKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BoruMatchRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    BotId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClanId = table.Column<string>(type: "TEXT", nullable: true),
                    ClanQuestXpContributed = table.Column<int>(type: "INTEGER", nullable: false),
                    CompletedSuccessfully = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CurrentLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    DisconnectReason = table.Column<string>(type: "TEXT", nullable: true),
                    GoldGained = table.Column<int>(type: "INTEGER", nullable: false),
                    IsMobile = table.Column<bool>(type: "INTEGER", nullable: false),
                    MatchDurationSec = table.Column<int>(type: "INTEGER", nullable: false),
                    MatchResult = table.Column<string>(type: "TEXT", nullable: false),
                    PlayedRole = table.Column<string>(type: "TEXT", nullable: false),
                    RosesReceived = table.Column<int>(type: "INTEGER", nullable: false),
                    RosesSent = table.Column<int>(type: "INTEGER", nullable: false),
                    Survived = table.Column<bool>(type: "INTEGER", nullable: false),
                    WasKicked = table.Column<bool>(type: "INTEGER", nullable: false),
                    WovUsername = table.Column<string>(type: "TEXT", nullable: false),
                    XpGained = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoruMatchRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SwarmLobbies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AutoOpenLootBoxes = table.Column<bool>(type: "INTEGER", nullable: false),
                    AutoSpinGoldWheel = table.Column<bool>(type: "INTEGER", nullable: false),
                    AutoSpinRoseWheel = table.Column<bool>(type: "INTEGER", nullable: false),
                    LobbyName = table.Column<string>(type: "TEXT", nullable: false),
                    OwnerWebAccountId = table.Column<string>(type: "TEXT", nullable: false),
                    ScheduledTime = table.Column<string>(type: "TEXT", nullable: false),
                    TargetHostName = table.Column<string>(type: "TEXT", nullable: false),
                    TargetPassword = table.Column<string>(type: "TEXT", nullable: false),
                    TargetRoomName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwarmLobbies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WovAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClanId = table.Column<string>(type: "TEXT", nullable: true),
                    CurrentXP = table.Column<int>(type: "INTEGER", nullable: false),
                    EarnedGold = table.Column<int>(type: "INTEGER", nullable: false),
                    EncryptedPassword = table.Column<string>(type: "TEXT", nullable: false),
                    FormationRole = table.Column<string>(type: "TEXT", nullable: false),
                    LastAssignedProxy = table.Column<string>(type: "TEXT", nullable: true),
                    LastHeartbeat = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerId = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WovAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WovAccounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BoruDownloadLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    BoruDownloadLinkId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DownloadedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IpAddress = table.Column<string>(type: "TEXT", nullable: false),
                    UserAgent = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoruDownloadLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoruDownloadLogs_BoruDownloadLinks_BoruDownloadLinkId",
                        column: x => x.BoruDownloadLinkId,
                        principalTable: "BoruDownloadLinks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BoruUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AssignedSwarmLobbyId = table.Column<Guid>(type: "TEXT", nullable: true),
                    AdminNote = table.Column<string>(type: "TEXT", nullable: true),
                    AutoRenewFromWallet = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    FrozenRemainingDays = table.Column<int>(type: "INTEGER", nullable: true),
                    IsFrozen = table.Column<bool>(type: "INTEGER", nullable: false),
                    LinkedWebAccountId = table.Column<string>(type: "TEXT", nullable: true),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    WovId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoruUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoruUsers_SwarmLobbies_AssignedSwarmLobbyId",
                        column: x => x.AssignedSwarmLobbyId,
                        principalTable: "SwarmLobbies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoruDownloadLogs_BoruDownloadLinkId",
                table: "BoruDownloadLogs",
                column: "BoruDownloadLinkId");

            migrationBuilder.CreateIndex(
                name: "IX_BoruUsers_AssignedSwarmLobbyId",
                table: "BoruUsers",
                column: "AssignedSwarmLobbyId");

            migrationBuilder.CreateIndex(
                name: "IX_WovAccounts_UserId",
                table: "WovAccounts",
                column: "UserId");
        }
    }
}
