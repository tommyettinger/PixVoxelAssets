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
    class MiniOrthoVoxels
    {
        private static int vwidth = 2, vheight = 4;
        public static PRNG r = PRNG.r;
        public static uint[] rState = PRNG.rState, altState = PRNG.altState;

        public const int factions = 1;
        public static string altFolder = "";

        public static int kcolorcount = 0, kpalettecount = 0;

        public static float[][] kcolors;

        public static byte[][][][] krendered;
        public static byte[][] kcurrent;
        public static byte[][][] kFleshRendered;
        public static byte[][] kFleshCurrent;

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

        private static byte[][][][] storeColorCubesK()
        {
            kcolorcount = DungeonPalettes.kdungeon[0][0].Length;
            clear = (byte)(253 - (kcolorcount - 1) * 4);
            DungeonPalettes.Initialize();

            kpalettecount = DungeonPalettes.kdungeon[0].Length;

            byte[,,,] cubes = new byte[DungeonPalettes.kdungeon.Length, kpalettecount, DungeonPalettes.kdungeon[0][0].Length * 2, 4 * vwidth * (vheight + 1)];

            for (int k = 0; k < DungeonPalettes.kdungeon.Length; k++)
            {
                float[][][] kpalettes = DungeonPalettes.kdungeon[k];
                float[][][] contrast = kpalettes.Replicate();
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
                for (int p = 0; p < kpalettes.Length; p++)
                {
                    for (int current_color = 0; current_color < kpalettes[p].Length; current_color++)
                    {
                        Bitmap b =
                        new Bitmap(width, height, PixelFormat.Format32bppArgb);

                        Graphics g = Graphics.FromImage((Image)b);

                        if (kpalettes[p][current_color][3] == eraser_alpha)
                        {
                            colorMatrix = new ColorMatrix(new float[][]{
       new float[] {0,  0,  0,  0, 0},
       new float[] {0,  0,  0,  0, 0},
       new float[] {0,  0,  0,  0, 0},
       new float[] {0,  0,  0,  1F, 0},
       new float[] {0,  0,  0,  0, 1F}});
                        }
                        else if (kpalettes[p][current_color][3] == 0F)
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
       new float[] {0.22F+kpalettes[p][current_color][0],  0,  0,  0, 0},
       new float[] {0,  0.251F+kpalettes[p][current_color][1],  0,  0, 0},
       new float[] {0,  0,  0.31F+kpalettes[p][current_color][2],  0, 0},
       new float[] {0,  0,  0,  1F, 0},
       new float[] {0, 0, 0, 0, 1F}});
                        }
                        imageAttributes.SetColorMatrix(
                           colorMatrix,
                           ColorMatrixFlag.Default,
                           ColorAdjustType.Bitmap);
                        string which_image = ((current_color >= 14 && current_color <= 22) || kpalettes[p][current_color][3] == 0F
                            || kpalettes[p][current_color][3] == flash_alpha_0 || kpalettes[p][current_color][3] == flash_alpha_1 || kpalettes[p][current_color][3] == borderless_alpha) ? "shine" :
                            (kpalettes[p][current_color][3] == flat_alpha || kpalettes[p][current_color][3] == bordered_flat_alpha) ? "flat" : "cube";
                        g.DrawImage(image,
                           new Rectangle(0, 0,
                               width, height),  // destination rectangle 
                                                //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                           0, 0,        // upper-left corner of source rectangle 
                           width,       // width of source rectangle
                           height,      // height of source rectangle
                           GraphicsUnit.Pixel,
                           imageAttributes);

                        int alt_k = 0;
                        double softness = 0.0;
                        if (current_color != 25 && (current_color < 3 || current_color > 7) && (current_color < 38 || current_color > 41) && (current_color < 12 || current_color > 18))
                        {
                            alt_k = k;
                        }
                        if (current_color == 25)
                        {
                            softness = 0.6;
                        }
                        for (int i = 0; i < width; i++)
                        {
                            for (int j = 0; j < height; j++)
                            {
                                Color c = b.GetPixel(i, j);
                                double h = 0.0, s = 1.0, v = 1.0;
                                double h2 = 0.0, s2 = 1.0, v2 = 1.0;
                                ColorToHSV(c, out h, out s, out v);

                                switch (which_image)
                                {
                                    case "cube":
                                        {

                                            //top
                                            if (j <= 1)
                                            {
                                                //ColorToHSV(c, out h, out s, out v);
                                                c = ColorFromHSV((h + alt_k * 100) % 360,
                                                                    MercifulClamp(s * (1.2 - alt_k * 0.2) - 0.1 * softness),
                                                                    MercifulClamp(v * ((kpalettes[p][current_color][0] + kpalettes[p][current_color][1] + kpalettes[p][current_color][2] > 2.5) ? 1.15 : 1.1 - alt_k * 0.15)));
                                            }
                                            // outline
                                            else if (j == height - 1)
                                            {
                                                //ColorToHSV(c, out h, out s, out v);
                                                //outline
                                                if (kpalettes[p][current_color][3] == fade_alpha)
                                                {
                                                    c = ColorFromHSV((h + alt_k * 100) % 360,
                                                                    0.1,
                                                                    MercifulClamp(v * 0.65));
                                                }
                                                else
                                                {
                                                    c = ColorFromHSV((h + alt_k * 100) % 360,
                                                        MercifulClamp(s * (1.0 - alt_k * 0.2) - 0.3 * softness),
                                                        MercifulClamp(v * ((kpalettes[p][current_color][0] + kpalettes[p][current_color][1] + kpalettes[p][current_color][2] > 2.5) ? 0.65 : 0.5 - alt_k * 0.2 + 0.1 * softness)));
                                                }
                                            }
                                            //lightly shaded side
                                            else
                                            {
                                                //ColorToHSV(c, out h, out s, out v);
                                                c = ColorFromHSV((h + alt_k * 100) % 360,
                                                        MercifulClamp(s * (1.3 - alt_k * 0.2) - 0.2 * softness),
                                                        MercifulClamp(v * ((kpalettes[p][current_color][0] + kpalettes[p][current_color][1] + kpalettes[p][current_color][2] > 2.5) ? 1.0 : 0.9 - alt_k * 0.2 + 0.05 * softness)));
                                            }
                                        }
                                        break;
                                    case "flat":
                                        {
                                            //above the region that should have color
                                            if (j < vheight / 2)
                                            {
                                                c = Color.Transparent;
                                            }
                                            //outline
                                            else if (j == height - 1)
                                            {

                                                if (kpalettes[p][current_color][3] == fade_alpha)
                                                {
                                                    c = ColorFromHSV((h + alt_k * 100) % 360,
                                                                    0.1,
                                                                    MercifulClamp(v * 0.65));
                                                }
                                                else
                                                {
                                                    c = ColorFromHSV((h + alt_k * 100) % 360,
                                                        MercifulClamp(s * (1.0 - alt_k * 0.2) - 0.3 * softness),
                                                        MercifulClamp(v * ((kpalettes[p][current_color][0] + kpalettes[p][current_color][1] + kpalettes[p][current_color][2] > 2.5) ? 0.65 : 0.5 - alt_k * 0.2 + 0.1 * softness)));

                                                }
                                            }
                                            //middle part
                                            else
                                            {
                                                //ColorToHSV(c, out h, out s, out v);
                                                c = ColorFromHSV((h + alt_k * 100) % 360,
                                                                    MercifulClamp(s * (1.2 - alt_k * 0.2) - 0.1 * softness),
                                                                    MercifulClamp(v * ((kpalettes[p][current_color][0] + kpalettes[p][current_color][1] + kpalettes[p][current_color][2] > 2.5) ? 1.15 : 1.1 - alt_k * 0.15)));
                                            }
                                        }
                                        break;
                                    default:
                                        {
                                            //outline
                                            if (j == height - 1)
                                            {
                                                //ColorToHSV(c, out h, out s, out v);
                                                if (kpalettes[p][current_color][3] == fade_alpha)
                                                {
                                                    c = ColorFromHSV((h + alt_k * 100) % 360,
                                                                    0.1,
                                                                    MercifulClamp(v * 0.65));
                                                }
                                                else
                                                {
                                                    c = ColorFromHSV((h + alt_k * 100) % 360,
                                                        MercifulClamp(s * (1.05 - alt_k * 0.2) + 0.1 * softness),
                                                        MercifulClamp(v * ((kpalettes[p][current_color][0] + kpalettes[p][current_color][1] + kpalettes[p][current_color][2] > 2.5) ? 0.85 : 0.7 - alt_k * 0.2 + 0.1 * softness)));

                                                }
                                            }
                                            //all same color
                                            else
                                            {
                                                //ColorToHSV(c, out h, out s, out v);
                                                c = ColorFromHSV((h + alt_k * 100) % 360,
                                                                    MercifulClamp(s * (1.3 - alt_k * 0.2) - 0.1 * softness),
                                                                    MercifulClamp(v * ((kpalettes[p][current_color][0] + kpalettes[p][current_color][1] + kpalettes[p][current_color][2] > 2.5) ? 1.1 : 1.0 - alt_k * 0.15)));
                                            }
                                        }
                                        break;
                                }

                                Color c2 = Color.FromArgb(c.ToArgb());
                                if (current_color != 25)
                                {
                                    ColorToHSV(c2, out h2, out s2, out v2);
                                    c2 = ColorFromHSV(h2,
                                        MercifulClamp(s2 * 1.15),
                                        MercifulClamp(v2 * ((kpalettes[p][current_color][0] + kpalettes[p][current_color][1] + kpalettes[p][current_color][2] > 2.5) ? 0.8 : ((kpalettes[p][current_color][3] == fade_alpha) ? 0.875 : 0.65))));
                                }
                                if (c.A != 0)
                                {
                                    cubes[k, p, current_color, i * 4 + j * width * 4 + 0] = Math.Max((byte)1, c.B);
                                    cubes[k, p, current_color, i * 4 + j * width * 4 + 1] = Math.Max((byte)1, c.G);
                                    cubes[k, p, current_color, i * 4 + j * width * 4 + 2] = Math.Max((byte)1, c.R);
                                    cubes[k, p, current_color, i * 4 + j * width * 4 + 3] = c.A;


                                    cubes[k, p, current_color + kcolorcount, i * 4 + j * width * 4 + 0] = Math.Max((byte)1, c2.B);
                                    cubes[k, p, current_color + kcolorcount, i * 4 + j * width * 4 + 1] = Math.Max((byte)1, c2.G);
                                    cubes[k, p, current_color + kcolorcount, i * 4 + j * width * 4 + 2] = Math.Max((byte)1, c2.R);
                                    cubes[k, p, current_color + kcolorcount, i * 4 + j * width * 4 + 3] = c2.A;
                                }
                                else
                                {
                                    cubes[k, p, current_color, i * 4 + j * 4 * width + 0] = 0;
                                    cubes[k, p, current_color, i * 4 + j * 4 * width + 1] = 0;
                                    cubes[k, p, current_color, i * 4 + j * 4 * width + 2] = 0;
                                    cubes[k, p, current_color, i * 4 + j * 4 * width + 3] = 0;


                                    cubes[k, p, current_color + kcolorcount, i * 4 + j * 4 * width + 0] = 0;
                                    cubes[k, p, current_color + kcolorcount, i * 4 + j * 4 * width + 1] = 0;
                                    cubes[k, p, current_color + kcolorcount, i * 4 + j * 4 * width + 2] = 0;
                                    cubes[k, p, current_color + kcolorcount, i * 4 + j * 4 * width + 3] = 0;
                                }
                            }
                        }
                    }
                    DungeonPalettes.kdungeon[k][p] = kpalettes[p].Concat(contrast[p]).ToArray();
                }
            }
            int blen = 4 * vwidth * (vheight + 1);
            byte[][][][] cubes2 = new byte[DungeonPalettes.kdungeon.Length][][][];
            for (int k = 0; k < DungeonPalettes.kdungeon.Length; k++)
            {
                cubes2[k] = new byte[DungeonPalettes.kdungeon[k].Length][][];
                for (int i = 0; i < DungeonPalettes.kdungeon[k].Length; i++)
                {
                    cubes2[k][i] = new byte[DungeonPalettes.kdungeon[k][i].Length][];
                    for (int c = 0; c < DungeonPalettes.kdungeon[k][i].Length; c++)
                    {
                        cubes2[k][i][c] = new byte[blen];
                        for (int j = 0; j < blen; j++)
                        {
                            cubes2[k][i][c][j] = cubes[k, i, c, j];
                        }
                    }
                }
            }
            return cubes2;
        }

        private static byte[][][] storeColorCubesFleshToneK()
        {
            int fleshPaletteCount = DungeonPalettes.fleshTones.Length;
            byte[,,] cubes = new byte[fleshPaletteCount, DungeonPalettes.fleshTones[0].Length * 2, 4 * vwidth * (vheight + 1)];

            float[][][] kpalettes = DungeonPalettes.fleshTones;
            int kcolorcount = kpalettes[0].Length;
            float[][][] contrast = kpalettes.Replicate();
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
            for (int p = 0; p < kpalettes.Length; p++)
            {
                for (int current_color = 0; current_color < kpalettes[p].Length; current_color++)
                {
                    Bitmap b =
                    new Bitmap(width, height, PixelFormat.Format32bppArgb);

                    Graphics g = Graphics.FromImage((Image)b);

                    if (kpalettes[p][current_color][3] == eraser_alpha)
                    {
                        colorMatrix = new ColorMatrix(new float[][]{
    new float[] {0,  0,  0,  0, 0},
    new float[] {0,  0,  0,  0, 0},
    new float[] {0,  0,  0,  0, 0},
    new float[] {0,  0,  0,  1F, 0},
    new float[] {0,  0,  0,  0, 1F}});
                    }
                    else if (kpalettes[p][current_color][3] == 0F)
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
    new float[] {0.22F+kpalettes[p][current_color][0],  0,  0,  0, 0},
    new float[] {0,  0.251F+kpalettes[p][current_color][1],  0,  0, 0},
    new float[] {0,  0,  0.31F+kpalettes[p][current_color][2],  0, 0},
    new float[] {0,  0,  0,  1F, 0},
    new float[] {0, 0, 0, 0, 1F}});
                    }
                    imageAttributes.SetColorMatrix(
                        colorMatrix,
                        ColorMatrixFlag.Default,
                        ColorAdjustType.Bitmap);
                    string which_image = ((current_color >= 14 && current_color <= 22) || kpalettes[p][current_color][3] == 0F
                        || kpalettes[p][current_color][3] == flash_alpha_0 || kpalettes[p][current_color][3] == flash_alpha_1) ? "shine" :
                        (kpalettes[p][current_color][3] == flat_alpha || kpalettes[p][current_color][3] == bordered_flat_alpha) ? "flat" : "cube";
                    g.DrawImage(image,
                        new Rectangle(0, 0,
                            width, height),  // destination rectangle 
                                             //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                        0, 0,        // upper-left corner of source rectangle 
                        width,       // width of source rectangle
                        height,      // height of source rectangle
                        GraphicsUnit.Pixel,
                        imageAttributes);

                    int alt_k = 0;
                    double softness = 0.0;

                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            Color c = b.GetPixel(i, j);
                            double h = 0.0, s = 1.0, v = 1.0;
                            double h2 = 0.0, s2 = 1.0, v2 = 1.0;
                            switch (which_image)
                            {
                                case "cube":
                                    {

                                        //top
                                        if (j <= 1)
                                        {
                                            ColorToHSV(c, out h, out s, out v);
                                            c = ColorFromHSV((h + alt_k * 100) % 360,
                                                                MercifulClamp(s * (1.2 - alt_k * 0.2) - 0.1 * softness),
                                                                MercifulClamp(v * ((kpalettes[p][current_color][0] + kpalettes[p][current_color][1] + kpalettes[p][current_color][2] > 2.5) ? 1.15 : 1.1 - alt_k * 0.15)));
                                        }
                                        //outline
                                        else if (j == height - 1)
                                        {
                                            ColorToHSV(c, out h, out s, out v);
                                            if (kpalettes[p][current_color][3] == fade_alpha)
                                            {
                                                c = ColorFromHSV((h + alt_k * 100) % 360,
                                                                0.1,
                                                                MercifulClamp(v * 0.65));
                                            }
                                            else
                                            {
                                                c = ColorFromHSV((h + alt_k * 100) % 360,
                                                    MercifulClamp(s * (1.0 - alt_k * 0.2) - 0.3 * softness),
                                                    MercifulClamp(v * ((kpalettes[p][current_color][0] + kpalettes[p][current_color][1] + kpalettes[p][current_color][2] > 2.5) ? 0.65 : 0.5 - alt_k * 0.2 + 0.1 * softness)));
                                            }
                                        }
                                        //lightly shaded side
                                        else
                                        {
                                            ColorToHSV(c, out h, out s, out v);
                                            c = ColorFromHSV((h + alt_k * 100) % 360,
                                                    MercifulClamp(s * (1.3 - alt_k * 0.2) - 0.2 * softness),
                                                    MercifulClamp(v * ((kpalettes[p][current_color][0] + kpalettes[p][current_color][1] + kpalettes[p][current_color][2] > 2.5) ? 1.0 : 0.9 - alt_k * 0.2 + 0.05 * softness)));
                                        }
                                    }
                                    break;
                                case "flat":
                                    {
                                        //above the region that should have color
                                        if (j < vheight / 2)
                                        {
                                            c = Color.Transparent;
                                        }
                                        //outline
                                        else if (j == height - 1)
                                        {

                                            if (kpalettes[p][current_color][3] == fade_alpha)
                                            {
                                                c = ColorFromHSV((h + alt_k * 100) % 360,
                                                                0.1,
                                                                MercifulClamp(v * 0.65));
                                            }
                                            else
                                            {
                                                c = ColorFromHSV((h + alt_k * 100) % 360,
                                                    MercifulClamp(s * (1.0 - alt_k * 0.2) - 0.3 * softness),
                                                    MercifulClamp(v * ((kpalettes[p][current_color][0] + kpalettes[p][current_color][1] + kpalettes[p][current_color][2] > 2.5) ? 0.65 : 0.5 - alt_k * 0.2 + 0.1 * softness)));

                                            }
                                        }
                                        //middle part
                                        else
                                        {
                                            ColorToHSV(c, out h, out s, out v);
                                            c = ColorFromHSV((h + alt_k * 100) % 360,
                                                                MercifulClamp(s * (1.2 - alt_k * 0.2) - 0.1 * softness),
                                                                MercifulClamp(v * ((kpalettes[p][current_color][0] + kpalettes[p][current_color][1] + kpalettes[p][current_color][2] > 2.5) ? 1.15 : 1.1 - alt_k * 0.15)));
                                        }
                                    }
                                    break;
                                //includes shine and anything else, all pretty much a solid block
                                default:
                                    {
                                        //outline
                                        if (j == height - 1)
                                        {

                                            if (kpalettes[p][current_color][3] == fade_alpha)
                                            {
                                                c = ColorFromHSV((h + alt_k * 100) % 360,
                                                                0.1,
                                                                MercifulClamp(v * 0.65));
                                            }
                                            else
                                            {
                                                c = ColorFromHSV((h + alt_k * 100) % 360,
                                                    MercifulClamp(s * (1.0 - alt_k * 0.2) - 0.3 * softness),
                                                    MercifulClamp(v * ((kpalettes[p][current_color][0] + kpalettes[p][current_color][1] + kpalettes[p][current_color][2] > 2.5) ? 0.9 : 0.75 - alt_k * 0.2 + 0.1 * softness)));

                                            }
                                        }
                                        //all same color
                                        else
                                        {
                                            ColorToHSV(c, out h, out s, out v);
                                            c = ColorFromHSV((h + alt_k * 100) % 360,
                                                                MercifulClamp(s * (1.3 - alt_k * 0.2) - 0.1 * softness),
                                                                MercifulClamp(v * ((kpalettes[p][current_color][0] + kpalettes[p][current_color][1] + kpalettes[p][current_color][2] > 2.5) ? 1.1 : 1.0 - alt_k * 0.15)));
                                        }
                                    }
                                    break;
                            }

                            Color c2 = Color.FromArgb(c.ToArgb());
                            ColorToHSV(c2, out h2, out s2, out v2);
                            c2 = ColorFromHSV(h2, Math.Min(1.0, s2 * 1.2), Math.Max(0.01, v2 * ((kpalettes[p][current_color][0] + kpalettes[p][current_color][1] + kpalettes[p][current_color][2] > 2.5) ? 1.0 : 0.75)));

                            if (c.A != 0)
                            {
                                cubes[p, current_color, i * 4 + j * width * 4 + 0] = Math.Max((byte)1, c.B);
                                cubes[p, current_color, i * 4 + j * width * 4 + 1] = Math.Max((byte)1, c.G);
                                cubes[p, current_color, i * 4 + j * width * 4 + 2] = Math.Max((byte)1, c.R);
                                cubes[p, current_color, i * 4 + j * width * 4 + 3] = c.A;


                                cubes[p, current_color + kcolorcount, i * 4 + j * width * 4 + 0] = Math.Max((byte)1, c2.B);
                                cubes[p, current_color + kcolorcount, i * 4 + j * width * 4 + 1] = Math.Max((byte)1, c2.G);
                                cubes[p, current_color + kcolorcount, i * 4 + j * width * 4 + 2] = Math.Max((byte)1, c2.R);
                                cubes[p, current_color + kcolorcount, i * 4 + j * width * 4 + 3] = c2.A;
                            }
                            else
                            {
                                cubes[p, current_color, i * 4 + j * 4 * width + 0] = 0;
                                cubes[p, current_color, i * 4 + j * 4 * width + 1] = 0;
                                cubes[p, current_color, i * 4 + j * 4 * width + 2] = 0;
                                cubes[p, current_color, i * 4 + j * 4 * width + 3] = 0;


                                cubes[p, current_color + kcolorcount, i * 4 + j * 4 * width + 0] = 0;
                                cubes[p, current_color + kcolorcount, i * 4 + j * 4 * width + 1] = 0;
                                cubes[p, current_color + kcolorcount, i * 4 + j * 4 * width + 2] = 0;
                                cubes[p, current_color + kcolorcount, i * 4 + j * 4 * width + 3] = 0;
                            }
                        }
                    }
                }
                DungeonPalettes.fleshTones[p] = kpalettes[p].Concat(contrast[p]).ToArray();
            }
            int blen = 4 * vwidth * (vheight + 1);
            byte[][][] cubes2 = new byte[DungeonPalettes.fleshTones.Length][][];
            for (int k = 0; k < DungeonPalettes.fleshTones.Length; k++)
            {
                cubes2[k] = new byte[DungeonPalettes.fleshTones[k].Length][];
                for (int c = 0; c < DungeonPalettes.fleshTones[k].Length; c++)
                {
                    cubes2[k][c] = new byte[blen];
                    for (int j = 0; j < blen; j++)
                    {
                        cubes2[k][c][j] = cubes[k, c, j];
                    }
                }
            }
            return cubes2;
        }

        public static void setupCurrentColorsK(int faction, int palette)
        {
            kcolors = DungeonPalettes.kdungeon[faction][palette];
            kcurrent = krendered[faction][palette];
        }
        public static void setupCurrentColorsK(int faction, int palette, int body)
        {
            kcolors = DungeonPalettes.kdungeon[faction][palette];
            kcurrent = krendered[faction][palette];
            for (int ft = 0; ft < 5; ft++)
            {
                kcolors[3 + ft] = DungeonPalettes.fleshTones[body][ft];
                kcurrent[3 + ft] = kFleshRendered[body][ft];
                kcolors[kcolorcount + 3 + ft] = DungeonPalettes.fleshTones[body][ft + 5];
                kcurrent[kcolorcount + 3 + ft] = kFleshRendered[body][ft + 5];
            }
        }


        private static int voxelToPixelK16(int innerX, int innerY, int x, int y, int z, int faction, int palette, int current_color, int stride, int jitter, bool still)
        {
            return 4 * ((y) * vwidth + 4 + ((DungeonPalettes.kdungeon[faction][palette][current_color][3] == waver_alpha) ? jitter - 2 : 0))
                + innerX +
                stride * (16 * (vheight - 2) + 4 + x * 2 - z * (vheight - 2) - ((DungeonPalettes.kdungeon[faction][palette][current_color][3] == flat_alpha)
                ? -2 : (still ^ (DungeonPalettes.kdungeon[faction][palette][current_color][3] == VoxelLogic.yver_alpha)) ? 0 : jitter) + innerY);
        }
        private static int voxelToPixelK32(int innerX, int innerY, int x, int y, int z, int faction, int palette, int current_color, int stride, int jitter, bool still)
        {
            return 4 * ((y) * vwidth + 4 + ((DungeonPalettes.kdungeon[faction][palette][current_color][3] == VoxelLogic.waver_alpha) ? jitter - 2 : 0))
                + innerX +
                stride * (32 * (vheight - 2) + 4 + x * 2 - z * (vheight - 2) - ((DungeonPalettes.kdungeon[faction][palette][current_color][3] == VoxelLogic.flat_alpha)
                ? -2 : (still ^ (DungeonPalettes.kdungeon[faction][palette][current_color][3] == VoxelLogic.yver_alpha)) ? 0 : jitter) + innerY);
        }


        private static Bitmap renderK(MagicaVoxelData[] voxels, int facing, int faction, int palette, int frame, int maxFrames, bool still, bool darkOutline)
        {
            Bitmap bmp = new Bitmap(vwidth * 16 + 8, vheight * 16 + 8, PixelFormat.Format32bppArgb);

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
                int current_color = ((255 - vx.color) % 4 == 0) ? (255 - vx.color) / 4 + kcolorcount : ((254 - vx.color) % 4 == 0) ? (253 - clear) / 4 : (253 - vx.color) / 4;
                bool is_shaded = (unshaded != current_color);
                int p = 0;
                if ((255 - vx.color) % 4 != 0 && (253 - vx.color) % 4 != 0)
                    continue;
                if ((255 - vx.color) % 4 != 0 && current_color >= kcolorcount)
                    continue;

                if (current_color >= 19 && current_color <= 22)
                    current_color = 19 + ((current_color + frame) % 4);
                if (current_color >= 19 + kcolorcount && current_color <= 22 + kcolorcount)
                    current_color = 19 + kcolorcount + ((current_color + frame) % 4);

                if (current_color >= 38 && current_color <= 41)
                    current_color = 38 + ((current_color - 38 + frame) % 4);
                if (current_color >= 38 + kcolorcount && current_color <= 41 + kcolorcount)
                    current_color = 38 + kcolorcount + ((current_color - 38 - kcolorcount + frame) % 4);



                if ((frame % 2 != 0) && (DungeonPalettes.kdungeon[faction][palette][current_color][3] == spin_alpha_0 || DungeonPalettes.kdungeon[faction][palette][current_color][3] == flash_alpha_0))
                    continue;
                else if ((frame % 2 != 1) && (DungeonPalettes.kdungeon[faction][palette][current_color][3] == spin_alpha_1 || DungeonPalettes.kdungeon[faction][palette][current_color][3] == flash_alpha_1))
                    continue;
                else if (DungeonPalettes.kdungeon[faction][palette][current_color][3] == 0F)
                    continue;
                else if (unshaded >= 13 && unshaded <= 16)
                {
                    int mod_color = current_color;
                    if (unshaded == 13 && r.Next(7) < 2) //smoke
                        continue;
                    if (unshaded == 14) //yellow fire
                    {
                        if (r.Next(3) > 0)
                        {
                            mod_color += r.Next(3);
                        }
                    }
                    else if (unshaded == 15) // orange fire
                    {
                        if (r.Next(5) < 4)
                        {
                            mod_color -= r.Next(3);
                        }
                    }
                    else if (unshaded == 16) // sparks
                    {
                        if (r.Next(5) > 0)
                        {
                            mod_color -= r.Next(3);
                        }
                    }

                    for (int j = 0; j < vheight; j++)
                    {
                        for (int i = 0; i < 4 * vwidth; i++)
                        {
                            p = voxelToPixelK16(i, j, vx.x, vx.y, vx.z, faction, palette, mod_color, bmpData.Stride, jitter, still);
                            if (argbValues[p] == 0) //  && argbValues[(p / 4) + 3] != 7 // check for erased pixels
                            {
                                if (DungeonPalettes.kdungeon[faction][palette][current_color][3] == bordered_alpha || DungeonPalettes.kdungeon[faction][palette][current_color][3] == bordered_flat_alpha)
                                {
                                    zbuffer[p] = vx.z;
                                    xbuffer[p] = vx.x;
                                }
                                argbValues[p] = kcurrent[mod_color][i + j * (vwidth * 4)];
                                barePositions[p] = !(DungeonPalettes.kdungeon[faction][palette][current_color][3] == bordered_alpha || DungeonPalettes.kdungeon[faction][palette][current_color][3] == bordered_flat_alpha);
                                if (!barePositions[p] && outlineColors[p] == 0)
                                    outlineColors[p] = kcurrent[mod_color][i + (vheight * vwidth * 4)];

                            }
                        }
                    }
                }
                else if (unshaded == 23)
                {
                    for (int j = 0; j < vheight; j++)
                    {
                        for (int i = 0; i < 4 * vwidth; i++)
                        {
                            p = voxelToPixelK16(i, j, vx.x, vx.y, vx.z, faction, palette, current_color, bmpData.Stride, jitter, still);

                            if (shadowValues[p] == 0)
                            {
                                shadowValues[p] = kcurrent[current_color][i + j * (vwidth * 4)];
                            }
                        }
                    }
                }
                else
                {
                    int mod_color = current_color;
                    if (mod_color == 25 && Simplex.FindNoiseWater(frame % 4, facing, vx.x + 72, vx.y + 72, vx.z) < 0.5 - (Math.Pow(Math.Max(Math.Abs(8 - vx.x), Math.Abs(8 - vx.y)), 3.0) / 48.0)) //water top, intentionally ignoring "shaded"
                        continue;
                    //                    if (mod_color == 25 && r.Next(7) < 2) //water top, intentionally ignoring "shaded"
                    //                        continue;
                    if ((unshaded >= 16 && unshaded <= 18) && r.Next(11) < 8) //rare sparks
                        continue;

                    for (int j = 0; j < vheight; j++)
                    {
                        for (int i = 0; i < 4 * vwidth; i++)
                        {
                            p = voxelToPixelK16(i, j, vx.x, vx.y, vx.z, faction, palette, mod_color, bmpData.Stride, jitter, still);

                            if (argbValues[p] == 0) //  && argbValues[(p / 4) + 3] != 7 // eraser stuff
                            {
                                zbuffer[p] = vx.z;
                                xbuffer[p] = vx.x;
                                mod_color = current_color;
                                if (current_color == 7 && vheight > 2)
                                {
                                    if (j <= 2 && i < 4)
                                    {
                                        mod_color = 6;
                                    }
                                    else
                                    {
                                        mod_color = 7;
                                    }
                                }
                                if (kcolors[mod_color][3] == gloss_alpha && i % 4 == 3 && r.Next(12) == 0)
                                {
                                    argbValues[p - 3] = (byte)Math.Min(kcurrent[mod_color][i - 3 + j * (vwidth * 4)] + 160, 255);
                                    argbValues[p - 2] = (byte)Math.Min(kcurrent[mod_color][i - 2 + j * (vwidth * 4)] + 160, 255);
                                    argbValues[p - 1] = (byte)Math.Min(kcurrent[mod_color][i - 1 + j * (vwidth * 4)] + 160, 255);
                                    argbValues[p - 0] = 255;
                                }
                                else if (kcolors[mod_color][3] == grain_hard_alpha && i % 4 == 3)
                                {
                                    float n = Simplex.FindNoiseBold(facing, vx.x + 72, vx.y + 72, vx.z);
                                    argbValues[p - 3] = (byte)Clamp(kcurrent[mod_color][i - 3 + j * (vwidth * 4)] * n, 1, 255);
                                    argbValues[p - 2] = (byte)Clamp(kcurrent[mod_color][i - 2 + j * (vwidth * 4)] * n, 1, 255);
                                    argbValues[p - 1] = (byte)Clamp(kcurrent[mod_color][i - 1 + j * (vwidth * 4)] * n, 1, 255);
                                    argbValues[p - 0] = 255;
                                }
                                else if (kcolors[mod_color][3] == grain_some_alpha && i % 4 == 3)
                                {
                                    float n = Simplex.FindNoise(facing, vx.x + 72, vx.y + 72, vx.z);
                                    argbValues[p - 3] = (byte)Clamp(kcurrent[mod_color][i - 3 + j * (vwidth * 4)] * n, 1, 255);
                                    argbValues[p - 2] = (byte)Clamp(kcurrent[mod_color][i - 2 + j * (vwidth * 4)] * n, 1, 255);
                                    argbValues[p - 1] = (byte)Clamp(kcurrent[mod_color][i - 1 + j * (vwidth * 4)] * n, 1, 255);
                                    argbValues[p - 0] = 255;
                                }
                                else if (kcolors[mod_color][3] == grain_mild_alpha && i % 4 == 3)
                                {
                                    float n = Simplex.FindNoiseLight(facing, vx.x + 72, vx.y + 72, vx.z);
                                    argbValues[p - 3] = (byte)Clamp(kcurrent[mod_color][i - 3 + j * (vwidth * 4)] * n, 1, 255);
                                    argbValues[p - 2] = (byte)Clamp(kcurrent[mod_color][i - 2 + j * (vwidth * 4)] * n, 1, 255);
                                    argbValues[p - 1] = (byte)Clamp(kcurrent[mod_color][i - 1 + j * (vwidth * 4)] * n, 1, 255);
                                    argbValues[p - 0] = 255;
                                }
                                else if (kcolors[mod_color][3] == fuzz_alpha && i % 4 == 3)
                                {
                                    float n = Simplex.FindNoiseTight(frame % 4, facing, vx.x + 72, vx.y + 72, vx.z);
                                    argbValues[p - 3] = (byte)Clamp(kcurrent[mod_color][i - 3 + j * (vwidth * 4)] * n, 1, 255);
                                    argbValues[p - 2] = (byte)Clamp(kcurrent[mod_color][i - 2 + j * (vwidth * 4)] * n, 1, 255);
                                    argbValues[p - 1] = (byte)Clamp(kcurrent[mod_color][i - 1 + j * (vwidth * 4)] * n, 1, 255);
                                    argbValues[p - 0] = 255;
                                }
                                else if (unshaded == 25 && i % 4 == 3)
                                {
                                    double wave = Simplex.FindNoiseWater(frame % 4, facing, vx.x + 72, vx.y + 72, vx.z);

                                    if (mod_color == 25)
                                    {
                                        if (wave > 0.73)
                                        {
                                            wave = 75 * wave;
                                        }
                                        else if (wave > 0.65)
                                        {
                                            wave = 55 * wave;
                                        }
                                        else if (wave > 0.6)
                                        {
                                            wave = 40 * wave;
                                        }
                                        else
                                        {
                                            wave = 8;
                                        }
                                    }
                                    else //solid body of water using shaded color
                                    {
                                        wave += 0.2;
                                        if (wave < 0.5)
                                        {
                                            wave = -12 / wave;
                                        }
                                        else if (wave < 0.55)
                                        {
                                            wave = -9 / wave;
                                        }
                                        else if (wave < 0.6)
                                        {
                                            wave = -6 / wave;
                                        }
                                        else
                                        {
                                            wave = 0;
                                        }
                                    }

                                    argbValues[p - 3] = (byte)Clamp(kcurrent[mod_color][i - 3 + j * (vwidth * 4)] + wave, 1, 255);
                                    argbValues[p - 2] = (byte)Clamp(kcurrent[mod_color][i - 2 + j * (vwidth * 4)] + wave, 1, 255);
                                    argbValues[p - 1] = (byte)Clamp(kcurrent[mod_color][i - 1 + j * (vwidth * 4)] + wave, 1, 255);
                                    argbValues[p - 0] = 255;
                                }

                                else
                                {
                                    argbValues[p] = kcurrent[mod_color][i + j * (vwidth * 4)];
                                }
                                barePositions[p] = (DungeonPalettes.kdungeon[faction][palette][mod_color][3] == flash_alpha_0 ||
                                    DungeonPalettes.kdungeon[faction][palette][mod_color][3] == flash_alpha_1 ||
                                    DungeonPalettes.kdungeon[faction][palette][mod_color][3] == borderless_alpha);
                                if (!barePositions[p] && outlineColors[p] == 0)
                                    outlineColors[p] = kcurrent[mod_color][i + (4 * vwidth * vheight)]; //(argbValues[p] * 1.2 + 2 < 255) ? (byte)(argbValues[p] * 1.2 + 2) : (byte)255;

                            }
                        }
                    }
                }
            }
            for (int i = 3; i < numBytes; i += 4)
            {
                if (argbValues[i] > 255 * waver_alpha && barePositions[i] == false)
                {
                    if (i + 4 >= 0 && i + 4 < argbValues.Length && argbValues[i + 4] == 0 && darkOutline && outlineValues[i + 4] == 0) { outlineValues[i + 4] = 255; } else if (i + 4 >= 0 && i + 4 < argbValues.Length && barePositions[i + 4] == false && (zbuffer[i] - 1 > zbuffer[i + 4] || xbuffer[i] - 1 > xbuffer[i + 4]) && outlineValues[i + 4] == 0) { outlineValues[i + 4] = 255; outlineValues[i + 4 - 1] = outlineColors[i - 1]; outlineValues[i + 4 - 2] = outlineColors[i - 2]; outlineValues[i + 4 - 3] = outlineColors[i - 3]; }
                    if (i - 4 >= 0 && i - 4 < argbValues.Length && argbValues[i - 4] == 0 && darkOutline && outlineValues[i - 4] == 0) { outlineValues[i - 4] = 255; } else if (i - 4 >= 0 && i - 4 < argbValues.Length && barePositions[i - 4] == false && (zbuffer[i] - 1 > zbuffer[i - 4] || xbuffer[i] - 1 > xbuffer[i - 4]) && outlineValues[i - 4] == 0) { outlineValues[i - 4] = 255; outlineValues[i - 4 - 1] = outlineColors[i - 1]; outlineValues[i - 4 - 2] = outlineColors[i - 2]; outlineValues[i - 4 - 3] = outlineColors[i - 3]; }
                    if (i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && argbValues[i + bmpData.Stride] == 0 && darkOutline && outlineValues[i + bmpData.Stride] == 0) { outlineValues[i + bmpData.Stride] = 255; } else if (i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && barePositions[i + bmpData.Stride] == false && (zbuffer[i] - 1 > zbuffer[i + bmpData.Stride] || xbuffer[i] - 1 > xbuffer[i + bmpData.Stride]) && outlineValues[i + bmpData.Stride] == 0) { outlineValues[i + bmpData.Stride] = 255; outlineValues[i + bmpData.Stride - 1] = outlineColors[i - 1]; outlineValues[i + bmpData.Stride - 2] = outlineColors[i - 2]; outlineValues[i + bmpData.Stride - 3] = outlineColors[i - 3]; }
                    if (i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && argbValues[i - bmpData.Stride] == 0 && darkOutline && outlineValues[i - bmpData.Stride] == 0) { outlineValues[i - bmpData.Stride] = 255; } else if (i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && barePositions[i - bmpData.Stride] == false && (zbuffer[i] - 1 > zbuffer[i - bmpData.Stride] || xbuffer[i] - 1 > xbuffer[i - bmpData.Stride]) && outlineValues[i - bmpData.Stride] == 0) { outlineValues[i - bmpData.Stride] = 255; outlineValues[i - bmpData.Stride - 1] = outlineColors[i - 1]; outlineValues[i - bmpData.Stride - 2] = outlineColors[i - 2]; outlineValues[i - bmpData.Stride - 3] = outlineColors[i - 3]; }
                    /*
                    if (i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && argbValues[i + bmpData.Stride + 4] == 0 && darkOutline && outlineValues[i + bmpData.Stride + 4] == 0) { outlineValues[i + bmpData.Stride + 4] = 255; } else if (i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && barePositions[i + bmpData.Stride + 4] == false && (zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 4] || xbuffer[i] - 1 > xbuffer[i + bmpData.Stride + 4]) && outlineValues[i + bmpData.Stride + 4] == 0) { outlineValues[i + bmpData.Stride + 4] = 255; outlineValues[i + bmpData.Stride + 4 - 1] = outlineColors[i - 1]; outlineValues[i + bmpData.Stride + 4 - 2] = outlineColors[i - 2]; outlineValues[i + bmpData.Stride + 4 - 3] = outlineColors[i - 3]; }
                    if (i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && argbValues[i - bmpData.Stride - 4] == 0 && darkOutline && outlineValues[i - bmpData.Stride - 4] == 0) { outlineValues[i - bmpData.Stride - 4] = 255; } else if (i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && barePositions[i - bmpData.Stride - 4] == false && (zbuffer[i] - 1 > zbuffer[i - bmpData.Stride - 4] || xbuffer[i] - 2 > xbuffer[i - bmpData.Stride - 4]) && outlineValues[i - bmpData.Stride - 4] == 0) { outlineValues[i - bmpData.Stride - 4] = 255; outlineValues[i - bmpData.Stride - 4 - 1] = outlineColors[i - 1]; outlineValues[i - bmpData.Stride - 4 - 2] = outlineColors[i - 2]; outlineValues[i - bmpData.Stride - 4 - 3] = outlineColors[i - 3]; }
                    if (i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && argbValues[i + bmpData.Stride - 4] == 0 && darkOutline && outlineValues[i + bmpData.Stride - 4] == 0) { outlineValues[i + bmpData.Stride - 4] = 255; } else if (i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && barePositions[i + bmpData.Stride - 4] == false && (zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 4] || xbuffer[i] - 1 > xbuffer[i + bmpData.Stride - 4]) && outlineValues[i + bmpData.Stride - 4] == 0) { outlineValues[i + bmpData.Stride - 4] = 255; outlineValues[i + bmpData.Stride - 4 - 1] = outlineColors[i - 1]; outlineValues[i + bmpData.Stride - 4 - 2] = outlineColors[i - 2]; outlineValues[i + bmpData.Stride - 4 - 3] = outlineColors[i - 3]; }
                    if (i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && argbValues[i - bmpData.Stride + 4] == 0 && darkOutline && outlineValues[i - bmpData.Stride + 4] == 0) { outlineValues[i - bmpData.Stride + 4] = 255; } else if (i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && barePositions[i - bmpData.Stride + 4] == false && (zbuffer[i] - 1 > zbuffer[i - bmpData.Stride + 4] || xbuffer[i] - 2 > xbuffer[i - bmpData.Stride + 4]) && outlineValues[i - bmpData.Stride + 4] == 0) { outlineValues[i - bmpData.Stride + 4] = 255; outlineValues[i - bmpData.Stride + 4 - 1] = outlineColors[i - 1]; outlineValues[i - bmpData.Stride + 4 - 2] = outlineColors[i - 2]; outlineValues[i - bmpData.Stride + 4 - 3] = outlineColors[i - 3]; }
                    */
                    /*
                                        if (argbValues[i] > 0 && i + 4 >= 0 && i + 4 < argbValues.Length && argbValues[i + 4] == 0 && darkOutline) { outlineValues[i + 4] = 255; } else if (i + 4 >= 0 && i + 4 < argbValues.Length && barePositions[i + 4] == false && (zbuffer[i] - 2 > zbuffer[i + 4] || zbuffer[i] + 2 < zbuffer[i + 4])) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = outlineValues[i - 1]; argbValues[i + 4 - 2] = outlineValues[i - 2]; argbValues[i + 4 - 3] = outlineValues[i - 3]; }
                                        if (argbValues[i] > 0 && i - 4 >= 0 && i - 4 < argbValues.Length && argbValues[i - 4] == 0 && darkOutline) { outlineValues[i - 4] = 255; } else if (i - 4 >= 0 && i - 4 < argbValues.Length && barePositions[i - 4] == false && (zbuffer[i] - 2 > zbuffer[i - 4] || zbuffer[i] + 2 < zbuffer[i - 4])) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = outlineValues[i - 1]; argbValues[i - 4 - 2] = outlineValues[i - 2]; argbValues[i - 4 - 3] = outlineValues[i - 3]; }
                                        if (argbValues[i] > 0 && i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && argbValues[i + bmpData.Stride] == 0 && darkOutline) { outlineValues[i + bmpData.Stride] = 255; } else if (i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && barePositions[i + bmpData.Stride] == false && (zbuffer[i] - 2 > zbuffer[i + bmpData.Stride] || zbuffer[i] + 2 < zbuffer[i + bmpData.Stride])) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; }
                                        if (argbValues[i] > 0 && i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && argbValues[i - bmpData.Stride] == 0 && darkOutline) { outlineValues[i - bmpData.Stride] = 255; } else if (i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && barePositions[i - bmpData.Stride] == false && (zbuffer[i] - 2 > zbuffer[i - bmpData.Stride] || zbuffer[i] + 2 < zbuffer[i - bmpData.Stride])) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; }
                                        if (argbValues[i] > 0 && i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && argbValues[i + bmpData.Stride + 4] == 0 && darkOutline) { outlineValues[i + bmpData.Stride + 4] = 255; } else if (i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && barePositions[i + bmpData.Stride + 4] == false && (zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 4] || zbuffer[i] + 2 < zbuffer[i + bmpData.Stride + 4])) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                        if (argbValues[i] > 0 && i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && argbValues[i - bmpData.Stride - 4] == 0 && darkOutline) { outlineValues[i - bmpData.Stride - 4] = 255; } else if (i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && barePositions[i - bmpData.Stride - 4] == false && (zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 4] || zbuffer[i] + 2 < zbuffer[i - bmpData.Stride - 4])) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                        if (argbValues[i] > 0 && i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && argbValues[i + bmpData.Stride - 4] == 0 && darkOutline) { outlineValues[i + bmpData.Stride - 4] = 255; } else if (i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && barePositions[i + bmpData.Stride - 4] == false && (zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 4] || zbuffer[i] + 2 < zbuffer[i + bmpData.Stride - 4])) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                        if (argbValues[i] > 0 && i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && argbValues[i - bmpData.Stride + 4] == 0 && darkOutline) { outlineValues[i - bmpData.Stride + 4] = 255; } else if (i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && barePositions[i - bmpData.Stride + 4] == false && (zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 4] || zbuffer[i] + 2 < zbuffer[i - bmpData.Stride + 4])) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                    */
                    /*
                                        if (argbValues[i] > 0 && i + 8 >= 0 && i + 8 < argbValues.Length && argbValues[i + 8] == 0 && darkOutline) { outlineValues[i + 8] = 255; } else if (i + 8 >= 0 && i + 8 < argbValues.Length && barePositions[i + 8] == false && zbuffer[i] - 2 > zbuffer[i + 8]) { argbValues[i + 8] = 255; argbValues[i + 8 - 1] = outlineValues[i - 1]; argbValues[i + 8 - 2] = outlineValues[i - 2]; argbValues[i + 8 - 3] = outlineValues[i - 3]; }
                                        if (argbValues[i] > 0 && i - 8 >= 0 && i - 8 < argbValues.Length && argbValues[i - 8] == 0 && darkOutline) { outlineValues[i - 8] = 255; } else if (i - 8 >= 0 && i - 8 < argbValues.Length && barePositions[i - 8] == false && zbuffer[i] - 2 > zbuffer[i - 8]) { argbValues[i - 8] = 255; argbValues[i - 8 - 1] = outlineValues[i - 1]; argbValues[i - 8 - 2] = outlineValues[i - 2]; argbValues[i - 8 - 3] = outlineValues[i - 3]; }
                                        if (argbValues[i] > 0 && i + bmpData.Stride * 2 >= 0 && i + bmpData.Stride * 2 < argbValues.Length && argbValues[i + bmpData.Stride * 2] == 0 && darkOutline) { outlineValues[i + bmpData.Stride * 2] = 255; } else if (i + bmpData.Stride * 2 >= 0 && i + bmpData.Stride * 2 < argbValues.Length && barePositions[i + bmpData.Stride * 2] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2]) { argbValues[i + bmpData.Stride * 2] = 255; argbValues[i + bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                        if (argbValues[i] > 0 && i - bmpData.Stride * 2 >= 0 && i - bmpData.Stride * 2 < argbValues.Length && argbValues[i - bmpData.Stride * 2] == 0 && darkOutline) { outlineValues[i - bmpData.Stride * 2] = 255; } else if (i - bmpData.Stride * 2 >= 0 && i - bmpData.Stride * 2 < argbValues.Length && barePositions[i - bmpData.Stride * 2] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2]) { argbValues[i - bmpData.Stride * 2] = 255; argbValues[i - bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                        if (argbValues[i] > 0 && i + bmpData.Stride + 8 >= 0 && i + bmpData.Stride + 8 < argbValues.Length && argbValues[i + bmpData.Stride + 8] == 0 && darkOutline) { outlineValues[i + bmpData.Stride + 8] = 255; } else if (i + bmpData.Stride + 8 >= 0 && i + bmpData.Stride + 8 < argbValues.Length && barePositions[i + bmpData.Stride + 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 8]) { argbValues[i + bmpData.Stride + 8] = 255; argbValues[i + bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                        if (argbValues[i] > 0 && i - bmpData.Stride + 8 >= 0 && i - bmpData.Stride + 8 < argbValues.Length && argbValues[i - bmpData.Stride + 8] == 0 && darkOutline) { outlineValues[i - bmpData.Stride + 8] = 255; } else if (i - bmpData.Stride + 8 >= 0 && i - bmpData.Stride + 8 < argbValues.Length && barePositions[i - bmpData.Stride + 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 8]) { argbValues[i - bmpData.Stride + 8] = 255; argbValues[i - bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                        if (argbValues[i] > 0 && i + bmpData.Stride - 8 >= 0 && i + bmpData.Stride - 8 < argbValues.Length && argbValues[i + bmpData.Stride - 8] == 0 && darkOutline) { outlineValues[i + bmpData.Stride - 8] = 255; } else if (i + bmpData.Stride - 8 >= 0 && i + bmpData.Stride - 8 < argbValues.Length && barePositions[i + bmpData.Stride - 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 8]) { argbValues[i + bmpData.Stride - 8] = 255; argbValues[i + bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                        if (argbValues[i] > 0 && i - bmpData.Stride - 8 >= 0 && i - bmpData.Stride - 8 < argbValues.Length && argbValues[i - bmpData.Stride - 8] == 0 && darkOutline) { outlineValues[i - bmpData.Stride - 8] = 255; } else if (i - bmpData.Stride - 8 >= 0 && i - bmpData.Stride - 8 < argbValues.Length && barePositions[i - bmpData.Stride - 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 8]) { argbValues[i - bmpData.Stride - 8] = 255; argbValues[i - bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                        if (argbValues[i] > 0 && i + bmpData.Stride * 2 + 8 >= 0 && i + bmpData.Stride * 2 + 8 < argbValues.Length && argbValues[i + bmpData.Stride * 2 + 8] == 0 && darkOutline) { outlineValues[i + bmpData.Stride * 2 + 8] = 255; } else if (i + bmpData.Stride * 2 + 8 >= 0 && i + bmpData.Stride * 2 + 8 < argbValues.Length && barePositions[i + bmpData.Stride * 2 + 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 + 8]) { argbValues[i + bmpData.Stride * 2 + 8] = 255; argbValues[i + bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                        if (argbValues[i] > 0 && i + bmpData.Stride * 2 + 4 >= 0 && i + bmpData.Stride * 2 + 4 < argbValues.Length && argbValues[i + bmpData.Stride * 2 + 4] == 0 && darkOutline) { outlineValues[i + bmpData.Stride * 2 + 4] = 255; } else if (i + bmpData.Stride * 2 + 4 >= 0 && i + bmpData.Stride * 2 + 4 < argbValues.Length && barePositions[i + bmpData.Stride * 2 + 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 + 4]) { argbValues[i + bmpData.Stride * 2 + 4] = 255; argbValues[i + bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                        if (argbValues[i] > 0 && i + bmpData.Stride * 2 - 4 >= 0 && i + bmpData.Stride * 2 - 4 < argbValues.Length && argbValues[i + bmpData.Stride * 2 - 4] == 0 && darkOutline) { outlineValues[i + bmpData.Stride * 2 - 4] = 255; } else if (i + bmpData.Stride * 2 - 4 >= 0 && i + bmpData.Stride * 2 - 4 < argbValues.Length && barePositions[i + bmpData.Stride * 2 - 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 - 4]) { argbValues[i + bmpData.Stride * 2 - 4] = 255; argbValues[i + bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                        if (argbValues[i] > 0 && i + bmpData.Stride * 2 - 8 >= 0 && i + bmpData.Stride * 2 - 8 < argbValues.Length && argbValues[i + bmpData.Stride * 2 - 8] == 0 && darkOutline) { outlineValues[i + bmpData.Stride * 2 - 8] = 255; } else if (i + bmpData.Stride * 2 - 8 >= 0 && i + bmpData.Stride * 2 - 8 < argbValues.Length && barePositions[i + bmpData.Stride * 2 - 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 - 8]) { argbValues[i + bmpData.Stride * 2 - 8] = 255; argbValues[i + bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                        if (argbValues[i] > 0 && i - bmpData.Stride * 2 + 8 >= 0 && i - bmpData.Stride * 2 + 8 < argbValues.Length && argbValues[i - bmpData.Stride * 2 + 8] == 0 && darkOutline) { outlineValues[i - bmpData.Stride * 2 + 8] = 255; } else if (i - bmpData.Stride * 2 + 8 >= 0 && i - bmpData.Stride * 2 + 8 < argbValues.Length && barePositions[i - bmpData.Stride * 2 + 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 + 8]) { argbValues[i - bmpData.Stride * 2 + 8] = 255; argbValues[i - bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                        if (argbValues[i] > 0 && i - bmpData.Stride * 2 + 4 >= 0 && i - bmpData.Stride * 2 + 4 < argbValues.Length && argbValues[i - bmpData.Stride * 2 + 4] == 0 && darkOutline) { outlineValues[i - bmpData.Stride * 2 + 4] = 255; } else if (i - bmpData.Stride * 2 + 4 >= 0 && i - bmpData.Stride * 2 + 4 < argbValues.Length && barePositions[i - bmpData.Stride * 2 + 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 + 4]) { argbValues[i - bmpData.Stride * 2 + 4] = 255; argbValues[i - bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                        if (argbValues[i] > 0 && i - bmpData.Stride * 2 - 4 >= 0 && i - bmpData.Stride * 2 - 4 < argbValues.Length && argbValues[i - bmpData.Stride * 2 - 4] == 0 && darkOutline) { outlineValues[i - bmpData.Stride * 2 - 4] = 255; } else if (i - bmpData.Stride * 2 - 4 >= 0 && i - bmpData.Stride * 2 - 4 < argbValues.Length && barePositions[i - bmpData.Stride * 2 - 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 - 4]) { argbValues[i - bmpData.Stride * 2 - 4] = 255; argbValues[i - bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                        if (argbValues[i] > 0 && i - bmpData.Stride * 2 - 8 >= 0 && i - bmpData.Stride * 2 - 8 < argbValues.Length && argbValues[i - bmpData.Stride * 2 - 8] == 0 && darkOutline) { outlineValues[i - bmpData.Stride * 2 - 8] = 255; } else if (i - bmpData.Stride * 2 - 8 >= 0 && i - bmpData.Stride * 2 - 8 < argbValues.Length && barePositions[i - bmpData.Stride * 2 - 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 - 8]) { argbValues[i - bmpData.Stride * 2 - 8] = 255; argbValues[i - bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                        */
                }
            }

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
            Bitmap bmp = new Bitmap(vwidth * 16 + 8, vheight * 16 + 8, PixelFormat.Format32bppArgb);

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
                    int current_color = 25;
                    //int unshaded = VoxelLogic.WithoutShadingK(vx.color);
                    //int current_color = ((255 - vx.color) % 4 == 0) ? (255 - vx.color) / 4 + kcolorcount : ((254 - vx.color) % 4 == 0) ? (253 - clear) / 4 : (253 - vx.color) / 4;
                    int p = 0;
                    int mod_color = current_color;

                    for (int j = 0; j < vheight; j++)
                    {
                        for (int i = 0; i < 4 * vwidth; i++)
                        {
                            //p = 4 * ((vx.x + vx.y) * 2 + 2) + i + bmpData.Stride * (128 + 2 - vx.y + vx.x + j);
                            p = voxelToPixelK16(i, j, vx.x, vx.y, 0, 0, 4, 25, bmpData.Stride, 0, true);
                            if (argbValues[p] == 0)
                            {
                                zbuffer[p] = vx.z;
                                xbuffer[p] = vx.x;
                                mod_color = current_color;
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
                                        }
                                        else if (wave < 0.55)
                                        {
                                            wave = -11 / wave;
                                        }
                                        else if (wave < 0.6)
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


                                    argbValues[p - 3] = (byte)Clamp(kcurrent[mod_color][i - 3 + j * (vwidth * 4)] + wave, 1, 255);
                                    argbValues[p - 2] = (byte)Clamp(kcurrent[mod_color][i - 2 + j * (vwidth * 4)] + wave, 1, 255);
                                    argbValues[p - 1] = (byte)Clamp(kcurrent[mod_color][i - 1 + j * (vwidth * 4)] + wave, 1, 255);
                                    argbValues[p - 0] = 255;
                                }
                                /*
                                else
                                {
                                    argbValues[p] = kcurrent[mod_color][i + j * (vwidth * 4)];
                                }*/
                                if (outlineColors[p] == 0)
                                    outlineColors[p] = kcurrent[mod_color][i + (4 * vwidth * vheight)]; //(argbValues[p] * 1.2 + 2 < 255) ? (byte)(argbValues[p] * 1.2 + 2) : (byte)255;
                            }
                        }

                    }
                }
            }
            bool darkOutline = false;
            for (int i = 3; i < numBytes; i += 4)
            {
                if (argbValues[i] > 255 * waver_alpha && barePositions[i] == false)
                {

                    if (i + 4 >= 0 && i + 4 < argbValues.Length && argbValues[i + 4] == 0 && darkOutline && outlineValues[i + 4] == 0) { outlineValues[i + 4] = 255; } else if (i + 4 >= 0 && i + 4 < argbValues.Length && barePositions[i + 4] == false && (zbuffer[i] - 1 > zbuffer[i + 4] || xbuffer[i] - 1 > xbuffer[i + 4]) && outlineValues[i + 4] == 0) { outlineValues[i + 4] = 255; outlineValues[i + 4 - 1] = outlineColors[i - 1]; outlineValues[i + 4 - 2] = outlineColors[i - 2]; outlineValues[i + 4 - 3] = outlineColors[i - 3]; }
                    if (i - 4 >= 0 && i - 4 < argbValues.Length && argbValues[i - 4] == 0 && darkOutline && outlineValues[i - 4] == 0) { outlineValues[i - 4] = 255; } else if (i - 4 >= 0 && i - 4 < argbValues.Length && barePositions[i - 4] == false && (zbuffer[i] - 1 > zbuffer[i - 4] || xbuffer[i] - 1 > xbuffer[i - 4]) && outlineValues[i - 4] == 0) { outlineValues[i - 4] = 255; outlineValues[i - 4 - 1] = outlineColors[i - 1]; outlineValues[i - 4 - 2] = outlineColors[i - 2]; outlineValues[i - 4 - 3] = outlineColors[i - 3]; }
                    if (i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && argbValues[i + bmpData.Stride] == 0 && darkOutline && outlineValues[i + bmpData.Stride] == 0) { outlineValues[i + bmpData.Stride] = 255; } else if (i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && barePositions[i + bmpData.Stride] == false && (zbuffer[i] - 1 > zbuffer[i + bmpData.Stride] || xbuffer[i] - 1 > xbuffer[i + bmpData.Stride]) && outlineValues[i + bmpData.Stride] == 0) { outlineValues[i + bmpData.Stride] = 255; outlineValues[i + bmpData.Stride - 1] = outlineColors[i - 1]; outlineValues[i + bmpData.Stride - 2] = outlineColors[i - 2]; outlineValues[i + bmpData.Stride - 3] = outlineColors[i - 3]; }
                    if (i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && argbValues[i - bmpData.Stride] == 0 && darkOutline && outlineValues[i - bmpData.Stride] == 0) { outlineValues[i - bmpData.Stride] = 255; } else if (i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && barePositions[i - bmpData.Stride] == false && (zbuffer[i] - 1 > zbuffer[i - bmpData.Stride] || xbuffer[i] - 1 > xbuffer[i - bmpData.Stride]) && outlineValues[i - bmpData.Stride] == 0) { outlineValues[i - bmpData.Stride] = 255; outlineValues[i - bmpData.Stride - 1] = outlineColors[i - 1]; outlineValues[i - bmpData.Stride - 2] = outlineColors[i - 2]; outlineValues[i - bmpData.Stride - 3] = outlineColors[i - 3]; }
                    /*
                    if (i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && argbValues[i + bmpData.Stride + 4] == 0 && darkOutline && outlineValues[i + bmpData.Stride + 4] == 0) { outlineValues[i + bmpData.Stride + 4] = 255; } else if (i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && barePositions[i + bmpData.Stride + 4] == false && (zbuffer[i] - 1 > zbuffer[i + bmpData.Stride + 4] || xbuffer[i] - 1 > xbuffer[i + bmpData.Stride + 4]) && outlineValues[i + bmpData.Stride + 4] == 0) { outlineValues[i + bmpData.Stride + 4] = 255; outlineValues[i + bmpData.Stride + 4 - 1] = outlineColors[i - 1]; outlineValues[i + bmpData.Stride + 4 - 2] = outlineColors[i - 2]; outlineValues[i + bmpData.Stride + 4 - 3] = outlineColors[i - 3]; }
                    if (i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && argbValues[i - bmpData.Stride - 4] == 0 && darkOutline && outlineValues[i - bmpData.Stride - 4] == 0) { outlineValues[i - bmpData.Stride - 4] = 255; } else if (i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && barePositions[i - bmpData.Stride - 4] == false && (zbuffer[i] - 1 > zbuffer[i - bmpData.Stride - 4] || xbuffer[i] - 1 > xbuffer[i - bmpData.Stride - 4]) && outlineValues[i - bmpData.Stride - 4] == 0) { outlineValues[i - bmpData.Stride - 4] = 255; outlineValues[i - bmpData.Stride - 4 - 1] = outlineColors[i - 1]; outlineValues[i - bmpData.Stride - 4 - 2] = outlineColors[i - 2]; outlineValues[i - bmpData.Stride - 4 - 3] = outlineColors[i - 3]; }
                    if (i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && argbValues[i + bmpData.Stride - 4] == 0 && darkOutline && outlineValues[i + bmpData.Stride - 4] == 0) { outlineValues[i + bmpData.Stride - 4] = 255; } else if (i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && barePositions[i + bmpData.Stride - 4] == false && (zbuffer[i] - 1 > zbuffer[i + bmpData.Stride - 4] || xbuffer[i] - 1 > xbuffer[i + bmpData.Stride - 4]) && outlineValues[i + bmpData.Stride - 4] == 0) { outlineValues[i + bmpData.Stride - 4] = 255; outlineValues[i + bmpData.Stride - 4 - 1] = outlineColors[i - 1]; outlineValues[i + bmpData.Stride - 4 - 2] = outlineColors[i - 2]; outlineValues[i + bmpData.Stride - 4 - 3] = outlineColors[i - 3]; }
                    if (i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && argbValues[i - bmpData.Stride + 4] == 0 && darkOutline && outlineValues[i - bmpData.Stride + 4] == 0) { outlineValues[i - bmpData.Stride + 4] = 255; } else if (i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && barePositions[i - bmpData.Stride + 4] == false && (zbuffer[i] - 1 > zbuffer[i - bmpData.Stride + 4] || xbuffer[i] - 1 > xbuffer[i - bmpData.Stride + 4]) && outlineValues[i - bmpData.Stride + 4] == 0) { outlineValues[i - bmpData.Stride + 4] = 255; outlineValues[i - bmpData.Stride + 4 - 1] = outlineColors[i - 1]; outlineValues[i - bmpData.Stride + 4 - 2] = outlineColors[i - 2]; outlineValues[i - bmpData.Stride + 4 - 3] = outlineColors[i - 3]; }
                    */
                }

            }



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

        private static Bitmap processKFrame(MagicaVoxelData[] parsed, int faction, int palette, int dir, int frame, int maxFrames, bool still, bool darkOutline)
        {
            Bitmap b;
            Bitmap b2 = new Bitmap(vwidth * 16 + 8, vheight * 16 + 8, PixelFormat.Format32bppArgb);

            b = renderK(parsed, dir, faction, palette, frame, maxFrames, still, darkOutline);
            /*
            Graphics g2 = Graphics.FromImage(b2);
            g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g2.DrawImage(b.Clone(new Rectangle(0, 0, 248 * 2, 308 * 2), b.PixelFormat), -40, -80, 248, 308);
            g2.Dispose();*/
            return b;
        }
        public static void processUnitK(string unit, int palette, bool still, bool autoshade)
        {
            for (int faction = 0; faction < factions; faction++)
            {
                Console.WriteLine("Processing: " + unit + ", faction " + faction + ", palette " + palette);
                BinaryReader bin = new BinaryReader(File.Open("Mini/" + unit + "_K.vox", FileMode.Open));
                List<MagicaVoxelData> voxes = VoxelLogic.PlaceShadowsK((autoshade) ? VoxelLogic.AutoShadeK(VoxelLogic.FromMagicaRaw(bin), 16, 16, 16) : VoxelLogic.FromMagicaRaw(bin));

                setupCurrentColorsK(faction, palette, 0);
                Directory.CreateDirectory("vox/Mini/" + altFolder);
                VoxelLogic.WriteVOX("vox/Mini/" + altFolder + unit + "_f" + faction + "_" + palette + ".vox", voxes, (faction == 0 ? "K_ALLY" : "K_OTHER"), palette, 16, 16, 20);
                MagicaVoxelData[] parsed = voxes.ToArray();

                int framelimit = 4;

                string folder = (altFolder + "faction" + faction + "/palette" + palette);//"color" + i;
                Directory.CreateDirectory(folder); //("color" + i);
                for (int bodyPalette = 0; bodyPalette < DungeonPalettes.fleshTones.Length; bodyPalette++)
                {

                    setupCurrentColorsK(faction, palette, bodyPalette);
                    MagicaVoxelData[] p2 = VoxelLogic.Lovecraftiate(parsed, kcolors);
                    for (int f = 0; f < framelimit; f++)
                    { //
                        for (int dir = 0; dir < 4; dir++)
                        {
                            Bitmap b = processKFrame(p2, faction, palette, dir, f, framelimit, still, true);
                            b.Save(folder + "/palette" + palette + "(" + bodyPalette + ")_" + unit + "_face" + dir + "_" + f + ".png", ImageFormat.Png);
                            b.Dispose();
                        }
                    }

                    Directory.CreateDirectory("gifs/Mini/" + altFolder + "/faction" + faction);
                    ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
                    startInfo.UseShellExecute = false;
                    string s = "";

                    s = folder + "/palette" + palette + "(" + bodyPalette + ")_" + unit + "_face* ";
                    startInfo.Arguments = "-dispose background -delay 25 -loop 0 " + s + " gifs/Mini/" + altFolder + "/faction" + faction + "/palette" + palette + "(" + bodyPalette + ")_" + unit + "_animated.gif";
                    Process.Start(startInfo).WaitForExit();
                }
            }
        }


        public static void processUnitK(string unit, int palette, bool preserveBodyPalette, bool still, bool autoshade)
        {
            if (!preserveBodyPalette)
            {
                processUnitK(unit, palette, still, autoshade);
                return;
            }
            for (int faction = 0; faction < 2; faction++)
            {
                Console.WriteLine("Processing: " + unit + ", faction " + faction + ", palette " + palette);
                BinaryReader bin = new BinaryReader(File.Open("Mini/" + unit + "_K.vox", FileMode.Open));
                List<MagicaVoxelData> voxes = VoxelLogic.PlaceShadowsK((autoshade) ? VoxelLogic.AutoShadeK(VoxelLogic.FromMagicaRaw(bin), 16, 16, 16) : VoxelLogic.FromMagicaRaw(bin));
                Directory.CreateDirectory("vox/Mini/" + altFolder);
                VoxelLogic.WriteVOX("vox/Mini/" + altFolder + unit + "_f" + faction + "_" + palette + ".vox", voxes, (faction == 0 ? "K_ALLY" : "K_OTHER"), palette, 16, 16, 20);
                MagicaVoxelData[] parsed = voxes.ToArray();
                /*
                for (int i = 0; i < parsed.Length; i++)
                {
                    if ((254 - parsed[i].color) % 4 == 0)
                        parsed[i].color--;
                }*/
                int framelimit = 4;

                setupCurrentColorsK(faction, palette);

                MagicaVoxelData[] p2 = VoxelLogic.Lovecraftiate(parsed, kcolors);

                string folder = (altFolder + "faction" + faction + "/palette" + palette);
                Directory.CreateDirectory(folder);
                for (int f = 0; f < framelimit; f++)
                { //
                    for (int dir = 0; dir < 4; dir++)
                    {
                        Bitmap b = processKFrame(p2, faction, palette, dir, f, framelimit, still, true);
                        b.Save(folder + "/palette" + palette + "(0)_" + unit + "_face" + dir + "_" + f + ".png", ImageFormat.Png);
                        b.Dispose();
                    }
                }
                Directory.CreateDirectory("gifs/Mini/" + altFolder + "/faction" + faction);
                ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
                startInfo.UseShellExecute = false;
                string s = "";

                s = folder + "/palette" + palette + "(0)_" + unit + "_face* ";
                startInfo.Arguments = "-dispose background -delay 25 -loop 0 " + s + " gifs/Mini/" + altFolder + "/faction" + faction + "/palette" + palette + "(0)_" + unit + "_animated.gif";
                Process.Start(startInfo).WaitForExit();
            }
        }

        public static void processTerrainK(string unit, string subfolder, int palette, bool still, bool autoshade, bool addFloor)
        {
            int faction = 0;
            Console.WriteLine("Processing: " + unit + ", palette " + palette);
            List<MagicaVoxelData> voxes;
            if (addFloor)
            {
                BinaryReader bin = new BinaryReader(File.Open("Mini/" + subfolder + "/" + unit + "_K.vox", FileMode.Open));
                BinaryReader binFloor = new BinaryReader(File.Open("Mini/" + subfolder + "/Floor_K.vox", FileMode.Open));
                IEnumerable<MagicaVoxelData> structure = ((autoshade) ? VoxelLogic.AutoShadeK(VoxelLogic.FromMagicaRaw(bin), 16, 16, 16) : VoxelLogic.FromMagicaRaw(bin))
                    .Select(v => VoxelLogic.AlterVoxel(v, 0, 0, 1, v.color));
                voxes = structure.Concat((autoshade) ? VoxelLogic.AutoShadeK(VoxelLogic.FromMagicaRaw(binFloor), 16, 16, 16) : VoxelLogic.FromMagicaRaw(binFloor)).ToList();
            }
            else
            {
                BinaryReader bin = new BinaryReader(File.Open("Mini/" + subfolder + "/" + unit + "_K.vox", FileMode.Open));
                voxes = VoxelLogic.PlaceShadowsK((autoshade) ? VoxelLogic.AutoShadeK(VoxelLogic.FromMagicaRaw(bin), 16, 16, 16) : VoxelLogic.FromMagicaRaw(bin));
            }

            Directory.CreateDirectory("vox/Mini/" + altFolder + subfolder + "/");
            VoxelLogic.WriteVOX("vox/Mini/" + altFolder + subfolder + "/" + unit + "_f0" + "_" + palette + ".vox", voxes, (faction == 0 ? "K_ALLY" : "K_OTHER"), palette, 16, 16, 20);
            MagicaVoxelData[] parsed = voxes.ToArray();

            int framelimit = 4;

            setupCurrentColorsK(faction, palette);

            MagicaVoxelData[] p2 = VoxelLogic.Lovecraftiate(parsed, kcolors);

            string folder = (altFolder + subfolder + "/" + "faction" + faction + "/palette" + palette);
            Directory.CreateDirectory(folder);
            for (int f = 0; f < framelimit; f++)
            { //
                for (int dir = 0; dir < 4; dir++)
                {
                    Bitmap b = processKFrame(p2, faction, palette, dir, f, framelimit, still, false);
                    b.Save(folder + "/palette" + palette + "(0)_" + unit + "_face" + dir + "_" + f + ".png", ImageFormat.Png);
                    b.Dispose();
                }
            }
            Directory.CreateDirectory("gifs/Mini/" + altFolder + subfolder + "/faction" + faction);
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";

            s = folder + "/palette" + palette + "(0)_" + unit + "_face* ";
            startInfo.Arguments = "-dispose background -delay 25 -loop 0 " + s + " gifs/Mini/" + altFolder + subfolder + "/faction" + faction + "/palette" + palette + "(0)_" + unit + "_animated.gif";
            Process.Start(startInfo).WaitForExit();

        }

        public static void processWaterK(string subfolder, int palette)
        {
            int faction = 0;
            Console.WriteLine("Processing: Water Tiles, palette " + palette);
            // int framelimit = 4;

            setupCurrentColorsK(faction, palette);

            string folder = (altFolder + subfolder + "/" + "faction" + faction + "/palette" + palette);
            Directory.CreateDirectory(folder);
            for (int v = 0; v < 16; v++)
            { //
                for (int dir = 0; dir < 4; dir++)
                {
                    Bitmap b = generateWaterMask(dir, v);
                    b.Save(folder + "/palette" + palette + "(0)_Water_face" + dir + "_" + string.Format("{0:x}", v) + ".png", ImageFormat.Png);
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

        static void Main(string[] args)
        {
            VoxelLogic.Initialize();
            altFolder = "MicroOrtho/";
            krendered = storeColorCubesK();
            VoxelLogic.krendered = krendered;
            kFleshRendered = storeColorCubesFleshToneK();
            VoxelLogic.kFleshRendered = kFleshRendered;
            
            processUnitK("Human_Male", 0, true, false);
            processUnitK("Human_Female", 10, true, false);

            processUnitK("Warrior_Male", 7, true, false);
            processUnitK("Warrior_Female", 7, true, false);
            processUnitK("Mage_Male", 8, true, false);
            processUnitK("Mage_Female", 8, true, false);
            processUnitK("Rogue_Male", 1, true, false);
            processUnitK("Rogue_Female", 9, true, false);
            processUnitK("Priest_Male", 11, true, false);
            processUnitK("Priest_Female", 11, true, false);
            
            processTerrainK("Water", "Caves", 4, true, false, false);
            
            processWaterK("Caves", 4);

            
            processTerrainK("Floor", "Caves", 4, true, false, false);
            processTerrainK("Wall_Straight", "Caves", 4, true, false, true);
            processTerrainK("Wall_Corner", "Caves", 4, true, false, true);
            processTerrainK("Wall_Tee", "Caves", 4, true, false, true);
            processTerrainK("Wall_Cross", "Caves", 4, true, false, true);
            processTerrainK("Door_Closed", "Caves", 4, true, false, true);
            processTerrainK("Door_Open", "Caves", 4, true, false, true);
            

        }
    }
}