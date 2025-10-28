using UnityEngine;

namespace NeoBonerooms.Mod.Utilities
{
    public class Timings
    {
        public static string FormatTime(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            return $"{minutes:00}:{seconds:00}";
        }
    }
}
