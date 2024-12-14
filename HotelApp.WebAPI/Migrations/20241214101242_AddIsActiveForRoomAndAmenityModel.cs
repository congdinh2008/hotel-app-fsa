using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveForRoomAndAmenityModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Amenities",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Amenities");
        }
    }
}
