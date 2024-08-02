using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesertPlanet.source.Interfaces;
using Newtonsoft.Json;

namespace DesertPlanet.source.Action
{
    public abstract class Action : IAction
    {
        private static GameMode map { get; set; }
        [JsonIgnore]
        public GameMode Map { get; }
        public int Idx { get; set; }

        public abstract void Backward();

        public abstract void Forward();

        public Action()
        {
            Map = map;
            Idx = Map.ActionManager.NextActionIdx;
            Map.ActionManager.RegisterAction(this);
        }

        public static void SetPlanteMap(GameMode map)
        { Action.map = map ?? throw new ArgumentNullException(nameof(map)); }
    }
}
