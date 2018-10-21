namespace Seminario.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EstadoUsuario : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuarios", "Estado", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuarios", "Estado");
        }
    }
}
