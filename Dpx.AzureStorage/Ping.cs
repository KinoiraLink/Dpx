using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dpx.AzureStorage.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
namespace Dpx.AzureStorage
{
    public static class Ping
    {
        [FunctionName("Ping")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var authorizationHeader 
                = req.Headers["Authorization"].FirstOrDefault();

            var token=authorizationHeader.Substring("Bearer ".Length).Trim();

            var s = new AuthenticationService();
            var result = await s.AuthenticateAsync(token);
            var a = new AuthorizationService(new AzureStorageAccountProvider());
            if (await a.AuthorizeAsync(result.GitHubUserId))
            {
                return new OkObjectResult("Pong"+ result.GitHubUserId);
            }
            else
            {
                return new OkObjectResult("Denid"); 
            }

            
        }
    }
}
