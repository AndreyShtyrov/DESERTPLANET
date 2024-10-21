using DesertPlanet.source.Buildings;
using DesertPlanet.source.Interfaces;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Field
{
    public class PathFounding
    {
        private Dictionary<int, PathNode[,]> pathFields;

        private GameMode gameMode;

        private Task PostSelectProcessingTask = null;
        public PathFounding(GameMode gameMode)
        {
            this.gameMode = gameMode;
        }
        public void BuildPathRoots(int objectId)
        {
            if (gameMode.GetObjectById(objectId) is IHasAbilities unit)
            {
                if (!unit.CanMoving)
                    return;
                var company = gameMode.GetCompany(gameMode.Player);
                var moveOnWater = company.CanHarvestorMoveOnWater;
                if (unit is Harvester)
                {
                    pathFields[objectId] = BuildPathField(unit.Position, moveOnWater, false);
                }
                else
                {
                    pathFields[objectId] = BuildPathField(unit.Position, moveOnWater, true);
                }
            }
        }

        private PathNode[,] BuildPathField(Vector2I start, bool canMoveOnWater, bool canMoveOnlyOnWater)
        {
            var result = new PathNode[gameMode.Map.Horizontal, gameMode.Map.Vertical];
            for (int i = 0; i < gameMode.Map.Horizontal; i++)
                for (int j = 0; j < gameMode.Map.Vertical; j++)
                    result[i, j] = new PathNode();
            var bypassedTiles = new List<Vector2I>();
            var newShell = new List<FieldToken>() { gameMode.Map[start] };
            int ActionCount = 0;
            bypassedTiles.Add(start);
            while (newShell.Count > 0)
            {
                var prevShell = newShell;
                newShell = new List<FieldToken>();
                foreach (var field in prevShell)
                {
                    foreach (var pos in field.ConnectedFields)
                    {
                        if (!gameMode.Map.InBound(pos))
                            continue;
                        if (gameMode.Map[pos] is Empty)
                        {
                            bypassedTiles.Add(pos);
                            continue;
                        }
                        if (gameMode.Map[pos] is Water && gameMode.Map[pos] is WaterOil && canMoveOnWater && !canMoveOnlyOnWater)
                        {
                            bypassedTiles.Add(pos);
                            continue;
                        }
                        if (gameMode.Map[pos] is Water && gameMode.Map[pos] is WaterOil && canMoveOnlyOnWater)
                        {
                            var buildings = gameMode.GetTokensByPos(pos.X, pos.Y);
                            foreach (var building in buildings)
                                if (building is Building)
                                {
                                    bypassedTiles.Add(pos);
                                    continue;
                                }
                            result[pos.X, pos.Y] = new PathNode(new Vector2I(field.X, field.Y), ActionCount + 1);
                            newShell.Add(gameMode.Map[pos]);
                            continue;
                        }
                        if (bypassedTiles.Contains(pos))
                            continue;
                        result[pos.X, pos.Y] = new PathNode(new Vector2I(field.X, field.Y), ActionCount + 1);
                        newShell.Add(gameMode.Map[pos]);
                    }
                }
                foreach (var field in newShell)
                    bypassedTiles.Add(new Vector2I(field.X, field.Y));
                ActionCount++;
            }
            return result;
        }

        public PathNode[,] GetPaths(int id)
        {
            if (pathFields.ContainsKey(id))
                return pathFields[id];
            PostSelectProcessingTask.Wait();
            return pathFields[id];
        }

        public async void UpdatePath()
        {
            if (PostSelectProcessingTask != null)
                await PostSelectProcessingTask;
            PostSelectProcessingTask = new Task(() =>
            {
                foreach (var unit in gameMode.Harvesters.Values)
                {
                    if (unit.Owner != gameMode.Player)
                        continue;
                    BuildPathRoots(unit.Id);
                }
                foreach (var unit in gameMode.Buildings.Values)
                {
                    if (!unit.CanMoving)
                        continue;
                    BuildPathRoots(unit.Id);
                }
                PostSelectProcessingTask = null;
                gameMode.NeedUpdatePaths = false;
            });
            PostSelectProcessingTask.Start();
        }

        public void Clear()
        {
            pathFields.Clear();
        }
    }
}
