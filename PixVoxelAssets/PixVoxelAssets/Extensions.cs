using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssetsCU;
namespace AssetsCU
{
    public static class Extensions
    {
        private static Random r = new Random();
        public static T RandomElement<T>(this IEnumerable<T> list)
        {
            if (list.Count() == 0)
                return default(T);
            int idx = 0, tgt = r.Next(list.Count());
            foreach (T t in list)
            {
                if (tgt == idx)
                {
                    return t;
                }
                idx++;
            }
            return default(T);
        }
        public static int FindByIndex<T>(this IList<T> list, T target)
        {
            if (list.Count() == 0)
                return -1;
            int idx = 0;
            foreach (T t in list)
            {
                if (target.Equals(list[idx]))
                {
                    return idx;
                }
                idx++;
            }
            return -1;
        }
        public static T RandomElement<T>(this T[,] mat)
        {
            if (mat.Length == 0)
                return default(T);

            return mat[r.Next(mat.GetLength(0)), r.Next(mat.GetLength(1))];
        }
        public static T[,] Fill<T>(this T[,] mat, T item)
        {
            if (mat.Length == 0)
                return mat;

            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    mat[i, j] = item;
                }
            }
            return mat;
        }
        public static T[] Fill<T>(this T[] arr, T item)
        {
            if (arr.Length == 0)
                return arr;

            for (int i = 0; i < arr.GetLength(0); i++)
            {
                    arr[i] = item;
            }
            return arr;
        }
        public static TallVoxels.UnitInfo RandomFactionUnit(this TallVoxels.UnitInfo[,] mat, int color)
        {
            if (mat.Length == 0)
                return new TallVoxels.UnitInfo();
            TallVoxels.UnitInfo u = new TallVoxels.UnitInfo();
            List<TallVoxels.UnitInfo> units = new List<TallVoxels.UnitInfo>();
            for (int i = 0; i < mat.GetLength(0); i++ )
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    if (mat[i, j] != null && mat[i, j].color == color && mat[i, j].speed > 0)
                    {
                        units.Add(mat[i, j]);
                    }
                }
            }
            return units.RandomElement();
        }
    }   
}
