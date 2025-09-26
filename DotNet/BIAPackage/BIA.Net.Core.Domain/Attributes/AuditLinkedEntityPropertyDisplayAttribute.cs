namespace BIA.Net.Core.Domain.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class AuditLinkedPropertyDisplayAttribute : Attribute
    {
        public AuditLinkedPropertyDisplayAttribute(Type linkedEntityType)
        {
            LinkedEntityType = linkedEntityType;
        }

        public Type LinkedEntityType { get; }
    }
}
