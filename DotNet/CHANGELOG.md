### [3.3.3] (2021-06-25)
* New helper in common to compare string.
### [3.3.2] (2021-05-28)
### [3.3.1] (2021-03-31)
* DeployDB use native code First mechanism
* Use the new clustered database
* Add the MapperMode flag in FilteredService to not multiplicate mapper when only a part of the field are to update.
* Add the project title on hangfire dashboard.
* Suppress all warning in test and generated code.
### [3.3.0] (2021-01-15)
* Add feature management (posibilité to activate and desactivate powerfull feature like swagger, SignalR...)
* Add Unitary Test
* Add feature in Api HubForClients (use SignalR to push messge to all client connected, compatible with multi front) 
* Add feature in Api DelegateJobToWorker (use Hangfire to launch job in the worker) 
* Add feature in worker DatabaseHandler (detect the change in db immediatlty)
* Add feature in worker HubForClients (use the Api feture HubForClients to push message to all web client connected)
* WorkerService is now a web api with the hangfire Dashboard.
### [3.2.2] (2020-10-16)
* Solve bug with Zodiac user
* Desactivate swagger in no dev environment
* Add color by environment
* Remove the popup when token expire
* Generate a new secretkey at deployement
### [3.2.1] (2020-10-16)
* Add the worker service (hangfire)
### [3.2.0] (2020-10-16)
* Use of BIA.core nugetpackage (1 by layer)
* Compatibility with multi ad environmemt (usage of user sid) => change the database model
### [3.2.0] (2020-10-16)
* Use of BIA.core nugetpackage (1 by layer)
* Compatibility with multi ad environmemt
### [3.1.0] (2020-05-04)
* views
### [3.0.0] (2020-10-02)
* .NET Core 3.1.1