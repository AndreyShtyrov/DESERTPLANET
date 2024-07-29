using DesertPlanet.source.Ability;
using DesertPlanet.source.Ability.Constructs;
using DesertPlanet.source.Buildings;
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
        public string Name { get; }

        public GameMode Mode { get; }

        public virtual bool CanHarvestorMoveOnWater => false;
        public string Description { get; }
        public Player Player { get; }

        public Dictionary<string, AbilityRecipe> AbilityRecepts { get; }
        public Dictionary<int, BuildingRecipe> Recepts { get; }

        public int BuildingLevel = 3;

        public virtual PlanetResource GetAlignResource(ResourceType type)
        {
            return new PlanetResource(type, Player.Id);
        }

        public List<PlanetResource> StartResources { get; }
        public Company(string name, Player player, GameMode mode)
        {
            Name = name;
            Player = player;
            Recepts = new Dictionary<int, BuildingRecipe>();
            AbilityRecepts = new Dictionary<string, AbilityRecipe>();
            StartResources = new List<PlanetResource>();
            Mode = mode;
            InitRecepts();
            InitStartResources();
        }

        internal virtual void InitStartResources()
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
                case 14: return new Manipulator(x, y, BuildingLevel, unitId, player);
            }
            throw new NotImplementedException();
        }

        public List<ConstructBuilding> GetCounstructAbility(IOwnedTokenWithAbilites unit, int startId)
        {
            var result = new List<ConstructBuilding>();
            startId++;
            result.Add(new ConstuctMill(Recepts[0], unit, startId));
            startId++;
            result.Add(new ConstructSolidPlatform(Recepts[1], unit, startId));
            startId++;
            result.Add(new ConstructDrill(Recepts[2], unit, startId));
            startId++;
            result.Add(new ConstructTidalStation(Recepts[3], unit, startId));
            startId++;
            result.Add(new ConstructFloatingPlatfrom(Recepts[5], unit, startId));
            startId++;
            result.Add(new ConstructConversionFabric(Recepts[6], unit, startId));
            startId++;
            result.Add(new ConstructFabric(Recepts[7], unit, startId));
            startId++;
            result.Add(new ConstructSolarPanel(Recepts[8], unit, startId));
            startId++;
            result.Add(new ConstructMeltingFurnace(Recepts[9], unit, startId));
            startId++;
            result.Add(new ConstructPowerStation(Recepts[11], unit, startId));
            startId++;
            result.Add(new ConstructThermalPowerPlant(Recepts[12], unit, startId));
            startId++;
            result.Add(new ConstructHelicopter(Recepts[13], unit, startId));
            startId++;
            result.Add(new ConstructManipulator(Recepts[14], unit, startId));
            return result;
        }
    }
}
