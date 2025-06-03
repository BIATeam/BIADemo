// <copyright file="UserExtensions.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.User
{
    using BIA.Net.Core.Domain.User.Entities;

    /// <summary>
    /// Extension for User.
    /// </summary>
    public static class UserExtensions
    {
        /// <summary>
        ///  Return the user format for display.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The formated display.</returns>
        public static string Display(this BaseUser user)
        {
            string display = null;

            if (user != null)
            {
                display = $"{user.LastName} {user.FirstName} ({user.Login})";
            }

            return display;
        }

        /// <summary>
        ///  Return the user short format to display in list ....
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The formated display.</returns>
        public static string DisplayShort(this BaseUser user)
        {
            string display = null;

            if (user != null)
            {
                display = $"{user.LastName} {user.FirstName}";
            }

            return display;
        }
    }
}