using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LasseVK.Jobs.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class RenameTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobEntityJobEntity_jobs_DependsOnId",
                table: "JobEntityJobEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_JobEntityJobEntity_jobs_DependsOnMeId",
                table: "JobEntityJobEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_logs_jobs_job",
                table: "logs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_jobs",
                table: "jobs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_logs",
                table: "logs");

            migrationBuilder.RenameTable(
                name: "jobs",
                newName: "Jobs");

            migrationBuilder.RenameTable(
                name: "logs",
                newName: "JobLogs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Jobs",
                table: "Jobs",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobLogs",
                table: "JobLogs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobEntityJobEntity_Jobs_DependsOnId",
                table: "JobEntityJobEntity",
                column: "DependsOnId",
                principalTable: "Jobs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobEntityJobEntity_Jobs_DependsOnMeId",
                table: "JobEntityJobEntity",
                column: "DependsOnMeId",
                principalTable: "Jobs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobLogs_Jobs_job",
                table: "JobLogs",
                column: "job",
                principalTable: "Jobs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobEntityJobEntity_Jobs_DependsOnId",
                table: "JobEntityJobEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_JobEntityJobEntity_Jobs_DependsOnMeId",
                table: "JobEntityJobEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_JobLogs_Jobs_job",
                table: "JobLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Jobs",
                table: "Jobs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobLogs",
                table: "JobLogs");

            migrationBuilder.RenameTable(
                name: "Jobs",
                newName: "jobs");

            migrationBuilder.RenameTable(
                name: "JobLogs",
                newName: "logs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_jobs",
                table: "jobs",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_logs",
                table: "logs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobEntityJobEntity_jobs_DependsOnId",
                table: "JobEntityJobEntity",
                column: "DependsOnId",
                principalTable: "jobs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobEntityJobEntity_jobs_DependsOnMeId",
                table: "JobEntityJobEntity",
                column: "DependsOnMeId",
                principalTable: "jobs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_logs_jobs_job",
                table: "logs",
                column: "job",
                principalTable: "jobs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
