using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JHashimoto.Repositories.Database {

    public enum DbProviderTypesEnum {
        [DbProviderInvariantName("Microsoft.Data.SqlClient")]
        SqlServer = 1
    }

    public static class DbProviderTypesExtensions {
        public static string GetProperName<T>(this T Value) where T : Enum {
            FieldInfo fieldInfo = Value.GetType().GetField(Value.ToString());

            // TODO:EnumExtensionsに移行
            // TODO:Exceptionの派生クラスを実装
            var attr = fieldInfo?.GetCustomAttributes(typeof(DbProviderInvariantNameAttribute), false).Cast<DbProviderInvariantNameAttribute>().FirstOrDefault()
                        ?? throw new Exception("指定された文字列に合致するEnumメンバーはありません。");

            return attr?.InvariantName ?? throw new Exception($"指定されたEnumメンバーに{nameof(DbProviderInvariantNameAttribute)}がありません。");
        }
    }
}
