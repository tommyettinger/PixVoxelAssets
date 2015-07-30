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
    class ForaysVoxels
    {
        private static int vwidth = 1, vheight = 2;
        private static Random r = new Random(0x1337beef);

        public const int factions = 1;
        public static string altFolder = "";

        public static int hcolorcount = 0, hpalettecount = 0;

        public static double[][] hcolors;

        public static byte[][] hrendered;

        public static byte clear = 255;

        public const float flat_alpha = VoxelLogic.flat_alpha;
        public const float fuzz_alpha = VoxelLogic.fuzz_alpha;
        public const float waver_alpha = VoxelLogic.waver_alpha;
        public const float yver_alpha = VoxelLogic.yver_alpha;
        public const float bordered_alpha = VoxelLogic.bordered_alpha;
        public const float bordered_flat_alpha = VoxelLogic.bordered_flat_alpha;
        public const float borderless_alpha = VoxelLogic.borderless_alpha;
        public const float eraser_alpha = VoxelLogic.eraser_alpha;
        public const float spin_alpha_0 = VoxelLogic.spin_alpha_0;
        public const float spin_alpha_1 = VoxelLogic.spin_alpha_1;
        public const float flash_alpha_0 = VoxelLogic.flash_alpha_0;
        public const float flash_alpha_1 = VoxelLogic.flash_alpha_1;
        public const float gloss_alpha = VoxelLogic.gloss_alpha;
        public const float grain_hard_alpha = VoxelLogic.grain_hard_alpha;
        public const float grain_some_alpha = VoxelLogic.grain_some_alpha;
        public const float grain_mild_alpha = VoxelLogic.grain_mild_alpha;
        public const float fade_alpha = VoxelLogic.fade_alpha;

        public static double Clamp(double x)
        {
            return Clamp(x, 0.0, 1.0);
        }

        public static double MercifulClamp(double x)
        {
            return Clamp(x, 0.01, 1.0);
        }

        public static double Clamp(double x, double min, double max)
        {
            return Math.Min(Math.Max(min, x), max);
        }

        public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }

        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

        private static byte[][] storeColorCubesH(Color basis)
        {

            clear = (byte)(253 - (hcolorcount - 1) * 4);
            DungeonPalettes.Initialize();

            byte[,] cubes = new byte[hcolorcount * 2, 4 * vwidth * (vheight + 1)];

            Image image = new Bitmap("white.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = vwidth;
            int height = vheight + 1;
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

            for (int current_color = 0; current_color < hcolorcount; current_color++)
            {
                Bitmap b =
                new Bitmap(width, height, PixelFormat.Format32bppArgb);

                Graphics g = Graphics.FromImage((Image)b);

                if (current_color == hcolorcount - 1)
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
       new float[] {0.22F+(basis.R / 256.0f),  0,  0,  0, 0},
       new float[] {0,  0.251F+ (basis.G / 256.0f),  0,  0, 0},
       new float[] {0,  0,  0.31F+ (basis.B / 256.0f),  0, 0},
       new float[] {0,  0,  0,  1F, 0},
       new float[] {0, 0, 0, 0, 1F}});
                }
                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);
                string which_image = (current_color >= hcolorcount - 2) ? "flat" : "cube";
                g.DrawImage(image,
                   new Rectangle(0, 0,
                       width, height),  // destination rectangle 
                                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        Color c = b.GetPixel(i, j);
                        Color c2 = b.GetPixel(i, j);
                        double h = 0.0, s = 1.0, v = 1.0;

                        ColorToHSV(c, out h, out s, out v);

                        switch (which_image)
                        {
                            case "cube":
                                {

                                    double smult = (current_color % 2 + 1) * 0.5,
                                        smult2 = smult - 0.25;
                                    double vmult = (current_color / 2) / 12.0;

                                    //top
                                    if (j <= 0)
                                    {
                                        c = ColorFromHSV((h) % 360,
                                                            MercifulClamp(s * smult * 1.1),
                                                            MercifulClamp(v * vmult * 1.05));
                                        c2 = ColorFromHSV((h) % 360,
                                                            MercifulClamp(s * smult2 * 1.1),
                                                            MercifulClamp(v * vmult * 1.05));
                                    }
                                    // outline
                                    else if (j == height - 1)
                                    {
                                        c = ColorFromHSV((h) % 360,
                                                            MercifulClamp(s * smult * 1.0),
                                                            MercifulClamp(v * vmult * 0.5));
                                        c2 = ColorFromHSV((h) % 360,
                                                            MercifulClamp(s * smult2 * 1.0),
                                                            MercifulClamp(v * vmult * 0.5));
                                    }
                                    //lightly shaded side
                                    else
                                    {
                                        c = ColorFromHSV((h) % 360,
                                                            MercifulClamp(s * smult * 1.35),
                                                            MercifulClamp(v * vmult * 0.8));
                                        c2 = ColorFromHSV((h) % 360,
                                                            MercifulClamp(s * smult2 * 1.35),
                                                            MercifulClamp(v * vmult * 0.8));
                                    }
                                }
                                break;
                            default:
                                {
                                    s = 0.0;
                                    v = 0.3;
                                    double smult = 0.0;
                                    double vmult = 0.85;
                                    //above the region that should have color
                                    if (current_color == hcolorcount - 1 || j < vheight / 2)
                                    {
                                        c = Color.Transparent;
                                        c2 = Color.Transparent;
                                    }
                                    //outline
                                    else if (j == height - 1)
                                    {
                                        c = ColorFromHSV((h) % 360,
                                                            MercifulClamp(s * smult * 1.0),
                                                            MercifulClamp(v * 0.5));
                                        c2 = ColorFromHSV((h) % 360,
                                                            MercifulClamp(s * smult * 1.0),
                                                            MercifulClamp(v * vmult * 0.5));
                                    }
                                    //middle part
                                    else
                                    {

                                        c = ColorFromHSV((h) % 360,
                                                            MercifulClamp(s * smult * 1.2),
                                                            MercifulClamp(v * vmult * 1.1));
                                        c2 = ColorFromHSV((h) % 360,
                                                            MercifulClamp(s * smult * 1.2),
                                                            MercifulClamp(v * vmult * 1.1));
                                    }
                                }
                                break;
                        }
                        if (c.A != 0)
                        {
                            cubes[current_color, i * 4 + j * width * 4 + 0] = Math.Max((byte)1, c.B);
                            cubes[current_color, i * 4 + j * width * 4 + 1] = Math.Max((byte)1, c.G);
                            cubes[current_color, i * 4 + j * width * 4 + 2] = Math.Max((byte)1, c.R);
                            cubes[current_color, i * 4 + j * width * 4 + 3] = c.A;


                            cubes[current_color + hcolorcount, i * 4 + j * width * 4 + 0] = Math.Max((byte)1, c2.B);
                            cubes[current_color + hcolorcount, i * 4 + j * width * 4 + 1] = Math.Max((byte)1, c2.G);
                            cubes[current_color + hcolorcount, i * 4 + j * width * 4 + 2] = Math.Max((byte)1, c2.R);
                            cubes[current_color + hcolorcount, i * 4 + j * width * 4 + 3] = c2.A;
                        }
                        else
                        {
                            cubes[current_color, i * 4 + j * 4 * width + 0] = 0;
                            cubes[current_color, i * 4 + j * 4 * width + 1] = 0;
                            cubes[current_color, i * 4 + j * 4 * width + 2] = 0;
                            cubes[current_color, i * 4 + j * 4 * width + 3] = 0;


                            cubes[current_color + hcolorcount, i * 4 + j * 4 * width + 0] = 0;
                            cubes[current_color + hcolorcount, i * 4 + j * 4 * width + 1] = 0;
                            cubes[current_color + hcolorcount, i * 4 + j * 4 * width + 2] = 0;
                            cubes[current_color + hcolorcount, i * 4 + j * 4 * width + 3] = 0;
                        }
                    }
                }
            }
            int blen = 4 * vwidth * (vheight + 1);
            byte[][] cubes2 = new byte[hcolorcount * 2][];
            for (int c = 0; c < hcolorcount * 2; c++)
            {
                cubes2[c] = new byte[blen];
                for (int j = 0; j < blen; j++)
                {
                    cubes2[c][j] = cubes[c, j];
                }
            }
            return cubes2;
        }

        public static void setupCurrentColorsH(Color basis)
        {
            hrendered = storeColorCubesH(basis);
        }
        // - ((current_color == hcolorcount - 2) ? -1 : 0)
        private static int voxelToPixelH16(int innerX, int innerY, int x, int y, int z, int current_color, int stride, int jitter, bool still)
        {
            return 4 * ((y) * vwidth)
                + innerX +
                stride * (16 * (vheight - 1) + x * 1 - z * (vheight - 1) - (vheight - 1) + innerY);
        }
        private static int voxelToPixelH32(int innerX, int innerY, int x, int y, int z, int current_color, int stride, int jitter, bool still)
        {
            return 4 * ((y) * vwidth)
                + innerX +
                stride * (32 * (vheight - 1) + x * 1 - z * (vheight - 1) - (vheight - 1) + innerY);
        }


        private static Bitmap renderH(MagicaVoxelData[] voxels, int facing, int frame, int maxFrames, bool still, bool darkOutline)
        {

            Bitmap bmp = new Bitmap(vwidth * 16, vheight * 16, PixelFormat.Format32bppArgb);

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
            byte[] outlineColors = new byte[numBytes];
            outlineColors.Fill<byte>(0);
            byte[] outlineValues = new byte[numBytes];
            outlineValues.Fill<byte>(0);
            bool[] barePositions = new bool[numBytes];
            barePositions.Fill<bool>(false);
            int xSize = 16, ySize = 16;
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
            int[] xbuffer = new int[numBytes];
            xbuffer.Fill<int>(-999);
            int[] zbuffer = new int[numBytes];
            zbuffer.Fill<int>(-999);


            int jitter = (((frame % 4) % 3) + ((frame % 4) / 3)) * 2;
            if (maxFrames >= 8) jitter = ((frame % 8 > 4) ? 4 - ((frame % 8) ^ 4) : frame % 8);

            foreach (MagicaVoxelData vx in vls.OrderByDescending(v => v.x * 40 - v.y + v.z * 40 * 40 - ((VoxelLogic.WithoutShadingK(v.color) == 23) ? 40 * 40 * 40 : 0))) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int unshaded = VoxelLogic.WithoutShadingK(vx.color);
                int current_color = ((255 - vx.color) % 4 == 0) ? (255 - vx.color) / 4 + hcolorcount : ((254 - vx.color) % 4 == 0) ? (253 - clear) / 4 : (253 - vx.color) / 4;
                bool is_shaded = (unshaded != current_color);
                int p = 0;
                if ((255 - vx.color) % 4 != 0 && (253 - vx.color) % 4 != 0)
                    continue;
                if ((255 - vx.color) % 4 != 0 && current_color >= hcolorcount)
                    continue;

                if (unshaded == hcolorcount - 1)
                {
                    for (int j = 0; j < vheight; j++)
                    {
                        for (int i = 0; i < 4 * vwidth; i++)
                        {
                            p = voxelToPixelH16(i, j, vx.x, vx.y, vx.z, current_color, bmpData.Stride, jitter, still);

                            if (shadowValues[p] == 0)
                            {
                                shadowValues[p] = hrendered[current_color][i + j * (vwidth * 4)];
                            }
                        }
                    }
                }
                else
                {

                    for (int j = 0; j < vheight; j++)
                    {
                        for (int i = 0; i < 4 * vwidth; i++)
                        {
                            p = voxelToPixelH16(i, j, vx.x, vx.y, vx.z, current_color, bmpData.Stride, jitter, still);

                            if (argbValues[p] == 0)
                            {
                                zbuffer[p] = vx.z;
                                xbuffer[p] = vx.x;
                                argbValues[p] = hrendered[current_color][i + j * (vwidth * 4)];
                                if (outlineColors[p] == 0)
                                    outlineColors[p] = hrendered[current_color][i + (4 * vwidth * vheight)]; //(argbValues[p] * 1.2 + 2 < 255) ? (byte)(argbValues[p] * 1.2 + 2) : (byte)255;

                            }
                        }
                    }
                }
            }
            /*
            for (int i = 3; i < numBytes; i += 4)
            {
                if (argbValues[i] > 255 * waver_alpha)
                {
                    if (i + 4 >= 0 && i + 4 < argbValues.Length && argbValues[i + 4] == 0 && darkOutline && outlineValues[i + 4] == 0) { outlineValues[i + 4] = 255; } else if (i + 4 >= 0 && i + 4 < argbValues.Length && barePositions[i + 4] == false && (zbuffer[i] - 1 > zbuffer[i + 4] || xbuffer[i] - 1 > xbuffer[i + 4]) && outlineValues[i + 4] == 0) { outlineValues[i + 4] = 255; outlineValues[i + 4 - 1] = outlineColors[i - 1]; outlineValues[i + 4 - 2] = outlineColors[i - 2]; outlineValues[i + 4 - 3] = outlineColors[i - 3]; }
                    if (i - 4 >= 0 && i - 4 < argbValues.Length && argbValues[i - 4] == 0 && darkOutline && outlineValues[i - 4] == 0) { outlineValues[i - 4] = 255; } else if (i - 4 >= 0 && i - 4 < argbValues.Length && barePositions[i - 4] == false && (zbuffer[i] - 1 > zbuffer[i - 4] || xbuffer[i] - 1 > xbuffer[i - 4]) && outlineValues[i - 4] == 0) { outlineValues[i - 4] = 255; outlineValues[i - 4 - 1] = outlineColors[i - 1]; outlineValues[i - 4 - 2] = outlineColors[i - 2]; outlineValues[i - 4 - 3] = outlineColors[i - 3]; }
                    if (i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && argbValues[i + bmpData.Stride] == 0 && darkOutline && outlineValues[i + bmpData.Stride] == 0) { outlineValues[i + bmpData.Stride] = 255; } else if (i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && barePositions[i + bmpData.Stride] == false && (zbuffer[i] - 1 > zbuffer[i + bmpData.Stride] || xbuffer[i] - 1 > xbuffer[i + bmpData.Stride]) && outlineValues[i + bmpData.Stride] == 0) { outlineValues[i + bmpData.Stride] = 255; outlineValues[i + bmpData.Stride - 1] = outlineColors[i - 1]; outlineValues[i + bmpData.Stride - 2] = outlineColors[i - 2]; outlineValues[i + bmpData.Stride - 3] = outlineColors[i - 3]; }
                    if (i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && argbValues[i - bmpData.Stride] == 0 && darkOutline && outlineValues[i - bmpData.Stride] == 0) { outlineValues[i - bmpData.Stride] = 255; } else if (i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && barePositions[i - bmpData.Stride] == false && (zbuffer[i] - 1 > zbuffer[i - bmpData.Stride] || xbuffer[i] - 1 > xbuffer[i - bmpData.Stride]) && outlineValues[i - bmpData.Stride] == 0) { outlineValues[i - bmpData.Stride] = 255; outlineValues[i - bmpData.Stride - 1] = outlineColors[i - 1]; outlineValues[i - bmpData.Stride - 2] = outlineColors[i - 2]; outlineValues[i - bmpData.Stride - 3] = outlineColors[i - 3]; }
                }
            }
            */
            for (int i = 3; i < numBytes; i += 4)
            {
                if (argbValues[i] > 0)
                    argbValues[i] = 255;

                if (outlineValues[i] == 255)
                {
                    argbValues[i] = 255;
                    argbValues[i - 1] = outlineValues[i - 1];
                    argbValues[i - 2] = outlineValues[i - 2];
                    argbValues[i - 3] = outlineValues[i - 3];
                }
            }

            for (int s = 3; s < numBytes; s += 4)
            {
                if (shadowValues[s] > 0)
                {
                    if (argbValues[s] == 0)
                    {
                        argbValues[s - 3] = shadowValues[s - 3];
                        argbValues[s - 2] = shadowValues[s - 2];
                        argbValues[s - 1] = shadowValues[s - 1];
                        argbValues[s - 0] = shadowValues[s - 0];
                    }
                }
            }
            /*
            for (int s = 3; s < numBytes; s += 4)
            {
                if (shadowValues[s] > 0)
                {
                    foreach (int i in new int[]{ s + 4, s - 4, s + bmpData.Stride, s - bmpData.Stride
                        //, s + bmpData.Stride + 4, s - bmpData.Stride + 4, s + bmpData.Stride - 4, s - bmpData.Stride - 4
                    })
                    {
                        if (i >= 3 && i < argbValues.Length && argbValues[i] == 0)
                        {
                            argbValues[i - 3] = (byte)(shadowValues[s - 3] + 50);
                            argbValues[i - 2] = (byte)(shadowValues[s - 2] + 50);
                            argbValues[i - 1] = (byte)(shadowValues[s - 1] + 50);
                            argbValues[i - 0] = shadowValues[s - 0];
                        }
                    }
                }
            }*/
            Marshal.Copy(argbValues, 0, ptr, numBytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            return bmp;
        }

        private static Bitmap generateWaterMask(int facing, int variant)
        {
            setupCurrentColorsH(Color.FromArgb(100, 230, 250));
            Bitmap bmp = new Bitmap(vwidth * 16, vheight * 16, PixelFormat.Format32bppArgb);

            //            Bitmap bmp = new Bitmap(4 * 128 + 8, 2 * 128 + 8, PixelFormat.Format32bppArgb);

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
            byte[] outlineColors = new byte[numBytes];
            outlineColors.Fill<byte>(0);
            byte[] outlineValues = new byte[numBytes];
            outlineValues.Fill<byte>(0);
            bool[] barePositions = new bool[numBytes];
            barePositions.Fill(false);

            int[] xbuffer = new int[numBytes];
            xbuffer.Fill<int>(-999);
            int[] zbuffer = new int[numBytes];
            zbuffer.Fill<int>(-999);

            for (int mvdx = 15; mvdx >= 0; mvdx--)
            {
                for (int mvdy = 0; mvdy <= 15; mvdy++)
                {
                    MagicaVoxelData vx = new MagicaVoxelData { x = (byte)mvdx, y = (byte)mvdy, z = 0, color = 153 };
                    int current_color = hcolorcount + 19;
                    //int unshaded = VoxelLogic.WithoutShadingK(vx.color);
                    //int current_color = ((255 - vx.color) % 4 == 0) ? (255 - vx.color) / 4 + hcolorcount : ((254 - vx.color) % 4 == 0) ? (253 - clear) / 4 : (253 - vx.color) / 4;
                    int p = 0;
                    int mod_color = current_color;

                    for (int j = 0; j < vheight; j++)
                    {
                        for (int i = 0; i < 4 * vwidth; i++)
                        {
                            //p = 4 * ((vx.x + vx.y) * 2 + 2) + i + bmpData.Stride * (128 + 2 - vx.y + vx.x + j);
                            mod_color = current_color;
                            p = voxelToPixelH16(i, j, vx.x, vx.y, 0, current_color, bmpData.Stride, 0, true);
                            if (argbValues[p] == 0)
                            {
                                zbuffer[p] = vx.z;
                                xbuffer[p] = vx.x;
                                if (i % 4 == 3)
                                {
                                    double wave = Simplex.FindNoiseFlatWater(facing, vx.x, vx.y, variant);

                                    if (wave > 0.73)
                                    {
                                        wave = 85 * wave;
                                    }
                                    else if (wave > 0.64)
                                    {
                                        wave = 65 * wave;
                                    }
                                    else if (wave > 0.55)
                                    {
                                        wave = 50 * wave;
                                    }
                                    else if (wave < 0.45)
                                    {

                                        wave += 0.2;
                                        if (wave < 0.5)
                                        {
                                            wave = -15 / wave;
                                            mod_color -= hcolorcount;
                                        }
                                        else if (wave < 0.55)
                                        {
                                            wave = -11 / wave;
                                            mod_color -= hcolorcount;
                                        }
                                        else if (wave < 0.6)
                                        {
                                            wave = -7 / wave;
                                            mod_color--;
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
                                    wave = Clamp(wave, -72.0, 72.0);
                                    mod_color = (byte)(((int)(wave / 12.2)) * 2 + mod_color);


                                    argbValues[p - 3] = hrendered[mod_color][i - 3 + j * (vwidth * 4)];
                                    argbValues[p - 2] = hrendered[mod_color][i - 2 + j * (vwidth * 4)];
                                    argbValues[p - 1] = hrendered[mod_color][i - 1 + j * (vwidth * 4)];
                                    argbValues[p - 0] = 255;
                                    if (outlineColors[p] == 0)
                                    {
                                        outlineColors[p] = hrendered[mod_color][i + (4 * vwidth * vheight)];
                                    }
                                }
                            }
                        }

                    }
                }
            }
            /*
            bool darkOutline = false;
            for (int i = 3; i < numBytes; i += 4)
            {
                if (argbValues[i] > 255 * waver_alpha && barePositions[i] == false)
                {

                    if (i + 4 >= 0 && i + 4 < argbValues.Length && argbValues[i + 4] == 0 && darkOutline && outlineValues[i + 4] == 0) { outlineValues[i + 4] = 255; } else if (i + 4 >= 0 && i + 4 < argbValues.Length && barePositions[i + 4] == false && (zbuffer[i] - 1 > zbuffer[i + 4] || xbuffer[i] - 1 > xbuffer[i + 4]) && outlineValues[i + 4] == 0) { outlineValues[i + 4] = 255; outlineValues[i + 4 - 1] = outlineColors[i - 1]; outlineValues[i + 4 - 2] = outlineColors[i - 2]; outlineValues[i + 4 - 3] = outlineColors[i - 3]; }
                    if (i - 4 >= 0 && i - 4 < argbValues.Length && argbValues[i - 4] == 0 && darkOutline && outlineValues[i - 4] == 0) { outlineValues[i - 4] = 255; } else if (i - 4 >= 0 && i - 4 < argbValues.Length && barePositions[i - 4] == false && (zbuffer[i] - 1 > zbuffer[i - 4] || xbuffer[i] - 1 > xbuffer[i - 4]) && outlineValues[i - 4] == 0) { outlineValues[i - 4] = 255; outlineValues[i - 4 - 1] = outlineColors[i - 1]; outlineValues[i - 4 - 2] = outlineColors[i - 2]; outlineValues[i - 4 - 3] = outlineColors[i - 3]; }
                    if (i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && argbValues[i + bmpData.Stride] == 0 && darkOutline && outlineValues[i + bmpData.Stride] == 0) { outlineValues[i + bmpData.Stride] = 255; } else if (i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && barePositions[i + bmpData.Stride] == false && (zbuffer[i] - 1 > zbuffer[i + bmpData.Stride] || xbuffer[i] - 1 > xbuffer[i + bmpData.Stride]) && outlineValues[i + bmpData.Stride] == 0) { outlineValues[i + bmpData.Stride] = 255; outlineValues[i + bmpData.Stride - 1] = outlineColors[i - 1]; outlineValues[i + bmpData.Stride - 2] = outlineColors[i - 2]; outlineValues[i + bmpData.Stride - 3] = outlineColors[i - 3]; }
                    if (i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && argbValues[i - bmpData.Stride] == 0 && darkOutline && outlineValues[i - bmpData.Stride] == 0) { outlineValues[i - bmpData.Stride] = 255; } else if (i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && barePositions[i - bmpData.Stride] == false && (zbuffer[i] - 1 > zbuffer[i - bmpData.Stride] || xbuffer[i] - 1 > xbuffer[i - bmpData.Stride]) && outlineValues[i - bmpData.Stride] == 0) { outlineValues[i - bmpData.Stride] = 255; outlineValues[i - bmpData.Stride - 1] = outlineColors[i - 1]; outlineValues[i - bmpData.Stride - 2] = outlineColors[i - 2]; outlineValues[i - bmpData.Stride - 3] = outlineColors[i - 3]; }
                }

            }
            */


            for (int i = 3; i < numBytes; i += 4)
            {
                if (argbValues[i] > 0)
                    argbValues[i] = 255;
                if (outlineValues[i] == 255)
                {
                    argbValues[i] = 255;
                    argbValues[i - 1] = outlineValues[i - 1];
                    argbValues[i - 2] = outlineValues[i - 2];
                    argbValues[i - 3] = outlineValues[i - 3];
                }
            }

            Marshal.Copy(argbValues, 0, ptr, numBytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            return bmp;

        }

        private static Bitmap processHFrame(MagicaVoxelData[] parsed, int dir, int frame, int maxFrames, bool still, bool darkOutline)
        {
            Bitmap b;
            Bitmap b2 = new Bitmap(vwidth * 16, vheight * 16, PixelFormat.Format32bppArgb);

            b = renderH(parsed, dir, frame, maxFrames, still, darkOutline);
            /*
            Graphics g2 = Graphics.FromImage(b2);
            g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g2.DrawImage(b.Clone(new Rectangle(0, 0, 248 * 2, 308 * 2), b.PixelFormat), -40, -80, 248, 308);
            g2.Dispose();*/
            return b;
        }
        public static void processUnitH(string unit, Color basis, bool autoshade)
        {
            string colorName = basis.ToString();
            colorName = System.Text.RegularExpressions.Regex.Match(colorName, "(?<=\\[)[^\\]]+").Value;
            Console.WriteLine("Processing: " + unit + ", base color " + colorName);
            BinaryReader bin = new BinaryReader(File.Open("Forays/" + unit + "_H.vox", FileMode.Open));
            List<MagicaVoxelData> voxes = VoxelLogic.PlaceShadowsH(VoxelLogic.FromMagicaRaw(bin));

            setupCurrentColorsH(basis);
            Directory.CreateDirectory("vox/Forays/" + altFolder);
            WriteVOX("vox/Forays/" + altFolder + unit + "_" + colorName + ".vox", voxes, 16, 16, 20, basis);
            MagicaVoxelData[] parsed = voxes.ToArray();

            int framelimit = 1;

            string folder = (altFolder);//"color" + i;
            Directory.CreateDirectory(folder); //("color" + i);
            for (int f = 0; f < framelimit; f++)
            { //
                for (int dir = 0; dir < 4; dir++)
                {
                    Bitmap b = processHFrame(parsed, dir, f, framelimit, true, true);
                    b.Save(folder + unit + "_" + colorName + "_face" + dir + "_" + f + ".png", ImageFormat.Png);
                    b.Dispose();
                }
            }


            Directory.CreateDirectory("gifs/Forays/" + altFolder);
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";

            s = folder + unit + "_" + colorName + "_face* ";
            startInfo.Arguments = "-dispose background -delay 25 -loop 0 " + s + " gifs/Forays/" + altFolder + unit + "_" + colorName + "_animated.gif";
            Process.Start(startInfo).WaitForExit();
        }


        
        public static void processTerrainH(string unit, string subfolder, Color basis, bool still, bool autoshade, bool addFloor)
        {
            string colorName = basis.ToString();
            colorName = System.Text.RegularExpressions.Regex.Match(colorName, "(?<=\\[)[^\\]]+").Value;

            Console.WriteLine("Processing: " + unit + ", base color " + colorName);
            List<MagicaVoxelData> voxes;
            if (addFloor)
            {
                BinaryReader bin = new BinaryReader(File.Open("Forays/" + subfolder + "/" + unit + "_H.vox", FileMode.Open));
                BinaryReader binFloor = new BinaryReader(File.Open("Forays/" + subfolder + "/Floor_H.vox", FileMode.Open));
                IEnumerable<MagicaVoxelData> structure = ((autoshade) ? VoxelLogic.AutoShadeH(VoxelLogic.FromMagicaRaw(bin), 16, 16, 16) : VoxelLogic.FromMagicaRaw(bin))
                    .Select(v => VoxelLogic.AlterVoxel(v, 0, 0, 1, v.color));
                voxes = structure.Concat((autoshade) ? VoxelLogic.AutoShadeH(VoxelLogic.FromMagicaRaw(binFloor), 16, 16, 16) : VoxelLogic.FromMagicaRaw(binFloor)).ToList();
            }
            else
            {
                BinaryReader bin = new BinaryReader(File.Open("Forays/" + subfolder + "/" + unit + "_H.vox", FileMode.Open));
                voxes = VoxelLogic.PlaceShadowsH((autoshade) ? VoxelLogic.AutoShadeH(VoxelLogic.FromMagicaRaw(bin), 16, 16, 16) : VoxelLogic.FromMagicaRaw(bin));
            }

            Directory.CreateDirectory("vox/Forays/" + altFolder + subfolder + "/");
            WriteVOX("vox/Forays/" + altFolder + subfolder + "/" + unit + "_" + colorName + ".vox", voxes, 16, 16, 20, basis);
            MagicaVoxelData[] parsed = voxes.ToArray();

            int framelimit = 1;
            

            string folder = (altFolder + subfolder + "/");
            Directory.CreateDirectory(folder);
            for (int f = 0; f < framelimit; f++)
            { //
                for (int dir = 0; dir < 4; dir++)
                {
                    Bitmap b = processHFrame(parsed, dir, f, framelimit, still, false);
                    b.Save(folder + unit + "_" + colorName + "_face" + dir + "_" + f + ".png", ImageFormat.Png);
                    b.Dispose();
                }
            }
            Directory.CreateDirectory("gifs/Forays/" + altFolder + subfolder + "/");
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";

            s = folder + unit + "_" + colorName + "_face* ";
            startInfo.Arguments = "-dispose background -delay 25 -loop 0 " + s + " gifs/Forays/" + altFolder + subfolder + "/" + unit + "_" + colorName + "_animated.gif";
            Process.Start(startInfo).WaitForExit();

        }

        public static void processWaterH(string subfolder)
        {
            Console.WriteLine("Processing: Water Tiles");
            // int framelimit = 4;
            
            string folder = (altFolder + subfolder + "/");
            Directory.CreateDirectory(folder);
            for (int v = 0; v < 16; v++)
            { //
                for (int dir = 0; dir < 4; dir++)
                {
                    Bitmap b = generateWaterMask(dir, v);
                    b.Save(folder + "Water_face" + dir + "_" + string.Format("{0:x}", v) + ".png", ImageFormat.Png);
                    b.Dispose();
                }
            }
            /*
            Directory.CreateDirectory("gifs/Mini/" + altFolder + subfolder + "/faction" + faction);
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";

            s = folder + "/palette" + palette + "(0)_Water_face* ";
            startInfo.Arguments = "-dispose background -delay 25 -loop 0 " + s + " gifs/Mini/" + altFolder + subfolder + "/faction" + faction + "/palette" + palette + "(0)_Water_animated.gif";
            Process.Start(startInfo).WaitForExit();
            */
        }
        private static int rowWidthBytes = 4;
        /// <summary>
        /// Write a MagicaVoxel .vox format file from a List of MagicaVoxelData and a palette from this program to use.
        /// </summary>
        /// <param name="filename">Name of the file to write.</param>
        /// <param name="voxelData">The voxels in indexed-color mode.</param>
        /// <returns>The voxel chunk data for the MagicaVoxel .vox file.</returns>
        public static void WriteVOX(string filename, List<MagicaVoxelData> voxelData, int xSize, int ySize, int zSize, Color basis)
        {
            // check out http://voxel.codeplex.com/wikipage?title=VOX%20Format&referringTitle=Home for the file format used below

            Stream stream = File.OpenWrite(filename);
            BinaryWriter bin = new BinaryWriter(stream);
            bool[,,] taken = new bool[xSize, ySize, zSize].Fill(false);

            List<byte> voxelsRaw = new List<byte>(voxelData.Count * 4);
            string colorName = basis.ToString();
            colorName = System.Text.RegularExpressions.Regex.Match(colorName, "(?<=\\[)[^\\]]+").Value;

            byte[] colors = new byte[1024];
            hrendered = storeColorCubesH(basis);
            foreach (MagicaVoxelData mvd in voxelData)
            {
                int unshaded = VoxelLogic.WithoutShadingK(mvd.color);
                if (mvd.x < xSize && mvd.y < ySize && mvd.z < zSize && !taken[mvd.x, mvd.y, mvd.z] && unshaded != hcolorcount - 2 && mvd.color > 253 - hcolorcount * 4)
                {
                    int current_color = ((255 - mvd.color) % 4 == 0) ? unshaded + hcolorcount : unshaded;
                    if ((255 - mvd.color) % 4 != 0 && current_color >= hcolorcount)
                        continue;
                    
                    voxelsRaw.Add((byte)(mvd.x));
                    voxelsRaw.Add((byte)(mvd.y));
                    voxelsRaw.Add((byte)(mvd.z));
                    voxelsRaw.Add((byte)(mvd.color));
                    taken[mvd.x, mvd.y, mvd.z] = true;
                }
            }
            for (int i = 1; i < 256; i++)
            {
                if ((253 - i) % 4 == 0 && (253 - i) / 4 < hcolorcount)
                {
                    colors[(i - 1) * 4] = hrendered[(253 - i) / 4][2 + rowWidthBytes];
                    colors[(i - 1) * 4 + 1] = hrendered[(253 - i) / 4][1 + rowWidthBytes];
                    colors[(i - 1) * 4 + 2] = hrendered[(253 - i) / 4][0 + rowWidthBytes];
                    colors[(i - 1) * 4 + 3] = hrendered[(253 - i) / 4][3 + rowWidthBytes];
                }
                else if ((255 - i) % 4 == 0 && hcolorcount + (255 - i) / 4 < hrendered.Length)
                {
                    colors[(i - 1) * 4] = hrendered[(255 - i) / 4 + hcolorcount][2 + rowWidthBytes];
                    colors[(i - 1) * 4 + 1] = hrendered[(255 - i) / 4 + hcolorcount][1 + rowWidthBytes];
                    colors[(i - 1) * 4 + 2] = hrendered[(255 - i) / 4 + hcolorcount][0 + rowWidthBytes];
                    colors[(i - 1) * 4 + 3] = hrendered[(255 - i) / 4 + hcolorcount][3 + rowWidthBytes];
                }
                else
                {
                    colors[(i - 1) * 4] = (byte)(VoxelLogic.mv_default_palette[i] & 0xff);
                    colors[(i - 1) * 4 + 1] = (byte)((VoxelLogic.mv_default_palette[i] >> 8) & 0xff);
                    colors[(i - 1) * 4 + 2] = (byte)((VoxelLogic.mv_default_palette[i] >> 16) & 0xff);
                    colors[(i - 1) * 4 + 3] = (byte)((VoxelLogic.mv_default_palette[i] >> 24) & 0xff);
                }
            }
            

            // a MagicaVoxel .vox file starts with a 'magic' 4 character 'VOX ' identifier
            bin.Write("VOX ".ToCharArray());
            // current version?
            bin.Write((int)150);

            bin.Write("MAIN".ToCharArray());
            bin.Write((int)0);
            bin.Write((int)12 + 12 + 12 + 4 + voxelsRaw.Count + 12 + 1024);

            bin.Write("SIZE".ToCharArray());
            bin.Write((int)12);
            bin.Write((int)0);
            bin.Write(xSize);
            bin.Write(ySize);
            bin.Write(zSize);

            bin.Write("XYZI".ToCharArray());
            bin.Write((int)(4 + voxelsRaw.Count));
            bin.Write((int)0);
            bin.Write((int)(voxelsRaw.Count / 4));
            bin.Write(voxelsRaw.ToArray());

            bin.Write("RGBA".ToCharArray());
            bin.Write((int)1024);
            bin.Write((int)0);
            bin.Write(colors);

            bin.Flush();
            bin.Close();
        }

        static void Main(string[] args)
        {
            VoxelLogic.Initialize();
            altFolder = "Forays/";
            Directory.CreateDirectory("Forays/" + altFolder + "Caves/");

            VoxelLogic.hpalettecount = hpalettecount = 1;
            VoxelLogic.hcolorcount = hcolorcount = 62;
            VoxelLogic.clear = (byte)(253 - (VoxelLogic.hcolorcount - 1) * 4);

            hrendered = storeColorCubesH(Color.Honeydew);
            VoxelLogic.hrendered = hrendered;


            processUnitH("Human_Male", Color.DarkSlateBlue, false);
            processUnitH("Human_Female", Color.Magenta, false);

            processUnitH("Warrior_Male", Color.DimGray, false);
            processUnitH("Warrior_Female", Color.Gray, false);
            processUnitH("Mage_Male", Color.Chartreuse, false);
            processUnitH("Mage_Female", Color.HotPink, false);
            processUnitH("Rogue_Male", Color.Goldenrod, false);
            processUnitH("Rogue_Female", Color.Goldenrod, false);
            processUnitH("Priest_Male", Color.GhostWhite, false);
            processUnitH("Priest_Female", Color.DarkGray, false);
            
            processWaterH("Caves");

            
            processTerrainH("Floor", "Caves", Color.LightGray, true, false, false);
            processTerrainH("Wall_Straight", "Caves", Color.LightGray, true, false, false);
            processTerrainH("Wall_Corner", "Caves", Color.LightGray, true, false, false);
            processTerrainH("Wall_Tee", "Caves", Color.LightGray, true, false, false);
            processTerrainH("Wall_Cross", "Caves", Color.LightGray, true, false, false);
            processTerrainH("Door_Closed", "Caves", Color.SaddleBrown, true, false, false);
            processTerrainH("Door_Open", "Caves", Color.SaddleBrown, true, false, false);
            

        }
    }
}