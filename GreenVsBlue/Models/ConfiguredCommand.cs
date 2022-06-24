// -----------------------------------------------------------------------
// <copyright file="ConfiguredCommand.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace GreenVsBlue.Models
{
    using System;
    using System.Collections.Generic;
    using Exiled.API.Features;
    using Exiled.Loader;
    using MEC;
    using UnityEngine;

    /// <summary>
    /// Represents a custom command to display a message in the console when executed.
    /// </summary>
    [Serializable]
    public class ConfiguredCommand
    {
        private static readonly List<CoroutineHandle> CommandCoroutines = new();
        private int chance = 100;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfiguredCommand"/> class.
        /// </summary>
        public ConfiguredCommand()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfiguredCommand"/> class.
        /// </summary>
        /// <param name="command"><inheritdoc cref="Command"/></param>
        /// <param name="delay"><inheritdoc cref="Delay"/></param>
        public ConfiguredCommand(string command, float delay)
        {
            Command = command;
            Delay = delay;
        }

        /// <summary>
        /// Gets or sets the command to execute.
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// Gets or sets the delay before the command should be executed.
        /// </summary>
        public float Delay { get; set; }

        /// <summary>
        /// Gets or sets the percentage chance that the command will be executed.
        /// </summary>
        public int Chance
        {
            get => chance;
            set => chance = Mathf.Clamp(value, 0, 100);
        }

        /// <summary>
        /// Gets or sets the minimum and maximum limit on the player count for the command to be executed.
        /// </summary>
        public Limit PlayerLimits { get; set; } = new(0, 20);

        /// <summary>
        /// Kills all running command coroutines.
        /// </summary>
        public static void KillAll()
        {
            foreach (CoroutineHandle coroutineHandle in CommandCoroutines)
                Timing.KillCoroutines(coroutineHandle);

            CommandCoroutines.Clear();
        }

        /// <summary>
        /// Executes the <see cref="Command"/> while respecting the <see cref="Delay"/>.
        /// </summary>
        public void Execute()
        {
            CommandCoroutines.Add(Timing.CallDelayed(Delay, () =>
            {
                if ((PlayerLimits is null || PlayerLimits.WithinLimit(Player.Dictionary.Count)) && Loader.Random.Next(100) < Chance)
                    GameCore.Console.singleton.TypeCommand(Command);
            }));
        }
    }
}