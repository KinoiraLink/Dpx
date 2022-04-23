using System;
using System.Collections.Generic;
using System.Text;
using Dpx.AzureStorage;
using Dpx.AzureStorage.Services;
using Dpx.AzureStorage.Services.Implementations;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;


[assembly:FunctionsStartup(typeof(Startup))]

namespace Dpx.AzureStorage
{
    public  class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
            builder.Services.AddSingleton<IAuthorizationService, AuthorizationService>();
            builder.Services.AddSingleton<IFavoriteStorage, FavoriteStorage>();
            builder.Services.AddSingleton<IAzureStorageAccountProvider, AzureStorageAccountProvider>();
        }
    }
}
