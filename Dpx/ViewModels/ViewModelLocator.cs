using System;
using System.Collections.Generic;
using System.Text;
using Dpx.Services;
using GalaSoft.MvvmLight.Ioc;

namespace Dpx.ViewModels
{
    /// <summary>
    /// ViewModel Localtor
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// 搜索结果页注入实例
        /// </summary>
        public ResultPageViewModel ResultPageViewModel => 
            SimpleIoc.Default.GetInstance<ResultPageViewModel>();

        /// <summary>
        /// 注册service,viewmodel;
        /// </summary>
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<IPreferenceStorage,PreferenceStoragecs>();
            SimpleIoc.Default.Register<IPoetryStorage,PoetryStorage>();
            SimpleIoc.Default.Register<ResultPageViewModel>();
        }
    }
}
