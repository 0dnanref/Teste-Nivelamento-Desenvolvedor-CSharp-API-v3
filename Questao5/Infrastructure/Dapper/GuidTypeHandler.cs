﻿using Dapper;
using System.Data;

namespace Questao5.Infrastructure.Dapper
{
    public class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
    {
        public override Guid Parse(object value) => Guid.Parse(value.ToString()!);
        public override void SetValue(IDbDataParameter parameter, Guid value) => parameter.Value = value.ToString();
    }
}
