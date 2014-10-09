using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace AssetsPV
{

    class TallPaletteDraw
    {
        public static Random r = new Random();
        private static byte[][] colors = null;
        
        public static float[][] flatcolors = new float[][]
        {
            //plains
            new float[] {0.63F,0.92F,0.3F,2F},
            //forest
            new float[] {0.2F,0.7F,0.15F,6F},
            //desert
            new float[] {1F,0.9F,0.0F,2F},
            //jungle
            new float[] {0F,0.5F,0.35F,6F},
            //hills
            new float[] {0.9F,0.6F,0.15F,10F},
            //mountains
            new float[] {0.7F,0.75F,0.82F,12F},
            //ruins
            new float[] {0.8F,0.4F,0.7F,8F},
            //tundra
            new float[] {0.8F,1F,1F,4F},
            //road
            new float[] {0.5F,0.5F,0.5F,4F},
            //river
            new float[] {0F,0.2F,0.85F,2F},
            //building base
            new float[] {0.55F,0.55F,0.55F,8F},
        };
        private static string[] terrainnames = new string[]
        {
            "Plains"
            ,"Forest"
            ,"Desert"
            ,"Jungle"
            ,"Hills"
            ,"Mountains"
            ,"Ruins"
            ,"Tundra"
            ,"Road"
            ,"River"
            ,"Basement"
            
        };
        /*
Plains	yellow-green
Road	50% gray
River	deep blue
Mountains	light brown
Forest	mid green
Jungle	dark teal
Tundra	whitish-blue
Desert	yellow-orange
Hills	red-brown
Ruins	purple-gray

         */
        public struct MagicaVoxelDataPaletted
        {
            public byte x;
            public byte y;
            public byte z;
            public byte color;

            public MagicaVoxelDataPaletted(BinaryReader stream, bool subsample)
            {
                x = (byte)(subsample ? stream.ReadByte() / 2 : stream.ReadByte());
                y = (byte)(subsample ? stream.ReadByte() / 2 : stream.ReadByte());
                z = (byte)(subsample ? stream.ReadByte() / 2 : stream.ReadByte());
                color = stream.ReadByte();
            }
        }
        private static int sizex = 0, sizey = 0, sizez = 0;
        /// <summary>
        /// Load a MagicaVoxel .vox format file into a MagicaVoxelData[] that we use for voxel chunks.
        /// </summary>
        /// <param name="stream">An open BinaryReader stream that is the .vox file.</param>
        /// <param name="overrideColors">Optional color lookup table for converting RGB values into my internal engine color format.</param>
        /// <returns>The voxel chunk data for the MagicaVoxel .vox file.</returns>
        public static MagicaVoxelData[] FromMagica(BinaryReader stream)
        {
            // check out http://voxel.codeplex.com/wikipage?title=VOX%20Format&referringTitle=Home for the file format used below
            // we're going to return a voxel chunk worth of data
            ushort[] data = new ushort[32 * 128 * 32];

            MagicaVoxelData[] voxelData = null;

            string magic = new string(stream.ReadChars(4));
            int version = stream.ReadInt32();

            // a MagicaVoxel .vox file starts with a 'magic' 4 character 'VOX ' identifier
            if (magic == "VOX ")
            {
                bool subsample = false;

                while (stream.BaseStream.Position < stream.BaseStream.Length)
                {
                    // each chunk has an ID, size and child chunks
                    char[] chunkId = stream.ReadChars(4);
                    int chunkSize = stream.ReadInt32();
                    int childChunks = stream.ReadInt32();
                    string chunkName = new string(chunkId);

                    // there are only 2 chunks we only care about, and they are SIZE and XYZI
                    if (chunkName == "SIZE")
                    {
                        sizex = stream.ReadInt32();
                        sizey = stream.ReadInt32();
                        sizez = stream.ReadInt32();

                        if (sizex > 32 || sizey > 32) subsample = true;

                        stream.ReadBytes(chunkSize - 4 * 3);
                    }
                    else if (chunkName == "XYZI")
                    {
                        // XYZI contains n voxels
                        int numVoxels = stream.ReadInt32();
                        int div = (subsample ? 2 : 1);

                        // each voxel has x, y, z and color index values
                        voxelData = new MagicaVoxelData[numVoxels];
                        for (int i = 0; i < voxelData.Length; i++)
                            voxelData[i] = new MagicaVoxelData(stream, subsample);
                    }
                    else if (chunkName == "RGBA")
                    {
                        colors = new byte[256][];

                        for (int i = 0; i < 256; i++)
                        {
                            byte r = stream.ReadByte();
                            byte g = stream.ReadByte();
                            byte b = stream.ReadByte();
                            byte a = stream.ReadByte();

                            colors[i] = new byte[] { r, g, b, a };
                        }
                    }
                    else stream.ReadBytes(chunkSize);   // read any excess bytes
                }

                if (voxelData.Length == 0) return voxelData; // failed to read any valid voxel data

                // now push the voxel data into our voxel chunk structure
                /*for (int i = 0; i < voxelData.Length; i++)
                {
                    // do not store this voxel if it lies out of range of the voxel chunk (32x128x32)
                    if (voxelData[i].x > 31 || voxelData[i].y > 31 || voxelData[i].z > 127) continue;

                    // use the voxColors array by default, or overrideColor if it is available
                    int voxel = (voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128);
                    //data[voxel] = (colors == null ? voxColors[voxelData[i].color - 1] : colors[voxelData[i].color - 1]);
                }*/
            }

            return voxelData;
        }

        public static Bitmap renderPixels(MagicaVoxelData[] voxels, string dir)
        {
            Bitmap b = new Bitmap(88, 54);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_gray_soft.png");
            //Image reversed = new Bitmap("cube_reversed.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 4;
            int xSize = 32, ySize = 32;
            //g.DrawImage(image, 10, 10, width, height);

            float[][] colorMatrixElements = { 
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            switch (dir)
            {
                case "SE":
                    vls = voxels;
                    break;
                case "SW":
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempY) + (ySize / 2));
                        vls[i].y = (byte)((tempX * -1) + (xSize / 2) - 1);
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;
                case "NW":
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempX * -1) + (xSize / 2) - 1);
                        vls[i].y = (byte)((tempY * -1) + (ySize / 2) - 1);
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;
                case "NE":
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempY * -1) + (ySize / 2) - 1);
                        vls[i].y = (byte)(tempX + (xSize / 2));
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;
            }

            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {

                int current_color = vx.color - 1;

                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {colors[current_color ][0],  0,  0,  0, 0},
   new float[] {0,  colors[current_color][1],  0,  0, 0},
   new float[] {0,  0,  colors[current_color][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(
                   image,
                   new Rectangle((vx.x + vx.y) * 2, 54 - 26 - vx.y + vx.x - vx.z * 3, width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }

        private static int voxelToPixelLarge(int innerX, int innerY, int x, int y, int z, int current_color, int stride, int jitter)
        {
            return 4 * ((x + y) * 2 + 4 + ((current_color == 136) ? jitter - 1 : 0)) + innerX + stride * (300 - 60 - y + x - z * 3 - ((VoxelLogic.xcolors[current_color][3] == VoxelLogic.flat_alpha) ? -2 : jitter) + innerY);
        }

        private static Bitmap renderLargeSmart(MagicaVoxelData[] voxels, int facing, int faction, int frame, bool still)
        {
            Bitmap bmp = new Bitmap(248, 308, PixelFormat.Format32bppArgb);

            // Specify a pixel format.
            PixelFormat pxf = PixelFormat.Format32bppArgb;

            // Lock the bitmap's bits.
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int numBytes = bmpData.Stride * bmp.Height;
            byte[] argbValues = new byte[numBytes];
            argbValues.Fill<byte>(0);
            byte[] shadowValues = new byte[numBytes];
            shadowValues.Fill<byte>(0);
            byte[] outlineValues = new byte[numBytes];
            outlineValues.Fill<byte>(0);
            byte[] bareValues = new byte[numBytes];
            bareValues.Fill<byte>(0);
            bool[] barePositions = new bool[numBytes];
            barePositions.Fill<bool>(false);
            int xSize = 60, ySize = 60;
            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            switch (facing)
            {
                case 0:
                    vls = voxels;
                    break;
                case 1:
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempY) + (ySize / 2));
                        vls[i].y = (byte)((tempX * -1) + (xSize / 2) - 1);
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;
                case 2:
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempX * -1) + (xSize / 2) - 1);
                        vls[i].y = (byte)((tempY * -1) + (ySize / 2) - 1);
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;
                case 3:
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempY * -1) + (ySize / 2) - 1);
                        vls[i].y = (byte)(tempX + (xSize / 2));
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;
            }

            int[] zbuffer = new int[numBytes];
            zbuffer.Fill<int>(-999);

            int jitter = (((frame % 4) % 3) + ((frame % 4) / 3)) * 2;
            if (still)
                jitter = 0;
            foreach (MagicaVoxelData vx in vls.OrderByDescending(v => v.x * 64 - v.y + v.z * 64 * 128 - ((v.color == 249 - 96) ? 64 * 128 * 64 : 0))) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color + 3; //249 - vx.color;
                int p = 0;
                if ((frame % 2 != 0) && VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.spin_alpha_0)
                    continue;
                else if ((frame % 2 != 1) && VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.spin_alpha_1)
                    continue;
