using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LibHouse.Infrastructure.Migrations
{
    public partial class RefreshToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Token = table.Column<string>(type: "char(71)", nullable: false),
                    JwtId = table.Column<string>(type: "varchar", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    ExpiresIn = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "idx_refresh_token",
                table: "RefreshTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

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
    }
}