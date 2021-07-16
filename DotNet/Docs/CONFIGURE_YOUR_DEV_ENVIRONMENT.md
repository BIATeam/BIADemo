# Configure your development environment:

## Minimum requierement

### Visual Studio 2019 & components
**Installation :**

Install [Visual Studio 2019](https://visualstudio.microsoft.com/fr/vs/) and be sure to add the latest SDK of .NET Core from the components list.
Add those components during installation :
- [Git for Visual Studio](https://subscription.packtpub.com/book/programming/9781789530094/9/ch09lvl1sec71/installing-git-for-visual-studio-2019)
- [Development Time IIS Support](https://devblogs.microsoft.com/aspnet/development-time-iis-support-for-asp-net-core-applications/)

If Visual Studio 2019 is already install, you can add thoses component by launching the VS Installer.

**Configuration :**

Set your "using placement" code style setting to **inside namespace**
Code style settings are available from Tools > Options > Text Editor > C# > Code Style.
![Code style settings](./Images/CodeStyleSetting.png)

**Enable** your "Place 'System' directives first when sorting usings"
This setting is available from Tools > Options > Text Editor > C# > Advanced 
![Code style settings](./Images/SystemUsing.png)
### Variable Environment
create the following system environment variable:  
Name: ASPNETCORE_ENVIRONMENT  
Value: Development  

## Company Customisation
If your company have a proxy specify it (exact values can be describe in your company docs : CompanyConfig.md)

### Configuration for the Company proxy
[Add the following **User** environment variables :](https://www.tenforums.com/tutorials/121664-set-new-user-system-environment-variables-windows.html#option1)  
* HTTP_PROXY: [Add here your proxy]
* HTTPS_PROXY: [Add here your proxy]
* NO_PROXY: [Add here the local domain extensions taht do not need proxy separate by a space]

### Git config
To find the path to the **.gitconfig** file, type the following command:   
`git config --list --show-origin`   
Open your **.gitconfig** file and add this configuration:
```
[http]
	sslVerify = false
	proxy = "[Add here your proxy if requiered]"
```
