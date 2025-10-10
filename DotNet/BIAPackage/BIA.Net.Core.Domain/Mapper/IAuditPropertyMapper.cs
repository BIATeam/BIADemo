namespace BIA.Net.Core.Domain.Mapper
{
    using System;

    public interface IAuditPropertyMapper
    {
        string ReferenceEntityPropertyName { get; }
        string ReferenceEntityPropertyIdentifierName { get; }
    }
}