// -----------------------------------------------------------------------
// <copyright file="AddReset.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace GreenVsBlue.Patches
{
    using HarmonyLib;
    using PlayableScps;

    /// <summary>
    /// Patches <see cref="Scp096.AddReset"/> to prevent adding more rage time.
    /// </summary>
    [HarmonyPatch(typeof(Scp096), nameof(Scp096.AddReset))]
    internal static class AddReset
    {
        private static bool Prefix() => false;
    }
}