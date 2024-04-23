using System;
using Ayla.Inspector.Editor;
using Ayla.Inspector.Meta;
using Ayla.Inspector.SpecialCase;
using Ayla.Inspector.Utilities;
using UnityEngine;

namespace Ayla.Timers.Runtime.Channels
{
    [Serializable]
    public struct ChannelBinder
    {
        [SerializeField]
        internal ChannelMixer m_Mixer;

        [SerializeField, HideInInspector]
        internal int m_ChannelIndex;

        public ChannelMixer mixer
        {
            get => m_Mixer;
            set => m_Mixer = value;
        }

        public int channelIndex
        {
            get => m_ChannelIndex;
            set => m_ChannelIndex = value;
        }

        public Channel channel => m_Mixer ? m_Mixer.GetChannel(m_ChannelIndex) : null;

#if UNITY_EDITOR
        [CustomInspector, OrderAfter(nameof(m_Mixer))]
        private bool OnInspectorGUI()
        {
            using var scope = Scopes.ChangedScope();

            if (m_Mixer)
            {
                string[] names = m_Mixer.channelNames;
                m_ChannelIndex = CustomInspectorLayout.Popup("Channel", m_ChannelIndex, names);

                using (Scopes.ChangedScope())
                {
                    var newChannel = (Channel)CustomInspectorLayout.ObjectField(string.Empty, channel, typeof(Channel), false);
                    if (GUI.changed)
                    {
                        m_ChannelIndex = m_Mixer.channels.FindIndex(p => p == newChannel);
                    }
                }
            }

            return GUI.changed;
        }
#endif
    }
}