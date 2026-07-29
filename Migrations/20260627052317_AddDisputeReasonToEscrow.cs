using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.varietyshop.com.tr.Migrations
{
    /// <inheritdoc />
    public partial class AddDisputeReasonToEscrow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisputeReason",
                table: "Escrows",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisputeReason",
                table: "Escrows");
        }
    }
}
