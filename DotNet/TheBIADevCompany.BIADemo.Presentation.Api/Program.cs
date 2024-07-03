// <copyright file="Program.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

#pragma warning disable SA1200 // Using directives should be placed correctly
using System;
using BIA.Net.Core.Application.Authentication;
using BIA.Net.Core.Common.Configuration;
using BIA.Net.Core.Presentation.Common.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using TheBIADevCompany.BIADemo.Presentation.Api;
#pragma warning restore SA1200 // Using directives should be placed correctly

// NLog: setup the logger first to catch all errors
try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    builder.Configuration.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: false, reloadOnChange: true);
    builder.Configuration.AddJsonFile("bianetconfig.json", optional: false, reloadOnChange: true);
    builder.Configuration.AddJsonFile($"bianetconfig.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: false, reloadOnChange: true);

    builder.Host.UseNLog();

    Startup startup = new Startup(builder.Configuration);

    startup.ConfigureServices(builder.Services);

    WebApplication app = builder.Build();

    BiaNetSection biaNetSection = new BiaNetSection();
    builder.Configuration.GetSection("BiaNet").Bind(biaNetSection);
    startup.Configure(app, app.Environment, new JwtFactory(Options.Create<Jwt>(biaNetSection.Jwt)));

    app.Run();
}
catch (Exception ex)
{
    // NLog: catch setup errors
    var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
    logger.Error(ex, "Stopped program because of exception");
    throw new BIA.Net.Core.Common.Exceptions.SystemException("Stopped program because of exception", ex);
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit
    LogManager.Shutdown();
}