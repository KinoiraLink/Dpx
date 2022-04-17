using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dpx.Services
{
    /// <summary>
    /// 同部服务接口
    /// </summary>
    public interface ISyncService : INotifyStatusChanged
    {
        /// <summary>
        /// 同步
        /// </summary>
        /// <returns></returns>
        Task SyncAsync();


    }
}
