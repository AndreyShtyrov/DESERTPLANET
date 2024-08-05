using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Godot;

namespace DesertPlanet.source
{
    public class ProgramData
    {
        [JsonIgnore]
        public ClientStatus Status { get; set; } = ClientStatus.Offline;

        [JsonIgnore]
        private static string ProjectSettingPath
            = Path.Combine(System.Environment.GetFolderPath(
                System.Environment.SpecialFolder.ApplicationData), "DesertPlanet");

        public string PlayerName { get; set; } = "Player";

        public ProgramData()
        {
            if (!Directory.Exists(ProjectSettingPath))
                Directory.CreateDirectory(ProjectSettingPath);
            var path = Path.Combine(ProjectSettingPath, "DP.json");

            GD.Print("Load data from " + path);
            if (!File.Exists(path))
                Save();
            else
            {
                var data = Load(path);
                PlayerName = data.PlayerName;
            }
        }

        public void ChangeName(string name)
        {
            PlayerName = name;
            var path = Path.Combine(ProjectSettingPath, "DP.json");
            string json = JsonSerializer.Serialize(this);
            using (StreamWriter sw = new StreamWriter(path))
                sw.WriteLine(json);
        }

        public void Save()
        {
            var path = Path.Combine(ProjectSettingPath, "DP.json");
            string json = JsonSerializer.Serialize(this);
            using (StreamWriter sw = new StreamWriter(path))
                sw.WriteLine(json);
        }

        private ProgramData Load(string path)
        {
            string json = "";
            using (StreamReader sr = new StreamReader(path))
                json = sr.ReadLine();
            var data = JsonSerializer.Deserialize<ProgramData>(json);
            return data;
        }

    }

    public enum ClientStatus
    {
        Host = 1,
        Client = 2,
        Offline = 3,
    }
}
