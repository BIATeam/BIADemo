# Unit tests for V3 projects
This file explains what to do in order to add unit tests for the back-end of your V3 project.

It contains an overview of the architecture, but if you just want to know how to create your own unit tests project, go directly to chapter **How to add unit tests for my project?**.

---

## Prerequisite

### Knowledge to have:
* [Unit test best practices and naming conventions](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)
* [MStest](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest)
* We cannot mock static methods with the default test framework (so, the same thing applies to extension methods).

###  Test attributes
As explained [here](https://docs.microsoft.com/en-us/previous-versions/visualstudio/visual-studio-2012/ms245572(v=vs.110)#examples) or in **UnitTestExample** (one of the files provided in the example project), there are some attributes that can/have to be used for unit tests:
* Every test suite shall be a class with the **<code>[TestClass]</code>** attribute. 
A test suite is class containing several tests related to the same topic.
* Every test shall be a method with the **<code>[TestMethod]</code>** attribute inside a test suite.
* If you want to test the same thing but with different inputs, you can use a **<code>[DataTestMethod]</code>** attribute intead of the **<code>TestMethod</code>** one.
This attribute can be used in combination with several **<code>[DataRow(x, y, z)]</code>** attributes. Each **<code>DataRow</code>** will be an execution of the test with the given inputs (x, y, z).
Note: you can add as many input parameters as you want.
Example:
  ```csharp
    [DataTestMethod]
    [DataRow(-1, true)]
    [DataRow(0, true)]
    [DataRow(1, true)]
    [DataRow(2, false)]**
    public void TestMethodFactorized(int value, bool expectedResult)
    {
        Assert.AreEqual(expectedResult, value < 2);
    }
  ```
* The **<code>[ClassInitialize]</code>** attribute can be used on a method that will be executed **once** for the whole test suite, **before the first test**. 
It can be used to setup a global context for the whole test suite.
* The **<code>[ClassCleanup]</code>** attribute can be used on a method that will be executed **once** for the whole test suite, **after the last test**.
It can be used to reset any configuration that was setup previously.
* The **<code>[TestInitialize]</code>** attribute can be used on a method that will be executed **before each test**. (in our case, we use it to reset the DB mock and IoC)
*  The **<code>[TestCleanup]</code>** attribute can be used on a method that will be executed **after each test**.

---

## Overview
Here is an overview of the architecture.
### BIA.Net.Core.Test
**BIA.Net.Core.Test** project contains some basic classes that:
* Hide some of the complexity of the tests
* Manage part of the IoC
* Allow to mock some user related data (user ID, user rights, etc)

It only uses common data (that are **not** strongly related to your project).

**PrincipalMockBuilder** helps you create a mock where you can easily customize user related information (user id, user rights, etc).
It follows the Builder pattern, so you can chain several method calls to configure your mock. The mocked object is automatically applied to the IoC before each test.

**BIAAbstractUnitTest** is the base class of all unit tests.
It contains mechanisms used to:
* Access the database mock (through the **<code>DbMock</code>** property)
* Manage the IoC services (through the **<code>servicesCollection</code>** attribute) and retrieve more easily instances of injected services and controllers (through the **<code>GetService\<T></code>** and **<code>GetControllerWithHttpContext\<TController></code>** methods)
* Manage the user mock (through the **<code>principalBuilder</code>** attribute)
* Eventually add default data at the beginning of each test (through the **<code>isInitDB</code>** attribute)

### Unit test project
Your unit test project shall:
* Contain all your tests
* Define more precisely how we interact with the database mock
* Define the IoC part that is strongly coupled to your project (services, controllers and DbContext)

By default, we are using an **'in memory'** Entity Framework database to mock the database context (through the use of **<code>MockEntityFrameworkInMemory</code>**).
It means that you can manipulate directly **<code>DbSet</code>** objects, but nothing will be stored on your file system (just kept in RAM).

Each test shall extend **<code>AbstractUnitTest</code>**.

IoC of classes strongly coupled to your project shall be defined in **<code>IocContainerTest</code>**.

---

## How to add unit tests for my project?
If you want to know what to do in order to add unit tests to your project, this is the way...

### Create your test project
#### Copy of TheBIADevCompany.BIADemo.Test
* Copy/paste **TheBIADevCompany.BIADemo.Test** project from [here](https://azure.devops.thebiadevcompany/TheBIADevCompanyElectricalAndPower/Digital%20Manufacturing/_git/BIADemo?path=%2FDotNet%2FTheBIADevCompany.BIADemo.Test) and add it in the "**DotNet**" folder of your solution.
* Rename the folder and csproj in order to match your company and project names, but keep the pattern **[CompanyName].[ProjectName].Test**.
* Add the project to your solution (usually inside a "**99 - Test**" folder).
* Check the properties of your test project and (if necessary) change information in the "Package" tab in order to match your project information.
* Replace all **TheBIADevCompany.BIADemo** occurences by **[CompanyName].[ProjectName]** (Ctrl+Shift+H on Visual Studio).

#### Remove examples
* Some example classes/interfaces have been added in order to show how to implement unit tests in a more realistic way.
You can keep a copy of them somewhere to have examples, but eventually you will have to remove them (otherwise your project will never compile):
  * **PlanesControllerTests**
  * **PlaneAppServiceTests**
  * **UnitTestExample**
* Remove anything that contains **Plane** (except if you really use planes in your project obviously :)):
  * **DataConstants.DefaultPlanesMsn**.
  * Imports to **[CompanyName].[ProjectName].Domain.PlaneModule**
  * Call to **<code>services.AddTransient<PlanesController, PlanesController>();</code>** in **IocContainerTest**.
  * Any use of the **<code>Plane</code>** class.
  * etc
  
#### Adapt project
Some additional modifications have to be made on your project (not the unit test project, but your real project, the one you want to test):
* Modify **[CompanyName].[ProjectName].Crosscutting.Ioc.IocContainer.ConfigureContainer(IServiceCollection collection, IConfiguration configuration)** method (required in order to use the 'in memory' database):
  * Add a third parameter **<code>bool isUnitTest = false</code>** to this method.
  * Inside this method, add a **<code>if (!isUnitTest)</code>** and put inside:
    * Anything related to **<code>configuration</code>** (it should concern the DbContext and the management of BiaNetSection).
    * The IoC for **<code>IGenericRepository</code>**.
* **ClaimsPrincipal**
  * Modify **[CompanyName].[ProjectName].Presentation.Api.Startup** in order to change the IoC for **<code>IPrincipal</code>** (required because we cannot mock extension methods). 
  If the previous value was **<code>services.AddTransient<IPrincipal>(provider => xxxx);</code>**,
  it should now be **<code>services.AddTransient<IPrincipal>(provider => new BIAClaimsPrincipal(xxxx));</code>** (just add <code>new BIAClaimsPrincipal()</code> around what was previsouly injected)
  * Everywhere you use **<code>ClaimsPrincipal</code>**, replace it by **<code>BIAClaimsPrincipal</code>**
* **UserDataDto**
  * This class has been moved to **BIA.Net.Core.Domain.Dto.User**. 
  * So remove your specific implementation (from **[CompanyName].[ProjectName].Domain.Dto.User**) and change usings where required.
* **DataRepository**
  * This class has been moved to **BIA.Net.Core.Infrastructure.Data.Repositories**. 
  * So remove your specific implementation (from **[CompanyName].[ProjectName].Infrastructure.Data.Repositories**) and change usings where required.

<p style="color:green; font-weight:bold">Once this is done, your solution should build successfully.</p>

If this is not the case, then here are some additional things to do:
* If your project is not up-to-date with the latest V3.3 framework, replace some of your project components by the generic ones that have been moved to BIA.Net.Core (such as **<code>IUnitOfWork, IQueryableUnitOfWork, IGenericRepository</code>**, etc).
* If you use specific implementations of **<code>Site</code>** or **<code>User</code>**, you might have to change some methods in **<code>MockEntityFrameworkInMemory</code>** (some properties might be missing).

#### Customize database mock and IoC
No the real work starts! :)
We will configure how to interact with the 'in memory' database and how to perform IoC.

