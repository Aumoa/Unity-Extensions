using System;
using System.Linq.Expressions;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Ayla.Conversations
{
    internal static class Internals
    {
        public static class Impl
        {
            public static Func<string, GUIContent> s_TextContent = CompileTextContent();

            private static Func<string, GUIContent> CompileTextContent()
            {
                var type = typeof(EditorGUIUtility);
                var method = type.GetMethod("TextContent", BindingFlags.Static | BindingFlags.NonPublic);

                var textAndTooltip = Expression.Parameter(typeof(string), "textAndTooltip");
                var call = Expression.Call(null, method, textAndTooltip);

                var lambda = Expression.Lambda<Func<string, GUIContent>>(call, textAndTooltip);
                return lambda.Compile();
            }

            public static Func<GUIStyle, GUIContent, Vector2, Vector2> s_CalcSizeWithConstraints = CompileCalcSizeWithConstraints();

            private static Func<GUIStyle, GUIContent, Vector2, Vector2> CompileCalcSizeWithConstraints()
            {
                var type = typeof(GUIStyle);
                var method = type.GetMethod("CalcSizeWithConstraints", BindingFlags.Instance | BindingFlags.NonPublic);

                var @this = Expression.Parameter(typeof(GUIStyle), "this");
                var content = Expression.Parameter(typeof(GUIContent), "content");
                var constraints = Expression.Parameter(typeof(Vector2), "constraints");
                var call = Expression.Call(@this, method, content, constraints);

                var lambda = Expression.Lambda<Func<GUIStyle, GUIContent, Vector2, Vector2>>(call, @this, content, constraints);
                return lambda.Compile();
            }
        }

        public static GUIContent TextContent(string textAndTooltip)
        {
            return Impl.s_TextContent(textAndTooltip);
        }

        public static Vector2 CalcSizeWithConstraints(this GUIStyle @this, GUIContent content, Vector2 constraints)
        {
            return Impl.s_CalcSizeWithConstraints(@this, content, constraints);
        }
    }
}