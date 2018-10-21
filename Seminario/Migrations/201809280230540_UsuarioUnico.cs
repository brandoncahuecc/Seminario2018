namespace Seminario.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsuarioUnico : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Usuarios", "User", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Usuarios", new[] { "User" });
        }
    }
}
