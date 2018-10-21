namespace Seminario.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoComisaria : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Municipios", "Comisaria", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Municipios", "Comisaria");
        }
    }
}
