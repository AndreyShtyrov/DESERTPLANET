using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Action
{
    public class ChangeGameState : Action
    {
        public int PlayerId { get; set; }

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
        }

        public ChangeGameState(int playreId, GameState prevState, GameState nextState):base()
        {
            PlayerId = playreId;
            PrevState = prevState;
            NextState = nextState;
        }
    }
}
