// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace GreenVsBlue
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Interfaces;
    using GreenVsBlue.Configs;
    using GreenVsBlue.Models;

    /// <inheritdoc />
    public class Config : IConfig
    {
        /// <inheritdoc/>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether disarming is disabled.
        /// </summary>
        [Description("Whether disarming is disabled.")]
        public bool DisableDisarming { get; set; } = true;

        /// <summary>
        /// Gets or sets the amount of time, in seconds, before a killed player will be respawned.
        /// </summary>
        [Description("The amount of time, in seconds, before a killed player will be respawned.")]
        public float RespawnDelay { get; set; } = 20f;

        /// <summary>
        /// Gets or sets a value indicating whether getting revived by Scp049 will cancel the target's ability to respawn.
        /// </summary>
        [Description("Whether getting revived by Scp049 will cancel the target's ability to respawn.")]
        public bool ReviveCancelsRespawn { get; set; } = true;

        /// <summary>
        /// Gets or sets the hints to display to players that spawn with the corresponding role.
        /// </summary>
        [Description("The hints to display to players that spawn with the corresponding role.")]
        public Dictionary<RoleType, Hint> Hints { get; set; } = new()
        {
            {
                RoleType.ClassD,
                new Hint("You are a ClassD", 5f)
            },
        };

        /// <summary>
        /// Gets or sets the configs for decontamination.
        /// </summary>
        public DecontaminationConfig Decontamination { get; set; } = new();

        /// <summary>
        /// Gets or sets the configs for scps.
        /// </summary>
        public ScpsConfig Scps { get; set; } = new();
    }
}