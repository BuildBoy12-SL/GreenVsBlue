// -----------------------------------------------------------------------
// <copyright file="ScpHandler.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace GreenVsBlue.Handlers
{
    using Exiled.API.Features.Roles;
    using Exiled.Events.EventArgs;
    using GreenVsBlue.Models;
    using PlayableScps.Messages;
    using Player = Exiled.API.Features.Player;
    using Scp079Handlers = Exiled.Events.Handlers.Scp079;
    using Scp096 = PlayableScps.Scp096;
    using Scp096Handlers = Exiled.Events.Handlers.Scp096;

    /// <inheritdoc />
    public class ScpHandler : Subscribable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScpHandler"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public ScpHandler(Plugin plugin)
            : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Subscribe()
        {
            Scp079Handlers.GainingExperience += OnGainingExperience;
            Scp079Handlers.GainingLevel += OnGainingLevel;
            Scp096Handlers.AddingTarget += OnAddingTarget;
        }

        /// <inheritdoc />
        public override void Unsubscribe()
        {
            Scp079Handlers.GainingExperience -= OnGainingExperience;
            Scp079Handlers.GainingLevel -= OnGainingLevel;
            Scp096Handlers.AddingTarget -= OnAddingTarget;
        }

        private void OnAddingTarget(AddingTargetEventArgs ev)
        {
            if (!ev.IsAllowed || ev.Scp096.CurrentScp is not Scp096 scp096)
                return;

            foreach (Player player in Player.List)
            {
                if (player.IsHuman && player != ev.Target && scp096._targets.Add(player.ReferenceHub))
                    player.Connection.Send(new Scp096ToTargetMessage(player.ReferenceHub));
            }

            scp096.EnrageTimeLeft = 15f;
        }

        private void OnGainingExperience(GainingExperienceEventArgs ev)
        {
            if (ev.Player.Role.Is(out Scp079Role scp079Role) && scp079Role.Level == Plugin.Config.Scps.MaximumScp079Level - 1)
                ev.IsAllowed = false;
        }

        private void OnGainingLevel(GainingLevelEventArgs ev)
        {
            if (ev.Player.Role.Is(out Scp079Role scp079) && ev.NewLevel == Plugin.Config.Scps.MaximumScp079Level)
                scp079.Experience = 0f;
        }
    }
}