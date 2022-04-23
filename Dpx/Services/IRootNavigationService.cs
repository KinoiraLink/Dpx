using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dpx.Views;
using Xamarin.Forms;

namespace Dpx.Services
{
    /// <summary>
    /// 根导航服务
    /// </summary>
    public interface IRootNavigationService
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
    /// 根导航常量
    /// </summary>
    public static class RootNavigationConstants
    {
        /// <summary>
        /// 诗词收藏页。
        /// </summary>
        public const string FavoritePage = nameof(Views.FavoritePage);

        

        /// <summary>
        /// 初始化页
        /// </summary>
        public const string InitialzationPage = nameof(Views.InitialzationPage);

        /// <summary>
        /// 同步页
        /// </summary>
        public const string SyncPage = nameof(Views.SyncPage);


        /// <summary>
        /// 今日推荐页
        /// </summary>
        public const string TodayPage = nameof(Views.TodayPage);

        /// <summary>
        /// 诗词搜索页
        /// </summary>
        public const string SearchPage = nameof(Views.SearchPage);





        /// <summary>
        /// 页面键-页面类型字典
        /// </summary>
        public static readonly Dictionary<string, Type> PageKeyTypeDictionary = new Dictionary<string, Type>
        {
            {FavoritePage,typeof(Views.FavoritePage)},
            {InitialzationPage,typeof(Views.InitialzationPage)},
            {SyncPage,typeof(Views.SyncPage)},
            {TodayPage,typeof(TodayPage)},
            {SearchPage,typeof(SearchPage)}
        };
    }
}
