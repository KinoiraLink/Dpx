using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage;

namespace Dpx.AzureStorage.Services
{

    /// <summary>
    /// Azure存储账号提供者（连接数据库）
    /// </summary>
    public  interface IAzureStorageAccountProvider
    {
        /// <summary>
        /// 获得云存储账户
        /// </summary>
        /// <returns></returns>
        CloudStorageAccount GetAccount();
    }
}
