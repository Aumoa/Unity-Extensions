using Ayla.Timers.Runtime.Channels;
using UnityEngine;
using UnityEngine.Playables;

namespace Ayla.Timers.Runtime.Playables
{
    [RequireComponent(typeof(PlayableDirector))]
    public class PlayableUpdater : MonoBehaviour
    {
        [SerializeField]
        private ChannelBinder m_ChannelBinder;

        private PlayableDirector m_PlayableDirector;

        public Channel channel => m_ChannelBinder.channel;

        private void Awake()
        {
            m_PlayableDirector = GetComponent<PlayableDirector>();
            m_PlayableDirector.timeUpdateMode = DirectorUpdateMode.UnscaledGameTime;
        }

        private void Update()
        {
            if (channel)
            {
                m_PlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(channel.scale);
            }
        }

        public void SetChannel(ChannelMixer mixer, int channelIndex)
        {
            m_ChannelBinder.mixer = mixer;
            m_ChannelBinder.channelIndex = channelIndex;
        }

        public void SetChannel(ChannelMixer mixer, string channelName)
        {
            SetChannel(mixer, mixer.channels.FindIndex(p => p.name == channelName));
        }
    }
}