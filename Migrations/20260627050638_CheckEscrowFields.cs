using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.varietyshop.com.tr.Migrations
{
    /// <inheritdoc />
    public partial class CheckEscrowFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Escrows_ListingId",
                table: "Escrows",
                column: "ListingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Escrows_NexusListings_ListingId",
                table: "Escrows",
                column: "ListingId",
                principalTable: "NexusListings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Escrows_NexusListings_ListingId",
                table: "Escrows");

            migrationBuilder.DropIndex(
                name: "IX_Escrows_ListingId",
                table: "Escrows");
        }
    }
}
