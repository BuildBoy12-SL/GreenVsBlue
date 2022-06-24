// -----------------------------------------------------------------------
// <copyright file="TicketHandler.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace GreenVsBlue.Handlers
{
    using System.Collections.Generic;
    using Exiled.API.Enums;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using GreenVsBlue.Extensions;
    using GreenVsBlue.Models;
    using MEC;
    using MonoMod.Utils;
    using Respawning;
    using PlayerHandlers = Exiled.Events.Handlers.Player;
    using ServerHandlers = Exiled.Events.Handlers.Server;

    /// <inheritdoc />
    public class TicketHandler : Subscribable
    {
        private readonly Dictionary<SpawnableTeamType, int> startingTickets = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketHandler"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public TicketHandler(Plugin plugin)
            : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Subscribe()
        {
            PlayerHandlers.Died += OnDied;
            PlayerHandlers.Escaping += OnEscaping;
            ServerHandlers.RoundStarted += OnRoundStarted;
        }

        /// <inheritdoc/>
        public override void Unsubscribe()
        {
            PlayerHandlers.Died -= OnDied;
            PlayerHandlers.Escaping -= OnEscaping;
            ServerHandlers.RoundStarted -= OnRoundStarted;
        }

        private void OnDied(DiedEventArgs ev)
        {
            if (ev.Killer is null || ev.Target is null || ev.Killer == ev.Target)
                return;

            if (ev.TargetOldRole.GetSide() == Side.Scp && ev.Killer.Role.Side.TryGetTeamType(out SpawnableTeamType teamType))
                RespawnTickets.Singleton._tickets[teamType] += startingTickets[teamType];
        }

        private void OnEscaping(EscapingEventArgs ev)
        {
            if (ev.Player.Role.Side.TryGetTeamType(out SpawnableTeamType teamType))
                RespawnTickets.Singleton._tickets[teamType]++;
        }

        private void OnRoundStarted()
        {
            startingTickets.Clear();
            RespawnTickets.Singleton._tickets.Clear();
            Timing.CallDelayed(2f, () =>
            {
                foreach (Player player in Player.List)
                {
                    if (!player.Role.Side.TryGetTeamType(out SpawnableTeamType teamType))
                        continue;

                    if (!startingTickets.ContainsKey(teamType))
                        startingTickets.Add(teamType, 0);

                    startingTickets[teamType]++;
                }

                RespawnTickets.Singleton._tickets.AddRange(startingTickets);
            });
        }
    }
}