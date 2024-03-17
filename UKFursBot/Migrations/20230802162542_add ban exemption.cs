using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UKFursBot.Migrations
{
    /// <inheritdoc />
    public partial class addbanexemption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "BansOnJoin",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ModID",
                table: "BansOnJoin",
                newName: "ModeratorId");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "BanLogs",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ModID",
                table: "BanLogs",
                newName: "ModeratorId");

            migrationBuilder.AddColumn<decimal>(
                name: "ModMailChannel",
                table: "BotConfigurations",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "BanExemptions",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    RoleType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BanExemptions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BanExemptions");

            migrationBuilder.DropColumn(
                name: "ModMailChannel",
                table: "BotConfigurations");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "BansOnJoin",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "ModeratorId",
                table: "BansOnJoin",
                newName: "ModID");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "BanLogs",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "ModeratorId",
                table: "BanLogs",
                newName: "ModID");
        }
    }
}
