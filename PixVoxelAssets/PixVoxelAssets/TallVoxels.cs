using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AssetsPV
{
    public class TallVoxels
    {
        private static Random r = new Random(TallPaletteDraw.r.Next());
        public static StringBuilder log = new StringBuilder();
        public static string[] Directions = { "SE", "SW", "NW", "NE" };
        public static string[] Terrains = new string[]
        {"Plains","Forest","Desert","Jungle","Hills"
        ,"Mountains","Ruins","Tundra","Road","River", "Basement"};

        private static bool PalettesInitialized = false;
        private static List<Color>[] SDPalettes = new List<Color>[18];
        public static void InitializePalettes()
        {
            Bitmap b;
            b = new Bitmap(256, 32, PixelFormat.Format32bppArgb);
            for (int c = 0; c < 18; c++)
            {
                Bitmap bmp;
                if (c == 8)
                {
                    bmp = new Bitmap("PaletteCrazy.png");
                }
                else if (c == 9)
                {
                    bmp = new Bitmap("PaletteTerrain.png");
                }
                else if (c > 9)
                {
                    bmp = new Bitmap("PaletteTerrainColor" + (c - 10) + ".png");
                }
                else
                {
                    bmp = new Bitmap("PaletteColor" + c + ".png");
                }
                SDPalettes[c] = new List<Color>();
                for (int i = 0; i < bmp.Width; i += (i % 3) + 1)
                {
                    SDPalettes[c].Add(bmp.GetPixel(i, 3));
                    //}
                    //for (int i = 1; i < bmp.Width; i += 3)
                    //{
                    SDPalettes[c].Add(bmp.GetPixel(i, 0));
                }
                for (int cl = 0; cl < SDPalettes[c].Count; cl++)
                {
                    b.SetPixel(cl, c, SDPalettes[c][cl]);
                }
                for (int cl = SDPalettes[c].Count; cl < 256; cl++)
                {
                    b.SetPixel(cl, c, Color.Black);
                }

                Console.WriteLine("Color " + c + " has: " + SDPalettes[c].Count + " entries.");

                //SDPalettes[c] = colors;

                bmp.Dispose();
            }
            b.Save("Palettes/Palette.png", ImageFormat.Png);

            b.Dispose();

            GC.Collect();
            PalettesInitialized = true;

        }
        public static void CreateChannelBitmap(Bitmap bmp, string savename)
        {
            if (PalettesInitialized == false)
                return;
            Bitmap b = new Bitmap(bmp);
            //byte[] array = new byte[bmp.Width * bmp.Height];

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    int cb = (SDPalettes[8].FindByIndex(bmp.GetPixel(x, y)));
                    if (cb != -1)
                    {
                        if (cb == 0)
                            b.SetPixel(x, y, Color.FromArgb(0, cb, 0, 0));
                        else
                            b.SetPixel(x, y, Color.FromArgb(cb, 0, 0));
                    }
                    else
                        Console.WriteLine("Color " + bmp.GetPixel(x, y).ToString() + " not found at: " + x + ", " + y);
                }
            }
            b.Save(savename, ImageFormat.Png);
            //BitmapSource bitmap = BitmapSource.Create(bmp.Width, bmp.Height, 96, 96, Media.PixelFormats.Indexed8, Palettes[palette], array, bmp.Width);
            /*
            using (FileStream stream = new FileStream(savename, FileMode.Create))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                // encoder.Palette = Palettes[palette];
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(stream);
            }*/
        }
        public static void CreateChannelBitmap(Bitmap bmp, string savename, int altPalette)
        {
            if (PalettesInitialized == false)
                return;
            Bitmap b = new Bitmap(bmp);
            //byte[] array = new byte[bmp.Width * bmp.Height];

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    int cb = (SDPalettes[altPalette].FindByIndex(bmp.GetPixel(x, y)));
                    if (cb != -1)
                    {
                        if (cb == 0)
                            b.SetPixel(x, y, Color.FromArgb(0, cb, 0, 0));
                        else
                            b.SetPixel(x, y, Color.FromArgb(cb, 0, 0));
                    }
                    else
                        Console.WriteLine("Color " + bmp.GetPixel(x, y).ToString() + " not found at: " + x + ", " + y);

                    //array[bmp.Width * y + x] = (byte)cb;
                }
            }
            b.Save(savename, ImageFormat.Png);

        }

        public static List<MagicaVoxelData> adjacent(MagicaVoxelData pos)
        {
            List<MagicaVoxelData> near = new List<MagicaVoxelData>(6);
            near.Add(new MagicaVoxelData { x = (byte)(pos.x + 1), y = (byte)(pos.y), z = (byte)(pos.z), color = (byte)(pos.color), });
            near.Add(new MagicaVoxelData { x = (byte)(pos.x - 1), y = (byte)(pos.y), z = (byte)(pos.z), color = (byte)(pos.color), });
            near.Add(new MagicaVoxelData { x = (byte)(pos.x), y = (byte)(pos.y + 1), z = (byte)(pos.z), color = (byte)(pos.color), });
            near.Add(new MagicaVoxelData { x = (byte)(pos.x), y = (byte)(pos.y - 1), z = (byte)(pos.z), color = (byte)(pos.color), });
            near.Add(new MagicaVoxelData { x = (byte)(pos.x), y = (byte)(pos.y), z = (byte)(pos.z + 1), color = (byte)(pos.color), });
            if (pos.z > 0)
                near.Add(new MagicaVoxelData { x = (byte)(pos.x), y = (byte)(pos.y), z = (byte)(pos.z - 1), color = (byte)(pos.color), });
            return near;

        }
        public static List<MagicaVoxelData> adjacent(MagicaVoxelData pos, int[] colors)
        {
            List<MagicaVoxelData> near = new List<MagicaVoxelData>(6);
            near.Add(new MagicaVoxelData { x = (byte)(pos.x + 1), y = (byte)(pos.y), z = (byte)(pos.z), color = (byte)(colors.RandomElement()), });
            near.Add(new MagicaVoxelData { x = (byte)(pos.x - 1), y = (byte)(pos.y), z = (byte)(pos.z), color = (byte)(colors.RandomElement()), });
            near.Add(new MagicaVoxelData { x = (byte)(pos.x), y = (byte)(pos.y + 1), z = (byte)(pos.z), color = (byte)(colors.RandomElement()), });
            near.Add(new MagicaVoxelData { x = (byte)(pos.x), y = (byte)(pos.y - 1), z = (byte)(pos.z), color = (byte)(colors.RandomElement()), });
            if (pos.z < 20)
                near.Add(new MagicaVoxelData { x = (byte)(pos.x), y = (byte)(pos.y), z = (byte)(pos.z + 1), color = (byte)(colors.RandomElement()), });
            if (pos.z > 0)
                near.Add(new MagicaVoxelData { x = (byte)(pos.x), y = (byte)(pos.y), z = (byte)(pos.z - 1), color = (byte)(colors.RandomElement()), });
            return near;

        }


        public static MagicaVoxelData[][] FieryExplosion(MagicaVoxelData[] voxels, bool blowback, bool old)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[9][];
            voxelFrames[0] = new MagicaVoxelData[voxels.Length];
            voxels.CopyTo(voxelFrames[0], 0);
            for (int i = 0; i < voxels.Length; i++)
            {
                voxelFrames[0][i].x += 5;
                voxelFrames[0][i].y += 5;
            }
            for (int f = 1; f < 4; f++)
            {
                List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxels.Length);
                MagicaVoxelData[] vls = new MagicaVoxelData[voxelFrames[f - 1].Length], working = new MagicaVoxelData[voxelFrames[f - 1].Length]; //.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)
                voxelFrames[f - 1].CopyTo(vls, 0);
                voxelFrames[f - 1].CopyTo(working, 0);

                int[] minX = new int[30];
                int[] maxX = new int[30];
                float[] midX = new float[30];
                for (int level = 0; level < 30; level++)
                {
                    minX[level] = vls.Min(v => v.x * ((v.z != level || v.color == 249 - 96 || v.color == 249 - 128 ||
                        v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color <= 249 - 168) ? 100 : 1));
                    maxX[level] = vls.Max(v => v.x * ((v.z != level || v.color == 249 - 96 || v.color == 249 - 128 ||
                        v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color <= 249 - 168) ? 0 : 1));
                    midX[level] = (maxX[level] + minX[level]) / 2F;
                }

                int[] minY = new int[30];
                int[] maxY = new int[30];
                float[] midY = new float[30];
                for (int level = 0; level < 30; level++)
                {
                    minY[level] = vls.Min(v => v.y * ((v.z != level || v.color == 249 - 96 || v.color == 249 - 128 ||
                        v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color <= 249 - 168) ? 100 : 1));
                    maxY[level] = vls.Max(v => v.y * ((v.z != level || v.color == 249 - 96 || v.color == 249 - 128 ||
                        v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color <= 249 - 168) ? 0 : 1));
                    midY[level] = (maxY[level] + minY[level]) / 2F;
                }

                int minZ = vls.Min(v => v.z * ((v.color == 249 - 96 || v.color == 249 - 128 ||
                        v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color <= 249 - 168) ? 100 : 1));
                int maxZ = vls.Max(v => v.z * ((v.color == 249 - 96 || v.color == 249 - 128 ||
                        v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color <= 249 - 168) ? 0 : 1));
                float midZ = (maxZ + minZ) / 2F;

                int iter = 0;
                foreach (MagicaVoxelData v in vls)
                {
                    MagicaVoxelData mvd = new MagicaVoxelData();
                    if (v.color == 249 - 64) //flesh
                        mvd.color = (byte)((r.Next(1 + f) == 0) ? 249 - 144 : (r.Next(8) == 0) ? 249 - 152 : 249 - 64); //random transform to guts
                    else if (v.color == 249 - 144) //guts
                        mvd.color = (byte)((r.Next(6) == 0) ? 249 - 152 : 249 - 144); //random transform to orange fire
                    else if (v.color == 249 - 80) //lights
                        mvd.color = 249 - 16; //cannon color for broken lights
                    else if (v.color == 249 - 88) //windows
                        mvd.color = (byte)((r.Next(3) == 0) ? 249 - 168 : 249 - 88); //random transform to clear
                    else if (v.color == 249 - 104) //rotors
                        mvd.color = 249 - 56; //helmet color for broken rotors
                    else if (v.color == 249 - 112)
                        mvd.color = 249 - 168; //clear non-active rotors
                    else if (v.color <= 249 - 168)
                        mvd.color = 249 - 168; //clear and markers become clear
                    else if (v.color == 249 - 152) //orange fire
                        mvd.color = (byte)((r.Next(3) <= 1) ? 249 - 160 : ((r.Next(5) == 0) ? 249 - 136 : 249 - 152)); //random transform to yellow fire or smoke
                    else if (v.color == 249 - 160) //yellow fire
                        mvd.color = (byte)((r.Next(3) <= 1) ? 249 - 152 : 249 - 160); //random transform to orange fire
                    else
                        mvd.color = (byte)((r.Next(7 - f) == 0) ? 249 - (152 + ((r.Next(4) == 0) ? 8 : 0)) : v.color); //random transform to orange or yellow fire
                    float xMove = 0, yMove = 0, zMove = 0;

                    if (mvd.color == 249 - 152 || mvd.color == 249 - 160 || mvd.color == 249 - 136)
                    {
                        zMove = f / 3F;
                    }
                    else
                    {
                        if (v.x > midX[v.z])// && v.x > maxX - 5)
                            xMove = ((midX[v.z] - r.Next(3) - ((blowback) ? 9 : 0) - (maxX[v.z] - v.x)) * 0.8F * ((v.z - minZ + 3) / (maxZ - minZ + 1F)));// / 300F) * (v.z + 5); //5 -
                        else if (v.x < midX[v.z])// && v.x < minX + 5)
                            xMove = ((0 + (v.x - midX[v.z] + r.Next(3) - ((blowback) ? 8 : 0))) * 0.8F * ((v.z - minZ + 3) / (maxZ - minZ + 1F))); // / 300F) * ((v.z + 5)) * f; //-5 +

                        if (v.y > midY[v.z])// && v.y > maxY - 5)
                            yMove = ((midY[v.z] - r.Next(3) - (maxY[v.z] - v.y)) * 0.8F * ((v.z - minZ + 3) / (maxZ - minZ + 1F))); // / 300F) * (v.z + 5);//5 -
                        else if (v.y < midY[v.z])// && v.y < minY + 5)
                            yMove = ((0 + (v.y - midY[v.z] + r.Next(3))) * 0.8F * ((v.z - minZ + 3) / (maxZ - minZ + 1F))); // / 300F) * (v.z + 5); //-5 +
                        /*
                        if (v.x > midX)// && v.x > maxX - 5)
                            xMove = (5 - (maxX - v.x)) / ((30F - maxX)) * 3;
                        else if (v.x < midX)// && v.x < minX + 5)
                            xMove = (-5 + (minX - v.x)) / (minX + 1) * 2;

                        if (v.y > midY)// && v.y > maxY - 5)
                            yMove = (5 - (maxY - v.y)) / ((30F - maxY)) * 3;
                        else if (v.y < midY)// && v.y < minY + 5)
                            yMove = (-5 + (minY - v.y)) / (minY + 1) * 3;
                        */
                        if (minZ > 0)
                            zMove = ((v.z) * (1 - f) / 10F);
                        else
                            zMove = (v.z / ((maxZ + 1) * (0.4F))) * (4 - f) * 0.6F;
                    }
                    if (xMove > 0)
                    {
                        float nv = (v.x + (xMove * f / 9F));
                        if (nv < 0) nv = 0;
                        if (nv > 29) nv = 29;
                        mvd.x = (byte)(Math.Ceiling(nv));
                    }
                    else if (xMove < 0)
                    {
                        float nv = (v.x + (xMove * f / 9F));
                        if (nv < 0) nv = 0;
                        if (nv > 29) nv = 29;
                        mvd.x = (byte)((blowback) ? Math.Floor(nv) : (Math.Ceiling(nv)));
                    }
                    else
                    {
                        mvd.x = v.x;
                    }
                    if (yMove > 0)
                    {
                        float nv = (v.y + (yMove * f / 9F));
                        if (nv < 0) nv = 0;
                        if (nv > 29) nv = 29;
                        mvd.y = (byte)(Math.Ceiling(nv));
                    }
                    else if (yMove < 0)
                    {
                        float nv = (v.y + (yMove * f / 9F));
                        if (nv < 0) nv = 0;
                        if (nv > 29) nv = 29;
                        mvd.y = (byte)(Math.Floor(nv));

                    }
                    else
                    {
                        mvd.y = v.y;
                    }

                    if (zMove != 0)
                    {
                        float nv = (v.z + (zMove / f));

                        if (nv <= 0 && f < 8) nv = r.Next(2); //bounce
                        else if (nv < 0) nv = 0;

                        if (nv > 29) nv = 29;
                        mvd.z = (byte)Math.Round(nv);
                    }
                    else
                    {
                        mvd.z = v.z;
                    }
                    working[iter] = mvd;
                    iter++;
                }
                voxelFrames[f] = new MagicaVoxelData[working.Length];
                working.ToArray().CopyTo(voxelFrames[f], 0);
            }
            for (int f = 4; f < 9; f++)
            {
                List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxels.Length);
                MagicaVoxelData[] vls = new MagicaVoxelData[voxelFrames[f - 1].Length], working = new MagicaVoxelData[voxelFrames[f - 1].Length]; //.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)
                voxelFrames[f - 1].CopyTo(vls, 0);
                voxelFrames[f - 1].CopyTo(working, 0);

                int[] minX = new int[30];
                int[] maxX = new int[30];
                float[] midX = new float[30];
                for (int level = 0; level < 30; level++)
                {
                    minX[level] = vls.Min(v => v.x * ((v.z != level || v.color == 249 - 96 || v.color == 249 - 128 ||
                        v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color < 249 - 168) ? 100 : 1));
                    maxX[level] = vls.Max(v => v.x * ((v.z != level || v.color == 249 - 96 || v.color == 249 - 128 ||
                        v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color < 249 - 168) ? 0 : 1));
                    midX[level] = (maxX[level] + minX[level]) / 2F;
                }

                int[] minY = new int[30];
                int[] maxY = new int[30];
                float[] midY = new float[30];
                for (int level = 0; level < 30; level++)
                {
                    minY[level] = vls.Min(v => v.y * ((v.z != level || v.color == 249 - 96 || v.color == 249 - 128 ||
                        v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color < 249 - 168) ? 100 : 1));
                    maxY[level] = vls.Max(v => v.y * ((v.z != level || v.color == 249 - 96 || v.color == 249 - 128 ||
                        v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color < 249 - 168) ? 0 : 1));
                    midY[level] = (maxY[level] + minY[level]) / 2F;
                }

                int minZ = vls.Min(v => v.z * ((v.color == 249 - 96 || v.color == 249 - 128 || v.color == 249 - 136) ? 100 : 1));
                int maxZ = vls.Max(v => v.z * ((v.color == 249 - 96 || v.color == 249 - 128 || v.color == 249 - 136 || v.color < 249 - 168) ? 0 : 1));
                float midZ = (maxZ + minZ) / 2F;

                int iter = 0;
                foreach (MagicaVoxelData v in vls)
                {
                    MagicaVoxelData mvd = new MagicaVoxelData();
                    if (v.color == 249 - 64) //flesh
                        mvd.color = (byte)((r.Next(1 + f) == 0) ? 249 - 144 : (r.Next(6) == 0) ? 249 - 152 : 249 - 64); //random transform to guts
                    else if (v.color == 249 - 144) //guts
                        mvd.color = (byte)((r.Next(5) == 0) ? 249 - 152 : 249 - 144); //random transform to orange fire
                    else if (v.color <= 249 - 168) //clear and markers
                        mvd.color = (byte)249 - 168; //clear stays clear
                    else if (v.color == 249 - 80) //lights
                        mvd.color = 249 - 16; //cannon color for broken lights
                    else if (v.color == 249 - 88) //windows
                        mvd.color = (byte)((r.Next(3) == 0) ? 249 - 168 : 249 - 88); //random transform to clear
                    else if (v.color == 249 - 104) //rotors
                        mvd.color = 249 - 56; //helmet color for broken rotors
                    else if (v.color == 249 - 112)
                        mvd.color = 249 - 168; //clear non-active rotors
                    else if (v.color == 249 - 152) //orange fire
                        mvd.color = (byte)((r.Next(8) <= f) ? 249 - 136 : ((r.Next(3) <= 1) ? 249 - 160 : ((r.Next(3) == 0) ? 249 - 136 : 249 - 152))); //random transform to yellow fire or smoke
                    else if (v.color == 249 - 160) //yellow fire
                        mvd.color = (byte)((r.Next(8) <= f) ? 249 - 136 : ((r.Next(3) <= 1) ? 249 - 152 : ((r.Next(4) == 0) ? 249 - 136 : 249 - 160))); //random transform to orange fire or smoke
                    else if (v.color == 249 - 136) //smoke
                        mvd.color = (byte)((r.Next(8) <= f - 1) ? 249 - 168 : 249 - 136); //random transform to clear
                    else
                        mvd.color = (byte)((r.Next(f * 2) <= 2) ? 249 - (152 + ((r.Next(4) == 0) ? 8 : 0)) : v.color); //random transform to orange or yellow fire //(f >= 6) ? 249 - 136 :
                    float xMove = 0, yMove = 0, zMove = 0;
                    if (mvd.color == 249 - 152 || mvd.color == 249 - 160 || mvd.color == 249 - 136)
                    {
                        zMove = f * 0.7F;
                    }
                    else
                    {
                        if (v.x > midX[v.z])// && v.x > maxX - 5)
                            xMove = ((midX[v.z] - r.Next(4) - ((blowback) ? 7 : 0) - (maxX[v.z] - v.x)) * 0.6F * ((v.z - minZ + 1) / (maxZ - minZ + 1F))); //5 -
                        else if (v.x < midX[v.z])// && v.x < minX + 5)
                            xMove = ((0 + (v.x - midX[v.z] + r.Next(4) - ((blowback) ? 6 : 0))) * 0.6F * ((v.z - minZ + 1) / (maxZ - minZ + 1F))); //-5 +
                        if (v.y > midY[v.z])// && v.y > maxY - 5)
                            yMove = ((midY[v.z] - r.Next(4) - (maxY[v.z] - v.y)) * 0.6F * ((v.z - minZ + 1) / (maxZ - minZ + 1F)));//5 -
                        else if (v.y < midY[v.z])// && v.y < minY + 5)
                            yMove = ((0 + (v.y - midY[v.z] + r.Next(4))) * 0.6F * ((v.z - minZ + 1) / (maxZ - minZ + 1F))); //-5 +


                        if (f < 5 && minZ == 0)
                            zMove = (v.z / ((maxZ + 1) * (0.4F))) * (5 - f) * 0.3F;
                        else
                            zMove = (1 - f * 1.1F);
                    }
                    if (xMove > 0)
                    {
                        float nv = (v.x + (xMove / f));
                        if (nv < 0) nv = 0;
                        if (nv > 29) nv = 29;
                        mvd.x = (byte)(Math.Floor(nv));
                    }
                    else if (xMove < 0)
                    {
                        float nv = (v.x + (xMove / f));
                        if (nv < 0) nv = 0;
                        if (nv > 29) nv = 29;
                        mvd.x = (byte)((blowback) ? Math.Floor(nv) : (Math.Ceiling(nv)));
                    }
                    else
                    {
                        mvd.x = v.x;
                    }
                    if (yMove > 0)
                    {
                        float nv = (v.y + (yMove / f));
                        if (nv < 0) nv = 0;
                        if (nv > 29) nv = 29;
                        mvd.y = (byte)(Math.Floor(nv));
                    }
                    else if (yMove < 0)
                    {
                        float nv = (v.y + (yMove / f));
                        if (nv < 0) nv = 0;
                        if (nv > 29) nv = 29;
                        mvd.y = (byte)(Math.Ceiling(nv));
                    }
                    else
                    {
                        mvd.y = v.y;
                    }
                    if (zMove != 0)
                    {
                        float nv = (v.z + (zMove));

                        if (nv <= 0 && f < 8) nv = r.Next(2); //bounce
                        else if (nv < 0) nv = 0;

                        if (nv > 29) nv = 29;
                        mvd.z = (byte)Math.Round(nv);
                    }
                    else
                    {
                        mvd.z = v.z;
                    }
                    working[iter] = mvd;
                    iter++;
                }
                voxelFrames[f] = new MagicaVoxelData[working.Length];
                working.ToArray().CopyTo(voxelFrames[f], 0);
            }
            for (int f = 1; f < 9; f++)
            {

                List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxels.Length);
                int[,] taken = new int[30, 30];
                taken.Fill(-1);
                for (int i = 0; i < voxelFrames[f].Length; i++)
                {
                    // do not store this voxel if it lies out of range of the voxel chunk (30x30x30)
                    if (voxelFrames[f][i].x >= 30 || voxelFrames[f][i].y >= 30 || voxelFrames[f][i].z >= 30)
                    {
                        Console.Write("Voxel out of bounds: " + voxelFrames[f][i].x + ", " + voxelFrames[f][i].y + ", " + voxelFrames[f][i].z);
                        continue;
                    }

                    altered.Add(voxelFrames[f][i]);
                    if (-1 == taken[voxelFrames[f][i].x, voxelFrames[f][i].y] && voxelFrames[f][i].color != 249 - 80 && voxelFrames[f][i].color != 249 - 104 && voxelFrames[f][i].color != 249 - 112
                         && voxelFrames[f][i].color != 249 - 96 && voxelFrames[f][i].color != 249 - 128 && voxelFrames[f][i].color != 249 - 136
                         && voxelFrames[f][i].color != 249 - 152 && voxelFrames[f][i].color != 249 - 160 && voxelFrames[f][i].color >= 249 - 168)
                    {
                        MagicaVoxelData vox = new MagicaVoxelData();
                        vox.x = voxelFrames[f][i].x;
                        vox.y = voxelFrames[f][i].y;
                        vox.z = (byte)(0);
                        vox.color = 249 - 96;
                        taken[vox.x, vox.y] = altered.Count();
                        altered.Add(vox);
                    }
                }
                voxelFrames[f] = altered.ToArray();
            }

            MagicaVoxelData[][] frames = new MagicaVoxelData[8][];

            for (int f = 1; f < 9; f++)
            {
                frames[f - 1] = new MagicaVoxelData[voxelFrames[f].Length];
                voxelFrames[f].ToArray().CopyTo(frames[f - 1], 0);
            }
            return frames;
        }
        public static MagicaVoxelData[][] FieryExplosion(MagicaVoxelData[] voxels, bool blowback)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[9][];
            voxelFrames[0] = new MagicaVoxelData[voxels.Length];
            voxels.CopyTo(voxelFrames[0], 0);
            for (int i = 0; i < voxels.Length; i++)
            {
                voxelFrames[0][i].x += 20;
                voxelFrames[0][i].y += 20;
            }
            for (int f = 1; f < 4; f++)
            {
                List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxels.Length);
                MagicaVoxelData[] vls = new MagicaVoxelData[voxelFrames[f - 1].Length], working = new MagicaVoxelData[voxelFrames[f - 1].Length]; //.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)
                voxelFrames[f - 1].CopyTo(vls, 0);
                voxelFrames[f - 1].CopyTo(working, 0);

                int[] minX = new int[40];
                int[] maxX = new int[40];
                float[] midX = new float[40];
                for (int level = 0; level < 40; level++)
                {
                    minX[level] = vls.Min(v => v.x * ((v.z != level || v.color == 249 - 96 || v.color == 249 - 128 ||
                        v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color <= 249 - 168) ? 100 : 1));
                    maxX[level] = vls.Max(v => v.x * ((v.z != level || v.color == 249 - 96 || v.color == 249 - 128 ||
                        v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color <= 249 - 168) ? 0 : 1));
                    midX[level] = (maxX[level] + minX[level]) / 2F;
                }

                int[] minY = new int[40];
                int[] maxY = new int[40];
                float[] midY = new float[40];
                for (int level = 0; level < 40; level++)
                {
                    minY[level] = vls.Min(v => v.y * ((v.z != level || v.color == 249 - 96 || v.color == 249 - 128 ||
                        v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color <= 249 - 168) ? 100 : 1));
                    maxY[level] = vls.Max(v => v.y * ((v.z != level || v.color == 249 - 96 || v.color == 249 - 128 ||
                        v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color <= 249 - 168) ? 0 : 1));
                    midY[level] = (maxY[level] + minY[level]) / 2F;
                }

                int minZ = vls.Min(v => v.z * ((v.color == 249 - 96 || v.color == 249 - 128 ||
                        v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color <= 249 - 168) ? 100 : 1));
                int maxZ = vls.Max(v => v.z * ((v.color == 249 - 96 || v.color == 249 - 128 ||
                        v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color <= 249 - 168) ? 0 : 1));
                float midZ = (maxZ + minZ) / 2F;

                int iter = 0;
                foreach (MagicaVoxelData v in vls)
                {
                    MagicaVoxelData mvd = new MagicaVoxelData();
                    if (v.color == 249 - 64) //flesh
                        mvd.color = (byte)((r.Next(1 + f) == 0) ? 249 - 144 : (r.Next(8) == 0) ? 249 - 152 : 249 - 64); //random transform to guts
                    else if (v.color == 249 - 144) //guts
                        mvd.color = (byte)((r.Next(6) == 0) ? 249 - 152 : 249 - 144); //random transform to orange fire
                    else if (v.color == 249 - 80) //lights
                        mvd.color = 249 - 16; //cannon color for broken lights
                    else if (v.color == 249 - 88) //windows
                        mvd.color = (byte)((r.Next(3) == 0) ? 249 - 168 : 249 - 88); //random transform to clear
                    else if (v.color == 249 - 104) //rotors
                        mvd.color = 249 - 56; //helmet color for broken rotors
                    else if (v.color == 249 - 112)
                        mvd.color = 249 - 168; //clear non-active rotors
                    else if (v.color <= 249 - 168)
                        mvd.color = 249 - 168; //clear and markers become clear
                    else if (v.color == 249 - 152) //orange fire
                        mvd.color = (byte)((r.Next(3) <= 1) ? 249 - 160 : ((r.Next(5) == 0) ? 249 - 136 : 249 - 152)); //random transform to yellow fire or smoke
                    else if (v.color == 249 - 160) //yellow fire
                        mvd.color = (byte)((r.Next(3) <= 1) ? 249 - 152 : 249 - 160); //random transform to orange fire
                    else
                        mvd.color = (byte)((r.Next(7 - f) == 0) ? 249 - (152 + ((r.Next(4) == 0) ? 8 : 0)) : v.color); //random transform to orange or yellow fire
                    float xMove = 0, yMove = 0, zMove = 0;

                    if (mvd.color == 249 - 152 || mvd.color == 249 - 160 || mvd.color == 249 - 136)
                    {
                        zMove = f / 3F;
                    }
                    else
                    {
                        if (v.x > midX[v.z])// && v.x > maxX - 5)
                            xMove = ((midX[v.z] - r.Next(3) - ((blowback) ? 9 : 0) - (maxX[v.z] - v.x)) * 0.8F * ((v.z - minZ + 3) / (maxZ - minZ + 1F)));// / 300F) * (v.z + 5); //5 -
                        else if (v.x < midX[v.z])// && v.x < minX + 5)
                            xMove = ((0 + (v.x - midX[v.z] + r.Next(3) - ((blowback) ? 8 : 0))) * 0.8F * ((v.z - minZ + 3) / (maxZ - minZ + 1F))); // / 300F) * ((v.z + 5)) * f; //-5 +

                        if (v.y > midY[v.z])// && v.y > maxY - 5)
                            yMove = ((midY[v.z] - r.Next(3) - (maxY[v.z] - v.y)) * 0.8F * ((v.z - minZ + 3) / (maxZ - minZ + 1F))); // / 300F) * (v.z + 5);//5 -
                        else if (v.y < midY[v.z])// && v.y < minY + 5)
                            yMove = ((0 + (v.y - midY[v.z] + r.Next(3))) * 0.8F * ((v.z - minZ + 3) / (maxZ - minZ + 1F))); // / 300F) * (v.z + 5); //-5 +
                        /*
                        if (v.x > midX)// && v.x > maxX - 5)
                            xMove = (5 - (maxX - v.x)) / ((30F - maxX)) * 3;
                        else if (v.x < midX)// && v.x < minX + 5)
                            xMove = (-5 + (minX - v.x)) / (minX + 1) * 2;

                        if (v.y > midY)// && v.y > maxY - 5)
                            yMove = (5 - (maxY - v.y)) / ((30F - maxY)) * 3;
                        else if (v.y < midY)// && v.y < minY + 5)
                            yMove = (-5 + (minY - v.y)) / (minY + 1) * 3;
                        */
                        if (minZ > 0)
                            zMove = ((v.z) * (1 - f) / 10F);
                        else
                            zMove = (v.z / ((maxZ + 1) * (0.4F))) * (4 - f) * 0.6F;
                    }
                    if (xMove > 0)
                    {
                        float nv = (v.x + (xMove * f / 9F));
                        if (nv < 0) nv = 0;
                        if (nv > 59) nv = 59;
                        mvd.x = (byte)(Math.Ceiling(nv));
                    }
                    else if (xMove < 0)
                    {
                        float nv = (v.x + (xMove * f / 9F));
                        if (nv < 0) nv = 0;
                        if (nv > 59) nv = 59;
                        mvd.x = (byte)((blowback) ? Math.Floor(nv) : (Math.Ceiling(nv)));
                    }
                    else
                    {
                        mvd.x = v.x;
                    }
                    if (yMove > 0)
                    {
                        float nv = (v.y + (yMove * f / 9F));
                        if (nv < 0) nv = 0;
                        if (nv > 59) nv = 59;
                        mvd.y = (byte)(Math.Ceiling(nv));
                    }
                    else if (yMove < 0)
                    {
                        float nv = (v.y + (yMove * f / 9F));
                        if (nv < 0) nv = 0;
                        if (nv > 59) nv = 59;
                        mvd.y = (byte)(Math.Floor(nv));

                    }
                    else
                    {
                        mvd.y = v.y;
                    }

                    if (zMove != 0)
                    {
                        float nv = (v.z + (zMove / f));

                        if (nv <= 0 && f < 8) nv = r.Next(2); //bounce
                        else if (nv < 0) nv = 0;

                        if (nv > 39) nv = 39;
                        mvd.z = (byte)Math.Round(nv);
                    }
                    else
                    {
                        mvd.z = v.z;
                    }
                    working[iter] = mvd;
                    iter++;
                }
                voxelFrames[f] = new MagicaVoxelData[working.Length];
                working.ToArray().CopyTo(voxelFrames[f], 0);
            }
            for (int f = 4; f < 9; f++)
            {
                List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxels.Length);
                MagicaVoxelData[] vls = new MagicaVoxelData[voxelFrames[f - 1].Length], working = new MagicaVoxelData[voxelFrames[f - 1].Length]; //.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)
                voxelFrames[f - 1].CopyTo(vls, 0);
                voxelFrames[f - 1].CopyTo(working, 0);

                int[] minX = new int[40];
                int[] maxX = new int[40];
                float[] midX = new float[40];
                for (int level = 0; level < 40; level++)
                {
                    minX[level] = vls.Min(v => v.x * ((v.z != level || v.color == 249 - 96 || v.color == 249 - 128 ||
                        v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color < 249 - 168) ? 100 : 1));
                    maxX[level] = vls.Max(v => v.x * ((v.z != level || v.color == 249 - 96 || v.color == 249 - 128 ||
                        v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color < 249 - 168) ? 0 : 1));
                    midX[level] = (maxX[level] + minX[level]) / 2F;
                }

                int[] minY = new int[40];
                int[] maxY = new int[40];
                float[] midY = new float[40];
                for (int level = 0; level < 40; level++)
                {
                    minY[level] = vls.Min(v => v.y * ((v.z != level || v.color == 249 - 96 || v.color == 249 - 128 ||
                        v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color < 249 - 168) ? 100 : 1));
                    maxY[level] = vls.Max(v => v.y * ((v.z != level || v.color == 249 - 96 || v.color == 249 - 128 ||
                        v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color < 249 - 168) ? 0 : 1));
                    midY[level] = (maxY[level] + minY[level]) / 2F;
                }

                int minZ = vls.Min(v => v.z * ((v.color == 249 - 96 || v.color == 249 - 128 || v.color == 249 - 136 || v.color < 249 - 168) ? 100 : 1));
                int maxZ = vls.Max(v => v.z * ((v.color == 249 - 96 || v.color == 249 - 128 || v.color == 249 - 136 || v.color < 249 - 168) ? 0 : 1));
                float midZ = (maxZ + minZ) / 2F;

                int iter = 0;
                foreach (MagicaVoxelData v in vls)
                {
                    MagicaVoxelData mvd = new MagicaVoxelData();
                    if (v.color == 249 - 64) //flesh
                        mvd.color = (byte)((r.Next(1 + f) == 0) ? 249 - 144 : (r.Next(6) == 0) ? 249 - 152 : 249 - 64); //random transform to guts
                    else if (v.color == 249 - 144) //guts
                        mvd.color = (byte)((r.Next(5) == 0) ? 249 - 152 : 249 - 144); //random transform to orange fire
                    else if (v.color <= 249 - 168) //clear and markers
                        mvd.color = (byte)249 - 168; //clear stays clear
                    else if (v.color == 249 - 80) //lights
                        mvd.color = 249 - 16; //cannon color for broken lights
                    else if (v.color == 249 - 88) //windows
                        mvd.color = (byte)((r.Next(3) == 0) ? 249 - 168 : 249 - 88); //random transform to clear
                    else if (v.color == 249 - 104) //rotors
                        mvd.color = 249 - 56; //helmet color for broken rotors
                    else if (v.color == 249 - 112)
                        mvd.color = 249 - 168; //clear non-active rotors
                    else if (v.color == 249 - 152) //orange fire
                        mvd.color = (byte)((r.Next(8) <= f) ? 249 - 136 : ((r.Next(3) <= 1) ? 249 - 160 : ((r.Next(3) == 0) ? 249 - 136 : 249 - 152))); //random transform to yellow fire or smoke
                    else if (v.color == 249 - 160) //yellow fire
                        mvd.color = (byte)((r.Next(8) <= f) ? 249 - 136 : ((r.Next(3) <= 1) ? 249 - 152 : ((r.Next(4) == 0) ? 249 - 136 : 249 - 160))); //random transform to orange fire or smoke
                    else if (v.color == 249 - 136) //smoke
                        mvd.color = (byte)((r.Next(8) <= f - 1) ? 249 - 168 : 249 - 136); //random transform to clear
                    else
                        mvd.color = (byte)((r.Next(f * 2) <= 2) ? 249 - (152 + ((r.Next(4) == 0) ? 8 : 0)) : v.color); //random transform to orange or yellow fire //(f >= 6) ? 249 - 136 :
                    float xMove = 0, yMove = 0, zMove = 0;
                    if (mvd.color == 249 - 152 || mvd.color == 249 - 160 || mvd.color == 249 - 136)
                    {
                        zMove = f * 0.7F;
                    }
                    else
                    {
                        if (v.x > midX[v.z])// && v.x > maxX - 5)
                            xMove = ((midX[v.z] - r.Next(4) - ((blowback) ? 7 : 0) - (maxX[v.z] - v.x)) * 0.6F * ((v.z - minZ + 1) / (maxZ - minZ + 1F))); //5 -
                        else if (v.x < midX[v.z])// && v.x < minX + 5)
                            xMove = ((0 + (v.x - midX[v.z] + r.Next(4) - ((blowback) ? 6 : 0))) * 0.6F * ((v.z - minZ + 1) / (maxZ - minZ + 1F))); //-5 +
                        if (v.y > midY[v.z])// && v.y > maxY - 5)
                            yMove = ((midY[v.z] - r.Next(4) - (maxY[v.z] - v.y)) * 0.6F * ((v.z - minZ + 1) / (maxZ - minZ + 1F)));//5 -
                        else if (v.y < midY[v.z])// && v.y < minY + 5)
                            yMove = ((0 + (v.y - midY[v.z] + r.Next(4))) * 0.6F * ((v.z - minZ + 1) / (maxZ - minZ + 1F))); //-5 +


                        if (f < 5 && minZ == 0)
                            zMove = (v.z / ((maxZ + 1) * (0.4F))) * (5 - f) * 0.3F;
                        else
                            zMove = (1 - f * 1.1F);
                    }
                    if (xMove > 0)
                    {
                        float nv = (v.x + (xMove / f));
                        if (nv < 0) nv = 0;
                        if (nv > 59) nv = 59;
                        mvd.x = (byte)(Math.Floor(nv));
                    }
                    else if (xMove < 0)
                    {
                        float nv = (v.x + (xMove / f));
                        if (nv < 0) nv = 0;
                        if (nv > 59) nv = 59;
                        mvd.x = (byte)((blowback) ? Math.Floor(nv) : (Math.Ceiling(nv)));
                    }
                    else
                    {
                        mvd.x = v.x;
                    }
                    if (yMove > 0)
                    {
                        float nv = (v.y + (yMove / f));
                        if (nv < 0) nv = 0;
                        if (nv > 59) nv = 59;
                        mvd.y = (byte)(Math.Floor(nv));
                    }
                    else if (yMove < 0)
                    {
                        float nv = (v.y + (yMove / f));
                        if (nv < 0) nv = 0;
                        if (nv > 59) nv = 59;
                        mvd.y = (byte)(Math.Ceiling(nv));
                    }
                    else
                    {
                        mvd.y = v.y;
                    }
                    if (zMove != 0)
                    {
                        float nv = (v.z + (zMove));

                        if (nv <= 0 && f < 8) nv = r.Next(2); //bounce
                        else if (nv < 0) nv = 0;

                        if (nv > 39) nv = 39;
                        mvd.z = (byte)Math.Round(nv);
                    }
                    else
                    {
                        mvd.z = v.z;
                    }
                    working[iter] = mvd;
                    iter++;
                }
                voxelFrames[f] = new MagicaVoxelData[working.Length];
                working.ToArray().CopyTo(voxelFrames[f], 0);
            }
            for (int f = 1; f < 9; f++)
            {

                List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxels.Length);
                int[,] taken = new int[60, 60];
                taken.Fill(-1);
                for (int i = 0; i < voxelFrames[f].Length; i++)
                {
                    // do not store this voxel if it lies out of range of the voxel chunk (30x30x30)
                    if (voxelFrames[f][i].x >= 60 || voxelFrames[f][i].y >= 60 || voxelFrames[f][i].z >= 40)
                    {
                        Console.Write("Voxel out of bounds: " + voxelFrames[f][i].x + ", " + voxelFrames[f][i].y + ", " + voxelFrames[f][i].z);
                        continue;
                    }

                    altered.Add(voxelFrames[f][i]);
                    if (-1 == taken[voxelFrames[f][i].x, voxelFrames[f][i].y] && voxelFrames[f][i].color != 249 - 80 && voxelFrames[f][i].color != 249 - 104 && voxelFrames[f][i].color != 249 - 112
                         && voxelFrames[f][i].color != 249 - 96 && voxelFrames[f][i].color != 249 - 128 && voxelFrames[f][i].color != 249 - 136
                         && voxelFrames[f][i].color != 249 - 152 && voxelFrames[f][i].color != 249 - 160 && voxelFrames[f][i].color > 249 - 168)
                    {
                        MagicaVoxelData vox = new MagicaVoxelData();
                        vox.x = voxelFrames[f][i].x;
                        vox.y = voxelFrames[f][i].y;
                        vox.z = (byte)(0);
                        vox.color = 249 - 96;
                        taken[vox.x, vox.y] = altered.Count();
                        altered.Add(vox);
                    }
                }
                voxelFrames[f] = altered.ToArray();
            }

            MagicaVoxelData[][] frames = new MagicaVoxelData[8][];

            for (int f = 1; f < 9; f++)
            {
                /*                for (int i = 0; i < voxels.Length; i++)
                                {
                                    voxelFrames[f][i].x += 15;
                                    voxelFrames[f][i].y += 15;
                                }*/
                frames[f - 1] = new MagicaVoxelData[voxelFrames[f].Length];
                voxelFrames[f].ToArray().CopyTo(frames[f - 1], 0);
            }
            return frames;
        }

        public static MagicaVoxelData[][] SmokePlume(MagicaVoxelData start, int height, int effectDuration)
        {
            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[16];
            voxelFrames[0] = new List<MagicaVoxelData>(height);
            //for (int i = 0; i < voxels.Length; i++)
            //{
            //    voxelFrames[0][i].x += 20;
            //    voxelFrames[0][i].y += 20;
            //}
            for (int i = 0; i < 1; i++)
            {
                voxelFrames[0].Add(new MagicaVoxelData { x = start.x, y = start.y, z = (byte)i, color = 249 - 160 });
            }
            for (int i = 1; i < 16; i++)
            {
                voxelFrames[i] = new List<MagicaVoxelData>(height);
            }
            for (int f = 1; f < 16 && f < effectDuration && f < height; f++)
            {
                for (int i = 0; i <= f; i++)
                {
                    voxelFrames[f].Add(new MagicaVoxelData { x = (byte)(start.x - i / 2), y = start.y, z = (byte)i, color = (byte)((f < 2 && i == 0) ? 249 - 160 : start.color) });
                }
            }
            for (int f = Math.Min(height, effectDuration); f < 16 && f <= effectDuration && f < height * 2; f++)
            {
                for (int i = height - 1; i >= f - height; i--)
                {
                    voxelFrames[f].Add(new MagicaVoxelData { x = (byte)(start.x - i / 2), y = (byte)(start.y + r.Next(3) - 1), z = (byte)i, color = start.color });
                }
            }
            //for (int f = 1; f < maxFrames+1; f++)
            //{

            //    List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxels.Length);
            //    int[,] taken = new int[60, 60];
            //    taken.Fill(-1);
            //    for (int i = 0; i < voxelFrames[f].Length; i++)
            //    {
            //        // do not store this voxel if it lies out of range of the voxel chunk (30x30x30)
            //        if (voxelFrames[f][i].x >= 60 || voxelFrames[f][i].y >= 60 || voxelFrames[f][i].z >= 60)
            //        {
            //            Console.Write("Voxel out of bounds: " + voxelFrames[f][i].x + ", " + voxelFrames[f][i].y + ", " + voxelFrames[f][i].z);
            //            continue;
            //        }

            //        altered.Add(voxelFrames[f][i]);
            //        if (-1 == taken[voxelFrames[f][i].x, voxelFrames[f][i].y] && voxelFrames[f][i].color != 249 - 80 && voxelFrames[f][i].color != 249 - 104 && voxelFrames[f][i].color != 249 - 112
            //             && voxelFrames[f][i].color != 249 - 96 && voxelFrames[f][i].color != 249 - 128 && voxelFrames[f][i].color != 249 - 136
            //             && voxelFrames[f][i].color != 249 - 152 && voxelFrames[f][i].color != 249 - 160 && voxelFrames[f][i].color >= 249 - 168)
            //        {
            //            MagicaVoxelData vox = new MagicaVoxelData();
            //            vox.x = voxelFrames[f][i].x;
            //            vox.y = voxelFrames[f][i].y;
            //            vox.z = (byte)(0);
            //            vox.color = 249 - 96;
            //            taken[vox.x, vox.y] = altered.Count();
            //            altered.Add(vox);
            //        }
            //    }
            //    voxelFrames[f] = altered.ToArray();
            //}

            MagicaVoxelData[][] frames = new MagicaVoxelData[16][];

            for (int f = 0; f < 16; f++)
            {
                frames[f] = new MagicaVoxelData[voxelFrames[f].Count];
                voxelFrames[f].ToArray().CopyTo(frames[f], 0);
            }
            return frames;
        }

        public static MagicaVoxelData[][] Sparks(MagicaVoxelData start, int sweepDuration)
        {
            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[16];
            voxelFrames[0] = new List<MagicaVoxelData>(10);
            //for (int i = 0; i < voxels.Length; i++)
            //{
            //    voxelFrames[0][i].x += 20;
            //    voxelFrames[0][i].y += 20;
            //}
            for (int i = 0; i < 1; i++)
            {
                voxelFrames[0].Add(new MagicaVoxelData { x = start.x, y = start.y, z = (byte)i, color = 249 - 160 });
            }
            for (int i = 1; i < 16; i++)
            {
                voxelFrames[i] = new List<MagicaVoxelData>(10);
            }
            bool sweepingPositive = true;
            int iter = 0;
            while (iter < 16)
            {
                for (int i = 0; i < sweepDuration && i < 16 && iter < 16; i++, iter++)
                {
                    int rx = (r.Next(3) - r.Next(2));
                    voxelFrames[iter].Add(new MagicaVoxelData { x = (byte)(start.x - rx), y = (byte)(start.y + ((sweepingPositive) ? i : sweepDuration - i) * 2), z = (byte)(0), color = (byte)(249 - 160) });
                    if (r.Next(2) == 0)
                    {
                        voxelFrames[iter].Add(new MagicaVoxelData { x = (byte)(start.x - rx), y = (byte)(start.y + ((sweepingPositive) ? i : sweepDuration - i) * 2), z = (byte)(1), color = (byte)(249 - 160) });
                        voxelFrames[iter].Add(new MagicaVoxelData { x = (byte)(start.x - rx - 1), y = (byte)(start.y + ((sweepingPositive) ? i : sweepDuration - i) * 2), z = (byte)(1), color = (byte)(249 - 160) });
                        voxelFrames[iter].Add(new MagicaVoxelData { x = (byte)(start.x - rx - 1), y = (byte)(start.y + ((sweepingPositive) ? i : sweepDuration - i) * 2), z = (byte)(2), color = (byte)(249 - 160) });
                        if (r.Next(2) == 0)
                        {
                            voxelFrames[iter].Add(new MagicaVoxelData { x = (byte)(start.x - rx - 2), y = (byte)(start.y + ((sweepingPositive) ? i : sweepDuration - i) * 2), z = (byte)(1), color = (byte)(249 - 160) });
                            voxelFrames[iter].Add(new MagicaVoxelData { x = (byte)(start.x - rx - 2), y = (byte)(start.y + ((sweepingPositive) ? i : sweepDuration - i) * 2), z = (byte)(2), color = (byte)(249 - 160) });
                            voxelFrames[iter].Add(new MagicaVoxelData { x = (byte)(start.x - rx - 2), y = (byte)(start.y + ((sweepingPositive) ? i : sweepDuration - i) * 2), z = (byte)(3), color = (byte)(249 - 160) });
                        }
                    }
                }
                sweepingPositive = !sweepingPositive;
            }

            MagicaVoxelData[][] frames = new MagicaVoxelData[16][];

            for (int f = 0; f < 16; f++)
            {
                frames[f] = new MagicaVoxelData[voxelFrames[f].Count];
                voxelFrames[f].ToArray().CopyTo(frames[f], 0);
            }
            return frames;
        }
        public static MagicaVoxelData[][] Burst(MagicaVoxelData start, int maxFrames, bool bigger)
        {
            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[maxFrames];
            voxelFrames[0] = new List<MagicaVoxelData>(10);
            //for (int i = 0; i < voxels.Length; i++)
            //{
            //    voxelFrames[0][i].x += 20;
            //    voxelFrames[0][i].y += 20;
            //}
            for (int i = 0; i < 1; i++)
            {
                voxelFrames[0].Add(new MagicaVoxelData { x = start.x, y = start.y, z = start.z, color = 249 - 160 });
            }
            for (int i = 1; i < maxFrames; i++)
            {
                voxelFrames[i] = new List<MagicaVoxelData>(10);
            }

            int rz = (r.Next(3) - 1);
            for (int i = 1; i < maxFrames; i++)
            {
                if (i <= 1)
                    voxelFrames[i].Add(new MagicaVoxelData { x = (byte)(start.x), y = (byte)(start.y), z = start.z, color = (byte)(249 - 152) });
                if (i > 1)
                {
                    voxelFrames[i].Add(new MagicaVoxelData { x = (byte)(start.x + 1), y = (byte)(start.y), z = (byte)(start.z + rz * (i - 1)), color = (byte)(249 - 152) });
                    voxelFrames[i].Add(new MagicaVoxelData { x = (byte)(start.x + 1), y = (byte)(start.y), z = (byte)(start.z + 1 + rz * (i - 1)), color = (byte)(249 - 152) });
                    voxelFrames[i].Add(new MagicaVoxelData { x = (byte)(start.x + 1), y = (byte)(start.y), z = (byte)(start.z - 1 + rz * (i - 1)), color = (byte)(249 - 152) });
                    voxelFrames[i].Add(new MagicaVoxelData { x = (byte)(start.x + 1), y = (byte)(start.y + 1), z = (byte)(start.z + rz * (i - 1)), color = (byte)(249 - 152) });
                    voxelFrames[i].Add(new MagicaVoxelData { x = (byte)(start.x + 1), y = (byte)(start.y - 1), z = (byte)(start.z + rz * (i - 1)), color = (byte)(249 - 152) });
                    if (bigger)
                    {
                        voxelFrames[i].Add(new MagicaVoxelData { x = (byte)(start.x + 2), y = (byte)(start.y), z = (byte)(start.z + rz * (i - 1)), color = (byte)(249 - 152) });
                        voxelFrames[i].Add(new MagicaVoxelData { x = (byte)(start.x + 2), y = (byte)(start.y), z = (byte)(start.z + 1 + rz * (i - 1)), color = (byte)(249 - 152) });
                        voxelFrames[i].Add(new MagicaVoxelData { x = (byte)(start.x + 2), y = (byte)(start.y), z = (byte)(start.z - 1 + rz * (i - 1)), color = (byte)(249 - 152) });
                        voxelFrames[i].Add(new MagicaVoxelData { x = (byte)(start.x + 2), y = (byte)(start.y + 1), z = (byte)(start.z + rz * (i - 1)), color = (byte)(249 - 152) });
                        voxelFrames[i].Add(new MagicaVoxelData { x = (byte)(start.x + 2), y = (byte)(start.y - 1), z = (byte)(start.z + rz * (i - 1)), color = (byte)(249 - 152) });
                        if (i > 2)
                        {
                            voxelFrames[i].Add(new MagicaVoxelData { x = (byte)(start.x + 3), y = (byte)(start.y), z = (byte)(start.z + 2 + rz * (i - 1)), color = (byte)(249 - 152) });
                            voxelFrames[i].Add(new MagicaVoxelData { x = (byte)(start.x + 3), y = (byte)(start.y), z = (byte)(start.z - 2 + rz * (i - 1)), color = (byte)(249 - 152) });
                            voxelFrames[i].Add(new MagicaVoxelData { x = (byte)(start.x + 3), y = (byte)(start.y + 2), z = (byte)(start.z + rz * (i - 1)), color = (byte)(249 - 152) });
                            voxelFrames[i].Add(new MagicaVoxelData { x = (byte)(start.x + 3), y = (byte)(start.y - 2), z = (byte)(start.z + rz * (i - 1)), color = (byte)(249 - 152) });
                        }
                    }
                }

            }

            MagicaVoxelData[][] frames = new MagicaVoxelData[16][];

            for (int f = 0; f < maxFrames; f++)
            {
                frames[f] = new MagicaVoxelData[voxelFrames[f].Count];
                voxelFrames[f].ToArray().CopyTo(frames[f], 0);
            }
            return frames;
        }

        public static MagicaVoxelData[][] HugeExplosion(MagicaVoxelData[] voxels, int blowback, int maxFrames, int trimLevel)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[maxFrames + 1][];
            voxelFrames[0] = new MagicaVoxelData[voxels.Length];
            voxels.CopyTo(voxelFrames[0], 0);
            //for (int i = 0; i < voxels.Length; i++)
            //{
            //    voxelFrames[0][i].x += 20;
            //    voxelFrames[0][i].y += 20;
            //}
            for (int f = 1; f <= maxFrames; f++)
            {
                List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxels.Length), working = new List<MagicaVoxelData>(voxelFrames[f - 1].Length * 2);
                MagicaVoxelData[] vls = new MagicaVoxelData[voxelFrames[f - 1].Length]; //.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)
                voxelFrames[f - 1].CopyTo(vls, 0);

                int[] minX = new int[30];
                int[] maxX = new int[30];
                float[] midX = new float[30];
                for (int level = 0; level < 30; level++)
                {
                    minX[level] = vls.Min(v => v.x * ((v.z != level || v.color < 249 - 168) ? 100 : 1));
                    maxX[level] = vls.Max(v => v.x * ((v.z != level || v.color < 249 - 168) ? 0 : 1));
                    midX[level] = (maxX[level] + minX[level]) / 2F;
                }

                int[] minY = new int[30];
                int[] maxY = new int[30];
                float[] midY = new float[30];
                for (int level = 0; level < 30; level++)
                {
                    minY[level] = vls.Min(v => v.y * ((v.z != level || v.color < 249 - 168) ? 100 : 1));
                    maxY[level] = vls.Max(v => v.y * ((v.z != level || v.color < 249 - 168) ? 0 : 1));
                    midY[level] = (maxY[level] + minY[level]) / 2F;
                }

                int minZ = vls.Min(v => v.z * ((v.color < 249 - 168) ? 100 : 1));
                int maxZ = vls.Max(v => v.z * ((v.color < 249 - 168) ? 0 : 1));
                float midZ = (maxZ + minZ) / 2F;

                foreach (MagicaVoxelData v in vls)
                {
                    MagicaVoxelData mvd = new MagicaVoxelData();
                    if (v.color == 249 - 64) //flesh
                        mvd.color = (byte)((r.Next(4 + f) == 0) ? 249 - 144 : (r.Next(6) == 0) ? 249 - 152 : 249 - 64); //random transform to guts
                    else if (v.color == 249 - 144) //guts
                        mvd.color = (byte)((r.Next(5) == 0) ? 249 - 152 : 249 - 144); //random transform to orange fire
                    else if (v.color <= 249 - 168) //clear and markers
                        mvd.color = (byte)249 - 168; //clear stays clear
                    else if (v.color == 249 - 80) //lights
                        mvd.color = 249 - 16; //cannon color for broken lights
                    else if (v.color == 249 - 88) //windows
                        mvd.color = (byte)((r.Next(3) == 0) ? 249 - 168 : 249 - 88); //random transform to clear
                    else if (v.color == 249 - 104) //rotors
                        mvd.color = 249 - 56; //helmet color for broken rotors
                    else if (v.color == 249 - 112)
                        mvd.color = 249 - 168; //clear non-active rotors
                    else if (v.color == 249 - 152) //orange fire
                        mvd.color = (byte)((f > maxFrames / 2 && r.Next(maxFrames) <= f + 2) ? 249 - 136 : ((r.Next(3) <= 1) ? 249 - 160 : ((r.Next(3) == 0) ? 249 - 136 : 249 - 152))); //random transform to yellow fire or smoke
                    else if (v.color == 249 - 160) //yellow fire
                        mvd.color = (byte)((f > maxFrames / 2 && r.Next(maxFrames) <= f + 2) ? 249 - 136 : ((r.Next(3) <= 1) ? 249 - 152 : ((r.Next(4) == 0) ? 249 - 136 : 249 - 160))); //random transform to orange fire or smoke
                    else if (v.color == 249 - 136) //smoke
                        mvd.color = (byte)((f > maxFrames * 3 / 5 && r.Next(maxFrames) <= f) ? 249 - 168 : 249 - 136); //random transform to clear
                    else
                        mvd.color = (byte)((r.Next((f + 3) * 2) <= 2) ? 249 - (152 + ((r.Next(4) == 0) ? 8 : 0)) : v.color); //random transform to orange or yellow fire //(f >= 6) ? 249 - 136 :
                    float xMove = 0, yMove = 0, zMove = 0;
                    if (f > maxFrames / 2 && (mvd.color == 249 - 152 || mvd.color == 249 - 160 || mvd.color == 249 - 136))
                    {

                        zMove = f * (16F * 0.2F / maxFrames);
                    }
                    else
                    {
                        if (v.x >= midX[v.z])// && v.x > maxX - 5)
                            xMove = r.Next(6) + 2 + blowback;//((midX[v.z] - r.Next(4) - ((blowback) ? 7 : 0) - (maxX[v.z] - v.x)) * 0.6F * ((v.z - minZ + 1) / (maxZ - minZ + 1F))); //5 -
                        else if (v.x < midX[v.z])// && v.x < minX + 5)
                            xMove = blowback - r.Next(6) - 2;// ((0 + (v.x - midX[v.z] + r.Next(4) - ((blowback) ? 6 : 0))) * 0.6F * ((v.z - minZ + 1) / (maxZ - minZ + 1F))); //-5 +
                        if (v.y >= midY[v.z])// && v.y > maxY - 5)
                            yMove = r.Next(6) + 2;//((midY[v.z] - r.Next(4) - (maxY[v.z] - v.y)) * 0.6F * ((v.z - minZ + 1) / (maxZ - minZ + 1F)));//5 -
                        else if (v.y < midY[v.z])// && v.y < minY + 5)
                            yMove = 0 - r.Next(6) - 2;//((0 + (v.y - midY[v.z] + r.Next(4))) * 0.6F * ((v.z - minZ + 1) / (maxZ - minZ + 1F))); //-5 +

                        if (mvd.color == 249 - 152 || mvd.color == 249 - 160 || mvd.color == 249 - 136)
                            zMove = f * 0.3F;
                        else if (f < (maxFrames - 3) && minZ == 0)
                            zMove = (v.z / ((maxZ + 1) * (0.4F))) * ((maxFrames - 3) - f) * 0.3F;
                        else
                            zMove = (1 - f * 1.1F);
                    }
                    if (xMove > 0)
                    {
                        float nv = (v.x + (xMove / (0.2f * (f + 3)))) - Math.Abs((yMove / (0.5f * (f + 3))));
                        if (nv < 1) nv = 1;
                        if (nv > 58) nv = 58;
                        mvd.x = (byte)((blowback <= 0) ? Math.Floor(nv) : (Math.Ceiling(nv)));
                    }
                    else if (xMove < 0)
                    {
                        float nv = (v.x + (xMove / (0.2f * (f + 3)))) + Math.Abs((yMove / (0.5f * (f + 3))));
                        if (nv < 1) nv = 1;
                        if (nv > 58) nv = 58;
                        mvd.x = (byte)((blowback > 0) ? Math.Floor(nv) : (Math.Ceiling(nv)));
                    }
                    else
                    {
                        if (v.x < 1) mvd.x = 1;
                        if (v.x > 58) mvd.x = 58;
                        else mvd.x = v.x;
                    }
                    if (yMove > 0)
                    {
                        float nv = (v.y + (yMove / (0.2f * (f + 3)))) - Math.Abs((xMove / (0.5f * (f + 3))));
                        if (nv < 1) nv = 1;
                        if (nv > 58) nv = 58;
                        mvd.y = (byte)(Math.Floor(nv));
                    }
                    else if (yMove < 0)
                    {
                        float nv = (v.y + (yMove / (0.2f * (f + 3)))) + Math.Abs((xMove / (0.5f * (f + 3))));
                        if (nv < 1) nv = 1;
                        if (nv > 58) nv = 58;
                        mvd.y = (byte)(Math.Ceiling(nv));
                    }
                    else
                    {
                        mvd.y = v.y;
                    }
                    if (zMove != 0)
                    {
                        float nv = (v.z + (zMove / (0.4f * (f + 3))));

                        if (nv <= 0 && f < maxFrames && !(mvd.color == 249 - 152 || mvd.color == 249 - 160 || mvd.color == 249 - 136)) nv = r.Next(2); //bounce
                        else if (nv < 0) nv = 0;

                        if (nv > 19) nv = 19;
                        mvd.z = (byte)Math.Round(nv);
                    }
                    else
                    {
                        mvd.z = v.z;
                    }
                    working.Add(mvd);
                    if (r.Next(maxFrames) > f + maxFrames / 6 && r.Next(maxFrames) > f + 1) working.AddRange(adjacent(mvd, new int[] { 249 - 152, 249 - 160, 249 - 152, 249 - 160, 249 - 136 }));
                }
                working = working.Where(_ => r.Next(7) < 8 - trimLevel).ToList();
                voxelFrames[f] = new MagicaVoxelData[working.Count];
                working.ToArray().CopyTo(voxelFrames[f], 0);
            }
            //for (int f = 1; f < maxFrames+1; f++)
            //{

            //    List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxels.Length);
            //    int[,] taken = new int[60, 60];
            //    taken.Fill(-1);
            //    for (int i = 0; i < voxelFrames[f].Length; i++)
            //    {
            //        // do not store this voxel if it lies out of range of the voxel chunk (30x30x30)
            //        if (voxelFrames[f][i].x >= 60 || voxelFrames[f][i].y >= 60 || voxelFrames[f][i].z >= 60)
            //        {
            //            Console.Write("Voxel out of bounds: " + voxelFrames[f][i].x + ", " + voxelFrames[f][i].y + ", " + voxelFrames[f][i].z);
            //            continue;
            //        }

            //        altered.Add(voxelFrames[f][i]);
            //        if (-1 == taken[voxelFrames[f][i].x, voxelFrames[f][i].y] && voxelFrames[f][i].color != 249 - 80 && voxelFrames[f][i].color != 249 - 104 && voxelFrames[f][i].color != 249 - 112
            //             && voxelFrames[f][i].color != 249 - 96 && voxelFrames[f][i].color != 249 - 128 && voxelFrames[f][i].color != 249 - 136
            //             && voxelFrames[f][i].color != 249 - 152 && voxelFrames[f][i].color != 249 - 160 && voxelFrames[f][i].color >= 249 - 168)
            //        {
            //            MagicaVoxelData vox = new MagicaVoxelData();
            //            vox.x = voxelFrames[f][i].x;
            //            vox.y = voxelFrames[f][i].y;
            //            vox.z = (byte)(0);
            //            vox.color = 249 - 96;
            //            taken[vox.x, vox.y] = altered.Count();
            //            altered.Add(vox);
            //        }
            //    }
            //    voxelFrames[f] = altered.ToArray();
            //}

            MagicaVoxelData[][] frames = new MagicaVoxelData[maxFrames][];

            for (int f = 1; f <= maxFrames; f++)
            {
                frames[f - 1] = new MagicaVoxelData[voxelFrames[f].Length];
                voxelFrames[f].ToArray().CopyTo(frames[f - 1], 0);
            }
            return frames;
        }


        public delegate MagicaVoxelData[][] AnimationGenerator(MagicaVoxelData[][] parsedFrames, int unit);

        //169 Bomb Drop
        //170 Arc Missile
        //171 Rocket
        //172 Long Cannon
        //173 Cannon
        //174 AA Gun
        //175 Machine Gun
        //176 Handgun
        public static string[] WeaponTypes = { "Handgun", "Machine_Gun", "AA_Gun", "Cannon", "Long_Cannon", "Rocket", "Arc_Missile", "Bomb" };
        public static MagicaVoxelData[][] HandgunAnimation(MagicaVoxelData[][] parsedFrames, int unit)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[parsedFrames.Length][];
            voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
            voxelFrames[parsedFrames.Length - 1] = new MagicaVoxelData[parsedFrames[parsedFrames.Length - 1].Length];
            parsedFrames[0].CopyTo(voxelFrames[0], 0);
            parsedFrames[parsedFrames.Length - 1].CopyTo(voxelFrames[parsedFrames.Length - 1], 0);
            List<MagicaVoxelData> launchers = new List<MagicaVoxelData>(4);
            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length - 2];
            foreach (MagicaVoxelData mvd in voxelFrames[0])
            {
                if (mvd.color == 249 - 176)
                {
                    launchers.Add(mvd);
                }
            }
            for (int f = 0; f < voxelFrames.Length - 2; f++) //going only through the middle
            {
                int currentlyFiring = f % 4;
                extra[f] = new List<MagicaVoxelData>(20);

                //if (f > 0)
                //{
                //    for (int i = 0; i < extra[f-1].Count; i++)
                //    {
                //        extra[f].Add(new MagicaVoxelData { x = (byte)(extra[f-1][i].x + 2), y = (byte)(extra[f-1][i].y + Math.Round(r.NextDouble() * 1.1 - 0.55)),
                //            z = extra[f-1][i].z, color = extra[f-1][i].color });
                //    }
                //}
                if (currentlyFiring < launchers.Count)
                {
                    extra[f].Add(new MagicaVoxelData { x = launchers[currentlyFiring].x, y = launchers[currentlyFiring].y, z = launchers[currentlyFiring].z, color = 249 - 160 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x + 1), y = launchers[currentlyFiring].y, z = launchers[currentlyFiring].z, color = 249 - 160 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x + 2), y = launchers[currentlyFiring].y, z = launchers[currentlyFiring].z, color = 249 - 160 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x + 3), y = launchers[currentlyFiring].y, z = launchers[currentlyFiring].z, color = 249 - 160 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x + 1), y = (byte)(launchers[currentlyFiring].y + 1), z = launchers[currentlyFiring].z, color = 249 - 152 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x + 1), y = (byte)(launchers[currentlyFiring].y - 1), z = launchers[currentlyFiring].z, color = 249 - 152 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x + 1), y = launchers[currentlyFiring].y, z = (byte)(launchers[currentlyFiring].z + 1), color = 249 - 152 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x + 1), y = launchers[currentlyFiring].y, z = (byte)(launchers[currentlyFiring].z - 1), color = 249 - 152 });
                }
                if (currentlyFiring <= launchers.Count && currentlyFiring > 0)
                {
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring - 1].x + 2), y = launchers[currentlyFiring - 1].y, z = launchers[currentlyFiring - 1].z, color = 249 - 152 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring - 1].x + 3), y = launchers[currentlyFiring - 1].y, z = launchers[currentlyFiring - 1].z, color = 249 - 152 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring - 1].x + 4), y = launchers[currentlyFiring - 1].y, z = launchers[currentlyFiring - 1].z, color = 249 - 152 });
                }
            }
            for (int f = 1; f < voxelFrames.Length - 1; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(parsedFrames[f]);
                working.AddRange(extra[f - 1]);
                voxelFrames[f] = working.ToArray();
            }
            return voxelFrames;
        }

        public static MagicaVoxelData[][] HandgunReceiveAnimation(MagicaVoxelData[][] parsedFrames, int strength)
        {
            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[parsedFrames.Length];
            MagicaVoxelData[][] finalFrames = new MagicaVoxelData[parsedFrames.Length][];

            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length];
            MagicaVoxelData[][][] plumes = new MagicaVoxelData[strength * 2][][];
            MagicaVoxelData[][][] bursts = new MagicaVoxelData[strength * 2][][];
            for (int s = 0; s < 2 * strength; s++)
            {
                plumes[s] = SmokePlume(new MagicaVoxelData
                {
                    x = (byte)((s % 2 == 0) ? 45 - r.Next(10) : r.Next(10) + 15),
                    y = (byte)((s % 2 == 0) ? (r.Next(15) + 10) : (50 - r.Next(15))),
                    z = 0,
                    color = 249 - 136
                }, 4, 7);
                bursts[s] = Burst(new MagicaVoxelData
                {
                    x = (byte)(35 + r.Next(3)),
                    y = (byte)(32 - r.Next(4)),
                    z = (byte)(7 + r.Next(3)),
                    color = 249 - 160
                }, 3, s >= strength);
            }
            for (int f = 0; f < voxelFrames.Length; f++)
            {
                extra[f] = new List<MagicaVoxelData>(20);
            }
            int secondMiss = 0, secondHit = 0;
            for (int f = 0; f < voxelFrames.Length - 2; f++)
            {
                int currentlyMissing = f, currentlyHitting = f + 4;
                if (currentlyMissing % 8 < f)
                {
                    currentlyMissing %= 8;
                    secondMiss ^= 1;
                }
                if (currentlyHitting % 8 < f)
                {
                    currentlyHitting %= 8;
                    secondHit ^= 1;
                }
                //if (f > 0)
                //{
                //    for (int i = 0; i < extra[f-1].Count; i++)
                //    {
                //        extra[f].Add(new MagicaVoxelData { x = (byte)(extra[f-1][i].x + 2), y = (byte)(extra[f-1][i].y + Math.Round(r.NextDouble() * 1.1 - 0.55)),
                //            z = extra[f-1][i].z, color = extra[f-1][i].color });
                //    }
                //}
                if (currentlyMissing < strength)
                {
                    for (int p = 0; p < 7 && f + p < parsedFrames.Length; p++)
                    {
                        extra[f + p].AddRange(plumes[currentlyMissing + strength * secondMiss][p]);
                    }
                }
                if (currentlyHitting < strength)
                {
                    for (int b = 0; b < 3 && f + b < parsedFrames.Length; b++)
                    {
                        extra[f + b].AddRange(bursts[currentlyHitting + strength * secondHit][b]);
                    }
                }
            }
            for (int f = 0; f < voxelFrames.Length; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(20);
                working.AddRange(extra[f]);
                finalFrames[f] = working.ToArray();
            }
            return finalFrames;
        }

        public static MagicaVoxelData[][] MachineGunAnimation(MagicaVoxelData[][] parsedFrames, int unit)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[parsedFrames.Length][];
            voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
            voxelFrames[parsedFrames.Length - 1] = new MagicaVoxelData[parsedFrames[parsedFrames.Length - 1].Length];
            parsedFrames[0].CopyTo(voxelFrames[0], 0);
            parsedFrames[parsedFrames.Length - 1].CopyTo(voxelFrames[parsedFrames.Length - 1], 0);
            List<MagicaVoxelData> launchers = new List<MagicaVoxelData>(4);
            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length - 2];
            foreach (MagicaVoxelData mvd in voxelFrames[0])
            {
                if (mvd.color == 249 - 175)
                {
                    launchers.Add(mvd);
                }
            }
            for (int f = 0; f < voxelFrames.Length - 2; f++) //going only through the middle
            {
                int currentlyFiring = f % (launchers.Count + 1);
                extra[f] = new List<MagicaVoxelData>(20);

                //if (f > 0)
                //{
                //    for (int i = 0; i < extra[f-1].Count; i++)
                //    {
                //        extra[f].Add(new MagicaVoxelData { x = (byte)(extra[f-1][i].x + 2), y = (byte)(extra[f-1][i].y + Math.Round(r.NextDouble() * 1.1 - 0.55)),
                //            z = extra[f-1][i].z, color = extra[f-1][i].color });
                //    }
                //}
                if (currentlyFiring % 2 == 0)
                {
                    foreach (MagicaVoxelData launcher in launchers)
                    {
                        if (currentlyFiring != 0)
                        {
                            currentlyFiring = (currentlyFiring + 1) % (launchers.Count + 1);
                            continue;
                        }
                        extra[f].Add(new MagicaVoxelData { x = launcher.x, y = launcher.y, z = launcher.z, color = 249 - 160 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = launcher.y, z = launcher.z, color = 249 - 160 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = launcher.z, color = 249 - 160 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = launcher.y, z = launcher.z, color = 249 - 160 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = launcher.y, z = launcher.z, color = 249 - 160 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = launcher.z, color = 249 - 160 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y - 1), z = launcher.z, color = 249 - 160 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = launcher.y, z = (byte)(launcher.z + 1), color = 249 - 160 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = launcher.y, z = (byte)(launcher.z - 1), color = 249 - 160 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y - 1), z = (byte)(launcher.z - 1), color = 249 - 152 });
                        currentlyFiring = (currentlyFiring + 1) % (launchers.Count + 1);
                    }
                }
                else
                {
                    foreach (MagicaVoxelData launcher in launchers)
                    {
                        if (currentlyFiring < 2 && launchers.Count > 2)
                        {
                            currentlyFiring = (currentlyFiring + 1) % (launchers.Count + 1);
                            continue;
                        }
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = launcher.z, color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = launcher.y, z = launcher.z, color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = launcher.y, z = launcher.z, color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = launcher.y, z = launcher.z, color = 249 - 152 });
                        currentlyFiring = (currentlyFiring + 1) % (launchers.Count + 1);
                    }
                }
            }
            for (int f = 1; f < voxelFrames.Length - 1; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(parsedFrames[f]);
                working.AddRange(extra[f - 1]);
                voxelFrames[f] = working.ToArray();
            }
            return voxelFrames;
        }


        public static MagicaVoxelData[][] MachineGunReceiveAnimation(MagicaVoxelData[][] parsedFrames, int strength)
        {
            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[parsedFrames.Length];
            MagicaVoxelData[][] finalFrames = new MagicaVoxelData[parsedFrames.Length][];

            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length];
            MagicaVoxelData[][][] sparks = new MagicaVoxelData[strength][][];
            for (int s = 0; s < strength; s++)
            {
                sparks[s] = Sparks(new MagicaVoxelData
                {
                    x = (byte)(33 + r.Next(5)),
                    y = (byte)(15 + s * 6),
                    z = 0,
                    color = 249 - 160
                }, 2 * (3 + strength));
            }
            for (int f = 0; f < voxelFrames.Length; f++)
            {
                extra[f] = new List<MagicaVoxelData>(20);
            }
            for (int f = 0; f < voxelFrames.Length - 1; f++)
            {
                //if (f > 0)
                //{
                //    for (int i = 0; i < extra[f-1].Count; i++)
                //    {
                //        extra[f].Add(new MagicaVoxelData { x = (byte)(extra[f-1][i].x + 2), y = (byte)(extra[f-1][i].y + Math.Round(r.NextDouble() * 1.1 - 0.55)),
                //            z = extra[f-1][i].z, color = extra[f-1][i].color });
                //    }
                //}
                for (int sp = 0; sp < sparks.Length; sp++)
                    extra[f + 1].AddRange(sparks[sp][f]);

            }
            for (int f = 0; f < voxelFrames.Length; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(20);
                working.AddRange(extra[f]);
                finalFrames[f] = working.ToArray();
            }
            return finalFrames;
        }


        public static MagicaVoxelData[][] AAGunAnimation(MagicaVoxelData[][] parsedFrames, int unit)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[parsedFrames.Length][];
            voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
            voxelFrames[parsedFrames.Length - 1] = new MagicaVoxelData[parsedFrames[parsedFrames.Length - 1].Length];
            parsedFrames[0].CopyTo(voxelFrames[0], 0);
            parsedFrames[parsedFrames.Length - 1].CopyTo(voxelFrames[parsedFrames.Length - 1], 0);
            List<MagicaVoxelData> launchers = new List<MagicaVoxelData>(4);
            List<Tuple<MagicaVoxelData, double>>[] extra = new List<Tuple<MagicaVoxelData, double>>[voxelFrames.Length - 2];
            foreach (MagicaVoxelData mvd in voxelFrames[0])
            {
                if (mvd.color == 249 - 174)
                {
                    launchers.Add(mvd);
                }
            }
            for (int f = 0; f < voxelFrames.Length - 2; f++) //going only through the middle
            {
                int currentlyFiring = f % 4;
                extra[f] = new List<Tuple<MagicaVoxelData, double>>(30);

                if (f > 0)
                {
                    for (int i = 0; i < extra[f - 1].Count; i++)
                    {
                        extra[f].Add(Tuple.Create(new MagicaVoxelData
                        {
                            x = (byte)(extra[f - 1][i].Item1.x + 2),
                            y = (byte)(extra[f - 1][i].Item1.y + extra[f - 1][i].Item2),
                            z = (byte)(extra[f - 1][i].Item1.z + 2),
                            color = (byte)(249 - 160 + ((r.Next(3) % 2) * 8))
                        }, extra[f - 1][i].Item2));
                    }
                }
                if (currentlyFiring < launchers.Count)
                {
                    double lean = r.NextDouble() * 1.2 - 0.6;
                    extra[f].Add(Tuple.Create(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x), y = (byte)(launchers[currentlyFiring].y), z = (byte)(launchers[currentlyFiring].z), color = (byte)(249 - 160 + ((r.Next(3) % 2) * 8)) }, lean));
                    extra[f].Add(Tuple.Create(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x + 1), y = (byte)(launchers[currentlyFiring].y), z = (byte)(launchers[currentlyFiring].z), color = (byte)(249 - 160 + ((r.Next(3) % 2) * 8)) }, lean));
                    extra[f].Add(Tuple.Create(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x), y = (byte)(launchers[currentlyFiring].y), z = (byte)(launchers[currentlyFiring].z + 1), color = (byte)(249 - 160 + ((r.Next(3) % 2) * 8)) }, lean));
                    extra[f].Add(Tuple.Create(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x + 1), y = (byte)(launchers[currentlyFiring].y), z = (byte)(launchers[currentlyFiring].z + 1), color = (byte)(249 - 160 + ((r.Next(3) % 2) * 8)) }, lean));
                    extra[f].Add(Tuple.Create(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x + 2), y = (byte)(launchers[currentlyFiring].y), z = (byte)(launchers[currentlyFiring].z + 1), color = (byte)(249 - 160 + ((r.Next(3) % 2) * 8)) }, lean));
                    extra[f].Add(Tuple.Create(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x + 1), y = (byte)(launchers[currentlyFiring].y), z = (byte)(launchers[currentlyFiring].z + 2), color = (byte)(249 - 160 + ((r.Next(3) % 2) * 8)) }, lean));
                    extra[f].Add(Tuple.Create(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x + 2), y = (byte)(launchers[currentlyFiring].y), z = (byte)(launchers[currentlyFiring].z + 2), color = (byte)(249 - 160 + ((r.Next(3) % 2) * 8)) }, lean));
                }
                currentlyFiring = currentlyFiring + 2 % 4;
                if (currentlyFiring < launchers.Count)
                {
                    double lean = r.NextDouble() * 1.2 - 0.6;
                    extra[f].Add(Tuple.Create(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x), y = (byte)(launchers[currentlyFiring].y), z = (byte)(launchers[currentlyFiring].z), color = (byte)(249 - 160 + ((r.Next(3) % 2) * 8)) }, lean));
                    extra[f].Add(Tuple.Create(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x + 1), y = (byte)(launchers[currentlyFiring].y), z = (byte)(launchers[currentlyFiring].z), color = (byte)(249 - 160 + ((r.Next(3) % 2) * 8)) }, lean));
                    extra[f].Add(Tuple.Create(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x), y = (byte)(launchers[currentlyFiring].y), z = (byte)(launchers[currentlyFiring].z + 1), color = (byte)(249 - 160 + ((r.Next(3) % 2) * 8)) }, lean));
                    extra[f].Add(Tuple.Create(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x + 1), y = (byte)(launchers[currentlyFiring].y), z = (byte)(launchers[currentlyFiring].z + 1), color = (byte)(249 - 160 + ((r.Next(3) % 2) * 8)) }, lean));
                    extra[f].Add(Tuple.Create(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x + 2), y = (byte)(launchers[currentlyFiring].y), z = (byte)(launchers[currentlyFiring].z + 1), color = (byte)(249 - 160 + ((r.Next(3) % 2) * 8)) }, lean));
                    extra[f].Add(Tuple.Create(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x + 1), y = (byte)(launchers[currentlyFiring].y), z = (byte)(launchers[currentlyFiring].z + 2), color = (byte)(249 - 160 + ((r.Next(3) % 2) * 8)) }, lean));
                    extra[f].Add(Tuple.Create(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x + 2), y = (byte)(launchers[currentlyFiring].y), z = (byte)(launchers[currentlyFiring].z + 2), color = (byte)(249 - 160 + ((r.Next(3) % 2) * 8)) }, lean));

                }
            }
            for (int f = 1; f < voxelFrames.Length - 1; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(parsedFrames[f]);
                working.AddRange(extra[f - 1].Select(t => t.Item1));
                voxelFrames[f] = working.ToArray();
            }
            return voxelFrames;
        }

        public static MagicaVoxelData[][] CannonAnimation(MagicaVoxelData[][] parsedFrames, int unit)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[parsedFrames.Length][];
            voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
            voxelFrames[parsedFrames.Length - 1] = new MagicaVoxelData[parsedFrames[parsedFrames.Length - 1].Length];
            parsedFrames[0].CopyTo(voxelFrames[0], 0);
            parsedFrames[parsedFrames.Length - 1].CopyTo(voxelFrames[parsedFrames.Length - 1], 0);
            List<MagicaVoxelData> launchers = new List<MagicaVoxelData>(4);
            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length - 2];
            foreach (MagicaVoxelData mvd in voxelFrames[0])
            {
                if (mvd.color == 249 - 173)
                {
                    launchers.Add(mvd);
                }
            }
            int maxY = launchers.Max(v => v.y);
            int minY = launchers.Max(v => v.y);
            float midY = (maxY + minY) / 2F;
            List<MagicaVoxelData>[] halves = { launchers.Where(mvd => (mvd.y <= midY)).ToList(), launchers.Where(mvd => (mvd.y > midY)).ToList() };

            for (int f = 0; f < voxelFrames.Length - 2; f++) //going only through the middle
            {
                extra[f] = new List<MagicaVoxelData>(100);

                //if (f > 0)
                //{
                //    for (int i = 0; i < extra[f-1].Count; i++)
                //    {
                //        extra[f].Add(new MagicaVoxelData { x = (byte)(extra[f-1][i].x + 2), y = (byte)(extra[f-1][i].y + Math.Round(r.NextDouble() * 1.1 - 0.55)),
                //            z = extra[f-1][i].z, color = extra[f-1][i].color });
                //    }
                //}
                if (f == 0 || f == 1)
                {
                    foreach (MagicaVoxelData launcher in halves[0])
                    {
                        extra[f].Add(new MagicaVoxelData { x = launcher.x, y = launcher.y, z = launcher.z, color = 249 - 160 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = launcher.y, z = launcher.z, color = 249 - 160 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = launcher.z, color = 249 - 160 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = launcher.y, z = launcher.z, color = 249 - 160 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = launcher.z, color = (byte)(249 - 160 + (r.Next(2) * 8)) });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y - 1), z = launcher.z, color = (byte)(249 - 160 + (r.Next(2) * 8)) });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = launcher.y, z = (byte)(launcher.z + 1), color = (byte)(249 - 160 + (r.Next(2) * 8)) });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = launcher.y, z = (byte)(launcher.z - 1), color = (byte)(249 - 160 + (r.Next(2) * 8)) });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 1), z = launcher.z, color = (byte)(249 - 160 + (r.Next(2) * 8)) });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y - 1), z = launcher.z, color = (byte)(249 - 160 + (r.Next(2) * 8)) });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 1), color = (byte)(249 - 160 + (r.Next(2) * 8)) });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z - 1), color = (byte)(249 - 160 + (r.Next(2) * 8)) });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = launcher.z, color = (byte)(249 - 160 + (r.Next(2) * 8)) });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 2), z = launcher.z, color = (byte)(249 - 160 + (r.Next(2) * 8)) });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = launcher.y, z = (byte)(launcher.z + 2), color = (byte)(249 - 160 + (r.Next(2) * 8)) });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = launcher.y, z = (byte)(launcher.z - 2), color = (byte)(249 - 160 + (r.Next(2) * 8)) });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y - 1), z = (byte)(launcher.z - 1), color = 249 - 152 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y - 1), z = (byte)(launcher.z - 1), color = 249 - 152 });


                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 1), z = (byte)(launcher.z - 2), color = 249 - 152 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z - 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 2), z = (byte)(launcher.z - 1), color = 249 - 152 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z - 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 2), z = (byte)(launcher.z - 2), color = 249 - 152 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 3), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 3), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 2), z = (byte)(launcher.z - 3), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 2), z = (byte)(launcher.z - 3), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 3), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 3), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 3), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 1), z = (byte)(launcher.z - 3), color = 249 - 152 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 3), z = (byte)(launcher.z + 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 3), z = (byte)(launcher.z + 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 3), z = (byte)(launcher.z - 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 3), z = (byte)(launcher.z - 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 3), z = (byte)(launcher.z + 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 3), z = (byte)(launcher.z + 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 3), z = (byte)(launcher.z - 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 3), z = (byte)(launcher.z - 1), color = 249 - 152 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 3), z = (byte)(launcher.z + 3), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 3), z = (byte)(launcher.z + 3), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 3), z = (byte)(launcher.z - 3), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 3), z = (byte)(launcher.z - 3), color = 249 - 152 });


                        extra[f] = extra[f].Where(v => r.Next(8) > 1).ToList();

                    }
                }
                else if (f == 1 || f == 2)
                {

                    foreach (MagicaVoxelData launcher in halves[1])
                    {

                        extra[f].Add(new MagicaVoxelData { x = launcher.x, y = launcher.y, z = launcher.z, color = 249 - 160 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = launcher.y, z = launcher.z, color = 249 - 160 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = launcher.z, color = 249 - 160 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = launcher.y, z = launcher.z, color = 249 - 160 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = launcher.z, color = (byte)(249 - 160 + (r.Next(2) * 8)) });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y - 1), z = launcher.z, color = (byte)(249 - 160 + (r.Next(2) * 8)) });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = launcher.y, z = (byte)(launcher.z + 1), color = (byte)(249 - 160 + (r.Next(2) * 8)) });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = launcher.y, z = (byte)(launcher.z - 1), color = (byte)(249 - 160 + (r.Next(2) * 8)) });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 1), z = launcher.z, color = (byte)(249 - 160 + (r.Next(2) * 8)) });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y - 1), z = launcher.z, color = (byte)(249 - 160 + (r.Next(2) * 8)) });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 1), color = (byte)(249 - 160 + (r.Next(2) * 8)) });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z - 1), color = (byte)(249 - 160 + (r.Next(2) * 8)) });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = launcher.z, color = (byte)(249 - 160 + (r.Next(2) * 8)) });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 2), z = launcher.z, color = (byte)(249 - 160 + (r.Next(2) * 8)) });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = launcher.y, z = (byte)(launcher.z + 2), color = (byte)(249 - 160 + (r.Next(2) * 8)) });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = launcher.y, z = (byte)(launcher.z - 2), color = (byte)(249 - 160 + (r.Next(2) * 8)) });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y - 1), z = (byte)(launcher.z - 1), color = 249 - 152 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y - 1), z = (byte)(launcher.z - 1), color = 249 - 152 });


                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 1), z = (byte)(launcher.z - 2), color = 249 - 152 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z - 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 2), z = (byte)(launcher.z - 1), color = 249 - 152 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z - 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 2), z = (byte)(launcher.z - 2), color = 249 - 152 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 3), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 3), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 2), z = (byte)(launcher.z - 3), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 2), z = (byte)(launcher.z - 3), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 3), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 3), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 3), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 1), z = (byte)(launcher.z - 3), color = 249 - 152 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 3), z = (byte)(launcher.z + 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 3), z = (byte)(launcher.z + 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 3), z = (byte)(launcher.z - 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 3), z = (byte)(launcher.z - 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 3), z = (byte)(launcher.z + 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 3), z = (byte)(launcher.z + 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 3), z = (byte)(launcher.z - 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 3), z = (byte)(launcher.z - 1), color = 249 - 152 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 3), z = (byte)(launcher.z + 3), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 3), z = (byte)(launcher.z + 3), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 3), z = (byte)(launcher.z - 3), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 3), z = (byte)(launcher.z - 3), color = 249 - 152 });


                        extra[f] = extra[f].Where(v => r.Next(8) > 1).ToList();

                    }
                }
                else if (f == 3)
                {
                    foreach (MagicaVoxelData launcher in launchers)
                    {
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = launcher.y, z = (byte)(launcher.z + 1), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 1), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 1), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 1), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 1), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 1), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = launcher.y, z = (byte)(launcher.z + 1), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 1), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 1), color = 249 - 136 });
                    }
                    extra[f] = extra[f].Where(v => r.Next(6) > 1).ToList();

                }
                else if (f == 4)
                {
                    foreach (MagicaVoxelData launcher in launchers)
                    {
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 2), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 2), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 2), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = launcher.y, z = (byte)(launcher.z + 2), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 2), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 2), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = launcher.y, z = (byte)(launcher.z + 3), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 3), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 3), color = 249 - 136 });
                    }
                    extra[f] = extra[f].Where(v => r.Next(6) > 2).ToList();

                }
                else if (f == 5)
                {
                    foreach (MagicaVoxelData launcher in launchers)
                    {
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 3), z = (byte)(launcher.z + 2), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y - 3), z = (byte)(launcher.z + 2), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 2), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 2), color = 249 - 136 });
                    }
                    extra[f] = extra[f].Where(v => r.Next(6) > 3).ToList();

                }
                else if (f == 6)
                {
                    foreach (MagicaVoxelData launcher in launchers)
                    {
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 3), z = (byte)(launcher.z + 3), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y - 3), z = (byte)(launcher.z + 3), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 1 + r.Next(3)), z = (byte)(launcher.z + 4), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y - 1 - r.Next(3)), z = (byte)(launcher.z + 4), color = 249 - 136 });
                    }
                    extra[f] = extra[f].Where(v => r.Next(6) > 4).ToList();

                }
            }
            for (int f = 1; f < voxelFrames.Length - 1; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(parsedFrames[f]);
                if (f == 2 || f == 3)
                {
                    for (int i = 0; i < working.Count; i++)
                    {
                        working[i] = new MagicaVoxelData { x = (byte)(working[i].x - 1), y = working[i].y, z = working[i].z, color = working[i].color };
                    }
                }
                working.AddRange(extra[f - 1]);
                voxelFrames[f] = working.ToArray();
            }
            return voxelFrames;
        }

        public static MagicaVoxelData[][] CannonReceiveAnimation(MagicaVoxelData[][] parsedFrames, int strength)
        {
            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[parsedFrames.Length];
            MagicaVoxelData[][] finalFrames = new MagicaVoxelData[parsedFrames.Length][];

            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length];
            MagicaVoxelData[][][] explosions = new MagicaVoxelData[strength][][];
            for (int s = 0; s < strength; s++)
            {
                explosions[s] = HugeExplosion(randomFill(37, 28 + s % 2, 8 + s / 2, 2, 2, 2, new int[] { 249 - 152, 249 - 152, 249 - 160, 249 - 160, 249 - 136 }).ToArray(), 3, 6 + strength, 2);
            }
            for (int f = 0; f < voxelFrames.Length; f++)
            {
                extra[f] = new List<MagicaVoxelData>(50);
            }
            for (int f = 0; f < voxelFrames.Length - 1; f++)
            {
                //if (f > 0)
                //{
                //    for (int i = 0; i < extra[f-1].Count; i++)
                //    {
                //        extra[f].Add(new MagicaVoxelData { x = (byte)(extra[f-1][i].x + 2), y = (byte)(extra[f-1][i].y + Math.Round(r.NextDouble() * 1.1 - 0.55)),
                //            z = extra[f-1][i].z, color = extra[f-1][i].color });
                //    }
                //}
                if (f == 3)
                {
                    for (int i = 0; i < 6 + strength; i++)
                    {
                        for (int sp = 0; sp < explosions.Length; sp++)
                            extra[f + i].AddRange(explosions[sp][i]);
                    }
                }
            }
            for (int f = 0; f < voxelFrames.Length; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(50);
                working.AddRange(extra[f]);
                finalFrames[f] = working.ToArray();
            }
            return finalFrames;
        }

        public static MagicaVoxelData[][] LongCannonAnimation(MagicaVoxelData[][] parsedFrames, int unit)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[parsedFrames.Length][];
            voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
            voxelFrames[parsedFrames.Length - 1] = new MagicaVoxelData[parsedFrames[parsedFrames.Length - 1].Length];
            parsedFrames[0].CopyTo(voxelFrames[0], 0);
            parsedFrames[parsedFrames.Length - 1].CopyTo(voxelFrames[parsedFrames.Length - 1], 0);
            List<MagicaVoxelData> launchers = new List<MagicaVoxelData>(4);
            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length - 2];
            foreach (MagicaVoxelData mvd in voxelFrames[0])
            {
                if (mvd.color == 249 - 172)
                {
                    launchers.Add(mvd);
                }
            }
            int maxY = launchers.Max(v => v.y);
            int minY = launchers.Max(v => v.y);
            float midY = (maxY + minY) / 2F;
            List<MagicaVoxelData>[] halves = { launchers.Where(mvd => (mvd.y <= midY)).ToList(), launchers.Where(mvd => (mvd.y > midY)).ToList() };

            for (int f = 0; f < voxelFrames.Length - 2; f++) //going only through the middle
            {
                extra[f] = new List<MagicaVoxelData>(100);

                //if (f > 0)
                //{
                //    for (int i = 0; i < extra[f-1].Count; i++)
                //    {
                //        extra[f].Add(new MagicaVoxelData { x = (byte)(extra[f-1][i].x + 2), y = (byte)(extra[f-1][i].y + Math.Round(r.NextDouble() * 1.1 - 0.55)),
                //            z = extra[f-1][i].z, color = extra[f-1][i].color });
                //    }
                //}
                if (f == 0 || f == 1 || f == 2)
                {
                    foreach (MagicaVoxelData launcher in launchers)
                    {
                        extra[f].Add(new MagicaVoxelData { x = launcher.x, y = launcher.y, z = launcher.z, color = 249 - 160 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = launcher.y, z = launcher.z, color = 249 - 160 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = launcher.z, color = 249 - 160 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = launcher.y, z = launcher.z, color = 249 - 160 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 1), color = (byte)(249 - 160 + (r.Next(2) * 8)) });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 1), color = (byte)(249 - 160 + (r.Next(2) * 8)) });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = launcher.y, z = (byte)(launcher.z + 2), color = (byte)(249 - 160 + (r.Next(2) * 8)) });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = launcher.y, z = (byte)(launcher.z), color = (byte)(249 - 160 + (r.Next(2) * 8)) });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 2), color = (byte)(249 - 160 + (r.Next(2) * 8)) });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 2), color = (byte)(249 - 160 + (r.Next(2) * 8)) });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 3), color = (byte)(249 - 160 + (r.Next(2) * 8)) });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 1), color = (byte)(249 - 160 + (r.Next(2) * 8)) });


                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 3), color = (byte)(249 - 160 + (r.Next(2) * 8)) });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 3), color = (byte)(249 - 160 + (r.Next(2) * 8)) });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = launcher.y, z = (byte)(launcher.z + 4), color = (byte)(249 - 160 + (r.Next(2) * 8)) });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = launcher.y, z = (byte)(launcher.z + 2), color = (byte)(249 - 160 + (r.Next(2) * 8)) });


                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 0), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y - 1), z = (byte)(launcher.z - 0), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 0), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y - 1), z = (byte)(launcher.z - 0), color = 249 - 152 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 3), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 3), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 0), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 0), color = 249 - 152 });


                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 4), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 4), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 0), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 0), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 3), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 3), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 1), color = 249 - 152 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z - 0), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 2), z = (byte)(launcher.z - 0), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z - 0), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 2), z = (byte)(launcher.z - 0), color = 249 - 152 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 4), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 4), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 0), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 0), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 3), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 3), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 1), color = 249 - 152 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 5), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 5), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 4), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 4), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 2), color = 249 - 152 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 4), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 4), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 2), z = (byte)(launcher.z - 0), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 2), z = (byte)(launcher.z - 0), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 3), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 3), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 1), color = 249 - 152 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 5), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 5), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 1), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 4), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 4), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 2), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 2), color = 249 - 152 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 4), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 4), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 5), color = 249 - 152 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 5), color = 249 - 152 });

                        if (f < 2)
                        {
                            extra[f] = extra[f].Where(v => r.Next(8) > 0).ToList();
                        }
                        else
                        {
                            extra[f] = extra[f].Where(v => r.Next(8) > 0).Select(v => new MagicaVoxelData { x = (byte)(v.x - 1), y = (byte)(v.y), z = (byte)(v.z), color = v.color }).ToList();
                        }
                    }
                }
                else if (f == 3)
                {
                    foreach (MagicaVoxelData launcher in launchers)
                    {
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = launcher.y, z = (byte)(launcher.z + 1), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 1), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 1), color = 249 - 136 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 1), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 1), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 1), color = 249 - 136 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = launcher.y, z = (byte)(launcher.z + 2), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 2), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 2), color = 249 - 136 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 2), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 2), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 2), color = 249 - 136 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 3), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 3), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 3), color = 249 - 136 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = launcher.y, z = (byte)(launcher.z + 2), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 2), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 2), color = 249 - 136 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = launcher.y, z = (byte)(launcher.z + 3), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 3), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 3), color = 249 - 136 });
                    }
                }
                else if (f == 4)
                {
                    foreach (MagicaVoxelData launcher in launchers)
                    {
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 2), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 2), color = 249 - 136 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 3), z = (byte)(launcher.z + 2), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y - 3), z = (byte)(launcher.z + 2), color = 249 - 136 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 3), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 3), color = 249 - 136 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 3), z = (byte)(launcher.z + 3), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y - 3), z = (byte)(launcher.z + 3), color = 249 - 136 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 3), z = (byte)(launcher.z + 3), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 3), z = (byte)(launcher.z + 3), color = 249 - 136 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 4), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 4), color = 249 - 136 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 3), z = (byte)(launcher.z + 4), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 3), z = (byte)(launcher.z + 4), color = 249 - 136 });
                    }
                }
                else if (f == 5)
                {
                    foreach (MagicaVoxelData launcher in launchers)
                    {
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 3 + (r.Next(3) - 1)), z = (byte)(launcher.z + 3), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y - 3 + (r.Next(3) - 1)), z = (byte)(launcher.z + 3), color = 249 - 136 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 3 + (r.Next(3) - 1)), z = (byte)(launcher.z + 4), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y - 3 + (r.Next(3) - 1)), z = (byte)(launcher.z + 4), color = 249 - 136 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 3 + (r.Next(3) - 1)), z = (byte)(launcher.z + 5), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 3 + (r.Next(3) - 1)), z = (byte)(launcher.z + 5), color = 249 - 136 });
                    }
                }
                else if (f == 6)
                {
                    foreach (MagicaVoxelData launcher in launchers)
                    {
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 3 + (r.Next(3) - 1)), z = (byte)(launcher.z + 5), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 3 + (r.Next(3) - 1)), z = (byte)(launcher.z + 5), color = 249 - 136 });

                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 3 + (r.Next(3) - 1)), z = (byte)(launcher.z + 6), color = 249 - 136 });
                        extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 3 + (r.Next(3) - 1)), z = (byte)(launcher.z + 6), color = 249 - 136 });
                    }
                }
            }
            for (int f = 1; f < voxelFrames.Length - 1; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(parsedFrames[f]);
                if (f == 2 || f == 4)
                {
                    for (int i = 0; i < working.Count; i++)
                    {
                        working[i] = new MagicaVoxelData { x = (byte)(working[i].x - 1), y = working[i].y, z = working[i].z, color = working[i].color };
                    }
                }
                else if (f == 3)
                {
                    for (int i = 0; i < working.Count; i++)
                    {
                        working[i] = new MagicaVoxelData { x = (byte)(working[i].x - 2), y = working[i].y, z = working[i].z, color = working[i].color };
                    }
                }
                working.AddRange(extra[f - 1]);
                voxelFrames[f] = working.ToArray();
            }
            return voxelFrames;
        }

        public static MagicaVoxelData[][] LongCannonReceiveAnimation(MagicaVoxelData[][] parsedFrames, int strength)
        {
            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[parsedFrames.Length];
            MagicaVoxelData[][] finalFrames = new MagicaVoxelData[parsedFrames.Length][];

            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length];
            MagicaVoxelData[][][] explosions = new MagicaVoxelData[strength][][];
            for (int s = 0; s < strength; s++)
            {
                explosions[s] = HugeExplosion(randomFill(38, 28 + s % 2, 0, 2, 2, 4, new int[] { 249 - 136, 249 - 152, 249 - 136, 249 - 152, 249 - 160 }).ToArray(), 0, 5 + strength, 2);
            }
            for (int f = 0; f < voxelFrames.Length; f++)
            {
                extra[f] = new List<MagicaVoxelData>(20);
            }
            for (int f = 0; f < voxelFrames.Length - 1; f++)
            {
                //if (f > 0)
                //{
                //    for (int i = 0; i < extra[f-1].Count; i++)
                //    {
                //        extra[f].Add(new MagicaVoxelData { x = (byte)(extra[f-1][i].x + 2), y = (byte)(extra[f-1][i].y + Math.Round(r.NextDouble() * 1.1 - 0.55)),
                //            z = extra[f-1][i].z, color = extra[f-1][i].color });
                //    }
                //}
                if (f == 4)
                {
                    for (int i = 0; i < 5 + strength; i++)
                    {
                        for (int sp = 0; sp < explosions.Length; sp++)
                            extra[f + i].AddRange(explosions[sp][i]);
                    }
                }

            }
            for (int f = 0; f < voxelFrames.Length; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(20);
                working.AddRange(extra[f]);
                finalFrames[f] = working.ToArray();
            }
            return finalFrames;
        }
        public static MagicaVoxelData[][] RocketAnimation(MagicaVoxelData[][] parsedFrames, int unit)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[parsedFrames.Length][];
            voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
            voxelFrames[parsedFrames.Length - 1] = new MagicaVoxelData[parsedFrames[parsedFrames.Length - 1].Length];
            parsedFrames[0].CopyTo(voxelFrames[0], 0);
            parsedFrames[parsedFrames.Length - 1].CopyTo(voxelFrames[parsedFrames.Length - 1], 0);
            List<MagicaVoxelData> launchers = new List<MagicaVoxelData>(4), trails = new List<MagicaVoxelData>(4);
            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length - 2], missile = new List<MagicaVoxelData>[voxelFrames.Length - 2];
            foreach (MagicaVoxelData mvd in voxelFrames[0])
            {
                if (mvd.color == 249 - 171)
                {
                    launchers.Add(mvd);
                }
                else if (mvd.color == 249 - 170)
                {
                    trails.Add(mvd);
                }
            }
            int maxY = launchers.Max(v => v.y);
            int minY = launchers.Max(v => v.y);
            float midY = (maxY + minY) / 2F;
            MagicaVoxelData launcher = launchers.RandomElement();
            MagicaVoxelData trail = trails.RandomElement();
            for (int f = 0; f < voxelFrames.Length - 2; f++) //going only through the middle
            {
                extra[f] = new List<MagicaVoxelData>(20);
                missile[f] = new List<MagicaVoxelData>(20);

                if (f > 1)
                {
                    for (int i = 0; i < missile[f - 1].Count; i++)
                    {
                        missile[f].Add(new MagicaVoxelData
                        {
                            x = (byte)(missile[f - 1][i].x + 4),
                            y = (byte)(missile[f - 1][i].y),
                            z = missile[f - 1][i].z,
                            color = missile[f - 1][i].color
                        });
                    }
                }
                if (f == 0)
                {
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = launcher.y, z = (byte)(launcher.z), color = 249 - 40 });
                }
                if (f == 1)
                {
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z), color = 249 - 40 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = launcher.y, z = (byte)(launcher.z), color = 249 - 72 });

                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 1), y = trail.y, z = (byte)(trail.z), color = 249 - 160 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 1), y = (byte)(trail.y + 1), z = (byte)(trail.z), color = 249 - 152 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 1), y = (byte)(trail.y - 1), z = (byte)(trail.z), color = 249 - 152 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 2), y = trail.y, z = (byte)(trail.z), color = 249 - 160 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 1), y = trail.y, z = (byte)(trail.z + 1), color = 249 - 152 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 1), y = trail.y, z = (byte)(trail.z - 1), color = 249 - 152 });


                }
                else if (f == 2)
                {

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = launcher.y, z = (byte)(launcher.z), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = launcher.y, z = (byte)(launcher.z), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = launcher.y, z = (byte)(launcher.z), color = 249 - 72 });

                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 1), y = trail.y, z = (byte)(trail.z), color = 249 - 160 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 1), y = (byte)(trail.y + 1), z = (byte)(trail.z), color = 249 - 152 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 1), y = (byte)(trail.y - 1), z = (byte)(trail.z), color = 249 - 152 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 2), y = trail.y, z = (byte)(trail.z), color = 249 - 160 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 2), y = (byte)(trail.y + 1), z = (byte)(trail.z), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 2), y = (byte)(trail.y - 1), z = (byte)(trail.z), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 3), y = trail.y, z = (byte)(trail.z), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 3), y = (byte)(trail.y + 1), z = (byte)(trail.z), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 3), y = (byte)(trail.y - 1), z = (byte)(trail.z), color = 249 - 136 });

                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 1), y = trail.y, z = (byte)(trail.z + 1), color = 249 - 152 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 1), y = (byte)(trail.y + 1), z = (byte)(trail.z + 1), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 1), y = (byte)(trail.y - 1), z = (byte)(trail.z + 1), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 2), y = trail.y, z = (byte)(trail.z + 1), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 2), y = (byte)(trail.y + 1), z = (byte)(trail.z + 1), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 2), y = (byte)(trail.y - 1), z = (byte)(trail.z + 1), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 3), y = trail.y, z = (byte)(trail.z + 1), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 3), y = (byte)(trail.y + 1), z = (byte)(trail.z + 1), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 3), y = (byte)(trail.y - 1), z = (byte)(trail.z + 1), color = 249 - 136 });

                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 1), y = trail.y, z = (byte)(trail.z - 1), color = 249 - 152 });


                }
                else if (f == 3)
                {
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = launcher.y, z = (byte)(launcher.z), color = 249 - 160 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = launcher.y, z = (byte)(launcher.z), color = 249 - 160 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z), color = 249 - 160 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 1), z = (byte)(launcher.z), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y), z = (byte)(launcher.z + 1), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = 249 - 152 });

                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 2), y = trail.y, z = (byte)(trail.z), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 2), y = (byte)(trail.y + 1), z = (byte)(trail.z), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 2), y = (byte)(trail.y - 1), z = (byte)(trail.z), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 3), y = trail.y, z = (byte)(trail.z), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 3), y = (byte)(trail.y + 1), z = (byte)(trail.z), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 3), y = (byte)(trail.y - 1), z = (byte)(trail.z), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 4), y = trail.y, z = (byte)(trail.z), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 4), y = (byte)(trail.y + 1), z = (byte)(trail.z), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 4), y = (byte)(trail.y - 1), z = (byte)(trail.z), color = 249 - 136 });

                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 2), y = trail.y, z = (byte)(trail.z + 1), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 2), y = (byte)(trail.y + 1), z = (byte)(trail.z + 1), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 2), y = (byte)(trail.y - 1), z = (byte)(trail.z + 1), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 3), y = trail.y, z = (byte)(trail.z + 1), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 3), y = (byte)(trail.y + 1), z = (byte)(trail.z + 1), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 3), y = (byte)(trail.y - 1), z = (byte)(trail.z + 1), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 4), y = trail.y, z = (byte)(trail.z + 1), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 4), y = (byte)(trail.y + 1), z = (byte)(trail.z + 1), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 4), y = (byte)(trail.y - 1), z = (byte)(trail.z + 1), color = 249 - 136 });


                    extra[f] = extra[f].Where(v => r.Next(7) > 0).ToList();

                }
                else if (f == 4)
                {
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 3), y = (byte)(trail.y + 1 + (r.Next(3) - 1)), z = (byte)(trail.z + 0), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 3), y = (byte)(trail.y - 1 + (r.Next(3) - 1)), z = (byte)(trail.z + 0), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 3), y = (byte)(trail.y + 1 + (r.Next(3) - 1)), z = (byte)(trail.z + 1), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 3), y = (byte)(trail.y - 1 + (r.Next(3) - 1)), z = (byte)(trail.z + 1), color = 249 - 136 });

                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 4), y = (byte)(trail.y + 1 + (r.Next(3) - 1)), z = (byte)(trail.z + 0), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 4), y = (byte)(trail.y - 1 + (r.Next(3) - 1)), z = (byte)(trail.z + 0), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 4), y = (byte)(trail.y + 1 + (r.Next(3) - 1)), z = (byte)(trail.z + 1), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 4), y = (byte)(trail.y - 1 + (r.Next(3) - 1)), z = (byte)(trail.z + 1), color = 249 - 136 });
                    extra[f] = extra[f].Where(v => r.Next(7) > 0).ToList();

                }
                else if (f == 5)
                {
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 4), y = (byte)(trail.y + 2 + (r.Next(3) - 1)), z = (byte)(trail.z + 1), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 4), y = (byte)(trail.y - 2 + (r.Next(3) - 1)), z = (byte)(trail.z + 1), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 5), y = (byte)(trail.y + 2 + (r.Next(3) - 1)), z = (byte)(trail.z + 1), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 5), y = (byte)(trail.y - 2 + (r.Next(3) - 1)), z = (byte)(trail.z + 1), color = 249 - 136 });
                }
                else if (f == 5)
                {
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 5), y = (byte)(trail.y + 2 + (r.Next(3) - 1)), z = (byte)(trail.z + 2), color = 249 - 136 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(trail.x - 5), y = (byte)(trail.y - 2 + (r.Next(3) - 1)), z = (byte)(trail.z + 2), color = 249 - 136 });
                }
            }
            for (int f = 1; f < voxelFrames.Length - 1; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(parsedFrames[f]);
                working.AddRange(missile[f - 1]);
                working.AddRange(extra[f - 1]);
                voxelFrames[f] = working.ToArray();
            }
            return voxelFrames;
        }

        public static MagicaVoxelData[][] RocketReceiveAnimation(MagicaVoxelData[][] parsedFrames, int distance)
        {
            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[parsedFrames.Length];
            MagicaVoxelData[][] finalFrames = new MagicaVoxelData[parsedFrames.Length][];

            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length];
            MagicaVoxelData[][] explosion = HugeExplosion(randomFill(36, 25, 10, 4, 2, 2, new int[] { 249 - 136, 249 - 152, 249 - 160, 249 - 152, 249 - 160 }).ToArray(), -2, 7, 2);

            for (int f = 0; f < voxelFrames.Length; f++)
            {
                extra[f] = new List<MagicaVoxelData>(20);
            }
            for (int f = 0; f < voxelFrames.Length - 1; f++)
            {
                //if (f > 0)
                //{
                //    for (int i = 0; i < extra[f-1].Count; i++)
                //    {
                //        extra[f].Add(new MagicaVoxelData { x = (byte)(extra[f-1][i].x + 2), y = (byte)(extra[f-1][i].y + Math.Round(r.NextDouble() * 1.1 - 0.55)),
                //            z = extra[f-1][i].z, color = extra[f-1][i].color });
                //    }
                //}
                if (f == 3 + distance)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        extra[f + i].AddRange(explosion[i]);
                    }
                }
            }
            for (int f = 0; f < voxelFrames.Length; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(20);
                working.AddRange(extra[f]);
                finalFrames[f] = working.ToArray();
            }
            return finalFrames;
        }
        private static List<MagicaVoxelData> generateCone(MagicaVoxelData start, int segments, int color)
        {
            List<MagicaVoxelData> cone = new List<MagicaVoxelData>(40);
            for (int x = 0; x < segments; x++)
            {
                for (float y = 0; y <= x * 0.75f; y++)
                {
                    for (float z = 0; z <= x * 0.75f; z++)
                    {
                        if (Math.Floor(y) == y && Math.Floor(z) == z)
                        {
                            cone.Add(new MagicaVoxelData { x = (byte)(start.x - x), y = (byte)(start.y + 0 - y), z = (byte)(start.z - z), color = (byte)color });
                            cone.Add(new MagicaVoxelData { x = (byte)(start.x - x), y = (byte)(start.y + 1 + y), z = (byte)(start.z - z), color = (byte)color });
                            cone.Add(new MagicaVoxelData { x = (byte)(start.x - x - 1), y = (byte)(start.y + 0 - y), z = (byte)(start.z - z), color = (byte)color });
                            cone.Add(new MagicaVoxelData { x = (byte)(start.x - x - 1), y = (byte)(start.y + 1 + y), z = (byte)(start.z - z), color = (byte)color });
                            cone.Add(new MagicaVoxelData { x = (byte)(start.x - x - 1), y = (byte)(start.y + 0 - y), z = (byte)(start.z - z - 1), color = (byte)color });
                            cone.Add(new MagicaVoxelData { x = (byte)(start.x - x - 1), y = (byte)(start.y + 1 + y), z = (byte)(start.z - z - 1), color = (byte)color });
                        }
                    }
                }
            }
            return cone;

        }

        private static List<MagicaVoxelData> generateDownwardCone(MagicaVoxelData start, int segments, int color)
        {
            List<MagicaVoxelData> cone = new List<MagicaVoxelData>(40);
            for (int x = 0; x < segments; x++)
            {
                for (int y = 0; y <= x; y++)
                {
                    for (int z = 0; z <= x; z++)
                    {

                        cone.Add(new MagicaVoxelData { x = (byte)(start.x + x), y = (byte)(start.y + 0 - y), z = (byte)(start.z + z), color = (byte)color });
                        cone.Add(new MagicaVoxelData { x = (byte)(start.x + x), y = (byte)(start.y + 1 + y), z = (byte)(start.z + z), color = (byte)color });
                        cone.Add(new MagicaVoxelData { x = (byte)(start.x + x + 1), y = (byte)(start.y + 0 - y), z = (byte)(start.z + z), color = (byte)color });
                        cone.Add(new MagicaVoxelData { x = (byte)(start.x + x + 1), y = (byte)(start.y + 1 + y), z = (byte)(start.z + z), color = (byte)color });
                        cone.Add(new MagicaVoxelData { x = (byte)(start.x + x + 1), y = (byte)(start.y + 0 - y), z = (byte)(start.z + z + 1), color = (byte)color });
                        cone.Add(new MagicaVoxelData { x = (byte)(start.x + x + 1), y = (byte)(start.y + 1 + y), z = (byte)(start.z + z + 1), color = (byte)color });
                    }
                }
            }
            return cone;

        }
        public static MagicaVoxelData[][] ArcMissileAnimation(MagicaVoxelData[][] parsedFrames, int unit)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[parsedFrames.Length][];
            voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
            voxelFrames[parsedFrames.Length - 1] = new MagicaVoxelData[parsedFrames[parsedFrames.Length - 1].Length];
            parsedFrames[0].CopyTo(voxelFrames[0], 0);
            parsedFrames[parsedFrames.Length - 1].CopyTo(voxelFrames[parsedFrames.Length - 1], 0);
            List<MagicaVoxelData> launchers = new List<MagicaVoxelData>(4), trails = new List<MagicaVoxelData>(4);
            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length - 2], missile = new List<MagicaVoxelData>[voxelFrames.Length - 2];
            foreach (MagicaVoxelData mvd in voxelFrames[0])
            {
                if (mvd.color == 249 - 170)
                {
                    launchers.Add(mvd);
                }
            }
            int maxY = launchers.Max(v => v.y);
            int minY = launchers.Max(v => v.y);
            float midY = (maxY + minY) / 2F;
            MagicaVoxelData launcher = launchers.RandomElement();
            MagicaVoxelData trail = trails.RandomElement();
            for (int f = 0; f < voxelFrames.Length - 2; f++) //going only through the middle
            {
                extra[f] = new List<MagicaVoxelData>(20);
                missile[f] = new List<MagicaVoxelData>(20);

                if (f > 1)
                {
                    for (int i = 0; i < missile[f - 1].Count; i++)
                    {
                        missile[f].Add(new MagicaVoxelData
                        {
                            x = (byte)(missile[f - 1][i].x + 4),
                            y = (byte)(missile[f - 1][i].y),
                            z = (byte)(missile[f - 1][i].z + 4),
                            color = missile[f - 1][i].color
                        });
                    }
                }
                if (f == 0)
                {
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y), z = (byte)(launcher.z), color = 249 - 40 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y), z = (byte)(launcher.z), color = 249 - 40 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 1), z = (byte)(launcher.z), color = 249 - 40 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y), z = (byte)(launcher.z + 1), color = 249 - 40 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z), color = 249 - 40 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 1), color = 249 - 40 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y), z = (byte)(launcher.z + 1), color = 249 - 40 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 1), color = 249 - 40 });
                }
                if (f == 1)
                {
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y), z = (byte)(launcher.z + 2), color = 249 - 40 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y), z = (byte)(launcher.z + 2), color = 249 - 40 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 2), color = 249 - 40 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y), z = (byte)(launcher.z + 3), color = 249 - 40 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 2), color = 249 - 40 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 3), color = 249 - 40 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y), z = (byte)(launcher.z + 3), color = 249 - 40 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 3), color = 249 - 40 });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = launcher.y, z = (byte)(launcher.z + 2), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 2), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 1), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 1), color = 249 - 72 });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y), z = (byte)(launcher.z), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y), z = (byte)(launcher.z), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 1), z = (byte)(launcher.z), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y), z = (byte)(launcher.z + 1), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 1), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y), z = (byte)(launcher.z + 1), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 1), color = 249 - 72 });

                }
                else if (f == 2)
                {

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y), z = (byte)(launcher.z + 3), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 3), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = launcher.y, z = (byte)(launcher.z + 4), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 4), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = launcher.y, z = (byte)(launcher.z + 3), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 3), color = 249 - 72 });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y), z = (byte)(launcher.z + 2), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 2), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 3), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 3), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = launcher.y, z = (byte)(launcher.z + 2), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 2), color = 249 - 72 });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y), z = (byte)(launcher.z + 1), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 1), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = launcher.y, z = (byte)(launcher.z + 2), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 2), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 1), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 1), color = 249 - 72 });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y), z = (byte)(launcher.z), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y), z = (byte)(launcher.z), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 1), z = (byte)(launcher.z), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y), z = (byte)(launcher.z + 1), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 1), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y), z = (byte)(launcher.z + 1), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 1), color = 249 - 72 });
                }
                else if (f == 3)
                {

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 0), z = (byte)(launcher.z + 4), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 4), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 0), z = (byte)(launcher.z + 3), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 3), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 0), z = (byte)(launcher.z + 3), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 3), color = 249 - 152 });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 4), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 4), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 3), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 3), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 3), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 3), color = 249 - 152 });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 3), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 3), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = launcher.y, z = (byte)(launcher.z + 2), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 2), color = 249 - 152 });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 3), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 3), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 2), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 2), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 2), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 2), color = 249 - 152 });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 1), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 1), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 3), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 3), color = 249 - 152 });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = launcher.y, z = (byte)(launcher.z + 2), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 2), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 1), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 1), color = 249 - 152 });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y), z = (byte)(launcher.z), color = 249 - 160 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y), z = (byte)(launcher.z), color = 249 - 160 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y), z = (byte)(launcher.z + 1), color = 249 - 160 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z), color = 249 - 160 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 1), color = 249 - 160 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y), z = (byte)(launcher.z + 1), color = 249 - 160 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 1), color = 249 - 160 });
                }
                else if (f == 4)
                {
                    /*
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = launcher.y, z = (byte)(launcher.z + 4), color = 249 - 136 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 4), color = 249 - 136 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = launcher.y, z = (byte)(launcher.z + 5), color = 249 - 136 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 5), color = 249 - 136 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = launcher.y, z = (byte)(launcher.z + 5), color = 249 - 136 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 5), color = 249 - 136 });
                    */
                    /*                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 4), color = 249 - 136 });
                                        missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 4), color = 249 - 136 });
                                        missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 3), color = 249 - 136 });
                                        missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 3), color = 249 - 136 });
                    //                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 4), color = 249 - 136 });
                      //                  missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 4), color = 249 - 136 });
                                        */

                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y), z = (byte)(launcher.z), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y), z = (byte)(launcher.z + 1), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 1), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y), z = (byte)(launcher.z + 1), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 1), color = 249 - 120 });

                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y - 1), z = (byte)(launcher.z), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 1), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 2), z = (byte)(launcher.z), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 1), color = 249 - 120 });
                    extra[f] = extra[f].Where(v => r.Next(5) > 0).ToList();


                }
                else if (f == 5)
                {

                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y), z = (byte)(launcher.z), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y), z = (byte)(launcher.z + 1), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 1), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y), z = (byte)(launcher.z + 1), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 1), color = 249 - 120 });

                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y - 1), z = (byte)(launcher.z), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 1), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 2), z = (byte)(launcher.z), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 1), color = 249 - 120 });
                    extra[f] = extra[f].Where(v => r.Next(5) > 1).ToList();
                    //extra[f].AddRange(generateCone(new MagicaVoxelData { x = (byte)(launcher.x + 6), y = (byte)(launcher.y), z = (byte)(launcher.z + 6), color = 249 - 136 }, 4, 249 - 120));

                }
                else if (f == 6)
                {

                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y), z = (byte)(launcher.z), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y), z = (byte)(launcher.z + 1), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 1), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y), z = (byte)(launcher.z + 1), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z + 1), color = 249 - 120 });

                    extra[f] = extra[f].Where(v => r.Next(5) > 1).ToList();

                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y - 1), z = (byte)(launcher.z), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y - 1), z = (byte)(launcher.z + 1), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 2), z = (byte)(launcher.z), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 1), color = 249 - 120 });

                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y - 2), z = (byte)(launcher.z), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 1), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 3), z = (byte)(launcher.z), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 3), z = (byte)(launcher.z + 1), color = 249 - 120 });
                    //extra[f].AddRange(generateCone(new MagicaVoxelData { x = (byte)(launcher.x + 9), y = (byte)(launcher.y), z = (byte)(launcher.z + 9), color = 249 - 136 }, 5, 249 - 120));
                    extra[f] = extra[f].Where(v => r.Next(5) > 1).ToList();

                }
                if (f >= 4)
                {
                    extra[f].AddRange(generateCone(new MagicaVoxelData { x = (byte)(launcher.x + 3 * (f - 3)), y = (byte)(launcher.y), z = (byte)(launcher.z + 3 * (f - 3)), color = 249 - 136 }, (f + f) / 3, 249 - 136).Where(v => r.Next(95) > f * f));

                }
            }
            for (int f = 1; f < voxelFrames.Length - 1; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(parsedFrames[f]);
                working.AddRange(missile[f - 1]);
                working.AddRange(extra[f - 1]);
                voxelFrames[f] = working.ToArray();
            }
            return voxelFrames;
        }

        public static MagicaVoxelData[][] ArcMissileReceiveAnimation(MagicaVoxelData[][] parsedFrames, int strength)
        {

            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[parsedFrames.Length];
            MagicaVoxelData[][] finalFrames = new MagicaVoxelData[parsedFrames.Length][];

            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length], missile = new List<MagicaVoxelData>[voxelFrames.Length];
            MagicaVoxelData[][] explosion = new MagicaVoxelData[16][];

            for (int f = 0; f < voxelFrames.Length; f++)
            {
                extra[f] = new List<MagicaVoxelData>(20);
            }
            MagicaVoxelData launcher = new MagicaVoxelData { x = 56, y = 28, z = 30 };
            bool isExploding = false;
            int firstBurst = 0;
            for (int f = 0; f < voxelFrames.Length; f++)
            {
                //if (f > 0)
                //{
                //    for (int i = 0; i < extra[f-1].Count; i++)
                //    {
                //        extra[f].Add(new MagicaVoxelData { x = (byte)(extra[f-1][i].x + 2), y = (byte)(extra[f-1][i].y + Math.Round(r.NextDouble() * 1.1 - 0.55)),
                //            z = extra[f-1][i].z, color = extra[f-1][i].color });
                //    }
                //}

                extra[f] = new List<MagicaVoxelData>(20);
                missile[f] = new List<MagicaVoxelData>(20);

                if (f > 0)
                {
                    for (int i = 0; i < missile[f - 1].Count; i++)
                    {
                        if (missile[f - 1][i].x - 4 < 36)
                        {
                            isExploding = true;
                            explosion = HugeExplosion(randomFill(24, 27, missile[f - 1][i].z, 4, 6, 3, new int[] { 249 - 136, 249 - 152, 249 - 160 }).Concat(missile[f - 1]).ToArray(), 0, 14 - f, 2);
                            missile[f].Clear();
                            firstBurst = f;
                            break;
                        }
                        else
                            missile[f].Add(new MagicaVoxelData
                        {
                            x = (byte)(missile[f - 1][i].x - 4),
                            y = (byte)(missile[f - 1][i].y),
                            z = (byte)(missile[f - 1][i].z - 4),
                            color = missile[f - 1][i].color
                        });
                    }
                }
                if (f == 0 && !isExploding)
                {
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y), z = (byte)(launcher.z - 2), color = 249 - 40 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y), z = (byte)(launcher.z - 2), color = 249 - 40 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 2), color = 249 - 40 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y), z = (byte)(launcher.z - 3), color = 249 - 40 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 2), color = 249 - 40 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 3), color = 249 - 40 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y), z = (byte)(launcher.z - 3), color = 249 - 40 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 3), color = 249 - 40 });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = launcher.y, z = (byte)(launcher.z - 2), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 2), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = launcher.y, z = (byte)(launcher.z - 1), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 72 });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y), z = (byte)(launcher.z), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y), z = (byte)(launcher.z), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 1), z = (byte)(launcher.z), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 72 });


                }
                if (f == 1 && !isExploding)
                {
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y), z = (byte)(launcher.z - 3), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 3), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = launcher.y, z = (byte)(launcher.z - 4), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 4), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 4), y = launcher.y, z = (byte)(launcher.z - 3), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 4), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 3), color = 249 - 72 });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y), z = (byte)(launcher.z - 2), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 2), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = launcher.y, z = (byte)(launcher.z - 3), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 3), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = launcher.y, z = (byte)(launcher.z - 2), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 2), color = 249 - 72 });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = launcher.y, z = (byte)(launcher.z - 2), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 2), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = launcher.y, z = (byte)(launcher.z - 1), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 72 });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y), z = (byte)(launcher.z), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y), z = (byte)(launcher.z), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 1), z = (byte)(launcher.z), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 72 });




                }
                else if (f == 2 && !isExploding)
                {

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y), z = (byte)(launcher.z - 3), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 3), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = launcher.y, z = (byte)(launcher.z - 4), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 4), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 4), y = launcher.y, z = (byte)(launcher.z - 3), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 4), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 3), color = 249 - 72 });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y), z = (byte)(launcher.z - 2), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 2), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = launcher.y, z = (byte)(launcher.z - 3), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 3), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = launcher.y, z = (byte)(launcher.z - 2), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 2), color = 249 - 72 });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = launcher.y, z = (byte)(launcher.z - 2), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 2), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = launcher.y, z = (byte)(launcher.z - 1), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 72 });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y), z = (byte)(launcher.z), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y), z = (byte)(launcher.z), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 1), z = (byte)(launcher.z), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = 249 - 72 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 72 });
                }
                else if (f == 3 && !isExploding)
                {

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y - 0), z = (byte)(launcher.z - 4), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 4), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 4), y = (byte)(launcher.y - 0), z = (byte)(launcher.z - 3), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 4), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 3), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y - 0), z = (byte)(launcher.z - 3), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 3), color = 249 - 152 });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y - 1), z = (byte)(launcher.z - 4), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z - 4), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 4), y = (byte)(launcher.y - 1), z = (byte)(launcher.z - 3), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 4), y = (byte)(launcher.y + 2), z = (byte)(launcher.z - 3), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y - 1), z = (byte)(launcher.z - 3), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z - 3), color = 249 - 152 });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = launcher.y, z = (byte)(launcher.z - 3), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 3), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = launcher.y, z = (byte)(launcher.z - 2), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 2), color = 249 - 152 });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y - 1), z = (byte)(launcher.z - 3), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y + 2), z = (byte)(launcher.z - 3), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y - 1), z = (byte)(launcher.z - 2), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z - 2), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y - 1), z = (byte)(launcher.z - 2), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y + 2), z = (byte)(launcher.z - 2), color = 249 - 152 });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y - 1), z = (byte)(launcher.z - 1), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z - 1), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y - 1), z = (byte)(launcher.z - 3), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 2), z = (byte)(launcher.z - 3), color = 249 - 152 });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = launcher.y, z = (byte)(launcher.z - 2), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 2), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = launcher.y, z = (byte)(launcher.z - 1), color = 249 - 152 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 152 });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y), z = (byte)(launcher.z), color = 249 - 160 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y), z = (byte)(launcher.z), color = 249 - 160 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = 249 - 160 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z), color = 249 - 160 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 160 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = 249 - 160 });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 160 });
                }
                else if (f == 4 && !isExploding)
                {
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y), z = (byte)(launcher.z), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 120 });

                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y - 1), z = (byte)(launcher.z), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y - 1), z = (byte)(launcher.z - 1), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 2), z = (byte)(launcher.z), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 2), z = (byte)(launcher.z - 1), color = 249 - 120 });
                    extra[f] = extra[f].Where(v => r.Next(5) > 0).ToList();
                }
                else if (f == 5 && !isExploding)
                {

                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y), z = (byte)(launcher.z), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 120 });

                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y - 1), z = (byte)(launcher.z), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y - 1), z = (byte)(launcher.z - 1), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 2), z = (byte)(launcher.z), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 2), z = (byte)(launcher.z - 1), color = 249 - 120 });
                    extra[f] = extra[f].Where(v => r.Next(5) > 1).ToList();

                }
                else if (f == 6 && !isExploding)
                {

                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y), z = (byte)(launcher.z), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 120 });

                    extra[f] = extra[f].Where(v => r.Next(5) > 1).ToList();

                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y - 1), z = (byte)(launcher.z), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y - 1), z = (byte)(launcher.z - 1), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 2), z = (byte)(launcher.z), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 2), z = (byte)(launcher.z - 1), color = 249 - 120 });

                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y - 2), z = (byte)(launcher.z), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y - 2), z = (byte)(launcher.z - 1), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 3), z = (byte)(launcher.z), color = 249 - 120 });
                    extra[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 3), z = (byte)(launcher.z - 1), color = 249 - 120 });
                    //extra[f].AddRange(generateCone(new MagicaVoxelData { x = (byte)(launcher.x - 9), y = (byte)(launcher.y), z = (byte)(launcher.z + 9), color = 249 - 136 }, 5, 249 - 120));
                    extra[f] = extra[f].Where(v => r.Next(5) > 1).ToList();

                }
                if (f >= 4 && !isExploding)
                {
                    extra[f].AddRange(generateDownwardCone(new MagicaVoxelData { x = (byte)(launcher.x - 3 * (f - 3)), y = (byte)(launcher.y), z = (byte)(launcher.z - 3 * (f - 3)), color = 249 - 136 }, (f + f) / 3, 249 - 136).Where(v => r.Next(95) > f * f));

                }
                if (f < 14 && isExploding)
                {
                    extra[f].AddRange(explosion[f - firstBurst]);
                }
            }
            for (int f = 0; f < voxelFrames.Length; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(20);
                working.AddRange(missile[f]);
                working.AddRange(extra[f]);
                finalFrames[f] = working.ToArray();
            }
            return finalFrames; ;
        }
        private static List<MagicaVoxelData> randomFill(int xStart, int yStart, int zStart, int xSize, int ySize, int zSize, int[] colors)
        {
            List<MagicaVoxelData> box = new List<MagicaVoxelData>(xSize * ySize * zSize);
            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    for (int z = 0; z < zSize; z++)
                    {
                        if (zStart + z < 0) //x + y == xSize + ySize - 2 || x + z == xSize + zSize - 2 || z + y == zSize + ySize - 2 || 
                            continue;
                        box.Add(new MagicaVoxelData { x = (byte)(xStart + x), y = (byte)(yStart + y), z = (byte)(zStart + z), color = (byte)colors.RandomElement() });
                    }
                }
            }
            return box;

        }

        public static MagicaVoxelData[][] BombAnimation(MagicaVoxelData[][] parsedFrames, int unit)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[parsedFrames.Length][];
            voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
            voxelFrames[parsedFrames.Length - 1] = new MagicaVoxelData[parsedFrames[parsedFrames.Length - 1].Length];
            parsedFrames[0].CopyTo(voxelFrames[0], 0);
            parsedFrames[parsedFrames.Length - 1].CopyTo(voxelFrames[parsedFrames.Length - 1], 0);
            List<MagicaVoxelData> launchers = new List<MagicaVoxelData>(4);
            // List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length - 2];
            foreach (MagicaVoxelData mvd in voxelFrames[0])
            {
                if (mvd.color == 249 - 169)
                {
                    launchers.Add(mvd);
                }
            }
            List<MagicaVoxelData>[][] missiles = new List<MagicaVoxelData>[launchers.Count][];
            MagicaVoxelData[][][] explosions = new MagicaVoxelData[launchers.Count][][];
            MagicaVoxelData[] centers = new MagicaVoxelData[launchers.Count];
            int[] exploding = new int[launchers.Count];
            for (int i = 0; i < launchers.Count; i++)
            {
                missiles[i] = new List<MagicaVoxelData>[voxelFrames.Length - 2];
                explosions[i] = new MagicaVoxelData[voxelFrames.Length - 2][];
                exploding[i] = -1;
            }
            for (int f = 0; f < voxelFrames.Length - 2; f++) //going only through the middle
            {
                //extra[f] = new List<MagicaVoxelData>(100);
                for (int m = 0; m < missiles.Length; m++)
                {

                    launchers = new List<MagicaVoxelData>(4);
                    foreach (MagicaVoxelData mvd in voxelFrames[0])
                    {
                        if (mvd.color == 249 - 169)
                        {
                            launchers.Add(mvd);
                        }
                    }
                    MagicaVoxelData launcher = launchers[m];
                    missiles[m][f] = new List<MagicaVoxelData>(40);

                    if (f > 0)
                    {
                        double drop = f * (r.NextDouble() * 0.7 + 0.5);
                        foreach (MagicaVoxelData missile in missiles[m][f - 1].OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128))
                        {
                            if (missile.z - drop < 1)
                            {
                                exploding[m] = 0;
                                centers[m] = missile;
                                missiles[m][f].Clear(); ;
                                break;
                            }
                            else
                            {
                                missiles[m][f].Add(new MagicaVoxelData
                                {
                                    x = (byte)(missile.x),
                                    y = (byte)(missile.y),
                                    z = (byte)(missile.z - drop),
                                    color = missile.color
                                });
                            }
                        }
                    }
                    if (f == 0)
                    {
                        missiles[m][f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 0), y = (byte)(launcher.y), z = (byte)(launcher.z - 0), color = 249 - 40 });
                        missiles[m][f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y), z = (byte)(launcher.z - 0), color = 249 - 40 });
                        missiles[m][f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 0), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 0), color = 249 - 40 });
                        missiles[m][f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 0), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = 249 - 40 });
                        missiles[m][f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 0), color = 249 - 40 });
                        missiles[m][f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 0), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 40 });
                        missiles[m][f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = 249 - 40 });
                        missiles[m][f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 40 });
                    }
                    else if (f == 1)
                    {
                        missiles[m][f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 0), y = (byte)(launcher.y), z = (byte)(launcher.z - 0), color = 249 - 40 });
                        missiles[m][f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y), z = (byte)(launcher.z - 0), color = 249 - 40 });
                        missiles[m][f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 0), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 0), color = 249 - 40 });
                        missiles[m][f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 0), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = 249 - 40 });
                        missiles[m][f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 0), color = 249 - 40 });
                        missiles[m][f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 0), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 40 });
                        missiles[m][f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = 249 - 40 });
                        missiles[m][f].Add(new MagicaVoxelData { x = (byte)(launcher.x + 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 40 });

                    }

                    if (exploding[m] > -1)
                    {
                        if (exploding[m] == 0)
                        {
                            explosions[m] = HugeExplosion(randomFill(centers[m].x - 1, centers[m].y - 1, 0, 4, 4, 4, new int[] { 249 - 136, 249 - 152, 249 - 160 }).ToArray(), 0, voxelFrames.Length - 2 - f, 4);
                        }
                        exploding[m]++;
                    }
                }
            }
            for (int f = 1; f < voxelFrames.Length - 1; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(parsedFrames[f]);
                for (int i = 0; i < launchers.Count; i++)
                {
                    working.AddRange(missiles[i][f - 1]);
                    if (f + 1 - voxelFrames.Length + exploding[i] >= 0 && f + 1 - voxelFrames.Length + exploding[i] < explosions[i].Length)
                        working.AddRange(explosions[i][f + 1 - voxelFrames.Length + exploding[i]]);
                }
                //working.AddRange(extra[f - 1]);
                voxelFrames[f] = working.ToArray();
            }

            return voxelFrames;
        }

        public static MagicaVoxelData[][] BombReceiveAnimation(MagicaVoxelData[][] parsedFrames, int strength)
        {
            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[parsedFrames.Length];
            MagicaVoxelData[][] finalFrames = new MagicaVoxelData[parsedFrames.Length][];

            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length];
            MagicaVoxelData[][][] explosions = new MagicaVoxelData[strength][][];
            for (int s = 0; s < strength; s++)
            {
                explosions[s] = HugeExplosion(randomFill(42, 25 + r.Next(6), r.Next(3), 4, 3, 3, new int[] { 249 - 152, 249 - 160, 249 - 136 }).ToArray(), -3, 9, 2);
            }
            for (int f = 0; f < voxelFrames.Length; f++)
            {
                extra[f] = new List<MagicaVoxelData>(200);
            }
            for (int f = 0; f < voxelFrames.Length - 1; f++)
            {
                //if (f > 0)
                //{
                //    for (int i = 0; i < extra[f-1].Count; i++)
                //    {
                //        extra[f].Add(new MagicaVoxelData { x = (byte)(extra[f-1][i].x + 2), y = (byte)(extra[f-1][i].y + Math.Round(r.NextDouble() * 1.1 - 0.55)),
                //            z = extra[f-1][i].z, color = extra[f-1][i].color });
                //    }
                //}
                if (f == 5)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        for (int sp = 0; sp < explosions.Length; sp++)
                            extra[f + i].AddRange(explosions[sp][i]);
                    }
                }
            }
            for (int f = 0; f < voxelFrames.Length; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(50);
                working.AddRange(extra[f]);
                finalFrames[f] = working.ToArray();
            }
            return finalFrames;
        }


        private static AnimationGenerator[] weaponAnimations = { HandgunAnimation, MachineGunAnimation, AAGunAnimation, CannonAnimation, LongCannonAnimation, RocketAnimation, ArcMissileAnimation, BombAnimation };
        private static AnimationGenerator[] receiveAnimations = { HandgunReceiveAnimation, MachineGunReceiveAnimation, MachineGunReceiveAnimation,
                                                                    CannonReceiveAnimation, LongCannonReceiveAnimation,
                                                                    RocketReceiveAnimation, ArcMissileReceiveAnimation, BombReceiveAnimation};

        public static MagicaVoxelData[][] makeFiringAnimation(MagicaVoxelData[] parsed, int unit, int weapon)
        {
            MagicaVoxelData[][] parsedFrames = new MagicaVoxelData[][] {
                parsed, parsed, parsed, parsed,
                parsed, parsed, parsed, parsed, 
                parsed, parsed, parsed, parsed,
                parsed, parsed, parsed, parsed, };
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[parsedFrames.Length][];
            //voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
            parsedFrames.CopyTo(voxelFrames, 0);
            for (int i = 0; i < parsedFrames[0].Length; i++)
            {
                voxelFrames[0][i].x += 20;
                voxelFrames[0][i].y += 20;
            }

            if (VoxelLogic.CurrentWeapons[unit][weapon] == -1)
            {
                return new MagicaVoxelData[0][];
            }
            else
            {
                voxelFrames = weaponAnimations[VoxelLogic.CurrentWeapons[unit][weapon]](voxelFrames, unit);
            }
            for (int f = 0; f < parsedFrames.Length; f++)
            {

                List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxelFrames[f].Length);
                int[,] taken = new int[60, 60];
                taken.Fill(-1);
                for (int i = 0; i < voxelFrames[f].Length; i++)
                {
                    // do not store this voxel if it lies out of range of the voxel chunk (60x60x60)
                    if (voxelFrames[f][i].x >= 60 || voxelFrames[f][i].y >= 60 || voxelFrames[f][i].z >= 60)
                    {
                        Console.Write("Voxel out of bounds: " + voxelFrames[f][i].x + ", " + voxelFrames[f][i].y + ", " + voxelFrames[f][i].z);
                        continue;
                    }

                    altered.Add(voxelFrames[f][i]);
                    if (-1 == taken[voxelFrames[f][i].x, voxelFrames[f][i].y] && voxelFrames[f][i].color != 249 - 80 && voxelFrames[f][i].color != 249 - 104 && voxelFrames[f][i].color != 249 - 112
                         && voxelFrames[f][i].color != 249 - 96 && voxelFrames[f][i].color != 249 - 128 && voxelFrames[f][i].color != 249 - 136
                         && voxelFrames[f][i].color != 249 - 152 && voxelFrames[f][i].color != 249 - 160 && voxelFrames[f][i].color > 249 - 168)
                    {
                        MagicaVoxelData vox = new MagicaVoxelData();
                        vox.x = voxelFrames[f][i].x;
                        vox.y = voxelFrames[f][i].y;
                        vox.z = (byte)(0);
                        vox.color = 249 - 96;
                        taken[vox.x, vox.y] = altered.Count();
                        altered.Add(vox);
                    }
                }
                voxelFrames[f] = altered.ToArray();
            }

            return voxelFrames;
        }
        public static MagicaVoxelData[][] makeReceiveAnimation(int weaponType, int strength)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[16][];
            //voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];

            voxelFrames = receiveAnimations[weaponType](voxelFrames, strength);

            for (int f = 0; f < voxelFrames.Length; f++)
            {

                List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxelFrames[f].Length);
                int[,] taken = new int[60, 60];
                taken.Fill(-1);
                for (int i = 0; i < voxelFrames[f].Length; i++)
                {
                    // do not store this voxel if it lies out of range of the voxel chunk (60x60x60)
                    if (voxelFrames[f][i].x >= 60 || voxelFrames[f][i].y >= 60 || voxelFrames[f][i].z >= 60)
                    {
                        Console.Write("Voxel out of bounds: " + voxelFrames[f][i].x + ", " + voxelFrames[f][i].y + ", " + voxelFrames[f][i].z);
                        continue;
                    }

                    altered.Add(voxelFrames[f][i]);
                    if (-1 == taken[voxelFrames[f][i].x, voxelFrames[f][i].y] && voxelFrames[f][i].color != 249 - 80 && voxelFrames[f][i].color != 249 - 104 && voxelFrames[f][i].color != 249 - 112
                         && voxelFrames[f][i].color != 249 - 96 && voxelFrames[f][i].color != 249 - 128 && voxelFrames[f][i].color != 249 - 136
                         && voxelFrames[f][i].color != 249 - 152 && voxelFrames[f][i].color != 249 - 160 && voxelFrames[f][i].color > 249 - 160)
                    {
                        MagicaVoxelData vox = new MagicaVoxelData();
                        vox.x = voxelFrames[f][i].x;
                        vox.y = voxelFrames[f][i].y;
                        vox.z = (byte)(0);
                        vox.color = 249 - 96;
                        taken[vox.x, vox.y] = altered.Count();
                        altered.Add(vox);
                    }
                }
                voxelFrames[f] = altered.ToArray();
            }

            return voxelFrames;
        }

        public static MagicaVoxelData[][] Flyover(MagicaVoxelData[] voxels)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[17][];
            voxelFrames[0] = new MagicaVoxelData[voxels.Length];
            voxels.CopyTo(voxelFrames[0], 0);
            for (int i = 0; i < voxels.Length; i++)
            {
                voxelFrames[0][i].x += 20;
                voxelFrames[0][i].y += 20;
            }
            for (int f = 1; f <= 8; f++)
            {
                voxelFrames[f] = new MagicaVoxelData[voxelFrames[f - 1].Length]; //.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)
                voxelFrames[f - 1].CopyTo(voxelFrames[f], 0);

                for (int i = 0; i < voxelFrames[f].Length; i++)
                {
                    voxelFrames[f][i].z++;
                }
            }
            /*            for (int f = 9; f <= 16; f++)
                        {
                            voxelFrames[f] = new MagicaVoxelData[voxelFrames[f - 1].Length]; //.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)
                            voxelFrames[f - 1].CopyTo(voxelFrames[f], 0);


                            for (int i = 0; i < voxelFrames[f].Length; i++)
                            {
                                voxelFrames[f][i].z--;
                            }
                        }*/
            for (int f = 1; f <= 8; f++)
            {

                List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxelFrames[f].Length);
                int[,] taken = new int[60, 60];
                taken.Fill(-1);

                int minX;
                int maxX;
                float midX;

                minX = voxelFrames[f].Min(v => v.x * ((v.color == 249 - 96 || v.color == 249 - 128 ||
                    v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color <= 249 - 168) ? 100 : 1));
                maxX = voxelFrames[f].Max(v => v.x * ((v.color == 249 - 96 || v.color == 249 - 128 ||
                    v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color <= 249 - 168) ? 0 : 1));
                midX = (maxX + minX) / 2F;

                int minY;
                int maxY;
                float midY;
                minY = voxelFrames[f].Min(v => v.y * ((v.color == 249 - 96 || v.color == 249 - 128 ||
                        v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color <= 249 - 168) ? 100 : 1));
                maxY = voxelFrames[f].Max(v => v.y * ((v.color == 249 - 96 || v.color == 249 - 128 ||
                        v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color <= 249 - 168) ? 0 : 1));
                midY = (maxY + minY) / 2F;

                for (int i = 0; i < voxelFrames[f].Length; i++)
                {
                    // do not store this voxel if it lies out of range of the voxel chunk (30x30x30)
                    if (voxelFrames[f][i].x >= 60 || voxelFrames[f][i].y >= 60 || voxelFrames[f][i].z >= 60)
                    {
                        Console.Write("Voxel out of bounds: " + voxelFrames[f][i].x + ", " + voxelFrames[f][i].y + ", " + voxelFrames[f][i].z);
                        continue;
                    }

                    altered.Add(voxelFrames[f][i]);
                    //-1 == taken[voxelFrames[f][i].x, voxelFrames[f][i].y] && 
                    if (voxelFrames[f][i].color != 249 - 80 && voxelFrames[f][i].color != 249 - 104 && voxelFrames[f][i].color != 249 - 112
                         && voxelFrames[f][i].color != 249 - 96 && voxelFrames[f][i].color != 249 - 128 && voxelFrames[f][i].color != 249 - 136
                         && voxelFrames[f][i].color != 249 - 152 && voxelFrames[f][i].color != 249 - 160 && voxelFrames[f][i].color > 249 - 168)
                    {
                        MagicaVoxelData vox = new MagicaVoxelData();
                        vox.color = (byte)(249 - 96);
                        if (i == 0)
                        {
                            vox.x = voxelFrames[f][i].x;
                            vox.y = voxelFrames[f][i].y;
                            vox.z = 0;
                        }
                        if (voxelFrames[f][i].x > midX)
                        {
                            vox.x = (byte)(voxelFrames[f][i].x - f * (r.NextDouble() + 0.35));
                            if (vox.x < midX)
                                vox.color = 249 - 168;
                        }
                        else
                        {
                            vox.x = (byte)(voxelFrames[f][i].x + f * (r.NextDouble() + 0.35));
                            if (vox.x > midX)
                                vox.color = 249 - 168;
                        }
                        if (voxelFrames[f][i].y > midY)
                        {
                            vox.y = (byte)(voxelFrames[f][i].y - f * (r.NextDouble() + 0.35));
                            if (vox.y < midY)
                                vox.color = 249 - 168;
                        }
                        else
                        {
                            vox.y = (byte)(voxelFrames[f][i].y + f * (r.NextDouble() + 0.35));
                            if (vox.y > midY)
                                vox.color = 249 - 168;
                        }
                        vox.z = (byte)(0);
                        taken[vox.x, vox.y] = altered.Count();
                        altered.Add(vox);
                    }
                }
                voxelFrames[f] = altered.ToArray();
            }
            /*            for (int f = 9; f <= 16; f++)
                        {

                            List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxelFrames[f].Length);
                            int[,] taken = new int[30, 30];
                            taken.Fill(-1);

                            int minX;
                            int maxX;
                            float midX;

                            minX = voxelFrames[f].Min(v => v.x * ((v.color == 249 - 96 || v.color == 249 - 128 ||
                                v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color < 249 - 168) ? 100 : 1));
                            maxX = voxelFrames[f].Max(v => v.x * ((v.color == 249 - 96 || v.color == 249 - 128 ||
                                v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color < 249 - 168) ? 0 : 1));
                            midX = (maxX + minX) / 2F;

                            int minY;
                            int maxY;
                            float midY;
                            minY = voxelFrames[f].Min(v => v.y * ((v.color == 249 - 96 || v.color == 249 - 128 ||
                                    v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color < 249 - 168) ? 100 : 1));
                            maxY = voxelFrames[f].Max(v => v.y * ((v.color == 249 - 96 || v.color == 249 - 128 ||
                                    v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color < 249 - 168) ? 0 : 1));
                            midY = (maxY + minY) / 2F;

                            for (int i = 0; i < voxelFrames[f].Length; i++)
                            {
                                // do not store this voxel if it lies out of range of the voxel chunk (30x30x30)
                                if (voxelFrames[f][i].x >= 30 || voxelFrames[f][i].y >= 30 || voxelFrames[f][i].z >= 30)
                                {
                                    Console.Write("Voxel out of bounds: " + voxelFrames[f][i].x + ", " + voxelFrames[f][i].y + ", " + voxelFrames[f][i].z);
                                    continue;
                                }

                                altered.Add(voxelFrames[f][i]);
                                if (-1 == taken[voxelFrames[f][i].x, voxelFrames[f][i].y] && voxelFrames[f][i].color != 249 - 80 && voxelFrames[f][i].color != 249 - 104 && voxelFrames[f][i].color != 249 - 112
                                     && voxelFrames[f][i].color != 249 - 96 && voxelFrames[f][i].color != 249 - 128 && voxelFrames[f][i].color != 249 - 136
                                     && voxelFrames[f][i].color != 249 - 152 && voxelFrames[f][i].color != 249 - 160 && voxelFrames[f][i].color != 249 - 168)
                                {
                                    MagicaVoxelData vox = new MagicaVoxelData();
                                    vox.x = (byte)((voxelFrames[f][i].x < midX) ? voxelFrames[f][i].x - 1 - (f / 4) : voxelFrames[f][i].x + 1 + (f / 4));
                                    vox.y = (byte)((voxelFrames[f][i].y < midY) ? voxelFrames[f][i].y - 1 - (f / 4) : voxelFrames[f][i].y + 1 + (f / 4));
                                    vox.z = (byte)(0);
                                    vox.color = 249 - 96;
                                    taken[vox.x, vox.y] = altered.Count();
                                    altered.Add(vox);
                                }
                            }
                            voxelFrames[f] = altered.ToArray();
                        }*/

            MagicaVoxelData[][] frames = new MagicaVoxelData[16][];

            for (int f = 1; f < 9; f++)
            {
                frames[f - 1] = new MagicaVoxelData[voxelFrames[f].Length];
                frames[16 - f] = new MagicaVoxelData[voxelFrames[f].Length];
                voxelFrames[f].CopyTo(frames[f - 1], 0);
                voxelFrames[f].CopyTo(frames[16 - f], 0);
            }
            return frames;
        }


        private static Bitmap[] renderW(MagicaVoxelData[] voxels, int facing, int frame, int maxFrames)
        {
            Bitmap[] b = {
            new Bitmap(88, 108, PixelFormat.Format32bppArgb),
            new Bitmap(88, 108, PixelFormat.Format32bppArgb),};

            Graphics g = Graphics.FromImage((Image)b[0]);
            Graphics gsh = Graphics.FromImage((Image)b[1]);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_soft.png");
            Image flat = new Bitmap("flat_soft.png");
            Image spin = new Bitmap("spin_soft.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 4;
            int xSize = 20, ySize = 20;
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
            int jitter = (frame % 3) + (frame / 3);
            if (maxFrames != 4)
                jitter = 0;
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128 - ((v.color == 249 - 96) ? 32 * 128 * 32 : 0))) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = (249 - vx.color) / 8;
                if (current_color >= VoxelLogic.wcolors.Length)
                    continue;
                // Console.Write(current_color + "  ");
                if ((frame % 2 != 0) && VoxelLogic.wcolors[current_color][3] == VoxelLogic.spin_alpha_0)
                    continue;
                else if ((frame % 2 != 1) && VoxelLogic.wcolors[current_color][3] == VoxelLogic.spin_alpha_1)
                    continue;
                else if (current_color == 168)
                    continue;
                else if (current_color == 96)
                {

                    colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+VoxelLogic.wcolors[current_color][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+VoxelLogic.wcolors[current_color][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+VoxelLogic.wcolors[current_color][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    gsh.DrawImage(flat,
                       new Rectangle((vx.x + vx.y) * 2 + 4, 100 - 20 - vx.y + vx.x - vx.z * 3 + 2
                           , width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                       0, 0,        // upper-left corner of source rectangle 
                       width,       // width of source rectangle
                       height,      // height of source rectangle
                       GraphicsUnit.Pixel,
                       imageAttributes);
                }
                else
                {
                    if (current_color == 80) //lights
                    {
                        float lightCalc = (0.5F - (((frame % 4) % 3) + ((frame % 4) / 3))) * 0.12F;
                        colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+VoxelLogic.wcolors[current_color][0] + lightCalc,  0,  0,  0, 0},
   new float[] {0,  0.251F+VoxelLogic.wcolors[current_color][1] + lightCalc,  0,  0, 0},
   new float[] {0,  0,  0.31F+VoxelLogic.wcolors[current_color][2] + lightCalc,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                    }
                    else if (current_color >= 168)
                    {

                        //169 Bomb Drop
                        //170 Arc Missile
                        //171 Rocket
                        //172 Long Cannon
                        //173 Cannon
                        //174 AA Gun
                        //175 Machine Gun
                        //176 Handgun

                        continue;
                    }
                    else
                    {
                        colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+VoxelLogic.wcolors[current_color][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+VoxelLogic.wcolors[current_color][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+VoxelLogic.wcolors[current_color][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                    }
                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    g.DrawImage((current_color == 80) ? spin :
                       (VoxelLogic.wcolors[current_color][3] == 1F) ? image : (VoxelLogic.wcolors[current_color][3] == VoxelLogic.flat_alpha) ? flat : spin,
                       new Rectangle((vx.x + vx.y) * 2 + 4 + ((current_color == 136) ? jitter - 1 : 0),
                           100 - 20 - vx.y + vx.x - vx.z * 3 - ((VoxelLogic.wcolors[current_color][3] == VoxelLogic.flat_alpha) ? -2 : jitter),
                           width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                       0, 0,        // upper-left corner of source rectangle 
                       width,       // width of source rectangle
                       height,      // height of source rectangle
                       GraphicsUnit.Pixel,
                       imageAttributes);
                }
            }
            return b;
        }
        public static Bitmap renderOutlineW(MagicaVoxelData[] voxels, int facing, int frame, int maxFrames)
        {
            Bitmap b = new Bitmap(88, 108, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            Image image = new Bitmap("black_outline_soft.png");
            //            Image flat = new Bitmap("flat_soft.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 8;
            int height = 8;

            int xSize = 20, ySize = 20;

            float[][] colorMatrixElements = { 
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

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

            int jitter = (frame % 3) + (frame / 3);
            if (maxFrames != 4)
                jitter = 0;
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128 // - ((v.color == 249 - 96) ? 32 * 128 * 32 : 0)
            {
                int current_color = (249 - vx.color) / 8;
                if (current_color >= VoxelLogic.wcolors.Length)
                    continue;
                if ((frame % 2 != 0) && VoxelLogic.wcolors[current_color][3] == VoxelLogic.spin_alpha_0)
                    continue;
                else if ((frame % 2 != 1) && VoxelLogic.wcolors[current_color][3] == VoxelLogic.spin_alpha_1)
                    continue;
                else if (current_color == 120 || current_color == 152 || current_color == 160 || current_color == 136 || current_color >= 168)
                    continue;
                if ((VoxelLogic.wcolors[current_color][3] != VoxelLogic.flat_alpha))
                {
                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    g.DrawImage(image, //((colors[current_color + faction][3] == VoxelLogic.flat_alpha) ? flat : image)
                    new Rectangle((vx.x + vx.y) * 2 + 2, //if flat use + 4
                        100 - 20 - 2 - vx.y + vx.x - vx.z * 3 - jitter, //((colors[current_color + faction][3] == VoxelLogic.flat_alpha) ? -4 : jitter
                        width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                       0, 0,        // upper-left corner of source rectangle 
                       width,       // width of source rectangle
                       height,      // height of source rectangle
                       GraphicsUnit.Pixel,
                       imageAttributes);
                }
            }
            return b;
        }
        private void MakeMoreBlue(Bitmap bmp)
        {
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
            byte[] rgbValues = new byte[numBytes];

            // Copy the RGB values into the array.

            Marshal.Copy(ptr, rgbValues, 0, numBytes);

            // Manipulate the bitmap, such as changing the 
            // blue value for every other pixel in the the bitmap. 
            for (int counter = 0; counter < rgbValues.Length; counter += 8)
                rgbValues[counter] = 255;

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, numBytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);
        }

        private static Bitmap[] render(MagicaVoxelData[] voxels, int facing, int faction, int frame, int maxFrames)
        {
            log.AppendLine("Frame: " + frame);
            Bitmap[] b = {
            new Bitmap(88, 108, PixelFormat.Format32bppArgb),
            new Bitmap(88, 108, PixelFormat.Format32bppArgb),};

            Graphics g = Graphics.FromImage((Image)b[0]);
            Graphics gsh = Graphics.FromImage((Image)b[1]);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_soft.png");
            Image flat = new Bitmap("flat_soft.png");
            Image spin = new Bitmap("spin_soft.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 4;
            int xSize = 20, ySize = 20;
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
            int jitter = (frame % 3) + (frame / 3);
            if (maxFrames != 4)
                jitter = 0;
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128 - ((v.color == 249 - 96) ? 32 * 128 * 32 : 0))) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;
                // Console.Write(current_color + "  ");
                if ((frame % 2 != 0) && VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.spin_alpha_0)
                    continue;
                else if ((frame % 2 != 1) && VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.spin_alpha_1)
                    continue;
                else if (current_color == 168)
                    continue;
                else if (current_color == 96)
                {

                    colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+VoxelLogic.xcolors[current_color + faction][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+VoxelLogic.xcolors[current_color + faction][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+VoxelLogic.xcolors[current_color + faction][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    gsh.DrawImage(flat,
                       new Rectangle((vx.x + vx.y) * 2 + 4, 100 - 20 - vx.y + vx.x - vx.z * 3 + 2
                           , width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                       0, 0,        // upper-left corner of source rectangle 
                       width,       // width of source rectangle
                       height,      // height of source rectangle
                       GraphicsUnit.Pixel,
                       imageAttributes);
                }
                else
                {
                    if (current_color == 80) //lights
                    {
                        float lightCalc = (0.5F - (((frame % 4) % 3) + ((frame % 4) / 3))) * 0.12F;
                        colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+VoxelLogic.xcolors[current_color + faction][0] + lightCalc,  0,  0,  0, 0},
   new float[] {0,  0.251F+VoxelLogic.xcolors[current_color + faction][1] + lightCalc,  0,  0, 0},
   new float[] {0,  0,  0.31F+VoxelLogic.xcolors[current_color + faction][2] + lightCalc,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                    }
                    else if (current_color >= 168)
                    {

                        //169 Bomb Drop
                        //170 Arc Missile
                        //171 Rocket
                        //172 Long Cannon
                        //173 Cannon
                        //174 AA Gun
                        //175 Machine Gun
                        //176 Handgun

                        continue;
                    }
                    else
                    {
                        colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+VoxelLogic.xcolors[current_color + faction][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+VoxelLogic.xcolors[current_color + faction][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+VoxelLogic.xcolors[current_color + faction][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                    }
                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    g.DrawImage((current_color == 80) ? spin :
                       (VoxelLogic.xcolors[current_color + faction][3] == 1F) ? image : (VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.flat_alpha) ? flat : spin,
                       new Rectangle((vx.x + vx.y) * 2 + 4 + ((current_color == 136) ? jitter - 1 : 0),
                           100 - 20 - vx.y + vx.x - vx.z * 3 - ((VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.flat_alpha) ? -2 : jitter),
                           width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                       0, 0,        // upper-left corner of source rectangle 
                       width,       // width of source rectangle
                       height,      // height of source rectangle
                       GraphicsUnit.Pixel,
                       imageAttributes);
                }
            }
            return b;
        }


        private static Bitmap[] renderDouble(MagicaVoxelData[] voxels, int facing, int faction, int frame, int maxFrames)
        {
            log.AppendLine("Frame: " + frame);
            Bitmap[] b = {
            new Bitmap(88*2, 108*2, PixelFormat.Format32bppArgb),
            new Bitmap(88*2, 108*2, PixelFormat.Format32bppArgb),};

            Graphics g = Graphics.FromImage((Image)b[0]);
            Graphics gsh = Graphics.FromImage((Image)b[1]);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_soft.png");
            Image flat = new Bitmap("flat_soft.png");
            Image spin = new Bitmap("spin_soft.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 4;
            int xSize = 40, ySize = 40;
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
            int jitter = (frame % 3) + (frame / 3);
            jitter *= 2;
            if (maxFrames != 4)
                jitter = 0;
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 40 - v.y + v.z * 40 * 128 - ((v.color == 249 - 96) ? 40 * 128 * 40 : 0))) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;
                // Console.Write(current_color + "  ");
                if ((frame % 2 != 0) && VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.spin_alpha_0)
                    continue;
                else if ((frame % 2 != 1) && VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.spin_alpha_1)
                    continue;
                else if (current_color == 168)
                    continue;
                else if (current_color == 96)
                {

                    colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+VoxelLogic.xcolors[current_color + faction][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+VoxelLogic.xcolors[current_color + faction][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+VoxelLogic.xcolors[current_color + faction][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    gsh.DrawImage(flat,
                       new Rectangle((vx.x + vx.y) * 2 + 4 * 2, 100 * 2 - 20 * 2 - vx.y + vx.x - vx.z * 3 + 2 * 2
                           , width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                       0, 0,        // upper-left corner of source rectangle 
                       width,       // width of source rectangle
                       height,      // height of source rectangle
                       GraphicsUnit.Pixel,
                       imageAttributes);
                }
                else
                {
                    if (current_color == 80) //lights
                    {
                        float lightCalc = (0.5F - (((frame % 4) % 3) + ((frame % 4) / 3))) * 0.12F;
                        colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+VoxelLogic.xcolors[current_color + faction][0] + lightCalc,  0,  0,  0, 0},
   new float[] {0,  0.251F+VoxelLogic.xcolors[current_color + faction][1] + lightCalc,  0,  0, 0},
   new float[] {0,  0,  0.31F+VoxelLogic.xcolors[current_color + faction][2] + lightCalc,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                    }
                    else if (current_color >= 168)
                    {

                        //169 Bomb Drop
                        //170 Arc Missile
                        //171 Rocket
                        //172 Long Cannon
                        //173 Cannon
                        //174 AA Gun
                        //175 Machine Gun
                        //176 Handgun

                        continue;
                    }
                    else
                    {
                        colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+VoxelLogic.xcolors[current_color + faction][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+VoxelLogic.xcolors[current_color + faction][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+VoxelLogic.xcolors[current_color + faction][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                    }
                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    g.DrawImage((current_color == 80) ? spin :
                       (VoxelLogic.xcolors[current_color + faction][3] == 1F) ? image : (VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.flat_alpha) ? flat : spin,
                       new Rectangle((vx.x + vx.y) * 2 + 4 * 2 + ((current_color == 136) ? jitter - 1 * 2 : 0),
                           100 * 2 - 20 * 2 - vx.y + vx.x - vx.z * 3 - ((VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.flat_alpha) ? -2 * 2 : jitter),
                           width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                       0, 0,        // upper-left corner of source rectangle 
                       width,       // width of source rectangle
                       height,      // height of source rectangle
                       GraphicsUnit.Pixel,
                       imageAttributes);
                }
            }
            return b;
        }


        public static Bitmap renderOutline(MagicaVoxelData[] voxels, int facing, int faction, int frame, int maxFrames)
        {
            Bitmap b = new Bitmap(88, 108, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            Image image = new Bitmap("black_outline_soft.png");
            //            Image flat = new Bitmap("flat_soft.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 8;
            int height = 8;

            int xSize = 20, ySize = 20;

            float[][] colorMatrixElements = { 
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

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

            int jitter = (frame % 3) + (frame / 3);
            if (maxFrames != 4)
                jitter = 0;
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128 // - ((v.color == 249 - 96) ? 32 * 128 * 32 : 0)
            {
                int current_color = 249 - vx.color;

                if ((frame % 2 != 0) && VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.spin_alpha_0)
                    continue;
                else if ((frame % 2 != 1) && VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.spin_alpha_1)
                    continue;
                else if (current_color == 120 || current_color == 152 || current_color == 160 || current_color == 136 || current_color >= 168)
                    continue;
                if ((VoxelLogic.xcolors[current_color + faction][3] != VoxelLogic.flat_alpha))
                {
                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    g.DrawImage(image, //((colors[current_color + faction][3] == VoxelLogic.flat_alpha) ? flat : image)
                    new Rectangle((vx.x + vx.y) * 2 + 2, //if flat use + 4
                        100 - 20 - 2 - vx.y + vx.x - vx.z * 3 - jitter, //((colors[current_color + faction][3] == VoxelLogic.flat_alpha) ? -4 : jitter
                        width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                       0, 0,        // upper-left corner of source rectangle 
                       width,       // width of source rectangle
                       height,      // height of source rectangle
                       GraphicsUnit.Pixel,
                       imageAttributes);
                }
            }
            return b;
        }

        public static Bitmap renderOutlineDouble(MagicaVoxelData[] voxels, int facing, int faction, int frame, int maxFrames)
        {
            Bitmap b = new Bitmap(88 * 2, 108 * 2, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            Image image = new Bitmap("black_outline_soft.png");
            //            Image flat = new Bitmap("flat_soft.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 8;
            int height = 8;

            int xSize = 40, ySize = 40;

            float[][] colorMatrixElements = { 
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

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

            int jitter = (frame % 3) + (frame / 3);
            jitter *= 2;
            if (maxFrames != 4)
                jitter = 0;
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 40 - v.y + v.z * 40 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128 // - ((v.color == 249 - 96) ? 32 * 128 * 32 : 0)
            {
                int current_color = 249 - vx.color;

                if ((frame % 2 != 0) && VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.spin_alpha_0)
                    continue;
                else if ((frame % 2 != 1) && VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.spin_alpha_1)
                    continue;
                else if (current_color == 120 || current_color == 152 || current_color == 160 || current_color == 136 || current_color >= 168)
                    continue;
                if ((VoxelLogic.xcolors[current_color + faction][3] != VoxelLogic.flat_alpha))
                {
                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    g.DrawImage(image, //((colors[current_color + faction][3] == VoxelLogic.flat_alpha) ? flat : image)
                    new Rectangle((vx.x + vx.y) * 2 + 4 * 2, //if flat use + 4
                        100 * 2 - 20 * 2 - 2 - vx.y + vx.x - vx.z * 3 - jitter, //((colors[current_color + faction][3] == VoxelLogic.flat_alpha) ? -4 : jitter
                        width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                       0, 0,        // upper-left corner of source rectangle 
                       width,       // width of source rectangle
                       height,      // height of source rectangle
                       GraphicsUnit.Pixel,
                       imageAttributes);
                }
            }
            return b;
        }
        private static int voxelToPixelLarge(int innerX, int innerY, int x, int y, int z, int current_color, int stride, int jitter)
        {
            /*
             4 * ((vx.x + vx.y) * 2 + 4 + ((current_color == 136) ? jitter - 1 : 0))
                                    + i +
                                    bmpData.Stride * (300 - 60 - vx.y + vx.x - vx.z * 3 - ((VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.flat_alpha) ? -2 : jitter) + j)
             */
            return 4 * ((x + y) * 2 + 4 + ((current_color == 136) ? jitter - 1 : 0)) + innerX + stride * (300 - 60 - y + x - z * 3 - ((VoxelLogic.xcolors[current_color][3] == VoxelLogic.flat_alpha) ? -2 : jitter) + innerY);
        }
        private static int voxelToPixelHuge(int innerX, int innerY, int x, int y, int z, int current_color, int stride, int jitter)
        {
            /*
4 * ((vx.x + vx.y) * 2 + 4 + ((current_color == 136) ? jitter - 1 : 0))
                                    + i +
                                    bmpData.Stride * (600 - 120 - vx.y + vx.x - vx.z * 3 - ((VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.flat_alpha) ? -2 : 0) + j)
             */
            return 4 * ((x + y) * 2 + 12 + ((current_color == 136) ? jitter - 1 : 0))
                                    + innerX +
                                    stride * (600 - 120 - y + x - z * 3 - ((VoxelLogic.xcolors[current_color][3] == VoxelLogic.flat_alpha) ? -2 : 0) + innerY);
        }
        private static int voxelToPixelLargeW(int innerX, int innerY, int x, int y, int z, int current_color, int stride, int jitter, bool still)
        {
            /*
             4 * ((vx.x + vx.y) * 2 + 4 + ((current_color == 136) ? jitter - 1 : 0))
                                    + i +
                                    bmpData.Stride * (300 - 60 - vx.y + vx.x - vx.z * 3 - ((VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.flat_alpha) ? -2 : jitter) + j)
             */
            return 4 * ((x + y) * 2 + 4 + ((VoxelLogic.wcolors[current_color][3] == VoxelLogic.waver_alpha) ? jitter - 2 : 0))
                + innerX +
                stride * (300 - 60 - y + x - z * 3 - ((VoxelLogic.wcolors[current_color][3] == VoxelLogic.flat_alpha) ? -2 : (still) ? 0 : jitter) + innerY);
            //((VoxelLogic.wcolors[current_color][3] == VoxelLogic.flat_alpha) ? -2 : jitter)
        }
        private static int voxelToPixelHugeW(int innerX, int innerY, int x, int y, int z, int current_color, int stride, int jitter, bool still)
        {
            /*
4 * ((vx.x + vx.y) * 2 + 4 + ((current_color == 136) ? jitter - 1 : 0))
                                    + i +
                                    bmpData.Stride * (600 - 120 - vx.y + vx.x - vx.z * 3 - ((VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.flat_alpha) ? -2 : 0) + j)
             */
            return 4 * ((x + y) * 2 + 12 + ((VoxelLogic.wcolors[current_color][3] == VoxelLogic.waver_alpha) ? jitter - 2 : 0))
                                    + innerX +
                                    stride * (600 - 120 - y + x - z * 3 - ((VoxelLogic.wcolors[current_color][3] == VoxelLogic.flat_alpha) ? -2 : (still) ? 0 : jitter) + innerY);
        }
        private static int voxelToPixelMassiveW(int innerX, int innerY, int x, int y, int z, int current_color, int stride, int jitter, bool still)
        {
            /*
4 * ((vx.x + vx.y) * 2 + 4 + ((current_color == 136) ? jitter - 1 : 0))
                                    + i +
                                    bmpData.Stride * (600 - 120 - vx.y + vx.x - vx.z * 3 - ((VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.flat_alpha) ? -2 : 0) + j)
             */
            return 4 * ((x + y) * 2 + 12 + ((VoxelLogic.wcolors[current_color][3] == VoxelLogic.waver_alpha) ? jitter - 2 : 0))
                                    + innerX +
                                    stride * (800 - 160 - y + x - z * 3 - ((VoxelLogic.wcolors[current_color][3] == VoxelLogic.flat_alpha) ? -2 : (still) ? 0 : jitter) + innerY);
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
                int current_color = 249 - vx.color;
                int p = 0;
                if ((frame % 2 != 0) && VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.spin_alpha_0)
                    continue;
                else if ((frame % 2 != 1) && VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.spin_alpha_1)
                    continue;
                else if (current_color >= 168)
                {
                    continue;
                }
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
                    int mod_color = current_color + faction;
                    if (current_color == 80) //lights
                    {
                        mod_color = 168 + faction + (frame % 4) * 8;
                    }
                    for (int j = 0; j < 4; j++)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            p = voxelToPixelLarge(i, j, vx.x, vx.y, vx.z, mod_color, bmpData.Stride, jitter);

                            if (argbValues[p] == 0)
                            {
                                argbValues[p] = VoxelLogic.xrendered[mod_color][i + j * 16];
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

        private static Bitmap renderHugeSmart(MagicaVoxelData[] voxels, int facing, int faction, int frame)
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
            bool[] barePositions = new bool[numBytes];
            barePositions.Fill<bool>(false);


            int xSize = 60 * 2, ySize = 60 * 2;
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

            // 0 1 2 3 4 5 6 7
            // 0 1 2 3 4 3 2 1
            int jitter = ((frame % 8 > 4) ? 4 - ((frame % 8) ^ 4) : frame % 8);
            foreach (MagicaVoxelData vx in vls.OrderByDescending(v => v.x * 128 - v.y + v.z * 128 * 128 - ((v.color == 249 - 96) ? 128 * 128 * 128 : 0))) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;
                int p = 0;
                if ((frame % 2 != 0) && VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.spin_alpha_0)
                    continue;
                else if ((frame % 2 != 1) && VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.spin_alpha_1)
                    continue;
                else if (current_color >= 168)
                {
                    continue;
                }
                else if (current_color == 152 || current_color == 160 || current_color == 136)// || current_color == 104 || current_color == 112) // || current_color == 80
                {
                    if (current_color == 136 && r.Next(7) < 2)
                        continue;
                    int mod_color = current_color + faction;

                    for (int j = 0; j < 4; j++)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            p = voxelToPixelHuge(i, j, vx.x, vx.y, vx.z, current_color + faction, bmpData.Stride, jitter);
                            if (argbValues[p] == 0
                                /*
                                &&
                                bareValues[4 * ((vx.x + vx.y) * 2 + 4 + ((current_color == 136) ? jitter - 1 : 0))
                                + i +
                                bmpData.Stride * (600 - 120 - vx.y + vx.x - vx.z * 3 - ((VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.flat_alpha) ? -2 : jitter) + j)] == 0
                                 */)
                            {
                                argbValues[p] = VoxelLogic.xrendered[mod_color][i + j * 16];
                                /*
                                bareValues[p] = VoxelLogic.xrendered[mod_color][i + j * 16];
                                barePositions[p] = true;
                                */
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
                            p = voxelToPixelHuge(i, j, vx.x, vx.y, vx.z, current_color + faction, bmpData.Stride, jitter);
                            if (shadowValues[p] == 0)
                            {
                                shadowValues[p] = VoxelLogic.xrendered[current_color][i + j * 16];
                            }
                        }
                    }
                }
                else
                {
                    int mod_color = current_color + faction;
                    if (current_color == 80) //lights
                    {
                        mod_color = 168 + faction + (frame % 4) * 8;
                    }
                    for (int j = 0; j < 4; j++)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            p = voxelToPixelHuge(i, j, vx.x, vx.y, vx.z, current_color + faction, bmpData.Stride, jitter);
                            if (argbValues[p] == 0)
                            {
                                argbValues[p] = VoxelLogic.xrendered[mod_color][i + j * 16];
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
            if (maxFrames >= 8) jitter = ((frame % 8 > 4) ? 4 - ((frame % 8) ^ 4) : frame % 8);

            foreach (MagicaVoxelData vx in vls.OrderByDescending(v => v.x * 64 - v.y + v.z * 64 * 128 - (((253 - v.color) / 4 == 25) ? 64 * 128 * 64 : 0))) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = (253 - vx.color) / 4;
                int p = 0;
                if (current_color >= VoxelLogic.wcolorcount)
                    continue;
                if ((frame % 2 != 0) && VoxelLogic.wcolors[current_color][3] == VoxelLogic.spin_alpha_0)
                    continue;
                else if ((frame % 2 != 1) && VoxelLogic.wcolors[current_color][3] == VoxelLogic.spin_alpha_1)
                    continue;
                else if (VoxelLogic.wcolors[current_color][3] == 0F)
                    continue;
                else if (current_color >= 17 && current_color <= 20)
                {
                    int mod_color = current_color;
                    if (mod_color == 17 && r.Next(7) < 2) //smoke
                        continue;
                    if (current_color == 18) //yellow fire
                    {
                        if (r.Next(3) > 0)
                        {
                            mod_color += r.Next(3);
                        }
                    }
                    else if (current_color == 19) // orange fire
                    {
                        if (r.Next(5) < 4)
                        {
                            mod_color -= r.Next(3);
                        }
                    }
                    for (int j = 0; j < 4; j++)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            /*
                             &&
                                bareValues[4 * ((vx.x + vx.y) * 2 + 4 + ((current_color == 136) ? jitter - 1 : 0))
                                + i +
                                bmpData.Stride * (300 - 60 - vx.y + vx.x - vx.z * 3 - ((VoxelLogic.xcolors[current_color][3] == VoxelLogic.flat_alpha) ? -2 : jitter) + j)] == 0
                             */
                            p = voxelToPixelLargeW(i, j, vx.x, vx.y, vx.z, mod_color, bmpData.Stride, jitter, still);
                            if (argbValues[p] == 0)
                            {
                                argbValues[p] = VoxelLogic.wcurrent[mod_color][i + j * 16];
                                //bareValues[p] = VoxelLogic.wcurrent[mod_color][i + j * 16];
                                barePositions[p] = true;
                            }
                        }
                    }
                }
                else if (current_color == 25)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            p = voxelToPixelLargeW(i, j, vx.x, vx.y, vx.z, current_color, bmpData.Stride, jitter, still);

                            if (shadowValues[p] == 0)
                            {
                                shadowValues[p] = VoxelLogic.wcurrent[current_color][i + j * 16];
                            }
                        }
                    }
                }
                else
                {
                    int mod_color = current_color;
                    if (current_color >= 21 && current_color <= 24) //lights
                    {
                        mod_color = 21 + frame % 4;
                    }
                    for (int j = 0; j < 4; j++)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            p = voxelToPixelLargeW(i, j, vx.x, vx.y, vx.z, mod_color, bmpData.Stride, jitter, still);

                            if (argbValues[p] == 0)
                            {
                                zbuffer[p] = vx.z + vx.x - vx.y;
                                argbValues[p] = VoxelLogic.wcurrent[((current_color == 28 || current_color == 29) ? mod_color +
                                    Math.Abs((((frame % 4) / 2) + zbuffer[p] + vx.x) % (((zbuffer[p] + vx.x + vx.y + vx.z) % 4 == 0) ? 5 : 4)) : mod_color)][i + j * 16];
                            }
                        }
                    }
                }
            }
            for (int i = 3; i < numBytes; i += 4)
            {
                if (argbValues[i] > 255 * VoxelLogic.waver_alpha && barePositions[i] == false)
                {
                    if (barePositions[i + 4] == false && zbuffer[i] - 2 > zbuffer[i + 4]) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = 0; argbValues[i + 4 - 2] = 0; argbValues[i + 4 - 3] = 0; }
                    if (barePositions[i - 4] == false && zbuffer[i] - 2 > zbuffer[i - 4]) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = 0; argbValues[i - 4 - 2] = 0; argbValues[i - 4 - 3] = 0; }
                    if (barePositions[i + bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride]) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = 0; argbValues[i + bmpData.Stride - 2] = 0; argbValues[i + bmpData.Stride - 3] = 0; }
                    if (barePositions[i - bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride]) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = 0; argbValues[i - bmpData.Stride - 2] = 0; argbValues[i - bmpData.Stride - 3] = 0; }
                    if (barePositions[i + bmpData.Stride + 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 4]) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = 0; argbValues[i + bmpData.Stride + 4 - 2] = 0; argbValues[i + bmpData.Stride + 4 - 3] = 0; }
                    if (barePositions[i - bmpData.Stride - 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 4]) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = 0; argbValues[i - bmpData.Stride - 4 - 2] = 0; argbValues[i - bmpData.Stride - 4 - 3] = 0; }
                    if (barePositions[i + bmpData.Stride - 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 4]) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = 0; argbValues[i + bmpData.Stride - 4 - 2] = 0; argbValues[i + bmpData.Stride - 4 - 3] = 0; }
                    if (barePositions[i - bmpData.Stride + 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 4]) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = 0; argbValues[i - bmpData.Stride + 4 - 2] = 0; argbValues[i - bmpData.Stride + 4 - 3] = 0; }

                    if (barePositions[i + 8] == false && zbuffer[i] - 2 > zbuffer[i + 8]) { argbValues[i + 8] = 255; argbValues[i + 8 - 1] = 0; argbValues[i + 8 - 2] = 0; argbValues[i + 8 - 3] = 0; }
                    if (barePositions[i - 8] == false && zbuffer[i] - 2 > zbuffer[i - 8]) { argbValues[i - 8] = 255; argbValues[i - 8 - 1] = 0; argbValues[i - 8 - 2] = 0; argbValues[i - 8 - 3] = 0; }
                    if (barePositions[i + bmpData.Stride * 2] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2]) { argbValues[i + bmpData.Stride * 2] = 255; argbValues[i + bmpData.Stride * 2 - 1] = 0; argbValues[i + bmpData.Stride * 2 - 2] = 0; argbValues[i + bmpData.Stride * 2 - 3] = 0; }
                    if (barePositions[i - bmpData.Stride * 2] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2]) { argbValues[i - bmpData.Stride * 2] = 255; argbValues[i - bmpData.Stride * 2 - 1] = 0; argbValues[i - bmpData.Stride * 2 - 2] = 0; argbValues[i - bmpData.Stride * 2 - 3] = 0; }
                    if (barePositions[i + bmpData.Stride + 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 8]) { argbValues[i + bmpData.Stride + 8] = 255; argbValues[i + bmpData.Stride + 8 - 1] = 0; argbValues[i + bmpData.Stride + 8 - 2] = 0; argbValues[i + bmpData.Stride + 8 - 3] = 0; }
                    if (barePositions[i - bmpData.Stride + 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 8]) { argbValues[i - bmpData.Stride + 8] = 255; argbValues[i - bmpData.Stride + 8 - 1] = 0; argbValues[i - bmpData.Stride + 8 - 2] = 0; argbValues[i - bmpData.Stride + 8 - 3] = 0; }
                    if (barePositions[i + bmpData.Stride - 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 8]) { argbValues[i + bmpData.Stride - 8] = 255; argbValues[i + bmpData.Stride - 8 - 1] = 0; argbValues[i + bmpData.Stride - 8 - 2] = 0; argbValues[i + bmpData.Stride - 8 - 3] = 0; }
                    if (barePositions[i - bmpData.Stride - 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 8]) { argbValues[i - bmpData.Stride - 8] = 255; argbValues[i - bmpData.Stride - 8 - 1] = 0; argbValues[i - bmpData.Stride - 8 - 2] = 0; argbValues[i - bmpData.Stride - 8 - 3] = 0; }
                    if (barePositions[i + bmpData.Stride * 2 + 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 + 8]) { argbValues[i + bmpData.Stride * 2 + 8] = 255; argbValues[i + bmpData.Stride * 2 + 8 - 1] = 0; argbValues[i + bmpData.Stride * 2 + 8 - 2] = 0; argbValues[i + bmpData.Stride * 2 + 8 - 3] = 0; }
                    if (barePositions[i + bmpData.Stride * 2 + 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 + 4]) { argbValues[i + bmpData.Stride * 2 + 4] = 255; argbValues[i + bmpData.Stride * 2 + 4 - 1] = 0; argbValues[i + bmpData.Stride * 2 + 4 - 2] = 0; argbValues[i + bmpData.Stride * 2 + 4 - 3] = 0; }
                    if (barePositions[i + bmpData.Stride * 2 - 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 - 4]) { argbValues[i + bmpData.Stride * 2 - 4] = 255; argbValues[i + bmpData.Stride * 2 - 4 - 1] = 0; argbValues[i + bmpData.Stride * 2 - 4 - 2] = 0; argbValues[i + bmpData.Stride * 2 - 4 - 3] = 0; }
                    if (barePositions[i + bmpData.Stride * 2 - 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 - 8]) { argbValues[i + bmpData.Stride * 2 - 8] = 255; argbValues[i + bmpData.Stride * 2 - 8 - 1] = 0; argbValues[i + bmpData.Stride * 2 - 8 - 2] = 0; argbValues[i + bmpData.Stride * 2 - 8 - 3] = 0; }
                    if (barePositions[i - bmpData.Stride * 2 + 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 + 8]) { argbValues[i - bmpData.Stride * 2 + 8] = 255; argbValues[i - bmpData.Stride * 2 + 8 - 1] = 0; argbValues[i - bmpData.Stride * 2 + 8 - 2] = 0; argbValues[i - bmpData.Stride * 2 + 8 - 3] = 0; }
                    if (barePositions[i - bmpData.Stride * 2 + 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 + 4]) { argbValues[i - bmpData.Stride * 2 + 4] = 255; argbValues[i - bmpData.Stride * 2 + 4 - 1] = 0; argbValues[i - bmpData.Stride * 2 + 4 - 2] = 0; argbValues[i - bmpData.Stride * 2 + 4 - 3] = 0; }
                    if (barePositions[i - bmpData.Stride * 2 - 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 - 4]) { argbValues[i - bmpData.Stride * 2 - 4] = 255; argbValues[i - bmpData.Stride * 2 - 4 - 1] = 0; argbValues[i - bmpData.Stride * 2 - 4 - 2] = 0; argbValues[i - bmpData.Stride * 2 - 4 - 3] = 0; }
                    if (barePositions[i - bmpData.Stride * 2 - 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 - 8]) { argbValues[i - bmpData.Stride * 2 - 8] = 255; argbValues[i - bmpData.Stride * 2 - 8 - 1] = 0; argbValues[i - bmpData.Stride * 2 - 8 - 2] = 0; argbValues[i - bmpData.Stride * 2 - 8 - 3] = 0; }
                }
            }

            for (int i = 3; i < numBytes; i += 4)
            {
                if (argbValues[i] > 0 && argbValues[i] <= 255 * VoxelLogic.flat_alpha) //outlineValues[i] > 0 ||
                    argbValues[i] = 255;
            }

            for (int i = 3; i < numBytes; i += 4)
            {
                /*                if (argbValues[i] == 0 && bareValues[i] > 0)
                                {
                                    argbValues[i - 3] = bareValues[i - 3];
                                    argbValues[i - 2] = bareValues[i - 2];
                                    argbValues[i - 1] = bareValues[i - 1];
                                    argbValues[i - 0] = bareValues[i - 0];
                                }*/
                if (argbValues[i] == 0 && shadowValues[i] > 0)
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
            bool[] barePositions = new bool[numBytes];
            barePositions.Fill<bool>(false);
            int xSize = 120, ySize = 120;
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
            if (maxFrames >= 8) jitter = ((frame % 8 > 4) ? 4 - ((frame % 8) ^ 4) : frame % 8);
            if (still) jitter = 0;
            foreach (MagicaVoxelData vx in vls.OrderByDescending(v => v.x * 128 - v.y + v.z * 128 * 128 - (((253 - v.color) / 4 == 25) ? 128 * 128 * 128 : 0))) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = (253 - vx.color) / 4;
                int p = 0;
                if (current_color >= VoxelLogic.wcolorcount)
                    continue;
                if ((frame % 2 != 0) && VoxelLogic.wcolors[current_color][3] == VoxelLogic.spin_alpha_0)
                    continue;
                else if ((frame % 2 != 1) && VoxelLogic.wcolors[current_color][3] == VoxelLogic.spin_alpha_1)
                    continue;
                else if (VoxelLogic.wcolors[current_color][3] == 0F)
                    continue;
                else if (current_color >= 17 && current_color <= 20)
                {
                    int mod_color = current_color;
                    if (mod_color == 17 && r.Next(7) < 2) //smoke
                        continue;
                    if (current_color == 18) //yellow fire
                    {
                        if (r.Next(3) > 0)
                        {
                            mod_color += r.Next(3);
                        }
                    }
                    else if (current_color == 19) // orange fire
                    {
                        if (r.Next(5) < 4)
                        {
                            mod_color -= r.Next(3);
                        }
                    }
                    for (int j = 0; j < 4; j++)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            /*
                             &&
                                bareValues[4 * ((vx.x + vx.y) * 2 + 4 + ((current_color == 136) ? jitter - 1 : 0))
                                + i +
                                bmpData.Stride * (300 - 60 - vx.y + vx.x - vx.z * 3 - ((VoxelLogic.xcolors[current_color][3] == VoxelLogic.flat_alpha) ? -2 : jitter) + j)] == 0
                             */
                            p = voxelToPixelHugeW(i, j, vx.x, vx.y, vx.z, mod_color, bmpData.Stride, jitter, still);
                            if (argbValues[p] == 0)
                            {
                                argbValues[p] = VoxelLogic.wcurrent[mod_color][i + j * 16];
                                //bareValues[p] = VoxelLogic.wcurrent[mod_color][i + j * 16];
                                barePositions[p] = true;
                            }
                        }
                    }
                }
                else if (current_color == 25)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            p = voxelToPixelHugeW(i, j, vx.x, vx.y, vx.z, current_color, bmpData.Stride, jitter, still);

                            if (shadowValues[p] == 0)
                            {
                                shadowValues[p] = VoxelLogic.wcurrent[current_color][i + j * 16];
                            }
                        }
                    }
                }
                else
                {
                    int mod_color = current_color;
                    if (current_color >= 21 && current_color <= 24) //lights
                    {
                        mod_color = 21 + frame % 4;
                    }
                    for (int j = 0; j < 4; j++)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            p = voxelToPixelHugeW(i, j, vx.x, vx.y, vx.z, mod_color, bmpData.Stride, jitter, still);

                            if (argbValues[p] == 0)
                            {
                                zbuffer[p] = vx.z + vx.x - vx.y;
                                argbValues[p] = VoxelLogic.wcurrent[((current_color == 28 || current_color == 29) ? mod_color +
                                    Math.Abs((((frame % 4) / 2) + zbuffer[p] + vx.x) % (((zbuffer[p] + vx.x + vx.y + vx.z) % 4 == 0) ? 5 : 4)) : mod_color)][i + j * 16];

                            }
                        }
                    }
                }
            }
            for (int i = 3; i < numBytes; i += 4)
            {
                if (argbValues[i] > 255 * VoxelLogic.waver_alpha && barePositions[i] == false)
                {
                    if (barePositions[i + 4] == false && zbuffer[i] - 2 > zbuffer[i + 4]) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = 0; argbValues[i + 4 - 2] = 0; argbValues[i + 4 - 3] = 0; }
                    if (barePositions[i - 4] == false && zbuffer[i] - 2 > zbuffer[i - 4]) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = 0; argbValues[i - 4 - 2] = 0; argbValues[i - 4 - 3] = 0; }
                    if (barePositions[i + bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride]) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = 0; argbValues[i + bmpData.Stride - 2] = 0; argbValues[i + bmpData.Stride - 3] = 0; }
                    if (barePositions[i - bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride]) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = 0; argbValues[i - bmpData.Stride - 2] = 0; argbValues[i - bmpData.Stride - 3] = 0; }
                    if (barePositions[i + bmpData.Stride + 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 4]) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = 0; argbValues[i + bmpData.Stride + 4 - 2] = 0; argbValues[i + bmpData.Stride + 4 - 3] = 0; }
                    if (barePositions[i - bmpData.Stride - 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 4]) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = 0; argbValues[i - bmpData.Stride - 4 - 2] = 0; argbValues[i - bmpData.Stride - 4 - 3] = 0; }
                    if (barePositions[i + bmpData.Stride - 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 4]) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = 0; argbValues[i + bmpData.Stride - 4 - 2] = 0; argbValues[i + bmpData.Stride - 4 - 3] = 0; }
                    if (barePositions[i - bmpData.Stride + 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 4]) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = 0; argbValues[i - bmpData.Stride + 4 - 2] = 0; argbValues[i - bmpData.Stride + 4 - 3] = 0; }

                    if (barePositions[i + 8] == false && zbuffer[i] - 2 > zbuffer[i + 8]) { argbValues[i + 8] = 255; argbValues[i + 8 - 1] = 0; argbValues[i + 8 - 2] = 0; argbValues[i + 8 - 3] = 0; }
                    if (barePositions[i - 8] == false && zbuffer[i] - 2 > zbuffer[i - 8]) { argbValues[i - 8] = 255; argbValues[i - 8 - 1] = 0; argbValues[i - 8 - 2] = 0; argbValues[i - 8 - 3] = 0; }
                    if (barePositions[i + bmpData.Stride * 2] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2]) { argbValues[i + bmpData.Stride * 2] = 255; argbValues[i + bmpData.Stride * 2 - 1] = 0; argbValues[i + bmpData.Stride * 2 - 2] = 0; argbValues[i + bmpData.Stride * 2 - 3] = 0; }
                    if (barePositions[i - bmpData.Stride * 2] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2]) { argbValues[i - bmpData.Stride * 2] = 255; argbValues[i - bmpData.Stride * 2 - 1] = 0; argbValues[i - bmpData.Stride * 2 - 2] = 0; argbValues[i - bmpData.Stride * 2 - 3] = 0; }
                    if (barePositions[i + bmpData.Stride + 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 8]) { argbValues[i + bmpData.Stride + 8] = 255; argbValues[i + bmpData.Stride + 8 - 1] = 0; argbValues[i + bmpData.Stride + 8 - 2] = 0; argbValues[i + bmpData.Stride + 8 - 3] = 0; }
                    if (barePositions[i - bmpData.Stride + 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 8]) { argbValues[i - bmpData.Stride + 8] = 255; argbValues[i - bmpData.Stride + 8 - 1] = 0; argbValues[i - bmpData.Stride + 8 - 2] = 0; argbValues[i - bmpData.Stride + 8 - 3] = 0; }
                    if (barePositions[i + bmpData.Stride - 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 8]) { argbValues[i + bmpData.Stride - 8] = 255; argbValues[i + bmpData.Stride - 8 - 1] = 0; argbValues[i + bmpData.Stride - 8 - 2] = 0; argbValues[i + bmpData.Stride - 8 - 3] = 0; }
                    if (barePositions[i - bmpData.Stride - 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 8]) { argbValues[i - bmpData.Stride - 8] = 255; argbValues[i - bmpData.Stride - 8 - 1] = 0; argbValues[i - bmpData.Stride - 8 - 2] = 0; argbValues[i - bmpData.Stride - 8 - 3] = 0; }
                    if (barePositions[i + bmpData.Stride * 2 + 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 + 8]) { argbValues[i + bmpData.Stride * 2 + 8] = 255; argbValues[i + bmpData.Stride * 2 + 8 - 1] = 0; argbValues[i + bmpData.Stride * 2 + 8 - 2] = 0; argbValues[i + bmpData.Stride * 2 + 8 - 3] = 0; }
                    if (barePositions[i + bmpData.Stride * 2 + 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 + 4]) { argbValues[i + bmpData.Stride * 2 + 4] = 255; argbValues[i + bmpData.Stride * 2 + 4 - 1] = 0; argbValues[i + bmpData.Stride * 2 + 4 - 2] = 0; argbValues[i + bmpData.Stride * 2 + 4 - 3] = 0; }
                    if (barePositions[i + bmpData.Stride * 2 - 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 - 4]) { argbValues[i + bmpData.Stride * 2 - 4] = 255; argbValues[i + bmpData.Stride * 2 - 4 - 1] = 0; argbValues[i + bmpData.Stride * 2 - 4 - 2] = 0; argbValues[i + bmpData.Stride * 2 - 4 - 3] = 0; }
                    if (barePositions[i + bmpData.Stride * 2 - 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 - 8]) { argbValues[i + bmpData.Stride * 2 - 8] = 255; argbValues[i + bmpData.Stride * 2 - 8 - 1] = 0; argbValues[i + bmpData.Stride * 2 - 8 - 2] = 0; argbValues[i + bmpData.Stride * 2 - 8 - 3] = 0; }
                    if (barePositions[i - bmpData.Stride * 2 + 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 + 8]) { argbValues[i - bmpData.Stride * 2 + 8] = 255; argbValues[i - bmpData.Stride * 2 + 8 - 1] = 0; argbValues[i - bmpData.Stride * 2 + 8 - 2] = 0; argbValues[i - bmpData.Stride * 2 + 8 - 3] = 0; }
                    if (barePositions[i - bmpData.Stride * 2 + 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 + 4]) { argbValues[i - bmpData.Stride * 2 + 4] = 255; argbValues[i - bmpData.Stride * 2 + 4 - 1] = 0; argbValues[i - bmpData.Stride * 2 + 4 - 2] = 0; argbValues[i - bmpData.Stride * 2 + 4 - 3] = 0; }
                    if (barePositions[i - bmpData.Stride * 2 - 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 - 4]) { argbValues[i - bmpData.Stride * 2 - 4] = 255; argbValues[i - bmpData.Stride * 2 - 4 - 1] = 0; argbValues[i - bmpData.Stride * 2 - 4 - 2] = 0; argbValues[i - bmpData.Stride * 2 - 4 - 3] = 0; }
                    if (barePositions[i - bmpData.Stride * 2 - 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 - 8]) { argbValues[i - bmpData.Stride * 2 - 8] = 255; argbValues[i - bmpData.Stride * 2 - 8 - 1] = 0; argbValues[i - bmpData.Stride * 2 - 8 - 2] = 0; argbValues[i - bmpData.Stride * 2 - 8 - 3] = 0; }
                }
            }

            for (int i = 3; i < numBytes; i += 4)
            {
                if (argbValues[i] > 0 && argbValues[i] <= 255 * VoxelLogic.flat_alpha) //outlineValues[i] > 0 ||
                    argbValues[i] = 255;
            }

            for (int i = 3; i < numBytes; i += 4)
            {
                /*                if (argbValues[i] == 0 && bareValues[i] > 0)
                                {
                                    argbValues[i - 3] = bareValues[i - 3];
                                    argbValues[i - 2] = bareValues[i - 2];
                                    argbValues[i - 1] = bareValues[i - 1];
                                    argbValues[i - 0] = bareValues[i - 0];
                                }*/
                if (argbValues[i] == 0 && shadowValues[i] > 0)
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
        private static Bitmap renderWSmartMassive(MagicaVoxelData[] voxels, int facing, int palette, int frame, int maxFrames, bool still)
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
            bool[] barePositions = new bool[numBytes];
            barePositions.Fill<bool>(false);
            int xSize = 160, ySize = 160;
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
            if (maxFrames >= 8) jitter = ((frame % 8 > 4) ? 4 - ((frame % 8) ^ 4) : frame % 8);
            if (still) jitter = 0;
            foreach (MagicaVoxelData vx in vls.OrderByDescending(v => v.x * 256 - v.y + v.z * 256 * 256 - (((253 - v.color) / 4 == 25) ? 256 * 256 * 256 : 0))) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = (253 - vx.color) / 4;
                int p = 0;
                if (current_color >= VoxelLogic.wcolorcount)
                    continue;
                if ((frame % 2 != 0) && VoxelLogic.wcolors[current_color][3] == VoxelLogic.spin_alpha_0)
                    continue;
                else if ((frame % 2 != 1) && VoxelLogic.wcolors[current_color][3] == VoxelLogic.spin_alpha_1)
                    continue;
                else if (VoxelLogic.wcolors[current_color][3] == 0F)
                    continue;
                else if (current_color >= 17 && current_color <= 20)
                {
                    int mod_color = current_color;
                    if (mod_color == 17 && r.Next(7) < 2) //smoke
                        continue;
                    if (current_color == 18) //yellow fire
                    {
                        if (r.Next(3) > 0)
                        {
                            mod_color += r.Next(3);
                        }
                    }
                    else if (current_color == 19) // orange fire
                    {
                        if (r.Next(5) < 4)
                        {
                            mod_color -= r.Next(3);
                        }
                    }
                    for (int j = 0; j < 4; j++)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            /*
                             &&
                                bareValues[4 * ((vx.x + vx.y) * 2 + 4 + ((current_color == 136) ? jitter - 1 : 0))
                                + i +
                                bmpData.Stride * (300 - 60 - vx.y + vx.x - vx.z * 3 - ((VoxelLogic.xcolors[current_color][3] == VoxelLogic.flat_alpha) ? -2 : jitter) + j)] == 0
                             */
                            p = voxelToPixelMassiveW(i, j, vx.x, vx.y, vx.z, mod_color, bmpData.Stride, jitter, still);
                            if (argbValues[p] == 0)
                            {
                                argbValues[p] = VoxelLogic.wcurrent[mod_color][i + j * 16];
                                //bareValues[p] = VoxelLogic.wcurrent[mod_color][i + j * 16];
                                barePositions[p] = true;
                            }
                        }
                    }
                }
                else if (current_color == 25)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            p = voxelToPixelMassiveW(i, j, vx.x, vx.y, vx.z, current_color, bmpData.Stride, jitter, still);

                            if (shadowValues[p] == 0)
                            {
                                shadowValues[p] = VoxelLogic.wcurrent[current_color][i + j * 16];
                            }
                        }
                    }
                }
                else
                {
                    int mod_color = current_color;
                    if (current_color >= 21 && current_color <= 24) //lights
                    {
                        mod_color = 21 + frame % 4;
                    }
                    for (int j = 0; j < 4; j++)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            p = voxelToPixelMassiveW(i, j, vx.x, vx.y, vx.z, mod_color, bmpData.Stride, jitter, still);

                            if (argbValues[p] == 0)
                            {
                                zbuffer[p] = vx.z + vx.x - vx.y;
                                argbValues[p] = VoxelLogic.wcurrent[((current_color == 28 || current_color == 29) ? mod_color +
                                    Math.Abs((((frame % 4) / 2) + zbuffer[p] + vx.x) % (((zbuffer[p] + vx.x + vx.y + vx.z) % 4 == 0) ? 5 : 4)) : mod_color)][i + j * 16];

                            }
                        }
                    }
                }
            }
            for (int i = 3; i < numBytes; i += 4)
            {
                if (argbValues[i] > 255 * VoxelLogic.waver_alpha && barePositions[i] == false)
                {
                    if (barePositions[i + 4] == false && zbuffer[i] - 2 > zbuffer[i + 4]) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = 0; argbValues[i + 4 - 2] = 0; argbValues[i + 4 - 3] = 0; }
                    if (barePositions[i - 4] == false && zbuffer[i] - 2 > zbuffer[i - 4]) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = 0; argbValues[i - 4 - 2] = 0; argbValues[i - 4 - 3] = 0; }
                    if (barePositions[i + bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride]) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = 0; argbValues[i + bmpData.Stride - 2] = 0; argbValues[i + bmpData.Stride - 3] = 0; }
                    if (barePositions[i - bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride]) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = 0; argbValues[i - bmpData.Stride - 2] = 0; argbValues[i - bmpData.Stride - 3] = 0; }
                    if (barePositions[i + bmpData.Stride + 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 4]) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = 0; argbValues[i + bmpData.Stride + 4 - 2] = 0; argbValues[i + bmpData.Stride + 4 - 3] = 0; }
                    if (barePositions[i - bmpData.Stride - 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 4]) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = 0; argbValues[i - bmpData.Stride - 4 - 2] = 0; argbValues[i - bmpData.Stride - 4 - 3] = 0; }
                    if (barePositions[i + bmpData.Stride - 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 4]) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = 0; argbValues[i + bmpData.Stride - 4 - 2] = 0; argbValues[i + bmpData.Stride - 4 - 3] = 0; }
                    if (barePositions[i - bmpData.Stride + 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 4]) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = 0; argbValues[i - bmpData.Stride + 4 - 2] = 0; argbValues[i - bmpData.Stride + 4 - 3] = 0; }

                    if (barePositions[i + 8] == false && zbuffer[i] - 2 > zbuffer[i + 8]) { argbValues[i + 8] = 255; argbValues[i + 8 - 1] = 0; argbValues[i + 8 - 2] = 0; argbValues[i + 8 - 3] = 0; }
                    if (barePositions[i - 8] == false && zbuffer[i] - 2 > zbuffer[i - 8]) { argbValues[i - 8] = 255; argbValues[i - 8 - 1] = 0; argbValues[i - 8 - 2] = 0; argbValues[i - 8 - 3] = 0; }
                    if (barePositions[i + bmpData.Stride * 2] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2]) { argbValues[i + bmpData.Stride * 2] = 255; argbValues[i + bmpData.Stride * 2 - 1] = 0; argbValues[i + bmpData.Stride * 2 - 2] = 0; argbValues[i + bmpData.Stride * 2 - 3] = 0; }
                    if (barePositions[i - bmpData.Stride * 2] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2]) { argbValues[i - bmpData.Stride * 2] = 255; argbValues[i - bmpData.Stride * 2 - 1] = 0; argbValues[i - bmpData.Stride * 2 - 2] = 0; argbValues[i - bmpData.Stride * 2 - 3] = 0; }
                    if (barePositions[i + bmpData.Stride + 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 8]) { argbValues[i + bmpData.Stride + 8] = 255; argbValues[i + bmpData.Stride + 8 - 1] = 0; argbValues[i + bmpData.Stride + 8 - 2] = 0; argbValues[i + bmpData.Stride + 8 - 3] = 0; }
                    if (barePositions[i - bmpData.Stride + 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 8]) { argbValues[i - bmpData.Stride + 8] = 255; argbValues[i - bmpData.Stride + 8 - 1] = 0; argbValues[i - bmpData.Stride + 8 - 2] = 0; argbValues[i - bmpData.Stride + 8 - 3] = 0; }
                    if (barePositions[i + bmpData.Stride - 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 8]) { argbValues[i + bmpData.Stride - 8] = 255; argbValues[i + bmpData.Stride - 8 - 1] = 0; argbValues[i + bmpData.Stride - 8 - 2] = 0; argbValues[i + bmpData.Stride - 8 - 3] = 0; }
                    if (barePositions[i - bmpData.Stride - 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 8]) { argbValues[i - bmpData.Stride - 8] = 255; argbValues[i - bmpData.Stride - 8 - 1] = 0; argbValues[i - bmpData.Stride - 8 - 2] = 0; argbValues[i - bmpData.Stride - 8 - 3] = 0; }
                    if (barePositions[i + bmpData.Stride * 2 + 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 + 8]) { argbValues[i + bmpData.Stride * 2 + 8] = 255; argbValues[i + bmpData.Stride * 2 + 8 - 1] = 0; argbValues[i + bmpData.Stride * 2 + 8 - 2] = 0; argbValues[i + bmpData.Stride * 2 + 8 - 3] = 0; }
                    if (barePositions[i + bmpData.Stride * 2 + 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 + 4]) { argbValues[i + bmpData.Stride * 2 + 4] = 255; argbValues[i + bmpData.Stride * 2 + 4 - 1] = 0; argbValues[i + bmpData.Stride * 2 + 4 - 2] = 0; argbValues[i + bmpData.Stride * 2 + 4 - 3] = 0; }
                    if (barePositions[i + bmpData.Stride * 2 - 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 - 4]) { argbValues[i + bmpData.Stride * 2 - 4] = 255; argbValues[i + bmpData.Stride * 2 - 4 - 1] = 0; argbValues[i + bmpData.Stride * 2 - 4 - 2] = 0; argbValues[i + bmpData.Stride * 2 - 4 - 3] = 0; }
                    if (barePositions[i + bmpData.Stride * 2 - 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 - 8]) { argbValues[i + bmpData.Stride * 2 - 8] = 255; argbValues[i + bmpData.Stride * 2 - 8 - 1] = 0; argbValues[i + bmpData.Stride * 2 - 8 - 2] = 0; argbValues[i + bmpData.Stride * 2 - 8 - 3] = 0; }
                    if (barePositions[i - bmpData.Stride * 2 + 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 + 8]) { argbValues[i - bmpData.Stride * 2 + 8] = 255; argbValues[i - bmpData.Stride * 2 + 8 - 1] = 0; argbValues[i - bmpData.Stride * 2 + 8 - 2] = 0; argbValues[i - bmpData.Stride * 2 + 8 - 3] = 0; }
                    if (barePositions[i - bmpData.Stride * 2 + 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 + 4]) { argbValues[i - bmpData.Stride * 2 + 4] = 255; argbValues[i - bmpData.Stride * 2 + 4 - 1] = 0; argbValues[i - bmpData.Stride * 2 + 4 - 2] = 0; argbValues[i - bmpData.Stride * 2 + 4 - 3] = 0; }
                    if (barePositions[i - bmpData.Stride * 2 - 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 - 4]) { argbValues[i - bmpData.Stride * 2 - 4] = 255; argbValues[i - bmpData.Stride * 2 - 4 - 1] = 0; argbValues[i - bmpData.Stride * 2 - 4 - 2] = 0; argbValues[i - bmpData.Stride * 2 - 4 - 3] = 0; }
                    if (barePositions[i - bmpData.Stride * 2 - 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 - 8]) { argbValues[i - bmpData.Stride * 2 - 8] = 255; argbValues[i - bmpData.Stride * 2 - 8 - 1] = 0; argbValues[i - bmpData.Stride * 2 - 8 - 2] = 0; argbValues[i - bmpData.Stride * 2 - 8 - 3] = 0; }
                }
            }

            for (int i = 3; i < numBytes; i += 4)
            {
                if (argbValues[i] > 0 && argbValues[i] <= 255 * VoxelLogic.flat_alpha) //outlineValues[i] > 0 ||
                    argbValues[i] = 255;
            }

            for (int i = 3; i < numBytes; i += 4)
            {
                /*                if (argbValues[i] == 0 && bareValues[i] > 0)
                                {
                                    argbValues[i - 3] = bareValues[i - 3];
                                    argbValues[i - 2] = bareValues[i - 2];
                                    argbValues[i - 1] = bareValues[i - 1];
                                    argbValues[i - 0] = bareValues[i - 0];
                                }*/
                if (argbValues[i] == 0 && shadowValues[i] > 0)
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

        private static Bitmap[] renderLarge(MagicaVoxelData[] voxels, int facing, int faction, int frame)
        {
            Bitmap[] b = {
            new Bitmap(248, 308, PixelFormat.Format32bppArgb),
            new Bitmap(248, 308, PixelFormat.Format32bppArgb),};

            Graphics g = Graphics.FromImage((Image)b[0]);
            Graphics gsh = Graphics.FromImage((Image)b[1]);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_soft.png");
            Image flat = new Bitmap("flat_soft.png");
            Image spin = new Bitmap("spin_soft.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 4;
            int xSize = 60, ySize = 60;
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
            int jitter = ((frame % 4) % 3) + ((frame % 4) / 3);
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 64 - v.y + v.z * 64 * 128 - ((v.color == 249 - 96) ? 64 * 128 * 64 : 0))) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;
                // Console.Write(current_color + "  ");
                if ((frame % 2 != 0) && VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.spin_alpha_0)
                    continue;
                else if ((frame % 2 != 1) && VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.spin_alpha_1)
                    continue;
                else if (current_color >= 168)
                {
                    //Console.Write(frame);
                    continue;
                }
                else if (current_color == 96)
                {

                    colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+VoxelLogic.xcolors[current_color + faction][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+VoxelLogic.xcolors[current_color + faction][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+VoxelLogic.xcolors[current_color + faction][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    gsh.DrawImage(flat,
                       new Rectangle((vx.x + vx.y) * 2 + 4, 300 - 60 - vx.y + vx.x - vx.z * 3 + 2
                           , width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                       0, 0,        // upper-left corner of source rectangle 
                       width,       // width of source rectangle
                       height,      // height of source rectangle
                       GraphicsUnit.Pixel,
                       imageAttributes);
                }
                else
                {

                    if (current_color == 80) //lights
                    {
                        float lightCalc = (0.5F - (((frame % 4) % 3) + ((frame % 4) / 3))) * 0.12F;
                        colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+VoxelLogic.xcolors[current_color + faction][0] + lightCalc,  0,  0,  0, 0},
   new float[] {0,  0.251F+VoxelLogic.xcolors[current_color + faction][1] + lightCalc,  0,  0, 0},
   new float[] {0,  0,  0.31F+VoxelLogic.xcolors[current_color + faction][2] + lightCalc,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                    }
                    else
                    {
                        colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+VoxelLogic.xcolors[current_color + faction][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+VoxelLogic.xcolors[current_color + faction][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+VoxelLogic.xcolors[current_color + faction][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                    }
                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    g.DrawImage((current_color == 80) ? spin :
                       (VoxelLogic.xcolors[current_color + faction][3] == 1F) ? image : (VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.flat_alpha) ? flat : spin,
                       new Rectangle((vx.x + vx.y) * 2 + 4 + ((current_color == 136) ? jitter - 1 : 0),
                           300 - 60 - vx.y + vx.x - vx.z * 3 - ((VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.flat_alpha) ? -2 : 0),
                           width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                       0, 0,        // upper-left corner of source rectangle 
                       width,       // width of source rectangle
                       height,      // height of source rectangle
                       GraphicsUnit.Pixel,
                       imageAttributes);
                }
            }
            return b;
        }



        public static Bitmap renderOutlineLarge(MagicaVoxelData[] voxels, int facing, int faction, int frame)
        {
            Bitmap b = new Bitmap(248, 308, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            Image image = new Bitmap("black_outline_soft.png");
            //            Image flat = new Bitmap("flat_soft.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 8;
            int height = 8;

            int xSize = 60, ySize = 60;

            float[][] colorMatrixElements = { 
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

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

            int jitter = (frame % 3) - 1;

            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 64 - v.y + v.z * 64 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128 // - ((v.color == 249 - 96) ? 32 * 128 * 32 : 0)
            {
                int current_color = 249 - vx.color;

                if ((frame % 2 != 0) && VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.spin_alpha_0)
                    continue;
                else if ((frame % 2 != 1) && VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.spin_alpha_1)
                    continue;
                else if (current_color == 120 || current_color == 152 || current_color == 160 || current_color == 136 || current_color >= 168)
                    continue;
                if (VoxelLogic.xcolors[current_color + faction][3] != VoxelLogic.flat_alpha && current_color != 120)
                {
                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    g.DrawImage(image, //((colors[current_color + faction][3] == VoxelLogic.flat_alpha) ? flat : image)
                    new Rectangle((vx.x + vx.y) * 2 + 2, //if flat use + 4
                        300 - 60 - 2 - vx.y + vx.x - vx.z * 3,// - jitter, //((colors[current_color + faction][3] == VoxelLogic.flat_alpha) ? -4 : jitter
                        width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                       0, 0,        // upper-left corner of source rectangle 
                       width,       // width of source rectangle
                       height,      // height of source rectangle
                       GraphicsUnit.Pixel,
                       imageAttributes);
                }
            }
            return b;
        }


        public static Bitmap renderOnlyTerrainColors()
        {
            Bitmap b = new Bitmap(128, 4, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            Image image = new Bitmap("cube_soft.png");
            //            Image gray = new Bitmap("cube_gray_soft.png");
            //Image reversed = new Bitmap("cube_reversed.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 4;
            float[][] flatcolors = TallPaletteDraw.flatcolors;
            for (int color = 0; color < 11; color++)
            {
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

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);
                g.DrawImage(
                  image,
                   new Rectangle(1 + 9 * color, 0,
                       width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
                imageAttributes.SetColorMatrix(
                   colorMatrixDark,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);
                g.DrawImage(
                  image,
                   new Rectangle(4 + 9 * color, 0,
                       width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
                imageAttributes.SetColorMatrix(
                   colorMatrixBright,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);
                g.DrawImage(
                  image,
                   new Rectangle(7 + 9 * color, 0,
                       width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }
        public static Bitmap renderOnlyTerrainColors(int faction)
        {
            Bitmap b = new Bitmap(128, 4, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            Bitmap image = new Bitmap("PaletteTerrain.png");
            //            Image gray = new Bitmap("cube_gray_soft.png");
            //Image reversed = new Bitmap("cube_reversed.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = image.Width;
            int height = image.Height;

            imageAttributes.SetColorMatrix(
            new ColorMatrix(new float[][]{ 
               new float[] {0.5F,  0,  0,  0, 0},
               new float[] {0,  0.5F,  0,  0, 0},
               new float[] {0,  0,  0.5F,  0, 0},
               new float[] {0,  0,     0,  1F, 0},
               new float[] {0.55F*(0.22F+VoxelLogic.xcolors[32 + faction][0]), 0.55F*(0.251F+VoxelLogic.xcolors[32 + faction][1]), 0.55F*(0.31F+VoxelLogic.xcolors[32 + faction][2]), 0, 1F}}),
                           ColorMatrixFlag.Default,
                           ColorAdjustType.Bitmap);
            g.DrawImage(image,
                   new Rectangle(0, 0, width, height),  // destination rectangle 
                //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            return b;
        }

        public static Bitmap renderOnlyColors(int faction)
        {
            Bitmap b = new Bitmap(128, 4, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_soft.png");
            Image flat = new Bitmap("flat_soft.png");
            Image spin = new Bitmap("spin_soft.png");
            Image outline = new Bitmap("black_outline_soft.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 4;
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
            //            if (maxFrames != 4)
            //              jitter = 0;
            for (int v = 0; v < 10; v++)
            {
                int current_color = v * 8;

                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+VoxelLogic.xcolors[current_color + faction][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+VoxelLogic.xcolors[current_color + faction][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+VoxelLogic.xcolors[current_color + faction][2],  0, 0},
   new float[] {0,  0,  0, VoxelLogic.xcolors[current_color + faction][3], 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);
                g.DrawImage(
                   (VoxelLogic.xcolors[current_color + faction][3] == 1F) ? image : (VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.flat_alpha) ? flat : spin,
                   new Rectangle(1 + 3 * v, 0,
                       width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            float[] lightCalcs = {
                                             (0.5F - (0 - 0)) * 0.12F,
                                             (0.5F - (1 - 0)) * 0.12F,
                                             (0.5F - (2 - 0)) * 0.12F,
                                             (0.5F - (0 + 1)) * 0.12F,};
            for (int i = 0; i < 4; i++)
            {
                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+VoxelLogic.xcolors[80 + faction][0] + lightCalcs[i],  0,  0,  0, 0},
   new float[] {0,  0.251F+VoxelLogic.xcolors[80 + faction][1] + lightCalcs[i],  0,  0, 0},
   new float[] {0,  0,  0.31F+VoxelLogic.xcolors[80 + faction][2] + lightCalcs[i],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);
                g.DrawImage(spin, new Rectangle(1 + 3 * (10 + i), 0, width, height), 0, 0, width, height, GraphicsUnit.Pixel, imageAttributes);
            }
            for (int v = 11; v < 24; v++)
            {
                int current_color = v * 8;

                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+VoxelLogic.xcolors[current_color + faction][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+VoxelLogic.xcolors[current_color + faction][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+VoxelLogic.xcolors[current_color + faction][2],  0, 0},
   new float[] {0,  0,  0, VoxelLogic.xcolors[current_color + faction][3], 0},
   new float[] {0, 0, 0, 0, 1F}});
                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);
                g.DrawImage(
                   (VoxelLogic.xcolors[current_color + faction][3] == 1F) ? image : (VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.flat_alpha) ? flat : spin,
                   new Rectangle(1 + 3 * (v + 3), 0,
                       width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}});
            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            g.DrawImage(outline,
               new Rectangle(1 + 3 * 28, 0,
                   8, 8),  // destination rectangle 
                //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
               1, 1,        // upper-left corner of source rectangle 
               8,       // width of source rectangle
               8,      // height of source rectangle
               GraphicsUnit.Pixel,
               imageAttributes);

            return b;
        }
        public static Bitmap renderOnlyColorsX(int faction)
        {
            Bitmap bmp = new Bitmap(128, 4, PixelFormat.Format32bppArgb);

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
            for (int v = 0; v < 10; v++)
            {
                int current_color = v * 8;
                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        argbValues[4 * (1 + 3 * v) + x + bmpData.Stride * y] = VoxelLogic.xrendered[current_color + faction][y * 16 + x];
                    }
                }
            }
            for (int i = 0; i < 4; i++)
            {
                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        argbValues[4 * (1 + 3 * (10 + i)) + x + bmpData.Stride * y] = VoxelLogic.xrendered[168 + i * 8 + faction][y * 16 + x];
                    }
                }
            }
            for (int v = 11; v < 21; v++)
            {
                int current_color = v * 8;

                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        argbValues[4 * (1 + 3 * (3 + v)) + x + bmpData.Stride * y] = VoxelLogic.xrendered[current_color + faction][y * 16 + x];
                    }
                }
            }

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    argbValues[4 * (1 + 3 * 24) + x + bmpData.Stride * y] = (byte)((x % 4 == 3) ? 255 : 0);
                }
            }

            Marshal.Copy(argbValues, 0, ptr, numBytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            return bmp;

        }

        private static void processUnitBasic(string u)
        {
            BinaryReader bin = new BinaryReader(File.Open(u + "_X.vox", FileMode.Open));
            MagicaVoxelData[] parsed = VoxelLogic.FromMagica(bin);

            for (int i = 0; i < 8; i++)
            {
                System.IO.Directory.CreateDirectory(u);
                for (int face = 0; face < 4; face++)
                {
                    Bitmap b = render(parsed, face, i, 0, 4)[0];
                    b.Save(u + "/color" + i + "_face" + face + "_" + Directions[face] + "_frame" + 0 + "_.png", ImageFormat.Png);
                }
            }
            bin.Close();

        }
        private static Bitmap processSingleOutlinedW(MagicaVoxelData[] parsed, int palette, int dir, int frame, int maxFrames)
        {
            Graphics g;
            Bitmap[] b;
            Bitmap o, n = new Bitmap(88, 108, PixelFormat.Format32bppArgb);

            VoxelLogic.wcolors = VoxelLogic.wpalettes[palette];
            b = renderW(parsed, dir, frame, maxFrames);
            o = renderOutlineW(parsed, dir, frame, maxFrames);
            g = Graphics.FromImage(b[1]);

            g.DrawImage(o, 0, 0);
            ImageAttributes attr = new ImageAttributes();
            g.DrawImage(b[0], 0, 0);//, new Rectangle(0, 0, 88, 108), 0, 0, 88, 108, GraphicsUnit.Pixel, attr
            //attr.SetColorKey(Color.FromArgb(255, 16, 16, 16), Color.White);
            //g = Graphics.FromImage(n);
            //g.DrawImage(b[1], new Rectangle(0, 0, 88, 108), 0, 0, 88, 108, GraphicsUnit.Pixel, attr);

            return b[1];
        }

        private static Bitmap processSingleOutlinedWDouble(MagicaVoxelData[] parsed, int palette, int dir, int frame, int maxFrames, bool still)
        {
            Graphics g;
            Bitmap b;
            Bitmap b2 = new Bitmap(88, 108, PixelFormat.Format32bppArgb);

            VoxelLogic.wcolors = VoxelLogic.wpalettes[palette];
            VoxelLogic.wcurrent = VoxelLogic.wrendered[palette];
            b = renderWSmart(parsed, dir, palette, frame, maxFrames, still);
            g = Graphics.FromImage(b);
            Graphics g2 = Graphics.FromImage(b2);
            g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g2.DrawImage(b.Clone(new Rectangle(32, 46 + 32, 88 * 2, 108 * 2), b.PixelFormat), 0, 0, 88, 108);
            g2.Dispose();
            return b2;
        }
        private static Bitmap processSingleOutlinedWQuad(MagicaVoxelData[] parsed, int palette, int dir, int frame, int maxFrames, bool still)
        {
            Graphics g;
            Bitmap b;
            Bitmap b2 = new Bitmap(168, 208, PixelFormat.Format32bppArgb);

            VoxelLogic.wcolors = VoxelLogic.wpalettes[palette];
            VoxelLogic.wcurrent = VoxelLogic.wrendered[palette];
            b = renderWSmartHuge(parsed, dir, palette, frame, maxFrames, still);
            //b.Save("palette" + palette + "/" + "Kaiju" + "_Gigantic_face" + dir + "_" + f + ".png", ImageFormat.Png);
            g = Graphics.FromImage(b);
            Graphics g2 = Graphics.FromImage(b2);
            g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g2.DrawImage(b.Clone(new Rectangle(0, 0, 248 * 2, 308 * 2), b.PixelFormat), -40, -80, 248, 308);
            g2.Dispose();
            return b2;
        }
        private static Bitmap processSingleOutlined(MagicaVoxelData[] parsed, int color, string dir, int frame, int maxFrames)
        {
            Graphics g;
            Bitmap[] b;
            Bitmap o, n = new Bitmap(88, 108, PixelFormat.Format32bppArgb);
            int d = 0;
            switch (dir)
            {
                case "SE":
                    break;
                case "SW": d = 1;
                    break;
                case "NW": d = 2;
                    break;
                case "NE": d = 3;
                    break;
                default:
                    break;
            }

            b = render(parsed, d, color, frame, maxFrames);
            o = renderOutline(parsed, d, color, frame, maxFrames);
            g = Graphics.FromImage(b[1]);

            g.DrawImage(o, 0, 0);
            ImageAttributes attr = new ImageAttributes();
            g.DrawImage(b[0], 0, 0);//, new Rectangle(0, 0, 88, 108), 0, 0, 88, 108, GraphicsUnit.Pixel, attr
            //attr.SetColorKey(Color.FromArgb(255, 16, 16, 16), Color.White);
            //g = Graphics.FromImage(n);
            //g.DrawImage(b[1], new Rectangle(0, 0, 88, 108), 0, 0, 88, 108, GraphicsUnit.Pixel, attr);

            return b[1];
        }

        private static Bitmap processSingleOutlinedDouble(MagicaVoxelData[] parsed, int color, string dir, int frame, int maxFrames, string u)
        {
            Graphics g;
            Bitmap b;
            Bitmap b2 = new Bitmap(88, 108, PixelFormat.Format32bppArgb);
            int d = 0;
            switch (dir)
            {
                case "SE":
                    break;
                case "SW": d = 1;
                    break;
                case "NW": d = 2;
                    break;
                case "NE": d = 3;
                    break;
                default:
                    break;
            }

            b = renderLargeSmart(parsed, d, color, frame, (VoxelLogic.CurrentMobilities[VoxelLogic.UnitLookup[u]] == MovementType.Immobile));
            // o = renderOutlineLarge(parsed, d, color, frame);
            g = Graphics.FromImage(b);

            //            g.DrawImage(o, 0, 0);
            //ImageAttributes attr = new ImageAttributes();
            //g.DrawImage(b[0], 0, 0);//, new Rectangle(0, 0, 88, 108), 0, 0, 88, 108, GraphicsUnit.Pixel, attr
            //attr.SetColorKey(Color.FromArgb(255, 16, 16, 16), Color.White);
            //g = Graphics.FromImage(n);
            //g.DrawImage(b[1], new Rectangle(0, 0, 88, 108), 0, 0, 88, 108, GraphicsUnit.Pixel, attr);

            Graphics g2 = Graphics.FromImage(b2);
            g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g2.DrawImage(b.Clone(new Rectangle(32, 46 + 32, 88 * 2, 108 * 2), b.PixelFormat), 0, 0, 88, 108);
            g2.Dispose();
            return b2;
        }
        private static Bitmap processSingleOutlinedHuge(MagicaVoxelData[] parsed, int color, string dir, int frame, int maxFrames)
        {
            Graphics g;
            Bitmap b;
            Bitmap b2 = new Bitmap(248, 308, PixelFormat.Format32bppArgb);
            int d = 0;
            switch (dir)
            {
                case "SE":
                    break;
                case "SW": d = 1;
                    break;
                case "NW": d = 2;
                    break;
                case "NE": d = 3;
                    break;
                default:
                    break;
            }

            b = renderHugeSmart(parsed, d, color, frame);
            // o = renderOutlineLarge(parsed, d, color, frame);
            g = Graphics.FromImage(b);

            //            g.DrawImage(o, 0, 0);
            //ImageAttributes attr = new ImageAttributes();
            //g.DrawImage(b[0], 0, 0);//, new Rectangle(0, 0, 88, 108), 0, 0, 88, 108, GraphicsUnit.Pixel, attr
            //attr.SetColorKey(Color.FromArgb(255, 16, 16, 16), Color.White);
            //g = Graphics.FromImage(n);
            //g.DrawImage(b[1], new Rectangle(0, 0, 88, 108), 0, 0, 88, 108, GraphicsUnit.Pixel, attr);

            Graphics g2 = Graphics.FromImage(b2);
            g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g2.DrawImage(b.Clone(new Rectangle(0, 0, 248 * 2, 308 * 2), b.PixelFormat), 0, 0, 248, 308);
            g2.Dispose();
            return b2;
        }

        private static void processExplosion(string u)
        {
            Console.WriteLine("Processing: " + u);
            BinaryReader bin = new BinaryReader(File.Open(u + "_X.vox", FileMode.Open));
            MagicaVoxelData[] parsed = VoxelLogic.FromMagicaRaw(bin).ToArray();
            //renderLarge(parsed, 0, 0, 0)[0].Save("junk_" + u + ".png");

            MagicaVoxelData[][] explode = FieryExplosion(parsed, ((VoxelLogic.CurrentMobilities[VoxelLogic.UnitLookup[u]] == MovementType.Immobile) ? false : true));
            string folder = ("frames");
            for (int color = 0; color < 8; color++)
            {
                for (int d = 0; d < 4; d++)
                {
                    System.IO.Directory.CreateDirectory(folder); //("color" + i);

                    for (int frame = 0; frame < 8; frame++)
                    {
                        Graphics g;
                        Bitmap[] b;
                        Bitmap o;

                        b = renderLarge(explode[frame], d, color, frame);
                        o = renderOutlineLarge(explode[frame], d, color, frame);
                        g = Graphics.FromImage(b[1]);

                        g.DrawImage(o, 0, 0);
                        g.DrawImage(b[0], 0, 0);

                        b[1].Save(folder + "/color" + color + "_" + u + "_face" + d + "_fiery_explode_" + (frame) + ".png", ImageFormat.Png);
                    }
                }
            }

            System.IO.Directory.CreateDirectory("gifs");
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";
            for (int i = 0; i < 8; i++)
            {
                for (int d = 0; d < 4; d++)
                {
                    for (int frame = 0; frame < 8; frame++)
                    {
                        s += folder + "/color" + i + "_" + u + "_face" + d + "_fiery_explode_" + frame + ".png ";
                    }
                }
            }
            startInfo.Arguments = "-dispose background -delay 11 -loop 0 " + s + " gifs/" + u + "_explosion_animated.gif";
            Console.WriteLine("Running convert.exe ...");
            Process.Start(startInfo).WaitForExit();

            bin.Close();
        }

        public static void processExplosionDouble(string u)
        {
            Console.WriteLine("Processing: " + u);
            BinaryReader bin = new BinaryReader(File.Open(u + "_Large_X.vox", FileMode.Open));
            MagicaVoxelData[] parsed = VoxelLogic.FromMagicaRaw(bin).ToArray();
            //renderLarge(parsed, 0, 0, 0)[0].Save("junk_" + u + ".png");

            MagicaVoxelData[][] explode = VoxelLogic.FieryExplosionDouble(parsed, true); //((CurrentMobilities[UnitLookup[u]] == MovementType.Immobile) ? false : true)
            string folder = ("frames");
            for (int color = 0; color < 8; color++)
            {
                for (int d = 0; d < 4; d++)
                {
                    System.IO.Directory.CreateDirectory(folder); //("color" + i);

                    for (int frame = 0; frame < 8; frame++)
                    {
                        Bitmap b = renderHugeSmart(explode[frame], d, color, frame);
                        Bitmap b2 = new Bitmap(248, 308, PixelFormat.Format32bppArgb);


                        //                        b.Save("temp.png", ImageFormat.Png);
                        Graphics g2 = Graphics.FromImage(b2);
                        g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                        Bitmap b3 = b.Clone(new Rectangle(0, 0, 248 * 2, 308 * 2), b.PixelFormat);
                        b.Dispose();
                        g2.DrawImage(b3, 0, 0, 248, 308);

                        b2.Save(folder + "/color" + color + "_" + u + "_Large_face" + d + "_fiery_explode_" + (frame) + ".png", ImageFormat.Png);
                        b2.Dispose();
                        g2.Dispose();
                    }
                }
            }

            System.IO.Directory.CreateDirectory("gifs");
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";
            for (int i = 0; i < 8; i++)
            {
                for (int d = 0; d < 4; d++)
                {
                    for (int frame = 0; frame < 8; frame++)
                    {
                        s += folder + "/color" + i + "_" + u + "_Large_face" + d + "_fiery_explode_" + frame + ".png ";
                    }
                }
            }
            startInfo.Arguments = "-dispose background -delay 11 -loop 0 " + s + " gifs/" + u + "_explosion_animated.gif";
            Console.WriteLine("Running convert.exe ...");
            Process.Start(startInfo).WaitForExit();

            bin.Close();
        }
        public static void processExplosionPartial(string u)
        {
            Console.WriteLine("Processing: " + u);
            BinaryReader bin = new BinaryReader(File.Open(u + "_Part_X.vox", FileMode.Open));
            MagicaVoxelData[] parsed = VoxelLogic.AssembleHeadToBody(bin, true);
            //renderLarge(parsed, 0, 0, 0)[0].Save("junk_" + u + ".png");

            MagicaVoxelData[][] explode = VoxelLogic.FieryExplosionDouble(parsed, true); //((CurrentMobilities[UnitLookup[u]] == MovementType.Immobile) ? false : true)
            string folder = ("frames");
            for (int color = 0; color < 8; color++)
            {
                for (int d = 0; d < 4; d++)
                {
                    System.IO.Directory.CreateDirectory(folder); //("color" + i);

                    for (int frame = 0; frame < 8; frame++)
                    {
                        Bitmap b = renderHugeSmart(explode[frame], d, color, frame);
                        Bitmap b2 = new Bitmap(248, 308, PixelFormat.Format32bppArgb);


                        //                        b.Save("temp.png", ImageFormat.Png);
                        Graphics g2 = Graphics.FromImage(b2);
                        g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                        Bitmap b3 = b.Clone(new Rectangle(0, 0, 248 * 2, 308 * 2), b.PixelFormat);
                        b.Dispose();
                        g2.DrawImage(b3, 0, 0, 248, 308);

                        b2.Save(folder + "/color" + color + "_" + u + "_Large_face" + d + "_fiery_explode_" + (frame) + ".png", ImageFormat.Png);
                        b2.Dispose();
                        g2.Dispose();
                        b3.Dispose();
                    }
                }
            }

            System.IO.Directory.CreateDirectory("gifs");
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";
            for (int i = 0; i < 8; i++)
            {
                for (int d = 0; d < 4; d++)
                {
                    for (int frame = 0; frame < 8; frame++)
                    {
                        s += folder + "/color" + i + "_" + u + "_Large_face" + d + "_fiery_explode_" + frame + ".png ";
                    }
                }
            }
            startInfo.Arguments = "-dispose background -delay 11 -loop 0 " + s + " gifs/" + u + "_explosion_animated.gif";
            Console.WriteLine("Running convert.exe ...");
            Process.Start(startInfo).WaitForExit();

            bin.Close();
        }
        public static void processExplosionChannelPartial(string u)
        {
            Console.WriteLine("Processing: " + u);
            BinaryReader bin = new BinaryReader(File.Open(u + "_Part_X.vox", FileMode.Open));
            MagicaVoxelData[] parsed = VoxelLogic.AssembleHeadToBody(bin, true);
            //renderLarge(parsed, 0, 0, 0)[0].Save("junk_" + u + ".png");

            MagicaVoxelData[][] explode = VoxelLogic.FieryExplosionDouble(parsed, true); //((CurrentMobilities[UnitLookup[u]] == MovementType.Immobile) ? false : true)
            string folder = ("indexed");
            int color = 0;
            for (int d = 0; d < 4; d++)
            {
                System.IO.Directory.CreateDirectory(folder); //("color" + i);

                for (int frame = 0; frame < 8; frame++)
                {
                    Bitmap b = renderHugeSmart(explode[frame], d, color, frame);
                    Bitmap b2 = new Bitmap(248, 308, PixelFormat.Format32bppArgb);
                    Graphics g2 = Graphics.FromImage(b2);
                    g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    Bitmap b3 = b.Clone(new Rectangle(0, 0, 248 * 2, 308 * 2), b.PixelFormat);
                    b.Dispose();
                    g2.DrawImage(b3, 0, 0, 248, 308);
                    CreateChannelBitmap(b2, folder + "/" + u + "_Explode_face" + d + "_" + frame + ".png");
                    b2.Dispose();
                    g2.Dispose();
                    b3.Dispose();
                }
            }

            bin.Close();
        }
        public static void processFieryExplosionDoubleW(string u, int palette)
        {
            Console.WriteLine("Processing: " + u);
            BinaryReader bin = new BinaryReader(File.Open(u + "_Large_W.vox", FileMode.Open));
            MagicaVoxelData[] parsed = VoxelLogic.FromMagicaRaw(bin).ToArray();
            //renderLarge(parsed, 0, 0, 0)[0].Save("junk_" + u + ".png");
            VoxelLogic.wcolors = VoxelLogic.wpalettes[palette];
            VoxelLogic.wcurrent = VoxelLogic.wrendered[palette];
            MagicaVoxelData[][] explode = VoxelLogic.FieryExplosionDoubleW(parsed, false); //((CurrentMobilities[UnitLookup[u]] == MovementType.Immobile) ? false : true)
            string folder = ("frames");

            for (int d = 0; d < 4; d++)
            {
                System.IO.Directory.CreateDirectory(folder); //("color" + i);

                for (int frame = 0; frame < 12; frame++)
                {
                    Bitmap b = renderWSmartHuge(explode[frame], d, palette, frame, 8, true);
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
                }
            }


            System.IO.Directory.CreateDirectory("gifs");
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";

            for (int d = 0; d < 4; d++)
            {
                for (int frame = 0; frame < 12; frame++)
                {
                    s += folder + "/palette" + palette + "_" + u + "_Large_face" + d + "_fiery_explode_" + frame + ".png ";
                }
            }

            startInfo.Arguments = "-dispose background -delay 11 -loop 0 " + s + " gifs/palette" + palette + "_" + u + "_explosion_animated.gif";
            Console.WriteLine("Running convert.exe ...");
            Process.Start(startInfo).WaitForExit();

            bin.Close();
        }
        public static void processFieryExplosionQuadW(string u, int palette)
        {
            Console.WriteLine("Processing: " + u);
            BinaryReader bin = new BinaryReader(File.Open(u + "_Huge_W.vox", FileMode.Open));
            MagicaVoxelData[] parsed = VoxelLogic.FromMagicaRaw(bin).ToArray();
            //renderLarge(parsed, 0, 0, 0)[0].Save("junk_" + u + ".png");
            VoxelLogic.wcolors = VoxelLogic.wpalettes[palette];
            VoxelLogic.wcurrent = VoxelLogic.wrendered[palette];
            MagicaVoxelData[][] explode = VoxelLogic.FieryExplosionQuadW(parsed, false); //((CurrentMobilities[UnitLookup[u]] == MovementType.Immobile) ? false : true)
            string folder = ("frames");

            for (int d = 0; d < 4; d++)
            {
                System.IO.Directory.CreateDirectory(folder); //("color" + i);

                for (int frame = 0; frame < 12; frame++)
                {
                    Bitmap b = renderWSmartMassive(explode[frame], d, palette, frame, 8, true);
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


            System.IO.Directory.CreateDirectory("gifs");
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";

            for (int d = 0; d < 4; d++)
            {
                for (int frame = 0; frame < 12; frame++)
                {
                    s += folder + "/palette" + palette + "_" + u + "_Huge_face" + d + "_fiery_explode_" + frame + ".png ";
                }
            }

            startInfo.Arguments = "-dispose background -delay 11 -loop 0 " + s + " gifs/palette" + palette + "_" + u + "_explosion_animated.gif";
            Console.WriteLine("Running convert.exe ...");
            Process.Start(startInfo).WaitForExit();

            bin.Close();
        }

        private static void processFiring(string u)
        {
            Console.WriteLine("Processing: " + u);
            string filename = u + "_X.vox";
            BinaryReader bin = new BinaryReader(File.Open(filename, FileMode.Open));
            bin.Close();
            MagicaVoxelData[] parsed;
            string folder = ("frames");

            for (int w = 0; w < 2; w++)
            {
                if ((w == 0 && u == "Infantry" || u == "Tank_S") || (w == 1 && (u == "Infantry_P" || u == "Infantry_T")))
                {
                    filename = u + "_Firing_X.vox";
                }
                if (VoxelLogic.CurrentWeapons[VoxelLogic.UnitLookup[u]][w] == 7)
                {
                    bin = new BinaryReader(File.Open(filename, FileMode.Open));
                    parsed = VoxelLogic.FromMagicaRaw(bin).ToArray();
                    MagicaVoxelData[][] flying = Flyover(parsed);
                    MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[16][];
                    //voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
                    for (int i = 0; i < 16; i++)
                    {
                        voxelFrames[i] = new MagicaVoxelData[flying[i].Length];
                        flying[i].CopyTo(voxelFrames[i], 0);
                    }
                    /*                    for (int i = 0; i < flying[4].Length; i++)
                                        {
                                            voxelFrames[0][i].x += 20;
                                            voxelFrames[0][i].y += 20;
                                        }*/
                    Console.WriteLine("X: " + voxelFrames[0].Min(mvd => mvd.x) + ", Y: " + voxelFrames[0].Min(mvd => mvd.y));

                    voxelFrames = weaponAnimations[VoxelLogic.CurrentWeapons[VoxelLogic.UnitLookup[u]][w]](voxelFrames, VoxelLogic.UnitLookup[u]);

                    for (int f = 0; f < 16; f++)
                    {
                        List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxelFrames[f].Length);
                        int[,] taken = new int[60, 60];
                        taken.Fill(-1);
                        for (int i = 0; i < voxelFrames[f].Length; i++)
                        {
                            // do not store this voxel if it lies out of range of the voxel chunk (30x30x30)
                            if (voxelFrames[f][i].x >= 60 || voxelFrames[f][i].y >= 60 || voxelFrames[f][i].z >= 60)
                            {
                                Console.Write("Voxel out of bounds: " + voxelFrames[f][i].x + ", " + voxelFrames[f][i].y + ", " + voxelFrames[f][i].z);
                                continue;
                            }
                            altered.Add(voxelFrames[f][i]);
                        }
                        flying[f] = altered.ToArray();
                    }
                    for (int color = 0; color < 8; color++)
                    {
                        for (int d = 0; d < 4; d++)
                        {
                            System.IO.Directory.CreateDirectory(folder); //("color" + i);

                            for (int frame = 0; frame < 16; frame++)
                            {
                                Graphics g;
                                Bitmap[] b;
                                Bitmap o;

                                b = renderLarge(flying[frame], d, color, frame);
                                o = renderOutlineLarge(flying[frame], d, color, frame);
                                g = Graphics.FromImage(b[1]);

                                g.DrawImage(o, 0, 0);
                                g.DrawImage(b[0], 0, 0);

                                b[1].Save(folder + "/color" + color + "_" + u + "_face" + d + "_attack_" + w + "_" + (frame) + ".png", ImageFormat.Png);
                            }
                        }
                    }

                    bin.Close();
                }
                else if (VoxelLogic.CurrentWeapons[VoxelLogic.UnitLookup[u]][w] != -1)
                {
                    bin = new BinaryReader(File.Open(filename, FileMode.Open));
                    parsed = VoxelLogic.FromMagicaRaw(bin).ToArray();
                    MagicaVoxelData[][] firing = makeFiringAnimation(parsed, VoxelLogic.UnitLookup[u], w);
                    for (int color = 0; color < 8; color++)
                    {
                        for (int d = 0; d < 4; d++)
                        {
                            System.IO.Directory.CreateDirectory(folder); //("color" + i);

                            for (int frame = 0; frame < 16; frame++)
                            {
                                Graphics g;
                                Bitmap[] b;
                                Bitmap o;

                                b = renderLarge(firing[frame], d, color, frame);
                                o = renderOutlineLarge(firing[frame], d, color, frame);
                                g = Graphics.FromImage(b[1]);

                                g.DrawImage(o, 0, 0);
                                g.DrawImage(b[0], 0, 0);

                                b[1].Save(folder + "/color" + color + "_" + u + "_face" + d + "_attack_" + w + "_" + (frame) + ".png", ImageFormat.Png);
                            }
                        }
                    }

                    bin.Close();
                }
                else continue;

                System.IO.Directory.CreateDirectory("gifs");
                ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
                startInfo.UseShellExecute = false;
                string s = "";
                for (int i = 0; i < 8; i++)
                {
                    for (int d = 0; d < 4; d++)
                    {
                        for (int frame = 0; frame < 16; frame++)
                        {
                            s += folder + "/color" + i + "_" + u + "_face" + d + "_attack_" + w + "_" + frame + ".png ";
                        }
                    }
                }
                startInfo.Arguments = "-dispose background -delay 11 -loop 0 " + s + " gifs/" + u + "_attack_" + w + "_animated.gif";
                Console.WriteLine("Running convert.exe ...");
                Process.Start(startInfo).WaitForExit();
            }

        }

        private static void processFiringDouble(string u)
        {
            Console.WriteLine("Processing: " + u);
            string filename = u + "_Large_X.vox";
            BinaryReader bin = new BinaryReader(File.Open(filename, FileMode.Open));
            bin.Close();
            MagicaVoxelData[] parsed;
            string folder = ("frames");

            for (int w = 0; w < 2; w++)
            {
                if ((w == 0 && u == "Infantry" || u == "Tank_S") || (w == 1 && (u == "Infantry_P" || u == "Infantry_T")))
                {
                    filename = u + "_Firing_X.vox";
                }
                if (VoxelLogic.CurrentWeapons[VoxelLogic.UnitLookup[u]][w] == 7)
                {
                    bin = new BinaryReader(File.Open(filename, FileMode.Open));
                    parsed = VoxelLogic.FromMagicaRaw(bin).ToArray();
                    MagicaVoxelData[][] flying = Flyover(parsed);
                    MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[16][];
                    //voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
                    for (int i = 0; i < 16; i++)
                    {
                        voxelFrames[i] = new MagicaVoxelData[flying[i].Length];
                        flying[i].CopyTo(voxelFrames[i], 0);
                    }
                    /*                    for (int i = 0; i < flying[4].Length; i++)
                                        {
                                            voxelFrames[0][i].x += 20;
                                            voxelFrames[0][i].y += 20;
                                        }*/
                    Console.WriteLine("X: " + voxelFrames[0].Min(mvd => mvd.x) + ", Y: " + voxelFrames[0].Min(mvd => mvd.y));

                    voxelFrames = weaponAnimations[VoxelLogic.CurrentWeapons[VoxelLogic.UnitLookup[u]][w]](voxelFrames, VoxelLogic.UnitLookup[u]);

                    for (int f = 0; f < 16; f++)
                    {
                        List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxelFrames[f].Length);
                        int[,] taken = new int[60, 60];
                        taken.Fill(-1);
                        for (int i = 0; i < voxelFrames[f].Length; i++)
                        {
                            // do not store this voxel if it lies out of range of the voxel chunk (30x30x30)
                            if (voxelFrames[f][i].x >= 60 || voxelFrames[f][i].y >= 60 || voxelFrames[f][i].z >= 60)
                            {
                                //Console.Write("Voxel out of bounds: " + voxelFrames[f][i].x + ", " + voxelFrames[f][i].y + ", " + voxelFrames[f][i].z);
                                continue;
                            }
                            altered.Add(voxelFrames[f][i]);
                        }
                        flying[f] = altered.ToArray();
                    }
                    for (int color = 0; color < 8; color++)
                    {
                        for (int d = 0; d < 4; d++)
                        {
                            System.IO.Directory.CreateDirectory(folder); //("color" + i);

                            for (int frame = 0; frame < 16; frame++)
                            {
                                Graphics g;
                                Bitmap[] b;
                                Bitmap o;

                                b = renderLarge(flying[frame], d, color, frame);
                                o = renderOutlineLarge(flying[frame], d, color, frame);
                                g = Graphics.FromImage(b[1]);

                                g.DrawImage(o, 0, 0);
                                g.DrawImage(b[0], 0, 0);

                                b[1].Save(folder + "/color" + color + "_" + u + "_face" + d + "_attack_" + w + "_" + (frame) + ".png", ImageFormat.Png);
                            }
                        }
                    }

                    bin.Close();
                }
                else if (VoxelLogic.CurrentWeapons[VoxelLogic.UnitLookup[u]][w] != -1)
                {
                    bin = new BinaryReader(File.Open(filename, FileMode.Open));
                    parsed = VoxelLogic.FromMagicaRaw(bin).ToArray();
                    MagicaVoxelData[][] firing = VoxelLogic.makeFiringAnimationDouble(parsed, VoxelLogic.UnitLookup[u], w);
                    for (int color = 0; color < 8; color++)
                    {
                        for (int d = 0; d < 4; d++)
                        {
                            System.IO.Directory.CreateDirectory(folder); //("color" + i);

                            for (int frame = 0; frame < 16; frame++)
                            {
                                Bitmap b = renderHugeSmart(firing[frame], d, color, frame);
                                Bitmap b2 = new Bitmap(248, 308, PixelFormat.Format32bppArgb);


                                //                        b.Save("temp.png", ImageFormat.Png);
                                Graphics g2 = Graphics.FromImage(b2);
                                g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                                Bitmap b3 = b.Clone(new Rectangle(0, 0, 248 * 2, 308 * 2), b.PixelFormat);
                                b.Dispose();
                                g2.DrawImage(b3, 0, 0, 248, 308);

                                b2.Save(folder + "/color" + color + "_" + u + "_Large_face" + d + "_attack_" + w + "_" + (frame) + ".png", ImageFormat.Png);
                                b2.Dispose();
                                g2.Dispose();
                            }
                        }
                    }
                    bin.Close();
                }
                else continue;

                System.IO.Directory.CreateDirectory("gifs");
                ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
                startInfo.UseShellExecute = false;
                string s = "";
                for (int i = 0; i < 8; i++)
                {
                    for (int d = 0; d < 4; d++)
                    {
                        for (int frame = 0; frame < 16; frame++)
                        {
                            s += folder + "/color" + i + "_" + u + "_Large_face" + d + "_attack_" + w + "_" + frame + ".png ";
                        }
                    }
                }
                startInfo.Arguments = "-dispose background -delay 11 -loop 0 " + s + " gifs/" + u + "_attack_" + w + "_animated.gif";
                Console.WriteLine("Running convert.exe ...");
                Process.Start(startInfo).WaitForExit();
            }

        }
        public static void processFiringPartial(string u)
        {
            Console.WriteLine("Processing: " + u);
            string filename = u + "_Part_X.vox";
            BinaryReader bin = new BinaryReader(File.Open(filename, FileMode.Open));
            bin.Close();
            MagicaVoxelData[] parsed;
            string folder = ("frames");

            for (int w = 0; w < 2; w++)
            {
                if ((w == 0 && u == "Infantry" || u == "Tank_S") || (w == 1 && (u == "Infantry_P" || u == "Infantry_T")))
                {
                    filename = u + "_Firing_Part_X.vox";
                }
                if (VoxelLogic.CurrentWeapons[VoxelLogic.UnitLookup[u]][w] == 7)
                {
                    bin = new BinaryReader(File.Open(filename, FileMode.Open));
                    parsed = VoxelLogic.FromMagicaRaw(bin).ToArray();
                    MagicaVoxelData[][] flying = VoxelLogic.Flyover(parsed);
                    MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[16][];
                    //voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
                    for (int i = 0; i < 16; i++)
                    {
                        voxelFrames[i] = new MagicaVoxelData[flying[i].Length];
                        flying[i].CopyTo(voxelFrames[i], 0);
                    }
                    /*                    for (int i = 0; i < flying[4].Length; i++)
                                        {
                                            voxelFrames[0][i].x += 20;
                                            voxelFrames[0][i].y += 20;
                                        }*/
                    Console.WriteLine("X: " + voxelFrames[0].Min(mvd => mvd.x) + ", Y: " + voxelFrames[0].Min(mvd => mvd.y));

                    voxelFrames = VoxelLogic.weaponAnimationsDouble[VoxelLogic.CurrentWeapons[VoxelLogic.UnitLookup[u]][w]](voxelFrames, VoxelLogic.UnitLookup[u]);

                    for (int f = 0; f < 16; f++)
                    {
                        List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxelFrames[f].Length);
                        int[,] taken = new int[120, 120];
                        taken.Fill(-1);
                        for (int i = 0; i < voxelFrames[f].Length; i++)
                        {
                            // do not store this voxel if it lies out of range of the voxel chunk (30x30x30)
                            if (voxelFrames[f][i].x >= 120 || voxelFrames[f][i].y >= 120 || voxelFrames[f][i].z >= 120)
                            {
                                //Console.Write("Voxel out of bounds: " + voxelFrames[f][i].x + ", " + voxelFrames[f][i].y + ", " + voxelFrames[f][i].z);
                                continue;
                            }
                            altered.Add(voxelFrames[f][i]);
                        }
                        flying[f] = altered.ToArray();
                    }
                    for (int color = 0; color < 8; color++)
                    {
                        for (int d = 0; d < 4; d++)
                        {
                            System.IO.Directory.CreateDirectory(folder); //("color" + i);

                            for (int frame = 0; frame < 16; frame++)
                            {
                                Bitmap b = renderHugeSmart(flying[frame], d, color, frame);
                                Bitmap b2 = new Bitmap(248, 308, PixelFormat.Format32bppArgb);
                                Graphics g2 = Graphics.FromImage(b2);
                                g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                                Bitmap b3 = b.Clone(new Rectangle(0, 0, 248 * 2, 308 * 2), b.PixelFormat);
                                b.Dispose();
                                g2.DrawImage(b3, 0, 0, 248, 308);

                                b2.Save(folder + "/color" + color + "_" + u + "_Large_face" + d + "_attack_" + w + "_" + (frame) + ".png", ImageFormat.Png);
                                b2.Dispose();
                                g2.Dispose();
                                b3.Dispose();
                            }
                        }
                    }

                    bin.Close();
                }
                else if (VoxelLogic.CurrentWeapons[VoxelLogic.UnitLookup[u]][w] != -1)
                {
                    bin = new BinaryReader(File.Open(filename, FileMode.Open));
                    parsed = VoxelLogic.AssembleHeadToBody(bin, false);
                    MagicaVoxelData[][] firing = VoxelLogic.makeFiringAnimationDouble(parsed, VoxelLogic.UnitLookup[u], w);
                    for (int color = 0; color < 8; color++)
                    {
                        for (int d = 0; d < 4; d++)
                        {
                            System.IO.Directory.CreateDirectory(folder); //("color" + i);

                            for (int frame = 0; frame < 16; frame++)
                            {
                                Bitmap b = renderHugeSmart(firing[frame], d, color, frame);
                                Bitmap b2 = new Bitmap(248, 308, PixelFormat.Format32bppArgb);
                                Graphics g2 = Graphics.FromImage(b2);
                                g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                                Bitmap b3 = b.Clone(new Rectangle(0, 0, 248 * 2, 308 * 2), b.PixelFormat);
                                b.Dispose();
                                g2.DrawImage(b3, 0, 0, 248, 308);

                                b2.Save(folder + "/color" + color + "_" + u + "_Large_face" + d + "_attack_" + w + "_" + (frame) + ".png", ImageFormat.Png);
                                b2.Dispose();
                                g2.Dispose();
                                b3.Dispose();
                            }
                        }
                    }
                    bin.Close();
                }
                else continue;

                System.IO.Directory.CreateDirectory("gifs");
                ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
                startInfo.UseShellExecute = false;
                string s = "";
                for (int i = 0; i < 8; i++)
                {
                    for (int d = 0; d < 4; d++)
                    {
                        for (int frame = 0; frame < 16; frame++)
                        {
                            s += folder + "/color" + i + "_" + u + "_Large_face" + d + "_attack_" + w + "_" + frame + ".png ";
                        }
                    }
                }
                startInfo.Arguments = "-dispose background -delay 11 -loop 0 " + s + " gifs/" + u + "_attack_" + w + "_animated.gif";
                Console.WriteLine("Running convert.exe ...");
                Process.Start(startInfo).WaitForExit();
            }

        }
        public static void processFiringChannelPartial(string u)
        {
            Console.WriteLine("Processing: " + u);
            string filename = u + "_Part_X.vox";
            BinaryReader bin = new BinaryReader(File.Open(filename, FileMode.Open));
            bin.Close();
            MagicaVoxelData[] parsed;
            string folder = ("indexed");

            for (int w = 0; w < 2; w++)
            {
                if ((w == 0 && u == "Infantry" || u == "Tank_S") || (w == 1 && (u == "Infantry_P" || u == "Infantry_T")))
                {
                    filename = u + "_Firing_Part_X.vox";
                }
                if (VoxelLogic.CurrentWeapons[VoxelLogic.UnitLookup[u]][w] == 7)
                {
                    bin = new BinaryReader(File.Open(filename, FileMode.Open));
                    parsed = VoxelLogic.FromMagicaRaw(bin).ToArray();
                    MagicaVoxelData[][] flying = VoxelLogic.Flyover(parsed);
                    MagicaVoxelData[][] receive = VoxelLogic.makeReceiveAnimationDouble(7, VoxelLogic.CurrentWeaponReceptions[VoxelLogic.UnitLookup[u]][w]);
                    MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[16][];
                    //voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
                    for (int i = 0; i < 16; i++)
                    {
                        voxelFrames[i] = new MagicaVoxelData[flying[i].Length];
                        flying[i].CopyTo(voxelFrames[i], 0);
                    }
                    /*                    for (int i = 0; i < flying[4].Length; i++)
                                        {
                                            voxelFrames[0][i].x += 20;
                                            voxelFrames[0][i].y += 20;
                                        }*/
                    //Console.WriteLine("X: " + voxelFrames[0].Min(mvd => mvd.x) + ", Y: " + voxelFrames[0].Min(mvd => mvd.y));

                    voxelFrames = VoxelLogic.weaponAnimationsDouble[VoxelLogic.CurrentWeapons[VoxelLogic.UnitLookup[u]][w]](voxelFrames, VoxelLogic.UnitLookup[u]);

                    for (int f = 0; f < 16; f++)
                    {
                        List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxelFrames[f].Length);
                        int[,] taken = new int[120, 120];
                        taken.Fill(-1);
                        for (int i = 0; i < voxelFrames[f].Length; i++)
                        {
                            // do not store this voxel if it lies out of range of the voxel chunk (30x30x30)
                            if (voxelFrames[f][i].x >= 120 || voxelFrames[f][i].y >= 120 || voxelFrames[f][i].z >= 120)
                            {
                                //Console.Write("Voxel out of bounds: " + voxelFrames[f][i].x + ", " + voxelFrames[f][i].y + ", " + voxelFrames[f][i].z);
                                continue;
                            }
                            altered.Add(voxelFrames[f][i]);
                        }
                        flying[f] = altered.ToArray();
                    }
                    int color = 0;

                    for (int d = 0; d < 4; d++)
                    {
                        System.IO.Directory.CreateDirectory(folder); //("color" + i);

                        for (int frame = 0; frame < 16; frame++)
                        {
                            Bitmap b = renderHugeSmart(flying[frame], d, color, frame);
                            Bitmap b2 = new Bitmap(248, 308, PixelFormat.Format32bppArgb);
                            Graphics g2 = Graphics.FromImage(b2);
                            g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                            Bitmap b3 = b.Clone(new Rectangle(0, 0, 248 * 2, 308 * 2), b.PixelFormat);
                            g2.DrawImage(b3, 0, 0, 248, 308);
                            b.Dispose();

                            CreateChannelBitmap(b2, folder + "/" + u + "_Attack_" + w + "_face" + d + "_" + (frame) + ".png");
                            b2.Dispose();
                            g2.Dispose();
                            b3.Dispose();

                            b = renderHugeSmart(receive[frame], d, color, frame);
                            b2 = new Bitmap(248, 308, PixelFormat.Format32bppArgb);
                            g2 = Graphics.FromImage(b2);
                            g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                            b3 = b.Clone(new Rectangle(0, 0, 248 * 2, 308 * 2), b.PixelFormat);
                            g2.DrawImage(b3, 0, 0, 248, 308);
                            b.Dispose();

                            CreateChannelBitmap(b2, folder + "/" + u + "_Receive_" + w + "_face" + d + "_" + (frame) + ".png");
                            b2.Dispose();
                            g2.Dispose();
                            b3.Dispose();

                        }
                    }


                    bin.Close();
                }
                else if (VoxelLogic.CurrentWeapons[VoxelLogic.UnitLookup[u]][w] != -1)
                {
                    bin = new BinaryReader(File.Open(filename, FileMode.Open));
                    parsed = VoxelLogic.AssembleHeadToBody(bin, false);
                    MagicaVoxelData[][] firing = VoxelLogic.makeFiringAnimationDouble(parsed, VoxelLogic.UnitLookup[u], w);
                    MagicaVoxelData[][] receive = VoxelLogic.makeReceiveAnimationDouble(VoxelLogic.CurrentWeapons[VoxelLogic.UnitLookup[u]][w],
                        VoxelLogic.CurrentWeaponReceptions[VoxelLogic.UnitLookup[u]][w]);

                    int color = 0;
                    for (int d = 0; d < 4; d++)
                    {
                        System.IO.Directory.CreateDirectory(folder); //("color" + i);

                        for (int frame = 0; frame < 16; frame++)
                        {
                            Bitmap b = renderHugeSmart(firing[frame], d, color, frame);
                            Bitmap b2 = new Bitmap(248, 308, PixelFormat.Format32bppArgb);
                            Graphics g2 = Graphics.FromImage(b2);
                            g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                            Bitmap b3 = b.Clone(new Rectangle(0, 0, 248 * 2, 308 * 2), b.PixelFormat);
                            g2.DrawImage(b3, 0, 0, 248, 308);
                            b2.Save("junk/color" + color + "_" + u + "_Large_face" + d + "_attack_" + w + "_" + (frame) + ".png", ImageFormat.Png);

                            CreateChannelBitmap(b2, folder + "/" + u + "_Attack_" + w + "_face" + d + "_" + (frame) + ".png");
                            b.Dispose();
                            g2.Dispose();
                            b3.Dispose();

                            b = renderHugeSmart(receive[frame], d, color, frame);
                            b2 = new Bitmap(248, 308, PixelFormat.Format32bppArgb);
                            g2 = Graphics.FromImage(b2);
                            g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                            b3 = b.Clone(new Rectangle(0, 0, 248 * 2, 308 * 2), b.PixelFormat);
                            b.Dispose();
                            g2.DrawImage(b3, 0, 0, 248, 308);

                            CreateChannelBitmap(b2, folder + "/" + u + "_Receive_" + w + "_face" + d + "_" + (frame) + ".png");
                            b2.Dispose();
                            g2.Dispose();
                            b3.Dispose();

                        }
                    }

                    bin.Close();
                }
                else continue;
            }

        }


        private static void processReceiving()
        {
            string folder = ("frames");

            for (int i = 0; i < 8; i++)
            {
                if (i == 2) continue;
                for (int s = 0; s < 4; s++)
                {
                    MagicaVoxelData[][] receive = makeReceiveAnimation(i, s + 1);
                    for (int color = 0; color < ((i == 6) ? 8 : 2); color++)
                    {
                        for (int d = 0; d < 4; d++)
                        {
                            System.IO.Directory.CreateDirectory(folder); //("color" + i);

                            for (int frame = 0; frame < 16; frame++)
                            {
                                Graphics g;
                                Bitmap[] b;
                                Bitmap o;

                                b = renderLarge(receive[frame], d, color, frame);
                                o = renderOutlineLarge(receive[frame], d, color, frame);
                                g = Graphics.FromImage(b[1]);

                                g.DrawImage(o, 0, 0);
                                g.DrawImage(b[0], 0, 0);

                                b[1].Save(folder + "/color" + color + "_" + WeaponTypes[i] + "_face" + d + "_strength_" + s + "_" + frame + ".png", ImageFormat.Png);
                            }
                        }
                    }
                }

                System.IO.Directory.CreateDirectory("gifs");
                ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
                startInfo.UseShellExecute = false;
                for (int color = 0; color < ((i == 6) ? 8 : 2); color++)
                {
                    string st = "";

                    for (int strength = 0; strength < 4; strength++)
                    {
                        for (int d = 0; d < 4; d++)
                        {
                            for (int frame = 0; frame < 16; frame++)
                            {
                                st += folder + "/color" + color + "_" + WeaponTypes[i] + "_face" + d + "_strength_" + strength + "_" + frame + ".png ";
                            }
                        }
                    }
                    startInfo.Arguments = "-dispose background -delay 11 -loop 0 " + st + " gifs/" + "color" + color + "_" + WeaponTypes[i] + "_animated.gif";
                    Console.WriteLine("Running convert.exe ...");
                    Console.WriteLine("Args: " + st);
                    Process.Start(startInfo).WaitForExit();
                }
            }
        }
        public static void processReceivingDouble()
        {
            string folder = ("frames");
            //START AT 0 WHEN PROCESSING ALL OF THE ANIMATIONS.
            for (int i = 7; i < 8; i++)
            {
                if (i == 2) continue;
                for (int s = 0; s < 4; s++)
                {
                    MagicaVoxelData[][] receive = VoxelLogic.makeReceiveAnimationDouble(i, s + 1);
                    for (int color = 0; color < ((i > 4) ? 8 : 2); color++)
                    {
                        for (int d = 0; d < 4; d++)
                        {
                            System.IO.Directory.CreateDirectory(folder); //("color" + i);

                            for (int frame = 0; frame < 16; frame++)
                            {
                                Bitmap b = renderHugeSmart(receive[frame], d, color, frame);
                                Bitmap b2 = new Bitmap(248, 308, PixelFormat.Format32bppArgb);
                                Graphics g2 = Graphics.FromImage(b2);
                                g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                                Bitmap b3 = b.Clone(new Rectangle(0, 0, 248 * 2, 308 * 2), b.PixelFormat);
                                b.Dispose();
                                g2.DrawImage(b3, 0, 0, 248, 308);

                                b2.Save(folder + "/color" + color + "_" + WeaponTypes[i] + "_face" + d + "_strength_" + s + "_" + frame + ".png", ImageFormat.Png);
                                b2.Dispose();
                                g2.Dispose();
                                b3.Dispose();
                            }
                        }
                    }
                }

                System.IO.Directory.CreateDirectory("gifs");
                ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
                startInfo.UseShellExecute = false;
                for (int color = 0; color < ((i > 4) ? 8 : 2); color++)
                {
                    string st = "";

                    for (int strength = 0; strength < 4; strength++)
                    {
                        for (int d = 0; d < 4; d++)
                        {
                            for (int frame = 0; frame < 16; frame++)
                            {
                                st += folder + "/color" + color + "_" + WeaponTypes[i] + "_face" + d + "_strength_" + strength + "_" + frame + ".png ";
                            }
                        }
                    }
                    startInfo.Arguments = "-dispose background -delay 11 -loop 0 " + st + " gifs/" + "color" + color + "_" + WeaponTypes[i] + "_animated.gif";
                    Console.WriteLine("Running convert.exe ...");
                    Console.WriteLine("Args: " + st);
                    Process.Start(startInfo).WaitForExit();
                }
            }
        }
        private static void processChannelFiring(string u)
        {
            Console.WriteLine("Processing: " + u);
            string filename = u + "_X.vox";
            BinaryReader bin = new BinaryReader(File.Open(filename, FileMode.Open));
            bin.Close();
            MagicaVoxelData[] parsed;
            string folder = ("indexed");

            for (int w = 0; w < 2; w++)
            {
                if ((w == 0 && u == "Infantry" || u == "Tank_S") || (w == 1 && (u == "Infantry_P" || u == "Infantry_T")))
                {
                    filename = u + "_Firing_X.vox";
                    bin = new BinaryReader(File.Open(filename, FileMode.Open));
                    parsed = VoxelLogic.FromMagicaRaw(bin).ToArray();
                    for (int f = 0; f < 4; f++)
                    { //"color" + i + "/"
                        CreateChannelBitmap(processSingleOutlined(parsed, 7, "SE", f, 4), "indexed/" + u + "_Firing_face0" + "_" + f + ".png");
                        CreateChannelBitmap(processSingleOutlined(parsed, 7, "SW", f, 4), "indexed/" + u + "_Firing_face1" + "_" + f + ".png");
                        CreateChannelBitmap(processSingleOutlined(parsed, 7, "NW", f, 4), "indexed/" + u + "_Firing_face2" + "_" + f + ".png");
                        CreateChannelBitmap(processSingleOutlined(parsed, 7, "NE", f, 4), "indexed/" + u + "_Firing_face3" + "_" + f + ".png");
                    }
                    bin.Close();
                }
                if (VoxelLogic.CurrentWeapons[VoxelLogic.UnitLookup[u]][w] == 7)
                {
                    bin = new BinaryReader(File.Open(filename, FileMode.Open));
                    parsed = VoxelLogic.FromMagicaRaw(bin).ToArray();
                    MagicaVoxelData[][] flying = Flyover(parsed);
                    MagicaVoxelData[][] receive = makeReceiveAnimation(7, VoxelLogic.CurrentWeaponReceptions[VoxelLogic.UnitLookup[u]][w]);
                    MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[16][];
                    //voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
                    for (int i = 0; i < 16; i++)
                    {
                        voxelFrames[i] = new MagicaVoxelData[flying[i].Length];
                        flying[i].CopyTo(voxelFrames[i], 0);
                    }
                    /*                    for (int i = 0; i < flying[4].Length; i++)
                                        {
                                            voxelFrames[0][i].x += 20;
                                            voxelFrames[0][i].y += 20;
                                        }*/
                    //Console.WriteLine("X: " + voxelFrames[0].Min(mvd => mvd.x) + ", Y: " + voxelFrames[0].Min(mvd => mvd.y));

                    voxelFrames = weaponAnimations[VoxelLogic.CurrentWeapons[VoxelLogic.UnitLookup[u]][w]](voxelFrames, VoxelLogic.UnitLookup[u]);

                    for (int f = 0; f < 16; f++)
                    {
                        List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxelFrames[f].Length);
                        int[,] taken = new int[60, 60];
                        taken.Fill(-1);
                        for (int i = 0; i < voxelFrames[f].Length; i++)
                        {
                            // do not store this voxel if it lies out of range of the voxel chunk (30x30x30)
                            if (voxelFrames[f][i].x >= 60 || voxelFrames[f][i].y >= 60 || voxelFrames[f][i].z >= 60)
                            {
                                Console.Write("Voxel out of bounds: " + voxelFrames[f][i].x + ", " + voxelFrames[f][i].y + ", " + voxelFrames[f][i].z);
                                continue;
                            }
                            altered.Add(voxelFrames[f][i]);
                        }
                        flying[f] = altered.ToArray();
                    }
                    for (int color = 0; color < 1; color++)
                    {
                        for (int d = 0; d < 4; d++)
                        {
                            System.IO.Directory.CreateDirectory(folder); //("color" + i);

                            for (int frame = 0; frame < 16; frame++)
                            {
                                CreateChannelBitmap(processSingleLargeFrame(flying[frame], 7, d, frame), folder + "/" + u + "_Attack_" + w + "_face" + d + "_" + (frame) + ".png");
                                CreateChannelBitmap(processSingleLargeFrame(receive[frame], 7, d, frame), folder + "/" + u + "_Receive_" + w + "_face" + d + "_" + (frame) + ".png");
                            }
                        }
                    }

                    bin.Close();
                }
                else if (VoxelLogic.CurrentWeapons[VoxelLogic.UnitLookup[u]][w] != -1)
                {
                    bin = new BinaryReader(File.Open(filename, FileMode.Open));
                    parsed = VoxelLogic.FromMagicaRaw(bin).ToArray();
                    MagicaVoxelData[][] firing = makeFiringAnimation(parsed, VoxelLogic.UnitLookup[u], w);
                    MagicaVoxelData[][] receive = makeReceiveAnimation(VoxelLogic.CurrentWeapons[VoxelLogic.UnitLookup[u]][w], VoxelLogic.CurrentWeaponReceptions[VoxelLogic.UnitLookup[u]][w]);

                    for (int d = 0; d < 4; d++)
                    {
                        System.IO.Directory.CreateDirectory(folder); //("color" + i);

                        for (int frame = 0; frame < 16; frame++)
                        {
                            CreateChannelBitmap(processSingleLargeFrame(firing[frame], 7, d, frame), folder + "/" + u + "_Attack_" + w + "_face" + d + "_" + (frame) + ".png");
                            CreateChannelBitmap(processSingleLargeFrame(receive[frame], 7, d, frame), folder + "/" + u + "_Receive_" + w + "_face" + d + "_" + (frame) + ".png");
                        }
                    }


                    bin.Close();
                }
                else continue;
            }

        }
        /*
        private static void processChannelReceiving()
        {
            string folder = ("frames");

            for (int i = 0; i < 8; i++)
            {
                if (i == 2) continue;
                for (int s = 0; s < 4; s++)
                {
                    MagicaVoxelData[][] receive = makeReceiveAnimation(i, s + 1);
                    for (int color = 0; color < ((i == 6) ? 8 : 2); color++)
                    {
                        for (int d = 0; d < 4; d++)
                        {
                            System.IO.Directory.CreateDirectory(folder); //("color" + i);

                            for (int frame = 0; frame < 16; frame++)
                            {
                                Graphics g;
                                Bitmap[] b;
                                Bitmap o;

                                b = renderLarge(receive[frame], d, color, frame);
                                o = renderOutlineLarge(receive[frame], d, color, frame);
                                g = Graphics.FromImage(b[1]);

                                g.DrawImage(o, 0, 0);
                                g.DrawImage(b[0], 0, 0);

                                b[1].Save(folder + "/color" + color + "_" + WeaponTypes[i] + "_face" + d + "_strength_" + s + "_" + frame + ".png", ImageFormat.Png);
                            }
                        }
                    }
                }

                System.IO.Directory.CreateDirectory("gifs");
                ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
                startInfo.UseShellExecute = false;
                for (int color = 0; color < ((i == 6) ? 8 : 2); color++)
                {
                    string st = "";

                    for (int strength = 0; strength < 4; strength++)
                    {
                        for (int d = 0; d < 4; d++)
                        {
                            for (int frame = 0; frame < 16; frame++)
                            {
                                st += folder + "/color" + color + "_" + WeaponTypes[i] + "_face" + d + "_strength_" + strength + "_" + frame + ".png ";
                            }
                        }
                    }
                    startInfo.Arguments = "-dispose background -delay 11 -loop 0 " + st + " gifs/" + "color" + color + "_" + WeaponTypes[i] + "_animated.gif";
                    Console.WriteLine("Running convert.exe ...");
                    //Console.WriteLine("Args: " + st);
                    Process.Start(startInfo).WaitForExit();
                }
            }
        }*/
        private static Bitmap processSingleLargeFrame(MagicaVoxelData[] parsedFrame, int color, int dir, int frame)
        {
            Graphics g;
            Bitmap[] b;
            Bitmap o;

            b = renderLarge(parsedFrame, dir, color, frame);
            o = renderOutlineLarge(parsedFrame, dir, color, frame);
            g = Graphics.FromImage(b[1]);

            g.DrawImage(o, 0, 0);
            g.DrawImage(b[0], 0, 0);
            return b[1];
        }
        private static void processChannelExplosion(string u)
        {
            Console.WriteLine("Processing: " + u);
            BinaryReader bin = new BinaryReader(File.Open(u + "_X.vox", FileMode.Open));
            MagicaVoxelData[] parsed = VoxelLogic.FromMagicaRaw(bin).ToArray();
            MagicaVoxelData[][] explode = FieryExplosion(parsed, ((VoxelLogic.CurrentMobilities[VoxelLogic.UnitLookup[u]] == MovementType.Immobile) ? false : true));
            string folder = ("indexed");

            for (int d = 0; d < 4; d++)
            {
                System.IO.Directory.CreateDirectory(folder); //("color" + i);

                for (int frame = 0; frame < 8; frame++)
                {
                    CreateChannelBitmap(processSingleLargeFrame(explode[frame], 7, d, frame), folder + "/" + u + "_Explode_face" + d + "_" + frame + ".png");
                }
            }

            bin.Close();

            if (File.Exists(u + "_Firing_X.vox"))
            {
                Console.WriteLine("Processing: " + u + " Firing");
                bin = new BinaryReader(File.Open(u + "_Firing_X.vox", FileMode.Open));
                parsed = VoxelLogic.FromMagicaRaw(bin).ToArray();
                explode = FieryExplosion(parsed, ((VoxelLogic.CurrentMobilities[VoxelLogic.UnitLookup[u]] == MovementType.Immobile) ? false : true));

                for (int d = 0; d < 4; d++)
                {
                    System.IO.Directory.CreateDirectory(folder); //("color" + i);

                    for (int frame = 0; frame < 8; frame++)
                    {
                        CreateChannelBitmap(processSingleLargeFrame(explode[frame], 7, d, frame), folder + "/" + u + "_Firing_Explode_face" + d + "_" + frame + ".png");
                    }
                }

                bin.Close();

            }
        }

        private static void processUnitOutlinedW(string u)
        {

            Console.WriteLine("Processing: " + u);
            BinaryReader bin = new BinaryReader(File.Open(u + "_W.vox", FileMode.Open));
            MagicaVoxelData[] parsed = VoxelLogic.FromMagica(bin);
            int framelimit = 1;

            for (int i = 0; i < 3; i++)
            {
                string folder = ("palette" + i);//"color" + i;
                System.IO.Directory.CreateDirectory(folder); //("color" + i);
                for (int f = 0; f < framelimit; f++)
                { //"color" + i + "/"
                    for (int dir = 0; dir < 4; dir++)
                    {
                        processSingleOutlinedW(parsed, i, dir, f, framelimit).Save(folder + "/" + u + "_face" + dir + "_" + f + ".png", ImageFormat.Png); //se
                    }
                }

            }
            /*
            System.IO.Directory.CreateDirectory("gifs");
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";
            for (int i = 0; i < 8; i++)
                s += "color" + i + "/" + u + "_face* ";
            startInfo.Arguments = "-dispose background -delay 25 -loop 0 " + s + " gifs/" + u + "_animated.gif";
            Process.Start(startInfo).WaitForExit();
            */
            bin.Close();

            //            processExplosion(u);

            //            processFiring(u);
        }

        private static void processUnitOutlined(string u)
        {

            Console.WriteLine("Processing: " + u);
            BinaryReader bin = new BinaryReader(File.Open(u + "_X.vox", FileMode.Open));
            MagicaVoxelData[] parsed = VoxelLogic.FromMagica(bin);
            int framelimit = 4;
            if (!VoxelLogic.UnitLookup.ContainsKey(u))
            {
                framelimit = 4;
                Bitmap b = new Bitmap(56, 108);
                Graphics g = Graphics.FromImage(b);
                for (int i = 0; i < 8; i++)
                {
                    for (int f = 0; f < framelimit; f++)
                    {
                        g.DrawImage(processSingleOutlined(parsed, i, "SE", f, framelimit), -16, (f - 2) * 26);
                    }
                    b.Save("color" + i + "_" + u + ".png", ImageFormat.Png);
                }
                bin.Close();
                return;
            }
            else if (VoxelLogic.CurrentMobilities[VoxelLogic.UnitLookup[u]] == MovementType.Immobile)
            {
                framelimit = 2;
            }

            for (int i = 0; i < 8; i++)
            {
                string folder = ("color" + i);//"color" + i;
                System.IO.Directory.CreateDirectory(folder); //("color" + i);
                for (int f = 0; f < framelimit; f++)
                { //"color" + i + "/"

                    processSingleOutlined(parsed, i, "SE", f, framelimit).Save(folder + "/" + u + "_face0" + "_" + f + ".png", ImageFormat.Png); //se
                    processSingleOutlined(parsed, i, "SW", f, framelimit).Save(folder + "/" + u + "_face1" + "_" + f + ".png", ImageFormat.Png); //sw
                    processSingleOutlined(parsed, i, "NW", f, framelimit).Save(folder + "/" + u + "_face2" + "_" + f + ".png", ImageFormat.Png); //nw
                    processSingleOutlined(parsed, i, "NE", f, framelimit).Save(folder + "/" + u + "_face3" + "_" + f + ".png", ImageFormat.Png); //ne

                }

            }

            System.IO.Directory.CreateDirectory("gifs");
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";
            for (int i = 0; i < 8; i++)
                s += "color" + i + "/" + u + "_face* ";
            startInfo.Arguments = "-dispose background -delay 25 -loop 0 " + s + " gifs/" + u + "_animated.gif";
            Process.Start(startInfo).WaitForExit();

            bin.Close();

            processExplosion(u);

            processFiring(u);
        }

        private static void processUnitOutlinedDouble(string u)
        {

            Console.WriteLine("Processing: " + u);
            BinaryReader bin = new BinaryReader(File.Open(u + "_Large_X.vox", FileMode.Open));
            MagicaVoxelData[] parsed = VoxelLogic.FromMagica(bin);
            for (int i = 0; i < parsed.Length; i++)
            {
                parsed[i].x += 10;
                parsed[i].y += 10;
            }
            int framelimit = 4;
            if (!VoxelLogic.UnitLookup.ContainsKey(u)) //used for the testing Block model
            {
                framelimit = 4;
                for (int i = 0; i < 8; i++)
                {
                    string folder = ("color" + i);//"color" + i;
                    System.IO.Directory.CreateDirectory(folder); //("color" + i);
                    for (int f = 0; f < framelimit; f++)
                    {
                        Bitmap b = processSingleOutlinedDouble(parsed, i, "SE", f, framelimit, u);
                        b.Save(folder + "/" + u + "_Large_face0" + "_" + f + ".png", ImageFormat.Png); //se
                        b.Dispose();
                    }
                }
                bin.Close();

                System.IO.Directory.CreateDirectory("gifs");
                ProcessStartInfo starter = new ProcessStartInfo(@"convert.exe");
                starter.UseShellExecute = false;
                string arrgs = "";
                for (int i = 0; i < 8; i++)
                    arrgs += "color" + i + "/" + u + "_Large_face* ";
                starter.Arguments = "-dispose background -delay 25 -loop 0 " + arrgs + " gifs/" + u + "_Large_animated.gif";
                Process.Start(starter).WaitForExit();

                for (int i = 0; i < parsed.Length; i++)
                {
                    parsed[i].x += 30;
                    parsed[i].y += 30;
                }
                for (int i = 0; i < 8; i++)
                {
                    string folder = ("color" + i);//"color" + i;
                    System.IO.Directory.CreateDirectory(folder); //("color" + i);
                    for (int f = 0; f < framelimit; f++)
                    {
                        Bitmap b = processSingleOutlinedHuge(parsed, i, "SE", f, framelimit);
                        b.Save(folder + "/" + u + "_Huge_face0" + "_" + f + ".png", ImageFormat.Png); //se
                        b.Dispose();
                    }
                }
                arrgs = "";
                for (int i = 0; i < 8; i++)
                    arrgs += "color" + i + "/" + u + "_Huge_face* ";
                starter.Arguments = "-dispose background -delay 25 -loop 0 " + arrgs + " gifs/" + u + "_Huge_animated.gif";
                Process.Start(starter).WaitForExit();
                return;
            }
            else if (VoxelLogic.CurrentMobilities[VoxelLogic.UnitLookup[u]] == MovementType.Immobile)
            {
                framelimit = 2;
            }

            for (int i = 0; i < 8; i++)
            {
                string folder = ("color" + i);//"color" + i;
                System.IO.Directory.CreateDirectory(folder); //("color" + i);
                for (int f = 0; f < framelimit; f++)
                { //"color" + i + "/"
                    Bitmap b = processSingleOutlinedDouble(parsed, i, "SE", f, framelimit, u);
                    b.Save(folder + "/" + u + "_Large_face0" + "_" + f + ".png", ImageFormat.Png); //se
                    b.Dispose();
                    b = processSingleOutlinedDouble(parsed, i, "SW", f, framelimit, u);
                    b.Save(folder + "/" + u + "_Large_face1" + "_" + f + ".png", ImageFormat.Png); //sw
                    b.Dispose();
                    b = processSingleOutlinedDouble(parsed, i, "NW", f, framelimit, u);
                    b.Save(folder + "/" + u + "_Large_face2" + "_" + f + ".png", ImageFormat.Png); //nw
                    b.Dispose();
                    b = processSingleOutlinedDouble(parsed, i, "NE", f, framelimit, u);
                    b.Save(folder + "/" + u + "_Large_face3" + "_" + f + ".png", ImageFormat.Png); //ne
                    b.Dispose();
                }

            }

            System.IO.Directory.CreateDirectory("gifs");
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";
            for (int i = 0; i < 8; i++)
                s += "color" + i + "/" + u + "_Large_face* ";
            startInfo.Arguments = "-dispose background -delay 25 -loop 0 " + s + " gifs/" + u + "_Large_animated.gif";
            Process.Start(startInfo).WaitForExit();

            bin.Close();

            processFiringDouble(u);

            processExplosionDouble(u);

        }
        public static void processUnitOutlinedWDouble(string u)
        {

            Console.WriteLine("Processing: " + u);
            BinaryReader bin = new BinaryReader(File.Open(u + "_Large_W.vox", FileMode.Open));
            MagicaVoxelData[] parsed = VoxelLogic.PlaceShadowsW(VoxelLogic.FromMagicaRaw(bin)).ToArray();
            for (int i = 0; i < parsed.Length; i++)
            {
                parsed[i].x += 10;
                parsed[i].y += 10;
            }
            int framelimit = 4;

            for (int i = 0; i < VoxelLogic.wpalettecount; i++)
            {
                string folder = ("palette" + i);//"color" + i;
                System.IO.Directory.CreateDirectory(folder); //("color" + i);
                for (int f = 0; f < framelimit; f++)
                { //"color" + i + "/"
                    for (int dir = 0; dir < 4; dir++)
                    {
                        Bitmap b = processSingleOutlinedWDouble(parsed, i, dir, f, framelimit, false);
                        b.Save(folder + "/" + u + "_Large_face" + dir + "_" + f + ".png", ImageFormat.Png); //se
                        b.Dispose();
                    }
                }
            }

            System.IO.Directory.CreateDirectory("gifs");
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";
            for (int i = 0; i < VoxelLogic.wpalettecount; i++)
            {
                s = "palette" + i + "/" + u + "_Large_face* ";
                startInfo.Arguments = "-dispose background -delay 25 -loop 0 " + s + " gifs/palette" + i + "_" + u + "_Large_animated.gif";
                Process.Start(startInfo).WaitForExit();
            }
            bin.Close();

            //            processFiringDouble(u);

            //            processExplosionDouble(u);

        }
        public static void processUnitOutlinedWDouble(string u, int palette)
        {

            Console.WriteLine("Processing: " + u);
            BinaryReader bin = new BinaryReader(File.Open(u + "_Large_W.vox", FileMode.Open));
            MagicaVoxelData[] parsed = VoxelLogic.PlaceShadowsW(VoxelLogic.FromMagicaRaw(bin)).ToArray();
            for (int i = 0; i < parsed.Length; i++)
            {
                parsed[i].x += 10;
                parsed[i].y += 10;
            }
            int framelimit = 4;


            string folder = ("palette" + palette);//"color" + i;
            System.IO.Directory.CreateDirectory(folder); //("color" + i);
            for (int f = 0; f < framelimit; f++)
            { //"color" + i + "/"
                for (int dir = 0; dir < 4; dir++)
                {
                    Bitmap b = processSingleOutlinedWDouble(parsed, palette, dir, f, framelimit, false);
                    b.Save(folder + "/" + u + "_Large_face" + dir + "_" + f + ".png", ImageFormat.Png); //se
                    b.Dispose();
                }
            }


            System.IO.Directory.CreateDirectory("gifs");
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";

            s = "palette" + palette + "/" + u + "_Large_face* ";
            startInfo.Arguments = "-dispose background -delay 25 -loop 0 " + s + " gifs/palette" + palette + "_" + u + "_Large_animated.gif";
            Process.Start(startInfo).WaitForExit();

            bin.Close();

            //            processFiringDouble(u);

            processFieryExplosionDoubleW(u, palette);

        }
        public static void processUnitOutlinedWDouble(string u, int palette, bool still)
        {

            Console.WriteLine("Processing: " + u);
            BinaryReader bin = new BinaryReader(File.Open(u + "_Large_W.vox", FileMode.Open));
            MagicaVoxelData[] parsed = VoxelLogic.FromMagica(bin);
            for (int i = 0; i < parsed.Length; i++)
            {
                parsed[i].x += 10;
                parsed[i].y += 10;
            }
            int framelimit = 4;


            string folder = ("palette" + palette);//"color" + i;
            System.IO.Directory.CreateDirectory(folder); //("color" + i);
            for (int f = 0; f < framelimit; f++)
            { //"color" + i + "/"
                for (int dir = 0; dir < 4; dir++)
                {
                    Bitmap b = processSingleOutlinedWDouble(parsed, palette, dir, f, framelimit, still);
                    b.Save(folder + "/" + u + "_Large_face" + dir + "_" + f + ".png", ImageFormat.Png); //se
                    b.Dispose();
                }
            }


            System.IO.Directory.CreateDirectory("gifs");
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";

            s = "palette" + palette + "/" + u + "_Large_face* ";
            startInfo.Arguments = "-dispose background -delay 25 -loop 0 " + s + " gifs/palette" + palette + "_" + u + "_Large_animated.gif";
            Process.Start(startInfo).WaitForExit();

            bin.Close();

            //            processFiringDouble(u);

            processFieryExplosionDoubleW(u, palette);

        }
        public static void processUnitOutlinedWQuad(string u, int palette, bool still)
        {

            Console.WriteLine("Processing: " + u);
            BinaryReader bin = new BinaryReader(File.Open(u + "_Huge_W.vox", FileMode.Open));
            MagicaVoxelData[] parsed = VoxelLogic.PlaceShadowsW(VoxelLogic.FromMagicaRaw(bin)).ToArray();
            for (int i = 0; i < parsed.Length; i++)
            {
                parsed[i].x += 20;
                parsed[i].y += 20;
            }
            int framelimit = 4;


            string folder = ("palette" + palette);//"color" + i;
            System.IO.Directory.CreateDirectory(folder); //("color" + i);
            for (int f = 0; f < framelimit; f++)
            { //"color" + i + "/"
                for (int dir = 0; dir < 4; dir++)
                {
                    Bitmap b = processSingleOutlinedWQuad(parsed, palette, dir, f, framelimit, still);
                    b.Save(folder + "/" + u + "_Huge_face" + dir + "_" + f + ".png", ImageFormat.Png); //se
                    b.Dispose();
                }
            }


            System.IO.Directory.CreateDirectory("gifs");
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";

            s = "palette" + palette + "/" + u + "_Huge_face* ";
            startInfo.Arguments = "-dispose background -delay 25 -loop 0 " + s + " gifs/palette" + palette + "_" + u + "_Huge_animated.gif";
            Process.Start(startInfo).WaitForExit();

            bin.Close();

            //            processFiringDouble(u);

            processFieryExplosionQuadW(u, palette);

        }
        public static void processUnitOutlinedWalkDouble(string u, int palette)
        {

            Console.WriteLine("Processing: " + u);
            BinaryReader bin = new BinaryReader(File.Open(u + "_Walk_0_Large_W.vox", FileMode.Open));
            MagicaVoxelData[][] parsed = new MagicaVoxelData[4][];
            parsed[0] = VoxelLogic.PlaceShadowsW(VoxelLogic.FromMagicaRaw(bin)).ToArray();
            bin.Close();

            bin = new BinaryReader(File.Open(u + "_Walk_1_Large_W.vox", FileMode.Open));
            parsed[1] = VoxelLogic.PlaceShadowsW(VoxelLogic.FromMagicaRaw(bin)).ToArray();
            bin.Close();

            bin = new BinaryReader(File.Open(u + "_Walk_2_Large_W.vox", FileMode.Open));
            parsed[2] = VoxelLogic.PlaceShadowsW(VoxelLogic.FromMagicaRaw(bin)).ToArray();
            bin.Close();

            bin = new BinaryReader(File.Open(u + "_Walk_3_Large_W.vox", FileMode.Open));
            parsed[3] = VoxelLogic.PlaceShadowsW(VoxelLogic.FromMagicaRaw(bin)).ToArray();
            bin.Close();

            for (int i = 0; i < parsed.Length; i++)
            {
                for (int j = 0; j < parsed[i].Length; j++)
                {
                    parsed[i][j].x += 10;
                    parsed[i][j].y += 10;

                }
            }
            int framelimit = 8;


            string folder = ("palette" + palette);//"color" + i;
            System.IO.Directory.CreateDirectory(folder); //("color" + i);
            for (int f = 0; f < framelimit; f++)
            { //"color" + i + "/"
                for (int dir = 0; dir < 4; dir++)
                {
                    Bitmap b = processSingleOutlinedWDouble(parsed[f % 4], palette, dir, f, framelimit, true);
                    b.Save(folder + "/" + u + "_Walk_Large_face" + dir + "_" + f + ".png", ImageFormat.Png);
                    b.Dispose();
                }
            }


            System.IO.Directory.CreateDirectory("gifs");
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";
            for (int dir = 0; dir < 4; dir++)
            {
                for (int i = 0; i < framelimit; i++)
                {
                    s += "palette" + palette + "/" + u + "_Walk_Large_face" + dir + "_" + i + ".png ";
                }
            }
            startInfo.Arguments = "-dispose background -delay 25 -loop 0 " + s + " gifs/palette" + palette + "_" + u + "_Walk_Large_animated.gif";
            Process.Start(startInfo).WaitForExit();


            //            processFiringDouble(u);

            //            processExplosionDouble(u);

        }
        public static void processUnitOutlinedWalkQuad(string u, int palette)
        {

            Console.WriteLine("Processing: " + u);
            BinaryReader bin = new BinaryReader(File.Open(u + "_Walk_0_Huge_W.vox", FileMode.Open));
            MagicaVoxelData[][] parsed = new MagicaVoxelData[4][];
            parsed[0] = VoxelLogic.PlaceShadowsW(VoxelLogic.FromMagicaRaw(bin)).ToArray();
            bin.Close();

            bin = new BinaryReader(File.Open(u + "_Walk_1_Huge_W.vox", FileMode.Open));
            parsed[1] = VoxelLogic.PlaceShadowsW(VoxelLogic.FromMagicaRaw(bin)).ToArray();
            bin.Close();

            bin = new BinaryReader(File.Open(u + "_Walk_2_Huge_W.vox", FileMode.Open));
            parsed[2] = VoxelLogic.PlaceShadowsW(VoxelLogic.FromMagicaRaw(bin)).ToArray();
            bin.Close();

            bin = new BinaryReader(File.Open(u + "_Walk_3_Huge_W.vox", FileMode.Open));
            parsed[3] = VoxelLogic.PlaceShadowsW(VoxelLogic.FromMagicaRaw(bin)).ToArray();
            bin.Close();

            for (int i = 0; i < parsed.Length; i++)
            {
                for (int j = 0; j < parsed[i].Length; j++)
                {
                    parsed[i][j].x += 20;
                    parsed[i][j].y += 20;

                }
            }
            int framelimit = 8;


            string folder = ("palette" + palette);//"color" + i;
            System.IO.Directory.CreateDirectory(folder); //("color" + i);
            for (int f = 0; f < framelimit; f++)
            { //"color" + i + "/"
                for (int dir = 0; dir < 4; dir++)
                {
                    Bitmap b = processSingleOutlinedWQuad(parsed[f % 4], palette, dir, f, framelimit, true);
                    b.Save(folder + "/" + u + "_Walk_Huge_face" + dir + "_" + f + ".png", ImageFormat.Png);
                    b.Dispose();
                }
            }


            System.IO.Directory.CreateDirectory("gifs");
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";
            for (int dir = 0; dir < 4; dir++)
            {
                for (int i = 0; i < framelimit; i++)
                {
                    s += "palette" + palette + "/" + u + "_Walk_Huge_face" + dir + "_" + i + ".png ";
                }
            }
            startInfo.Arguments = "-dispose background -delay 25 -loop 0 " + s + " gifs/palette" + palette + "_" + u + "_Walk_Huge_animated.gif";
            Process.Start(startInfo).WaitForExit();


            //            processFiringDouble(u);

            //            processExplosionDouble(u);

        }

        /*
                    if (!VoxelLogic.UnitLookup.ContainsKey(u)) //used for the testing Block model
            {
                framelimit = 4;
                for (int i = 0; i < 8; i++)
                {
                    string folder = ("color" + i);//"color" + i;
                    System.IO.Directory.CreateDirectory(folder); //("color" + i);
                    for (int f = 0; f < framelimit; f++)
                    {
                        Bitmap b = processSingleOutlinedDouble(parsed, i, "SE", f, framelimit, u);
                        b.Save(folder + "/" + u + "_Large_face0" + "_" + f + ".png", ImageFormat.Png); //se
                        b.Dispose();
                    }
                }
                bin.Close();

                System.IO.Directory.CreateDirectory("gifs");
                ProcessStartInfo starter = new ProcessStartInfo(@"convert.exe");
                starter.UseShellExecute = false;
                string arrgs = "";
                for (int i = 0; i < 8; i++)
                    arrgs += "color" + i + "/" + u + "_Large_face* ";
                starter.Arguments = "-dispose background -delay " + ((framelimit != 4) ? 50 : 25) + " -loop 0 " + arrgs + " gifs/" + u + "_Large_animated.gif";
                Process.Start(starter).WaitForExit();

                for (int i = 0; i < parsed.Length; i++)
                {
                    parsed[i].x += 30;
                    parsed[i].y += 30;
                }
                for (int i = 0; i < 8; i++)
                {
                    string folder = ("color" + i);//"color" + i;
                    System.IO.Directory.CreateDirectory(folder); //("color" + i);
                    for (int f = 0; f < framelimit; f++)
                    {
                        Bitmap b = processSingleOutlinedHuge(parsed, i, "SE", f, framelimit);
                        b.Save(folder + "/" + u + "_Huge_face0" + "_" + f + ".png", ImageFormat.Png); //se
                        b.Dispose();
                    }
                }
                arrgs = "";
                for (int i = 0; i < 8; i++)
                    arrgs += "color" + i + "/" + u + "_Huge_face* ";
                starter.Arguments = "-dispose background -delay 25 -loop 0 " + arrgs + " gifs/" + u + "_Huge_animated.gif";
                Process.Start(starter).WaitForExit();
                return;
            }

         */
        public static void processUnitOutlinedPartial(string u)
        {

            Console.WriteLine("Processing: " + u);
            BinaryReader bin = new BinaryReader(File.Open(u + "_Part_X.vox", FileMode.Open));
            MagicaVoxelData[] parsed = VoxelLogic.AssembleHeadToBody(bin, false);

            for (int i = 0; i < parsed.Length; i++)
            {
                parsed[i].x += 10;
                parsed[i].y += 10;
            }
            int framelimit = 4;
            if (VoxelLogic.CurrentMobilities[VoxelLogic.UnitLookup[u]] == MovementType.Immobile)
            {
                framelimit = 2;
            }

            for (int i = 0; i < 8; i++)
            {
                string folder = ("color" + i);//"color" + i;
                System.IO.Directory.CreateDirectory(folder); //("color" + i);
                for (int f = 0; f < 4; f++)
                { //"color" + i + "/"
                    Bitmap b = processSingleOutlinedDouble(parsed, i, "SE", f, framelimit, u);
                    b.Save(folder + "/" + u + "_Large_face0" + "_" + f + ".png", ImageFormat.Png); //se
                    b.Dispose();
                    b = processSingleOutlinedDouble(parsed, i, "SW", f, framelimit, u);
                    b.Save(folder + "/" + u + "_Large_face1" + "_" + f + ".png", ImageFormat.Png); //sw
                    b.Dispose();
                    b = processSingleOutlinedDouble(parsed, i, "NW", f, framelimit, u);
                    b.Save(folder + "/" + u + "_Large_face2" + "_" + f + ".png", ImageFormat.Png); //nw
                    b.Dispose();
                    b = processSingleOutlinedDouble(parsed, i, "NE", f, framelimit, u);
                    b.Save(folder + "/" + u + "_Large_face3" + "_" + f + ".png", ImageFormat.Png); //ne
                    b.Dispose();
                }

            }

            System.IO.Directory.CreateDirectory("gifs");
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";
            for (int i = 0; i < 8; i++)
                s += "color" + i + "/" + u + "_Large_face* ";
            startInfo.Arguments = "-dispose background -delay " + ((framelimit != 4) ? 32 : 25) + " -loop 0 " + s + " gifs/" + u + "_Large_animated.gif";
            Process.Start(startInfo).WaitForExit();

            bin.Close();

            processFiringPartial(u);

            processExplosionPartial(u);

        }

        public static void processTerrainChannel()
        {
            for (int i = 0; i < 11; i++)
            {
                TallPaletteDraw.drawPixelsFlat(i);

                Bitmap b = new Bitmap("Terrain/" + Terrains[i] + ".png");
                Bitmap bold = new Bitmap("Terrain/" + Terrains[i] + "_bold.png");
                CreateChannelBitmap(b, "indexed/" + Terrains[i] + ".png", 9);
                CreateChannelBitmap(bold, "indexed/" + Terrains[i] + "_bold.png", 9);
                /*for(int c = 0; c < 8; c++)
                {
                    CreateChannelBitmap(b, "indexed/" + Terrains[i] +"_color"+ c + ".png", 9);
                    CreateChannelBitmap(bold, "indexed/" + Terrains[i] + "_bold_color" + c + ".png", 9);
                }*/
            }
        }
        public static void processTerrainChannelDouble()
        {
            for (int i = 0; i < 11; i++)
            {
                TallPaletteDraw.drawPixelsFlatDouble(i);

                Bitmap b = new Bitmap("Terrain/" + Terrains[i] + ".png");
                Bitmap bold = new Bitmap("Terrain/" + Terrains[i] + "_bold.png");
                CreateChannelBitmap(b, "indexed/" + Terrains[i] + ".png", 9);
                CreateChannelBitmap(bold, "indexed/" + Terrains[i] + "_bold.png", 9);
                /*for(int c = 0; c < 8; c++)
                {
                    CreateChannelBitmap(b, "indexed/" + Terrains[i] +"_color"+ c + ".png", 9);
                    CreateChannelBitmap(bold, "indexed/" + Terrains[i] + "_bold_color" + c + ".png", 9);
                }*/
            }
        }

        public static void processMedalChannel(string u)
        {
            Console.WriteLine("Processing: " + u);
            log.AppendLine(u);
            BinaryReader bin = new BinaryReader(File.Open(u + "_X.vox", FileMode.Open));
            MagicaVoxelData[] parsed = VoxelLogic.FromMagica(bin);
            int framelimit = 4;

            System.IO.Directory.CreateDirectory("indexed"); //("color" + i);
            for (int f = 0; f < framelimit; f++)
            { //"color" + i + "/"
                CreateChannelBitmap(processSingleOutlined(parsed, 0, "SE", f, framelimit), "indexed/" + u + "_face0" + "_" + f + ".png");
                CreateChannelBitmap(processSingleOutlined(parsed, 0, "SW", f, framelimit), "indexed/" + u + "_face1" + "_" + f + ".png");
                CreateChannelBitmap(processSingleOutlined(parsed, 0, "NW", f, framelimit), "indexed/" + u + "_face2" + "_" + f + ".png");
                CreateChannelBitmap(processSingleOutlined(parsed, 0, "NE", f, framelimit), "indexed/" + u + "_face3" + "_" + f + ".png");
            }
            bin.Close();
        }
        public static void processUnitChannel(string u)
        {

            Console.WriteLine("Processing: " + u);
            log.AppendLine(u);
            BinaryReader bin = new BinaryReader(File.Open(u + "_X.vox", FileMode.Open));
            MagicaVoxelData[] parsed = VoxelLogic.FromMagica(bin);
            int framelimit = 4;
            if (VoxelLogic.CurrentMobilities[VoxelLogic.UnitLookup[u]] == MovementType.Immobile)
            {
                framelimit = 2;
            }

            System.IO.Directory.CreateDirectory("indexed"); //("color" + i);
            for (int f = 0; f < framelimit; f++)
            { //"color" + i + "/"
                CreateChannelBitmap(processSingleOutlined(parsed, 0, "SE", f, framelimit), "indexed/" + u + "_face0" + "_" + f + ".png");
                CreateChannelBitmap(processSingleOutlined(parsed, 0, "SW", f, framelimit), "indexed/" + u + "_face1" + "_" + f + ".png");
                CreateChannelBitmap(processSingleOutlined(parsed, 0, "NW", f, framelimit), "indexed/" + u + "_face2" + "_" + f + ".png");
                CreateChannelBitmap(processSingleOutlined(parsed, 0, "NE", f, framelimit), "indexed/" + u + "_face3" + "_" + f + ".png");
            }
            bin.Close();

            processChannelFiring(u);

            processChannelExplosion(u);

            if (u == "Plane_P" || u == "Plane_T" || u == "Copter_T")
            {
                Console.WriteLine("Processing: " + u + " Flyover");
                log.AppendLine(u + " Flyover");
                bin = new BinaryReader(File.Open(u + "_X.vox", FileMode.Open));
                parsed = VoxelLogic.FromMagicaRaw(bin).ToArray();
                MagicaVoxelData[][] flyover = Flyover(parsed);
                framelimit = 16;
                for (int d = 0; d < 4; d++)
                {
                    for (int f = 0; f < framelimit; f++)
                    {
                        CreateChannelBitmap(processSingleLargeFrame(flyover[f], 7, d, f), "indexed/" + u + "_Flyover_face" + d + "_" + f + ".png");
                    }

                    bin.Close();
                }
            }

        }
        public static void processUnitChannelPartial(string u)
        {

            Console.WriteLine("Processing: " + u);
            BinaryReader bin = new BinaryReader(File.Open(u + "_Part_X.vox", FileMode.Open));
            MagicaVoxelData[] parsed = VoxelLogic.AssembleHeadToBody(bin, false);

            for (int i = 0; i < parsed.Length; i++)
            {
                parsed[i].x += 10;
                parsed[i].y += 10;
            }
            int framelimit = 4;
            if (VoxelLogic.CurrentMobilities[VoxelLogic.UnitLookup[u]] == MovementType.Immobile)
            {
                framelimit = 2;
            }

            string folder = ("indexed");//"color" + i;
            System.IO.Directory.CreateDirectory(folder); //("color" + i);
            for (int f = 0; f < 4; f++)
            { //"color" + i + "/"
                CreateChannelBitmap(processSingleOutlinedDouble(parsed, 0, "SE", f, framelimit, u), "indexed/" + u + "_face0" + "_" + f + ".png"); //se
                CreateChannelBitmap(processSingleOutlinedDouble(parsed, 0, "SW", f, framelimit, u), "indexed/" + u + "_face1" + "_" + f + ".png"); //se
                CreateChannelBitmap(processSingleOutlinedDouble(parsed, 0, "NW", f, framelimit, u), "indexed/" + u + "_face2" + "_" + f + ".png"); //se
                CreateChannelBitmap(processSingleOutlinedDouble(parsed, 0, "NE", f, framelimit, u), "indexed/" + u + "_face3" + "_" + f + ".png"); //se
            }


            bin.Close();

            processFiringChannelPartial(u);

            processExplosionChannelPartial(u);

        }

        /*        private static void processBases()
                {
                    BinaryReader[] powers = new BinaryReader[8];
                    BinaryReader[] speeds = new BinaryReader[8];
                    BinaryReader[] techniques = new BinaryReader[8];


                    MagicaVoxelData[][] basepowers = new MagicaVoxelData[8][];
                    MagicaVoxelData[][] basespeeds = new MagicaVoxelData[8][];
                    MagicaVoxelData[][] basetechniques = new MagicaVoxelData[8][];

                    for (int i = 0; i < 8; i++)
                    {
                        powers[i] = new BinaryReader(File.OpenRead(@"Bases\Anim_P_" + i + ".vox"));
                        basepowers[i] = FromMagica(powers[i]);
                        speeds[i] = new BinaryReader(File.OpenRead(@"Bases\Anim_S_" + i + ".vox"));
                        basespeeds[i] = FromMagica(speeds[i]);
                        techniques[i] = new BinaryReader(File.OpenRead(@"Bases\Anim_T_" + i + ".vox"));
                        basetechniques[i] = FromMagica(techniques[i]);

                    }

                    System.IO.Directory.CreateDirectory("Power");
                    System.IO.Directory.CreateDirectory("Speed");
                    System.IO.Directory.CreateDirectory("Technique");
                    Graphics g;
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {


                            Bitmap power = drawPixelsSE(basepowers[j], i, 0), oPower = drawOutlineSE(basepowers[j], i, 0);
                            g = Graphics.FromImage(oPower);
                            g.DrawImage(power, 2, 2);
                            oPower.Save("Power/color" + i + "_frame_" + j + ".png", ImageFormat.Png);

                            Bitmap speed = drawPixelsSE(basespeeds[j], i, 0), oSpeed = drawOutlineSE(basespeeds[j], i, 0);
                            g = Graphics.FromImage(oSpeed);
                            g.DrawImage(speed, 2, 2);
                            oSpeed.Save("Speed/color" + i + "_frame_" + j + ".png", ImageFormat.Png);

                            Bitmap technique = drawPixelsSE(basetechniques[j], i, 0), oTechnique = drawOutlineSE(basetechniques[j], i, 0);
                            g = Graphics.FromImage(oTechnique);
                            g.DrawImage(technique, 2, 2);
                            oTechnique.Save("Technique/color" + i + "_frame_" + j + ".png", ImageFormat.Png);
                        }
                    }

                    for (int i = 0; i < 8; i++)
                    {
                        powers[i].Close();
                        speeds[i].Close();
                        techniques[i].Close();
                    }
                }
                private static void processUnit(string u)
                {
                    BinaryReader bin = new BinaryReader(File.Open(u + "_X.vox", FileMode.Open));
                    MagicaVoxelData[] parsed = FromMagica(bin);

                    BinaryReader[] bases = {
                                           new BinaryReader(File.Open("Base_Power.vox", FileMode.Open)),
                                           new BinaryReader(File.Open("Base_Speed.vox", FileMode.Open)),
                                           new BinaryReader(File.Open("Base_Technique.vox", FileMode.Open))
                    };
                    BinaryReader[] powers = new BinaryReader[8];
                    BinaryReader[] speeds = new BinaryReader[8];
                    BinaryReader[] techniques = new BinaryReader[8];


                    MagicaVoxelData[][] basepowers = new MagicaVoxelData[8][];
                    MagicaVoxelData[][] basespeeds = new MagicaVoxelData[8][];
                    MagicaVoxelData[][] basetechniques = new MagicaVoxelData[8][];

                    for (int i = 0; i < 8; i++)
                    {
                        powers[i] = new BinaryReader(File.OpenRead(@"Bases\Anim_P_" + i + ".vox"));
                        basepowers[i] = FromMagica(powers[i]);
                        speeds[i] = new BinaryReader(File.OpenRead(@"Bases\Anim_S_" + i + ".vox"));
                        basespeeds[i] = FromMagica(speeds[i]);
                        techniques[i] = new BinaryReader(File.OpenRead(@"Bases\Anim_T_" + i + ".vox"));
                        basetechniques[i] = FromMagica(techniques[i]);

                    }
                    for (int i = 0; i < 8; i++)
                    {
                        System.IO.Directory.CreateDirectory(u);
                        System.IO.Directory.CreateDirectory(u + "color" + i);
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/power");
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/power/SE");
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/power/SW");
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/power/NW");
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/power/NE");
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/speed");
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/speed/SE");
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/speed/SW");
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/speed/NW");
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/speed/NE");
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/technique");
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/technique/SE");
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/technique/SW");
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/technique/NW");
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/technique/NE");
                        Bitmap bSE = drawPixelsSE(parsed, i, 0);
                        bSE.Save(u + "/color" + i + "_" + u + "_default_SE" + ".png", ImageFormat.Png);
                        Bitmap bSW = drawPixelsSW(parsed, i, 0);
                        bSW.Save(u + "/color" + i + "_" + u + "_default_SW" + ".png", ImageFormat.Png);
                        Bitmap bNW = drawPixelsNW(parsed, i, 0);
                        bNW.Save(u + "/color" + i + "_" + u + "_default_NW" + ".png", ImageFormat.Png);
                        Bitmap bNE = drawPixelsNE(parsed, i, 0);
                        bNE.Save(u + "/color" + i + "_" + u + "_default_NE" + ".png", ImageFormat.Png);
                        for (int j = 0; j < 8; j++)
                        {
                            Bitmap power = drawPixelsSE(basepowers[j], i, j % 2);
                            Graphics g = Graphics.FromImage(power);
                            g.DrawImage(bSE, 0, 0);
                            power.Save(u + "color" + i + "/power/SE/" + j + ".png", ImageFormat.Png);

                            Bitmap speed = drawPixelsSE(basespeeds[j], i, j % 2);
                            g = Graphics.FromImage(speed);
                            g.DrawImage(bSE, 0, 0);
                            speed.Save(u + "color" + i + "/speed/SE/" + j + ".png", ImageFormat.Png);

                            Bitmap technique = drawPixelsSE(basetechniques[j], i, j % 2);
                            g = Graphics.FromImage(technique);
                            g.DrawImage(bSE, 0, 0);
                            technique.Save(u + "color" + i + "/technique/SE/" + j + ".png", ImageFormat.Png);
                        }
                        for (int j = 0; j < 8; j++)
                        {
                            Bitmap power = drawPixelsSE(basepowers[j], i, j % 2);
                            Graphics g = Graphics.FromImage(power);
                            g.DrawImage(bSW, 0, 0);
                            power.Save(u + "color" + i + "/power/SW/" + j + ".png", ImageFormat.Png);

                            Bitmap speed = drawPixelsSE(basespeeds[j], i, j % 2);
                            g = Graphics.FromImage(speed);
                            g.DrawImage(bSW, 0, 0);
                            speed.Save(u + "color" + i + "/speed/SW/" + j + ".png", ImageFormat.Png);

                            Bitmap technique = drawPixelsSE(basetechniques[j], i, j % 2);
                            g = Graphics.FromImage(technique);
                            g.DrawImage(bSW, 0, 0);
                            technique.Save(u + "color" + i + "/technique/SW/" + j + ".png", ImageFormat.Png);
                        }
                        for (int j = 0; j < 8; j++)
                        {
                            Bitmap power = drawPixelsSE(basepowers[j], i, j % 2);
                            Graphics g = Graphics.FromImage(power);
                            g.DrawImage(bNE, 0, 0);
                            power.Save(u + "color" + i + "/power/NE/" + j + ".png", ImageFormat.Png);

                            Bitmap speed = drawPixelsSE(basespeeds[j], i, j % 2);
                            g = Graphics.FromImage(speed);
                            g.DrawImage(bNE, 0, 0);
                            speed.Save(u + "color" + i + "/speed/NE/" + j + ".png", ImageFormat.Png);

                            Bitmap technique = drawPixelsSE(basetechniques[j], i, j % 2);
                            g = Graphics.FromImage(technique);
                            g.DrawImage(bNE, 0, 0);
                            technique.Save(u + "color" + i + "/technique/NE/" + j + ".png", ImageFormat.Png);
                        }
                        for (int j = 0; j < 8; j++)
                        {
                            Bitmap power = drawPixelsSE(basepowers[j], i, j % 2);
                            Graphics g = Graphics.FromImage(power);
                            g.DrawImage(bNW, 0, 0);
                            power.Save(u + "color" + i + "/power/NW/" + j + ".png", ImageFormat.Png);

                            Bitmap speed = drawPixelsSE(basespeeds[j], i, j % 2);
                            g = Graphics.FromImage(speed);
                            g.DrawImage(bNW, 0, 0);
                            speed.Save(u + "color" + i + "/speed/NW/" + j + ".png", ImageFormat.Png);

                            Bitmap technique = drawPixelsSE(basetechniques[j], i, j % 2);
                            g = Graphics.FromImage(technique);
                            g.DrawImage(bNW, 0, 0);
                            technique.Save(u + "color" + i + "/technique/NW/" + j + ".png", ImageFormat.Png);
                        }
                        ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
                        startInfo.UseShellExecute = false;
                        startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/power/SE/* " + u + "/color" + i + "_" + u + "_power_SE.gif";
                        Process.Start(startInfo).WaitForExit();
                        startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/speed/SE/* " + u + "/color" + i + "_" + u + "_speed_SE.gif";
                        Process.Start(startInfo).WaitForExit();
                        startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/technique/SE/* " + u + "/color" + i + "_" + u + "_technique_SE.gif";
                        Process.Start(startInfo).WaitForExit();

                        startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/power/SW/* " + u + "/color" + i + "_" + u + "_power_SW.gif";
                        Process.Start(startInfo).WaitForExit();
                        startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/speed/SW/* " + u + "/color" + i + "_" + u + "_speed_SW.gif";
                        Process.Start(startInfo).WaitForExit();
                        startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/technique/SW/* " + u + "/color" + i + "_" + u + "_technique_SW.gif";
                        Process.Start(startInfo).WaitForExit();

                        startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/power/NW/* " + u + "/color" + i + "_" + u + "_power_NW.gif";
                        Process.Start(startInfo).WaitForExit();
                        startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/speed/NW/* " + u + "/color" + i + "_" + u + "_speed_NW.gif";
                        Process.Start(startInfo).WaitForExit();
                        startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/technique/NW/* " + u + "/color" + i + "_" + u + "_technique_NW.gif";
                        Process.Start(startInfo).WaitForExit();

                        startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/power/NE/* " + u + "/color" + i + "_" + u + "_power_NE.gif";
                        Process.Start(startInfo).WaitForExit();
                        startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/speed/NE/* " + u + "/color" + i + "_" + u + "_speed_NE.gif";
                        Process.Start(startInfo).WaitForExit();
                        startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/technique/NE/* " + u + "/color" + i + "_" + u + "_technique_NE.gif";
                        Process.Start(startInfo).WaitForExit();

                    }
                    bin.Close();
                    bases[0].Close();
                    bases[1].Close();
                    bases[2].Close();
                    for (int i = 0; i < 8; i++)
                    {
                        powers[i].Close();
                        speeds[i].Close();
                        techniques[i].Close();
                    }
                }
                */
        private static Bitmap[] processFloor(string u)
        {
            BinaryReader bin = new BinaryReader(File.Open(u + "_P.vox", FileMode.Open));
            MagicaVoxelData[] parsed = TallPaletteDraw.FromMagica(bin);


            System.IO.Directory.CreateDirectory(u);
            Bitmap[] bits = new Bitmap[] {
                TallPaletteDraw.renderPixels(parsed, "SE"),
                TallPaletteDraw.renderPixels(parsed, "SW"),
                TallPaletteDraw.renderPixels(parsed, "NW"),
                TallPaletteDraw.renderPixels(parsed, "NE")
            };
            /*Random r = new Random();
            Bitmap b = new Bitmap(80,40);
            Graphics tiling = Graphics.FromImage(b);
            tiling.DrawImageUnscaled(bits[r.Next(4)], -40, -20);
            tiling.DrawImageUnscaled(bits[r.Next(4)], 40, -20);
            tiling.DrawImageUnscaled(bits[r.Next(4)], 0, 0);
            tiling.DrawImageUnscaled(bits[r.Next(4)], -40, 20);
            tiling.DrawImageUnscaled(bits[r.Next(4)], 40, 20);*/
            bits[0].Save(u + "/" + u + "_default_SE" + ".png", ImageFormat.Png);
            bits[1].Save(u + "/" + u + "_default_SW" + ".png", ImageFormat.Png);
            bits[2].Save(u + "/" + u + "_default_NW" + ".png", ImageFormat.Png);
            bits[3].Save(u + "/" + u + "_default_NE" + ".png", ImageFormat.Png);
            //b.Save(u + "/tiled.png", ImageFormat.Png);

            bin.Close();
            return bits;
        }

        public static List<Tuple<int, int>> getSoftPath(int width, int height)
        {
            List<Tuple<int, int>> path = new List<Tuple<int, int>>();
            int x = r.Next(3, width - 1);
            int y;
            int midpoint = height / 2;
            for (y = 0; y <= midpoint; y++)
            {
                if (y % 2 == 1)
                {
                    x += r.Next(2);
                }
                else
                {
                    x -= r.Next(2);
                }

                if (x < 1)
                {
                    x++;
                }
                else if (x > width - 1)
                {
                    x--;
                }
                path.Add(new Tuple<int, int>(x, y));
            }
            List<Tuple<int, int>> path2 = path.ToList();
            path2.Reverse();
            path2.RemoveAt(0);
            int iter = 0;
            foreach (Tuple<int, int> t in path2)
            {
                iter++;
                path.Add(new Tuple<int, int>(t.Item1, midpoint + iter));
            }
            return path;
        }

        public static List<Tuple<int, int>> getHardPath(int width, int height)
        {
            int[,] grid = new int[width, height];

            List<Tuple<int, int>> path = new List<Tuple<int, int>>();
            int x;
            int initial_y = (r.Next(6, (height * 2) - 4) / 2) + 1;
            int y = initial_y;
            int midpoint = width / 2;
            int dir = (r.Next(2) == 0) ? -1 : 1;
            for (x = 0; x < width * 0.75; )
            {
                path.Add(new Tuple<int, int>(x, y));
                grid[x, y] = 10;
                if (y % 2 == 0)
                {
                    x++;
                }

                if (r.Next(5) == 0) dir *= -1;
                y += dir;

                if (y < 3)
                {
                    y += 2;
                    dir *= -1;
                }
                else if (y >= height - 2)
                {
                    y -= 2;
                    dir *= -1;
                }
            }

            y = initial_y;
            for (x = width - 1; x >= width * 0.25; )
            {
                path.Add(new Tuple<int, int>(x, y));

                if (r.Next(6) == 0) dir *= -1;
                y += dir;

                if (y % 2 == 0)
                {
                    x--;
                }

                if (y < 2)
                {
                    y += 2;
                    dir *= -1;
                }
                else if (y >= height - 1)
                {
                    y -= 2;
                    dir *= -1;
                }
                //if (x > 1 && x < width - 1 && y > 2 && y < height - 2)
                //{
                //    int[] adj = { grid[x, y + 2], grid[x + 1, y], grid[x, y - 2], grid[x - 1, y] };
                //    {
                //        if (adj.Count(i => i == 10) > 1)
                //        {
                //            if (y % 2 == 0)
                //            {
                //                x++;
                //            }
                //            dir *= -1;
                //        }
                //    }
                //}
            }

            return path;
        }
        public static Bitmap makeFlatTiling()
        {
            Bitmap b = new Bitmap(128 * 16, 32 * 32);
            Graphics g = Graphics.FromImage(b);

            Bitmap[] tilings = new Bitmap[10];
            for (int i = 0; i < 10; i++)
            {
                tilings[i] = TallPaletteDraw.drawPixelsFlatDouble(i);
            }
            int[,] grid = new int[17, 33];
            Random r = new Random();

            //tilings[0].Save("flatgrass.png", ImageFormat.Png);
            /*
            for (int i = 0; i < 9; i++)
            {
                grid[i, 0] = 0;
                grid[i, 1] = 0;
                grid[i, 16] = 0;
            }
            for (int i = 1; i < 16; i++)
            {
                grid[0, i] = 0;
                grid[8, i] = 0;
            }*/

            int[,] takenLocations = new int[17, 33];
            for (int i = 0; i < 17; i++)
            {
                for (int j = 0; j < 33; j++)
                {
                    takenLocations[i, j] = 0;
                    grid[i, j] = 0;
                }
            }
            List<Tuple<int, int>> p = getSoftPath(10, 33);
            foreach (Tuple<int, int> t in p)
            {
                grid[t.Item1 + 6, t.Item2] = 9;
                takenLocations[t.Item1 + 6, t.Item2] = 1;
            }
            int numMountains = r.Next(17, 30);
            int iter = 0;
            int rx = r.Next(15) + 1, ry = r.Next(30) + 2;
            do
            {
                if (takenLocations[rx, ry] < 1 && r.Next(6) > 0 && ((ry + 1) / 2 != ry))
                {
                    takenLocations[rx, ry] = 2;
                    grid[rx, ry] = r.Next(4, 6);
                    int ydir = ((ry + 1) / 2 > ry) ? 1 : -1;
                    int xdir = (ry % 2 == 0) ? rx + r.Next(2) : rx - r.Next(2);
                    if (xdir <= 1) xdir++;
                    if (xdir >= 15) xdir--;
                    rx = xdir;
                    ry = ry + ydir;

                }
                else
                {
                    rx = r.Next(15) + 1;
                    ry = r.Next(30) + 2;
                }
                iter++;
            } while (iter < numMountains);

            List<Tuple<int, int>> h = getHardPath(17, 13);
            foreach (Tuple<int, int> t in h)
            {
                grid[t.Item1, t.Item2 + 6] = 8;
                takenLocations[t.Item1, t.Item2 + 6] = 4;
            }

            int extreme = 0;
            switch (r.Next(5))
            {
                case 0: extreme = 7;
                    break;
                case 1: extreme = 2;
                    break;
                case 2: extreme = 2;
                    break;
                case 3: extreme = 1;
                    break;
                case 4: extreme = 1;
                    break;
            }
            for (int i = 1; i < 16; i++)
            {
                for (int j = 2; j < 31; j++)
                {
                    for (int v = 0; v < 3; v++)
                    {

                        int[] adj = { 0, 0, 0, 0,
                                        0,0,0,0,
                                    0, 0, 0, 0, };
                        adj[0] = grid[i, j + 1];
                        adj[1] = grid[i, j - 1];
                        if (j % 2 == 0)
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
                        if (!adj.Contains(1) && extreme == 2 && r.Next(5) > 1)
                            likeliest = extreme;
                        if ((adj.Contains(2) && r.Next(4) == 0))
                            likeliest = extreme;
                        if (extreme == 7 && (r.Next(4) == 0) || (adj.Contains(7) && r.Next(3) > 0))
                            likeliest = extreme;
                        if ((adj.Contains(1) && r.Next(5) > 1) || r.Next(4) == 0)
                            likeliest = r.Next(2) * 2 + 1;
                        if (adj.Contains(5) && r.Next(3) == 0)
                            likeliest = r.Next(4, 6);
                        if (r.Next(45) == 0)
                            likeliest = 6;
                        if (takenLocations[i, j] == 0)
                        {
                            grid[i, j] = likeliest;
                        }
                    }
                }
            }


            for (int j = 0; j < 33; j++)
            {
                for (int i = 0; i < 17; i++)
                {
                    g.DrawImageUnscaled(tilings[grid[i, j]], (128 * i) - ((j % 2 == 0) ? 0 : 64), (32 * j) - 32 + 8 - 32);
                }
            }
            return b;
        }
        static Bitmap makeTiling()
        {

            Bitmap[] tilings = new Bitmap[20];
            processFloor("Grass").CopyTo(tilings, 0);
            processFloor("Grass").CopyTo(tilings, 4);
            processFloor("Grass").CopyTo(tilings, 8);
            processFloor("Jungle").CopyTo(tilings, 12);
            processFloor("Forest").CopyTo(tilings, 16);


            Random r = new Random();
            Bitmap[] lines = new Bitmap[20];
            int showRoadsOrRivers = r.Next(1);
            if (showRoadsOrRivers == 0)
            {
                processFloor("Road_Cross").CopyTo(lines, 0);
                processFloor("Road_Curve").CopyTo(lines, 4);
                processFloor("Road_End").CopyTo(lines, 8);
                processFloor("Road_Straight").CopyTo(lines, 12);
                processFloor("Road_Tee").CopyTo(lines, 16);
            }
            else
            {
                processFloor("River_Cross").CopyTo(lines, 0);
                processFloor("River_Curve").CopyTo(lines, 4);
                processFloor("River_End").CopyTo(lines, 8);
                processFloor("River_Straight").CopyTo(lines, 12);
                processFloor("River_Tee").CopyTo(lines, 16);
            }
            Bitmap b = new Bitmap(88 * 9, 44 * 18);
            Graphics tiling = Graphics.FromImage(b);

            Bitmap[,] grid = new Bitmap[10, 19];
            Bitmap[,] midgrid = new Bitmap[9, 18];
            for (int i = 0; i < 10; i++)
            {
                grid[i, 0] = tilings[r.Next(4)];
                grid[i, 18] = tilings[r.Next(4)];
            }
            for (int i = 0; i < 19; i++)
            {
                grid[0, i] = tilings[r.Next(4)];
                grid[9, i] = tilings[r.Next(4)];
            }
            for (int i = 1; i < 9; i++)
            {
                for (int j = 1; j < 18; j++)
                {
                    grid[i, j] = tilings[r.Next(20)];
                }
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 18; j++)
                {
                    midgrid[i, j] = tilings[r.Next(20)];
                }
            }
            int rl = r.Next(2, 4), rw = r.Next(2, 5);
            int randNorthX = r.Next(rw + 1, 8 - rl);
            int randNorthY = r.Next(3, 15 - rl - rw);
            int randWestX = randNorthX - rw, randWestY = randNorthY + rw;
            int randEastX = randNorthX + rl, randEastY = randNorthY + rl;
            int randSouthX = randNorthX + rl - rw, randSouthY = randNorthY + rl + rw;

            for (int w = 0; w < rw; w++)
            {
                grid[randWestX + w, randWestY - w] = null;
                midgrid[randWestX + w, randWestY - w - 1] = null;
                grid[randSouthX + w, randSouthY - w] = null;
                midgrid[randSouthX + w, randSouthY - w - 1] = null;
            }
            grid[randWestX + rw, randWestY - rw] = null;
            grid[randSouthX + rw, randSouthY - rw] = null;
            for (int l = 0; l < rl; l++)
            {
                grid[randWestX + l, randWestY + l] = null;
                midgrid[randWestX + l, randWestY + l] = null;
                grid[randNorthX + l, randNorthY + l] = null;
                midgrid[randNorthX + l, randNorthY + l] = null;
            }
            grid[randWestX + rl, randWestY + rl] = null;
            grid[randNorthX + rl, randNorthY + rl] = null;
            rl = r.Next(2, 5);
            rw = r.Next(2, 4);

            randNorthX = r.Next(rw + 1, 8 - rl);
            randNorthY = r.Next(3, 16 - rl - rw);
            randWestX = randNorthX - rw;
            randWestY = randNorthY + rw;
            randEastX = randNorthX + rl;
            randEastY = randNorthY + rl;
            randSouthX = randNorthX + rl - rw;
            randSouthY = randNorthY + rl + rw;
            for (int w = 0; w < rw; w++)
            {
                grid[randWestX + w, randWestY - w] = null;
                midgrid[randWestX + w, randWestY - w - 1] = null;
                grid[randSouthX + w, randSouthY - w] = null;
                midgrid[randSouthX + w, randSouthY - w - 1] = null;
            }
            grid[randWestX + rw, randWestY - rw] = null;
            grid[randSouthX + rw, randSouthY - rw] = null;
            for (int l = 0; l < rl; l++)
            {
                grid[randWestX + l, randWestY + l] = null;
                midgrid[randWestX + l, randWestY + l] = null;
                grid[randNorthX + l, randNorthY + l] = null;
                midgrid[randNorthX + l, randNorthY + l] = null;
            }
            grid[randWestX + rl, randWestY + rl] = null;
            grid[randNorthX + rl, randNorthY + rl] = null;
            int[,] adjGrid = new int[10, 19], adjMidGrid = new int[9, 18];
            adjGrid.Initialize();
            adjMidGrid.Initialize();

            for (int i = 1; i < 9; i++)
            {
                for (int j = 1; j < 18; j++)
                {
                    /*if (grid[i, j] == null)
                    {
                        adjGrid[i, j] |= 1;
                    }*/
                    if (midgrid[i, j] == null) // southeast
                    {
                        adjGrid[i, j] |= 2;
                    }
                    if (midgrid[i - 1, j] == null) // southwest
                    {
                        adjGrid[i, j] |= 4;
                    }
                    if (midgrid[i - 1, j - 1] == null) // northwest
                    {
                        adjGrid[i, j] |= 8;
                    }
                    if (midgrid[i, j - 1] == null) // northeast
                    {
                        adjGrid[i, j] |= 16;
                    }
                }
            }

            for (int i = 1; i < 8; i++)
            {
                for (int j = 1; j < 18; j++)
                {
                    /*if (midgrid[i, j] == null)
                    {
                        adjMidGrid[i, j] |= 1;
                    }*/
                    if (grid[i + 1, j + 1] == null) // southeast
                    {
                        adjMidGrid[i, j] |= 2;
                    }
                    if (grid[i, j + 1] == null) // southwest
                    {
                        adjMidGrid[i, j] |= 4;
                    }
                    if (grid[i, j] == null) // northwest
                    {
                        adjMidGrid[i, j] |= 8;
                    }
                    if (grid[i + 1, j] == null) // northeast
                    {
                        adjMidGrid[i, j] |= 16;
                    }
                }
            }
            for (int i = 1; i < 9; i++)
            {
                for (int j = 1; j < 18; j++)
                {
                    if (grid[i, j] == null)
                    {
                        switch (adjGrid[i, j])
                        {
                            case 2: grid[i, j] = lines[8]; //se
                                break;
                            case 4: grid[i, j] = lines[9]; //sw
                                break;
                            case 8: grid[i, j] = lines[10]; //nw
                                break;
                            case 16: grid[i, j] = lines[11]; //ne
                                break;
                            case 6: grid[i, j] = lines[4]; //se sw
                                break;
                            case 10: grid[i, j] = lines[12]; //se nw
                                break;
                            case 12: grid[i, j] = lines[5]; //sw nw
                                break;
                            case 18: grid[i, j] = lines[7]; //ne se
                                break;
                            case 20: grid[i, j] = lines[13]; //sw ne
                                break;
                            case 24: grid[i, j] = lines[6]; //nw ne
                                break;
                            case 14: grid[i, j] = lines[17]; //se sw nw
                                break;
                            case 22: grid[i, j] = lines[16]; //ne se sw
                                break;
                            case 26: grid[i, j] = lines[19]; //se ne nw
                                break;
                            case 28: grid[i, j] = lines[18]; //sw nw ne
                                break;
                            case 30: grid[i, j] = lines[0]; //sw nw ne
                                break;
                            default: grid[i, j] = tilings[0];
                                break;
                        }
                    }
                }
            }

            for (int i = 1; i < 8; i++)
            {
                for (int j = 1; j < 18; j++)
                {
                    if (midgrid[i, j] == null)
                    {
                        switch (adjMidGrid[i, j])
                        {
                            case 2: midgrid[i, j] = lines[8]; //se
                                break;
                            case 4: midgrid[i, j] = lines[9]; //sw
                                break;
                            case 8: midgrid[i, j] = lines[10]; //nw
                                break;
                            case 16: midgrid[i, j] = lines[11]; //ne
                                break;
                            case 6: midgrid[i, j] = lines[4]; //se sw
                                break;
                            case 10: midgrid[i, j] = lines[12]; //se nw
                                break;
                            case 12: midgrid[i, j] = lines[5]; //sw nw
                                break;
                            case 18: midgrid[i, j] = lines[7]; //ne se
                                break;
                            case 20: midgrid[i, j] = lines[13]; //sw ne
                                break;
                            case 24: midgrid[i, j] = lines[6]; //nw ne
                                break;
                            case 14: midgrid[i, j] = lines[17]; //se sw nw
                                break;
                            case 22: midgrid[i, j] = lines[16]; //ne se sw
                                break;
                            case 26: midgrid[i, j] = lines[19]; //se ne nw
                                break;
                            case 28: midgrid[i, j] = lines[18]; //sw nw ne
                                break;
                            case 30: midgrid[i, j] = lines[0]; //sw nw ne
                                break;
                            default: midgrid[i, j] = tilings[0];
                                break;
                        }
                    }
                }
            }
            for (int j = 0; j < 18; j++)
            {
                for (int i = 0; i < 10; i++)
                {
                    tiling.DrawImageUnscaled(grid[i, j], (88 * i) - 44, (44 * j) - 22 - 7);
                }
                for (int i = 0; i < 9; i++)
                {
                    tiling.DrawImageUnscaled(midgrid[i, j], (88 * i), (44 * j) - 7);
                }
            }

            for (int i = 0; i < 10; i++)
            {
                tiling.DrawImageUnscaled(grid[i, 18], (88 * i) - 44, (44 * 18) - 22 - 7);
            }
            return b;
        }

        /// <summary>
        /// This will take a long time to run.  It should produce a ton of assets and an animated gif preview.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            VoxelLogic.Initialize();

            //            System.IO.Directory.CreateDirectory("Palettes");
            //            System.IO.Directory.CreateDirectory("indexed");
            /*
            renderOnlyTerrainColors().Save("PaletteTerrain.png", ImageFormat.Png);

            for (int c = 0; c < 8; c++)
            {
                renderOnlyColors(c).Save("PaletteColor" + c + ".png", ImageFormat.Png);
                renderOnlyTerrainColors(c).Save("PaletteTerrainColor" + c + ".png", ImageFormat.Png);
            }

            Madden();
            renderOnlyColors(7).Save("PaletteCrazy.png", ImageFormat.Png);
            */
            //InitializePalettes();
            //            Madden();
            //processTerrainChannel();
            //processReceiving();

            //makeFlatTiling().Save("tiling_flat.png", ImageFormat.Png);
            //            processUnitOutlinedDouble("Block");

            VoxelLogic.InitializeXPalette();
            VoxelLogic.InitializeWPalette();
            //            processUnitOutlinedWDouble("Person");
            /*
            processUnitOutlinedWDouble("Person", 0, true);
            processUnitOutlinedWDouble("Person", 1, true);
            processUnitOutlinedWDouble("Shinobi", 3, true);
            processUnitOutlinedWDouble("Shinobi_Unarmed", 3, true);
            processUnitOutlinedWDouble("Lord", 4, true);
            processUnitOutlinedWDouble("Guard", 5, true);
            */
            /*
            processUnitOutlinedWDouble("Human_Male", 15, true);
            processUnitOutlinedWalkDouble("Human_Male", 15);
            processUnitOutlinedWDouble("Human_Male", 16, true);
            processUnitOutlinedWalkDouble("Human_Male", 16);
            processUnitOutlinedWDouble("Human_Male", 17, true);
            processUnitOutlinedWalkDouble("Human_Male", 17);
            processUnitOutlinedWDouble("Human_Male", 0, true);
            processUnitOutlinedWalkDouble("Human_Male", 0);
            processUnitOutlinedWDouble("Human_Male", 1, true);
            processUnitOutlinedWalkDouble("Human_Male", 1);
            processUnitOutlinedWQuad("Nodebpe", 10, true);
            processUnitOutlinedWalkQuad("Nodebpe", 10);
            processUnitOutlinedWDouble("Kurguiv", 13, false);
            processUnitOutlinedWalkDouble("Kurguiv", 13);
            processUnitOutlinedWQuad("Nodebpe", 10, true);
            processUnitOutlinedWalkQuad("Nodebpe", 10);
            processUnitOutlinedWDouble("Tassar", 11, false);
            processUnitOutlinedWalkDouble("Tassar", 11);
            processUnitOutlinedWDouble("Ilapa", 12, true);
            processUnitOutlinedWalkDouble("Ilapa", 12);
            processUnitOutlinedWDouble("Erezdo", 14, true);
            processUnitOutlinedWalkDouble("Erezdo", 14);
            processUnitOutlinedWQuad("Vashk", 19, true);
            processUnitOutlinedWalkQuad("Vashk", 19);
            processUnitOutlinedWDouble("Cerpali", 18, true);
            processUnitOutlinedWalkDouble("Cerpali", 18);
            processUnitOutlinedWDouble("Human_Female", 15, true);
            processUnitOutlinedWalkDouble("Human_Female", 15);
            processUnitOutlinedWDouble("Human_Female", 16, true);
            processUnitOutlinedWalkDouble("Human_Female", 16);
            processUnitOutlinedWDouble("Human_Female", 17, true);
            processUnitOutlinedWalkDouble("Human_Female", 17);
            processUnitOutlinedWDouble("Human_Female", 0, true);
            processUnitOutlinedWalkDouble("Human_Female", 0);
            processUnitOutlinedWDouble("Human_Female", 1, true);
            processUnitOutlinedWalkDouble("Human_Female", 1);
            */
            processUnitOutlinedWDouble("Lomuk", 20, false);

            /*
            processUnitOutlinedWDouble("Zombie", 2, true);
            processUnitOutlinedWDouble("Skeleton", 6, true);
            processUnitOutlinedWDouble("Skeleton_Spear", 6, true);
            processUnitOutlinedWDouble("Spirit", 7, false);
            processUnitOutlinedWDouble("Wraith", 8, false);
            processUnitOutlinedWDouble("Cinder", 9, true);
            processUnitOutlinedWDouble("Spectral_Knight", 7, false);
          */
            /*
            System.IO.Directory.CreateDirectory("ortho");
            OrthoVoxels.InitializeXPalette();
            OrthoVoxels.InitializeWPalette();
            OrthoVoxels.processUnitOutlinedWQuad("Nodebpe", 10);
            OrthoVoxels.processUnitOutlinedWDouble("Tassar", 11);
            OrthoVoxels.processUnitOutlinedWDouble("Ilapa", 12);

            OrthoVoxels.processUnitOutlinedWDouble("Zombie", 2);
            OrthoVoxels.processUnitOutlinedWDouble("Skeleton", 6);
            OrthoVoxels.processUnitOutlinedWDouble("Skeleton_Spear", 6);
            OrthoVoxels.processUnitOutlinedWDouble("Spirit", 7);
            OrthoVoxels.processUnitOutlinedWDouble("Wraith", 8);
            OrthoVoxels.processUnitOutlinedWDouble("Cinder", 9);
            OrthoVoxels.processUnitOutlinedWDouble("Spectral_Knight", 7);
            */
            //processUnitOutlinedPartial("Copter");
            //processUnitOutlinedPartial("Copter_P");
            //processUnitOutlinedPartial("Copter_S");
            //processUnitOutlinedPartial("Copter_T");
            //processUnitOutlinedPartial("Infantry");
            //processUnitOutlinedPartial("Infantry_P");
            //processUnitOutlinedPartial("Infantry_S");
            //processUnitOutlinedPartial("Infantry_T");
            //processUnitOutlinedPartial("Tank");
            //processUnitOutlinedPartial("Tank_P");
            //processUnitOutlinedPartial("Tank_S");
            //processUnitOutlinedPartial("Tank_T");
            //processUnitOutlinedPartial("Artillery");
            //processUnitOutlinedPartial("Artillery_P");
            //processUnitOutlinedPartial("Artillery_S");
            //processUnitOutlinedPartial("Artillery_T");
            //processUnitOutlinedPartial("Supply");
            //processUnitOutlinedPartial("Supply_P");
            //processUnitOutlinedPartial("Supply_S");
            //processUnitOutlinedPartial("Supply_T");
            //processUnitOutlinedPartial("Plane");
            //processUnitOutlinedPartial("Plane_P");
            //processUnitOutlinedPartial("Plane_S");
            //processUnitOutlinedPartial("Plane_T");

            //CreateChannelBitmap(new Bitmap(88, 108, PixelFormat.Format32bppArgb), "indexed/clear.png");
            //CreateChannelBitmap(new Bitmap(128, 158, PixelFormat.Format32bppArgb), "indexed/clear_large.png");


            //processUnitOutlined("Block");
            /*
            Madden();
            processMedalChannel("Medal_P");
            processMedalChannel("Medal_S");
            processMedalChannel("Medal_T");
            
            processUnitChannel("Plane");
            processUnitChannel("Plane_P");
            processUnitChannel("Plane_S");
            processUnitChannel("Plane_T");

            processUnitChannel("Copter");
            processUnitChannel("Copter_P");
            processUnitChannel("Copter_S");
            processUnitChannel("Copter_T");
            
            processUnitChannel("Infantry");
            processUnitChannel("Infantry_P");
            processUnitChannel("Infantry_S");
            processUnitChannel("Infantry_T");

            processUnitChannel("Artillery");
            processUnitChannel("Artillery_P");
            processUnitChannel("Artillery_S");
            processUnitChannel("Artillery_T");

            processUnitChannel("Tank");
            processUnitChannel("Tank_P");
            processUnitChannel("Tank_S");
            processUnitChannel("Tank_T");

            processUnitChannel("Supply");
            processUnitChannel("Supply_P");
            processUnitChannel("Supply_S");
            processUnitChannel("Supply_T");
            
            processUnitChannel("City");
            processUnitChannel("Factory");
            processUnitChannel("Airport");
            processUnitChannel("Laboratory");
            processUnitChannel("Castle");
            processUnitChannel("Estate");
            

            //File.WriteAllText("FiringPositions.txt", log.ToString());

            /*
            processUnitOutlined("Infantry");
            processUnitOutlined("Infantry_P");
            processUnitOutlined("Infantry_S");
            processUnitOutlined("Infantry_T");

            processUnitOutlined("Artillery");
            processUnitOutlined("Artillery_P");
            processUnitOutlined("Artillery_S");
            processUnitOutlined("Artillery_T");

            processUnitOutlined("Tank");
            processUnitOutlined("Tank_P");
            processUnitOutlined("Tank_S");
            processUnitOutlined("Tank_T");
            
            processUnitOutlined("Supply");
            processUnitOutlined("Supply_P");
            processUnitOutlined("Supply_S");
            processUnitOutlined("Supply_T");
            
            processUnitOutlined("Plane");
            processUnitOutlined("Plane_P");
            processUnitOutlined("Plane_S");
            processUnitOutlined("Plane_T");
            
            processUnitOutlined("Copter");
            processUnitOutlined("Copter_P");
            processUnitOutlined("Copter_S");
            processUnitOutlined("Copter_T");

            
            processUnitOutlined("City");
            processUnitOutlined("Factory");
            processUnitOutlined("Airport");
            processUnitOutlined("Laboratory");
            processUnitOutlined("Castle");
            processUnitOutlined("Estate");
            
            */

            //       makeGamePreview(9, 18);


            /*
            processFloor("Grass");
            processFloor("Forest");
            processFloor("Jungle");
            
            processFloor("Road_Straight");
            processFloor("Road_Curve");
            processFloor("Road_Tee");
            processFloor("Road_End");
            processFloor("Road_Cross");

            processFloor("River_Straight");
            processFloor("River_Curve");
            processFloor("River_Tee");
            processFloor("River_End");
            processFloor("River_Cross");
            */
            //processBases();

            /*for (int i = 0; i < 40; i++)
            {
                makeFlatTiling().Save("bg" + i + ".png", ImageFormat.Png);
            }*/
            /*
            Bitmap[] randomTilings = new Bitmap[18];
            for (int i = 0; i < 18; i++)
            {
                randomTilings[i] = makeTiling();
            }
            Bitmap b = new Bitmap(720 + 72, 720 + 72);
            Graphics g = Graphics.FromImage(b);
            for (int i = 0; i < 18; i++)
            {
                g.DrawImageUnscaled(randomTilings[i], 264 * (i % 3), 132 * (i / 3));
            }
            b.Save("tiling_large.png", ImageFormat.Png);*/
            /*
            ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Program Files\ImageMagick-6.8.9-Q16\convert.EXE");
            startInfo.UseShellExecute = false;
            startInfo.Arguments = "tiling_large.png -modulate 110,45 tiling_grayed.png";
            Process.Start(startInfo).WaitForExit();
            */

        }
    }
}
