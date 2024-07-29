using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DesertPlanet.source
{
    public struct HexVector
    {
        public int Q;
        public int R;
        public int S;

        public HexVector(int q, int r, int s)
        {
            Q = q;
            R = r;
            S = s;
        }

        public HexVector(int x, int y)
        {
            Q = x;
            R = y;
            S = -x - y;
        }
        public HexVector()
        {
            Q = 0;
            R = 0;
            S = 0;
        }

        public static HexVector operator +(HexVector left, HexVector right)
        {
            return new HexVector(left.Q + right.Q, left.R + right.R, left.S + right.S);
        }

        public static HexVector operator -(HexVector left, HexVector right)
        {
            return new HexVector(left.Q - right.Q, left.R - right.R, left.S - right.S);
        }

        public int Length()
        {
            return (Math.Abs(Q) + Math.Abs(R) + Math.Abs(S)) / 2;
        }
    }
    public class CubeHexalTools
    {
        public bool IsFlipTop { get; set; } = true;

        public HexVector CubeToHex(int X, int Y)
        {
            return new HexVector(X, Y, -X - Y);
        }

        public HexVector CubeToEvenQ(int  X, int Y, int centerX)
        {
            if (centerX % 2 == 0)
            {
                var x = X;
                var y = Y - (int)(X + (X & 1)) / 2;
                return new HexVector(x, y);
            }
            else
            {
                var x = X;
                var y = Y - (int)(X - (X & 1)) / 2;
                return new HexVector(x, y);
            }
        }

        public Vector2I HexToOffset(HexVector hex, Vector2I center)
        {
            return new Vector2I(hex.Q + center.X, hex.R + center.Y);
        }
    }
}
