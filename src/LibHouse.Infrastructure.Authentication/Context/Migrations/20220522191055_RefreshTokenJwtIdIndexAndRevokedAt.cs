using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LibHouse.Infrastructure.Authentication.Migrations
{
    public partial class RefreshTokenJwtIdIndexAndRevokedAt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5eea6e8f-63a9-4388-b444-55df383fc0f0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b95cfeac-2dbc-4c73-b84c-57ec1ac693ad");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "de56e9d9-041f-46a9-9aa4-dbfa7ff03120");

            migrationBuilder.AlterColumn<string>(
                name: "JwtId",
                table: "RefreshTokens",
                type: "varchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "RevokedAt",
                table: "RefreshTokens",
                type: "datetime",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5e1b1251-604d-444c-a721-17f74baafbbe", "86384735-7ab0-4335-8158-d6427f17678d", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8710bab1-c798-4ee1-9072-126875506718", "f0950596-3016-4c1a-8ee4-f393b7416287", "Resident", "RESIDENT" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a5787f0c-fd63-4c73-a49c-3f32b6d31d49", "85626691-2cc2-4319-97e7-e19a813ee066", "Owner", "OWNER" });

            migrationBuilder.CreateIndex(
                name: "idx_jwt_id",
                table: "RefreshTokens",
                column: "JwtId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_jwt_id",
                table: "RefreshTokens");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5e1b1251-604d-444c-a721-17f74baafbbe");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8710bab1-c798-4ee1-9072-126875506718");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a5787f0c-fd63-4c73-a49c-3f32b6d31d49");

            migrationBuilder.DropColumn(
                name: "RevokedAt",
                table: "RefreshTokens");

            migrationBuilder.AlterColumn<string>(
                name: "JwtId",
                table: "RefreshTokens",
                type: "varchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "de56e9d9-041f-46a9-9aa4-dbfa7ff03120", "aed8c709-bb64-43b8-8291-3255599ffb5b", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5eea6e8f-63a9-4388-b444-55df383fc0f0", "f55b5e59-9bd3-4261-b34e-7ea9c2da1206", "Resident", "RESIDENT" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b95cfeac-2dbc-4c73-b84c-57ec1ac693ad", "da06ee47-133b-4c29-acce-79a8473177ee", "Owner", "OWNER" });
        }
    }
}