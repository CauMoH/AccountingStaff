namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employee",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 4000),
                        Surname = c.String(maxLength: 4000),
                        Patronymic = c.String(maxLength: 4000),
                        СompletingСourse = c.DateTime(),
                        NextCourse = c.DateTime(),
                        Protocol = c.String(maxLength: 4000),
                        Meta = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Employee");
        }
    }
}
