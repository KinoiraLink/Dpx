using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dpx.Services;
using System.Threading.Tasks;
using Auth0.OidcClient;
using Azure.Core;
using Dpx.Confidential;
using Dpx.Models;

namespace Dpx.Droid.Service
{
    public class AuthenticationServiceAndroid : IAuthenticationService
    {

        //******** 公有变量


        //******** 私有变量

        /// <summary>
        /// Auth0客户端
        /// </summary>
        private Auth0Client _auth0Client;

        //******** 继承方法


        /// <summary>
        ///安卓身份验证服务
        /// </summary>
        /// <returns>省份验证结果</returns>
        public async Task<AuthenticationResult> AuthenticateAsync()
        {
            var auth0LoginResult = 
                await _auth0Client.LoginAsync(new
            {
                audience = AuthenticationSettings.Audience
            });

            AuthenticationResult authenticationResult;
            if (!auth0LoginResult.IsError)
            {
                authenticationResult = new AuthenticationResult()
                {
                    AccessToken = auth0LoginResult.AccessToken, 
                    AccessTokenExpiration = auth0LoginResult.AccessTokenExpiration.DateTime
                };
            }
            else
            {
                authenticationResult = 
                    new AuthenticationResult(auth0LoginResult.IsError, auth0LoginResult.Error);
            }

            return authenticationResult;
        }

        public AuthenticationServiceAndroid()
        {
            _auth0Client = new Auth0Client(new Auth0ClientOptions
            {
                Domain = AuthenticationSettings.Domain,//域名
                ClientId = AuthenticationSettings.ClientId//访问权限
            });
        }
    }
}