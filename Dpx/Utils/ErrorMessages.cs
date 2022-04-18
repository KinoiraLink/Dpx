using System;
using System.Collections.Generic;
using System.Text;

namespace Dpx.Utils
{
    public static class ErrorMessages
    {
        /// <summary>
        /// HTTP错误标题
        /// </summary>
        public const string HTTP_CLIENT_ERROR_TITLE = "连接错误";

        public static string HttpClientErrorMessage(string server, string message) =>
            string.Format($"与{server}连接时发生了错误：\n{message}");


        /// <summary>
        /// 错误按钮
        /// </summary>
        public const string HTTP_CLIENT_BUTTON = "确定";
    }
}
