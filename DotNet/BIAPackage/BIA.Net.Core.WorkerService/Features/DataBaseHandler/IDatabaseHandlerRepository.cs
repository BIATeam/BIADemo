// <copyright file="DatabaseHandlerRepository.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.WorkerService.Features.DataBaseHandler
{
    using System;

    public interface IDatabaseHandlerRepository
    {
        void Start(IServiceProvider serviceProvider);
        void Stop();
    }
}