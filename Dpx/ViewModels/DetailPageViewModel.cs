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
    public class DetailPageViewModel : ViewModelBase
    {
        //******** 构造函数 step3
        /// <summary>
        /// 收藏存储
        /// </summary>
        private IFavoriteStorage _favoriteStorage;

        /// <summary>
        /// 诗词详情页ViewModel
        /// </summary>
        /// <param name="favoriteStorage">收藏存储</param>
        public DetailPageViewModel(IFavoriteStorage favoriteStorage)
        {
            _favoriteStorage = favoriteStorage;
        }



        //******** 绑定属性 step1
        /// <summary>
        /// 诗词
        /// </summary>
        private Poetry _poetry;

        
        public Poetry Poetry
        {
            get => _poetry;
            set
            {
                Set(nameof(Poetry), ref _poetry, value);
                _isNewPoetry = true;//更新诗词是否是新收藏
            }

        }

        /// <summary>
        /// 收藏
        /// </summary>
        private Favorite _favorite;

        public Favorite Favorite
        {
            get => _favorite;
            set => Set(nameof(Favorite), ref _favorite, value);
        }


        //******** 绑定命令 step2


        /// <summary>
        /// 显示命令
        /// </summary>
        private RelayCommand _pageAppearingCommand;

        public RelayCommand PageAppearingCommand =>
            _pageAppearingCommand ?? (_pageAppearingCommand = new RelayCommand(async () => await PageAppearingCommandFunction()));

        public async Task PageAppearingCommandFunction()
        {
            if (!_isNewPoetry)
            {
                return;
            }

            _isNewPoetry = false;

            var favorite = await _favoriteStorage.GetFavoriteAsync(Poetry.Id)??new Favorite{PoetryId = Poetry.Id};
            _isFavorite = favorite.IsFavorite;
            Favorite = favorite;

        }

        /// <summary>
        /// 收藏切换命令
        /// </summary>
        private RelayCommand _favoriteToggleCommand;

        public RelayCommand FavoriteToggledCommand =>
            _favoriteToggleCommand ?? (_favoriteToggleCommand = new RelayCommand(async () => await FavoriteToggledCommandFunction()));

        public async Task FavoriteToggledCommandFunction()
        {
            if(Favorite.IsFavorite == _isFavorite)return;
            _isFavorite = Favorite.IsFavorite;
            
            Favorite.Timestamp = DateTime.Now.Ticks;
            await _favoriteStorage.SaveFavoriteAsync(Favorite);

        }
        //=> await _favoriteStorage.SaveFavoriteAsync(Favorite);

        //******** 私有变量 step4

        /// <summary>
        /// 是否是新诗词
        /// </summary>
        private bool _isNewPoetry;

        /// <summary>
        /// 诗词从数据库中读出的原生收藏状态
        /// </summary>
        public bool _isFavorite;
    }
}
