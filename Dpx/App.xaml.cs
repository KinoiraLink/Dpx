using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dpx.Services;
using Dpx.Views;

namespace Dpx
{
    public partial class App : Application
    {
        /// <summary>
        /// android onedirve登录容器
        /// </summary>
        public static object AuthUIParent;
        
        public static object iOSKeychainSecurytGroup;
        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
