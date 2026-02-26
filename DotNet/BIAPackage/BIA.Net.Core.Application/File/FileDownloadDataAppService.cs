// <copyright file="FileDownloadDataAppService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.File
{
    using System;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.File;
    using BIA.Net.Core.Domain.File.Entities;
    using BIA.Net.Core.Domain.File.Mappers;
    using BIA.Net.Core.Domain.RepoContract;

    /// <summary>
    /// Service for FileDownloadData.
    /// </summary>
    public class FileDownloadDataAppService : CrudAppServiceBase<FileDownloadDataDto, FileDownloadData, Guid, PagingFilterFormatDto, FileDownloadDataMapper>, IFileDownloadDataAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileDownloadDataAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public FileDownloadDataAppService(ITGenericRepository<FileDownloadData, Guid> repository)
            : base(repository)
        {
        }
    }
}
