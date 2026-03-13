using UnityEngine;
using MelonLoader;

namespace BetterRacingBis
{
    public static class RaceUI
    {
        // UI Settings
        public static bool Visible = true;
        private static float panelHeight = 500f;
        private static float panelWidth = 350f;
        private static float y = (Screen.height - panelHeight) / 2f;
        // Configurable keys
        private static KeyCode _toggleUIKey = KeyCode.F1;
        private static KeyCode _restartKey = KeyCode.F2;

        // Rebinding state
        private static bool _rebindingToggle = false;
        private static bool _rebindingSecond = false;

        // Public accessors for the keys (use these elsewhere)
        public static KeyCode ToggleUIKey => _toggleUIKey;
        public static KeyCode SecondKey => _restartKey;


        // ── Textures ──────────────────────────────────────────────────
        private static Texture2D _texWindowBg;      // dark transparent panel
        private static Texture2D _texBtnNormal;     // button normal
        private static Texture2D _texBtnHover;      // button hover
        private static Texture2D _texBtnActive;     // button pressed
        private static bool _stylesBuilt;

        // ── Colours ───────────────────────────────────────────────────
        private static readonly Color Gold = new Color(1.00f, 0.84f, 0.00f, 1f);
        private static readonly Color GoldDim = new Color(0.90f, 0.75f, 0.00f, 1f);
        private static readonly Color PanelBg = new Color(0.05f, 0.05f, 0.05f, 0.85f);
        private static readonly Color BtnNormal = new Color(0.10f, 0.10f, 0.10f, 0.85f);
        private static readonly Color BtnHover = new Color(0.20f, 0.16f, 0.00f, 0.95f);
        private static readonly Color BtnActive = new Color(0.30f, 0.24f, 0.00f, 1.00f);

        // ── Styles ────────────────────────────────────────────────────
        private static GUIStyle _styleWindow;
        private static GUIStyle _styleButton;
        private static GUIStyle _styleRebind;
        private static GUIStyle _styleLabel;

        // ─────────────────────────────────────────────────────────────
        private static Texture2D MakeTex(int w, int h, Color c)
        {
            var tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
            tex.hideFlags = HideFlags.HideAndDontSave;

            var pix = new Color[w * h];
            for (int i = 0; i < pix.Length; i++) pix[i] = c;

            tex.SetPixels(pix);
            tex.Apply();

            return tex;
        }


        // Bordered texture: fill + 1-px gold frame
        private static Texture2D MakeBorderedTex(int w, int h, Color fill, Color border, int borderPx = 2)
        {
            var tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
            tex.hideFlags = HideFlags.HideAndDontSave;

            var pix = new Color[w * h];

            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    bool isBorder = x < borderPx || x >= w - borderPx ||
                                    y < borderPx || y >= h - borderPx;

                    pix[y * w + x] = isBorder ? border : fill;
                }

            tex.SetPixels(pix);
            tex.Apply();

