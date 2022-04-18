using System;
using System.Collections.Generic;
using System.Text;
using Dpx.Views;
using Xamarin.Forms;

namespace Dpx.Services.Implementations
{
    /// <summary>
    /// 警告服务
    /// </summary>
    public class AlertService : IAlertService
    {
        //******** 私有变量
        /// <summary>
        /// MainPage的实例
        /// </summary>
        private MainPage _mainPage;
        public MainPage MainPage => _mainPage ?? (Application.Current.MainPage as MainPage);


        //******** 继承方法
        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="button">按钮文字</param>
        public void DisplayAlert(string title, string content, string button)
        {
            Device.BeginInvokeOnMainThread(()=> MainPage.DisplayAlert(title, content, button));
        }
    }
}
