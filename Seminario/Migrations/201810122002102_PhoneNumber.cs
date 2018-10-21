namespace Seminario.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhoneNumber : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Usuarios", "Telefono", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Usuarios", "Telefono", c => c.String(maxLength: 8));
        }
    }
}
