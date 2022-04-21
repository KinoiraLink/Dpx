using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dpx.Models;

namespace Dpx.Services
{
    /// <summary>
    /// 今日图片服务接口
    /// </summary>
    public interface ITodayImageService
    {
        Task<TodayImage> GetTodayImageAsync();
        Task<TodayImageServiceCheckUpdateResult> CheckAsync();
    }

    public class TodayImageServiceCheckUpdateResult
    {
        public bool HasUpdate { get; set; }

        public TodayImage TodayImage { get; set; }
    }

}
