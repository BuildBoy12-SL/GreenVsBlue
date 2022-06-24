// -----------------------------------------------------------------------
// <copyright file="RunDecontamination.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace GreenVsBlue.Patches
{
    using CustomPlayerEffects;
    using Exiled.API.Enums;
    using HarmonyLib;
    using LightContainmentZoneDecontamination;

    /// <summary>
    /// Patches <see cref="DecontaminationController.KillPlayers"/> to prevent the disabling of decontamination of players in zones other than <see cref="ZoneType.LightContainment"/>.
    /// </summary>
    [HarmonyPatch(typeof(Decontaminating), nameof(Decontaminating.OnUpdate))]
    internal static class RunDecontamination
    {
        private static bool Prefix() => false;
    }
}