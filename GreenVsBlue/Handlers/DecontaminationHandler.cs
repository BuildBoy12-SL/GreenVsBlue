// -----------------------------------------------------------------------
// <copyright file="DecontaminationHandler.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace GreenVsBlue.Handlers
{
    using System.Collections.Generic;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using Exiled.Loader;
    using GreenVsBlue.Models;
    using LightContainmentZoneDecontamination;
    using MEC;
    using ServerHandlers = Exiled.Events.Handlers.Server;

    /// <inheritdoc />
    public class DecontaminationHandler : Subscribable
    {
        private static readonly List<ZoneType> AvailableZones = new()
        {
            ZoneType.LightContainment,
            ZoneType.HeavyContainment,
            ZoneType.Entrance,
        };

        private readonly Dictionary<Player, int> surfacePlayers = new();
        private CoroutineHandle cycleCoroutine;
        private CoroutineHandle decontaminationCoroutine;
        private CoroutineHandle surfaceCoroutine;

        /// <summary>
        /// Initializes a new instance of the <see cref="DecontaminationHandler"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public DecontaminationHandler(Plugin plugin)
            : base(plugin)
        {
        }

        /// <inheritdoc/>
        public override void Subscribe()
        {
            ServerHandlers.RoundEnded += OnRoundEnded;
            ServerHandlers.RoundStarted += OnRoundStarted;
            ServerHandlers.WaitingForPlayers += OnWaitingForPlayers;
        }

        /// <inheritdoc />
        public override void Unsubscribe()
        {
            ServerHandlers.RoundEnded -= OnRoundEnded;
            ServerHandlers.RoundStarted -= OnRoundStarted;
            ServerHandlers.WaitingForPlayers -= OnWaitingForPlayers;
        }

        private void OnRoundEnded(RoundEndedEventArgs ev)
        {
            Timing.KillCoroutines(cycleCoroutine, surfaceCoroutine);
        }

        private void OnRoundStarted()
        {
            cycleCoroutine = Timing.RunCoroutine(RunCycle());
            surfaceCoroutine = Timing.RunCoroutine(RunSurfaceCheck());
        }

        private void OnWaitingForPlayers()
        {
            if (decontaminationCoroutine.IsRunning)
                Timing.KillCoroutines(decontaminationCoroutine);

            if (surfaceCoroutine.IsRunning)
                Timing.KillCoroutines(surfaceCoroutine);

            DecontaminationController.Singleton.NetworkRoundStartTime = -1.0;
            DecontaminationController.Singleton._stopUpdating = true;
        }

        private IEnumerator<float> RunCycle()
        {
            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(Plugin.Config.Decontamination.CycleTime);

                ZoneType toDecontaminate = AvailableZones[Loader.Random.Next(AvailableZones.Count)];
                if (Plugin.Config.Decontamination.Commands.TryGetValue(toDecontaminate, out List<ConfiguredCommand> commands) && commands is not null)
                {
                    foreach (ConfiguredCommand command in commands)
                        command.Execute();
                }

                yield return Timing.WaitForSeconds(Plugin.Config.Decontamination.Delay);

                if (decontaminationCoroutine.IsRunning)
                    Timing.KillCoroutines(decontaminationCoroutine);

                decontaminationCoroutine = Timing.RunCoroutine(RunDecontamination(toDecontaminate));

                yield return Timing.WaitForSeconds(Plugin.Config.Decontamination.Duration);
            }
        }

        private IEnumerator<float> RunDecontamination(ZoneType zoneType)
        {
            for (int i = 0; i < Plugin.Config.Decontamination.Duration; i++)
            {
                foreach (Player player in Player.List)
                {
                    if (player.Zone == zoneType)
                        player.EnableEffect(EffectType.Decontaminating, 1.1f);
                }

                yield return Timing.WaitForSeconds(1f);
            }
        }

        private IEnumerator<float> RunSurfaceCheck()
        {
            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(1f);
                foreach (Player player in Player.List)
                {
                    if (player.Zone != ZoneType.Surface)
                    {
                        surfacePlayers.Remove(player);
                        continue;
                    }

                    if (!surfacePlayers.ContainsKey(player))
                        surfacePlayers.Add(player, 0);

                    surfacePlayers[player]++;
                    if (surfacePlayers[player] > Plugin.Config.Decontamination.SurfaceDelay)
                        player.EnableEffect(EffectType.Decontaminating, 1.1f);
                }
            }
        }
    }
}