// <copyright file="ExceptionHelper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Common.Helpers
{
    using BIA.Net.Core.Common.Error;

    /// <summary>
    /// Helpers for exceptions.
    /// </summary>
    public static class ExceptionHelper
    {
        /// <summary>
        /// Return the error message associated to the <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The <see cref="BiaErrorId"/>.</param>
        /// <returns>A <see cref="string"/> of the formated error message.</returns>
        public static string GetErrorMessage(BiaErrorId key)
        {
            return key switch
            {
                BiaErrorId.AddEntity => "Unable to add {0}.",
                BiaErrorId.DeleteEntity => "Unable to delete {0}.",
                BiaErrorId.ModifyEntity => "Unable to modify {0}.",
                BiaErrorId.ValidationEntity => "{0}",
                BiaErrorId.DatabaseForeignKeyConstraint => "One or many {0} are linked to dependants entities. Ensure to remove all dependants entities first.",
                BiaErrorId.DatabaseUniqueConstraint => "An entity {0} exists already with these values.",
                BiaErrorId.DatabaseDuplicateKey => "An entity {0} exists already with these values.",
                BiaErrorId.DatabaseNullValueInsert => "Field {0} cannot be null in entity {1}.",
                BiaErrorId.DatabaseLoginUser => "Unable to login to the database.",
                BiaErrorId.DatabaseOpen => "Unable to open the database.",
                _ => "Internal server error",
            };
        }
    }
}
