using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsPV
{
    [Flags]
    public enum Slope
    {
        Top = 0x1,
        Bright = 0x2,
        Dim = 0x4,
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
    }
    public delegate FaceVoxel[,,] FaceModifier(FaceVoxel[,,] faces);

    class FaceLogic
    {

        public static byte[,,] VoxListToArray(List<MagicaVoxelData> voxelData, int xSize, int ySize, int zSize, byte shadowColor)
        {
            byte[,,] data = new byte[xSize, ySize, zSize];
            foreach(MagicaVoxelData mvd in voxelData)
            {
                if(data[mvd.x, mvd.y, mvd.z] == 0 || data[mvd.x, mvd.y, mvd.z] == shadowColor)
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
                        if(voxelData[x, y, z] > 0)
                            vlist.Add(new MagicaVoxelData { x = x, y = y, z = z, color = voxelData[x, y, z] });
                    }
                }
            }
            return vlist;
        }

        public static FaceVoxel[,,] GetFaces(byte[,,] voxelData)
        {
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);
            FaceVoxel[,,] data = new FaceVoxel[xSize, ySize, zSize];
            int[] adjx = new int[] { 0, -1, 0, 1, 0, 0 },
                  adjy = new int[] { 0, 0, -1, 0, 1, 0 },
                  adjz = new int[] { -1, 0, 0, 0, 0, 1 };

            for(int z = 0; z < zSize; z++)
            {
                for(int x = 0; x < xSize; x++)
                {
                    for(int y = 0; y < ySize; y++)
                    {
                        if(voxelData[x, y, z] > 0)
                            data[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)z, color = voxelData[x, y, z] }, Slope.Top | Slope.Bright | Slope.Dim);
                    }
                }
            }
            foreach(FaceModifier fm in Modifiers)
            {
                data = fm(data);
            }
            return data;
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
                            ((faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Top) == Slope.Top)) &&
    //                        (faces[x-1, y, z] == null || (faces[x-1, y, z] != null && (faces[x-1, y, z].slope & Slope.Top) == Slope.Top)) &&
       //                     (faces[x+1, y, z] == null || (faces[x+1, y, z] != null && (faces[x+1, y, z].slope & Slope.Top) == Slope.Top)) &&
                            ((faces[x, y+1, z + 1] != null && (faces[x, y+1, z + 1].slope & Slope.Top) == Slope.Top))
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
                            ((faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Top) == Slope.Top)) &&
  //                          (faces[x, y-1, z] == null || (faces[x, y-1, z] != null && (faces[x, y-1, z].slope & Slope.Top) == Slope.Top)) &&
    //                        (faces[x, y+1, z] == null || (faces[x, y+1, z] != null && (faces[x, y+1, z].slope & Slope.Top) == Slope.Top)) &&
                            ((faces[x - 1, y, z + 1] != null && (faces[x - 1, y, z + 1].slope & Slope.Top) == Slope.Top))
    //                        (faces[x-1, y-1, z+1] == null || (faces[x - 1, y-1, z + 1] != null && (faces[x - 1, y-1, z + 1].slope & Slope.Top) == Slope.Top)) &&
      //                      (faces[x-1, y+1, z+1] == null || (faces[x - 1, y+1, z + 1] != null && (faces[x - 1, y+1, z + 1].slope & Slope.Top) == Slope.Top))
                            )
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
                            ((faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Top) == Slope.Top)) &&
