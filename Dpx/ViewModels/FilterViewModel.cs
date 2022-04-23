using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dpx.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Dpx.ViewModels
{
    /// <summary>
    /// 搜索条件ViewModel
    /// </summary>
    public class FilterViewModel : ViewModelBase
    {
        //******** 私有变量
        /// <summary>
        /// 诗词搜索页
        /// </summary>
        private SearchPageViewModel _searchPageViewModel;

        //******** 构造函数
        /// <summary>
        /// 诗词搜索页
        /// </summary>
        /// <param name="searchPageViewModel">诗词搜索页ViewModel</param>
        public FilterViewModel(SearchPageViewModel searchPageViewModel)
        {
            _searchPageViewModel = searchPageViewModel;
        }

        //******** 绑定属性
        /// <summary>
        /// 条件内容
        /// </summary>
        private string _content;

        public string Content
        {
            get => _content;
            set => Set(nameof(Content), ref _content, value);
        }


        /// <summary>
        /// 条件类型
        /// </summary>
        private FilterType _filterType = Models.FilterType.NameFilter;

        public FilterType FilterType
        {
            get => _filterType;
            set => Set(nameof(FilterType), ref _filterType, value);
        }

        //******** 绑定命令
        /// <summary>
        /// 添加命令
        /// </summary>
        private RelayCommand _addCommand;

        public RelayCommand AddCommand =>
            _addCommand ?? (_addCommand = new RelayCommand(() => AddCommandFunction()));

        public void AddCommandFunction()
            => _searchPageViewModel.AddFilterViewModel(this);

        /// <summary>
        /// 删除命令
        /// </summary>
        private RelayCommand _removeCommand;

        public RelayCommand RemoveCommand =>
            _removeCommand ?? (_removeCommand = new RelayCommand(() => RemoveCommandFunction()));

        public void RemoveCommandFunction()
            => _searchPageViewModel.RemoveFilterViewModel(this);



    }
}
