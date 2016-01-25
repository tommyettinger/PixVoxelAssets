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
        BackBack = 0x10000
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
    }
    public delegate FaceVoxel[,,] FaceModifier(FaceVoxel[,,] faces);

    class FaceLogic
    {

        public static byte[,,] VoxListToArrayPure(List<MagicaVoxelData> voxelData, int xSize, int ySize, int zSize)
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
        public static byte[,,] VoxListToArray(List<MagicaVoxelData> voxelData, int xSize, int ySize, int zSize, byte shadowColor)
        {
            byte[,,] data = new byte[xSize, ySize, zSize];
            foreach(MagicaVoxelData mvd in voxelData)
            {
                if(mvd.x >= xSize || mvd.y >= ySize || mvd.z >= zSize)
                    continue;
                if(!(mvd.color == VoxelLogic.clear || (VoxelLogic.VisualMode == "CU" && (mvd.color == CURedux.emitter0 || mvd.color == CURedux.trail0
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
        public static FaceVoxel[,,] GetFaces(byte[,,] voxelData)
        {
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);
            FaceVoxel[,,] data = new FaceVoxel[xSize, ySize, zSize];

            for(int z = 0; z < zSize; z++)
            {
                for(int x = 0; x < xSize; x++)
                {
                    for(int y = 0; y < ySize; y++)
                    {
                        byte mvd = voxelData[x, y, z];
                        if(mvd > 0 && (VoxelLogic.VisualMode != "CU" || mvd > CURedux.emitter0))
                        {
                            data[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)z, color = mvd }, Slope.Cube);
                        }
                    }
                }
            }

            data = AddAll(data);

            return data;
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

                            if(zdown)
                            {

                                if(!xdown && yup && !xup && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x, y, z - 1].vox.color }, Slope.BrightTop);
                                }
                                else if(xdown && !yup && !xup && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x, y, z - 1].vox.color }, Slope.DimTop);
                                }
                                else if(xdown && yup && !xup && !ydown)
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
                            }
                            else if(zup)
                            {

                                if(!xdown && yup && !xup && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x, y, z + 1].vox.color }, Slope.BrightBottom);
                                }
                                else if(xdown && !yup && !xup && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x, y, z + 1].vox.color }, Slope.DimBottom);
                                }
                                else if(xdown && yup && !xup && !ydown)
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
                            }
                            else
                            {
                                if(xdown && yup && !xup && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x - 1, y, z].vox.color }, Slope.BrightDim);
                                }
                                else if(!xdown && !yup && xup && ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x + 1, y, z].vox.color }, Slope.BackBack);
                                }
                                else if(xup && yup && !xdown && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x + 1, y, z].vox.color }, Slope.BrightBack);
                                }
                                else if(!xup && !yup && xdown && ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x - 1, y, z].vox.color }, Slope.DimBack);
                                }
                            }
                            //                            if(faces2[x, y, z] != null)
                            //                                Console.WriteLine("x,y,z:" + x + "," + y + "," + z + ": " + faces2[x,y,z].slope.ToString());
                        }
                    }
                }
            }
            return faces2;
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
            switch(VoxelLogic.VisualMode)
            {
                case "CU":
                    return VoxelLogic.ImportantColorCU(fv.vox.color) > 16;
                case "Mecha":
                    return VoxelLogic.ImportantColorMecha(fv.vox.color) > 16;
                case "None":
                    return false;
                default:
                    return VoxelLogic.ImportantColorW(fv.vox.color) > 16;
            }
        }

        public static bool ImportantNeighborhood(FaceVoxel[,,] faces, int x, int y, int z)
        {
            if(faces == null)
                return false;
            FaceVoxel a = faces[x + 1, y, z], b = faces[x - 1, y, z], c = faces[x, y + 1, z], d = faces[x, y - 1, z], e = faces[x, y, z + 1], f = faces[x, y, z - 1];
            byte ac = 255, bc = 255, cc = 255, dc = 255, ec = 255, fc = 255;
            int tgt =  512;
            if(a != null) { ac = a.vox.color; if(tgt == 512) tgt = ac; else if(tgt != ac) tgt = 1024; }
            if(b != null) { bc = b.vox.color; if(tgt == 512) tgt = bc; else if(tgt != bc) tgt = 1024; }
            if(c != null) { cc = c.vox.color; if(tgt == 512) tgt = cc; else if(tgt != cc) tgt = 1024; }
            if(d != null) { dc = d.vox.color; if(tgt == 512) tgt = dc; else if(tgt != dc) tgt = 1024; }
            if(e != null) { ec = e.vox.color; if(tgt == 512) tgt = ec; else if(tgt != ec) tgt = 1024; }
            if(f != null) { fc = f.vox.color; if(tgt == 512) tgt = fc; else if(tgt != fc) tgt = 1024; }

            if((ac & bc & cc & dc & ec & fc) == tgt)
            {
                //if(tgt == (253 - 9 * 4) && faces[x,y,z] == null)
                //    Console.WriteLine("Homogeneous area: " + ((253 - tgt) / 4));
                return false;
            }
            return (ImportantVisual(faces[x + 1, y, z]) || ImportantVisual(faces[x - 1, y, z]) ||
                    ImportantVisual(faces[x, y + 1, z]) || ImportantVisual(faces[x, y - 1, z]) ||
                    ImportantVisual(faces[x, y, z + 1]) || ImportantVisual(faces[x, y, z - 1]));
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
                                    //result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightDimTop);
                                    //result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDim);
                                    //result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightDimTop);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightDim);
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
                                case Slope.BrightDimBottom:
                                    //result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightDimTop);
                                    //result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightDimBottom);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightDimTop);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDimBottom);
                                    //result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDimBottom);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
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
                                    //result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightBack);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BrightTopBack);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightTopBack);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    //result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDim);
                                    //result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightBack);
                                    //result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightBack);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightTopBack);
                                    break;
                                case Slope.DimTopBack:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.DimTopBack);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.DimTopBack);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.DimTopBack);
                                    //result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.DimBack);
                                    //result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.DimBack);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.BrightBottomBack:
                                    //result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightBack);
                                    //result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BrightBack);
                                    //result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightBack);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightBottomBack);
                                    //result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDim);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightBottomBack);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightBottomBack);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.DimBottomBack:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightBottomBack);
                                    //result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.DimBack);
                                    //result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.DimBack);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightBottomBack);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightBottomBack);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
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

                                    /*
        BrightTopBack = 0x1000,
        DimTopBack = 0x2000,
        BrightBottomBack = 0x4000,
        DimBottomBack = 0x8000,
        BackBack = 0x10000
                                */
                            }
                        }
                    }
                }
            }
            return result;
        }               
    }
}
