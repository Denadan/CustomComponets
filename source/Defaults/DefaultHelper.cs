﻿using BattleTech;
using BattleTech.UI;
using CustomComponents.Changes;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;


namespace CustomComponents
{
    public static class DefaultHelper
    {
        #region EXTENSIONS

        private static Dictionary<string, bool> defaults = new Dictionary<string, bool>();



        public static bool IsModuleFixed(this MechComponentRef item, MechDef mech)
        {
            Control.LogDebug(DType.FixedCheck, $"IsModuleFixed: {item.ComponentDefID}");
            if (!item.IsFixed)
            {
                Control.LogDebug(DType.FixedCheck, $"-- false: not fixed");
                return false;

            }


            if (mech.Chassis.FixedEquipment != null && mech.Chassis.FixedEquipment.Length > 0)
                foreach (var mref in mech.Chassis.FixedEquipment)
                {

                    if (mref.MountedLocation == item.MountedLocation && item.ComponentDefID == mref.ComponentDefID)
                    {
                        Control.LogDebug(DType.FixedCheck, $"-- true!");
                        return true;
                    }
                }
            Control.LogDebug(DType.FixedCheck, $"-- false: not really fixed");


            return false;
        }

        #endregion

        public static MechComponentRef CreateRef(string id, ComponentType type)
        {
            var component_ref = new MechComponentRef(id, string.Empty, type, ChassisLocations.None);
            component_ref.DataManager = UnityGameInstance.BattleTechGame.DataManager;
            component_ref.RefreshComponentDef();

            var state = UnityGameInstance.BattleTechGame.Simulation;
            if (state != null)
                component_ref.SetSimGameUID(state.GenerateSimGameUID());
            return component_ref;
        }

        public static MechComponentRef CreateRef(string id, ComponentType type, ChassisLocations location)
        {
            var component_ref = new MechComponentRef(id, string.Empty, type, ChassisLocations.None);
            component_ref.DataManager = UnityGameInstance.BattleTechGame.DataManager;
            component_ref.SetData(location,0, ComponentDamageLevel.Functional, false);
            component_ref.RefreshComponentDef();


            var state = UnityGameInstance.BattleTechGame.Simulation;
            if (state != null)
                component_ref.SetSimGameUID(state.GenerateSimGameUID());
            return component_ref;
        }

        public static MechComponentDef GetComponentDef(string id, ComponentType type)
        {
            var dm = UnityGameInstance.BattleTechGame.DataManager;
            switch (type)
            {
                case ComponentType.Weapon:
                    dm.WeaponDefs.TryGet(id, out var weapon);
                    return weapon;
                case ComponentType.AmmunitionBox:
                    dm.AmmoBoxDefs.TryGet(id, out var ammobox);
                    return ammobox;
                case ComponentType.HeatSink:
                    dm.HeatSinkDefs.TryGet(id, out var hs);
                    return hs;
                case ComponentType.JumpJet:
                    dm.JumpJetDefs.TryGet(id, out var jj);
                    return jj;
                case ComponentType.Upgrade:
                    dm.UpgradeDefs.TryGet(id, out var upgrade);
                    return upgrade;
                default:
                    CustomComponents.Control.LogError($"Cannot find {id} of type {type}");
                    return null;
            }
        }

        public static void RemoveInventory(string defaultID, MechDef mech, ChassisLocations location, ComponentType type)
        {
            var item = mech.Inventory.FirstOrDefault(i => i.MountedLocation == location && i.ComponentDefID == defaultID);
            if (item != null)
            {
                var inv = mech.Inventory.ToList();
                inv.Remove(item);
                mech.SetInventory(inv.ToArray());
            }
        }

        public static void AddInventory(string defaultID, MechDef mech, ChassisLocations location, ComponentType type, SimGameState state)
        {
            var r = CreateRef(defaultID, type);
            if (r != null)
            {
                r.SetData(location, -1, ComponentDamageLevel.Functional, true);
                var inv = mech.Inventory.ToList();
                inv.Add(r);
                mech.SetInventory(inv.ToArray());

            }
        }

