using System;
using System.Collections.Generic;
using System.Text;

namespace Dpx.AzureStorage.Models
{
    /// <summary>
    /// 身份验证结果——服务端
    /// </summary>
    public class AuthenticationResult
    {
        /// <summary>
        /// GitHub用户类
        /// </summary>
        public int GitHubUserId { get; set; }

        /// <summary>
        /// 是否通过验证
        /// </summary>
        public bool Passed { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
    }
}
