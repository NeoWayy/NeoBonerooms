using HarmonyLib;
using NeoBonerooms.Mod.Utilities;
using System;
using UnityEngine;

namespace NeoBonerooms.Mod.Fixes
{
    [HarmonyPatch(typeof(scrSoundMuffler), "Update")]
    public class FixSoundMuffler
    {
        public static bool Prefix(scrSoundMuffler __instance)
        {
            try
            {
                if (__instance.audioSource == null || __instance.audioSource.Length == 0)
                    return false;

                var occlusionCheckCDField = AccessTools.Field(typeof(scrSoundMuffler), "occlusionCheckCD");
                var occlusionCheckFrequencyField = AccessTools.Field(typeof(scrSoundMuffler), "occlusionCheckFrequency");
                float occlusionCheckCD = (float)occlusionCheckCDField.GetValue(__instance);
                float occlusionCheckFrequency = (float)occlusionCheckFrequencyField.GetValue(__instance);

                occlusionCheckCD -= Time.deltaTime * 60f;
                if (occlusionCheckCD > 0f)
                {
                    occlusionCheckCDField.SetValue(__instance, occlusionCheckCD);
                    return false;
                }

                occlusionCheckCD = occlusionCheckFrequency;
                occlusionCheckCDField.SetValue(__instance, occlusionCheckCD);

                Camera cam = Camera.main;
                if (cam == null)
                {
                    Logger<NeoBackroomsPlugin>.Warning($"scrSoundMuffler.Update: No main camera found!");
                    return false;
                }

                if (scrGameControl.Instance == null)
                {
                    Logger<NeoBackroomsPlugin>.Warning($"scrSoundMuffler.Update: scrGameControl.Instance is null!");
                    return false;
                }

                var mixerGroups = scrGameControl.Instance.mixerGroups;
                if (mixerGroups == null || mixerGroups.Length < 3)
                {
                    Logger<NeoBackroomsPlugin>.Warning($"scrSoundMuffler.Update: mixerGroups missing or too small!");
                    return false;
                }

                Vector3 direction = cam.transform.position - __instance.transform.position;
                float distance = direction.magnitude;
                int hitCount = Physics.RaycastAll(__instance.transform.position, direction, distance, __instance.occludesAudio).Length;

                for (int i = 0; i < __instance.audioSource.Length; i++)
                {
                    if (__instance.audioSource[i] == null)
                    {
                        Logger<NeoBackroomsPlugin>.Warning($"scrSoundMuffler.Update: Null audioSource at index {1}");
                        continue;
                    }

                    __instance.audioSource[i].outputAudioMixerGroup = mixerGroups[Mathf.Clamp(hitCount, 0, 2)];
                }

                return false;
            }
            catch (Exception e)
            {
                Logger<NeoBackroomsPlugin>.Error($"scrSoundMuffler.Update patch error: {e}");
                return false;
            }
        }
    }
}
