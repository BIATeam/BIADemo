// <copyright file="BaseQueryModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.QueryModel
{
    using BIA.Net.Core.Domain.Dto.Base;

    /// <summary>
    /// Serves as a base class for query models that include a key identifier.
    /// </summary>
    /// <typeparam name="TKey">The type of the key identifier for the query model.</typeparam>
    public abstract class BaseQueryModel<TKey> : BaseDto<TKey>
    {
    }
}
