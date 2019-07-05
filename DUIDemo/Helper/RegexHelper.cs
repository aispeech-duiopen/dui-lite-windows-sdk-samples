using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DUIDemo.Helper
{
    public static class RegexHelper
    {
        /// <summary>
        /// 
        /// 验证多个email加逗号分隔
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsEmailTextValid(string text)
        {
            string regEmail = @"([\w - \.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)";
            string regEmailbracket = @"\<" + regEmail + "\\>";
            string regEmailWithName = @"[\w - \.]+" + regEmailbracket;
            string regEmailNormal = "(" + regEmail + "|(" + regEmailWithName + "))";

            string reg = "^(" + regEmailNormal + @"+\,)*" + regEmailNormal + "+$";
            Regex regex = new Regex(reg);
            return regex.IsMatch(text);
        }

        /// <summary>
        /// check email
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsEmailValid(string text)
        {
            string reg = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex regex = new Regex(reg);
            return regex.IsMatch(text);
        }

        /// <summary>
        /// 检测通信号码
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsSMSNumValid(string text)
        {
            string reg = @"^(\+?\d+\,)*\+?\d+$";
            Regex regex = new Regex(reg);
            return regex.IsMatch(text);
        }

        /// <summary>
        /// 判断路径是否合法
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsPathLegal(string filePath)
        {
            var pattern = string.Join("", System.IO.DriveInfo.GetDrives().Select(t => t.Name.First()).ToList());
            //pattern = string.Format(@"(?i)^[{0}]:(((\\[^ /:*?<>\""|\\]+)+\\?)|(\\)?)\s*$", pattern);

            string legalCharWithoutSpace = string.Format(@"[^ /:*?<>\""|\\]");
            string legalChar = string.Format(@"[^/:*?<>\""|\\]");
            string ClientPathCheck = string.Format(@"(?i)^[{0}]:(((\\{1}*{2}{1}*)+\\?)|(\\)?)\s*$", pattern, legalChar, legalCharWithoutSpace);
            string RemotePathCheck = string.Format(@"^\\(\\{0}*{1}{0}*)+\\?$", legalChar, legalCharWithoutSpace);
            string PathCheck = string.Format(@"({0})|({1})", ClientPathCheck, RemotePathCheck);
            //pattern = string.Format(@"(?i)^[{0}]:((((\\[^ /:*?<>\""|\\][^/:*?<>\""|\\]*[^ /:*?<>\""|\\])|(\\[^ /:*?<>\""|\\]))+\\?)|(\\)?)\s*$", pattern);
            Regex myRegex = new Regex(PathCheck);
            if (!myRegex.IsMatch(filePath))
            {
                return false;

            }
            return true;
        }

        /// <summary>
        /// check text is Positive int 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsTextAllowedPositiveInt(string text)
        {
            Regex regex = new Regex("[^0-9]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        /// <summary>
        /// check is only number and char 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsOnlyNumAndChar(string text)
        {
            Regex regex = new Regex(@"^[a-zA-Z0-9]*$");
            return regex.IsMatch(text);
        }
    }
}
