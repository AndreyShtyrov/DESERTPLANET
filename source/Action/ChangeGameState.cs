﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DesertPlanet.source.Action
{
    public class ChangeGameState : Action
    {
        public int PlayerId { get; set; }

        public int OccupiedLanding { get; set; }

        public GameState PrevState { get; set; }

        public GameState NextState { get; set; }

        public override void Backward()
        {
            throw new NotImplementedException();
        }

        public override void Forward()
        {
            if (Map.Player.Id == PlayerId)
            {
                Map.State = NextState;
            }
            if (PrevState is GameState.ChooseStartResource)
                Map.OccupiedLandings.Add(OccupiedLanding);
        }

        [JsonConstructor]
        public ChangeGameState() { }
        public ChangeGameState(int playreId, GameState prevState, GameState nextState):base()
        {
            PlayerId = playreId;
            PrevState = prevState;
            NextState = nextState;
            if (PrevState == GameState.ChooseStartResource)
            {
                OccupiedLanding = Map.LandingRegion;
            }
            else
            {
                OccupiedLanding = -1;
            }
        }
    }
}
