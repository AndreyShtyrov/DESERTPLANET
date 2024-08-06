using DesertPlanet.source.Interfaces;
using Godot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Godot.HttpRequest;
using static System.Collections.Specialized.BitVector32;

namespace DesertPlanet.source.Action
{
    public class ActionManager
    {
        public List<IAction> PreviousStates = new List<IAction>();
        private List<IAction> CashedActions = new List<IAction>();
        private List<IAction> FullActions = new List<IAction>();
        public int ActionIdx => _ActionIdx;

        private GameMode GameMode;

        public int CurrentActionIdx = 0;

        private int _ActionIdx = 0;
        public int NextActionIdx
        {
            get
            {
                _ActionIdx++;
                return _ActionIdx;
            }
        }

        public void ApplyActions(List<IAction> actions)
        {
            CashedActions.Clear();
            var correctActions = new List<IAction>();
            if (PreviousStates.Count > 0 && actions.Count > 0)
            {
                var firstId = PreviousStates[PreviousStates.Count - 1].Idx;
                var secondId = actions[0].Idx;
                if (firstId != secondId + 1)
                {
                    GD.Print("Some actions is missed, mangear need load missed from FullActions" + firstId + " " + secondId);
                    foreach (var action in actions)
                    {
                        if (action.Idx > firstId && action.Idx < secondId)
                        {
                            correctActions.Add(action);
                        }
                    }
                }
            }
            correctActions.AddRange(actions);
            foreach (var action in correctActions)
            {
                if (CurrentActionIdx < action.Idx)
                {
                    GD.Print(action.ToString());
                    action.Forward();
                    CurrentActionIdx = action.Idx;
                    PreviousStates.Add(action);
                }
            }
            if (PreviousStates.Count - 1 > 0)
                _ActionIdx = PreviousStates[PreviousStates.Count - 1].Idx;
            //GameMode.TriggerUpdateActions();
        }

        public List<IAction> GetMissedActions(int clientActionIndx)
        {
            List<IAction> response = new List<IAction>();
            bool trigger = false;
            int lastSharredElement = 0;
            foreach (var action in PreviousStates)
            {
                if (trigger)
                    response.Add(action);
                if (action.Idx == clientActionIndx)
                {
                    trigger = true;
                }
                else
                {
                    if (action.Idx < clientActionIndx)
                        lastSharredElement = PreviousStates.IndexOf(action);
                }
            }

            if (!trigger)
            {
                for (int i = lastSharredElement; i < PreviousStates.Count; i++)
                {
                    response.Add(PreviousStates[i]);
                }
            }

            return response;
        }

        public void UnDoActions()
        {

        }

        public void RegisterAction(IAction action)
        {
            FullActions.Add(action);
        }

        public ActionManager(GameMode map)
        {
            Action.SetPlanteMap(map);
            GameMode = map;
        }

        public string GetStringForLastActions(int prevState)
        {
            var tail = new ActionsList();
            tail.Actions = new List<IAction>();
            foreach (var action in PreviousStates)
            {
                if (action.Idx < prevState)
                    continue;
                tail.Actions.Add(action);
            }
            
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(tail,
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            return json;
        }

        public void ApplyActionsFromJson(string json)
        {
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<ActionsList>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            ApplyActions(data.Actions);
        }
    }

    public class ActionsList
    {
        public List<IAction> Actions { get; set; }
    }
}
