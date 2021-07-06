# FRAMEWORK

## Customize PrimeNG Theme
The PrimeNG theme chosen for this framework is the <a href="https://www.primefaces.org/ultima-ng/">Ultima theme</a>.
In the BIADemo project, the content of the theme can be found in the following folders :<br/>
src/assets/bia/primeng<br/>
To be able to customize the theme, you must first install node-sass globally with the following command: <b>npm install -g node-sass</b>.<br/>
You only must change the <b>src/assets/bia/primeng/sass/overrides</b> folder.<br/>
Once the changes have been made, open a cmd. Position yourself on the <b>Angular</b> folder of your project. Then, launch the following command to generate the css.<br/>
Replace xxxxxxxxxxxxxxxxxxxx by MD5 Hash of each files with this site: <a href="https://emn178.github.io/online-tools/md5_checksum.html">md5 checksum</a>.  
``` cmd
node-sass .\src\assets\bia\primeng\theme\theme-primeng-dark-thebiadevcompany.scss .\src\assets\bia\primeng\theme\theme-primeng-dark-thebiadevcompany.xxxxxxxxxxxxxxxxxxxx.css --output-style compressed && node-sass .\src\assets\bia\primeng\layout\css\layout-primeng-dark-thebiadevcompany.scss .\src\assets\bia\primeng\layout\css\layout-primeng-dark-thebiadevcompany.xxxxxxxxxxxxxxxxxxxx.css --output-style compressed && node-sass .\src\assets\bia\primeng\theme\theme-primeng-light-thebiadevcompany.scss .\src\assets\bia\primeng\theme\theme-primeng-light-thebiadevcompany.xxxxxxxxxxxxxxxxxxxx.css --output-style compressed && node-sass .\src\assets\bia\primeng\layout\css\layout-primeng-light-thebiadevcompany.scss .\src\assets\bia\primeng\layout\css\layout-primeng-light-thebiadevcompany.xxxxxxxxxxxxxxxxxxxx.css --output-style compressed
```