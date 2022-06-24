// -----------------------------------------------------------------------
// <copyright file="DisarmingHandler.cs" company="Build">
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
    public class DisarmingHandler : Subscribable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisarmingHandler"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public DisarmingHandler(Plugin plugin)
            : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Subscribe()
        {
            PlayerHandlers.Handcuffing += OnHandcuffing;
        }

        /// <inheritdoc />
        public override void Unsubscribe()
        {
            PlayerHandlers.Handcuffing -= OnHandcuffing;
        }

        private void OnHandcuffing(HandcuffingEventArgs ev)
        {
            if (Plugin.Config.DisableDisarming)
                ev.IsAllowed = false;
        }
    }
}