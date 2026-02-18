// <copyright file="FileDownloadDataAppService.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.File
{
    using System;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.File;
    using BIA.Net.Core.Domain.File.Entities;
    using BIA.Net.Core.Domain.File.Mappers;
    using BIA.Net.Core.Domain.RepoContract;

    /// <summary>
    /// The application service used to manage file download data.
    /// </summary>
    public class FileDownloadDataAppService : CrudAppServiceBase<FileDownloadDataDto, FileDownloadData, Guid, PagingFilterFormatDto, FileDownloadDataMapper>, IFileDownloadDataAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileDownloadDataAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The principal.</param>
        public FileDownloadDataAppService(ITGenericRepository<FileDownloadData, Guid> repository, IPrincipal principal)
            : base(repository)
        {
        }
    }
}
