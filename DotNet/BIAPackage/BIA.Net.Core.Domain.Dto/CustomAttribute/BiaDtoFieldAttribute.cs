// <copyright file="BiaDtoFieldAttribute.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
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
        /// "Block generation" field value.
        /// </summary>
        public bool IsParent { get; set; }

        /// <summary>
        /// "Item type" field value.
        /// </summary>
        public string ItemType { get; set; }

        /// <summary>
        /// Indicates whether the property model validation must be active or not.
        /// </summary>
        public bool EnableModelValidation { get; set; }

        /// <summary>
        /// Indicates whether the property required or not.
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Set the minimum value of the number property.
        /// </summary>
        public double RangeMin { get; set; }

        /// <summary>
        /// Set the maximum value of the number property.
        /// </summary>
        public double RangeMax { get; set; }

        /// <summary>
        /// Set the minimum length of the string property.
        /// </summary>
        public int MinLength { get; set; }

        /// <summary>
        /// Set the maximum length of the string property.
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// Set the regex pattern of the string property.
        /// </summary>
        public string RegexPattern { get; set; }

        /// <summary>
        /// Indicates whether the property is email format or not.
        /// </summary>
        public bool Email { get; set; }

        /// <summary>
        /// Indicates whether the property is phone format or not.
        /// </summary>
        public bool Phone { get; set; }

        /// <summary>
        /// Indicates whether the property is url format or not.
        /// </summary>
        public bool Url { get; set; }

        /// <summary>
        /// Indicates whether the property is credit card format or not.
        /// </summary>
        public bool CreditCard { get; set; }

        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!this.EnableModelValidation)
            {
                return ValidationResult.Success;
            }

            var validationResults = new List<ValidationResult>
            {
                this.ValidateRequired(value, validationContext),
                this.ValidateRange(value, validationContext),
                this.ValidateStringLength(value, validationContext),
                this.ValidateRegularExpression(value, validationContext),
                this.ValidateEmail(value, validationContext),
                this.ValidatePhone(value, validationContext),
                this.ValidateUrl(value, validationContext),
                this.ValidateCreditCard(value, validationContext),
            };

            var errorMessages = validationResults
                .Where(vr => vr != ValidationResult.Success)
                .Select(vr => vr.ErrorMessage);

            if (errorMessages.Any())
            {
                return new ValidationResult(string.Join(" ", errorMessages));
            }

            return ValidationResult.Success;
        }

        private static bool IsNumericType(object value)
        {
            return value is sbyte || value is byte ||
                   value is short || value is ushort ||
                   value is int || value is uint ||
                   value is long || value is ulong ||
                   value is float || value is double ||
                   value is decimal;
        }

        private static object GetMinNumericValue(object value)
        {
            return value switch
            {
                sbyte _ => sbyte.MinValue,
                byte _ => byte.MinValue,
                short _ => short.MinValue,
                ushort _ => ushort.MinValue,
                int _ => int.MinValue,
                uint _ => uint.MinValue,
                long _ => long.MinValue,
                ulong _ => ulong.MinValue,
                float _ => float.MinValue,
                double _ => double.MinValue,
                decimal _ => decimal.MinValue,
                _ => throw new ArgumentException("Invalid numeric type", nameof(value)),
            };
        }

        private static object GetMaxNumerciValue(object value)
        {
            return value switch
            {
                sbyte _ => sbyte.MaxValue,
                byte _ => byte.MaxValue,
                short _ => short.MaxValue,
                ushort _ => ushort.MaxValue,
                int _ => int.MaxValue,
                uint _ => uint.MaxValue,
                long _ => long.MaxValue,
                ulong _ => ulong.MaxValue,
                float _ => float.MaxValue,
                double _ => double.MaxValue,
                decimal _ => decimal.MaxValue,
                _ => throw new ArgumentException("Invalid numeric type", nameof(value)),
            };
        }

        private ValidationResult ValidateRequired(object value, ValidationContext validationContext)
        {
            if (this.Required)
            {
                var requiredAttribute = new RequiredAttribute();
                return requiredAttribute.GetValidationResult(value, validationContext);
            }

            return ValidationResult.Success;
        }

        private ValidationResult ValidateRange(object value, ValidationContext validationContext)
        {
#pragma warning disable S1244 // Floating point numbers should not be tested for equality
            if (value != null && IsNumericType(value) && (this.RangeMin != 0 || this.RangeMax != 0))
            {
                var rangeMax = this.RangeMax != 0 ? this.RangeMax : GetMaxNumerciValue(value);
                var rangeMin = this.RangeMin != 0 ? this.RangeMin : GetMinNumericValue(value);
                var rangeAttribute = new RangeAttribute(Convert.ToDouble(rangeMin), Convert.ToDouble(rangeMax));

                return rangeAttribute.GetValidationResult(value, validationContext);
            }
#pragma warning restore S1244 // Floating point numbers should not be tested for equality

            return ValidationResult.Success;
        }

        private ValidationResult ValidateStringLength(object value, ValidationContext validationContext)
        {
            if (value is string stringValue && (this.MinLength > 0 || this.MaxLength > 0))
            {
                var stringLengthAttribute = new StringLengthAttribute(this.MaxLength == 0 ? int.MaxValue : this.MaxLength)
                {
                    MinimumLength = this.MinLength <= 0 ? 0 : this.MinLength,
                };
                return stringLengthAttribute.GetValidationResult(stringValue, validationContext);
            }

            return ValidationResult.Success;
        }

        private ValidationResult ValidateRegularExpression(object value, ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(this.RegexPattern) && value is string stringValue)
            {
                var regexAttribute = new RegularExpressionAttribute(this.RegexPattern);
                return regexAttribute.GetValidationResult(stringValue, validationContext);
            }

            return ValidationResult.Success;
        }

        private ValidationResult ValidateEmail(object value, ValidationContext validationContext)
        {
            if (this.Email && value is string emailValue)
            {
                var emailAttribute = new EmailAddressAttribute();
                return emailAttribute.GetValidationResult(emailValue, validationContext);
            }

            return ValidationResult.Success;
        }

        private ValidationResult ValidatePhone(object value, ValidationContext validationContext)
        {
            if (this.Phone && value is string phoneValue)
            {
                var phoneAttribute = new PhoneAttribute();
                return phoneAttribute.GetValidationResult(phoneValue, validationContext);
            }

            return ValidationResult.Success;
        }

        private ValidationResult ValidateUrl(object value, ValidationContext validationContext)
        {
            if (this.Url && value is string urlValue)
            {
                var urlAttribute = new UrlAttribute();
                return urlAttribute.GetValidationResult(urlValue, validationContext);
            }

            return ValidationResult.Success;
        }

        private ValidationResult ValidateCreditCard(object value, ValidationContext validationContext)
        {
            if (this.CreditCard && value is string creditCardValue)
            {
                var creditCardAttribute = new CreditCardAttribute();
                return creditCardAttribute.GetValidationResult(creditCardValue, validationContext);
            }

            return ValidationResult.Success;
        }
    }
}
