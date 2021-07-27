## [3.4.0] (2021-07-16)
### DotNet
* .Net5.0
* Bulk function in repository.
* PostgreSQL compatibility.
## [3.3.3] (2021-06-25)
### DotNet
* New helper in common to compare string.
### Angular
* Universal Mode for CRUD.
## [3.3.2] (2021-05-28)
### Angular
* Possibility for the user to choice his role.
* Click on site open manage member.
* Member is a children of site with related service and breadcrumb.
* Adding the Calc mode for CRUD.
## [3.3.1] (2021-03-31)
### DotNet
* DeployDB use native code First mechanism
* Use the new clustered database
* Add the MapperMode flag in FilteredService to not multiplicate mapper when only a part of the field are to update.
* Add the project title on hangfire dashboard.
* Suppress all warning in test and generated code.
## [3.3.0] (2021-01-15)
### DotNet
* Add feature management (posibilité to activate and desactivate powerfull feature like swagger, SignalR...)
* Add Unitary Test
* Add feature in Api HubForClients (use SignalR to push messge to all client connected, compatible with multi front) 
* Add feature in Api DelegateJobToWorker (use Hangfire to launch job in the worker) 
* Add feature in worker DatabaseHandler (detect the change in db immediatlty)
* Add feature in worker HubForClients (use the Api feture HubForClients to push message to all web client connected)
* WorkerService is now a web api with the hangfire Dashboard.
### Angular
*  Date bug fix
*  Matomo integration
*  Crud generation support complexe name (like plane-type)
*  Add choice of the site for Admin
## [3.2.2] (2020-10-16)
### DotNet
* Solve bug with Zodiac user
* Desactivate swagger in no dev environment
* Add color by environment
* Remove the popup when token expire
* Generate a new secretkey at deployement
### Angular
*  Color by env.
## [3.2.1] (2020-10-16)
### DotNet
* Add the worker service (hangfire)
## [3.2.0] (2020-10-16)
### DotNet
* Use of BIA.core nugetpackage (1 by layer)
* Compatibility with multi ad environmemt (usage of user sid) => change the database model
### Angular
*  angular 9.1.12
## [3.2.0] (2020-10-16)
### DotNet
* Use of BIA.core nugetpackage (1 by layer)
* Compatibility with multi ad environmemt
### [3.1.1] (2020-06-26)
### Angular
*  Bug Fix
## [3.1.0] (2020-05-04)
* views
## [3.0.0] (2020-10-02)
### DotNet
* .NET Core 3.1.1
### Angular
*  angular 8.2.14