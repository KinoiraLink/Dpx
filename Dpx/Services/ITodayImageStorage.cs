using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dpx.Models;

namespace Dpx.Services
{
    /// <summary>
    /// 今日图片存储接口
    /// </summary>
    public interface ITodayImageStorage
    {
        /// <summary>
        /// 存图
        /// </summary>
        /// <param name="image">今日图片</param>
        /// <returns></returns>
        Task SaveAsync(TodayImage image);


        /// <summary>
        /// b保存成今日图片的过期时间
        /// </summary>
        /// <param name="todayImage">今日图片</param>
        /// <returns></returns>
        Task SaveExpiresAsAsync(TodayImage todayImage);

        /// <summary>
        /// 读图
        /// </summary>
        /// <param name="includesImageByte">是否包含图片数组</param>
        /// <returns></returns>
        Task<TodayImage> GetAsync(bool includesImageByte);
    }
}
