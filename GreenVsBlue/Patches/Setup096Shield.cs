// -----------------------------------------------------------------------
// <copyright file="Setup096Shield.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace GreenVsBlue.Patches
{
#pragma warning disable SA1313
    using HarmonyLib;
    using PlayableScps;

    /// <summary>
    /// Patches <see cref="Scp096.SetupShield"/> to replace the maximum health amount.
    /// </summary>
    [HarmonyPatch(typeof(Scp096), nameof(Scp096.SetupShield))]
    internal static class Setup096Shield
    {
        private static bool Prefix(Scp096 __instance)
        {
            __instance.Shield.Limit = Plugin.Instance.Config.Scps.Scp096HumeLimit;
            __instance.Shield.DecayRate = -__instance.ShieldRechargeRate;
            __instance.ShieldAmount = Plugin.Instance.Config.Scps.Scp096HumeLimit;
            return false;
        }
    }
}