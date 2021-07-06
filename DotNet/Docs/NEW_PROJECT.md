# New Project
This document explains how to create a new .NET Core project based on the BIA framework.<br/>

## Prerequisite

### Knowledge to have:
* [C# 8](https://docs.microsoft.com/fr-fr/dotnet/csharp/)
* [.NET Core 3.1](https://docs.microsoft.com/fr-fr/dotnet/core/)

### Configure your environment
* Follow the steps in [CONFIGURE_YOUR_DEV_ENVIRONMENT.md](./CONFIGURE_YOUR_DEV_ENVIRONMENT.md)

## Create a new project ...
You must respect the following tree structure:   
**ProjectName\Angular** and **ProjectName\DotNet**    
   
Install the latest version of the **[BIA.ProjectCreator.vsix](../../Docs/BIAExtension)** extension for Visual Studio 2019.  
You can now go to File > New > Project in Visual Studio.  
Select the BIA template in the type of projects and click on Next button.  
Fill **Project name** field. For **Location**, type [MyLocationSourceCode]\\[ProjectName] and check **Place solution and project in the same directory**.  
Close Visual studio and rename the second level of folder [ProjectName] (in [MyLocationSourceCode]\\[ProjectName]\\**[ProjectName]**) in DotNet

## ... Or Clone an existing one

## Prepare the Presentation WebApi:
* Follow the steps "Prepare the Presentation WebApi" in [01 - PRESENTATION.API.md](./Projects/01%20-%20PRESENTATION.API.md)

## Prepare the database
* Follow the steps " Preparation of the Database" in [04 - INFRASTRUCTURE.DATA.md](./Projects/04%20-%20INFRASTRUCTURE.DATA.md)

## Run the project
* you can now run the project Presentation Api in IIS 
* In the swagger page verify that the login work by clicking on the "BIA Log In" button  at the bottom right.

## Use standard BIA Framework Component

* To use the project created by the framework have a look in the docs in [this folder](./Projects).

* To use the features proposed by the framework have a look in the docs in [this folder](./Features).
