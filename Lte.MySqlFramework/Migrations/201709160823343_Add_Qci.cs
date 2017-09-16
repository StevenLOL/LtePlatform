namespace Lte.MySqlFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Qci : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QciHuaweis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StatTime = c.DateTime(nullable: false, precision: 0),
                        ENodebId = c.Int(nullable: false),
                        LocalCellId = c.Byte(nullable: false),
                        Cqi0Times = c.Int(nullable: false),
                        Cqi1Times = c.Int(nullable: false),
                        Cqi2Times = c.Int(nullable: false),
                        Cqi3Times = c.Int(nullable: false),
                        Cqi4Times = c.Int(nullable: false),
                        Cqi5Times = c.Int(nullable: false),
                        Cqi6Times = c.Int(nullable: false),
                        Cqi7Times = c.Int(nullable: false),
                        Cqi8Times = c.Int(nullable: false),
                        Cqi9Times = c.Int(nullable: false),
                        Cqi10Times = c.Int(nullable: false),
                        Cqi11Times = c.Int(nullable: false),
                        Cqi12Times = c.Int(nullable: false),
                        Cqi13Times = c.Int(nullable: false),
                        Cqi14Times = c.Int(nullable: false),
                        Cqi15Times = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QciZtes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StatTime = c.DateTime(nullable: false, precision: 0),
                        ENodebId = c.Int(nullable: false),
                        SectorId = c.Byte(nullable: false),
                        Cqi0Times = c.Int(nullable: false),
                        Cqi1Times = c.Int(nullable: false),
                        Cqi2Times = c.Int(nullable: false),
                        Cqi3Times = c.Int(nullable: false),
                        Cqi4Times = c.Int(nullable: false),
                        Cqi5Times = c.Int(nullable: false),
                        Cqi6Times = c.Int(nullable: false),
                        Cqi7Times = c.Int(nullable: false),
                        Cqi8Times = c.Int(nullable: false),
                        Cqi9Times = c.Int(nullable: false),
                        Cqi10Times = c.Int(nullable: false),
                        Cqi11Times = c.Int(nullable: false),
                        Cqi12Times = c.Int(nullable: false),
                        Cqi13Times = c.Int(nullable: false),
                        Cqi14Times = c.Int(nullable: false),
                        Cqi15Times = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TownQciStats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TownId = c.Int(nullable: false),
                        StatTime = c.DateTime(nullable: false, precision: 0),
                        Cqi0Times = c.Int(nullable: false),
                        Cqi1Times = c.Int(nullable: false),
                        Cqi2Times = c.Int(nullable: false),
                        Cqi3Times = c.Int(nullable: false),
                        Cqi4Times = c.Int(nullable: false),
                        Cqi5Times = c.Int(nullable: false),
                        Cqi6Times = c.Int(nullable: false),
                        Cqi7Times = c.Int(nullable: false),
                        Cqi8Times = c.Int(nullable: false),
                        Cqi9Times = c.Int(nullable: false),
                        Cqi10Times = c.Int(nullable: false),
                        Cqi11Times = c.Int(nullable: false),
                        Cqi12Times = c.Int(nullable: false),
                        Cqi13Times = c.Int(nullable: false),
                        Cqi14Times = c.Int(nullable: false),
                        Cqi15Times = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TownQciStats");
            DropTable("dbo.QciZtes");
            DropTable("dbo.QciHuaweis");
        }
    }
}
