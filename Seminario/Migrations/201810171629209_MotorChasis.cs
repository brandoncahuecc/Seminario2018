namespace Seminario.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MotorChasis : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HechoDelictivoes", "Motor", c => c.String(maxLength: 20));
            AddColumn("dbo.HechoDelictivoes", "Chasis", c => c.String(maxLength: 20));
            AddColumn("dbo.Municipios", "SubEstacion", c => c.Int(nullable: false));
            AlterColumn("dbo.HechoDelictivoes", "Modelo", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.HechoDelictivoes", "Modelo", c => c.String(maxLength: 10));
            DropColumn("dbo.Municipios", "SubEstacion");
            DropColumn("dbo.HechoDelictivoes", "Chasis");
            DropColumn("dbo.HechoDelictivoes", "Motor");
        }
    }
}
