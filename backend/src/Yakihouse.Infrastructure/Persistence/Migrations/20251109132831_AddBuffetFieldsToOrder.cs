using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yakihouse.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBuffetFieldsToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdultCount",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BuffetType",
                table: "Orders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Nuong");

            migrationBuilder.AddColumn<int>(
                name: "ChildCount",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ChildHeights",
                table: "Orders",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasDessertBuffet",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "KitchenStationId",
                table: "MenuItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_KitchenStationId",
                table: "MenuItems",
                column: "KitchenStationId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_KitchenStations_KitchenStationId",
                table: "MenuItems",
                column: "KitchenStationId",
                principalTable: "KitchenStations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_KitchenStations_KitchenStationId",
                table: "MenuItems");

            migrationBuilder.DropIndex(
                name: "IX_MenuItems_KitchenStationId",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "AdultCount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BuffetType",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ChildCount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ChildHeights",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "HasDessertBuffet",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "KitchenStationId",
                table: "MenuItems");
        }
    }
}
