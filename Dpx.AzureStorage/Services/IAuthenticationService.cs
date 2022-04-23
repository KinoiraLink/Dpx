using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dpx.AzureStorage.Models;

namespace Dpx.AzureStorage.Services
{
    /// <summary>
    /// 身份验证服务——服务端
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// 验证身份
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<AuthenticationResult> AuthenticateAsync(string token);
    }
}
