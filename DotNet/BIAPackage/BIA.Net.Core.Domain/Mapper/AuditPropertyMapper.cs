namespace BIA.Net.Core.Domain.Mapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    public class AuditPropertyMapper<TReferenceEntity> : IAuditPropertyMapper
    {
        public string ReferenceEntityPropertyName => Common.Helpers.PropertyMapper.GetPropertyName(this.ReferenceEntityProperty);
        public string ReferenceEntityPropertyIdentifierName => Common.Helpers.PropertyMapper.GetPropertyName(this.ReferenceEntityPropertyIdentifier);

        public required Expression<Func<TReferenceEntity, object>> ReferenceEntityProperty { get; set; }
        public required Expression<Func<TReferenceEntity, object>> ReferenceEntityPropertyIdentifier { get; set; }
    }
}