            return tex;
        }
        private static void BuildStyles()
        {
            if (_stylesBuilt) return;
            _stylesBuilt = true;

            // ── Textures ──────────────────────────────────────────────
            _texWindowBg = MakeBorderedTex(4, 4, PanelBg, PanelBg, 2);
            _texBtnNormal = MakeBorderedTex(4, 4, BtnNormal, GoldDim, 1);
            _texBtnHover = MakeBorderedTex(4, 4, BtnHover, Gold, 1);
            _texBtnActive = MakeBorderedTex(4, 4, BtnActive, Gold, 1);

            // ── Window style ──────────────────────────────────────────
            _styleWindow = new GUIStyle(GUI.skin.window);
            _styleWindow.normal.background = _texWindowBg;
            _styleWindow.normal.textColor = Gold;
            _styleWindow.fontStyle = FontStyle.Bold;
            _styleWindow.fontSize = 13;

            // ── Main button style ─────────────────────────────────────
            _styleButton = new GUIStyle(GUI.skin.button);
            _styleButton.normal.background = _texBtnNormal;
            _styleButton.hover.background = _texBtnHover;
            _styleButton.active.background = _texBtnActive;
            _styleButton.normal.textColor = Gold;
            _styleButton.hover.textColor = Color.white;
            _styleButton.active.textColor = Color.white;
            _styleButton.fontStyle = FontStyle.Bold;
            _styleButton.fontSize = 12;
            _styleButton.fixedHeight = 30;
            _styleButton.margin = new RectOffset(0, 0, 3, 3);
            _styleButton.border = new RectOffset(1, 1, 1, 1);

            // ── Rebind button (slightly muted) ────────────────────────
            _styleRebind = new GUIStyle(_styleButton);
            _styleRebind.fontSize = 11;
            _styleRebind.fixedHeight = 24;
            _styleRebind.normal.textColor = GoldDim;
            _styleRebind.hover.textColor = Gold;

            // ── Label ─────────────────────────────────────────────────
            _styleLabel = new GUIStyle(GUI.skin.label);
            _styleLabel.normal.textColor = GoldDim;
            _styleLabel.fontStyle = FontStyle.Italic;
            _styleLabel.fontSize = 11;
            _styleLabel.alignment = TextAnchor.MiddleCenter;
        }

        public static void Update()
        {
            if (_rebindingToggle || _rebindingSecond)
            {
                foreach (KeyCode kc in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(kc))
                    {
                        if (_rebindingToggle) { _toggleUIKey = kc; _rebindingToggle = false; }
                        else { _restartKey = kc; _rebindingSecond = false; }
                        return;
                    }
                }
                return;
            }

            if (Input.GetKeyDown(_toggleUIKey)) Visible = !Visible;
            if (Input.GetKeyDown(_restartKey)) onRestartKeyPressed();
        }


        // Override or expand this method for your F2 usage
        private static void onRestartKeyPressed()
        {
            GeneralUtils.RestartRace();
        }

        public static void Draw()
        {
            if (!Visible) return;

            BuildStyles();
            float x = 20f;
            float y = (Screen.height - panelHeight) / 2f;
            GUILayout.BeginArea(
                new Rect(x, y, panelWidth, panelHeight),
                "Teleport Panel",
                _styleWindow);

            // ── Teleport buttons ──────────────────────────────────────
            if (GUILayout.Button("Red Race", _styleButton)) GeneralUtils.teleportRedRace();
            if (GUILayout.Button("Blue Race", _styleButton)) GeneralUtils.teleportBlueRace();
            if (GUILayout.Button("Reverse Race", _styleButton)) GeneralUtils.teleportReverseRace();
            if (GUILayout.Button("FOG Green/Blue Path", _styleButton)) GeneralUtils.teleportGreenBluePath();
            if (GUILayout.Button("FOG Orange/Red Path", _styleButton)) GeneralUtils.teleportOrangeRedPath();
            if (GUILayout.Button("FOG Pink Path", _styleButton)) GeneralUtils.teleportPingPath();
            if (GUILayout.Button("Custom Race", _styleButton)) GeneralUtils.teleportCustomRace();

            // ── Key bindings ──────────────────────────────────────────
            GUILayout.Space(8);
            GUILayout.Label("— Key Bindings —", _styleLabel);

            string toggleLabel = _rebindingToggle
                ? "▶ Press any key..."
                : $"Toggle UI: [{_toggleUIKey}]";
            if (GUILayout.Button(toggleLabel, _styleRebind))
            {
                _rebindingToggle = true;
                _rebindingSecond = false;
            }

            string secondLabel = _rebindingSecond
                ? "▶ Press any key..."
                : $"Quick Race Restart: [{_restartKey}]";
            if (GUILayout.Button(secondLabel, _styleRebind))
            {
                _rebindingSecond = true;
                _rebindingToggle = false;
            }

            GUILayout.EndArea();
        }

    }
}