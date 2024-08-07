using DesertPlanet.source.Interfaces;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DesertPlanet.source.Field
{
    public class FieldToken: Tile, IHasResource, FieldTileShifts
    {
        public string Name { get; }

        public ResourceContainer Resources { get; }

        private List<Vector2I> _connectedFields = null;

        public int BlockSeds { get; set; }

        [JsonConstructor]
        public FieldToken(int x, int y, string name, ResourceContainer resources, int blockSeds): base(x, y)
        {
            Name = name;
            BlockSeds = blockSeds;
            Resources = resources;
        }

        public FieldToken(int x, int y, string name, ResourceContainer resources): base(x, y)
        {
            Name = name;
            BlockSeds = 0;
            Resources = resources;
        }

        public static FieldToken CastAsType(FieldToken token)
        {
            if (token.Name == "Sand")
                return new Sand(token.X, token.Y, token.Resources, token.BlockSeds);
            if (token.Name == "Stone")
                return new Stone(token.X, token.Y, token.Resources, token.BlockSeds);
            if (token.Name == "StaticCosmoport")
                return new StaticCosmoport(token.X, token.Y, token.Resources, token.BlockSeds);
            if (token.Name == "Empty")
                return new Empty(token.X, token.Y, token.Resources, token.BlockSeds);
            if (token.Name == "Water")
                return new Water(token.X, token.Y, token.Resources, token.BlockSeds);
            if (token.Name == "SandBoskit")
                return new SandBoskit(token.X, token.Y, token.Resources, token.BlockSeds);
            if (token.Name == "SandLime")
                return new SandLime(token.X, token.Y, token.Resources, token.BlockSeds);
            if (token.Name == "StoneIron")
                return new StoneIron(token.X, token.Y, token.Resources, token.BlockSeds);
            if (token.Name == "StoneOil")
                return new StoneOil(token.X, token.Y, token.Resources, token.BlockSeds);
            if (token.Name == "WaterOil")
                return new WaterOil(token.X, token.Y, token.Resources, token.BlockSeds);
            if (token.Name == "Blocked")
                return new Blocked(token.X, token.Y, token.Resources, token.BlockSeds);
            return null;
        }

        public static int BorderTypeFromString(string str)
        {
            switch (str)
            {
                case "0": return 0;
                case "1": return 1;
                case "2": return 2;
                case "3": return 3;
                case "4": return 4;
                case "5": return 5;
                case "6": return 6;
                case "1,2": return 7;
                case "2,3": return 8;
                case "3,4": return 9;
                case "4,5": return 10;
                case "5,6": return 11;
                case "1,6": return 12;
                case "1,2,3": return 13;
                case "2,3,4": return 14;
                case "3,4,5": return 15;
                case "4,5,6": return 16;
                case "1,5,6": return 17;
                case "1,2,6": return 18;
                case "1,2,3,4,5,6": return 19;
            }
            throw new Exception("Not such Border type " + str);
        }

        public static List<int> BorderPostions(int N)
        {
            if (N == 0)
                return new List<int>();
            var str = BorderStringFromType(N);
            var result = new List<int>();
            foreach (var val in str.Split(','))
            {
                result.Add(val.ToInt());
            }
            return result;
        }
        public static string BorderStringFromType(int N)
        {
            switch (N)
            {
                case 0: return "";
                case 1: return "1";
                case 2: return "2";
                case 3: return "3";
                case 4: return "4";
                case 5: return "5";
                case 6: return "6";
                case 7: return "1,2";
                case 8: return "2,3";
                case 9: return "3,4";
                case 10: return "4,5";
                case 11: return "5,6";
                case 12: return "1,6";
                case 13: return "1,2,3";
                case 14: return "2,3,4";
                case 15: return "3,4,5";
                case 16: return "4,5,6";
                case 17: return "1,5,6";
                case 18: return "1,2,6";
                case 19: return "1,2,3,4,5,6";
            }
            throw new Exception("Not such Border type " + N);
        }

        [JsonIgnore]
        public Vector2I TileShift
        {
            get
            {
                if (Name == "Sand")
                    return FieldTileShifts.SandTileShift;
                if (Name == "Stone")
                    return FieldTileShifts.StoneTileShift;
                if (Name == "Water")
                    return FieldTileShifts.WaterTileShift;
                if (Name == "StaticCosmoport")
                    return FieldTileShifts.SpaceDockTileShift;
                if (Name == "Empty")
                    return FieldTileShifts.EmptyTileShift;
                if (Name == "StoneIron")
                    return FieldTileShifts.StoneIronTileShift;
                if (Name == "StoneOil")
                    return FieldTileShifts.StoneOilTileShift;
                if (Name == "SandLime")
                    return FieldTileShifts.SandLimeTileShift;
                if (Name == "SandBoskit")
                    return FieldTileShifts.SandBokcitTileShift;
                if (Name == "Blocked")
                    return FieldTileShifts.BlockedTileShift;
                if (Name == "WaterOil")
                    return FieldTileShifts.WaterOilTileShift;
                GD.Print("Error Don't Found such token " + Name);
                return new Vector2I(-1, -1);
            }
        }

        [JsonIgnore]
        public List<Vector2I> ConnectedFields
        {
            get
            {
                if (_connectedFields != null)
                    return _connectedFields;
                var neighbors = Neighbors;
                switch (BlockSeds)
                {
                    case 1:
                        neighbors.RemoveAt(0); break;
                    case 2:
                        neighbors.RemoveAt(1); break;
                    case 3:
                        neighbors.RemoveAt(2); break;
                    case 4:
                        neighbors.RemoveAt(3); break;
                    case 5:
                        neighbors.RemoveAt(4); break;
                    case 6:
                        neighbors.RemoveAt(5); break;
                    case 7:
                        neighbors.RemoveAt(0);
                        neighbors.RemoveAt(0);
                        break;
                    case 8:
                        neighbors.RemoveAt(1);
                        neighbors.RemoveAt(1);
                        break;
                    case 9:
                        neighbors.RemoveAt(2);
                        neighbors.RemoveAt(2);
                        break;
                    case 10:
                        neighbors.RemoveAt(3);
                        neighbors.RemoveAt(3);
                        break;
                    case 11:
                        neighbors.RemoveAt(4);
                        neighbors.RemoveAt(4);
                        break;
                    case 12:
                        neighbors.RemoveAt(5);
                        neighbors.RemoveAt(0);
                        break;
                    case 13:
                        neighbors.RemoveAt(0);
                        neighbors.RemoveAt(0);
                        neighbors.RemoveAt(0);
                        break;
                    case 14:
                        neighbors.RemoveAt(1);
                        neighbors.RemoveAt(1);
                        neighbors.RemoveAt(1);
                        break;
                    case 15:
                        neighbors.RemoveAt(2);
                        neighbors.RemoveAt(2);
                        neighbors.RemoveAt(2);
                        break;
                    case 16:
                        neighbors.RemoveAt(3);
                        neighbors.RemoveAt(3);
                        neighbors.RemoveAt(3);
                        break;
                    case 17:
                        neighbors.RemoveAt(4);
                        neighbors.RemoveAt(4);
                        neighbors.RemoveAt(0);
                        break;
                    case 18:
                        neighbors.RemoveAt(5);
                        neighbors.RemoveAt(0);
                        neighbors.RemoveAt(0);
                        break;

                }
                _connectedFields = neighbors;
                return neighbors;
            }
        }

        private List<int> DecompresBorderString(string str)
            {
                var result = new List<int>();
                foreach(var val in str.Split(',')) {
                    result.Add(val.ToInt());
                }
                return result;
            }

        public void ExpandBlocks(int blockPosition)
        {
            if (BlockSeds == 0)
            {
                BlockSeds = blockPosition;
                return;
            }
            var blockNubers = DecompresBorderString(FieldToken.BorderStringFromType(BlockSeds));
            if (!blockNubers.Contains(blockPosition))
                blockNubers.Add(blockPosition);
            blockNubers.Sort();
            var newBorderString = "";
            foreach(var val in blockNubers)
            {
                newBorderString += "," + val.ToString();
            }
            newBorderString = newBorderString.Remove(0, 1);
            BlockSeds = FieldToken.BorderTypeFromString(newBorderString);
        }

        public void DeleteBlocks(int blockPosition)
        {
            if (BlockSeds == 0)
            {
                BlockSeds = blockPosition;
                return;
            }
            var blockNubers = DecompresBorderString(FieldToken.BorderStringFromType(BlockSeds));
            blockNubers.Remove(blockPosition);
            blockNubers.Sort();
            var newBorderString = "";
            foreach (var val in blockNubers)
            {
                newBorderString += "," + val.ToString();
            }
            newBorderString.Remove(0);
            BlockSeds = FieldToken.BorderTypeFromString(newBorderString);
        }

        public Vector2I BorderTileShift
        {
            get
            {
                var result = new Vector2I(-1, -1);
                switch (BlockSeds)
                {
                    case 1: result = new Vector2I(0, 0); break;
                    case 2: result = new Vector2I(1, 0); break;
                    case 3: result = new Vector2I(2, 0); break;
                    case 4: result = new Vector2I(3, 0); break;
                    case 5: result = new Vector2I(4, 0); break;
                    case 6: result = new Vector2I(2, 2); break;
                    case 7: result = new Vector2I(1, 2); break;
                    case 8: result = new Vector2I(3, 2); break;
                    case 9: result = new Vector2I(0, 1); break;
                    case 10: result = new Vector2I(1, 1); break;
                    case 11: result = new Vector2I(4, 2); break;
                    case 12: result = new Vector2I(0, 2); break;
                    case 13: result = new Vector2I(0, 4); break;
                    case 14: result = new Vector2I(0, 3); break;
                    case 15: result = new Vector2I(3, 3); break;
                    case 16: result = new Vector2I(1, 3); break;
                    case 17: result = new Vector2I(1, 4); break;
                    case 18: result = new Vector2I(3, 4); break;
                    case 19: result = new Vector2I(2, 5); break;
                }
                return result;
            }
        }
    }      
            
}           
            
            