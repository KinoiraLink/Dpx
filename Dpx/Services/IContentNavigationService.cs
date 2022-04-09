using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dpx.Services
{
    /// <summary>
    /// 内容导航服务接口
    /// </summary>
    public interface IContentNavigationService
    {
        /// <summary>
        /// 导航到页面
        /// </summary>
        /// <param name="pageKey">页面键</param>
        /// <returns></returns>
        Task NavigateToAsync(string pageKey);

    }
}
