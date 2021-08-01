using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StandardFramework.Migrations
{
    public partial class AddedScreenMetricModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScreenMetrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScreenName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScreenViewCount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScreenMetrics", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScreenMetrics");
        }
    }
}
