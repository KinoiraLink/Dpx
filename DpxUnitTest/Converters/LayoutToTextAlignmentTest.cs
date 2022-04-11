using System;
using System.Collections.Generic;
using System.Text;
using Dpx.Converters;
using Dpx.Models;
using Dpx.Utils;
using NUnit.Framework;
using Xamarin.Forms;

namespace DpxUnitTest.Converters
{
    /// <summary>
    /// 布局到文本转换器测试
    /// </summary>
    public class LayoutToTextAlignmentTest
    {
        [Test]
        public void TestConverterBack()
        {
            var layoutToTextAlignment = new LayoutToTextAlignment();
            Assert.Catch<DoNotCallMeException>(()=>layoutToTextAlignment.ConvertBack(null,null,null,null));
        }

        [Test]
        public void TestConverter()
        {
            var layoutToTextAlignment = new LayoutToTextAlignment();
            Assert.AreEqual(TextAlignment.Center,layoutToTextAlignment.Convert(Poetry.CenterLayout,null,null,null));
            Assert.AreEqual(TextAlignment.Start, layoutToTextAlignment.Convert(Poetry.IndentLayout, null, null, null));
            Assert.AreEqual(null, layoutToTextAlignment.Convert(null, null, null, null));
            Assert.AreEqual(null, layoutToTextAlignment.Convert(string.Empty, null, null, null));
        }
    }
}
