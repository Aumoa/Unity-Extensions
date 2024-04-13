using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace Ayla.Timers.Runtime.Channels
{
    public static class ChannelUpdateSystem
    {
        private static readonly HashSet<ChannelMixer> s_Mixers = new();

        private struct TimingUpdateRunner
        {
            public static void DispatchUpdateChannels()
            {
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

            s_Mixers.RemoveWhere(p => p == null);
        }

        public static void RegisterMixer(ChannelMixer mixer)
        {
            s_Mixers.Add(mixer);
        }
    }
}