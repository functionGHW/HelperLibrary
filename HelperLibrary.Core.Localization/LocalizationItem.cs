/* 
 * FileName:    LocalizationItem.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  1/21/2017 8:50:31 PM
 * Version:     v1.1
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary.Core.Localization
{
    /// <summary>
    /// 表示本地化字典词条项的类型
    /// </summary>
    public class LocalizationItem : IEquatable<LocalizationItem>
    {
        /// <summary>
        /// 创建新的字典项
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public LocalizationItem(string scope, string key, string value)
        {
            Key = key;
            Scope = scope;
            Value = value;
        }

        /// <summary>
        /// 获取 限定域
        /// </summary>
        public string Scope { get; private set; }

        /// <summary>
        /// 获取键
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// 获取本地化值
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// 确定两个本地化字典项是否是等价的字典项
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(LocalizationItem other)
        {
            /* 只有全部属性都相同才会认定为等价
             */

            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return string.Equals(Scope, other.Scope)
                && string.Equals(Key, other.Key)
                && string.Equals(Value, other.Value);
        }

        /// <summary>
        /// 确定指定示例是否和此字典项等价
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((LocalizationItem)obj);
        }

        /// <summary>
        /// 获取实例的哈希值
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Scope != null ? Scope.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Key != null ? Key.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Value != null ? Value.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
