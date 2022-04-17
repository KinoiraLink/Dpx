using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dpx.Models;
using Dpx.Services;
using Moq;
using NUnit.Framework;

namespace DpxUnitTest.Services
{
    /// <summary>
    /// 同步服务测试
    /// </summary>
    public class SyncServiceTest
    {
        /// <summary>
        /// 测试同步
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestSyncAsync()
        {
            var localFavoriteList = new List<Favorite>();
            for (int i = 2; i >= 0; i--)
            {
                localFavoriteList.Add(new Favorite
                {
                    PoetryId = i* 2,
                    IsFavorite = i % 2 == 0,
                    Timestamp = i *2 
                });
            }
            /*
             * 4,true,4
             * 2,false,2
             * 0,true,0
             */

            var remoteFavoriteList = new List<Favorite>(); 
            for (int i = 1; i >= 0; i--)
            {
                remoteFavoriteList.Add(new Favorite
                {
                    PoetryId = i * 2 +1,
                    IsFavorite = i % 2 == 0,
                    Timestamp = i * 2 +1
                });
            }
            /*
             * 3,false,3
             * 1,true,1
             */
            var firstremote = remoteFavoriteList.First();
            var firstlocal = localFavoriteList.First();

            var firstRemoteFavoriteInLocal = new Favorite
            {
                PoetryId = firstremote.PoetryId,
                IsFavorite = firstremote.IsFavorite,
                Timestamp = int.MaxValue
            };
            localFavoriteList.Add(firstRemoteFavoriteInLocal);

            var firstLocalFavoriteInRemote = new Favorite
            {
                PoetryId = firstlocal.PoetryId,
                IsFavorite = firstlocal.IsFavorite,
                Timestamp = int.MaxValue
            };
            remoteFavoriteList.Add(firstLocalFavoriteInRemote);


            Dictionary<int, Favorite> localFavoriteDictionarySave = new Dictionary<int, Favorite>();
            Dictionary<int, Favorite> remoteFavoriteDictionarySave = null;

            var localFavoriteStorageMock =new Mock<IFavoriteStorage>();
            localFavoriteStorageMock.Setup(p => p.GetFavoriteItemsAsync()).ReturnsAsync(localFavoriteList);
            localFavoriteStorageMock.Setup(p => p.SaveFavoriteAsync(
                It.IsAny<Favorite>()
                
                )).Callback<Favorite>(p => localFavoriteDictionarySave[p.PoetryId]=p);
            var mockLocalFavoriteStorage = localFavoriteStorageMock.Object;
            var remoteFavoriteStorageMock = new Mock<IRemoteFavoriteStorage>();
            remoteFavoriteStorageMock.Setup(p => p.GetFavoriteItemAsync()).ReturnsAsync(remoteFavoriteList);

            remoteFavoriteStorageMock.Setup(p => p.SaveFavoriteItemsAsync(It.IsAny<List<Favorite>>()))
                .Callback<IList<Favorite>>(p => remoteFavoriteDictionarySave = p.ToDictionary(q => q.PoetryId, q => q));


            var mockRemoteFavoriteStorage = remoteFavoriteStorageMock.Object;


            var syncService = new SyncService(mockLocalFavoriteStorage, mockRemoteFavoriteStorage);

            await syncService.SyncAsync();

            localFavoriteStorageMock.Verify(p => p.GetFavoriteItemsAsync(),Times.Once);
            remoteFavoriteStorageMock.Verify(p =>p.GetFavoriteItemAsync(),Times.Once);

            Assert.AreEqual(5,localFavoriteDictionarySave.Count);
            Assert.AreEqual(5, remoteFavoriteDictionarySave.Count);

            foreach (var remoteFavorite in remoteFavoriteList)
            {
                Assert.IsTrue(localFavoriteDictionarySave.ContainsKey(remoteFavorite.PoetryId));
            }

            foreach (var localFavorite in localFavoriteList)
            {
                Assert.IsTrue(remoteFavoriteDictionarySave.ContainsKey(localFavorite.PoetryId));
            }

            Assert.AreEqual(int.MaxValue,localFavoriteDictionarySave[firstLocalFavoriteInRemote.PoetryId].Timestamp);
            Assert.AreEqual(int.MaxValue,remoteFavoriteDictionarySave[firstRemoteFavoriteInLocal.PoetryId].Timestamp);
        }
    }
}
