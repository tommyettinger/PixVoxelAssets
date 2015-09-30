using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssetsPV;
using System.Numerics;

namespace AssetsPV
{
    public static class Extensions
    {
        public const float DegreesToRadians = (float)(Math.PI / 180), RadiansToDegrees = (float)(180 / Math.PI);
        private static Random r = new Random();
        public static T RandomElement<T>(this IEnumerable<T> list)
        {
            if(list == null || list.Count() == 0)
                return default(T);
            int idx = 0, tgt = r.Next(list.Count());
            foreach(T t in list)
            {
                if(tgt == idx)
                {
                    return t;
                }
                idx++;
            }
            return default(T);
        }
        public static int FindByIndex<T>(this IList<T> list, T target)
        {
            if(list == null || list.Count() == 0)
                return -1;
            int idx = 0;
            foreach(T t in list)
            {
                if(target.Equals(list[idx]))
                {
                    return idx;
                }
                idx++;
            }
            return -1;
        }
        public static T RandomElement<T>(this T[,] mat)
        {
            if(mat == null || mat.Length == 0)
                return default(T);

            return mat[r.Next(mat.GetLength(0)), r.Next(mat.GetLength(1))];
        }
        public static T[,] Fill<T>(this T[,] mat, T item)
        {
            if(mat == null || mat.Length == 0)
                return mat;

            for(int i = 0; i < mat.GetLength(0); i++)
            {
                for(int j = 0; j < mat.GetLength(1); j++)
                {
                    mat[i, j] = item;
                }
            }
            return mat;
        }

        public static T[] Repeat<T>(this T item, int count)
        {
            if(item == null)
                return null;
            T[] items = new T[count];
            for(int i = 0; i < count; i++)
            {
                items[i] = item;
            }
            return items;
        }

        public static byte[,] Fill(this byte[,] mat, byte item)
        {
            if(mat == null || mat.Length == 0)
                return mat;

            for(int i = 0; i < mat.GetLength(0); i++)
            {
                for(int j = 0; j < mat.GetLength(1); j++)
                {
                    mat[i, j] = item;
                }
            }
            return mat;
        }
        public static byte[,,] Fill(this byte[,,] mat, byte item)
        {
            if(mat == null || mat.Length == 0)
                return mat;

            for(int i = 0; i < mat.GetLength(0); i++)
            {
                for(int j = 0; j < mat.GetLength(1); j++)
                {
                    for(int k = 0; k < mat.GetLength(2); k++)
                    {
                        mat[i, j, k] = item;
                    }
                }
            }
            return mat;
        }
        public static short[,] Fill(this short[,] mat, short item)
        {
            if(mat == null || mat.Length == 0)
                return mat;

            for(int i = 0; i < mat.GetLength(0); i++)
            {
                for(int j = 0; j < mat.GetLength(1); j++)
                {
                    mat[i, j] = item;
                }
            }
            return mat;
        }
        public static T[,,] Fill<T>(this T[,,] mat, T item)
        {
            if(mat == null || mat.Length == 0)
                return mat;

            for(int i = 0; i < mat.GetLength(0); i++)
            {
                for(int j = 0; j < mat.GetLength(1); j++)
                {
                    for(int k = 0; k < mat.GetLength(2); k++)
                    {
                        mat[i, j, k] = item;
                    }
                }
            }
            return mat;
        }
        public static T[] Fill<T>(this T[] arr, T item)
        {
            if(arr == null || arr.Length == 0)
                return arr;

            for(int i = 0; i < arr.GetLength(0); i++)
            {
                arr[i] = item;
            }
            return arr;
        }

        public static List<T> ToList<T>(this T[,,] mat)
        {
            if(mat == null || mat.Length == 0)
                return new List<T>();
            int xs = mat.GetLength(0), ys = mat.GetLength(1), zs = mat.GetLength(2);
            List<T> l = new List<T>(xs * ys * zs);
            for(int k = 0; k < zs; k++)
            {
                for(int j = 0; j < ys; j++)
                {
                    for(int i = 0; i < xs; i++)
                    {
                        l.Add(mat[i, j, k]);
                    }
                }
            }
            return l;
        }
        public static T[] ToArray<T>(this T[,,] mat)
        {
            if(mat == null || mat.Length == 0)
                return new T[0];
            int xs = mat.GetLength(0), ys = mat.GetLength(1), zs = mat.GetLength(2);
            T[] a = new T[xs * ys * zs];
            int idx = 0;
            for(int k = 0; k < zs; k++)
            {
                for(int j = 0; j < ys; j++)
                {
                    for(int i = 0; i < xs; i++)
                    {
                        a[idx++] = mat[i, j, k];
                    }
                }
            }
            return a;
        }
        public static T[] Flatten<T>(this T[][] mat)
        {
            if(mat == null)
                return null;
            if(mat.Length == 0)
                return new T[0];
            int xs = mat.Length, ys = mat[0].Length;
            T[] a = new T[xs * ys];
            int idx = 0;

            for(int j = 0; j < ys; j++)
            {
                for(int i = 0; i < xs; i++)
                {
                    a[idx++] = mat[i][j];
                }
            }

            return a;
        }
        public static T[,,] Replicate<T>(this T[,,] mat)
        {
            if(mat == null)
                return null;
            if(mat.Length == 0)
                return new T[0, 0, 0];
            int xs = mat.GetLength(0), ys = mat.GetLength(1), zs = mat.GetLength(2);
            T[,,] dupe = new T[xs, ys, zs];

            for(int i = 0; i < xs; i++)
            {
                for(int j = 0; j < ys; j++)
                {
                    for(int k = 0; k < zs; k++)
                    {
                        dupe[i, j, k] = mat[i, j, k];
                    }
                }
            }
            return dupe;
        }
        public static T[,] Replicate<T>(this T[,] mat)
        {
            if(mat == null)
                return null;
            if(mat.Length == 0)
                return new T[0, 0];
            int xs = mat.GetLength(0), ys = mat.GetLength(1);
            T[,] dupe = new T[xs, ys];

            for(int i = 0; i < xs; i++)
            {
                for(int j = 0; j < ys; j++)
                {
                    dupe[i, j] = mat[i, j];
                }

            }
            return dupe;
        }
        public static T[] Replicate<T>(this T[] mat)
        {
            if(mat == null)
                return null;
            if(mat.Length == 0)
                return new T[0];
            int xs = mat.Length;
            T[] dupe = new T[xs];

            for(int i = 0; i < xs; i++)
            {
                dupe[i] = mat[i];
            }
            return dupe;
        }
        public static bool IsPresent(this byte[] mat, byte elem)
        {
            for(int i = 0; i < mat.Length; i++)
            {
                if(mat[i] == elem)
                    return true;
            }
            return false;
        }


