using HarmonyLib;
using NeoBonerooms.Mod.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.InputSystem;

namespace NeoBonerooms.Mod.Patches
{
    public class PlayerPatches
    {
        public PlayerPatches()
        {
            Logger<NeoBackroomsPlugin>.Info("Loaded PlayerPatches");
        }
    }

    [HarmonyPatch(typeof(scrPlayer), "Update")]
    public class infiniteStaminaPatch
    {
        #region Infinite Stamina
        private static readonly AccessTools.FieldRef<scrPlayer, float> staminaField =
            AccessTools.FieldRefAccess<scrPlayer, float>("stamina");
        private static readonly AccessTools.FieldRef<scrPlayer, float> maxStaminaField =
            AccessTools.FieldRefAccess<scrPlayer, float>("maxStamina");
        private static readonly AccessTools.FieldRef<scrPlayer, float> staminaDrainRateField =
            AccessTools.FieldRefAccess<scrPlayer, float>("staminaDrainRate");
        private static readonly AccessTools.FieldRef<scrPlayer, float> staminaRechargeRateField =
            AccessTools.FieldRefAccess<scrPlayer, float>("staminaRechargeRate");

        public static bool Enabled { get; set; } = true;

        public static void Postfix(scrPlayer __instance)
        {
            if (!Enabled)
                return;

            staminaDrainRateField(__instance) = 0f;

            staminaField(__instance) = maxStaminaField(__instance);
        }

        private static bool IsDraining(scrPlayer player)
        {
            return Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed;
        }
        #endregion
    }
}
