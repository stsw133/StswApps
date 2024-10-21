﻿CREATE TABLE [dbo].[Apps]
(
	[ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] VARCHAR(100) NOT NULL,
    [Description] VARCHAR(1000) NULL,
    [CategoryID] INT NOT NULL,
    [Icon] VARBINARY(MAX) NULL,
    [VersionID] INT NULL,
    [SupporterID] INT NULL,
    [RepositoryURL] VARCHAR(1000) NULL,
    [ProcessName] VARCHAR(100) NULL,
    CONSTRAINT [FK_Apps_CategoryID] FOREIGN KEY ([CategoryID]) REFERENCES [AppCategories]([ID]) ON UPDATE CASCADE ON DELETE SET DEFAULT,
    CONSTRAINT [FK_Apps_VersionID] FOREIGN KEY ([VersionID]) REFERENCES [AppVersions]([ID]) ON UPDATE CASCADE ON DELETE SET NULL,
    CONSTRAINT [FK_Apps_SupporterID] FOREIGN KEY ([SupporterID]) REFERENCES [Users]([ID]) ON UPDATE CASCADE ON DELETE SET NULL
)
