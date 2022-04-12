using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dpx.Models;
using Dpx.Services;
using Dpx.ViewModels;
using Moq;
using NUnit.Framework;

namespace DpxUnitTest.ViewModel
{
    /// <summary>
    ///诗词详情页测试
    /// </summary>
    public class DetailPageViewModelTest
    {
        [Test]
        public async Task TestPageAppearingCommand()
        {
            var poetry = new Poetry{Id = 1};
            var favorite = new Favorite {PoetryId = poetry.Id,IsFavorite = true};

            var favoriteStorageMock = new Mock<IFavoriteStorage>();
            favoriteStorageMock.Setup(p => p.GetFavoriteAsync(poetry.Id)).ReturnsAsync(favorite);
            var mockFavoriteStorage = favoriteStorageMock.Object;


            var detailPageViewModel = new DetailPageViewModel(mockFavoriteStorage);

            detailPageViewModel.Poetry = poetry;
            await detailPageViewModel.PageAppearingCommandFunction();
            Assert.AreSame(favorite,detailPageViewModel.Favorite);
        }

    }
}
