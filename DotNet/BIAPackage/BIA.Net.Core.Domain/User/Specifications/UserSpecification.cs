// <copyright file="UserSpecification.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.User.Specifications
{
    using BIA.Net.Core.Domain.Specification;
    using BIA.Net.Core.Domain.User.Entities;

    /// <summary>
    /// The specifications of the user entity.
    /// </summary>
    /// <typeparam name="TUser">The type of user.</typeparam>
    public static class UserSpecification<TUser>
        where TUser : User
    {
        /// <summary>
        /// Search users using the filter on lastname, firstname or login.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>The specification.</returns>
        public static Specification<TUser> Search(string filter)
        {
            Specification<TUser> specification = new TrueSpecification<TUser>();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                specification &= new DirectSpecification<TUser>(u =>
                    u.LastName.Contains(filter) || u.FirstName.Contains(filter) || u.Login.Contains(filter));
            }

            specification &= new DirectSpecification<TUser>(u => u.IsActive);

            return specification;
        }
    }
}