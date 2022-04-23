using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Dpx.Models;
using Dpx.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Dpx.ViewModels
{
    /// <summary>
    /// 诗词搜索页ViewModel
    /// </summary>
    public class SearchPageViewModel : ViewModelBase
    {
        //******** 私有变量

        /// <summary>
        /// 内容页导航服务
        /// </summary>
        private IContentNavigationService _contentNavigationService;

        //******** 构造函数
        public SearchPageViewModel(IContentNavigationService contentNavigationService) 
        {
            _contentNavigationService = contentNavigationService;
            FilterViewModelCollection.Add(new FilterViewModel(this));
        }
        //******** 绑定属性


        /// <summary>
        /// 搜索条件集合
        /// </summary>
        public ObservableCollection<FilterViewModel> FilterViewModelCollection { get; } = 
            new ObservableCollection<FilterViewModel>();

        //******** 公开方法
        /// <summary>
        /// 在给定的搜索田间后面添加搜索条件
        /// </summary>
        /// <param name="filterViewModel">搜索条件</param>
        public void AddFilterViewModel(FilterViewModel filterViewModel)
        {
            FilterViewModelCollection.Insert(
                FilterViewModelCollection.IndexOf(filterViewModel)+1,
                new FilterViewModel(this));
            
        }
        /// <summary>
        /// 删除搜索条件
        /// </summary>
        /// <param name="filterViewModel">搜索条件</param>
        public void RemoveFilterViewModel(FilterViewModel filterViewModel)
        {
            FilterViewModelCollection.Remove(filterViewModel);
            if (!FilterViewModelCollection.Any())
            {
                FilterViewModelCollection.Add(new FilterViewModel(this));
            }
        }


        //******** 绑定命令
        /// <summary>
        /// 搜索命令
        /// </summary>
        private RelayCommand _queryCommand;

        public RelayCommand QueryCommand =>
            _queryCommand ?? (_queryCommand = new RelayCommand(async () => await QueryCommandFunction()));

        public async Task QueryCommandFunction()
        {
            var parameter = Expression.Parameter(typeof(Poetry), "p");
            var aggregateExpression = FilterViewModelCollection
                //FilterViewModels with not null content,
                .Where(p => !string.IsNullOrWhiteSpace(p.Content))
                //Translate FilterViewModel into Expression
                //e.g. p.AuthorName.Contains("something")
                .Select(p => GetExpression(p, parameter))
                //true && condition && condition && condition
                .Aggregate(Expression.Constant(true) as Expression, Expression.AndAlso);

            var where = Expression.Lambda<Func<Poetry, bool>>(aggregateExpression, parameter);
            await _contentNavigationService.NavigateToAsync(ContentNavigationConstants.ResultPage, where);
        }

        private static Expression GetExpression(FilterViewModel filterViewModel, 
            ParameterExpression parameter)
        {
            //p.Name p.Content p.AuthorName
            var property = Expression.Property(parameter, 
                filterViewModel.FilterType.PropertyName);

            //p.Contains("")
            var method = typeof(string).GetMethod("Contains", new[] {typeof(string)});

            //"something"
            var condition = Expression.Constant(filterViewModel.Content, typeof(string));

            //p.Name.Contains.("something")
            return Expression.Call(property, method, condition);
        }
    }
}
