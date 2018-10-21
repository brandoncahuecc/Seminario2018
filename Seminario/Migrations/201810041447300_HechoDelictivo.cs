namespace Seminario.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HechoDelictivo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Lugars", "MunicipioId", "dbo.Municipios");
            DropForeignKey("dbo.Usuarios", "MunicipioId", "dbo.Municipios");
            CreateTable(
                "dbo.HechoDelictivoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UsuarioId = c.Int(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        LugarId = c.Int(nullable: false),
                        Direccion = c.String(nullable: false, maxLength: 100),
                        NombreVictima = c.String(nullable: false, maxLength: 60),
                        Genero = c.Int(nullable: false),
                        Edad = c.Int(nullable: false),
                        Tipo = c.String(maxLength: 30),
                        Placas = c.String(maxLength: 20),
                        Marca = c.String(maxLength: 30),
                        Color = c.String(maxLength: 20),
                        Modelo = c.String(maxLength: 10),
                        Movil = c.String(maxLength: 15),
                        Registro = c.String(maxLength: 15),
                        Calibre = c.String(maxLength: 10),
                        DelitoId = c.Int(nullable: false),
                        Causa = c.String(maxLength: 50),
                        Oficio = c.String(maxLength: 5),
                        Denunciante = c.String(maxLength: 60),
                        Observaciones = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lugars", t => t.LugarId)
                .ForeignKey("dbo.Usuarios", t => t.UsuarioId)
                .Index(t => t.UsuarioId)
                .Index(t => t.LugarId);
            
            AddForeignKey("dbo.Lugars", "MunicipioId", "dbo.Municipios", "Id");
            AddForeignKey("dbo.Usuarios", "MunicipioId", "dbo.Municipios", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Usuarios", "MunicipioId", "dbo.Municipios");
            DropForeignKey("dbo.Lugars", "MunicipioId", "dbo.Municipios");
            DropForeignKey("dbo.HechoDelictivoes", "UsuarioId", "dbo.Usuarios");
            DropForeignKey("dbo.HechoDelictivoes", "LugarId", "dbo.Lugars");
            DropIndex("dbo.HechoDelictivoes", new[] { "LugarId" });
            DropIndex("dbo.HechoDelictivoes", new[] { "UsuarioId" });
            DropTable("dbo.HechoDelictivoes");
            AddForeignKey("dbo.Usuarios", "MunicipioId", "dbo.Municipios", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Lugars", "MunicipioId", "dbo.Municipios", "Id", cascadeDelete: true);
        }
    }
}
