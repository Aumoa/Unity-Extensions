using Ayla.Timers.Runtime.Channels;
using UnityEngine;

namespace Ayla.Timers.Runtime.Animators
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorChannelUpdater : MonoBehaviour
    {
        [SerializeField]
        private ChannelMixer m_Mixer;

        [SerializeField]
        private int m_MixerChannelIndex;

        private Animator m_Animator;
        private Channel m_Channel;

        public Channel channel => m_Channel;

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_Animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            RebindChannel();
        }

        private void Update()
        {
            if (m_Channel != null)
            {
                m_Animator.speed = (float)m_Channel.scale;
            }
        }

        public void SetChannel(ChannelMixer mixer, int channelIndex)
        {
            m_Mixer = mixer;
            m_MixerChannelIndex = channelIndex;

            if (didAwake)
            {
                RebindChannel();
            }
        }

        public void SetChannel(ChannelMixer mixer, string channelName)
        {
            SetChannel(mixer, mixer.channels.FindIndex(p => p.name == channelName));
        }

#if UNITY_EDITOR
        public void RebindChannel()
        {
            if (didAwake == false)
            {
                return;
            }

            var channels = m_Mixer ? m_Mixer.channels : null;
            var length = channels?.Length ?? 0;
            if (m_MixerChannelIndex >= 0 && m_MixerChannelIndex < length)
            {
                m_Channel = m_Mixer.channels[m_MixerChannelIndex];
            }
            else
            {
                m_Channel = null;
            }
        }
#endif
    }
}