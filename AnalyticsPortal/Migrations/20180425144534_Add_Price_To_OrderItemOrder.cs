using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AnalyticsPortal.Migrations
{
    public partial class Add_Price_To_OrderItemOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemOrder_Orders_OrderId",
                table: "OrderItemOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemOrder_OrderItems_OrderItemId",
                table: "OrderItemOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItemOrder",
                table: "OrderItemOrder");

            migrationBuilder.RenameTable(
                name: "OrderItemOrder",
                newName: "OrderItemOrders");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItemOrder_OrderItemId",
                table: "OrderItemOrders",
                newName: "IX_OrderItemOrders_OrderItemId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItemOrder_OrderId",
                table: "OrderItemOrders",
                newName: "IX_OrderItemOrders_OrderId");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "OrderItemOrders",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItemOrders",
                table: "OrderItemOrders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemOrders_Orders_OrderId",
                table: "OrderItemOrders",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemOrders_OrderItems_OrderItemId",
                table: "OrderItemOrders",
                column: "OrderItemId",
                principalTable: "OrderItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemOrders_Orders_OrderId",
                table: "OrderItemOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemOrders_OrderItems_OrderItemId",
                table: "OrderItemOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItemOrders",
                table: "OrderItemOrders");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderItemOrders");

            migrationBuilder.RenameTable(
                name: "OrderItemOrders",
                newName: "OrderItemOrder");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItemOrders_OrderItemId",
                table: "OrderItemOrder",
                newName: "IX_OrderItemOrder_OrderItemId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItemOrders_OrderId",
                table: "OrderItemOrder",
                newName: "IX_OrderItemOrder_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItemOrder",
                table: "OrderItemOrder",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemOrder_Orders_OrderId",
                table: "OrderItemOrder",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemOrder_OrderItems_OrderItemId",
                table: "OrderItemOrder",
                column: "OrderItemId",
                principalTable: "OrderItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
