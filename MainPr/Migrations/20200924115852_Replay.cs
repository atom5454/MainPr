using Microsoft.EntityFrameworkCore.Migrations;

namespace MainPr.Migrations
{
    public partial class Replay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3b62472e-4f66-49fa-a20f-e7685b9125d8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "64a9a90e-4160-4745-a5bb-b7e5e1258d70", "AQAAAAEAACcQAAAAEMlA12yWjZLptGlwxluGZzTE5tJdmTkQ9rUxwNDiEh9Rmfy8gZ17jf4Ez1P3CWKabQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3b62472e-4f66-49fa-a20f-e7685b9565d8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "84c4965c-9c4e-49b0-944b-4f8323235e54", "AQAAAAEAACcQAAAAEKNoaTGYUyJT3gthHQhPRlQb4N+Sl/4WazvOoR60UBcRw3ZAWfIkNR+zWe3l+lIRFA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3b62472e-4f66-49fa-a20f-e7685b9125d8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "498864df-9180-4022-bb13-e0d14cf96da9", "AQAAAAEAACcQAAAAEMCevGt5p12BshHBA/RrVGRsDfvBwEYWsU0XV0pgmiyos7ekIFrb1c8QnyXl2tDywA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3b62472e-4f66-49fa-a20f-e7685b9565d8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f8cf4ae3-1cc3-4f37-9cbe-8d24e414448c", "AQAAAAEAACcQAAAAEIZcm/zWyRtQ9Fdx63o5lYKejFId+2EnJsp0+FR80CMN6ZZdSABa9XNCQFRxfmEXOQ==" });
        }
    }
}
