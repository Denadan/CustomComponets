﻿using BattleTech.UI;
using Harmony;

namespace CustomComponents.Patches
{
    [HarmonyPatch(typeof(MechLabPanel))]
    [HarmonyPatch("ExitMechLab")]
    public static class MechLabPanel_ExitMechLab
    {
        [HarmonyPostfix]
        public static void OnMechLabClose()
        {
            MechLabHelper.CloseMechLab();
        }
    }
}