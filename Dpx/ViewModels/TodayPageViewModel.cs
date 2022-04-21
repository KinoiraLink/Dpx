using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dpx.Models;
using Dpx.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Dpx.ViewModels
{
    /// <summary>
    /// 今日推荐ViewModel
    /// </summary>
    public class TodayPageViewModel : ViewModelBase
    {
        //******** 绑定属性
        /// <summary>
        /// 今日图片
        /// </summary>
        private TodayImage _todayImage;
        public TodayImage TodayImage
        {
            get => _todayImage;
            set => Set(nameof(TodayImage), ref _todayImage, value);
        }

        /// <summary>
        /// 今日图片
        /// </summary>
        private TodayPoetry _todayPoetry;

        public TodayPoetry TodayPoetry
        {
            get => _todayPoetry;
            set => Set(nameof(TodayPoetry), ref _todayPoetry, value);
        }

        /// <summary>
        /// 已加载
        /// </summary>
        private bool _todayPoetryLoaded;

        public bool TodayPoetryLoaded
        {
            get => _todayPoetryLoaded;
            set => Set(nameof(TodayPoetryLoaded), ref _todayPoetryLoaded, value);
        }


        /// <summary>
        /// 正在加载
        /// </summary>
        private bool _todayPoetryLoading;

        public bool TodayPoetryLoading
        {
            get => _todayPoetryLoading;
            set => Set(nameof(TodayPoetryLoading), ref _todayPoetryLoading, value);
        }

        //******** 绑定命令
        /// <summary>
        /// 页面显示
        /// </summary>
        private RelayCommand _pageAppearingCommand;

        public RelayCommand PageAppearingCommand =>
            _pageAppearingCommand ?? (_pageAppearingCommand = new RelayCommand(PageAppearingCommandFunction));

        internal void PageAppearingCommandFunction()
        {
            Task.Run(async () =>
            {
                TodayPoetryLoaded = false;
                TodayPoetryLoading = true;
                TodayPoetry = await _todayPoetryService.GetTodayPoetryAsync();
                TodayPoetryLoaded = true;
                TodayPoetryLoading = false;
            });

            Task.Run(async () =>
            {
                TodayImage = await _todayImageService.GetTodayImageAsync();

                var checkUpdateResult = await _todayImageService.CheckAsync();
                if (checkUpdateResult.HasUpdate)
                {
                    TodayImage = checkUpdateResult.TodayImage;
                }
            });
            
        }



        /// <summary>
        /// 查看详细命令
        /// </summary>
        private RelayCommand _showDetail;

        public RelayCommand ShowDetailCommand =>
            _showDetail ?? (_showDetail = new RelayCommand(async () => await ShowDetailCommandFunction()));

        public async Task ShowDetailCommandFunction()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 今日诗词命令
        /// </summary>
        private RelayCommand _jinrishiciCommand;

        public RelayCommand JirishiciCommand =>
            _jinrishiciCommand ?? (_jinrishiciCommand = new RelayCommand(async () => await JirishiciCommandFunction()));

        public async Task JirishiciCommandFunction()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 版权信息命令
        /// </summary>
        private RelayCommand _copyrightCommand;

        public RelayCommand CopyrightCommand =>
            _copyrightCommand ?? (_copyrightCommand = new RelayCommand(async () => await CopyrightCommandFunction()));

        public async Task CopyrightCommandFunction()
        {
            throw new NotImplementedException();
        }

        //******** 私有变量

        /// <summary>
        /// 页面已加载
        /// </summary>
        private volatile bool pageLoaded;

        private readonly object pageLoadedLock = new object();


        //******** 构造函数

        /// <summary>
        /// 今日图片服务
        /// </summary>
        private ITodayImageService _todayImageService;
        /// <summary>
        /// 今日诗词服务
        /// </summary>
        private ITodayPoetryService _todayPoetryService;

        /// <summary>
        /// 内容导航服务
        /// </summary>
        private IContentNavigationService _contentNavigationService;

        private IRootNavigationService _rootNavigationService;

        /// <summary>
        /// 今日诗词
        /// </summary>
        /// <param name="todayImageService">今日图片服务</param>
        /// <param name="todayPoetryService">今日诗词服务</param>
        /// <param name="contentNavigationService">内容页导航服务</param>
        /// <param name="rootNavigationService">根导航服务</param>
        public TodayPageViewModel(
            ITodayImageService todayImageService,
            ITodayPoetryService todayPoetryService,
            IContentNavigationService contentNavigationService,
            IRootNavigationService rootNavigationService)
        {
            _todayImageService = todayImageService;
            _todayPoetryService = todayPoetryService;
            _contentNavigationService = contentNavigationService;
            _rootNavigationService = rootNavigationService;
        }

    }
}
