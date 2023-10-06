﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorServerTemplate.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table =>
                    new
                    {
                        Id = table
                            .Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        Value = table.Column<string>(
                            type: "nvarchar(1000)",
                            maxLength: 1000,
                            nullable: false
                        )
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table =>
                    new
                    {
                        Id = table
                            .Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        FirstName = table.Column<string>(
                            type: "nvarchar(1000)",
                            maxLength: 1000,
                            nullable: false
                        ),
                        LastName = table.Column<string>(
                            type: "nvarchar(1000)",
                            maxLength: 1000,
                            nullable: false
                        ),
                        FullName = table.Column<string>(
                            type: "nvarchar(1000)",
                            maxLength: 1000,
                            nullable: false
                        ),
                        EmailAddress = table.Column<string>(
                            type: "nvarchar(1000)",
                            maxLength: 1000,
                            nullable: false
                        ),
                        NameIdentifier = table.Column<string>(
                            type: "nvarchar(1000)",
                            maxLength: 1000,
                            nullable: false
                        ),
                        Settings_IsDarkMode = table.Column<bool>(type: "bit", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "PermissionUser",
                columns: table =>
                    new
                    {
                        PermissionsId = table.Column<int>(type: "int", nullable: false),
                        UsersId = table.Column<int>(type: "int", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionUser", x => new { x.PermissionsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_PermissionUser_Permissions_PermissionsId",
                        column: x => x.PermissionsId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_PermissionUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_PermissionUser_UsersId",
                table: "PermissionUser",
                column: "UsersId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "PermissionUser");

            migrationBuilder.DropTable(name: "Permissions");

            migrationBuilder.DropTable(name: "Users");
        }
    }
}
