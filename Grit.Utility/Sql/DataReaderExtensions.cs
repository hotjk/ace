using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Utility.Sql
{
    public static class DataReaderExtensions
    {
        private const string MinSqlDateValue = "1/1/1753";

        public static DateTime GetDateTime(this object value)
        {
            DateTime dateValue = DateTime.MinValue;
            if ((value != null) && (value != DBNull.Value))
            {
                if ((DateTime)value > DateTime.Parse(DataReaderExtensions.MinSqlDateValue))
                {
                    dateValue = (DateTime)value;
                }
            }
            return dateValue;
        }

        public static DateTime? GetNullableDateTime(this object value)
        {
            DateTime? dateTimeValue = null;
            DateTime dbDateTimeValue;
            if (value != null && !Convert.IsDBNull(value))
            {
                if (DateTime.TryParse(value.ToString(), out dbDateTimeValue))
                {
                    dateTimeValue = dbDateTimeValue;
                }
            }
            return dateTimeValue;
        }

        public static int GetInteger(this object value)
        {
            int integerValue = 0;
            if (value != null && !Convert.IsDBNull(value))
            {
                int.TryParse(value.ToString(), out integerValue);
            }
            return integerValue;
        }

        public static int? GetNullableInteger(this object value)
        {
            int? integerValue = null;
            int parseIntegerValue = 0;
            if (value != null && !Convert.IsDBNull(value))
            {
                if (int.TryParse(value.ToString(), out parseIntegerValue))
                {
                    integerValue = parseIntegerValue;
                }
            }
            return integerValue;
        }

        public static decimal GetDecimal(this object value)
        {
            decimal decimalValue = 0;
            if (value != null && !Convert.IsDBNull(value))
            {
                decimal.TryParse(value.ToString(), out decimalValue);
            }
            return decimalValue;
        }

        public static decimal? GetNullableDecimal(this object value)
        {
            decimal? decimalValue = null;
            decimal parseDecimalValue = 0;
            if (value != null && !Convert.IsDBNull(value))
            {
                if (decimal.TryParse(value.ToString(), out parseDecimalValue))
                {
                    decimalValue = parseDecimalValue;
                }
            }
            return decimalValue;
        }

        public static double GetDouble(this object value)
        {
            double doubleValue = 0;
            if (value != null && !Convert.IsDBNull(value))
            {
                double.TryParse(value.ToString(), out doubleValue);
            }
            return doubleValue;
        }

        public static double? GetNullableDouble(this object value)
        {
            double? doubleValue = null;
            double parseDoubleValue = 0;
            if (value != null && !Convert.IsDBNull(value))
            {
                if (double.TryParse(value.ToString(), out parseDoubleValue))
                {
                    doubleValue = parseDoubleValue;
                }
            }

            return doubleValue;
        }

        public static Guid GetGuid(this object value)
        {
            Guid guidValue = Guid.Empty;
            if (value != null && !Convert.IsDBNull(value))
            {
                try
                {
                    guidValue = new Guid(value.ToString());
                }
                catch
                {
                    // really do nothing, because we want to return a value for the guid = Guid.Empty;
                }
            }
            return guidValue;
        }

        public static Guid? GetNullableGuid(this object value)
        {
            Guid? guidValue = null;
            if (value != null && !Convert.IsDBNull(value))
            {
                try
                {
                    guidValue = new Guid(value.ToString());
                }
                catch
                {
                    // really do nothing, because we want to return a value for the guid = null;
                }
            }
            return guidValue;
        }

        public static string GetString(this object value)
        {
            string stringValue = string.Empty;
            if (value != null && !Convert.IsDBNull(value))
            {
                stringValue = value.ToString();
            }
            return stringValue;
        }
        public static string GetNullableString(this object value)
        {
            string stringValue = null;
            if (value != null && !Convert.IsDBNull(value))
            {
                stringValue = value.ToString();
            }
            return stringValue;
        }

        public static bool GetBoolean(this object value)
        {
            bool bReturn = false;
            if (value != null && value != DBNull.Value)
            {
                bReturn = Convert.ToBoolean(value);
            }
            return bReturn;
        }

        public static bool GetBooleanFromInt(this object value)
        {
            bool bReturn = false;
            if (value != null && value != DBNull.Value)
            {
                bReturn = Convert.ToInt32(value) == 1 ? true : false;
            }
            return bReturn;
        }

        public static bool? GetNullableBoolean(this object value)
        {
            bool? bReturn = null;
            if (value != null && value != DBNull.Value)
            {
                bReturn = (bool)value;
            }

            return bReturn;
        }

        public static bool? GetNullableBooleanFromInt(this object value)
        {
            bool? bReturn = null;
            if (value != null && value != DBNull.Value)
            {
                bReturn = Convert.ToInt32(value) == 1 ? true : false;
            }

            return bReturn;
        }

        public static T GetEnumValue<T>(this object databaseValue) where T : struct
        {
            T enumValue = default(T);

            if (databaseValue != null && databaseValue.ToString().Length > 0)
            {
                object parsedValue = Enum.Parse(typeof(T), databaseValue.ToString());
                if (parsedValue != null)
                {
                    enumValue = (T)parsedValue;
                }
            }

            return enumValue;
        }

        public static byte[] GetByteArrayValue(this object value)
        {
            byte[] arrayValue = null;
            if (value != null && value != DBNull.Value)
            {
                arrayValue = (byte[])value;
            }
            return arrayValue;
        }

    }
}
