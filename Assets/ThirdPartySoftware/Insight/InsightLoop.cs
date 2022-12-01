using System;
using UnityEngine;

#if UNITY_2019_3_OR_NEWER
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
#else
using UnityEngine.Experimental.LowLevel;
using UnityEngine.Experimental.PlayerLoop;
#endif

namespace Insight {
    //see NetworkLoop.cs
    public class InsightLoop {

        internal enum AddMode {
            Beginning, End
        }

        internal static int FindPlayerLoopEntryIndex(PlayerLoopSystem.UpdateFunction function, PlayerLoopSystem playerLoop, Type playerLoopSystemType)
        {
            if(playerLoop.type == playerLoopSystemType)
                return Array.FindIndex(playerLoop.subSystemList, (elem => elem.updateDelegate == function));

            if(playerLoop.subSystemList != null) {
                for(int i = 0; i < playerLoop.subSystemList.Length; ++i) {
                    int index = FindPlayerLoopEntryIndex(function, playerLoop.subSystemList[i], playerLoopSystemType);
                    if(index != -1)
                        return index;
                }
            }
            return -1;
        }

        internal static bool AddToPlayerLoop(PlayerLoopSystem.UpdateFunction function, Type ownerType, ref PlayerLoopSystem playerLoop, Type playerLoopSystemType, AddMode addMode) {
            if(playerLoop.type == playerLoopSystemType) {
                int oldListLength = (playerLoop.subSystemList != null) ? playerLoop.subSystemList.Length : 0;
                Array.Resize(ref playerLoop.subSystemList, oldListLength + 1);

                PlayerLoopSystem system = new PlayerLoopSystem {
                    type = ownerType,
                    updateDelegate = function
                };

                if(addMode == AddMode.Beginning) {
                    Array.Copy(playerLoop.subSystemList, 0, playerLoop.subSystemList, 1, playerLoop.subSystemList.Length - 1);
                    playerLoop.subSystemList[0] = system;

                } else if(addMode == AddMode.End) {
                    playerLoop.subSystemList[oldListLength] = system;
                }
                return true;
            }
            if(playerLoop.subSystemList != null) {
                for(int i = 0; i < playerLoop.subSystemList.Length; ++i) {
                    if(AddToPlayerLoop(function, ownerType, ref playerLoop.subSystemList[i], playerLoopSystemType, addMode))
                        return true;
                }
            }
            return false;
        }

        [RuntimeInitializeOnLoadMethod]
        static void RuntimeInitializeOnLoad() {
            Debug.Log("Insight: adding Network[Early/Late]Update to Unity...");

            PlayerLoopSystem playerLoop =
#if UNITY_2019_3_OR_NEWER
                PlayerLoop.GetCurrentPlayerLoop();
#else
                PlayerLoop.GetDefaultPlayerLoop();
#endif

            AddToPlayerLoop(NetworkEarlyUpdate, typeof(InsightLoop), ref playerLoop, typeof(EarlyUpdate), AddMode.End);

            AddToPlayerLoop(NetworkLateUpdate, typeof(InsightLoop), ref playerLoop, typeof(PreLateUpdate), AddMode.End);

            PlayerLoop.SetPlayerLoop(playerLoop);
        }

        static void NetworkEarlyUpdate() {
            if(InsightServer.instance) InsightServer.instance.NetworkEarlyUpdate();
            if(InsightClient.instance) InsightClient.instance.NetworkEarlyUpdate();
        }

        static void NetworkLateUpdate() {
            if(InsightServer.instance) InsightServer.instance.NetworkLateUpdate();
            if(InsightClient.instance) InsightClient.instance.NetworkLateUpdate();
        }
    }
}