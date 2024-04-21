using System;
using System.Linq;
using Ayla.Inspector.Runtime.Utilities;
using Ayla.Inspector.SpecialCase;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ayla.Timers.Runtime.Channels
{
    [Serializable]
    public struct ChannelBinder
    {
        [SerializeField, HideInInspector]
        internal ChannelMixer m_Mixer;

        [SerializeField, HideInInspector]
        internal int m_ChannelIndex;

        public ChannelMixer mixer { get; set; }

        public Channel channel => m_Mixer ? m_Mixer.GetChannel(m_ChannelIndex) : null;

#if UNITY_EDITOR
        private float inspectorHeight => PopulateInspectorRenderCommands(false, out _);

        [CustomInspector(nameof(inspectorHeight))]
        private bool OnInspectorGUI()
        {
            PopulateInspectorRenderCommands(true, out bool isModified);
            return isModified;
        }

        private float PopulateInspectorRenderCommands(bool doRender, out bool isModified)
        {
            float spacing = 0;

            T ObjectField<T>(string label, T unityObject) where T : Object
            {
                spacing += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                if (doRender)
                {
                    return (T)EditorGUILayout.ObjectField(label, unityObject, typeof(T), allowSceneObjects: false);
                }
                return unityObject;
            }

            GUI.changed = false;

            m_Mixer = ObjectField("Mixer", m_Mixer);
            if (m_Mixer)
            {
                spacing += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                string[] names = m_Mixer.channelNames;
                if (doRender)
                {
                    m_ChannelIndex = EditorGUILayout.Popup("Channel", m_ChannelIndex, names);
                }

                using (Scopes.ChangedScope())
                {
                    Channel newChannel = ObjectField(string.Empty, channel);
                    if (GUI.changed)
                    {
                        m_ChannelIndex = m_Mixer.channels.FindIndex(p => p == newChannel);
                    }
                }
            }

            isModified = GUI.changed;
            return spacing;
        }
#endif
    }
}