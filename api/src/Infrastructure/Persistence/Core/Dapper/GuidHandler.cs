using System;
using System.Data;
using Dapper;

/// <summary>
/// Handler to ignore empty guids, and correctly parse them when storing / retrieving
/// from database.
/// </summary>
public class GuidHandler : SqlMapper.TypeHandler<Guid> {
    public override void SetValue(IDbDataParameter parameter, Guid value) {
        if (value == Guid.Empty) {
            parameter.Value = null;
        } else {
            parameter.Value = value;
        }
    }

    public override Guid Parse(object value) {
        if (value == null) {
            return Guid.Empty;
        } else {
            return (Guid)value;
        }
    }
}