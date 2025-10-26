using HarmonyLib;
using NeoBonerooms.Mod.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;
using UnityEngine;

namespace NeoBonerooms.Mod.Fixes
{

    [HarmonyPatch(typeof(scrGameControllerBonerooms), "teleportPlayerClientRpc")]
    public class FixTeleportRPC
    {
        public static bool Prefix(ulong playerID, Vector3 pos)
        {
            if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.ContainsKey(playerID))
            {
                Logger<NeoBackroomsPlugin>.Warning($"Player {playerID} not spawned yet. Skipping teleport");
                return false;
            }
            return true;
        }

        public static void Postfix(ulong playerID, Vector3 pos)
        {
            Logger<NeoBackroomsPlugin>.Info($"TeleportRPC called for {playerID} at {pos}");
        }
    }
}
