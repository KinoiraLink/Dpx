using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Dpx.Services;
using Moq;

namespace DpxUnitTest.Helpers
{
    /// <summary>
    /// 收藏存储帮助类
    /// </summary>
    public class FavoriteStorageHelper
    {
        //获得已初始化的收藏存储；
        public static async Task<FavoriteStorage> GetInitilizedFavoriteStorageAsync()
        {
            var favoriteStorage = new FavoriteStorage(new Mock<IPreferenceStorage>().Object);
            await favoriteStorage.InitializeAsync();
            return favoriteStorage;
        }

        /// <summary>
        /// 删除收藏数据文件
        /// </summary>
        public static void RemoveDatabaseFile()
        {
            File.Delete(FavoriteStorage.FavoriteDbPath);
        }
    }
}