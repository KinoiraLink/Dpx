using System;
using System.Collections.Generic;
using System.Text;
using DpxUnitTest.Helpers;
using NUnit.Framework;

namespace DpxUnitTest.Services
{
    /// <summary>
    /// 收藏存储测试
    /// </summary>
    public class FavoriteStorageTest
    {
        /// <summary>
        /// 删除数据文件
        /// </summary>
        [SetUp, TearDown]
        public static void RemoveDatabaseFile() =>
            FavoriteStorageHelper.RemoveDatabaseFile();


    }
}
