// <copyright file="ExceptionHelper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Helpers
{
    using BIA.Net.Core.Common.Enum;

    /// <summary>
    /// Helpers for exceptions.
    /// </summary>
    public static class ExceptionHelper
    {
        /// <summary>
        /// Return the error message associated to the <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The <see cref="FrontUserExceptionErrorMessageKey"/>.</param>
        /// <returns>A <see cref="string"/> of the formated error message.</returns>
        public static string GetErrorMessage(FrontUserExceptionErrorMessageKey key)
        {
            return key switch
            {
                FrontUserExceptionErrorMessageKey.AddEntity => "Unable to add {0}.",
                FrontUserExceptionErrorMessageKey.DeleteEntity => "Unable to delete {0}.",
                FrontUserExceptionErrorMessageKey.ModifyEntity => "Unable to modify {0}.",
                FrontUserExceptionErrorMessageKey.ValidationEntity => "{0}",
                FrontUserExceptionErrorMessageKey.DatabaseForeignKeyConstraint => "One or many {0} are linked to dependants entities. Ensure to remove all dependants entities first.",
                FrontUserExceptionErrorMessageKey.DatabaseUniqueConstraint => "An entity {0} exists already with these values.",
                FrontUserExceptionErrorMessageKey.DatabaseDuplicateKey => "An entity {0} exists already with these values.",
                FrontUserExceptionErrorMessageKey.DatabaseNullValueInsert => "Field {0} cannot be null in entity {1}.",
                FrontUserExceptionErrorMessageKey.DatabaseLoginUser => "Unable to login to the database.",
                FrontUserExceptionErrorMessageKey.DatabaseOpen => "Unable to open the database.",
                _ => "Internal server error",
            };
        }
    }
}
