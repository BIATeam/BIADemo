// <copyright file="DataRepository.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Repositories
{
    using BIA.Net.Core.Infrastructure.Data;
    using System;

    /// <summary>
    /// The class representing a GenericRepository.
    /// </summary>
    [Obsolete("Not used any more.", false)]
    public class DataRepository : GenericRepositoryEF
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">QueryableUnitOfWork.</param>
        public DataRepository(IQueryableUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}