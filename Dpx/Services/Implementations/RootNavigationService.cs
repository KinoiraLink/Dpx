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
    public class RootNavigationService : IRootNavigationService
    {
        //******** 私有变量
        /// <summary>
        /// MainPage的实例
        /// </summary>
        private MainPage _mainPage;
        public MainPage MainPage => _mainPage ?? (Application.Current.MainPage as MainPage);

        /// <summary>
        /// 根页面激活服务
        /// </summary>
        private IRootPageActivationService _rootPageActivationService;

        //******** 构造函数
        /// <summary>
        /// 根导航服务
        /// </summary>
        /// <param name="rootPageActivationService">根页面激活服务</param>
        public RootNavigationService(IRootPageActivationService rootPageActivationService)
        {
            _rootPageActivationService = rootPageActivationService;
        }
        //******** 继承方法

        /// <summary>
        /// 导航到页面
        /// </summary>
        /// <param name="pageKey">页面键</param>
        public async Task NavigateToAsync(string pageKey)
        {
            await MainPage.Detail.Navigation.PopToRootAsync();
            MainPage.Detail = _rootPageActivationService.Activate(pageKey);
            MainPage.IsPresented = false;
        }

        /// <summary>
        /// 导航到页面
        /// </summary>
        /// <param name="pageKey">页面键</param>
        /// <param name="parameter">参数</param>
        /// <returns></returns>
        public async Task NavigateToAsync(string pageKey, object parameter)
        {
            await MainPage.Detail.Navigation.PopToRootAsync();  
            var page = _rootPageActivationService.Activate(pageKey);
            await page.Navigation.PopToRootAsync();
            NavigationContext.SetParameter(page.CurrentPage,parameter);
            MainPage.Detail = page;
            MainPage.IsPresented = false;
        }
    }
}
