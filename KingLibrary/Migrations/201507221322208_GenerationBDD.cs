namespace KingLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GenerationBDD : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        pseudo = c.String(),
                        mail = c.String(),
                        password = c.String(),
                        avatar = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Players");
        }
    }
}
