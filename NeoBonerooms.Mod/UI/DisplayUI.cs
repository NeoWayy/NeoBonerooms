using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using NeoBonerooms.Mod.Patches;
using NeoBonerooms.Mod.Utilities;

public class DisplayUI : MonoBehaviour
{
    public List<string> lines = new();

    // Menu Settings
    private bool _isVisible = false;
    private float _alpha = 1f;
    private float _slideProgress = 1f;
    private float _fadeSpeed = 2f;
    private float _slideSpeed = 3f;

    private GUIStyle _boxStyle;
    private GUIStyle _labelStyle;
    private Texture2D _backgroundTexture;

    public void Update()
    {
        if (Keyboard.current.f5Key.wasPressedThisFrame)
        {
            _isVisible = !_isVisible;
            Debug.Log("Toggled GUI: " + _isVisible);
        }

        // Smooth fade
        float targetAlpha = _isVisible ? 1f : 0f;
        _alpha = Mathf.MoveTowards(_alpha, targetAlpha, _fadeSpeed * Time.deltaTime);

        // Smooth slide
        float targetSlide = _isVisible ? 1f : 0f;
        _slideProgress = Mathf.MoveTowards(_slideProgress, targetSlide, _slideSpeed * Time.deltaTime);
    }

    public void OnGUI()
    {
        if (_alpha > 0f)
        {
            if (_boxStyle == null)
            {
                _labelStyle = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 18,
                    normal = { textColor = Color.white }
                };

                _boxStyle = new GUIStyle(GUI.skin.box);
                _backgroundTexture = new Texture2D(1, 1);
                _backgroundTexture.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.5f));
                _backgroundTexture.Apply();

                _boxStyle.normal.background = _backgroundTexture;
                _boxStyle.padding = new RectOffset(15, 15, 15, 15);
                _boxStyle.margin = new RectOffset(10, 10, 10, 10);
            }

            if (lines.Count == 0) return;

            Color originalColor = GUI.color;
            GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, _alpha);

            float lineHeight = _labelStyle.lineHeight + 4f;
            float boxWidth = 350f;
            float boxHeight = lines.Count * lineHeight + _boxStyle.padding.top + _boxStyle.padding.bottom;

            float startX = -boxWidth;
            float targetX = 20f;
            float currentX = Mathf.Lerp(startX, targetX, _slideProgress);

            Rect boxRect = new Rect(currentX, 20f, boxWidth, boxHeight);
            GUI.Box(boxRect, GUIContent.none, _boxStyle);

            float yOffset = boxRect.y + _boxStyle.padding.top;
            foreach (string line in lines)
            {
                Rect labelRect = new Rect(boxRect.x + _boxStyle.padding.left, yOffset, boxWidth - 30, lineHeight);
                GUI.Label(labelRect, line, _labelStyle);
                yOffset += lineHeight;
            }

            Rect buttonRect = new Rect(boxRect.x + _boxStyle.padding.left, yOffset, 150, lineHeight + 4);
            string buttonText = infiniteStaminaPatch.Enabled ? "Infinite Stamina: ON" : "Infinite Stamina: OFF";
            GUI.backgroundColor = infiniteStaminaPatch.Enabled ? Color.green : Color.red;

            if (GUI.Button(buttonRect, buttonText))
            {
                infiniteStaminaPatch.Enabled = !infiniteStaminaPatch.Enabled;
            }

            GUI.color = originalColor;
        }
    }

    public void OnDestroy()
    {
        if (_backgroundTexture != null)
        {
            Destroy(_backgroundTexture);
        }
    }
}
