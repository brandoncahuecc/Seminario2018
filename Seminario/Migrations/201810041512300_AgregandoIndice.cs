namespace Seminario.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregandoIndice : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.HechoDelictivoes", "DelitoId");
            AddForeignKey("dbo.HechoDelictivoes", "DelitoId", "dbo.Delitoes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HechoDelictivoes", "DelitoId", "dbo.Delitoes");
            DropIndex("dbo.HechoDelictivoes", new[] { "DelitoId" });
        }
    }
}
