using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DesertPlanet.source
{
    public class ResourceContainer: IEnumerable<PlanetResource>
    {
        public int Energy { get; set; } = 0;
        public int Iron { get; set; } = 0;
        public int Alinium { get; set; } = 0;

        public int Plastic { get; set; } = 0;

        public int Oil { get; set; } = 0;

        public int Cement { get; set; } = 0;

        public int Lime { get; set; } = 0;

        public int Uran { get; set; } = 0;

        public int Glass { get; set; } = 0;

        public int Baskit { get; set; } = 0;

        private List<PlanetResource> resources = new List<PlanetResource>();

        public PlanetResource this[int i]
        {
            get  { return resources[i]; }
            set { resources[i] = value; }
        }

        public IEnumerator<PlanetResource> GetEnumerator()
        {
            return resources.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach ( var value in  resources )
            {
                yield return value;
            }
        }

        public static ResourceContainer CreateFromList( List<PlanetResource> resources )
        {
            var container = new ResourceContainer();
            foreach ( var resource in resources )
                container.Add(resource);
            return container;
        }
        public void Add( PlanetResource resource )
        {
            resources.Add( resource );
            SetResource();
        }

        public void AddRange(ResourceContainer other)
        {
            foreach ( var item in other )
                resources.Add( item );
            SetResource();
        }

        public ResourceContainer Copy()
        {
            var result = new ResourceContainer();
            result.AddRange(this);
            return result;
        }


        public void SetResource()
        {
            Iron = 0;
            Energy = 0;
            Plastic = 0;
            Oil = 0;
            Alinium = 0;
            Uran = 0;
            Cement = 0;
            Lime = 0;
            Baskit = 0;
            Glass = 0;
            foreach (var resource in this)
            {
                if (resource.Type == ResourceType.Iron)
                    Iron++;
                if (resource.Type == ResourceType.Energy)
                    Energy++;
                if (resource.Type == ResourceType.Plastic)
                    Plastic++;
                if (resource.Type == ResourceType.Oil)
                    Oil++;
                if (resource.Type == ResourceType.Baksits)
                    Baskit++;
                if (resource.Type == ResourceType.Uran)
                    Uran++;
                if (resource.Type == ResourceType.Cement)
                    Cement++;
                if (resource.Type == ResourceType.Lime)
                    Lime++;
                if (resource.Type == ResourceType.Glass)
                    Glass++;
                if (resource.Type == ResourceType.Aliminium)
                    Alinium++;
            }
        }

        public int CountResourceWithType(ResourceType resource)
        {
            if (resource == ResourceType.Iron)
                return Iron;
            if (resource == ResourceType.Energy)
                return Energy;
            if (resource == ResourceType.Plastic)
                return Plastic;
            if (resource == ResourceType.Oil)
                return Oil;
            if (resource == ResourceType.Baksits)
                return Baskit;
            if (resource == ResourceType.Uran)
                return Uran;
            if (resource == ResourceType.Cement)
                return Cement;
            if (resource == ResourceType.Lime)
                return Lime;
            if (resource == ResourceType.Glass)
                return Glass;
            if (resource == ResourceType.Aliminium)
                return Alinium;
            throw new KeyNotFoundException();
        }

        public void Remove( PlanetResource resource )
        {
            resources.Remove( resource );
            SetResource();
        }

        public void Clean()
        {
            resources.Clear();
            SetResource();
        }
        public PlanetResource? PopupByType(ResourceType type )
        {
            
            foreach( var resource in this)
                if ( resource.Type == type )
                {
                    resources.Remove(resource);
                    SetResource();
                    return resource;
                }
            return new Nullable<PlanetResource>();
        }

        public PlanetResource? PopupByAlter(ResourceType type)
        {
            if (type == ResourceType.None)
                return new Nullable<PlanetResource>();
            foreach (var resource in this)
                if (resource.Alternative == type)
                {
                    resources.Remove(resource);
                    SetResource();
                    return resource;
                }
            return new Nullable<PlanetResource>();
        }
        public int Count { get { return resources.Count; } }

        public bool HasSubSeq(ResourceContainer other)
        {
            List<int> foundInputs = new();
            bool notFound = true;
            foreach(var res in other)
            {
                notFound = true;
                for (int i = 0; i < Count; i++)
                {
                    if (resources[i] == res && !foundInputs.Contains(i))
                    {
                        foundInputs.Add(i);
                        notFound = false;
                        break;
                    }
                }
                if (notFound)
                    break;
            }
            if(notFound)
                return false;
            else return true;
        }

        public bool HasSubSeqByTypes(ResourceContainer other)
        {
            List<int> foundInputs = new();
            bool notFound = true;
            foreach (var res in other)
            {
                notFound = true;
                for (int i = 0; i < Count; i++)
                {
                    if (resources[i].Type == res.Type && resources[i].OwnerId == res.OwnerId && !foundInputs.Contains(i))
                    {
                        foundInputs.Add(i);
                        notFound = false;
                        break;
                    }
                }
                if (notFound)
                    break;
            }
            if (notFound)
                return false;
            else return true;
        }

        public bool HasSubSeqByTypes(List<PlanetResource> other)
        {
            List<int> foundInputs = new();
            bool notFound = true;
            foreach (var res in other)
            {
                notFound = true;
                for (int i = 0; i < Count; i++)
                {
                    if (resources[i].Type == res.Type && resources[i].OwnerId == res.OwnerId && !foundInputs.Contains(i))
                    {
                        foundInputs.Add(i);
                        notFound = false;
                        break;
                    }
                }
                if (notFound)
                    break;
            }
            if (notFound)
                return false;
            else return true;
        }

        public bool HasSubSeq(List<PlanetResource> other)
        {
            List<int> foundInputs = new();
            bool notFound = true;
            foreach (var res in other)
            {
                notFound = true;
                for (int i = 0; i < Count; i++)
                {
                    if (resources[i] == res && !foundInputs.Contains(i))
                    {
                        foundInputs.Add(i);
                        notFound = false;
                        break;
                    }
                }
                if (notFound)
                    break;
            }
            if (notFound)
                return false;
            else return true;
        }
        public bool Contains(PlanetResource resource )
        {
           foreach( var value in resources )
                if (value == resource) return true;
           return false;
        }

        public int CountMinerals { get
            {
                var res = 0;
                foreach( var value in resources )
                    if (value.OwnerId == -1)
                        res ++;
                return res;
            } }
        public override string ToString()
        {
            var line = "";
            if(Count > 0)
                line = this[0].ToString();
            for (int i = 1; i < Count; i++)
                line += "|" + this[i];
            return line;
        }

        public List<PlanetResource> ToList()
        {
            var result = new List<PlanetResource>();
            foreach (var res in this)
                result.Add(res);
            return result;
        }

        public string ToPrint(Player player)
        {
            var result = "";
            var CountRes = new Dictionary<string, int>();
            foreach (var res in this)
            {
                if (res.OwnerId != player.Id) continue;
                switch (res.Type)
                {
                    case ResourceType.Iron:
                        {
                            if (CountRes.ContainsKey("Ir"))
                                CountRes["Ir"]++;
                            else
                                CountRes["Ir"] = 1; break;
                        }
                    case ResourceType.Oil:
                        {
                            if (CountRes.ContainsKey("Oi"))
                                CountRes["Oi"]++;
                            else
                                CountRes["Oi"] = 1; break;
                        }
                    case ResourceType.Energy:
                        {
                            if (CountRes.ContainsKey("En"))
                                CountRes["En"]++;
                            else
                                CountRes["En"] = 1; break;
                        }
                    case ResourceType.Plastic:
                        {
                            if (CountRes.ContainsKey("Pl"))
                                CountRes["Pl"]++;
                            else
                                CountRes["Pl"] = 1; break;
                        }
                    case ResourceType.Aliminium:
                        {
                            if (CountRes.ContainsKey("Al"))
                                CountRes["Al"]++;
                            else
                                CountRes["Al"] = 1; break;
                        }
                    case ResourceType.Glass:
                        {
                            if (CountRes.ContainsKey("Gl"))
                                CountRes["Gl"]++;
                            else
                                CountRes["Gl"] = 1; break;
                        }
                    case ResourceType.Lime:
                        {
                            if (CountRes.ContainsKey("Li"))
                                CountRes["Li"]++;
                            else
                                CountRes["Li"] = 1; break;
                        }
                    case ResourceType.Baksits:
                        {
                            if (CountRes.ContainsKey("Ba"))
                                CountRes["Ba"]++;
                            else
                                CountRes["Ba"] = 1; break;
                        }
                }
            }
            result += "En " + CountRes["En"].ToString();
            foreach(var key in CountRes.Keys)
            {
                if (key == "En")
                    continue;
                result += key + " " + CountRes[key];
            }
            return result;
        }
    }

    public class ResourceContainerJsonConverter : JsonConverter<ResourceContainer>
    {
        public override ResourceContainer Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var line = reader.GetString();
            var parts = line.Split("|");
            var result = new ResourceContainer();
            if (line.Length == 0)
                return result;
            foreach(var part in parts )
                result.Add(PlanetResource.FromString(part));
            return result;
        }

        public override void Write(Utf8JsonWriter writer, ResourceContainer value, JsonSerializerOptions options)
        {
            var line = "";
            if (value.Count > 0)
                line = value[0].ToString();
            for (int i = 1; i < value.Count; i++)
                line += "|" + value[i];
            writer.WriteStringValue(line);
        }
    }
}
