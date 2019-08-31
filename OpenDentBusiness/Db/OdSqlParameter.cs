using System;

namespace OpenDentBusiness
{
    /// <summary>
    /// Hold parameter info in a database independent manner.
    /// </summary>
    public class OdSqlParameter
    {
        public OdDbType DbType { get; set; }

        public string ParameterName { get; set; }

        public Object Value { get; set; }

        public OdSqlParameter(string parameterName, OdDbType dbType, Object value)
        {
            ParameterName = parameterName;
            DbType = dbType;
            Value = value;
        }
    }
}