using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Dpx.Models;
using Dpx.Services;
using Moq;
using NUnit.Framework;

namespace DpxUnitTest.Helpers
{
    /// <summary>
    /// 诗词存储帮助类
    /// </summary>
    public static class PoetryStorageHelper
    {
        /// <summary>
        /// 数据库中调用一组的数量
        /// </summary>
        public const int NumberPoetry = 139;


        /// <summary>
        /// 获得已经初始化的诗词存储.
        /// </summary>
        /// <returns></returns>
        public static async Task<PoetryStorage> GetInitializedPoetryStorageAsync()
        {
            var poetryStorage = new PoetryStorage(new Mock<IPreferenceStorage>().Object);

            await poetryStorage.InitializeAsync();
            return poetryStorage;
        }
        /// <summary>
        /// 删除上一次数据库初始
        /// </summary>                                                                                      
                                                          
        public static void RemoveDatabaseFile()
        {
            File.Delete(PoetryStorage.PoetryDbPath);
        }
    }
}