* Modify **MockEntityFrameWorkInMemory**. 
This is the class mocking the database context to an 'in memory' Entity Framework database.
  * It shall extend **<code>AbstractMockEntityFramework\<T></code>** where T is your **<code>DbContext</code>**.
  If you are using the default name for your **<code>DbContext</code>**, it shall be **<code>DataContext</code>** and you have nothing to change.
  * Normally, you should already have gotten rid of any reference to **<code>Plane</code>**, but if this is not the case, now is the time to do so.
  * If you want to manipulate other tables, you can add methods to do so here. 
  The **<code>GetDbContext()</code>** method gives you access to all available tables, so you can use it to implement those new methods.
  * The **<code>InitDefaultData()</code>** method can be used to add some data before each test that is configured to do so. 
  So, feel free to modify it, **but be careful: it can have an impact on every test that has been created with <code>isInitDB = true</code>**.

* Modify **IocContainerTest**. 
This is the class configuring the IoC for our unit tests.
  * If you followed the previous steps, you already modified **<code>IocContainer.ConfigureContainer()</code>** in order to add a third parameter and change its implementation.
  * Modify **<code>IocContainerTest.ConfigureControllerContainer()</code>**. You should add a dependency injection for every controller you want to test. This is usually quite a dummy one: **<code>services.AddTransient<MyController, MyController>();</code>**
  
