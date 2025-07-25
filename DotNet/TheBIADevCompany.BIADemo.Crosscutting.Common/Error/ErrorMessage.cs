// <copyright file="ErrorMessage.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Crosscutting.Common.Error
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using static TheBIADevCompany.BIADemo.Crosscutting.Common.Constants;

    /// <summary>
    /// The class containing all constants.
    /// </summary>
    public static class ErrorMessage
    {
        /// <summary>
        /// List of all custom error translated generated by the back.
        /// </summary>
        private static readonly ImmutableList<ErrorTranslation> Translation =
        [
            new ErrorTranslation() { ErrorId = ErrorId.MemberAlreadyExists, LanguageId = LanguageId.English, Label = "Member already exists." },
            new ErrorTranslation() { ErrorId = ErrorId.MemberAlreadyExists, LanguageId = LanguageId.French, Label = "Le membre existe d�j�." },
            new ErrorTranslation() { ErrorId = ErrorId.MemberAlreadyExists, LanguageId = LanguageId.Spanish, Label = "El miembro ya existe." },

            // Begin project error messages

            // End project error messages
        ];

        /// <summary>
        /// Return the translated message.
        /// </summary>
        /// <param name="errorId">The error Id.</param>
        /// <param name="languageId">The language Id.</param>
        /// <returns>The translated message.</returns>
        public static string GetMessage(ErrorId errorId, int languageId)
        {
            return Translation.Find(t => t.ErrorId == errorId && t.LanguageId == languageId)?.Label;
        }
    }
}