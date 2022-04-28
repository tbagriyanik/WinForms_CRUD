namespace winFormsCRUD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ilk : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.grups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        grupAdi = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.personels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        isim = c.String(),
                        personelGrubu_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.grups", t => t.personelGrubu_Id)
                .Index(t => t.personelGrubu_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.personels", "personelGrubu_Id", "dbo.grups");
            DropIndex("dbo.personels", new[] { "personelGrubu_Id" });
            DropTable("dbo.personels");
            DropTable("dbo.grups");
        }
    }
}
