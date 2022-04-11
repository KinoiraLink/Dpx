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
        /// <summary>
        /// 诗词数据库的接口
        /// </summary>
        private IPoetryStorage _poetryStorage;

        /// <summary>
        /// 内容服务接口
        /// </summary>
        private IContentNavigationService _contentNavigationService;
        //******** 构造函数
        /// <summary>
        ///诗词搜索页：数据库全显页
        /// </summary>
        /// <param name="poetryStorage">诗词存储，数据库</param>
        /// <param name="contentNavigationService">导航服务</param>
        public ResultPageViewModel(IPoetryStorage poetryStorage,IContentNavigationService contentNavigationService)
        {
            //tapped点击进行由搜索页（数据库全查询）到详情页（数据库单挑查询）导航 step1
            _contentNavigationService = contentNavigationService;
            //显示所有结果数据库全查询 step1
            _poetryStorage = poetryStorage;
            // *无限滚动插件，固有定义
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
        /// <summary>
        /// 无限滚动插件，固有定义
        /// </summary>
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
        /// 用于代码测试，可写于上述语法糖内，不可unit测试
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

        private RelayCommand<Poetry> _poetryTappedCommand;

        /// <summary>
        /// 诗词点击命令
        /// </summary>
        public RelayCommand<Poetry> PoetryTappedCommand =>
            _poetryTappedCommand ?? (_poetryTappedCommand = new RelayCommand<Poetry>(
                async p => await PoetryTappedCommandFunction(p)
                
                
                ));

        public async Task PoetryTappedCommandFunction(Poetry poetry) =>
            await _contentNavigationService.NavigateToAsync(ContentNavigationContenstants.DetailPage, poetry);
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
