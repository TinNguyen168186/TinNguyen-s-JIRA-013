using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;

namespace Core.Utilities
{
    public static class DapperExtension
    {
        public static IEnumerable<Dictionary<string, string?>> Query(this OleDbConnection connection, string query)
        {
            var data = Dapper.SqlMapper.Query(connection, query) as IEnumerable<Dictionary<string, object>>;

            return data?.Select(x => x.ToDictionary(y => y.Key, y => y.Value?.ToString())) 
                   ?? Enumerable.Empty<Dictionary<string, string?>>();
        }
    }
}