namespace Seminario.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntidadUsuario : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(maxLength: 40),
                        Apellido = c.String(maxLength: 40),
                        Telefono = c.String(maxLength: 8),
                        Correo = c.String(maxLength: 30),
                        User = c.String(maxLength: 20),
                        Password = c.String(maxLength: 20),
                        Municipio = c.Int(nullable: false),
                        TipoUsuario = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Usuarios");
        }
    }
}
