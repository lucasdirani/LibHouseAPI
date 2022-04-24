using Microsoft.EntityFrameworkCore.Migrations;

namespace LibHouse.Infrastructure.Authentication.Migrations
{
    public partial class RefreshTokenJwtIdColumnLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2e876689-146f-4a62-b3c0-a651175c0232");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3db752fa-e078-4710-a258-dfdd72bdf4ac");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fd7826a3-9fa2-4fda-8aee-409a1bd2e642");

            migrationBuilder.AlterColumn<string>(
                name: "JwtId",
                table: "RefreshTokens",
                type: "varchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                type: "varchar",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2e876689-146f-4a62-b3c0-a651175c0232", "e65bbe18-95a8-4340-be44-cd61df32245f", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3db752fa-e078-4710-a258-dfdd72bdf4ac", "52d096f7-e456-4c5f-a1c8-a36b96e16108", "Resident", "RESIDENT" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fd7826a3-9fa2-4fda-8aee-409a1bd2e642", "fec09863-c516-4150-8980-8ac729e5493a", "Owner", "OWNER" });
        }
    }
}