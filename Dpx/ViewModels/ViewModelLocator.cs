using System;
using System.Collections.Generic;
using System.Text;
using Dpx.Services;
using GalaSoft.MvvmLight.Ioc;

namespace Dpx.ViewModels
{
    /// <summary>
    /// ViewModel Localtor
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// 搜索结果页注入实例
        /// </summary>
        public ResultPageViewModel ResultPageViewModel => 
            SimpleIoc.Default.GetInstance<ResultPageViewModel>();

        /// <summary>
        /// 详情页面注入实例
        /// </summary>
        public DetailPageViewModel DetailPageViewModel => SimpleIoc.Default.GetInstance<DetailPageViewModel>();

        /// <summary>
        /// 收藏页面注入实例
        /// </summary>
        public FavoritePageViewModel FavoritePageViewModel => SimpleIoc.Default.GetInstance<FavoritePageViewModel>();


        public TestViewModel TestViewModel => SimpleIoc.Default.GetInstance<TestViewModel>();



        

        /// <summary>
        /// 收藏页面注入实例
        /// </summary>
        public MenuPageViewModel MenuPageViewModel => SimpleIoc.Default.GetInstance<MenuPageViewModel>();


        /// <summary>
        /// 初始化ViewModel
        /// </summary>
        public InitialzationPageViewModel InitialzationPageViewModel =>
            SimpleIoc.Default.GetInstance<InitialzationPageViewModel>();

        public MainPageViewModel MainPageViewModel => SimpleIoc.Default.GetInstance<MainPageViewModel>();


        /// <summary>
        /// 注册service,viewmodel;
        /// </summary>
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<IPreferenceStorage,PreferenceStoragecs>();
            SimpleIoc.Default.Register<IPoetryStorage,PoetryStorage>();
            SimpleIoc.Default.Register<ResultPageViewModel>();
            SimpleIoc.Default.Register<DetailPageViewModel>();
            SimpleIoc.Default.Register<FavoritePageViewModel>();

            SimpleIoc.Default.Register<TestViewModel>();

            SimpleIoc.Default.Register<IContentNavigationService,ContentNavigationService>();
            SimpleIoc.Default.Register<IContentPageActivationService,ContentPageActivationService>();
            SimpleIoc.Default.Register<IFavoriteStorage,FavoriteStorage>();
            SimpleIoc.Default.Register<IRootNavigationService,RootNavigationService>();
            SimpleIoc.Default.Register<IRootPageActivationService,RootPageActivationService>();
            SimpleIoc.Default.Register<MenuPageViewModel>();
            SimpleIoc.Default.Register<InitialzationPageViewModel>();
            SimpleIoc.Default.Register<MainPageViewModel>();
        }
    }
}
