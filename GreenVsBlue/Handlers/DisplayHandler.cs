// -----------------------------------------------------------------------
// <copyright file="DisplayHandler.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace GreenVsBlue.Handlers
{
    using Exiled.Events.EventArgs;
    using GreenVsBlue.Models;
    using PlayerHandlers = Exiled.Events.Handlers.Player;

    /// <inheritdoc />
    public class DisplayHandler : Subscribable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayHandler"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public DisplayHandler(Plugin plugin)
            : base(plugin)
        {
        }

        /// <inheritdoc/>
        public override void Subscribe()
        {
            PlayerHandlers.Spawned += OnSpawned;
        }

        /// <inheritdoc/>
        public override void Unsubscribe()
        {
            PlayerHandlers.Spawned -= OnSpawned;
        }

        private void OnSpawned(SpawnedEventArgs ev)
        {
            if (Plugin.Config.Hints.TryGetValue(ev.Player.Role.Type, out Hint hint))
                hint.Display(ev.Player);
        }
    }
}