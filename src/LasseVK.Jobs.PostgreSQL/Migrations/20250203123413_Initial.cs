using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LasseVK.Jobs.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "jobs",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    group = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    json = table.Column<string>(type: "json", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    queued = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    started = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    completed = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jobs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dependencies",
                columns: table => new
                {
                    dependencyId = table.Column<string>(type: "character varying(32)", nullable: false),
                    dependentId = table.Column<string>(type: "character varying(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dependencies", x => new { x.dependencyId, x.dependentId });
                    table.ForeignKey(
                        name: "FK_dependencies_jobs_dependencyId",
                        column: x => x.dependencyId,
                        principalTable: "jobs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dependencies_jobs_dependentId",
                        column: x => x.dependentId,
                        principalTable: "jobs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "logs",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    job = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    when = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    log = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logs", x => x.id);
                    table.ForeignKey(
                        name: "FK_logs_jobs_job",
                        column: x => x.job,
                        principalTable: "jobs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dependencies_dependentId",
                table: "dependencies",
                column: "dependentId");

            migrationBuilder.CreateIndex(
                name: "logs_job_id",
                table: "logs",
                column: "job");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dependencies");

            migrationBuilder.DropTable(
                name: "logs");

            migrationBuilder.DropTable(
                name: "jobs");
        }
    }
}
