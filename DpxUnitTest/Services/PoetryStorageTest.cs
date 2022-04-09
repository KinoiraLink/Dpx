using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Dpx.Models;
using Dpx.Services;
using DpxUnitTest.Helpers;
using Moq;
using NUnit.Framework;


namespace DpxUnitTest.Service
{
    

    /// <summary>
    /// 诗词存储测试
    /// </summary>
    public class PoetryStorageTest
    {
        [SetUp, TearDown]//运行开始前,运行结束前   
        public static void RemoveDatabaseFile() =>
            PoetryStorageHelper.RemoveDatabaseFile();

        /// <summary>
        /// 测试数据库初始化
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestInitializeAsync()
        {
            Assert.IsFalse(File.Exists(PoetryStorage.PoetryDbPath));
            var preferenceStorageMock = new Mock<IPreferenceStorage>();
            var mockPreferenceStorage = preferenceStorageMock.Object;
            var poetryStorage = new PoetryStorage(mockPreferenceStorage);
            await poetryStorage.InitializeAsync();

            Assert.IsTrue(File.Exists(PoetryStorage.PoetryDbPath));
            preferenceStorageMock.Verify(p =>p.Set(PoetryStorageConstants.VersionKey,PoetryStorageConstants.Version),Times.Once);
        }

        /// <summary>
        /// 测试IsInitialized,数据库是否已经初始化
        /// </summary>
        [Test]
        public void TestIsInitialized()
        {
            var preferenceStorageMock = new Mock<IPreferenceStorage>();

            preferenceStorageMock.Setup(p =>
                p.Get(
                    PoetryStorageConstants.VersionKey, 
                    PoetryStorageConstants.DefultVersion)).Returns(PoetryStorageConstants.Version);
            var mockPreferenceStorage = preferenceStorageMock.Object;

            var poetryStorage = new PoetryStorage(mockPreferenceStorage);
            Assert.IsTrue(poetryStorage.IsInitialized());
         
        }


        /// <summary>
        /// 测试获取一首诗
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestGetPoetryAsync()
        {
            var poetryStorage = await PoetryStorageHelper.GetInitializedPoetryStorageAsync();
           
            var poetry = await poetryStorage.GetPoetryAsync(10001);
            Assert.AreEqual("临江仙 · 夜归临皋", poetry.Name);
            
            await poetryStorage.CloseAsync();
        }

        /// <summary>
        /// 测试一组满足条件的数据
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestGetPoetriesAsync()
        {
            var poetryStorage = await PoetryStorageHelper.GetInitializedPoetryStorageAsync();

            var where = Expression.Lambda<Func<Poetry, bool>>(Expression.Constant(true),
                Expression.Parameter(typeof(Poetry), "p"));
            var poetries = 
                await poetryStorage.GetPoetriesAsync(@where, 0, int.MaxValue);
            Assert.AreEqual(PoetryStorageHelper.NumberPoetry,poetries.Count);
            await poetryStorage.CloseAsync();
        }

    }
}
