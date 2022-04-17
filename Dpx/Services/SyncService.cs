using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dpx.Models;

namespace Dpx.Services
{
    public class SyncService : ISyncService
    {
        //******** 私有变量
        /// <summary>
        /// 本地收藏存储
        /// </summary>
        private IFavoriteStorage _localFavoriteStorage;
        /// <summary>
        /// 远程收藏存储
        /// </summary>
        private IRemoteFavoriteStorage _remoteFavoriteStorage;
        //******** 继承方法

        /// <summary>
        /// 状态
        /// </summary>
        public string Status 
        { 
            get=>_status;
            private set
            {
                _status = value;
                StatusChanged?.Invoke(this,EventArgs.Empty);
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        private string _status;

        /// <summary>
        /// 状态改变事件
        /// </summary>
        public event EventHandler StatusChanged;


        /// <summary>
        /// 同步
        /// </summary>
        /// <returns></returns>
        public async Task SyncAsync()
        {
            var localList = await _localFavoriteStorage.GetFavoriteItemsAsync();

            Status = "正在读取远程收藏项。";

            var remoteList = await _remoteFavoriteStorage.GetFavoriteItemAsync();
            Dictionary<int, Favorite> LocalDictionary =new Dictionary<int, Favorite>();
            //**同步算法实现**；
            //将远程收藏项合并到本地
            await Task.Run(
                () =>
                {
                    LocalDictionary = localList.ToDictionary(p => p.PoetryId, p => p);
                    foreach (var remoteFavorite in remoteList)
                    {
                        if(LocalDictionary.TryGetValue(remoteFavorite.PoetryId,out var localFavorite))
                        {
                            //远程本地有数据，比较时间戳，远程比本地新
                            if (remoteFavorite.Timestamp > localFavorite.Timestamp)
                            {
                                //远程合并到本地
                                localFavorite.IsFavorite = remoteFavorite.IsFavorite;
                                localFavorite.Timestamp = remoteFavorite.Timestamp;
                            }
                            else
                            {
                                //todo 
                            }
                        }
                        else
                        {
                            //远程有本地无，把远程合并到本地
                            LocalDictionary[remoteFavorite.PoetryId] = remoteFavorite;
                        }
                    }
                }
            );
            foreach (var localFavorite in LocalDictionary.Values)
            {
                await _localFavoriteStorage.SaveFavoriteAsync(localFavorite);
            }




            //将本地收藏项合并到远程
            Dictionary<int, Favorite> remoteDictionary = new Dictionary<int, Favorite>();

            await Task.Run(
                () =>
                {
                    remoteDictionary = remoteList.ToDictionary(p => p.PoetryId, p => p);
                    foreach (var localFavorite in localList)
                    {
                        if (remoteDictionary.TryGetValue(localFavorite.PoetryId, out var remoteFavorite))
                        {
                            if (localFavorite.Timestamp > remoteFavorite.Timestamp)
                            {
                                remoteFavorite.IsFavorite = localFavorite.IsFavorite;
                                remoteFavorite.Timestamp = localFavorite.Timestamp;
                            }
                        }
                        else
                        {
                            remoteDictionary[localFavorite.PoetryId] = localFavorite;
                        }
                    }

                }

            );

            Status = "正在保存远程收藏项";
            await _remoteFavoriteStorage.SaveFavoriteItemsAsync(remoteDictionary.Values.ToList());
        }


        //******** 公开方法
        /// <summary>
        /// 同步服务
        /// </summary>
        /// <param name="localFavoriteStorage">本地收藏存储</param>
        /// <param name="remoteFavoriteStorage">远程收藏存储</param>
        public SyncService(IFavoriteStorage localFavoriteStorage,IRemoteFavoriteStorage remoteFavoriteStorage)
        {
            _localFavoriteStorage = localFavoriteStorage;
            _remoteFavoriteStorage = remoteFavoriteStorage;
        }
    }
}
