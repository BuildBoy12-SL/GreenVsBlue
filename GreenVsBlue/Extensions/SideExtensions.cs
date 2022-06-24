// -----------------------------------------------------------------------
// <copyright file="SideExtensions.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace GreenVsBlue.Extensions
{
    using System.Collections.Generic;
    using Exiled.API.Enums;
    using Respawning;

    /// <summary>
    /// Extensions for the <see cref="Side"/> enum.
    /// </summary>
    public static class SideExtensions
    {
        private static readonly Dictionary<Side, SpawnableTeamType> TeamSides = new()
        {
            { Side.Mtf, SpawnableTeamType.NineTailedFox },
            { Side.ChaosInsurgency, SpawnableTeamType.ChaosInsurgency },
        };

        /// <summary>
        /// Gets the respective <see cref="SpawnableTeamType"/> for a <see cref="Side"/>.
        /// </summary>
        /// <param name="side">The side to convert.</param>
        /// <param name="spawnableTeamType">The respective <see cref="SpawnableTeamType"/> or <see cref="SpawnableTeamType.None"/> if there is no available conversion.</param>
        /// <returns>Whether the <see cref="Side"/> could be converted to a <see cref="SpawnableTeamType"/>.</returns>
        public static bool TryGetTeamType(this Side side, out SpawnableTeamType spawnableTeamType) => TeamSides.TryGetValue(side, out spawnableTeamType);
    }
}