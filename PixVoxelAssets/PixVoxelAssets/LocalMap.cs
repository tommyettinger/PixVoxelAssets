/* This file is licensed under
 * The MIT License (MIT)
 * 
 * Copyright (c) 2014 Thomas Ettinger
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssetsCU
{
    public class Position : IEquatable<Position>, ICloneable
    {
        public int x { get; set; }
        public int y { get; set; }
        public Position()
        {
            x = 0;
            y = 0;
        }
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public List<Position> Adjacent(int width, int height)
        {
            List<Position> l = new List<Position>();
            if (x > 0)
                l.Add(new Position(x - 1, y));
            if (y > 0)
                l.Add(new Position(x, y - 1));
            if (x < width - 1)
                l.Add(new Position(x + 1, y));
            if (y < height - 1)
                l.Add(new Position(x, y + 1));
            return l;
        }
        public static List<Position> Adjacent(int x, int y, int width, int height)
        {
            List<Position> l = new List<Position>();
            if (x > 0)
                l.Add(new Position(x - 1, y));
            if (y > 0)
                l.Add(new Position(x, y - 1));
            if (x < width - 1)
                l.Add(new Position(x + 1, y));
            if (y < height - 1)
                l.Add(new Position(x, y + 1));
            return l;
        }
        public List<Position> Nearby(int width, int height, int radius)
        {
            List<Position> l = new List<Position>();
            for (int i = (x - radius >= 0) ? x - radius : 0; i <= x + radius && i < width; i++)
            {
                for (int j = (y - radius >= 0) ? y - radius : 0; j <= y + radius && j < height; j++)
                {
                    if (Math.Abs(i - x) + Math.Abs(j - y) > radius || (x == i && y == j))
                        continue;
                    l.Add(new Position(i, j));
                }
            }
            return l;
        }
        public static List<Position> Nearby(int x, int y, int width, int height, int radius)
        {
            List<Position> l = new List<Position>();
            for (int i = (x - radius >= 0) ? x - radius : 0; i <= x + radius && i < width; i++)
            {
                for (int j = (y - radius >= 0) ? y - radius : 0; j <= y + radius && j < height; j++)
                {
                    if (Math.Abs(i - x) + Math.Abs(j - y) > radius || (x == i && y == j))
                        continue;
                    l.Add(new Position(i, j));
                }
            }
            return l;
        }
        public List<Position> WithinRange(int width, int height, int min, int max)
        {
            List<Position> l = new List<Position>();
            for (int i = (x - max >= 0) ? x - max : 0; i <= x + max && i < width; i++)
            {
                for (int j = (y - max >= 0) ? y - max : 0; j <= y + max && j < height; j++)
                {
                    if (Math.Abs(i - x) + Math.Abs(j - y) < min || Math.Abs(i - x) + Math.Abs(j - y) > max || (x == i && y == j))
                        continue;
                    l.Add(new Position(i, j));
                }
            }
            return l;
        }
        public static List<Position> WithinRange(int x, int y, int lowerX, int lowerY, int width, int height, int min, int max)
        {
            List<Position> l = new List<Position>();
            for (int i = (x - max >= lowerX) ? x - max : lowerX; i <= x + max && i < width; i++)
            {
                for (int j = (y - max >= lowerY) ? y - max : lowerY; j <= y + max && j < height; j++)
                {
                    if (Math.Abs(i - x) + Math.Abs(j - y) < min || Math.Abs(i - x) + Math.Abs(j - y) > max || (x == i && y == j))
                        continue;
                    l.Add(new Position(i, j));
                }
            }
            return l;
        }

        public bool ValidatePosition(int Width, int Height)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return false;
            return true;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Position);
        }

        public bool Equals(Position p)
        {
            // If parameter is null, return false. 
            if (Object.ReferenceEquals(p, null))
            {
                return false;
            }

            // Optimization for a common success case. 
            if (Object.ReferenceEquals(this, p))
            {
                return true;
            }

            // If run-time types are not exactly the same, return false. 
            if (this.GetType() != p.GetType())
                return false;

            // Return true if the fields match. 
            // Note that the base class is not invoked because it is 
            // System.Object, which defines Equals as reference equality. 
            return (x == p.x) && (y == p.y);
        }

        public override int GetHashCode()
        {
            return x * 0x00010000 + y;
        }

        public static bool operator ==(Position lhs, Position rhs)
        {
            // Check for null on left side. 
            if (Object.ReferenceEquals(lhs, null))
            {
                if (Object.ReferenceEquals(rhs, null))
                {
                    // null == null = true. 
                    return true;
                }

                // Only the left side is null. 
                return false;
            }
            // Equals handles case of null on right side. 
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Position lhs, Position rhs)
        {
            return !(lhs == rhs);
        }
        public Object Clone()
        {
            return new Position(x, y);
        }
    }

    public class LocalMap
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int[,] Land { get; set; }
        public int[,] takenLocations { get; set; }
        public static string[] Terrains = new string[]
        {"Plains","Forest","Desert","Jungle","Hills"
        ,"Mountains","Ruins","Tundra","Road","River",
        "Basement"};

        public static int[] Depths = new int[]
            {
                3, 3, 3, 3, 7, 
                7, 5, 3, 5, 1,
                5,5,5,5,5,5,5,5
            };
        private static Random r = new Random();

        public LocalMap(int MapWidth, int MapHeight)
        {

            Width = MapWidth;
            Height = MapHeight;
            Land = new int[Width, Height];
            takenLocations = new int[Width, Height];
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    takenLocations[i, j] = 0;
                    Land[i, j] = 0;
                }
            }
            List<Position> rivers = MakeSoftPath(RandomSpot(), RandomSpot());
            foreach (Position t in rivers)
            {
                Land[t.x, t.y] = 9;
                takenLocations[t.x, t.y] = 2;
            }
            int numMountains = r.Next((int)(Width * 0.25), (int)(Width));
            MakeMountains(numMountains);
            List<Position> roads = MakeHardPath(RandomSpot(), RandomSpot());
            foreach (Position t in roads)
            {
                Land[t.x, t.y] = 8;
                takenLocations[t.x, t.y] = 4;
            }
            roads = MakeHardPath(RandomSpot(), RandomSpot());
            foreach (Position t in roads)
            {
                Land[t.x, t.y] = 8;
                takenLocations[t.x, t.y] = 4;
            }
            roads = MakeHardPath(RandomSpot(), RandomSpot());
            foreach (Position t in roads)
            {
                Land[t.x, t.y] = 8;
                takenLocations[t.x, t.y] = 4;
            }

            int extreme = 0;
            switch (r.Next(5))
            {
                case 0: extreme = 7;
                    break;
                case 1: extreme = 2;
                    break;
                case 2: extreme = 2;
                    break;
                case 3: extreme = 1;
                    break;
                case 4: extreme = 1;
                    break;
            }
            for (int i = 1; i < Width - 1; i++)
            {
                for (int j = 2; j < Height - 2; j++)
                {
                    for (int v = 0; v < 3; v++)
                    {

                        List<Position> near = new Position(i, j).Nearby(Width, Height, 2);
                        List<int> adj = new List<int>(12);
                        foreach(Position p in near)
                        {
                            adj.Add(Land[p.x, p.y]);
                        }
                        int likeliest = 0;
                        if (!adj.Contains(1) && extreme == 2 && r.Next(5) > 1)
                            likeliest = extreme;
                        if ((adj.Contains(2) && r.Next(4) == 0))
                            likeliest = extreme;
                        if (extreme == 7 && (r.Next(4) == 0) || (adj.Contains(7) && r.Next(3) > 0))
                            likeliest = extreme;
                        if ((adj.Contains(1) && r.Next(5) > 2) || r.Next(7) == 0)
                            likeliest = r.Next(2) * 2 + 1;
                        if (adj.Contains(5) && r.Next(3) == 0)
                            likeliest = r.Next(4, 6);
                        if (r.Next(45) == 0)
                            likeliest = 6;
                        if (takenLocations[i, j] == 0)
                        {
                            Land[i, j] = likeliest;
                        }
                    }
                }
            }
            for (int i = 0; i < Width; i++)
            {
                Land[i, 0] = 0;
                Land[i, Height - 1] = 0;
            }
            for (int i = 0; i < 18; i++)
            {
                Land[0, i] = 0;
                Land[Width - 1, i] = 0;
            }
            return;
        }

        public Position RandomSpot()
        {
            return new Position(r.Next(0, Width - 1), r.Next(0, Height - 1));
        }

        public static List<Position> Bresenham(Position start, Position end)
        {
            List<Position> path = new List<Position>();
            int d = 0;
            int x1 = start.x, x2 = end.x;
            int y1 = start.y, y2 = end.y;

            int dy = Math.Abs(y2 - y1);
            int dx = Math.Abs(x2 - x1);

            int dy2 = (dy << 1); // slope scaling factors to avoid floating
            int dx2 = (dx << 1); // point

            int ix = x1 < x2 ? 1 : -1; // increment direction
            int iy = y1 < y2 ? 1 : -1;

            if (dy <= dx)
            {
                for (; ; )
                {
                    path.Add(new Position(x1, y1));
                    if (x1 == x2)
                        break;
                    x1 += ix;
                    d += dy2;
                    if (d > dx)
                    {
                        path.Add(new Position(x1, y1));
                        y1 += iy;
                        d -= dx2;
                    }
                }
            }
            else
            {
                for (; ; )
                {
                    path.Add(new Position(x1, y1));
                    if (y1 == y2)
                        break;
                    y1 += iy;
                    d += dx2;
                    if (d > dy)
                    {
                        path.Add(new Position(x1, y1));
                        x1 += ix;
                        d -= dy2;
                    }
                }
            }
            return path;
        }
        public bool ValidatePosition(Position p)
        {
            if (p.x < 0 || p.x >= Width || p.y < 0 || p.y >= Height)
                return false;
            return true;
        }
        public Position CorrectPosition(Position p)
        {
            if (p.x < 0)
                p.x = 0;
            if (p.x >= Width)
                p.x = Width - 1;
            if (p.y < 0)
                p.y = 0;
            if (p.y >= Height)
                p.y = Height - 1;
            return p;
        }
        public List<Position> MakeSoftPath(Position start, Position end)
        {
            List<Position> path = Bresenham(start, end), path2 = new List<Position>();

            Position midpoint = path[path.Count / 2], early = path[path.Count / 4], late = path[path.Count*3 / 4];

            midpoint.x += r.Next(Width / 4) - Width / 8;
            midpoint = CorrectPosition(midpoint);

            early.x += r.Next(Width / 3) - Width / 6;
            early = CorrectPosition(early);

            late.x += r.Next(Width / 3) - Width / 6;
            late = CorrectPosition(late);
            path2.AddRange(Bresenham(start, early));
            path2.RemoveAt(path2.Count - 1);
            path2.AddRange(Bresenham(early, midpoint));
            path2.RemoveAt(path2.Count - 1);
            path2.AddRange(Bresenham(midpoint, late));
            path2.RemoveAt(path2.Count - 1);
            path2.AddRange(Bresenham(late, end));


            return path2;
        }

        public List<Position> MakeHardPath(Position start, Position end)
        {

            List<Position> path = new List<Position>();
            if (!ValidatePosition(start) || !ValidatePosition(end))
                return path;
            int x = start.x;
            int y = start.y;
            int dx = Math.Abs(end.x - start.x);
            int dy = Math.Abs(end.y - start.y);
            path.Add(new Position(x, y));
            if (dx > dy)
            {
                if (end.x > start.x)
                {
                    while (end.x > x)
                    {
                        x++;
                        path.Add(new Position(x, y));
                    }
                }
                else
                {
                    while (end.x < x)
                    {
                        x--;
                        path.Add(new Position(x, y));
                    }
                }

                if (end.y > start.y)
                {
                    while (end.y > y)
                    {
                        y++;
                        path.Add(new Position(x, y));
                    }
                }
                else
                {
                    while (end.y < y)
                    {
                        y--;
                        path.Add(new Position(x, y));
                    }
                }

            }
            else
            {
                if (end.y > start.y)
                {
                    while (end.y > y)
                    {
                        y++;
                        path.Add(new Position(x, y));
                    }
                }
                else
                {
                    while (end.y < y)
                    {
                        y--;
                        path.Add(new Position(x, y));
                    }
                }

                if (end.x > start.x)
                {
                    while (end.x > x)
                    {
                        x++;
                        path.Add(new Position(x, y));
                    }
                }
                else
                {
                    while (end.x < x)
                    {
                        x--;
                        path.Add(new Position(x, y));
                    }
                }
            }
            return path;
        }
        public void MakeMountains(int numMountains)
        {

            int iter = 0;
            int rx = r.Next(1, Width - 2), ry = r.Next(1, Height - 2);
            do
            {
                if (takenLocations[rx, ry] < 1 && r.Next(6) > 0) // && ((ry + 1) / 2 != ry)
                {
                    takenLocations[rx, ry] = 2;
                    Land[rx, ry] = r.Next(4, 6);

                    Position np = new Position(rx, ry);
                    np = np.Adjacent(Width, Height).RandomElement();
                    rx = np.x;
                    ry = np.y;
                }
                else
                {
                    rx = r.Next(1, Width - 2);
                    ry = r.Next(1, Height - 2);
                }
                iter++;
            } while (iter < numMountains);
            return;
        }


    }
}
