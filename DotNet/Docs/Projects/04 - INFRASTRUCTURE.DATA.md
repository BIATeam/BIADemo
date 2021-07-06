# Infrastructure.Data project

## Preparation of the Database

1. Create the database on your local instance
    - The name of the database should be the name of the project you can verify the connection string in the appsettings.json file and the declined files by environment
2. Launch the Package Manager Console in VS 2019 (Tools > Nuget Package Manager > Package Manager Console).
3. Be sure to have the project **TheBIADevCompany.[ProjectName].Infrastructure.Data** selected as the Default Project in the console and the project **TheBIADevCompany.[ProjectName].Presentation.Api** as the Startup Project of your solution.
4. (ONLY if no migration have been done = new project or never use) Run the **Add-Migration** command to initialize the migrations for the database project. `Add-Migration [nameOfYourMigration]`
5. Run the **Update-Database** command to update you database schema (you can check if everything is fine in SQL Server Management Studio).
6. (OPTIONNALY) Update the Roles section in the bianetconfig.json file and the declined files by environment to use the correct AD groups or the Fakes roles.
7. (OPTIONNALY) Update the version of the application. To do this, change the variable: **TheBIADevCompany.[ProjectName].Crosscutting.Common.Constants.Application.Version**.