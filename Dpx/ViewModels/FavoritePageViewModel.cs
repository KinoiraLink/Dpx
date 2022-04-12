using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dpx.Models;
using Dpx.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MvvmHelpers;

namespace Dpx.ViewModels
{
    public class FavoritePageViewModel : ViewModelBase
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
        /// 内容导航服务
        /// </summary>
        private IContentNavigationService _contentNavigationService;

        //******** 构造函数
        /// <summary>
        /// 诗词收藏ViewModel
        /// </summary>
        /// <param name="poetryStorage">诗词</param>
        /// <param name="favoriteStorage">收藏</param>
        /// <param name="contentNavigationService">导航</param>
        public FavoritePageViewModel(IPoetryStorage poetryStorage,
            IFavoriteStorage favoriteStorage,
            IContentNavigationService contentNavigationService)
        {
            _poetryStorage = poetryStorage;
            _favoriteStorage = favoriteStorage;
            _contentNavigationService = contentNavigationService;
        }

        //******** 绑定属性
        //private Poetry _poetry;

        //public Poetry Poetry
        //{
        //    get => _poetry;
        //    set => Set(nameof(Poetry), ref _poetry, value);
        //}


        public ObservableRangeCollection<Poetry> PoetryCollection { get; } = 
            new ObservableRangeCollection<Poetry>();

        //******** 绑定命令

        /// <summary>
        /// 页面显示命令
        /// </summary>
        private RelayCommand __pageAppearingCommand;

        public RelayCommand PageAppearingCommand =>
            __pageAppearingCommand ?? (__pageAppearingCommand = new RelayCommand(async () => await PageAppearingCommandFunction()));

        public async Task PageAppearingCommandFunction()
        {
            if (_isLoaded)
            {
                return;
            }
            _isLoaded = true;


            var favorites = await _favoriteStorage.GetFavoritesAsync();


            ////LinQ语言特性方法
            var poetryTaskList = favorites.Select(async p => await _poetryStorage.GetPoetryAsync(p.PoetryId)).ToList();

            var poetries = (await Task.WhenAll(poetryTaskList)).ToList();

            PoetryCollection.AddRange(poetries);
            ////传统方法*

            //var poetries = new List<Poetry>();

            //foreach (var favorite in favorites)
            //{
            //    poetries.Add(
            //        await _poetryStorage.GetPoetryAsync(favorite.PoetryId)
            //        );
            //}
            //PoetryCollection.AddRange(poetries);
        }


        /// <summary>
        /// 诗词点击命令
        /// </summary>
        private RelayCommand<Poetry> _poetryTappedCommand;


        public RelayCommand<Poetry> PoetryTappedCommand =>
            _poetryTappedCommand ?? (_poetryTappedCommand = new RelayCommand<Poetry>(
                async p => await PoetryTappedCommandFunction(p)


            ));

        public async Task PoetryTappedCommandFunction(Poetry poetry) =>
            await _contentNavigationService.NavigateToAsync(ContentNavigationContenstants.DetailPage, poetry);


        //******** 私有变量
        /// <summary>
        ///ye mian shi fou jia zai
        /// </summary>
        private bool _isLoaded;
    }
}
