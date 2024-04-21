using Ayla.Timers.Runtime.Channels;
using UnityEngine;

namespace Ayla.Timers.Runtime.Animators
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorChannelUpdater : MonoBehaviour
    {
        [SerializeField]
        private ChannelBinder m_ChannelBinder;

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
            m_ChannelBinder.mixer = mixer;
            m_ChannelBinder.channelIndex = channelIndex;

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

            m_Channel = m_ChannelBinder.channel;
        }
#endif
    }
}