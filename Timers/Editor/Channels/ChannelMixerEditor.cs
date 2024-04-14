using System.Collections.Generic;
using System.Linq;
using Ayla.Timers.Editor.Utility;
using Ayla.Timers.Runtime.Channels;
using UnityEditor;
using UnityEngine;
using EditorUtility = Ayla.Timers.Editor.Utility.EditorUtility;

namespace Ayla.Timers.Editor.Channels
{
    [CustomEditor(typeof(ChannelMixer))]
    public class ChannelMixerEditor : UnityEditor.Editor
    {
        private readonly List<Channel> channels = new();
        private string[] namesCachedArray;
        private int namesIndex;
        private string createChildName;

        public override void OnInspectorGUI()
        {
            channels.Clear();

            if (target is ChannelMixer mixer)
            {
                void RenderChannelRecursive(int indentLevel, bool bold, bool canRemove, Channel channel)
                {
                    if (RenderChannel(indentLevel, bold, canRemove, channel))
                    {
                        mixer.RemoveChild(channel);
                        return;
                    }

                    channels.Add(channel);

                    foreach (var child in channel.children)
                    {
                        RenderChannelRecursive(indentLevel + 1, false, true, child);
                    }
                }

                RenderChannelRecursive(0, true, false, mixer.masterChannel);

                if (namesCachedArray == null || channels.Count != namesCachedArray.Length)
                {
                    namesCachedArray = channels.Select(p => p.name).ToArray();
                }
                else
                {
                    for (int i = 0; i < channels.Count; ++i)
                    {
                        namesCachedArray[i] = channels[i].name;
                    }
                }

                using (LayoutUtility.HorizontalScope())
                {
                    createChildName = EditorGUILayout.TextField(createChildName, GUILayout.Width(EditorGUIUtility.labelWidth));
                    if (GUILayout.Button("Create Child For") || EditorUtility.IsEnterKeyPressed())
                    {
                        var channel = channels[namesIndex];
                        mixer.CreateChildForAsset(channel, createChildName);
                    }
                    namesIndex = EditorGUILayout.Popup(namesIndex, namesCachedArray);
                }
            }
        }

        public static bool RenderChannel(int indentLevel, bool bold, bool canRemove, Channel channel)
        {
            using (LayoutUtility.HorizontalScope())
            {
                float labelWidth = EditorGUIUtility.labelWidth;
                float indentWidth = 11.0f * indentLevel;
                if (indentWidth > 0)
                {
                    GUILayout.Space(indentWidth);
                }

                GUIStyle skin = bold ? EditorStyles.boldLabel : EditorStyles.label;
                GUILayout.Label(channel.name, skin, GUILayout.Width(labelWidth - indentWidth));
                GUILayout.Label($"{channel.scale:F2} ({channel.scaleSelf:F2})", GUILayout.Width(80.0f));

                using (LayoutUtility.HorizontalScope())
                {
                    channel.scaleSelf = (float)EditorGUILayout.Slider((float)channel.scaleSelf, 0.0f, 10.0f);
                    if (Application.isPlaying)
                    {
                        GUILayout.Label(channel.time.ToString("F2"));
                        GUILayout.Label($"({channel.deltaTime:F2})");
                    }
                }

                if (canRemove)
                {
                    return GUILayout.Button("x", GUILayout.Width(40));
                }
                else
                {
                    return false;
                }
            }
        }
    }
}