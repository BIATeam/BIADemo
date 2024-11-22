// <copyright file="Program.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

#pragma warning disable SA1200 // Using directives should be placed correctly
using System;
using BIA.Net.Core.Application.Authentication;
using BIA.Net.Core.Common.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Web;
using TheBIADevCompany.BIADemo.Crosscutting.Common;
using TheBIADevCompany.BIADemo.Presentation.Api;
#pragma warning restore SA1200 // Using directives should be placed correctly

Logger logger = default;

// NLog: setup the logger first to catch all errors
try
{
    logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
    string environment = GetEnvironment(logger);

    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    builder.Configuration.AddJsonFile($"appsettings.{environment}.json", optional: false, reloadOnChange: true);
    builder.Configuration.AddJsonFile("bianetconfig.json", optional: false, reloadOnChange: true);
    builder.Configuration.AddJsonFile($"bianetconfig.{environment}.json", optional: false, reloadOnChange: true);

    builder.Host.UseNLog();

    Startup startup = new Startup(builder.Configuration);

    startup.ConfigureServices(builder);

    WebApplication app = builder.Build();

    BiaNetSection biaNetSection = new BiaNetSection();
    builder.Configuration.GetSection("BiaNet").Bind(biaNetSection);
    startup.Configure(app, app.Environment, new JwtFactory(Options.Create<Jwt>(biaNetSection.Jwt)));

    await app.RunAsync();
}
catch (Exception ex)
{
    logger?.Error(ex, "Stopped program because of exception");
    throw new BIA.Net.Core.Common.Exceptions.SystemException("Stopped program because of exception", ex);
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit
    LogManager.Shutdown();
}

static string GetEnvironment(ILogger logger)
{
    string environment = Environment.GetEnvironmentVariable(Constants.Application.Environment);
    string msg = $"{Constants.Application.Environment}: {environment}";
    logger.Info(msg);
    return environment;
}