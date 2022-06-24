// -----------------------------------------------------------------------
// <copyright file="DecontaminationConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace GreenVsBlue.Configs
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Enums;
    using GreenVsBlue.Models;

    /// <summary>
    /// Handles the configs for decontamination.
    /// </summary>
    public class DecontaminationConfig
    {
        /// <summary>
        /// Gets or sets the amount of time, in seconds, between zones being selected for decontamination.
        /// </summary>
        [Description("The amount of time, in seconds, between zones being selected for decontamination.")]
        public float CycleTime { get; set; } = 240f;

        /// <summary>
        /// Gets or sets the amount of time, in seconds, between the zone being chosen and the decontamination occuring.
        /// </summary>
        [Description("The amount of time, in seconds, between the zone being chosen and the decontamination occuring.")]
        public float Delay { get; set; } = 60f;

        /// <summary>
        /// Gets or sets the amount of time, in seconds, to decontaminate the selected zone.
        /// </summary>
        [Description("The amount of time, in seconds, to decontaminate the selected zone.")]
        public float Duration { get; set; } = 20f;

        /// <summary>
        /// Gets or sets the commands that will fire each cycle.
        /// </summary>
        [Description("The commands that will fire each cycle.")]
        public Dictionary<ZoneType, List<ConfiguredCommand>> Commands { get; set; } = new()
        {
            {
                ZoneType.LightContainment, new List<ConfiguredCommand>
                {
                    new("ExampleLCZ", 0f),
                }
            },
            {
                ZoneType.HeavyContainment, new List<ConfiguredCommand>
                {
                    new("ExampleHCZ", 0f),
                }
            },
            {
                ZoneType.Entrance, new List<ConfiguredCommand>
                {
                    new("ExampleENT", 0f),
                }
            },
        };

        /// <summary>
        /// Gets or sets the amount of time, in seconds, before a player on surface zone will receive the decontamination effect.
        /// </summary>
        [Description("The amount of time, in seconds, before a player on surface zone will receive the decontamination effect.")]
        public float SurfaceDelay { get; set; } = 120f;
    }
}