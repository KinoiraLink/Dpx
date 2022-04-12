using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dpx.Models;

namespace Dpx.Services
{
    /// <summary>
    /// 收藏存储接口
    /// </summary>
    public interface IFavoriteStorage
    {
        /// <summary>
        /// 初始化数据库  
        /// </summary>
        Task InitializeAsync();

        /// <summary>
        /// 判断数据库已经初始化
        /// </summary>
        /// <returns></returns>
        bool IsInitialized();



        /// <summary>
        /// 获取一首是否被收藏的信息
        /// </summary>
        /// <param name="poetryId">诗词的</param>
        /// <returns></returns>
        Task<Favorite> GetFavoriteAsync(int poetryId);



        /// <summary>
        /// 保存收藏信息
        /// </summary>
        /// <remarks>
        /// 收藏信息中已经隐含了诗词信息
        /// </remarks>
        /// <param name="favorite"></param>
        /// <returns></returns>
        Task SaveFavoriteAsync(Favorite favorite);

        /// <summary>
        /// 获取所有收藏信息
        /// </summary>
        /// <returns></returns>
        Task<IList<Favorite>> GetFavoritesAsync();

        Task DeleteFavoritesAsync(Favorite favorite);


    }


    public static class FavoriteStorageConstants
    {
        /// <summary>
        /// 收藏数据库版本号
        /// </summary>
        public const int Version = 1;

        /// <summary>
        /// 默认版本号
        /// </summary>
        public const int DefultVersion = 0;
        /// <summary>
        /// 收藏数据库版本号的键。
        /// </summary>
        public const string VersionKey = nameof(FavoriteStorageConstants) + "." + nameof(Version);
    }
}
