# Database Handler (Worker Feature)
This file explains what to use the the database handler feature in your V3 project.

## Prerequisite

### Knowledge to have:
* [SQL language](https://sql.sh/)

### Database:
* The project database should be SQL Server
* Broker should be enable on the project database
```SQL
ALTER DATABASE [YourProjectDatabase] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
ALTER DATABASE [YourProjectDatabase] SET ENABLE_BROKER;
ALTER DATABASE [YourProjectDatabase] SET MULTI_USER WITH ROLLBACK IMMEDIATE
```

WARNING: If the database is in an availability group, you should remove the database from availability group before apply this script.
And readded after (requiered to delete before the databse in secondary server).

Give the right to the YourUserRW to read and write the database and run this script (replace the YourUserRW by the corresponding user):
 
```SQL
USE [YourProjectDatabase];

--create user for schema ownership
CREATE USER SqlDependencySchemaOwner WITHOUT LOGIN;
GO

--create schema for SqlDependency ojbects
CREATE SCHEMA SqlDependency AUTHORIZATION SqlDependencySchemaOwner;
GO

--set the default schema of minimally privileged user to SqlDependency
ALTER USER "YourUserRW" WITH DEFAULT_SCHEMA = SqlDependency;

--grant user control permissions on SqlDependency schema
GRANT CONTROL ON SCHEMA::SqlDependency TO "YourUserRW";

--grant user impersonate permissions on SqlDependency schema owner
GRANT IMPERSONATE ON USER::SqlDependencySchemaOwner TO "YourUserRW";
GO

--grant database permissions needed to create and use SqlDependency objects
GRANT CREATE PROCEDURE TO "YourUserRW";
GRANT CREATE QUEUE TO "YourUserRW";
GRANT CREATE SERVICE TO "YourUserRW";
GRANT REFERENCES ON
    CONTRACT::[http://schemas.microsoft.com/SQL/Notifications/PostQueryNotification] TO "YourUserRW";
GRANT VIEW DEFINITION TO "YourUserRW";
GRANT SELECT to "YourUserRW";
GRANT SUBSCRIBE QUERY NOTIFICATIONS TO "YourUserRW";
GRANT RECEIVE ON QueryNotificationErrorsQueue TO "YourUserRW";
GO
```
=> TODO : - Testé une seule fois sur BIADemo
- N'a marché qu'aprés avoir tourné une premiere fois en dbowner (sans le shema)
- Comprend pourquoi il faut faire tourner une fois en Owner avant (est ce premiere database BIADemo...)

## Overview
* The worker service run code when there is change on the database.
* A fine selection of the rows to track can be done with a SQL query.

## Activation
### Api: 
* bianetconfig.json
In the BIANet Section add:
```Json
    "WorkerFeatures": {
      "DatabaseHandler": {
        "Activate": true
      }
    },
```

## Usage
## Create the handler repositories:
Create a repository classe in the worker project in floder Features this classe inherit of DatabaseHandlerRepository.
Example from BIADemo:
```CSharp
namespace TheBIADevCompany.BIADemo.WorkerService.Features
{
    using System.Data.SqlClient;
    using BIA.Net.Core.WorkerService.Features.DataBaseHandler;
    using BIA.Net.Core.WorkerService.Features.HubForClients;
    using Microsoft.Extensions.Configuration;

    public class PlaneHandlerRepository : DatabaseHandlerRepository
    {
        public PlaneHandlerRepository(IConfiguration configuration)
            : base(
            configuration.GetConnectionString("BIADemoDatabase"),
            "SELECT RowVersion FROM [dbo].[Planes]",
            "" /*"SELECT TOP (1) [Id] FROM [dbo].[Planes] ORDER BY [RowVersion] DESC"*/,
            r => PlaneChange(r))
            { }

        public static void PlaneChange(SqlDataReader reader)
        {
            //int id = reader.GetInt32(0);

            _ = HubForClientsService.SendMessage("refresh-planes", "");
        }
    }
}
```
In the constructor base you specify the parameters:
- The connection string
- The SQL query to track change
- The SQL query to execute when a change appear (if empty it do not execute query)
- The callback fonction to execute when a change appear

In the callback function :
- you can read the result of the query passed in 3th parameters, by using the reader passed in parameter.
- If the 3th paramter of the base constructor is empty the reader parameter is null.

### Parameters those repositories
In program.cs you should pass the list of all yours database handler repositories class in the function config.DatabaseHandler.Activate.
Example from BIADemo:
```CSharp
        services.AddBiaWorkerFeatures(config =>
        {
            config.Configuration = hostContext.Configuration;
            var biaNetSection = new BiaNetSection();
            config.Configuration.GetSection("BiaNet").Bind(biaNetSection);

            if (biaNetSection.WorkerFeatures.DatabaseHandler.IsActive)
            {
                config.DatabaseHandler.Activate(new List<DatabaseHandlerRepository>()
                {
                    new PlaneHandlerRepository(hostContext.Configuration),
                });
            }
        });
```
