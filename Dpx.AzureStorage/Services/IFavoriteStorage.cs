using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dpx.AzureStorage.Services
{
    /// <summary>
    /// 收藏存储
    /// </summary>
    public interface IFavoriteStorage
    {
        /// <summary>
        /// 保存收藏数据
        /// </summary>
        /// <param name="favoriteBytes">收藏数据</param>
        /// <param name="gitHubId">GitHub Id</param>
        /// <returns></returns>
        Task SaveAsync(byte[] favoriteBytes, int gitHubId);

        /// <summary>
        /// 读取收藏数据
        /// </summary>
        /// <param name="gitHubId">GitHub Id</param>
        /// <returns></returns>
        Task<byte[]> GetAsync(int gitHubId);
    }
}
