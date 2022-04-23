using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dpx.AzureStorage.Models;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Dpx.AzureStorage.Services.Implementations
{
    /// <summary>
    /// 身份验证服务——服务端
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        //******** 私有变量
        /// <summary>
        /// OpenId配置管理器
        /// </summary>
        private IConfigurationManager<OpenIdConnectConfiguration> _configurationManager;


        /// <summary>
        /// 身份验证签发者
        /// </summary>
        private const string Issuer = "https://dpx201810405319.us.auth0.com/";

        /// <summary>
        /// 监听者
        /// </summary>
        public const string Audience = "https://dpx-azure-storage/";


        //******** 继承方法

        public async Task<AuthenticationResult> AuthenticateAsync(string token)
        {
            
            var name = "";//存取解析出来的用户名

            try
            {
                var openIdConfig = await _configurationManager.GetConfigurationAsync(CancellationToken.None);

                TokenValidationParameters validationParameters = new TokenValidationParameters //验证参数
                {
                    ValidIssuer = Issuer, //签发者
                    ValidAudiences = new[] {Audience}, //监听者，访问的权限
                    IssuerSigningKeys = openIdConfig.SigningKeys //签名
                };

                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

                var validationResult = handler.ValidateToken(token, validationParameters, out _);
                //身份验证结果
                name = validationResult.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            }
            catch (SecurityTokenSignatureKeyNotFoundException e)
            {
                _configurationManager.RequestRefresh();
                return new AuthenticationResult
                {
                    Message = $"SecurityTokenSignatureKeyNotFoundException:{e.Message}"
                };
            }
            catch (SecurityTokenException e)
            {
                return new AuthenticationResult
                {
                    Message = $"SecurityTokenException:{e.Message}"
                };
            }

            return new AuthenticationResult()
            {
                GitHubUserId = int.Parse(name.Split("|")[1]),
                Passed = true
            };
        }

        //******** 公开方法

        /// <summary>
        /// 身份验证服务
        /// </summary>
        public AuthenticationService()
        {
            var documentRetriever = new HttpDocumentRetriever
            {
                RequireHttps = Issuer.StartsWith("https://")
            };

            _configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                $"{Issuer}.well-known/openid-configuration", 
                new OpenIdConnectConfigurationRetriever(),
                documentRetriever);
        }

    }
}
