using Dpx.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dpx.Utils;
using Newtonsoft.Json;

namespace Dpx.Services.Implementations
{
    public class JinrishiciService : ITodayPoetryService
    {
        //******** 公有变量
        /// <summary>
        /// Token键
        /// </summary>
        public const string TokenKey = nameof(JinrishiciService) + ".Token";


        /// <summary>
        /// 今日诗词服务器
        /// </summary>
        public const string JinrishiciServer = "今日诗词";


        //******** 私有变量

        /// <summary>
        /// 在内存中缓存的的token
        /// </summary>
        public string _token;


        /// <summary>
        /// 偏好存储
        /// </summary>
        private IPreferenceStorage _preference;


        /// <summary>
        /// HttpClient
        /// </summary>
        private HttpClient _httpClient = new HttpClient();

        /// <summary>
        /// 警告服务
        /// </summary>
        private IAlertService _alertService;

        /// <summary>
        /// 诗词存储
        /// </summary>
        private IPoetryStorage _poetryStorage;





        /// <summary>
        /// 获取今日诗词
        /// </summary>
        /// <returns></returns>
        public async Task<TodayPoetry> GetTodayPoetryAsync()
        {
            if (string.IsNullOrEmpty(_token))
            {
                await InitializeTokenAsync();
                if (string.IsNullOrEmpty(_token))
                {
                    return await GetRandomPoetryAsync();
                }

                var headers = _httpClient.DefaultRequestHeaders;
                headers.Add("X-User-Token", _token);
            }

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.GetAsync("https://v2.jinrishici.com/sentence");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                _alertService.DisplayAlert(
                    ErrorMessages.HTTP_CLIENT_ERROR_TITLE,
                    ErrorMessages.HttpClientErrorMessage(JinrishiciServer, e.Message),
                    ErrorMessages.HTTP_CLIENT_BUTTON);
                return await GetRandomPoetryAsync();
            }

            var json = await response.Content.ReadAsStringAsync();

            var jinrishiciSentence = JsonConvert.DeserializeObject<JinrishiciSentence>(json);

            return new TodayPoetry()
            {
                AuthorName = jinrishiciSentence.data.origin.author,
                Content = string.Join("\n",jinrishiciSentence.data.origin.content),
                Dynasty = jinrishiciSentence.data.origin.dynasty,
                Name = jinrishiciSentence.data.origin.title,
                Snippet = jinrishiciSentence.data.content,
                Source = TodayPoetry.Jinrishici
            };
        }

        //******** 公开方法
        /// <summary>
        /// 今日诗词服务
        /// </summary>
        /// <param name="preferenceStorage">偏好存储</param>
        /// <param name="alertService">警告服务</param>
        public JinrishiciService(IPreferenceStorage preferenceStorage, IAlertService alertService,IPoetryStorage poetryStorage)
        {
            _preference = preferenceStorage;
            _alertService = alertService;
            _poetryStorage = poetryStorage;
        }
        //******** 私有方法

        /// <summary>
        /// 初始化Token
        /// </summary>
        /// <returns></returns>
        public async Task InitializeTokenAsync()
        {
            if (!string.IsNullOrEmpty(_token))
            {
                return ;
            }

            _token = _preference.Get(TokenKey, string.Empty);

            if (!string.IsNullOrEmpty(_token))
            {
                return ;
            }

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.GetAsync("https://v2.jinrishici.com/token");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                _alertService.DisplayAlert(
                    ErrorMessages.HTTP_CLIENT_ERROR_TITLE,
                    ErrorMessages.HttpClientErrorMessage(JinrishiciServer, e.Message),
                    ErrorMessages.HTTP_CLIENT_BUTTON);
                return ;
            }

            var json = await response.Content.ReadAsStringAsync();

            var JinrishiciToken = JsonConvert.DeserializeObject<JinrishiciToken>(json);

            _token = JinrishiciToken.data;
            _preference.Set(TokenKey, _token);

            return ;
        }

        /// <summary>
        /// 获得随机诗词
        /// </summary>
        /// <returns></returns>
        public async Task<TodayPoetry> GetRandomPoetryAsync()
        {
            var poetries = await _poetryStorage.GetPoetriesAsync(Expression.Lambda<Func<Poetry, bool>>(Expression.Constant(true),
                    Expression.Parameter(typeof(Poetry), "p")),
                new Random().Next(PoetryStorageConstants.NumberPoetry), 1);
            return new TodayPoetry
            {
                AuthorName = poetries[0].AuthorName,
                Content = poetries[0].Content,
                Dynasty = poetries[0].Dynasty,
                Name = poetries[0].Name,
                Snippet = poetries[0].Snippet,
                Source = TodayPoetry.Local
            };
        }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class JinrishiciToken
    {
        public string status { get; set; }
        public string data { get; set; }
    }

    public class JinrishiciSentence
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Origin
        {
            public string title { get; set; }
            public string dynasty { get; set; }
            public string author { get; set; }
            public List<string> content { get; set; }
        }

        public class Data
        {
            public string content { get; set; }
            public Origin origin { get; set; }
        }


        public Data data { get; set; }
    }
}
