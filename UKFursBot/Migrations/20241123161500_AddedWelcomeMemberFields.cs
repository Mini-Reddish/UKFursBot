using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UKFursBot.Migrations
{
    /// <inheritdoc />
    public partial class AddedWelcomeMemberFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MemberRoleId",
                table: "BotConfigurations",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MemberWelcomeChannelId",
                table: "BotConfigurations",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "MemberWelcomeMessage",
                table: "BotConfigurations",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemberRoleId",
                table: "BotConfigurations");

            migrationBuilder.DropColumn(
                name: "MemberWelcomeChannelId",
                table: "BotConfigurations");

            migrationBuilder.DropColumn(
                name: "MemberWelcomeMessage",
                table: "BotConfigurations");
        }
    }
}
