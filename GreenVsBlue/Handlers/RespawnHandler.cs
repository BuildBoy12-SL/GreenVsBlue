// -----------------------------------------------------------------------
// <copyright file="RespawnHandler.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace GreenVsBlue.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using Exiled.API.Enums;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using GreenVsBlue.Extensions;
    using GreenVsBlue.Models;
    using MEC;
    using Respawning;
    using PlayerHandlers = Exiled.Events.Handlers.Player;
    using Scp049Handlers = Exiled.Events.Handlers.Scp049;
    using ServerHandlers = Exiled.Events.Handlers.Server;

    /// <inheritdoc />
    public class RespawnHandler : Subscribable
    {
        private static readonly Dictionary<RoleType, RoleType> ProgressionSettings = new()
        {
            { RoleType.ClassD, RoleType.ChaosMarauder },
            { RoleType.ChaosMarauder, RoleType.ChaosRifleman },
            { RoleType.ChaosRifleman, RoleType.ChaosRepressor },
            { RoleType.Scientist, RoleType.NtfPrivate },
            { RoleType.NtfPrivate, RoleType.NtfSergeant },
            { RoleType.NtfSergeant, RoleType.NtfCaptain },
        };

        private readonly Dictionary<Player, (RoleType, CoroutineHandle)> respawnCoroutines = new();
        private readonly List<Player> revivedPlayers = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="RespawnHandler"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public RespawnHandler(Plugin plugin)
            : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Subscribe()
        {
            PlayerHandlers.Died += OnDied;
            Scp049Handlers.FinishingRecall += OnFinishingRecall;
            ServerHandlers.RoundEnded += OnRoundEnded;
            ServerHandlers.WaitingForPlayers += OnWaitingForPlayers;
        }

        /// <inheritdoc />
        public override void Unsubscribe()
        {
            PlayerHandlers.Died -= OnDied;
            Scp049Handlers.FinishingRecall -= OnFinishingRecall;
            ServerHandlers.RoundEnded -= OnRoundEnded;
            ServerHandlers.WaitingForPlayers -= OnWaitingForPlayers;
        }

        private void OnDied(DiedEventArgs ev)
        {
            if (!revivedPlayers.Contains(ev.Target))
                respawnCoroutines.Add(ev.Target, (ev.TargetOldRole, Timing.RunCoroutine(RunRespawn(ev.Target, ev.TargetOldRole))));
        }

        private void OnFinishingRecall(FinishingRecallEventArgs ev)
        {
            if (!Plugin.Config.ReviveCancelsRespawn)
                return;

            if (respawnCoroutines.TryGetValue(ev.Target, out var pair))
            {
                Timing.KillCoroutines(pair.Item2);
                respawnCoroutines.Remove(ev.Target);
                EnsureRevives();
            }

            revivedPlayers.Add(ev.Target);
        }

        private void OnRoundEnded(RoundEndedEventArgs ev) => KillRespawns();

        private void OnWaitingForPlayers()
        {
            revivedPlayers.Clear();
            KillRespawns();
        }

        private void KillRespawns()
        {
            foreach (var pair in respawnCoroutines.Values)
                Timing.KillCoroutines(pair.Item2);

            respawnCoroutines.Clear();
        }

        private void EnsureRevives()
        {
            int chi = Player.Get(Side.ChaosInsurgency).Count() + respawnCoroutines.Values.Select(pair => pair.Item1.GetSide() == Side.ChaosInsurgency).Count();
            int mtf = Player.Get(Side.Mtf).Count() + respawnCoroutines.Values.Select(pair => pair.Item1.GetSide() == Side.Mtf).Count();
            if (chi == 0)
                RespawnTickets.Singleton._tickets[SpawnableTeamType.ChaosInsurgency] = 0;

            if (mtf == 0)
                RespawnTickets.Singleton._tickets[SpawnableTeamType.NineTailedFox] = 0;
        }

        private IEnumerator<float> RunRespawn(Player player, RoleType oldRole)
        {
            yield return Timing.WaitForSeconds(Plugin.Config.RespawnDelay);
            if (player.IsDead && ProgressionSettings.TryGetValue(oldRole, out RoleType newRole) &&
                oldRole.GetSide().TryGetTeamType(out SpawnableTeamType spawnableTeamType) && RespawnTickets.Singleton._tickets[spawnableTeamType] > 0)
            {
                player.SetRole(newRole, SpawnReason.Respawn);
                RespawnTickets.Singleton._tickets[spawnableTeamType]--;
            }
        }
    }
}