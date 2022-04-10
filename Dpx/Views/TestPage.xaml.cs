using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dpx.Models;
using Dpx.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dpx.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestPage : ContentPage
    {
        public TestPage()
        {
            InitializeComponent();
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            var cns = new ContentNavigationService(new ContentPageActivationService());
            await cns.NavigateToAsync(ContentNavigationContenstants.DetailPage,new Poetry{Name = "滁州西渐"});
        }
    }
}