using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dpx.Models;
using Dpx.Utils;
using Newtonsoft.Json;

namespace Dpx.Services.Implementations
{
    public class BingImageService : ITodayImageService
    {
        //******** 私有变量
        private ITodayImageService _todayImageService;

        private ITodayImageStorage _todayImageStorage;

        private HttpClient _httpClient = new HttpClient();

        private const string BigServer = "必应每日图片";

        
        private IAlertService _alertService;

        //******** 构造函数


        /// <summary>
        /// 从bing中获取图片
        /// </summary>
        /// <param name="ITodayImageStorage">今日图片存储</param>
        /// <param name="IAlertService">警告服务</param>
        public BingImageService(ITodayImageStorage todayImageStorage,IAlertService alertService)
        {
            _alertService = alertService;
            _todayImageStorage = todayImageStorage;
        }


        //******** 继承方法
        /// <summary>
        /// 获取今日图片
        /// </summary>
        /// <returns></returns>
        public async Task<TodayImage> GetTodayImageAsync()
            => await _todayImageStorage.GetAsync(true);

        /// <summary>
        /// 检查更新
        /// </summary>
        /// <returns></returns>
        public async Task<TodayImageServiceCheckUpdateResult> CheckAsync()
        {
            var todayImage = await _todayImageStorage.GetAsync(false);
            if (todayImage.ExpiresAt > DateTime.Now)
            {
                return new TodayImageServiceCheckUpdateResult {HasUpdate = false};
            }

            HttpResponseMessage response;

            try
            {
                response = await _httpClient.GetAsync(
                    "https://cn.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt=zh-CN");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                _alertService.DisplayAlert(
                    ErrorMessages.HTTP_CLIENT_ERROR_TITLE,
                    ErrorMessages.HttpClientErrorMessage(BigServer, e.Message),
                    ErrorMessages.HTTP_CLIENT_BUTTON);
                return new TodayImageServiceCheckUpdateResult
                {
                    HasUpdate = false
                };
            }

            var json = await response.Content.ReadAsStringAsync();
            var bingImage = JsonConvert.DeserializeObject<BingImageOfToday>(json).images[0];

            if (bingImage.fullstartdate == todayImage.FullStartDate)
            {
                todayImage.ExpiresAt = todayImage.ExpiresAt.AddHours(2);
                await _todayImageStorage.SaveExpiresAsAsync(todayImage);
                return new TodayImageServiceCheckUpdateResult
                {
                    HasUpdate = false
                };
            }

            todayImage = new TodayImage
            {
                Copyright = bingImage.copyright,
                ExpiresAt = DateTime.ParseExact(bingImage.fullstartdate,"yyyyMMddHHmm",CultureInfo.InvariantCulture).AddDays(1),
                CopyrightLink = bingImage.copyrightlink,
                FullStartDate = bingImage.fullstartdate
            };

            try
            {
                response = await _httpClient.GetAsync("https://www.bing.com" + bingImage.url);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                _alertService.DisplayAlert(
                    ErrorMessages.HTTP_CLIENT_ERROR_TITLE,
                    ErrorMessages.HttpClientErrorMessage(BigServer, e.Message),
                    ErrorMessages.HTTP_CLIENT_BUTTON);
                return new TodayImageServiceCheckUpdateResult
                {
                    HasUpdate = false
                };
            }

            todayImage.ImageBytes=await response.Content.ReadAsByteArrayAsync();
            await _todayImageStorage.SaveAsync(todayImage);
            return new TodayImageServiceCheckUpdateResult {HasUpdate = true, TodayImage = todayImage};
        }


    }

    public class BingImageOfToday
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Image
        {
            public string fullstartdate { get; set; }
            public string url { get; set; }
            public string copyright { get; set; }
            public string copyrightlink { get; set; }
        }


        public IList<Image> images { get; set; }
        
    }
}
