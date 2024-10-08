﻿using DesertPlanet.source.Buildings;
using DesertPlanet.source.Field;
using DesertPlanet.source.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DesertPlanet.source.Action
{
    public class IncomeResource: Action
    {
        public int UnitId { get; set; }

        public int PlayerId { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public ResourceType Type1 { get; set; }

        public ResourceType Type2 { get; set; }

        [JsonConstructor]
        public IncomeResource() { }
        public IncomeResource(int unitId, int x, int y ,ResourceType type1, ResourceType type2): base()
        {
            UnitId = unitId;
            X = x;
            Y = y;
            Type1 = type1;
            Type2 = type2;
            PlayerId = Map.GetObjectById(UnitId).Owner.Id;
        }

        public IncomeResource(object unit, ResourceType type1, ResourceType type2, Player player)
        {
            if (unit is Harvester token)
            {
                UnitId = token.Id;
                X = token.X;
                Y = token.Y;
                Type1 = type1;
                Type2 = type2;
                PlayerId = player.Id;
            }
            if (unit is FieldToken field)
            {
                UnitId = -1;
                X = field.X;
                Y = field.Y;
                Type1 = type1;
                Type2 = type2;
            }
            if (unit is Building building)
            {
                UnitId = -1;
                X = building.X;
                Y = building.Y;
                Type1 = type1;
                Type2 = type2;
            }
        }

        public override void Backward()
        {
            throw new NotImplementedException();
        }

        public override void Forward()
        {
            if (UnitId != -1)
            {
                var unit = Map.GetObjectById(UnitId);
                if (unit is IHasResource harvester)
                {
                    harvester.Resources.Add(new PlanetResource(Type1, Type2, PlayerId));
                }
            }
            else
            {
                var field = Map.Map[X, Y];
                field.Resources.Add(new PlanetResource(Type1, Type2, PlayerId));
            }
        }
    }
}
