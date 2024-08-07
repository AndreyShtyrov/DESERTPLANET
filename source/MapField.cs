using DesertPlanet.source.Field;
using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace DesertPlanet.source
{
    public class MapField
    {
        public CubeHexalTools HexalTools;
        public string Name { get; }

        public int MaxPlayers { get; set; }

        public bool HasSubSectors { get; set; } = false;
        public int Horizontal { get; }

        public int Vertical { get; }


        private FieldToken[,] _fields = null;

        public SelectionTile[,] LandingFields { get; }

        public MapField(string name, int horizontal, int vertical)
        {
            Name = name;
            Horizontal = horizontal;
            Vertical = vertical;
            MaxPlayers = 2;
            HexalTools = new CubeHexalTools();
            _fields = new FieldToken[horizontal, vertical];
            for (int i = 0; i < horizontal; i++)
                for (int j = 0; j < vertical; j++)
                    _fields[i, j] = new Blocked(i, j);
            LandingFields = new SelectionTile[horizontal, vertical];
            for (int i = 0; i < horizontal; i++)
                for (int j = 0; j < vertical; j++)
                    LandingFields[i, j] = new SelectionTile(-1, i, j);
        }

        public List<FieldToken> Area(int x0, int y0)
        {
            int radius = 1;
            var result = new List<FieldToken>();
            for (var x = x0 - radius - 2; x <= x0 + radius + 2; x++)
            {
                if (x < 0 || x >= Horizontal)
                    continue;
                for (var y = y0 - radius - 2; y <= y0 + radius + 2; y++)
                {
                    if (y < 0 || y >= Vertical)
                        continue;
                    if (HexalTools.CubeToEvenQ(x - x0, y - y0, x0).Length() <= radius)
                        result.Add(this[x,y]);
                }
            }
            return result;
        }

        public bool InBound(Vector2I pos)
        {
            if ((pos.X < 0 || pos.X >= Horizontal || pos.Y < 0 || pos.Y >= Vertical))
                return false;
            return true;
        }
        public FieldToken this[int i, int j]
        {
            get
            {
                return _fields[i, j];
            }
            set
            {
                _fields[i, j] = value;
            }
        }

        public FieldToken this[Vector2I vector2I]
        {
            get
            {
                return _fields[vector2I.X, vector2I.Y];
            }
            set
            {
                _fields[vector2I.X, vector2I.Y] = value;
            }

        }

        public static MapField Load(string file)
        {
            string jsonLine = "";
            using (StreamReader fs = new StreamReader(file))
                jsonLine = fs.ReadToEnd();
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new ResourceContainerJsonConverter() }
            };
            var data = JsonSerializer.Deserialize<SaveMap>(jsonLine, options);
            var result = new MapField(file, data.Horizontal, data.Vertical);
            for (int i = 0; i < data.Horizontal; i++)
                for (int j = 0; j < data.Vertical; j++)
                {
                    result[i, j] = FieldToken.CastAsType(data.Fields[i][j]);
                    result.LandingFields[i, j] = data.Landing[i][j];
                }
            result.MaxPlayers = data.MaxPlayer;
            result.HasSubSectors = data.HasSubSectors;
            return result;
        }

        public void Save(string file)
        {
            var saveMap = new SaveMap();
            saveMap.Fields = new List<List<FieldToken>>();
            saveMap.Landing = new List<List<SelectionTile>>();
            for(int i = 0; i < Horizontal; i++)
            {
                var line = new List<FieldToken>();
                var lineL = new List<SelectionTile>();
                for (int j = 0; j < Vertical; j++)
                {
                    line.Add(_fields[i, j]);
                    lineL.Add(LandingFields[i, j]);
                }
                saveMap.Fields.Add(line);
                saveMap.Landing.Add(lineL);
            }
            saveMap.Name = Name;
            saveMap.Vertical = Vertical;
            saveMap.Horizontal = Horizontal;
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new ResourceContainerJsonConverter() }
            };
            saveMap.MaxPlayer = MaxPlayers;
            saveMap.HasSubSectors = HasSubSectors;
            string json = JsonSerializer.Serialize<SaveMap>(saveMap, options);
            using (StreamWriter fw = new StreamWriter(file))
                fw.WriteLine(json);
        }

        public MapField Copy()
        {
            var result = new MapField(Name, Horizontal, Vertical);
            for (int i = 0; i < Horizontal; i++)
                for (int j = 0; j < Vertical; j++)
                    result[i, j] = FieldToken.CastAsType(this[i, j]);
            for (int i = 0; i < Horizontal; i++)
                for (int j = 0; j < Vertical; j++)
                    result.LandingFields[i, j] = new SelectionTile(LandingFields[i, j].Value, i, j);
            return result;
        }
    }

    public class SaveMap
    {
        public List<List<FieldToken>> Fields { get; set; }

        public List<List<SelectionTile>> Landing { get; set; }

        public bool HasSubSectors { get; set; }
        public int MaxPlayer { get; set; }
        public int Vertical { get; set; }

        public int Horizontal { get; set; }

        public string Name { get; set; }

    }
}
