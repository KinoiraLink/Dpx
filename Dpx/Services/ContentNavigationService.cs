using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dpx.Views;
using Xamarin.Forms;

namespace Dpx.Services
{
    public class ContentNavigationService : IContentNavigationService
    {
        //******** 私有变量
        /// <summary>
        /// MainPage的实例
        /// </summary>
        private MainPage _mainPage;
        public MainPage MainPage => _mainPage ?? (Application.Current.MainPage as MainPage);

        /// <summary>
        /// 内容页激活服务
        /// </summary>
        private IContentPageActivationService _contentPageActivationService;


        //******** 继承方法
        /// <summary>
        /// 导航到页面
        /// </summary>
        /// <param name="pageKey">页面键</param>
        public async Task NavigateToAsync(string pageKey)
        =>
            await MainPage.Detail.Navigation.PushAsync(
                _contentPageActivationService.Activate(pageKey));

        /// <summary>
        /// 带参导航到页面
        /// </summary>
        /// <param name="pageKey">页面键</param>
        /// <param name="parameter">参数</param>
        /// <returns></returns>
        public async Task NavigateToAsync(string pageKey, object parameter)
        {
            var page = _contentPageActivationService.Activate(pageKey);
            NavigationContext.SetParameter(page, parameter);
            await MainPage.Detail.Navigation.PushAsync(page);

        }


        //******** 公开方法
        /// <summary>
        /// 内容页导航服务
        /// </summary>
        /// <param name="contentPageActivationService">内容页激活服务</param>
        public ContentNavigationService(IContentPageActivationService contentPageActivationService)
        {
            _contentPageActivationService = contentPageActivationService;
        }
    }
}
