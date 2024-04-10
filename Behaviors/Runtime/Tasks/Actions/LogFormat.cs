using System;
using System.Linq;
using Ayla.Inspector.Meta;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ayla.Behaviors.Runtime.Tasks.Actions
{
    [AddComponentMenu("Behaviors/Actions/Log Format")]
    public class LogFormat : Action
    {
        public enum ArgumentType
        {
            Int = 0,
            Single = 1,
            String = 2,
            Boolean = 3,
            UnityObject = 4
        }

        public enum LogVerbosity
        {
            Info,
            Warning,
            Error,
            Assertion
        }

        [Serializable]
        public struct FormatArgument
        {
            public ArgumentType type;

            [ShowIf(nameof(type), ArgumentType.Int)]
            public int intValue;

            [ShowIf(nameof(type), ArgumentType.Single)]
            public float floatValue;

            [ShowIf(nameof(type), ArgumentType.String)]
            public string stringValue;

            [ShowIf(nameof(type), ArgumentType.Boolean)]
            public bool boolValue;

            [ShowIf(nameof(type), ArgumentType.UnityObject)]
            public Object unityObject;

            public object GetObject()
            {
                return type switch
                {
                    ArgumentType.Int => intValue,
                    ArgumentType.Single => floatValue,
                    ArgumentType.String => stringValue,
                    ArgumentType.Boolean => boolValue,
                    ArgumentType.UnityObject => unityObject,
                    _ => throw new InvalidOperationException($"Invalid argument type({type}) detected.")
                };
            }
        }

        [SerializeField]
        private string format;

        [SerializeField]
        private FormatArgument[] arguments;

        [SerializeField]
        private LogVerbosity verbosity = LogVerbosity.Info;

        private object[] cachedArguments;

        protected override void OnStandby()
        {
            ValidateAndCache();
            Debug.LogFormat("OnStandby()");
        }

        protected override TaskStatus OnUpdate()
        {
            switch (verbosity)
            {
                case LogVerbosity.Info:
                    Debug.LogFormat(format, cachedArguments);
                    break;
                case LogVerbosity.Warning:
                    Debug.LogWarningFormat(format, cachedArguments);
                    break;
                case LogVerbosity.Error:
                    Debug.LogErrorFormat(format, cachedArguments);
                    break;
                case LogVerbosity.Assertion:
                    Debug.LogAssertionFormat(format, cachedArguments);
                    break;
            }

            return TaskStatus.Success;
        }

        private void ValidateAndCache()
        {
            cachedArguments = arguments.Select(p => p.GetObject()).ToArray();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            ValidateAndCache();
        }
#endif
    }
}