<p style="color:green; font-weight:bold">Normally, everything should be set up right now.</p>

If this is not the case, well here are some leads:
* Your **<code>DbContext</code>** is not named **<code>DataContext</code>**. Then you have to replace **<code>DataContext</code>** by the correct name everywhere.


#### Create your own tests
Now that we configured everything correctly, it's time to write some tests!
You can take examples on the existing ones, but here are some guidelines.

##### Architecture
* Your test suites shall be created in the "**Tests**" folder.
The default structure is the following one:
> [CompanyName].[ProjectName].Test
> &nbsp;&nbsp;|\_ Tests
> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|\_ Controllers
> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|\_ One class for each controller you want to test
> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|\_ Services
> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|\_ One class for each service you want to test
* Create a test suite for every topic (for example, sites, users, planes, etc).
But you can even be more specific. 
For example, you can create several test suites related to the same global topic (for example, sites), but each test suite having a specific context (for example, some specific user rights). 
This can allow you to centralize some initialization in the **<code>[TestInitialize]</code>** method rather than doing it in every test.

##### Test suites
* Each test suite shall be a class:
  * With the **<code>[TestClass]</code>** attribute.
  * Extending **<code>AbstractUnitTest</code>**.
  * With a default constructor (without parameter) calling the base constructor with a boolean parameter:
  ```csharp
   public MyControllerTests() 
   : base(false) 
   {

   }
   ```
  This boolean parameter is used to define if we shall call the **<code>MockEntityFrameworkInMemory.InitDefaultData()</code>** before each test or not (in order to add some default data in the DB). It is up to you to decide if you want to use <code>true</code> or <code>false</code>.
  * **[Optional]** With a method with the **<code>[TestInitialize]</code>** attribute.
  In this method, you can setup a context that is common to every test.
  For example, you can instanciate the controller/service you want to test, add some data in the DB, mock some user related data, etc.

##### Tests
* To easily create a test for each method of a service/controller, you can do the following in Visual Studio:
  * In your test project, create the file where you want to put your tests.
  * Copy its namespace (it will save you some time later).
  * Open the service/controller you want to create tests for.
  * Right-click inside the class and select "**Create Unit Tests**".
  * Change only the following options:
    * **Test Project**: select your existing test project.
    * **Namespace**: paste the namespace you copied earlier.
    * **Output file**: select the test file you created.
  * Click OK. Visual Studio will create a test method for each method of your service/controller, even the constructor!
  * Remove the constructor test.
  * **[Optional]** Remove the "**()**" in each **<code>[TestMethod()]</code>** attribute or fill it with the desired display name.
