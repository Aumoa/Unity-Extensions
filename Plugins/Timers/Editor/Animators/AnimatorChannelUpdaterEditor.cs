using System.Linq;
using Ayla.Timers.Editor.Channels;
using Ayla.Timers.Runtime.Animators;
using Ayla.Timers.Runtime.Channels;
using UnityEditor;
using UnityEngine;

namespace Ayla.Timers.Editor.Animators
{
    [CustomEditor(typeof(AnimatorChannelUpdater))]
    public class AnimatorChannelUpdaterEditor : UnityEditor.Editor
    {
        private SerializedProperty m_Mixer;
        private SerializedProperty m_MixerChannelIndex;

        private void OnEnable()
        {
            m_Mixer = serializedObject.FindProperty(nameof(m_Mixer));
            m_MixerChannelIndex = serializedObject.FindProperty(nameof(m_MixerChannelIndex));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(m_Mixer);

            bool needRebind = false;
            if (m_Mixer.objectReferenceValue is ChannelMixer mixer)
            {
                GUI.changed = false;
                string[] names = mixer.channels.Select(p => p.name).ToArray();
                m_MixerChannelIndex.intValue = EditorGUILayout.Popup("Channel", m_MixerChannelIndex.intValue, names);
                if (GUI.changed)
                {
                    needRebind = true;
                    GUI.changed = false;
                }

                GUILayout.Space(8);

                GUILayout.Label("Current channel");

                var channels = mixer.channels;
                var index = m_MixerChannelIndex.intValue;
                Channel channel = null;
                if (index >= 0 && index < channels.Length)
                {
                    channel = channels[index];
                }

                if (channel == null)
                {
                    GUILayout.Label("None");
                }
                else
                {
                    ChannelMixerEditor.RenderChannel(0, true, false, channel);
                }
            }

            serializedObject.ApplyModifiedProperties();
            if (needRebind && target is AnimatorChannelUpdater animatorUpdater)
            {
                animatorUpdater.RebindChannel();
            }
        }
    }
}