using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace AssetsCU
{

    class OrthoPaletteDraw
    {
        public static Random r = new Random();
        public static float[][] flatcolors = new float[][]
        {
            //plains
            new float[] {0.63F,0.92F,0.3F,3F},
            //forest
            new float[] {0.2F,0.7F,0.15F,3F},
            //desert
            new float[] {1F,0.9F,0.0F,3F},
            //jungle
            new float[] {0F,0.5F,0.35F,3F},
            //hills
            new float[] {0.9F,0.6F,0.15F,7F},
            //mountains
            new float[] {0.7F,0.75F,0.82F,7F},
            //ruins
            new float[] {0.8F,0.4F,0.7F,5F},
            //tundra
            new float[] {0.8F,1F,1F,3F},
            //road
            new float[] {0.5F,0.5F,0.5F,5F},
            //river
            new float[] {0F,0.2F,0.85F,1F},
            //building base
            new float[] {0.55F,0.55F,0.55F,5F},
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
        private static float[][] colors = null;
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
        public static MagicaVoxelDataPaletted[] FromMagica(BinaryReader stream)
        {
            // check out http://voxel.codeplex.com/wikipage?title=VOX%20Format&referringTitle=Home for the file format used below
            // we're going to return a voxel chunk worth of data
            ushort[] data = new ushort[32 * 128 * 32];
            
            MagicaVoxelDataPaletted[] voxelData = null;

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
                        voxelData = new MagicaVoxelDataPaletted[numVoxels];
                        for (int i = 0; i < voxelData.Length; i++)
                            voxelData[i] = new MagicaVoxelDataPaletted(stream, subsample);
                    }
                    else if (chunkName == "RGBA")
                    {
                        colors = new float[256][];

                        for (int i = 0; i < 256; i++)
                        {
                            byte r = stream.ReadByte();
                            byte g = stream.ReadByte();
                            byte b = stream.ReadByte();
                            byte a = stream.ReadByte();

                            colors[i] = new float[] { r / 256.0f, g / 256.0f, b / 256.0f, a / 256.0f};
                        }
                    }
                    else stream.ReadBytes(chunkSize);   // read any excess bytes
                }
                
                if (voxelData.Length == 0) return voxelData; // failed to read any valid voxel data

                // now push the voxel data into our voxel chunk structure
                for (int i = 0; i < voxelData.Length; i++)
                {
                    // do not store this voxel if it lies out of range of the voxel chunk (32x128x32)
                    if (voxelData[i].x > 31 || voxelData[i].y > 31 || voxelData[i].z > 127) continue;
                    
                    // use the voxColors array by default, or overrideColor if it is available
                    int voxel = (voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128);
                    //data[voxel] = (colors == null ? voxColors[voxelData[i].color - 1] : colors[voxelData[i].color - 1]);
                }
            }

            return voxelData;
        }
        private static void drawPixels(MagicaVoxelDataPaletted[] voxels)
        {
            Bitmap b = new Bitmap(80, 80);
            Graphics g = Graphics.FromImage((Image)b);
            Image image = new Bitmap("cube.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 3;

            //g.DrawImage(image, 10, 10, width, height);

            float[][] colorMatrixElements = { 
   new float[] {1F,  0,  0,  0, 0},
   new float[] {0,  1F,  0,  0, 0},
   new float[] {0,  0,  1F,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            foreach (MagicaVoxelDataPaletted vx in voxels.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {

                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {colors[vx.color - 1][0],  0,  0,  0, 0},
   new float[] {0,  colors[vx.color - 1][1],  0,  0, 0},
   new float[] {0,  0,  colors[vx.color - 1][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);

                g.DrawImage(
                   image,
                   new Rectangle((vx.x + vx.y) * 2, 80 - 3 - 32 - vx.y + vx.x - 2 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            b.Save("output.png", ImageFormat.Png);
        }

        public static Bitmap drawPixelsFlat(int color)
        {
            Bitmap b = new Bitmap(64, 48, PixelFormat.Format32bppArgb);
            Bitmap bold = new Bitmap(64, 48, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(b);
            Graphics gBold = Graphics.FromImage(bold);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_ortho.png");
            //            Image gray = new Bitmap("cube_gray_soft.png");
            //Image reversed = new Bitmap("cube_reversed.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 2;
            int height = 3;

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
            for (int z = 0; z < depth; z++)
            {
                //vx.y * 2, 44 - 22 - 2 + vx.x - vx.z*2
                for (int y = 31; y >= 0; y--)
                {
                    g.DrawImage(
                   image,
                   new Rectangle(y * 2, 48 - 3 - z * 2, width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
                }
            }

            //            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            for (int z = 0; z < depth; z++)
            {
                for (int y = 31; y >= 0; y--)
                {
                    gBold.DrawImage(
                   image,
                   new Rectangle(y * 2, 48 - 3 - z * 2, width, height),  // destination rectangle 
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
                   new Rectangle(y * 2, 48 - 32 + x - depth * 2, width, height),  // destination rectangle 
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
                    new Rectangle(y * 2, 48 - 32 + x - depth * 2, width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                    0, 0,        // upper-left corner of source rectangle 
                    width,       // width of source rectangle
                    height,      // height of source rectangle
                    GraphicsUnit.Pixel,
                    imageAttributes);
                }
            }
            System.IO.Directory.CreateDirectory("TerrainOrtho");
            b.Save("TerrainOrtho/" + terrainnames[color] + ".png");
            bold.Save("TerrainOrtho/" + terrainnames[color] + "_bold.png");

            return b;
        }


        static void Main(string[] args)
        {
            BinaryReader bin = new BinaryReader(File.Open("Grass_P.vox", FileMode.Open));
            drawPixels(FromMagica(bin));
        }
    }
}
