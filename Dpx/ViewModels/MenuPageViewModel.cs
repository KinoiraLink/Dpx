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
    /// 菜单ViewModel。
    /// </summary>
    public class MenuPageViewModel : ViewModelBase
    {
      


        //******** 私有变量
        /// <summary>
        /// 根导航服务
        /// </summary>
        private IRootNavigationService _rootNavigationService;

        //******** 构造函数


        /// <summary>
        /// 菜单页ViewModel
        /// </summary>
        /// <param name="rootNavigationService">根导航服务</param>
        public MenuPageViewModel(IRootNavigationService rootNavigationService)
        {
            _rootNavigationService = rootNavigationService;
        }

        //******** 绑定命令

        /// <summary>
        /// 菜单项点击命令
        /// </summary>
        private RelayCommand<MenuItem> _menuItemTappedCommand;

        public RelayCommand<MenuItem> MenuItemTappedCommand =>
            _menuItemTappedCommand ?? (_menuItemTappedCommand = 
                new RelayCommand<MenuItem>(async menuItem => await MenuItemTappedCommandFunction(menuItem)));

        public async Task MenuItemTappedCommandFunction(MenuItem menuItem)
            => await _rootNavigationService.NavigateToAsync(menuItem.PageKey);

    }
}
