IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250611153545_InitialProductMigration'
)
BEGIN
    CREATE TABLE [Product] (
        [Id] int NOT NULL,
        [Name] nvarchar(50) NOT NULL,
        [Stock] int NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_Product] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250611153545_InitialProductMigration'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250611153545_InitialProductMigration', N'9.0.6');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612084737_ProductPoolMigration'
)
BEGIN
    CREATE TABLE [ProductIdPool] (
        [Id] int NOT NULL,
        [IsAvailable] bit NOT NULL DEFAULT CAST(0 AS bit),
        CONSTRAINT [PK_ProductIdPool] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612084737_ProductPoolMigration'
)
BEGIN

                    WITH Numbers AS (
                        SELECT 100000 AS Number
                        UNION ALL
                        SELECT Number + 1 FROM Numbers WHERE Number < 999999
                    )
                    INSERT INTO ProductIdPool (Id)
                    SELECT Number FROM Numbers
                    OPTION (MAXRECURSION 0);
                
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612084737_ProductPoolMigration'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250612084737_ProductPoolMigration', N'9.0.6');
END;

COMMIT;
GO

