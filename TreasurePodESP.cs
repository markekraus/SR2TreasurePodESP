using Il2Cpp;
using MelonLoader;
using UnityEngine;
using System.Reflection;
using System.IO;

namespace TreasurePodESP
{
    internal class Entry : MelonMod
    {
        public static AssetBundle _bundle;
        private const string bundleName = "treasurepodesp.unity3d";
        protected const string materialPath = "treasurePod";
        protected static readonly string[] _colorProperties = {"_Color", "_OccludeColor", "_EmissionColor"};
        public static Shader _shader;
        public override void OnInitializeMelon()
        {
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
            foreach (var asset in _bundle.LoadAllAssets())
            {
                _shader = asset.Cast<Shader>();
                Object.DontDestroyOnLoad(_shader);
                _shader.hideFlags |= HideFlags.HideAndDontSave;
            }
        }
    }
}
