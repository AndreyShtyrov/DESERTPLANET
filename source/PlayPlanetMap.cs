using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source
{
    public class PlayPlanetMap
    {
        public Player Player { get; }

        public List<Player> PlayerList { get; }
        public MapField Map { get; }
        public PlayPlanetMap(Player player, string path) {
            Player = player;
            Map = MapField.Load(path);
            PlayerList = new List<Player>();

        }
    }
}
