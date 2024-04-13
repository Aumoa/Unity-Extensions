using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Ayla.Timers.Runtime.Channels
{
    public class ChannelMixer : ScriptableObject
    {
        [SerializeField, HideInInspector]
        private Channel m_MasterChannel;

        private List<Channel> m_ChannelsCache;

        public Channel masterChannel => m_MasterChannel;

        private void Awake()
        {
            ChannelUpdateSystem.RegisterMixer(this);
        }

        public Channel[] channels
        {
            get
            {
                m_ChannelsCache ??= new();
                m_ChannelsCache.Clear();

                void AddRecursive(Channel channel)
                {
                    m_ChannelsCache.Add(channel);
                    foreach (var child in channel.children)
                    {
                        AddRecursive(child);
                    }
                }

                AddRecursive(m_MasterChannel);
                return m_ChannelsCache.ToArray();
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            var assetDir = AssetDatabase.GetAssetPath(this);
            var channels = AssetDatabase.LoadAllAssetsAtPath(assetDir)
                .Where(p => AssetDatabase.IsSubAsset(p))
                .OfType<Channel>()
                .ToArray();

            foreach (var channel in channels)
            {
                if (channel.parent == null || channel.parent.children.Contains(channel) == false)
                {
                    DestroyImmediate(channel, true);
                }
            }
        }

        public Channel CreateChildForAsset(Channel parent, string name)
        {
            var channel = CreateInstance<Channel>();
            channel.name = name;
            channel.SetParent(parent);
            EditorUtility.SetDirty(channel);
            EditorUtility.SetDirty(parent);
            AssetDatabase.AddObjectToAsset(channel, this);
            AssetDatabase.SaveAssets();
            return channel;
        }

        public void RemoveChild(Channel channel)
        {
            InternalRemoveChild(channel);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        private void InternalRemoveChild(Channel channel)
        {
            foreach (var child in channel.children)
            {
                RemoveChild(child);
            }

            channel.SetParent(null);
            DestroyImmediate(channel, true);
        }

        private void CreateMasterChannel()
        {
            m_MasterChannel = CreateInstance<Channel>();
            m_MasterChannel.name = "Master";
            m_MasterChannel.hideFlags |= HideFlags.HideInHierarchy;
        }

        [MenuItem("Assets/Create/Ayla/Timers/Timer Channel Mixer")]
        private static void CreateAsset()
        {
            var assetPath = GenerateAssetPath();
            var mixer = CreateInstance<ChannelMixer>();
            AssetDatabase.CreateAsset(mixer, assetPath);

            mixer.CreateMasterChannel();
            AssetDatabase.AddObjectToAsset(mixer.m_MasterChannel, mixer);
            AssetDatabase.SaveAssets();
        }

        private static string GenerateAssetPath()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }
            return AssetDatabase.GenerateUniqueAssetPath(path + "/ChannelMixer.asset");
        }
#endif
    }
}