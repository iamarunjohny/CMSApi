using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMSApi.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Check if RefreshTokens table exists before creating
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'RefreshTokens')
                BEGIN
                    CREATE TABLE [RefreshTokens] (
                        [Id] int NOT NULL IDENTITY,
                        [Token] nvarchar(max) NOT NULL,
                        [ExpiryDate] datetime2 NOT NULL,
                        [Username] nvarchar(max) NOT NULL,
                        CONSTRAINT [PK_RefreshTokens] PRIMARY KEY ([Id])
                    );
                END;
            ");

            // Check if Users table exists before creating
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
                BEGIN
                    CREATE TABLE [Users] (
                        [UserId] int NOT NULL IDENTITY,
                        [Username] nvarchar(max) NOT NULL,
                        [Passwords] nvarchar(max) NOT NULL,
                        [Email] nvarchar(max),
                        [FirstName] nvarchar(max),
                        [LastName] nvarchar(max),
                        CONSTRAINT [PK_Users] PRIMARY KEY ([UserId])
                    );
                END;
            ");

            // Check if Contacts table exists before creating
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Contacts')
                BEGIN
                    CREATE TABLE [Contacts] (
                        [ContactId] int NOT NULL IDENTITY,
                        [UserId] int,
                        [FirstName] nvarchar(max),
                        [LastName] nvarchar(max),
                        [Company] nvarchar(max),
                        [JobTitle] nvarchar(max),
                        [PhoneNumber] nvarchar(max),
                        [Email] nvarchar(max),
                        [Notes] nvarchar(max),
                        CONSTRAINT [PK_Contacts] PRIMARY KEY ([ContactId])
                    );

                    -- Add Foreign Key only if Users table exists
                    IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
                    BEGIN
                        ALTER TABLE [Contacts] ADD CONSTRAINT [FK_Contacts_Users_UserId]
                        FOREIGN KEY ([UserId]) REFERENCES [Users]([UserId]);
                    END;
                END;
            ");

            // Create Index for UserId in Contacts table if Contacts table exists
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Contacts')
                BEGIN
                    CREATE INDEX [IX_Contacts_UserId] ON [Contacts] ([UserId]);
                END;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop tables only if they exist
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Contacts')
                BEGIN
                    DROP TABLE [Contacts];
                END;
            ");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.tables WHERE name = 'RefreshTokens')
                BEGIN
                    DROP TABLE [RefreshTokens];
                END;
            ");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
                BEGIN
                    DROP TABLE [Users];
                END;
            ");
        }
    }
}
