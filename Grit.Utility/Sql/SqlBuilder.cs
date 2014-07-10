using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Utility.Sql
{
    public static class SqlBuilder
    {
        public static string Columns(IEnumerable<string> columns)
        {
            return string.Join(", ", columns);
        }
        public static string Columns(string table, IEnumerable<string> columns)
        {
            return string.Join(", ", columns.Select(n => table + "." + n).ToArray());
        }
        public static string ColumnsWithAlias(string table, IEnumerable<string> columns)
        {
            return string.Join(", ", columns.Select(n => table + "." + n + " AS " + table + n).ToArray());
        }
        public static string Params(IEnumerable<string> columns)
        {
            return "@" + string.Join(", @", columns);
        }
        public static string Sets(IEnumerable<string> columns)
        {
            return string.Join(", ", columns.Select(n => n + " = @" + n).ToArray());
        }
    }
}
