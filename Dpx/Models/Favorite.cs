using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Dpx.Models
{
    /// <summary>
    /// 收藏类
    /// </summary>
    public class Favorite
    {
        /// <summary>
        /// 诗词的ID
        /// </summary>
        [PrimaryKey]
        public int PoetryId { get; set; }

        /// <summary>
        /// 是否收藏
        /// </summary>
        public bool IsFavorite { get; set; }
    }
}
