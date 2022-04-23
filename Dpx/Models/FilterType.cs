using System;
using System.Collections.Generic;
using System.Text;

namespace Dpx.Models
{
    /// <summary>
    /// 条件类型
    /// </summary>
    public class FilterType
    {
        /// <summary>
        /// 类型名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 属性名
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 标题条件
        /// </summary>
        public static readonly FilterType NameFilter = new FilterType
        {
            Name = "标题",
            PropertyName = nameof(Poetry.Name),
        };

        /// <summary>
        /// 作者条件
        /// </summary>
        public static readonly FilterType AuthorFilter = new FilterType
        {
            Name = "作者",
            PropertyName = nameof(Poetry.AuthorName),
        };

        /// <summary>
        /// 正文条件
        /// </summary>
        public static readonly FilterType ContentFilter = new FilterType
        {
            Name = "正文",
            PropertyName = nameof(Poetry.Content),
        };


        /// <summary>
        /// 条件类型数组
        /// </summary>
        public static List<FilterType> FilterTypes { get; } =
            new List<FilterType>
            {
                NameFilter, AuthorFilter, ContentFilter
            };
    }
}