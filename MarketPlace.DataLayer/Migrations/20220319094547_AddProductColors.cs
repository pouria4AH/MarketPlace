﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketPlace.DataLayer.Migrations
{
    public partial class AddProductColors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ShortDescription",
                table: "Products",
                type: "nvarchar(230)",
                maxLength: 230,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(230)",
                oldMaxLength: 230);

            migrationBuilder.CreateTable(
                name: "ProductColors",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    ColorName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductColors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductColors_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductColors_ProductId",
                table: "ProductColors",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductColors");

            migrationBuilder.AlterColumn<string>(
                name: "ShortDescription",
                table: "Products",
                type: "nvarchar(230)",
                maxLength: 230,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(230)",
                oldMaxLength: 230,
                oldNullable: true);
        }
    }
}
