using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DUIDemo.Helper
{
    public static class PublicFunction
    {
        public static string NumberDecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        public static int intParse(object value)
        {
            if (value == DBNull.Value) return 0;
            if (value == null) return 0;
            int viTemp = 0;
            Type ValueType = value.GetType();

            switch (Type.GetTypeCode(ValueType))
            {
                case TypeCode.Boolean:
                case TypeCode.Decimal:
                case TypeCode.Double:
                    viTemp = Convert.ToInt32(value);
                    break;
                default:
                    if (value.GetType().BaseType == typeof(Enum))
                    {
                        viTemp = (int)value;
                    }
                    else
                    {
                        int.TryParse(value.ToString(), out viTemp);
                    }
                    break;
            }
            return viTemp;
        }

        public static bool boolParse(object value)
        {
            if (value == DBNull.Value) return false;
            if (value == null) return false;

            if (value.ToString() == "1") return true;
            if (value.ToString() == "0") return false;

            bool vbTemp = false;
            bool.TryParse(value.ToString(), out vbTemp);

            return vbTemp;
        }

        public static DateTime dateTimeParse(object value)
        {
            if (value == DBNull.Value) return new DateTime();
            if (value == null) return new DateTime();

            DateTime viTemp = new DateTime();
            DateTime.TryParse(value.ToString(), out viTemp);

            return viTemp;

        }

        public static decimal decimalParse(string value)
        {
            if (string.IsNullOrEmpty(value)) return 0;

            //DecimalSeparator in db always be "."
            value = value.Replace(".", NumberDecimalSeparator);

            decimal viTemp = 0;
            decimal.TryParse(value, out viTemp);

            return viTemp;
        }

        public static decimal decimalParse(object value)
        {
            if (value == DBNull.Value) return 0;
            if (value == null) return 0;

            //DecimalSeparator in db always be "."
            value = value.ToString().Replace(".", NumberDecimalSeparator);

            decimal viTemp = 0;
            decimal.TryParse(value.ToString(), out viTemp);

            return viTemp;
        }
    }
}
