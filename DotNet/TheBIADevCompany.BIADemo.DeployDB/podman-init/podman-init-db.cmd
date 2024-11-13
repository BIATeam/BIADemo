REM Get the current directory path
set "SCRIPT_DIR=%cd%"

REM Extract the drive letter
set "DRIVE_LETTER=%SCRIPT_DIR:~0,1%"

REM Convert the drive letter to lowercase using the powershell command
for /f "delims=" %%i in ('powershell -command "(Get-Location).Drive.Name.ToLower()"') do set "DRIVE_LETTER=%%i"

REM Replace the drive letter with the format expected by Linux
set "LINUX_PATH=%SCRIPT_DIR:\=/%"

REM Add /mnt/ in front and remove the original drive letter with the first slash
set "LINUX_PATH=/mnt/%DRIVE_LETTER%%LINUX_PATH:~2%"

REM Execute the podman command with the modified path
podman machine ssh "podman cp %LINUX_PATH%/initialize.sql mssql_container:/tmp"

podman machine ssh "podman exec -it mssql_container /opt/mssql-tools18/bin/sqlcmd -S tcp:localhost,14330 -U sa -P '<YourStrong!Passw0rd1>' -N -C -i /tmp/initialize.sql"
pause

