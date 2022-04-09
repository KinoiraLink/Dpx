using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dpx.Models;
using SQLite;
using Xamarin.Essentials;

namespace Dpx.Services
{
    public class PoetryStorage : IPoetryStorage
    {
        //******** 公有变量
        /// <summary>
        /// 数据库文件路径
        /// </summary>
        public static readonly string PoetryDbPath =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DbName);
        //******** 私有变量
        /// <summary>
        /// 数据库文件名
        /// </summary>
        private const string DbName = "poetrydb.sqlite3";
        

        private SQLiteAsyncConnection _connection;

        public SQLiteAsyncConnection Connection => _connection ?? new SQLiteAsyncConnection(PoetryDbPath);

        /// <summary>
        /// 偏好存储
        /// </summary>
        private IPreferenceStorage _preferenceStorage;

        //******** 继承方法
        /// <returns></returns>
        /// <summary>
        /// 初始化数据库  
        /// </summary>
        /// <returns></returns>
        public async Task InitializeAsync()
        {
            using (var dbFileStream = new FileStream(PoetryDbPath, FileMode.Create))
            {
                using (var dbAssertStream = 
                       Assembly.GetExecutingAssembly().GetManifestResourceStream(DbName))
                {
                    await dbAssertStream.CopyToAsync(dbFileStream);
                }
            }

            _preferenceStorage.Set(PoetryStorageConstants.VersionKey,PoetryStorageConstants.Version);
            
           
        }
        /// <summary>
        /// 判断数据库已经初始化
        /// </summary>
        /// <returns></returns>
        //public bool IsInitialized()
        //{
        //    Preferences.Get(PoetryStorageConstants.VersionKey, -1) == PoetryStorageConstants.Version;
        //}
        public bool IsInitialized()=>
         _preferenceStorage.Get(PoetryStorageConstants.VersionKey, PoetryStorageConstants.DefultVersion) == PoetryStorageConstants.Version;

        /// <summary>
        /// 获取一首诗词，
        /// <param name="id">诗词id</param>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Poetry> GetPoetryAsync(int id) =>
            await Connection.Table<Poetry>().
                FirstOrDefaultAsync(p => p.Id == id);

        //public async Task<Poetry> GetPoetryAsync(int id)
        //{
        //    var re = await Connection.Table<Poetry>().FirstOrDefaultAsync(p => p.Id == id);
        //    return re;
        //}


        /// <summary>
        /// 获取满足给定条件的诗词集合
        /// 自动查询条件
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="skip">跳过结果的数量</param>
        /// <param name="take">读取结果的数量</param>
        /// <returns></returns>
        //public async Task<IList<Poetry>> GetPoetryAsync(Expression<Func<Poetry, bool>> where, int skip, int take)
        //{
        //    var p = await Connection.Table<Poetry>().Where(where).Skip(skip).Take(take).ToListAsync();
        //    return p;
        //}
        public async Task<IList<Poetry>> GetPoetriesAsync(Expression<Func<Poetry, bool>> where, int skip, int take)=>
         await Connection.Table<Poetry>().Where(where).Skip(skip).Take(take).ToListAsync();



        //******** 公开方法
        /// <summary>
        /// 诗词存储
        /// </summary>
        /// <param name="preferenceStorage">偏好存储</param>
        public PoetryStorage(IPreferenceStorage preferenceStorage)
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