/*                else if (current_color >= 168)
                {
                    continue;
                }*/
                else if (current_color == 152 || current_color == 160 || current_color == 136)// || current_color == 104 || current_color == 112) // || current_color == 80
                {

                    if (current_color == 136 && r.Next(7) < 2)
                        continue;
                    int mod_color = current_color + faction;

                    for (int j = 0; j < 4; j++)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            /*
                             &&
                                bareValues[4 * ((vx.x + vx.y) * 2 + 4 + ((current_color == 136) ? jitter - 1 : 0))
                                + i +
                                bmpData.Stride * (300 - 60 - vx.y + vx.x - vx.z * 3 - ((VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.flat_alpha) ? -2 : jitter) + j)] == 0
                             */
                            p = voxelToPixelLarge(i, j, vx.x, vx.y, vx.z, mod_color, bmpData.Stride, jitter);
                            if (argbValues[p] == 0)
                            {
                                argbValues[p] = VoxelLogic.xrendered[mod_color][i + j * 16];

                                /*bareValues[p] = VoxelLogic.xrendered[mod_color][i + j * 16];
                                barePositions[p] = true;*/
                            }
                        }
                    }
                }
                else if (current_color == 96)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            p = voxelToPixelLarge(i, j, vx.x, vx.y, vx.z, current_color + faction, bmpData.Stride, jitter);
                            if (shadowValues[p] == 0)
                            {
                                shadowValues[p] = VoxelLogic.xrendered[current_color + faction][i + j * 16];
                            }
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < 1; j++)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            p = voxelToPixelLarge(i, j, vx.x, vx.y, vx.z, current_color, bmpData.Stride, jitter);

                            if (argbValues[p] == 0)
                            {
                                argbValues[p] = (byte)(colors[current_color][i % 4]);
                                zbuffer[p] = vx.z + vx.x - vx.y;
                            }
                        }
                    }
                    for (int j = 1; j < 4; j++)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            p = voxelToPixelLarge(i, j, vx.x, vx.y, vx.z, current_color, bmpData.Stride, jitter);

                            if (argbValues[p] == 0)
                            {
                                argbValues[p] = (byte)(colors[current_color][i % 4] / 4 * 3);
                                zbuffer[p] = vx.z + vx.x - vx.y;

                            }
                        }
                    }
                    for (int j = 1; j < 4; j++)
                    {
                        for (int i = 8; i < 16; i++)
                        {
                            p = voxelToPixelLarge(i, j, vx.x, vx.y, vx.z, current_color, bmpData.Stride, jitter);

                            if (argbValues[p] == 0)
                            {
                                argbValues[p] = (byte)(colors[current_color][i % 4] / 2);
                                zbuffer[p] = vx.z + vx.x - vx.y;

                            }
                        }
                    }
                }
            }
            for (int i = 3; i < numBytes; i += 4)
            {
                if (argbValues[i] > 255 * VoxelLogic.flat_alpha && barePositions[i] == false)
                {

                    if (zbuffer[i] - 2 > zbuffer[i + 4]) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = 0; argbValues[i + 4 - 2] = 0; argbValues[i + 4 - 3] = 0; }
                    if (zbuffer[i] - 2 > zbuffer[i - 4]) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = 0; argbValues[i - 4 - 2] = 0; argbValues[i - 4 - 3] = 0; }
                    if (zbuffer[i] - 2 > zbuffer[i + bmpData.Stride]) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = 0; argbValues[i + bmpData.Stride - 2] = 0; argbValues[i + bmpData.Stride - 3] = 0; }
                    if (zbuffer[i] - 2 > zbuffer[i - bmpData.Stride]) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = 0; argbValues[i - bmpData.Stride - 2] = 0; argbValues[i - bmpData.Stride - 3] = 0; }
                    if (zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 4]) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = 0; argbValues[i + bmpData.Stride + 4 - 2] = 0; argbValues[i + bmpData.Stride + 4 - 3] = 0; }
                    if (zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 4]) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = 0; argbValues[i - bmpData.Stride - 4 - 2] = 0; argbValues[i - bmpData.Stride - 4 - 3] = 0; }
                    if (zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 4]) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = 0; argbValues[i + bmpData.Stride - 4 - 2] = 0; argbValues[i + bmpData.Stride - 4 - 3] = 0; }
                    if (zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 4]) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = 0; argbValues[i - bmpData.Stride + 4 - 2] = 0; argbValues[i - bmpData.Stride + 4 - 3] = 0; }

                    if (zbuffer[i] - 2 > zbuffer[i + 8]) { argbValues[i + 8] = 255; argbValues[i + 8 - 1] = 0; argbValues[i + 8 - 2] = 0; argbValues[i + 8 - 3] = 0; }
                    if (zbuffer[i] - 2 > zbuffer[i - 8]) { argbValues[i - 8] = 255; argbValues[i - 8 - 1] = 0; argbValues[i - 8 - 2] = 0; argbValues[i - 8 - 3] = 0; }
                    if (zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2]) { argbValues[i + bmpData.Stride * 2] = 255; argbValues[i + bmpData.Stride * 2 - 1] = 0; argbValues[i + bmpData.Stride * 2 - 2] = 0; argbValues[i + bmpData.Stride * 2 - 3] = 0; }
                    if (zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2]) { argbValues[i - bmpData.Stride * 2] = 255; argbValues[i - bmpData.Stride * 2 - 1] = 0; argbValues[i - bmpData.Stride * 2 - 2] = 0; argbValues[i - bmpData.Stride * 2 - 3] = 0; }
                    if (zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 8]) { argbValues[i + bmpData.Stride + 8] = 255; argbValues[i + bmpData.Stride + 8 - 1] = 0; argbValues[i + bmpData.Stride + 8 - 2] = 0; argbValues[i + bmpData.Stride + 8 - 3] = 0; }
                    if (zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 8]) { argbValues[i - bmpData.Stride + 8] = 255; argbValues[i - bmpData.Stride + 8 - 1] = 0; argbValues[i - bmpData.Stride + 8 - 2] = 0; argbValues[i - bmpData.Stride + 8 - 3] = 0; }
                    if (zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 8]) { argbValues[i + bmpData.Stride - 8] = 255; argbValues[i + bmpData.Stride - 8 - 1] = 0; argbValues[i + bmpData.Stride - 8 - 2] = 0; argbValues[i + bmpData.Stride - 8 - 3] = 0; }
                    if (zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 8]) { argbValues[i - bmpData.Stride - 8] = 255; argbValues[i - bmpData.Stride - 8 - 1] = 0; argbValues[i - bmpData.Stride - 8 - 2] = 0; argbValues[i - bmpData.Stride - 8 - 3] = 0; }
                    if (zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 + 8]) { argbValues[i + bmpData.Stride * 2 + 8] = 255; argbValues[i + bmpData.Stride * 2 + 8 - 1] = 0; argbValues[i + bmpData.Stride * 2 + 8 - 2] = 0; argbValues[i + bmpData.Stride * 2 + 8 - 3] = 0; }
                    if (zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 + 4]) { argbValues[i + bmpData.Stride * 2 + 4] = 255; argbValues[i + bmpData.Stride * 2 + 4 - 1] = 0; argbValues[i + bmpData.Stride * 2 + 4 - 2] = 0; argbValues[i + bmpData.Stride * 2 + 4 - 3] = 0; }
                    if (zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 - 4]) { argbValues[i + bmpData.Stride * 2 - 4] = 255; argbValues[i + bmpData.Stride * 2 - 4 - 1] = 0; argbValues[i + bmpData.Stride * 2 - 4 - 2] = 0; argbValues[i + bmpData.Stride * 2 - 4 - 3] = 0; }
                    if (zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 - 8]) { argbValues[i + bmpData.Stride * 2 - 8] = 255; argbValues[i + bmpData.Stride * 2 - 8 - 1] = 0; argbValues[i + bmpData.Stride * 2 - 8 - 2] = 0; argbValues[i + bmpData.Stride * 2 - 8 - 3] = 0; }
                    if (zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 + 8]) { argbValues[i - bmpData.Stride * 2 + 8] = 255; argbValues[i - bmpData.Stride * 2 + 8 - 1] = 0; argbValues[i - bmpData.Stride * 2 + 8 - 2] = 0; argbValues[i - bmpData.Stride * 2 + 8 - 3] = 0; }
                    if (zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 + 4]) { argbValues[i - bmpData.Stride * 2 + 4] = 255; argbValues[i - bmpData.Stride * 2 + 4 - 1] = 0; argbValues[i - bmpData.Stride * 2 + 4 - 2] = 0; argbValues[i - bmpData.Stride * 2 + 4 - 3] = 0; }
                    if (zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 - 4]) { argbValues[i - bmpData.Stride * 2 - 4] = 255; argbValues[i - bmpData.Stride * 2 - 4 - 1] = 0; argbValues[i - bmpData.Stride * 2 - 4 - 2] = 0; argbValues[i - bmpData.Stride * 2 - 4 - 3] = 0; }
                    if (zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 - 8]) { argbValues[i - bmpData.Stride * 2 - 8] = 255; argbValues[i - bmpData.Stride * 2 - 8 - 1] = 0; argbValues[i - bmpData.Stride * 2 - 8 - 2] = 0; argbValues[i - bmpData.Stride * 2 - 8 - 3] = 0; }

                    /*
                    outlineValues[i + 4] = 255;
                    outlineValues[i - 4] = 255;
                    outlineValues[i + bmpData.Stride] = 255;
                    outlineValues[i - bmpData.Stride] = 255;
                    outlineValues[i + bmpData.Stride + 4] = 255;
                    outlineValues[i - bmpData.Stride - 4] = 255;
                    outlineValues[i + bmpData.Stride - 4] = 255;
                    outlineValues[i - bmpData.Stride + 4] = 255;

                    outlineValues[i + 8] = 255;
                    outlineValues[i - 8] = 255;
                    outlineValues[i + bmpData.Stride * 2] = 255;
                    outlineValues[i - bmpData.Stride * 2] = 255;
                    outlineValues[i + bmpData.Stride + 8] = 255;
                    outlineValues[i - bmpData.Stride + 8] = 255;
                    outlineValues[i + bmpData.Stride - 8] = 255;
                    outlineValues[i - bmpData.Stride - 8] = 255;
                    outlineValues[i + bmpData.Stride * 2 + 8] = 255;
                    outlineValues[i + bmpData.Stride * 2 + 4] = 255;
                    outlineValues[i + bmpData.Stride * 2 - 4] = 255;
                    outlineValues[i + bmpData.Stride * 2 - 8] = 255;
                    outlineValues[i - bmpData.Stride * 2 + 8] = 255;
                    outlineValues[i - bmpData.Stride * 2 + 4] = 255;
                    outlineValues[i - bmpData.Stride * 2 - 4] = 255;
                    outlineValues[i - bmpData.Stride * 2 - 8] = 255;*/
                }
            }

            for (int i = 3; i < numBytes; i += 4)
            {
                if (outlineValues[i] > 0 || (argbValues[i] > 0 && argbValues[i] <= 255 * VoxelLogic.flat_alpha))
                    argbValues[i] = 255;
            }

            for (int i = 3; i < numBytes; i += 4)
            {
                if (argbValues[i] == 0 && bareValues[i] > 0)
                {
                    argbValues[i - 3] = bareValues[i - 3];
                    argbValues[i - 2] = bareValues[i - 2];
                    argbValues[i - 1] = bareValues[i - 1];
                    argbValues[i - 0] = bareValues[i - 0];
                }
                else if (argbValues[i] == 0 && shadowValues[i] > 0)
                {
                    argbValues[i - 3] = shadowValues[i - 3];
                    argbValues[i - 2] = shadowValues[i - 2];
                    argbValues[i - 1] = shadowValues[i - 1];
                    argbValues[i - 0] = shadowValues[i - 0];
                }
            }
            Marshal.Copy(argbValues, 0, ptr, numBytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            return bmp;
        }

        public static Bitmap drawPixelsFlatDouble(int color)
        {
            Bitmap b = new Bitmap(256, 180, PixelFormat.Format32bppArgb);
            Bitmap bold = new Bitmap(256, 180, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(b);
            Graphics gBold = Graphics.FromImage(bold);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_soft.png");
            //            Image gray = new Bitmap("cube_gray_soft.png");
            //Image reversed = new Bitmap("cube_reversed.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 4;

            int[,] shades = new int[64, 64];
            int depth = (int)(flatcolors[color][3]);

            for (int y = 63; y >= 0; y--)
            {
                for (int x = 0; x <= 63; x++)
                {
                    if ((y >= 60 || y <= 3) && (x < 32 + depth) && (x > 32 - depth) && (Math.Abs(32 - x) + depth) % 2 == 1)
                    {
                        shades[x, y] = 2;
                    }

                    else if ((x >= 60 || x <= 3) && (y < 32 + depth) && (y > 32 - depth) && (Math.Abs(32 - y) + depth) % 2 == 1)// && (y % 2 == 1)
                    {
                        shades[x, y] = 2;
                    }
                    else
                    {
                        shades[x, y] = (x == 0 || y == 0 || x == 63 || y == 63) ? 0 : (r.Next(40) == 0) ? r.Next(2) : 1;
                    }
                }
            }



            //g.DrawImage(image, 10, 10, width, height);
            float merged = (flatcolors[color][0] + flatcolors[color][1] + flatcolors[color][2]) * 0.45F;


            ColorMatrix colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {(merged + flatcolors[color][0]) * 0.5F,  0,  0,  0, 0},
   new float[] {0,  (merged + flatcolors[color][1]) * 0.5F,  0,  0, 0},
   new float[] {0,  0,  (merged + flatcolors[color][2]) * 0.5F,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
            ColorMatrix colorMatrixDark = new ColorMatrix(new float[][]{ 
   new float[] {merged*0.3F + flatcolors[color][0] * 0.5F,  0,  0,  0, 0},
   new float[] {0,  merged*0.3F + flatcolors[color][1] * 0.52F,  0,  0, 0},
   new float[] {0,  0,  merged*0.3F + flatcolors[color][2] * 0.58F,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
            ColorMatrix colorMatrixBright = new ColorMatrix(new float[][]{ 
   new float[] {merged*0.55F + flatcolors[color][0] * 0.85F,  0,  0,  0, 0},
   new float[] {0,  merged*0.55F + flatcolors[color][1] * 0.85F,  0,  0, 0},
   new float[] {0,  0,  merged*0.55F + flatcolors[color][2] * 0.85F,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

            ColorMatrix[] mats = new ColorMatrix[] { colorMatrixDark, colorMatrix, colorMatrixBright };
            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);




            imageAttributes.SetColorMatrix(colorMatrixDark, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            for (int z = 1; z < depth; z++)
            {
                for (int x = 0; x <= 63; x++)
                {
                    g.DrawImage(
                   image,
                   new Rectangle((x + 0) * 2, 180 - 64 - 0 - 0 + x - z * 3, width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
                }
                for (int y = 63; y >= 0; y--)
                {
                    g.DrawImage(
                   image,
                   new Rectangle((63 + y) * 2, 180 - 64 - 0 - y + 63 - z * 3, width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);

                }
            }

            //            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            for (int z = 1; z < depth; z++)
            {
                for (int x = 0; x <= 63; x++)
                {
                    gBold.DrawImage(
                    image,
                    new Rectangle((x + 0) * 2, 180 - 64 - 0 - 0 + x - z * 3, width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                    0, 0,        // upper-left corner of source rectangle
                    width,       // width of source rectangle
                    height,      // height of source rectangle
                    GraphicsUnit.Pixel,
                    imageAttributes);
                }
                for (int y = 63; y >= 0; y--)
                {
                    gBold.DrawImage(
                    image,
                    new Rectangle((63 + y) * 2, 180 - 64 - 0 - y + 63 - z * 3, width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                    0, 0,        // upper-left corner of source rectangle 
                    width,       // width of source rectangle
                    height,      // height of source rectangle
                    GraphicsUnit.Pixel,
                    imageAttributes);
                }
            }


            for (int y = 63; y >= 0; y--)
            {
                for (int x = 0; x <= 63; x++)
                {
                    imageAttributes.SetColorMatrix(mats[shades[x, y]], ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    //if ((y >= 30 || y <= 1) && (x < 16 + depth) && (x > 16 - depth) && (Math.Abs(16 - x) + depth) % 2 == 1)
                    //{

                    //    float[] power = new float[] { 0.4F, 0.6F }; //, 0.4F, 0.7F, 0.4F, 0.8F
                    //    //                        float[] power = new float[] { 0.3F, 0.6F, 0.32F, 0.65F, 0.34F, 0.7F, 0.36F, 0.75F, 0.38F, 0.8F };
                    //    int dist = (Math.Abs(16 - x) + depth) % 2;
                    //    //int dist = ((x - 8)/2) % 10;

                    //    imageAttributes.SetColorMatrix(
                    //       colorMatrixBright,
                    //       ColorMatrixFlag.Default,
                    //       ColorAdjustType.Bitmap);
                    //}
                    //else if ((x >= 30 || x <= 1) && (y < 16 + depth) && (y > 16 - depth) && (Math.Abs(16 - y) + depth) % 2 == 1)// && (y % 2 == 1)
                    //{
                    //    float[] power = new float[] { 0.4F, 0.6F }; //, 0.5F, 0.8F, 0.5F, 0.8F,
                    //    //                        float[] power = new float[] { 0.4F, 0.6F, 0.4F, 0.7F, 0.4F, 0.8F };
                    //    int dist = (Math.Abs(16 - y) + depth) % 2;
                    //    imageAttributes.SetColorMatrix(
                    //       colorMatrixBright,
                    //       ColorMatrixFlag.Default,
                    //       ColorAdjustType.Bitmap);
                    //}
                    //else
                    //{
                    //    imageAttributes.SetColorMatrix(
                    //        (x == 0 || y == 0 || x == 31 || y == 31) ? colorMatrixDark : colorMatrix,
                    //        //(x == z || y == z || x == 31 - z || y == 31 - z) ? colorMatrixDark : colorMatrix,
                    //       ColorMatrixFlag.Default,
                    //       ColorAdjustType.Bitmap);
                    //}
                    g.DrawImage(
                   image,
                   new Rectangle((x + y) * 2, 180 - 64 - 0 - y + x - depth * 3, width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);

                    ///////////////
                    ///////////////BRIGHT VERSION CODE
                    ///////////////
                    if ((y >= 56 || y <= 6) && (x < 32 + depth) && (x > 32 - depth) && (Math.Abs(32 - x) + depth) % 2 == 1)
                    {

                        //                        float[] power = new float[] { 0.5F, 0.8F }; //, 0.5F, 0.8F, 0.5F, 0.8F,
                        //                        float[] power = new float[] { 0.3F, 0.6F, 0.32F, 0.65F, 0.34F, 0.7F, 0.36F, 0.75F, 0.38F, 0.8F };
                        int dist = (Math.Abs(32 - x) + depth) % 2;
                        //int dist = ((x - 8)/2) % 10;

                        imageAttributes.SetColorMatrix(
                           colorMatrixBright,
                           ColorMatrixFlag.Default,
                           ColorAdjustType.Bitmap);
                    }
                    else if ((x >= 56 || x <= 6) && (y < 32 + depth) && (y > 32 - depth) && (Math.Abs(32 - y) + depth) % 2 == 1)
                    {
                        //                        float[] power = new float[] { 0.5F, 0.8F }; //, 0.4F, 0.7F, 0.4F, 0.8F 
                        int dist = (Math.Abs(32 - y) + depth) % 2;
                        imageAttributes.SetColorMatrix(
                           colorMatrixBright,
                           ColorMatrixFlag.Default,
                           ColorAdjustType.Bitmap);
                    }
                    else
                    {
                        imageAttributes.SetColorMatrix(
                            (x <= 6 || y <= 6 || x >= 56 || y >= 56) ? colorMatrixDark : mats[shades[x, y]],
                            //(x == z || y == z || x == 31 - z || y == 31 - z) ? colorMatrixDark : colorMatrix,
                           ColorMatrixFlag.Default,
                           ColorAdjustType.Bitmap);
                    }
                    gBold.DrawImage(
                    image,
                    new Rectangle((x + y) * 2, 180 - 64 - 0 - y + x - depth * 3, width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                    0, 0,        // upper-left corner of source rectangle 
                    width,       // width of source rectangle
                    height,      // height of source rectangle
                    GraphicsUnit.Pixel,
                    imageAttributes);
                }
            }
            System.IO.Directory.CreateDirectory("Terrain");
            Bitmap b2 = new Bitmap(128, 90, PixelFormat.Format32bppArgb);
            Graphics g2 = Graphics.FromImage(b2);
            g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            Bitmap b3 = bold.Clone(new Rectangle(0, 0, 256, 180), b.PixelFormat);
            g2.DrawImage(b3, 0, 0, 128, 90);
            b2.Save("Terrain/" + terrainnames[color] + "_bold.png");

            b2 = new Bitmap(128, 90, PixelFormat.Format32bppArgb);
            g2 = Graphics.FromImage(b2);
            g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            b3 = b.Clone(new Rectangle(0, 0, 256, 180), b.PixelFormat);
            g2.DrawImage(b3, 0, 0, 128, 90);
            b2.Save("Terrain/" + terrainnames[color] + ".png");
            b.Dispose();
            g2.Dispose();
            b3.Dispose();

            return b2;
        }
        public static Bitmap drawPixelsFlat(int color)
        {
            Bitmap b = new Bitmap(128, 90, PixelFormat.Format32bppArgb);
            Bitmap bold = new Bitmap(128, 90, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(b);
            Graphics gBold = Graphics.FromImage(bold);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_soft.png");
            //            Image gray = new Bitmap("cube_gray_soft.png");
            //Image reversed = new Bitmap("cube_reversed.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 4;

            int[,] shades = new int[32, 32];
            int depth = (int)(flatcolors[color][3]);

            for (int y = 31; y >= 0; y--)
            {
                for (int x = 0; x <= 31; x++)
                {
                    if ((y >= 30 || y <= 1) && (x < 16 + depth) && (x > 16 - depth) && (Math.Abs(16 - x) + depth) % 2 == 1)
                    {
                        shades[x, y] = 2;
                    }

                    else if ((x >= 30 || x <= 1) && (y < 16 + depth) && (y > 16 - depth) && (Math.Abs(16 - y) + depth) % 2 == 1)// && (y % 2 == 1)
                    {
                        shades[x, y] = 2;
                    }
                    else
                    {
                        shades[x, y] = (x == 0 || y == 0 || x == 31 || y == 31) ? 0 : (r.Next(50) == 0) ? r.Next(2) : 1;
                    }
                }
            }



            //g.DrawImage(image, 10, 10, width, height);
            float merged = (flatcolors[color][0] + flatcolors[color][1] + flatcolors[color][2]) * 0.45F;


            ColorMatrix colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {(merged + flatcolors[color][0]) * 0.5F,  0,  0,  0, 0},
   new float[] {0,  (merged + flatcolors[color][1]) * 0.5F,  0,  0, 0},
   new float[] {0,  0,  (merged + flatcolors[color][2]) * 0.5F,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
            ColorMatrix colorMatrixDark = new ColorMatrix(new float[][]{ 
   new float[] {merged*0.3F + flatcolors[color][0] * 0.5F,  0,  0,  0, 0},
   new float[] {0,  merged*0.3F + flatcolors[color][1] * 0.52F,  0,  0, 0},
   new float[] {0,  0,  merged*0.3F + flatcolors[color][2] * 0.58F,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
            ColorMatrix colorMatrixBright = new ColorMatrix(new float[][]{ 
   new float[] {merged*0.55F + flatcolors[color][0] * 0.85F,  0,  0,  0, 0},
   new float[] {0,  merged*0.55F + flatcolors[color][1] * 0.85F,  0,  0, 0},
   new float[] {0,  0,  merged*0.55F + flatcolors[color][2] * 0.85F,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

            ColorMatrix[] mats = new ColorMatrix[] { colorMatrixDark, colorMatrix, colorMatrixBright };
            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);




            imageAttributes.SetColorMatrix(colorMatrixDark, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            for (int z = 1; z < depth; z++)
            {
                for (int x = 0; x < 31; x++)
                {
                    g.DrawImage(
                   image,
                   new Rectangle((x + 0) * 2, 90 - 32 - 0 - 0 + x - z * 3, width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
                }
                for (int y = 31; y >= 0; y--)
                {
                    g.DrawImage(
                   image,
                   new Rectangle((31 + y) * 2, 90 - 32 - 0 - y + 31 - z * 3, width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);

                }
            }

            //            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            for (int z = 1; z < depth; z++)
            {
                for (int x = 0; x < 31; x++)
                {
                    gBold.DrawImage(
                    image,
                    new Rectangle((x + 0) * 2, 90 - 32 - 0 - 0 + x - z * 3, width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                    0, 0,        // upper-left corner of source rectangle 
                    width,       // width of source rectangle
                    height,      // height of source rectangle
                    GraphicsUnit.Pixel,
                    imageAttributes);
                }
                for (int y = 31; y >= 0; y--)
                {
                    gBold.DrawImage(
                    image,
                    new Rectangle((31 + y) * 2, 90 - 32 - 0 - y + 31 - z * 3, width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                    0, 0,        // upper-left corner of source rectangle 
                    width,       // width of source rectangle
                    height,      // height of source rectangle
                    GraphicsUnit.Pixel,
                    imageAttributes);
                }
            }


            for (int y = 31; y >= 0; y--)
            {
                for (int x = 0; x <= 31; x++)
                {
                    imageAttributes.SetColorMatrix(mats[shades[x, y]], ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    //if ((y >= 30 || y <= 1) && (x < 16 + depth) && (x > 16 - depth) && (Math.Abs(16 - x) + depth) % 2 == 1)
                    //{

                    //    float[] power = new float[] { 0.4F, 0.6F }; //, 0.4F, 0.7F, 0.4F, 0.8F
                    //    //                        float[] power = new float[] { 0.3F, 0.6F, 0.32F, 0.65F, 0.34F, 0.7F, 0.36F, 0.75F, 0.38F, 0.8F };
                    //    int dist = (Math.Abs(16 - x) + depth) % 2;
                    //    //int dist = ((x - 8)/2) % 10;

                    //    imageAttributes.SetColorMatrix(
                    //       colorMatrixBright,
                    //       ColorMatrixFlag.Default,
                    //       ColorAdjustType.Bitmap);
                    //}
                    //else if ((x >= 30 || x <= 1) && (y < 16 + depth) && (y > 16 - depth) && (Math.Abs(16 - y) + depth) % 2 == 1)// && (y % 2 == 1)
                    //{
                    //    float[] power = new float[] { 0.4F, 0.6F }; //, 0.5F, 0.8F, 0.5F, 0.8F,
                    //    //                        float[] power = new float[] { 0.4F, 0.6F, 0.4F, 0.7F, 0.4F, 0.8F };
                    //    int dist = (Math.Abs(16 - y) + depth) % 2;
                    //    imageAttributes.SetColorMatrix(
                    //       colorMatrixBright,
                    //       ColorMatrixFlag.Default,
                    //       ColorAdjustType.Bitmap);
                    //}
                    //else
                    //{
                    //    imageAttributes.SetColorMatrix(
                    //        (x == 0 || y == 0 || x == 31 || y == 31) ? colorMatrixDark : colorMatrix,
                    //        //(x == z || y == z || x == 31 - z || y == 31 - z) ? colorMatrixDark : colorMatrix,
                    //       ColorMatrixFlag.Default,
                    //       ColorAdjustType.Bitmap);
                    //}
                    g.DrawImage(
                   image,
                   new Rectangle((x + y) * 2, 90 - 32 - 0 - y + x - depth * 3, width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);

                    ///////////////
                    ///////////////BRIGHT VERSION CODE
                    ///////////////
                    if ((y >= 28 || y <= 3) && (x < 16 + depth) && (x > 16 - depth) && (Math.Abs(16 - x) + depth) % 2 == 1)
                    {

                        //                        float[] power = new float[] { 0.5F, 0.8F }; //, 0.5F, 0.8F, 0.5F, 0.8F,
                        //                        float[] power = new float[] { 0.3F, 0.6F, 0.32F, 0.65F, 0.34F, 0.7F, 0.36F, 0.75F, 0.38F, 0.8F };
                        int dist = (Math.Abs(16 - x) + depth) % 2;
                        //int dist = ((x - 8)/2) % 10;

                        imageAttributes.SetColorMatrix(
                           colorMatrixBright,
                           ColorMatrixFlag.Default,
                           ColorAdjustType.Bitmap);
                    }
                    else if ((x >= 28 || x <= 3) && (y < 16 + depth) && (y > 16 - depth) && (Math.Abs(16 - y) + depth) % 2 == 1)
                    {
                        //                        float[] power = new float[] { 0.5F, 0.8F }; //, 0.4F, 0.7F, 0.4F, 0.8F 
                        int dist = (Math.Abs(16 - y) + depth) % 2;
                        imageAttributes.SetColorMatrix(
                           colorMatrixBright,
                           ColorMatrixFlag.Default,
                           ColorAdjustType.Bitmap);
                    }
                    else
                    {
                        imageAttributes.SetColorMatrix(
                            (x <= 3 || y <= 3 || x >= 28 || y >= 28) ? colorMatrixDark : mats[shades[x, y]],
                            //(x == z || y == z || x == 31 - z || y == 31 - z) ? colorMatrixDark : colorMatrix,
                           ColorMatrixFlag.Default,
                           ColorAdjustType.Bitmap);
                    }
                    gBold.DrawImage(
                    image,
                    new Rectangle((x + y) * 2, 90 - 32 - 0 - y + x - depth * 3, width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                    0, 0,        // upper-left corner of source rectangle 
                    width,       // width of source rectangle
                    height,      // height of source rectangle
                    GraphicsUnit.Pixel,
                    imageAttributes);
                }
            }
            System.IO.Directory.CreateDirectory("Terrain");
            b.Save("Terrain/" + terrainnames[color] + ".png");
            bold.Save("Terrain/" + terrainnames[color] + "_bold.png");

            //            Bitmap normal = new Bitmap("Terrain/" + terrainnames[color] + ".png");
            //            Bitmap dim = new Bitmap(128, 100, PixelFormat.Format32bppArgb);
            //            g = Graphics.FromImage(dim);
            //            imageAttributes.SetColorMatrix(
            //                           new ColorMatrix(new float[][]{ 
            //   new float[] {0.7F,  0,  0,  0, 0},
            //   new float[] {0,  0.7F,  0,  0, 0},
            //   new float[] {0,  0,  0.7F,  0, 0},
            //   new float[] {0,  0,  0,  1F, 0},
            //   new float[] {0, 0, 0, 0, 1F}}),
            //                           ColorMatrixFlag.Default,
            //                           ColorAdjustType.Bitmap);
            //            g.DrawImage(normal,
            //                   new Rectangle(0, 0, 128, 100),  // destination rectangle 
            //                //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
            //                   0, 0,        // upper-left corner of source rectangle 
            //                   128,       // width of source rectangle
            //                   100,      // height of source rectangle
            //                   GraphicsUnit.Pixel,
            //                   imageAttributes);
            ////            dim.Save("Terrain/" + terrainnames[color] + "_dim.png");

            //            Bitmap[] spectrum = { new Bitmap(128, 100, PixelFormat.Format32bppArgb), new Bitmap(128, 100, PixelFormat.Format32bppArgb), 
            //                                   new Bitmap(128, 100, PixelFormat.Format32bppArgb), new Bitmap(128, 100, PixelFormat.Format32bppArgb),
            //                                   new Bitmap(128, 100, PixelFormat.Format32bppArgb), new Bitmap(128, 100, PixelFormat.Format32bppArgb), 
            //                                   new Bitmap(128, 100, PixelFormat.Format32bppArgb), new Bitmap(128, 100, PixelFormat.Format32bppArgb)};
            //            for (int faction = 0; faction < 8; faction++)
            //            {
            //                g = Graphics.FromImage(spectrum[faction]);
            //                imageAttributes.SetColorMatrix(
            //               new ColorMatrix(new float[][]{ 
            //   new float[] {0.5F,  0,  0,  0, 0},
            //   new float[] {0,  0.5F,  0,  0, 0},
            //   new float[] {0,  0,  0.5F,  0, 0},
            //   new float[] {0,  0,     0,  1F, 0},
            //   new float[] {0.55F*(0.22F+PlusVoxels.colors[32 + faction][0]), 0.55F*(0.251F+PlusVoxels.colors[32 + faction][1]), 0.55F*(0.31F+PlusVoxels.colors[32 + faction][2]), 0, 1F}}),
            //                               ColorMatrixFlag.Default,
            //                               ColorAdjustType.Bitmap);
            //                g.DrawImage(normal,
            //                       new Rectangle(0, 0, 128, 100),  // destination rectangle 
            //                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
            //                       0, 0,        // upper-left corner of source rectangle 
            //                       128,       // width of source rectangle
            //                       100,      // height of source rectangle
            //                       GraphicsUnit.Pixel,
            //                       imageAttributes);

            //           spectrum[faction].Save("Terrain/" + terrainnames[color] + "_color" + faction + ".png");
            //            }


            //            spectrum = new Bitmap[]{ new Bitmap(128, 100, PixelFormat.Format32bppArgb), new Bitmap(128, 100, PixelFormat.Format32bppArgb), 
            //                                     new Bitmap(128, 100, PixelFormat.Format32bppArgb), new Bitmap(128, 100, PixelFormat.Format32bppArgb),
            //                                     new Bitmap(128, 100, PixelFormat.Format32bppArgb), new Bitmap(128, 100, PixelFormat.Format32bppArgb), 
            //                                     new Bitmap(128, 100, PixelFormat.Format32bppArgb), new Bitmap(128, 100, PixelFormat.Format32bppArgb)};
            //            for (int faction = 0; faction < 8; faction++)
            //            {
            //                g = Graphics.FromImage(spectrum[faction]);
            //                imageAttributes.SetColorMatrix(
            //               new ColorMatrix(new float[][]{ 
            //   new float[] {0.5F,  0,  0,  0, 0},
            //   new float[] {0,  0.5F,  0,  0, 0},
            //   new float[] {0,  0,  0.5F,  0, 0},
            //   new float[] {0,  0,     0,  1F, 0},
            //   new float[] {0.55F*(0.22F+PlusVoxels.colors[32 + faction][0]), 0.55F*(0.251F+PlusVoxels.colors[32 + faction][1]), 0.55F*(0.31F+PlusVoxels.colors[32 + faction][2]), 0, 1F}}),
            //               ColorMatrixFlag.Default,
            //               ColorAdjustType.Bitmap);

            //                /*            imageAttributes.SetColorMatrix(
            //                                           new ColorMatrix(new float[][]{ 
            //                   new float[] {0.22F+PlusVoxels.colors[32 + faction][0],  0,  0,  0, 0},
            //                   new float[] {0,  0.251F+PlusVoxels.colors[32 + faction][1],  0,  0, 0},
            //                   new float[] {0,  0,  0.31F+PlusVoxels.colors[32 + faction][2],  0, 0},
            //                   new float[] {0,  0,  0,  1F, 0},
            //                   new float[] {0, 0, 0, 0, 1F}}),
            //                                           ColorMatrixFlag.Default,
            //                                           ColorAdjustType.Bitmap);*/
            //                g.DrawImage(bold,
            //                       new Rectangle(0, 0, 128, 100),  // destination rectangle 
            //                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
            //                       0, 0,        // upper-left corner of source rectangle 
            //                       128,       // width of source rectangle
            //                       100,      // height of source rectangle
            //                       GraphicsUnit.Pixel,
            //                       imageAttributes);

            ////                spectrum[faction].Save("Terrain/" + terrainnames[color] + "_bold_color" + faction + ".png");
            //            }
            //AlterChannels(1, 1, 1);
            //        public static ShaderProgram Bright = AlterChannels(1.35f, 1.35f, 1.35f);
            //public static ShaderProgram[] Spectrum = { AlterChannels(1.4f, 0.8f, 0.8f), AlterChannels(1.4f, 1.4f, 0.7f), AlterChannels(0.8f, 1.4f, 0.8f), AlterChannels(0.85f, 0.85f, 1.4f)};
            return b;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Processing: Pangolin");
            BinaryReader bin = new BinaryReader(File.Open("Pangolin_Custom.vox", FileMode.Open));
            MagicaVoxelData[] parsed = FromMagica(bin);
            for (int i = 0; i < parsed.Length; i++)
            {
                parsed[i].x += 10;
                parsed[i].y += 10;
            }
            System.IO.Directory.CreateDirectory("custom");
            for(int d = 0; d < 4; d++)
            {
                Graphics g;
                Bitmap b;
                Bitmap b2 = new Bitmap(88, 108, PixelFormat.Format32bppArgb);
                b = renderLargeSmart(parsed, d, 0, 0, true);
                g = Graphics.FromImage(b);
                Graphics g2 = Graphics.FromImage(b2);
                g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g2.DrawImage(b.Clone(new Rectangle(32, 46 + 32, 88 * 2, 108 * 2), b.PixelFormat), 0, 0, 88, 108);
                g2.Dispose();
                b2.Save("custom/Pangolin_Large_face" + d + "_0.png", ImageFormat.Png);
            }
        }
    }
}
