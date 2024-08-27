namespace BIA.Net.Core.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Common.Exceptions;

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
                FrontUserExceptionErrorMessageKey.AddEntity => "Adding {0} entity failed",
                FrontUserExceptionErrorMessageKey.DeleteEntity => "Deleting {0} entity failed",
                FrontUserExceptionErrorMessageKey.EditEntity => "Edit {0} entity failed",
                FrontUserExceptionErrorMessageKey.ValidationEntity => "Entity validation failed for following members : {0}\n{1}",
                FrontUserExceptionErrorMessageKey.DatabaseForeignKeyConstraint => "An entity {0} is still linked to other entities",
                _ => "Internal server error",
            };
        }
    }
}
