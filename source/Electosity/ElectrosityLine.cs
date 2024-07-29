using DesertPlanet.source.Interfaces;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Electosity
{
    public class ElectrosityLine
    {

        private List<int> pointIds = new List<int>();
        public List<Vector2I> Line { get; }

        public ElectrosityLine()
        {
            Line = new List<Vector2I>();
        }

        public bool Contain(IOwnedToken token)
        {
            if (pointIds.Contains(token.Id))
                return true;
            return false;
        }

        public void Add(IOwnedToken point) {
            Line.Add(point.Position);
            pointIds.Add(point.Id);
        }

        public void AddRange(IEnumerable<IOwnedToken> points)
        {
            foreach(var point in points)
            {
                Line.Add(point.Position);
                pointIds.Add(point.Id);
            }
        }

        public void Remove(IOwnedToken point)
        {
            var i = pointIds.IndexOf(point.Id);
            Line.RemoveAt(i);
            pointIds.RemoveAt(i);
        }


        public Tuple<List<int>, List<Vector2I>> GetConnected(List<Vector2I> area)
        {
            foreach (var field in area)
            {
                if (Line.Contains(field))
                {
                    var ids = pointIds;
                    var points = new List<Vector2I>();
                    foreach (var point in Line)
                    {
                        if (!points.Contains(point))
                            points.Add(point);
                    }
                    return new Tuple<List<int>, List<Vector2I>>(ids, points);
                }
            }
            return new Tuple<List<int>, List<Vector2I>>(new List<int>(), new List<Vector2I>());
        }

       

    }
}
