using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsPV
{
    public enum Slope
    {
        Cube = 0x1,
        BrightTop = 0x8,
        DimTop = 0x10,
        BrightDim = 0x20,
        BrightDimTop = 0x40,
        BrightBottom = 0x80,
        DimBottom = 0x100,
        BrightDimBottom = 0x200,
        BrightBack = 0x400,
        DimBack = 0x800,
        BrightTopBack = 0x1000,
        DimTopBack = 0x2000,
        BrightBottomBack = 0x4000,
        DimBottomBack = 0x8000,
        BackBack = 0x10000,
        BackBackTop = 0x20000,
        BackBackBottom = 0x40000,
        RearBrightTop = 0x80000,
        RearDimTop = 0x100000,
        RearBrightBottom = 0x200000,
        RearDimBottom = 0x400000,

        BrightDimTopThick = 0x42,
        BrightDimBottomThick = 0x202,
        BrightTopBackThick = 0x1002,
        BrightBottomBackThick = 0x4002,
        DimTopBackThick = 0x2002,
        DimBottomBackThick = 0x8002,
        BackBackTopThick = 0x20002,
        BackBackBottomThick = 0x40002,
    }
    public class FaceVoxel
    {
        public MagicaVoxelData vox;
        public Slope slope;

        public FaceVoxel(MagicaVoxelData v, Slope slp)
        {
            vox = v;
            slope = slp;
        }
        public FaceVoxel(int x, int y, int z, int color, Slope slp)
        {
            vox = new MagicaVoxelData(x, y, z, color);
            slope = slp;
        }
        public FaceVoxel Replicate()
        {
            return new FaceVoxel(vox, slope);
        }
    }

    public struct Point3D : IComparable<Point3D>, IEquatable<Point3D>
    {
        int X, Y, Z;
        public Point3D(int x, int y, int z)
        {
            X = x; Y = y; Z = z;
        }

        public int CompareTo(Point3D other)
        {
            if(Z > other.Z)
                return 3;
            if(Z < other.Z)
                return -3;
            if(X > other.X)
                return 2;
            if(X < other.X)
                return -2;
            if(Y > other.Y)
                return -1;
            if(Y < other.Y)
                return 1;
            return 0;
        }

        public bool Equals(Point3D other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override int GetHashCode()
        {
            return Z * 0x10000 + X * 0x100 + Y;
        }
        public override string ToString()
        {
            return X + " " + Y + " " + Z;
        }
    }

    public struct Vertex3D : IComparable<Vertex3D>, IEquatable<Vertex3D>
    {
        public int Color;
        public float X, Y, Z;
        public Vertex3D(int color, float x, float y, float z)
        {
            X = x; Y = y; Z = z; Color = color;
        }

        public int CompareTo(Vertex3D other)
        {
            if(Z > other.Z)
                return 3;
            if(Z < other.Z)
                return -3;
            if(X > other.X)
                return 2;
            if(X < other.X)
                return -2;
            if(Y > other.Y)
                return -1;
            if(Y < other.Y)
                return 1;
            return 0;
        }

        public bool Equals(Vertex3D other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                hash = hash * 31 + X.GetHashCode();
                hash = hash * 31 + Y.GetHashCode();
                hash = hash * 31 + Z.GetHashCode();
                hash = hash * 31 + Color;
            }
            return hash;
        }
        public override string ToString()
        {
            return X + " " + Y + " " + Z;
        }
        public string Show(int palette)
        {
            int c = (253 - Color) / 4;
            return ToString() + " " +  ((VoxelLogic.wrendered[palette][c][18] * 256 / 255) / 256.0) + " " + ((VoxelLogic.wrendered[palette][c][17] * 256 / 255) / 256.0) +
                " " + ((VoxelLogic.wrendered[palette][c][16] * 256 / 255) / 256.0) + " 1.0";
        }
    }

    public struct Face3D : IEquatable<Face3D>
    {
        public Point3D[] Points;
        public int Color;
        public Face3D(int color, params Point3D[] points)
        {
            Color = color;
            Points = points;
        }

        public bool Equals(Face3D other)
        {
            if(other.Points.Length != Points.Length)
                return false;
            for(int p = 0; p < Points.Length; p++)
            {
                if(!other.Points.Contains(Points[p]))
                    return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                for(int p = 0; p < Points.Length; p++)
                {
                    hash = hash * 31 + Points[p].GetHashCode();
                }
            }

            return hash;
        }
    }

    public class OrientedFace3D : IEquatable<OrientedFace3D>
    {
        public Vertex3D[] Points;
        public int Orient;
        public OrientedFace3D(Vertex3D center, params int[] points)
        {
            Points = new Vertex3D[points.Length / 3];
            Orient = 0;
            for(int p = 0, o = 0; p < Points.Length; p++)
            {
                Points[p] = new Vertex3D(center.Color, center.X + (points[p * 3 + 0] > 0 ? 0.48f : -0.48f), center.Y + (points[p * 3 + 1] > 0 ? 0.48f : -0.48f), center.Z + (points[p * 3 + 2] > 0 ? 0.48f : -0.48f));
                Orient |= points[p * 3 + 0] << o++;
                Orient |= points[p * 3 + 1] << o++;
                Orient |= points[p * 3 + 2] << o++;
            }
        }
        public OrientedFace3D(params Vertex3D[] points)
        {
            Points = points;
            Orient = -1;
        }

        public bool Equals(OrientedFace3D other)
        {
            if(other.Points.Length != Points.Length || other.Orient != Orient)
                return false;
            for(int p = 0; p < Points.Length; p++)
            {
                if(!other.Points.Contains(Points[p]))
                    return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                for(int p = 0; p < Points.Length; p++)
                {
                    hash = hash * 31 + Points[p].GetHashCode();
                }
            }

            return hash;
        }
    }

    public delegate FaceVoxel[,,] FaceModifier(FaceVoxel[,,] faces);

    class FaceLogic
    {

        public static byte[,,] VoxListToArrayPure(IEnumerable<MagicaVoxelData> voxelData, int xSize, int ySize, int zSize)
        {
            byte[,,] data = new byte[xSize, ySize, zSize];
            foreach(MagicaVoxelData mvd in voxelData)
            {
                if(mvd.x >= xSize || mvd.y >= ySize || mvd.z >= zSize)
                    continue;
                data[mvd.x, mvd.y, mvd.z] = mvd.color;
            }
            return data;
        }
        public static byte[,,] VoxListToArray(IEnumerable<MagicaVoxelData> voxelData, int xSize, int ySize, int zSize, byte shadowColor)
        {
            byte[,,] data = new byte[xSize, ySize, zSize];
            foreach(MagicaVoxelData mvd in voxelData)
            {
                if(mvd.x >= xSize || mvd.y >= ySize || mvd.z >= zSize)
                    continue;
                if(!(mvd.color == VoxelLogic.clear || (VoxelLogic.VisualMode == "CU" && (mvd.color == CURedux.inner_shadow || mvd.color == CURedux.emitter0 || mvd.color == CURedux.trail0
                     || mvd.color == CURedux.emitter1 || mvd.color == CURedux.trail1)))
                     && (data[mvd.x, mvd.y, mvd.z] == 0 || data[mvd.x, mvd.y, mvd.z] == shadowColor))
                    data[mvd.x, mvd.y, mvd.z] = mvd.color;
            }
            return data;
        }
        public static List<MagicaVoxelData> VoxArrayToList(byte[,,] voxelData)
        {
            List<MagicaVoxelData> vlist = new List<MagicaVoxelData>(voxelData.Length);
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);

            for(byte x = 0; x < xSize; x++)
            {
                for(byte y = 0; y < ySize; y++)
                {
                    for(byte z = 0; z < zSize; z++)
                    {
                        if(voxelData[x, y, z] > 0 && (VoxelLogic.VisualMode != "CU" || voxelData[x, y, z] > CURedux.emitter0))
                            vlist.Add(new MagicaVoxelData { x = x, y = y, z = z, color = voxelData[x, y, z] });
                    }
                }
            }
            return vlist;
        }
        /*
        public static FaceVoxel[,,] TransformFaces(byte[,,] voxelData)
        {
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);
            FaceVoxel[,,] data = new FaceVoxel[xSize, ySize, zSize];

            for(int z = 0; z < zSize; z++)
            {
                for(int x = 0; x < xSize; x++)
                {
                    for(int y = 0; y < ySize; y++)
                    {
                        if(voxelData[x, y, z] > 0)
                        {
                            data[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)z, color = voxelData[x, y, z] }, Slope.Cube); //  | Slope.Bright | Slope.Dim
                        }
                    }
                }
            }
            for(int z = 0; z < zSize; z++)
            {
                for(int x = 0; x < xSize; x++)
                {
                    for(int y = 0; y < ySize; y++)
                    {
                        bool[,,] neighbors = new bool[3, 3, 3];

                        if(voxelData[x, y, z] > 0)
                        {
                            for(int i = Math.Max(0, x - 1), n0 = i - x; i <= Math.Min(xSize - 1, x + 1); i++, n0++)
                            {
                                for(int j = Math.Max(0, y - 1), n1 = j - y; j <= Math.Min(ySize - 1, y + 1); j++, n1++)
                                {
                                    for(int k = Math.Max(0, z - 1), n2 = k - z; k <= Math.Min(zSize - 1, z + 1); k++, n2++)
                                    {
                                        neighbors[n0 + 1, n1 + 1, n2 + 1] = voxelData[i, j, k] > 0;
                                    }
                                }
                            }
                            // top exposed
                            if(!neighbors[1, 1, 2])
                            {
                                if(neighbors[0, 1, 2] && neighbors[1, 2, 2])
                                {
                                    data[x, y, z].slope = Slope.BrightDimTop;
                                }
                                else if(!neighbors[1, 0, 1] && neighbors[1, 0, 0] && neighbors[1, 2, 2])
                                {
                                    data[x, y, z].slope = Slope.BrightTop;
                                    if(x > 0 && voxelData[x - 1, y, z] == 0)
                                        data[x - 1, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)(x - 1), y = (byte)y, z = (byte)z, color = voxelData[x, y, z] }, Slope.Cube); // | Slope.Bright | Slope.Dim
                                }
                                else if(!neighbors[2, 1, 1] && neighbors[2, 1, 0] && neighbors[0, 1, 2])
                                {
                                    data[x, y, z].slope = Slope.DimTop;
                                    if(y < ySize - 2 && voxelData[x, y + 1, z] == 0)
                                        data[x, y + 1, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)(y + 1), z = (byte)z, color = voxelData[x, y, z] }, Slope.Cube); // | Slope.Bright | Slope.Dim
                                }
                                else if(!neighbors[0, 0, 1] && neighbors[1, 2, 2] && neighbors[2, 1, 2] && neighbors[1, 1, 0])
                                {
                                    data[x, y, z].slope = Slope.BrightTopBack;
                                }
                                else if(!neighbors[2, 2, 1] && neighbors[0, 1, 2] && neighbors[1, 0, 2] && neighbors[1, 1, 0])
                                {
                                    data[x, y, z].slope = Slope.DimTopBack;
                                }
                                else
                                {
                                    data[x, y, z].slope = Slope.Cube;
                                    
                                    //if(!neighbors[2, 1, 1] && !neighbors[1, 0, 1])
                                    //{
                                    //    data[x, y, z].slope |= Slope.Dim | Slope.Bright;
                                    //}
                                    //else if(!neighbors[2, 1, 1])
                                    //{
                                    //    data[x, y, z].slope |= Slope.Dim;
                                    //}
                                    //else if(!neighbors[1, 0, 1])
                                    //{
                                    //    data[x, y, z].slope |= Slope.Bright;
                                    //}

                                    if(!neighbors[2, 0, 1] && neighbors[0, 2, 1] && (neighbors[0, 0, 1] || neighbors[2, 2, 1]))
                                    {
                                        data[x, y, z].slope = Slope.BrightDim;
                                        if(x > 0 && y < ySize - 2 && voxelData[x - 1, y + 1, z] == 0)
                                            data[x - 1, y + 1, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)(x - 1), y = (byte)(y + 1), z = (byte)z, color = voxelData[x, y, z] }, Slope.BackBack);
                                    }
                                    else if(!neighbors[0, 0, 1] && (neighbors[0, 2, 1] || neighbors[2, 0, 1])) // && neighbors[2, 2, 1]
                                    {
                                        data[x, y, z].slope = Slope.BrightBack;
                                        if(x < xSize - 2 && y < ySize - 2 && voxelData[x + 1, y + 1, z] == 0)
                                            data[x + 1, y + 1, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)(x + 1), y = (byte)(y + 1), z = (byte)z, color = voxelData[x, y, z] }, Slope.DimBack);
                                    }
                                    else if(!neighbors[2, 2, 1] && (neighbors[0, 2, 1] || neighbors[2, 0, 1])) // && neighbors[0, 0, 1]
                                    {
                                        data[x, y, z].slope = Slope.DimBack;
                                        if(x > 0 && y > 0 && voxelData[x - 1, y - 1, z] == 0)
                                            data[x - 1, y - 1, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)(x - 1), y = (byte)(y - 1), z = (byte)z, color = voxelData[x, y, z] }, Slope.BrightBack);
                                    }
                                    else if(!neighbors[0, 2, 1] && neighbors[0, 0, 1] && (neighbors[2, 2, 1] || neighbors[2, 0, 1]))
                                    {
                                        data[x, y, z].slope = Slope.BackBack;
                                        if(x < xSize - 2 && y > 0 && voxelData[x + 1, y - 1, z] == 0)
                                            data[x + 1, y - 1, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)(x + 1), y = (byte)(y - 1), z = (byte)z, color = voxelData[x, y, z] }, Slope.BrightDim);
                                    }
                                }
                            }
                            // top connected
                            else
                            {
                                if(!neighbors[2, 0, 1] && neighbors[0, 2, 1] && (neighbors[0, 0, 1] || neighbors[2, 2, 1]))
                                {
                                    data[x, y, z].slope = Slope.BrightDim;
                                    if(x > 0 && y < ySize - 2 && voxelData[x - 1, y + 1, z] == 0)
                                        data[x - 1, y + 1, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)(x - 1), y = (byte)(y + 1), z = (byte)z, color = voxelData[x, y, z] }, Slope.BackBack);
                                }
                                else if(!neighbors[0, 0, 1] && (neighbors[0, 2, 1] || neighbors[2, 0, 1])) // && neighbors[2, 2, 1]
                                {
                                    data[x, y, z].slope = Slope.BrightBack;
                                    if(x < xSize - 2 && y < ySize - 2 && voxelData[x + 1, y + 1, z] == 0)
                                        data[x + 1, y + 1, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)(x + 1), y = (byte)(y + 1), z = (byte)z, color = voxelData[x, y, z] }, Slope.DimBack);
                                }
                                else if(!neighbors[2, 2, 1] && (neighbors[0, 2, 1] || neighbors[2, 0, 1])) // && neighbors[0, 0, 1]
                                {
                                    data[x, y, z].slope = Slope.DimBack;
                                    if(x > 0 && y > 0 && voxelData[x - 1, y - 1, z] == 0)
                                        data[x - 1, y - 1, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)(x - 1), y = (byte)(y - 1), z = (byte)z, color = voxelData[x, y, z] }, Slope.BrightBack);
                                }
                                else if(!neighbors[0, 2, 1] && neighbors[0, 0, 1] && (neighbors[2, 2, 1] || neighbors[2, 0, 1]))
                                {
                                    data[x, y, z].slope = Slope.BackBack;
                                    if(x < xSize - 2 && y > 0 && voxelData[x + 1, y - 1, z] == 0)
                                        data[x + 1, y - 1, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)(x + 1), y = (byte)(y - 1), z = (byte)z, color = voxelData[x, y, z] }, Slope.BrightDim);
                                }
                                else
                                {

                                    if((!neighbors[2, 1, 1] && !neighbors[1, 0, 1]) || !neighbors[2, 1, 1] || !neighbors[1, 0, 1])
                                    {
                                        data[x, y, z].slope = Slope.Cube; // Slope.Dim | Slope.Bright | 
                                    }
                                    
                                    //else if(!neighbors[2, 1, 1])
                                    //{
                                    //    data[x, y, z].slope = Slope.Dim | Slope.Top;
                                    //}
                                    //else if(!neighbors[1, 0, 1])
                                    //{
                                    //    data[x, y, z].slope = Slope.Bright | Slope.Top;
                                    //}
                                    
                                }
                                //data[x, y, z].slope |= Slope.Top;
                            }

                        }
                    }
                }
            }
            return data;
        }
        */
        public static FaceVoxel[,,] GetCubes(byte[,,] voxelData)
        {
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);
            FaceVoxel[,,] data = new FaceVoxel[xSize, ySize, zSize];
            bool[] nearby = new bool[27];
            for(int z = 0; z < zSize; z++)
            {
                for(int x = 0; x < xSize; x++)
                {
                    for(int y = 0; y < ySize; y++)
                    {
                        byte mvd = voxelData[x, y, z];
                        if(mvd > 0 && (VoxelLogic.VisualMode != "CU" || mvd > CURedux.emitter0))
                        {
                            nearby.Fill(false);
                            for(int i = 0; i < 3; i++)
                            {
                                if(x - 1 + i >= 0 && x - 1 + i < xSize)
                                {
                                    for(int j = 0; j < 3; j++)
                                    {
                                        if(y - 1 + j >= 0 && y - 1 + j < ySize)
                                        {
                                            for(int k = 0; k < 3; k++)
                                            {
                                                if(z - 1 + k >= 0 && z - 1 + k < zSize)
                                                {
                                                    if(voxelData[(x - 1 + i), (y - 1 + j), (z - 1 + k)] == 0)
                                                    {
                                                        nearby[i * 9 + j * 3 + k] = voxelData[(x - 1 + i), (y - 1 + j), (z - 1 + k)] != 0;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if(!AllArray(nearby))
                                data[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)z, color = mvd }, Slope.Cube);
                        }
                    }
                }
            }
            
            return data;
        }
        public static FaceVoxel[,,] GetFaces(byte[,,] voxelData)
        {
            return AddAll(GetCubes(voxelData));
        }

        public static FaceVoxel[,,] GetFaces(IEnumerable<MagicaVoxelData> voxelData, int xSize, int ySize, int zSize)
        {
            FaceVoxel[,,] data = new FaceVoxel[xSize, ySize, zSize];

            foreach(MagicaVoxelData mvd in voxelData)
            {
                data[mvd.x, mvd.y, mvd.z] = new FaceVoxel(mvd, Slope.Cube);
            }
            data = AddAll(data);

            return data;
        }

        public static FaceVoxel[,,] AddAll(FaceVoxel[,,] faces)
        {
            int xSize = faces.GetLength(0), ySize = faces.GetLength(1), zSize = faces.GetLength(2);
            FaceVoxel[,,] faces2 = new FaceVoxel[xSize, ySize, zSize];
            byte mvd;
            for(int z = zSize - 1; z >= 0; z--)
            {
                for(int x = 0; x < xSize - 1; x++)
                {
                    for(int y = ySize - 1; y >= 0; y--)
                    {
                        if(z == 0 || x == 0 || y == 0 || z == zSize - 1 || x == xSize - 1 || y == ySize - 1 || faces[x, y, z] != null)
                        {
                            faces2[x, y, z] = faces[x, y, z];
                        }
                        else
                        {
                            if(ImportantNeighborhood(faces, x, y, z))
                            {
                                continue;
                            }
                            bool xup = faces[x + 1, y, z] != null,
                                 xdown = faces[x - 1, y, z] != null,
                                 yup = faces[x, y + 1, z] != null,
                                 ydown = faces[x, y - 1, z] != null,
                                 zup = faces[x, y, z + 1] != null,
                                 zdown = faces[x, y, z - 1] != null;

                            if(zdown && !zup)
                            {
                                // faces[x, y, z - 1].vox.color;
                                mvd = NearbyFilled(faces, x, y, z);
                                if(ImportantVisual(mvd))
                                {
                                    faces2[x, y, z] = null;
                                    continue;
                                }
                                
                                if(!xdown && yup && !xup && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.BrightTop);
                                }
                                else if(xdown && !yup && !xup && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.DimTop);
                                }
                                else if(!xdown && !yup && xup && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.RearBrightTop);
                                }
                                else if(!xdown && !yup && !xup && ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.RearDimTop);
                                }
                                else if(xdown && yup && !xup && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.BrightDimTopThick);
                                }
                                else if(xup && yup && !xdown && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.BrightTopBackThick);
                                }
                                else if(!xup && !yup && xdown && ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.DimTopBackThick);
                                }
                                else if(!xdown && !yup && xup && ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.BackBackTopThick);
                                }
                            }
                            else if(zup)
                            {

                                mvd = NearbyFilled(faces, x, y, z);
                                if(ImportantVisual(mvd))
                                {
                                    faces2[x, y, z] = null;
                                    continue;
                                }

                                if(!xdown && yup && !xup && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.BrightBottom);
                                }
                                else if(xdown && !yup && !xup && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.DimBottom);
                                }
                                else if(!xdown && !yup && xup && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.RearBrightBottom);
                                }
                                else if(!xdown && !yup && !xup && ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.RearDimBottom);
                                }
                                else if(xdown && yup && !xup && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.BrightDimBottomThick);
                                }
                                else if(xup && yup && !xdown && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.BrightBottomBackThick);
                                }
                                else if(!xup && !yup && xdown && ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.DimBottomBackThick);
                                }
                                else if(!xdown && !yup && xup && ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.BackBackBottomThick);
                                }
                            }
                            else if(xdown)
                            {

                                mvd = NearbyFilled(faces, x, y, z);
                                if(ImportantVisual(mvd))
                                {
                                    faces2[x, y, z] = null;
                                    continue;
                                }


                                if(yup && !xup && !ydown) // && xdown
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.BrightDim);
                                }
                                else if(!xup && !yup && ydown) // && xdown
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.DimBack);
                                }

                            }
                            else if(xup)
                            {

                                mvd = NearbyFilled(faces, x, y, z);
                                if(ImportantVisual(mvd))
                                {
                                    faces2[x, y, z] = null;
                                    continue;
                                }


                                if(!xdown && !yup && ydown) // && xup
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.BackBack);
                                }
                                else if(yup && !xdown && !ydown) // && xup
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.BrightBack);
                                }
                            }
                        }
                    }
                }
            }
            return faces2;
        }

        public static FaceVoxel[,,] Overlap(FaceVoxel[,,] start, FaceVoxel[,,] adding)
        {
            FaceVoxel[,,] next = start.Replicate();
            int xSize = start.GetLength(0), ySize = start.GetLength(1), zSize = start.GetLength(2);
            int xSize2 = adding.GetLength(0), ySize2 = adding.GetLength(1), zSize2 = adding.GetLength(2);

            for(int z = 0; z < zSize && z < zSize2; z++)
            {
                for(int y = 0; y < ySize && y < ySize2; y++)
                {
                    for(int x = 0; x < xSize && x < xSize2; x++)
                    {
                        if(adding[x, y, z] != null && next[x, y, z] == null)
                            next[x, y, z] = adding[x, y, z];
                    }
                }
            }
            return next;
        }
        public static FaceVoxel[,,] OverlapAll(params FaceVoxel[][,,] adding)
        {
            return adding.Aggregate(Overlap);
        }
        public static FaceVoxel[,,] OverlapAll(IEnumerable<FaceVoxel[,,]> adding)
        {
            return adding.Aggregate(Overlap);
        }
        public static byte NearbyFilled(FaceVoxel[,,] voxelData, int x, int y, int z)
        {
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);
            byte[] nearby = new byte[27], counts = new byte[27];
            int running = 0, empty = 0;
            for(int i = Math.Max(0, x - 1); i <= x + 1 && i < xSize; i++)
            {
                for(int j = Math.Max(0, y - 1); j <= y + 1 && j < ySize; j++)
                {
                    for(int k = Math.Max(0, z - 1); k <= z + 1 && k < zSize; k++)
                    {
                        for(int c = 0; c <= running; c++)
                        {
                            if(voxelData[i, j, k] == null)
                            {
                                empty++;
                                break;
                            }
                            if(nearby[c] == voxelData[i, j, k].vox.color)
                            {
                                counts[c]++;
                                break;
                            }
                            if(c == running && (253 - voxelData[i, j, k].vox.color) % 4 == 0)
                            {
                                nearby[c] = voxelData[i, j, k].vox.color;
                                counts[c]++;
                                running++;
                                break;
                            }
                        }
                    }
                }
            }
            byte best = 0, bestCount = 0;
            for(int c = 0; c < running; c++)
            {
                if(counts[c] > bestCount)
                {
                    best = nearby[c];
                    bestCount = counts[c];
                }
                else if(bestCount > 0 && counts[c] == bestCount && best < nearby[c])
                {
                    best = nearby[c];
                }
            }
            return best;
        }
        public static byte NearbyColor(FaceVoxel[,,] voxelData, int x, int y, int z)
        {
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);
            byte[] nearby = new byte[27], counts = new byte[27];
            int running = 0, empty = 0;
            for(int i = Math.Max(0, x - 1); i <= x + 1 && i < xSize; i++)
            {
                for(int j = Math.Max(0, y - 1); j <= y + 1 && j < ySize; j++)
                {
                    for(int k = Math.Max(0, z - 1); k <= z + 1 && k < zSize; k++)
                    {
                        for(int c = 0; c <= running; c++)
                        {
                            if(voxelData[i, j, k] == null)
                            {
                                empty++;
                                break;
                            }
                            if(nearby[c] == voxelData[i, j, k].vox.color)
                            {
                                counts[c]++;
                                break;
                            }
                            if(c == running && (253 - voxelData[i, j, k].vox.color) % 4 == 0)
                            {
                                nearby[c] = voxelData[i, j, k].vox.color;
                                counts[c]++;
                                running++;
                                break;
                            }
                        }
                    }
                }
            }
            byte best = 0, bestCount = 0;
            for(int c = 0; c < running; c++)
            {
                if(counts[c] > bestCount)
                {
                    best = nearby[c];
                    bestCount = counts[c];
                }
            }
            if(empty > bestCount)
                return 0;
            return best;
        }
        public static bool MixedMechanical(FaceVoxel[,,] voxelData, int x, int y, int z)
        {
            if(VoxelLogic.VisualMode != "CU")
                return false;
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);
            byte mvd;
            bool foundMech = false, foundOther = false;
            for(int i = Math.Max(0, x - 1); i <= x + 1 && i < xSize; i++)
            {
                for(int j = Math.Max(0, y - 1); j <= y + 1 && j < ySize; j++)
                {
                    for(int k = Math.Max(0, z - 1); k <= z + 1 && k < zSize; k++)
                    {
                        if(voxelData[i, j, k] != null)
                        {
                            mvd = voxelData[i, j, k].vox.color;
                            if(mvd == 253 - 0 * 4 || mvd == 253 - 1 * 4 || mvd == 253 - 14 * 4 || mvd == 253 - 15 * 4)
                                foundMech = true;
                            else if(mvd != 0 && mvd != CURedux.shadow && mvd != CURedux.inner_shadow && mvd > CURedux.emitter0)
                                foundOther = true;
                        }
                    }
                }
            }
            return foundMech && foundOther;
        }
        

        public static FaceVoxel[,,] GetAlteredFaces(byte[,,] voxelData)
        {
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);
            FaceVoxel[,,] faces = new FaceVoxel[xSize, ySize, zSize];
            bool[] nearby = new bool[27];
            FaceVoxel[] nearFaces = new FaceVoxel[27];
            for(int z = 0; z < zSize; z++)
            {
                for(int x = 0; x < xSize; x++)
                {
                    for(int y = 0; y < ySize; y++)
                    {
                        //if(x == 19 && y == 11 && z == 23)
                        //    Console.Write("!");
                        byte color = voxelData[x, y, z];
                        if(color > 0 && (VoxelLogic.VisualMode != "CU" || color > CURedux.emitter0))
                        {
                            MagicaVoxelData mvd = new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)z, color = color };
                            
                            if(ImportantNeighborhood(voxelData, x, y, z))
                            {
                                faces[x, y, z] = new FaceVoxel(mvd, Slope.Cube);

                                continue;
                            }
                            nearby.Fill(false);
                            for(int i = 0; i < 3 && x - 1 + i < xSize; i++)
                            {
                                if(x > 0 || i > 0)
                                {
                                    for(int j = 0; j < 3 && y - 1 + j < ySize; j++)
                                    {
                                        if(y > 0 || j > 0)
                                        {
                                            for(int k = 0; k < 3 && z - 1 + k < zSize; k++)
                                            {
                                                if(z > 0 || k > 0)
                                                {
                                                    nearby[i * 9 + j * 3 + k] = voxelData[(x - 1 + i), (y - 1 + j), (z - 1 + k)] != 0;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            bool xup = nearby[18 + 3 + 1],
                                 xdown = nearby[0 + 3 + 1],
                                 yup = nearby[9 + 6 + 1],
                                 ydown = nearby[9 + 0 + 1],
                                 zup = nearby[9 + 3 + 2],
                                 zdown = nearby[9 + 3 + 0];

                            if(zdown && zup)
                            {
                                if(xdown && xup)
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.Cube);
                                }
                                else if(ydown && yup)
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.Cube);
                                }
                                else if(xdown && ydown && !xup && !yup)
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.DimBack);
                                }
                                else if(xup && yup && !xdown && !ydown)
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.BrightBack);
                                }
                                else if(xdown && yup && !xup && !ydown)
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.BrightDim);
                                }
                                else if(xup && ydown && !xdown && !yup)
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.BackBack);
                                }
                                else
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.Cube);
                                }
                            }
                            else if(!zup)
                            {

                                if(xdown && yup && xup && ydown)
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.Cube);
                                }
                                else if(!xup && !ydown && nearby[0 + 3 + 2] && nearby[9 + 6 + 2] && !nearby[18 + 3 + 2] && !nearby[9 + 0 + 2])
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.BrightDimTop);
                                }
                                //else if(xup && yup && !xdown && !ydown)
                                else if(!xdown && !ydown && !nearby[0 + 3 + 2] && nearby[9 + 6 + 2] && nearby[18 + 3 + 2] && !nearby[9 + 0 + 2])
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.BrightTopBack);
                                }
                                //else if(!xup && !yup && xdown && ydown)
                                else if(!xup && !yup && nearby[0 + 3 + 2] && !nearby[9 + 6 + 2] && !nearby[18 + 3 + 2] && nearby[9 + 0 + 2])
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.DimTopBack);
                                }
                                //else if(!xdown && !yup && xup && ydown)
                                else if(!xdown && !yup && !nearby[0 + 3 + 2] && !nearby[9 + 6 + 2] && nearby[18 + 3 + 2] && nearby[9 + 0 + 2])
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.BackBackTop);
                                }

                                else if(!xup && !ydown && nearby[18 + 3 + 0] && nearby[9 + 0 + 0])
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.BrightDimTop);
                                }
                                //else if(xup && yup && !xdown && !ydown)
                                else if(!xdown && !ydown && nearby[9 + 0 + 0] && nearby[0 + 3 + 0])
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.BrightTopBack);
                                }
                                //else if(!xup && !yup && xdown && ydown)
                                else if(!xup && !yup && nearby[18 + 3 + 0] && nearby[9 + 6 + 0])
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.DimTopBack);
                                }
                                //else if(!xdown && !yup && xup && ydown)
                                else if(!xdown && !yup && nearby[0 + 3 + 0] && nearby[9 + 6 + 0])
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.BackBackTop);
                                }

                                else if(!ydown && ((nearby[9 + 6 + 2] && !nearby[9 + 0 + 2])))
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.BrightTop);
                                }
                                else if(!xup && ((nearby[0 + 3 + 2] && !nearby[18 + 3 + 2])))
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.DimTop);
                                }
                                //else if(!xdown && !yup && xup && !ydown)
                                else if(!xdown && ((nearby[18 + 3 + 2] && !nearby[0 + 3 + 2])))
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.RearBrightTop);
                                }
                                //else if(!xdown && !yup && !xup && ydown)
                                else if(!yup && ((nearby[9 + 0 + 2] && !nearby[9 + 6 + 2])))
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.RearDimTop);
                                }


                                else if(!ydown && yup) // && ((nearby[9 + 0 + 0]))
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.BrightTop);
                                }
                                else if(!xup && xdown) // && ((nearby[18 + 3 + 0]))
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.DimTop);
                                }
                                //else if(!xdown && !yup && xup && !ydown)
                                else if(!xdown && xup) // && ((nearby[0 + 3 + 0]))
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.RearBrightTop);
                                }
                                //else if(!xdown && !yup && !xup && ydown)
                                else if(!yup && ydown) // && ((nearby[9 + 6 + 0]))
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.RearDimTop);
                                }
                                else
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.Cube);
                                }
                                /*
                                
                                else if(!ydown && ((nearby[9 + 6 + 2] && !nearby[9 + 0 + 2]) || (nearby[9 + 0 + 0] && yup)))
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.BrightTop);
                                }
                                else if(!xup && ((nearby[0 + 3 + 2] && !nearby[18 + 3 + 2]) || (nearby[18 + 3 + 0] && xdown)))
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.DimTop);
                                }
                                //else if(!xdown && !yup && xup && !ydown)
                                else if(!xdown && ((nearby[18 + 3 + 2] && !nearby[0 + 3 + 2]) || (nearby[0 + 3 + 0] && xup)))
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.RearBrightTop);
                                }
                                //else if(!xdown && !yup && !xup && ydown)
                                else if(!yup && ((nearby[9 + 0 + 2] && !nearby[9 + 6 + 2]) || (nearby[9 + 6 + 0] && ydown)))
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.RearDimTop);
                                }
                                */
                            }
                            else // if(zup)
                            {
                                if(xdown && yup && xup && ydown)
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.Cube);
                                }

                                //else if(xdown && yup && !xup && !ydown)
                                else if(!xup && !ydown && nearby[0 + 3 + 0] && nearby[9 + 6 + 0] && !nearby[18 + 3 + 0] && !nearby[9 + 0 + 0])
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.BrightDimBottom);
                                }
                                //else if(xup && yup && !xdown && !ydown)
                                else if(!xdown && !ydown && !nearby[0 + 3 + 0] && nearby[9 + 6 + 0] && nearby[18 + 3 + 0] && !nearby[9 + 0 + 0])
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.BrightBottomBack);
                                }
                                //else if(!xup && !yup && xdown && ydown)
                                else if(!xup && !yup && nearby[0 + 3 + 0] && !nearby[9 + 6 + 0] && !nearby[18 + 3 + 0] && nearby[9 + 0 + 0])
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.DimBottomBack);
                                }
                                //else if(!xdown && !yup && xup && ydown)
                                else if(!xdown && !yup && !nearby[0 + 3 + 0] && !nearby[9 + 6 + 0] && nearby[18 + 3 + 0] && nearby[9 + 0 + 0])
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.BackBackBottom);
                                }

                                //else if(xdown && yup && !xup && !ydown)
                                else if(!xup && !ydown && nearby[18 + 3 + 2] && nearby[9 + 0 + 2])
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.BrightDimBottom);
                                }
                                //else if(xup && yup && !xdown && !ydown)
                                else if(!xdown && !ydown && nearby[9 + 0 + 2] && nearby[0 + 3 + 2])
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.BrightBottomBack);
                                }
                                //else if(!xup && !yup && xdown && ydown)
                                else if(!xup && !yup && nearby[18 + 3 + 2] && nearby[9 + 6 + 2])
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.DimBottomBack);
                                }
                                //else if(!xdown && !yup && xup && ydown)
                                else if(!xdown && !yup && nearby[0 + 3 + 2] && nearby[9 + 6 + 2])
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.BackBackBottom);
                                }

                                //else if(!xdown && yup && !xup && !ydown)
                                else if(!ydown && ((nearby[9 + 6 + 0] && !nearby[9 + 0 + 0])))
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.BrightBottom);
                                }
                                //else if(xdown && !yup && !xup && !ydown)
                                else if(!xup && ((nearby[0 + 3 + 0] && !nearby[18 + 3 + 0])))
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.DimBottom);
                                }
                                //else if(!xdown && !yup && xup && !ydown)
                                else if(!xdown && ((nearby[18 + 3 + 0] && !nearby[0 + 3 + 0])))
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.RearBrightBottom);
                                }
                                //else if(!xdown && !yup && !xup && ydown)
                                else if(!yup && ((nearby[9 + 0 + 0] && !nearby[9 + 6 + 0])))
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.RearDimBottom);
                                }

                                else if(!ydown && yup) // && ((nearby[9 + 0 + 0]))
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.BrightBottom);
                                }
                                else if(!xup && xdown) // && ((nearby[18 + 3 + 0]))
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.DimBottom);
                                }
                                //else if(!xdown && !yup && xup && !ydown)
                                else if(!xdown && xup) // && ((nearby[0 + 3 + 0]))
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.RearBrightBottom);
                                }
                                //else if(!xdown && !yup && !xup && ydown)
                                else if(!yup && ydown) // && ((nearby[9 + 6 + 0]))
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.RearDimBottom);
                                }

                                /*
                                else if(!ydown && ((nearby[9 + 0 + 2] && yup)))
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.BrightBottom);
                                }
                                //else if(xdown && !yup && !xup && !ydown)
                                else if(!xup && ((nearby[18 + 3 + 2] && xdown)))
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.DimBottom);
                                }
                                //else if(!xdown && !yup && xup && !ydown)
                                else if(!xdown && ((nearby[0 + 3 + 2] && xup)))
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.RearBrightBottom);
                                }
                                //else if(!xdown && !yup && !xup && ydown)
                                else if(!yup && ((nearby[9 + 6 + 2] && ydown)))
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.RearDimBottom);
                                }
                                */
                                else
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.Cube);
                                }
                                /*
                                else if(!ydown && ((nearby[9 + 6 + 0] && !nearby[9 + 0 + 0]) || (nearby[9 + 0 + 2] && yup)))
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.BrightBottom);
                                }
                                //else if(xdown && !yup && !xup && !ydown)
                                else if(!xup && ((nearby[0 + 3 + 0] && !nearby[18 + 3 + 0]) || (nearby[18 + 3 + 2] && xdown)))
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.DimBottom);
                                }
                                //else if(!xdown && !yup && xup && !ydown)
                                else if(!xdown && ((nearby[18 + 3 + 0] && !nearby[0 + 3 + 0]) || (nearby[0 + 3 + 2] && xup)))
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.RearBrightBottom);
                                }
                                //else if(!xdown && !yup && !xup && ydown)
                                else if(!yup && ((nearby[9 + 0 + 0] && !nearby[9 + 6 + 0]) || (nearby[9 + 6 + 2] && ydown)))
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.RearDimBottom);
                                }

                                */
                            }
                            /*
                            else
                            {
                                if(xdown && yup && xup && ydown)
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.Cube);
                                }
                                else if(xdown && yup && !xup && !ydown)
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.BrightDim);
                                }
                                else if(!xdown && !yup && xup && ydown)
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.BackBack);
                                }
                                else if(xup && yup && !xdown && !ydown)
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.BrightBack);
                                }
                                else if(!xup && !yup && xdown && ydown)
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.DimBack);
                                }
                                else
                                {
                                    faces[x, y, z] = new FaceVoxel(mvd, Slope.Cube);
                                }
                            }
                            */
                        }
                    }
                }
            }
            FaceVoxel[,,] faces2 = new FaceVoxel[xSize,ySize,zSize];
            
            for(int z = 0; z < zSize; z++)
            {
                for(int x = 0; x < xSize; x++)
                {
                    for(int y = 0; y < ySize; y++)
                    {

                        if(ImportantNeighborhood(faces, x, y, z))
                        {
                            faces2[x, y, z] = faces[x, y, z];

                            continue;
                        }
                        /*
                        case Slope.BackBackBottom:
                        case Slope.BackBackTop:
                                                        case Slope.BrightBottomBack:
                                                        case Slope.BrightDimBottom:
                                                        case Slope.BrightDimTop:
                                                        case Slope.BrightTopBack:
                                                        case Slope.DimBottomBack:
                                                        case Slope.DimTopBack:

                        */

                        nearFaces.Fill(null);
                        nearby.Fill(false);
                        for(int i = 0; i < 3; i++)
                        {
                            if(x - 1 + i >= 0 && x - 1 + i < xSize)
                            {
                                for(int j = 0; j < 3; j++)
                                {
                                    if(y - 1 + j >= 0 && y - 1 + j < ySize)
                                    {
                                        for(int k = 0; k < 3; k++)
                                        {
                                            if(z - 1 + k >= 0 && z - 1 + k < zSize)
                                            {
                                                if(faces[(x - 1 + i), (y - 1 + j), (z - 1 + k)] != null)
                                                {
                                                    nearby[i * 9 + j * 3 + k] = faces[(x - 1 + i), (y - 1 + j), (z - 1 + k)] != null;
                                                    nearFaces[i * 9 + j * 3 + k] = faces[(x - 1 + i), (y - 1 + j), (z - 1 + k)];
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        
                        faces2[x, y, z] = faces[x, y, z];
                        FaceVoxel f = null;
                        if((f = SmoothPeak(faces, x, y, z, nearFaces, 18, 0, 2, Slope.BrightDimTopThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = SmoothPeak(faces, x, y, z, nearFaces, 0, 0, 2, Slope.BrightTopBackThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = SmoothPeak(faces, x, y, z, nearFaces, 0, 6, 2, Slope.BackBackTopThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = SmoothPeak(faces, x, y, z, nearFaces, 18, 6, 2, Slope.DimTopBackThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = SmoothPeak(faces, x, y, z, nearFaces, 18, 0, 0, Slope.BrightDimBottomThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = SmoothPeak(faces, x, y, z, nearFaces, 0, 0, 0, Slope.BrightBottomBackThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = SmoothPeak(faces, x, y, z, nearFaces, 0, 6, 0, Slope.BackBackBottomThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = SmoothPeak(faces, x, y, z, nearFaces, 18, 6, 0, Slope.DimBottomBackThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }

                        else if((f = SmoothPeakBackup(faces, x, y, z, nearFaces, 18, 0, 2, Slope.BrightDimTopThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = SmoothPeakBackup(faces, x, y, z, nearFaces, 0, 0, 2, Slope.BrightTopBackThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = SmoothPeakBackup(faces, x, y, z, nearFaces, 0, 6, 2, Slope.BackBackTopThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = SmoothPeakBackup(faces, x, y, z, nearFaces, 18, 6, 2, Slope.DimTopBackThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = SmoothPeakBackup(faces, x, y, z, nearFaces, 18, 0, 0, Slope.BrightDimBottomThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = SmoothPeakBackup(faces, x, y, z, nearFaces, 0, 0, 0, Slope.BrightBottomBackThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = SmoothPeakBackup(faces, x, y, z, nearFaces, 0, 6, 0, Slope.BackBackBottomThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = SmoothPeakBackup(faces, x, y, z, nearFaces, 18, 6, 0, Slope.DimBottomBackThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if(AllArray(nearby))
                        {
                            faces2[x, y, z] = null;
                        }

                        /*

                    if((xup && ydown && zup && !nearby[18 + 0 + 2]) && (xupv.slope != Slope.Cube && ydownv.slope != Slope.Cube && zupv.slope != Slope.Cube))
                        {
                            if(faces[x, y, z] != null)
                                faces2[x, y, z].slope = Slope.BrightDimTopThick;
                            else
                                faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData(x, y, z, Choose(xupv.vox.color, ydownv.vox.color, zupv.vox.color)), Slope.BrightDimTopThick);
                        }
                        else if((xdown && ydown && zup && !nearby[0 + 0 + 2]) && (xdownv.slope != Slope.Cube && ydownv.slope != Slope.Cube && zupv.slope != Slope.Cube))
                        {
                            if(faces[x, y, z] != null)
                                faces2[x, y, z].slope = Slope.BrightTopBackThick;
                            else
                                faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData(x, y, z, Choose(xdownv.vox.color, ydownv.vox.color, zupv.vox.color)), Slope.BrightTopBackThick);
                        }
                        else if((xdown && yup && zup && !nearby[0 + 6 + 2]) && (xdownv.slope != Slope.Cube && yupv.slope != Slope.Cube && zupv.slope != Slope.Cube))
                        {
                            if(faces[x, y, z] != null)
                                faces2[x, y, z].slope = Slope.BackBackTopThick;
                            else
                                faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData(x, y, z, Choose(xdownv.vox.color, yupv.vox.color, zupv.vox.color)), Slope.BackBackTopThick);
                        }
                        else if((xup && yup && zup && !nearby[18 + 6 + 2]) && (xupv.slope != Slope.Cube && yupv.slope != Slope.Cube && zupv.slope != Slope.Cube))
                        {
                            if(faces[x, y, z] != null)
                                faces2[x, y, z].slope = Slope.DimTopBackThick;
                            else
                                faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData(x, y, z, Choose(xupv.vox.color, yupv.vox.color, zupv.vox.color)), Slope.DimTopBackThick);
                        }

                        else if((xup && ydown && zdown && !nearby[18 + 0 + 0]) && (xupv.slope != Slope.Cube && ydownv.slope != Slope.Cube && zdownv.slope != Slope.Cube))
                        {
                            if(faces[x, y, z] != null)
                                faces2[x, y, z].slope = Slope.BrightDimBottomThick;
                            else
                                faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData(x, y, z, Choose(xupv.vox.color, ydownv.vox.color, zdownv.vox.color)), Slope.BrightDimBottomThick);
                        }
                        else if((xdown && ydown && zdown && !nearby[0 + 0 + 0]) && (xdownv.slope != Slope.Cube && ydownv.slope != Slope.Cube && zdownv.slope != Slope.Cube))
                        {
                            if(faces[x, y, z] != null)
                                faces2[x, y, z].slope = Slope.BrightBottomBackThick;
                            else
                                faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData(x, y, z, Choose(xdownv.vox.color, ydownv.vox.color, zdownv.vox.color)), Slope.BrightBottomBackThick);
                        }
                        else if((xdown && yup && zdown && !nearby[0 + 6 + 0]) && (xdownv.slope != Slope.Cube && yupv.slope != Slope.Cube && zdownv.slope != Slope.Cube))
                        {
                            if(faces[x, y, z] != null)
                                faces2[x, y, z].slope = Slope.BackBackBottomThick;
                            else
                                faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData(x, y, z, Choose(xdownv.vox.color, yupv.vox.color, zdownv.vox.color)), Slope.BackBackBottomThick);
                        }
                        else if((xup && yup && zdown && !nearby[18 + 6 + 0]) && (xupv.slope != Slope.Cube && yupv.slope != Slope.Cube && zdownv.slope != Slope.Cube))
                        {
                            if(faces[x, y, z] != null)
                                faces2[x, y, z].slope = Slope.DimBottomBackThick;
                            else
                                faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData(x, y, z, Choose(xupv.vox.color, yupv.vox.color, zdownv.vox.color)), Slope.DimBottomBackThick);
                        }
                        else if(AllArray(nearby))
                        {
                            faces2[x, y, z] = null;
                        }

                        */

                        /*
                        if(zdown && !zup)
                        {

                            if(xdown && yup && !xup && !ydown)
                            {
                                faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x, y, z - 1].vox.color }, Slope.BrightDimTop);
                            }
                            else if(xup && yup && !xdown && !ydown)
                            {
                                faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x, y, z - 1].vox.color }, Slope.BrightTopBack);
                            }
                            else if(!xup && !yup && xdown && ydown)
                            {
                                faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x, y, z - 1].vox.color }, Slope.DimTopBack);
                            }
                            else if(!xdown && !yup && xup && ydown)
                            {
                                faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x, y, z - 1].vox.color }, Slope.BackBackTop);
                            }
                        }
                        else if(zup)
                        {

                            if(xdown && yup && !xup && !ydown)
                            {
                                faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x, y, z + 1].vox.color }, Slope.BrightDimBottom);
                            }
                            else if(xup && yup && !xdown && !ydown)
                            {
                                faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x, y, z + 1].vox.color }, Slope.BrightBottomBack);
                            }
                            else if(!xup && !yup && xdown && ydown)
                            {
                                faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x, y, z + 1].vox.color }, Slope.DimBottomBack);
                            }
                            else if(!xdown && !yup && xup && ydown)
                            {
                                faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x, y, z + 1].vox.color }, Slope.BackBackBottom);
                            }
                        }
                        */
                    }
                }
            }


            return faces2;
        }


        public static FaceVoxel[,,] SmoothAll(FaceVoxel[,,] faces)

        {
            int xSize = faces.GetLength(0), ySize = faces.GetLength(1), zSize = faces.GetLength(2);

            FaceVoxel[,,] faces2 = new FaceVoxel[xSize, ySize, zSize];
            bool[] nearby = new bool[27];
            FaceVoxel[] nearFaces = new FaceVoxel[27];

            for(int z = 0; z < zSize; z++)
            {
                for(int x = 0; x < xSize; x++)
                {
                    for(int y = 0; y < ySize; y++)
                    {

                        if(ImportantNeighborhood(faces, x, y, z))
                        {
                            faces2[x, y, z] = faces[x, y, z];

                            continue;
                        }
                        /*
                        case Slope.BackBackBottom:
                        case Slope.BackBackTop:
                                                        case Slope.BrightBottomBack:
                                                        case Slope.BrightDimBottom:
                                                        case Slope.BrightDimTop:
                                                        case Slope.BrightTopBack:
                                                        case Slope.DimBottomBack:
                                                        case Slope.DimTopBack:

                        */

                        nearFaces.Fill(null);
                        nearby.Fill(false);
                        for(int i = 0; i < 3; i++)
                        {
                            if(x - 1 + i >= 0 && x - 1 + i < xSize)
                            {
                                for(int j = 0; j < 3; j++)
                                {
                                    if(y - 1 + j >= 0 && y - 1 + j < ySize)
                                    {
                                        for(int k = 0; k < 3; k++)
                                        {
                                            if(z - 1 + k >= 0 && z - 1 + k < zSize)
                                            {
                                                if(faces[(x - 1 + i), (y - 1 + j), (z - 1 + k)] != null)
                                                {
                                                    nearby[i * 9 + j * 3 + k] = faces[(x - 1 + i), (y - 1 + j), (z - 1 + k)] != null;
                                                    nearFaces[i * 9 + j * 3 + k] = faces[(x - 1 + i), (y - 1 + j), (z - 1 + k)];
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        faces2[x, y, z] = faces[x, y, z];
                        FaceVoxel f = null;
                        if((f = SmoothPeak(faces, x, y, z, nearFaces, 18, 0, 2, Slope.BrightDimTopThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = SmoothPeak(faces, x, y, z, nearFaces, 0, 0, 2, Slope.BrightTopBackThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = SmoothPeak(faces, x, y, z, nearFaces, 0, 6, 2, Slope.BackBackTopThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = SmoothPeak(faces, x, y, z, nearFaces, 18, 6, 2, Slope.DimTopBackThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = SmoothPeak(faces, x, y, z, nearFaces, 18, 0, 0, Slope.BrightDimBottomThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = SmoothPeak(faces, x, y, z, nearFaces, 0, 0, 0, Slope.BrightBottomBackThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = SmoothPeak(faces, x, y, z, nearFaces, 0, 6, 0, Slope.BackBackBottomThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = SmoothPeak(faces, x, y, z, nearFaces, 18, 6, 0, Slope.DimBottomBackThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }

                        else if((f = SmoothPeakBackup(faces, x, y, z, nearFaces, 18, 0, 2, Slope.BrightDimTopThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = SmoothPeakBackup(faces, x, y, z, nearFaces, 0, 0, 2, Slope.BrightTopBackThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = SmoothPeakBackup(faces, x, y, z, nearFaces, 0, 6, 2, Slope.BackBackTopThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = SmoothPeakBackup(faces, x, y, z, nearFaces, 18, 6, 2, Slope.DimTopBackThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = SmoothPeakBackup(faces, x, y, z, nearFaces, 18, 0, 0, Slope.BrightDimBottomThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = SmoothPeakBackup(faces, x, y, z, nearFaces, 0, 0, 0, Slope.BrightBottomBackThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = SmoothPeakBackup(faces, x, y, z, nearFaces, 0, 6, 0, Slope.BackBackBottomThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = SmoothPeakBackup(faces, x, y, z, nearFaces, 18, 6, 0, Slope.DimBottomBackThick)) != null)
                        {
                            faces2[x, y, z] = f;
                        }/*
                        else if(AllArray(nearby))
                        {
                            faces2[x, y, z] = null;
                        }*/

                    }
                }
            }


            return faces2;

        }

        public static FaceVoxel[,,] FillAllGaps(FaceVoxel[,,] faces)

        {
            int xSize = faces.GetLength(0), ySize = faces.GetLength(1), zSize = faces.GetLength(2);

            FaceVoxel[,,] faces2 = new FaceVoxel[xSize, ySize, zSize];
            bool[] nearby = new bool[27];
            FaceVoxel[] nearFaces = new FaceVoxel[27];

            for(int z = 0; z < zSize; z++)
            {
                for(int x = 0; x < xSize; x++)
                {
                    for(int y = 0; y < ySize; y++)
                    {

                        if(ImportantNeighborhood(faces, x, y, z))
                        {
                            faces2[x, y, z] = faces[x, y, z];

                            continue;
                        }
                        /*
                        case Slope.BackBackBottom:
                        case Slope.BackBackTop:
                                                        case Slope.BrightBottomBack:
                                                        case Slope.BrightDimBottom:
                                                        case Slope.BrightDimTop:
                                                        case Slope.BrightTopBack:
                                                        case Slope.DimBottomBack:
                                                        case Slope.DimTopBack:

                        */

                        nearFaces.Fill(null);
                        nearby.Fill(false);
                        for(int i = 0; i < 3; i++)
                        {
                            if(x - 1 + i >= 0 && x - 1 + i < xSize)
                            {
                                for(int j = 0; j < 3; j++)
                                {
                                    if(y - 1 + j >= 0 && y - 1 + j < ySize)
                                    {
                                        for(int k = 0; k < 3; k++)
                                        {
                                            if(z - 1 + k >= 0 && z - 1 + k < zSize)
                                            {
                                                if(faces[(x - 1 + i), (y - 1 + j), (z - 1 + k)] != null)
                                                {
                                                    nearby[i * 9 + j * 3 + k] = faces[(x - 1 + i), (y - 1 + j), (z - 1 + k)] != null;
                                                    nearFaces[i * 9 + j * 3 + k] = faces[(x - 1 + i), (y - 1 + j), (z - 1 + k)];
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        faces2[x, y, z] = faces[x, y, z];
                        FaceVoxel f = null;
                        if((f = FillGap(faces, x, y, z, nearFaces, 0, 6, 0, Slope.BrightDimTop)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = FillGap(faces, x, y, z, nearFaces, 18, 6, 0, Slope.BrightTopBack)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = FillGap(faces, x, y, z, nearFaces, 18, 0, 0, Slope.BackBackTop)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = FillGap(faces, x, y, z, nearFaces, 0, 0, 0, Slope.DimTopBack)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = FillGap(faces, x, y, z, nearFaces, 0, 6, 2, Slope.BrightDimBottom)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = FillGap(faces, x, y, z, nearFaces, 18, 6, 2, Slope.BrightBottomBack)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = FillGap(faces, x, y, z, nearFaces, 18, 0, 2, Slope.BackBackBottom)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = FillGap(faces, x, y, z, nearFaces, 0, 0, 2, Slope.DimBottomBack)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        /*
                        else if((f = FillGapBackup(faces, x, y, z, nearFaces, 0, 6, 0, Slope.BrightDimTop)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = FillGapBackup(faces, x, y, z, nearFaces, 18, 6, 0, Slope.BrightTopBack)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = FillGapBackup(faces, x, y, z, nearFaces, 18, 0, 0, Slope.BackBackTop)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = FillGapBackup(faces, x, y, z, nearFaces, 0, 0, 0, Slope.DimTopBack)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = FillGapBackup(faces, x, y, z, nearFaces, 0, 6, 2, Slope.BrightDimBottom)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = FillGapBackup(faces, x, y, z, nearFaces, 18, 6, 2, Slope.BrightBottomBack)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = FillGapBackup(faces, x, y, z, nearFaces, 18, 0, 2, Slope.BackBackBottom)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        else if((f = FillGapBackup(faces, x, y, z, nearFaces, 0, 0, 2, Slope.DimBottomBack)) != null)
                        {
                            faces2[x, y, z] = f;
                        }
                        */
                        /*
                        else if(AllArray(nearby))
                        {
                            faces2[x, y, z] = null;
                        }*/

                    }
                }
            }


            return faces2;

        }

        public static FaceVoxel SmoothPeak(FaceVoxel[,,] faces, int x, int y, int z, FaceVoxel[] nearby, int a, int b, int c, Slope targetSlope)
        {
            FaceVoxel aa = nearby[a + 3 + 1], bb = nearby[9 + b + 1], cc = nearby[9 + 3 + c], grr = nearby[a + b + c], o = faces[x, y, z];
            if(o == null)
                return null;

            o = new FaceVoxel(o.vox, o.slope);

            if((aa != null && bb != null && cc != null && grr == null) && (aa.slope != Slope.Cube && bb.slope != Slope.Cube && cc.slope != Slope.Cube))
            {
                if(o != null)
                {
                    o.slope = targetSlope;
                    o.vox.color = (byte)Choose(aa.vox.color, bb.vox.color, cc.vox.color);
                }
                else
                    o = new FaceVoxel(new MagicaVoxelData(x, y, z, Choose(aa.vox.color, bb.vox.color, cc.vox.color)), targetSlope);
                return o;
            }
            return null;
        }
        public static FaceVoxel SmoothPeakBackup(FaceVoxel[,,] faces, int x, int y, int z, FaceVoxel[] nearby, int a, int b, int c, Slope targetSlope)
        {
            FaceVoxel aa = nearby[a + 3 + 1], bb = nearby[9 + b + 1], cc = nearby[9 + 3 + c], grr = nearby[a + b + c], o = faces[x, y, z];
            if(o == null)
                return null;

            o = new FaceVoxel(o.vox, o.slope);
            
            if((aa != null && bb != null && grr == null) && (aa.slope != Slope.Cube && bb.slope != Slope.Cube))
            {
                if(o != null)
                {
                    o.slope = targetSlope;
                    o.vox.color = aa.vox.color;
                }
                else
                    o = new FaceVoxel(new MagicaVoxelData(x, y, z, aa.vox.color), targetSlope);
                return o;
            }
            else if((aa != null && cc != null && grr == null) && (aa.slope != Slope.Cube && cc.slope != Slope.Cube))
            {
                if(o != null)
                {
                    o.slope = targetSlope;
                    o.vox.color = cc.vox.color;
                }
                else
                    o = new FaceVoxel(new MagicaVoxelData(x, y, z, cc.vox.color), targetSlope);
                return o;
            }
            else if((bb != null && cc != null && grr == null) && (bb.slope != Slope.Cube && cc.slope != Slope.Cube))
            {
                if(o != null)
                {
                    o.slope = targetSlope;
                    o.vox.color = bb.vox.color;
                }
                else
                    o = new FaceVoxel(new MagicaVoxelData(x, y, z, bb.vox.color), targetSlope);
                return o;
            }
            return null;
        }
        public static int Choose(int a, int b, int c)
        {
            if(a == b)
                return a;
            if(a == c)
                return a;
            if(b == c)
                return b;
            return a;
        }
        public static bool AllArray(bool[] checks)
        {
            for(int i = 0; i < checks.Length; i++)
            {
                if(!checks[i])
                    return false;
            }
            return true;
        }


        public static FaceVoxel FillGap(FaceVoxel[,,] faces, int x, int y, int z, FaceVoxel[] nearby, int a, int b, int c, Slope targetSlope)
        {
            FaceVoxel aa = nearby[a + 3 + 1], bb = nearby[9 + b + 1], cc = nearby[9 + 3 + c], grr = nearby[18 - a + 6 - b + 2 - c], need = nearby[a + b + c], o = faces[x, y, z];
            if(o != null)
                return null;
            
            if(aa != null && bb != null && cc != null && grr == null && need != null)// && aa.slope != Slope.Cube && bb.slope != Slope.Cube && cc.slope != Slope.Cube))
            {
                switch(targetSlope)
                {
                    case Slope.BrightDimTop:
                        if(aa.slope != Slope.BrightDimTopThick && aa.slope != Slope.Cube && aa.slope != Slope.BrightTop && aa.slope != Slope.BrightTopBack && aa.slope != Slope.BrightTopBackThick)
                            return null;
                        if(bb.slope != Slope.BrightDimTopThick && bb.slope != Slope.Cube && bb.slope != Slope.DimTop && bb.slope != Slope.DimTopBack && bb.slope != Slope.DimTopBackThick)
                            return null;
                        if(cc.slope != Slope.BrightDimTopThick && cc.slope != Slope.Cube && cc.slope != Slope.BrightDim && cc.slope != Slope.BrightDimBottom && cc.slope != Slope.BrightDimBottomThick)
                            return null;
                        break;
                    case Slope.BackBackTop:
                        if(aa.slope != Slope.BackBackTopThick && aa.slope != Slope.Cube && aa.slope != Slope.RearDimTop && aa.slope != Slope.DimTopBack && aa.slope != Slope.DimTopBackThick)
                            return null;
                        if(bb.slope != Slope.BackBackTopThick && bb.slope != Slope.Cube && bb.slope != Slope.RearBrightTop && bb.slope != Slope.BrightTopBack && bb.slope != Slope.BrightTopBackThick)
                            return null;
                        if(cc.slope != Slope.BackBackTopThick && cc.slope != Slope.Cube && cc.slope != Slope.BackBack && cc.slope != Slope.BackBackBottom && cc.slope != Slope.BackBackBottomThick)
                            return null;
                        break;
                    case Slope.BrightTopBack:
                        if(aa.slope != Slope.BrightTopBackThick && aa.slope != Slope.Cube && aa.slope != Slope.BrightTop && aa.slope != Slope.BrightDimTop && aa.slope != Slope.BrightDimTopThick)
                            return null;
                        if(bb.slope != Slope.BrightTopBackThick && bb.slope != Slope.Cube && bb.slope != Slope.RearBrightTop && bb.slope != Slope.BackBackTop && bb.slope != Slope.BackBackTopThick)
                            return null;
                        if(cc.slope != Slope.BrightTopBackThick && cc.slope != Slope.Cube && cc.slope != Slope.BrightBack && cc.slope != Slope.BrightBottomBack && cc.slope != Slope.BrightBottomBackThick)
                            return null;
                        break;
                    case Slope.DimTopBack:
                        if(aa.slope != Slope.DimTopBackThick && aa.slope != Slope.Cube && aa.slope != Slope.RearDimTop && aa.slope != Slope.BackBackTop && aa.slope != Slope.BackBackTopThick)
                            return null;
                        if(bb.slope != Slope.DimTopBackThick && bb.slope != Slope.Cube && bb.slope != Slope.DimTop && bb.slope != Slope.BrightDimTop && bb.slope != Slope.BrightDimTopThick)
                            return null;
                        if(cc.slope != Slope.DimTopBackThick && cc.slope != Slope.Cube && cc.slope != Slope.DimBack && cc.slope != Slope.DimBottomBack && cc.slope != Slope.DimBottomBackThick)
                            return null;
                        break;


                    case Slope.BrightDimBottom:
                        if(aa.slope != Slope.BrightDimBottomThick && aa.slope != Slope.Cube && aa.slope != Slope.BrightBottom && aa.slope != Slope.BrightBottomBack && aa.slope != Slope.BrightBottomBackThick)
                            return null;
                        if(bb.slope != Slope.BrightDimBottomThick && bb.slope != Slope.Cube && bb.slope != Slope.DimBottom && bb.slope != Slope.DimBottomBack && bb.slope != Slope.DimBottomBackThick)
                            return null;
                        if(cc.slope != Slope.BrightDimBottomThick && cc.slope != Slope.Cube && cc.slope != Slope.BrightDim && cc.slope != Slope.BrightDimTop && cc.slope != Slope.BrightDimTopThick)
                            return null;
                        break;
                    case Slope.BackBackBottom:
                        if(aa.slope != Slope.BackBackBottomThick && aa.slope != Slope.Cube && aa.slope != Slope.RearDimBottom && aa.slope != Slope.DimBottomBack && aa.slope != Slope.DimBottomBackThick)
                            return null;
                        if(bb.slope != Slope.BackBackBottomThick && bb.slope != Slope.Cube && bb.slope != Slope.RearBrightBottom && bb.slope != Slope.BrightBottomBack && bb.slope != Slope.BrightBottomBackThick)
                            return null;
                        if(cc.slope != Slope.BackBackBottomThick && cc.slope != Slope.Cube && cc.slope != Slope.BackBack && cc.slope != Slope.BackBackTop && cc.slope != Slope.BackBackTopThick)
                            return null;
                        break;
                    case Slope.BrightBottomBack:
                        if(aa.slope != Slope.BrightBottomBackThick && aa.slope != Slope.Cube && aa.slope != Slope.BrightBottom && aa.slope != Slope.BrightDimBottom && aa.slope != Slope.BrightDimBottomThick)
                            return null;
                        if(bb.slope != Slope.BrightBottomBackThick && bb.slope != Slope.Cube && bb.slope != Slope.RearBrightBottom && bb.slope != Slope.BackBackBottom && bb.slope != Slope.BackBackBottomThick)
                            return null;
                        if(cc.slope != Slope.BrightBottomBackThick && cc.slope != Slope.Cube && cc.slope != Slope.BrightBack && cc.slope != Slope.BrightTopBack && cc.slope != Slope.BrightTopBackThick)
                            return null;
                        break;
                    case Slope.DimBottomBack:
                        if(aa.slope != Slope.DimBottomBackThick && aa.slope != Slope.Cube && aa.slope != Slope.RearDimBottom && aa.slope != Slope.BackBackBottom && aa.slope != Slope.BackBackBottomThick)
                            return null;
                        if(bb.slope != Slope.DimBottomBackThick && bb.slope != Slope.Cube && bb.slope != Slope.DimBottom && bb.slope != Slope.BrightDimBottom && bb.slope != Slope.BrightDimBottomThick)
                            return null;
                        if(cc.slope != Slope.DimBottomBackThick && cc.slope != Slope.Cube && cc.slope != Slope.DimBack && cc.slope != Slope.DimTopBack && cc.slope != Slope.DimTopBackThick)
                            return null;
                        break;
                    default: return null;
                }
                o = new FaceVoxel(new MagicaVoxelData(x, y, z, Choose(aa.vox.color, bb.vox.color, cc.vox.color)), targetSlope);
                return o;
            }
            return null;
        }
        public static FaceVoxel FillGapBackup(FaceVoxel[,,] faces, int x, int y, int z, FaceVoxel[] nearby, int a, int b, int c, Slope targetSlope)
        {
            FaceVoxel aa = nearby[a + 3 + 1], bb = nearby[9 + b + 1], cc = nearby[9 + 3 + c], grr = nearby[18 - a + 6 - b + 2 - c], need = nearby[a + b + c], o = faces[x, y, z];
            if(o != null)
                return null;
            
            if((aa != null && bb != null && grr == null))// && (aa.slope != Slope.Cube && bb.slope != Slope.Cube))
            {
                o = new FaceVoxel(new MagicaVoxelData(x, y, z, aa.vox.color), targetSlope);
                return o;
            }
            else if((aa != null && cc != null && grr == null))// && (aa.slope != Slope.Cube && cc.slope != Slope.Cube))
            {
                o = new FaceVoxel(new MagicaVoxelData(x, y, z, cc.vox.color), targetSlope);
                return o;
            }
            else if((bb != null && cc != null && grr == null))// && (bb.slope != Slope.Cube && cc.slope != Slope.Cube))
            {
                o = new FaceVoxel(new MagicaVoxelData(x, y, z, bb.vox.color), targetSlope);
                return o;
            }
            return null;
        }


        public static FaceVoxel[,,] AddBrightTop(FaceVoxel[,,] faces)
        {
            int xSize = faces.GetLength(0), ySize = faces.GetLength(1), zSize = faces.GetLength(2);

            for(int z = zSize - 2; z >= 0; z--)
            {
                for(int x = 1; x < xSize - 2; x++)
                {
                    for(int y = ySize - 2; y > 0; y--)
                    {
                        if(faces[x, y, z + 1] == null &&
                            ((faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Cube) == Slope.Cube && !ImportantVisual(faces[x, y, z]))) &&
                            //                        (faces[x-1, y, z] == null || (faces[x-1, y, z] != null && (faces[x-1, y, z].slope & Slope.Top) == Slope.Top)) &&
                            //                     (faces[x+1, y, z] == null || (faces[x+1, y, z] != null && (faces[x+1, y, z].slope & Slope.Top) == Slope.Top)) &&
                            ((faces[x, y + 1, z + 1] != null && (faces[x, y + 1, z + 1].slope & Slope.Cube) == Slope.Cube && !ImportantVisual(faces[x, y + 1, z + 1])))
                            && ((faces[x - 1, y, z + 1] == null || (faces[x - 1, y, z + 1].slope & Slope.Cube) != Slope.Cube) || (faces[x + 1, y, z + 1] == null || (faces[x + 1, y, z + 1].slope & Slope.Cube) != Slope.Cube))
                            //                   (faces[x - 1, y + 1, z + 1] == null || (faces[x - 1, y + 1, z + 1] != null && (faces[x - 1, y + 1, z + 1].slope & Slope.Top) == Slope.Top)) &&
                            //                 (faces[x + 1, y + 1, z + 1] == null || (faces[x + 1, y + 1, z + 1] != null && (faces[x + 1, y + 1, z + 1].slope & Slope.Top) == Slope.Top))
                            )
                        {
                            faces[x, y, z + 1] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z + 1), color = faces[x, y, z].vox.color }, Slope.BrightTop);
                        }/*
                        else if(faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Top) == Slope.Top && faces[x, y + 1, z + 1] != null && (faces[x, y + 1, z + 1].slope & Slope.Top) == Slope.Top && faces[x, y, z + 1] != null
                            && (((faces[x, y, z + 1].slope & Slope.DimTop) == Slope.DimTop) ||
                                (faces[x, y, z + 1].slope & Slope.BrightDim) == Slope.BrightDim))
                        {
                            faces[x, y, z + 1].slope = Slope.BrightDimTop;
                        }*/
                    }
                }
            }
            return faces;
        }

        public static FaceVoxel[,,] AddDimTop(FaceVoxel[,,] faces)
        {
            int xSize = faces.GetLength(0), ySize = faces.GetLength(1), zSize = faces.GetLength(2);

            for(int z = zSize - 2; z >= 0; z--)
            {
                for(int x = 1; x < xSize; x++)
                {
                    for(int y = 1; y < ySize - 2; y++)
                    {
                        if(faces[x, y, z + 1] == null &&
                            ((faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Cube) == Slope.Cube && !ImportantVisual(faces[x, y, z]))) &&
                            ((faces[x - 1, y, z + 1] != null && (faces[x - 1, y, z + 1].slope & Slope.Cube) == Slope.Cube && !ImportantVisual(faces[x - 1, y, z + 1])))
                            && ((faces[x, y - 1, z + 1] == null || (faces[x, y - 1, z + 1].slope & Slope.Cube) != Slope.Cube) || (faces[x, y + 1, z + 1] == null || (faces[x, y + 1, z + 1].slope & Slope.Cube) != Slope.Cube))
                            )
                        //                          (faces[x, y-1, z] == null || (faces[x, y-1, z] != null && (faces[x, y-1, z].slope & Slope.Top) == Slope.Top)) &&
                        //                        (faces[x, y+1, z] == null || (faces[x, y+1, z] != null && (faces[x, y+1, z].slope & Slope.Top) == Slope.Top)) &&
                        //                        (faces[x-1, y-1, z+1] == null || (faces[x - 1, y-1, z + 1] != null && (faces[x - 1, y-1, z + 1].slope & Slope.Top) == Slope.Top)) &&
                        //                      (faces[x-1, y+1, z+1] == null || (faces[x - 1, y+1, z + 1] != null && (faces[x - 1, y+1, z + 1].slope & Slope.Top) == Slope.Top))

                        {
                            faces[x, y, z + 1] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z + 1), color = faces[x, y, z].vox.color }, Slope.DimTop);
                        }/*
                        else if(faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Top) == Slope.Top && faces[x - 1, y, z + 1] != null && (faces[x - 1, y, z + 1].slope & Slope.Top) == Slope.Top && faces[x, y, z + 1] != null
                             && (((faces[x, y, z + 1].slope & Slope.BrightTop) == Slope.BrightTop) ||
                                 (faces[x, y, z + 1].slope & Slope.BrightDim) == Slope.BrightDim))
                        {
                            faces[x, y, z + 1].slope = Slope.BrightDimTop;
                        }*/
                    }
                }
            }
            return faces;
        }


        public static FaceVoxel[,,] AddBrightDim(FaceVoxel[,,] faces)
        {
            int xSize = faces.GetLength(0), ySize = faces.GetLength(1), zSize = faces.GetLength(2);

            for(int z = zSize - 2; z > 0; z--)
            {
                for(int x = xSize - 2; x >= 0; x--)
                {
                    for(int y = ySize - 2; y >= 0; y--)
                    {
                        if(faces[x + 1, y, z] == null &&
                            ((faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Cube) == Slope.Cube && !ImportantVisual(faces[x, y, z]))) &&
                           //                            (faces[x, y, z-1] == null || (faces[x, y, z-1] != null && (faces[x, y, z-1].slope & Slope.Top) == Slope.Top)) &&
                           //                           (faces[x, y, z+1] == null || (faces[x, y, z+1] != null && (faces[x, y, z+1].slope & Slope.Top) == Slope.Top)) &&
                           ((faces[x + 1, y + 1, z] != null && (faces[x + 1, y + 1, z].slope & Slope.Cube) == Slope.Cube && !ImportantVisual(faces[x + 1, y + 1, z])))
                               //                         (faces[x + 1, y + 1, z - 1] == null || (faces[x + 1, y + 1, z - 1] != null && (faces[x + 1, y + 1, z - 1].slope & Slope.Top) == Slope.Top)) &&
                               //                       (faces[x + 1, y + 1, z + 1] == null || (faces[x + 1, y + 1, z + 1] != null && (faces[x + 1, y + 1, z + 1].slope & Slope.Top) == Slope.Top))
                               && ((faces[x + 1, y, z + 1] == null || (faces[x + 1, y, z + 1].slope & Slope.Cube) != Slope.Cube) || (faces[x + 1, y, z - 1] == null || (faces[x + 1, y, z - 1].slope & Slope.Cube) != Slope.Cube))

     )
                        {
                            faces[x + 1, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)(x + 1), y = (byte)y, z = (byte)(z), color = faces[x, y, z].vox.color }, Slope.BrightDim);
                        }
                        /*
                        else if(faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Top) == Slope.Top && faces[x + 1, y + 1, z] != null && (faces[x + 1, y + 1, z].slope & Slope.Top) == Slope.Top && faces[x + 1, y, z] != null
                             && (((faces[x + 1, y, z].slope & Slope.DimTop) == Slope.DimTop) ||
                                 (faces[x + 1, y, z].slope & Slope.BrightTop) == Slope.BrightTop))
                        {
                            faces[x + 1, y, z].slope = Slope.BrightDimTop;
                        }
                        else if(faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Top) == Slope.Top && faces[x + 1, y + 1, z] != null && (faces[x + 1, y + 1, z].slope & Slope.Top) == Slope.Top && faces[x + 1, y, z] != null
                             && (((faces[x + 1, y, z].slope & Slope.DimBottom) == Slope.DimBottom) ||
                                 (faces[x + 1, y, z].slope & Slope.BrightBottom) == Slope.BrightBottom))
                        {
                            faces[x + 1, y, z].slope = Slope.BrightDimBottom;
                        }*/
                    }
                }
            }
            return faces;
        }


        public static FaceVoxel[,,] AddBrightBottom(FaceVoxel[,,] faces)
        {
            int xSize = faces.GetLength(0), ySize = faces.GetLength(1), zSize = faces.GetLength(2);

            for(int x = 0; x < xSize; x++)
            {
                for(int y = ySize - 2; y >= 0; y--)
                {
                    for(int z = 1; z < zSize; z++)
                    {

                        if(faces[x, y, z - 1] == null &&
                            ((faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Cube) == Slope.Cube)) &&
                            ((faces[x, y + 1, z - 1] != null && (faces[x, y + 1, z - 1].slope & Slope.Cube) == Slope.Cube))
                            )
                        //                            if(faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Top) == Slope.Top  && faces[x, y + 1, z - 1] != null && (faces[x, y + 1, z - 1].slope & Slope.Top) == Slope.Top && faces[x, y, z - 1] == null)
                        {
                            faces[x, y, z - 1] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z - 1), color = faces[x, y, z].vox.color }, Slope.BrightBottom);
                        }
                    }
                }
            }
            return faces;
        }

        public static FaceVoxel[,,] AddDimBottom(FaceVoxel[,,] faces)
        {
            int xSize = faces.GetLength(0), ySize = faces.GetLength(1), zSize = faces.GetLength(2);

            for(int x = 1; x < xSize; x++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int z = 1; z < zSize; z++)
                    {

                        if(faces[x, y, z - 1] == null &&
                            ((faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Cube) == Slope.Cube)) &&
                            ((faces[x - 1, y, z - 1] != null && (faces[x - 1, y, z - 1].slope & Slope.Cube) == Slope.Cube))
                            )
                        //                            if(faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Top) == Slope.Top  && faces[x - 1, y, z - 1] != null && (faces[x - 1, y, z - 1].slope & Slope.Top) == Slope.Top && faces[x, y, z - 1] == null)
                        {

                            faces[x, y, z - 1] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z - 1), color = faces[x, y, z].vox.color }, Slope.DimBottom);
                        }
                    }
                }
            }
            return faces;
        }

        public static FaceVoxel[,,] AddBrightBack(FaceVoxel[,,] faces)
        {
            int xSize = faces.GetLength(0), ySize = faces.GetLength(1), zSize = faces.GetLength(2);

            for(int x = 1; x < xSize; x++)
            {
                for(int y = ySize - 2; y > 0; y--)
                {
                    for(int z = 0; z < zSize; z++)
                    {

                        if(faces[x - 1, y, z] == null &&
                            ((faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Cube) == Slope.Cube)) &&
                            ((faces[x - 1, y + 1, z] != null && (faces[x - 1, y + 1, z].slope & Slope.Cube) == Slope.Cube))
                            )
                        //                            if(faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Top) == Slope.Top  && faces[x - 1, y + 1, z] != null && (faces[x - 1, y + 1, z].slope & Slope.Top) == Slope.Top && faces[x - 1, y, z] == null)
                        {
                            faces[x - 1, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)(x - 1), y = (byte)y, z = (byte)(z), color = faces[x, y, z].vox.color }, Slope.BrightBack);
                        }
                    }
                }
            }
            return faces;
        }
        public static FaceVoxel[,,] AddDimBack(FaceVoxel[,,] faces)
        {
            int xSize = faces.GetLength(0), ySize = faces.GetLength(1), zSize = faces.GetLength(2);

            for(int x = 1; x < xSize; x++)
            {
                for(int y = ySize - 2; y > 0; y--)
                {
                    for(int z = 0; z < zSize; z++)
                    {

                        if(faces[x, y + 1, z] == null &&
                            ((faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Cube) == Slope.Cube)) &&
                            ((faces[x - 1, y + 1, z] != null && (faces[x - 1, y + 1, z].slope & Slope.Cube) == Slope.Cube))
                            )
                        //                            if(faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Top) == Slope.Top  && faces[x - 1, y + 1, z] != null && (faces[x - 1, y + 1, z].slope & Slope.Top) == Slope.Top && faces[x, y + 1, z] == null)
                        {
                            faces[x, y + 1, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)(y + 1), z = (byte)(z), color = faces[x, y, z].vox.color }, Slope.DimBack);
                        }
                    }
                }
            }
            return faces;
        }


        public static FaceVoxel[,,] AddBackBack(FaceVoxel[,,] faces)
        {
            int xSize = faces.GetLength(0), ySize = faces.GetLength(1), zSize = faces.GetLength(2);

            for(int x = xSize - 2; x > 0; x--)
            {
                for(int y = ySize - 2; y > 0; y--)
                {
                    for(int z = 0; z < zSize; z++)
                    {

                        if(faces[x, y + 1, z] == null &&
                            ((faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Cube) == Slope.Cube)) &&
                            ((faces[x + 1, y + 1, z] != null && (faces[x + 1, y + 1, z].slope & Slope.Cube) == Slope.Cube))
                            )
                        //                            if(faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Top) == Slope.Top  && faces[x + 1, y + 1, z] != null && (faces[x + 1, y + 1, z].slope & Slope.Top) == Slope.Top && faces[x, y + 1, z] == null)
                        {
                            faces[x, y + 1, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)(y + 1), z = (byte)(z), color = faces[x, y, z].vox.color }, Slope.BackBack);
                        }
                    }
                }
            }
            return faces;
        }
        public static FaceVoxel[,,] AddCorners(FaceVoxel[,,] faces)
        {
            int xSize = faces.GetLength(0), ySize = faces.GetLength(1), zSize = faces.GetLength(2);

            for(int x = 1; x < xSize - 2; x++)
            {
                for(int y = ySize - 2; y > 0; y--)
                {
                    for(int z = zSize - 2; z > 0; z--)
                    {
                        // && (faces[x, y, z].slope & Slope.BrightDim) == Slope.BrightDim)) &&
                        // && (faces[x, y + 1, z + 1].slope & Slope.DimTop) == Slope.DimTop)) &&
                        // && (faces[x - 1, y, z + 1].slope & Slope.BrightTop) == Slope.BrightTop))
                        if((faces[x, y, z + 1] == null) &&// || (faces[x, y, z + 1].slope & Slope.BrightTop) == Slope.BrightTop || (faces[x, y, z + 1].slope & Slope.DimTop) == Slope.DimTop) &&
                            ((faces[x, y, z] != null && !ImportantVisual(faces[x, y, z]))) &&
                            ((faces[x, y + 1, z + 1] != null && !ImportantVisual(faces[x, y + 1, z + 1]))) &&
                            ((faces[x - 1, y, z + 1] != null && !ImportantVisual(faces[x - 1, y, z + 1]))) &&
                            ((faces[x, y - 1, z + 1] == null)) &&
                            ((faces[x + 1, y, z + 1] == null))
                            )
                        {
                            faces[x, y, z + 1] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)(y), z = (byte)(z + 1), color = faces[x, y, z].vox.color }, Slope.BrightDimTop);
                            /*for(int i = Math.Max(0, x - 1); i <= Math.Min(xSize - 1, x + 1); i++)
                            {
                                for(int j = Math.Max(0, y - 1); j <= Math.Min(ySize - 1, y + 1); j++)
                                {
                                    for(int k = Math.Max(0, z); k <= Math.Min(zSize - 1, z + 2); k++)
                                    {
                                        if(faces[i, j, k] != null && (faces[i,j,k].slope & Slope.Top) == Slope.Top)
                                        {
                                            faces[i, j, k].slope = Slope.BrightDimTop;
                                        }
                                    }
                                }
                            }*/

                        }

                        // && (faces[x + 1, y, z + 1].slope & Slope.BrightTop) == Slope.BrightTop))
                        // && (faces[x, y, z].slope & Slope.BrightBack) == Slope.BrightBack)) &&
                        else if((faces[x, y, z + 1] == null) &&// || (faces[x, y, z + 1].slope & Slope.BrightTop) == Slope.BrightTop) && // || (faces[x, y, z + 1].slope & Slope.BrightBack) == Slope.BrightBack
                            ((faces[x, y, z] != null && !ImportantVisual(faces[x, y, z]))) &&
                            ((faces[x + 1, y, z + 1] != null && !ImportantVisual(faces[x + 1, y, z + 1]))) &&
                            ((faces[x, y + 1, z + 1] != null && !ImportantVisual(faces[x, y + 1, z + 1]))) &&
                            ((faces[x, y - 1, z + 1] == null)) &&
                            ((faces[x - 1, y, z + 1] == null)))
                        {
                            faces[x, y, z + 1] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)(y), z = (byte)(z + 1), color = faces[x, y, z].vox.color }, Slope.BrightTopBack);
                            /*for(int i = Math.Max(0, x - 1); i <= Math.Min(xSize - 1, x + 1); i++)
                            {
                                for(int j = Math.Max(0, y - 1); j <= Math.Min(ySize - 1, y + 1); j++)
                                {
                                    for(int k = Math.Max(0, z); k <= Math.Min(zSize - 1, z + 2); k++)
                                    {
                                        if(faces[i, j, k] != null && (faces[i, j, k].slope & Slope.Top) == Slope.Top)
                                        {
                                            faces[i, j, k].slope = Slope.BrightTopBack;
                                        }
                                    }
                                }
                            }*/
                        }
                        // && (faces[x, y, z].slope & Slope.DimBack) == Slope.DimBack)) &&
                        // && (faces[x, y-1, z + 1].slope & Slope.DimTop) == Slope.DimTop))
                        else if((faces[x, y, z + 1] == null) &&// || (faces[x, y, z + 1].slope & Slope.DimTop) == Slope.DimTop) && //  || (faces[x, y, z + 1].slope & Slope.DimBack) == Slope.DimBack
                            ((faces[x, y, z] != null && !ImportantVisual(faces[x, y, z]))) &&
                            ((faces[x, y - 1, z + 1] != null && !ImportantVisual(faces[x, y - 1, z + 1]))) &&
                            ((faces[x - 1, y, z + 1] != null && !ImportantVisual(faces[x - 1, y, z + 1]))) &&
                            ((faces[x + 1, y, z + 1] == null)) &&
                            ((faces[x, y + 1, z + 1] == null)))
                        {
                            faces[x, y, z + 1] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)(y), z = (byte)(z + 1), color = faces[x, y, z].vox.color }, Slope.DimTopBack);
                            /*for(int i = Math.Max(0, x - 1); i <= Math.Min(xSize - 1, x + 1); i++)
                            {
                                for(int j = Math.Max(0, y - 1); j <= Math.Min(ySize - 1, y + 1); j++)
                                {
                                    for(int k = Math.Max(0, z); k <= Math.Min(zSize - 1, z + 2); k++)
                                    {
                                        if(faces[i, j, k] != null && (faces[i, j, k].slope & Slope.Top) == Slope.Top)
                                        {
                                            faces[i, j, k].slope = Slope.DimTopBack;
                                        }
                                    }
                                }
                            }
                            */
                        }
                    }
                }
            }

            for(int x = 1; x < xSize - 2; x++)
            {
                for(int y = ySize - 2; y > 0; y--)
                {
                    for(int z = 1; z < zSize - 2; z++)
                    {
                        if(faces[x, y, z] == null &&
                            ((faces[x, y, z + 1] != null && !ImportantVisual(faces[x, y, z]))) &&
                            ((faces[x, y + 1, z] != null && !ImportantVisual(faces[x, y + 1, z - 1]))) &&
                            ((faces[x - 1, y, z] != null && !ImportantVisual(faces[x - 1, y, z - 1]))) &&
                            ((faces[x, y - 1, z] == null)) &&
                            ((faces[x + 1, y, z] == null))
                            )
                        {
                            faces[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)(y), z = (byte)(z), color = faces[x, y, z].vox.color }, Slope.BrightDimBottom);
                            faces[x, y + 1, z].slope = Slope.BrightDimBottom;
                            faces[x - 1, y, z].slope = Slope.BrightDimBottom;
                        }
                    }
                }
            }
            return faces;
        }
        public static FaceModifier[] Modifiers = new FaceModifier[] { AddBrightTop, AddDimTop, AddBrightDim, AddBrightBack, AddDimBack, AddBackBack,
            AddBrightBottom, AddDimBottom, AddCorners
        };

        public static bool ImportantVisual(FaceVoxel fv)
        {
            if(fv == null)
                return false;
            return TransformLogic.ColorValue(fv.vox.color) > 16;
        }

        public static bool ImportantVisual(byte color)
        {
            if(color == 0)
                return false;
            return TransformLogic.ColorValue(color) > 16;
        }
        public static bool ImportantVisualOrPlaceholder(byte color)
        {
            if(color == 0)
                return false;
            return TransformLogic.ColorValue(color) > 16 || (257 - color) % 4 != 0;
        }

        public static bool ImportantNeighborhood(FaceVoxel[,,] faces, int x, int y, int z)
        {
            if(faces == null)
                return false;
            FaceVoxel a = (x == faces.GetLength(0) - 1) ? null : faces[x + 1, y, z], b = (x == 0) ? null : faces[x - 1, y, z],
                c = (y == faces.GetLength(1) - 1) ? null : faces[x, y + 1, z], d = (y == 0) ? null : faces[x, y - 1, z],
                e = (z == faces.GetLength(2) - 1) ? null : faces[x, y, z + 1], f = (z == 0) ? null : faces[x, y, z - 1];
            byte ac = 255, bc = 255, cc = 255, dc = 255, ec = 255, fc = 255;
            int tgt = 512;
            if(a != null) { ac = a.vox.color; if(tgt == 512) tgt = ac; else if(tgt != ac) tgt = 1024; }
            if(b != null) { bc = b.vox.color; if(tgt == 512) tgt = bc; else if(tgt != bc) tgt = 1024; }
            if(c != null) { cc = c.vox.color; if(tgt == 512) tgt = cc; else if(tgt != cc) tgt = 1024; }
            if(d != null) { dc = d.vox.color; if(tgt == 512) tgt = dc; else if(tgt != dc) tgt = 1024; }
            if(e != null) { ec = e.vox.color; if(tgt == 512) tgt = ec; else if(tgt != ec) tgt = 1024; }
            if(f != null) { fc = f.vox.color; if(tgt == 512) tgt = fc; else if(tgt != fc) tgt = 1024; }

            if((ac & bc & cc & dc & ec & fc) == tgt && (ac | bc | cc | dc | ec | fc) == tgt)
            {
                //if(tgt == (253 - 9 * 4) && faces[x,y,z] == null)
                //    Console.WriteLine("Homogeneous area: " + ((253 - tgt) / 4));
                return false;
            }
            return (ImportantVisual(a) || ImportantVisual(b) ||
                    ImportantVisual(c) || ImportantVisual(d) ||
                    ImportantVisual(e) || ImportantVisual(f));
        }
        public static bool ImportantNeighborhood(byte[,,] faces, int x, int y, int z)
        {
            if(faces == null)
                return false;
            byte a = (x == faces.GetLength(0) - 1) ? (byte)0 : faces[x + 1, y, z], b = (x == 0) ? (byte)0 : faces[x - 1, y, z],
                c = (y == faces.GetLength(1) - 1) ? (byte)0 : faces[x, y + 1, z], d = (y == 0) ? (byte)0 : faces[x, y - 1, z],
                e = (z == faces.GetLength(2) - 1) ? (byte)0 : faces[x, y, z + 1], f = (z == 0) ? (byte)0 : faces[x, y, z - 1];
            byte ac = 255, bc = 255, cc = 255, dc = 255, ec = 255, fc = 255;
            int tgt = 512;
            if(a != 0) { ac = a; if(tgt == 512) tgt = ac; else if(tgt != ac) tgt = 1024; }
            if(b != 0) { bc = b; if(tgt == 512) tgt = bc; else if(tgt != bc) tgt = 1024; }
            if(c != 0) { cc = c; if(tgt == 512) tgt = cc; else if(tgt != cc) tgt = 1024; }
            if(d != 0) { dc = d; if(tgt == 512) tgt = dc; else if(tgt != dc) tgt = 1024; }
            if(e != 0) { ec = e; if(tgt == 512) tgt = ec; else if(tgt != ec) tgt = 1024; }
            if(f != 0) { fc = f; if(tgt == 512) tgt = fc; else if(tgt != fc) tgt = 1024; }

            if((ac & bc & cc & dc & ec & fc) == tgt && (ac | bc | cc | dc | ec | fc) == tgt)
            {
                //if(tgt == (253 - 9 * 4) && faces[x,y,z] == null)
                //    Console.WriteLine("Homogeneous area: " + ((253 - tgt) / 4));
                return false;
            }
            return (ImportantVisual(a) || ImportantVisual(b) ||
                    ImportantVisual(c) || ImportantVisual(d) ||
                    ImportantVisual(e) || ImportantVisual(f));
        }

        public static FaceVoxel[][,,] FieryExplosionLargeW(FaceVoxel[,,] faces, bool blowback, bool shadowless)
        {
            List<FaceVoxel>[] voxelFrames = new List<FaceVoxel>[13];
            voxelFrames[0] = FaceArrayToList(faces);

            for(int i = 0; i < voxelFrames[0].Count; i++)
            {
                voxelFrames[0][i].vox.x += 40;
                voxelFrames[0][i].vox.y += 40;
            }
            for(int f = 1; f < 5; f++)
            {
                List<FaceVoxel> altered = new List<FaceVoxel>(voxelFrames[f - 1].Count);
                FaceVoxel[] vls = new FaceVoxel[voxelFrames[f - 1].Count], working = new FaceVoxel[voxelFrames[f - 1].Count]; //.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)
                voxelFrames[f - 1].CopyTo(vls, 0);
                voxelFrames[f - 1].CopyTo(working, 0);

                int[] minX = new int[40 * 2];
                int[] maxX = new int[40 * 2];
                float[] midX = new float[40 * 2];
                for(int level = 0; level < 40 * 2; level++)
                {
                    minX[level] = vls.Min(v => v.vox.x * ((v.vox.z != level || v.vox.color == 253 - 25 * 4 ||
                        v.vox.color == 253 - 17 * 4 || v.vox.color == 253 - 19 * 4 || v.vox.color == 253 - 18 * 4 || v.vox.color <= 257 - VoxelLogic.wcolorcount * 4) ? 2000 : 1));
                    maxX[level] = vls.Max(v => v.vox.x * ((v.vox.z != level || v.vox.color == 253 - 25 * 4 ||
                        v.vox.color == 253 - 17 * 4 || v.vox.color == 253 - 19 * 4 || v.vox.color == 253 - 18 * 4 || v.vox.color <= 257 - VoxelLogic.wcolorcount * 4) ? 0 : 1));
                    midX[level] = (maxX[level] + minX[level]) / 2F;
                }

                int[] minY = new int[40 * 2];
                int[] maxY = new int[40 * 2];
                float[] midY = new float[40 * 2];
                for(int level = 0; level < 40 * 2; level++)
                {
                    minY[level] = vls.Min(v => v.vox.y * ((v.vox.z != level || v.vox.color == 253 - 25 * 4 ||
                        v.vox.color == 253 - 17 * 4 || v.vox.color == 253 - 19 * 4 || v.vox.color == 253 - 18 * 4 || v.vox.color <= 257 - VoxelLogic.wcolorcount * 4) ? 2000 : 1));
                    maxY[level] = vls.Max(v => v.vox.y * ((v.vox.z != level || v.vox.color == 253 - 25 * 4 ||
                        v.vox.color == 253 - 17 * 4 || v.vox.color == 253 - 19 * 4 || v.vox.color == 253 - 18 * 4 || v.vox.color <= 257 - VoxelLogic.wcolorcount * 4) ? 0 : 1));
                    midY[level] = (maxY[level] + minY[level]) / 2F;
                }

                int minZ = vls.Min(v => v.vox.z * ((v.vox.color == 253 - 25 * 4 ||
                        v.vox.color == 253 - 17 * 4 || v.vox.color == 253 - 19 * 4 || v.vox.color == 253 - 18 * 4 || v.vox.color <= 257 - VoxelLogic.wcolorcount * 4) ? 2000 : 1));
                int maxZ = vls.Max(v => v.vox.z * ((v.vox.color == 253 - 25 * 4 ||
                        v.vox.color == 253 - 17 * 4 || v.vox.color == 253 - 19 * 4 || v.vox.color == 253 - 18 * 4 || v.vox.color <= 257 - VoxelLogic.wcolorcount * 4) ? 0 : 1));
                float midZ = (maxZ + minZ) / 2F;

                int iter = 0;
                foreach(FaceVoxel v in vls)
                {
                    MagicaVoxelData mvd = new MagicaVoxelData();
                    int c = ((255 - v.vox.color) % 4 == 0) ? (255 - v.vox.color) / 4 + VoxelLogic.wcolorcount : (253 - v.vox.color) / 4;
                    if(c == 8 || c == 9) //flesh
                        mvd.color = (byte)((TallFaces.r.Next(1 + f) == 0) ? 253 - 34 * 4 : (TallFaces.r.Next(8) == 0) ? 253 - 19 * 4 : v.vox.color); //random transform to guts
                    else if(c == 34) //guts
                        mvd.color = (byte)((TallFaces.r.Next(18) == 0) ? 253 - 19 * 4 : v.vox.color); //random transform to orange fire
                    else if(c == VoxelLogic.wcolorcount - 1) //clear
                        mvd.color = VoxelLogic.clear; //clear stays clear
                    else if(c == 16)
                        mvd.color = VoxelLogic.clear; //clear inner shadow
                    else if(c == 25)
                        mvd.color = 253 - 25 * 4; //shadow stays shadow
                    else if(c == 27)
                        mvd.color = 253 - 27 * 4; //water stays water
                    else if(c >= VoxelLogic.wcolorcount && c < VoxelLogic.wcolorcount + 5)
                        mvd.color = v.vox.color; // falling water stays falling water
                    else if(c == 40)
                        mvd.color = 253 - 20 * 4; //flickering sparks become normal sparks
                    else if(c >= 21 && c <= 24) //lights
                        mvd.color = 253 - 35 * 4; //glass color for broken lights
                    else if(c == 35) //windows
                        mvd.color = (byte)((TallFaces.r.Next(3) == 0) ? VoxelLogic.clear : v.vox.color); //random transform to clear
                    else if(c == 36) //rotor contrast
                        mvd.color = 253 - 0 * 4; //"foot contrast" or "raw metal contrast" color for broken rotors contrast
                    else if(c == 37) //rotor
                        mvd.color = 253 - 1 * 4; //"foot" or "raw metal" color for broken rotors
                    else if(c == 38 || c == 39)
                        mvd.color = VoxelLogic.clear; //clear non-active rotors
                    else if(c == 19) //orange fire
                        mvd.color = (byte)((TallFaces.r.Next(3) <= 1) ? 253 - 18 * 4 : ((TallFaces.r.Next(5) == 0) ? 253 - 17 * 4 : v.vox.color)); //random transform to yellow fire or smoke
                    else if(c == 18) //yellow fire
                        mvd.color = (byte)((TallFaces.r.Next(4) <= 1) ? 253 - 19 * 4 : ((TallFaces.r.Next(4) == 0) ? 253 - 20 * 4 : v.vox.color)); //random transform to orange fire or sparks
                    else if(c == 20) //sparks
                        mvd.color = (byte)((TallFaces.r.Next(4) == 0) ? VoxelLogic.clear : v.vox.color); //random transform to clear
                    else if(c == 17)
                        mvd.color = v.vox.color; // smoke stays smoke
                    else
                        mvd.color = (byte)((TallFaces.r.Next(9 - f) == 0) ? 253 - ((TallFaces.r.Next(4) == 0) ? 18 * 4 : 19 * 4) : v.vox.color); //random transform to orange or yellow fire
                    float xMove = 0, yMove = 0, zMove = 0;

                    if(mvd.color == 253 - 19 * 4 || mvd.color == 253 - 18 * 4 || mvd.color == 253 - 17 * 4)
                    {
                        zMove = (f - 1) * 1.8f;
                        xMove = (float)(TallFaces.r.NextDouble() * 2.0 - 1.0);
                        yMove = (float)(TallFaces.r.NextDouble() * 2.0 - 1.0);
                    }
                    else
                    {
                        if(v.vox.x > midX[v.vox.z])
                            xMove = ((0 - TallFaces.r.Next(3) - ((blowback) ? 9 : 0) + (v.vox.x - midX[v.vox.z])) * 25F / f * ((v.vox.z - minZ + 1) / (maxZ - minZ + 1F)));
                        else if(v.vox.x < midX[v.vox.z])
                            xMove = ((0 + TallFaces.r.Next(3) - ((blowback) ? 8 : 0) - midX[v.vox.z] + v.vox.x) * 25F / f * ((v.vox.z - minZ + 1) / (maxZ - minZ + 1F)));// / 300F) * (v.vox.z + 5); //5 -
                        if(v.vox.y > midY[v.vox.z])
                            yMove = ((0 - TallFaces.r.Next(3) + (v.vox.y - midY[v.vox.z])) * 25F / f * ((v.vox.z - minZ + 1) / (maxZ - minZ + 1F))); //maxX[v.vox.z] - minX[v.vox.z]
                        else if(v.vox.y < midY[v.vox.z])
                            yMove = ((0 + TallFaces.r.Next(3) - midY[v.vox.z] + v.vox.y) * 25F / f * ((v.vox.z - minZ + 1) / (maxZ - minZ + 1F)));
                        if(mvd.color == 253 - 20 * 4)
                        {
                            zMove = 0.1f;
                            xMove *= 2;
                            yMove *= 2;
                        }
                        else if(minZ > 0)
                            zMove = ((v.vox.z) * (1 - f) / 6F);
                        else
                            zMove = (v.vox.z / ((maxZ + 1 + midZ) * (0.15F))) * (4 - f) * 1.1F;
                    }

                    if(xMove > 20) xMove = 20;
                    if(xMove < -20) xMove = -20;
                    if(yMove > 20) yMove = 20;
                    if(yMove < -20) yMove = -20;
                    //float magnitude = Math.Abs(xMove) + Math.Abs(yMove);
                    float magnitude = (float)Math.Sqrt(xMove * xMove + yMove * yMove);
                    if(xMove > 0)
                    {
                        float nv = v.vox.x + (float)TallFaces.r.NextDouble() * ((xMove / magnitude) * f / 0.3F) + (float)(TallFaces.r.NextDouble() * 6.0 - 3.0); //(Math.Sqrt(Math.Abs(Math.Abs((xMove * f / 3)) - Math.Abs((yMove * f / 4)))) + (float)(TallFaces.r.NextDouble() * 2.0 - 1.0));
                        if(nv < 0)
                        {
                            nv = 0;
                            mvd.color = VoxelLogic.clear;
                        }
                        if(nv > 119)
                        {
                            nv = 119;
                            mvd.color = VoxelLogic.clear;
                        }
                        mvd.x = (byte)(Math.Floor(nv));
                    }
                    else if(xMove < 0)
                    {
                        float nv = v.vox.x - (float)TallFaces.r.NextDouble() * ((xMove / magnitude) * f / -0.3F) + (float)(TallFaces.r.NextDouble() * 6.0 - 3.0); //(float)(Math.Sqrt(Math.Abs(Math.Abs((xMove * f / 3)) - Math.Abs((yMove * f / 4)))) + (float)(TallFaces.r.NextDouble() * 2.0 - 1.0));
                        if(nv < 0)
                        {
                            nv = 0;
                            mvd.color = VoxelLogic.clear;
                        }
                        if(nv > 119)
                        {
                            nv = 119;
                            mvd.color = VoxelLogic.clear;
                        }
                        mvd.x = (byte)(Math.Floor(nv));
                    }
                    else
                    {
                        if(v.vox.x < 0)
                        {
                            mvd.x = 0;
                            mvd.color = VoxelLogic.clear;
                        }
                        if(v.vox.x > 119)
                        {
                            mvd.x = 119;
                            mvd.color = VoxelLogic.clear;
                        }
                        else mvd.x = v.vox.x;
                    }
                    if(yMove > 0)
                    {
                        float nv = v.vox.y + (float)TallFaces.r.NextDouble() * ((yMove / magnitude) * f / 0.3F) + (float)(TallFaces.r.NextDouble() * 6.0 - 3.0); //(float)(Math.Sqrt(Math.Abs(Math.Abs((yMove * f / 3)) - Math.Abs((xMove * f / 4)))) + (float)(TallFaces.r.NextDouble() * 2.0 - 1.0));
                        if(nv < 0)
                        {
                            nv = 0;
                            mvd.color = VoxelLogic.clear;
                        }
                        if(nv > 119)
                        {
                            nv = 119;
                            mvd.color = VoxelLogic.clear;
                        }
                        mvd.y = (byte)(Math.Floor(nv));
                    }
                    else if(yMove < 0)
                    {
                        float nv = v.vox.y - (float)TallFaces.r.NextDouble() * ((yMove / magnitude) * f / -0.3F) + (float)(TallFaces.r.NextDouble() * 6.0 - 3.0); //(float)(Math.Sqrt(Math.Abs(Math.Abs((yMove * f / 3)) - Math.Abs((xMove * f / 4)))) + (float)(TallFaces.r.NextDouble() * 2.0 - 1.0));
                        if(nv < 0)
                        {
                            nv = 0;
                            mvd.color = VoxelLogic.clear;
                        }
                        if(nv > 119)
                        {
                            nv = 119;
                            mvd.color = VoxelLogic.clear;
                        }
                        mvd.y = (byte)(Math.Floor(nv));
                    }
                    else
                    {
                        if(v.vox.y < 0)
                        {
                            mvd.y = 0;
                            mvd.color = VoxelLogic.clear;
                        }
                        if(v.vox.y > 119)
                        {
                            mvd.y = 119;
                            mvd.color = VoxelLogic.clear;
                        }
                        else mvd.y = v.vox.y;
                    }

                    if(zMove != 0)
                    {
                        float nv = (v.vox.z + (zMove * 1.5f / f));

                        if(nv <= 0 && f < 8) nv = TallFaces.r.Next(2); //bounce
                        else if(nv < 0) nv = 0;

                        if(nv > 79) nv = 79;
                        mvd.z = (byte)Math.Round(nv);
                    }
                    else
                    {
                        mvd.z = v.vox.z;
                    }
                    working[iter] = new FaceVoxel(mvd, randomSlope());
                    iter++;
                }
                voxelFrames[f] = working.ToList();
            }
            for(int f = 5; f < 13; f++)
            {

                List<FaceVoxel> altered = new List<FaceVoxel>(voxelFrames[f - 1].Count);
                FaceVoxel[] vls = new FaceVoxel[voxelFrames[f - 1].Count], working = new FaceVoxel[voxelFrames[f - 1].Count]; //.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)
                voxelFrames[f - 1].CopyTo(vls, 0);
                voxelFrames[f - 1].CopyTo(working, 0);


                int[] minX = new int[40 * 2];
                int[] maxX = new int[40 * 2];
                float[] midX = new float[40 * 2];
                for(int level = 0; level < 40 * 2; level++)
                {
                    minX[level] = vls.Min(v => v.vox.x * ((v.vox.z != level || v.vox.color == 253 - 25 * 4 ||
                        v.vox.color == 253 - 17 * 4 || v.vox.color == 253 - 19 * 4 || v.vox.color == 253 - 18 * 4 || v.vox.color <= 257 - VoxelLogic.wcolorcount * 4) ? 2000 : 1));
                    maxX[level] = vls.Max(v => v.vox.x * ((v.vox.z != level || v.vox.color == 253 - 25 * 4 ||
                        v.vox.color == 253 - 17 * 4 || v.vox.color == 253 - 19 * 4 || v.vox.color == 253 - 18 * 4 || v.vox.color <= 257 - VoxelLogic.wcolorcount * 4) ? 0 : 1));
                    midX[level] = (maxX[level] + minX[level]) / 2F;
                }

                int[] minY = new int[40 * 2];
                int[] maxY = new int[40 * 2];
                float[] midY = new float[40 * 2];
                for(int level = 0; level < 40 * 2; level++)
                {
                    minY[level] = vls.Min(v => v.vox.y * ((v.vox.z != level || v.vox.color == 253 - 25 * 4 ||
                        v.vox.color == 253 - 17 * 4 || v.vox.color == 253 - 19 * 4 || v.vox.color == 253 - 18 * 4 || v.vox.color <= 257 - VoxelLogic.wcolorcount * 4) ? 2000 : 1));
                    maxY[level] = vls.Max(v => v.vox.y * ((v.vox.z != level || v.vox.color == 253 - 25 * 4 ||
                        v.vox.color == 253 - 17 * 4 || v.vox.color == 253 - 19 * 4 || v.vox.color == 253 - 18 * 4 || v.vox.color <= 257 - VoxelLogic.wcolorcount * 4) ? 0 : 1));
                    midY[level] = (maxY[level] + minY[level]) / 2F;
                }

                int minZ = vls.Min(v => v.vox.z * ((v.vox.color == 253 - 25 * 4 ||
                        v.vox.color == 253 - 17 * 4 || v.vox.color == 253 - 19 * 4 || v.vox.color == 253 - 18 * 4 || v.vox.color <= 257 - VoxelLogic.wcolorcount * 4) ? 2000 : 1));
                int maxZ = vls.Max(v => v.vox.z * ((v.vox.color == 253 - 25 * 4 ||
                        v.vox.color == 253 - 17 * 4 || v.vox.color == 253 - 19 * 4 || v.vox.color == 253 - 18 * 4 || v.vox.color <= 257 - VoxelLogic.wcolorcount * 4) ? 0 : 1));
                float midZ = (maxZ + minZ) / 2F;

                int iter = 0;
                foreach(FaceVoxel v in vls)
                {
                    MagicaVoxelData mvd = new MagicaVoxelData();
                    int c = ((255 - v.vox.color) % 4 == 0) ? (255 - v.vox.color) / 4 + VoxelLogic.wcolorcount : (253 - v.vox.color) / 4;
                    if(c == 8 || c == 9) //flesh
                        mvd.color = (byte)((TallFaces.r.Next(f) == 0) ? 253 - 34 * 4 : (TallFaces.r.Next(6) == 0 && f < 10) ? 253 - 19 * 4 : v.vox.color); //random transform to guts
                    else if(c == 34) //guts
                        mvd.color = (byte)((TallFaces.r.Next(20) == 0 && f < 10) ? 253 - 19 * 4 : v.vox.color); //random transform to orange fire
                    else if(c == VoxelLogic.wcolorcount - 1) //VoxelLogic.clear and markers
                        mvd.color = (byte)VoxelLogic.clear; //VoxelLogic.clear stays VoxelLogic.clear
                    else if(c == 16)
                        mvd.color = VoxelLogic.clear; //VoxelLogic.clear inner shadow
                    else if(c == 25)
                        mvd.color = 253 - 25 * 4; //shadow stays shadow
                    else if(c == 27)
                        mvd.color = 253 - 27 * 4; //water stays water
                    else if(c >= VoxelLogic.wcolorcount && c < VoxelLogic.wcolorcount + 5)
                        mvd.color = (byte)(255 - (c - VoxelLogic.wcolorcount) * 4); // falling water stays falling water
                    else if(c == 40)
                        mvd.color = 253 - 20 * 4; //flickering sparks become normal sparks
                    else if(c >= 21 && c <= 24) //lights
                        mvd.color = 253 - 35 * 4; //glass color for broken lights
                    else if(c == 35) //windows
                        mvd.color = (byte)((TallFaces.r.Next(3) == 0) ? VoxelLogic.clear : v.vox.color); //random transform to VoxelLogic.clear
                    else if(c == 36) //rotor contrast
                        mvd.color = 253 - 0 * 4; //"foot contrast" color for broken rotors contrast
                    else if(c == 37) //rotor
                        mvd.color = 253 - 1 * 4; //"foot" color for broken rotors
                    else if(c == 38 || c == 39)
                        mvd.color = VoxelLogic.clear; //VoxelLogic.clear non-active rotors
                    else if(c == 19) //orange fire
                        mvd.color = (byte)((TallFaces.r.Next(9) + 2 <= f) ? 253 - 17 * 4 : ((TallFaces.r.Next(3) <= 1) ? 253 - 18 * 4 : ((TallFaces.r.Next(3) == 0) ? 253 - 17 * 4 : v.vox.color))); //random transform to yellow fire or smoke
                    else if(c == 18) //yellow fire
                        mvd.color = (byte)((TallFaces.r.Next(9) + 1 <= f) ? 253 - 17 * 4 : ((TallFaces.r.Next(3) <= 1) ? 253 - 19 * 4 : ((TallFaces.r.Next(4) == 0) ? 253 - 17 * 4 : ((TallFaces.r.Next(4) == 0) ? 253 - 20 * 4 : v.vox.color)))); //random transform to orange fire, smoke, or sparks
                    else if(c == 20) //sparks
                        mvd.color = (byte)((TallFaces.r.Next(4) > 0 && TallFaces.r.Next(12) > f) ? v.vox.color : VoxelLogic.clear); //random transform to VoxelLogic.clear
                    else if(c == 17) //smoke
                        mvd.color = (byte)((TallFaces.r.Next(10) + 3 <= f) ? VoxelLogic.clear : 253 - 17 * 4); //random transform to VoxelLogic.clear
                    else
                        mvd.color = (byte)((TallFaces.r.Next(f * 4) <= 6) ? 253 - ((TallFaces.r.Next(4) == 0) ? 18 * 4 : 19 * 4) : v.vox.color); //random transform to orange or yellow fire

                    float xMove = 0, yMove = 0, zMove = 0;
                    if(mvd.color == 253 - 19 * 4 || mvd.color == 253 - 18 * 4 || mvd.color == 253 - 17 * 4)
                    {
                        zMove = f * 0.5f;
                        xMove = (float)(TallFaces.r.NextDouble() * 2.0 - 1.0);
                        yMove = (float)(TallFaces.r.NextDouble() * 2.0 - 1.0);
                    }
                    else
                    {
                        /*
                         if (v.x > midX[v.vox.z])
                            xMove = ((0 - TallFaces.r.Next(3) - ((blowback) ? 9 : 0) + (v.x - midX[v.vox.z])) * 2F * ((v.vox.z - minZ + 1) / (maxZ - minZ + 1F)));
                        //for lower values: distance from current voxel x to center + (between 2 to 0), times variable based on height
                        else if (v.x < midX[v.vox.z])
                            xMove = ((0 + TallFaces.r.Next(3) - ((blowback) ? 8 : 0) - midX[v.vox.z] + v.x) * 2F * ((v.vox.z - minZ + 1) / (maxZ - minZ + 1F)));// / 300F) * (v.vox.z + 5); //5 -
                        if (v.y > midY[v.vox.z])
                            yMove = ((0 - TallFaces.r.Next(3) + (v.y - midY[v.vox.z])) * 2F * ((v.vox.z - minZ + 3) / (maxZ - minZ + 1F))); //maxX[v.vox.z] - minX[v.vox.z]
                        else if (v.y < midY[v.vox.z])
                            yMove = ((0 + TallFaces.r.Next(3) - midY[v.vox.z] + v.y) * 2F * ((v.vox.z - minZ + 3) / (maxZ - minZ + 1F)));
                         */


                        if(v.vox.x > midX[v.vox.z])
                            xMove = ((0 - TallFaces.r.Next(3) - ((blowback) ? 7 : 0) + (v.vox.x - midX[v.vox.z])) / (f + 8) * 25F * ((v.vox.z - minZ + 1) / (maxZ - minZ + 1F)));
                        else if(v.vox.x < midX[v.vox.z])
                            xMove = ((0 + TallFaces.r.Next(3) - ((blowback) ? 6 : 0) - midX[v.vox.z] + v.vox.x) / (f + 8) * 25F * ((v.vox.z - minZ + 1) / (maxZ - minZ + 1F)));
                        if(v.vox.y > midY[v.vox.z])
                            yMove = ((0 - TallFaces.r.Next(3) + (v.vox.y - midY[v.vox.z])) / (f + 8) * 25F * ((v.vox.z - minZ + 1) / (maxZ - minZ + 1F)));
                        else if(v.vox.y < midY[v.vox.z])
                            yMove = ((0 + TallFaces.r.Next(3) - midY[v.vox.z] + v.vox.y) / (f + 8) * 25F * ((v.vox.z - minZ + 1) / (maxZ - minZ + 1F)));

                        if(mvd.color == 253 - 20 * 4)
                        {
                            zMove = 0.1f;
                            xMove *= 2;
                            yMove *= 2;
                        }
                        else if(f < 6 && minZ == 0)
                            zMove = (v.vox.z / ((maxZ + 1) * (0.2F))) * (5 - f) * 0.6F;
                        else
                            zMove = (1 - f * 1.85F);
                    }
                    if(v.vox.z <= 1 && f >= 10)
                    {
                        xMove = 0;
                        yMove = 0;
                    }
                    if(xMove > 20) xMove = 20;
                    if(xMove < -20) xMove = -20;
                    if(yMove > 20) yMove = 20;
                    if(yMove < -20) yMove = -20;
                    // float magnitude = Math.Abs(xMove) + Math.Abs(yMove);
                    float magnitude = (float)Math.Sqrt(xMove * xMove + yMove * yMove);

                    if(xMove > 0)
                    {
                        float nv = v.vox.x + (float)TallFaces.r.NextDouble() * ((xMove / magnitude) * 35F / f) + (float)(TallFaces.r.NextDouble() * 8.0 - 4.0);
                        //                        float nv = (float)(v.vox.x + Math.Sqrt(Math.Abs(Math.Abs(xMove / (0.07f * (f + 5))) - Math.Abs((yMove / (0.09f * (f + 5)))))) + (float)(TallFaces.r.NextDouble() * 2.0 - 1.0));
                        if(nv < 0)
                        {
                            nv = 0;
                            mvd.color = VoxelLogic.clear;
                        }
                        if(nv > 119)
                        {
                            nv = 119;
                            mvd.color = VoxelLogic.clear;
                        }
                        mvd.x = (byte)(Math.Floor(nv));
                    }
                    else if(xMove < 0)
                    {
                        float nv = v.vox.x - (float)TallFaces.r.NextDouble() * ((xMove / magnitude) * -35F / f) + (float)(TallFaces.r.NextDouble() * 8.0 - 4.0);

                        //float nv = (float)(v.vox.x - Math.Sqrt(Math.Abs(Math.Abs(xMove / (0.07f * (f + 5))) - Math.Abs((yMove / (0.09f * (f + 5)))))) + (float)(TallFaces.r.NextDouble() * 2.0 - 1.0));
                        if(nv < 0)
                        {
                            nv = 0;
                            mvd.color = VoxelLogic.clear;
                        }
                        if(nv > 119)
                        {
                            nv = 119;
                            mvd.color = VoxelLogic.clear;
                        }
                        mvd.x = (byte)(Math.Floor(nv));
                    }
                    else
                    {
                        if(v.vox.x < 0)
                        {
                            mvd.x = 0;
                            mvd.color = VoxelLogic.clear;
                        }
                        if(v.vox.x > 119)
                        {
                            mvd.x = 119;
                            mvd.color = VoxelLogic.clear;
                        }
                        else mvd.x = v.vox.x;
                    }
                    if(yMove > 0)
                    {
                        float nv = v.vox.y + (float)TallFaces.r.NextDouble() * ((yMove / magnitude) * 35F / f) + (float)(TallFaces.r.NextDouble() * 8.0 - 4.0);
                        //float nv = (float)(v.y + Math.Sqrt(Math.Abs(Math.Abs(yMove / (0.07f * (f + 5))) - Math.Abs((xMove / (0.09f * (f + 5)))))) + (float)(TallFaces.r.NextDouble() * 2.0 - 1.0));
                        if(nv < 0)
                        {
                            nv = 0;
                            mvd.color = VoxelLogic.clear;
                        }
                        if(nv > 119)
                        {
                            nv = 119;
                            mvd.color = VoxelLogic.clear;
                        }
                        mvd.y = (byte)(Math.Floor(nv));
                    }
                    else if(yMove < 0)
                    {
                        float nv = v.vox.y - (float)TallFaces.r.NextDouble() * ((yMove / magnitude) * -35F / f) + (float)(TallFaces.r.NextDouble() * 8.0 - 4.0);
                        //(float)(v.y - Math.Sqrt(Math.Abs(Math.Abs(yMove / (0.07f * (f + 5))) - Math.Abs((xMove / (0.09f * (f + 5)))))) + (float)(TallFaces.r.NextDouble() * 2.0 - 1.0));
                        if(nv < 0)
                        {
                            nv = 0;
                            mvd.color = VoxelLogic.clear;
                        }
                        if(nv > 119)
                        {
                            nv = 119;
                            mvd.color = VoxelLogic.clear;
                        }
                        mvd.y = (byte)(Math.Ceiling(nv));
                    }
                    else
                    {
                        if(v.vox.y < 0)
                        {
                            mvd.y = 0;
                            mvd.color = VoxelLogic.clear;
                        }
                        if(v.vox.y > 119)
                        {
                            mvd.y = 119;
                            mvd.color = VoxelLogic.clear;
                        }
                        else mvd.y = v.vox.y;
                    }

                    if(zMove != 0)
                    {
                        float nv = (v.vox.z + (zMove * 1.3f));

                        if(nv <= 0 && f < 8) nv = TallFaces.r.Next(2); //bounce
                        else if(nv < 0) nv = 0;

                        if(nv > 79)
                        {
                            nv = 79;
                            mvd.color = VoxelLogic.clear;
                        }
                        mvd.z = (byte)Math.Round(nv);
                    }
                    else
                    {
                        mvd.z = v.vox.z;
                    }
                    working[iter] = new FaceVoxel(mvd, v.slope);
                    iter++;
                }
                voxelFrames[f] = working.ToList();
            }
            for(int f = 1; f < 13; f++)
            {

                List<FaceVoxel> altered = new List<FaceVoxel>(voxelFrames[0].Count);
                int[,] taken = new int[120, 120];
                taken.Fill(-1);
                for(int i = 0; i < voxelFrames[f].Count; i++)
                {
                    // do not store this voxel if it lies out of range of the voxel chunk (30x30x30)
                    if(voxelFrames[f][i].vox.x >= 120 || voxelFrames[f][i].vox.y >= 120 || voxelFrames[f][i].vox.z >= 80)
                    {
                        continue;
                    }

                    altered.Add(voxelFrames[f][i]);
                    if(!shadowless && -1 == taken[voxelFrames[f][i].vox.x, voxelFrames[f][i].vox.y]
                         && (voxelFrames[f][i].vox.color > 253 - 21 * 4 || voxelFrames[f][i].vox.color < 253 - 24 * 4)
                         && voxelFrames[f][i].vox.color != 253 - 25 * 4 && voxelFrames[f][i].vox.color != 253 - 27 * 4
                         && voxelFrames[f][i].vox.color != 253 - 17 * 4 && voxelFrames[f][i].vox.color != 253 - 18 * 4 && voxelFrames[f][i].vox.color != 253 - 19 * 4
                         && voxelFrames[f][i].vox.color > 257 - VoxelLogic.wcolorcount * 4)
                    {
                        MagicaVoxelData vox = new MagicaVoxelData();
                        vox.x = voxelFrames[f][i].vox.x;
                        vox.y = voxelFrames[f][i].vox.y;
                        vox.z = (byte)(0);
                        vox.color = 253 - 25 * 4;
                        taken[vox.x, vox.y] = altered.Count();
                        altered.Add(new FaceVoxel(vox, Slope.Cube));
                    }
                }
                voxelFrames[f] = altered.ToList();
            }

            FaceVoxel[][,,] frames = new FaceVoxel[12][,,];

            for(int f = 1; f < 13; f++)
            {
                /*                for (int i = 0; i < voxels.Length; i++)
                                {
                                    voxelFrames[f][i].x += 15;
                                    voxelFrames[f][i].y += 15;
                                }*/
                frames[f - 1] = FaceListToArray(voxelFrames[f], 120, 120, 80, 153);
            }
            return frames;
        }
        public static FaceVoxel[][,,] FieryExplosionHugeW(FaceVoxel[,,] faces, bool blowback, bool shadowless)
        {
            List<FaceVoxel>[] voxelFrames = new List<FaceVoxel>[13];
            voxelFrames[0] = FaceArrayToList(faces);

            for(int i = 0; i < voxelFrames[0].Count; i++)
            {
                voxelFrames[0][i].vox.x += 40;
                voxelFrames[0][i].vox.y += 40;
            }
            for(int f = 1; f < 5; f++)
            {
                List<FaceVoxel> altered = new List<FaceVoxel>(voxelFrames[f - 1].Count);
                FaceVoxel[] vls = new FaceVoxel[voxelFrames[f - 1].Count], working = new FaceVoxel[voxelFrames[f - 1].Count]; //.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)
                voxelFrames[f - 1].CopyTo(vls, 0);
                voxelFrames[f - 1].CopyTo(working, 0);

                int[] minX = new int[60 * 2];
                int[] maxX = new int[60 * 2];
                float[] midX = new float[60 * 2];
                for(int level = 0; level < 60 * 2; level++)
                {
                    minX[level] = vls.Min(v => v.vox.x * ((v.vox.z != level || v.vox.color == 253 - 25 * 4 ||
                        v.vox.color == 253 - 17 * 4 || v.vox.color == 253 - 19 * 4 || v.vox.color == 253 - 18 * 4 || v.vox.color <= 257 - VoxelLogic.wcolorcount * 4) ? 2000 : 1));
                    maxX[level] = vls.Max(v => v.vox.x * ((v.vox.z != level || v.vox.color == 253 - 25 * 4 ||
                        v.vox.color == 253 - 17 * 4 || v.vox.color == 253 - 19 * 4 || v.vox.color == 253 - 18 * 4 || v.vox.color <= 257 - VoxelLogic.wcolorcount * 4) ? 0 : 1));
                    midX[level] = (maxX[level] + minX[level]) / 2F;
                }

                int[] minY = new int[60 * 2];
                int[] maxY = new int[60 * 2];
                float[] midY = new float[60 * 2];
                for(int level = 0; level < 60 * 2; level++)
                {
                    minY[level] = vls.Min(v => v.vox.y * ((v.vox.z != level || v.vox.color == 253 - 25 * 4 ||
                        v.vox.color == 253 - 17 * 4 || v.vox.color == 253 - 19 * 4 || v.vox.color == 253 - 18 * 4 || v.vox.color <= 257 - VoxelLogic.wcolorcount * 4) ? 2000 : 1));
                    maxY[level] = vls.Max(v => v.vox.y * ((v.vox.z != level || v.vox.color == 253 - 25 * 4 ||
                        v.vox.color == 253 - 17 * 4 || v.vox.color == 253 - 19 * 4 || v.vox.color == 253 - 18 * 4 || v.vox.color <= 257 - VoxelLogic.wcolorcount * 4) ? 0 : 1));
                    midY[level] = (maxY[level] + minY[level]) / 2F;
                }

                int minZ = vls.Min(v => v.vox.z * ((v.vox.color == 253 - 25 * 4 ||
                        v.vox.color == 253 - 17 * 4 || v.vox.color == 253 - 19 * 4 || v.vox.color == 253 - 18 * 4 || v.vox.color <= 257 - VoxelLogic.wcolorcount * 4) ? 2000 : 1));
                int maxZ = vls.Max(v => v.vox.z * ((v.vox.color == 253 - 25 * 4 ||
                        v.vox.color == 253 - 17 * 4 || v.vox.color == 253 - 19 * 4 || v.vox.color == 253 - 18 * 4 || v.vox.color <= 257 - VoxelLogic.wcolorcount * 4) ? 0 : 1));
                float midZ = (maxZ + minZ) / 2F;

                int iter = 0;
                foreach(FaceVoxel v in vls)
                {
                    MagicaVoxelData mvd = new MagicaVoxelData();
                    int c = ((255 - v.vox.color) % 4 == 0) ? (255 - v.vox.color) / 4 + VoxelLogic.wcolorcount : (253 - v.vox.color) / 4;
                    if(c == 8 || c == 9) //flesh
                        mvd.color = (byte)((TallFaces.r.Next(1 + f) == 0) ? 253 - 34 * 4 : (TallFaces.r.Next(8) == 0) ? 253 - 19 * 4 : v.vox.color); //random transform to guts
                    else if(c == 34) //guts
                        mvd.color = (byte)((TallFaces.r.Next(18) == 0) ? 253 - 19 * 4 : v.vox.color); //random transform to orange fire
                    else if(c == VoxelLogic.wcolorcount - 1) //clear
                        mvd.color = VoxelLogic.clear; //clear stays clear
                    else if(c == 16)
                        mvd.color = VoxelLogic.clear; //clear inner shadow
                    else if(c == 25)
                        mvd.color = 253 - 25 * 4; //shadow stays shadow
                    else if(c == 27)
                        mvd.color = 253 - 27 * 4; //water stays water
                    else if(c >= VoxelLogic.wcolorcount && c < VoxelLogic.wcolorcount + 5)
                        mvd.color = v.vox.color; // falling water stays falling water
                    else if(c == 40)
                        mvd.color = 253 - 20 * 4; //flickering sparks become normal sparks
                    else if(c >= 21 && c <= 24) //lights
                        mvd.color = 253 - 35 * 4; //glass color for broken lights
                    else if(c == 35) //windows
                        mvd.color = (byte)((TallFaces.r.Next(3) == 0) ? VoxelLogic.clear : v.vox.color); //random transform to clear
                    else if(c == 36) //rotor contrast
                        mvd.color = 253 - 0 * 4; //"foot contrast" or "raw metal contrast" color for broken rotors contrast
                    else if(c == 37) //rotor
                        mvd.color = 253 - 1 * 4; //"foot" or "raw metal" color for broken rotors
                    else if(c == 38 || c == 39)
                        mvd.color = VoxelLogic.clear; //clear non-active rotors
                    else if(c == 19) //orange fire
                        mvd.color = (byte)((TallFaces.r.Next(3) <= 1) ? 253 - 18 * 4 : ((TallFaces.r.Next(5) == 0) ? 253 - 17 * 4 : v.vox.color)); //random transform to yellow fire or smoke
                    else if(c == 18) //yellow fire
                        mvd.color = (byte)((TallFaces.r.Next(4) <= 1) ? 253 - 19 * 4 : ((TallFaces.r.Next(4) == 0) ? 253 - 20 * 4 : v.vox.color)); //random transform to orange fire or sparks
                    else if(c == 20) //sparks
                        mvd.color = (byte)((TallFaces.r.Next(4) == 0) ? VoxelLogic.clear : v.vox.color); //random transform to clear
                    else if(c == 17)
                        mvd.color = v.vox.color; // smoke stays smoke
                    else
                        mvd.color = (byte)((TallFaces.r.Next(9 - f) == 0) ? 253 - ((TallFaces.r.Next(4) == 0) ? 18 * 4 : 19 * 4) : v.vox.color); //random transform to orange or yellow fire
                    float xMove = 0, yMove = 0, zMove = 0;

                    if(mvd.color == 253 - 19 * 4 || mvd.color == 253 - 18 * 4 || mvd.color == 253 - 17 * 4)
                    {
                        zMove = (f - 1) * 1.8f;
                        xMove = (float)(TallFaces.r.NextDouble() * 2.0 - 1.0);
                        yMove = (float)(TallFaces.r.NextDouble() * 2.0 - 1.0);
                    }
                    else
                    {
                        if(v.vox.x > midX[v.vox.z])
                            xMove = ((0 - TallFaces.r.Next(3) - ((blowback) ? 9 : 0) + (v.vox.x - midX[v.vox.z])) * 25F / f * ((v.vox.z - minZ + 1) / (maxZ - minZ + 1F)));
                        else if(v.vox.x < midX[v.vox.z])
                            xMove = ((0 + TallFaces.r.Next(3) - ((blowback) ? 8 : 0) - midX[v.vox.z] + v.vox.x) * 25F / f * ((v.vox.z - minZ + 1) / (maxZ - minZ + 1F)));// / 300F) * (v.vox.z + 5); //5 -
                        if(v.vox.y > midY[v.vox.z])
                            yMove = ((0 - TallFaces.r.Next(3) + (v.vox.y - midY[v.vox.z])) * 25F / f * ((v.vox.z - minZ + 1) / (maxZ - minZ + 1F))); //maxX[v.vox.z] - minX[v.vox.z]
                        else if(v.vox.y < midY[v.vox.z])
                            yMove = ((0 + TallFaces.r.Next(3) - midY[v.vox.z] + v.vox.y) * 25F / f * ((v.vox.z - minZ + 1) / (maxZ - minZ + 1F)));
                        if(mvd.color == 253 - 20 * 4)
                        {
                            zMove = 0.1f;
                            xMove *= 2;
                            yMove *= 2;
                        }
                        else if(minZ > 0)
                            zMove = ((v.vox.z) * (1 - f) / 6F);
                        else
                            zMove = (v.vox.z / ((maxZ + 1 + midZ) * (0.15F))) * (4 - f) * 1.1F;
                    }

                    if(xMove > 20) xMove = 20;
                    if(xMove < -20) xMove = -20;
                    if(yMove > 20) yMove = 20;
                    if(yMove < -20) yMove = -20;
                    //float magnitude = Math.Abs(xMove) + Math.Abs(yMove);
                    float magnitude = (float)Math.Sqrt(xMove * xMove + yMove * yMove);
                    if(xMove > 0)
                    {
                        float nv = v.vox.x + (float)TallFaces.r.NextDouble() * ((xMove / magnitude) * f / 0.3F) + (float)(TallFaces.r.NextDouble() * 6.0 - 3.0); //(Math.Sqrt(Math.Abs(Math.Abs((xMove * f / 3)) - Math.Abs((yMove * f / 4)))) + (float)(TallFaces.r.NextDouble() * 2.0 - 1.0));
                        if(nv < 0)
                        {
                            nv = 0;
                            mvd.color = VoxelLogic.clear;
                        }
                        if(nv > 159)
                        {
                            nv = 159;
                            mvd.color = VoxelLogic.clear;
                        }
                        mvd.x = (byte)(Math.Floor(nv));
                    }
                    else if(xMove < 0)
                    {
                        float nv = v.vox.x - (float)TallFaces.r.NextDouble() * ((xMove / magnitude) * f / -0.3F) + (float)(TallFaces.r.NextDouble() * 6.0 - 3.0); //(float)(Math.Sqrt(Math.Abs(Math.Abs((xMove * f / 3)) - Math.Abs((yMove * f / 4)))) + (float)(TallFaces.r.NextDouble() * 2.0 - 1.0));
                        if(nv < 0)
                        {
                            nv = 0;
                            mvd.color = VoxelLogic.clear;
                        }
                        if(nv > 159)
                        {
                            nv = 159;
                            mvd.color = VoxelLogic.clear;
                        }
                        mvd.x = (byte)(Math.Floor(nv));
                    }
                    else
                    {
                        if(v.vox.x < 0)
                        {
                            mvd.x = 0;
                            mvd.color = VoxelLogic.clear;
                        }
                        if(v.vox.x > 159)
                        {
                            mvd.x = 159;
                            mvd.color = VoxelLogic.clear;
                        }
                        else mvd.x = v.vox.x;
                    }
                    if(yMove > 0)
                    {
                        float nv = v.vox.y + (float)TallFaces.r.NextDouble() * ((yMove / magnitude) * f / 0.3F) + (float)(TallFaces.r.NextDouble() * 6.0 - 3.0); //(float)(Math.Sqrt(Math.Abs(Math.Abs((yMove * f / 3)) - Math.Abs((xMove * f / 4)))) + (float)(TallFaces.r.NextDouble() * 2.0 - 1.0));
                        if(nv < 0)
                        {
                            nv = 0;
                            mvd.color = VoxelLogic.clear;
                        }
                        if(nv > 159)
                        {
                            nv = 159;
                            mvd.color = VoxelLogic.clear;
                        }
                        mvd.y = (byte)(Math.Floor(nv));
                    }
                    else if(yMove < 0)
                    {
                        float nv = v.vox.y - (float)TallFaces.r.NextDouble() * ((yMove / magnitude) * f / -0.3F) + (float)(TallFaces.r.NextDouble() * 6.0 - 3.0); //(float)(Math.Sqrt(Math.Abs(Math.Abs((yMove * f / 3)) - Math.Abs((xMove * f / 4)))) + (float)(TallFaces.r.NextDouble() * 2.0 - 1.0));
                        if(nv < 0)
                        {
                            nv = 0;
                            mvd.color = VoxelLogic.clear;
                        }
                        if(nv > 159)
                        {
                            nv = 159;
                            mvd.color = VoxelLogic.clear;
                        }
                        mvd.y = (byte)(Math.Floor(nv));
                    }
                    else
                    {
                        if(v.vox.y < 0)
                        {
                            mvd.y = 0;
                            mvd.color = VoxelLogic.clear;
                        }
                        if(v.vox.y > 159)
                        {
                            mvd.y = 159;
                            mvd.color = VoxelLogic.clear;
                        }
                        else mvd.y = v.vox.y;
                    }

                    if(zMove != 0)
                    {
                        float nv = (v.vox.z + (zMove * 1.5f / f));

                        if(nv <= 0 && f < 8) nv = TallFaces.r.Next(2); //bounce
                        else if(nv < 0) nv = 0;

                        if(nv > 119) nv = 119;
                        mvd.z = (byte)Math.Round(nv);
                    }
                    else
                    {
                        mvd.z = v.vox.z;
                    }
                    working[iter] = new FaceVoxel(mvd, randomSlope());
                    iter++;
                }
                voxelFrames[f] = working.ToList();
            }
            for(int f = 5; f < 13; f++)
            {

                List<FaceVoxel> altered = new List<FaceVoxel>(voxelFrames[f - 1].Count);
                FaceVoxel[] vls = new FaceVoxel[voxelFrames[f - 1].Count], working = new FaceVoxel[voxelFrames[f - 1].Count]; //.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)
                voxelFrames[f - 1].CopyTo(vls, 0);
                voxelFrames[f - 1].CopyTo(working, 0);


                int[] minX = new int[60 * 2];
                int[] maxX = new int[60 * 2];
                float[] midX = new float[60 * 2];
                for(int level = 0; level < 60 * 2; level++)
                {
                    minX[level] = vls.Min(v => v.vox.x * ((v.vox.z != level || v.vox.color == 253 - 25 * 4 ||
                        v.vox.color == 253 - 17 * 4 || v.vox.color == 253 - 19 * 4 || v.vox.color == 253 - 18 * 4 || v.vox.color <= 257 - VoxelLogic.wcolorcount * 4) ? 2000 : 1));
                    maxX[level] = vls.Max(v => v.vox.x * ((v.vox.z != level || v.vox.color == 253 - 25 * 4 ||
                        v.vox.color == 253 - 17 * 4 || v.vox.color == 253 - 19 * 4 || v.vox.color == 253 - 18 * 4 || v.vox.color <= 257 - VoxelLogic.wcolorcount * 4) ? 0 : 1));
                    midX[level] = (maxX[level] + minX[level]) / 2F;
                }

                int[] minY = new int[60 * 2];
                int[] maxY = new int[60 * 2];
                float[] midY = new float[60 * 2];
                for(int level = 0; level < 60 * 2; level++)
                {
                    minY[level] = vls.Min(v => v.vox.y * ((v.vox.z != level || v.vox.color == 253 - 25 * 4 ||
                        v.vox.color == 253 - 17 * 4 || v.vox.color == 253 - 19 * 4 || v.vox.color == 253 - 18 * 4 || v.vox.color <= 257 - VoxelLogic.wcolorcount * 4) ? 2000 : 1));
                    maxY[level] = vls.Max(v => v.vox.y * ((v.vox.z != level || v.vox.color == 253 - 25 * 4 ||
                        v.vox.color == 253 - 17 * 4 || v.vox.color == 253 - 19 * 4 || v.vox.color == 253 - 18 * 4 || v.vox.color <= 257 - VoxelLogic.wcolorcount * 4) ? 0 : 1));
                    midY[level] = (maxY[level] + minY[level]) / 2F;
                }

                int minZ = vls.Min(v => v.vox.z * ((v.vox.color == 253 - 25 * 4 ||
                        v.vox.color == 253 - 17 * 4 || v.vox.color == 253 - 19 * 4 || v.vox.color == 253 - 18 * 4 || v.vox.color <= 257 - VoxelLogic.wcolorcount * 4) ? 2000 : 1));
                int maxZ = vls.Max(v => v.vox.z * ((v.vox.color == 253 - 25 * 4 ||
                        v.vox.color == 253 - 17 * 4 || v.vox.color == 253 - 19 * 4 || v.vox.color == 253 - 18 * 4 || v.vox.color <= 257 - VoxelLogic.wcolorcount * 4) ? 0 : 1));
                float midZ = (maxZ + minZ) / 2F;

                int iter = 0;
                foreach(FaceVoxel v in vls)
                {
                    MagicaVoxelData mvd = new MagicaVoxelData();
                    int c = ((255 - v.vox.color) % 4 == 0) ? (255 - v.vox.color) / 4 + VoxelLogic.wcolorcount : (253 - v.vox.color) / 4;
                    if(c == 8 || c == 9) //flesh
                        mvd.color = (byte)((TallFaces.r.Next(f) == 0) ? 253 - 34 * 4 : (TallFaces.r.Next(6) == 0 && f < 10) ? 253 - 19 * 4 : v.vox.color); //random transform to guts
                    else if(c == 34) //guts
                        mvd.color = (byte)((TallFaces.r.Next(20) == 0 && f < 10) ? 253 - 19 * 4 : v.vox.color); //random transform to orange fire
                    else if(c == VoxelLogic.wcolorcount - 1) //VoxelLogic.clear and markers
                        mvd.color = (byte)VoxelLogic.clear; //VoxelLogic.clear stays VoxelLogic.clear
                    else if(c == 16)
                        mvd.color = VoxelLogic.clear; //VoxelLogic.clear inner shadow
                    else if(c == 25)
                        mvd.color = 253 - 25 * 4; //shadow stays shadow
                    else if(c == 27)
                        mvd.color = 253 - 27 * 4; //water stays water
                    else if(c >= VoxelLogic.wcolorcount && c < VoxelLogic.wcolorcount + 5)
                        mvd.color = (byte)(255 - (c - VoxelLogic.wcolorcount) * 4); // falling water stays falling water
                    else if(c == 40)
                        mvd.color = 253 - 20 * 4; //flickering sparks become normal sparks
                    else if(c >= 21 && c <= 24) //lights
                        mvd.color = 253 - 35 * 4; //glass color for broken lights
                    else if(c == 35) //windows
                        mvd.color = (byte)((TallFaces.r.Next(3) == 0) ? VoxelLogic.clear : v.vox.color); //random transform to VoxelLogic.clear
                    else if(c == 36) //rotor contrast
                        mvd.color = 253 - 0 * 4; //"foot contrast" color for broken rotors contrast
                    else if(c == 37) //rotor
                        mvd.color = 253 - 1 * 4; //"foot" color for broken rotors
                    else if(c == 38 || c == 39)
                        mvd.color = VoxelLogic.clear; //VoxelLogic.clear non-active rotors
                    else if(c == 19) //orange fire
                        mvd.color = (byte)((TallFaces.r.Next(9) + 2 <= f) ? 253 - 17 * 4 : ((TallFaces.r.Next(3) <= 1) ? 253 - 18 * 4 : ((TallFaces.r.Next(3) == 0) ? 253 - 17 * 4 : v.vox.color))); //random transform to yellow fire or smoke
                    else if(c == 18) //yellow fire
                        mvd.color = (byte)((TallFaces.r.Next(9) + 1 <= f) ? 253 - 17 * 4 : ((TallFaces.r.Next(3) <= 1) ? 253 - 19 * 4 : ((TallFaces.r.Next(4) == 0) ? 253 - 17 * 4 : ((TallFaces.r.Next(4) == 0) ? 253 - 20 * 4 : v.vox.color)))); //random transform to orange fire, smoke, or sparks
                    else if(c == 20) //sparks
                        mvd.color = (byte)((TallFaces.r.Next(4) > 0 && TallFaces.r.Next(12) > f) ? v.vox.color : VoxelLogic.clear); //random transform to VoxelLogic.clear
                    else if(c == 17) //smoke
                        mvd.color = (byte)((TallFaces.r.Next(10) + 3 <= f) ? VoxelLogic.clear : 253 - 17 * 4); //random transform to VoxelLogic.clear
                    else
                        mvd.color = (byte)((TallFaces.r.Next(f * 4) <= 6) ? 253 - ((TallFaces.r.Next(4) == 0) ? 18 * 4 : 19 * 4) : v.vox.color); //random transform to orange or yellow fire

                    float xMove = 0, yMove = 0, zMove = 0;
                    if(mvd.color == 253 - 19 * 4 || mvd.color == 253 - 18 * 4 || mvd.color == 253 - 17 * 4)
                    {
                        zMove = f * 0.5f;
                        xMove = (float)(TallFaces.r.NextDouble() * 2.0 - 1.0);
                        yMove = (float)(TallFaces.r.NextDouble() * 2.0 - 1.0);
                    }
                    else
                    {
                        /*
                         if (v.x > midX[v.vox.z])
                            xMove = ((0 - TallFaces.r.Next(3) - ((blowback) ? 9 : 0) + (v.x - midX[v.vox.z])) * 2F * ((v.vox.z - minZ + 1) / (maxZ - minZ + 1F)));
                        //for lower values: distance from current voxel x to center + (between 2 to 0), times variable based on height
                        else if (v.x < midX[v.vox.z])
                            xMove = ((0 + TallFaces.r.Next(3) - ((blowback) ? 8 : 0) - midX[v.vox.z] + v.x) * 2F * ((v.vox.z - minZ + 1) / (maxZ - minZ + 1F)));// / 300F) * (v.vox.z + 5); //5 -
                        if (v.y > midY[v.vox.z])
                            yMove = ((0 - TallFaces.r.Next(3) + (v.y - midY[v.vox.z])) * 2F * ((v.vox.z - minZ + 3) / (maxZ - minZ + 1F))); //maxX[v.vox.z] - minX[v.vox.z]
                        else if (v.y < midY[v.vox.z])
                            yMove = ((0 + TallFaces.r.Next(3) - midY[v.vox.z] + v.y) * 2F * ((v.vox.z - minZ + 3) / (maxZ - minZ + 1F)));
                         */


                        if(v.vox.x > midX[v.vox.z])
                            xMove = ((0 - TallFaces.r.Next(3) - ((blowback) ? 7 : 0) + (v.vox.x - midX[v.vox.z])) / (f + 8) * 25F * ((v.vox.z - minZ + 1) / (maxZ - minZ + 1F)));
                        else if(v.vox.x < midX[v.vox.z])
                            xMove = ((0 + TallFaces.r.Next(3) - ((blowback) ? 6 : 0) - midX[v.vox.z] + v.vox.x) / (f + 8) * 25F * ((v.vox.z - minZ + 1) / (maxZ - minZ + 1F)));
                        if(v.vox.y > midY[v.vox.z])
                            yMove = ((0 - TallFaces.r.Next(3) + (v.vox.y - midY[v.vox.z])) / (f + 8) * 25F * ((v.vox.z - minZ + 1) / (maxZ - minZ + 1F)));
                        else if(v.vox.y < midY[v.vox.z])
                            yMove = ((0 + TallFaces.r.Next(3) - midY[v.vox.z] + v.vox.y) / (f + 8) * 25F * ((v.vox.z - minZ + 1) / (maxZ - minZ + 1F)));

                        if(mvd.color == 253 - 20 * 4)
                        {
                            zMove = 0.1f;
                            xMove *= 2;
                            yMove *= 2;
                        }
                        else if(f < 6 && minZ == 0)
                            zMove = (v.vox.z / ((maxZ + 1) * (0.2F))) * (5 - f) * 0.6F;
                        else
                            zMove = (1 - f * 1.85F);
                    }
                    if(v.vox.z <= 1 && f >= 10)
                    {
                        xMove = 0;
                        yMove = 0;
                    }
                    if(xMove > 20) xMove = 20;
                    if(xMove < -20) xMove = -20;
                    if(yMove > 20) yMove = 20;
                    if(yMove < -20) yMove = -20;
                    // float magnitude = Math.Abs(xMove) + Math.Abs(yMove);
                    float magnitude = (float)Math.Sqrt(xMove * xMove + yMove * yMove);

                    if(xMove > 0)
                    {
                        float nv = v.vox.x + (float)TallFaces.r.NextDouble() * ((xMove / magnitude) * 35F / f) + (float)(TallFaces.r.NextDouble() * 8.0 - 4.0);
                        //                        float nv = (float)(v.vox.x + Math.Sqrt(Math.Abs(Math.Abs(xMove / (0.07f * (f + 5))) - Math.Abs((yMove / (0.09f * (f + 5)))))) + (float)(TallFaces.r.NextDouble() * 2.0 - 1.0));
                        if(nv < 0)
                        {
                            nv = 0;
                            mvd.color = VoxelLogic.clear;
                        }
                        if(nv > 159)
                        {
                            nv = 159;
                            mvd.color = VoxelLogic.clear;
                        }
                        mvd.x = (byte)(Math.Floor(nv));
                    }
                    else if(xMove < 0)
                    {
                        float nv = v.vox.x - (float)TallFaces.r.NextDouble() * ((xMove / magnitude) * -35F / f) + (float)(TallFaces.r.NextDouble() * 8.0 - 4.0);

                        //float nv = (float)(v.vox.x - Math.Sqrt(Math.Abs(Math.Abs(xMove / (0.07f * (f + 5))) - Math.Abs((yMove / (0.09f * (f + 5)))))) + (float)(TallFaces.r.NextDouble() * 2.0 - 1.0));
                        if(nv < 0)
                        {
                            nv = 0;
                            mvd.color = VoxelLogic.clear;
                        }
                        if(nv > 159)
                        {
                            nv = 159;
                            mvd.color = VoxelLogic.clear;
                        }
                        mvd.x = (byte)(Math.Floor(nv));
                    }
                    else
                    {
                        if(v.vox.x < 0)
                        {
                            mvd.x = 0;
                            mvd.color = VoxelLogic.clear;
                        }
                        if(v.vox.x > 159)
                        {
                            mvd.x = 159;
                            mvd.color = VoxelLogic.clear;
                        }
                        else mvd.x = v.vox.x;
                    }
                    if(yMove > 0)
                    {
                        float nv = v.vox.y + (float)TallFaces.r.NextDouble() * ((yMove / magnitude) * 35F / f) + (float)(TallFaces.r.NextDouble() * 8.0 - 4.0);
                        //float nv = (float)(v.y + Math.Sqrt(Math.Abs(Math.Abs(yMove / (0.07f * (f + 5))) - Math.Abs((xMove / (0.09f * (f + 5)))))) + (float)(TallFaces.r.NextDouble() * 2.0 - 1.0));
                        if(nv < 0)
                        {
                            nv = 0;
                            mvd.color = VoxelLogic.clear;
                        }
                        if(nv > 159)
                        {
                            nv = 159;
                            mvd.color = VoxelLogic.clear;
                        }
                        mvd.y = (byte)(Math.Floor(nv));
                    }
                    else if(yMove < 0)
                    {
                        float nv = v.vox.y - (float)TallFaces.r.NextDouble() * ((yMove / magnitude) * -35F / f) + (float)(TallFaces.r.NextDouble() * 8.0 - 4.0);
                        //(float)(v.y - Math.Sqrt(Math.Abs(Math.Abs(yMove / (0.07f * (f + 5))) - Math.Abs((xMove / (0.09f * (f + 5)))))) + (float)(TallFaces.r.NextDouble() * 2.0 - 1.0));
                        if(nv < 0)
                        {
                            nv = 0;
                            mvd.color = VoxelLogic.clear;
                        }
                        if(nv > 159)
                        {
                            nv = 159;
                            mvd.color = VoxelLogic.clear;
                        }
                        mvd.y = (byte)(Math.Ceiling(nv));
                    }
                    else
                    {
                        if(v.vox.y < 0)
                        {
                            mvd.y = 0;
                            mvd.color = VoxelLogic.clear;
                        }
                        if(v.vox.y > 159)
                        {
                            mvd.y = 159;
                            mvd.color = VoxelLogic.clear;
                        }
                        else mvd.y = v.vox.y;
                    }

                    if(zMove != 0)
                    {
                        float nv = (v.vox.z + (zMove * 1.3f));

                        if(nv <= 0 && f < 8) nv = TallFaces.r.Next(2); //bounce
                        else if(nv < 0) nv = 0;

                        if(nv > 119)
                        {
                            nv = 119;
                            mvd.color = VoxelLogic.clear;
                        }
                        mvd.z = (byte)Math.Round(nv);
                    }
                    else
                    {
                        mvd.z = v.vox.z;
                    }
                    working[iter] = new FaceVoxel(mvd, v.slope);
                    iter++;
                }
                voxelFrames[f] = working.ToList();
            }
            for(int f = 1; f < 13; f++)
            {

                List<FaceVoxel> altered = new List<FaceVoxel>(voxelFrames[0].Count);
                int[,] taken = new int[160, 160];
                taken.Fill(-1);
                for(int i = 0; i < voxelFrames[f].Count; i++)
                {
                    // do not store this voxel if it lies out of range of the voxel chunk (30x30x30)
                    if(voxelFrames[f][i].vox.x >= 160 || voxelFrames[f][i].vox.y >= 160 || voxelFrames[f][i].vox.z >= 120)
                    {
                        continue;
                    }

                    altered.Add(voxelFrames[f][i]);
                    if(!shadowless && -1 == taken[voxelFrames[f][i].vox.x, voxelFrames[f][i].vox.y]
                         && (voxelFrames[f][i].vox.color > 253 - 21 * 4 || voxelFrames[f][i].vox.color < 253 - 24 * 4)
                         && voxelFrames[f][i].vox.color != 253 - 25 * 4 && voxelFrames[f][i].vox.color != 253 - 27 * 4
                         && voxelFrames[f][i].vox.color != 253 - 17 * 4 && voxelFrames[f][i].vox.color != 253 - 18 * 4 && voxelFrames[f][i].vox.color != 253 - 19 * 4
                         && voxelFrames[f][i].vox.color > 257 - VoxelLogic.wcolorcount * 4)
                    {
                        MagicaVoxelData vox = new MagicaVoxelData();
                        vox.x = voxelFrames[f][i].vox.x;
                        vox.y = voxelFrames[f][i].vox.y;
                        vox.z = (byte)(0);
                        vox.color = 253 - 25 * 4;
                        taken[vox.x, vox.y] = altered.Count();
                        altered.Add(new FaceVoxel(vox, Slope.Cube));
                    }
                }
                voxelFrames[f] = altered.ToList();
            }

            FaceVoxel[][,,] frames = new FaceVoxel[12][,,];

            for(int f = 1; f < 13; f++)
            {
                /*                for (int i = 0; i < voxels.Length; i++)
                                {
                                    voxelFrames[f][i].x += 15;
                                    voxelFrames[f][i].y += 15;
                                }*/
                frames[f - 1] = FaceListToArray(voxelFrames[f], 160, 160, 120, 153);
            }
            return frames;
        }
        private static Slope[] Slopes = new Slope[] {
        Slope.Cube,
        Slope.BrightTop,
        Slope.DimTop,
        Slope.BrightDim,
        Slope.BrightDimTop,
        Slope.BrightBottom,
        Slope.DimBottom,
        Slope.BrightDimBottom,
        Slope.BrightBack,
        Slope.DimBack,
        Slope.BrightTopBack,
        Slope.DimTopBack,
        Slope.BrightBottomBack,
        Slope.DimBottomBack,
        Slope.BackBack
        };

        private static Slope randomSlope()
        {
            return Slopes[TallFaces.r.Next(Slopes.Length)];
        }

        public static List<FaceVoxel> FaceArrayToList(FaceVoxel[,,] faces)
        {
            List<FaceVoxel> flist = new List<FaceVoxel>(100000);
            int xSize = faces.GetLength(0), ySize = faces.GetLength(1), zSize = faces.GetLength(2);

            for(byte x = 0; x < xSize; x++)
            {
                for(byte y = 0; y < ySize; y++)
                {
                    for(byte z = 0; z < zSize; z++)
                    {
                        if(faces[x, y, z] != null)
                            flist.Add(new FaceVoxel(faces[x, y, z].vox, faces[x, y, z].slope));
                    }
                }
            }
            return flist;
        }

        public static FaceVoxel[,,] FaceListToArray(List<FaceVoxel> voxelData, int xSize, int ySize, int zSize, byte shadowColor)
        {
            FaceVoxel[,,] data = new FaceVoxel[xSize, ySize, zSize];
            foreach(FaceVoxel mvd in voxelData)
            {
                if(mvd == null || mvd.vox.x >= xSize || mvd.vox.y >= ySize || mvd.vox.z >= zSize)
                    continue;
                if(data[mvd.vox.x, mvd.vox.y, mvd.vox.z] == null ||
                    (!(mvd.vox.color == VoxelLogic.clear || (VoxelLogic.VisualMode == "CU" && (data[mvd.vox.x, mvd.vox.y, mvd.vox.z].vox.color == CURedux.emitter0 || data[mvd.vox.x, mvd.vox.y, mvd.vox.z].vox.color == CURedux.trail0
                     || data[mvd.vox.x, mvd.vox.y, mvd.vox.z].vox.color == CURedux.emitter1 || data[mvd.vox.x, mvd.vox.y, mvd.vox.z].vox.color == CURedux.trail1)))
                     && data[mvd.vox.x, mvd.vox.y, mvd.vox.z].vox.color == shadowColor))
                    data[mvd.vox.x, mvd.vox.y, mvd.vox.z] = mvd;
            }
            return data;
        }

        public static bool IsSlopeThick(Slope s)
        {
            return s == Slope.Cube || s == Slope.BackBackBottomThick || s == Slope.BackBackTopThick || s == Slope.BrightBottomBackThick || s == Slope.BrightDimBottomThick || s == Slope.BrightDimTopThick
                 || s == Slope.BrightTopBackThick || s == Slope.DimBottomBackThick || s == Slope.DimTopBackThick;
        }

        public static byte[,,] FaceArrayToByteArray(FaceVoxel[,,] faces)
        {
            int xSize = faces.GetLength(0), ySize = faces.GetLength(1), zSize = faces.GetLength(2);

            byte[,,] b = new byte[xSize, ySize, zSize];

            for(byte x = 0; x < xSize; x++)
            {
                for(byte y = 0; y < ySize; y++)
                {
                    for(byte z = 0; z < zSize; z++)
                    {
                        if(faces[x, y, z] != null && IsSlopeThick(faces[x, y, z].slope))
                            b[x,y,z] = faces[x, y, z].vox.color;
                    }
                }
            }
            return b;
        }


        public static FaceVoxel[,,] DoubleSize(FaceVoxel[,,] faces)
        {
            int xSize = faces.GetLength(0), ySize = faces.GetLength(1), zSize = faces.GetLength(2);
            FaceVoxel[,,] result = new FaceVoxel[xSize * 2, ySize * 2, zSize * 2];


            for(int z = zSize - 1; z >= 0; z--)
            {
                for(int x = 0; x < xSize - 1; x++)
                {
                    for(int y = ySize - 1; y >= 0; y--)
                    {
                        if(faces[x, y, z] != null)
                        {
                            FaceVoxel fv = faces[x, y, z];

                            switch(fv.slope)
                            {
                                case Slope.Cube:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.BrightTop:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightTop);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BrightTop);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    //result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    //result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightTop);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightTop);
                                    break;
                                case Slope.DimTop:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.DimTop);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.DimTop);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.DimTop);
                                    //result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.DimTop);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.RearBrightTop:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.RearBrightTop);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.RearBrightTop);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    //result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.RearBrightTop);
                                    //result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightTop);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.RearBrightTop);
                                    break;
                                case Slope.RearDimTop:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.RearDimTop);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.RearDimTop);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.RearDimTop);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.RearDimTop);
                                    //result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.BrightDim:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightDim);
                                    //result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightDim);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDim);
                                    //result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightDim);
                                    break;
                                case Slope.BrightDimTop:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightDimTop);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BrightDimTop);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightDimTop); //cube
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightDimTop);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDimTop);
                                    //result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightDimTop);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightDimTop);
                                    break;
                                case Slope.BrightBottom:
                                    //result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightTop);
                                    //result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BrightTop);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightBottom);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightBottom);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightBottom);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightBottom);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.DimBottom:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.DimBottom);
                                    //result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.DimTop);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.DimBottom);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.DimTop);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.DimBottom);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.DimBottom);
                                    break;
                                case Slope.RearBrightBottom:
                                    //result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.RearBrightTop);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.RearBrightBottom);
                                    //result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.RearBrightTop);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.RearBrightBottom);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.RearBrightBottom);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.RearBrightBottom);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.RearDimBottom:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.RearDimBottom);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.RearDimBottom);
                                    //result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.RearDimTop);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.RearDimTop);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.RearDimBottom);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.RearDimBottom);
                                    break;
                                case Slope.BrightDimBottom:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightDimBottom);
                                    //result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightDimBottom);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightDimBottom);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDimBottom);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDimBottom);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightDimBottom); // cube
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightDimBottom);
                                    break;
                                case Slope.BrightBack:
                                    //result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightBack);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BrightBack);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightBack);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    //result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDim);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightBack);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightBack);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.DimBack:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.DimBack);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.DimBack);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.DimBack);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.DimBack);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.BrightTopBack:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightTopBack); // newest
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BrightTopBack);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightTopBack);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightTopBack); // cube
                                    //result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDim);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightTopBack);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightTopBack);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightTopBack);
                                    break;
                                case Slope.DimTopBack:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.DimTopBack); // cube
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.DimTopBack);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.DimTopBack);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.DimTopBack); //newest
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.DimTopBack);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.DimTopBack);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.DimTopBack);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.BrightBottomBack:
                                    //result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightBack);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BrightBottomBack);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightBottomBack);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightBottomBack);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightBottomBack);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightBottomBack);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightBottomBack);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightBottomBack); //cube
                                    break;
                                case Slope.DimBottomBack:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.DimBottomBack);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.DimBottomBack);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.DimBottomBack);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.DimBottomBack); //cube
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.DimBottomBack);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.DimBottomBack);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.DimBottomBack);
                                    break;
                                case Slope.BackBack:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BackBack);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    //result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BackBack);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BackBack);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    //result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BackBack);
                                    break;
                                case Slope.BackBackTop:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BackBackTop);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BackBackTop); //cube
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BackBackTop);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BackBackTop);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BackBackTop);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BackBackTop);
                                    //result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BackBackTop);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BackBackTop);
                                    break;
                                case Slope.BackBackBottom:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BackBackBottom);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BackBackBottom);
                                    //result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BackBackBottom);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BackBackBottom);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BackBackBottom);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BackBackBottom); //cube
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BackBackBottom);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BackBackBottom);
                                    break;
                                case Slope.BrightDimTopThick:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BrightDimTopThick);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDimTopThick);
                                    //result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightDimTopThick);
                                    break;
                                case Slope.BrightDimBottomThick:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightDimBottomThick);
                                    //result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightDimBottomThick);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDimBottomThick);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.BrightTopBackThick:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightTopBackThick); // newest
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube); // cube
                                    //result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDim);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightTopBackThick);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightTopBackThick);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.DimTopBackThick:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.Cube); // cube
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.DimTopBackThick); //newest
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.DimTopBackThick);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.DimTopBackThick);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.BrightBottomBackThick:
                                    //result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightBack);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BrightBottomBackThick);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightBottomBackThick);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightBottomBackThick);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.DimBottomBackThick:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.DimBottomBackThick);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.DimBottomBackThick);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.DimBottomBackThick);
                                    break;
                                case Slope.BackBackTopThick:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BackBackTopThick);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BackBackTopThick);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    //result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BackBackTop);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BackBackTopThick);
                                    break;
                                case Slope.BackBackBottomThick:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BackBackBottomThick);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    //result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BackBackBottom);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BackBackBottomThick);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BackBackBottomThick);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                            }
                        }
                    }
                }
            }
            return result;
        }

        public static FaceVoxel[,,] DoubleSizePrint(FaceVoxel[,,] faces)
        {
            int xSize = faces.GetLength(0), ySize = faces.GetLength(1), zSize = faces.GetLength(2);
            FaceVoxel[,,] result = new FaceVoxel[xSize * 2, ySize * 2, zSize * 2];


            for(int z = zSize - 1; z >= 0; z--)
            {
                for(int x = 0; x < xSize - 1; x++)
                {
                    for(int y = ySize - 1; y >= 0; y--)
                    {
                        if(faces[x, y, z] != null)
                        {
                            FaceVoxel fv = faces[x, y, z];

                            switch(fv.slope)
                            {
                                case Slope.Cube:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.BrightTop:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightTop);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BrightTop);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    //result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    //result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightTop);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightTop);
                                    break;
                                case Slope.DimTop:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.DimTop);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.DimTop);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.DimTop);
                                    //result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.DimTop);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.RearBrightTop:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.RearBrightTop);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.RearBrightTop);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    //result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.RearBrightTop);
                                    //result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightTop);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.RearBrightTop);
                                    break;
                                case Slope.RearDimTop:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.RearDimTop);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.RearDimTop);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.RearDimTop);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.RearDimTop);
                                    //result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.BrightDim:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightDim);
                                    //result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightDim);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDim);
                                    //result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightDim);
                                    break;
                                case Slope.BrightDimTop:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightDimTop);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BrightDimTop);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightDimTopThick); //cube
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightDimTop);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDimTop);
                                    //result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightDimTop);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightDimTop);
                                    break;
                                case Slope.BrightBottom:
                                    //result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightTop);
                                    //result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BrightTop);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightBottom);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightBottom);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightBottom);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightBottom);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.DimBottom:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.DimBottom);
                                    //result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.DimTop);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.DimBottom);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.DimTop);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.DimBottom);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.DimBottom);
                                    break;
                                case Slope.RearBrightBottom:
                                    //result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.RearBrightTop);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.RearBrightBottom);
                                    //result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.RearBrightTop);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.RearBrightBottom);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.RearBrightBottom);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.RearBrightBottom);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.RearDimBottom:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.RearDimBottom);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.RearDimBottom);
                                    //result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.RearDimTop);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.RearDimTop);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.RearDimBottom);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.RearDimBottom);
                                    break;
                                case Slope.BrightDimBottom:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightDimBottom);
                                    //result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightDimBottom);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightDimBottom);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDimBottom);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDimBottom);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightDimBottomThick); // cube
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightDimBottom);
                                    break;
                                case Slope.BrightBack:
                                    //result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightBack);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BrightBack);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightBack);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    //result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDim);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightBack);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightBack);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.DimBack:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.DimBack);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.DimBack);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.DimBack);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.DimBack);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.BrightTopBack:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightTopBack); // newest
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BrightTopBack);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightTopBack);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightTopBackThick); // cube
                                    //result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDim);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightTopBack);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightTopBack);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightTopBack);
                                    break;
                                case Slope.DimTopBack:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.DimTopBackThick); // cube
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.DimTopBack);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.DimTopBack);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.DimTopBack); //newest
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.DimTopBack);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.DimTopBack);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.DimTopBack);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.BrightBottomBack:
                                    //result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightBack);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BrightBottomBack);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightBottomBack);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightBottomBack);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightBottomBack);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightBottomBack);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightBottomBack);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightBottomBackThick); //cube
                                    break;
                                case Slope.DimBottomBack:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.DimBottomBack);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.DimBottomBack);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.DimBottomBack);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.DimBottomBackThick); //cube
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.DimBottomBack);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.DimBottomBack);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.DimBottomBack);
                                    break;
                                case Slope.BackBack:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BackBack);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    //result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BackBack);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BackBack);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    //result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BackBack);
                                    break;
                                case Slope.BackBackTop:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BackBackTop);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BackBackTopThick); //cube
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BackBackTop);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BackBackTop);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BackBackTop);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BackBackTop);
                                    //result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BackBackTop);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BackBackTop);
                                    break;
                                case Slope.BackBackBottom:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BackBackBottom);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BackBackBottom);
                                    //result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BackBackBottom);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BackBackBottom);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BackBackBottom);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BackBackBottomThick); //cube
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BackBackBottom);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BackBackBottom);
                                    break;
                                case Slope.BrightDimTopThick:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BrightDimTopThick);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDimTopThick);
                                    //result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightDimTopThick);
                                    break;
                                case Slope.BrightDimBottomThick:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightDimBottomThick);
                                    //result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightDimBottomThick);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDimBottomThick);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.BrightTopBackThick:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightTopBackThick); // newest
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube); // cube
                                    //result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDim);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightTopBackThick);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightTopBackThick);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.DimTopBackThick:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.Cube); // cube
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.DimTopBackThick); //newest
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.DimTopBackThick);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.DimTopBackThick);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.BrightBottomBackThick:
                                    //result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightBack);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BrightBottomBackThick);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightBottomBackThick);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightBottomBackThick);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.DimBottomBackThick:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.DimBottomBackThick);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.DimBottomBackThick);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.DimBottomBackThick);
                                    break;
                                case Slope.BackBackTopThick:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BackBackTopThick);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BackBackTopThick);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    //result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BackBackTop);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BackBackTopThick);
                                    break;
                                case Slope.BackBackBottomThick:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BackBackBottomThick);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    //result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BackBackBottom);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BackBackBottomThick);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BackBackBottomThick);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                            }
                        }
                    }
                }
            }
            return result;
        }

        public static FaceVoxel[,,] RecolorCA(FaceVoxel[,,] voxelData, int smoothLevel)
        {
            if(smoothLevel <= 1)
                return voxelData;
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);
            //Dictionary<byte, int> colorCount = new Dictionary<byte, int>();
            int[] colorCount = new int[256];
            FaceVoxel[][,,] vs = new FaceVoxel[smoothLevel][,,];
            vs[0] = voxelData;
            for(int v = 1; v < smoothLevel; v++)
            {
                vs[v] = vs[v - 1].Replicate();
                for(int x = 0; x < xSize; x++)
                {
                    for(int y = 0; y < ySize; y++)
                    {
                        for(int z = 0; z < zSize; z++)
                        {
                            if(vs[v - 1][x, y, z] == null)
                                continue;
                            /*
                            Array.Clear(colorCount, 0, 256);
                            if(x == 0 || y == 0 || z == 0 || x == xSize - 1 || y == ySize - 1 || z == zSize - 1
                                || (254 - vs[v - 1][x, y, z].vox.color) % 4 == 0 || TransformLogic.ColorValue(vs[v - 1][x, y, z].vox.color) > 50)
                            {
                                colorCount[vs[v - 1][x, y, z].vox.color] = 10000;
                            }
                            else
                            {
                                for(int xx = -1; xx < 2; xx++)
                                {
                                    for(int yy = -1; yy < 2; yy++)
                                    {
                                        for(int zz = -1; zz < 2; zz++)
                                        {

                                            if(vs[v - 1][x + xx, y + yy, z + zz] != null && TransformLogic.ColorValue(vs[v - 1][x + xx, y + yy, z + zz].vox.color) >= 16)
                                            {
                                                colorCount[vs[v - 1][x + xx, y + yy, z + zz].vox.color]++;
                                            }
                                        }
                                    }
                                }
                            }
                            int max = 0, cc = colorCount[0], tmp;
                            for(int idx = 1; idx < 256; idx++)
                            {
                                tmp = colorCount[idx];
                                if(tmp > cc)
                                {
                                    cc = tmp;
                                    max = idx;
                                }
                            }
                            vs[v][x, y, z].vox.color = (byte)max;
                            */
                            byte m = NearbyFilled(vs[v - 1], x, y, z), old = vs[v - 1][x, y, z].vox.color;
                            int cv = TransformLogic.ColorValue(m), oldv = TransformLogic.ColorValue(old);
                            if((cv >= 16 && cv < 50 && oldv < 50) || (cv >= 50 && oldv >= 50))
                                vs[v][x, y, z].vox.color = m;
                        }
                    }

                }
            }
            return vs[smoothLevel - 1];
        }

        public static string WriteOFF(FaceVoxel[,,] faces, int palette)
        {
            OrderedDictionary<Point3D, int> pts = new OrderedDictionary<Point3D, int>();
            HashSet<Face3D> fs = new HashSet<Face3D>();
            int ord = 0;
            StringBuilder sb = new StringBuilder(10000);
            sb.AppendLine("OFF");
            int xSize = faces.GetLength(0), ySize = faces.GetLength(1), zSize = faces.GetLength(2);
            FaceVoxel fv;
            for(int x = 0; x < xSize; x++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int z = 0; z < zSize; z++)
                    {
                        fv = faces[x, y, z];
                        if(fv == null)
                            continue;
                        Point3D p000 = new Point3D(x, y, z), p100 = new Point3D(x + 1, y, z), p010 = new Point3D(x, y + 1, z), p110 = new Point3D(x + 1, y + 1, z),
                            p001 = new Point3D(x, y, z + 1), p101 = new Point3D(x + 1, y, z + 1), p011 = new Point3D(x, y + 1, z + 1), p111 = new Point3D(x + 1, y + 1, z + 1);
                        switch(fv.slope)
                        {
                            case Slope.Cube:
                                if(!pts.ContainsKey(p000))
                                    pts.Add(p000, ord++);
                                if(!pts.ContainsKey(p100))
                                    pts.Add(p100, ord++);
                                if(!pts.ContainsKey(p010))
                                    pts.Add(p010, ord++);
                                if(!pts.ContainsKey(p110))
                                    pts.Add(p110, ord++);
                                if(!pts.ContainsKey(p001))
                                    pts.Add(p001, ord++);
                                if(!pts.ContainsKey(p101))
                                    pts.Add(p101, ord++);
                                if(!pts.ContainsKey(p011))
                                    pts.Add(p011, ord++);
                                if(!pts.ContainsKey(p111))
                                    pts.Add(p111, ord++);
                                fs.Add(new Face3D(fv.vox.color, p000, p010, p110, p100));
                                fs.Add(new Face3D(fv.vox.color, p000, p001, p011, p010));
                                fs.Add(new Face3D(fv.vox.color, p000, p100, p101, p001));
                                fs.Add(new Face3D(fv.vox.color, p111, p011, p001, p101));
                                fs.Add(new Face3D(fv.vox.color, p111, p101, p100, p110));
                                fs.Add(new Face3D(fv.vox.color, p111, p110, p010, p011));
                                break;
                            case Slope.BackBack:
                                if(!pts.ContainsKey(p000))
                                    pts.Add(p000, ord++);
                                if(!pts.ContainsKey(p100))
                                    pts.Add(p100, ord++);
                                if(!pts.ContainsKey(p110))
                                    pts.Add(p110, ord++);
                                if(!pts.ContainsKey(p001))
                                    pts.Add(p001, ord++);
                                if(!pts.ContainsKey(p101))
                                    pts.Add(p101, ord++);
                                if(!pts.ContainsKey(p111))
                                    pts.Add(p111, ord++);
                                fs.Add(new Face3D(fv.vox.color, p100, p000, p110));
                                fs.Add(new Face3D(fv.vox.color, p000, p001, p111, p110));
                                fs.Add(new Face3D(fv.vox.color, p000, p100, p101, p001));
                                fs.Add(new Face3D(fv.vox.color, p111, p001, p101));
                                fs.Add(new Face3D(fv.vox.color, p111, p101, p100, p110));
                                break;
                            case Slope.BrightBack:
                                if(!pts.ContainsKey(p010))
                                    pts.Add(p010, ord++);
                                if(!pts.ContainsKey(p100))
                                    pts.Add(p100, ord++);
                                if(!pts.ContainsKey(p110))
                                    pts.Add(p110, ord++);
                                if(!pts.ContainsKey(p011))
                                    pts.Add(p011, ord++);
                                if(!pts.ContainsKey(p101))
                                    pts.Add(p101, ord++);
                                if(!pts.ContainsKey(p111))
                                    pts.Add(p111, ord++);
                                fs.Add(new Face3D(fv.vox.color, p100, p010, p110));
                                fs.Add(new Face3D(fv.vox.color, p010, p011, p111, p110));
                                fs.Add(new Face3D(fv.vox.color, p010, p100, p101, p011));
                                fs.Add(new Face3D(fv.vox.color, p111, p011, p101));
                                fs.Add(new Face3D(fv.vox.color, p111, p101, p100, p110));
                                break;
                            case Slope.BrightDim:
                                if(!pts.ContainsKey(p010))
                                    pts.Add(p010, ord++);
                                if(!pts.ContainsKey(p000))
                                    pts.Add(p000, ord++);
                                if(!pts.ContainsKey(p110))
                                    pts.Add(p110, ord++);
                                if(!pts.ContainsKey(p011))
                                    pts.Add(p011, ord++);
                                if(!pts.ContainsKey(p001))
                                    pts.Add(p001, ord++);
                                if(!pts.ContainsKey(p111))
                                    pts.Add(p111, ord++);
                                fs.Add(new Face3D(fv.vox.color, p000, p010, p110));
                                fs.Add(new Face3D(fv.vox.color, p010, p011, p111, p110));
                                fs.Add(new Face3D(fv.vox.color, p000, p001, p011, p010));
                                fs.Add(new Face3D(fv.vox.color, p111, p011, p001));
                                fs.Add(new Face3D(fv.vox.color, p111, p001, p000, p110));
                                break;

                            case Slope.DimBack:
                                if(!pts.ContainsKey(p000))
                                    pts.Add(p000, ord++);
                                if(!pts.ContainsKey(p100))
                                    pts.Add(p100, ord++);
                                if(!pts.ContainsKey(p010))
                                    pts.Add(p010, ord++);
                                if(!pts.ContainsKey(p001))
                                    pts.Add(p001, ord++);
                                if(!pts.ContainsKey(p101))
                                    pts.Add(p101, ord++);
                                if(!pts.ContainsKey(p011))
                                    pts.Add(p011, ord++);
                                fs.Add(new Face3D(fv.vox.color, p000, p010, p100));
                                fs.Add(new Face3D(fv.vox.color, p000, p001, p011, p010));
                                fs.Add(new Face3D(fv.vox.color, p000, p100, p101, p001));
                                fs.Add(new Face3D(fv.vox.color, p011, p001, p101));
                                fs.Add(new Face3D(fv.vox.color, p011, p101, p100, p010));
                                break;

                            case Slope.BrightTop:
                                if(!pts.ContainsKey(p000))
                                    pts.Add(p000, ord++);
                                if(!pts.ContainsKey(p100))
                                    pts.Add(p100, ord++);
                                if(!pts.ContainsKey(p010))
                                    pts.Add(p010, ord++);
                                if(!pts.ContainsKey(p110))
                                    pts.Add(p110, ord++);
                                if(!pts.ContainsKey(p011))
                                    pts.Add(p011, ord++);
                                if(!pts.ContainsKey(p111))
                                    pts.Add(p111, ord++);
                                fs.Add(new Face3D(fv.vox.color, p000, p011, p010));
                                fs.Add(new Face3D(fv.vox.color, p010, p011, p111, p110));
                                fs.Add(new Face3D(fv.vox.color, p000, p010, p110, p100));
                                fs.Add(new Face3D(fv.vox.color, p100, p110, p111));
                                fs.Add(new Face3D(fv.vox.color, p111, p011, p000, p100));
                                break;

                            case Slope.BrightBottom:
                                if(!pts.ContainsKey(p001))
                                    pts.Add(p001, ord++);
                                if(!pts.ContainsKey(p101))
                                    pts.Add(p101, ord++);
                                if(!pts.ContainsKey(p010))
                                    pts.Add(p010, ord++);
                                if(!pts.ContainsKey(p110))
                                    pts.Add(p110, ord++);
                                if(!pts.ContainsKey(p011))
                                    pts.Add(p011, ord++);
                                if(!pts.ContainsKey(p111))
                                    pts.Add(p111, ord++);
                                fs.Add(new Face3D(fv.vox.color, p001, p011, p010));
                                fs.Add(new Face3D(fv.vox.color, p010, p011, p111, p110));
                                fs.Add(new Face3D(fv.vox.color, p001, p010, p110, p101));
                                fs.Add(new Face3D(fv.vox.color, p101, p110, p111));
                                fs.Add(new Face3D(fv.vox.color, p111, p011, p001, p101));
                                break;

                            case Slope.RearDimBottom:
                                if(!pts.ContainsKey(p001))
                                    pts.Add(p001, ord++);
                                if(!pts.ContainsKey(p101))
                                    pts.Add(p101, ord++);
                                if(!pts.ContainsKey(p000))
                                    pts.Add(p000, ord++);
                                if(!pts.ContainsKey(p100))
                                    pts.Add(p100, ord++);
                                if(!pts.ContainsKey(p011))
                                    pts.Add(p011, ord++);
                                if(!pts.ContainsKey(p111))
                                    pts.Add(p111, ord++);
                                fs.Add(new Face3D(fv.vox.color, p000, p001, p011));
                                fs.Add(new Face3D(fv.vox.color, p000, p011, p111, p100));
                                fs.Add(new Face3D(fv.vox.color, p000, p100, p101, p001));
                                fs.Add(new Face3D(fv.vox.color, p111, p101, p100));
                                fs.Add(new Face3D(fv.vox.color, p111, p011, p001, p101));
                                break;

                            case Slope.RearDimTop:
                                if(!pts.ContainsKey(p000))
                                    pts.Add(p000, ord++);
                                if(!pts.ContainsKey(p100))
                                    pts.Add(p100, ord++);
                                if(!pts.ContainsKey(p010))
                                    pts.Add(p010, ord++);
                                if(!pts.ContainsKey(p110))
                                    pts.Add(p110, ord++);
                                if(!pts.ContainsKey(p001))
                                    pts.Add(p001, ord++);
                                if(!pts.ContainsKey(p101))
                                    pts.Add(p101, ord++);
                                fs.Add(new Face3D(fv.vox.color, p000, p001, p010));
                                fs.Add(new Face3D(fv.vox.color, p010, p001, p101, p110));
                                fs.Add(new Face3D(fv.vox.color, p000, p010, p110, p100));
                                fs.Add(new Face3D(fv.vox.color, p110, p101, p100));
                                fs.Add(new Face3D(fv.vox.color, p101, p001, p000, p100));
                                break;


                            case Slope.DimTop:
                                if(!pts.ContainsKey(p000))
                                    pts.Add(p000, ord++);
                                if(!pts.ContainsKey(p100))
                                    pts.Add(p100, ord++);
                                if(!pts.ContainsKey(p010))
                                    pts.Add(p010, ord++);
                                if(!pts.ContainsKey(p110))
                                    pts.Add(p110, ord++);
                                if(!pts.ContainsKey(p001))
                                    pts.Add(p001, ord++);
                                if(!pts.ContainsKey(p011))
                                    pts.Add(p011, ord++);
                                fs.Add(new Face3D(fv.vox.color, p000, p100, p001));
                                fs.Add(new Face3D(fv.vox.color, p011, p001, p100, p110));
                                fs.Add(new Face3D(fv.vox.color, p000, p010, p110, p100));
                                fs.Add(new Face3D(fv.vox.color, p010, p011, p110));
                                fs.Add(new Face3D(fv.vox.color, p000, p001, p011, p010));
                                break;

                            case Slope.DimBottom:
                                if(!pts.ContainsKey(p000))
                                    pts.Add(p000, ord++);
                                if(!pts.ContainsKey(p101))
                                    pts.Add(p101, ord++);
                                if(!pts.ContainsKey(p010))
                                    pts.Add(p010, ord++);
                                if(!pts.ContainsKey(p111))
                                    pts.Add(p111, ord++);
                                if(!pts.ContainsKey(p001))
                                    pts.Add(p001, ord++);
                                if(!pts.ContainsKey(p011))
                                    pts.Add(p011, ord++);
                                fs.Add(new Face3D(fv.vox.color, p000, p101, p001));
                                fs.Add(new Face3D(fv.vox.color, p101, p001, p011, p111));
                                fs.Add(new Face3D(fv.vox.color, p010, p111, p101, p000));
                                fs.Add(new Face3D(fv.vox.color, p010, p011, p111));
                                fs.Add(new Face3D(fv.vox.color, p000, p001, p011, p010));
                                break;

                            case Slope.RearBrightBottom:
                                if(!pts.ContainsKey(p100))
                                    pts.Add(p100, ord++);
                                if(!pts.ContainsKey(p101))
                                    pts.Add(p101, ord++);
                                if(!pts.ContainsKey(p110))
                                    pts.Add(p110, ord++);
                                if(!pts.ContainsKey(p111))
                                    pts.Add(p111, ord++);
                                if(!pts.ContainsKey(p001))
                                    pts.Add(p001, ord++);
                                if(!pts.ContainsKey(p011))
                                    pts.Add(p011, ord++);
                                fs.Add(new Face3D(fv.vox.color, p100, p101, p001));
                                fs.Add(new Face3D(fv.vox.color, p101, p001, p011, p111));
                                fs.Add(new Face3D(fv.vox.color, p101, p111, p110, p100));
                                fs.Add(new Face3D(fv.vox.color, p111, p110, p011));
                                fs.Add(new Face3D(fv.vox.color, p100, p001, p011, p110));
                                break;

                            case Slope.RearBrightTop:
                                if(!pts.ContainsKey(p100))
                                    pts.Add(p100, ord++);
                                if(!pts.ContainsKey(p101))
                                    pts.Add(p101, ord++);
                                if(!pts.ContainsKey(p110))
                                    pts.Add(p110, ord++);
                                if(!pts.ContainsKey(p111))
                                    pts.Add(p111, ord++);
                                if(!pts.ContainsKey(p000))
                                    pts.Add(p000, ord++);
                                if(!pts.ContainsKey(p010))
                                    pts.Add(p010, ord++);
                                fs.Add(new Face3D(fv.vox.color, p100, p101, p000));
                                fs.Add(new Face3D(fv.vox.color, p010, p000, p101, p111));
                                fs.Add(new Face3D(fv.vox.color, p101, p111, p110, p100));
                                fs.Add(new Face3D(fv.vox.color, p111, p110, p010));
                                fs.Add(new Face3D(fv.vox.color, p000, p010, p110, p100));
                                break;

                            case Slope.BrightDimTop:
                                if(!pts.ContainsKey(p010))
                                    pts.Add(p010, ord++);
                                if(!pts.ContainsKey(p000))
                                    pts.Add(p000, ord++);
                                if(!pts.ContainsKey(p110))
                                    pts.Add(p110, ord++);
                                if(!pts.ContainsKey(p011))
                                    pts.Add(p011, ord++);
                                fs.Add(new Face3D(fv.vox.color, p010, p000, p110));
                                fs.Add(new Face3D(fv.vox.color, p010, p011, p110));
                                fs.Add(new Face3D(fv.vox.color, p010, p000, p011));
                                fs.Add(new Face3D(fv.vox.color, p110, p011, p000));
                                break;

                            case Slope.BrightDimBottom:
                                if(!pts.ContainsKey(p011))
                                    pts.Add(p011, ord++);
                                if(!pts.ContainsKey(p001))
                                    pts.Add(p001, ord++);
                                if(!pts.ContainsKey(p111))
                                    pts.Add(p111, ord++);
                                if(!pts.ContainsKey(p010))
                                    pts.Add(p010, ord++);
                                fs.Add(new Face3D(fv.vox.color, p011, p001, p111));
                                fs.Add(new Face3D(fv.vox.color, p011, p010, p111));
                                fs.Add(new Face3D(fv.vox.color, p011, p001, p010));
                                fs.Add(new Face3D(fv.vox.color, p010, p111, p001));
                                break;


                            case Slope.BrightTopBack:
                                if(!pts.ContainsKey(p110))
                                    pts.Add(p110, ord++);
                                if(!pts.ContainsKey(p100))
                                    pts.Add(p100, ord++);
                                if(!pts.ContainsKey(p010))
                                    pts.Add(p010, ord++);
                                if(!pts.ContainsKey(p111))
                                    pts.Add(p111, ord++);
                                fs.Add(new Face3D(fv.vox.color, p010, p100, p110));
                                fs.Add(new Face3D(fv.vox.color, p010, p110, p111));
                                fs.Add(new Face3D(fv.vox.color, p100, p110, p111));
                                fs.Add(new Face3D(fv.vox.color, p010, p100, p111));
                                break;

                            case Slope.BrightBottomBack:
                                if(!pts.ContainsKey(p111))
                                    pts.Add(p111, ord++);
                                if(!pts.ContainsKey(p101))
                                    pts.Add(p101, ord++);
                                if(!pts.ContainsKey(p011))
                                    pts.Add(p011, ord++);
                                if(!pts.ContainsKey(p110))
                                    pts.Add(p110, ord++);
                                fs.Add(new Face3D(fv.vox.color, p011, p101, p111));
                                fs.Add(new Face3D(fv.vox.color, p011, p110, p111));
                                fs.Add(new Face3D(fv.vox.color, p101, p110, p111));
                                fs.Add(new Face3D(fv.vox.color, p101, p011, p110));
                                break;

                            case Slope.BackBackTop:
                                if(!pts.ContainsKey(p100))
                                    pts.Add(p100, ord++);
                                if(!pts.ContainsKey(p110))
                                    pts.Add(p110, ord++);
                                if(!pts.ContainsKey(p000))
                                    pts.Add(p000, ord++);
                                if(!pts.ContainsKey(p101))
                                    pts.Add(p101, ord++);
                                fs.Add(new Face3D(fv.vox.color, p000, p100, p110));
                                fs.Add(new Face3D(fv.vox.color, p000, p100, p101));
                                fs.Add(new Face3D(fv.vox.color, p100, p101, p110));
                                fs.Add(new Face3D(fv.vox.color, p000, p101, p110));
                                break;

                            case Slope.BackBackBottom:
                                if(!pts.ContainsKey(p101))
                                    pts.Add(p101, ord++);
                                if(!pts.ContainsKey(p111))
                                    pts.Add(p111, ord++);
                                if(!pts.ContainsKey(p001))
                                    pts.Add(p001, ord++);
                                if(!pts.ContainsKey(p100))
                                    pts.Add(p100, ord++);
                                fs.Add(new Face3D(fv.vox.color, p001, p101, p111));
                                fs.Add(new Face3D(fv.vox.color, p001, p100, p101));
                                fs.Add(new Face3D(fv.vox.color, p100, p101, p111));
                                fs.Add(new Face3D(fv.vox.color, p100, p001, p111));
                                break;

                            case Slope.DimTopBack:
                                if(!pts.ContainsKey(p000))
                                    pts.Add(p000, ord++);
                                if(!pts.ContainsKey(p010))
                                    pts.Add(p010, ord++);
                                if(!pts.ContainsKey(p100))
                                    pts.Add(p100, ord++);
                                if(!pts.ContainsKey(p001))
                                    pts.Add(p001, ord++);
                                fs.Add(new Face3D(fv.vox.color, p000, p010, p100));
                                fs.Add(new Face3D(fv.vox.color, p000, p001, p100));
                                fs.Add(new Face3D(fv.vox.color, p000, p010, p001));
                                fs.Add(new Face3D(fv.vox.color, p100, p010, p001));
                                break;

                            case Slope.DimBottomBack:
                                if(!pts.ContainsKey(p001))
                                    pts.Add(p001, ord++);
                                if(!pts.ContainsKey(p011))
                                    pts.Add(p011, ord++);
                                if(!pts.ContainsKey(p101))
                                    pts.Add(p101, ord++);
                                if(!pts.ContainsKey(p000))
                                    pts.Add(p000, ord++);
                                fs.Add(new Face3D(fv.vox.color, p001, p011, p101));
                                fs.Add(new Face3D(fv.vox.color, p001, p000, p101));
                                fs.Add(new Face3D(fv.vox.color, p001, p011, p000));
                                fs.Add(new Face3D(fv.vox.color, p101, p000, p011));
                                break;

                            case Slope.BrightDimTopThick:

                                if(!pts.ContainsKey(p000))
                                    pts.Add(p000, ord++);
                                if(!pts.ContainsKey(p100))
                                    pts.Add(p100, ord++);
                                if(!pts.ContainsKey(p010)) // base
                                    pts.Add(p010, ord++);
                                if(!pts.ContainsKey(p110))
                                    pts.Add(p110, ord++);
                                if(!pts.ContainsKey(p001))
                                    pts.Add(p001, ord++);
                                //if(!pts.ContainsKey(p101)) // remove
                                //  pts.Add(p101, ord++);
                                if(!pts.ContainsKey(p011))
                                    pts.Add(p011, ord++);
                                if(!pts.ContainsKey(p111))
                                    pts.Add(p111, ord++);


                                fs.Add(new Face3D(fv.vox.color, p000, p010, p110, p100));
                                fs.Add(new Face3D(fv.vox.color, p000, p010, p011, p001));
                                fs.Add(new Face3D(fv.vox.color, p000, p100, p001));
                                fs.Add(new Face3D(fv.vox.color, p111, p011, p001));
                                fs.Add(new Face3D(fv.vox.color, p111, p100, p110));
                                fs.Add(new Face3D(fv.vox.color, p111, p011, p010, p110));

                                fs.Add(new Face3D(fv.vox.color, p100, p111, p001));
                                break;

                            case Slope.BrightDimBottomThick:
                                if(!pts.ContainsKey(p000))
                                    pts.Add(p000, ord++);
                                //if(!pts.ContainsKey(p100)) // remove
                                //  pts.Add(p100, ord++);
                                if(!pts.ContainsKey(p010))
                                    pts.Add(p010, ord++);
                                if(!pts.ContainsKey(p110))
                                    pts.Add(p110, ord++);
                                if(!pts.ContainsKey(p001))
                                    pts.Add(p001, ord++);
                                if(!pts.ContainsKey(p101))
                                    pts.Add(p101, ord++);
                                if(!pts.ContainsKey(p011)) // base
                                    pts.Add(p011, ord++);
                                if(!pts.ContainsKey(p111))
                                    pts.Add(p111, ord++);


                                fs.Add(new Face3D(fv.vox.color, p000, p010, p110));
                                fs.Add(new Face3D(fv.vox.color, p000, p010, p011, p001));
                                fs.Add(new Face3D(fv.vox.color, p000, p101, p001));
                                fs.Add(new Face3D(fv.vox.color, p111, p011, p001, p101));
                                fs.Add(new Face3D(fv.vox.color, p111, p101, p110));
                                fs.Add(new Face3D(fv.vox.color, p111, p011, p010, p110));

                                fs.Add(new Face3D(fv.vox.color, p000, p110, p101));
                                break;


                            case Slope.BrightTopBackThick:

                                if(!pts.ContainsKey(p000))
                                    pts.Add(p000, ord++);
                                if(!pts.ContainsKey(p100))
                                    pts.Add(p100, ord++);
                                if(!pts.ContainsKey(p010))
                                    pts.Add(p010, ord++);
                                if(!pts.ContainsKey(p110)) //base
                                    pts.Add(p110, ord++);
                                //if(!pts.ContainsKey(p001)) //remove
                                //  pts.Add(p001, ord++);
                                if(!pts.ContainsKey(p101))
                                    pts.Add(p101, ord++);
                                if(!pts.ContainsKey(p011))
                                    pts.Add(p011, ord++);
                                if(!pts.ContainsKey(p111))
                                    pts.Add(p111, ord++);


                                fs.Add(new Face3D(fv.vox.color, p000, p010, p110, p100));
                                fs.Add(new Face3D(fv.vox.color, p000, p011, p010));
                                fs.Add(new Face3D(fv.vox.color, p000, p100, p101));
                                fs.Add(new Face3D(fv.vox.color, p111, p011, p101));
                                fs.Add(new Face3D(fv.vox.color, p111, p101, p100, p110));
                                fs.Add(new Face3D(fv.vox.color, p111, p011, p010, p110));

                                fs.Add(new Face3D(fv.vox.color, p000, p101, p011));
                                break;

                            case Slope.BrightBottomBackThick:


                                //if(!pts.ContainsKey(p000)) //remove
                                //  pts.Add(p000, ord++);
                                if(!pts.ContainsKey(p100))
                                    pts.Add(p100, ord++);
                                if(!pts.ContainsKey(p010))
                                    pts.Add(p010, ord++);
                                if(!pts.ContainsKey(p110))
                                    pts.Add(p110, ord++);
                                if(!pts.ContainsKey(p001))
                                    pts.Add(p001, ord++);
                                if(!pts.ContainsKey(p101))
                                    pts.Add(p101, ord++);
                                if(!pts.ContainsKey(p011))
                                    pts.Add(p011, ord++);
                                if(!pts.ContainsKey(p111)) //base
                                    pts.Add(p111, ord++);


                                fs.Add(new Face3D(fv.vox.color, p010, p110, p100));
                                fs.Add(new Face3D(fv.vox.color, p011, p010, p001));
                                fs.Add(new Face3D(fv.vox.color, p001, p100, p101));
                                fs.Add(new Face3D(fv.vox.color, p111, p011, p001, p101));
                                fs.Add(new Face3D(fv.vox.color, p111, p101, p100, p110));
                                fs.Add(new Face3D(fv.vox.color, p111, p011, p010, p110));

                                fs.Add(new Face3D(fv.vox.color, p001, p010, p100));
                                break;

                            case Slope.BackBackTopThick:


                                if(!pts.ContainsKey(p000))
                                    pts.Add(p000, ord++);
                                if(!pts.ContainsKey(p100)) //base
                                    pts.Add(p100, ord++);
                                if(!pts.ContainsKey(p010))
                                    pts.Add(p010, ord++);
                                if(!pts.ContainsKey(p110))
                                    pts.Add(p110, ord++);
                                if(!pts.ContainsKey(p001))
                                    pts.Add(p001, ord++);
                                if(!pts.ContainsKey(p101))
                                    pts.Add(p101, ord++);
                                //if(!pts.ContainsKey(p011)) //remove
                                //  pts.Add(p011, ord++);
                                if(!pts.ContainsKey(p111))
                                    pts.Add(p111, ord++);


                                fs.Add(new Face3D(fv.vox.color, p000, p010, p110, p100));
                                fs.Add(new Face3D(fv.vox.color, p000, p001, p010));
                                fs.Add(new Face3D(fv.vox.color, p000, p100, p101, p001));
                                fs.Add(new Face3D(fv.vox.color, p111, p001, p101));
                                fs.Add(new Face3D(fv.vox.color, p111, p101, p100, p110));
                                fs.Add(new Face3D(fv.vox.color, p111, p110, p010));

                                fs.Add(new Face3D(fv.vox.color, p010, p001, p111));
                                break;

                            case Slope.BackBackBottomThick:


                                if(!pts.ContainsKey(p000))
                                    pts.Add(p000, ord++);
                                if(!pts.ContainsKey(p100))
                                    pts.Add(p100, ord++);
                                //if(!pts.ContainsKey(p010)) //remove
                                //  pts.Add(p010, ord++);
                                if(!pts.ContainsKey(p110))
                                    pts.Add(p110, ord++);
                                if(!pts.ContainsKey(p001))
                                    pts.Add(p001, ord++);
                                if(!pts.ContainsKey(p101)) //base
                                    pts.Add(p101, ord++);
                                if(!pts.ContainsKey(p011))
                                    pts.Add(p011, ord++);
                                if(!pts.ContainsKey(p111))
                                    pts.Add(p111, ord++);


                                fs.Add(new Face3D(fv.vox.color, p000, p110, p100));
                                fs.Add(new Face3D(fv.vox.color, p000, p001, p011));
                                fs.Add(new Face3D(fv.vox.color, p000, p100, p101, p001));
                                fs.Add(new Face3D(fv.vox.color, p111, p011, p001, p101));
                                fs.Add(new Face3D(fv.vox.color, p111, p101, p100, p110));
                                fs.Add(new Face3D(fv.vox.color, p111, p110, p011));

                                fs.Add(new Face3D(fv.vox.color, p000, p011, p110));
                                break;

                            case Slope.DimTopBackThick:


                                if(!pts.ContainsKey(p000)) //base
                                    pts.Add(p000, ord++);
                                if(!pts.ContainsKey(p100))
                                    pts.Add(p100, ord++);
                                if(!pts.ContainsKey(p010))
                                    pts.Add(p010, ord++);
                                if(!pts.ContainsKey(p110))
                                    pts.Add(p110, ord++);
                                if(!pts.ContainsKey(p001))
                                    pts.Add(p001, ord++);
                                if(!pts.ContainsKey(p101))
                                    pts.Add(p101, ord++);
                                if(!pts.ContainsKey(p011))
                                    pts.Add(p011, ord++);
                                //if(!pts.ContainsKey(p111)) //remove
                                //  pts.Add(p111, ord++);


                                fs.Add(new Face3D(fv.vox.color, p000, p010, p110, p100));
                                fs.Add(new Face3D(fv.vox.color, p000, p010, p011, p001));
                                fs.Add(new Face3D(fv.vox.color, p000, p100, p101, p001));
                                fs.Add(new Face3D(fv.vox.color, p011, p001, p101));
                                fs.Add(new Face3D(fv.vox.color, p101, p100, p110));
                                fs.Add(new Face3D(fv.vox.color, p010, p011, p110));

                                fs.Add(new Face3D(fv.vox.color, p011, p101, p110));
                                break;

                            case Slope.DimBottomBackThick:


                                if(!pts.ContainsKey(p000))
                                    pts.Add(p000, ord++);
                                if(!pts.ContainsKey(p100))
                                    pts.Add(p100, ord++);
                                if(!pts.ContainsKey(p010))
                                    pts.Add(p010, ord++);
                                //if(!pts.ContainsKey(p110)) //remove
                                //  pts.Add(p110, ord++);
                                if(!pts.ContainsKey(p001)) //base
                                    pts.Add(p001, ord++);
                                if(!pts.ContainsKey(p101))
                                    pts.Add(p101, ord++);
                                if(!pts.ContainsKey(p011))
                                    pts.Add(p011, ord++);
                                if(!pts.ContainsKey(p111))
                                    pts.Add(p111, ord++);

                                fs.Add(new Face3D(fv.vox.color, p000, p010, p100));
                                fs.Add(new Face3D(fv.vox.color, p000, p010, p011, p001));
                                fs.Add(new Face3D(fv.vox.color, p000, p100, p101, p001));
                                fs.Add(new Face3D(fv.vox.color, p111, p011, p001, p101));
                                fs.Add(new Face3D(fv.vox.color, p111, p101, p100));
                                fs.Add(new Face3D(fv.vox.color, p010, p011, p111));

                                fs.Add(new Face3D(fv.vox.color, p100, p010, p111));
                                break;
                        }

                    }
                }
            }
            sb.AppendLine(ord + " " + fs.Count + " 0");
            foreach(var kv in pts)
            {
                sb.AppendLine(kv.Key.ToString());
            }
            foreach(Face3D f in fs)
            {
                sb.Append(f.Points.Length);
                sb.Append(" ");
                for(int i = 0; i < f.Points.Length; i++)
                {
                    sb.Append(pts[f.Points[i]]);
                    sb.Append(" ");
                }
                int c = (253 - f.Color) / 4;
                sb.AppendLine(((VoxelLogic.wrendered[palette][c][18] + 1) / 256.0) + " " + ((VoxelLogic.wrendered[palette][c][17] + 1) / 256.0) +
                    " " + ((VoxelLogic.wrendered[palette][c][16] + 1) / 256.0) + " 1.0");
            }
            return sb.ToString();
        }
        /*
if(zdown || faces[x, y, z - 1].slope != fv.slope)
    fs.Add(new OrientedFace3D(center, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0));
if(xdown || faces[x - 1, y, z].slope != fv.slope)
    fs.Add(new OrientedFace3D(center, 0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0));
if(ydown || faces[x, y - 1, z].slope != fv.slope)
    fs.Add(new OrientedFace3D(center, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1));
if(zup || faces[x, y, z + 1].slope != fv.slope)
    fs.Add(new OrientedFace3D(center, 1, 1, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1));
if(xup || faces[x + 1, y, z].slope != fv.slope)
    fs.Add(new OrientedFace3D(center, 1, 1, 1, 1, 0, 1, 1, 0, 0, 1, 1, 0));
if(yup || faces[x, y + 1, z].slope != fv.slope)
    fs.Add(new OrientedFace3D(center, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1, 1));
break;*/
        public static void ProcessFV(FaceVoxel fv, FaceVoxel[,,] faces, OrientedFace3D[] fs)
        {
            MagicaVoxelData mvd = fv.vox;
            int x = mvd.x, y = mvd.y, z = mvd.z, color = mvd.color;
            Vertex3D center = new Vertex3D(color, x + 0.5f, y + 0.5f, z + 0.5f);
            int xSize = faces.GetLength(0), ySize = faces.GetLength(1), zSize = faces.GetLength(2);
            int i = x * ySize * zSize * 7 + y * zSize * 7 + z * 7;
            //bool xdown = (x == 0 || faces[x - 1, y, z] == null), xup = (x == xSize - 1 || faces[x + 1, y, z] == null),
            //     ydown = (y == 0 || faces[x, y - 1, z] == null), yup = (y == ySize - 1 || faces[x, y + 1, z] == null),
            //     zdown = (z == 0 || faces[x, y, z - 1] == null), zup = (z == zSize - 1 || faces[x, y, z + 1] == null);
            switch(fv.slope)
            {
                case Slope.Cube:
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 1, 0, 1, 1, 0, 0, 1, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1, 1));
                    break;
                case Slope.BackBack:
                    fs[i++] = (new OrientedFace3D(center, 1, 0, 0, 0, 0, 0, 1, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 0, 0, 1, 1, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 1, 0, 1, 1, 0, 0, 1, 1, 0));
                    break;
                case Slope.BrightBack:
                    fs[i++] = (new OrientedFace3D(center, 1, 0, 0, 0, 1, 0, 1, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 1, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 0, 1, 1, 1, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 1, 0, 1, 1, 0, 0, 1, 1, 0));
                    break;
                case Slope.BrightDim:
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 1, 0, 1, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 0, 1, 1, 0, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 0, 0, 1, 0, 0, 0, 1, 1, 0));
                    break;
                case Slope.DimBack:
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 1, 0, 1, 0, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 1, 0, 0, 1, 1, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 1, 1, 0, 1, 1, 0, 0, 0, 1, 0));
                    break;
                case Slope.BrightTop:
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 1, 1, 0, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0));
                    fs[i++] = (new OrientedFace3D(center, 1, 0, 0, 1, 1, 0, 1, 1, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 0, 1, 1, 0, 0, 0, 1, 0, 0));
                    break;
                case Slope.BrightBottom:
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 1, 0, 1, 1, 0, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 1, 0, 1, 0, 1, 1, 0, 1, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 0, 1, 1, 1, 0, 1, 1, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1));
                    break;
                case Slope.RearDimBottom:
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 0, 1, 0, 1, 1));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 1, 0, 1, 1, 0, 0));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1));
                    break;
                case Slope.RearDimTop:
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 0, 1, 0, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 0, 0, 0, 1, 1, 0, 1, 1, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 0, 1, 0, 1, 1, 0, 0));
                    fs[i++] = (new OrientedFace3D(center, 1, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0, 0));
                    break;
                case Slope.DimTop:
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 1, 0, 0, 0, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 0, 0, 1, 1, 1, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0));
                    break;
                case Slope.DimBottom:
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 1, 0, 1, 0, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 0, 1, 0, 0, 1, 0, 1, 1, 1, 1, 1));
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 0, 1, 1, 1, 1, 0, 1, 0, 0, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 0, 0, 1, 1, 1, 1, 1));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0));
                    break;
                case Slope.RearBrightBottom:
                    fs[i++] = (new OrientedFace3D(center, 1, 0, 0, 1, 0, 1, 0, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 0, 1, 0, 0, 1, 0, 1, 1, 1, 1, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0, 0));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 1, 1, 0, 0, 1, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 0));
                    break;
                case Slope.RearBrightTop:
                    fs[i++] = (new OrientedFace3D(center, 1, 0, 0, 1, 0, 1, 0, 0, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0, 0));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 1, 1, 0, 0, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0));
                    break;
                case Slope.BrightDimTop:
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 0, 0, 0, 0, 1, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 0, 0, 1, 1, 1, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 0, 0, 0, 0, 0, 1, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 0, 0, 1, 1, 0, 0, 0));
                    break;
                case Slope.BrightDimBottom:
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 1, 0, 0, 1, 1, 1, 1));
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 1, 0, 1, 0, 1, 1, 1));
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 1, 0, 0, 1, 0, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 0, 1, 1, 1, 0, 0, 1));
                    break;
                case Slope.BrightTopBack:
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 0, 1, 0, 0, 1, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 0, 1, 1, 0, 1, 1, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 0, 0, 1, 1, 0, 1, 1, 1));
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 0, 1, 0, 0, 1, 1, 1));
                    break;
                case Slope.BrightBottomBack:
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 1, 1, 0, 1, 1, 1, 1));
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 1, 1, 1, 0, 1, 1, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 0, 1, 1, 1, 0, 1, 1, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 0, 1, 0, 1, 1, 1, 1, 0));
                    break;
                case Slope.BackBackTop:
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 1, 0, 0, 1, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 1, 0, 0, 1, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 0, 0, 1, 0, 1, 1, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 1, 0, 1, 1, 1, 0));
                    break;
                case Slope.BackBackBottom:
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 1, 1, 0, 1, 1, 1, 1));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 1, 1, 0, 0, 1, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 0, 0, 1, 0, 1, 1, 1, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 0, 0, 0, 0, 1, 1, 1, 1));
                    break;
                case Slope.DimTopBack:
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 1, 0, 1, 0, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 0, 1, 1, 0, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 1, 0, 0, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 0, 0, 0, 1, 0, 0, 0, 1));
                    break;
                case Slope.DimBottomBack:
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 1, 0, 1, 1, 1, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 1, 0, 0, 0, 1, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 1, 0, 1, 1, 0, 0, 0));
                    fs[i++] = (new OrientedFace3D(center, 1, 0, 1, 0, 0, 0, 0, 1, 1));
                    break;
                case Slope.BrightDimTopThick:
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 1, 0, 0, 0, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 0, 1, 1, 0, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 1, 0, 0, 1, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 0, 1, 1, 0, 1, 0, 1, 1, 0));

                    fs[i++] = (new OrientedFace3D(center, 1, 0, 0, 1, 1, 1, 0, 0, 1));
                    break;
                case Slope.BrightDimBottomThick:
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 1, 0, 1, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 1, 0, 1, 0, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 1, 0, 1, 1, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 0, 1, 1, 0, 1, 0, 1, 1, 0));

                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 1, 1, 0, 1, 0, 1));
                    break;
                case Slope.BrightTopBackThick:
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 1, 1, 0, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 1, 0, 0, 1, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 0, 1, 1, 1, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 1, 0, 1, 1, 0, 0, 1, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 0, 1, 1, 0, 1, 0, 1, 1, 0));

                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 1, 0, 1, 0, 1, 1));
                    break;
                case Slope.BrightBottomBackThick:
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 0, 1, 1, 0, 1, 0, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 1, 0, 1, 0, 0, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 1, 1, 0, 0, 1, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 1, 0, 1, 1, 0, 0, 1, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 0, 1, 1, 0, 1, 0, 1, 1, 0));

                    fs[i++] = (new OrientedFace3D(center, 0, 0, 1, 0, 1, 0, 1, 0, 0));
                    break;
                case Slope.BackBackTopThick:
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 0, 1, 0, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 0, 0, 1, 1, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 1, 0, 1, 1, 0, 0, 1, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 1, 1, 0, 0, 1, 0));

                    fs[i++] = (new OrientedFace3D(center, 0, 1, 0, 0, 0, 1, 1, 1, 1));
                    break;
                case Slope.BackBackBottomThick:
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 1, 1, 0, 1, 0, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 0, 1, 0, 1, 1));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 1, 0, 1, 1, 0, 0, 1, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 1, 1, 0, 0, 1, 1));

                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 1, 1, 1, 1, 0));
                    break;
                case Slope.DimTopBackThick:
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 1, 0, 0, 1, 1, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 0, 1, 1, 0, 0, 1, 1, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 0, 0, 1, 1, 1, 1, 0));

                    fs[i++] = (new OrientedFace3D(center, 0, 1, 1, 1, 0, 1, 1, 1, 0));
                    break;
                case Slope.DimBottomBackThick:
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 1, 0, 1, 0, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1));
                    fs[i++] = (new OrientedFace3D(center, 1, 1, 1, 1, 0, 1, 1, 0, 0));
                    fs[i++] = (new OrientedFace3D(center, 0, 1, 0, 0, 1, 1, 1, 1, 1));

                    fs[i++] = (new OrientedFace3D(center, 1, 0, 0, 0, 1, 0, 1, 1, 1));
                    break;
            }
        }

        public static Vertex3D[] MatchOrientations(Vertex3D[] p1, Vertex3D[] p2)
        {
            List<Vertex3D> vs = new List<Vertex3D>(4);
            for(int i = 0; i < p1.Length; i++)
            {
                for(int j = 0; j < p2.Length; j++)
                {
                    if(Math.Abs(p1[i].X - p2[j].X) < 0.1 && Math.Abs(p1[i].Y - p2[j].Y) < 0.1 && Math.Abs(p1[i].Z - p2[j].Z) < 0.1)
                    {
                        vs.Add(p1[i]);
                        vs.Add(p2[j]);
                    }
                }
            }
            //if(vs.Count >= 1 && vs.Count < 3)
            //    Console.Write("!");
            if(vs.Count >= 3)
                return vs.ToArray();
            else
                return null;
        }
        public static Vertex3D[] Nearby(Vertex3D p, params OrientedFace3D[] fs)
        {
            List<Vertex3D> vs = new List<Vertex3D>(4);
            for(int f = 0; f < fs.Length; f++)
            {
                if(fs[f] == null)
                    continue;
                for(int j = 0; j < fs[f].Points.Length; j++)
                {
                    if(Math.Abs(p.X - fs[f].Points[j].X) < 0.1 && Math.Abs(p.Y - fs[f].Points[j].Y) < 0.1 && Math.Abs(p.Z - fs[f].Points[j].Z) < 0.1)
                    {
                        vs.Add(fs[f].Points[j]);
                    }
                }
            }

            if(vs.Count >= 3)
                return vs.ToArray();
            else
                return null;
        }
        public static OrientedFace3D[] CleanupFaces(FaceVoxel[,,] faces, OrientedFace3D[] fs)
        {
            int xSize = faces.GetLength(0), ySize = faces.GetLength(1), zSize = faces.GetLength(2);
            OrientedFace3D[] oriented = new OrientedFace3D[xSize * ySize * zSize * 8 * 7];
            for(int x = 0; x < xSize; x++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int z = 0; z < zSize; z++)
                    {
                        for(int i = 0; i < 7; i++)
                        {
                            oriented[(x * ySize * zSize * 8 + y * zSize * 4 + z * 2) * 7 + i] = fs[x * ySize * zSize * 7 + y * zSize * 7 + z * 7 + i];
                        }
                    }
                }
            }
            FaceVoxel fv, nfv;
            int pos = 0, npos = 0;
            OrientedFace3D ori, nori;
            Vertex3D[] matching;
            Vertex3D center;
            for(int x = 1; x < xSize; x++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int z = 0; z < zSize; z++)
                    {
                        fv = faces[x, y, z];
                        if(fv == null)
                            continue;
                        nfv = faces[x - 1, y, z];
                        if(nfv == null)
                            continue;
                        pos = x * ySize * zSize * 7 + y * zSize * 7 + z * 7;
                        npos = (x - 1) * ySize * zSize * 7 + y * zSize * 7 + z * 7;
                        for(int a = 0; a < 7; a++)
                        {
                            if(fs[pos + a] == null)
                                break;
                            ori = fs[pos + a];
                            for(int b = 0; b < 7; b++)
                            {
                                if(fs[npos + b] == null)
                                    break;
                                nori = fs[npos + b];
                                if(ori.Points.Length == 4 && nori.Points.Length == 4 && (ori.Orient & nori.Orient) == 0 && (ori.Orient & 585) == 0 && (nori.Orient & 585) == 585)
                                {
                                    oriented[pos * 2 + a] = null;
                                    oriented[npos * 2 + b] = null;
                                    continue;
                                }
                                if(ori.Points.Length == 3 && nori.Points.Length == 3 && (ori.Orient & nori.Orient) == 0 && (ori.Orient & 73) == 0 && (nori.Orient & 73) == 73)
                                {
                                    oriented[pos * 2 + a] = null;
                                    oriented[npos * 2 + b] = null;
                                    continue;
                                }
                                if((matching = MatchOrientations(ori.Points, nori.Points)) != null)
                                {
                                    oriented[((x * 2 - 1) * ySize * zSize * 4 + y * zSize * 4 + z * 2) * 7 + b] = new OrientedFace3D(matching);
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
            for(int x = 0; x < xSize; x++)
            {
                for(int y = 1; y < ySize; y++)
                {
                    for(int z = 0; z < zSize; z++)
                    {
                        fv = faces[x, y, z];
                        if(fv == null)
                            continue;
                        nfv = faces[x, y - 1, z];
                        if(nfv == null)
                            continue;
                        pos = x * ySize * zSize * 7 + y * zSize * 7 + z * 7;
                        npos = x * ySize * zSize * 7 + (y - 1) * zSize * 7 + z * 7;
                        for(int a = 0; a < 7; a++)
                        {
                            if(fs[pos + a] == null)
                                break;
                            ori = fs[pos + a];
                            for(int b = 0; b < 7; b++)
                            {
                                if(fs[npos + b] == null)
                                    break;
                                nori = fs[npos + b];
                                if(ori.Points.Length == 4 && nori.Points.Length == 4 && (ori.Orient & nori.Orient) == 0 && (ori.Orient & 1170) == 0 && (nori.Orient & 1170) == 1170)
                                {
                                    oriented[pos * 2 + a] = null;
                                    oriented[npos * 2 + b] = null;
                                    continue;
                                }
                                if(ori.Points.Length == 3 && nori.Points.Length == 3 && (ori.Orient & nori.Orient) == 0 && (ori.Orient & 146) == 0 && (nori.Orient & 146) == 146)
                                {
                                    oriented[pos * 2 + a] = null;
                                    oriented[npos * 2 + b] = null;
                                    continue;
                                }
                                if((matching = MatchOrientations(ori.Points, nori.Points)) != null)
                                {
                                    oriented[(x * ySize * zSize * 8 + (y * 2 - 1) * zSize * 2 + z * 2) * 7 + b] = new OrientedFace3D(matching);
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
            for(int x = 0; x < xSize; x++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int z = 1; z < zSize; z++)
                    {
                        fv = faces[x, y, z];
                        if(fv == null)
                            continue;
                        nfv = faces[x, y, z - 1];
                        if(nfv == null)
                            continue;
                        pos = x * ySize * zSize * 7 + y * zSize * 7 + z * 7;
                        npos = x * ySize * zSize * 7 + y * zSize * 7 + (z - 1) * 7;
                        for(int a = 0; a < 7; a++)
                        {
                            if(fs[pos + a] == null)
                                break;
                            ori = fs[pos + a];
                            for(int b = 0; b < 7; b++)
                            {
                                if(fs[npos + b] == null)
                                    break;
                                nori = fs[npos + b];
                                if(ori.Points.Length == 4 && nori.Points.Length == 4 && (ori.Orient & nori.Orient) == 0 && (ori.Orient & 2340) == 0 && (nori.Orient & 2340) == 2340)
                                {
                                    oriented[pos * 2 + a] = null;
                                    oriented[npos * 2 + b] = null;
                                    continue;
                                }
                                if(ori.Points.Length == 3 && nori.Points.Length == 3 && (ori.Orient & nori.Orient) == 0 && (ori.Orient & 292) == 0 && (nori.Orient & 292) == 292)
                                {
                                    oriented[pos * 2 + a] = null;
                                    oriented[npos * 2 + b] = null;
                                    continue;
                                }
                                if((matching = MatchOrientations(ori.Points, nori.Points)) != null)
                                {
                                    oriented[(x * ySize * zSize * 8 + y * zSize * 4 + (z * 2 - 1)) * 7 + b] = new OrientedFace3D(matching);
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
            //int[] offsets = new int[] { 7, zSize * 12, ySize * zSize * 24, zSize * -12, ySize * zSize * -24, -7 };
            int[] offsets = new int[] { 7, zSize * 7, ySize * zSize * 7, zSize * -7, ySize * zSize * -7, -7 };
            List<OrientedFace3D> oris = new List<OrientedFace3D>(7);
            for(int x = 1; x < xSize * 2; x += 2)
            {
                for(int y = 1; y < ySize * 2; y += 2)
                {
                    for(int z = 1; z < zSize * 2; z += 2)
                    {
                        pos = (x * ySize * zSize + y * zSize + z) * 7;
                        for(int o = 0; o < 6; o++)
                        {
                            npos = pos + offsets[o];
                            for(int a = 0; a < 7; a++)
                            {
                                if(oriented[npos + a] == null)
                                    break;
                                oris.Add(oriented[npos + a]);
                            }
                        }
                        center = new Vertex3D(1, x / 2 + 1, y / 2 + 1, z / 2 + 1);
                        if((matching = Nearby(center, oris.ToArray())) != null)
                        {
                            oriented[(x * ySize * zSize + y * zSize + z) * 7] = new OrientedFace3D(matching);
                            continue;
                        }
                    }
                }
            }


            return oriented;
        }

        public static string WriteVertexColorOFF(FaceVoxel[,,] faces, int palette)
        {
            OrderedDictionary<Vertex3D, int> pts = new OrderedDictionary<Vertex3D, int>();
            int ord = 0;
            StringBuilder sb = new StringBuilder(100000);
            sb.AppendLine("OFF");
            int xSize = faces.GetLength(0), ySize = faces.GetLength(1), zSize = faces.GetLength(2);
            OrientedFace3D[] fs = new OrientedFace3D[xSize * ySize * zSize * 7];


            FaceVoxel fv;
            
            for(int x = 0; x < xSize; x++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int z = 0; z < zSize; z++)
                    {
                        fv = faces[x, y, z];
                        if(fv == null)
                            continue;
                        ProcessFV(fv, faces, fs);
                    }
                }
            }
            fs = CleanupFaces(faces, fs);
            for(int i = 0; i < fs.Length; i++)
            {
                if(fs[i] == null)
                    continue;
                for(int v = 0; v < fs[i].Points.Length; v++)
                {
                    if(!pts.ContainsKey(fs[i].Points[v]))
                        pts.Add(fs[i].Points[v], ord++);
                }
            }

            sb.AppendLine(ord + " " + fs.Count(o => o != null) + " 0");
            foreach(var kv in pts)
            {
                sb.AppendLine(kv.Key.Show(palette));
            }
            foreach(OrientedFace3D f in fs)
            {
                if(f == null)
                    continue;
                sb.Append(f.Points.Length);
                sb.Append(" ");
                for(int i = 0; i < f.Points.Length - 1; i++)
                {
                    sb.Append(pts[f.Points[i]]);
                    sb.Append(" ");
                }
                sb.Append(pts[f.Points[f.Points.Length - 1]]);
                sb.AppendLine();
            }
            
            return sb.ToString();
        }

    }
}
