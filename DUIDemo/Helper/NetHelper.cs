using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace DUIDemo.Helper
{
    public static class NetHelper
    {
        private const int INTERNET_CONNECTION_MODEM = 1;
        private const int INTERNET_CONNECTION_LAN = 2;

        [DllImport("winInet.dll")]
        private static extern bool InternetGetConnectedState(ref int dwFlag, int dwReserved);

        /// <summary>
        /// 判断本机是否联网
        /// </summary>
        /// <returns></returns>
        public static bool IsConnectNetwork()
        {
            try
            {
                int dwFlag = 0;
                //false表示没有连接到任何网络,true表示已连接到网络上
                if (!InternetGetConnectedState(ref dwFlag, 0))
                {
                    return false;
                }
                //判断当前网络是否可用
                IPAddress[] addresslist = Dns.GetHostAddresses("www.baidu.com");
                if (addresslist[0].ToString().Length <= 6)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
