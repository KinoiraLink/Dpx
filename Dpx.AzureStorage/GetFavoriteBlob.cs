using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dpx.AzureStorage.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Dpx.AzureStorage
{
    /// <summary>
    /// 获取收藏数据
    /// </summary>
    public  class GetFavoriteBlob
    {
        /// <summary>
        /// 身份验证服务
        /// </summary>
        private IAuthenticationService _authenticationService;

        /// <summary>
        /// 授权服务
        /// </summary>
        private IAuthorizationService _authorizationService;

        /// <summary>
        /// 收藏存储
        /// </summary>
        private IFavoriteStorage _favoriteStorage;

        /// <summary>
        /// 获取收藏数据
        /// </summary>
        /// <param name="authenticationService">身份验证服务</param>
        /// <param name="authorizationService">授权服务</param>
        /// <param name="favoriteStorage">收藏存储</param>
        public GetFavoriteBlob(IAuthenticationService authenticationService, 
            IAuthorizationService authorizationService, 
            IFavoriteStorage favoriteStorage)
        {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _favoriteStorage = favoriteStorage;
        }

        [FunctionName("GetFavoriteBlob")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var authorizationHeader
                = req.Headers["Authorization"].FirstOrDefault();
            if (authorizationHeader == null || !authorizationHeader.StartsWith("Bearer"))
            {
                return new UnauthorizedResult();
            }

            string bearerToken = authorizationHeader.Substring("Bearer ".Length).Trim();
            var authenticationResult = await _authenticationService.AuthenticateAsync(bearerToken);
            if (!authenticationResult.Passed ||
                !await _authorizationService.AuthorizeAsync(authenticationResult.GitHubUserId))
            {
                return new UnauthorizedResult();
            }

            var bytes = await _favoriteStorage.GetAsync(authenticationResult.GitHubUserId);
            if (bytes == null)
            {
                return new NoContentResult();
            }

            return new FileContentResult(bytes, "application/zip");
        }
    }
}
