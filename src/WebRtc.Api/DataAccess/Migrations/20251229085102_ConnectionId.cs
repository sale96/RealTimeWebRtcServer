using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebRtc.Api.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ConnectionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConnectionId",
                table: "Clients",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectionId",
                table: "Clients");
        }
    }
}
