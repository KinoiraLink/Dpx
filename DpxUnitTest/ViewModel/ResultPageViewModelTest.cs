using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dpx.Models;
using Dpx.Services;
using Dpx.ViewModels;
using DpxUnitTest.Helpers;
using Moq;
using NUnit.Framework;

namespace DpxUnitTest.ViewModel
{
    /// <summary>
    /// 搜索结果页测试
    /// </summary>
    public class ResultPageViewModelTest
    {
        [SetUp, TearDown]//运行开始前,运行结束前   
        public static void RemoveDatabaseFile() =>
            PoetryStorageHelper.RemoveDatabaseFile();

        [Test]
        public async Task TestPoetryColloection()
        {
            var where = Expression.Lambda<Func<Poetry, bool>>(Expression.Constant(true),
                Expression.Parameter(typeof(Poetry), "p"));


            var poetryStorage = await PoetryStorageHelper.GetInitializedPoetryStorageAsync();

            var resultPageViewModel = new ResultPageViewModel(poetryStorage,null);
            resultPageViewModel.Where = where;
            List<string> statusList = new List<string>();
            resultPageViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(ResultPageViewModel.Status))
                {
                    statusList.Add(resultPageViewModel.Status);
                }
            };
            Assert.AreEqual(0, resultPageViewModel.PoetryCollection.Count);
            await resultPageViewModel.PageAppearingCommandFunction();
            Assert.AreEqual(20, resultPageViewModel.PoetryCollection.Count);

            Assert.AreEqual(2,statusList.Count);
            Assert.AreEqual((ResultPageViewModel.Loading),statusList[0]);

            var poetryCollectionChanged = false;
            resultPageViewModel.PoetryCollection.CollectionChanged += (sender, args) =>
            {
                poetryCollectionChanged = true;
            };

            await resultPageViewModel.PageAppearingCommandFunction();
            Assert.IsFalse(poetryCollectionChanged);

            await poetryStorage.CloseAsync();
        }

        /// <summary>
        /// 测试点击命令
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestPoetryTappedCommand()
        {
            var contentNavigationServiceMock = new Mock<IContentNavigationService>();
            var mockContentNavigationService = contentNavigationServiceMock.Object;


            var poetryTapped = new Poetry();
            var resultPageViewModel = new ResultPageViewModel(null, mockContentNavigationService);
            await resultPageViewModel.PoetryTappedCommandFunction(poetryTapped);
            contentNavigationServiceMock.Verify(p => p.NavigateToAsync(ContentNavigationContenstants.DetailPage,poetryTapped),Times.Once);
        }
    }
}
