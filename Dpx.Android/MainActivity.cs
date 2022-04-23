using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Dpx.Droid.Service;
using Dpx.Services;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Identity.Client;

namespace Dpx.Droid
{
    [Activity(Label = "Dpx", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,LaunchMode = LaunchMode.SingleTask)]

    [IntentFilter(new []{Intent.ActionView},
        Categories = new [] {Intent.CategoryDefault,Intent.CategoryBrowsable},
        //Todo bo be Updated
        DataScheme = "com.companyname.dpx", DataHost = "dpx201810405319.us.auth0.com",
        DataPathPrefix = "/android/com.companyname.dpx/callback")]



    
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            SimpleIoc.Default.Register<IAuthenticationService,AuthenticationServiceAndroid>();



            LoadApplication(new App());

            App.AuthUIParent = this;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode,resultCode,data);
        }


        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            Auth0.OidcClient.ActivityMediator.Instance.Send(intent.DataString);
        }
    }
}