        public static MechLabItemSlotElement CreateSlot(string id, ComponentType type)
        {
            var component_ref = new MechComponentRef(id, string.Empty, type, ChassisLocations.None, isFixed: true);
            component_ref.DataManager = UnityGameInstance.BattleTechGame.DataManager;
            component_ref.RefreshComponentDef();

            if (!component_ref.IsDefault())
            {
                Control.LogError($"CreateDefault: {id} not default or not exist");
            }

            var mechLab = MechLabHelper.CurrentMechLab.MechLab;
            if (mechLab.IsSimGame)
                component_ref.SetSimGameUID(mechLab.Sim.GenerateSimGameUID());

            return mechLab.CreateMechComponentItem(component_ref, false, ChassisLocations.None, mechLab);
        }

        public static MechLabItemSlotElement CreateSlot(MechComponentRef item)
        {
            return MechLabHelper.CurrentMechLab.MechLab.CreateMechComponentItem(item, false, ChassisLocations.None, MechLabHelper.CurrentMechLab.MechLab);
        }

        internal static void AddMechLab(MechComponentRef replace)
        {
            Control.LogDebug(DType.DefaultHandle, $"DefaultHelper.AddMechLab: adding {replace.ComponentDefID} to {replace.MountedLocation}");

            var target = MechLabHelper.CurrentMechLab.GetLocationWidget(replace.MountedLocation);

            if (target == null)
            {
                Control.LogDebug(DType.DefaultHandle, $"DefaultHelper: Cannot add - wrong location ");
                return;
            }

            var slot = CreateSlot(replace);
            slot.MountedLocation = replace.MountedLocation;
            target.OnAddItem(slot, false);
        }

        public static void AddMechLab(string id, ComponentType type, ChassisLocations location)
        {
            if (!MechLabHelper.CurrentMechLab.InMechLab)
                return;
            Control.LogDebug(DType.DefaultHandle, $"DefaultHelper.AddMechLab: adding {id} to {location}");

            var target = MechLabHelper.CurrentMechLab.GetLocationWidget(location);
            if (target == null)
            {
                Control.LogDebug(DType.DefaultHandle, $"DefaultHelper: Cannot add {id} to {location} - wrong location ");
                return;
            }

            var slot = CreateSlot(id, type);
            slot.MountedLocation = location;
            target.OnAddItem(slot, false);
        }

        public static void RemoveMechLab(ChassisLocations location, MechLabItemSlotElement slot)
        {
            if (slot == null)
                return;

            var helper = MechLabHelper.CurrentMechLab.GetLocationHelper(location);

            if (helper.LocalInventory.Contains(slot))
            {

                helper.widget.OnRemoveItem(slot, true);
                Control.LogDebug(DType.DefaultHandle, $"- removed");
                slot.thisCanvasGroup.blocksRaycasts = true;
                MechLabHelper.CurrentMechLab.MechLab.dataManager.PoolGameObject(MechLabPanel.MECHCOMPONENT_ITEM_PREFAB, slot.GameObject);
            }
            else
                Control.LogDebug(DType.DefaultHandle, $"DefaultHelper: Cannot remove {slot.ComponentRef.ComponentDefID} from {helper.LocationName} - wrong location ");
        }

        public static MechComponentRef[] ClearInventory(MechDef source_mech, SimGameState state)
        {

            Control.LogDebug(DType.ClearInventory, "Clearing Inventory");

            var list = source_mech.Inventory.ToList();
            var changes = new Queue<IChange>();
            foreach (var mechComponentRef in list.Where(i => !i.IsFixed ))
                changes.Enqueue(new Change_Remove(mechComponentRef, mechComponentRef.MountedLocation));

            var inv_state = new InventoryOperationState(changes, source_mech);
            inv_state.DoChanges();
            var to_apply = inv_state.GetResults();
            foreach (var invChange in to_apply)
                invChange.ApplyToInventory(source_mech, list);

            Control.LogDebug(DType.ClearInventory, $"- setting guids");
            foreach (var item in list)
            {
                if (string.IsNullOrEmpty(item.SimGameUID))
                    item.SetSimGameUID(state.GenerateSimGameUID());
                Control.LogDebug(DType.ClearInventory, $"-- {item.ComponentDefID} - {item.SimGameUID}");
            }

            return list.ToArray();
        }
    }
}