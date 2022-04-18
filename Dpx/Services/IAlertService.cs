using System;
using System.Collections.Generic;
using System.Text;

namespace Dpx.Services
{
    /// <summary>
    /// 警告服务
    /// </summary>
    public interface IAlertService
    {
        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="button">按钮文字</param>
        void DisplayAlert(string title, string content, string button);


    }
}
