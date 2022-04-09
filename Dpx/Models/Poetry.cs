using System;
using System.Collections.Generic;
using System.Text;

namespace Dpx.Models
{
    /// <summary>
    /// 诗词类
    /// [SQLite.Table("works")]特性，对应到数据库表works
    /// </summary>
    [SQLite.Table("works")]
    public class Poetry
    {
        
        /// <summary>
        /// 主键  映射表中是id
        /// </summary>
        /// <summary>
        /// 
        /// </summary>
        //
        [SQLite.Column("id")] public int Id { get; set; }
        //标题
        [SQLite.Column("name")] public string Name { get; set; }
        [SQLite.Column("author_name")] public string AuthorName { get; set; }
        [SQLite.Column("dynasty")] public string Dynasty { get; set; }
        [SQLite.Column("content")] public string Content { get; set; }
        [SQLite.Column("translation")] public string Translation { get; set; }
        [SQLite.Column("layout")] public string Layout { get; set; }

        /// <summary>
        /// 居中布局
        /// </summary>
        public const string CenterLayout = "center";

        public const string IndentLayout = "indent";

        /// <summary>
        /// 预览存储变量
         /// </summary>
        private string _snippet;

        public string Snippet => _snippet ??
                                 (_snippet = Content.Split('。')[0].Replace("\r\n", " "));
    }
}
