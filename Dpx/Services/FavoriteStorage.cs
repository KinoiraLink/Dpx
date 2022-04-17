using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Dpx.Models;
using SQLite;

namespace Dpx.Services
{
    /// <summary>
    /// 收藏存储实现
    /// </summary>
    public class FavoriteStorage :IFavoriteStorage
    {
        //******** 公有变量
        /// <summary>
        /// 数据库文件路径
        /// </summary>
        public static readonly string FavoriteDbPath = 
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DbName);

        //******** 私有变量

        /// <summary>
        /// 数据库文件名
        /// </summary>
        private const string DbName = "favoritedb.sqlite3";

        private SQLiteAsyncConnection _connection;

        public SQLiteAsyncConnection Connection => _connection ?? new SQLiteAsyncConnection(FavoriteDbPath);

        /// <summary>
        /// 偏好存储
        /// </summary>
        private IPreferenceStorage _preferenceStorage;

        

        //******** 继承方法
        /// <summary>
        /// 初始化数据库  
        /// </summary>
        public async Task InitializeAsync()
        {
            await Connection.CreateTableAsync<Favorite>();
            _preferenceStorage.Set(FavoriteStorageConstants.VersionKey, FavoriteStorageConstants.Version);
        }

        /// <summary>
        /// 判断数据库已经初始化
        /// </summary>
        /// <returns></returns>
        public bool IsInitialized() =>
                _preferenceStorage.Get(FavoriteStorageConstants.VersionKey, FavoriteStorageConstants.DefultVersion) == FavoriteStorageConstants.Version;

        /// <summary>
        /// 获取一首是否被收藏的信息
        /// </summary>
        /// <param name="poetryId">诗词的</param>
        /// <returns></returns>
        public async Task<Favorite> GetFavoriteAsync(int poetryId)=>
            await Connection.Table<Favorite>()
                .FirstOrDefaultAsync(p => p.PoetryId == poetryId);

        /// <summary>
        /// 保存收藏信息
        /// </summary>
        /// <remarks>
        /// 收藏信息中已经隐含了诗词信息
        /// </remarks>
        /// <param name="favorite"></param>
        /// <returns></returns>
        public async Task SaveFavoriteAsync(Favorite favorite)
        {

            await Connection.InsertOrReplaceAsync(favorite);
            UpdateMode?.Invoke(this,new FavoriteStorageUpdateEventArgs(favorite));
        }
            

        /// <summary>
        /// 获取所有收藏信息
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Favorite>> GetFavoritesAsync() =>
            await Connection.Table<Favorite>().Where(p =>p.IsFavorite).ToListAsync();


        /// <summary>
        /// 删除一条数据，本程序用不到
        /// </summary>
        /// <param name="favorite"></param>
        /// <returns></returns>
        public async Task DeleteFavoritesAsync(Favorite favorite)
        {
            await Connection.DeleteAsync(favorite);
        }

        public event EventHandler<FavoriteStorageUpdateEventArgs> UpdateMode;



        public  async Task<IList<Favorite>> GetFavoriteItemsAsync()
        =>
            await Connection.Table<Favorite>().ToListAsync();
        
        //******** 公开方法


        /// <summary>
        /// 收藏存储
        /// </summary>
        /// <param name="preferenceStorage">偏好存储</param>
        public FavoriteStorage(IPreferenceStorage preferenceStorage)
        {
            _preferenceStorage = preferenceStorage;
        }


        /// <summary>
        /// 关闭数据库
        /// </summary>
        /// <returns></returns>
        public async Task CloseAsync() => await Connection.CloseAsync();

        
    }
}
