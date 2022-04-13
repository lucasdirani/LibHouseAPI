using Microsoft.EntityFrameworkCore.Migrations;

namespace LibHouse.Infrastructure.Authentication.Migrations
{
    public partial class IdentityRolesNormalizedName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3af39a61-a179-4072-9bc1-d792fc3e6bf3", "7aaf5932-3199-43cc-9c71-e165827c1308", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "acc880ff-e87e-467b-9e62-d8df310dd639", "f0edea99-2ebd-403d-9474-86854f0e6398", "Resident", "RESIDENT" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "44cb36e8-5762-4f44-bba8-4ba95559f663", "a8857822-ed92-471f-bc76-b1c1f9d736e0", "Owner", "OWNER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3af39a61-a179-4072-9bc1-d792fc3e6bf3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44cb36e8-5762-4f44-bba8-4ba95559f663");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "acc880ff-e87e-467b-9e62-d8df310dd639");

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
    }
}