        public static int MinXAtZ(this byte[,,] mat, int z, byte[] dismiss)
        {
            int xSize = mat.GetLength(0), ySize = mat.GetLength(1);
            for(int x = 0; x < xSize; x++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    if(!dismiss.IsPresent(mat[x, y, z]))
                        return x;
                }
            }
            return xSize - 1;
        }
        public static int MinYAtZ(this byte[,,] mat, int z, byte[] dismiss)
        {
            int xSize = mat.GetLength(0), ySize = mat.GetLength(1);

            for(int y = 0; y < ySize; y++)
            {
                for(int x = 0; x < xSize; x++)
                {
                    if(!dismiss.IsPresent(mat[x, y, z]))
                        return y;
                }
            }
            return ySize - 1;
        }
        public static int MaxXAtZ(this byte[,,] mat, int z, byte[] dismiss)
        {
            int xSize = mat.GetLength(0), ySize = mat.GetLength(1);
            for(int x = xSize-1; x >= 0; x--)
            {
                for(int y = 0; y < ySize; y++)
                {
                    if(!dismiss.IsPresent(mat[x, y, z]))
                        return x;
                }
            }
            return 0;
        }
        public static int MaxYAtZ(this byte[,,] mat, int z, byte[] dismiss)
        {
            int xSize = mat.GetLength(0), ySize = mat.GetLength(1);

            for(int y = ySize-1; y >= 0; y--)
            {
                for(int x = 0; x < xSize; x++)
                {
                    if(!dismiss.IsPresent(mat[x, y, z]))
                        return y;
                }
            }
            return 0;
        }

        public static int MinZ(this byte[,,] mat, byte[] dismiss)
        {
            int xSize = mat.GetLength(0), ySize = mat.GetLength(1), zSize = mat.GetLength(2);

            for(int z = 0; z < zSize; z++)
            {
                for(int x = 0; x < xSize; x++)
                {
                    for(int y = 0; y < ySize; y++)
                    {
                        if(!dismiss.IsPresent(mat[x, y, z]))
                            return z;
                    }
                }
            }
            return zSize - 1;
        }
        public static int MaxZ(this byte[,,] mat, byte[] dismiss)
        {
            int xSize = mat.GetLength(0), ySize = mat.GetLength(1), zSize = mat.GetLength(2);

            for(int z = zSize - 1; z >= 0; z--)
            {
                for(int x = 0; x < xSize; x++)
                {
                    for(int y = 0; y < ySize; y++)
                    {
                        if(!dismiss.IsPresent(mat[x, y, z]))
                            return z;
                    }
                }
            }
            return 0;
        }
        public static int GimbalPole(this Quaternion q)
        {
            float t = q.Y * q.X + q.Z * q.W;
            return t > 0.499f ? 1 : (t < -0.499f ? -1 : 0);
        }
        public static float Roll(this Quaternion q)
        {
            int pole = q.GimbalPole();
            return (float)(pole == 0 ? Math.Atan2(2f * (q.W * q.Z + q.Y * q.X), 1f - 2f * (q.X * q.X + q.Z * q.Z)) : pole * 2f * Math.Atan2(q.Y, q.W)) * RadiansToDegrees;
        }
        public static float Pitch(this Quaternion q)
        {
            int pole = q.GimbalPole();
            return (pole == 0 ? (float)Math.Asin(VoxelLogic.Clamp(2 * (q.W * q.X - q.Z * q.Y), -1, 1)) : (float)(pole * Math.PI * 0.5)) * RadiansToDegrees;
        }
        public static float Yaw(this Quaternion q)
        {
            return (q.GimbalPole() == 0 ? (float)Math.Atan2(2f * (q.Y * q.W +q.X * q.Z), 1f - 2f * (q.Y * q.Y + q.X * q.X)) : 0f) * RadiansToDegrees;
        }
    }
}
