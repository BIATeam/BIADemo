// <copyright file="FileDownloadTokenRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Repositories
{
    using System;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.File.Entities;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// FileDownloadTokenRepository is responsible for managing the persistence of FileDownloadToken entities in the database.
    /// </summary>
    public class FileDownloadTokenRepository : IFileDownloadTokenRepository
    {
        private readonly IQueryableUnitOfWork queryableUnitOfWork;
        private readonly DbSet<FileDownloadToken> fileDownloadTokens;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDownloadTokenRepository"/> class.
        /// </summary>
        /// <param name="queryableUnitOfWork">The queryable unit of work.</param>
        public FileDownloadTokenRepository(IQueryableUnitOfWork queryableUnitOfWork)
        {
            this.queryableUnitOfWork = queryableUnitOfWork;
            this.fileDownloadTokens = this.queryableUnitOfWork.RetrieveSet<FileDownloadToken>();
        }

        /// <inheritdoc/>
        public async Task<FileDownloadToken> GetAsync(Guid fileGuid, string token)
        {
            return await this.fileDownloadTokens
                .Include(x => x.FileDownloadData)
                .ThenInclude(x => x.RequestByUser)
                .AsNoTracking()
                .FirstOrDefaultAsync(fdt => fdt.FileGuid == fileGuid && fdt.Token == token);
        }

        /// <inheritdoc/>
        public async Task AddAsync(FileDownloadToken fileDownloadToken)
        {
            await this.fileDownloadTokens.AddAsync(fileDownloadToken);
            await this.queryableUnitOfWork.CommitAsync();
        }

        /// <inheritdoc/>
        public async Task RemoveAsync(FileDownloadToken fileDownloadToken)
        {
            this.fileDownloadTokens.Remove(fileDownloadToken);
            await this.queryableUnitOfWork.CommitAsync();
        }
    }
}
