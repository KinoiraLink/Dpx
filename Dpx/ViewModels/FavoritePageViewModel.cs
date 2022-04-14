using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            favoriteStorage.UpdateMode += FavoriteStorage_UpdateMode;
        }

        private async void FavoriteStorage_UpdateMode(object sender, FavoriteStorageUpdateEventArgs e)
        {
            if (e.UpdateFavorite.IsFavorite)
            {
                var poetry = await _poetryStorage.GetPoetryAsync(e.UpdateFavorite.PoetryId);
                PoetryCollection.Add(poetry);
            }
            else
            {
                PoetryCollection.Remove(PoetryCollection.FirstOrDefault(p => p.Id == e.UpdateFavorite.PoetryId));
            }
            //var eUpdateFavorite = e.UpdateFavorite;

            //if (!eUpdateFavorite.IsFavorite)
            //{
            //    PoetryCollection.Remove(PoetryCollection.FirstOrDefault(p => p.Id == eUpdateFavorite.PoetryId));
            //}
            //else
            //{
            //    var poetry = await _poetryStorage.GetPoetryAsync(eUpdateFavorite.PoetryId);
            //    PoetryCollection.Add(poetry);
            //}

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
            var canRun = false;

            //二次加锁
            if (!_isLoaded)//A,B
            {
                lock (_isLoadedLock)//(A in,B blocked) or (A blocked,B in)
                {
                    if (!_isLoaded)
                    {
                        canRun = true;
                        _isLoaded = true;
                    }
                }
            }

            if (!canRun)
            {
                return;
            }

            

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

        public async Task PoetryTappedCommandFunction(Poetry poetry)
        {

            
            await _contentNavigationService.NavigateToAsync(ContentNavigationContenstants.DetailPage, poetry);
            //PoetryCollection.Remove(poetry);此为事例用，绝对不对
        }
        //=>await _contentNavigationService.NavigateToAsync(ContentNavigationContenstants.DetailPage, poetry);


        //******** 私有变量
        /// <summary>
        ///页面是否已加载
        /// </summary>
        private volatile bool _isLoaded;//volatile禁用Cpu编译器的优化


        /// <summary>
        /// 页面是否已加载锁
        /// </summary>
        private readonly object _isLoadedLock = new object();
    }
}
