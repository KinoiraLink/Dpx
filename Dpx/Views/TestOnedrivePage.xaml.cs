using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dpx.Models;
using Dpx.Services;
using Dpx.Services.Implementations;
using GalaSoft.MvvmLight.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dpx.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestOnedrivePage : ContentPage
    {
        public TestOnedrivePage()
        {
            InitializeComponent();
        }
        

        private async void CButton_OnClicked(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                new AlertService().DisplayAlert("1","2","3");
            });
        }

        private async void DButton_OnClicked(object sender, EventArgs e)
        {
            Result.Text = (await new JinrishiciService(SimpleIoc.Default.GetInstance<IPreferenceStorage>(),
                SimpleIoc.Default.GetInstance<IAlertService>(), SimpleIoc.Default.GetInstance<IPoetryStorage>()).GetTodayPoetryAsync()).Name;
        }
    }
}