* Each test shall be a method with the **<code>[TestMethod]</code>** or **<code>[DataTestMethod]</code>** attribute.
You can add an optional parameter to this attribute in order to configure the name which will be displayed in the test report. It can be a good idea to do so, because by default it only uses the method name (so if you have several methods with the same name in different test suites, you won't directly differenciate them).
* Refer to [Unit test best practices and naming conventions](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices) in order to name your tests correctly.
* Try to keep your tests small and with one single objective.
* For **controller tests**, you can:
  * Retrieve the **HTTP status code** of an API by casting its returned value into an **<code>IStatusCodeActionResult</code>**.
  For example:
    ```csharp
    this.controller = this.GetControllerWithHttpContext<SitesController>();
    IStatusCodeActionResult response = this.controller.Add(siteDto).Result as IStatusCodeActionResult;
    Assert.IsNotNull(response);
    Assert.AreEqual((int)HttpStatusCode.Created, response.StatusCode);
    ```
  * Retrieve the **HTTP status code and the returned value** of an API by casting its returned value into an **<code>ObjectResult</code>**.
  For example:
    ```csharp
    this.controller = this.GetControllerWithHttpContext<SitesController>();
    ObjectResult response = this.controller.GetAll(filter).Result as ObjectResult;
    Assert.IsNotNull(response);
    Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);
    IEnumerable<SiteInfoDto> listSites = response.Value as IEnumerable<SiteInfoDto>;
    Assert.IsNotNull(listSites);
    Assert.AreEqual(1, listSites.Count());
    ```
* For **service tests**, you can:
  * Check the returned DTO.
  * Check the DB has been correctly updated by using **<code>this.DbMock</code>**:
    * Either by calling the helper methods you created in **<code>MockEntityFrameworkInMemory</code>**
    * Or by using **<code>this.DbMock.GetDbContext()</code>** which gives you a direct access to the **<code>DbSet</code>** objects.

##### IoC and mock
* Use **<code>GetService\<T></code>** and **<code>GetControllerWithHttpContext\<TController></code>** methods from **<code>BIAAbstractUnitTest</code>** to instanciate your services and controllers through IoC.
For the controllers, it will automatically configure an HttpContext that is required by most of the APIs we implemented in V3 projects.
* Use **<code>this.DbMock</code>** in order to access to the database and:
  * Check if the correct data is stored in DB,
  * Add/Remove data from the DB to **setup your test context**.
* Use **<code>this.principalBuilder</code>** to mock some user related information (user ID, user rights, etc).
You only have to call the **<code>MockXxxx()</code>** methods in order to setup the information you want to mock. The mocked object is automatically generated and applied when initializing the test.
* Since most of our APIs are asynchronous, use **<code>Result</code>** to wait for the call to be complete and retrieve the returned value.
For example: 
  ```csharp
  ISiteAppService service = this.GetService<ISiteAppService>();  
  SiteDto site = service.GetAsync(1).Result;
  ```

---

### Pipelines
In order to execute your unit tests before each build on Azure DevOps, do the following:
* Edit your pipeline (AutomatedBuild or release).
* Add a "**Visual Studio Test**" task if it is not already there (usually it has been renamed "**API Tests**").
* Use the same options as the "**BIADemo - AutomatedBuild**" pipeline:
  * Check it is in **Task version 2.***.
  * Set the "**Test files**" option to the following value:
    > \*\*\\\$(BuildConfiguration)\\netcoreapp3.1\\*$(ProjectName).Test.dll
    !\*\*\\obj\\\*\*</code>
* When you execute the pipeline, check the results of the "**API Tests**" task.
You should see something like that:
  > 2020-12-22T10:39:52.7079229Z "D:\Agent\_work\8\s\DotNet\TheBIADevCompany.BIADemo.Test\bin\Release\netcoreapp3.1\TheBIADevCompany.BIADemo.Test.dll"
  2020-12-22T10:39:52.7079265Z /logger:"trx"
  2020-12-22T10:39:52.7079308Z /TestAdapterPath:"D:\Agent\_work\8\s"
  2020-12-22T10:39:52.7079341Z **Starting test execution, please wait...**
  2020-12-22T10:39:53.4299256Z 
  2020-12-22T10:39:53.4299811Z **A total of 1 test files matched the specified pattern.**
  2020-12-22T10:39:56.0532657Z   √ **TestMethod** [640ms]
  2020-12-22T10:39:56.0533075Z   √ **TestMethodFactorized** [4ms]
  2020-12-22T10:39:56.0533872Z   √ **TestMethodFactorized (-1,True)** [1ms]
  2020-12-22T10:39:56.0534709Z   √ **TestMethodFactorized (0,True)** [< 1ms]
  2020-12-22T10:39:56.0535006Z   √ **TestMethodFactorized (1,True)** [< 1ms]
  2020-12-22T10:39:56.0535243Z   √ **TestMethodFactorized (2,False)** [< 1ms]
  ...
  2020-12-22T10:39:56.6390388Z Results File: D:\Agent\_work\8\s\TestResults\lrAPP003_DS002AZS_2020-12-22_11_39_56.trx
  2020-12-22T10:39:56.6413163Z 
  2020-12-22T10:39:56.6415197Z **Test Run Successful.**
  2020-12-22T10:39:56.6416408Z **Total tests: 41**
  2020-12-22T10:39:56.6417058Z      **Passed: 41**
  2020-12-22T10:39:56.6425008Z  Total time: 3,1740 Seconds
  2020-12-22T10:39:56.8593561Z ##[section]Async Command Start: Publish test results
  2020-12-22T10:39:56.8686539Z Publishing test results to test run '57'
  2020-12-22T10:39:56.8686682Z Test results remaining: 22. Test run id: 57
  2020-12-22T10:39:57.1301682Z Published Test Run : https://azure.devops.thebiadevcompany/TheBIADevCompanyElectricalAndPower/Digital%20Manufacturing/_TestManagement/Runs?runId=57&_a=runCharts
  2020-12-22T10:39:57.1315111Z ##[section]Async Command End: Publish test results
  2020-12-22T10:39:57.1316208Z ##[section]Finishing: API Tests