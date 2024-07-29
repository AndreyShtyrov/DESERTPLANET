using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Companies.Projects
{
    public class ManipulatorDrill: CompanyProject
    {
        public ManipulatorDrill()
        {
            Name = "ManipulatorDrill";
            Description = "";
            Repo = 2;
            Id = 0;
            IsSold = false;
            Texture = ImageTexture.CreateFromImage(Image.LoadFromFile("res://source/Assets/ProjectIcons/DrillM.png"));
        }
    }
}
