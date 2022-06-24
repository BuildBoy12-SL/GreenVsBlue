// -----------------------------------------------------------------------
// <copyright file="GrantTickets.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace GreenVsBlue.Patches
{
    using HarmonyLib;
    using Respawning;

    /// <summary>
    /// Patches <see cref="RespawnTickets.GrantTickets"/> to prevent the natural granting of tickets.
    /// </summary>
    [HarmonyPatch(typeof(RespawnTickets), nameof(RespawnTickets.GrantTickets))]
    internal static class GrantTickets
    {
        private static bool Prefix(bool overrideLocks) => overrideLocks;
    }
}