//                            (faces[x, y, z-1] == null || (faces[x, y, z-1] != null && (faces[x, y, z-1].slope & Slope.Top) == Slope.Top)) &&
 //                           (faces[x, y, z+1] == null || (faces[x, y, z+1] != null && (faces[x, y, z+1].slope & Slope.Top) == Slope.Top)) &&
                            ((faces[x + 1, y+1, z] != null && (faces[x + 1, y+1, z].slope & Slope.Top) == Slope.Top))
   //                         (faces[x + 1, y + 1, z - 1] == null || (faces[x + 1, y + 1, z - 1] != null && (faces[x + 1, y + 1, z - 1].slope & Slope.Top) == Slope.Top)) &&
     //                       (faces[x + 1, y + 1, z + 1] == null || (faces[x + 1, y + 1, z + 1] != null && (faces[x + 1, y + 1, z + 1].slope & Slope.Top) == Slope.Top))
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
                for(int y = 0; y < ySize - 1; y++)
                {
                    for(int z = 1; z < zSize; z++)
                    {
                        if(faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Top) == Slope.Top  && faces[x, y + 1, z - 1] != null && (faces[x, y + 1, z - 1].slope & Slope.Top) == Slope.Top && faces[x, y, z - 1] == null)
                        {
                            faces[x, y, z - 1] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z - 1), color = faces[x, y, z].vox.color }, Slope.BrightBottom);
                        }
                        else if(faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Top) == Slope.Top  && faces[x, y + 1, z - 1] != null && (faces[x, y + 1, z - 1].slope & Slope.Top) == Slope.Top && faces[x, y, z - 1] != null
                            && (((faces[x, y, z - 1].slope & Slope.DimBottom) == Slope.DimBottom) ||
                                (faces[x, y, z - 1].slope & Slope.BrightDim) == Slope.BrightDim))
                        {
                            faces[x, y, z - 1].slope = Slope.BrightDimBottom;
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
                        if(faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Top) == Slope.Top  && faces[x - 1, y, z - 1] != null && (faces[x - 1, y, z - 1].slope & Slope.Top) == Slope.Top && faces[x, y, z - 1] == null)
                        {
                            faces[x, y, z - 1] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z - 1), color = faces[x, y, z].vox.color }, Slope.DimBottom);
                        }
                        else if(faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Top) == Slope.Top  && faces[x - 1, y, z - 1] != null && (faces[x - 1, y, z - 1].slope & Slope.Top) == Slope.Top && faces[x, y, z - 1] != null
                            && (((faces[x, y, z - 1].slope & Slope.BrightBottom) == Slope.BrightBottom) ||
                                (faces[x, y, z - 1].slope & Slope.BrightDim) == Slope.BrightDim))
                        {
                            faces[x, y, z - 1].slope = Slope.BrightDimBottom;
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
                for(int y = ySize - 1; y > 0; y--)
                {
                    for(int z = 0; z < zSize; z++)
                    {
                        if(faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Top) == Slope.Top  && faces[x - 1, y + 1, z] != null && (faces[x - 1, y + 1, z].slope & Slope.Top) == Slope.Top && faces[x - 1, y, z] == null)
                        {
                            faces[x - 1, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)(x - 1), y = (byte)y, z = (byte)(z), color = faces[x, y, z].vox.color }, Slope.BrightBack);
                        }
                        else if(faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Top) == Slope.Top  && faces[x - 1, y + 1, z] != null && (faces[x - 1, y + 1, z].slope & Slope.Top) == Slope.Top && faces[x - 1, y, z] != null
                             &&
                                 (faces[x - 1, y, z].slope & Slope.BrightTop) == Slope.BrightTop)
                        {
                            faces[x - 1, y, z].slope = Slope.BrightTopBack;
                        }
                        else if(faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Top) == Slope.Top  && faces[x - 1, y + 1, z] != null && (faces[x - 1, y + 1, z].slope & Slope.Top) == Slope.Top && faces[x - 1, y, z] != null
                             && (
                                 (faces[x - 1, y, z].slope & Slope.BrightBottom) == Slope.BrightBottom))
                        {
                            faces[x - 1, y, z].slope = Slope.BrightBottomBack;
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
                for(int y = ySize - 1; y > 0; y--)
                {
                    for(int z = 0; z < zSize; z++)
                    {
                        if(faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Top) == Slope.Top  && faces[x - 1, y + 1, z] != null && (faces[x - 1, y + 1, z].slope & Slope.Top) == Slope.Top && faces[x, y + 1, z] == null)
                        {
                            faces[x, y + 1, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)(y+1), z = (byte)(z), color = faces[x, y, z].vox.color }, Slope.DimBack);
                        }
                        else if(faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Top) == Slope.Top  && faces[x - 1, y + 1, z] != null && (faces[x - 1, y + 1, z].slope & Slope.Top) == Slope.Top && faces[x, y + 1, z] != null
                             &&
                                 (faces[x, y + 1, z].slope & Slope.DimTop) == Slope.DimTop)
                        {
                            faces[x, y + 1, z].slope = Slope.DimTopBack;
                        }
                        else if(faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Top) == Slope.Top  && faces[x - 1, y + 1, z] != null && (faces[x - 1, y + 1, z].slope & Slope.Top) == Slope.Top && faces[x, y + 1, z] != null
                             && (
                                 (faces[x, y + 1, z].slope & Slope.DimBottom) == Slope.DimBottom))
                        {
                            faces[x, y + 1, z].slope = Slope.DimBottomBack;
                        }
                    }
                }
            }
            return faces;
        }


        public static FaceVoxel[,,] AddBackBack(FaceVoxel[,,] faces)
        {
            int xSize = faces.GetLength(0), ySize = faces.GetLength(1), zSize = faces.GetLength(2);

            for(int x = 0; x < xSize - 1; x++)
            {
                for(int y = ySize - 1; y > 0; y--)
                {
                    for(int z = 0; z < zSize; z++)
                    {
                        if(faces[x, y, z] != null && (faces[x, y, z].slope & Slope.Top) == Slope.Top  && faces[x + 1, y + 1, z] != null && (faces[x + 1, y + 1, z].slope & Slope.Top) == Slope.Top && faces[x, y + 1, z] == null)
                        {
                            faces[x, y+1, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)(y+1), z = (byte)(z), color = faces[x, y, z].vox.color }, Slope.BackBack);
                        }
                    }
                }
            }
            return faces;
        }
        public static FaceModifier[] Modifiers = new FaceModifier[] { AddBrightTop, AddDimTop, AddBrightDim,
            //AddBrightBottom, AddDimBottom, AddBrightBack, AddDimBack, AddBackBack
        };
    }
}
