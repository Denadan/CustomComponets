﻿using System;
using BattleTech;
using BattleTech.UI;
using Harmony;

namespace CustomComponents.Patches
{
    [HarmonyPatch(typeof(InventoryDataObject_InventoryGear), "RefreshItemColor")]
    public class InventoryDataObject_InventoryGear_RefreshItemColor
    {
        [HarmonyPrefix]
        public static bool ChangeColor(InventoryDataObject_InventoryGear __instance, InventoryItemElement theWidget)
        {
            try
            {
                ColorExtentions.ChangeBackColor(__instance.componentRef.Def, theWidget);
                TColorExtentions.ChangeTextIconColor(__instance.componentRef.Def, theWidget);
            }
            catch
            {
                return true;
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(InventoryDataObject_InventoryWeapon), "RefreshItemColor")]
    public class InventoryDataObject_InventoryWeapon_RefreshItemColor
    {
        [HarmonyPrefix]
        public static bool ChangeColor(InventoryDataObject_InventoryWeapon __instance, InventoryItemElement theWidget)
        {
            try
            {
                ColorExtentions.ChangeBackColor(__instance.componentRef.Def, theWidget);
                TColorExtentions.ChangeTextIconColor(__instance.componentRef.Def, theWidget);
            }
            catch
            {
                return true;
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(InventoryItemElement), "RefreshItemColor")]
    public class InventoryItemElement_RefreshItemColor
    {
        [HarmonyPrefix]
        public static bool ChangeColor(InventoryItemElement __instance, MechComponentRef ___componentRef)
        {
            try
            {
                ColorExtentions.ChangeBackColor(___componentRef.Def, __instance);
                TColorExtentions.ChangeTextIconColor(___componentRef.Def, __instance);
            }
            catch
            {
                return true;
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(InventoryItemElement_NotListView), "RefreshItemColor")]
    public class InventoryItemElement_NotListView_RefreshItemColor
    {
        [HarmonyPrefix]
        public static bool ChangeColor(InventoryItemElement_NotListView __instance, MechComponentRef ___componentRef)
        {
            try
            {
                ColorExtentions.ChangeBackColor(___componentRef.Def, __instance);
                TColorExtentions.ChangeTextIconColor(___componentRef.Def, __instance);
            }
            catch
            {
                return true;
            }
            return false;
        }
    }


    [HarmonyPatch(typeof(ListElementController_InventoryGear_NotListView), "RefreshItemColor")]
    public class ListElementController_InventoryGear_NotListView_RefreshItemColor
    {
        [HarmonyPrefix]
        public static bool ChangeColor(ListElementController_InventoryGear_NotListView __instance, InventoryItemElement_NotListView theWidget)
        {
            try
            {
                ColorExtentions.ChangeBackColor(__instance.componentRef.Def, theWidget);
                TColorExtentions.ChangeTextIconColor(__instance.componentRef.Def, theWidget);
            }
            catch
            {
                return true;
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(ListElementController_InventoryWeapon_NotListView), "RefreshItemColor")]
    public class ListElementController_InventoryWeapon_NotListViewn_RefreshItemColor
    {
        [HarmonyPrefix]
        public static bool ChangeColor(ListElementController_InventoryWeapon_NotListView __instance, InventoryItemElement_NotListView theWidget)
        {
            try
            {
                ColorExtentions.ChangeBackColor(__instance.componentRef.Def, theWidget);
                TColorExtentions.ChangeTextIconColor(__instance.componentRef.Def, theWidget);
            }
            catch
            {
                return true;
            }
            return false;
        }
    }
}