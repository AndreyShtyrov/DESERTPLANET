using DesertPlanet.source.Buildings;
using DesertPlanet.source.Interfaces;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Electosity
{
    public class ElectrosityGraph
    {
        private Dictionary<int, Building> Buildings;

        private Dictionary<int, Harvester> Harvesters;

        private List<ElectrosityLine> Lines;

        private CubeHexalTools HexalTools;

        private GameMode Mode;

        private int Horizontal;

        private int Vertical;
        public ElectrosityGraph(GameMode mode)
        {
            Buildings = mode.Buildings;
            Harvesters = mode.Harvesters;
            Mode = mode;
            Horizontal = mode.Map.Horizontal;
            Vertical = mode.Map.Vertical;
            Lines = new List<ElectrosityLine>();
            HexalTools = new CubeHexalTools();
        }

        private void findAllConnected(IOwnedToken first)
        {
            var LineGraph = new ElectrosityLine();
            var points = new List<Vector2I>() { first.Position };
            LineGraph.Add(first);
            while (points.Count > 0)
            {
                var newPoints = new List<Vector2I>();
                foreach (var point in points)
                    foreach(var field in Area(point.X, point.Y, 1, false))
                    {
                        var tokens = Mode.GetTokensByPos(field.X, field.Y);
                        if (tokens.Count == 0)
                            continue;
                        if (LineGraph.Contain(tokens[0]))
                            continue;
                        newPoints.Add(tokens[0].Position);
                        LineGraph.AddRange(tokens);
                    }
                points = newPoints;
            }
            Lines.Add(LineGraph);
        }

        public void Rebuild()
        {
            Lines.Clear();
            List<IOwnedToken> units = Buildings.Values.ToList<IOwnedToken>();
            units.AddRange(Harvesters.Values.ToList());

            var firstPoint = units[0];
            while(firstPoint != null)
            {
                if (Vertical * Horizontal < Lines.Count)
                    break;
                findAllConnected(firstPoint);
                firstPoint = null;
                foreach(var unit in units)
                {
                    var isContend = true;
                    foreach (var eline in Lines)
                        if (eline.Contain(unit))
                            { isContend = false; break; }
                    if (isContend)
                    {
                        firstPoint = unit;
                        break;
                    }
                }

            }
        }

        public List<IHasResource> GetEnergy(IOwnedToken token)
        {
            var previosIds = new List<int>();
            var result = new List<IHasResource>();
            if (token is Harvester harvester)
            {
                result.Add(harvester);
                previosIds.Add(harvester.Id);
            }
            var ids = new List<int>();
            var fields = new List<Vector2I>();
            var area = Area(token.X, token.Y, 1);
            foreach(var eline in Lines)
            {
                var res = eline.GetConnected(area);
                ids.AddRange(res.Item1);
                fields.AddRange(res.Item2);
            }
            
            foreach(var id in ids)
            {
                if (previosIds.Contains(id))
                    continue;
                var unit = Mode.GetObjectById(id);
                if (unit is IHasResource hasResource)
                    result.Add(hasResource);
                previosIds.Add(id);
            }
            var previosFields = new List<Vector2I>();
            foreach (var field in fields)
            {
                if (previosFields.Contains(field))
                    continue;
                var hasResource = Mode.Map[field.X, field.Y];
                result.Add(hasResource);
                previosFields.Add(field);
            }

            return result;
        }


        public List<Vector2I> Area(int x0, int y0, int radius, bool isFill = true)
        {
            var result = new List<Vector2I>();
            for (var x = x0 - radius - 2; x <= x0 + radius + 2; x++)
            {
                if (x < 0 || x >= Horizontal)
                    continue;
                for (var y = y0 - radius - 2; y <= y0 + radius + 2; y++)
                {
                    if (y < 0 || y >= Vertical)
                        continue;
                    if (isFill)
                    {
                        if (HexalTools.CubeToEvenQ(x - x0, y - y0, x0).Length() <= radius)
                            result.Add(new Vector2I(x, y));
                    }
                    else
                    {
                        if (HexalTools.CubeToEvenQ(x - x0, y - y0, x0).Length() == radius)
                            result.Add(new Vector2I(x, y));
                    }

                }
            }
            return result;
        }
    }
}
