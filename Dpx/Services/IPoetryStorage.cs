using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dpx.Models;

namespace Dpx.Services
{
    /// <summary>
    /// 诗词存储接口
    /// </summary>
    public interface IPoetryStorage
    {
        /// <summary>
        /// 初始化数据库  
        /// </summary>
        /// <returns></returns>
        Task InitializeAsync();

        /// <summary>
        /// 判断数据库已经初始化
        /// </summary>
        /// <returns></returns>
        bool IsInitialized();

        /// <summary>
        /// 获取一首诗词，
        /// <param name="id">诗词id</param>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Poetry> GetPoetryAsync(int id);

        /// <summary>
        /// 获取满足给定条件的诗词集合
        /// 自动查询条件
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="skip">跳过结果的数量</param>
        /// <param name="take">读取结果的数量</param>
        /// <returns></returns>
        Task<IList<Poetry>> GetPoetriesAsync(
            Expression<Func<Poetry,bool>> where, int skip,int take);


    }

    /// <summary>
    /// 与诗词存储有关的常量
    /// </summary>
    public static class PoetryStorageConstants
    {
        /// <summary>
        /// 诗词数据库版本号
        /// </summary>
        public const int Version = 1;

        /// <summary>
        /// 默认版本号
        /// </summary>
        public const int DefultVersion = 0;
        /// <summary>
        /// 诗词数据库版本号的键。
        /// </summary>
        public const string VersionKey = nameof(PoetryStorageConstants) + "." + nameof(Version);
    }
}
