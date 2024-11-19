set imageSqlServer=mcr.microsoft.com/mssql/server@sha256:fab59ada87c64cd2dced8b7b964d6600e17797cf68122ae6a18ec8d2fe7281f8

podman rm -f mssql_container
podman volume rm mssql_data
:: podman network rm bia_network -f

podman network create bia_network
podman run -e ACCEPT_EULA=Y -e MSSQL_PID="Developer" -e MSSQL_SA_PASSWORD="<YourStrong!Passw0rd1>" -e MSSQL_TCP_PORT=14330 -v mssql_data:/var/opt/mssql -p 14330:14330 --network bia_network --name mssql_container -d %imageSqlServer%
pause