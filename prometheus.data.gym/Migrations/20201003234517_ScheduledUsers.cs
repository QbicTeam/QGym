using Microsoft.EntityFrameworkCore.Migrations;

namespace prometheus.data.gym.Migrations
{
    public partial class ScheduledUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CovidMsg",
                table: "GeneralSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScheduledWeek",
                table: "GeneralSettings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CovidMsg",
                table: "GeneralSettings");

            migrationBuilder.DropColumn(
                name: "ScheduledWeek",
                table: "GeneralSettings");
        }
    }
}
