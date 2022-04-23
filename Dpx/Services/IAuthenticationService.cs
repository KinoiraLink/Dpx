using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dpx.Models;

namespace Dpx.Services
{
    /// <summary>
    /// 身份验证服务
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// 身份验证
        /// </summary>
        /// <returns></returns>
        Task<AuthenticationResult> AuthenticateAsync();

    }
}
