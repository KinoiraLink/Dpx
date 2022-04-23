using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Dpx.Models;
using Dpx.Utils;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace Dpx.Services.Implementations
{
    /// <summary>
    /// Azure收藏存储
    /// </summary>
    public class AzureFavoriteStorage : IRemoteFavoriteStorage
    {
        //******** 私有变量
        /// <summary>
        /// 偏好存储
        /// </summary>
        private IPreferenceStorage _preferenceStorage;

        /// <summary>
        /// 身份验证服务
        /// </summary>
        private IAuthenticationService _authenticationService;

        /// <summary>
        /// 警告服务
        /// </summary>
        private IAlertService _alertService;

        /// <summary>
        /// 缓存的Token
        /// </summary>
        private string _token;


        /// <summary>
        /// Token键
        /// </summary>
        private const string TokenKey = nameof(AzureFavoriteStorage) + ".Token";

        /// <summary>
        /// Token过期时间键
        /// </summary>
        private const string TokenExpiration = nameof(AzureFavoriteStorage) + ".TokenExpiration";


        /// <summary>
        /// Dpx Azure 服务器
        /// </summary>
        private const string Server = "DPx Azure服务器";


        //******** 继承方法

        /// <summary>
        /// 状态
        /// </summary>
        public string Status
        {

            get => _status;
            private set
            {
                _status = value;
                StatusChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        private string _status;

        /// <summary>
        /// 状态改变事件
        /// </summary>
        public event EventHandler StatusChanged;

        

        /// <summary>
        /// HttpClient是否已初始化
        /// </summary>
        private bool _isHttpClientInitialized = false;



        /// <summary>
        /// HtttpClent,AzureFavorite
        /// </summary>
        private HttpClient _httpClient = new HttpClient();

        private HttpClient HttpClient
        {
            get
            {
                if (!_isHttpClientInitialized)
                {
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", _token);
                    _isHttpClientInitialized = true;
                }
                return _httpClient;
            }
        }

        /// <summary>
        /// 服务端点
        /// </summary>
        private const string Endpoint = "http://10.38.58.10:7071/api/";
        /// <summary>
        /// 读取
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IList<Favorite>> GetFavoriteItemAsync()
        {
            HttpResponseMessage response;
            try
            {
                response =await _httpClient.GetAsync(Endpoint + "GetFavoriteBlob");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                _alertService.DisplayAlert(
                    ErrorMessages.HTTP_CLIENT_ERROR_TITLE,
                    ErrorMessages.HttpClientErrorMessage(Server, e.Message),
                    ErrorMessages.HTTP_CLIENT_BUTTON);
                return new List<Favorite>();
            }

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return new List<Favorite>();
            }

            var responseStream = await response.Content.ReadAsStreamAsync();
            ZipInputStream zipStream = new ZipInputStream(responseStream);
            ZipEntry zipEntry = zipStream.GetNextEntry();

            if (zipEntry == null)
            {
                return new List<Favorite>();
            }


            var jsonStream = new MemoryStream();

            await Task.Run(() =>
                StreamUtils.Copy(zipStream, jsonStream, new byte[1024]));

            zipStream.Close();
            responseStream.Close();

            jsonStream.Position = 0;
            var jsonReader = new StreamReader(jsonStream);
            var favoriteList = JsonConvert.DeserializeObject<IList<Favorite>>(await jsonReader.ReadToEndAsync());

            jsonReader.Close();
            jsonStream.Close();

            return favoriteList ?? new List<Favorite>();
        }



        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="favoriteList"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task SaveFavoriteItemsAsync(IList<Favorite> favoriteList)
        {
            var json = JsonConvert.SerializeObject(favoriteList);

            MemoryStream fileStream = new MemoryStream();
            ZipOutputStream zipStream = new ZipOutputStream(fileStream);
            zipStream.SetLevel(3);
            ZipEntry newEntry = new ZipEntry("DPX.json");
            newEntry.DateTime = DateTime.Now;
            zipStream.PutNextEntry(newEntry);

            var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            await Task.Run(() =>
                StreamUtils.Copy(jsonStream, zipStream, new byte[1024]));

            jsonStream.Close();
            zipStream.CloseEntry();
            zipStream.IsStreamOwner = false;
            zipStream.Close();

            Status = "正在上传远程收藏项";
            fileStream.Position = 0;
            
            //上传
            try
            {
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StreamContent(fileStream),"Dpx.zip","Dpx.zip");
                    var response = await HttpClient.PutAsync(Endpoint + "SaveFavoriteBlob",content);
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (Exception e)
            {
                _alertService.DisplayAlert(
                    ErrorMessages.HTTP_CLIENT_ERROR_TITLE,
                    ErrorMessages.HttpClientErrorMessage(Server, e.Message),
                    ErrorMessages.HTTP_CLIENT_BUTTON);

            }
            finally
            {
                fileStream.Close();
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SignInAsync()
        {
            Status = "正在登录到DPX Azure服务器";

            var authenticationResult = await _authenticationService.AuthenticateAsync();

            if (authenticationResult.IsError)
            {
                return false;
            }
            _token  = authenticationResult.AccessToken;

            await SecureStorage.SetAsync(TokenKey, _token);
            _preferenceStorage.Set(TokenExpiration, authenticationResult.AccessTokenExpiration);

            return true;
        }


        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task SignOutAsync()
        {
            Status = "正在注销";

            _token = "";

            SecureStorage.Remove(TokenKey);

            _preferenceStorage.Set(TokenExpiration,DateTime.MinValue);
            _isHttpClientInitialized = false;
        }

        /// <summary>
        /// 是否已经登录
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> IsSignedInAsync()
        {
            var isSignedIn = _preferenceStorage.Get(TokenExpiration, DateTime.MinValue) > DateTime.Now;
            if (!isSignedIn)
            {
                _isHttpClientInitialized = false;
            }
            else
            {
                _token = await SecureStorage.GetAsync(TokenKey);
            }
            return isSignedIn;
        }
        //不完备的做法//完备做法参考OneDriveFavoriteStorage

        //******** 公开方法
        /// <summary>
        /// Dpx Azure服务器存储
        /// </summary>
        /// <param name="preferenceStorage">偏好存储</param>
        /// <param name="authenticationService">身份验证服务</param>
        /// <param name="alertService">警告服务</param>
        public AzureFavoriteStorage(IPreferenceStorage preferenceStorage,
            IAuthenticationService authenticationService,
            IAlertService alertService)
        {
            _preferenceStorage = preferenceStorage;
            _authenticationService = authenticationService;
            _alertService = alertService;
        }


        //******** 测试函数
        //Todo to be delete
        public async Task<string> TestPingAsync()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer",_token);
            var response = await httpClient.GetAsync("http://10.38.58.10:7071/api/Ping");
            return await response.Content.ReadAsStringAsync();
        }

    }
}
