using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UKFursBot.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBanExemption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BanExemptions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
