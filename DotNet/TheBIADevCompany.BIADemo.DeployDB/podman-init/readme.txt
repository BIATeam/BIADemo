1/ Install Podman:
https://bw861987.medium.com/visual-studio-2022-vscode-integrate-with-podman-on-windows-4a6cb4b9b49b

2/ Add settings.json here: C:\Users\<YourLogin>\AppData\Roaming\Docker
Set the settings.json and change the <YourLogin> in "docker.context.defaultPath": "C:\\Users\\<YourLogin>\\AppData\\Roaming\\Docker",

3/ Load SqlServer2022 image in Podman

4/ Edit the podman-init-sqlserver.cmd and set the variable "imageSqlServer" with the correct name
5/ Execute the podman-init-sqlserver.cmd
6/ Execute the podman-init-db.cmd

7/ Run your project in Podman mode