using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dpx.Views;
using Xamarin.Forms;

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

        Task NavigateToAsync(string pageKey);

        /// <summary>
        /// 带参导航到页面
        /// </summary>
        /// <param name="pageKey">页面键</param>
        /// <param name="parameter">参数</param>
        /// <returns></returns>
        Task NavigateToAsync(string pageKey, object parameter);
    }

    /// <summary>
        /// 类容导航常量
        /// </summary>
        public static class ContentNavigationContenstants
    {
        /// <summary>
        /// 诗词详情页
        /// </summary>
        public const string DetailPage = nameof(Views.DetailPage);

        /// <summary>
        /// 页面键-页面类型字典
        /// </summary>
        public static readonly Dictionary<string, Type> PageKeyTypeDictionary = new Dictionary<string, Type>
        {
            {DetailPage,typeof(DetailPage)}
        };

    }
    
}
