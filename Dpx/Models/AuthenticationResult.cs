using System;
using System.Collections.Generic;
using System.Text;

namespace Dpx.Models
{
    /// <summary>
    /// 省份验证信息
    /// </summary>
    public class AuthenticationResult
    {
        /// <summary>
        /// 访问token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 访问token到期时间
        /// </summary>
        public DateTime AccessTokenExpiration { get; set; }
        /// <summary>
        /// 是否存在错误信息
        /// </summary>
        public bool IsError { get; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error { get; }

        public AuthenticationResult()
        {

        }


        /// <summary>
        /// 身份验证结果
        /// </summary>
        /// <param name="isError">是否存在错误</param>
        /// <param name="error">错误信息</param>
        public AuthenticationResult(bool isError, string error)
        {
            
            IsError = isError;
            Error = error;
        }
    }
}
