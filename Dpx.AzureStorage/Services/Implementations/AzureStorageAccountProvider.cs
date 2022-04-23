using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using Dpx.AzureStorage.Confidential;

namespace Dpx.AzureStorage.Services.Implementations
{
    /// <summary>
    /// Azure存储账号提供者（连接数据库）
    /// </summary>
    public class AzureStorageAccountProvider : IAzureStorageAccountProvider
    {
        //******** 私有变量
        /// <summary>
        /// 云存储账号
        /// </summary>
        private CloudStorageAccount _account;


        //******** 继承方法
        /// <summary>
        /// 获得云存储账户
        /// </summary>
        /// <returns></returns>
        public CloudStorageAccount GetAccount()
            => _account;


        public AzureStorageAccountProvider()
        {
            _account = CloudStorageAccount.Parse(AzureStorageSettings.ConnectionString);
        }
    }
}
