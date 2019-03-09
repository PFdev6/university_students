namespace University_students.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLoginToUserMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Login", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Login");
        }
    }
}
