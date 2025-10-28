using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NeoBonerooms.Mod.Features
{
    public class DotESP : MonoBehaviour
    {
        public static bool enabled = true;
        private Camera mainCamera;

        void Start()
        {
            mainCamera = Camera.main;
        }

        void Update()
        {
            if (Keyboard.current.f2Key.wasPressedThisFrame)
                enabled = !enabled;

            if (Camera.main == null)
                return;
            if (scrGameControl.Instance == null)
                return;
        }

        void OnGUI()
        {
            if (!enabled) return;
            if (mainCamera == null)
                mainCamera = Camera.main;

            var control = scrGameControl.Instance;
            if (control == null || control.gameBoneroomsScript == null)
                return;

            List<scrDot> dots = control.gameBoneroomsScript.dotList;
            if (dots == null) return;

            GUI.color = Color.green;

            foreach (scrDot dot in dots)
            {
                if (dot == null || dot.dotQuad == null) continue;

                if (!dot.dotQuad.activeSelf)
                    continue;

                Vector3 worldPos = dot.transform.position;
                Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);

                if (screenPos.z > 0)
                {
                    float y = Screen.height - screenPos.y;
                    GUI.Label(new Rect(screenPos.x - 20, y - 10, 40, 20), "● DOT");
                }
            }
        }
    }
}
