using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UKFursBot.Migrations
{
    /// <inheritdoc />
    public partial class addedannouncementmessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnnouncementMessages",
                columns: table => new
                {
                    MessageId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    MessageContent = table.Column<string>(type: "text", nullable: false),
                    MessagePurpose = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncementMessages", x => x.MessageId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnouncementMessages");
        }
    }
}
