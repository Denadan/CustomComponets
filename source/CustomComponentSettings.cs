﻿using System;
using System.Collections.Generic;
using BattleTech.UI;
using HBS.Collections;
using HBS.Logging;
using Newtonsoft.Json;
using UnityEngine;

namespace CustomComponents
{
    [SerializeField]
    public struct CCColor
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public Color ToColor()
        {
            return new Color(R / 255f, G / 255f, B / 255f, A / 255f);
        }
    }

    [SerializeField]
    public struct TagColor
    {
        public string Color { get; set; }
        public string Tag { get; set; }

        public Color ToColor()
        {
            if (ColorUtility.TryParseHtmlString(Color, out var color))
                return color;
            return UnityEngine.Color.magenta;
        }
    }

    [Flags]

    public enum DType
    {
        NONE = 0,
        SalvageProccess = 1,
        EffectNull = 1 << 1,
        ShowConfig = 1 << 2,
        ComponentInstall = 1 << 3,
        MechValidation = 1 << 4,
        CCLoading = 1 << 5,
        CCLoadingSummary = 1 << 6,
        InstallCost = 1 << 7,
        FixedCheck = 1 << 8,
        DefaultHandle = 1 << 9,
        ClearInventory = 1 << 10,
        CustomResource = 1 << 11,
        IsDestroyed = 1 << 12,
        AutoFix = 1 << 13,
        Filter = 1 << 14,
        Color = 1 << 15,
        Icons = 1 << 16,
        AutoFixFAKE = 1 << 17,
        AutoFixBase = 1 << 18,
        DetailTagValidation = 1 << 19,
        AutofixValidate = 1 << 20,
        UnitType = 1 << 21,
        FULL = 0xffffff,
    }


    public class CustomComponentSettings
    {
        public DType DebugInfo = DType.Color;

        public bool DEBUG_DumpMechDefs = false;
        public string DEBUG_MechDefsDir = "D:/MechDefs";
        public bool DEBUG_ValidateMechDefs = false;
        public bool DEBUG_ShowOnlyErrors = false;

        public LogLevel LogLevel = LogLevel.Debug;
        public List<TagColor> ColorTags = new List<TagColor>();

        public bool OverrideSalvageGeneration = true;
        public bool NoLootCTDestroyed = false;
        public bool BaseECMValidation = true;

        public bool OverrideRecoveryChance = true;
        public bool SalvageUnrecoveredMech = true;
        public float LimbRecoveryPenalty = 0.05f;
        public float TorsoRecoveryPenalty = 0.1f;
        public float HeadRecoveryPenaly = 0;
        public float EjectRecoveryBonus = 0.25f;

        public bool OverrideMechPartCalculation = true;
        public int CenterTorsoDestroyedParts = 1;
        public float SalvageArmWeight = 0.75f;
        public float SalvageLegWeight = 0.75f;
        public float SalvageTorsoWeight = 1f;
        public float SalvageHeadWeight = 0.5f;
        

        public bool RunAutofixer = true;
        public bool FixDeletedComponents = true;
        public bool FixDefaults = true;
        public bool FixSaveGameMech = false;
        public bool TagRestrictionDropValidateRequiredTags = false;
        public bool TagRestrictionDropValidateIncompatibleTags = true;
        public bool CheckCriticalComponent = true;
        public bool AddTagsToDescription = true;
        public bool UseDefaultFixedColor = false;

        public UIColor DefaultFlagBackgroundColor = UIColor.UpgradeColor;
        public UIColor InvalidFlagBackgroundColor = UIColor.Red;

        public UIColor DefaultOverlayColor = UIColor.DarkGrayEighth;
        public CCColor PreinstalledOverlayCColor = new CCColor() { A = 12, R = 255, B = 180, G = 180 };
        public CCColor DefaultFlagOverlayCColor = new CCColor() { A = 12, R = 180, B = 180, G = 255 };
        public string CategoryDescriptionColor = "#008000";
        public string ShortTagsColor = "#008000";

        [JsonIgnore] public Color PreinstalledOverlayColor;
        [JsonIgnore] public Color DefaultFlagOverlayColor;
        [JsonIgnore] public Dictionary<string, Color> ColorTagsDictionary;

        public bool TestEnableAllTags = false;
        public string OmniTechFlag = "cc_omnitech";
        public bool OmniTechCostBySize = false;
        public int OmniTechInstallCost = 1;
        public bool DontUseFilter = false;
        public bool FixIcons = true;
        public string[] IgnoreAutofixTags = { "ignore_autofix", "noautofix" };

        public string DefaultOldCategoryType = "Mech";
        public bool CheckWeaponCount = false;
        public int MaxWeaponCount = 14;
        public string WrongWeaponCountMessage = "Cannot equip more then {0} weapons";
        internal TagSet ignoreAutofixTags;
        public string[] IgnoreValidationTags = null;
        public TagUnitType[] UnitTypes = null;

        public void Complete()
        {
            PreinstalledOverlayColor = PreinstalledOverlayCColor.ToColor();
            DefaultFlagOverlayColor = DefaultFlagOverlayCColor.ToColor();

            ColorTagsDictionary = new Dictionary<string, Color>();
            if (ColorTags != null)
                foreach (var colorTag in ColorTags)
                {
                    if (!ColorTagsDictionary.ContainsKey(colorTag.Tag))
                        ColorTagsDictionary.Add(colorTag.Tag, colorTag.ToColor());
                }

            ignoreAutofixTags = new TagSet(IgnoreAutofixTags);

            if (UnitTypes != null && UnitTypes.Length > 0)
                foreach (var tagUnitType in UnitTypes)
                {
                    UnitTypeDatabase.Instance.RegisterUnitType(tagUnitType);
                }
        }
    }
}
