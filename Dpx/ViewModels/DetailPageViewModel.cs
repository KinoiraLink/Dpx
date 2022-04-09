using System;
using System.Collections.Generic;
using System.Text;
using Dpx.Models;
using GalaSoft.MvvmLight;

namespace Dpx.ViewModels
{
    public class DetailPageViewModel : ViewModelBase
    {
        //******** 绑定属性

        private Poetry _poetry;

        /// <summary>
        /// 诗词
        /// </summary>
        public Poetry Poetry
        {
            get => _poetry;
            set => Set(nameof(Poetry), ref _poetry, value);
        }
    }
}
