using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Product.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProductPoolMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductIdPool",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductIdPool", x => x.Id);
                });

            migrationBuilder.Sql(@"
                WITH Numbers AS (
                    SELECT 100000 AS Number
                    UNION ALL
                    SELECT Number + 1 FROM Numbers WHERE Number < 999999
                )
                INSERT INTO ProductIdPool (Id)
                SELECT Number FROM Numbers
                OPTION (MAXRECURSION 0);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductIdPool");
        }
    }
}
