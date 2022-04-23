using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dpx.AzureStorage.Services
{
    /// <summary>
    /// 服务器授权服务
    /// </summary>
    public interface IAuthorizationService
    {
        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="gitHubid">Github ID</param>
        /// <returns></returns>
        Task<bool> AuthorizeAsync(int gitHubid);


    }
}
