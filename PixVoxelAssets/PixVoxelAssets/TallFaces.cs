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
        Top = 0,
        Bright = 1,
        Dim = 2,
        BrightTop = 3,
        DimTop = 4,
        BrightDim = 5,
        BrightDimTop = 6,
        BrightBottom = 7,
        DimBottom = 8,
        BrightDimBottom = 9,
        BrightBack = 10,
        DimBack = 11,
        BrightTopBack = 12,
        DimTopBack = 13,
        BrightBottomBack = 14,
        DimBottomBack = 15,
        BackBack = 16;
        private static Random r = new Random(0x1337BEEF);
        public static string altFolder = "";

        /*
        
                                    if(current_color == -1 && (current_color == 8 || current_color == 9 || current_color == 11))
                                    {
                                        if(j == 0)
                                        {
                                            c = VoxelLogic.ColorFromHSV(h, Math.Min(1.0, s * 1.1), v);
                                        }
                                        else if(i >= width / 2 || j == height - 1)
                                        {
                                            c = VoxelLogic.ColorFromHSV(h, Math.Min(1.0, s * 1.35), Math.Max(0.01, v * ((VoxelLogic.wpalettes[p][current_color][0] + VoxelLogic.wpalettes[p][current_color][1] + VoxelLogic.wpalettes[p][current_color][2] > 2.5) ? 1.0 : 0.85)));
                                        }
                                        else
                                        {
                                            c = VoxelLogic.ColorFromHSV(h, Math.Min(1.0, s * 1.2), Math.Max(0.01, v * ((VoxelLogic.wpalettes[p][current_color][0] + VoxelLogic.wpalettes[p][current_color][1] + VoxelLogic.wpalettes[p][current_color][2] > 2.5) ? 1.0 : 0.95)));
                                        }
                                    }
        */


        //        private static byte[][][][] storeColorCubesWBold()
        //        {
        //            VoxelLogic.wpalettecount = VoxelLogic.wpalettes.Length;
        //            //            wcolorcount = VoxelLogic.wpalettes[0].Length;
        //            // 17 is the number of Slope enum types.
        //            byte[,,,] cubes = new byte[VoxelLogic.wpalettecount, VoxelLogic.wpalettes[0].Length, 17, 80];

        //            Image image = new Bitmap("white.png");
        //            ImageAttributes imageAttributes = new ImageAttributes();
        //            int width = 4;
        //            int height = 5;
        //            float[][] colorMatrixElements = {
        //   new float[] {1F, 0,  0,  0,  0},
        //   new float[] {0, 1F,  0,  0,  0},
        //   new float[] {0,  0,  1F, 0,  0},
        //   new float[] {0,  0,  0,  1F, 0},
        //   new float[] {0,  0,  0,  0, 1F}};

        //            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

        //            imageAttributes.SetColorMatrix(
        //               colorMatrix,
        //               ColorMatrixFlag.Default,
        //               ColorAdjustType.Bitmap);
        //            for(int p = 0; p < VoxelLogic.wpalettes.Length; p++)
        //            {
        //                for(int current_color = 0; current_color < VoxelLogic.wpalettes[0].Length; current_color++)
        //                {
        //                    Bitmap b =
        //                    new Bitmap(width, height, PixelFormat.Format32bppArgb);

        //                    Graphics g = Graphics.FromImage((Image)b);

        //                    if(current_color == 25)
        //                    {
        //                        colorMatrix = new ColorMatrix(new float[][]{
        //   new float[] {0.22F+VoxelLogic.wpalettes[p][current_color][0],  0,  0,  0, 0},
        //   new float[] {0,  0.251F+VoxelLogic.wpalettes[p][current_color][1],  0,  0, 0},
        //   new float[] {0,  0,  0.31F+VoxelLogic.wpalettes[p][current_color][2],  0, 0},
        //   new float[] {0,  0,  0,  1, 0},
        //   new float[] {0, 0, 0, 0, 1F}});
        //                    }
        //                    else if(VoxelLogic.wpalettes[p][current_color][3] == VoxelLogic.eraser_alpha)
        //                    {
        //                        colorMatrix = new ColorMatrix(new float[][]{
        //   new float[] {0,  0,  0,  0, 0},
        //   new float[] {0,  0,  0,  0, 0},
        //   new float[] {0,  0,  0,  0, 0},
        //   new float[] {0,  0,  0,  1F, 0},
        //   new float[] {0,  0,  0,  0, 1F}});
        //                    }
        //                    else if(VoxelLogic.wpalettes[p][current_color][3] == 0F)
        //                    {
        //                        colorMatrix = new ColorMatrix(new float[][]{
        //   new float[] {0,  0,  0,  0, 0},
        //   new float[] {0,  0,  0,  0, 0},
        //   new float[] {0,  0,  0,  0, 0},
        //   new float[] {0,  0,  0,  0, 0},
        //   new float[] {0,  0,  0,  0, 1F}});
        //                    }
        //                    else
        //                    {
        //                        colorMatrix = new ColorMatrix(new float[][]{
        //   new float[] {0.235F+VoxelLogic.wpalettes[p][current_color][0],  0,  0,  0, 0},
        //   new float[] {0,  0.26F+VoxelLogic.wpalettes[p][current_color][1],  0,  0, 0},
        //   new float[] {0,  0,  0.30F+VoxelLogic.wpalettes[p][current_color][2],  0, 0},
        //   new float[] {0,  0,  0,  1F, 0},
        //   new float[] {0, 0, 0, 0, 1F}});
        //                    }
        //                    imageAttributes.SetColorMatrix(
        //                       colorMatrix,
        //                       ColorMatrixFlag.Default,
        //                       ColorAdjustType.Bitmap);

        //                    string which_image = ((current_color >= 18 && current_color <= 24) || VoxelLogic.wpalettes[p][current_color][3] == 0F
        //                        || VoxelLogic.wpalettes[p][current_color][3] == VoxelLogic.flash_alpha_0 || VoxelLogic.wpalettes[p][current_color][3] == VoxelLogic.flash_alpha_1) ? "shine" :
        //                       (VoxelLogic.wpalettes[p][current_color][3] == VoxelLogic.flat_alpha || VoxelLogic.wpalettes[p][current_color][3] == VoxelLogic.bordered_flat_alpha) ? "flat" : "image";
        //                    g.DrawImage(image,
        //                       new Rectangle(0, 0,
        //                           width, height),  // destination rectangle 
        //                                            //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
        //                       0, 0,        // upper-left corner of source rectangle 
        //                       width,       // width of source rectangle
        //                       height,      // height of source rectangle
        //                       GraphicsUnit.Pixel,
        //                       imageAttributes);
        //                    for(int i = 0; i < width; i++)
        //                    {
        //                        for(int j = 0; j < height; j++)
        //                        {
        //                            Color c = b.GetPixel(i, j);
        //                            double h = 0.0, s = 1.0, v = 1.0;
        //                            VoxelLogic.ColorToHSV(c, out h, out s, out v);

        //                            for(int slp = 0; slp < 17; slp++)
        //                            {
        //                                Color c2 = Color.Transparent;
        //                                if(which_image.Equals("image"))
        //                                {
        //                                    if(j == height - 1)
        //                                    {
        //                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Pow(s, 0.3)) * 1.5, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.65, 0.01, 1.0));
        //                                    }
        //                                    else
        //                                    {
        //                                        switch(slp)
        //                                        {
        //                                            case Top:
        //                                            case BackBack:
        //                                                {
        //                                                    if(j == 0)
        //                                                    {
        //                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.05, 0.0112, 1.0), VoxelLogic.Clamp(v * 1.1, 0.09, 1.0));
        //                                                    }
        //                                                }
        //                                                break;
        //                                            case Bright:
        //                                                {
        //                                                    if(i < width / 2 && j > 0)
        //                                                    {
        //                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.2, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.95, 0.06, 1.0));
        //                                                    }

        //                                                }
        //                                                break;
        //                                            case Dim:
        //                                                {
        //                                                    if(i >= width / 2 && j > 0)
        //                                                    {
        //                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.35, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.8, 0.03, 1.0));
        //                                                    }
        //                                                }
        //                                                break;
        //                                            case BrightTop:
        //                                                {

        //                                                    if(j == 0)// && i == width - 1)
        //                                                    {
        //                                                        //c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.05, 0.0112, 1.0), VoxelLogic.Clamp(v * 1.1, 0.09, 1.0));
        //                                                    }
        //                                                    else if(i + j > 4)
        //                                                    {
        //                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.35, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.8, 0.03, 1.0));
        //                                                    }
        //                                                    else if((i + j) > 1)
        //                                                    //if(j >= 2 &&  i >=  2 - (j / 3) * 2)
        //                                                    {
        //                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.125, 0.0112, 1.0), VoxelLogic.Clamp(v * 1.05, 0.08, 1.0));
        //                                                    }
        //                                                }
        //                                                break;
        //                                            case DimTop:
        //                                                {

        //                                                    if(j == 0)// && i == 0)
        //                                                    {
        ////                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.05, 0.0112, 1.0), VoxelLogic.Clamp(v * 1.1, 0.09, 1.0));
        //                                                    }
        //                                                    else if(i <= j - 2)
        //                                                    {
        //                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.2, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.95, 0.06, 1.0));
        //                                                    }
        //                                                    else if(i <= j)
        //                                                    //                                                    if(j >= 2 && i < (j / 3) * 2 + 2)
        //                                                    {
        //                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.25, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.9, 0.05, 1.0));
        //                                                    }
        //                                                }
        //                                                break;
        //                                            case BrightDim:
        //                                                {
        //                                                    if(j == 0)
        //                                                    {
        //                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.05, 0.0112, 1.0), VoxelLogic.Clamp(v * 1.1, 0.09, 1.0));
        //                                                    }
        //                                                    else
        //                                                    {
        //                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.275, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.875, 0.045, 1.0));
        //                                                    }
        //                                                }
        //                                                break;
        //                                            case BrightDimTop:
        //                                                {
        //                                                    if((i + j) / 2 >= 1 && i <= j / 2 && j > 0)
        //                                                    {
        //                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.15, 0.0112, 1.0), VoxelLogic.Clamp(v * 1.0, 0.08, 1.0));
        //                                                    }
        //                                                    else // else if (j > 0)
        //                                                    {
        //                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.2, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.95, 0.08, 1.0));
        //                                                    }
        //                                                }
        //                                                break;
        //                                            /*
        //                                        case BrightBottom:
        //                                            {

        //                                                if(j == 0)
        //                                                {
        //                                                    c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.05, 0.0112, 1.0) * 0.9, VoxelLogic.Clamp(v * 1.1, 0.09, 1.0));
        //                                                }
        //                                                else if(i - 1 >= j)
        //                                                {
        //                                                    c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.35, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.8, 0.03, 1.0));
        //                                                }
        //                                                else if(i >= j)
        //                                                {
        //                                                    c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.4, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.75, 0.02, 1.0));
        //                                                }
        //                                            }
        //                                            break;
        //                                        case DimBottom:
        //                                            {

        //                                                if(j == 0)
        //                                                {
        //                                                    c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.05, 0.0112, 1.0) * 0.9, VoxelLogic.Clamp(v * 1.1, 0.09, 1.0));
        //                                                }
        //                                                else if(i + j < 3)
        //                                                {
        //                                                    c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.2, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.95, 0.06, 1.0));
        //                                                }
        //                                                else if(i + j < 4)
        //                                                {
        //                                                    c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.45, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.7, 0.015, 1.0));
        //                                                }
        //                                            }
        //                                            break;
        //                                        case BrightDimBottom:
        //                                            {

        //                                                if(j == 0)
        //                                                {
        //                                                    c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.05, 0.0112, 1.0) * 0.9, VoxelLogic.Clamp(v * 1.1, 0.09, 1.0));
        //                                                }
        //                                                else if(i + 1 >= j && i + j < 5)
        //                                                {
        //                                                    c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.45, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.7, 0.015, 1.0));
        //                                                }
        //                                            }
        //                                            break;*/
        //                                            case BrightBack:
        //                                                {

        //                                                    if(j == 0 && i >= width / 2)
        //                                                    {
        //                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s* s * s* Math.Sqrt(s)) * 1.05, 0.0112, 1.0), VoxelLogic.Clamp(v* 1.1, 0.09, 1.0));
        //                                                    }
        //}
        //                                                break;
        //                                            case DimBack:
        //                                                {

        //                                                    if(j == 0 && i<width / 2)
        //                                                    {
        //                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s* s * s * Math.Sqrt(s)) * 1.05, 0.0112, 1.0), VoxelLogic.Clamp(v* 1.1, 0.09, 1.0));
        //                                                    }
        //                                                }
        //                                                break;
        //                                            case BrightBottomBack:
        //                                            case BrightTopBack:
        //                                            case DimTopBack:
        //                                            case DimBottomBack:
        //                                            default:
        //                                                {

        //                                                }
        //                                                break;
        //                                        }
        //                                    }
        //                                }
        //                                else if(which_image == "shine")
        //                                {
        //                                    c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp(s* 0.85, 0.0112, 1.0), VoxelLogic.Clamp(v* 1.1, 0.1, 1.0));
        //                                }
        //                                else if(which_image == "flat")
        //                                {
        //                                    if(j >= 2 && j<height - 1)
        //                                    {
        //                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp(s* 0.65, 0.0112, 1.0), VoxelLogic.Clamp(v* 0.85, 0.01, 1.0));
        //                                    }
        //                                }
        //                                if(c2.A != 0)
        //                                {
        //                                    cubes[p, current_color, slp, i * 4 + j * width * 4 + 0] = Math.Max((byte)1, c2.B);
        //                                    cubes[p, current_color, slp, i * 4 + j * width * 4 + 1] = Math.Max((byte)1, c2.G);
        //                                    cubes[p, current_color, slp, i * 4 + j * width * 4 + 2] = Math.Max((byte)1, c2.R);
        //                                    cubes[p, current_color, slp, i * 4 + j * width * 4 + 3] = c2.A;
        //                                }
        //                                else
        //                                {
        //                                    cubes[p, current_color, slp, i * 4 + j * 4 * width + 0] = 0;
        //                                    cubes[p, current_color, slp, i * 4 + j * 4 * width + 1] = 0;
        //                                    cubes[p, current_color, slp, i * 4 + j * 4 * width + 2] = 0;
        //                                    cubes[p, current_color, slp, i * 4 + j * 4 * width + 3] = 0;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            byte[][][][] cubes2 = new byte[VoxelLogic.wpalettes.Length][][][];
        //VoxelLogic.wrendered = new byte[VoxelLogic.wpalettes.Length][][];
        //            for(int i = 0; i<VoxelLogic.wpalettes.Length; i++)
        //            {
        //                cubes2[i] = new byte[VoxelLogic.wpalettes[0].Length][][];
        //                VoxelLogic.wrendered[i] = new byte[VoxelLogic.wpalettes[0].Length][];
        //                for(int c = 0; c<VoxelLogic.wpalettes[0].Length; c++)
        //                {
        //                    cubes2[i][c] = new byte[17][];
        //                    for(int sp = 0; sp< 17; sp++)
        //                    {
        //                        cubes2[i][c][sp] = new byte[80];
        //                        VoxelLogic.wrendered[i][c] = new byte[80];
        //                        for(int j = 0; j< 80; j++)
        //                        {
        //                            cubes2[i][c][sp][j] = cubes[i, c, sp, j];
        //                            if(sp == 1)
        //                            {
        //                                VoxelLogic.wrendered[i][c][j] = cubes[i, c, sp, j];
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            return cubes2;
        //        }


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

                    string which_image = ((current_color >= 18 && current_color <= 20) || VoxelLogic.wpalettes[p][current_color][3] == 0F
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

                            for(int slp = 0; slp < 17; slp++)
                            {
                                Color c2 = Color.Transparent;
                                if(which_image.Equals("image"))
                                {
                                    if(j == height - 1)
                                    {
                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Pow(s, 0.3)) * 1.5, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.65, 0.01, 1.0));
                                    }
                                    else
                                    {
                                        switch(slp)
                                        {
                                            case Top:
                                            case BackBack:
                                                {
                                                    if(j == 0)
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.05, 0.0112, 1.0), VoxelLogic.Clamp(v * 1.1, 0.09, 1.0));
                                                    }
                                                }
                                                break;
                                            case Bright:
                                                {
                                                    if(i < width / 2 && j > 0)
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.2, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.95, 0.06, 1.0));
                                                    }

                                                }
                                                break;
                                            case Dim:
                                                {
                                                    if(i >= width / 2 && j > 0)
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.35, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.8, 0.03, 1.0));
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
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.35, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.8, 0.03, 1.0));
                                                    }
                                                    else if(i + j / 2 >= 1)
                                                    //if(j >= 2 &&  i >=  2 - (j / 3) * 2)
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.0, 0.0112, 1.0), VoxelLogic.Clamp(v * 1.15, 0.10, 1.0));

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

                                                    if(i < (j+1) / 2 && j > 0)
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.2, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.95, 0.06, 1.0));

                                                    }
                                                    else if(i <= j / 2 + 2)
                                                    //                                                    if(j >= 2 && i < (j / 3) * 2 + 2)
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.25, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.9, 0.05, 1.0));

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
                                                    c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.275, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.875, 0.05, 1.0));

                                                }
                                                break;
                                            case BrightDimTop:
                                                {
                                                    //     if((i + j) / 2 >= 1 && i <= j / 2 && j > 0)
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.1, 0.0112, 1.0), VoxelLogic.Clamp(v * 1.05, 0.08, 1.0));
                                                    }
                                                    //   else // else if (j > 0)
                                                    //   {
                                                    //       c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.2, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.95, 0.08, 1.0));
                                                    //   }
                                                }
                                                break;

                                            case BrightBottom:
                                                {
                                                    if(i > (j + 1) / 2 + 1)
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.4, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.7, 0.02, 1.0));
                                                    }
                                                }
                                                break;
                                            case DimBottom:
                                                {
                                                    if(i + (j + 1) / 2 < 2)
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.5, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.6, 0.01, 1.0));
                                                    }
                                                }
                                                break;
                                            case BrightDimBottom:
                                                {
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.45, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.65, 0.015, 1.0));
                                                    }
                                                }
                                                break;
                                            case BrightBack:
                                                {

                                                    if(i >= 2)
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.1, 0.0112, 1.0), VoxelLogic.Clamp(v * 1.1, 0.09, 1.0));
                                                    }
                                                }
                                                break;
                                            case DimBack:
                                                {

                                                    if(i < 2)
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.1, 0.0112, 1.0), VoxelLogic.Clamp(v * 1.1, 0.09, 1.0));
                                                    }
                                                }
                                                break;
                                            case BrightTopBack:
                                                {
                                                    if(i + (j + 1) / 2 > 3) // && i >= 2
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.05, 0.0112, 1.0), VoxelLogic.Clamp(v * 1.15, 0.09, 1.0));
                                                    }
                                                }
                                                break;
                                            case DimTopBack:
                                                {
                                                    if(i * 2 < j) // && i < 2)
                                                    {
                                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp((s + s * s * s * Math.Sqrt(s)) * 1.2, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.95, 0.09, 1.0));
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
                                    c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp(s * 0.85, 0.0112, 1.0), VoxelLogic.Clamp(v * 1.1, 0.1, 1.0));
                                }
                                else if(which_image == "flat")
                                {
                                    if(j >= 2 && j < height - 1)
                                    {
                                        c2 = VoxelLogic.ColorFromHSV(h, VoxelLogic.Clamp(s * 0.65, 0.0112, 1.0), VoxelLogic.Clamp(v * 0.85, 0.01, 1.0));
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
                    for(int sp = 0; sp < 17; sp++)
                    {
                        cubes2[i][c][sp] = new byte[80];
                        VoxelLogic.wrendered[i][c] = new byte[80];
                        for(int j = 0; j < 80; j++)
                        {
                            cubes2[i][c][sp][j] = cubes[i, c, sp, j];
                            if(sp == 1)
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
        Slope.Top,
        Slope.Bright,
        Slope.Dim,
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
        public static void processUnitLargeW(string u, int palette, bool still)
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
            for(int f = 0; f < framelimit; f++)
            { //
                for(int dir = 0; dir < 4; dir++)
                {
                    Bitmap b = processFrameLargeW(parsed, palette, dir, f, framelimit, still);
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

            // processFieryExplosionLargeW(u, palette);

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
            int framelimit = 8;


            string folder = (altFolder);//"color" + i;
            Directory.CreateDirectory(folder); //("color" + i);
            for(int f = 0; f < framelimit; f++)
            { //
                for(int dir = 0; dir < 4; dir++)
                {
                    Bitmap b = processFrameLargeW(parsed[f % 4], palette, dir, f, framelimit, true);
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

        private static Bitmap processFrameLargeW(MagicaVoxelData[] parsed, int palette, int dir, int frame, int maxFrames, bool still)
        {
            Bitmap b;
//            Bitmap b2 = new Bitmap(88, 108, PixelFormat.Format32bppArgb);

            VoxelLogic.wcolors = VoxelLogic.wpalettes[palette];
            wcurrent = wrendered[palette];
            b = renderWSmart(parsed, dir, palette, frame, maxFrames, still);
            return b;
            /*
            Graphics g2 = Graphics.FromImage(b2);
            g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g2.DrawImage(b.Clone(new Rectangle(32, 46 + 32, 88 * 2, 108 * 2), b.PixelFormat), 0, 0, 88, 108);
            g2.Dispose();
            return smoothScale(b2, 3);
            */
            /*string folder = "palette" + palette + "_big";
            System.IO.Directory.CreateDirectory(folder);
            b.Save(folder + "/" + (System.IO.Directory.GetFiles(folder).Length) + "_Gigantic_face" + dir + "_" + frame + ".png", ImageFormat.Png); g = Graphics.FromImage(b);
            */
        }
        public static void processUnitHugeW(string u, int palette, bool still)
        {

            Console.WriteLine("Processing: " + u + ", palette " + palette);
            BinaryReader bin = new BinaryReader(File.Open(u + "_Huge_W.vox", FileMode.Open));
            List<MagicaVoxelData> voxes = VoxelLogic.FromMagicaRaw(bin); //VoxelLogic.PlaceShadowsW(
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
            for(int f = 0; f < framelimit; f++)
            { //
                for(int dir = 0; dir < 4; dir++)
                {
                    Bitmap b = processFrameHugeW(parsed, palette, dir, f, framelimit, still);
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

            // processFieryExplosionHugeW(u, palette);

        }
        public static void processUnitWalkHugeW(string u, int palette)
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


            string folder = (altFolder);//"color" + i;
            Directory.CreateDirectory(folder); //("color" + i);
            for(int f = 0; f < framelimit; f++)
            { //
                for(int dir = 0; dir < 4; dir++)
                {
                    Bitmap b = processFrameHugeW(parsed[f % 4], palette, dir, f, framelimit, true);
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

        private static Bitmap processFrameHugeW(MagicaVoxelData[] parsed, int palette, int dir, int frame, int maxFrames, bool still)
        {
            Bitmap b;
//            Bitmap b2 = new Bitmap(88, 108, PixelFormat.Format32bppArgb);

            VoxelLogic.wcolors = VoxelLogic.wpalettes[palette];
            wcurrent = wrendered[palette];
            b = renderWSmartHuge(parsed, dir, palette, frame, maxFrames, still);
            return b;
            /*
            Graphics g2 = Graphics.FromImage(b2);
            g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g2.DrawImage(b.Clone(new Rectangle(32, 46 + 32, 88 * 2, 108 * 2), b.PixelFormat), 0, 0, 88, 108);
            g2.Dispose();
            return smoothScale(b2, 3);
            */
            /*string folder = "palette" + palette + "_big";
            System.IO.Directory.CreateDirectory(folder);
            b.Save(folder + "/" + (System.IO.Directory.GetFiles(folder).Length) + "_Gigantic_face" + dir + "_" + frame + ".png", ImageFormat.Png); g = Graphics.FromImage(b);
            */
        }

        private static int voxelToPixelLargeW(int innerX, int innerY, int x, int y, int z, int current_color, int stride, int jitter, bool still)
        {
            return 4 * ((x + y) * 2 + 4 + ((VoxelLogic.wcolors[current_color][3] == VoxelLogic.waver_alpha) ? jitter - 2 : 0))
                + innerX +
                stride * (300 - 60 - y + x - z * 3 - ((VoxelLogic.wcolors[current_color][3] == VoxelLogic.flat_alpha || current_color == 27
                || current_color == VoxelLogic.wcolorcount + 10 || current_color == VoxelLogic.wcolorcount + 20)
                ? -2 : (still) ? 0 : jitter) + innerY);
        }
        private static int voxelToPixelHugeW(int innerX, int innerY, int x, int y, int z, int current_color, int stride, int jitter, bool still)
        {
            return 4 * ((x + y) * 2 + 12 + ((VoxelLogic.wcolors[current_color][3] == VoxelLogic.waver_alpha) ? jitter - 2 : 0))
                                    + innerX +
                                    stride * (600 - 120 - y + x - z * 3 - ((VoxelLogic.wcolors[current_color][3] == VoxelLogic.flat_alpha || current_color == 27
                                    || current_color == VoxelLogic.wcolorcount + 10 || current_color == VoxelLogic.wcolorcount + 20)
                                    ? -2 : (still) ? 0 : jitter) + innerY);
        }
        private static int voxelToPixelMassiveW(int innerX, int innerY, int x, int y, int z, int current_color, int stride, int jitter, bool still)
        {
            return 4 * ((x + y) * 2 + 12 + ((VoxelLogic.wcolors[current_color][3] == VoxelLogic.waver_alpha) ? jitter - 2 : 0))
                                    + innerX +
                                    stride * (800 - 160 - y + x - z * 3 - ((VoxelLogic.wcolors[current_color][3] == VoxelLogic.flat_alpha || current_color == 27
                                    || current_color == VoxelLogic.wcolorcount + 10 || current_color == VoxelLogic.wcolorcount + 20)
                                    ? -2 : (still) ? 0 : jitter) + innerY);
        }

        private static Bitmap renderWSmart(MagicaVoxelData[] voxels, int facing, int palette, int frame, int maxFrames, bool still)
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
            int xSize = 60, ySize = 60, zSize = 40;
            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            switch(facing)
            {
                case 0:
                    vls = voxels;
                    break;
                case 1:
                    for(int i = 0; i < voxels.Length; i++)
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
                    for(int i = 0; i < voxels.Length; i++)
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
                    for(int i = 0; i < voxels.Length; i++)
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
            if(maxFrames >= 8) jitter = ((frame % 8 > 4) ? 4 - ((frame % 8) ^ 4) : frame % 8);
            FaceVoxel[,,] faces = FaceLogic.GetFaces(FaceLogic.VoxListToArray(vls.ToList(), xSize, ySize, zSize, 153));

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
                            if(mod_color == 17 && r.Next(7) < 2) //smoke
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
                                    mod_color -= r.Next(3);
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
                                        for(int sp = 0; sp < 17; sp++)
                                        {
                                            if((slope & Slopes[sp]) == Slopes[sp] && wcurrent[mod_color][sp][(i / 4 + 3) + j * 16] != 0)
                                            {
                                                barePositions[p] = !(VoxelLogic.wcolors[current_color][3] == VoxelLogic.bordered_alpha || VoxelLogic.wcolors[current_color][3] == VoxelLogic.bordered_flat_alpha);
                                                if(VoxelLogic.wcolors[current_color][3] == VoxelLogic.bordered_alpha || VoxelLogic.wcolors[current_color][3] == VoxelLogic.bordered_flat_alpha)
                                                    zbuffer[p] = vx.z + vx.x * 3 - vx.y * 3;

                                                argbValues[p] = wcurrent[mod_color][sp][i + j * 16];
                                                break;
                                            }
                                        }
                                        if(!barePositions[p] && outlineValues[p] == 0)
                                            outlineValues[p] = wcurrent[mod_color][0][i + 64];

                                    }
                                }
                            }
                        }
                        else if(current_color == 25)
                        {
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
                            if((mod_color == 40 || mod_color == VoxelLogic.wcolorcount + 5 || mod_color == VoxelLogic.wcolorcount + 20) && r.Next(11) < 8) //rare sparks
                                continue;

                            for(int j = 0; j < 4; j++)
                            {
                                for(int i = 0; i < 16; i++)
                                {
                                    p = voxelToPixelLargeW(i, j, vx.x, vx.y, vx.z, mod_color, bmpData.Stride, jitter, still);

                                    if(argbValues[p] == 0 && argbValues[(p / 4) + 3] != 7)
                                    {
                                        for(int sp = 0; sp < 17; sp++)
                                        {
                                            if((slope & Slopes[sp]) == Slopes[sp] && wcurrent[mod_color][sp][((i / 4) * 4 + 3) + j * 16] != 0)
                                            {
                                                if(VoxelLogic.wcolors[mod_color][3] == VoxelLogic.gloss_alpha && i % 4 == 3 && r.Next(12) == 0)
                                                {
                                                    argbValues[p - 3] = (byte)Math.Min(wcurrent[mod_color][sp][i - 3 + j * 16] + 160, 255);
                                                    argbValues[p - 2] = (byte)Math.Min(wcurrent[mod_color][sp][i - 2 + j * 16] + 160, 255);
                                                    argbValues[p - 1] = (byte)Math.Min(wcurrent[mod_color][sp][i - 1 + j * 16] + 160, 255);
                                                    argbValues[p - 0] = 255;
                                                }
                                                else if(VoxelLogic.wcolors[mod_color][3] == VoxelLogic.grain_hard_alpha && i % 4 == 3)
                                                {
                                                    float n = Simplex.FindNoiseBold(facing, vx.x + 50, vx.y + 50, vx.z);
                                                    argbValues[p - 3] = (byte)Math.Min(wcurrent[mod_color][sp][i - 3 + j * 16] * n, 255);
                                                    argbValues[p - 2] = (byte)Math.Min(wcurrent[mod_color][sp][i - 2 + j * 16] * n, 255);
                                                    argbValues[p - 1] = (byte)Math.Min(wcurrent[mod_color][sp][i - 1 + j * 16] * n, 255);
                                                    argbValues[p - 0] = 255;
                                                }
                                                else if(VoxelLogic.wcolors[mod_color][3] == VoxelLogic.grain_some_alpha && i % 4 == 3)
                                                {
                                                    float n = Simplex.FindNoise(facing, vx.x + 50, vx.y + 50, vx.z);
                                                    argbValues[p - 3] = (byte)Math.Min(wcurrent[mod_color][sp][i - 3 + j * 16] * n, 255);
                                                    argbValues[p - 2] = (byte)Math.Min(wcurrent[mod_color][sp][i - 2 + j * 16] * n, 255);
                                                    argbValues[p - 1] = (byte)Math.Min(wcurrent[mod_color][sp][i - 1 + j * 16] * n, 255);
                                                    argbValues[p - 0] = 255;
                                                }
                                                else if(VoxelLogic.wcolors[mod_color][3] == VoxelLogic.grain_mild_alpha && i % 4 == 3)
                                                {
                                                    float n = Simplex.FindNoiseLight(facing, vx.x + 50, vx.y + 50, vx.z);
                                                    argbValues[p - 3] = (byte)Math.Min(wcurrent[mod_color][sp][i - 3 + j * 16] * n, 255);
                                                    argbValues[p - 2] = (byte)Math.Min(wcurrent[mod_color][sp][i - 2 + j * 16] * n, 255);
                                                    argbValues[p - 1] = (byte)Math.Min(wcurrent[mod_color][sp][i - 1 + j * 16] * n, 255);
                                                    argbValues[p - 0] = 255;
                                                }
                                                else if(VoxelLogic.wcolors[mod_color][3] == VoxelLogic.fuzz_alpha && i % 4 == 3)
                                                {
                                                    float n = Simplex.FindNoiseTight(frame % 4, facing, vx.x + 50, vx.y + 50, vx.z) + 0.3f;
                                                    argbValues[p - 3] = (byte)VoxelLogic.Clamp(wcurrent[mod_color][sp][i - 3 + j * 16] * n + 16 * n, 1, 255);
                                                    argbValues[p - 2] = (byte)VoxelLogic.Clamp(wcurrent[mod_color][sp][i - 2 + j * 16] * n + 16 * n, 1, 255);
                                                    argbValues[p - 1] = (byte)VoxelLogic.Clamp(wcurrent[mod_color][sp][i - 1 + j * 16] * n + 16 * n, 1, 255);
                                                    argbValues[p - 0] = 255;
                                                }
                                                else
                                                {
                                                    argbValues[p] = wcurrent[mod_color][sp][i + j * 16];
                                                }

                                                zbuffer[p] = vx.z + vx.x * 3 - vx.y * 3;
                                                barePositions[p] = (VoxelLogic.wcolors[mod_color][3] == VoxelLogic.flash_alpha_0 || VoxelLogic.wcolors[mod_color][3] == VoxelLogic.flash_alpha_1 || VoxelLogic.wcolors[mod_color][3] == VoxelLogic.borderless_alpha);
                                                break;
                                            }
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

            for(int i = 3; i < numBytes; i += 4)
            {
                if(argbValues[i] == 7)
                    argbValues[i] = 0;
            }
            bool lightOutline = !VoxelLogic.subtlePalettes.Contains(palette);
            for(int i = 3; i < numBytes; i += 4)
            {
                if(argbValues[i] > 255 * VoxelLogic.waver_alpha && barePositions[i] == false)
                {
                    bool shade = false, blacken = false;
                    /*
                    if (argbValues[i] > 0 && i - 4 >= 0 && i - 4 < argbValues.Length && argbValues[i - 4] == 0 && lightOutline) { editValues[i - 4] = 255; editValues[i - 4 - 1] = 0; editValues[i - 4 - 2] = 0; editValues[i - 4 - 3] = 0; blacken = true; } else if (i - 4 >= 0 && i - 4 < argbValues.Length && barePositions[i - 4] == false && zbuffer[i] - 2 > zbuffer[i - 4]) { editValues[i - 4] = 255; editValues[i - 4 - 1] = outlineValues[i - 1]; editValues[i - 4 - 2] = outlineValues[i - 2]; editValues[i - 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i + 4 >= 0 && i + 4 < argbValues.Length && argbValues[i + 4] == 0 && lightOutline) { editValues[i + 4] = 255; editValues[i + 4 - 1] = 0; editValues[i + 4 - 2] = 0; editValues[i + 4 - 3] = 0; blacken = true; } else if (i + 4 >= 0 && i + 4 < argbValues.Length && barePositions[i + 4] == false && zbuffer[i] - 2 > zbuffer[i + 4]) { editValues[i + 4] = 255; editValues[i + 4 - 1] = outlineValues[i - 1]; editValues[i + 4 - 2] = outlineValues[i - 2]; editValues[i + 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && argbValues[i - bmpData.Stride] == 0 && lightOutline) { editValues[i - bmpData.Stride] = 255; editValues[i - bmpData.Stride - 1] = 0; editValues[i - bmpData.Stride - 2] = 0; editValues[i - bmpData.Stride - 3] = 0; blacken = true; } else if (i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && barePositions[i - bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride]) { editValues[i - bmpData.Stride] = 255; editValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && argbValues[i + bmpData.Stride] == 0 && lightOutline) { editValues[i + bmpData.Stride] = 255; editValues[i + bmpData.Stride - 1] = 0; editValues[i + bmpData.Stride - 2] = 0; editValues[i + bmpData.Stride - 3] = 0; blacken = true; } else if (i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && barePositions[i + bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride]) { editValues[i + bmpData.Stride] = 255; editValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
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
                        //editValues[i] = 255; editValues[i - 1] = 0; editValues[i - 2] = 0; editValues[i - 3] = 0;
                    }
                    else if(shade) { editValues[i] = 255; editValues[i - 1] = outlineValues[i - 1]; editValues[i - 2] = outlineValues[i - 2]; editValues[i - 3] = outlineValues[i - 3]; }
                }
            }

            for(int i = 3; i < numBytes; i += 4)
            {
                if(argbValues[i] == 7)
                    argbValues[i] = 0;
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
            Marshal.Copy(argbValues, 0, ptr, numBytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            return bmp;
        }

        private static Bitmap renderWSmartHuge(MagicaVoxelData[] voxels, int facing, int palette, int frame, int maxFrames, bool still)
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
            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            switch(facing)
            {
                case 0:
                    vls = voxels;
                    break;
                case 1:
                    for(int i = 0; i < voxels.Length; i++)
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
                    for(int i = 0; i < voxels.Length; i++)
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
                    for(int i = 0; i < voxels.Length; i++)
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
            if(maxFrames >= 8) jitter = ((frame % 8 > 4) ? 4 - ((frame % 8) ^ 4) : frame % 8);
            FaceVoxel[,,] faces = FaceLogic.GetFaces(FaceLogic.VoxListToArray(vls.ToList(), xSize, ySize, zSize, 153));

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
                            if(mod_color == 17 && r.Next(7) < 2) //smoke
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
                                    mod_color -= r.Next(3);
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
                                        for(int sp = 0; sp < 17; sp++)
                                        {
                                            if((slope & Slopes[sp]) == Slopes[sp] && wcurrent[mod_color][sp][(i / 4 + 3) + j * 16] != 0)
                                            {
                                                barePositions[p] = !(VoxelLogic.wcolors[current_color][3] == VoxelLogic.bordered_alpha || VoxelLogic.wcolors[current_color][3] == VoxelLogic.bordered_flat_alpha);
                                                if(VoxelLogic.wcolors[current_color][3] == VoxelLogic.bordered_alpha || VoxelLogic.wcolors[current_color][3] == VoxelLogic.bordered_flat_alpha)
                                                    zbuffer[p] = vx.z + vx.x * 3 - vx.y * 3;

                                                argbValues[p] = wcurrent[mod_color][sp][i + j * 16];
                                                break;
                                            }
                                        }
                                        if(!barePositions[p] && outlineValues[p] == 0)
                                            outlineValues[p] = wcurrent[mod_color][0][i + 64];

                                    }
                                }
                            }
                        }
                        else if(current_color == 25)
                        {
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

                            for(int j = 0; j < 4; j++)
                            {
                                for(int i = 0; i < 16; i++)
                                {
                                    p = voxelToPixelHugeW(i, j, vx.x, vx.y, vx.z, mod_color, bmpData.Stride, jitter, still);

                                    if(argbValues[p] == 0 && argbValues[(p / 4) + 3] != 7)
                                    {
                                        for(int sp = 0; sp < 17; sp++)
                                        {
                                            if((slope & Slopes[sp]) == Slopes[sp] && wcurrent[mod_color][sp][((i / 4) * 4 + 3) + j * 16] != 0)
                                            {
                                                if(VoxelLogic.wcolors[mod_color][3] == VoxelLogic.gloss_alpha && i % 4 == 3 && r.Next(12) == 0)
                                                {
                                                    argbValues[p - 3] = (byte)Math.Min(wcurrent[mod_color][sp][i - 3 + j * 16] + 160, 255);
                                                    argbValues[p - 2] = (byte)Math.Min(wcurrent[mod_color][sp][i - 2 + j * 16] + 160, 255);
                                                    argbValues[p - 1] = (byte)Math.Min(wcurrent[mod_color][sp][i - 1 + j * 16] + 160, 255);
                                                    argbValues[p - 0] = 255;
                                                }
                                                else if(VoxelLogic.wcolors[mod_color][3] == VoxelLogic.grain_hard_alpha && i % 4 == 3)
                                                {
                                                    float n = Simplex.FindNoiseBold(facing, vx.x + 50, vx.y + 50, vx.z);
                                                    argbValues[p - 3] = (byte)Math.Min(wcurrent[mod_color][sp][i - 3 + j * 16] * n, 255);
                                                    argbValues[p - 2] = (byte)Math.Min(wcurrent[mod_color][sp][i - 2 + j * 16] * n, 255);
                                                    argbValues[p - 1] = (byte)Math.Min(wcurrent[mod_color][sp][i - 1 + j * 16] * n, 255);
                                                    argbValues[p - 0] = 255;
                                                }
                                                else if(VoxelLogic.wcolors[mod_color][3] == VoxelLogic.grain_some_alpha && i % 4 == 3)
                                                {
                                                    float n = Simplex.FindNoise(facing, vx.x + 50, vx.y + 50, vx.z);
                                                    argbValues[p - 3] = (byte)Math.Min(wcurrent[mod_color][sp][i - 3 + j * 16] * n, 255);
                                                    argbValues[p - 2] = (byte)Math.Min(wcurrent[mod_color][sp][i - 2 + j * 16] * n, 255);
                                                    argbValues[p - 1] = (byte)Math.Min(wcurrent[mod_color][sp][i - 1 + j * 16] * n, 255);
                                                    argbValues[p - 0] = 255;
                                                }
                                                else if(VoxelLogic.wcolors[mod_color][3] == VoxelLogic.grain_mild_alpha && i % 4 == 3)
                                                {
                                                    float n = Simplex.FindNoiseLight(facing, vx.x + 50, vx.y + 50, vx.z);
                                                    argbValues[p - 3] = (byte)Math.Min(wcurrent[mod_color][sp][i - 3 + j * 16] * n, 255);
                                                    argbValues[p - 2] = (byte)Math.Min(wcurrent[mod_color][sp][i - 2 + j * 16] * n, 255);
                                                    argbValues[p - 1] = (byte)Math.Min(wcurrent[mod_color][sp][i - 1 + j * 16] * n, 255);
                                                    argbValues[p - 0] = 255;
                                                }
                                                else if(VoxelLogic.wcolors[mod_color][3] == VoxelLogic.fuzz_alpha && i % 4 == 3)
                                                {
                                                    float n = Simplex.FindNoiseTight(frame % 4, facing, vx.x + 50, vx.y + 50, vx.z) + 0.3f;
                                                    argbValues[p - 3] = (byte)VoxelLogic.Clamp(wcurrent[mod_color][sp][i - 3 + j * 16] * n + 16 * n, 1, 255);
                                                    argbValues[p - 2] = (byte)VoxelLogic.Clamp(wcurrent[mod_color][sp][i - 2 + j * 16] * n + 16 * n, 1, 255);
                                                    argbValues[p - 1] = (byte)VoxelLogic.Clamp(wcurrent[mod_color][sp][i - 1 + j * 16] * n + 16 * n, 1, 255);
                                                    argbValues[p - 0] = 255;
                                                }
                                                else
                                                {
                                                    argbValues[p] = wcurrent[mod_color][sp][i + j * 16];
                                                }

                                                zbuffer[p] = vx.z + vx.x * 3 - vx.y * 3;
                                                barePositions[p] = (VoxelLogic.wcolors[mod_color][3] == VoxelLogic.flash_alpha_0 || VoxelLogic.wcolors[mod_color][3] == VoxelLogic.flash_alpha_1 || VoxelLogic.wcolors[mod_color][3] == VoxelLogic.borderless_alpha);
                                                break;
                                            }
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

            for(int i = 3; i < numBytes; i += 4)
            {
                if(argbValues[i] == 7)
                    argbValues[i] = 0;
            }
            bool lightOutline = !VoxelLogic.subtlePalettes.Contains(palette);
            for(int i = 3; i < numBytes; i += 4)
            {
                if(argbValues[i] > 255 * VoxelLogic.waver_alpha && barePositions[i] == false)
                {
                    bool shade = false, blacken = false;
                    /*
                    if (argbValues[i] > 0 && i - 4 >= 0 && i - 4 < argbValues.Length && argbValues[i - 4] == 0 && lightOutline) { editValues[i - 4] = 255; editValues[i - 4 - 1] = 0; editValues[i - 4 - 2] = 0; editValues[i - 4 - 3] = 0; blacken = true; } else if (i - 4 >= 0 && i - 4 < argbValues.Length && barePositions[i - 4] == false && zbuffer[i] - 2 > zbuffer[i - 4]) { editValues[i - 4] = 255; editValues[i - 4 - 1] = outlineValues[i - 1]; editValues[i - 4 - 2] = outlineValues[i - 2]; editValues[i - 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i + 4 >= 0 && i + 4 < argbValues.Length && argbValues[i + 4] == 0 && lightOutline) { editValues[i + 4] = 255; editValues[i + 4 - 1] = 0; editValues[i + 4 - 2] = 0; editValues[i + 4 - 3] = 0; blacken = true; } else if (i + 4 >= 0 && i + 4 < argbValues.Length && barePositions[i + 4] == false && zbuffer[i] - 2 > zbuffer[i + 4]) { editValues[i + 4] = 255; editValues[i + 4 - 1] = outlineValues[i - 1]; editValues[i + 4 - 2] = outlineValues[i - 2]; editValues[i + 4 - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && argbValues[i - bmpData.Stride] == 0 && lightOutline) { editValues[i - bmpData.Stride] = 255; editValues[i - bmpData.Stride - 1] = 0; editValues[i - bmpData.Stride - 2] = 0; editValues[i - bmpData.Stride - 3] = 0; blacken = true; } else if (i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && barePositions[i - bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride]) { editValues[i - bmpData.Stride] = 255; editValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; editValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; editValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
                    if (argbValues[i] > 0 && i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && argbValues[i + bmpData.Stride] == 0 && lightOutline) { editValues[i + bmpData.Stride] = 255; editValues[i + bmpData.Stride - 1] = 0; editValues[i + bmpData.Stride - 2] = 0; editValues[i + bmpData.Stride - 3] = 0; blacken = true; } else if (i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && barePositions[i + bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride]) { editValues[i + bmpData.Stride] = 255; editValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; editValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; editValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; if (!blacken) shade = true; }
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
                        //editValues[i] = 255; editValues[i - 1] = 0; editValues[i - 2] = 0; editValues[i - 3] = 0;
                    }
                    else if(shade) { editValues[i] = 255; editValues[i - 1] = outlineValues[i - 1]; editValues[i - 2] = outlineValues[i - 2]; editValues[i - 3] = outlineValues[i - 3]; }
                }
            }

            for(int i = 3; i < numBytes; i += 4)
            {
                if(argbValues[i] == 7)
                    argbValues[i] = 0;
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
            Marshal.Copy(argbValues, 0, ptr, numBytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            return bmp;
        }


        static void Main(string[] args)
        {
            //   altFolder = "botl4/";

            VoxelLogic.Initialize();

            SaPalettes.Initialize();
            InitializeWPalette();
            altFolder = "sau8/";

            processUnitLargeW("Rakgar", 18, true);

            processUnitLargeW("Lomuk", 13, false);
            processUnitLargeWalkW("Lomuk", 13);

            processUnitLargeW("Axarik", 0, true);
            processUnitLargeWalkW("Axarik", 0);
            processUnitLargeW("Tassar", 17, false);
            processUnitLargeWalkW("Tassar", 17);
            processUnitLargeW("Erezdo", 2, true);
            processUnitLargeWalkW("Erezdo", 2);
            processUnitLargeW("Ceglia", 1, true);
            processUnitLargeWalkW("Ceglia", 1);

            processUnitHugeW("Nodebpe", 14, true);
            processUnitWalkHugeW("Nodebpe", 14);

            processUnitLargeW("Glarosp", 3, true);
            processUnitLargeWalkW("Glarosp", 3);

            processUnitLargeW("Ilapa", 11, true);
            processUnitLargeWalkW("Ilapa", 11);
            processUnitLargeW("Kurguiv", 12, false);
            processUnitLargeWalkW("Kurguiv", 12);
            processUnitHugeW("Oah", 15, true);
            processUnitWalkHugeW("Oah", 15);
            processUnitLargeW("Sfyst", 16, true);
            processUnitLargeWalkW("Sfyst", 16);
            processUnitLargeW("Tassar", 17, false);
            processUnitLargeWalkW("Tassar", 17);
            processUnitHugeW("Vashk", 18, true);
            processUnitWalkHugeW("Vashk", 18);
            processUnitLargeW("Vih", 43, false);
            processUnitLargeWalkW("Vih", 43);


            processUnitLargeW("Human_Male", 4, true);
            processUnitLargeWalkW("Human_Male", 4);
            processUnitLargeW("Human_Male", 5, true);
            processUnitLargeWalkW("Human_Male", 5);
            processUnitLargeW("Human_Male", 6, true);
            processUnitLargeWalkW("Human_Male", 6);
            processUnitLargeW("Human_Male", 7, true);
            processUnitLargeWalkW("Human_Male", 7);
            processUnitLargeW("Human_Male", 8, true);
            processUnitLargeWalkW("Human_Male", 8);
            processUnitLargeW("Human_Male", 9, true);
            processUnitLargeWalkW("Human_Male", 9);
            processUnitLargeW("Human_Male", 10, true);
            processUnitLargeWalkW("Human_Male", 10);

            processUnitLargeW("Human_Female", 4, true);
            processUnitLargeWalkW("Human_Female", 4);
            processUnitLargeW("Human_Female", 5, true);
            processUnitLargeWalkW("Human_Female", 5);
            processUnitLargeW("Human_Female", 6, true);
            processUnitLargeWalkW("Human_Female", 6);
            processUnitLargeW("Human_Female", 7, true);
            processUnitLargeWalkW("Human_Female", 7);
            processUnitLargeW("Human_Female", 8, true);
            processUnitLargeWalkW("Human_Female", 8);
            processUnitLargeW("Human_Female", 9, true);
            processUnitLargeWalkW("Human_Female", 9);
            processUnitLargeW("Human_Female", 10, true);
            processUnitLargeWalkW("Human_Female", 10);

            /*
            processUnitHugeW("Barrel", 38, true);


            processUnitHugeW("Table", 39, true);
            processUnitHugeW("Desk", 39, true);
            processUnitHugeW("Computer_Desk", 39, true);
            processUnitHugeW("Computer_Desk", 40, true);

            processUnitHugeW("Table", 41, true);
            processUnitHugeW("Desk", 41, true);
            processUnitHugeW("Computer_Desk", 41, true);
            processUnitHugeW("Computer_Desk", 42, true);

            processUnitHugeW("Grass", 35, true);
            processUnitHugeW("Tree", 35, true);
            processUnitHugeW("Boulder", 36, true);
            processUnitHugeW("Rubble", 36, true);
            */

        }
    }
}
