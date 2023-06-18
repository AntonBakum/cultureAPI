using FluentMigrator;
using FluentMigrator.SqlServer;

namespace CultureAPI.Infrastructure.DataLayer.Migrations
{
    [Migration(3)]
    public class UpdateUserMigration : Migration
    {
        public override void Down()
        {
            
        }

        public override void Up()
        {
            Alter.Table("Users").AddColumn("Phone").AsString(15).Nullable();
            Alter.Table("Users").AddColumn("Nickname").AsString(50).Nullable();
            
        }
    }
}
