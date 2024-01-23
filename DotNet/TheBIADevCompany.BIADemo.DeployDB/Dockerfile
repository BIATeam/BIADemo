#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Url Red Hat container:
#https://catalog.redhat.com/software/containers/search

#.NET Runtime Only
FROM registry.access.redhat.com/ubi8/dotnet-80-runtime@sha256:bcca46f2b836ebcb0d552aac3e455f7131907e64627b2d5711cfbb4ef381e9b6 AS base
WORKDIR /app
#USER root
#COPY /ca-certificates/ca-bundle.crt /etc/pki/ca-trust/source/anchors/ca-bundle.crt
#RUN update-ca-trust
#RUN chmod a+rx /etc/pki/ca-trust/source/anchors/ca-bundle.crt
#USER 1001

#.NET SDK
FROM registry.access.redhat.com/ubi8/dotnet-80@sha256:d2fc25735545ed3649a094cc36f78cfaebed2d0744a456cae9a33909025ac4da AS build
WORKDIR /src
#USER root
#COPY /ca-certificates/ca-bundle.crt /etc/pki/ca-trust/source/anchors/ca-bundle.crt
#RUN update-ca-trust

COPY ["nuget.config", "."]
COPY ["TheBIADevCompany.BIADemo.DeployDB/TheBIADevCompany.BIADemo.DeployDB.csproj", "TheBIADevCompany.BIADemo.DeployDB/"]
COPY ["TheBIADevCompany.BIADemo.Application/TheBIADevCompany.BIADemo.Application.csproj", "TheBIADevCompany.BIADemo.Application/"]
COPY ["TheBIADevCompany.BIADemo.Crosscutting.Common/TheBIADevCompany.BIADemo.Crosscutting.Common.csproj", "TheBIADevCompany.BIADemo.Crosscutting.Common/"]
COPY ["BIAPackage/BIA.Net.Core.Common/BIA.Net.Core.Common.csproj", "BIAPackage/BIA.Net.Core.Common/"]
COPY ["TheBIADevCompany.BIADemo.Domain.Dto/TheBIADevCompany.BIADemo.Domain.Dto.csproj", "TheBIADevCompany.BIADemo.Domain.Dto/"]
COPY ["BIAPackage/BIA.Net.Core.Domain.Dto/BIA.Net.Core.Domain.Dto.csproj", "BIAPackage/BIA.Net.Core.Domain.Dto/"]
COPY ["TheBIADevCompany.BIADemo.Domain/TheBIADevCompany.BIADemo.Domain.csproj", "TheBIADevCompany.BIADemo.Domain/"]
COPY ["BIAPackage/BIA.Net.Core.Domain/BIA.Net.Core.Domain.csproj", "BIAPackage/BIA.Net.Core.Domain/"]
COPY ["BIAPackage/BIA.Net.Core.Application/BIA.Net.Core.Application.csproj", "BIAPackage/BIA.Net.Core.Application/"]
COPY ["TheBIADevCompany.BIADemo.Infrastructure.Data/TheBIADevCompany.BIADemo.Infrastructure.Data.csproj", "TheBIADevCompany.BIADemo.Infrastructure.Data/"]
COPY ["BIAPackage/BIA.Net.Core.Infrastructure.Data/BIA.Net.Core.Infrastructure.Data.csproj", "BIAPackage/BIA.Net.Core.Infrastructure.Data/"]
RUN dotnet restore "TheBIADevCompany.BIADemo.DeployDB/TheBIADevCompany.BIADemo.DeployDB.csproj"
COPY . .
WORKDIR "/src/TheBIADevCompany.BIADemo.DeployDB"
RUN dotnet build "TheBIADevCompany.BIADemo.DeployDB.csproj" -c Release -o /app/build /p:RunAnalyzers=false

FROM build AS publish
RUN dotnet publish "TheBIADevCompany.BIADemo.DeployDB.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TheBIADevCompany.BIADemo.DeployDB.dll"]