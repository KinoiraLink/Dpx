using Dpx.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Dpx.Confidential;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Newtonsoft.Json;

namespace Dpx.Services
{
    public class OneDriveFavoriteStorage : IRemoteFavoriteStorage
    {
        //******** 私有变量

        /// <summary>
        /// Mircosoft Graph权限数组
        /// </summary>
        private string[] _scopes = OneDriveOAuthSettings.Scopes.Split(' ');

        /// <summary>
        /// Micosoft Graph 身份验证
        /// </summary>
        private IPublicClientApplication _pca;

        /// <summary>
        /// Micosoft Graph 客户端
        /// </summary>
        private GraphServiceClient _graphClient;

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
        /// 获取所有收藏项，包括收藏与非收藏。
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Favorite>> GetFavoriteItemAsync()
        {
            var rootChildren = await _graphClient.Me.Drive.Root.Children.Request().GetAsync();
            if (!rootChildren.Any(p => p.Name == "DPX.zip"))
            {
                return new List<Favorite>();
            }

            var flieStream = await _graphClient.Me.Drive.Root.
                ItemWithPath("/DPX.zip").Content.Request().GetAsync();


            ZipInputStream zipStream = new ZipInputStream(flieStream);
            ZipEntry zipEntry = zipStream.GetNextEntry();

            if (zipEntry == null)
            {
                return new List<Favorite>();
            }


            var jsonStream = new MemoryStream();

            await Task.Run(()=>
                StreamUtils.Copy(zipStream,jsonStream,new byte[1024]));

            zipStream.Close();
            flieStream.Close();

            jsonStream.Position = 0;
            var jsonReader = new StreamReader(jsonStream);
            var favoriteList = JsonConvert.DeserializeObject<IList<Favorite>>(await jsonReader.ReadToEndAsync());

            jsonReader.Close();
            jsonStream.Close();

            return favoriteList ?? new List<Favorite>();



        }

        /// <summary>
        /// 是否登录
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsSignedInAsync()
        {
            Status = "正在检查OneDrive登录状态";
            string accessToken = string.Empty;
            try
            {
                var accounts = await _pca.GetAccountsAsync();
                if (accounts.Any())
                {
                    var silentAuthResult = await _pca
                        .AcquireTokenSilent(_scopes, accounts.FirstOrDefault())
                        .ExecuteAsync();
                    accessToken = silentAuthResult.AccessToken;
                }
            }
            catch (MsalUiRequiredException e)
            {
                return false;
            }

            return !string.IsNullOrEmpty(accessToken);
        }

        /// <summary>
        /// 保存所有收藏项，包括收藏与非收藏。
        /// </summary>
        /// <param name="favoriteList"></param>
        /// <returns></returns>
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

            try
            {
                await _graphClient.Me.Drive.Root.ItemWithPath("/DPX.zip").Content.Request()
                    .PutAsync<DriveItem>(fileStream);
            }
            catch (MsalClientException e)
            {
                //Todo
                throw;
            }
            finally
            {
                fileStream.Close();
            }
        }


        /// <summary>
        /// 登录
        /// </summary>
        public async Task<bool> SignInAsync()
        {
            Status = "正在登录到OneDrive";
            try
            {
                var interactiveRequest = _pca.AcquireTokenInteractive(_scopes);

                if (App.AuthUIParent != null)
                {
                    interactiveRequest = interactiveRequest.WithParentActivityOrWindow(App.AuthUIParent);
                }

                await interactiveRequest.ExecuteAsync();
            }
            catch (MsalClientException e)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        public async Task SignOutAsync()
        {
            Status = "正在退出登录OneDrive";
            var accounts = await _pca.GetAccountsAsync();
            while (accounts.Any())
            {
                await _pca.RemoveAsync(accounts.First());
                accounts = await _pca.GetAccountsAsync();
            }

        }
        //******** 公开方法

        public OneDriveFavoriteStorage()
        {
            var builder = PublicClientApplicationBuilder.Create(OneDriveOAuthSettings.ApplicationId);

            //Undo Ios 下的处理

            _pca = builder.Build();

            _graphClient = new GraphServiceClient(
                new DelegateAuthenticationProvider(async (requestMessage) =>
                {
                    var accounts = await _pca.GetAccountsAsync();

                    var result = await _pca.AcquireTokenSilent(_scopes, accounts.FirstOrDefault()).ExecuteAsync();
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
                })
                );
        }
    }
}
