using MelonLoader;
using UnityEngine;
using System.Reflection;
using System.IO;
using MelonLoader.Preferences;
using System.Collections.Generic;
using System.Linq;
using System;

namespace TreasurePodESP
{
    internal class Entry : MelonMod
    {
        private static AssetBundle _bundle;
        private const string bundleName = "treasurepodesp.unity3d";
        private const string espObjectName = "TreasurePodESPNode";
        private const string categoryName = "TreasurePodESP";
        protected static readonly string[] _colorProperties = {"_Color", "_OccludeColor", "_EmissionColor"};
        protected static GameObject _espPrefab;
        private static MelonPreferences_Category _category;
        private static MelonPreferences_Entry<string> _color;
        private static readonly Color markekraus = new Color(255f, 0, 188f);
        protected static Dictionary<string, Color> _colors = new (){
            {"green", Color.green},
            {"red", Color.red},
            {"blue", Color.blue},
            {"yellow", Color.yellow},
            {"white", Color.white},
            {"cyan", Color.cyan},
            {"magenta", Color.magenta},
            {"grey", Color.grey},
            {"markekraus", markekraus}
        };
        public override void OnInitializeMelon()
        {
            _category = MelonPreferences.CreateCategory(categoryName, "Treasure pod ESP Settings");
            _color = _category.CreateEntry<string>(
                "ESPColor",
                "green",
                "ESP Color",
                "Color for ESP. Default: 'green'. Valid: 'green', 'red', 'blue, 'yellow', 'white', 'cyan', 'magenta', 'grey', 'markekraus'",
                false,
                false,
                new ColorValidator("green", _colors.Keys.ToList())
            );
            var myAsm = Assembly.GetExecutingAssembly();
            byte[] assetBytes;
            using (var assetStream = myAsm.GetManifestResourceStream(bundleName))
            {
                using (var binaryReader = new BinaryReader(assetStream))
                {
                    assetBytes = binaryReader.ReadBytes((int)assetStream.Length);
                }
            }
            _bundle = AssetBundle.LoadFromMemory(assetBytes);
            _espPrefab = _bundle.LoadAsset(espObjectName).Cast<GameObject>();
            GameObject.DontDestroyOnLoad(_espPrefab);
            _espPrefab.hideFlags |= HideFlags.HideAndDontSave;
        }

        protected static Color GetColor() => _colors[_color.Value.ToLowerInvariant()];

        protected class ColorValidator : ValueValidator
        {
            private string defaultColor;
            HashSet<string> validColors;
            public ColorValidator(string DefaultColor, IList<string> ValidColors)
            {
                defaultColor = DefaultColor;
                validColors = ValidColors.ToHashSet(StringComparer.InvariantCultureIgnoreCase);
                validColors.Add(defaultColor);
            }
            public override object EnsureValid(object value)
            {
                if (validColors.TryGetValue((string)value, out var color))
                {
                    return color;
                }
                return defaultColor;
            }

            public override bool IsValid(object value)
            {
                return validColors.Contains((string)value);
            }
        }
    }
}
