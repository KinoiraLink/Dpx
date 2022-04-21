using System;
using System.Collections.Generic;
using System.Text;

namespace Dpx.Models
{
    /// <summary>
    /// 今日图片
    /// </summary>
    public class TodayImage
    {
        /// <summary>
        /// 版权信息
        /// </summary>
        public string Copyright { get; set; }

        /// <summary>
        /// 版权连接
        /// </summary>
        public string CopyrightLink { get; set; }

        /// <summary>
        /// 图片二进制
        /// </summary>
        public byte[] ImageBytes { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// 完整的开始时间
        /// </summary>
        public string FullStartDate { get; set; }
    }


}
