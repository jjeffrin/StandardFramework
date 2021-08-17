using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StandardFramework.Migrations
{
    public partial class ModifyNotificationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StackTrace",
                table: "Notifications",
                newName: "SourceFilePath");

            migrationBuilder.RenameColumn(
                name: "Source",
                table: "Notifications",
                newName: "ExceptionStackTrace");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "Notifications",
                newName: "ExceptionSource");

            migrationBuilder.RenameColumn(
                name: "HelpLink",
                table: "Notifications",
                newName: "ExceptionMessage");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Notifications",
                newName: "ExceptionHelpLink");

            migrationBuilder.AddColumn<string>(
                name: "ActionInvokeMemberName",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActionInvokeTime",
                table: "Notifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ActionType",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsException",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SourceLineNumber",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionInvokeMemberName",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ActionInvokeTime",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ActionType",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "IsException",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "SourceLineNumber",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "SourceFilePath",
                table: "Notifications",
                newName: "StackTrace");

            migrationBuilder.RenameColumn(
                name: "ExceptionStackTrace",
                table: "Notifications",
                newName: "Source");

            migrationBuilder.RenameColumn(
                name: "ExceptionSource",
                table: "Notifications",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "ExceptionMessage",
                table: "Notifications",
                newName: "HelpLink");

            migrationBuilder.RenameColumn(
                name: "ExceptionHelpLink",
                table: "Notifications",
                newName: "Content");
        }
    }
}
