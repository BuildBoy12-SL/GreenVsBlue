// -----------------------------------------------------------------------
// <copyright file="RoundHandler.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace GreenVsBlue.Handlers
{
    using Exiled.Events.EventArgs;
    using GreenVsBlue.Models;
    using Respawning;
    using ServerHandlers = Exiled.Events.Handlers.Server;

    /// <inheritdoc />
    public class RoundHandler : Subscribable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoundHandler"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public RoundHandler(Plugin plugin)
            : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Subscribe()
        {
            ServerHandlers.EndingRound += OnEndingRound;
        }

        /// <inheritdoc />
        public override void Unsubscribe()
        {
            ServerHandlers.EndingRound -= OnEndingRound;
        }

        private void OnEndingRound(EndingRoundEventArgs ev)
        {
            if ((RespawnTickets.Singleton._tickets.TryGetValue(SpawnableTeamType.NineTailedFox, out int ntfTickets) && ntfTickets > 0) ||
                (RespawnTickets.Singleton._tickets.TryGetValue(SpawnableTeamType.ChaosInsurgency, out int chiTickets) && chiTickets > 0) ||
                (ev.ClassList.chaos_insurgents > 0 && ev.ClassList.scps_except_zombies + ev.ClassList.zombies > 0))
            {
                ev.IsAllowed = false;
            }
        }
    }
}