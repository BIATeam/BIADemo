// <copyright file="UserExtensions.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate
{
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
        public static string SelectDisplay(this User user)
        {
            string display = null;

            if (user != null)
            {
                display = $"{user.FirstName} {user.LastName} ({user.Login})";
            }

            return display;
        }
    }
}