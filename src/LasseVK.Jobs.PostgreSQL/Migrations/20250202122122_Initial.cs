using Microsoft.EntityFrameworkCore.Migrations;

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
                    identifier = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    group = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    json = table.Column<string>(type: "TEXT", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jobs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "JobEntityJobEntity",
                columns: table => new
                {
                    DependsOnId = table.Column<string>(type: "character varying(32)", nullable: false),
                    DependsOnMeId = table.Column<string>(type: "character varying(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobEntityJobEntity", x => new { x.DependsOnId, x.DependsOnMeId });
                    table.ForeignKey(
                        name: "FK_JobEntityJobEntity_jobs_DependsOnId",
                        column: x => x.DependsOnId,
                        principalTable: "jobs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobEntityJobEntity_jobs_DependsOnMeId",
                        column: x => x.DependsOnMeId,
                        principalTable: "jobs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobEntityJobEntity_DependsOnMeId",
                table: "JobEntityJobEntity",
                column: "DependsOnMeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobEntityJobEntity");

            migrationBuilder.DropTable(
                name: "jobs");
        }
    }
}
