﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DesertPlanet.source.Action
{
    public class ChangeActivePlayer: Action
    {
        public int PreviousPlayer {  get; set; }

        public int NextPlayer { get; set; }

        public override void Backward()
        {
            throw new NotImplementedException();
        }

        public override void Forward()
        {
            Map.ActivePlayer = Map.GetPlayer(NextPlayer);
            if (Map.PlayerList.Count != 1 && Map.ActivePlayer == Map.Player)
            {
                Map.CleanPaths();
                Map.NeedApplyTurnStartActions = true;
                Map.NeedUpdatePaths = true;
            }
        }
        [JsonConstructor]
        public ChangeActivePlayer() { }

        public ChangeActivePlayer(Player previousPlayer, Player nextPlayer)
        {
            PreviousPlayer = previousPlayer.Id;
            NextPlayer = nextPlayer.Id;
        }
    }
}
