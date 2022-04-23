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
    /// �����ղ�����
    /// </summary>
    public class SaveFavoriteBlob
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
        /// �����ղ�����
        /// </summary>
        /// <param name="authenticationService">�����֤����</param>
        /// <param name="authorizationService">��Ȩ����</param>
        /// <param name="favoriteStorage">�ղش洢</param>
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
