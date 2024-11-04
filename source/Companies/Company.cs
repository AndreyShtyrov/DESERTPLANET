using DesertPlanet.source.Ability;
using DesertPlanet.source.Ability.Constructs;
using DesertPlanet.source.Buildings;
using DesertPlanet.source.Companies.Projects;
using DesertPlanet.source.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Companies
{
    public class Company
    {
        public virtual string Name { get; }

        public GameMode Mode { get; }

        public static List<string> Avalialve = new List<string> { "base", "ExTerra" };
        public virtual bool CanHarvestorMoveOnWater => false;
        public string Description { get; }
        public Player Player { get; }
        public virtual int VerticalShiftForHarvesterTile { get; }

        public Dictionary<string, AbilityRecipe> AbilityRecepts { get; }
        public Dictionary<int, BuildingRecipe> Recepts { get; }

        public int BuildingLevel = 3;

        public List<CompanyProject> Projects { get; }

        public virtual PlanetResource GetAlignResource(ResourceType type)
        {
            return new PlanetResource(type, Player.Id);
        }

        public List<PlanetResource> StartResources { get; }
        public Company(string name, Player player, GameMode mode)
        {
            Name = name;
            Player = player;
            Player.Repos = 2;
            Recepts = new Dictionary<int, BuildingRecipe>();
            AbilityRecepts = new Dictionary<string, AbilityRecipe>();
            StartResources = new List<PlanetResource>();
            Mode = mode;
            Projects = new List<CompanyProject>();
            InitRecepts();
            InitStartResources();
            VerticalShiftForHarvesterTile = 0;
        }

        public Company(Player player, GameMode mode)
        {
            Player = player;
            Name = "Unknown";
            Recepts = new Dictionary<int, BuildingRecipe>();
            AbilityRecepts = new Dictionary<string, AbilityRecipe>();
            StartResources = new List<PlanetResource>();
            Mode = mode;
            Projects = new List<CompanyProject>();
        }

        public virtual void InitStartResources()
        {
            StartResources.Add(new PlanetResource(ResourceType.Iron, Player.Id));
            StartResources.Add(new PlanetResource(ResourceType.Iron, Player.Id));
            StartResources.Add(new PlanetResource(ResourceType.Iron, Player.Id));
            StartResources.Add(new PlanetResource(ResourceType.Iron, Player.Id));
            StartResources.Add(new PlanetResource(ResourceType.Plastic, Player.Id));
            StartResources.Add(new PlanetResource(ResourceType.Plastic, Player.Id));
            StartResources.Add(new PlanetResource(ResourceType.Glass, Player.Id));
            StartResources.Add(new PlanetResource(ResourceType.Glass, Player.Id));
            StartResources.Add(new PlanetResource(ResourceType.Aliminium, Player.Id));
            StartResources.Add(new PlanetResource(ResourceType.Aliminium, Player.Id));
            StartResources.Add(new PlanetResource(ResourceType.Uran, Player.Id));
            StartResources.Add(new PlanetResource(ResourceType.Uran, Player.Id));

            Projects.Add(new ManipulatorDrill());
        }

        public CompanyProject GetCompanyProject(int id)
        {
            foreach (var project in Projects)
            {
                if (project.Id == id) return project;
            }
            return null;
        }
        public void InitRecepts()
        {
            Recepts.Add(0, new ElectroMillRecept(Player));
            Recepts.Add(1, new SolidPlatfromRecept(Player));
            Recepts.Add(2, new DrilleRecept(Player));
            Recepts.Add(3, new TidalElectroStationRecept(Player));
            Recepts.Add(5, new FloatPlatformReceipt(Player));
            Recepts.Add(6, new ConversionFabricRecept(Player));
            Recepts.Add(7, new FabricRecept(Player));
            Recepts.Add(8, new SolarPanelRecept(Player));
            Recepts.Add(9, new MeltingFurnaceRecept(Player));
            Recepts.Add(10, new BuildHarvesterRecept(Player));
            Recepts.Add(11, new PowerStationRecept(Player));
            Recepts.Add(12, new ThermalPowerPlantRecept(Player));
            Recepts.Add(13, new HelicopterRecipe(Player));
            Recepts.Add(14, new ManipulatorRecipe(Player));
            Recepts.Add(15, new RadarBuildingRecipe(Player));
            Recepts.Add(16, new HouseRecipe(Player));
            Recepts.Add(17, new ParkRecipe(Player));
            Recepts.Add(18, new CompanyOfficeRecipe(Player));
            Recepts.Add(19, new HydroponicFermRecipe(Player));

            AbilityRecepts.Add("RefineOil", new RefineOilRecept(Player));
            AbilityRecepts.Add("RefineAliminium", new RefineAliminiumRecept(Player));
            AbilityRecepts.Add("RefineGlass", new RefineGlassRecept(Player));
            AbilityRecepts.Add("RefineCement", new RefineCementRecept(Player));
            AbilityRecepts.Add("RefineCementFromBaskit", new RefineCementFromBaskitRecept(Player));
            AbilityRecepts.Add("BurnOil", new BurnOilRecept(Player));
        }

        public virtual Building CreateBuilding(int x, int y, int unitId, int code, Player player)
        {
            switch(code)
            {
                case 0: return new ElectroMill(x, y, BuildingLevel, unitId, player);
                case 1: return new SolidPlatform(x, y, BuildingLevel, unitId, player);
                case 2: return new Drille(x, y, BuildingLevel, unitId, player);
                case 3: return new TidalElectrostation(x, y, BuildingLevel, unitId, player);
                case 4: return new TidalElectrostationWaterPart(x, y, BuildingLevel, unitId, player);
                case 5: return new FloatPlatform(x, y, BuildingLevel, unitId, player);
                case 6: return new ConversionFabric(x, y, BuildingLevel, unitId, player, Mode);
                case 7: return new Fabric(x, y, BuildingLevel, unitId, player, Mode);
                case 8: return new SolarPanel(x, y, BuildingLevel, unitId, player);
                case 9: return new MeltingFurnace(x, y, BuildingLevel, unitId, player, Mode);
                case 11: return new PowerStation(x, y, BuildingLevel, unitId, player);
                case 12: return new ThermalPowerPlant(x, y, BuildingLevel, unitId, player, Mode);
                case 13: return new Helicopter(x, y, BuildingLevel, unitId, player);
                case 14: return new Manipulator(x, y, BuildingLevel, unitId, player, Mode);
                case 15: return new Radar(x, y, BuildingLevel, unitId, player);
                case 16: return new House(x, y, BuildingLevel, unitId, player);
                case 17: return new Park(x, y, BuildingLevel, unitId, player);
                case 18: return new CompanyOffice(x, y, BuildingLevel, unitId, player);
                case 19: return new HydroponicFerm(x, y, BuildingLevel, unitId, player);
            }
            throw new NotImplementedException();
        }

        public virtual List<ConstructBuilding> GetCounstructAbility(IOwnedTokenWithAbilites unit, int startId)
        {
            var result = new List<ConstructBuilding>();
            startId++;
            result.Add(new ConstuctMill(Recepts[0], unit, 14));
            startId++;
            result.Add(new ConstructSolidPlatform(Recepts[1], unit, 1));
            startId++;
            result.Add(new ConstructDrill(Recepts[2], unit, 2));
            startId++;
            result.Add(new ConstructTidalStation(Recepts[3], unit, 16));
            startId++;
            result.Add(new ConstructFloatingPlatfrom(Recepts[5], unit, 5));
            startId++;
            result.Add(new ConstructConversionFabric(Recepts[6], unit, 6));
            startId++;
            result.Add(new ConstructFabric(Recepts[7], unit, 7));
            startId++;
            result.Add(new ConstructSolarPanel(Recepts[8], unit, 8));
            startId++;
            result.Add(new ConstructMeltingFurnace(Recepts[9], unit, 9));
            startId++;
            result.Add(new ConstructPowerStation(Recepts[11], unit, 11));
            startId++;
            result.Add(new ConstructThermalPowerPlant(Recepts[12], unit, 12));
            startId++;
            result.Add(new ConstructHelicopter(Recepts[13], unit, 14));
            startId++;
            result.Add(new ConstructManipulator(Recepts[14], unit, 17));
            startId++;
            result.Add(new ConstructRadar(Recepts[15], unit, 13));
            startId++;
            result.Add(new ConstructHouse(Recepts[16], unit, 16));
            startId++;
            result.Add(new ConstructPark(Recepts[17], unit, 17));
            startId++;
            result.Add(new ConstructCompanyOffice(Recepts[18], unit, 18));
            startId++;
            result.Add(new ConstructHydroponicFerm(Recepts[19], unit, 19));
            return result;
        }

        public static Company CreateFromName(string name, Player player, GameMode gameMode)
        {
            if (name == "base")
                return new Company("base", player, gameMode);
            if (name == "ExTerra")
                return new ExTerra("ExTerra", player, gameMode);
            throw new NotImplementedException("--------Unknown company name");
        }
    }
}
