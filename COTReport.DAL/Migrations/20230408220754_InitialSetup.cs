using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace COTReport.DAL.Migrations
{
    public partial class InitialSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cot_reports",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    report_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    instrument = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    total_long = table.Column<decimal>(type: "numeric", nullable: false),
                    total_short = table.Column<decimal>(type: "numeric", nullable: false),
                    change_long = table.Column<decimal>(type: "numeric", nullable: true),
                    change_short = table.Column<decimal>(type: "numeric", nullable: true),
                    percent_long = table.Column<decimal>(type: "numeric", nullable: true),
                    percent_short = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cot_reports", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cot_reports");
        }
    }
}
