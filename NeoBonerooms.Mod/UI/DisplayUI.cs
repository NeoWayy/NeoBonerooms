using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using NeoBonerooms.Mod.Patches;
using NeoBonerooms.Mod.Features;

public class DisplayUI : MonoBehaviour
{
    public List<string> lines = new();

    private bool _isVisible = true;
    private float _alpha = 1f;
    private float _slideProgress = 1f;
    private float _fadeSpeed = 2f;
    private float _slideSpeed = 3f;

    private GUIStyle _boxStyle;
    private GUIStyle _labelStyle;
    private GUIStyle _buttonStyle;
    private Texture2D _backgroundTexture;

    public void Update()
    {
        if (Keyboard.current.f5Key.wasPressedThisFrame)
        {
            _isVisible = !_isVisible;
        }

        float targetAlpha = _isVisible ? 1f : 0f;
        _alpha = Mathf.MoveTowards(_alpha, targetAlpha, _fadeSpeed * Time.deltaTime);

        float targetSlide = _isVisible ? 1f : 0f;
        _slideProgress = Mathf.MoveTowards(_slideProgress, targetSlide, _slideSpeed * Time.deltaTime);
    }

    public void OnGUI()
    {
        if (_alpha <= 0f) return;

        InitStyles();

        Color originalColor = GUI.color;
        GUI.color = new Color(1f, 1f, 1f, _alpha);

        float lineHeight = _labelStyle.lineHeight + 4f;
        float boxWidth = 360f;
        float boxHeight = lines.Count * lineHeight + _boxStyle.padding.top + _boxStyle.padding.bottom + 60f;

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

        yOffset += 10f;

        Rect staminaButtonRect = new Rect(boxRect.x + _boxStyle.padding.left, yOffset, 160, lineHeight + 10);
        string staminaText = infiniteStaminaPatch.Enabled ? "Infinite Stamina: ON" : "Infinite Stamina: OFF";
        Color staminaColor = infiniteStaminaPatch.Enabled ? new Color(0.2f, 0.8f, 0.2f) : new Color(0.6f, 0.2f, 0.2f);

        _buttonStyle.normal.background = MakeColoredTexture(staminaColor);
        if (GUI.Button(staminaButtonRect, staminaText, _buttonStyle))
        {
            infiniteStaminaPatch.Enabled = !infiniteStaminaPatch.Enabled;
        }

        Rect espButtonRect = new Rect(staminaButtonRect.xMax + 10, yOffset, 160, lineHeight + 10);
        string espText = DotESP.enabled ? "DotESP: ON" : "DotESP: OFF";
        Color espColor = DotESP.enabled ? new Color(0.2f, 0.6f, 0.8f) : new Color(0.3f, 0.3f, 0.3f);

        _buttonStyle.normal.background = MakeColoredTexture(espColor);
        if (GUI.Button(espButtonRect, espText, _buttonStyle))
        {
            DotESP.enabled = !DotESP.enabled;
        }

        GUI.color = originalColor;
    }

    private void InitStyles()
    {
        if (_boxStyle != null) return;

        _backgroundTexture = new Texture2D(1, 1);
        _backgroundTexture.SetPixel(0, 0, new Color(0.15f, 0.15f, 0.15f, 0.9f));
        _backgroundTexture.Apply();

        _boxStyle = new GUIStyle(GUI.skin.box)
        {
            padding = new RectOffset(15, 15, 15, 15),
            margin = new RectOffset(10, 10, 10, 10),
            normal = { background = _backgroundTexture }
        };

        _labelStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 16,
            fontStyle = FontStyle.Bold,
            normal = { textColor = Color.white }
        };

        _buttonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 14,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = Color.white }
        };
    }

    private Texture2D MakeColoredTexture(Color color)
    {
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, color);
        tex.Apply();
        return tex;
    }

    public void OnDestroy()
    {
        if (_backgroundTexture != null) Destroy(_backgroundTexture);
    }
}