using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dpx.Models;
using Dpx.Services;
using DpxUnitTest.Helpers;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.DataCollection;
using Moq;
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

        [Test]
        public async Task TestInitialize()
        {
            var mock = new Mock<IPreferenceStorage>();
            mock.Setup(p =>p.Get(FavoriteStorageConstants.VersionKey,FavoriteStorageConstants.DefultVersion)).Returns(FavoriteStorageConstants.Version-1);
            var mockPreferenceStorage = mock.Object;

            var favoriteStorage = new FavoriteStorage(mockPreferenceStorage);

            Assert.IsFalse(favoriteStorage.IsInitialized());

            mock.Setup(p => p.Get(FavoriteStorageConstants.VersionKey, FavoriteStorageConstants.DefultVersion)).Returns(FavoriteStorageConstants.Version);

            Assert.IsTrue(favoriteStorage.IsInitialized());

            Assert.IsFalse(File.Exists(FavoriteStorage.FavoriteDbPath));
            await favoriteStorage.InitializeAsync();
            
            Assert.IsTrue(File.Exists(FavoriteStorage.FavoriteDbPath));
            await favoriteStorage.CloseAsync();

            mock.Verify(p => p.Set(FavoriteStorageConstants.VersionKey,FavoriteStorageConstants.Version),Times.Once);


        }

        /// <summary>
        /// 测试增删改查
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestCrud()
        {
            var favoriteStorage = await FavoriteStorageHelper.GetInitilizedFavoriteStorageAsync();

            var favorites = new List<Favorite>()
            {
                new Favorite{PoetryId = 1,IsFavorite = true},
                new Favorite{PoetryId = 2,IsFavorite = false}
            };
            Favorite updatedFavorite = null;
            favoriteStorage.UpdateMode += (sender, args) => updatedFavorite = args.UpdateFavorite;

            //插入或修改
            await favoriteStorage.SaveFavoriteAsync(favorites[0]);
            await favoriteStorage.SaveFavoriteAsync(favorites[1]);

            Assert.AreSame(favorites[1],updatedFavorite);

            //查一条数据
            var favoriteAsync =await favoriteStorage.GetFavoriteAsync(favorites[0].PoetryId);
            Assert.AreEqual(favorites[0].PoetryId,favoriteAsync.PoetryId);
            Assert.AreEqual(favorites[0].IsFavorite, favoriteAsync.IsFavorite);

            //查所有
            var favoritesAsync = await favoriteStorage.GetFavoritesAsync();
            Assert.AreEqual(favorites.Count(p =>p.IsFavorite),favoritesAsync.Count);
            Assert.IsTrue(favoritesAsync.All(p =>p.IsFavorite));
            //删除一条数据
            await favoriteStorage.DeleteFavoritesAsync(favorites[0]);
             favoritesAsync = await favoriteStorage.GetFavoritesAsync();
            Assert.AreEqual(favorites.Count(p =>p.IsFavorite)-1, favoritesAsync.Count);
            await favoriteStorage.CloseAsync();

        }

    }
}
