using RF.Modules.UIElements.RF.Modules.UIElements.Models;
using System;
using System.Linq.Expressions;

namespace RF.Modules.UIElements.RF.Modules.UIElements.Util
{
    internal static class SettingsUtil
    {
        public static string GetKey<TModel, TProperty>(
            this TModel _,
            Expression<Func<TModel, TProperty>> expression
            )
            where TModel : SettingsModel
            => $"RF.Modules.UIElements.Settings.{typeof(TModel).Name}.{(expression as MemberExpression)?.Member.Name}";
    }
}