# Commission Factory Modernisation Challenge

This solution contains projects that use .NET Framework 4.8, Linq to SQL, and ASP.NET Web Forms.

The goal of this challenge is to modernise the solution by migrating to .NET 6, Entity Framework Core 6, and ASP.NET Core 6, with your choice of front end web framework (e.g. AngularJS, Knockout, React, Vue.js).

To complete this challenge, you will need Visual Studio Professional and SQL Server (Developer and Express editions will work correctly).

Create a database called `ModernisationChallenge`, then execute the following script to create the required table:

``` sql
USE [ModernisationChallenge];

CREATE TABLE [Tasks] (
    [Id] INT NOT NULL IDENTITY(1, 1),
    [DateCreated] DATETIME NOT NULL,
    [DateModified] DATETIME NOT NULL,
    [DateDeleted] DATETIME NULL,
    [Completed] BIT NOT NULL,
    [Details] NVARCHAR(MAX) NOT NULL,
    CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED ([Id] ASC)
);
```
