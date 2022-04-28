namespace winFormsCRUD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class iki : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.personels", "personelGrubu_Id", "dbo.grups");
            DropIndex("dbo.personels", new[] { "personelGrubu_Id" });
            RenameColumn(table: "dbo.personels", name: "personelGrubu_Id", newName: "grupID");
            AlterColumn("dbo.personels", "grupID", c => c.Int(nullable: false));
            CreateIndex("dbo.personels", "grupID");
            AddForeignKey("dbo.personels", "grupID", "dbo.grups", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.personels", "grupID", "dbo.grups");
            DropIndex("dbo.personels", new[] { "grupID" });
            AlterColumn("dbo.personels", "grupID", c => c.Int());
            RenameColumn(table: "dbo.personels", name: "grupID", newName: "personelGrubu_Id");
            CreateIndex("dbo.personels", "personelGrubu_Id");
            AddForeignKey("dbo.personels", "personelGrubu_Id", "dbo.grups", "Id");
        }
    }
}
