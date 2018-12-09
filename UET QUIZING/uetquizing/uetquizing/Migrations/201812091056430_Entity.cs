namespace uetquizing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Entity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "userRole", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "userRole");
        }
    }
}
