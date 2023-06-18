using FluentMigrator;
using FluentMigrator.SqlServer;


namespace CultureAPI.Infrastructure.DataLayer.Migrations
{
    [Migration(1)]
    public class InitialMigration : Migration
    {
        public override void Down()
        {
            Delete.Table("Users");
            Delete.Table("RefreshTokens");
            Delete.Table("News");
            Delete.Table("Initiatives");
        }

        public override void Up()
        {
            Create.Table("Users")
               .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity(1, 1)
               .WithColumn("Name").AsString(50).NotNullable()
               .WithColumn("Email").AsString(50).NotNullable().Unique()
               .WithColumn("Password").AsString().NotNullable()
               .WithColumn("PasswordSalt").AsString(20).NotNullable();

            Create.Table("RefreshTokens")
               .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity(1, 1)
               .WithColumn("UserId").AsInt32().NotNullable().ForeignKey("Users", "Id")
               .WithColumn("Token").AsString().NotNullable().Unique()
               .WithColumn("JwtId").AsString().NotNullable().Unique()
               .WithColumn("isUsed").AsBoolean().NotNullable()
               .WithColumn("isRevoked").AsBoolean().NotNullable()
               .WithColumn("AddedDate").AsDateTime().NotNullable()
               .WithColumn("ExpiryDate").AsDateTime().NotNullable();

            Create.Table("News")
              .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity(1, 1)
              .WithColumn("UserId").AsInt32().NotNullable().ForeignKey("Users", "Id")
              .WithColumn("Title").AsString().Nullable()
              .WithColumn("Content").AsString().NotNullable()
              .WithColumn("PublicationDate").AsString().NotNullable()
              .WithColumn("Image").AsString().Nullable();

            Create.Table("Initiatives")
              .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity(1, 1)
              .WithColumn("UserId").AsInt32().NotNullable().ForeignKey("Users", "Id")
              .WithColumn("Username").AsString(50).NotNullable()
              .WithColumn("Title").AsString().Nullable()
              .WithColumn("CreationDate").AsString().NotNullable()
              .WithColumn("NumberOfSupporters").AsInt32().NotNullable()
              .WithColumn("Description").AsString(1000).Nullable();


        }
    }
}
