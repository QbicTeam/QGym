using Microsoft.EntityFrameworkCore.Migrations;

namespace prometheus.data.gym.Migrations
{
    public partial class AddConfigGeneralNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NotificationCapacity",
                table: "GeneralSettings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationCapacity",
                table: "GeneralSettings");
        }
    }
}
