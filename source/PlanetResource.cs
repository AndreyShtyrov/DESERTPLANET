using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source
{
    public enum ResourceType
    {
        Iron = 0,
        Plastic = 1,
        Oil = 2,
        Baksits = 3,
        Lime = 4,
        Glass = 5,
        Aliminium = 6,
        Cement = 7,
        None = 8,
        Uran = 9,
        Energy = 10,
    }
    public struct PlanetResource
    {
        public ResourceType Type { get; }

        public ResourceType Alternative { get; }

        public int OwnerId { get; }
        public PlanetResource(ResourceType type) { Type = type;  Alternative = ResourceType.None; OwnerId = -1; }

        public PlanetResource(ResourceType type, ResourceType alter) 
        {
            Type = type;
            Alternative = alter;
            OwnerId = -1;
        }

        public PlanetResource(ResourceType type, int ownerId)
        {
            Type = type;
            OwnerId = ownerId;
            Alternative = ResourceType.None;
        }

        public PlanetResource(ResourceType type, ResourceType alter,int ownerId)
        {
            Type = type;
            OwnerId = ownerId;
            Alternative = alter;
        }

        public static bool operator ==(PlanetResource left, PlanetResource right)
        {
            if (left.Type == right.Type || left.Alternative == right.Type || left.Type == right.Alternative || left.Alternative == right.Alternative)
                if (left.OwnerId == right.OwnerId)
                    return true;
            return false;
        }
        public static bool operator !=(PlanetResource left, PlanetResource right)
        {
            if (left.Type != right.Type || left.Alternative != right.Type || left.Type != right.Alternative || left.Alternative != right.Alternative)
                if (left.OwnerId !=  right.OwnerId)
                    return true;
            return false;
        }

        public override int GetHashCode()
        {
            return (int)Type * 100 + (int)Alternative * 10 + OwnerId;
        }

        public static PlanetResource FromString(string line)
        {
            var parts = line.Split('*');
            var p1 = int.Parse(parts[1]);
            var p2 = int.Parse(parts[2]);
            var p1enum = ResourceType.None;
            var p2enum = ResourceType.None;
            switch (p1)
            {
                case 0: p1enum = ResourceType.Iron; break;
                case 1: p1enum = ResourceType.Plastic; break;
                case 2: p1enum = ResourceType.Oil; break;
                case 3: p1enum = ResourceType.Baksits; break;
                case 4: p1enum = ResourceType.Lime; break;
                case 5: p1enum = ResourceType.Glass; break;
                case 6: p1enum = ResourceType.Aliminium; break;
                case 7: p1enum = ResourceType.Cement; break;
                case 8: p1enum = ResourceType.None; break;
                case 9: p1enum = ResourceType.Uran; break;
                case 10: p1enum = ResourceType.Energy; break;
            }
            switch (p2)
            {
                case 0: p2enum = ResourceType.Iron; break;
                case 1: p2enum = ResourceType.Plastic; break;
                case 2: p2enum = ResourceType.Oil; break;
                case 3: p2enum = ResourceType.Baksits; break;
                case 4: p2enum = ResourceType.Lime; break;
                case 5: p2enum = ResourceType.Glass; break;
                case 6: p2enum = ResourceType.Aliminium; break;
                case 7: p2enum = ResourceType.Cement; break;
                case 8: p2enum = ResourceType.None; break;
                case 9: p2enum = ResourceType.Uran; break;
                case 10: p2enum = ResourceType.Energy; break;
            }
            var ownerId = -1;
            if (parts.Length == 4)
                ownerId = int.Parse(parts[3]);
            return new PlanetResource(p1enum, p2enum, ownerId);
        }

        public override string ToString()
        {
            string result = "RT*";
            result += ((int)Type).ToString();
            result += "*";
            result += ((int)Alternative).ToString();
            result += "*" + OwnerId.ToString();
            return result;
        }

    }
}
