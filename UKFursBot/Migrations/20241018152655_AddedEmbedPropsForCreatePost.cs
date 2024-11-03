using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UKFursBot.Migrations
{
    /// <inheritdoc />
    public partial class AddedEmbedPropsForCreatePost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EmbedColour",
                table: "CreatePosts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "UseEmbed",
                table: "CreatePosts",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmbedColour",
                table: "CreatePosts");

            migrationBuilder.DropColumn(
                name: "UseEmbed",
                table: "CreatePosts");
        }
    }
}
