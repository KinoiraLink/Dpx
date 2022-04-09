using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dpx.Models;
using Dpx.Services;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms.Extended;

namespace Dpx.ViewModels
{
    public class ResultPageViewModel : ViewModelBase
    {
        //******** 构造函数
        private IPoetryStorage _poetryStorage;
        /// <summary>
        /// 诗词存储
        /// </summary>
        /// <param name="poetryStorage"></param>
        public ResultPageViewModel(IPoetryStorage poetryStorage)
        {
            _poetryStorage = poetryStorage;

            PoetryCollection = new InfiniteScrollCollection<Poetry>
            {
                OnCanLoadMore = () =>_canLoadMore,
                OnLoadMore = async () =>
                {
                    Status = Loading;
                    var poetries = await poetryStorage.GetPoetriesAsync(Where,PoetryCollection.Count,PageSize);
                    Status = "";
                    if (poetries.Count < PageSize)
                    {
                        _canLoadMore = false;
                        Status = NoMoreResult;
                    }

                    if (poetries.Count == 0 && PoetryCollection.Count == 0)
                    {
                        Status = NoResult;
                    }
                    return poetries;
                }
            };
        }

        //******** 绑定属性
        public InfiniteScrollCollection<Poetry> PoetryCollection { get; }

        /// <summary>
        /// 加载状态
        /// </summary>
        private string _status;

        public string Status
        {
            get => _status;
            set => Set(nameof(Status), ref _status, value);
        }

        /// <summary>
        /// where条件
        /// </summary>
        private Expression<Func<Poetry,bool>> _where;

        public Expression<Func<Poetry,bool>> Where
        {
            get => _where;
            set
            {
                Set(nameof(Where), ref _where, value);
                
                _isNewQuery = true;
            }
        }


        //******** 绑定命令
        /// <summary>
        /// 页面显示命令
        /// </summary>
        private RelayCommand _pageAppearingCommand;

        public RelayCommand PageAppearingCommand =>
            _pageAppearingCommand ?? new RelayCommand(async () =>
            {
                await PageAppearingCommandFunction();//方便代码测试

            });


        /// <summary>
        /// 用于代码测试
        /// </summary>
        /// <returns></returns>
        public async Task PageAppearingCommandFunction()
        {
            Where = Expression.Lambda<Func<Poetry, bool>>(Expression.Constant(true),
                Expression.Parameter(typeof(Poetry), "p"));

            await _poetryStorage.InitializeAsync();

            if (!_isNewQuery) return;

            _isNewQuery = false;

            PoetryCollection.Clear();
            _canLoadMore = true;
            await PoetryCollection.LoadMoreAsync();
        }
        //******** 公有变量

        /// <summary>
        /// 一组诗词显示数量
        /// </summary>
        public const int PageSize = 20; 


        /// <summary>
        ///无限滚栏状态
        /// </summary>
        public const string Loading = "正在载入";
        public const string NoResult = "没有满足条件的结果";
        public const string NoMoreResult = "没有更多结果";
        
        //******** 私有变量
        /// <summary>
        /// 能否加载更多
        /// </summary>
        private bool _canLoadMore;

        /// <summary>
        /// 是否为新查询
        /// </summary>
        private bool _isNewQuery;

    }
}
