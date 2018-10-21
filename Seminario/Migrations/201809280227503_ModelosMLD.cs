namespace Seminario.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelosMLD : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Delitoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 50),
                        TipoDelito = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Nombre, unique: true);
            
            CreateTable(
                "dbo.Lugars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MunicipioId = c.Int(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Municipios", t => t.MunicipioId, cascadeDelete: true)
                .Index(t => t.MunicipioId)
                .Index(t => t.Nombre, unique: true);
            
            CreateTable(
                "dbo.Municipios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 25),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Nombre, unique: true);
            
            AddColumn("dbo.Usuarios", "MunicipioId", c => c.Int(nullable: false));
            AlterColumn("dbo.Usuarios", "Nombre", c => c.String(nullable: false, maxLength: 40));
            AlterColumn("dbo.Usuarios", "Apellido", c => c.String(nullable: false, maxLength: 40));
            AlterColumn("dbo.Usuarios", "User", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Usuarios", "Password", c => c.String(nullable: false, maxLength: 20));
            CreateIndex("dbo.Usuarios", "MunicipioId");
            AddForeignKey("dbo.Usuarios", "MunicipioId", "dbo.Municipios", "Id", cascadeDelete: true);
            DropColumn("dbo.Usuarios", "Municipio");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Usuarios", "Municipio", c => c.Int(nullable: false));
            DropForeignKey("dbo.Usuarios", "MunicipioId", "dbo.Municipios");
            DropForeignKey("dbo.Lugars", "MunicipioId", "dbo.Municipios");
            DropIndex("dbo.Usuarios", new[] { "MunicipioId" });
            DropIndex("dbo.Municipios", new[] { "Nombre" });
            DropIndex("dbo.Lugars", new[] { "Nombre" });
            DropIndex("dbo.Lugars", new[] { "MunicipioId" });
            DropIndex("dbo.Delitoes", new[] { "Nombre" });
            AlterColumn("dbo.Usuarios", "Password", c => c.String(maxLength: 20));
            AlterColumn("dbo.Usuarios", "User", c => c.String(maxLength: 20));
            AlterColumn("dbo.Usuarios", "Apellido", c => c.String(maxLength: 40));
            AlterColumn("dbo.Usuarios", "Nombre", c => c.String(maxLength: 40));
            DropColumn("dbo.Usuarios", "MunicipioId");
            DropTable("dbo.Municipios");
            DropTable("dbo.Lugars");
            DropTable("dbo.Delitoes");
        }
    }
}
