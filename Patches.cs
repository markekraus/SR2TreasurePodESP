using System.Reflection;
using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace TreasurePodESP
{
    [HarmonyPatch(typeof(TreasurePod), nameof(TreasurePod.Awake))]
    internal class TreasurePodAwakePatch : Entry
    {
        public static void Postfix(TreasurePod __instance)
        {
            var pod = __instance.gameObject.transform.Find(materialPath).gameObject.GetComponent<SkinnedMeshRenderer>();
            pod.material.shader = _shader;
            // foreach (var prop in _colorProperties)
            // {
            //     pod.material.SetColor(prop, Color.green);
            // }
        }
    }
}