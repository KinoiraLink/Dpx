using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dpx.Models;

namespace Dpx.Services
{
    /// <summary>
    /// 远程收藏存储。
    /// </summary>
    public interface IRemoteFavoriteStorage : INotifyStatusChanged
    {
        /// <summary>
        /// 获取所有收藏项，包括收藏与非收藏。
        /// </summary>
        /// <returns></returns>
        Task<IList<Favorite>> GetFavoriteItemAsync();

        /// <summary>
        /// 保存所有收藏项，包括收藏与非收藏。
        /// </summary>
        /// <param name="favoriteList"></param>
        /// <returns></returns>
        Task SaveFavoriteItemsAsync(IList<Favorite> favoriteList);


        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        Task<bool> SignInAsync();

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        Task SignOutAsync();

        /// <summary>
        /// 是否已登录
        /// </summary>
        /// <returns></returns>
        Task<bool> IsSignedInAsync();
    }
}
