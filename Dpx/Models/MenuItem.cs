using System;
using System.Collections.Generic;
using System.Text;
using Dpx.Services;

namespace Dpx.Models
{
    /// <summary>
    /// 菜单项
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// 标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 页面键。
        /// </summary>
        public string PageKey { get; set; }

        public static IList<MenuItem> ItemList { get; } = new List<MenuItem>
        {
            new MenuItem{Title = "诗词收藏",PageKey = RootNavigationConstants.FavoritePage},
            //Todo  供测试
            new MenuItem{Title = "搜索结果（测试）",PageKey =RootNavigationConstants.ResultPage},
            new MenuItem{Title = "数据同步",PageKey = RootNavigationConstants.SyncPage},
            new MenuItem{Title = "今日推荐",PageKey = RootNavigationConstants.TodayPage}
        };
    }
}
