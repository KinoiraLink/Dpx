using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dpx.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Dpx.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        //******** 私有变量

        /// <summary>
        /// 诗词存储
        /// </summary>
        private IPoetryStorage _poetryStorage;
        /// <summary>
        /// 收藏存储
        /// </summary>
        private IFavoriteStorage _favoriteStorage;

        /// <summary>
        /// 根导航服务
        /// </summary>
        private IRootNavigationService _rootNavigationService;

        //******** 构造函数
        public MainPageViewModel(
            IPoetryStorage poetryStorage,
            IFavoriteStorage favoriteStorage,
            IRootNavigationService rootNavigationService)
        {
            _poetryStorage = poetryStorage;
            _favoriteStorage = favoriteStorage;
            _rootNavigationService = rootNavigationService;
        }

        //******** 绑定命令

        /// <summary>
        /// 页面显示命令
        /// </summary>
        private RelayCommand _paheAppearingCommand;

        public RelayCommand PageApperingCommand =>
            _paheAppearingCommand ?? (_paheAppearingCommand = new RelayCommand(async () => await PageApperingCommandFunction()));

        public async Task PageApperingCommandFunction()
        {
            if (!_poetryStorage.IsInitialized()|| !_favoriteStorage.IsInitialized())
            {
                await _rootNavigationService.NavigateToAsync(RootNavigationConstants.InitialzationPage);
            }

        }
    }
}
