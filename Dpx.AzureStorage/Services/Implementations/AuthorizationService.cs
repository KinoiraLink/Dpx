using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dpx.AzureStorage.Models;
using Microsoft.WindowsAzure.Storage.Table;

namespace Dpx.AzureStorage.Services.Implementations
{
    /// <summary>
    /// 服务器授权服务实现
    /// </summary>
    public class AuthorizationService : IAuthorizationService
    {
        //******** 私有变量

        /// <summary>
        /// 表名
        /// </summary>
        private const string TableName = "Authorization";

        /// <summary>
        /// 分区键，一般不与表名相同
        /// </summary>
        private const string PartitionKey = TableName;


        /// <summary>
        /// 表存储
        /// </summary>
        private CloudTable _table;

        //******** 继承方法
        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="gitHubid">Github ID</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> AuthorizeAsync(int gitHubid)
        {
            var tableOperation = TableOperation.Retrieve<AuthorizationEntity>(PartitionKey, gitHubid.ToString());

            return (await _table.ExecuteAsync(tableOperation)).Result as AuthorizationEntity != null;
        }


        //******** 构造函数

        /// <summary>
        /// 授权服务
        /// </summary>
        /// <param name="accountProvider">Azure存储账号提供者</param>
        public AuthorizationService(IAzureStorageAccountProvider accountProvider)
        {
            var tableClient = accountProvider.GetAccount().CreateCloudTableClient();

            _table = tableClient.GetTableReference(TableName);
        }
    }
}
