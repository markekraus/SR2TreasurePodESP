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
            if(__instance.CurrState == TreasurePod.State.OPEN) return;

            var pod = __instance.gameObject;
            var nodeScript = pod.AddComponent<TreasurePodESPNode>();
            var esp = GameObject.Instantiate(_espPrefab, pod.transform.position + Vector3.down, Quaternion.identity);
            nodeScript.espNode = esp;
            var material = esp.GetComponent<MeshRenderer>().material;
            foreach (var prop in _colorProperties)
            {
                material.SetColor(prop, GetColor());
            }
        }

    }
    [HarmonyPatch(typeof(TreasurePod), nameof(TreasurePod.Activate))]
    internal class TreasurePodActivatePatch : Entry
    {
        public static void Postfix(TreasurePod __instance)
        {
            var nodeScript = __instance.gameObject.GetComponent<TreasurePodESPNode>();
            GameObject.Destroy(nodeScript.espNode);
        }
    }
}