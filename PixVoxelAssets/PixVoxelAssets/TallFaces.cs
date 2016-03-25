using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AssetsPV
{
    class TallFaces
    {
        public const int
        Cube = 0,
        BrightTop = 1,
        DimTop = 2,
        BrightDim = 3,
        BrightDimTop = 4,
        BrightBottom = 5,
        DimBottom = 6,
        BrightDimBottom = 7,
        BrightBack = 8,
        DimBack = 9,
        BrightTopBack = 10,
        DimTopBack = 11,
        BrightBottomBack = 12,
        DimBottomBack = 13,
        BackBack = 14;

        public static Dictionary<Slope, int> slopes = new Dictionary<Slope, int> { { Slope.Cube, Cube },
            {  Slope.BrightTop, BrightTop }, {  Slope.DimTop, DimTop }, {  Slope.BrightDim, BrightDim }, {  Slope.BrightDimTop, BrightDimTop }, {  Slope.BrightBottom, BrightBottom }, { Slope.DimBottom, DimBottom },
            {  Slope.BrightDimBottom, BrightDimBottom }, {  Slope.BrightBack, BrightBack }, {  Slope.DimBack, DimBack },
            {  Slope.BrightTopBack, BrightTopBack }, {  Slope.DimTopBack, DimTopBack }, {  Slope.BrightBottomBack, BrightBottomBack }, {  Slope.DimBottomBack, DimBottomBack }, {  Slope.BackBack, BackBack } };
        public static Random r = new Random(0x1337BEEF);
        public static string altFolder = "";

        private static FileStream offbin = new FileStream("offsets2.bin", FileMode.OpenOrCreate);
        private static BinaryWriter offsets = new BinaryWriter(offbin);
        public static Tuple<string, int>[] undead = TallVoxels.undead,
            living = TallVoxels.living,
            hats = TallVoxels.hats,
            ghost_hats = TallVoxels.ghost_hats,
            terrain = TallVoxels.terrain,
            landscape = TallVoxels.landscape;
        public static string[] classes = TallVoxels.classes;
        private static byte[][][][] storeColorCubesWBold()
        {
            VoxelLogic.wpalettecount = VoxelLogic.wpalettes.Length;
            //            wcolorcount = VoxelLogic.wpalettes[0].Length;
            // 17 is the number of Slope enum types.
            byte[,,,] cubes = new byte[VoxelLogic.wpalettecount, VoxelLogic.wpalettes[0].Length, 17, 80];

            Image image = new Bitmap("white.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 5;
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
            for(int p = 0; p < VoxelLogic.wpalettes.Length; p++)
            {
                for(int current_color = 0; current_color < VoxelLogic.wpalettes[0].Length; current_color++)
                {
                    Bitmap b =
                    new Bitmap(width, height, PixelFormat.Format32bppArgb);

                    Graphics g = Graphics.FromImage((Image)b);

                    if(current_color == 25)
                    {
                        colorMatrix = new ColorMatrix(new float[][]{
   new float[] {0.22F+VoxelLogic.wpalettes[p][current_color][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+VoxelLogic.wpalettes[p][current_color][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+VoxelLogic.wpalettes[p][current_color][2],  0, 0},
   new float[] {0,  0,  0,  1, 0},
   new float[] {0, 0, 0, 0, 1F}});
                    }
                    else if(VoxelLogic.wpalettes[p][current_color][3] == VoxelLogic.eraser_alpha)
                    {
                        colorMatrix = new ColorMatrix(new float[][]{
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}});
                    }
                    else if(VoxelLogic.wpalettes[p][current_color][3] == 0F)
                    {
                        colorMatrix = new ColorMatrix(new float[][]{
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 1F}});
                    }
                    else
                    {
                        colorMatrix = new ColorMatrix(new float[][]{
   new float[] {0.235F+VoxelLogic.wpalettes[p][current_color][0],  0,  0,  0, 0},
   new float[] {0,  0.26F+VoxelLogic.wpalettes[p][current_color][1],  0,  0, 0},
   new float[] {0,  0,  0.30F+VoxelLogic.wpalettes[p][current_color][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                    }
                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);

                    string which_image = ((current_color >= 18 && current_color <= 20) || current_color == 40 || VoxelLogic.wpalettes[p][current_color][3] == 0F
                        || VoxelLogic.wpalettes[p][current_color][3] == VoxelLogic.flash_alpha
                        || VoxelLogic.wpalettes[p][current_color][3] == VoxelLogic.flash_alpha_0 || VoxelLogic.wpalettes[p][current_color][3] == VoxelLogic.flash_alpha_1) ? "shine" :
                       (VoxelLogic.wpalettes[p][current_color][3] == VoxelLogic.flat_alpha || VoxelLogic.wpalettes[p][current_color][3] == VoxelLogic.bordered_flat_alpha) ? "flat" : "image";
                    g.DrawImage(image,
                       new Rectangle(0, 0,
                           width, height),  // destination rectangle 
                                            //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                       0, 0,        // upper-left corner of source rectangle 
                       width,       // width of source rectangle
                       height,      // height of source rectangle
                       GraphicsUnit.Pixel,
                       imageAttributes);
                    for(int i = 0; i < width; i++)
                    {
                        for(int j = 0; j < height; j++)
                        {
                            Color c = b.GetPixel(i, j);
                            double h = 0.0, s = 1.0, v = 1.0;
                            VoxelLogic.ColorToHSV(c, out h, out s, out v);
                            //                            if(VoxelLogic.subtlePalettes.Contains(p) && i == 0 && j == 0)
                            //                                Console.WriteLine("palette: " + p + ", current_color: " + current_color + ", s before: " + s + ", s after: " + (VoxelLogic.Clamp(s - 0.3, 0.0, 0.6)));
                            if(VoxelLogic.subtlePalettes.Contains(p))
                            {
                                s = VoxelLogic.Clamp((s * 0.5), 0.0, 0.5);
                                v = VoxelLogic.Clamp(v * 0.9, 0.01, 0.9);
                            }
                            for(int slp = 0; slp < 17; slp++)
                            {
                                Color c2 = Color.Transparent;
                                if(which_image.Equals("image"))
                                {
                                    double s_alter = (s * 0.7 + s * s * s * Math.Sqrt(s)),
                                        v_alter = Math.Pow(v, 2.0 - 2.0 * v);
                                    v_alter *= Math.Pow(v_alter, 0.6);
                                    if(j == height - 1)
                                    {
                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Pow(s, 0.3)) * 1.55, 0.0112, 1.0), VoxelLogic.Clamp(v_alter * 0.65, 0.01, 1.0));
                                    }
                                    else
                                    {
                                        switch(slp)
                                        {
                                            case Cube:
                                            case BackBack:
                                                {
                                                    if(j == 0)
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp(s_alter * 1.05, 0.0112, 1.0), VoxelLogic.Clamp(v_alter * 1.1, 0.09, 1.0));
                                                    }
                                                    else if(i < width / 2)
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp(s_alter * 1.2, 0.0112, 1.0), VoxelLogic.Clamp(v_alter * 0.95, 0.06, 1.0));
                                                    }
                                                    else if(i >= width / 2)
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp(s_alter * 1.35, 0.0112, 1.0), VoxelLogic.Clamp(v_alter * 0.8, 0.03, 1.0));
                                                    }
                                                }
                                                break;
                                            case BrightTop:
                                                {
                                                    /*
                                                    if(j == 0)// && i == width - 1)
                                                    {
                                                        //c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.05, 0.0112, 1.0), VoxelLogic.Clamp(v * 1.1, 0.09, 1.0));
                                                    }*/
                                                    if(i + (j + 1) / 2 > 3 && j > 0)
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp(s_alter * 1.35, 0.0112, 1.0), VoxelLogic.Clamp(v_alter * 0.8, 0.03, 1.0));
                                                    }
                                                    else if(i + j / 2 >= 1)
                                                    //if(j >= 2 &&  i >=  2 - (j / 3) * 2)
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp(s_alter * 1.0, 0.0112, 1.0), VoxelLogic.Clamp(v_alter * 1.15, 0.10, 1.0));

                                                        //                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.35, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.8, 0.03, 1.0));
                                                    }
                                                }
                                                break;
                                            case DimTop:
                                                {
                                                    /*
                                                    if(j == 0)// && i == 0)
                                                    {
                                                        //                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.05, 0.0112, 1.0), VoxelLogic.Clamp(v * 1.1, 0.09, 1.0));
                                                    }*/

                                                    if(i < (j + 1) / 2 && j > 0)
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp(s_alter * 1.2, 0.0112, 1.0), VoxelLogic.Clamp(v_alter * 0.95, 0.06, 1.0));

                                                    }
                                                    else if(i <= j / 2 + 2)
                                                    //                                                    if(j >= 2 && i < (j / 3) * 2 + 2)
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp(s_alter * 1.25, 0.0112, 1.0), VoxelLogic.Clamp(v_alter * 0.9, 0.05, 1.0));

                                                        //                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.2, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.95, 0.06, 1.0));
                                                    }
                                                    else //if(i <= j / 2 + 2)
                                                    {
                                                        //                                                      c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.25, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.9, 0.05, 1.0));
                                                    }
                                                }
                                                break;
                                            case BrightDim:
                                                {
                                                    /*
                                                    if(j == 0)
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.05, 0.0112, 1.0), VoxelLogic.Clamp(v * 1.1, 0.09, 1.0));
                                                    }
                                                    else
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.275, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.875, 0.045, 1.0));
                                                    }*/
                                                    c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp(s_alter * 1.275, 0.0112, 1.0), VoxelLogic.Clamp(v_alter * 0.875, 0.05, 1.0));

                                                }
                                                break;
                                            case BrightDimTop:
                                                {
                                                    //     if((i + j) / 2 >= 1 && i <= j / 2 && j > 0)
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp(s_alter * 1.1, 0.0112, 1.0), VoxelLogic.Clamp(v_alter * 1.05, 0.08, 1.0));
                                                    }
                                                    //   else // else if (j > 0)
                                                    //   {
                                                    //       c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.2, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.95, 0.08, 1.0));
                                                    //   }
                                                }
                                                break;

                                            case BrightBottom:
                                                {
                                                    if(i > (j + 1) / 2 + 2)
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp(s_alter * 1.35, 0.0112, 1.0), VoxelLogic.Clamp(v_alter * 0.8, 0.03, 1.0));
                                                    }
                                                    else if(i > (j + 1) / 2 + 1)
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp(s_alter * 1.4, 0.0112, 1.0), VoxelLogic.Clamp(v_alter * 0.75, 0.02, 1.0));
                                                    }
                                                }
                                                break;
                                            case DimBottom:
                                                {
                                                    if(i + (j + 1) / 2 < 1)
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp(s_alter * 1.2, 0.0112, 1.0), VoxelLogic.Clamp(v_alter * 0.95, 0.06, 1.0));
                                                    }
                                                    else if(i + (j + 1) / 2 < 2)
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp(s_alter * 1.5, 0.0112, 1.0), VoxelLogic.Clamp(v_alter * 0.65, 0.01, 1.0));
                                                    }
                                                }
                                                break;
                                            case BrightDimBottom:
                                                {
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp(s_alter * 1.45, 0.0112, 1.0), VoxelLogic.Clamp(v_alter * 0.7, 0.015, 1.0));
                                                    }
                                                }
                                                break;
                                            case BrightBack:
                                                {

                                                    if(i >= 2)
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp(s_alter * 1.1, 0.0112, 1.0), VoxelLogic.Clamp(v_alter * 1.1, 0.09, 1.0));
                                                    }
                                                }
                                                break;
                                            case DimBack:
                                                {

                                                    if(i < 2)
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp(s_alter * 1.1, 0.0112, 1.0), VoxelLogic.Clamp(v_alter * 1.1, 0.09, 1.0));
                                                    }
                                                }
                                                break;
                                            case BrightTopBack:
                                                {
                                                    if(i + (j + 1) / 2 > 3) // && i >= 2
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp(s_alter * 1.05, 0.0112, 1.0), VoxelLogic.Clamp(v_alter * 1.15, 0.09, 1.0));
                                                    }
                                                }
                                                break;
                                            case DimTopBack:
                                                {
                                                    if(i * 2 < j) // && i < 2)
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp(s_alter * 1.2, 0.0112, 1.0), VoxelLogic.Clamp(v_alter * 0.95, 0.09, 1.0));
                                                    }
                                                }
                                                break;
                                            case DimBottomBack:
                                            case BrightBottomBack:
                                            default:
                                                {

                                                }
                                                break;
                                        }
                                    }
                                }
                                else if(which_image == "shine")
                                {
                                    c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp(s * 0.9, 0.0112, 1.0), VoxelLogic.Clamp(v * 1.1, 0.1, 1.0));
                                }
                                else if(which_image == "flat")
                                {
                                    if(j >= 2 && j < height - 1)
                                    {
                                        double h2 = h, s2 = 1.0, v2 = v;
                                        VoxelLogic.ColorToHSV(c, out h2, out s2, out v2);
                                        c2 = VoxelLogic.ColorFromHSV(h2, VoxelLogic.Clamp(s2 * 0.65, 0.0112, 1.0), VoxelLogic.Clamp(v2 * 0.85, 0.01, 1.0));
                                    }
                                }
                                if(c2.A != 0)
                                {
                                    cubes[p, current_color, slp, i * 4 + j * width * 4 + 0] = Math.Max((byte)1, c2.B);
                                    cubes[p, current_color, slp, i * 4 + j * width * 4 + 1] = Math.Max((byte)1, c2.G);
                                    cubes[p, current_color, slp, i * 4 + j * width * 4 + 2] = Math.Max((byte)1, c2.R);
                                    cubes[p, current_color, slp, i * 4 + j * width * 4 + 3] = c2.A;
                                }
                                else
                                {
                                    cubes[p, current_color, slp, i * 4 + j * 4 * width + 0] = 0;
                                    cubes[p, current_color, slp, i * 4 + j * 4 * width + 1] = 0;
                                    cubes[p, current_color, slp, i * 4 + j * 4 * width + 2] = 0;
                                    cubes[p, current_color, slp, i * 4 + j * 4 * width + 3] = 0;
                                }
                            }
                        }
                    }
                }
            }
            byte[][][][] cubes2 = new byte[VoxelLogic.wpalettes.Length][][][];
            VoxelLogic.wrendered = new byte[VoxelLogic.wpalettes.Length][][];
            for(int i = 0; i < VoxelLogic.wpalettes.Length; i++)
            {
                cubes2[i] = new byte[VoxelLogic.wpalettes[0].Length][][];
                VoxelLogic.wrendered[i] = new byte[VoxelLogic.wpalettes[0].Length][];
                for(int c = 0; c < VoxelLogic.wpalettes[0].Length; c++)
                {
                    cubes2[i][c] = new byte[17][];
                    VoxelLogic.wrendered[i][c] = new byte[80];
                    for(int sp = 0; sp < 17; sp++)
                    {
                        cubes2[i][c][sp] = new byte[80];
                        for(int j = 0; j < 80; j++)
                        {
                            cubes2[i][c][sp][j] = cubes[i, c, sp, j];
                            if(sp == 0)
                            {
                                VoxelLogic.wrendered[i][c][j] = cubes[i, c, sp, j];
                            }
                        }
                    }
                }
            }
            return cubes2;
        }


        public static byte[][][][] wrendered;
        public static byte[][][] wcurrent;

        public static void InitializeWPalette()
        {
            wrendered = storeColorCubesWBold();
            wcurrent = wrendered[0];
        }
        public static Slope[] Slopes = new Slope[] {
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
        public static void processUnitLargeW(string u, int palette, bool still, bool shadowless)
        {

            Console.WriteLine("Processing: " + u + ", palette " + palette);
            
            BinaryReader bin = new BinaryReader(File.Open(u + "_Large_W.vox", FileMode.Open));
            List<MagicaVoxelData> voxes = VoxelLogic.FromMagicaRaw(bin); //VoxelLogic.PlaceShadowsW(
            Directory.CreateDirectory("vox/" + altFolder);
            VoxelLogic.WriteVOX("vox/" + altFolder + u + "_" + palette + ".vox", voxes, "W", palette, 40, 40, 40);
            MagicaVoxelData[] parsed = voxes.ToArray();
            for(int i = 0; i < parsed.Length; i++)
            {
                parsed[i].x += 10;
                parsed[i].y += 10;
                if((254 - parsed[i].color) % 4 == 0)
                    parsed[i].color--;
            }
            int framelimit = 4;


            string folder = (altFolder);//"color" + i;
            Directory.CreateDirectory(folder); //("color" + i);
            for(int dir = 0; dir < 4; dir++)
            {
                FaceVoxel[,,] faces = FaceLogic.GetFaces(FaceLogic.VoxListToArray(VoxelLogic.BasicRotateLarge(parsed, dir), 60, 60, 60, 153));
                for(int f = 0; f < framelimit; f++)
                {
                    Bitmap b = processFrameLargeW(faces, palette, dir, f, framelimit, still, shadowless);
                    b.Save(folder + "/palette" + palette + "_" + u + "_Large_face" + dir + "_" + f + ".png", ImageFormat.Png);
                    b.Dispose();
                }
            }


            Directory.CreateDirectory("gifs/" + altFolder);
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";

            s = folder + "/palette" + palette + "_" + u + "_Large_face* ";
            startInfo.Arguments = "-dispose background -delay 25 -loop 0 " + s + " gifs/" + altFolder + "palette" + palette + "_" + u + "_Large_animated.gif";
            Process.Start(startInfo).WaitForExit();
            
            //bin.Close();

            //            processFiringLarge(u);

            processExplosionLargeW(u, palette, shadowless);

        }
        public static void processUnitLargeWalkW(string u, int palette)
        {

            Console.WriteLine("Processing: " + u);
            BinaryReader bin = new BinaryReader(File.Open(u + "_Walk_0_Large_W.vox", FileMode.Open));
            MagicaVoxelData[][] parsed = new MagicaVoxelData[4][];
            parsed[0] = VoxelLogic.FromMagicaRaw(bin).ToArray();
            //bin.Close();

            bin = new BinaryReader(File.Open(u + "_Walk_1_Large_W.vox", FileMode.Open));
            parsed[1] = VoxelLogic.FromMagicaRaw(bin).ToArray();
            //bin.Close();

            bin = new BinaryReader(File.Open(u + "_Walk_2_Large_W.vox", FileMode.Open));
            parsed[2] = VoxelLogic.FromMagicaRaw(bin).ToArray();
            //bin.Close();

            bin = new BinaryReader(File.Open(u + "_Walk_3_Large_W.vox", FileMode.Open));
            parsed[3] = VoxelLogic.FromMagicaRaw(bin).ToArray();
            //bin.Close();

            for(int i = 0; i < parsed.Length; i++)
            {
                for(int j = 0; j < parsed[i].Length; j++)
                {
                    parsed[i][j].x += 10;
                    parsed[i][j].y += 10;

                }
            }
            FaceVoxel[][,,] faces = new FaceVoxel[4][,,];
            
            int framelimit = 8;


            string folder = (altFolder);//"color" + i;
            Directory.CreateDirectory(folder); //("color" + i);
            for(int dir = 0; dir < 4; dir++)
            {
                for(int i = 0; i < 4; i++)
                {
                    faces[i] = FaceLogic.GetFaces(FaceLogic.VoxListToArray(VoxelLogic.BasicRotateLarge(parsed[i], dir), 60, 60, 60, 153));
                }
                for(int f = 0; f < framelimit; f++)
            { //
                    Bitmap b = processFrameLargeW(faces[f % 4], palette, dir, f, framelimit, true, false);
                    b.Save(folder + "/palette" + palette + "_" + u + "_Walk_Large_face" + dir + "_" + f + ".png", ImageFormat.Png);
                    b.Dispose();
                }
            }


            Directory.CreateDirectory("gifs/" + altFolder);
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";
            for(int dir = 0; dir < 4; dir++)
            {
                for(int i = 0; i < framelimit; i++)
                {
                    s += folder + "/palette" + palette + "_" + u + "_Walk_Large_face" + dir + "_" + i + ".png ";
                }
            }
            startInfo.Arguments = "-dispose background -delay 25 -loop 0 " + s + " gifs/" + altFolder + "palette" + palette + "_" + u + "_Walk_Large_animated.gif";
            Process.Start(startInfo).WaitForExit();


            //            processFiringDouble(u);

            //            processExplosionDouble(u);

        }

        private static Bitmap processFrameLargeW(FaceVoxel[,,] parsed, int palette, int dir, int frame, int maxFrames, bool still, bool shadowless)
        {
            Bitmap b;
            Bitmap b2 = new Bitmap(88, 108, PixelFormat.Format32bppArgb);

            VoxelLogic.wcolors = VoxelLogic.wpalettes[palette];
            wcurrent = wrendered[palette];
            b = renderWSmart(parsed, dir, palette, frame, maxFrames, still, shadowless);
            //            return b;

            Graphics g2 = Graphics.FromImage(b2);
            g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g2.DrawImage(b.Clone(new Rectangle(32, 46 + 32, 88 * 2, 108 * 2), b.PixelFormat), 0, 0, 88, 108);
            g2.Dispose();
            return b2;

            /*string folder = "palette" + palette + "_big";
            System.IO.Directory.CreateDirectory(folder);
            b.Save(folder + "/" + (System.IO.Directory.GetFiles(folder).Length) + "_Gigantic_face" + dir + "_" + frame + ".png", ImageFormat.Png); g = Graphics.FromImage(b);
            */
        }
        public static void processUnitHugeW(string u, int palette, bool still, bool shadowless)
        {

            Console.WriteLine("Processing: " + u + ", palette " + palette);
            BinaryReader bin = new BinaryReader(File.Open(u + "_Huge_W.vox", FileMode.Open));
            List<MagicaVoxelData> voxes = VoxelLogic.FromMagicaRaw(bin);
            Directory.CreateDirectory("vox/" + altFolder);
            VoxelLogic.WriteVOX("vox/" + altFolder + u + "_" + palette + ".vox", voxes, "W", palette, 80, 80, 80);
            MagicaVoxelData[] parsed = voxes.ToArray();
            for(int i = 0; i < parsed.Length; i++)
            {
                parsed[i].x += 20;
                parsed[i].y += 20;
                if((254 - parsed[i].color) % 4 == 0)
                    parsed[i].color--;
            }
            int framelimit = 4;


            string folder = (altFolder);//"color" + i;
            Directory.CreateDirectory(folder); //("color" + i);

            for(int dir = 0; dir < 4; dir++)
            {
                FaceVoxel[,,] faces = FaceLogic.GetFaces(FaceLogic.VoxListToArray(VoxelLogic.BasicRotateHuge(parsed, dir), 80, 80, 80, 153));
                for(int f = 0; f < framelimit; f++)
                {
                    Bitmap b = processFrameHugeW(faces, palette, dir, f, framelimit, still, shadowless);
                    b.Save(folder + "/palette" + palette + "_" + u + "_Huge_face" + dir + "_" + f + ".png", ImageFormat.Png);
                    b.Dispose();
                }
            }


            Directory.CreateDirectory("gifs/" + altFolder);
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";

            s = folder + "/palette" + palette + "_" + u + "_Huge_face* ";
            startInfo.Arguments = "-dispose background -delay 25 -loop 0 " + s + " gifs/" + altFolder + "palette" + palette + "_" + u + "_Huge_animated.gif";
            Process.Start(startInfo).WaitForExit();

            //bin.Close();

            //            processFiringHuge(u);

            processExplosionHugeW(u, palette);

        }
        public static void processTerrainHugeW(string u, int palette, bool shadowless, bool addFloor)
        {

            Console.WriteLine("Processing: " + u + ", palette " + palette);
            List<MagicaVoxelData> voxes;
            if(addFloor)
            {
                BinaryReader bin = new BinaryReader(File.Open("Terrain/" + u + "_Huge_W.vox", FileMode.Open));
                BinaryReader binFloor = new BinaryReader(File.Open("Terrain/Floor_Huge_W.vox", FileMode.Open));
                IEnumerable<MagicaVoxelData> structure = VoxelLogic.FromMagicaRaw(bin)
                    .Select(v => VoxelLogic.AlterVoxel(v, 0, 0, 1, v.color));
                voxes = structure.Concat(VoxelLogic.FromMagicaRaw(binFloor)).ToList();
            }
            else
            {
                BinaryReader bin = new BinaryReader(File.Open("Terrain/" + u + "_Huge_W.vox", FileMode.Open));
                if(shadowless)
                    voxes = VoxelLogic.FromMagicaRaw(bin);
                else
                    voxes = VoxelLogic.PlaceShadowsW(VoxelLogic.FromMagicaRaw(bin));
            }
            VoxelLogic.WriteVOX("vox/" + altFolder + u + ".vox", voxes, "W", palette, 80, 80, 80);
            MagicaVoxelData[] parsed = voxes.ToArray();
            for(int i = 0; i < parsed.Length; i++)
            {
                parsed[i].x += 20;
                parsed[i].y += 20;
                if((254 - parsed[i].color) % 4 == 0)
                    parsed[i].color--;
            }
            int framelimit = 4;


            string folder = (altFolder);//"color" + i;
            Directory.CreateDirectory(folder); //("color" + i);

            for(int dir = 0; dir < 4; dir++)
            {
                FaceVoxel[,,] faces = FaceLogic.GetFaces(FaceLogic.VoxListToArray(VoxelLogic.BasicRotateHuge(parsed, dir), 120, 120, 80, 153));
                for(int f = 0; f < framelimit; f++)
                {
                    Bitmap b = processFrameHugeW(faces, palette, dir, f, framelimit, true, shadowless);
                    b.Save(folder + "/palette" + palette + "_" + u + "_Huge_face" + dir + "_" + f + ".png", ImageFormat.Png);
                    b.Dispose();
                }
            }
        }

        public static void processUnitLargeWMilitary(string u)
        {
            BinaryReader bin = new BinaryReader(File.Open("CU2/" + u + "_Large_W.vox", FileMode.Open));
            List<MagicaVoxelData> voxes = VoxelLogic.AssembleHeadToModelW(bin); //VoxelLogic.PlaceShadowsW(
            Directory.CreateDirectory("vox/" + altFolder);
            VoxelLogic.WriteVOX("vox/" + altFolder + u + "_0.vox", voxes, "W", 0, 40, 40, 40);
            MagicaVoxelData[] parsed = voxes.ToArray(), explode_parsed = voxes.ToArray();
            for(int i = 0; i < parsed.Length; i++)
            {
                parsed[i].x += 10;
                parsed[i].y += 10;
                if((254 - parsed[i].color) % 4 == 0)
                    parsed[i].color--;
            }
            int framelimit = 4;


            string folder = (altFolder);//"color" + i;
            Directory.CreateDirectory(folder); //("color" + i);

            for(int palette = 0; palette < 8; palette++)
            {
                for(int dir = 0; dir < 4; dir++)
                {
                    FaceVoxel[,,] faces = FaceLogic.GetFaces(FaceLogic.VoxListToArray(VoxelLogic.BasicRotateLarge(parsed, dir), 60, 60, 60, 153));

                    Console.WriteLine("Processing: " + u + ", palette " + palette);
                    for(int f = 0; f < framelimit; f++)
                    { //
                        Bitmap b = processFrameLargeW(faces, palette, dir, f, framelimit, (VoxelLogic.CurrentMobilities[VoxelLogic.UnitLookup[u]] != MovementType.Flight), false);
                        b.Save(folder + "/palette" + palette + "_" + u + "_Large_face" + dir + "_" + f + ".png", ImageFormat.Png);
                        b.Dispose();
                    }
                }


                Directory.CreateDirectory("gifs/" + altFolder);
                ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
                startInfo.UseShellExecute = false;
                string s = "";

                s = folder + "/palette" + palette + "_" + u + "_Large_face* ";
                startInfo.Arguments = "-dispose background -delay 25 -loop 0 " + s + " gifs/" + altFolder + "palette" + palette + "_" + u + "_Large_animated.gif";
                Process.Start(startInfo).WaitForExit();

                //bin.Close();

                processExplosionLargeW(u, palette, explode_parsed, false);
            }

            processUnitLargeWFiring(u);
        }
        public static void processUnitLargeWFiring(string u)
        {
            Console.WriteLine("Processing: " + u);
            string filename = "CU2/" + u + "_Large_W.vox";
            BinaryReader bin;
            MagicaVoxelData[] parsed;
            string folder = ("frames");

            for(int w = 0; w < 2; w++)
            {
                if((w == 0 && u == "Infantry" || u == "Tank_S") || (w == 1 && (u == "Infantry_P" || u == "Infantry_T" || u == "Infantry_PT")))
                {
                    filename = "CU2/" + u + "_Firing_Large_W.vox";
                }
                if(VoxelLogic.CurrentWeapons[VoxelLogic.UnitLookup[u]][w] == 7)
                {
                    bin = new BinaryReader(File.Open(filename, FileMode.Open));
                    parsed = VoxelLogic.FromMagicaRaw(bin).ToArray();
                    MagicaVoxelData[][] flying = CURedux.Flyover(parsed);
                    MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[16][];
                    //voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
                    for(int i = 0; i < 16; i++)
                    {
                        voxelFrames[i] = new MagicaVoxelData[flying[i].Length];
                        flying[i].CopyTo(voxelFrames[i], 0);
                    }
                    /*                    for (int i = 0; i < flying[4].Length; i++)
                                        {
                                            voxelFrames[0][i].x += 20;
                                            voxelFrames[0][i].y += 20;
                                        }*/

                    voxelFrames = CURedux.weaponAnimationsLarge[VoxelLogic.CurrentWeapons[VoxelLogic.UnitLookup[u]][w]](voxelFrames, VoxelLogic.UnitLookup[u], w);

                    for(int f = 0; f < 16; f++)
                    {
                        List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxelFrames[f].Length);
                        int[,] taken = new int[120, 120];
                        taken.Fill(-1);
                        for(int i = 0; i < voxelFrames[f].Length; i++)
                        {
                            // do not store this voxel if it lies out of range of the voxel chunk (30x30x30)
                            if(voxelFrames[f][i].x >= 120 || voxelFrames[f][i].y >= 120 || voxelFrames[f][i].z >= 120)
                            {
                                //Console.Write("Voxel out of bounds: " + voxelFrames[f][i].x + ", " + voxelFrames[f][i].y + ", " + voxelFrames[f][i].z);
                                continue;
                            }
                            altered.Add(voxelFrames[f][i]);
                        }
                        flying[f] = altered.ToArray();
                    }
                    Directory.CreateDirectory(folder); //("color" + i);
                    for(int d = 0; d < 4; d++)
                    {
                        for(int frame = 0; frame < 16; frame++)
                        {
                            FaceVoxel[,,] faces = FaceLogic.GetFaces(FaceLogic.VoxListToArray(VoxelLogic.BasicRotateHuge(flying[frame], d), 120, 120, 80, 153));

                            for(int color = 0; color < 8; color++)
                            {
                                Bitmap b = processFrameHugeW(faces, color, d, frame, 16, true, false);
                                b.Save(folder + "/color" + color + "_" + u + "_Large_face" + d + "_attack_" + w + "_" + (frame) + ".png", ImageFormat.Png);
                                b.Dispose();
                            }
                        }
                    }

                    //bin.Close();
                }
                else if(VoxelLogic.CurrentWeapons[VoxelLogic.UnitLookup[u]][w] != -1)
                {
                    bin = new BinaryReader(File.Open(filename, FileMode.Open));
                    parsed = VoxelLogic.AssembleHeadToModelW(bin).ToArray();
                    MagicaVoxelData[][] firing = CURedux.makeFiringAnimationLarge(parsed, VoxelLogic.UnitLookup[u], w);
                    for(int color = 0; color < 8; color++)
                    {
                        for(int d = 0; d < 4; d++)
                        {
                            Directory.CreateDirectory(folder); //("color" + i);

                            for(int frame = 0; frame < 16; frame++)
                            {
                                FaceVoxel[,,] faces = FaceLogic.GetFaces(FaceLogic.VoxListToArray(VoxelLogic.BasicRotateHuge(firing[frame], d), 120, 120, 80, 153));

                                Bitmap b = processFrameHugeW(faces, color, d, frame, 16, true, false);
                                b.Save(folder + "/color" + color + "_" + u + "_Large_face" + d + "_attack_" + w + "_" + (frame) + ".png", ImageFormat.Png);
                                b.Dispose();
                            }
                        }
                    }
                    //bin.Close();
                }
                else continue;

                Directory.CreateDirectory("gifs/" + altFolder);
                ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
                startInfo.UseShellExecute = false;
                string s = "";
                for(int i = 0; i < 8; i++)
                {
                    for(int d = 0; d < 4; d++)
                    {
                        for(int frame = 0; frame < 16; frame++)
                        {
                            s += folder + "/color" + i + "_" + u + "_Large_face" + d + "_attack_" + w + "_" + frame + ".png ";
                        }
                    }
                }
                startInfo.Arguments = "-dispose background -delay 11 -loop 0 " + s + " gifs/" + altFolder + u + "_attack_" + w + "_animated.gif";
                Console.WriteLine("Running convert.exe ...");
                Process.Start(startInfo).WaitForExit();

            }

        }


        public static void processHatLargeW(string u, int palette, string hat)
        {

            Console.WriteLine("Processing: " + hat);
            BinaryReader bin;
            int framelimit = 4;

            string folder = (altFolder);
            Directory.CreateDirectory(folder);

            bin = new BinaryReader(File.Open(hat + "_Hat_W.vox", FileMode.Open));
            List<MagicaVoxelData> vlist = VoxelLogic.FromMagicaRaw(bin);
            Directory.CreateDirectory("vox/" + altFolder);
            VoxelLogic.WriteVOX("vox/" + altFolder + hat + "_" + palette + ".vox", vlist, "W", palette, 40, 40, 40);
            MagicaVoxelData[] parsed = vlist.ToArray();
            for(int i = 0; i < parsed.Length; i++)
            {
                parsed[i].x += 10;
                parsed[i].y += 10;
                if((254 - parsed[i].color) % 4 == 0)
                    parsed[i].color = VoxelLogic.clear;
            }

            for(int dir = 0; dir < 4; dir++)
            {
                FaceVoxel[,,] faces = FaceLogic.GetFaces(FaceLogic.VoxListToArray(parsed.ToList(), 60, 60, 60, 153));

                for(int f = 0; f < framelimit; f++)
                { //
                    Bitmap b = processFrameLargeW(faces, palette, dir, f, framelimit, true, true);
                    b.Save(folder + "/" + "palette" + palette + "_" + hat + "_Hat_face" + dir + "_" + f + ".png", ImageFormat.Png);
                    b.Dispose();
                }
            }

        }

        public static void processUnitHugeWalkW(string u, int palette)
        {

            Console.WriteLine("Processing: " + u);
            BinaryReader bin = new BinaryReader(File.Open(u + "_Walk_0_Huge_W.vox", FileMode.Open));
            MagicaVoxelData[][] parsed = new MagicaVoxelData[4][];
            parsed[0] = VoxelLogic.FromMagicaRaw(bin).ToArray();
            //bin.Close();

            bin = new BinaryReader(File.Open(u + "_Walk_1_Huge_W.vox", FileMode.Open));
            parsed[1] = VoxelLogic.FromMagicaRaw(bin).ToArray();
            //bin.Close();

            bin = new BinaryReader(File.Open(u + "_Walk_2_Huge_W.vox", FileMode.Open));
            parsed[2] = VoxelLogic.FromMagicaRaw(bin).ToArray();
            //bin.Close();

            bin = new BinaryReader(File.Open(u + "_Walk_3_Huge_W.vox", FileMode.Open));
            parsed[3] = VoxelLogic.FromMagicaRaw(bin).ToArray();
            //bin.Close();

            for(int i = 0; i < parsed.Length; i++)
            {
                for(int j = 0; j < parsed[i].Length; j++)
                {
                    parsed[i][j].x += 20;
                    parsed[i][j].y += 20;

                }
            }
            int framelimit = 8;

            FaceVoxel[][,,] faces = new FaceVoxel[4][,,];


            string folder = (altFolder);//"color" + i;
            Directory.CreateDirectory(folder); //("color" + i);
            for(int dir = 0; dir < 4; dir++)
            {
                for(int i = 0; i < 4; i++)
                {
                    faces[i] = FaceLogic.GetFaces(FaceLogic.VoxListToArray(VoxelLogic.BasicRotateHuge(parsed[i], dir), 80, 80, 80, 153));
                }
                for(int f = 0; f < framelimit; f++)
                {
                    Bitmap b = processFrameHugeW(faces[f % 4], palette, dir, f, framelimit, true, false);
                    b.Save(folder + "/palette" + palette + "_" + u + "_Walk_Huge_face" + dir + "_" + f + ".png", ImageFormat.Png);
                    b.Dispose();
                }
            }


            Directory.CreateDirectory("gifs/" + altFolder);
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";
            for(int dir = 0; dir < 4; dir++)
            {
                for(int i = 0; i < framelimit; i++)
                {
                    s += folder + "/palette" + palette + "_" + u + "_Walk_Huge_face" + dir + "_" + i + ".png ";
                }
            }
            startInfo.Arguments = "-dispose background -delay 25 -loop 0 " + s + " gifs/" + altFolder + "palette" + palette + "_" + u + "_Walk_Huge_animated.gif";
            Process.Start(startInfo).WaitForExit();


            //            processFiringDouble(u);

            //            processExplosionDouble(u);

        }

        private static Bitmap processFrameHugeW(FaceVoxel[,,] faces, int palette, int dir, int frame, int maxFrames, bool still, bool shadowless)
        {
            Bitmap b;
            Bitmap b2 = new Bitmap(168, 208, PixelFormat.Format32bppArgb);

            VoxelLogic.wcolors = VoxelLogic.wpalettes[palette];
            wcurrent = wrendered[palette];
            b = renderWSmartHuge(faces, dir, palette, frame, maxFrames, still, shadowless);
            //            return b;

            Graphics g2 = Graphics.FromImage(b2);
            g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g2.DrawImage(b.Clone(new Rectangle(0, 0, 248 * 2, 308 * 2), b.PixelFormat), -40, -80, 248, 308);
            g2.Dispose();
            return b2;

            /*string folder = "palette" + palette + "_big";
            System.IO.Directory.CreateDirectory(folder);
            b.Save(folder + "/" + (System.IO.Directory.GetFiles(folder).Length) + "_Gigantic_face" + dir + "_" + frame + ".png", ImageFormat.Png); g = Graphics.FromImage(b);
            */
        }
        public static void processUnitLargeWHat(string u, int palette, bool still, string hat)
        {
            /*          if (render_hat_gifs)
                        {
                            processUnitOutlinedWDoubleHatModel(u, palette, still, hat);
                            return;
                        }*/
            Console.WriteLine("Processing: " + u + " " + hat);

            BinaryReader bin = new BinaryReader(File.Open(u + "_Large_W.vox", FileMode.Open));
            MagicaVoxelData[] headpoints = VoxelLogic.GetHeadVoxels(bin, hat).ToArray();

            int framelimit = 4;

            string folder = (altFolder);//"color" + i;
            Directory.CreateDirectory(folder); //("color" + i);

            Bitmap bmp = new Bitmap(248, 308, PixelFormat.Format32bppArgb);

            PixelFormat pxf = PixelFormat.Format32bppArgb;

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);
            int stride = bmpData.Stride;
            bmp.UnlockBits(bmpData);
            bmp.Dispose();

            for(int dir = 0; dir < 4; dir++)
            {
                for(int f = 0; f < framelimit; f++)
                {

                    int jitter = (((f % 4) % 3) + ((f % 4) / 3)) * 2;
                    if(framelimit >= 8) jitter = ((f % 8 > 4) ? 4 - ((f % 8) ^ 4) : f % 8);
                    int body_coord = voxelToPixelLargeW(0, 0, headpoints[0 + dir * 2].x, headpoints[0 + dir * 2].y, headpoints[0 + dir * 2].z, (byte)(253 - headpoints[0 + dir * 2].color) / 4, stride, jitter, still) / 4;
                    //model_headpoints.AppendLine("BODY: " + u + "_" + hat + " facing " + dir + " frame " + f + ": x " +
                    //    ((body_coord % (stride / 4) - 32) / 2) + ", y " + (108 - ((body_coord / (stride / 4) - 78) / 2)));
                    int hat_coord = voxelToPixelLargeW(0, 0, headpoints[1 + dir * 2].x, headpoints[1 + dir * 2].y, headpoints[1 + dir * 2].z, (byte)(253 - headpoints[1 + dir * 2].color) / 4, stride, 0, true) / 4;
                    //hat_headpoints.AppendLine("HAT: " + u + "_" + hat + " facing " + dir + " frame " + f + ": x " +
                    //    ((hat_coord % (stride / 4) - 32) / 2) + ", y " + (108 - ((hat_coord / (stride / 4) - 78) / 2)));

                    Graphics hat_graphics;
                    Bitmap hat_image = new Bitmap(Image.FromFile(altFolder + "palette" + ((hat == "Woodsman") ? 44 : (hat == "Farmer") ? 49 : (palette == 7 || palette == 8 || palette == 42) ? 7 : 0) + "_"
                        // + ((palette == 7 || palette == 8 || palette == 42) ? "Spirit_" : "Generic_Male_")
                        + hat + "_Hat_face" + dir + "_" + ((hat == "Farmer") ? 0 : f) + ".png"));
                    Bitmap body_image = new Bitmap(Image.FromFile(altFolder + "palette" + palette + "_" + u + "_Large_face" + dir + "_" + f + ".png"));

                    VoxelLogic.wcolors = VoxelLogic.wpalettes[palette];
                    wcurrent = wrendered[palette];
                    hat_graphics = Graphics.FromImage(hat_image);
                    Graphics body_graphics = Graphics.FromImage(body_image);
                    body_graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    body_graphics.DrawImage(hat_image, (((body_coord % (stride / 4) - 32) / 2) - ((hat_coord % (stride / 4) - 32) / 2)),
                         (((body_coord / (stride / 4) - 78) / 2) - ((hat_coord / (stride / 4) - 78) / 2)), 88, 108);

                    offsets.Write(((body_coord % (stride / 4) - 32) / 2) - ((hat_coord % (stride / 4) - 32) / 2));
                    offsets.Write(((body_coord / (stride / 4) - 78) / 2) - ((hat_coord / (stride / 4) - 78) / 2));
                    //                    model_headpoints.AppendLine("BODY: " + u + "_" + hat + " facing " + dir + " frame " + f + ": x " +
                    //                        (((body_coord % (stride / 4) - 32) / 2) - ((hat_coord % (stride / 4) - 32) / 2))
                    //                        + ", y " + (((body_coord / (stride / 4) - 78) / 2) - ((hat_coord / (stride / 4) - 78) / 2)));

                    body_graphics.Dispose();
                    body_image.Save(altFolder + "/palette" + palette + "_" + u + "_" + hat + "_Large_face" + dir + "_" + f + ".png");
                    hat_graphics.Dispose();
                    hat_image.Dispose();
                    body_image.Dispose();
                }
            }

            System.IO.Directory.CreateDirectory("gifs");
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";

            s = altFolder + "/palette" + palette + "_" + u + "_" + hat + "_Large_face* ";
            startInfo.Arguments = "-dispose background -delay 25 -loop 0 " + s + " gifs/" + altFolder + "palette" + palette + "_" + u + "_" + hat + "_Large_animated.gif";
            Process.Start(startInfo).WaitForExit();

            //bin.Close();

            //            processFiringDouble(u);

            processExplosionLargeWHat(u, palette, hat, headpoints);

            /*
            bin = new BinaryReader(File.Open(hat + "_Hat_W.vox", FileMode.Open));
            MagicaVoxelData[] parsed = VoxelLogic.FromMagicaRaw(bin).ToArray();
            for (int i = 0; i < parsed.Length; i++)
            {
                parsed[i].x += 10;
                parsed[i].y += 10;
                if ((254 - parsed[i].color) % 4 == 0)
                    parsed[i].color = VoxelLogic.clear;
            }

            for (int f = 0; f < framelimit; f++)
            { //
                for (int dir = 0; dir < 4; dir++)
                {
                    Bitmap b = processSingleOutlinedWDouble(parsed, palette, dir, f, framelimit, true);
                    b.Save(folder + "/" + u + "_" + hat + "_Hat_face" + dir + "_" + f + ".png", ImageFormat.Png);
                    b.Dispose();
                }
            }
            */
        }

        public static void processUnitLargeDeadW(string u, int palette, bool still)
        {

            Console.WriteLine("Processing: " + u + " Dead");
            BinaryReader bin = new BinaryReader(File.Open(u + "_Dead_Large_W.vox", FileMode.Open));
            List<MagicaVoxelData> vlist = VoxelLogic.PlaceBloodPoolW(VoxelLogic.FromMagicaRaw(bin));
            Directory.CreateDirectory("vox/" + altFolder);
            VoxelLogic.WriteVOX("vox/" + altFolder + u + "_Dead_" + palette + ".vox", vlist, "W", palette, 40, 40, 40);
            MagicaVoxelData[] parsed = vlist.ToArray();
            for(int i = 0; i < parsed.Length; i++)
            {
                parsed[i].x += 10;
                parsed[i].y += 10;
                if((254 - parsed[i].color) % 4 == 0)
                    parsed[i].color--;
            }
            int framelimit = 4;


            string folder = (altFolder + "palette" + palette);//"color" + i;
            Directory.CreateDirectory(folder); //("color" + i);

            for(int dir = 0; dir < 4; dir++)
            {
                FaceVoxel[,,] faces = FaceLogic.GetFaces(FaceLogic.VoxListToArray(VoxelLogic.BasicRotateLarge(parsed, dir), 60, 60, 60, 153));
                for(int f = 0; f < framelimit; f++)
                {
                    Bitmap b = processFrameLargeW(faces, palette, dir, f, framelimit, still, false);
                    b.Save(folder + "/palette" + palette + "_" + u + "_Dead_Large_face" + dir + "_" + f + ".png", ImageFormat.Png); //se
                    b.Dispose();
                }
            }


            Directory.CreateDirectory("gifs/" + altFolder);
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";

            s = folder + "/palette" + palette + "_" + u + "_Dead_Large_face* ";
            startInfo.Arguments = "-dispose background -delay 25 -loop 0 " + s + " gifs/" + altFolder + "palette" + palette + "_" + u + "_Dead_Large_animated.gif";
            Process.Start(startInfo).WaitForExit();

            //bin.Close();

            //            processFiringDouble(u);

            //processFieryExplosionDoubleW(u, palette);

        }

        private static StringBuilder model_headpoints = new StringBuilder(), hat_headpoints = new StringBuilder();


        private static void processHats(string u, int palette, bool hover, string[] classes)
        {

            processUnitLargeW(u, palette, hover, false);
            //processUnitOutlinedWDoubleDead(u, palette, hover);
            foreach(string s in classes)
            {
                processUnitLargeWHat(u, palette, hover, s);
            }

            // /* REMOVE COMMENT
            string doc = File.ReadAllText("TemplateStill.html");
            string html = String.Format(doc, palette, u);


            string doc2 = File.ReadAllText("TemplateGif.html");
            string html2 = String.Format(doc2, palette, u);
            System.IO.Directory.CreateDirectory(altFolder + "html");
            File.WriteAllText(altFolder + "html/" + u + "_still.html", html);
            File.WriteAllText(altFolder + "html/" + u + ".html", html2);
            // */

        }

        private static void processHats(string u, int palette, bool hover, string[] classes, string alternateName)
        {

            processUnitLargeW(u, palette, hover, false);
            processUnitLargeDeadW(u, palette, hover);
            foreach(string s in classes)
            {
                processUnitLargeWHat(u, palette, hover, s);
            }
            // /* REMOVE COMMENT
            string doc = File.ReadAllText("LivingTemplateStill.html");
            string html = String.Format(doc, palette, u);

            string doc2 = File.ReadAllText("LivingTemplateGif.html");
            string html2 = String.Format(doc2, palette, u);
            System.IO.Directory.CreateDirectory(altFolder + "html");
            File.WriteAllText(altFolder + "html/" + alternateName + "_still.html", html);
            File.WriteAllText(altFolder + "html/" + alternateName + ".html", html2);
            // */
        }

        public static void processExplosionLargeW(string u, int palette, bool shadowless)
        {
            Console.WriteLine("Processing: " + u);
            BinaryReader bin = new BinaryReader(File.Open(u + "_Large_W.vox", FileMode.Open));
            MagicaVoxelData[] parsed = VoxelLogic.FromMagicaRaw(bin).ToArray();
            //renderLarge(parsed, 0, 0, 0)[0].Save("junk_" + u + ".png");
            VoxelLogic.wcolors = VoxelLogic.wpalettes[palette];
            wcurrent = wrendered[palette];
            // MagicaVoxelData[][] explode = VoxelLogic.FieryExplosionDoubleW(parsed, false, true); //((CurrentMobilities[UnitLookup[u]] == MovementType.Immobile) ? false : true)
            string folder = ("frames");
            Directory.CreateDirectory(folder); //("color" + i);

            for(int d = 0; d < 4; d++)
            {
                //FaceVoxel[,,] faces = FaceLogic.GetFaces(FaceLogic.VoxListToArray(VoxelLogic.BasicRotateLarge(parsed, d), 60, 60, 60, 153));

                FaceVoxel[][,,] explode = FaceLogic.FieryExplosionLargeW(FaceLogic.GetFaces(FaceLogic.VoxListToArray(VoxelLogic.BasicRotateLarge(parsed, d), 60, 60, 60, 153)), false, false);

                for(int frame = 0; frame < 12; frame++)
                {
                    Bitmap b = renderWSmartHuge(explode[frame], d, palette, frame, 8, true, shadowless);
                    /*                    string folder2 = "palette" + palette + "_big";
                                        System.IO.Directory.CreateDirectory(folder2);
                                        b.Save(folder + "/" + (System.IO.Directory.GetFiles(folder2).Length) + "_Gigantic_face" + d + "_" + frame + ".png", ImageFormat.Png);
                    */
                    Bitmap b2 = new Bitmap(248, 308, PixelFormat.Format32bppArgb);


                    //                        b.Save("temp.png", ImageFormat.Png);
                    Graphics g2 = Graphics.FromImage(b2);
                    g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    Bitmap b3 = b.Clone(new Rectangle(0, 0, 248 * 2, 308 * 2), b.PixelFormat);
                    b.Dispose();
                    g2.DrawImage(b3, 0, 0, 248, 308);

                    b2.Save(folder + "/palette" + palette + "_" + u + "_Large_face" + d + "_fiery_explode_" + frame + ".png", ImageFormat.Png);
                    b2.Dispose();
                    g2.Dispose();
                    b3.Dispose();
                }
            }


            Directory.CreateDirectory("gifs/" + altFolder);
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";

            for(int d = 0; d < 4; d++)
            {
                for(int frame = 0; frame < 12; frame++)
                {
                    s += folder + "/palette" + palette + "_" + u + "_Large_face" + d + "_fiery_explode_" + frame + ".png ";
                }
            }

            startInfo.Arguments = "-dispose background -delay 11 -loop 0 " + s + " gifs/" + altFolder + "palette" + palette + "_" + u + "_explosion_animated.gif";
            Console.WriteLine("Running convert.exe ...");
            Process.Start(startInfo).WaitForExit();

            //bin.Close();
        }
        public static void processExplosionLargeW(string u, int palette, MagicaVoxelData[] voxels, bool shadowless)
        {
            Console.WriteLine("Processing: " + u);
//            BinaryReader bin = new BinaryReader(File.Open(u + "_Large_W.vox", FileMode.Open));
            //renderLarge(parsed, 0, 0, 0)[0].Save("junk_" + u + ".png");
            VoxelLogic.wcolors = VoxelLogic.wpalettes[palette];
            wcurrent = wrendered[palette];
            //MagicaVoxelData[][] explode = VoxelLogic.FieryExplosionDoubleW(parsed, false, true); //((CurrentMobilities[UnitLookup[u]] == MovementType.Immobile) ? false : true)
            string folder = ("frames");
            Directory.CreateDirectory(folder); //("color" + i);

            for(int d = 0; d < 4; d++)
            {
                
                FaceVoxel[,,] faces = FaceLogic.GetFaces(FaceLogic.VoxListToArray(VoxelLogic.BasicRotateLarge(voxels, d), 60, 60, 60, 153));

                FaceVoxel[][,,] explode = FaceLogic.FieryExplosionLargeW(faces, false, false);
                for(int frame = 0; frame < 12; frame++)
                {
                    Bitmap b = renderWSmartHuge(explode[frame], d, palette, frame, 8, true, shadowless);
                    /*                    string folder2 = "palette" + palette + "_big";
                                        System.IO.Directory.CreateDirectory(folder2);
                                        b.Save(folder + "/" + (System.IO.Directory.GetFiles(folder2).Length) + "_Gigantic_face" + d + "_" + frame + ".png", ImageFormat.Png);
                    */
                    Bitmap b2 = new Bitmap(248, 308, PixelFormat.Format32bppArgb);


                    //                        b.Save("temp.png", ImageFormat.Png);
                    Graphics g2 = Graphics.FromImage(b2);
                    g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    Bitmap b3 = b.Clone(new Rectangle(0, 0, 248 * 2, 308 * 2), b.PixelFormat);
                    b.Dispose();
                    g2.DrawImage(b3, 0, 0, 248, 308);

                    b2.Save(folder + "/palette" + palette + "_" + u + "_Large_face" + d + "_fiery_explode_" + frame + ".png", ImageFormat.Png);
                    b2.Dispose();
                    g2.Dispose();
                    b3.Dispose();
                }
            }


            Directory.CreateDirectory("gifs/" + altFolder);
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";

            for(int d = 0; d < 4; d++)
            {
                for(int frame = 0; frame < 12; frame++)
                {
                    s += folder + "/palette" + palette + "_" + u + "_Large_face" + d + "_fiery_explode_" + frame + ".png ";
                }
            }

            startInfo.Arguments = "-dispose background -delay 11 -loop 0 " + s + " gifs/" + altFolder + "palette" + palette + "_" + u + "_explosion_animated.gif";
            Console.WriteLine("Running convert.exe ...");
            Process.Start(startInfo).WaitForExit();

            //bin.Close();
        }

        public static void processExplosionLargeWHat(string u, int palette, string hat, MagicaVoxelData[] headpoints)
        {
            Console.WriteLine("Processing: " + u + " " + hat + ", palette " + palette);
            Stream body = File.Open(u + "_Large_W.vox", FileMode.Open);
            BinaryReader bin = new BinaryReader(body);
            int framelimit = 12;

            string folder = ("frames");//"color" + i;
            Directory.CreateDirectory(folder); //("color" + i);
            Bitmap bmp = new Bitmap(248, 308, PixelFormat.Format32bppArgb);

            PixelFormat pxf = PixelFormat.Format32bppArgb;

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);
            int stride = bmpData.Stride;
            bmp.UnlockBits(bmpData);
            Bitmap hat_image = new Bitmap(bmp);
            Bitmap body_image = new Bitmap(bmp);
            Graphics body_graphics = Graphics.FromImage(body_image);
            Graphics hat_graphics = Graphics.FromImage(hat_image);
            for(int dir = 0; dir < 4; dir++)
            {
                int minimum_z = headpoints[0 + dir * 2].z + 16;

                for(int f = 0; f < framelimit; f++)
                {

                    int jitter = (((f % 4) % 3) + ((f % 4) / 3)) * 2;
                    if(framelimit >= 8) jitter = ((f % 8 > 4) ? 4 - ((f % 8) ^ 4) : f % 8);
                    minimum_z += (10 - ((f * f > 22) ? 22 : f * f)) * 7 / 5;
                    if(minimum_z <= 0) minimum_z = 0;
                    int body_coord = voxelToPixelHugeW(0, 0, headpoints[0 + dir * 2].x, headpoints[0 + dir * 2].y + ((minimum_z == 0) ? 0 : (jitter - 2) * 2),
                        minimum_z, (byte)(253 - headpoints[0 + dir * 2].color) / 4, stride, jitter, true) / 4;
                    //     model_headpoints.AppendLine("EXPLODE: " + u + "_" + hat + " facing " + dir + " frame " + f + ": x " +
                    //         ((body_coord % (stride / 4) - 32) / 2 + 80) + ", y " + (308 - ((body_coord / (stride / 4) - (308 - 90)) / 2)));
                    int hat_coord = voxelToPixelLargeW(0, 0, headpoints[1 + dir * 2].x, headpoints[1 + dir * 2].y,
                        headpoints[1 + dir * 2].z, (byte)(253 - headpoints[1 + dir * 2].color) / 4, stride, 0, true) / 4;
                    //     hat_headpoints.AppendLine("EXPLODE_HAT: " + u + "_" + hat + " facing " + dir + " frame " + f + ": x " +
                    //         ((hat_coord % (stride / 4) - 32) / 2) + ", y " + (108 - ((hat_coord / (stride / 4) - 78) / 2)));
                    Image h2 = Image.FromFile(altFolder + "palette" + ((hat == "Woodsman") ? 44 : (hat == "Farmer") ? 49 : (palette == 7 || palette == 8 || palette == 42) ? 7 : 0) + "_"
                        // + ((palette == 7 || palette == 8 || palette == 42) ? "Spirit_" : "Generic_Male_")
                        + hat + "_Hat_face" + dir + "_" + ((hat == "Farmer") ? 0 : f % 4) + ".png");
                    hat_image = new Bitmap(h2);
                    h2.Dispose();
                    Image b2 = Image.FromFile(folder + "/palette" + palette + "_" + u + "_Large_face" + dir + "_fiery_explode_" + f + ".png");
                    body_image = new Bitmap(b2);
                    b2.Dispose();

                    VoxelLogic.wcolors = VoxelLogic.wpalettes[palette];
                    wcurrent = wrendered[palette];
                    hat_graphics = Graphics.FromImage(hat_image);
                    body_graphics = Graphics.FromImage(body_image);
                    body_graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    body_graphics.DrawImage(hat_image, 80 + (((body_coord % (stride / 4) - 32) / 2) - ((hat_coord % (stride / 4) - 32) / 2)),
                         40 + (((body_coord / (stride / 4) - (108 - 30)) / 2) - ((hat_coord / (stride / 4) - (108 - 30)) / 2)), 88, 108);
                    //                    model_headpoints.AppendLine("EXPLODE: " + u + "_" + hat + " facing " + dir + " frame " + f + ": x " +
                    //                        (80 + (((body_coord % (stride / 4) - 32) / 2) - ((hat_coord % (stride / 4) - 32) / 2))) +
                    //                        ", y " + (40 + (((body_coord / (stride / 4) - (108 - 30)) / 2) - ((hat_coord / (stride / 4) - (108 - 30)) / 2))));
                    body_image.Save(folder + "/palette" + palette + "_" + u + "_" + hat + "_Large_face" + dir + "_fiery_explode_" + f + ".png", ImageFormat.Png);

                }
            }

            body_graphics.Dispose();
            hat_graphics.Dispose();
            hat_image.Dispose();
            body_image.Dispose();
            bmp.Dispose();
            body.Close();
            bin.Close();

            System.IO.Directory.CreateDirectory("gifs/" + altFolder);
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";

            for(int d = 0; d < 4; d++)
            {
                for(int frame = 0; frame < 12; frame++)
                {
                    s += folder + "/palette" + palette + "_" + u + "_" + hat + "_Large_face" + d + "_fiery_explode_" + frame + ".png ";
                }
            }

            startInfo.Arguments = "-dispose background -delay 11 -loop 0 " + s + " gifs/" + altFolder + "palette" + palette + "_" + u + "_" + hat + "_explosion_animated.gif";
            Console.WriteLine("Running convert.exe ...");
            Process.Start(startInfo).WaitForExit();

            //bin.Close();
        }
        public static void processExplosionHugeW(string u, int palette)
        {
            Console.WriteLine("Processing: " + u + ", palette " + palette);
            BinaryReader bin = new BinaryReader(File.Open(u + "_Huge_W.vox", FileMode.Open));
            MagicaVoxelData[] parsed = VoxelLogic.FromMagicaRaw(bin).ToArray();
            //renderLarge(parsed, 0, 0, 0)[0].Save("junk_" + u + ".png");

            VoxelLogic.wcolors = VoxelLogic.wpalettes[palette];
            wcurrent = wrendered[palette];

            // MagicaVoxelData[][] explode = VoxelLogic.FieryExplosionQuadW(parsed, false, true); //((CurrentMobilities[UnitLookup[u]] == MovementType.Immobile) ? false : true)
            string folder = ("frames");

            Directory.CreateDirectory(folder); //("color" + i);

            for(int d = 0; d < 4; d++)
            {
                FaceVoxel[,,] faces = FaceLogic.GetFaces(FaceLogic.VoxListToArray(VoxelLogic.BasicRotateHuge(parsed, d), 120, 120, 80, 153));

                FaceVoxel[][,,] explode = FaceLogic.FieryExplosionLargeW(faces, false, false);

                for(int frame = 0; frame < 12; frame++)
                {
                    Bitmap b = renderWSmartMassive(explode[frame], d, palette, frame, 8, true, false);
                    Bitmap b2 = new Bitmap(328, 408, PixelFormat.Format32bppArgb);

                    //                        b.Save("temp.png", ImageFormat.Png);
                    Graphics g2 = Graphics.FromImage(b2);
                    g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    Bitmap b3 = b.Clone(new Rectangle(0, 0, 328 * 2, 408 * 2), b.PixelFormat);
                    b.Dispose();
                    g2.DrawImage(b3, 0, 0, 328, 408);

                    b2.Save(folder + "/palette" + palette + "_" + u + "_Huge_face" + d + "_fiery_explode_" + frame + ".png", ImageFormat.Png);
                    b2.Dispose();
                    g2.Dispose();
                }
            }


            Directory.CreateDirectory("gifs/" + altFolder);
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";

            for(int d = 0; d < 4; d++)
            {
                for(int frame = 0; frame < 12; frame++)
                {
                    s += folder + "/palette" + palette + "_" + u + "_Huge_face" + d + "_fiery_explode_" + frame + ".png ";
                }
            }

            startInfo.Arguments = "-dispose background -delay 11 -loop 0 " + s + " gifs/" + altFolder + "palette" + palette + "_" + u + "_explosion_animated.gif";
            Console.WriteLine("Running convert.exe ...");
            Process.Start(startInfo).WaitForExit();

            //bin.Close();
        }
        public static void processExplosionHugeW(string u, int palette, bool shadowless)
        {
            Console.WriteLine("Processing: " + u + ", palette " + palette);
            BinaryReader bin = new BinaryReader(File.Open(u + "_Huge_W.vox", FileMode.Open));
            MagicaVoxelData[] parsed = VoxelLogic.FromMagicaRaw(bin).ToArray();
            //renderLarge(parsed, 0, 0, 0)[0].Save("junk_" + u + ".png");

            VoxelLogic.wcolors = VoxelLogic.wpalettes[palette];
            wcurrent = wrendered[palette];
            // MagicaVoxelData[][] explode = VoxelLogic.FieryExplosionQuadW(parsed, false, true); //((CurrentMobilities[UnitLookup[u]] == MovementType.Immobile) ? false : true)
            string folder = ("frames");

            Directory.CreateDirectory(folder); //("color" + i);

            for(int d = 0; d < 4; d++)
            {
                FaceVoxel[,,] faces = FaceLogic.GetFaces(FaceLogic.VoxListToArray(VoxelLogic.BasicRotateHuge(parsed, d), 120, 120, 80, 153));

                FaceVoxel[][,,] explode = FaceLogic.FieryExplosionLargeW(faces, false, false);
                for(int frame = 0; frame < 12; frame++)
                {
                    Bitmap b = renderWSmartMassive(explode[frame], d, palette, frame, 8, true, shadowless);
                    Bitmap b2 = new Bitmap(328, 408, PixelFormat.Format32bppArgb);

                    //                        b.Save("temp.png", ImageFormat.Png);
                    Graphics g2 = Graphics.FromImage(b2);
                    g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    Bitmap b3 = b.Clone(new Rectangle(0, 0, 328 * 2, 408 * 2), b.PixelFormat);
                    b.Dispose();
                    g2.DrawImage(b3, 0, 0, 328, 408);

                    b2.Save(folder + "/palette" + palette + "_" + u + "_Huge_face" + d + "_fiery_explode_" + frame + ".png", ImageFormat.Png);
                    b2.Dispose();
                    g2.Dispose();
                }
            }


            Directory.CreateDirectory("gifs/" + altFolder);
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";

            for(int d = 0; d < 4; d++)
            {
                for(int frame = 0; frame < 12; frame++)
                {
                    s += folder + "/palette" + palette + "_" + u + "_Huge_face" + d + "_fiery_explode_" + frame + ".png ";
                }
            }

            startInfo.Arguments = "-dispose background -delay 11 -loop 0 " + s + " gifs/" + altFolder + "palette" + palette + "_" + u + "_explosion_animated.gif";
            Console.WriteLine("Running convert.exe ...");
            Process.Start(startInfo).WaitForExit();

            //bin.Close();
        }

        public static void processPureAttackW(string u, int palette)
        {
            Console.WriteLine("Processing: " + u);
            BinaryReader[] bins = new BinaryReader[12];
            MagicaVoxelData[][] attacking = new MagicaVoxelData[12][];
            for(int i = 0; i < 12; i++)
            {
                bins[i] = new BinaryReader(File.Open("animations/" + u + "/" + u + "_Attack_" + i + ".vox", FileMode.Open));
                attacking[i] = VoxelLogic.FromMagicaRaw(bins[i]).ToArray();
                for(int j = 0; j < attacking[i].Length; j++)
                {
                    attacking[i][j].x += 10;
                    attacking[i][j].y += 10;
                    /*if((254 - attacking[i][j].color) % 4 == 0)
                    {
                        Console.WriteLine("Base Found: X" + attacking[i][j].x + ", Y " + attacking[i][j].y + ", Z " + attacking[i][j].z);
                    }*/
                }
                bins[i].Close();
            }
            VoxelLogic.wcolors = VoxelLogic.wpalettes[palette];
            wcurrent = wrendered[palette];
            string folder = ("frames");

            Directory.CreateDirectory(folder); //("color" + i);

            for(int d = 0; d < 4; d++)
            {
                for(int frame = 0; frame < 12; frame++)
                {

                    FaceVoxel[,,] faces = FaceLogic.GetFaces(FaceLogic.VoxListToArray(VoxelLogic.BasicRotateLarge(attacking[frame], d), 60, 60, 60, 153));

                    Bitmap b = renderWSmart(faces, d, palette, frame, 12, true, true);
                    /*                    string folder2 = "palette" + palette + "_big";
                                        System.IO.Directory.CreateDirectory(folder2);
                                        b.Save(folder + "/" + (System.IO.Directory.GetFiles(folder2).Length) + "_Gigantic_face" + d + "_" + frame + ".png", ImageFormat.Png);
                    */
                    Bitmap b2 = new Bitmap(88, 108, PixelFormat.Format32bppArgb);
                    Graphics g2 = Graphics.FromImage(b2);
                    g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    g2.DrawImage(b.Clone(new Rectangle(32, 46 + 32, 88 * 2, 108 * 2), b.PixelFormat), 0, 0, 88, 108);
                    g2.Dispose();
                    b2.Save(folder + "/palette" + palette + "_" + u + "_Attack_face" + d + "_" + frame + ".png", ImageFormat.Png);

                }
            }


            Directory.CreateDirectory("gifs/" + altFolder);
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";

            for(int d = 0; d < 4; d++)
            {
                for(int frame = 0; frame < 12; frame++)
                {
                    s += folder + "/palette" + palette + "_" + u + "_Attack_face" + d + "_" + frame + ".png ";
                }
            }

            startInfo.Arguments = "-dispose background -delay 11 -loop 0 " + s + " gifs/" + altFolder + "palette" + palette + "_" + u + "_attack_animated.gif";
            Console.WriteLine("Running convert.exe ...");
            Process.Start(startInfo).WaitForExit();
            //Console.ReadKey();
            //bin.Close();
        }

        public static void processUnitLargeWMecha(string moniker = "Maku", bool still = true,
            string legs = "Armored", string torso = "Armored", string left_arm = "Armored", string right_arm = "Armored", string head = "Blocky", string left_weapon = null, string right_weapon = null)
        {

            Dictionary<string, List<MagicaVoxelData>> components = new Dictionary<string, List<MagicaVoxelData>>
            { {"Legs", VoxelLogic.readPart(legs + "_Legs")},
                {"Torso", VoxelLogic.readPart(torso + "_Torso")},
                 {"Left_Arm", VoxelLogic.readPart(left_arm + "_Left_Arm")},
                 {"Right_Arm", VoxelLogic.readPart(right_arm + "_Right_Arm")},
                 {"Head",  VoxelLogic.readPart(head + "_Head")},
                 {"Left_Weapon", VoxelLogic.readPart(left_weapon)},
                 {"Right_Weapon", VoxelLogic.readPart(right_weapon)}
            };
            List<MagicaVoxelData> work = VoxelLogic.MergeVoxels(components["Head"], components["Torso"], 0);
            work = VoxelLogic.MergeVoxels(VoxelLogic.MergeVoxels(components["Right_Arm"], components["Right_Weapon"], 4, 6, VoxelLogic.clear), work, 2);
            work = VoxelLogic.MergeVoxels(VoxelLogic.MergeVoxels(components["Left_Arm"], components["Left_Weapon"], 4, 6, VoxelLogic.clear), work, 3);
            work = VoxelLogic.MergeVoxels(work, components["Legs"], 1);
            work = VoxelLogic.PlaceShadowsPartialW(work);
            Directory.CreateDirectory("vox/" + altFolder);
            VoxelLogic.WriteVOX("vox/" + altFolder + moniker + "_0.vox", work, "W", 0, 40, 40, 40);

            MagicaVoxelData[] parsed = work.ToArray();
            for(int i = 0; i < parsed.Length; i++)
            {
                parsed[i].x += 10;
                parsed[i].y += 10;
            }
            int framelimit = 4;

            for(int palette = 0; palette < VoxelLogic.wpalettecount; palette++)
            {
                Console.WriteLine("Processing: " + moniker + ", palette " + palette);
                string folder = (altFolder);//"color" + i;
                Directory.CreateDirectory(folder); //("color" + i);

                for(int dir = 0; dir < 4; dir++)
                {
                    FaceVoxel[,,] faces = FaceLogic.GetFaces(FaceLogic.VoxListToArray(VoxelLogic.BasicRotateLarge(parsed, dir), 60, 60, 60, 153));
                    for(int f = 0; f < framelimit; f++)
                    {
                        Bitmap b = processFrameLargeW(faces, palette, dir, f, framelimit, still, false);
                        b.Save(folder + "/palette" + palette + "_" + moniker + "_Large_face" + dir + "_" + f + ".png", ImageFormat.Png); //se
                        b.Dispose();
                    }
                }


                Directory.CreateDirectory("gifs/" + altFolder);
                ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
                startInfo.UseShellExecute = false;
                string s = "";

                s = folder + "/palette" + palette + "_" + moniker + "_Large_face* ";
                startInfo.Arguments = "-dispose background -delay 25 -loop 0 " + s + " gifs/" + altFolder + "palette" + palette + "_" + moniker + "_Large_animated.gif";
                Process.Start(startInfo).WaitForExit();

                //bin.Close();

                //            processFiringDouble(u);

                processExplosionLargeW(moniker, palette, work.ToArray(), false);

            }
        }
        public static void processUnitLargeWMechaFiring(string moniker = "Maku", bool still = true,
            string legs = "Armored", string torso = "Armored", string left_arm = "Armored", string right_arm = "Armored", string head = "Blocky", string left_weapon = null, string right_weapon = null,
            string left_projectile = null, string right_projectile = null)
        {
            Bitmap bmp = new Bitmap(248, 308, PixelFormat.Format32bppArgb);

            PixelFormat pxf = PixelFormat.Format32bppArgb;

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);
            int stride = bmpData.Stride;
            bmp.UnlockBits(bmpData);
            bmp.Dispose();


            for(int combo = 0; combo < 3; combo++)
            {
                string firing_name = "Firing_Left";
                Dictionary<string, List<MagicaVoxelData>> components = new Dictionary<string, List<MagicaVoxelData>>
            { {"Legs", VoxelLogic.readPart(legs + "_Legs")},
                {"Torso", VoxelLogic.readPart(torso + "_Torso")},
                 {"Left_Arm", VoxelLogic.readPart(left_arm + "_Left_Arm")},
                 {"Right_Arm", VoxelLogic.readPart(right_arm + "_Right_Arm")},
                 {"Head",  VoxelLogic.readPart(head + "_Head")},
                 {"Left_Weapon", VoxelLogic.readPart(left_weapon)},
                 {"Right_Weapon", VoxelLogic.readPart(right_weapon)}
            };
                List<MagicaVoxelData> work = VoxelLogic.MergeVoxels(components["Head"], components["Torso"], 0),
                    right_projectors = new List<MagicaVoxelData>(1), left_projectors = new List<MagicaVoxelData>(1);
                MagicaVoxelData bogus = new MagicaVoxelData { x = 255, y = 255, z = 255, color = 255 }, right_projector = bogus, left_projector = bogus;
                switch(combo)
                {
                    case 0:
                        if(right_weapon != null)
                        {
                            firing_name = "Firing_Right";
                            work = VoxelLogic.MergeVoxels(VoxelLogic.RotatePitch(VoxelLogic.MergeVoxels(components["Right_Arm"], components["Right_Weapon"], 4), 3, 40, 40), work, 2);
                            work = VoxelLogic.MergeVoxels(VoxelLogic.MergeVoxels(components["Left_Arm"], components["Left_Weapon"], 4, 6, VoxelLogic.clear), work, 3);

                        }
                        else continue;
                        break;
                    case 1:
                        if(left_weapon != null)
                        {
                            work = VoxelLogic.MergeVoxels(VoxelLogic.RotatePitch(VoxelLogic.MergeVoxels(components["Left_Arm"], components["Left_Weapon"], 4, 6, 254 - 7 * 4), 3, 40, 40), work, 3);
                            work = VoxelLogic.MergeVoxels(VoxelLogic.MergeVoxels(components["Right_Arm"], components["Right_Weapon"], 4, 6, VoxelLogic.clear), work, 2);
                        }
                        else continue;
                        break;
                    case 2:
                        if(left_weapon != null && right_weapon != null)
                        {
                            firing_name = "Firing_Both";
                            work = VoxelLogic.MergeVoxels(VoxelLogic.RotatePitch(VoxelLogic.MergeVoxels(components["Right_Arm"], components["Right_Weapon"], 4), 3, 40, 40), work, 2);
                            work = VoxelLogic.MergeVoxels(VoxelLogic.RotatePitch(VoxelLogic.MergeVoxels(components["Left_Arm"], components["Left_Weapon"], 4, 6, 254 - 7 * 4), 3, 40, 40), work, 3);
                        }
                        else continue;
                        break;
                }
                work = VoxelLogic.MergeVoxels(work, components["Legs"], 1);

                try
                {
                    right_projector = work.First(m => (254 - m.color) == 4 * 6);
                    right_projector.x += 10;
                    right_projector.y += 10;
                    right_projectors.Add(right_projector);
                }
                catch(InvalidOperationException)
                {
                    right_projector = bogus;
                }
                try
                {
                    left_projector = work.First(m => (254 - m.color) == 4 * 7);
                    left_projector.x += 10;
                    left_projector.y += 10;
                    left_projectors.Add(left_projector);
                }
                catch(InvalidOperationException)
                {
                    left_projector = bogus;
                }
                work = VoxelLogic.PlaceShadowsPartialW(work);
                Directory.CreateDirectory("vox/" + altFolder);
                VoxelLogic.WriteVOX("vox/" + altFolder + moniker + "_" + firing_name + "_0.vox", work, "W", 0, 40, 40, 40);
                MagicaVoxelData[] parsed = work.ToArray();
                for(int i = 0; i < parsed.Length; i++)
                {
                    parsed[i].x += 10;
                    parsed[i].y += 10;
                }
                int framelimit = 4;

                for(int palette = 0; palette < VoxelLogic.wpalettecount; palette++)
                {
                    Console.WriteLine("Processing: " + moniker + ", palette " + palette + ", " + firing_name);
                    string folder = (altFolder);//"color" + i;
                    Directory.CreateDirectory(folder); //("color" + i);

                    for(int dir = 0; dir < 4; dir++)
                    {
                        FaceVoxel[,,] faces = FaceLogic.GetFaces(FaceLogic.VoxListToArray(VoxelLogic.BasicRotateLarge(parsed, dir), 60, 60, 60, 153));
                        for(int f = 0; f < framelimit; f++)
                        {
                            Bitmap b = processFrameLargeW(faces, palette, dir, f, framelimit, still, false);
                            b.Save(folder + "/palette" + palette + "_" + moniker + "_" + firing_name + "_Large_face" + dir + "_" + f + ".png", ImageFormat.Png);
                            b.Dispose();
                        }
                    }

                    Directory.CreateDirectory("gifs/" + altFolder);
                    ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
                    startInfo.UseShellExecute = false;
                    string s = "";

                    s = folder + "/palette" + palette + "_" + moniker + "_" + firing_name + "_Large_face* ";
                    startInfo.Arguments = "-dispose background -delay 25 -loop 0 " + s + " gifs/" + altFolder + "palette" + palette + "_" + moniker + "_" + firing_name + "_Large_animated.gif";
                    Process.Start(startInfo).WaitForExit();

                    // Animation generation starts here

                    switch(combo)
                    {
                        case 0:
                            if(right_projectile == null)
                                continue;
                            break;
                        case 1:
                            if(left_projectile == null)
                                continue;
                            break;
                        case 2:
                            if(left_projectile == null || right_projectile == null)
                                continue;
                            break;
                    }
                    for(int f = 0; f < 12; f++)
                    {
                        for(int dir = 0; dir < 4; dir++)
                        {
                            List<MagicaVoxelData> right_projectors_adj = VoxelLogic.RotateYaw(right_projectors, dir, 60, 60), left_projectors_adj = VoxelLogic.RotateYaw(left_projectors, dir, 60, 60);
                            Bitmap b = new Bitmap(folder + "/palette" + palette + "_" + moniker + "_" + firing_name + "_Large_face" + dir + "_" + (f % framelimit) + ".png"),
                                b_base = new Bitmap(248, 308, PixelFormat.Format32bppArgb);// new Bitmap("palette50/palette50_Terrain_Huge_face0_0.png"),
                            Bitmap b_left = new Bitmap(88, 108, PixelFormat.Format32bppArgb), b_right = new Bitmap(88, 108, PixelFormat.Format32bppArgb);
                            if(left_projectile != null) b_left = new Bitmap("frames/palette0_" + left_projectile + "_Attack_face" + dir + "_" + f + ".png");
                            if(right_projectile != null) b_right = new Bitmap("frames/palette0_" + right_projectile + "_Attack_face" + dir + "_" + f + ".png");
                            MagicaVoxelData left_emission = bogus, right_emission = bogus;
                            if(left_projectile != null)
                            {
                                BinaryReader leftbin = new BinaryReader(File.Open("animations/" + left_projectile + "/" + left_projectile + "_Attack_" + f + ".vox", FileMode.Open));
                                left_emission = VoxelLogic.RotateYaw(VoxelLogic.FromMagicaRaw(leftbin), dir, 40, 40).First(m => 254 - m.color == 4 * 6);
                            }

                            if(right_projectile != null)
                            {
                                BinaryReader rightbin = new BinaryReader(File.Open("animations/" + right_projectile + "/" + right_projectile + "_Attack_" + f + ".vox", FileMode.Open));
                                right_emission = VoxelLogic.RotateYaw(VoxelLogic.FromMagicaRaw(rightbin), dir, 40, 40).First(m => 254 - m.color == 4 * 6);
                            }
                            //left_emission.x += 40;
                            //left_emission.y += 40;
                            //right_emission.x += 40;
                            //right_emission.y += 40;
                            //Bitmap b_base = new Bitmap(248, 308, PixelFormat.Format32bppArgb);
                            Graphics g = Graphics.FromImage(b_base);
                            if(dir < 2)
                            {
                                g.DrawImage(b, 80, 160);
                                if(right_projectors_adj.Count > 0)
                                {
                                    int proj_location = voxelToPixelHugeW(0, 0, right_projectors_adj.First().x, right_projectors_adj.First().y, right_projectors_adj.First().z, 0, stride, 0, still) / 4,
                                        emit_location = voxelToPixelHugeW(0, 0, right_emission.x, right_emission.y, right_emission.z, 0, stride, 0, still) / 4;

                                    g.DrawImage(b_right, 60 + (((proj_location % (stride / 4) - 32) / 2) - ((emit_location % (stride / 4) - 32) / 2)),
                                         160 + (((proj_location / (stride / 4) - (108 - 30)) / 2) - ((emit_location / (stride / 4) - (108 - 30)) / 2)), 88, 108);
                                }
                                if(left_projectors_adj.Count > 0)
                                {
                                    int proj_location = voxelToPixelHugeW(0, 0, left_projectors_adj.First().x, left_projectors_adj.First().y, left_projectors_adj.First().z, 0, stride, 0, still) / 4,
                                        emit_location = voxelToPixelHugeW(0, 0, left_emission.x, left_emission.y, left_emission.z, 0, stride, 0, still) / 4;

                                    g.DrawImage(b_left, 60 + (((proj_location % (stride / 4) - 32) / 2) - ((emit_location % (stride / 4) - 32) / 2)),
                                         160 + (((proj_location / (stride / 4) - (108 - 30)) / 2) - ((emit_location / (stride / 4) - (108 - 30)) / 2)), 88, 108);
                                }
                            }
                            else
                            {
                                if(right_projectors_adj.Count > 0)
                                {
                                    int proj_location = voxelToPixelHugeW(0, 0, right_projectors_adj.First().x, right_projectors_adj.First().y, right_projectors_adj.First().z, 0, stride, 0, still) / 4,
                                        emit_location = voxelToPixelHugeW(0, 0, right_emission.x, right_emission.y, right_emission.z, 0, stride, 0, still) / 4;

                                    g.DrawImage(b_right, 60 + (((proj_location % (stride / 4) - 32) / 2) - ((emit_location % (stride / 4) - 32) / 2)),
                                         160 + (((proj_location / (stride / 4) - (108 - 30)) / 2) - ((emit_location / (stride / 4) - (108 - 30)) / 2)), 88, 108);
                                }
                                if(left_projectors_adj.Count > 0)
                                {
                                    int proj_location = voxelToPixelHugeW(0, 0, left_projectors_adj.First().x, left_projectors_adj.First().y, left_projectors_adj.First().z, 0, stride, 0, still) / 4,
                                        emit_location = voxelToPixelHugeW(0, 0, left_emission.x, left_emission.y, left_emission.z, 0, stride, 0, still) / 4;

                                    g.DrawImage(b_left, 60 + (((proj_location % (stride / 4) - 32) / 2) - ((emit_location % (stride / 4) - 32) / 2)),
                                         160 + (((proj_location / (stride / 4) - (108 - 30)) / 2) - ((emit_location / (stride / 4) - (108 - 30)) / 2)), 88, 108);
                                }
                                g.DrawImage(b, 80, 160);
                            }
                            b_base.Save("frames/palette" + palette + "_" + moniker + "_" + firing_name + "_" +
                                ((firing_name == "Firing_Left") ? left_projectile : (firing_name == "Firing_Right") ? right_projectile : left_projectile + "_" + right_projectile)
                                + "_Huge_face" + dir + "_" + f + ".png");

                        }
                    }

                    s = "";
                    for(int dir = 0; dir < 4; dir++)
                    {
                        for(int f = 0; f < 12; f++)
                        {
                            s += "frames/palette" + palette + "_" + moniker + "_" + firing_name + "_" +
                            ((firing_name == "Firing_Left") ? left_projectile : (firing_name == "Firing_Right") ? right_projectile : left_projectile + "_" + right_projectile) +
                            "_Huge_face" + dir + "_" + f + ".png ";
                        }
                    }
                    startInfo.Arguments = "-dispose background -delay 11 -loop 0 " + s + " gifs/" + altFolder + "palette" + palette + "_" + moniker + "_" + firing_name + "_" +
                                ((firing_name == "Firing_Left") ? left_projectile : (firing_name == "Firing_Right") ? right_projectile : left_projectile + "_" + right_projectile) + "_Huge_animated.gif";
                    Process.Start(startInfo).WaitForExit();

                    //bin.Close();

                    //            processFiringDouble(u);

                    //processFieryExplosionDoubleW(moniker, work.ToList(), palette);

                }
            }
        }
        public static void processUnitLargeWMechaAiming(string moniker = "Mark_Zero", bool still = true,
            string legs = "Armored", string torso = "Armored", string left_arm = "Armored_Aiming", string right_arm = "Armored_Aiming", string head = "Armored_Aiming", string right_weapon = "Rifle", string right_projectile = "Autofire")
        {
            Bitmap bmp = new Bitmap(248, 308, PixelFormat.Format32bppArgb);

            PixelFormat pxf = PixelFormat.Format32bppArgb;

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);
            int stride = bmpData.Stride;
            bmp.UnlockBits(bmpData);
            bmp.Dispose();

            Dictionary<string, List<MagicaVoxelData>> components = new Dictionary<string, List<MagicaVoxelData>>
            { {"Legs", VoxelLogic.readPart(legs + "_Legs")},
                {"Torso", VoxelLogic.readPart(torso + "_Torso")},
                 {"Left_Arm", VoxelLogic.readPart(left_arm + "_Left_Arm")},
                 {"Right_Arm", VoxelLogic.readPart(right_arm + "_Right_Arm")},
                 {"Head",  VoxelLogic.readPart(head + "_Head")},
                 {"Right_Weapon", VoxelLogic.readPart(right_weapon)}
            };
            List<MagicaVoxelData> work = VoxelLogic.MergeVoxels(components["Head"], components["Torso"], 0),
                                    projectors = new List<MagicaVoxelData>(1);
            MagicaVoxelData bogus = new MagicaVoxelData { x = 255, y = 255, z = 255, color = 255 }, right_projector = bogus;

            work = VoxelLogic.MergeVoxels(VoxelLogic.MergeVoxels(components["Right_Arm"], components["Right_Weapon"], 4), work, 2);
            work = VoxelLogic.MergeVoxels(components["Left_Arm"], work, 3);
            work = VoxelLogic.MergeVoxels(work, components["Legs"], 1);

            work = VoxelLogic.RotateYaw(work, 1, 40, 40);
            try
            {
                right_projector = work.First(m => (254 - m.color) == 4 * 6);
                right_projector.x += 10;
                right_projector.y += 10;
                projectors.Add(right_projector);
            }
            catch(InvalidOperationException)
            {
                right_projector = bogus;
            }
            work = VoxelLogic.PlaceShadowsPartialW(work);
            Directory.CreateDirectory("vox/" + altFolder);
            VoxelLogic.WriteVOX("vox/" + altFolder + moniker + "_Firing_Both_0.vox", work, "W", 0, 40, 40, 40);
            MagicaVoxelData[] parsed = work.ToArray();
            for(int i = 0; i < parsed.Length; i++)
            {
                parsed[i].x += 10;
                parsed[i].y += 10;
            }
            int framelimit = 4;

            for(int palette = 0; palette < VoxelLogic.wpalettecount; palette++)
            {
                Console.WriteLine("Processing: " + moniker + ", palette " + palette + ", " + "Firing_Both");
                string folder = (altFolder);//"color" + i;
                Directory.CreateDirectory(folder); //("color" + i);

                for(int dir = 0; dir < 4; dir++)
                {
                    FaceVoxel[,,] faces = FaceLogic.GetFaces(FaceLogic.VoxListToArray(VoxelLogic.BasicRotateLarge(parsed, dir), 60, 60, 60, 153));
                    for(int f = 0; f < framelimit; f++)
                    {
                        Bitmap b = processFrameLargeW(faces, palette, dir, f, framelimit, still, false);
                        b.Save(folder + "/palette" + palette + "_" + moniker + "_Firing_Both_Large_face" + dir + "_" + f + ".png", ImageFormat.Png); //se
                        b.Dispose();
                    }
                }


                Directory.CreateDirectory("gifs/" + altFolder);
                ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
                startInfo.UseShellExecute = false;
                string s = "";

                s = folder + "/palette" + palette + "_" + moniker + "_Firing_Both_Large_face* ";
                startInfo.Arguments = "-dispose background -delay 25 -loop 0 " + s + " gifs/" + altFolder + "palette" + palette + "_" + moniker + "_Firing_Both_Large_animated.gif";
                Process.Start(startInfo).WaitForExit();

                // Animation generation starts here

                if(right_projectile == null)
                    continue;

                for(int f = 0; f < 12; f++)
                {
                    for(int dir = 0; dir < 4; dir++)
                    {
                        List<MagicaVoxelData> projectors_adj = VoxelLogic.RotateYaw(projectors, dir, 60, 60);
                        Bitmap b = new Bitmap(folder + "/palette" + palette + "_" + moniker + "_Firing_Both_Large_face" + dir + "_" + (f % framelimit) + ".png"),
                            b_base = new Bitmap(248, 308, PixelFormat.Format32bppArgb);// new Bitmap("palette50/palette50_Terrain_Huge_face0_0.png"),
                        Bitmap b_right = new Bitmap(88, 108, PixelFormat.Format32bppArgb);
                        if(right_projectile != null) b_right = new Bitmap("frames/palette0_" + right_projectile + "_Attack_face" + dir + "_" + f + ".png");
                        MagicaVoxelData emission = bogus;
                        if(right_projectile != null)
                        {
                            BinaryReader bin = new BinaryReader(File.Open("animations/" + right_projectile + "/" + right_projectile + "_Attack_" + f + ".vox", FileMode.Open));
                            emission = VoxelLogic.RotateYaw(VoxelLogic.FromMagicaRaw(bin), dir, 40, 40).First(m => 254 - m.color == 4 * 6);
                        }
                        //left_emission.x += 40;
                        //left_emission.y += 40;
                        //right_emission.x += 40;
                        //right_emission.y += 40;
                        //Bitmap b_base = new Bitmap(248, 308, PixelFormat.Format32bppArgb);
                        Graphics g = Graphics.FromImage(b_base);
                        if(dir < 2)
                        {
                            g.DrawImage(b, 80, 160);
                            if(projectors_adj.Count > 0)
                            {
                                int proj_location = voxelToPixelHugeW(0, 0, projectors_adj.First().x, projectors_adj.First().y, projectors_adj.First().z, 0, stride, 0, still) / 4,
                                    emit_location = voxelToPixelHugeW(0, 0, emission.x, emission.y, emission.z, 0, stride, 0, still) / 4;

                                g.DrawImage(b_right, 60 + (((proj_location % (stride / 4) - 32) / 2) - ((emit_location % (stride / 4) - 32) / 2)),
                                     160 + (((proj_location / (stride / 4) - (108 - 30)) / 2) - ((emit_location / (stride / 4) - (108 - 30)) / 2)), 88, 108);
                            }
                        }
                        else
                        {
                            if(projectors_adj.Count > 0)
                            {
                                int proj_location = voxelToPixelHugeW(0, 0, projectors_adj.First().x, projectors_adj.First().y, projectors_adj.First().z, 0, stride, 0, still) / 4,
                                    emit_location = voxelToPixelHugeW(0, 0, emission.x, emission.y, emission.z, 0, stride, 0, still) / 4;

                                g.DrawImage(b_right, 60 + (((proj_location % (stride / 4) - 32) / 2) - ((emit_location % (stride / 4) - 32) / 2)),
                                     160 + (((proj_location / (stride / 4) - (108 - 30)) / 2) - ((emit_location / (stride / 4) - (108 - 30)) / 2)), 88, 108);
                            }
                            g.DrawImage(b, 80, 160);
                        }
                        b_base.Save("frames/palette" + palette + "_" + moniker + "_Firing_Both_" + right_projectile
                            + "_Huge_face" + dir + "_" + f + ".png");

                    }
                }

                s = "";
                for(int dir = 0; dir < 4; dir++)
                {
                    for(int f = 0; f < 12; f++)
                    {
                        s += "frames/palette" + palette + "_" + moniker + "_Firing_Both_" + right_projectile + "_Huge_face" + dir + "_" + f + ".png ";
                    }
                }
                startInfo.Arguments = "-dispose background -delay 11 -loop 0 " + s + " gifs/" + altFolder + "palette" + palette + "_" + moniker + "_Firing_Both_" +
                            right_projectile + "_Huge_animated.gif";
                Process.Start(startInfo).WaitForExit();


                //bin.Close();

                //            processFiringDouble(u);

                //processFieryExplosionDoubleW(moniker, work.ToList(), palette);
            }
        }
        public static void processUnitLargeWMechaSwinging(string moniker = "Maku", bool still = true,
            string legs = "Armored", string torso = "Armored", string left_arm = "Armored", string right_arm = "Armored", string head = "Blocky", string left_weapon = null, string right_weapon = "Katana",
            string left_projectile = null, string right_projectile = "Swing")
        {
            Bitmap bmp = new Bitmap(248, 308, PixelFormat.Format32bppArgb);

            PixelFormat pxf = PixelFormat.Format32bppArgb;

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);
            int stride = bmpData.Stride;
            bmp.UnlockBits(bmpData);
            bmp.Dispose();


            for(int combo = 0; combo < 3; combo++)
            {
                string firing_name = "Firing_Left";
                Dictionary<string, List<MagicaVoxelData>> components = new Dictionary<string, List<MagicaVoxelData>>
                {
                    { "Legs", VoxelLogic.readPart(legs + "_Legs")},
                    { "Torso", VoxelLogic.readPart(torso + "_Torso")},
                    {"Left_Arm", VoxelLogic.readPart(left_arm + "_Left_Arm")},
                    {"Right_Arm", VoxelLogic.readPart(right_arm + "_Right_Arm")},
                    {"Head",  VoxelLogic.readPart(head + "_Head")},
                    {"Left_Weapon", VoxelLogic.readPart(left_weapon)},
                    {"Right_Weapon", VoxelLogic.readPart(right_weapon)}
                };
                List<MagicaVoxelData> work = VoxelLogic.MergeVoxels(components["Head"], components["Torso"], 0),
                    right_projectors = new List<MagicaVoxelData>(1), left_projectors = new List<MagicaVoxelData>(1);
                MagicaVoxelData bogus = new MagicaVoxelData { x = 255, y = 255, z = 255, color = 255 }, right_projector = bogus, left_projector = bogus;
                switch(combo)
                {
                    case 0:
                        if(right_weapon != null)
                        {
                            firing_name = "Firing_Right";
                            work = VoxelLogic.MergeVoxels(VoxelLogic.RotatePitch(VoxelLogic.MergeVoxels(components["Right_Arm"], components["Right_Weapon"], 4), 3, 40, 40), work, 2);
                            work = VoxelLogic.MergeVoxels(VoxelLogic.MergeVoxels(components["Left_Arm"], components["Left_Weapon"], 4, 6, VoxelLogic.clear), work, 3);

                        }
                        else continue;
                        break;
                    case 1:
                        if(left_weapon != null)
                        {
                            work = VoxelLogic.MergeVoxels(VoxelLogic.RotatePitch(VoxelLogic.MergeVoxels(components["Left_Arm"], components["Left_Weapon"], 4, 6, 254 - 7 * 4), 3, 40, 40), work, 3);
                            work = VoxelLogic.MergeVoxels(VoxelLogic.MergeVoxels(components["Right_Arm"], components["Right_Weapon"], 4, 6, VoxelLogic.clear), work, 2);
                        }
                        else continue;
                        break;
                    case 2:
                        if(left_weapon != null && right_weapon != null)
                        {
                            firing_name = "Firing_Both";
                            work = VoxelLogic.MergeVoxels(VoxelLogic.RotatePitch(VoxelLogic.MergeVoxels(components["Right_Arm"], components["Right_Weapon"], 4), 3, 40, 40), work, 2);
                            work = VoxelLogic.MergeVoxels(VoxelLogic.RotatePitch(VoxelLogic.MergeVoxels(components["Left_Arm"], components["Left_Weapon"], 4, 6, 254 - 7 * 4), 3, 40, 40), work, 3);
                        }
                        else continue;
                        break;
                }
                work = VoxelLogic.MergeVoxels(work, components["Legs"], 1);

                try
                {
                    right_projector = work.First(m => (254 - m.color) == 4 * 6);
                    right_projector.x += 10;
                    right_projector.y += 10;
                    right_projectors.Add(right_projector);
                }
                catch(InvalidOperationException)
                {
                    right_projector = bogus;
                }
                try
                {
                    left_projector = work.First(m => (254 - m.color) == 4 * 7);
                    left_projector.x += 10;
                    left_projector.y += 10;
                    left_projectors.Add(left_projector);
                }
                catch(InvalidOperationException)
                {
                    left_projector = bogus;
                }
                work = VoxelLogic.PlaceShadowsPartialW(work);
                Directory.CreateDirectory("vox/" + altFolder);
                VoxelLogic.WriteVOX("vox/" + altFolder + moniker + "_" + firing_name + "_0.vox", work, "W", 0, 40, 40, 40);
                MagicaVoxelData[] parsed = work.ToArray();
                for(int i = 0; i < parsed.Length; i++)
                {
                    parsed[i].x += 10;
                    parsed[i].y += 10;
                }
                int framelimit = 4;
                byte[] spreadColors = new byte[] {253 - 12 * 4, 253 - 40 * 4 };
                for(int palette = 0; palette < VoxelLogic.wpalettecount; palette++)
                {
                    Console.WriteLine("Processing: " + moniker + ", palette " + palette + ", " + firing_name);
                    string folder = (altFolder);//"color" + i;
                    Directory.CreateDirectory(folder); //("color" + i);

                    for(int dir = 0; dir < 4; dir++)
                    {
                        FaceVoxel[,,] faces = FaceLogic.GetFaces(FaceLogic.VoxListToArray(VoxelLogic.BasicRotateLarge(parsed, dir), 60, 60, 60, 153));
                        for(int f = 0; f < framelimit; f++)
                        {
                            Bitmap b = processFrameLargeW(faces, palette, dir, f, framelimit, still, false);
                            b.Save(folder + "/palette" + palette + "_" + moniker + "_" + firing_name + "_Large_face" + dir + "_" + f + ".png", ImageFormat.Png);
                            b.Dispose();
                        }
                    }

                    Directory.CreateDirectory("gifs/" + altFolder);
                    ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
                    startInfo.UseShellExecute = false;
                    string s = "";

                    s = folder + "/palette" + palette + "_" + moniker + "_" + firing_name + "_Large_face* ";
                    startInfo.Arguments = "-dispose background -delay 25 -loop 0 " + s + " gifs/" + altFolder + "palette" + palette + "_" + moniker + "_" + firing_name + "_Large_animated.gif";
                    Process.Start(startInfo).WaitForExit();

                    // Animation generation starts here

                    switch(combo)
                    {
                        case 0:
                            if(right_projectile != "Swing")
                                continue;
                            break;
                        case 1:
                            if(left_projectile != "Swing")
                                continue;
                            break;
                        case 2:
                            if(left_projectile != "Swing" && right_projectile != "Swing")
                                continue;
                            break;
                    }
                    for(int f = 0; f < 12; f++)
                    {
                        Dictionary<string, List<MagicaVoxelData>> components2 = new Dictionary<string, List<MagicaVoxelData>>
                {
                    { "Legs", VoxelLogic.readPart(legs + "_Legs")},
                    { "Torso", VoxelLogic.readPart(torso + "_Torso")},
                    {"Left_Arm", VoxelLogic.readPart(left_arm + "_Left_Arm")},
                    {"Right_Arm", VoxelLogic.readPart(right_arm + "_Right_Arm")},
                    {"Head",  VoxelLogic.readPart(head + "_Head")},
                    {"Left_Weapon", VoxelLogic.readPart(left_weapon)},
                    {"Right_Weapon", VoxelLogic.readPart(right_weapon)}
                };

                        int[] swingTable = new int[] { 0, 330, 300, 270, 240, 225, 285, 345, 0, 0, 0, 0, },
                             effectTable = new int[] {-1,  -1,  -1,  -1,  -1, 215, 225, 255,-1,-1,-1,-1, };
                        //int[] swingTable = new int[] { 0, 330, 300, 270, 240, 210, 180, 150, 120, 90, 60, 30};
                        List<MagicaVoxelData> work2 = VoxelLogic.MergeVoxels(components["Head"], components["Torso"], 0);
                        switch(combo)
                        {
                            case 0:
                                if(right_weapon != null && right_projectile == "Swing")
                                {
                                    firing_name = "Firing_Right";
                                    if(effectTable[f] >= 0)
                                    {
                                        work2 = VoxelLogic.MergeVoxels(VoxelLogic.RotatePitchPartialSpread(VoxelLogic.MergeVoxels(components["Right_Arm"], components["Right_Weapon"], 4),
                                            swingTable[f],effectTable[f], 40, 40, 40, spreadColors), work2, 2);
                                    }
                                    else
                                    {
                                        work2 = VoxelLogic.MergeVoxels(VoxelLogic.RotatePitchPartial(VoxelLogic.MergeVoxels(components["Right_Arm"], components["Right_Weapon"], 4),
                                            swingTable[f], 40, 40, 40), work2, 2);
                                    }
                                    work2 = VoxelLogic.MergeVoxels(VoxelLogic.MergeVoxels(components["Left_Arm"], components["Left_Weapon"], 4, 6, VoxelLogic.clear), work2, 3);
                                    right_projectors.Clear();

                                }
                                else if(right_weapon != null)
                                {
                                    firing_name = "Firing_Right";
                                    work2 = VoxelLogic.MergeVoxels(VoxelLogic.RotatePitch(VoxelLogic.MergeVoxels(components["Right_Arm"], components["Right_Weapon"], 4), 3, 40, 40), work2, 2);
                                    work2 = VoxelLogic.MergeVoxels(VoxelLogic.MergeVoxels(components["Left_Arm"], components["Left_Weapon"], 4, 6, VoxelLogic.clear), work2, 3);

                                }
                                else continue;
                                break;
                            case 1:
                                if(left_weapon != null && left_projectile == "Swing")
                                {
                                    if(effectTable[f] >= 0)
                                    {
                                        work2 = VoxelLogic.MergeVoxels(VoxelLogic.RotatePitchPartialSpread(VoxelLogic.MergeVoxels(components["Left_Arm"], components["Left_Weapon"], 4, 6, 254 - 7 * 4),
                                            swingTable[f], effectTable[f], 40, 40, 40, spreadColors), work2, 3);
                                    }
                                    else
                                    {
                                        work2 = VoxelLogic.MergeVoxels(VoxelLogic.RotatePitchPartial(VoxelLogic.MergeVoxels(components["Left_Arm"], components["Left_Weapon"], 4, 6, 254 - 7 * 4),
                                            swingTable[f], 40, 40, 40), work2, 3);
                                    }
                                    work2 = VoxelLogic.MergeVoxels(VoxelLogic.MergeVoxels(components["Right_Arm"], components["Right_Weapon"], 4, 6, VoxelLogic.clear), work2, 2);
                                    left_projectors.Clear();
                                }
                                else if(left_weapon != null)
                                {
                                    work2 = VoxelLogic.MergeVoxels(VoxelLogic.RotatePitch(VoxelLogic.MergeVoxels(components["Left_Arm"], components["Left_Weapon"], 4, 6, 254 - 7 * 4), 3, 40, 40), work2, 3);
                                    work2 = VoxelLogic.MergeVoxels(VoxelLogic.MergeVoxels(components["Right_Arm"], components["Right_Weapon"], 4, 6, VoxelLogic.clear), work2, 2);
                                }
                                else continue;
                                break;
                            case 2:
                                
                                if(left_weapon != null && right_weapon != null)
                                {

                                    firing_name = "Firing_Both";
                                    if(right_projectile == "Swing")
                                    {
                                        if(effectTable[f] >= 0)
                                        {
                                            work2 = VoxelLogic.MergeVoxels(VoxelLogic.RotatePitchPartialSpread(VoxelLogic.MergeVoxels(components["Right_Arm"], components["Right_Weapon"], 4),
                                                swingTable[f], effectTable[f], 40, 40, 40, spreadColors), work2, 2);
                                        }
                                        else
                                        {
                                            work2 = VoxelLogic.MergeVoxels(VoxelLogic.RotatePitchPartial(VoxelLogic.MergeVoxels(components["Right_Arm"], components["Right_Weapon"], 4),
                                                swingTable[f], 40, 40, 40), work2, 2);
                                        }
                                    }
                                    else
                                        work2 = VoxelLogic.MergeVoxels(VoxelLogic.RotatePitch(VoxelLogic.MergeVoxels(components["Right_Arm"], components["Right_Weapon"], 4), 3, 40, 40), work2, 2);
                                    if(left_projectile == "Swing")
                                    {
                                        if(effectTable[f] >= 0)
                                        {
                                            work2 = VoxelLogic.MergeVoxels(VoxelLogic.RotatePitchPartialSpread(VoxelLogic.MergeVoxels(components["Left_Arm"], components["Left_Weapon"], 4, 6, 254 - 7 * 4),
                                                swingTable[f], effectTable[f], 40, 40, 40, spreadColors), work2, 3);
                                        }
                                        else
                                        {
                                            work2 = VoxelLogic.MergeVoxels(VoxelLogic.RotatePitchPartial(VoxelLogic.MergeVoxels(components["Left_Arm"], components["Left_Weapon"], 4, 6, 254 - 7 * 4),
                                                swingTable[f], 40, 40, 40), work2, 3);
                                        }
                                    }
                                    else
                                        work2 = VoxelLogic.MergeVoxels(VoxelLogic.RotatePitch(VoxelLogic.MergeVoxels(components["Left_Arm"], components["Left_Weapon"], 4, 6, 254 - 7 * 4), 3, 40, 40), work2, 3);
                                }
                                else continue;
                                break;
                        }
                        work2 = VoxelLogic.MergeVoxels(work2, components["Legs"], 1);

                        try
                        {
                            right_projector = work.First(m => (254 - m.color) == 4 * 6);
                            right_projector.x += 10;
                            right_projector.y += 10;
                            right_projectors.Add(right_projector);
                        }
                        catch(InvalidOperationException)
                        {
                            right_projector = bogus;
                        }
                        try
                        {
                            left_projector = work.First(m => (254 - m.color) == 4 * 7);
                            left_projector.x += 10;
                            left_projector.y += 10;
                            left_projectors.Add(left_projector);
                        }
                        catch(InvalidOperationException)
                        {
                            left_projector = bogus;
                        }
                        
                        work2 = VoxelLogic.PlaceShadowsPartialW(work2);
                        parsed = work2.ToArray();
                        for(int i = 0; i < parsed.Length; i++)
                        {
                            parsed[i].x += 10;
                            parsed[i].y += 10;
                        }

                        for(int dir = 0; dir < 4; dir++)
                        {
                            FaceVoxel[,,] faces = FaceLogic.GetFaces(FaceLogic.VoxListToArray(VoxelLogic.BasicRotateLarge(parsed, dir), 60, 60, 60, 153));
                            List<MagicaVoxelData> right_projectors_adj = VoxelLogic.RotateYaw(right_projectors, dir, 60, 60), left_projectors_adj = VoxelLogic.RotateYaw(left_projectors, dir, 60, 60);
                            Bitmap b = processFrameLargeW(faces, palette, dir, f % 4, 4, still, false),
                                b_base = new Bitmap(248, 308, PixelFormat.Format32bppArgb);// new Bitmap("palette50/palette50_Terrain_Huge_face0_0.png"),
                            Bitmap b_left = new Bitmap(88, 108, PixelFormat.Format32bppArgb), b_right = new Bitmap(88, 108, PixelFormat.Format32bppArgb);
                            if(left_projectile != null && left_projectile != "Swing") b_left = new Bitmap("frames/palette0_" + left_projectile + "_Attack_face" + dir + "_" + f + ".png");
                            if(right_projectile != null && right_projectile != "Swing") b_right = new Bitmap("frames/palette0_" + right_projectile + "_Attack_face" + dir + "_" + f + ".png");
                            MagicaVoxelData left_emission = bogus, right_emission = bogus;
                            if(left_projectile != null && left_projectile != "Swing")
                            {
                                BinaryReader leftbin = new BinaryReader(File.Open("animations/" + left_projectile + "/" + left_projectile + "_Attack_" + f + ".vox", FileMode.Open));
                                left_emission = VoxelLogic.RotateYaw(VoxelLogic.FromMagicaRaw(leftbin), dir, 40, 40).First(m => 254 - m.color == 4 * 6);
                            }

                            if(right_projectile != null && right_projectile != "Swing")
                            {
                                BinaryReader rightbin = new BinaryReader(File.Open("animations/" + right_projectile + "/" + right_projectile + "_Attack_" + f + ".vox", FileMode.Open));
                                right_emission = VoxelLogic.RotateYaw(VoxelLogic.FromMagicaRaw(rightbin), dir, 40, 40).First(m => 254 - m.color == 4 * 6);
                            }
                            //left_emission.x += 40;
                            //left_emission.y += 40;
                            //right_emission.x += 40;
                            //right_emission.y += 40;
                            //Bitmap b_base = new Bitmap(248, 308, PixelFormat.Format32bppArgb);
                            Graphics g = Graphics.FromImage(b_base);
                            if(dir < 2)
                            {
                                g.DrawImage(b, 80, 160);
                                if(right_projectors_adj.Count > 0)
                                {
                                    int proj_location = voxelToPixelHugeW(0, 0, right_projectors_adj.First().x, right_projectors_adj.First().y, right_projectors_adj.First().z, 0, stride, 0, still) / 4,
                                        emit_location = voxelToPixelHugeW(0, 0, right_emission.x, right_emission.y, right_emission.z, 0, stride, 0, still) / 4;

                                    g.DrawImage(b_right, 60 + (((proj_location % (stride / 4) - 32) / 2) - ((emit_location % (stride / 4) - 32) / 2)),
                                         160 + (((proj_location / (stride / 4) - (108 - 30)) / 2) - ((emit_location / (stride / 4) - (108 - 30)) / 2)), 88, 108);
                                }
                                if(left_projectors_adj.Count > 0)
                                {
                                    int proj_location = voxelToPixelHugeW(0, 0, left_projectors_adj.First().x, left_projectors_adj.First().y, left_projectors_adj.First().z, 0, stride, 0, still) / 4,
                                        emit_location = voxelToPixelHugeW(0, 0, left_emission.x, left_emission.y, left_emission.z, 0, stride, 0, still) / 4;

                                    g.DrawImage(b_left, 60 + (((proj_location % (stride / 4) - 32) / 2) - ((emit_location % (stride / 4) - 32) / 2)),
                                         160 + (((proj_location / (stride / 4) - (108 - 30)) / 2) - ((emit_location / (stride / 4) - (108 - 30)) / 2)), 88, 108);
                                }
                            }
                            else
                            {
                                if(right_projectors_adj.Count > 0)
                                {
                                    int proj_location = voxelToPixelHugeW(0, 0, right_projectors_adj.First().x, right_projectors_adj.First().y, right_projectors_adj.First().z, 0, stride, 0, still) / 4,
                                        emit_location = voxelToPixelHugeW(0, 0, right_emission.x, right_emission.y, right_emission.z, 0, stride, 0, still) / 4;

                                    g.DrawImage(b_right, 60 + (((proj_location % (stride / 4) - 32) / 2) - ((emit_location % (stride / 4) - 32) / 2)),
                                         160 + (((proj_location / (stride / 4) - (108 - 30)) / 2) - ((emit_location / (stride / 4) - (108 - 30)) / 2)), 88, 108);
                                }
                                if(left_projectors_adj.Count > 0)
                                {
                                    int proj_location = voxelToPixelHugeW(0, 0, left_projectors_adj.First().x, left_projectors_adj.First().y, left_projectors_adj.First().z, 0, stride, 0, still) / 4,
                                        emit_location = voxelToPixelHugeW(0, 0, left_emission.x, left_emission.y, left_emission.z, 0, stride, 0, still) / 4;

                                    g.DrawImage(b_left, 60 + (((proj_location % (stride / 4) - 32) / 2) - ((emit_location % (stride / 4) - 32) / 2)),
                                         160 + (((proj_location / (stride / 4) - (108 - 30)) / 2) - ((emit_location / (stride / 4) - (108 - 30)) / 2)), 88, 108);
                                }
                                g.DrawImage(b, 80, 160);
                            }
                            b_base.Save("frames/palette" + palette + "_" + moniker + "_" + firing_name + "_" +
                                ((firing_name == "Firing_Left") ? left_projectile : (firing_name == "Firing_Right") ? right_projectile : left_projectile + "_" + right_projectile)
                                + "_Huge_face" + dir + "_" + f + ".png");

                        }
                    }

                    s = "";
                    for(int dir = 0; dir < 4; dir++)
                    {
                        for(int f = 0; f < 12; f++)
                        {
                            s += "frames/palette" + palette + "_" + moniker + "_" + firing_name + "_" +
                            ((firing_name == "Firing_Left") ? left_projectile : (firing_name == "Firing_Right") ? right_projectile : left_projectile + "_" + right_projectile) +
                            "_Huge_face" + dir + "_" + f + ".png ";
                        }
                    }
                    startInfo.Arguments = "-dispose background -delay 11 -loop 0 " + s + " gifs/" + altFolder + "palette" + palette + "_" + moniker + "_" + firing_name + "_" +
                                ((firing_name == "Firing_Left") ? left_projectile : (firing_name == "Firing_Right") ? right_projectile : left_projectile + "_" + right_projectile) + "_Huge_animated.gif";
                    Process.Start(startInfo).WaitForExit();

                    //bin.Close();

                    //            processFiringDouble(u);

                    //processFieryExplosionDoubleW(moniker, work.ToList(), palette);

                }
            }
        }



        public static void processUnitLargeWBones(string moniker = "Dude", bool still = true,
            string right_leg = "Human_Male", string left_leg = "Human_Male", string torso = "Human_Male",
            string left_upper_arm = "Human_Male", string left_lower_arm = "Human_Male", string right_upper_arm = "Human_Male", string right_lower_arm = "Human_Male",
            string head = "Human_Male", string face = "Neutral", string left_weapon = null, string right_weapon = null, int palette = 0)
        {

            Dictionary<string, List<MagicaVoxelData>> components = new Dictionary<string, List<MagicaVoxelData>>
            { {"Left_Leg", VoxelLogic.readComponent(left_leg + "_LLeg")},
              {"Right_Leg", VoxelLogic.readComponent(right_leg + "_RLeg")},
              {"Torso", VoxelLogic.readComponent(torso + "_Torso")},
              {"Left_Upper_Arm", VoxelLogic.readComponent(left_upper_arm + "_LUpperArm")},
              {"Right_Upper_Arm", VoxelLogic.readComponent(right_upper_arm + "_RUpperArm")},
              {"Left_Lower_Arm", VoxelLogic.readComponent(left_lower_arm + "_LLowerArm")},
              {"Right_Lower_Arm", VoxelLogic.readComponent(right_lower_arm + "_RLowerArm")},
              {"Face",  VoxelLogic.readComponent(face + "_Face")},
              {"Head",  VoxelLogic.readComponent(head + "_Head")},
              {"Left_Weapon", VoxelLogic.readComponent(left_weapon)},
              {"Right_Weapon", VoxelLogic.readComponent(right_weapon)}
            };
            byte[,,] work = TransformLogic.MergeVoxels(TransformLogic.TransformStartLarge(components["Head"]), TransformLogic.TransformStartLarge(components["Face"]), 9);
            work = TransformLogic.MergeVoxels(work, TransformLogic.TransformStartLarge(components["Torso"]), 8);
            byte[,,] right_weapon_arm = TransformLogic.MergeVoxels(TransformLogic.TransformStartLarge(components["Right_Weapon"]), TransformLogic.TransformStartLarge(components["Right_Lower_Arm"]), 6);
            byte[,,] left_weapon_arm = TransformLogic.RotatePitchPartial(TransformLogic.TransformStartLarge(components["Left_Weapon"]), 15);
            left_weapon_arm = TransformLogic.MergeVoxels(TransformLogic.TransformStartLarge(components["Left_Lower_Arm"]), left_weapon_arm, 6);
            left_weapon_arm = TransformLogic.RotateYawPartial(left_weapon_arm, 20);
            left_weapon_arm = TransformLogic.RotatePitchPartial(left_weapon_arm, 315);
            right_weapon_arm = TransformLogic.MergeVoxels(right_weapon_arm, TransformLogic.TransformStartLarge(components["Right_Upper_Arm"]), 5);
            left_weapon_arm = TransformLogic.MergeVoxels(left_weapon_arm, TransformLogic.TransformStartLarge(components["Left_Upper_Arm"]), 4);
            left_weapon_arm = TransformLogic.RotatePitchPartial(left_weapon_arm, 315);
            work = TransformLogic.MergeVoxels(right_weapon_arm, work, 3);
            work = TransformLogic.MergeVoxels(left_weapon_arm, work, 2);
            work = TransformLogic.MergeVoxels(work, TransformLogic.TransformStartLarge(components["Left_Leg"]), 0);
            work = TransformLogic.MergeVoxels(TransformLogic.TransformStartLarge(components["Right_Leg"]), work, 1);
            //Directory.CreateDirectory("vox/" + altFolder);
            //VoxelLogic.WriteVOX("vox/" + altFolder + moniker + "_" + palette + ".vox", work, "W", palette, 40, 40, 40);

            MagicaVoxelData[] parsed = TransformLogic.TransformEnd(work).ToArray();
            for(int i = 0; i < parsed.Length; i++)
            {
                parsed[i].x += 10;
                parsed[i].y += 10;
            }
            MagicaVoxelData[] explodeParsed = parsed.Replicate();
            int framelimit = 4;


            Console.WriteLine("Processing: " + moniker + ", palette " + palette);
            string folder = (altFolder);//"color" + i;
            Directory.CreateDirectory(folder); //("color" + i);

            for(int dir = 0; dir < 4; dir++)
            {
                FaceVoxel[,,] faces = FaceLogic.GetFaces(FaceLogic.VoxListToArray(VoxelLogic.BasicRotateLarge(parsed, dir), 60, 60, 60, 153));
                for(int f = 0; f < framelimit; f++)
                {
                    Bitmap b = processFrameLargeW(faces, palette, dir, f, framelimit, still, false);
                    b.Save(folder + "/palette" + palette + "_" + moniker + "_Large_face" + dir + "_" + f + ".png", ImageFormat.Png); //se
                    b.Dispose();
                }
            }


            Directory.CreateDirectory("gifs/" + altFolder);
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";

            s = folder + "/palette" + palette + "_" + moniker + "_Large_face* ";
            startInfo.Arguments = "-dispose background -delay 25 -loop 0 " + s + " gifs/" + altFolder + "palette" + palette + "_" + moniker + "_Large_animated.gif";
            Process.Start(startInfo).WaitForExit();

            //bin.Close();

            //            processFiringDouble(u);

            processExplosionLargeW(moniker, palette, explodeParsed, false);

        }


        public static int voxelToPixelLargeW(int innerX, int innerY, int x, int y, int z, int current_color, int stride, int jitter, bool still)
        {
            return 4 * ((x + y) * 2 + 4 + ((VoxelLogic.wcolors[current_color][3] == VoxelLogic.waver_alpha) ? jitter - 2 : 0))
                + innerX +
                stride * (300 - 60 - y + x - z * 3 - ((VoxelLogic.wcolors[current_color][3] == VoxelLogic.flat_alpha || current_color == 27
                || current_color == VoxelLogic.wcolorcount + 10 || current_color == VoxelLogic.wcolorcount + 20)
                ? -2 : (still) ? 0 : jitter) + innerY);
        }
        public static int voxelToPixelHugeW(int innerX, int innerY, int x, int y, int z, int current_color, int stride, int jitter, bool still)
        {
            return 4 * ((x + y) * 2 + 12 + ((VoxelLogic.wcolors[current_color][3] == VoxelLogic.waver_alpha) ? jitter - 2 : 0))
                                    + innerX +
                                    stride * (600 - 120 - y + x - z * 3 - ((VoxelLogic.wcolors[current_color][3] == VoxelLogic.flat_alpha || current_color == 27
                                    || current_color == VoxelLogic.wcolorcount + 10 || current_color == VoxelLogic.wcolorcount + 20)
                                    ? -2 : (still) ? 0 : jitter) + innerY);
        }
        public static int voxelToPixelMassiveW(int innerX, int innerY, int x, int y, int z, int current_color, int stride, int jitter, bool still)
        {
            return 4 * ((x + y) * 2 + 12 + ((VoxelLogic.wcolors[current_color][3] == VoxelLogic.waver_alpha) ? jitter - 2 : 0))
                                    + innerX +
                                    stride * (800 - 160 - y + x - z * 3 - ((VoxelLogic.wcolors[current_color][3] == VoxelLogic.flat_alpha || current_color == 27
                                    || current_color == VoxelLogic.wcolorcount + 10 || current_color == VoxelLogic.wcolorcount + 20)
                                    ? -2 : (still) ? 0 : jitter) + innerY);
        }

        private static Bitmap renderWSmart(FaceVoxel[,,] faces, int facing, int palette, int frame, int maxFrames, bool still, bool shadowless)
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
            // int numBytes = bmp.Width * bmp.Height * 3; 
            int numBytes = bmpData.Stride * bmp.Height;
            byte[] argbValues = new byte[numBytes];
            argbValues.Fill<byte>(0);
            byte[] shadowValues = new byte[numBytes];
            shadowValues.Fill<byte>(0);
            byte[] outlineValues = new byte[numBytes];
            outlineValues.Fill<byte>(0);
            byte[] bareValues = new byte[numBytes];
            bareValues.Fill<byte>(0);

            byte[] editValues = new byte[numBytes];
            editValues.Fill<byte>(0);

            bool[] barePositions = new bool[numBytes];
            barePositions.Fill<bool>(false);
            int xSize = 60, ySize = 60, zSize = 60;
