using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUIDemo.Helper
{
    public class JsonHelper
    {
        public static string ToJson<T>(T data)
        {
            return JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// 反序列化Json对象，转化成需要的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T Parse<T>(string jsonString)
        {
            try
            {
                T objs = JsonConvert.DeserializeObject<T>(jsonString);
                return objs;
            }
            catch
            {
                return default(T);
            }
        }
    }
}
