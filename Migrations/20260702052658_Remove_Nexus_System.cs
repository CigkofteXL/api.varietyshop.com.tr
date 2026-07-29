using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.varietyshop.com.tr.Migrations
{
    /// <inheritdoc />
    public partial class Remove_Nexus_System : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Escrows_NexusListings_ListingId",
                table: "Escrows");

            migrationBuilder.DropTable(
                name: "NexusListings");

            migrationBuilder.DropIndex(
                name: "IX_Escrows_ListingId",
                table: "Escrows");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NexusListings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SellerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    Stock = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NexusListings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NexusListings_Users_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Escrows_ListingId",
                table: "Escrows",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_NexusListings_SellerId",
                table: "NexusListings",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Escrows_NexusListings_ListingId",
                table: "Escrows",
                column: "ListingId",
                principalTable: "NexusListings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
