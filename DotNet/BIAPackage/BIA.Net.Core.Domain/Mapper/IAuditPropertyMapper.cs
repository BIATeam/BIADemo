namespace BIA.Net.Core.Domain.Mapper
{
    using System;

    public interface IAuditPropertyMapper
    {
        Type LinkedEntityType { get; }
        string ReferenceEntityPropertyName { get; }
        string ReferenceEntityPropertyIdentifierName { get; }
        string LinkedEntityPropertyDisplayName { get; }
    }
}