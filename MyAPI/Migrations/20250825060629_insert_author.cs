using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyAPI.Migrations
{
    /// <inheritdoc />
    public partial class insert_author : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string[] firstName = { "Vincent", "Angela", "Gordon", "Tracy", "Edward" };
            string[] lastName = { "Ambleton", "Maple", "Sampson", "Hanley", "Connor" };
            DateTime[] dateOfBirth = { new DateTime(1990, 2, 2), new DateTime(1980, 12, 25), new DateTime(1985, 5, 9),
            new DateTime(1972, 4, 16), new DateTime(1983, 8, 22) };
            string[] mainCategory = { "Business", "Travel", "Computer", "Business", "Computer" };
            for (int i = 0; i < 5; i++)
            {
                migrationBuilder.Sql($"insert into authors (firstname, lastname, dateofbirth,maincategory) " +
                $"values('{firstName[i]}', '{lastName[i]}', '{dateOfBirth[i]}', '{mainCategory[i]}')");
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
