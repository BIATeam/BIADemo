# Configure your development environment:

## Minimum requierement

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

## Company Customisation
If your company have a proxy specify it (exact values can be describe in your company docs : CompanyConfig.md)

### Configuration for the Company proxy
[Add the following **User** environment variables :](https://www.tenforums.com/tutorials/121664-set-new-user-system-environment-variables-windows.html#option1)  
* HTTP_PROXY: [Add here your proxy]
* HTTPS_PROXY: [Add here your proxy]
* NO_PROXY: [Add here the local domain extensions taht do not need proxy separate by a space]
* 
### Git config
To find the path to the **.gitconfig** file, type the following command:   
`git config --list --show-origin`   
Open your **.gitconfig** file and add this configuration:
```
[http]
	sslVerify = false
	proxy = "[Add here your proxy if requiered]"
```