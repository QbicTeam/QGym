using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace prometheus.data.gym.Migrations
{
    public partial class CreateUserScheduling2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSchedulings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    Schedule = table.Column<DateTime>(nullable: false),
                    IsAttended = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSchedulings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSchedulings_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "securitas",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSchedulings_UserId",
                table: "UserSchedulings",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSchedulings");
        }
    }
}
