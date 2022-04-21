using Dpx.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dpx.Services.Implementations
{
    /// <summary>
    /// 今日图片存储实现
    /// </summary>
    public class TodayImageStorage : ITodayImageStorage
    {
        //******** 公有变量
        /// <summary>
        /// 完整开始日期配置项键
        /// </summary>
        public static readonly string FullStartDateKey = nameof(TodayImage) + "." + nameof(TodayImage.FullStartDate);


        /// <summary>
        /// 完整开始日期默认值
        /// </summary>
        public const string FullStartDateDefault = "201901010700";

        /// <summary>
        /// 过期时间配置项键
        /// </summary>
        public static readonly string ExpiresAtKey = nameof(TodayImage) + "." + nameof(TodayImage.ExpiresAt);


        /// <summary>
        /// 过期时间默认值。
        /// </summary>
        public static readonly DateTime ExpiresAtDefault = new DateTime(2019, 1, 2, 7, 0, 0);

        /// <summary>
        /// 版权信息配置项键
        /// </summary>
        public static readonly string CopyrightKey = nameof(TodayImage) + "." + nameof(TodayImage.Copyright);

        /// <summary>
        /// 版权信息默认值
        /// </summary>
        public const string CopyrightDefault = "salt field province vietnm work(@ Quangrapha/Pixabay)";
        
        /// <summary>
        /// 版权链接配置项键
        /// </summary>
        public static readonly string CopyrightLinkKey = nameof(TodayImage) + "." + nameof(TodayImage.CopyrightLink);

        public const string CopyrightLinkDefault =
            "https://www.bing.com/search?q=%E5%86%B0%E5%B2%9B%E5%88%9D%E5%A4%8F%E8%8A%82&form=hpcapt&mkt=zh-cn";

        /// <summary>
        /// 今日图片文件名
        /// </summary>
        public const string FileName = "todayImage.bin";

        public static readonly string TodayImagePath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), FileName);


        //******** 私有变量
        /// <summary>
        /// 偏好存储
        /// </summary>
        private IPreferenceStorage _preferenceStorage;

        //******** 公开方法

        /// <summary>
        /// 今日图片存储
        /// </summary>
        /// <param name="preferenceStorage">偏好存储方法</param>
        public TodayImageStorage(IPreferenceStorage preferenceStorage)
        {
            _preferenceStorage = preferenceStorage;
        }






        //******** 继承方法
        /// <summary>
        /// 存图
        /// </summary>
        /// <param name="includesImageByte">是否包含图片</param>
        /// <returns></returns>
        public async  Task<TodayImage> GetAsync(bool includesImageByte)
        {
            var todayImage = new TodayImage
            {
                FullStartDate = _preferenceStorage.Get(FullStartDateKey, FullStartDateDefault),
                ExpiresAt = _preferenceStorage.Get(ExpiresAtKey, ExpiresAtDefault),
                Copyright = _preferenceStorage.Get(CopyrightKey, CopyrightDefault),
                CopyrightLink = _preferenceStorage.Get(CopyrightLinkKey, CopyrightLinkDefault)
            };

            if (!includesImageByte)
            {
                return todayImage;
            }

            using (var imageMemoryStream = new MemoryStream())
            {
                if (File.Exists(TodayImagePath))
                {
                    using (var imageFileStream = new FileStream(TodayImagePath, FileMode.Open))
                    {
                        await imageFileStream.CopyToAsync(imageMemoryStream);
                    }

                    todayImage.ImageBytes = imageMemoryStream.ToArray();
                }
                else
                {
                    using (var dbAssertStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(FileName))
                    {
                        await dbAssertStream.CopyToAsync(imageMemoryStream);
                    }
                }

                todayImage.ImageBytes = imageMemoryStream.ToArray();
            }
            return todayImage;
            
            
        }

        /// <summary>
        /// b保存成今日图片的过期时间
        /// </summary>
        /// <param name="todayImage">今日图片</param>
        /// <returns></returns>
        public async Task SaveAsync(TodayImage todayImage)
        {
            _preferenceStorage.Set(ExpiresAtKey,todayImage.ExpiresAt);
            _preferenceStorage.Set(FullStartDateKey,todayImage.FullStartDate);
            _preferenceStorage.Set(CopyrightKey,todayImage.Copyright);
            _preferenceStorage.Set(CopyrightLinkKey,todayImage.CopyrightLink);

            using (var imageFileStream = new FileStream(TodayImagePath,FileMode.Create))
            {
                await imageFileStream.WriteAsync(todayImage.ImageBytes, 0, todayImage.ImageBytes.Length);
            }
        }


        /// <summary>
        /// 存储过期时限
        /// </summary>
        /// <param name="todayImage">是否包含图片数组</param>
        /// <returns></returns>
        public async Task SaveExpiresAsAsync(TodayImage todayImage)=>
        _preferenceStorage.Set(ExpiresAtKey,todayImage.ExpiresAt);
        
    }
}
