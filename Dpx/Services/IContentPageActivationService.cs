using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Dpx.Services
{
    /// <summary>
    /// 内容页激活服务接口
    /// </summary>
    public interface IContentPageActivationService
    {
        /// <summary>
        /// 激活内容页
        /// </summary>
        /// <param name="pageKey">内容页</param>
        /// <returns></returns>
        ContentPage Activate(string pageKey);
    }
}
