using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Ayla.Inspector.Utilities
{
    internal static class ReflectionUtility
    {
        public static Func<TClass, TField> GetField<TClass, TField>(string name, BindingFlags bindingFlags)
        {
            var type = typeof(TClass);
            var fieldInfo = type.GetField(name, bindingFlags);
            if (fieldInfo == null)
            {
                return null;
            }

            var parameter = Expression.Parameter(type, "self");
            var field = Expression.Field(parameter, fieldInfo);
            return Expression.Lambda<Func<TClass, TField>>(field, parameter).Compile();
        }

        public static Action<TClass, TField> SetField<TClass, TField>(string name, BindingFlags bindingFlags)
        {
            var type = typeof(TClass);
            var fieldInfo = type.GetField(name, bindingFlags);
            if (fieldInfo == null)
            {
                return null;
            }

            var self = Expression.Parameter(type, "self");
            var value = Expression.Parameter(typeof(TField), "value");
            var field = Expression.Field(self, fieldInfo);
            var assign = Expression.Assign(field, value);
            return Expression.Lambda<Action<TClass, TField>>(assign, self, value).Compile();
        }
    }
}