using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DUIDemo.Helper
{
    public class EnumHelper
    {

        public class MEnumData
        {
            private string _Des;
            private string _Key;
            private int _Value;
            private string _Name;

            public string Des
            {
                get
                {
                    return _Des;
                }

                set
                {
                    _Des = value;
                }
            }

            public string Key
            {
                get
                {
                    return _Key;
                }

                set
                {
                    _Key = value;
                }
            }

            public int Value
            {
                get
                {
                    return _Value;
                }

                set
                {
                    _Value = value;
                }
            }

            public string Name
            {
                get
                {
                    return _Name;
                }

                set
                {
                    _Name = value;
                }
            }
        }

        /// <summary>
        /// 获取一个枚举值的中文描述
        /// </summary>
        /// <param name="obj">枚举值</param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum obj)
        {
            FieldInfo fi = obj.GetType().GetField(obj.ToString());
            DescriptionAttribute[] arrDesc = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return arrDesc[0].Description;
        }

        public static List<MEnumData> GetEnumList<T>()
        {
            List<MEnumData> list = new List<MEnumData>();
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                MEnumData data = new MEnumData();
                object[] objArr = item.GetType().GetField(item.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (objArr != null && objArr.Length > 0)
                {
                    DescriptionAttribute da = objArr[0] as DescriptionAttribute;
                    data.Des = da.Description;
                }
                object[] disArr = item.GetType().GetField(item.ToString()).GetCustomAttributes(typeof(DisplayNameAttribute), true);
                if (disArr != null && disArr.Length > 0)
                {
                    DisplayNameAttribute da = disArr[0] as DisplayNameAttribute;
                    data.Name = da.DisplayName;
                }

                data.Value = Convert.ToInt32(item);
                data.Key = item.ToString();

                list.Add(data);
            }
            return list;
        }

        //public static List<EnumberCreditType> EnumToList<T>()
        //{
        //    List<EnumberCreditType> list = new List<EnumberCreditType>();

        //    foreach (var e in Enum.GetValues(typeof(T)))
        //    {
        //        EnumberCreditType m = new EnumberCreditType();
        //        object[] objArr = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
        //        if (objArr != null && objArr.Length > 0)
        //        {
        //            DescriptionAttribute da = objArr[0] as DescriptionAttribute;
        //            m.Desction = da.Description;
        //        }
        //        //SetClassification 
        //        object[] setClassificationArr = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(SetClassificationAttribute), true);
        //        if (setClassificationArr != null && setClassificationArr.Length > 0)
        //        {
        //            SetClassificationAttribute da = setClassificationArr[0] as SetClassificationAttribute;
        //            m.Classification = da.Type;
        //        }
        //        //Display
        //        object[] disArr = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DisplayNameAttribute), true);
        //        if (disArr != null && disArr.Length > 0)
        //        {
        //            DisplayNameAttribute da = disArr[0] as DisplayNameAttribute;
        //            m.Name = da.DisplayName;
        //        }

        //        m.Value = Convert.ToInt32(e);
        //        m.Key = e.ToString();
        //        list.Add(m);
        //    }
        //    return list;
        //}
    }
}