/*            switch(facing)
            {
                case 0:
                    break;
                case 1:
                    for(int fz = zSize - 1; fz >= 0; fz--)
                    {
                        for(int fx = xSize - 1; fx >= 0; fx--)
                        {
                            for(int fy = 0; fy < ySize; fy++)
                            {
                                byte tempX = (byte)(fx - (xSize / 2));
                                byte tempY = (byte)(fy - (ySize / 2));
                                MagicaVoxelData mvd = new MagicaVoxelData();
                                mvd.x = (byte)((tempY) + (ySize / 2));
                                mvd.y = (byte)((tempX * -1) + (xSize / 2) - 1);
                                mvd.z = (byte)fz;
                                mvd.color = faceVoxels[fx, fy, fz].vox.color;
                                faces[mvd.x, mvd.y, fz] = new FaceVoxel(mvd, faceVoxels[fx, fy, fz].slope);
                            }
                        }
                    }
                    break;
                case 2:
                    for(int fz = zSize - 1; fz >= 0; fz--)
                    {
                        for(int fx = xSize - 1; fx >= 0; fx--)
                        {
                            for(int fy = 0; fy < ySize; fy++)
                            {
                                byte tempX = (byte)(fx - (xSize / 2));
                                byte tempY = (byte)(fy - (ySize / 2));
                                MagicaVoxelData mvd = new MagicaVoxelData();
                                mvd.x = (byte)((tempX * -1) + (xSize / 2) - 1);
                                mvd.y = (byte)((tempY * -1) + (ySize / 2) - 1);
                                mvd.z = (byte)fz;
                                mvd.color = faceVoxels[fx, fy, fz].vox.color;
                                faces[mvd.x, mvd.y, fz] = new FaceVoxel(mvd, faceVoxels[fx, fy, fz].slope);
                            }
                        }
                    }
                    break;
                case 3:
                    for(int fz = zSize - 1; fz >= 0; fz--)
                    {
                        for(int fx = xSize - 1; fx >= 0; fx--)
                        {
                            for(int fy = 0; fy < ySize; fy++)
                            {
                                byte tempX = (byte)(fx - (xSize / 2));
                                byte tempY = (byte)(fy - (ySize / 2));
                                MagicaVoxelData mvd = new MagicaVoxelData();
                                mvd.x = (byte)((tempY * -1) + (ySize / 2) - 1);
                                mvd.y = (byte)(tempX + (xSize / 2));
                                mvd.z = (byte)fz;
                                mvd.color = faceVoxels[fx, fy, fz].vox.color;
                                faces[mvd.x, mvd.y, fz] = new FaceVoxel(mvd, faceVoxels[fx, fy, fz].slope);
                            }
                        }
                    }
                    break;
            }*/
            int[] zbuffer = new int[numBytes];
            zbuffer.Fill<int>(-999);

            int jitter = (((frame % 4) % 3) + ((frame % 4) / 3)) * 2;
            if(maxFrames >= 8) jitter = ((frame % 8 > 4) ? 4 - ((frame % 8) ^ 4) : frame % 8);
            //FaceVoxel[,,] faces = FaceLogic.GetFaces(FaceLogic.VoxListToArray(vls.ToList(), xSize, ySize, zSize, 153), frame, shadowless);
            bool[,] taken = new bool[xSize, ySize];
            taken.Fill(false);
            //            foreach(MagicaVoxelData vx in vls.OrderByDescending(v => v.x * 64 - v.y + v.z * 64 * 128))
            for(int fz = zSize - 1; fz >= 0; fz--)
            {
                for(int fx = xSize - 1; fx >= 0; fx--)
                {
                    for(int fy = 0; fy < ySize; fy++)
                    {
                        if(faces[fx, fy, fz] == null) continue;
                        MagicaVoxelData vx = faces[fx, fy, fz].vox;
                        Slope slope = faces[fx, fy, fz].slope;
                        int current_color = ((255 - vx.color) % 4 == 0) ? (255 - vx.color) / 4 + VoxelLogic.wcolorcount : ((254 - vx.color) % 4 == 0) ? (253 - VoxelLogic.clear) / 4 : (253 - vx.color) / 4;
                        int p = 0;
                        if((255 - vx.color) % 4 != 0 && current_color >= VoxelLogic.wcolorcount)
                            continue;

                        if(current_color >= 21 && current_color <= 24)
                            current_color = 21 + ((current_color + frame) % 4);

                        if(current_color >= VoxelLogic.wcolorcount && current_color < VoxelLogic.wcolorcount + 4)
                            current_color = VoxelLogic.wcolorcount + ((current_color + frame) % 4);
                        if(current_color >= VoxelLogic.wcolorcount + 6 && current_color < VoxelLogic.wcolorcount + 10)
                            current_color = VoxelLogic.wcolorcount + 6 + ((current_color + frame) % 4);
                        if(current_color >= VoxelLogic.wcolorcount + 14 && current_color < VoxelLogic.wcolorcount + 18)
                            current_color = VoxelLogic.wcolorcount + 14 + ((current_color + frame) % 4);

                        if((frame % 2 != 0) && (VoxelLogic.wcolors[current_color][3] == VoxelLogic.spin_alpha_0 || VoxelLogic.wcolors[current_color][3] == VoxelLogic.flash_alpha_0))
                            continue;
                        else if((frame % 2 != 1) && (VoxelLogic.wcolors[current_color][3] == VoxelLogic.spin_alpha_1 || VoxelLogic.wcolors[current_color][3] == VoxelLogic.flash_alpha_1))
                            continue;
                        else if(VoxelLogic.wcolors[current_color][3] == 0F)
                            continue;
                        else if(VoxelLogic.wcolors[current_color][3] == VoxelLogic.eraser_alpha)
                        {

                            for(int j = 0; j < 4; j++)
                            {
                                for(int i = 3; i < 16; i += 4)
                                {
                                    p = voxelToPixelLargeW(i, j, vx.x, vx.y, vx.z, current_color, bmpData.Stride, jitter, still);
                                    if(argbValues[p] == 0)
                                    {
                                        argbValues[p] = 7;
                                    }
                                }
                            }
                        }
                        else if(current_color >= 17 && current_color <= 20)
                        {
                            int mod_color = current_color;
                            if(mod_color == 17 && r.Next(7) < 3) //smoke
                                continue;
                            if(current_color == 18) //yellow fire
                            {
                                if(r.Next(3) > 0)
                                {
                                    mod_color += r.Next(3);
                                }
                            }
                            else if(current_color == 19) // orange fire
                            {
                                if(r.Next(5) < 4)
                                {
                                    mod_color -= Math.Min(r.Next(3), r.Next(3));
                                }
                            }
                            else if(current_color == 20) // sparks
                            {
                                if(r.Next(5) > 0)
                                {
                                    mod_color -= r.Next(3);
                                }
                            }

                            /*
                             &&
                                bareValues[4 * ((vx.x + vx.y) * 2 + 4 + ((current_color == 136) ? jitter - 1 : 0))
                                + i +
                                bmpData.Stride * (300 - 60 - vx.y + vx.x - vx.z * 3 - ((VoxelLogic.xcolors[current_color][3] == VoxelLogic.flat_alpha) ? -2 : jitter) + j)] == 0
                             */
                            for(int j = 0; j < 4; j++)
                            {
                                for(int i = 0; i < 16; i++)
                                {
                                    p = voxelToPixelLargeW(i, j, vx.x, vx.y, vx.z, mod_color, bmpData.Stride, jitter, still);
                                    if(argbValues[p] == 0 && argbValues[(p / 4) + 3] != 7)
                                    {
                                        int sp = slopes[slope];
                                        if(wcurrent[mod_color][sp][(i / 4 + 3) + j * 16] != 0)
                                        {
                                            barePositions[p] = !(VoxelLogic.wcolors[current_color][3] == VoxelLogic.bordered_alpha || VoxelLogic.wcolors[current_color][3] == VoxelLogic.bordered_flat_alpha);
                                            if(VoxelLogic.wcolors[current_color][3] == VoxelLogic.bordered_alpha || VoxelLogic.wcolors[current_color][3] == VoxelLogic.bordered_flat_alpha)
                                                zbuffer[p] = vx.z + vx.x * 3 - vx.y * 3;

                                            argbValues[p] = wcurrent[mod_color][sp][i + j * 16];
                                        }

                                        if(!barePositions[p] && outlineValues[p] == 0)
                                            outlineValues[p] = wcurrent[mod_color][0][i + 64];

                                    }
                                }
                            }
                        }
                        else if(current_color == 25)
                        {
                            taken[vx.x, vx.y] = true;
                            for(int j = 0; j < 4; j++)
                            {
                                for(int i = 0; i < 16; i++)
                                {
                                    p = voxelToPixelLargeW(i, j, vx.x, vx.y, vx.z, current_color, bmpData.Stride, jitter, still);

                                    if(shadowValues[p] == 0)
                                    {
                                        shadowValues[p] = wcurrent[current_color][0][i + j * 16];
                                    }
                                }
                            }
                        }
                        else
                        {
                            int mod_color = current_color;
                            if((mod_color == 27 || mod_color == VoxelLogic.wcolorcount + 4) && r.Next(7) < 2) //water
                                continue;
                            if((mod_color == 40 || mod_color == VoxelLogic.wcolorcount + 5 || mod_color == VoxelLogic.wcolorcount + 20)) //rare sparks
                            {
                                if(r.Next(11) < 8) continue;
                            }
                            else
                                taken[vx.x, vx.y] = true;
                            int sp = slopes[slope];


                            for(int j = 0; j < 4; j++)
                            {
                                for(int i = 0; i < 16; i++)
                                {
                                    p = voxelToPixelLargeW(i, j, vx.x, vx.y, vx.z, mod_color, bmpData.Stride, jitter, still);

                                    if(argbValues[p] == 0 && argbValues[(p / 4) + 3] != 7)
                                    {

                                        if(wcurrent[mod_color][sp][((i / 4) * 4 + 3) + j * 16] != 0)
                                        {
                                            if(VoxelLogic.wcolors[mod_color][3] == VoxelLogic.gloss_alpha && i % 4 == 3 && r.Next(12) == 0)
                                            {
                                                argbValues[p - 3] = (byte)Math.Min(wcurrent[mod_color][sp][i - 3 + j * 16] + 160, 255);
                                                argbValues[p - 2] = (byte)Math.Min(wcurrent[mod_color][sp][i - 2 + j * 16] + 160, 255);
                                                argbValues[p - 1] = (byte)Math.Min(wcurrent[mod_color][sp][i - 1 + j * 16] + 160, 255);
                                                argbValues[p - 0] = wcurrent[mod_color][sp][i + j * 16];
                                            }
                                            else if(VoxelLogic.wcolors[mod_color][3] == VoxelLogic.grain_hard_alpha && i % 4 == 3)
                                            {
                                                float n = Simplex.FindNoiseBold(facing, vx.x + 50, vx.y + 50, vx.z);
                                                argbValues[p - 3] = (byte)Math.Min(wcurrent[mod_color][sp][i - 3 + j * 16] * n + 16 * (n - 0.8), 255);
                                                argbValues[p - 2] = (byte)Math.Min(wcurrent[mod_color][sp][i - 2 + j * 16] * n + 16 * (n - 0.8), 255);
                                                argbValues[p - 1] = (byte)Math.Min(wcurrent[mod_color][sp][i - 1 + j * 16] * n + 16 * (n - 0.8), 255);
                                                argbValues[p - 0] = wcurrent[mod_color][sp][i + j * 16];
                                            }
                                            else if(VoxelLogic.wcolors[mod_color][3] == VoxelLogic.grain_some_alpha && i % 4 == 3)
                                            {
                                                float n = Simplex.FindNoise(facing, vx.x + 50, vx.y + 50, vx.z);
                                                argbValues[p - 3] = (byte)Math.Min(wcurrent[mod_color][sp][i - 3 + j * 16] * n + 16 * (n - 0.8), 255);
                                                argbValues[p - 2] = (byte)Math.Min(wcurrent[mod_color][sp][i - 2 + j * 16] * n + 16 * (n - 0.8), 255);
                                                argbValues[p - 1] = (byte)Math.Min(wcurrent[mod_color][sp][i - 1 + j * 16] * n + 16 * (n - 0.8), 255);
                                                argbValues[p - 0] = wcurrent[mod_color][sp][i + j * 16];
                                            }
                                            else if(VoxelLogic.wcolors[mod_color][3] == VoxelLogic.grain_mild_alpha && i % 4 == 3)
                                            {
                                                float n = Simplex.FindNoiseLight(facing, vx.x + 50, vx.y + 50, vx.z);
                                                argbValues[p - 3] = (byte)Math.Min(wcurrent[mod_color][sp][i - 3 + j * 16] * n + 16 * (n - 0.8), 255);
                                                argbValues[p - 2] = (byte)Math.Min(wcurrent[mod_color][sp][i - 2 + j * 16] * n + 16 * (n - 0.8), 255);
                                                argbValues[p - 1] = (byte)Math.Min(wcurrent[mod_color][sp][i - 1 + j * 16] * n + 16 * (n - 0.8), 255);
                                                argbValues[p - 0] = wcurrent[mod_color][sp][i + j * 16];
                                            }
                                            else if(VoxelLogic.wcolors[mod_color][3] == VoxelLogic.fuzz_alpha && i % 4 == 3)
                                            {
                                                float n = Simplex.FindNoiseTight(frame % 4, facing, vx.x + 50, vx.y + 50, vx.z) + 0.3f;
                                                argbValues[p - 3] = (byte)VoxelLogic.Clamp(wcurrent[mod_color][sp][i - 3 + j * 16] * n + 24 * (n - 0.8), 1, 255);
                                                argbValues[p - 2] = (byte)VoxelLogic.Clamp(wcurrent[mod_color][sp][i - 2 + j * 16] * n + 24 * (n - 0.8), 1, 255);
                                                argbValues[p - 1] = (byte)VoxelLogic.Clamp(wcurrent[mod_color][sp][i - 1 + j * 16] * n + 24 * (n - 0.8), 1, 255);
                                                argbValues[p - 0] = wcurrent[mod_color][sp][i + j * 16];
                                            }
                                            else
                                            {
                                                argbValues[p] = wcurrent[mod_color][sp][i + j * 16];
                                            }

                                            zbuffer[p] = vx.z + vx.x * 3 - vx.y * 3;
                                            barePositions[p] = (VoxelLogic.wcolors[mod_color][3] == VoxelLogic.flash_alpha || VoxelLogic.wcolors[mod_color][3] == VoxelLogic.flash_alpha_0 ||
                                                VoxelLogic.wcolors[mod_color][3] == VoxelLogic.flash_alpha_1 || VoxelLogic.wcolors[mod_color][3] == VoxelLogic.borderless_alpha ||
                                                VoxelLogic.wcolors[mod_color][3] == VoxelLogic.flat_alpha);
                                        }

                                        if(!barePositions[p] && outlineValues[p] == 0)
                                            outlineValues[p] = wcurrent[mod_color][0][i + 64]; //(argbValues[p] * 1.2 + 2 < 255) ? (byte)(argbValues[p] * 1.2 + 2) : (byte)255;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            int[] xmods = new int[] { -1, 0, 1, -1, 0, 1, -1, 0, 1 }, ymods = new int[] { -1, -1, -1, 0, 0, 0, 1, 1, 1 };
            bool[,] nextTaken = new bool[xSize, ySize];
            for(int iter = 0; iter < 4; iter++)
            {
                for(int x = 1; x < xSize - 1; x++)
                {
                    for(int y = 1; y < ySize - 1; y++)
                    {
                        int ctr = 0;
                        for(int m = 0; m < 9; m++)
                        {
                            if(taken[x + xmods[m], y + ymods[m]])
                                ctr++;
                        }
                        if(ctr >= 5)
                            nextTaken[x, y] = true;

                    }
                }
                taken = nextTaken.Replicate();
            }
            for(int x = 0; x < xSize; x++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    if(taken[x, y])
                    {
                        int p = 0;

                        for(int j = 0; j < 4; j++)
                        {
                            for(int i = 0; i < 16; i++)
                            {
                                p = voxelToPixelLargeW(i, j, x, y, 0, 25, bmpData.Stride, jitter, still);

                                if(shadowValues[p] == 0)
                                {
                                    shadowValues[p] = wcurrent[25][0][i + j * 16];
                                }
                            }
                        }
                    }
                }
            }
            
            for(int i = 3; i < numBytes; i += 4)
            {
                if(argbValues[i] == 7)
                    argbValues[i] = 0;
            }
            bool lightOutline = true;//!VoxelLogic.subtlePalettes.Contains(palette);
            for(int i = 3; i < numBytes; i += 4)
            {
                if(argbValues[i] > 255 * VoxelLogic.waver_alpha && barePositions[i] == false)
                {
                    bool shade = false, blacken = false;
                    /*
                    if (i - 4 >= 0 && i - 4 < argbValues.Length && argbValues[i - 4] == 0 && lightOutline) { editValues[i - 4] = 255; editValues[i - 4 - 1] = 0; editValues[i - 4 - 2] = 0; editValues[i - 4 - 3] = 0; blacken = true; } else if (i - 4 >= 0 && i - 4 < argbValues.Length && barePositions[i - 4] == false && zbuffer[i] - 7 > zbuffer[i - 4]) { editValues[i - 4] = 255; editValues[i - 4 - 1] = outlineValues[i - 1]; editValues[i - 4 - 2] = outlineValues[i - 2]; editValues[i - 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (i + 4 >= 0 && i + 4 < argbValues.Length && argbValues[i + 4] == 0 && lightOutline) { editValues[i + 4] = 255; editValues[i + 4 - 1] = 0; editValues[i + 4 - 2] = 0; editValues[i + 4 - 3] = 0; blacken = true; } else if (i + 4 >= 0 && i + 4 < argbValues.Length && barePositions[i + 4] == false && zbuffer[i] - 7 > zbuffer[i + 4]) { editValues[i + 4] = 255; editValues[i + 4 - 1] = outlineValues[i - 1]; editValues[i + 4 - 2] = outlineValues[i - 2]; editValues[i + 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && argbValues[i - bmpData.Stride] == 0 && lightOutline) { editValues[i - bmpData.Stride] = 255; editValues[i - bmpData.Stride - 1] = 0; editValues[i - bmpData.Stride - 2] = 0; editValues[i - bmpData.Stride - 3] = 0; blacken = true; } else if (i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && barePositions[i - bmpData.Stride] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride]) { editValues[i - bmpData.Stride] = 255; editValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && argbValues[i + bmpData.Stride] == 0 && lightOutline) { editValues[i + bmpData.Stride] = 255; editValues[i + bmpData.Stride - 1] = 0; editValues[i + bmpData.Stride - 2] = 0; editValues[i + bmpData.Stride - 3] = 0; blacken = true; } else if (i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && barePositions[i + bmpData.Stride] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride]) { editValues[i + bmpData.Stride] = 255; editValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    */

                    
                    if((i - 4 >= 0 && i - 4 < argbValues.Length) && ((argbValues[i] > 0 && argbValues[i - 4] == 0 && lightOutline) || (barePositions[i - 4] == false && zbuffer[i] - 7 > zbuffer[i - 4]))) { editValues[i - 4] = 255; editValues[i - 4 - 1] = outlineValues[i - 1]; editValues[i - 4 - 2] = outlineValues[i - 2]; editValues[i - 4 - 3] = outlineValues[i - 3]; if(!blacken) shade = true; }
                    if((i + 4 >= 0 && i + 4 < argbValues.Length) && ((argbValues[i] > 0 && argbValues[i + 4] == 0 && lightOutline) || (barePositions[i + 4] == false && zbuffer[i] - 7 > zbuffer[i + 4]))) { editValues[i + 4] = 255; editValues[i + 4 - 1] = outlineValues[i - 1]; editValues[i + 4 - 2] = outlineValues[i - 2]; editValues[i + 4 - 3] = outlineValues[i - 3]; if(!blacken) shade = true; }
                    if((i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length) && ((argbValues[i] > 0 && argbValues[i - bmpData.Stride] == 0 && lightOutline) || (barePositions[i - bmpData.Stride] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride]))) { editValues[i - bmpData.Stride] = 255; editValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; if(!blacken) shade = true; }
                    if((i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length) && ((argbValues[i] > 0 && argbValues[i + bmpData.Stride] == 0 && lightOutline) || (barePositions[i + bmpData.Stride] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride]))) { editValues[i + bmpData.Stride] = 255; editValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; if(!blacken) shade = true; }
                    


                    /*
                    if (argbValues[i] > 0 && i + 4 >= 0 && i + 4 < argbValues.Length && argbValues[i + 4] == 0 && lightOutline) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = 0; argbValues[i - 4 - 2] = 0; argbValues[i - 4 - 3] = 0; blacken = true; } else if (i + 4 >= 0 && i + 4 < argbValues.Length && barePositions[i + 4] == false && zbuffer[i] - 2 > zbuffer[i + 4]) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = outlineValues[i - 1]; argbValues[i - 4 - 2] = outlineValues[i - 2]; argbValues[i - 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i - 4 >= 0 && i - 4 < argbValues.Length && argbValues[i - 4] == 0 && lightOutline) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = 0; argbValues[i + 4 - 2] = 0; argbValues[i + 4 - 3] = 0; blacken = true; } else if (i - 4 >= 0 && i - 4 < argbValues.Length && barePositions[i - 4] == false && zbuffer[i] - 2 > zbuffer[i - 4]) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = outlineValues[i - 1]; argbValues[i + 4 - 2] = outlineValues[i - 2]; argbValues[i + 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && argbValues[i + bmpData.Stride] == 0 && lightOutline) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = 0; argbValues[i - bmpData.Stride - 2] = 0; argbValues[i - bmpData.Stride - 3] = 0; blacken = true; } else if (i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && barePositions[i + bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride]) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && argbValues[i - bmpData.Stride] == 0 && lightOutline) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = 0; argbValues[i + bmpData.Stride - 2] = 0; argbValues[i + bmpData.Stride - 3] = 0; blacken = true; } else if (i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && barePositions[i - bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride]) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    */
                    /*
                    if (argbValues[i] > 0 && i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && argbValues[i + bmpData.Stride + 4] == 0 && lightOutline) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = 0; argbValues[i - bmpData.Stride - 4 - 2] = 0; argbValues[i - bmpData.Stride - 4 - 3] = 0; blacken = true; } else if (i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && barePositions[i + bmpData.Stride + 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 4]) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && argbValues[i - bmpData.Stride - 4] == 0 && lightOutline) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = 0; argbValues[i + bmpData.Stride + 4 - 2] = 0; argbValues[i + bmpData.Stride + 4 - 3] = 0; blacken = true; } else if (i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && barePositions[i - bmpData.Stride - 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 4]) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && argbValues[i + bmpData.Stride - 4] == 0 && lightOutline) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = 0; argbValues[i - bmpData.Stride + 4 - 2] = 0; argbValues[i - bmpData.Stride + 4 - 3] = 0; blacken = true; } else if (i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && barePositions[i + bmpData.Stride - 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 4]) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && argbValues[i - bmpData.Stride + 4] == 0 && lightOutline) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = 0; argbValues[i + bmpData.Stride - 4 - 2] = 0; argbValues[i + bmpData.Stride - 4 - 3] = 0; blacken = true; } else if (i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && barePositions[i - bmpData.Stride + 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 4]) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    */
                    if(blacken)
                    {
                        editValues[i] = 255; editValues[i - 1] = 0; editValues[i - 2] = 0; editValues[i - 3] = 0;
                    }
                    else if(shade) { editValues[i] = 255; editValues[i - 1] = outlineValues[i - 1]; editValues[i - 2] = outlineValues[i - 2]; editValues[i - 3] = outlineValues[i - 3]; }
                }
            }

            for(int i = 3; i < numBytes; i += 4)
            {
                if(argbValues[i] > 0) // && argbValues[i] <= 255 * VoxelLogic.flat_alpha
                    argbValues[i] = 255;
                if(editValues[i] > 0)
                {
                    argbValues[i - 0] = editValues[i - 0];
                    argbValues[i - 1] = editValues[i - 1];
                    argbValues[i - 2] = editValues[i - 2];
                    argbValues[i - 3] = editValues[i - 3];
                }
            }

            if(!shadowless)
            {
                for(int i = 3; i < numBytes; i += 4)
                {
                    if(argbValues[i] == 0 && shadowValues[i] > 0)
                    {
                        argbValues[i - 3] = shadowValues[i - 3];
                        argbValues[i - 2] = shadowValues[i - 2];
                        argbValues[i - 1] = shadowValues[i - 1];
                        argbValues[i - 0] = shadowValues[i - 0];
                    }
                }
            }
            Marshal.Copy(argbValues, 0, ptr, numBytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            return bmp;
        }

        private static Bitmap renderWSmartHuge(FaceVoxel[,,] faces, int facing, int palette, int frame, int maxFrames, bool still, bool shadowless)
        {
            Bitmap bmp = new Bitmap(248 * 2, 308 * 2, PixelFormat.Format32bppArgb);

            // Specify a pixel format.
            PixelFormat pxf = PixelFormat.Format32bppArgb;

            // Lock the bitmap's bits.
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            // int numBytes = bmp.Width * bmp.Height * 3; 
            int numBytes = bmpData.Stride * bmp.Height;
            byte[] argbValues = new byte[numBytes];
            argbValues.Fill<byte>(0);
            byte[] shadowValues = new byte[numBytes];
            shadowValues.Fill<byte>(0);
            byte[] outlineValues = new byte[numBytes];
            outlineValues.Fill<byte>(0);
            byte[] bareValues = new byte[numBytes];
            bareValues.Fill<byte>(0);

            byte[] editValues = new byte[numBytes];
            editValues.Fill<byte>(0);

            bool[] barePositions = new bool[numBytes];
            barePositions.Fill<bool>(false);
            int xSize = 120, ySize = 120, zSize = 80;
/*            switch(facing)
            {
                case 0:
                    break;
                case 1:
                    for(int fz = zSize - 1; fz >= 0; fz--)
                    {
                        for(int fx = xSize - 1; fx >= 0; fx--)
                        {
                            for(int fy = 0; fy < ySize; fy++)
                            {
                                byte tempX = (byte)(fx - (xSize / 2));
                                byte tempY = (byte)(fy - (ySize / 2));
                                MagicaVoxelData mvd = new MagicaVoxelData();
                                mvd.x = (byte)((tempY) + (ySize / 2));
                                mvd.y = (byte)((tempX * -1) + (xSize / 2) - 1);
                                mvd.z = (byte)fz;
                                mvd.color = faceVoxels[fx, fy, fz].vox.color;
                                faces[mvd.x, mvd.y, fz] = new FaceVoxel(mvd, faceVoxels[fx, fy, fz].slope);
                            }
                        }
                    }
                    break;
                case 2:
                    for(int fz = zSize - 1; fz >= 0; fz--)
                    {
                        for(int fx = xSize - 1; fx >= 0; fx--)
                        {
                            for(int fy = 0; fy < ySize; fy++)
                            {
                                byte tempX = (byte)(fx - (xSize / 2));
                                byte tempY = (byte)(fy - (ySize / 2));
                                MagicaVoxelData mvd = new MagicaVoxelData();
                                mvd.x = (byte)((tempX * -1) + (xSize / 2) - 1);
                                mvd.y = (byte)((tempY * -1) + (ySize / 2) - 1);
                                mvd.z = (byte)fz;
                                mvd.color = faceVoxels[fx, fy, fz].vox.color;
                                faces[mvd.x, mvd.y, fz] = new FaceVoxel(mvd, faceVoxels[fx, fy, fz].slope);
                            }
                        }
                    }
                    break;
                case 3:
                    for(int fz = zSize - 1; fz >= 0; fz--)
                    {
                        for(int fx = xSize - 1; fx >= 0; fx--)
                        {
                            for(int fy = 0; fy < ySize; fy++)
                            {
                                byte tempX = (byte)(fx - (xSize / 2));
                                byte tempY = (byte)(fy - (ySize / 2));
                                MagicaVoxelData mvd = new MagicaVoxelData();
                                mvd.x = (byte)((tempY * -1) + (ySize / 2) - 1);
                                mvd.y = (byte)(tempX + (xSize / 2));
                                mvd.z = (byte)fz;
                                mvd.color = faceVoxels[fx, fy, fz].vox.color;
                                faces[mvd.x, mvd.y, fz] = new FaceVoxel(mvd, faceVoxels[fx, fy, fz].slope);
                            }
                        }
                    }
                    break;
            }*/
            int[] zbuffer = new int[numBytes];
            zbuffer.Fill<int>(-999);

            int jitter = (((frame % 4) % 3) + ((frame % 4) / 3)) * 2;
            if(maxFrames >= 8) jitter = ((frame % 8 > 4) ? 4 - ((frame % 8) ^ 4) : frame % 8);
//            FaceVoxel[,,] faces = FaceLogic.GetFaces(FaceLogic.VoxListToArray(vls.ToList(), xSize, ySize, zSize, 153), frame, shadowless);
            bool[,] taken = new bool[xSize, ySize];
            taken.Fill(false);
            //            foreach(MagicaVoxelData vx in vls.OrderByDescending(v => v.x * 64 - v.y + v.z * 64 * 128))
            for(int fz = zSize - 1; fz >= 0; fz--)
            {
                for(int fx = xSize - 1; fx >= 0; fx--)
                {
                    for(int fy = 0; fy < ySize; fy++)
                    {
                        if(faces[fx, fy, fz] == null) continue;
                        MagicaVoxelData vx = faces[fx, fy, fz].vox;
                        Slope slope = faces[fx, fy, fz].slope;
                        int current_color = ((255 - vx.color) % 4 == 0) ? (255 - vx.color) / 4 + VoxelLogic.wcolorcount : ((254 - vx.color) % 4 == 0) ? (253 - VoxelLogic.clear) / 4 : (253 - vx.color) / 4;
                        int p = 0;
                        if((255 - vx.color) % 4 != 0 && current_color >= VoxelLogic.wcolorcount)
                            continue;

                        if(current_color >= 21 && current_color <= 24)
                            current_color = 21 + ((current_color + frame) % 4);

                        if(current_color >= VoxelLogic.wcolorcount && current_color < VoxelLogic.wcolorcount + 4)
                            current_color = VoxelLogic.wcolorcount + ((current_color + frame) % 4);
                        if(current_color >= VoxelLogic.wcolorcount + 6 && current_color < VoxelLogic.wcolorcount + 10)
                            current_color = VoxelLogic.wcolorcount + 6 + ((current_color + frame) % 4);
                        if(current_color >= VoxelLogic.wcolorcount + 14 && current_color < VoxelLogic.wcolorcount + 18)
                            current_color = VoxelLogic.wcolorcount + 14 + ((current_color + frame) % 4);

                        if((frame % 2 != 0) && (VoxelLogic.wcolors[current_color][3] == VoxelLogic.spin_alpha_0 || VoxelLogic.wcolors[current_color][3] == VoxelLogic.flash_alpha_0))
                            continue;
                        else if((frame % 2 != 1) && (VoxelLogic.wcolors[current_color][3] == VoxelLogic.spin_alpha_1 || VoxelLogic.wcolors[current_color][3] == VoxelLogic.flash_alpha_1))
                            continue;
                        else if(VoxelLogic.wcolors[current_color][3] == 0F)
                            continue;
                        else if(VoxelLogic.wcolors[current_color][3] == VoxelLogic.eraser_alpha)
                        {

                            for(int j = 0; j < 4; j++)
                            {
                                for(int i = 3; i < 16; i += 4)
                                {
                                    p = voxelToPixelHugeW(i, j, vx.x, vx.y, vx.z, current_color, bmpData.Stride, jitter, still);
                                    if(argbValues[p] == 0)
                                    {
                                        argbValues[p] = 7;
                                    }
                                }
                            }
                        }
                        else if(current_color >= 17 && current_color <= 20)
                        {
                            int mod_color = current_color;
                            if(mod_color == 17 && r.Next(7) < 3) //smoke
                                continue;
                            if(current_color == 18) //yellow fire
                            {
                                if(r.Next(3) > 0)
                                {
                                    mod_color += r.Next(3);
                                }
                            }
                            else if(current_color == 19) // orange fire
                            {
                                if(r.Next(5) < 4)
                                {
                                    mod_color -= Math.Min(r.Next(3), r.Next(3));
                                }
                            }
                            else if(current_color == 20) // sparks
                            {
                                if(r.Next(5) > 0)
                                {
                                    mod_color -= r.Next(3);
                                }
                            }

                            /*
                             &&
                                bareValues[4 * ((vx.x + vx.y) * 2 + 4 + ((current_color == 136) ? jitter - 1 : 0))
                                + i +
                                bmpData.Stride * (300 - 60 - vx.y + vx.x - vx.z * 3 - ((VoxelLogic.xcolors[current_color][3] == VoxelLogic.flat_alpha) ? -2 : jitter) + j)] == 0
                             */
                            for(int j = 0; j < 4; j++)
                            {
                                for(int i = 0; i < 16; i++)
                                {
                                    p = voxelToPixelHugeW(i, j, vx.x, vx.y, vx.z, mod_color, bmpData.Stride, jitter, still);
                                    if(argbValues[p] == 0 && argbValues[(p / 4) + 3] != 7)
                                    {

                                        int sp = slopes[slope];
                                        if(wcurrent[mod_color][sp][(i / 4 + 3) + j * 16] != 0)
                                        {
                                            barePositions[p] = !(VoxelLogic.wcolors[current_color][3] == VoxelLogic.bordered_alpha || VoxelLogic.wcolors[current_color][3] == VoxelLogic.bordered_flat_alpha);
                                            if(VoxelLogic.wcolors[current_color][3] == VoxelLogic.bordered_alpha || VoxelLogic.wcolors[current_color][3] == VoxelLogic.bordered_flat_alpha)
                                                zbuffer[p] = vx.z + vx.x * 3 - vx.y * 3;

                                            argbValues[p] = wcurrent[mod_color][sp][i + j * 16];
                                        }

                                        if(!barePositions[p] && outlineValues[p] == 0)
                                            outlineValues[p] = wcurrent[mod_color][0][i + 64];

                                    }
                                }
                            }
                        }
                        else if(current_color == 25)
                        {
                            taken[vx.x, vx.y] = true;
                            for(int j = 0; j < 4; j++)
                            {
                                for(int i = 0; i < 16; i++)
                                {
                                    p = voxelToPixelHugeW(i, j, vx.x, vx.y, vx.z, current_color, bmpData.Stride, jitter, still);

                                    if(shadowValues[p] == 0)
                                    {
                                        shadowValues[p] = wcurrent[current_color][0][i + j * 16];
                                    }
                                }
                            }
                        }
                        else
                        {
                            int mod_color = current_color;
                            if((mod_color == 27 || mod_color == VoxelLogic.wcolorcount + 4) && r.Next(7) < 2) //water
                                continue;
                            if((mod_color == 40 || mod_color == VoxelLogic.wcolorcount + 5 || mod_color == VoxelLogic.wcolorcount + 20) && r.Next(11) < 8) //rare sparks
                                continue;

                            taken[vx.x, vx.y] = true;

                            for(int j = 0; j < 4; j++)
                            {
                                for(int i = 0; i < 16; i++)
                                {
                                    p = voxelToPixelHugeW(i, j, vx.x, vx.y, vx.z, mod_color, bmpData.Stride, jitter, still);

                                    if(argbValues[p] == 0 && argbValues[(p / 4) + 3] != 7)
                                    {

                                        int sp = slopes[slope];
                                        if(wcurrent[mod_color][sp][((i / 4) * 4 + 3) + j * 16] != 0)
                                        {
                                            if(VoxelLogic.wcolors[mod_color][3] == VoxelLogic.gloss_alpha && i % 4 == 3 && r.Next(12) == 0)
                                            {
                                                argbValues[p - 3] = (byte)Math.Min(wcurrent[mod_color][sp][i - 3 + j * 16] + 160, 255);
                                                argbValues[p - 2] = (byte)Math.Min(wcurrent[mod_color][sp][i - 2 + j * 16] + 160, 255);
                                                argbValues[p - 1] = (byte)Math.Min(wcurrent[mod_color][sp][i - 1 + j * 16] + 160, 255);
                                                argbValues[p - 0] = wcurrent[mod_color][sp][i + j * 16];
                                            }
                                            else if(VoxelLogic.wcolors[mod_color][3] == VoxelLogic.grain_hard_alpha && i % 4 == 3)
                                            {
                                                float n = Simplex.FindNoiseBold(facing, vx.x + 20, vx.y + 20, vx.z);
                                                argbValues[p - 3] = (byte)Math.Min(wcurrent[mod_color][sp][i - 3 + j * 16] * n + 16 * (n - 0.8), 255);
                                                argbValues[p - 2] = (byte)Math.Min(wcurrent[mod_color][sp][i - 2 + j * 16] * n + 16 * (n - 0.8), 255);
                                                argbValues[p - 1] = (byte)Math.Min(wcurrent[mod_color][sp][i - 1 + j * 16] * n + 16 * (n - 0.8), 255);
                                                argbValues[p - 0] = wcurrent[mod_color][sp][i + j * 16];
                                            }
                                            else if(VoxelLogic.wcolors[mod_color][3] == VoxelLogic.grain_some_alpha && i % 4 == 3)
                                            {
                                                float n = Simplex.FindNoise(facing, vx.x + 20, vx.y + 20, vx.z);
                                                argbValues[p - 3] = (byte)Math.Min(wcurrent[mod_color][sp][i - 3 + j * 16] * n + 16 * (n - 0.8), 255);
                                                argbValues[p - 2] = (byte)Math.Min(wcurrent[mod_color][sp][i - 2 + j * 16] * n + 16 * (n - 0.8), 255);
                                                argbValues[p - 1] = (byte)Math.Min(wcurrent[mod_color][sp][i - 1 + j * 16] * n + 16 * (n - 0.8), 255);
                                                argbValues[p - 0] = wcurrent[mod_color][sp][i + j * 16];
                                            }
                                            else if(VoxelLogic.wcolors[mod_color][3] == VoxelLogic.grain_mild_alpha && i % 4 == 3)
                                            {
                                                float n = Simplex.FindNoiseLight(facing, vx.x + 20, vx.y + 20, vx.z);
                                                argbValues[p - 3] = (byte)Math.Min(wcurrent[mod_color][sp][i - 3 + j * 16] * n + 16 * (n - 0.8), 255);
                                                argbValues[p - 2] = (byte)Math.Min(wcurrent[mod_color][sp][i - 2 + j * 16] * n + 16 * (n - 0.8), 255);
                                                argbValues[p - 1] = (byte)Math.Min(wcurrent[mod_color][sp][i - 1 + j * 16] * n + 16 * (n - 0.8), 255);
                                                argbValues[p - 0] = wcurrent[mod_color][sp][i + j * 16];
                                            }
                                            else if(VoxelLogic.wcolors[mod_color][3] == VoxelLogic.fuzz_alpha && i % 4 == 3)
                                            {
                                                float n = Simplex.FindNoiseTight(frame % 4, facing, vx.x + 20, vx.y + 20, vx.z) + 0.3f;
                                                argbValues[p - 3] = (byte)VoxelLogic.Clamp(wcurrent[mod_color][sp][i - 3 + j * 16] * n + 24 * (n - 0.8), 1, 255);
                                                argbValues[p - 2] = (byte)VoxelLogic.Clamp(wcurrent[mod_color][sp][i - 2 + j * 16] * n + 24 * (n - 0.8), 1, 255);
                                                argbValues[p - 1] = (byte)VoxelLogic.Clamp(wcurrent[mod_color][sp][i - 1 + j * 16] * n + 24 * (n - 0.8), 1, 255);
                                                argbValues[p - 0] = wcurrent[mod_color][sp][i + j * 16];
                                            }
                                            else
                                            {
                                                argbValues[p] = wcurrent[mod_color][sp][i + j * 16];
                                            }

                                            zbuffer[p] = vx.z + vx.x * 3 - vx.y * 3;
                                            barePositions[p] = (VoxelLogic.wcolors[mod_color][3] == VoxelLogic.flash_alpha || VoxelLogic.wcolors[mod_color][3] == VoxelLogic.flash_alpha_0 ||
                                                VoxelLogic.wcolors[mod_color][3] == VoxelLogic.flash_alpha_1 || VoxelLogic.wcolors[mod_color][3] == VoxelLogic.borderless_alpha ||
                                                VoxelLogic.wcolors[mod_color][3] == VoxelLogic.flat_alpha);
                                        }

                                        if(!barePositions[p] && outlineValues[p] == 0)
                                            outlineValues[p] = wcurrent[mod_color][0][i + 64]; //(argbValues[p] * 1.2 + 2 < 255) ? (byte)(argbValues[p] * 1.2 + 2) : (byte)255;

                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            int[] xmods = new int[] { -1, 0, 1, -1, 0, 1, -1, 0, 1 }, ymods = new int[] { -1, -1, -1, 0, 0, 0, 1, 1, 1 };
            bool[,] nextTaken = new bool[xSize, ySize];
            for(int iter = 0; iter < 4; iter++)
            {
                for(int x = 1; x < xSize - 1; x++)
                {
                    for(int y = 1; y < ySize - 1; y++)
                    {
                        int ctr = 0;
                        for(int m = 0; m < 9; m++)
                        {
                            if(taken[x + xmods[m], y + ymods[m]])
                                ctr++;
                        }
                        if(ctr >= 5)
                            nextTaken[x, y] = true;
                    }
                }
                taken = nextTaken.Replicate();
            }
            for(int x = 0; x < xSize; x++)
            {
                for(int y = 0; y < ySize; y++)
                {

                    if(taken[x, y])
                    {
                        int p = 0;

                        for(int j = 0; j < 4; j++)
                        {
                            for(int i = 0; i < 16; i++)
                            {
                                p = voxelToPixelHugeW(i, j, x, y, 0, 25, bmpData.Stride, jitter, still);

                                if(shadowValues[p] == 0)
                                {
                                    shadowValues[p] = wcurrent[25][0][i + j * 16];
                                }
                            }
                        }
                    }
                }
            }
            
            for(int i = 3; i < numBytes; i += 4)
            {
                if(argbValues[i] == 7)
                    argbValues[i] = 0;
            }
            bool lightOutline = true;//!VoxelLogic.subtlePalettes.Contains(palette);
            for(int i = 3; i < numBytes; i += 4)
            {
                if(argbValues[i] > 255 * VoxelLogic.waver_alpha && barePositions[i] == false)
                {
                    bool shade = false, blacken = false;

                    /*
                    if(i - 4 >= 0 && i - 4 < argbValues.Length && argbValues[i - 4] == 0 && lightOutline) { editValues[i - 4] = 255; editValues[i - 4 - 1] = 0; editValues[i - 4 - 2] = 0; editValues[i - 4 - 3] = 0; blacken = true; } else if(i - 4 >= 0 && i - 4 < argbValues.Length && barePositions[i - 4] == false && zbuffer[i] - 7 > zbuffer[i - 4]) { editValues[i - 4] = 255; editValues[i - 4 - 1] = outlineValues[i - 1]; editValues[i - 4 - 2] = outlineValues[i - 2]; editValues[i - 4 - 3] = outlineValues[i - 3]; if(!blacken) shade = true; }
                    if(i + 4 >= 0 && i + 4 < argbValues.Length && argbValues[i + 4] == 0 && lightOutline) { editValues[i + 4] = 255; editValues[i + 4 - 1] = 0; editValues[i + 4 - 2] = 0; editValues[i + 4 - 3] = 0; blacken = true; } else if(i + 4 >= 0 && i + 4 < argbValues.Length && barePositions[i + 4] == false && zbuffer[i] - 7 > zbuffer[i + 4]) { editValues[i + 4] = 255; editValues[i + 4 - 1] = outlineValues[i - 1]; editValues[i + 4 - 2] = outlineValues[i - 2]; editValues[i + 4 - 3] = outlineValues[i - 3]; if(!blacken) shade = true; }
                    if(i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && argbValues[i - bmpData.Stride] == 0 && lightOutline) { editValues[i - bmpData.Stride] = 255; editValues[i - bmpData.Stride - 1] = 0; editValues[i - bmpData.Stride - 2] = 0; editValues[i - bmpData.Stride - 3] = 0; blacken = true; } else if(i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && barePositions[i - bmpData.Stride] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride]) { editValues[i - bmpData.Stride] = 255; editValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; if(!blacken) shade = true; }
                    if(i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && argbValues[i + bmpData.Stride] == 0 && lightOutline) { editValues[i + bmpData.Stride] = 255; editValues[i + bmpData.Stride - 1] = 0; editValues[i + bmpData.Stride - 2] = 0; editValues[i + bmpData.Stride - 3] = 0; blacken = true; } else if(i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && barePositions[i + bmpData.Stride] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride]) { editValues[i + bmpData.Stride] = 255; editValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; if(!blacken) shade = true; }
                    */

                    
                    if((i - 4 >= 0 && i - 4 < argbValues.Length) && ((argbValues[i] > 0 && argbValues[i - 4] == 0 && lightOutline) || (barePositions[i - 4] == false && zbuffer[i] - 7 > zbuffer[i - 4]))) { editValues[i - 4] = 255; editValues[i - 4 - 1] = outlineValues[i - 1]; editValues[i - 4 - 2] = outlineValues[i - 2]; editValues[i - 4 - 3] = outlineValues[i - 3]; if(!blacken) shade = true; }
                    if((i + 4 >= 0 && i + 4 < argbValues.Length) && ((argbValues[i] > 0 && argbValues[i + 4] == 0 && lightOutline) || (barePositions[i + 4] == false && zbuffer[i] - 7 > zbuffer[i + 4]))) { editValues[i + 4] = 255; editValues[i + 4 - 1] = outlineValues[i - 1]; editValues[i + 4 - 2] = outlineValues[i - 2]; editValues[i + 4 - 3] = outlineValues[i - 3]; if(!blacken) shade = true; }
                    if((i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length) && ((argbValues[i] > 0 && argbValues[i - bmpData.Stride] == 0 && lightOutline) || (barePositions[i - bmpData.Stride] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride]))) { editValues[i - bmpData.Stride] = 255; editValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; if(!blacken) shade = true; }
                    if((i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length) && ((argbValues[i] > 0 && argbValues[i + bmpData.Stride] == 0 && lightOutline) || (barePositions[i + bmpData.Stride] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride]))) { editValues[i + bmpData.Stride] = 255; editValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; if(!blacken) shade = true; }
                    


                    /*
                    if (argbValues[i] > 0 && i + 4 >= 0 && i + 4 < argbValues.Length && argbValues[i + 4] == 0 && lightOutline) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = 0; argbValues[i - 4 - 2] = 0; argbValues[i - 4 - 3] = 0; blacken = true; } else if (i + 4 >= 0 && i + 4 < argbValues.Length && barePositions[i + 4] == false && zbuffer[i] - 2 > zbuffer[i + 4]) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = outlineValues[i - 1]; argbValues[i - 4 - 2] = outlineValues[i - 2]; argbValues[i - 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i - 4 >= 0 && i - 4 < argbValues.Length && argbValues[i - 4] == 0 && lightOutline) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = 0; argbValues[i + 4 - 2] = 0; argbValues[i + 4 - 3] = 0; blacken = true; } else if (i - 4 >= 0 && i - 4 < argbValues.Length && barePositions[i - 4] == false && zbuffer[i] - 2 > zbuffer[i - 4]) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = outlineValues[i - 1]; argbValues[i + 4 - 2] = outlineValues[i - 2]; argbValues[i + 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && argbValues[i + bmpData.Stride] == 0 && lightOutline) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = 0; argbValues[i - bmpData.Stride - 2] = 0; argbValues[i - bmpData.Stride - 3] = 0; blacken = true; } else if (i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && barePositions[i + bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride]) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && argbValues[i - bmpData.Stride] == 0 && lightOutline) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = 0; argbValues[i + bmpData.Stride - 2] = 0; argbValues[i + bmpData.Stride - 3] = 0; blacken = true; } else if (i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && barePositions[i - bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride]) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    */
                    /*
                    if (argbValues[i] > 0 && i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && argbValues[i + bmpData.Stride + 4] == 0 && lightOutline) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = 0; argbValues[i - bmpData.Stride - 4 - 2] = 0; argbValues[i - bmpData.Stride - 4 - 3] = 0; blacken = true; } else if (i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && barePositions[i + bmpData.Stride + 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 4]) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && argbValues[i - bmpData.Stride - 4] == 0 && lightOutline) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = 0; argbValues[i + bmpData.Stride + 4 - 2] = 0; argbValues[i + bmpData.Stride + 4 - 3] = 0; blacken = true; } else if (i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && barePositions[i - bmpData.Stride - 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 4]) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && argbValues[i + bmpData.Stride - 4] == 0 && lightOutline) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = 0; argbValues[i - bmpData.Stride + 4 - 2] = 0; argbValues[i - bmpData.Stride + 4 - 3] = 0; blacken = true; } else if (i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && barePositions[i + bmpData.Stride - 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 4]) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && argbValues[i - bmpData.Stride + 4] == 0 && lightOutline) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = 0; argbValues[i + bmpData.Stride - 4 - 2] = 0; argbValues[i + bmpData.Stride - 4 - 3] = 0; blacken = true; } else if (i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && barePositions[i - bmpData.Stride + 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 4]) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    */
                    if(blacken)
                    {
                        editValues[i] = 255; editValues[i - 1] = 0; editValues[i - 2] = 0; editValues[i - 3] = 0;
                    }
                    else if(shade) { editValues[i] = 255; editValues[i - 1] = outlineValues[i - 1]; editValues[i - 2] = outlineValues[i - 2]; editValues[i - 3] = outlineValues[i - 3]; }
                }
            }

            for(int i = 3; i < numBytes; i += 4)
            {
                if(argbValues[i] > 0) // && argbValues[i] <= 255 * VoxelLogic.flat_alpha
                    argbValues[i] = 255;
                if(editValues[i] > 0)
                {
                    argbValues[i - 0] = editValues[i - 0];
                    argbValues[i - 1] = editValues[i - 1];
                    argbValues[i - 2] = editValues[i - 2];
                    argbValues[i - 3] = editValues[i - 3];
                }
            }
            if(!shadowless)
            {
                for(int i = 3; i < numBytes; i += 4)
                {
                    if(argbValues[i] == 0 && shadowValues[i] > 0)
                    {
                        argbValues[i - 3] = shadowValues[i - 3];
                        argbValues[i - 2] = shadowValues[i - 2];
                        argbValues[i - 1] = shadowValues[i - 1];
                        argbValues[i - 0] = shadowValues[i - 0];
                    }
                }
            }
            Marshal.Copy(argbValues, 0, ptr, numBytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            return bmp;
        }

        private static Bitmap renderWSmartMassive(FaceVoxel[,,] faces, int facing, int palette, int frame, int maxFrames, bool still, bool shadowless)
        {
            Bitmap bmp = new Bitmap(328 * 2, 408 * 2, PixelFormat.Format32bppArgb);

            // Specify a pixel format.
            PixelFormat pxf = PixelFormat.Format32bppArgb;

            // Lock the bitmap's bits.
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            // int numBytes = bmp.Width * bmp.Height * 3; 
            int numBytes = bmpData.Stride * bmp.Height;
            byte[] argbValues = new byte[numBytes];
            argbValues.Fill<byte>(0);
            byte[] shadowValues = new byte[numBytes];
            shadowValues.Fill<byte>(0);
            byte[] outlineValues = new byte[numBytes];
            outlineValues.Fill<byte>(0);
            byte[] bareValues = new byte[numBytes];
            bareValues.Fill<byte>(0);

            byte[] editValues = new byte[numBytes];
            editValues.Fill<byte>(0);

            bool[] barePositions = new bool[numBytes];
            barePositions.Fill<bool>(false);
            int xSize = 160, ySize = 160, zSize = 120;
            /*
            switch(facing)
            {
                case 0:
                    break;
                case 1:
                    for(int fz = zSize - 1; fz >= 0; fz--)
                    {
                        for(int fx = xSize - 1; fx >= 0; fx--)
                        {
                            for(int fy = 0; fy < ySize; fy++)
                            {
                                byte tempX = (byte)(fx - (xSize / 2));
                                byte tempY = (byte)(fy - (ySize / 2));
                                MagicaVoxelData mvd = new MagicaVoxelData();
                                mvd.x = (byte)((tempY) + (ySize / 2));
                                mvd.y = (byte)((tempX * -1) + (xSize / 2) - 1);
                                mvd.z = (byte)fz;
                                mvd.color = faceVoxels[fx, fy, fz].vox.color;
                                faces[mvd.x, mvd.y, fz] = new FaceVoxel(mvd, faceVoxels[fx, fy, fz].slope);
                            }
                        }
                    }
                    break;
                case 2:
                    for(int fz = zSize - 1; fz >= 0; fz--)
                    {
                        for(int fx = xSize - 1; fx >= 0; fx--)
                        {
                            for(int fy = 0; fy < ySize; fy++)
                            {
                                byte tempX = (byte)(fx - (xSize / 2));
                                byte tempY = (byte)(fy - (ySize / 2));
                                MagicaVoxelData mvd = new MagicaVoxelData();
                                mvd.x = (byte)((tempX * -1) + (xSize / 2) - 1);
                                mvd.y = (byte)((tempY * -1) + (ySize / 2) - 1);
                                mvd.z = (byte)fz;
                                mvd.color = faceVoxels[fx, fy, fz].vox.color;
                                faces[mvd.x, mvd.y, fz] = new FaceVoxel(mvd, faceVoxels[fx, fy, fz].slope);
                            }
                        }
                    }
                    break;
                case 3:
                    for(int fz = zSize - 1; fz >= 0; fz--)
                    {
                        for(int fx = xSize - 1; fx >= 0; fx--)
                        {
                            for(int fy = 0; fy < ySize; fy++)
                            {
                                byte tempX = (byte)(fx - (xSize / 2));
                                byte tempY = (byte)(fy - (ySize / 2));
                                MagicaVoxelData mvd = new MagicaVoxelData();
                                mvd.x = (byte)((tempY * -1) + (ySize / 2) - 1);
                                mvd.y = (byte)(tempX + (xSize / 2));
                                mvd.z = (byte)fz;
                                mvd.color = faceVoxels[fx, fy, fz].vox.color;
                                faces[mvd.x, mvd.y, fz] = new FaceVoxel(mvd, faceVoxels[fx, fy, fz].slope);
                            }
                        }
                    }
                    break;
            }*/
            int[] zbuffer = new int[numBytes];
            zbuffer.Fill<int>(-999);

            int jitter = (((frame % 4) % 3) + ((frame % 4) / 3)) * 2;
            if(maxFrames >= 8) jitter = ((frame % 8 > 4) ? 4 - ((frame % 8) ^ 4) : frame % 8);
//            FaceVoxel[,,] faces = FaceLogic.GetFaces(FaceLogic.VoxListToArray(vls.ToList(), xSize, ySize, zSize, 153), frame, shadowless);
            bool[,] taken = new bool[xSize, ySize];
            taken.Fill(false);
            //            foreach(MagicaVoxelData vx in vls.OrderByDescending(v => v.x * 64 - v.y + v.z * 64 * 128))
            for(int fz = zSize - 1; fz >= 0; fz--)
            {
                for(int fx = xSize - 1; fx >= 0; fx--)
                {
                    for(int fy = 0; fy < ySize; fy++)
                    {
                        if(faces[fx, fy, fz] == null) continue;
                        MagicaVoxelData vx = faces[fx, fy, fz].vox;
                        Slope slope = faces[fx, fy, fz].slope;
                        int current_color = ((255 - vx.color) % 4 == 0) ? (255 - vx.color) / 4 + VoxelLogic.wcolorcount : ((254 - vx.color) % 4 == 0) ? (253 - VoxelLogic.clear) / 4 : (253 - vx.color) / 4;
                        int p = 0;
                        if((255 - vx.color) % 4 != 0 && current_color >= VoxelLogic.wcolorcount)
                            continue;

                        if(current_color >= 21 && current_color <= 24)
                            current_color = 21 + ((current_color + frame) % 4);

                        if(current_color >= VoxelLogic.wcolorcount && current_color < VoxelLogic.wcolorcount + 4)
                            current_color = VoxelLogic.wcolorcount + ((current_color + frame) % 4);
                        if(current_color >= VoxelLogic.wcolorcount + 6 && current_color < VoxelLogic.wcolorcount + 10)
                            current_color = VoxelLogic.wcolorcount + 6 + ((current_color + frame) % 4);
                        if(current_color >= VoxelLogic.wcolorcount + 14 && current_color < VoxelLogic.wcolorcount + 18)
                            current_color = VoxelLogic.wcolorcount + 14 + ((current_color + frame) % 4);

                        if((frame % 2 != 0) && (VoxelLogic.wcolors[current_color][3] == VoxelLogic.spin_alpha_0 || VoxelLogic.wcolors[current_color][3] == VoxelLogic.flash_alpha_0))
                            continue;
                        else if((frame % 2 != 1) && (VoxelLogic.wcolors[current_color][3] == VoxelLogic.spin_alpha_1 || VoxelLogic.wcolors[current_color][3] == VoxelLogic.flash_alpha_1))
                            continue;
                        else if(VoxelLogic.wcolors[current_color][3] == 0F)
                            continue;
                        else if(VoxelLogic.wcolors[current_color][3] == VoxelLogic.eraser_alpha)
                        {

                            for(int j = 0; j < 4; j++)
                            {
                                for(int i = 3; i < 16; i += 4)
                                {
                                    p = voxelToPixelMassiveW(i, j, vx.x, vx.y, vx.z, current_color, bmpData.Stride, jitter, still);
                                    if(argbValues[p] == 0)
                                    {
                                        argbValues[p] = 7;
                                    }
                                }
                            }
                        }
                        else if(current_color >= 17 && current_color <= 20)
                        {
                            int mod_color = current_color;
                            if(mod_color == 17 && r.Next(7) < 3) //smoke
                                continue;
                            if(current_color == 18) //yellow fire
                            {
                                if(r.Next(3) > 0)
                                {
                                    mod_color += r.Next(3);
                                }
                            }
                            else if(current_color == 19) // orange fire
                            {
                                if(r.Next(5) < 4)
                                {
                                    mod_color -= Math.Min(r.Next(3), r.Next(3));
                                }
                            }
                            else if(current_color == 20) // sparks
                            {
                                if(r.Next(5) > 0)
                                {
                                    mod_color -= r.Next(3);
                                }
                            }

                            /*
                             &&
                                bareValues[4 * ((vx.x + vx.y) * 2 + 4 + ((current_color == 136) ? jitter - 1 : 0))
                                + i +
                                bmpData.Stride * (300 - 60 - vx.y + vx.x - vx.z * 3 - ((VoxelLogic.xcolors[current_color][3] == VoxelLogic.flat_alpha) ? -2 : jitter) + j)] == 0
                             */
                            for(int j = 0; j < 4; j++)
                            {
                                for(int i = 0; i < 16; i++)
                                {
                                    p = voxelToPixelMassiveW(i, j, vx.x, vx.y, vx.z, mod_color, bmpData.Stride, jitter, still);
                                    if(argbValues[p] == 0 && argbValues[(p / 4) + 3] != 7)
                                    {

                                        int sp = slopes[slope];
                                        if(wcurrent[mod_color][sp][(i / 4 + 3) + j * 16] != 0)
                                        {
                                            barePositions[p] = !(VoxelLogic.wcolors[current_color][3] == VoxelLogic.bordered_alpha || VoxelLogic.wcolors[current_color][3] == VoxelLogic.bordered_flat_alpha);
                                            if(VoxelLogic.wcolors[current_color][3] == VoxelLogic.bordered_alpha || VoxelLogic.wcolors[current_color][3] == VoxelLogic.bordered_flat_alpha)
                                                zbuffer[p] = vx.z + vx.x * 3 - vx.y * 3;

                                            argbValues[p] = wcurrent[mod_color][sp][i + j * 16];
                                        }

                                        if(!barePositions[p] && outlineValues[p] == 0)
                                            outlineValues[p] = wcurrent[mod_color][0][i + 64];

                                    }
                                }
                            }
                        }
                        else if(current_color == 25)
                        {
                            taken[vx.x, vx.y] = true;
                            for(int j = 0; j < 4; j++)
                            {
                                for(int i = 0; i < 16; i++)
                                {
                                    p = voxelToPixelMassiveW(i, j, vx.x, vx.y, vx.z, current_color, bmpData.Stride, jitter, still);

                                    if(shadowValues[p] == 0)
                                    {
                                        shadowValues[p] = wcurrent[current_color][0][i + j * 16];
                                    }
                                }
                            }
                        }
                        else
                        {
                            int mod_color = current_color;
                            if((mod_color == 27 || mod_color == VoxelLogic.wcolorcount + 4) && r.Next(7) < 2) //water
                                continue;
                            if((mod_color == 40 || mod_color == VoxelLogic.wcolorcount + 5 || mod_color == VoxelLogic.wcolorcount + 20) && r.Next(11) < 8) //rare sparks
                                continue;

                            taken[vx.x, vx.y] = true;

                            for(int j = 0; j < 4; j++)
                            {
                                for(int i = 0; i < 16; i++)
                                {
                                    p = voxelToPixelMassiveW(i, j, vx.x, vx.y, vx.z, mod_color, bmpData.Stride, jitter, still);

                                    if(argbValues[p] == 0 && argbValues[(p / 4) + 3] != 7)
                                    {

                                        int sp = slopes[slope];
                                        if(wcurrent[mod_color][sp][((i / 4) * 4 + 3) + j * 16] != 0)
                                        {
                                            if(VoxelLogic.wcolors[mod_color][3] == VoxelLogic.gloss_alpha && i % 4 == 3 && r.Next(12) == 0)
                                            {
                                                argbValues[p - 3] = (byte)Math.Min(wcurrent[mod_color][sp][i - 3 + j * 16] + 160, 255);
                                                argbValues[p - 2] = (byte)Math.Min(wcurrent[mod_color][sp][i - 2 + j * 16] + 160, 255);
                                                argbValues[p - 1] = (byte)Math.Min(wcurrent[mod_color][sp][i - 1 + j * 16] + 160, 255);
                                                argbValues[p - 0] = wcurrent[mod_color][sp][i + j * 16];
                                            }
                                            else if(VoxelLogic.wcolors[mod_color][3] == VoxelLogic.grain_hard_alpha && i % 4 == 3)
                                            {
                                                float n = Simplex.FindNoiseBold(facing, vx.x + 0, vx.y + 0, vx.z);
                                                argbValues[p - 3] = (byte)Math.Min(wcurrent[mod_color][sp][i - 3 + j * 16] * n + 16 * (n - 0.8), 255);
                                                argbValues[p - 2] = (byte)Math.Min(wcurrent[mod_color][sp][i - 2 + j * 16] * n + 16 * (n - 0.8), 255);
                                                argbValues[p - 1] = (byte)Math.Min(wcurrent[mod_color][sp][i - 1 + j * 16] * n + 16 * (n - 0.8), 255);
                                                argbValues[p - 0] = wcurrent[mod_color][sp][i + j * 16];
                                            }
                                            else if(VoxelLogic.wcolors[mod_color][3] == VoxelLogic.grain_some_alpha && i % 4 == 3)
                                            {
                                                float n = Simplex.FindNoise(facing, vx.x + 0, vx.y + 0, vx.z);
                                                argbValues[p - 3] = (byte)Math.Min(wcurrent[mod_color][sp][i - 3 + j * 16] * n + 16 * (n - 0.8), 255);
                                                argbValues[p - 2] = (byte)Math.Min(wcurrent[mod_color][sp][i - 2 + j * 16] * n + 16 * (n - 0.8), 255);
                                                argbValues[p - 1] = (byte)Math.Min(wcurrent[mod_color][sp][i - 1 + j * 16] * n + 16 * (n - 0.8), 255);
                                                argbValues[p - 0] = wcurrent[mod_color][sp][i + j * 16];
                                            }
                                            else if(VoxelLogic.wcolors[mod_color][3] == VoxelLogic.grain_mild_alpha && i % 4 == 3)
                                            {
                                                float n = Simplex.FindNoiseLight(facing, vx.x + 0, vx.y + 0, vx.z);
                                                argbValues[p - 3] = (byte)Math.Min(wcurrent[mod_color][sp][i - 3 + j * 16] * n + 16 * (n - 0.8), 255);
                                                argbValues[p - 2] = (byte)Math.Min(wcurrent[mod_color][sp][i - 2 + j * 16] * n + 16 * (n - 0.8), 255);
                                                argbValues[p - 1] = (byte)Math.Min(wcurrent[mod_color][sp][i - 1 + j * 16] * n + 16 * (n - 0.8), 255);
                                                argbValues[p - 0] = wcurrent[mod_color][sp][i + j * 16];
                                            }
                                            else if(VoxelLogic.wcolors[mod_color][3] == VoxelLogic.fuzz_alpha && i % 4 == 3)
                                            {
                                                float n = Simplex.FindNoiseTight(frame % 4, facing, vx.x + 0, vx.y + 0, vx.z) + 0.3f;
                                                argbValues[p - 3] = (byte)VoxelLogic.Clamp(wcurrent[mod_color][sp][i - 3 + j * 16] * n + 24 * (n - 0.8), 1, 255);
                                                argbValues[p - 2] = (byte)VoxelLogic.Clamp(wcurrent[mod_color][sp][i - 2 + j * 16] * n + 24 * (n - 0.8), 1, 255);
                                                argbValues[p - 1] = (byte)VoxelLogic.Clamp(wcurrent[mod_color][sp][i - 1 + j * 16] * n + 24 * (n - 0.8), 1, 255);
                                                argbValues[p - 0] = wcurrent[mod_color][sp][i + j * 16];
                                            }
                                            else
                                            {
                                                argbValues[p] = wcurrent[mod_color][sp][i + j * 16];
                                            }

                                            zbuffer[p] = vx.z + vx.x * 3 - vx.y * 3;
                                            barePositions[p] = (VoxelLogic.wcolors[mod_color][3] == VoxelLogic.flash_alpha || VoxelLogic.wcolors[mod_color][3] == VoxelLogic.flash_alpha_0 ||
                                                VoxelLogic.wcolors[mod_color][3] == VoxelLogic.flash_alpha_1 || VoxelLogic.wcolors[mod_color][3] == VoxelLogic.borderless_alpha ||
                                                VoxelLogic.wcolors[mod_color][3] == VoxelLogic.flat_alpha);
                                        }
                                        if(!barePositions[p] && outlineValues[p] == 0)
                                            outlineValues[p] = wcurrent[mod_color][0][i + 64]; //(argbValues[p] * 1.2 + 2 < 255) ? (byte)(argbValues[p] * 1.2 + 2) : (byte)255;

                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            int[] xmods = new int[] { -1, 0, 1, -1, 0, 1, -1, 0, 1 }, ymods = new int[] { -1, -1, -1, 0, 0, 0, 1, 1, 1 };
            bool[,] nextTaken = new bool[xSize, ySize];
            for(int iter = 0; iter < 4; iter++)
            {
                for(int x = 1; x < xSize - 1; x++)
                {
                    for(int y = 1; y < ySize - 1; y++)
                    {
                        int ctr = 0;
                        for(int m = 0; m < 9; m++)
                        {
                            if(taken[x + xmods[m], y + ymods[m]])
                                ctr++;
                        }
                        if(ctr >= 5)
                            nextTaken[x, y] = true;

                    }
                }
                taken = nextTaken.Replicate();
            }
            for(int x = 0; x < xSize; x++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    if(taken[x, y])
                    {
                        int p = 0;

                        for(int j = 0; j < 4; j++)
                        {
                            for(int i = 0; i < 16; i++)
                            {
                                p = voxelToPixelMassiveW(i, j, x, y, 0, 25, bmpData.Stride, jitter, still);

                                if(shadowValues[p] == 0)
                                {
                                    shadowValues[p] = wcurrent[25][0][i + j * 16];
                                }
                            }
                        }
                    }
                }
            }
            
            for(int i = 3; i < numBytes; i += 4)
            {
                if(argbValues[i] == 7)
                    argbValues[i] = 0;
            }
            bool lightOutline = true;//!VoxelLogic.subtlePalettes.Contains(palette);
            for(int i = 3; i < numBytes; i += 4)
            {
                if(argbValues[i] > 255 * VoxelLogic.waver_alpha && barePositions[i] == false)
                {
                    bool shade = false, blacken = false;

                    /*
                    if(i - 4 >= 0 && i - 4 < argbValues.Length && argbValues[i - 4] == 0 && lightOutline) { editValues[i - 4] = 255; editValues[i - 4 - 1] = 0; editValues[i - 4 - 2] = 0; editValues[i - 4 - 3] = 0; blacken = true; } else if(i - 4 >= 0 && i - 4 < argbValues.Length && barePositions[i - 4] == false && zbuffer[i] - 7 > zbuffer[i - 4]) { editValues[i - 4] = 255; editValues[i - 4 - 1] = outlineValues[i - 1]; editValues[i - 4 - 2] = outlineValues[i - 2]; editValues[i - 4 - 3] = outlineValues[i - 3]; if(!blacken) shade = true; }
                    if(i + 4 >= 0 && i + 4 < argbValues.Length && argbValues[i + 4] == 0 && lightOutline) { editValues[i + 4] = 255; editValues[i + 4 - 1] = 0; editValues[i + 4 - 2] = 0; editValues[i + 4 - 3] = 0; blacken = true; } else if(i + 4 >= 0 && i + 4 < argbValues.Length && barePositions[i + 4] == false && zbuffer[i] - 7 > zbuffer[i + 4]) { editValues[i + 4] = 255; editValues[i + 4 - 1] = outlineValues[i - 1]; editValues[i + 4 - 2] = outlineValues[i - 2]; editValues[i + 4 - 3] = outlineValues[i - 3]; if(!blacken) shade = true; }
                    if(i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && argbValues[i - bmpData.Stride] == 0 && lightOutline) { editValues[i - bmpData.Stride] = 255; editValues[i - bmpData.Stride - 1] = 0; editValues[i - bmpData.Stride - 2] = 0; editValues[i - bmpData.Stride - 3] = 0; blacken = true; } else if(i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && barePositions[i - bmpData.Stride] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride]) { editValues[i - bmpData.Stride] = 255; editValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; if(!blacken) shade = true; }
                    if(i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && argbValues[i + bmpData.Stride] == 0 && lightOutline) { editValues[i + bmpData.Stride] = 255; editValues[i + bmpData.Stride - 1] = 0; editValues[i + bmpData.Stride - 2] = 0; editValues[i + bmpData.Stride - 3] = 0; blacken = true; } else if(i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && barePositions[i + bmpData.Stride] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride]) { editValues[i + bmpData.Stride] = 255; editValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; if(!blacken) shade = true; }
                    */
                    
                    if((i - 4 >= 0 && i - 4 < argbValues.Length) && ((argbValues[i] > 0 && argbValues[i - 4] == 0 && lightOutline) || (barePositions[i - 4] == false && zbuffer[i] - 7 > zbuffer[i - 4]))) { editValues[i - 4] = 255; editValues[i - 4 - 1] = outlineValues[i - 1]; editValues[i - 4 - 2] = outlineValues[i - 2]; editValues[i - 4 - 3] = outlineValues[i - 3]; if(!blacken) shade = true; }
                    if((i + 4 >= 0 && i + 4 < argbValues.Length) && ((argbValues[i] > 0 && argbValues[i + 4] == 0 && lightOutline) || (barePositions[i + 4] == false && zbuffer[i] - 7 > zbuffer[i + 4]))) { editValues[i + 4] = 255; editValues[i + 4 - 1] = outlineValues[i - 1]; editValues[i + 4 - 2] = outlineValues[i - 2]; editValues[i + 4 - 3] = outlineValues[i - 3]; if(!blacken) shade = true; }
                    if((i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length) && ((argbValues[i] > 0 && argbValues[i - bmpData.Stride] == 0 && lightOutline) || (barePositions[i - bmpData.Stride] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride]))) { editValues[i - bmpData.Stride] = 255; editValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; if(!blacken) shade = true; }
                    if((i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length) && ((argbValues[i] > 0 && argbValues[i + bmpData.Stride] == 0 && lightOutline) || (barePositions[i + bmpData.Stride] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride]))) { editValues[i + bmpData.Stride] = 255; editValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; if(!blacken) shade = true; }
                    



                    /*
                    if (argbValues[i] > 0 && i + 4 >= 0 && i + 4 < argbValues.Length && argbValues[i + 4] == 0 && lightOutline) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = 0; argbValues[i - 4 - 2] = 0; argbValues[i - 4 - 3] = 0; blacken = true; } else if (i + 4 >= 0 && i + 4 < argbValues.Length && barePositions[i + 4] == false && zbuffer[i] - 2 > zbuffer[i + 4]) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = outlineValues[i - 1]; argbValues[i - 4 - 2] = outlineValues[i - 2]; argbValues[i - 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i - 4 >= 0 && i - 4 < argbValues.Length && argbValues[i - 4] == 0 && lightOutline) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = 0; argbValues[i + 4 - 2] = 0; argbValues[i + 4 - 3] = 0; blacken = true; } else if (i - 4 >= 0 && i - 4 < argbValues.Length && barePositions[i - 4] == false && zbuffer[i] - 2 > zbuffer[i - 4]) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = outlineValues[i - 1]; argbValues[i + 4 - 2] = outlineValues[i - 2]; argbValues[i + 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && argbValues[i + bmpData.Stride] == 0 && lightOutline) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = 0; argbValues[i - bmpData.Stride - 2] = 0; argbValues[i - bmpData.Stride - 3] = 0; blacken = true; } else if (i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && barePositions[i + bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride]) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && argbValues[i - bmpData.Stride] == 0 && lightOutline) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = 0; argbValues[i + bmpData.Stride - 2] = 0; argbValues[i + bmpData.Stride - 3] = 0; blacken = true; } else if (i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && barePositions[i - bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride]) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    */
                    /*
                    if (argbValues[i] > 0 && i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && argbValues[i + bmpData.Stride + 4] == 0 && lightOutline) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = 0; argbValues[i - bmpData.Stride - 4 - 2] = 0; argbValues[i - bmpData.Stride - 4 - 3] = 0; blacken = true; } else if (i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && barePositions[i + bmpData.Stride + 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 4]) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && argbValues[i - bmpData.Stride - 4] == 0 && lightOutline) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = 0; argbValues[i + bmpData.Stride + 4 - 2] = 0; argbValues[i + bmpData.Stride + 4 - 3] = 0; blacken = true; } else if (i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && barePositions[i - bmpData.Stride - 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 4]) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && argbValues[i + bmpData.Stride - 4] == 0 && lightOutline) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = 0; argbValues[i - bmpData.Stride + 4 - 2] = 0; argbValues[i - bmpData.Stride + 4 - 3] = 0; blacken = true; } else if (i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && barePositions[i + bmpData.Stride - 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 4]) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && argbValues[i - bmpData.Stride + 4] == 0 && lightOutline) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = 0; argbValues[i + bmpData.Stride - 4 - 2] = 0; argbValues[i + bmpData.Stride - 4 - 3] = 0; blacken = true; } else if (i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && barePositions[i - bmpData.Stride + 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 4]) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    */
                    if(blacken)
                    {
                        editValues[i] = 255; editValues[i - 1] = 0; editValues[i - 2] = 0; editValues[i - 3] = 0;
                    }
                    else if(shade) { editValues[i] = 255; editValues[i - 1] = outlineValues[i - 1]; editValues[i - 2] = outlineValues[i - 2]; editValues[i - 3] = outlineValues[i - 3]; }
                }
            }

            for(int i = 3; i < numBytes; i += 4)
            {
                if(argbValues[i] > 0) // && argbValues[i] <= 255 * VoxelLogic.flat_alpha
                    argbValues[i] = 255;
                if(editValues[i] > 0)
                {
                    argbValues[i - 0] = editValues[i - 0];
                    argbValues[i - 1] = editValues[i - 1];
                    argbValues[i - 2] = editValues[i - 2];
                    argbValues[i - 3] = editValues[i - 3];
                }
            }
            if(!shadowless)
            {
                for(int i = 3; i < numBytes; i += 4)
                {
                    if(argbValues[i] == 0 && shadowValues[i] > 0)
                    {
                        argbValues[i - 3] = shadowValues[i - 3];
                        argbValues[i - 2] = shadowValues[i - 2];
                        argbValues[i - 1] = shadowValues[i - 1];
                        argbValues[i - 0] = shadowValues[i - 0];
                    }
                }
            }
            Marshal.Copy(argbValues, 0, ptr, numBytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            return bmp;
        }

        private static Bitmap generateWaterMask(int facing, int variant)
        {
            Bitmap bmp = new Bitmap(248 * 2, 308 * 2, PixelFormat.Format32bppArgb);

            // Specify a pixel format.
            PixelFormat pxf = PixelFormat.Format32bppArgb;

            // Lock the bitmap's bits.
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            // int numBytes = bmp.Width * bmp.Height * 3; 
            int numBytes = bmpData.Stride * bmp.Height;
            byte[] argbValues = new byte[numBytes];
            argbValues.Fill<byte>(0);
            byte[] outlineValues = new byte[numBytes];
            outlineValues.Fill<byte>(0);
            byte[] bareValues = new byte[numBytes];
            bareValues.Fill<byte>(0);

            byte[] editValues = new byte[numBytes];
            editValues.Fill<byte>(0);

            bool[] barePositions = new bool[numBytes];
            barePositions.Fill<bool>(false);

            int[] zbuffer = new int[numBytes];
            zbuffer.Fill<int>(-999);
            int sp = 0;
            for(int mvdx = 83; mvdx >= 36; mvdx--)
            {
                for(int mvdy = 36; mvdy <= 83; mvdy++)
                {
                    MagicaVoxelData vx = new MagicaVoxelData { x = (byte)mvdx, y = (byte)mvdy, z = 0, color = 145 };
                    int current_color = 27;
                    //int unshaded = VoxelLogic.WithoutShadingK(vx.color);
                    //int current_color = ((255 - vx.color) % 4 == 0) ? (255 - vx.color) / 4 + kcolorcount : ((254 - vx.color) % 4 == 0) ? (253 - clear) / 4 : (253 - vx.color) / 4;
                    int p = 0;
                    int mod_color = current_color;

                    for(int j = 0; j < 4; j++)
                    {
                        for(int i = 0; i < 4 * 4; i++)
                        {
                            //p = 4 * ((vx.x + vx.y) * 2 + 2) + i + bmpData.Stride * (128 + 2 - vx.y + vx.x + j);
                            p = voxelToPixelHugeW(i, j, vx.x, vx.y, 0, 27, bmpData.Stride, 0, true);
                            if(argbValues[p] == 0)
                            {
                                zbuffer[p] = vx.z + vx.x * 3 - vx.y * 3;
                                mod_color = current_color;
                                if(i % 4 == 3)
                                {
                                    double wave = Simplex.FindNoiseFlatWaterHuge(facing, vx.x, vx.y, variant);

                                    if(wave > 0.73)
                                    {
                                        wave = 85 * wave;
                                    }
                                    else if(wave > 0.64)
                                    {
                                        wave = 65 * wave;
                                    }
                                    else if(wave > 0.55)
                                    {
                                        wave = 50 * wave;
                                    }
                                    else if(wave < 0.45)
                                    {

                                        wave += 0.2;
                                        if(wave < 0.5)
                                        {
                                            wave = -15 / wave;
                                        }
                                        else if(wave < 0.55)
                                        {
                                            wave = -11 / wave;
                                        }
                                        else if(wave < 0.6)
                                        {
                                            wave = -7 / wave;
                                        }
                                        else
                                        {
                                            wave = 6.0 * (wave - 0.25);
                                        }
                                    }
                                    else
                                    {
                                        wave = 32.0 * wave;
                                    }

                                    argbValues[p - 3] = (byte)VoxelLogic.Clamp(wcurrent[mod_color][sp][i - 3 + j * 16] + wave, 1, 255);
                                    argbValues[p - 2] = (byte)VoxelLogic.Clamp(wcurrent[mod_color][sp][i - 2 + j * 16] + wave, 1, 255);
                                    argbValues[p - 1] = (byte)VoxelLogic.Clamp(wcurrent[mod_color][sp][i - 1 + j * 16] + wave, 1, 255);
                                    argbValues[p - 0] = 255;
                                }
                                /*
                                else
                                {
                                    argbValues[p] = kcurrent[mod_color][i + j * (vwidth * 4)];
                                }*/
                                if(outlineValues[p] == 0)
                                    outlineValues[p] = wcurrent[mod_color][0][i + 64];
                            }
                        }

                    }
                }
            }
            bool lightOutline = true;//!VoxelLogic.subtlePalettes.Contains(palette);
            for(int i = 3; i < numBytes; i += 4)
            {
                if(argbValues[i] > 255 * VoxelLogic.waver_alpha && barePositions[i] == false)
                {
                    bool shade = false, blacken = false;

                    /*
                    if(i - 4 >= 0 && i - 4 < argbValues.Length && argbValues[i - 4] == 0 && lightOutline) { editValues[i - 4] = 255; editValues[i - 4 - 1] = 0; editValues[i - 4 - 2] = 0; editValues[i - 4 - 3] = 0; blacken = true; } else if(i - 4 >= 0 && i - 4 < argbValues.Length && barePositions[i - 4] == false && zbuffer[i] - 7 > zbuffer[i - 4]) { editValues[i - 4] = 255; editValues[i - 4 - 1] = outlineValues[i - 1]; editValues[i - 4 - 2] = outlineValues[i - 2]; editValues[i - 4 - 3] = outlineValues[i - 3]; if(!blacken) shade = true; }
                    if(i + 4 >= 0 && i + 4 < argbValues.Length && argbValues[i + 4] == 0 && lightOutline) { editValues[i + 4] = 255; editValues[i + 4 - 1] = 0; editValues[i + 4 - 2] = 0; editValues[i + 4 - 3] = 0; blacken = true; } else if(i + 4 >= 0 && i + 4 < argbValues.Length && barePositions[i + 4] == false && zbuffer[i] - 7 > zbuffer[i + 4]) { editValues[i + 4] = 255; editValues[i + 4 - 1] = outlineValues[i - 1]; editValues[i + 4 - 2] = outlineValues[i - 2]; editValues[i + 4 - 3] = outlineValues[i - 3]; if(!blacken) shade = true; }
                    if(i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && argbValues[i - bmpData.Stride] == 0 && lightOutline) { editValues[i - bmpData.Stride] = 255; editValues[i - bmpData.Stride - 1] = 0; editValues[i - bmpData.Stride - 2] = 0; editValues[i - bmpData.Stride - 3] = 0; blacken = true; } else if(i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && barePositions[i - bmpData.Stride] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride]) { editValues[i - bmpData.Stride] = 255; editValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; if(!blacken) shade = true; }
                    if(i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && argbValues[i + bmpData.Stride] == 0 && lightOutline) { editValues[i + bmpData.Stride] = 255; editValues[i + bmpData.Stride - 1] = 0; editValues[i + bmpData.Stride - 2] = 0; editValues[i + bmpData.Stride - 3] = 0; blacken = true; } else if(i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && barePositions[i + bmpData.Stride] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride]) { editValues[i + bmpData.Stride] = 255; editValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; if(!blacken) shade = true; }
                    */

                    
                    if((i - 4 >= 0 && i - 4 < argbValues.Length) && ((argbValues[i] > 0 && argbValues[i - 4] == 0 && lightOutline) || (barePositions[i - 4] == false && zbuffer[i] - 7 > zbuffer[i - 4]))) { editValues[i - 4] = 255; editValues[i - 4 - 1] = outlineValues[i - 1]; editValues[i - 4 - 2] = outlineValues[i - 2]; editValues[i - 4 - 3] = outlineValues[i - 3]; if(!blacken) shade = true; }
                    if((i + 4 >= 0 && i + 4 < argbValues.Length) && ((argbValues[i] > 0 && argbValues[i + 4] == 0 && lightOutline) || (barePositions[i + 4] == false && zbuffer[i] - 7 > zbuffer[i + 4]))) { editValues[i + 4] = 255; editValues[i + 4 - 1] = outlineValues[i - 1]; editValues[i + 4 - 2] = outlineValues[i - 2]; editValues[i + 4 - 3] = outlineValues[i - 3]; if(!blacken) shade = true; }
                    if((i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length) && ((argbValues[i] > 0 && argbValues[i - bmpData.Stride] == 0 && lightOutline) || (barePositions[i - bmpData.Stride] == false && zbuffer[i] - 7 > zbuffer[i - bmpData.Stride]))) { editValues[i - bmpData.Stride] = 255; editValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; if(!blacken) shade = true; }
                    if((i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length) && ((argbValues[i] > 0 && argbValues[i + bmpData.Stride] == 0 && lightOutline) || (barePositions[i + bmpData.Stride] == false && zbuffer[i] - 7 > zbuffer[i + bmpData.Stride]))) { editValues[i + bmpData.Stride] = 255; editValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; if(!blacken) shade = true; }
                    


                    /*
                    if (argbValues[i] > 0 && i + 4 >= 0 && i + 4 < argbValues.Length && argbValues[i + 4] == 0 && lightOutline) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = 0; argbValues[i - 4 - 2] = 0; argbValues[i - 4 - 3] = 0; blacken = true; } else if (i + 4 >= 0 && i + 4 < argbValues.Length && barePositions[i + 4] == false && zbuffer[i] - 2 > zbuffer[i + 4]) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = outlineValues[i - 1]; argbValues[i - 4 - 2] = outlineValues[i - 2]; argbValues[i - 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i - 4 >= 0 && i - 4 < argbValues.Length && argbValues[i - 4] == 0 && lightOutline) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = 0; argbValues[i + 4 - 2] = 0; argbValues[i + 4 - 3] = 0; blacken = true; } else if (i - 4 >= 0 && i - 4 < argbValues.Length && barePositions[i - 4] == false && zbuffer[i] - 2 > zbuffer[i - 4]) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = outlineValues[i - 1]; argbValues[i + 4 - 2] = outlineValues[i - 2]; argbValues[i + 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && argbValues[i + bmpData.Stride] == 0 && lightOutline) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = 0; argbValues[i - bmpData.Stride - 2] = 0; argbValues[i - bmpData.Stride - 3] = 0; blacken = true; } else if (i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && barePositions[i + bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride]) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && argbValues[i - bmpData.Stride] == 0 && lightOutline) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = 0; argbValues[i + bmpData.Stride - 2] = 0; argbValues[i + bmpData.Stride - 3] = 0; blacken = true; } else if (i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && barePositions[i - bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride]) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    */
                    /*
                    if (argbValues[i] > 0 && i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && argbValues[i + bmpData.Stride + 4] == 0 && lightOutline) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = 0; argbValues[i - bmpData.Stride - 4 - 2] = 0; argbValues[i - bmpData.Stride - 4 - 3] = 0; blacken = true; } else if (i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && barePositions[i + bmpData.Stride + 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 4]) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && argbValues[i - bmpData.Stride - 4] == 0 && lightOutline) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = 0; argbValues[i + bmpData.Stride + 4 - 2] = 0; argbValues[i + bmpData.Stride + 4 - 3] = 0; blacken = true; } else if (i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && barePositions[i - bmpData.Stride - 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 4]) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && argbValues[i + bmpData.Stride - 4] == 0 && lightOutline) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = 0; argbValues[i - bmpData.Stride + 4 - 2] = 0; argbValues[i - bmpData.Stride + 4 - 3] = 0; blacken = true; } else if (i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && barePositions[i + bmpData.Stride - 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 4]) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && argbValues[i - bmpData.Stride + 4] == 0 && lightOutline) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = 0; argbValues[i + bmpData.Stride - 4 - 2] = 0; argbValues[i + bmpData.Stride - 4 - 3] = 0; blacken = true; } else if (i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && barePositions[i - bmpData.Stride + 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 4]) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    */
                    /*
                    if(blacken)
                    {
                        editValues[i] = 255; editValues[i - 1] = 0; editValues[i - 2] = 0; editValues[i - 3] = 0;
                    }
                    else if(shade) { editValues[i] = 255; editValues[i - 1] = outlineValues[i - 1]; editValues[i - 2] = outlineValues[i - 2]; editValues[i - 3] = outlineValues[i - 3]; }
                    */
                }
            }

            for(int i = 3; i < numBytes; i += 4)
            {
                if(argbValues[i] > 0) // && argbValues[i] <= 255 * VoxelLogic.flat_alpha
                    argbValues[i] = 255;
                /*if(editValues[i] > 0)
                {
                    argbValues[i - 0] = editValues[i - 0];
                    argbValues[i - 1] = editValues[i - 1];
                    argbValues[i - 2] = editValues[i - 2];
                    argbValues[i - 3] = editValues[i - 3];
                }*/
            }
            Marshal.Copy(argbValues, 0, ptr, numBytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            return bmp;
        }


        public static void processWater()
        {
            Console.WriteLine("Processing: Water Tiles, palette " + 0);
            // int framelimit = 4;

            VoxelLogic.wcolors = VoxelLogic.wpalettes[48];
            wcurrent = wrendered[48];


            string folder = altFolder;
            Directory.CreateDirectory(folder);
            for(int v = 0; v < 16; v++)
            { //
                for(int dir = 0; dir < 4; dir++)
                {
                    Bitmap b;
                    Bitmap b2 = new Bitmap(168, 208, PixelFormat.Format32bppArgb);
                    
                    b = b = generateWaterMask(dir, v);
                    //            return b;

                    Graphics g2 = Graphics.FromImage(b2);
                    g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    g2.DrawImage(b.Clone(new Rectangle(0, 0, 248 * 2, 308 * 2), b.PixelFormat), -40, -80, 248, 308);
                    g2.Dispose();
                    b2.Save(folder + "/palette" + 0 + "_Water_Huge_face" + dir + "_" + string.Format("{0:x}", v) + ".png", ImageFormat.Png);
                    b.Dispose();
                    b2.Dispose();
                }
            }
        }


        public static Bitmap makeFlatTiling()
        {
            Bitmap b = new Bitmap(48 * 4 * 8, 12 * 4 * 16);
            Graphics g = Graphics.FromImage(b);

            Bitmap[] tilings = new Bitmap[11];

            BinaryReader bin = new BinaryReader(File.Open("Terrain_Huge_W.vox", FileMode.Open));
            FaceVoxel[,,] voxes = FaceLogic.FaceListToArray(VoxelLogic.FromMagicaRaw(bin).Select(m => new FaceVoxel(m, Slope.Cube)).ToList(), 80, 80, 60, 153);
            for(int i = 0; i < 11; i++)
            {
                wcurrent = wrendered[VoxelLogic.wpalettecount - 11 + i];
                tilings[i] = renderWSmartHuge(voxes, 0, VoxelLogic.wpalettecount - 11 + i, 0, 1, true, true);
            }
            int[,] grid = new int[9, 17];

            int[,] takenLocations = new int[9, 17];
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 17; j++)
                {
                    takenLocations[i, j] = 0;
                    grid[i, j] = 0;
                    if(i > 0 && i < 8 && j > 0 && j < 16 && r.Next(3) > 0)
                    {
                        grid[i, j] = r.Next(11);
                    }
                }
            }
            /*
            int numMountains = r.Next(17, 30);
            int iter = 0;
            int rx = r.Next(8) + 1, ry = r.Next(15) + 2;
            do
            {
                if(takenLocations[rx, ry] < 1 && r.Next(6) > 0 && ((ry + 1) / 2 != ry))
                {
                    takenLocations[rx, ry] = 2;
                    grid[rx, ry] = r.Next(9) + 1;
                    int ydir = ((ry + 1) / 2 > ry) ? 1 : -1;
                    int xdir = (ry % 2 == 0) ? rx + r.Next(2) : rx - r.Next(2);
                    if(xdir <= 1) xdir++;
                    if(xdir >= 9) xdir--;
                    rx = xdir;
                    ry = ry + ydir;

                }
                else
                {
                    rx = r.Next(8) + 1;
                    ry = r.Next(15) + 2;
                }
                iter++;
            } while(iter < numMountains);

            int extreme = 0;
            switch(r.Next(5))
            {
                case 0:
                    extreme = 7;
                    break;
                case 1:
                    extreme = 2;
                    break;
                case 2:
                    extreme = 2;
                    break;
                case 3:
                    extreme = 1;
                    break;
                case 4:
                    extreme = 1;
                    break;
            }
            */
            /*
            for(int i = 1; i < 8; i++)
            {
                for(int j = 2; j < 15; j++)
                {
                    for(int v = 0; v < 3; v++)
                    {

                        int[] adj = { 0, 0, 0, 0,
                                        0,0,0,0,
                                    0, 0, 0, 0, };
                        adj[0] = grid[i, j + 1];
                        adj[1] = grid[i, j - 1];
                        if(j % 2 == 0)
                        {
                            adj[2] = grid[i + 1, j + 1];
                            adj[3] = grid[i + 1, j - 1];
                        }
                        else
                        {
                            adj[2] = grid[i - 1, j + 1];
                            adj[3] = grid[i - 1, j - 1];
                        }
                        adj[4] = grid[i, j + 2];
                        adj[5] = grid[i, j - 2];
                        adj[6] = grid[i + 1, j];
                        adj[7] = grid[i - 1, j];
                        int likeliest = 0;
                        if(!adj.Contains(1) && extreme == 2 && r.Next(5) > 1)
                            likeliest = extreme;
                        if((adj.Contains(2) && r.Next(4) == 0))
                            likeliest = extreme;
                        if(extreme == 7 && (r.Next(4) == 0) || (adj.Contains(7) && r.Next(3) > 0))
                            likeliest = extreme;
                        if((adj.Contains(1) && r.Next(5) > 1) || r.Next(4) == 0)
                            likeliest = r.Next(2) * 2 + 1;
                        if(adj.Contains(5) && r.Next(3) == 0)
                            likeliest = r.Next(4, 6);
                        if(r.Next(45) == 0)
                            likeliest = 6;
                        if(takenLocations[i, j] == 0)
                        {
                            grid[i, j] = likeliest;
                        }
                    }
                }
            }
            */

            for(int j = 0; j < 17; j++)
            {
                for(int i = 0; i < 9; i++)
                {
                    g.DrawImageUnscaled(tilings[grid[i, j]], (48 * 4 * i) - ((j % 2 == 0) ? 0 : 24 * 4) - 24 * 4 + 20, (12 * 4 * j) - 12 * 2 * 17 - 40);
                }
            }
            return b;
        }

        static void Main(string[] args)
        {
            //            altFolder = "botl6/";
            //            VoxelLogic.wpalettes = AlternatePalettes.mecha_palettes;
            VoxelLogic.VisualMode = "W";
            altFolder = "Dungeon/";
            System.IO.Directory.CreateDirectory("Dungeon");

            /*
            altFolder = "Forays2/";
            System.IO.Directory.CreateDirectory("Forays2");
            VoxelLogic.voxFolder = "ForaysBones/";
            */
            //System.IO.Directory.CreateDirectory("mecha2");
            //System.IO.Directory.CreateDirectory("vox/mecha2");

            VoxelLogic.Initialize();

//            CURedux.Initialize();
            //            SaPalettes.Initialize();
            InitializeWPalette();

            //            altFolder = "sau10/";
            //            makeFlatTiling().Save("tiling_flat_gray.png");
            
            processTerrainHugeW("Floor", 48, true, false);
            processTerrainHugeW("Wall_Straight", 48, true, true);
            processTerrainHugeW("Wall_Corner", 48, true, true);
            processTerrainHugeW("Wall_Tee", 48, true, true);
            processTerrainHugeW("Wall_Cross", 48, true, true);
            processTerrainHugeW("Door_Closed", 48, true, true);
            processTerrainHugeW("Door_Open", 48, true, true);
            processTerrainHugeW("Boulder", 48, true, true);
            
            processWater();

            /*
            processUnitHugeW("Grass", 35, true, false);
            processUnitHugeW("Tree", 35, true, false);
            processUnitHugeW("Boulder", 36, true, false);
            processUnitHugeW("Rubble", 36, true, false);
            
            processUnitLargeW("Rakgar", 18, true, false);

            processUnitLargeW("Lomuk", 13, false, false);
            processUnitLargeWalkW("Lomuk", 13);

            processUnitLargeW("Axarik", 0, true, false);
            processUnitLargeWalkW("Axarik", 0);
            processUnitLargeW("Tassar", 17, false, false);
            processUnitLargeWalkW("Tassar", 17);
            processUnitLargeW("Erezdo", 2, true, false);
            processUnitLargeWalkW("Erezdo", 2);
            processUnitLargeW("Ceglia", 1, true, false);
            processUnitLargeWalkW("Ceglia", 1);

            processUnitHugeW("Nodebpe", 14, true, false);
            processUnitHugeWalkW("Nodebpe", 14);

            processUnitLargeW("Glarosp", 3, true, false);
            processUnitLargeWalkW("Glarosp", 3);

            processUnitLargeW("Ilapa", 11, true, false);
            processUnitLargeWalkW("Ilapa", 11);
            processUnitLargeW("Kurguiv", 12, false, false);
            processUnitLargeWalkW("Kurguiv", 12);
            processUnitHugeW("Oah", 15, true, false);
            processUnitHugeWalkW("Oah", 15);
            processUnitLargeW("Sfyst", 16, true, false);
            processUnitLargeWalkW("Sfyst", 16);
            processUnitLargeW("Tassar", 17, false, false);
            processUnitLargeWalkW("Tassar", 17);
            processUnitHugeW("Vashk", 18, true, false);
            processUnitHugeWalkW("Vashk", 18);
            processUnitLargeW("Meisan", 43, false, false);
            processUnitLargeWalkW("Meisan", 43);


            processUnitLargeW("Human_Male", 4, true, false);
            processUnitLargeWalkW("Human_Male", 4);
            processUnitLargeW("Human_Male", 5, true, false);
            processUnitLargeWalkW("Human_Male", 5);
            processUnitLargeW("Human_Male", 6, true, false);
            processUnitLargeWalkW("Human_Male", 6);
            processUnitLargeW("Human_Male", 7, true, false);
            processUnitLargeWalkW("Human_Male", 7);
            processUnitLargeW("Human_Male", 8, true, false);
            processUnitLargeWalkW("Human_Male", 8);
            processUnitLargeW("Human_Male", 9, true, false);
            processUnitLargeWalkW("Human_Male", 9);
            processUnitLargeW("Human_Male", 10, true, false);
            processUnitLargeWalkW("Human_Male", 10);

            processUnitLargeW("Human_Female", 4, true, false);
            processUnitLargeWalkW("Human_Female", 4);
            processUnitLargeW("Human_Female", 5, true, false);
            processUnitLargeWalkW("Human_Female", 5);
            processUnitLargeW("Human_Female", 6, true, false);
            processUnitLargeWalkW("Human_Female", 6);
            processUnitLargeW("Human_Female", 7, true, false);
            processUnitLargeWalkW("Human_Female", 7);
            processUnitLargeW("Human_Female", 8, true, false);
            processUnitLargeWalkW("Human_Female", 8);
            processUnitLargeW("Human_Female", 9, true, false);
            processUnitLargeWalkW("Human_Female", 9);
            processUnitLargeW("Human_Female", 10, true, false);
            processUnitLargeWalkW("Human_Female", 10);
            

            processUnitHugeW("Barrel", 38, true, false);
            
            processUnitHugeW("Table", 39, true, false);
            processUnitHugeW("Desk", 39, true, false);
            processUnitHugeW("Computer_Desk", 39, true, false);
            processUnitHugeW("Computer_Desk", 40, true, false);

            processUnitHugeW("Table", 41, true, false);
            processUnitHugeW("Desk", 41, true, false);
            processUnitHugeW("Computer_Desk", 41, true, false);
            processUnitHugeW("Computer_Desk", 42, true, false);
            */
            /*
            processHatLargeW("Generic_Male", 0, "Berserker");
            processHatLargeW("Generic_Male", 0, "Witch");
            processHatLargeW("Generic_Male", 0, "Scout");
            processHatLargeW("Generic_Male", 0, "Captain");
            processHatLargeW("Generic_Male", 0, "Mystic");
            processHatLargeW("Generic_Male", 0, "Wizard");
            processHatLargeW("Generic_Male", 0, "Provocateur");
            processHatLargeW("Generic_Male", 0, "Noble");
            processHatLargeW("Generic_Male", 44, "Woodsman");
            processHatLargeW("Generic_Male", 0, "Sheriff");
            processHatLargeW("Generic_Male", 0, "Thief");
            processHatLargeW("Generic_Male", 0, "Merchant");
            processHatLargeW("Generic_Male", 49, "Farmer");
            processHatLargeW("Generic_Male", 0, "Officer");
            processHatLargeW("Generic_Male", 0, "Dervish");
            processHatLargeW("Generic_Male", 0, "Thug");
            processHatLargeW("Generic_Male", 0, "Bishop");


            processHatLargeW("Spirit", 7, "Berserker");
            processHatLargeW("Spirit", 7, "Witch");
            processHatLargeW("Spirit", 7, "Scout");
            processHatLargeW("Spirit", 7, "Captain");
            processHatLargeW("Spirit", 7, "Mystic");
            processHatLargeW("Spirit", 7, "Wizard");
            processHatLargeW("Spirit", 7, "Provocateur");
            processHatLargeW("Spirit", 7, "Noble");
            processHatLargeW("Spirit", 44, "Woodsman");
            processHatLargeW("Spirit", 7, "Sheriff");
            processHatLargeW("Spirit", 7, "Thief");
            processHatLargeW("Spirit", 7, "Merchant");
            processHatLargeW("Spirit", 49, "Farmer");
            processHatLargeW("Spirit", 7, "Officer");
            processHatLargeW("Spirit", 7, "Dervish");
            processHatLargeW("Spirit", 7, "Thug");
            processHatLargeW("Spirit", 7, "Bishop");
            */

            /*
            processHats("Zombie", 2, true, classes);

            processHats("Skeleton", 6, true, classes);

            processHats("Spirit", 7, false, classes);

            processHats("Wraith", 8, false, classes);

            processHats("Cinder", 9, true, classes);

            processHats("Ghoul", 39, true, classes);

            processHats("Wight", 40, true, classes);

            processHats("Spectre", 42, false, classes);

            processHats("Mummy", 43, true, classes);
            
            processHats("Drowned", 45, true, classes);
            
            processHats("Banshee", 46, false, classes);

            processHats("Damned", 63, true, classes);

            processHats("Husk", 64, true, classes);
            
            
            processHats("Generic_Male", 0, true, classes, "Living_Men_A");

            processHats("Generic_Male", 1, true, classes, "Living_Men_B");

            processHats("Generic_Male", 15, true, classes, "Living_Men_C");

            processHats("Generic_Male", 16, true, classes, "Living_Men_D");

            processHats("Generic_Male", 17, true, classes, "Living_Men_E");

            processHats("Generic_Female", 0, true, classes, "Living_Women_A");

            processHats("Generic_Female", 1, true, classes, "Living_Women_B");

            processHats("Generic_Female", 15, true, classes, "Living_Women_C");

            processHats("Generic_Female", 16, true, classes, "Living_Women_D");

            processHats("Generic_Female", 17, true, classes, "Living_Women_E");


            processHats("Bulky_Male", 0, true, classes, "Bulky_Men_A");

            processHats("Bulky_Male", 1, true, classes, "Bulky_Men_B");

            processHats("Bulky_Male", 15, true, classes, "Bulky_Men_C");

            processHats("Bulky_Male", 16, true, classes, "Bulky_Men_D");

            processHats("Bulky_Male", 17, true, classes, "Bulky_Men_E");


            processHats("Bulky_Female", 0, true, classes, "Bulky_Women_A");

            processHats("Bulky_Female", 1, true, classes, "Bulky_Women_B");

            processHats("Bulky_Female", 15, true, classes, "Bulky_Women_C");

            processHats("Bulky_Female", 16, true, classes, "Bulky_Women_D");

            processHats("Bulky_Female", 17, true, classes, "Bulky_Women_E");
            
            
            processHats("Armored_Male", 0, true, classes, "Armored_Men_A");

            processHats("Armored_Male", 1, true, classes, "Armored_Men_B");

            processHats("Armored_Male", 15, true, classes, "Armored_Men_C");

            processHats("Armored_Male", 16, true, classes, "Armored_Men_D");

            processHats("Armored_Male", 17, true, classes, "Armored_Men_E");

            processUnitLargeW("Necromancer", 65, true, false);

            File.WriteAllText("relative-hat-positions.txt", model_headpoints.ToString());
            File.WriteAllText("hats.txt", hat_headpoints.ToString());

            processHats("Birthday_Necromancer", 65, true, classes);
            

            offsets.Close();
            offbin.Close();
            
            TallVoxels.makeFlatTilingDrab().Save("tiling_96x48.png", ImageFormat.Png);
            
            for(int i = 50; i <= 60; i++)
            {
                processUnitHugeW("Terrain", i, true, true);
            }
            
            
            processUnitHugeW("Grass", 47, true, true);
            processUnitHugeW("Tree", 47, true, false);
            processUnitHugeW("Boulder", 48, true, false);
            processUnitHugeW("Rubble", 48, true, false);
            processUnitHugeW("Headstone", 48, true, false);

            processUnitHugeW("Roof_Flat", 49, true, true);
            processUnitHugeW("Roof_Straight", 49, true, true);
            processUnitHugeW("Roof_Corner", 49, true, true);
            processUnitHugeW("Roof_Solid_Flat", 49, true, true);
            processUnitHugeW("Roof_Solid_Straight", 49, true, true);
            processUnitHugeW("Roof_Solid_Corner", 49, true, true);
            processUnitHugeW("Roof_Solid_Straight_Off", 49, true, true);
            processUnitHugeW("Roof_Solid_Corner_Off", 49, true, true);

            processUnitHugeW("Door_Closed", 49, true, false);
            processUnitHugeW("Door_Open", 49, true, false);
            processUnitHugeW("Wall_Straight", 49, true, false);
            processUnitHugeW("Wall_Cross", 49, true, false);
            processUnitHugeW("Wall_Tee", 49, true, false);
            processUnitHugeW("Wall_Corner", 49, true, false);
            processUnitHugeW("Wall_Straight_Upper", 49, true, true);
            processUnitHugeW("Wall_Cross_Upper", 49, true, true);
            processUnitHugeW("Wall_Tee_Upper", 49, true, true);
            processUnitHugeW("Wall_Corner_Upper", 49, true, true);
            TallVoxels.altFolder = altFolder;
            TallVoxels.generateBotLSpritesheet();
            */
            /*
            processPureAttackW("Autofire", 0);
            processPureAttackW("Beam", 0);
            processPureAttackW("Cannon", 0);
            processPureAttackW("Lightning", 0);
            
            processUnitOutlinedWMecha(moniker: "Vox_Populi", head: "Blocky", torso: "Blocky", legs: "Blocky", left_arm: "Blocky", right_arm: "Blocky", right_weapon: "Bazooka", left_weapon: "Pistol");
            processUnitOutlinedWMechaFiring(moniker: "Vox_Populi", head: "Blocky", torso: "Blocky", legs: "Blocky", left_arm: "Blocky", right_arm: "Blocky", right_weapon: "Bazooka", left_weapon: "Pistol", right_projectile: "Beam", left_projectile: "Autofire");
            processUnitOutlinedWMechaFiring(moniker: "Vox_Populi", head: "Blocky", torso: "Blocky", legs: "Blocky", left_arm: "Blocky", right_arm: "Blocky", right_weapon: "Bazooka", left_weapon: "Pistol", right_projectile: "Lightning", left_projectile: "Cannon");

            processUnitOutlinedWMecha(moniker: "Vox_Nihilus", head: "Blocky", torso: "Blocky", legs: "Blocky", left_arm: "Blocky_Aiming", right_arm: "Blocky_Aiming", right_weapon: "Rifle");
            processUnitOutlinedWMechaAiming(moniker: "Vox_Nihilus", head: "Blocky_Aiming", torso: "Blocky", legs: "Blocky", left_arm: "Blocky_Aiming", right_arm: "Blocky_Aiming", right_weapon: "Rifle", right_projectile: "Cannon");
            processUnitOutlinedWMechaAiming(moniker: "Vox_Nihilus", head: "Blocky_Aiming", torso: "Blocky", legs: "Blocky", left_arm: "Blocky_Aiming", right_arm: "Blocky_Aiming", right_weapon: "Rifle", right_projectile: "Lightning");

            processUnitOutlinedWMecha(moniker: "Maku", left_weapon: "Bazooka");
            processUnitOutlinedWMechaFiring(moniker: "Maku", left_weapon: "Bazooka", left_projectile: "Beam");
            processUnitOutlinedWMechaFiring(moniker: "Maku", left_weapon: "Bazooka", left_projectile: "Lightning");

            processUnitOutlinedWMecha(moniker: "Mark_Zero", head: "Armored", left_arm: "Armored_Aiming", right_arm: "Armored_Aiming", right_weapon: "Rifle");
            processUnitOutlinedWMechaAiming(moniker: "Mark_Zero", head: "Armored_Aiming", left_arm: "Armored_Aiming", right_arm: "Armored_Aiming", right_weapon: "Rifle", right_projectile: "Beam");
            processUnitOutlinedWMechaAiming(moniker: "Mark_Zero", head: "Armored_Aiming", left_arm: "Armored_Aiming", right_arm: "Armored_Aiming", right_weapon: "Rifle", right_projectile: "Lightning");
            
            processUnitOutlinedWMecha(moniker: "Deadman", head: "Armored", left_weapon: "Pistol", right_weapon: "Katana");
            processUnitOutlinedWMechaSwinging(moniker: "Deadman", head: "Armored", left_weapon: "Pistol", right_weapon: "Katana", left_projectile: "Autofire");
            processUnitOutlinedWMechaSwinging(moniker: "Deadman", head: "Armored", left_weapon: "Pistol", right_weapon: "Katana", left_projectile: "Lightning");
            
            processUnitOutlinedWMecha(moniker: "Chivalry", head: "Armored", right_weapon: "Beam_Sword");
            processUnitOutlinedWMechaSwinging(moniker: "Chivalry", head: "Armored", right_weapon: "Beam_Sword");
            
            processUnitOutlinedWMecha(moniker: "Banzai", left_weapon: "Pistol", right_weapon: "Pistol");
            processUnitOutlinedWMecha(moniker: "Banzai_Flying", left_weapon: "Pistol", right_weapon: "Pistol",
                legs: "Armored_Jet", still: false);
            processUnitOutlinedWMechaFiring(moniker: "Banzai", left_weapon: "Pistol", right_weapon: "Pistol", left_projectile: "Autofire", right_projectile: "Autofire");
            processUnitOutlinedWMechaFiring(moniker: "Banzai_Flying", left_weapon: "Pistol", right_weapon: "Pistol", left_projectile: "Autofire", right_projectile: "Autofire",
                legs: "Armored_Jet", still: false);
            */
            /*

            processUnitLargeWMilitary("Civilian");

            processUnitLargeWMilitary("Infantry_PS");
            processUnitLargeWMilitary("Infantry_PT");
            processUnitLargeWMilitary("Infantry_ST");

            processUnitLargeWMilitary("Tank");
            processUnitLargeWMilitary("Tank_P");
            processUnitLargeWMilitary("Tank_S");
            processUnitLargeWMilitary("Tank_T");

            processUnitLargeWMilitary("Infantry");
            processUnitLargeWMilitary("Infantry_P");
            processUnitLargeWMilitary("Infantry_S");
            processUnitLargeWMilitary("Infantry_T");
            
            processUnitLargeWMilitary("Supply");
            processUnitLargeWMilitary("Supply_P");
            processUnitLargeWMilitary("Supply_S");
            processUnitLargeWMilitary("Supply_T");
            
            processUnitLargeWMilitary("Artillery");
            processUnitLargeWMilitary("Artillery_P");
            processUnitLargeWMilitary("Artillery_S");
            processUnitLargeWMilitary("Artillery_T");

            processUnitLargeWMilitary("Copter");
            processUnitLargeWMilitary("Copter_P");
            processUnitLargeWMilitary("Copter_S");
            processUnitLargeWMilitary("Copter_T");

            processUnitLargeWMilitary("Plane");
            processUnitLargeWMilitary("Plane_P");
            processUnitLargeWMilitary("Plane_S");
            processUnitLargeWMilitary("Plane_T");

            processUnitLargeWMilitary("Boat");
            processUnitLargeWMilitary("Boat_P");
            processUnitLargeWMilitary("Boat_S");
            processUnitLargeWMilitary("Boat_T");
            
            processUnitLargeWMilitary("Laboratory");
            processUnitLargeWMilitary("Dock");
            processUnitLargeWMilitary("Airport");
            processUnitLargeWMilitary("City");
            processUnitLargeWMilitary("Factory");
            processUnitLargeWMilitary("Castle");
            processUnitLargeWMilitary("Estate");

            processReceivingMilitaryW();
            
            */

            //processUnitLargeWBones(left_weapon: "Longsword");
        }
    }
}
