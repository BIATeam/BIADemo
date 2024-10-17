// <copyright file="BiaDtoFieldAttribute.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.CustomAttribute
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    /// <summary>
    /// The custom attibute class for DTO fields.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class BiaDtoFieldAttribute : ValidationAttribute
    {
        /// <summary>
        /// The Dto field type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// "Is required" field value.
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// "Block generation" field value.
        /// </summary>
        public bool IsParent { get; set; }

        /// <summary>
        /// "Item type" field value.
        /// </summary>
        public string ItemType { get; set; }

        public int RangeMin { get; set; }
        public int RangeMax { get; set; }
        public int MinLength { get; set; }
        public int MaxLength { get; set; }
        public string Pattern { get; set; }
        public bool Email { get; set; }
        public bool Phone { get; set; }
        public bool Url { get; set; }
        public bool CreditCard { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Collect all validation results
            var validationResults = new List<ValidationResult>
            {
                ValidateRequired(value, validationContext),
                ValidateRange(value, validationContext),
                ValidateStringLength(value, validationContext),
                ValidateRegularExpression(value, validationContext),
                ValidateEmail(value, validationContext),
                ValidatePhone(value, validationContext),
                ValidateUrl(value, validationContext),
                ValidateCreditCard(value, validationContext)
            };

            // Filter out successful validations and concatenate error messages
            var errorMessages = validationResults
                .Where(vr => vr != ValidationResult.Success)
                .Select(vr => vr.ErrorMessage)
                .ToList();

            // If there are any validation errors, return them as a single concatenated message
            if (errorMessages.Any())
            {
                return new ValidationResult(string.Join(" ", errorMessages));
            }

            // If all validations pass, return success.
            return ValidationResult.Success;
        }

        // Each Validate method now returns a ValidationResult

        private ValidationResult ValidateRequired(object value, ValidationContext validationContext)
        {
            if (Required)
            {
                var requiredAttribute = new RequiredAttribute();
                return requiredAttribute.GetValidationResult(value, validationContext);
            }
            return ValidationResult.Success;
        }

        private ValidationResult ValidateRange(object value, ValidationContext validationContext)
        {
            if (RangeMin > 0 && RangeMax > 0 && value != null)
            {
                var rangeAttribute = new RangeAttribute(RangeMin, RangeMax);
                return rangeAttribute.GetValidationResult(value, validationContext);
            }
            return ValidationResult.Success;
        }

        private ValidationResult ValidateStringLength(object value, ValidationContext validationContext)
        {
            if (value is string stringValue)
            {
                if (MinLength > 0 || MaxLength > 0)
                {
                    var stringLengthAttribute = new StringLengthAttribute(MaxLength == 0 ? int.MaxValue : MaxLength)
                    {
                        MinimumLength = MinLength == 0 ? 0 : MinLength
                    };
                    return stringLengthAttribute.GetValidationResult(stringValue, validationContext);
                }
            }
            return ValidationResult.Success;
        }

        private ValidationResult ValidateRegularExpression(object value, ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Pattern) && value is string stringValue)
            {
                var regexAttribute = new RegularExpressionAttribute(Pattern);
                return regexAttribute.GetValidationResult(stringValue, validationContext);
            }
            return ValidationResult.Success;
        }

        private ValidationResult ValidateEmail(object value, ValidationContext validationContext)
        {
            if (Email && value is string emailValue)
            {
                var emailAttribute = new EmailAddressAttribute();
                return emailAttribute.GetValidationResult(emailValue, validationContext);
            }
            return ValidationResult.Success;
        }

        private ValidationResult ValidatePhone(object value, ValidationContext validationContext)
        {
            if (Phone && value is string phoneValue)
            {
                var phoneAttribute = new PhoneAttribute();
                return phoneAttribute.GetValidationResult(phoneValue, validationContext);
            }
            return ValidationResult.Success;
        }

        private ValidationResult ValidateUrl(object value, ValidationContext validationContext)
        {
            if (Url && value is string urlValue)
            {
                var urlAttribute = new UrlAttribute();
                return urlAttribute.GetValidationResult(urlValue, validationContext);
            }
            return ValidationResult.Success;
        }

        private ValidationResult ValidateCreditCard(object value, ValidationContext validationContext)
        {
            if (CreditCard && value is string creditCardValue)
            {
                var creditCardAttribute = new CreditCardAttribute();
                return creditCardAttribute.GetValidationResult(creditCardValue, validationContext);
            }
            return ValidationResult.Success;
        }
    }
}
