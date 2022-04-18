using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dpx.Models;

namespace Dpx.Services
{
    /// <summary>
    /// 今日诗词服务
    /// </summary>
    public interface ITodayPoetryService
    {

        /// <summary>
        /// 获取今日诗词
        /// </summary>
        /// <returns></returns>
        Task<TodayPoetry> GetTodayPoetryAsync();

    }

    
}
