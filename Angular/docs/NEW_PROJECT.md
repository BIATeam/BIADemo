# New Project
This document explains how to create a new project based on the TheBIADevCompany Angular framework.   

## Prerequisite

### Knowledge to have:
* [Angular](https://angular.io/)
* [RXJS](https://www.learnrxjs.io/)
* [NGRX](https://ngrx.io/)
* [PrimeNG component list](https://www.primefaces.org/primeng/v9.1.4-lts/)
* [angular/flex-layout](https://github.com/angular/flex-layout/wiki)

### Configuration for the TheBIADevCompany proxy
[Add the following **User** environment variables :](https://www.tenforums.com/tutorials/121664-set-new-user-system-environment-variables-windows.html#option1)  
* HTTP_PROXY: http://10.179.8.30:3128/
* HTTPS_PROXY: http://10.179.8.30:3128/
* NO_PROXY: https://tfsdm.eu.labinal.snecma

### Git
Install [Git for Visual Studio](https://subscription.packtpub.com/book/programming/9781789530094/9/ch09lvl1sec71/installing-git-for-visual-studio-2019)

### Node.js
Install the same version of node.js as the one installed on the build server (12.18.3)   
To check the installed version of [node.js](https://nodejs.org/en/download/releases/), use the following command: `node -v`   

### angular cli
Upgrade npm using `npm install -g @angular/cli@9.1.12`   

### Visual Studio Code
Install [Visual Studio Code](https://code.visualstudio.com/Download) and add the following extensions:
* adrianwilczynski.csharp-to-typescript
* alexiv.vscode-angular2-files
* Angular.ng-template
* danwahlin.angular2-snippets
* donjayamanne.githistory
* esbenp.prettier-vscode
* johnpapa.Angular2
* kisstkondoros.vscode-codemetrics
* Mikael.Angular-BeastCode
* ms-dotnettools.csharp
* ms-vscode.powershell
* ms-vscode.vscode-typescript-tslint-plugin
* ms-vsts.team
* msjsdiag.debugger-for-chrome
* PKief.material-icon-theme
* shd101wyy.markdown-preview-enhanced
* VisualStudioExptTeam.vscodeintellicode
* yzhang.markdown-all-in-one


### Git config
To find the path to the **.gitconfig** file, type the following command:   
`git config --list --show-origin`   
Open your **.gitconfig** file and add this configuration:
```
[http "https://tfsdm.eu.labinal.snecma/"]
                sslVerify = false
                proxy = ""
[http "https://azure.devops.thebiadevcompany/"]
                sslVerify = false
                proxy = ""
```


## Create a new project
Retrieve the latest version of the **[BIA.AngularTemplate.X.Y.Z.zip](../../Docs/BIAExtension)**.   
Copy/Paste the contents of the zip into the Angular folder of your new project.   
Inside the Angular folder of your new project, run the powershell script `new-angular-project.ps1`.   
For **old project name?**, type **BIATemplate**   
For **new project name?**, type the name of your project   
Update the version of the application. To do this, change the `version` variable in **src\environments\environment.ts** and **src\environments\environment.prod.ts**.   
Warning in **src\environments\environment.ts** the apiUrl could be : 'http://localhost/[ProjectName]/**WebApi**/api' or 'http://localhost/[ProjectName]/api' it depend how you have configure the backend api in IIS (or properties of the Visual studio project)
And serverLoggingUrl: 'http://localhost/JobMonitor/**WebApi**/api/logs' or http://localhost/JobMonitor/api/logs'

## File not to be modified
Some files are part of the Framework and should not be modified.

* src/app/core/bia-core
* src/app/shared/bia-shared
* src/assets/bia
* src/scss/bia
* src/app/features/sites
* src/app/features/users


## NPM Package
The content of the framework is normally sufficient for the needs of any project. You should never install any other npm package other than those provided by the Framework.   You should not use the `ng update` command.   
The component library chosen for this framework is [PrimeNG](https://www.primefaces.org/primeng/v9.1.4-lts/). You must use only these components.   
If the content of this framework is not enough, please contact first [Jérémie Souques](mailto:jeremie.souques@thebiadevcompanygroup.com) before installing an npm package on your project.

## Design / Layout
If you need to modify the PrimeNG component design, you can modify the following file: src\scss\\_app-custom-theme.scss   
For example you can change the row/cell size of the tables by changing the following `padding` property:
``` scss
p-table {
  td {
    font-weight: 300;
    padding: 0.414em 0.857em !important;
  }
}
```
For the layout, [angular/flex-layout](https://github.com/angular/flex-layout/wiki) is used. [Here](https://tburleson-layouts-demos.firebaseapp.com/#/docs) is a help site.
## NGRX Store
The framework and management of the store is based on this application. You can follow this example for the implementation of your store:   
[angular-contacts-app-example](https://github.com/avatsaev/angular-contacts-app-example)

## Before commit
before committing your changes, run the following commands:

* ng lint: You must have the following message: "All files pass linting".
* ng build --aot: You must not get an error message.

