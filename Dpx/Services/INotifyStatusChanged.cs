using System;
using System.Collections.Generic;
using System.Text;

namespace Dpx.Services
{
    /// <summary>
    /// 状态改变通知接口
    /// </summary>
    public interface INotifyStatusChanged
    {
        /// <summary>
        /// 状态
        /// </summary>
        string Status { get; }

        /// <summary>
        /// 状态改变事件
        /// </summary>
        event EventHandler StatusChanged;
    }
}
