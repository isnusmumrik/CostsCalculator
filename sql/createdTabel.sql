CREATE TABLE [dbo].[Users] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Email]    NVARCHAR (50) NOT NULL,
    [Name]     NVARCHAR (50) NOT NULL,
    [Password] NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Purchases] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (50) NOT NULL,
    [Amount]     INT           NOT NULL,
    [Unit]       NVARCHAR (50) NOT NULL,
    [UnitCost]   FLOAT (53)    NOT NULL,
    [Date]       DATE          NOT NULL,
    [CategoryId] INT           NOT NULL,
    [UserId]     INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Purchases_Categories] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Purchases_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);

CREATE TABLE [dbo].[Categories] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (50) NOT NULL,
    [UserId]          INT           NOT NULL,
    [ColorForDiagram] NCHAR (6)     NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);