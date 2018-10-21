namespace Seminario.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregandoFecha : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HechoDelictivoes", "FechaIngreso", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HechoDelictivoes", "FechaIngreso");
        }
    }
}
