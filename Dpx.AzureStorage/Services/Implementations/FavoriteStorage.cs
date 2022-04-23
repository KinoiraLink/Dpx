using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Dpx.AzureStorage.Services.Implementations
{
    /// <summary>
    /// 收藏存储
    /// </summary>
    public class FavoriteStorage : IFavoriteStorage
    {
        //******** 私有变量

        /// <summary>
        /// 容器名
        /// </summary>
        private const string ContainerName = "favorite-storage";

        /// <summary>
        /// 容器
        /// </summary>
        private CloudBlobContainer _container;


        //******** 继承方法]

        /// <summary>
        /// 保存收藏数据
        /// </summary>
        /// <param name="favoriteBytes">收藏数据</param>
        /// <param name="gitHubId">GitHub Id</param>
        /// <returns></returns>
        public async Task SaveAsync(byte[] favoriteBytes, int gitHubId)
        {
            var blob = _container.GetBlockBlobReference(gitHubId + ".zip");
            await blob.UploadFromByteArrayAsync(favoriteBytes, 0, favoriteBytes.Length);
        }

        /// <summary>
        /// 读取收藏数据
        /// </summary>
        /// <param name="gitHubId">GitHub Id</param>
        /// <returns></returns>
        public async Task<byte[]> GetAsync(int gitHubId)
        {
            var blob = _container.GetBlockBlobReference(gitHubId + ".zip");
            if (!await blob.ExistsAsync())
            {
                return null;
            }

            var memorySteam = new MemoryStream();
            await blob.DownloadToStreamAsync(memorySteam);
            return memorySteam.ToArray();
        }

        //******** 公开方法


        /// <summary>
        /// 收藏存储
        /// </summary>
        /// <param name="_accountProvider">Azure存储账户提供者</param>
        public FavoriteStorage(IAzureStorageAccountProvider _accountProvider)
        {
            var blobClient = _accountProvider.GetAccount().CreateCloudBlobClient();
            _container = blobClient.GetContainerReference(ContainerName);
        }
    }
}
