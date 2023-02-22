#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Url Red Hat container:
#https://catalog.redhat.com/software/containers/search

#.NET 6.0 Runtime Only
FROM registry.access.redhat.com/ubi8/dotnet-60-runtime@sha256:b5e7a494fc04700108eb80eb28fa5ad044abf1fb7dc59a204c066935a06e3a03 AS base
WORKDIR /app
#USER root
#COPY /ca-certificates/ca-bundle.crt /etc/pki/ca-trust/source/anchors/ca-bundle.crt
#RUN update-ca-trust
#RUN chmod a+rx /etc/pki/ca-trust/source/anchors/ca-bundle.crt
#USER 1001

#.NET 6.0 SDK
FROM registry.access.redhat.com/ubi8/dotnet-60@sha256:b1d8c102abc3212aa0261883157814d15115ccc3c6d93463532eb35a5c1497c0 AS build
WORKDIR /src
#USER root
#COPY /ca-certificates/ca-bundle.crt /etc/pki/ca-trust/source/anchors/ca-bundle.crt
#RUN update-ca-trust

COPY ["nuget.config", "."]
COPY ["TheBIADevCompany.BIADemo.WorkerService/TheBIADevCompany.BIADemo.WorkerService.csproj", "TheBIADevCompany.BIADemo.WorkerService/"]
COPY ["TheBIADevCompany.BIADemo.Crosscutting.Ioc/TheBIADevCompany.BIADemo.Crosscutting.Ioc.csproj", "TheBIADevCompany.BIADemo.Crosscutting.Ioc/"]
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
COPY ["TheBIADevCompany.BIADemo.Infrastructure.Service/TheBIADevCompany.BIADemo.Infrastructure.Service.csproj", "TheBIADevCompany.BIADemo.Infrastructure.Service/"]
COPY ["BIAPackage/BIA.Net.Core.Infrastructure.Service/BIA.Net.Core.Infrastructure.Service.csproj", "BIAPackage/BIA.Net.Core.Infrastructure.Service/"]
COPY ["BIAPackage/BIA.Net.Core.Ioc/BIA.Net.Core.Ioc.csproj", "BIAPackage/BIA.Net.Core.Ioc/"]
COPY ["BIAPackage/BIA.Net.Core.WorkerService/BIA.Net.Core.WorkerService.csproj", "BIAPackage/BIA.Net.Core.WorkerService/"]
COPY ["BIAPackage/BIA.Net.Core.Presentation.Common/BIA.Net.Core.Presentation.Common.csproj", "BIAPackage/BIA.Net.Core.Presentation.Common/"]
RUN dotnet restore "TheBIADevCompany.BIADemo.WorkerService/TheBIADevCompany.BIADemo.WorkerService.csproj"
COPY . .
WORKDIR "/src/TheBIADevCompany.BIADemo.WorkerService"
RUN dotnet build "TheBIADevCompany.BIADemo.WorkerService.csproj" -c Release -o /app/build /p:RunAnalyzers=false

FROM build AS publish
RUN dotnet publish "TheBIADevCompany.BIADemo.WorkerService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TheBIADevCompany.BIADemo.WorkerService.dll"]