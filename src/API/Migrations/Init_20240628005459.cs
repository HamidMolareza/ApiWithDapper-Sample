using FluentMigrator;

namespace ApiWithDapper.Migrations;

[Migration(20240628005459)]
public class Init_20240628005459: Migration {
    public override void Up() {
        Create.Table("Todos")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Title").AsString(200).NotNullable()
            .WithColumn("Completed").AsBoolean().NotNullable();
    }

    public override void Down() {
        Delete.Table("Todos");
    }
}