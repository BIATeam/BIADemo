namespace BIA.Net.Core.Domain.Dto.CustomAttribute
{
    using System;

    /// <summary>
    /// The custom attibute class for DTO class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class BIADtoClassAttribute : Attribute
    {
        /// <summary>
        /// The Dto field type.
        /// </summary>
        public string AncestorTeam { get; set; }


        /// <summary>
        /// Constructor.
        /// </summary>
        public BIADtoClassAttribute(string ancestorTeam = null)
        {
            this.AncestorTeam = ancestorTeam;
        }
    }
}
