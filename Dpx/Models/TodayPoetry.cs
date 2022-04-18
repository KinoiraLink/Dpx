using System;
using System.Collections.Generic;
using System.Text;

namespace Dpx.Models
{
    /// <summary>
    /// 今日诗词类
    /// </summary>
    public class TodayPoetry
    {
        public string Name { get; set; }
        public string AuthorName { get; set; }
        public string Dynasty { get; set; }
        public string Content { get; set; }

        /// <summary>
        /// 预览
        /// </summary>
        public string Snippet { get; set; }
        public string Source { get; set; }


        /// <summary>
        /// 今日诗词
        /// </summary>
        public const string Jinrishici = nameof(Jinrishici);

        /// <summary>
        /// 本地
        /// </summary>
        public const string Local = nameof(Local);
    }
}
