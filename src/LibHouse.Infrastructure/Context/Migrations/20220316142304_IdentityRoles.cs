using Microsoft.EntityFrameworkCore.Migrations;

namespace LibHouse.Infrastructure.Migrations
{
    public partial class IdentityRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "dbeb7388-004a-46b7-a2bf-3261a23778ab", "f687a39e-df13-4a4e-b9f6-126569b4fce0", "User", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4acf6277-e383-43c2-862b-e3937637a2b3", "c9917850-0154-4d88-9f96-07f22ff5943d", "Resident", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b7a1ece2-f6ac-46fd-9ae3-f6d76a9dfcee", "6f7fb254-0b44-4982-bbb9-d17272151bae", "Owner", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4acf6277-e383-43c2-862b-e3937637a2b3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b7a1ece2-f6ac-46fd-9ae3-f6d76a9dfcee");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dbeb7388-004a-46b7-a2bf-3261a23778ab");
        }
    }
}