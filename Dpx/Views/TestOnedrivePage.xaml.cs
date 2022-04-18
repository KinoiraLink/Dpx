using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dpx.Models;
using Dpx.Services;
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
        OneDriveFavoriteStorage o = new OneDriveFavoriteStorage();
        private async void Button_OnClicked(object sender, EventArgs e)
        {
            
            Result.Text = (await o.SignInAsync()).ToString();
        }

        private async void AButton_OnClicked(object sender, EventArgs e)
        {
            await o.SaveFavoriteItemsAsync(
                new List<Favorite> {new Favorite {PoetryId = 0}, new Favorite {PoetryId = 1}});

        }

        private async void BButton_OnClicked(object sender, EventArgs e)
        {
            var favorites = await o.GetFavoriteItemAsync();
            Result.Text = favorites.Count.ToString();
        }
    }
}