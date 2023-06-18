using FluentMigrator;
using FluentMigrator.SqlServer;

namespace CultureAPI.Infrastructure.DataLayer.Migrations
{
    [Migration(2)]
    public class UsersInitiativesMigration : Migration
    {
        public override void Down()
        {
            Delete.Table("UsersInitiatives");
        }

        public override void Up()
        {
            Create.Table("UsersInitiatives")
              .WithColumn("UserId").AsInt32().NotNullable().PrimaryKey().ForeignKey("Users", "Id")
              .WithColumn("InitiativeId").AsInt32().NotNullable().PrimaryKey().ForeignKey("Initiatives", "Id");
        }
    }
}
