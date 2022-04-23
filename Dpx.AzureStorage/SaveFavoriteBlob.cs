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
    /// 保存收藏数据
    /// </summary>
    public class SaveFavoriteBlob
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
        /// 保存收藏数据
        /// </summary>
        /// <param name="authenticationService">身份验证服务</param>
        /// <param name="authorizationService">授权服务</param>
        /// <param name="favoriteStorage">收藏存储</param>
        public SaveFavoriteBlob(IAuthenticationService authenticationService, IAuthorizationService authorizationService, IFavoriteStorage favoriteStorage)
        {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _favoriteStorage = favoriteStorage;
        }

        [FunctionName("SaveFavoriteBlob")]
        public  async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)] HttpRequest req,
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

            int gitHubId = authenticationResult.GitHubUserId;

            if (!req.ContentType.Contains("multipart/form-data") || 
                (req.Form.Files?.Count ?? 0) == 0)
            {
                return new BadRequestResult();
            }

            var file = req.Form.Files.First();
            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            await _favoriteStorage.SaveAsync(memoryStream.ToArray(), gitHubId);
            return new NoContentResult();
        }
    }
}
