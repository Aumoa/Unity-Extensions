using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace Ayla.Timers.Runtime.Channels
{
    public static class ChannelUpdateSystem
    {
        private static readonly HashSet<ChannelMixerUpdater> s_MixerUpdaters = new();

        private struct TimingUpdateRunner
        {
            public static void DispatchUpdateChannels()
            {
                foreach (var component in s_MixerUpdaters)
                {
                    component.DispatchUpdateChannel();
                }
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void InitializeComponents()
        {
            // Insert loop system for update timer.
            var playerLoop = PlayerLoop.GetCurrentPlayerLoop();
            ref var subsystem = ref playerLoop.subSystemList.FindRef(p => p.type == typeof(TimeUpdate));
            ArrayExtensions.AddRef(ref subsystem.subSystemList, new PlayerLoopSystem
            {
                type = typeof(TimingUpdateRunner),
                updateDelegate = TimingUpdateRunner.DispatchUpdateChannels
            });
            PlayerLoop.SetPlayerLoop(playerLoop);

            s_MixerUpdaters.RemoveWhere(p => p == null);
        }

        internal static void RegisterUpdater(ChannelMixerUpdater component)
        {
            s_MixerUpdaters.Add(component);
        }

        internal static void UnregisterUpdater(ChannelMixerUpdater component)
        {
            s_MixerUpdaters.Remove(component);
        }
    }
}