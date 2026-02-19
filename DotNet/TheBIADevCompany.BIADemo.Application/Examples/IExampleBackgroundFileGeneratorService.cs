// <copyright file="IExampleBackgroundFileGeneratorService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Examples
{
    using BIA.Net.Core.Application.Services;

    /// <summary>
    /// Example interface for a background file generator service that can be used with the <see cref="IBiaFileDownloaderService"/> to prepare file downloads in background jobs.
    /// </summary>
    public interface IExampleBackgroundFileGeneratorService : IBiaBackgroundFileGeneratorService
    {
    }
}
