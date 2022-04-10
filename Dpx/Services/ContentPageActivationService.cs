using System;
using System.Collections.Generic;
using System.Text;
using Dpx.Views;
using Xamarin.Forms;

namespace Dpx.Services
{
    /// <summary>
    /// 内容页激活服务实现
    /// </summary>
    public class ContentPageActivationService : IContentPageActivationService
    {
        //******** 私有变量
        /// <summary>
        /// 页面缓存
        /// </summary>
        private Dictionary<string, ContentPage> cache = new Dictionary<string, ContentPage>();
        //******** 继承方法

        /// <summary>
        /// 激活内容页
        /// </summary>
        /// <param name="pageKey">内容页</param>
        /// <returns></returns>
        public ContentPage Activate(string pageKey)
        => cache.ContainsKey(pageKey)?cache[pageKey]: cache[pageKey]=(ContentPage)Activator.CreateInstance(
            ContentNavigationContenstants.PageKeyTypeDictionary[pageKey]);//语法糖
        //{
        //    if (cache.ContainsKey(pageKey))
        //    {
        //        return cache[pageKey];
        //    }

        //    //反射 根据类型 创建类型实例，不需要new
        //    cache[pageKey] = (ContentPage)Activator.CreateInstance(
        //        ContentNavigationContenstants.PageKeyTypeDictionary[pageKey]);
        //    return cache[pageKey];
        //}
    }
}
