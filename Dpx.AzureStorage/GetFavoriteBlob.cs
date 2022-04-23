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
    /// ��ȡ�ղ�����
    /// </summary>
    public  class GetFavoriteBlob
    {
        /// <summary>
        /// �����֤����
        /// </summary>
        private IAuthenticationService _authenticationService;

        /// <summary>
        /// ��Ȩ����
        /// </summary>
        private IAuthorizationService _authorizationService;

        /// <summary>
        /// �ղش洢
        /// </summary>
        private IFavoriteStorage _favoriteStorage;

        /// <summary>
        /// ��ȡ�ղ�����
        /// </summary>
        /// <param name="authenticationService">�����֤����</param>
        /// <param name="authorizationService">��Ȩ����</param>
        /// <param name="favoriteStorage">�ղش洢</param>
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
