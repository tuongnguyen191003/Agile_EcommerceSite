using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agile_Ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class AddICollectionOrderDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderModelId",
                table: "OrderDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderModelId",
                table: "OrderDetails",
                column: "OrderModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Orders_OrderModelId",
                table: "OrderDetails",
                column: "OrderModelId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Orders_OrderModelId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_OrderModelId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "OrderModelId",
                table: "OrderDetails");
        }
    }
}
