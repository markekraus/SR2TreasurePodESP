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
            var pod = __instance.gameObject;
            var esp = GameObject.Instantiate(_espPrefab, pod.transform.position + Vector3.down, Quaternion.identity);
            var material = esp.GetComponent<MeshRenderer>().material;
            foreach (var prop in _colorProperties)
            {
                material.SetColor(prop, GetColor());
            }
        }
    }
}