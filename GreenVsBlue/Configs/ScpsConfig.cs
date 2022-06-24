// -----------------------------------------------------------------------
// <copyright file="ScpsConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace GreenVsBlue.Configs
{
    using System.ComponentModel;

    /// <summary>
    /// Handles the configs for scps.
    /// </summary>
    public class ScpsConfig
    {
        /// <summary>
        /// Gets or sets the maximum hume shield that Scp096 can have.
        /// </summary>
        [Description("The maximum hume shield that Scp096 can have.")]
        public float Scp096HumeLimit { get; set; } = 500f;

        /// <summary>
        /// Gets or sets the maximum level Scp079 can reach.
        /// </summary>
        [Description("The maximum level Scp079 can reach.")]
        public byte MaximumScp079Level { get; set; } = 3;
    }
}