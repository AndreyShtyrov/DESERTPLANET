using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertPlanet.source.Field
{
    public class SimpleField
    {
        private int iron = 0;
        private int oil = 0;
        private int uran = 0;

        private int maxIron = 4;
        private int maxOil = 3;
        private int maxUran = 1;
        public int Iron { 
            get { return iron; } 
            set {
                if (!isZero && iron == 0)
                    return;
                if (value > -1 && value <= maxIron)
                    iron = value;
                else
                    throw new ArgumentOutOfRangeException();
            } }

        public int Oil { get { return oil; } set
            {
                if (!isZero && oil == 0)
                    return;
                if (value > -1 && value <= maxOil)
                    oil = value;
                else throw new ArgumentOutOfRangeException();
            }
        }

        public int Uran { get { return uran; } set {
                if (!isZero && oil == 0)
                    return;
                if (value > -1 && value <= maxUran)
                    uran = value;
                else { throw new ArgumentOutOfRangeException();}
            } }

        public bool isZero
        {
            get {  if (iron + oil + uran == 0) return true;
            return false;
            }
        }
        public SimpleField Copy()
        {
            var result = new SimpleField();
            result.iron = iron;
            result.oil = oil;
            result.uran = uran;
            return result;
        }

        public static bool operator ==(SimpleField first, SimpleField second)
        {
            if (first.Iron == second.Iron && first.Uran == second.Uran && first.Oil == second.Oil)
                return true;
            return false;
        }

        public static bool operator !=(SimpleField first, SimpleField second)
        {
            if (first.Iron == second.Iron && first.Uran == second.Uran && first.Oil == second.Oil)
                return false;
            return true;
        }

        public Vector2I GetTileShift()
        {
            if (iron == 1) return new Vector2I(0, 0);
            if (iron == 2) return new Vector2I(0, 1);
            if (iron == 3) return new Vector2I(0, 2);
            if (iron == 4) return new Vector2I(1, 2);
            if (uran == 1) return new Vector2I(1, 0);
            if (oil == 1) return new Vector2I(3, 2);
            if (oil == 2) return new Vector2I(3, 1);
            if (oil == 3) return new Vector2I(3, 0);
            return new Vector2I(1, 1);
        }
    }
}
