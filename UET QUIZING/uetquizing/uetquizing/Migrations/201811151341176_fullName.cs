namespace uetquizing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fullName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "fullName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "fullName");
        }
    }
}
