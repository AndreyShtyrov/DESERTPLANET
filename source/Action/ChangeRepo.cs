using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Action
{
    public class ChangeRepo : Action
    {

        public int Repo { get; set; }

        public int Player { get; set; }

        public override void Backward()
        {
            throw new NotImplementedException();
        }

        public override void Forward()
        {
            var player = Map.GetPlayer(Player);
            player.Repos = Repo + player.Repos;
        }

        public ChangeRepo(int PlayerId, int RepoChange)
        {
            Player = PlayerId;
            Repo = RepoChange;
        }
    }
}
