using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsPV
{
    public class KolonizePalettes
    {
        public const float flat_alpha = VoxelLogic.flat_alpha;
        public const float fuzz_alpha = VoxelLogic.fuzz_alpha;
        public const float waver_alpha = VoxelLogic.waver_alpha;
        public const float bordered_alpha = VoxelLogic.bordered_alpha;
        public const float borderless_alpha = VoxelLogic.borderless_alpha;
        public const float eraser_alpha = VoxelLogic.eraser_alpha;
        public const float spin_alpha_0 = VoxelLogic.spin_alpha_0;
        public const float spin_alpha_1 = VoxelLogic.spin_alpha_1;
        public const float flash_alpha_0 = VoxelLogic.flash_alpha_0;
        public const float flash_alpha_1 = VoxelLogic.flash_alpha_1;
        public const float gloss_alpha = VoxelLogic.gloss_alpha;

        public static float[][][] kolonize1 = new float[][][]
        {
            new float[][] { //0 green shirt, blue jeans human
            //0 shoes, boots, brown leather
            new float[] {0.45F,0.25F,0.1F,1F},
            //1 pants
            new float[] {0.5F,0.65F,0.95F,1F},
            //2 shirt
            new float[] {0.3F,0.55F,0.3F,1F},
            //3 hair
            new float[] {0.4F,0.15F,0.05F,1F},
            //4 lips/ears
            new float[] {0.83F,0.49F,0.18F,1F},
            //5 skin
            new float[] {0.93F,0.74F,0.39F,1F},
            //6 eyes shine
            new float[] {1.4F,1.4F,1.4F,1F},
            //7 eyes
            new float[] {0.15F,0.1F,0.0F,1F},
            //8 metal shine
            new float[] {0.7F,0.85F,1.1F,1F},
            //9 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //10 flowing clothes
            new float[] {0.3F,0.55F,0.3F,waver_alpha},
            //11 flowing clothes pattern
            new float[] {0.15F,0.45F,0.1F,waver_alpha},
            //12 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //13 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //14 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //15 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //16 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //17 flickering sparks
            new float[] {1.3F,0.9F,0.55F,borderless_alpha},
            //18 flickering sparks alternate
            new float[] {0.85F,0.1F,0.7F,borderless_alpha},
            //19 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //20 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //21 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //22 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //23 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //24 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //25 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //26 fuzz deepest
            new float[] {0.5F,0.0F,-0.05F,fuzz_alpha},
            //27 fuzz deep
            new float[] {0.55F,0.05F,-0.05F,fuzz_alpha},
            //28 fuzz mid-deep
            new float[] {0.6F,0.1F,0.0F,fuzz_alpha},
            //29 fuzz mid-light
            new float[] {0.65F,0.15F,0.05F,fuzz_alpha},
            //30 fuzz light
            new float[] {0.7F,0.2F,0.1F,fuzz_alpha},
            //31 fuzz lightest
            new float[] {0.8F,0.25F,0.15F,fuzz_alpha},
            //32 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //33 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //34 spinning objects pattern, even frames 
            new float[] {0.5F,0.5F,0.5F,spin_alpha_0},
            //35 spinning objects, even frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_0},
            //36 spinning objects pattern, odd frames
            new float[] {0.5F,0.5F,0.5F,spin_alpha_1},
            //37 spinning objects, odd frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_1},
            //38 dripping fluid frame 0
            new float[] {0.4F,0.6F,0.9F,1F},
            //39 dripping fluid frame 1
            new float[] {0.4F,0.6F,0.9F,0F},
            //40 dripping fluid frame 2
            new float[] {0.4F,0.6F,0.9F,0F},
            //41 dripping fluid frame 3
            new float[] {0.4F,0.6F,0.9F,0F},
            //42 always red
            new float[] {0.9F,0.05F,0.0F,1F},
            //43 always orange
            new float[] {0.95F,0.4F,-0.05F,1F},
            //44 always yellow
            new float[] {1.05F,1.05F,0.25F,1F},
            //45 always green
            new float[] {0.25F,0.55F,0.1F,1F},
            //46 always blue
            new float[] {0.05F,0.05F,0.85F,1F},
            //47 always violet
            new float[] {0.35F,0.1F,0.55F,1F},
            //48 always brown
            new float[] {0.55F,0.4F,0.25F,1F},
            //49 always tan
            new float[] {0.85F,0.7F,0.45F,1F},
            //50 always pink
            new float[] {0.95F,0.3F,0.7F,1F},
            //51 always black
            new float[] {0.0F,-0.03F,-0.09F,1F},
            //52 always dark gray
            new float[] {0.25F,0.22F,0.16F,1F},
            //53 always light gray
            new float[] {0.65F,0.65F,0.65F,1F},
            //54 always off-white
            new float[] {0.9F,0.9F,0.85F,1F},
            //55 always bright white
            new float[] {1.25F,1.25F,0.95F,1F},
            //56 always gold
            new float[] {0.92F,0.85F,0.4F,1F},
            //57 always silver
            new float[] {0.7F,0.77F,0.83F,1F},
            //58 total transparent
            new float[] {0F,0F,0F,0F},
            },
            
            new float[][] { //1 sorcerer
            //0 shoes, boots, brown leather
            new float[] {0.45F,0.25F,0.1F,1F},
            //1 pants
            new float[] {0.3F,0.24F,0.22F,1F},
            //2 shirt
            new float[] {0.1F,0.0F,0.0F,1F},
            //3 hair
            new float[] {0.4F,0.15F,0.05F,1F},
            //4 lips/ears
            new float[] {0.83F,0.49F,0.18F,1F},
            //5 skin
            new float[] {0.93F,0.74F,0.39F,1F},
            //6 eyes shine
            new float[] {1.4F,1.4F,1.4F,1F},
            //7 eyes
            new float[] {0.15F,0.1F,0.0F,1F},
            //8 metal shine
            new float[] {0.7F,0.85F,1.1F,1F},
            //9 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //10 flowing clothes
            new float[] {0.3F,0.55F,0.3F,waver_alpha},
            //11 flowing clothes pattern
            new float[] {0.15F,0.45F,0.1F,waver_alpha},
            //12 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //13 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //14 yellow fire
            new float[] {1.25F,1.1F,0.45F,borderless_alpha},
            //15 orange fire
            new float[] {1.25F,0.7F,0.3F,borderless_alpha},
            //16 sparks
            new float[] {1.3F,1.2F,0.85F,borderless_alpha},
            //17 flickering sparks
            new float[] {0.15F,0.8F,0.4F,borderless_alpha},
            //18 flickering sparks alternate
            new float[] {0.85F,0.1F,0.7F,borderless_alpha},
            //19 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //20 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //21 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //22 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //23 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //24 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //25 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //26 fuzz deepest
            new float[] {0.5F,0.0F,-0.05F,fuzz_alpha},
            //27 fuzz deep
            new float[] {0.55F,0.05F,-0.05F,fuzz_alpha},
            //28 fuzz mid-deep
            new float[] {0.6F,0.1F,0.0F,fuzz_alpha},
            //29 fuzz mid-light
            new float[] {0.65F,0.15F,0.05F,fuzz_alpha},
            //30 fuzz light
            new float[] {0.7F,0.2F,0.1F,fuzz_alpha},
            //31 fuzz lightest
            new float[] {0.8F,0.25F,0.15F,fuzz_alpha},
            //32 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //33 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //34 spinning objects pattern, even frames 
            new float[] {0.5F,0.5F,0.5F,spin_alpha_0},
            //35 spinning objects, even frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_0},
            //36 spinning objects pattern, odd frames
            new float[] {0.5F,0.5F,0.5F,spin_alpha_1},
            //37 spinning objects, odd frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_1},
            //38 dripping fluid frame 0
            new float[] {0.4F,0.6F,0.9F,1F},
            //39 dripping fluid frame 1
            new float[] {0.4F,0.6F,0.9F,0F},
            //40 dripping fluid frame 2
            new float[] {0.4F,0.6F,0.9F,0F},
            //41 dripping fluid frame 3
            new float[] {0.4F,0.6F,0.9F,0F},
            //42 always red
            new float[] {0.9F,0.05F,0.0F,1F},
            //43 always orange
            new float[] {0.95F,0.4F,-0.05F,1F},
            //44 always yellow
            new float[] {1.05F,1.05F,0.25F,1F},
            //45 always green
            new float[] {0.25F,0.55F,0.1F,1F},
            //46 always blue
            new float[] {0.05F,0.05F,0.85F,1F},
            //47 always violet
            new float[] {0.35F,0.1F,0.55F,1F},
            //48 always brown
            new float[] {0.55F,0.4F,0.25F,1F},
            //49 always tan
            new float[] {0.85F,0.7F,0.45F,1F},
            //50 always pink
            new float[] {0.95F,0.3F,0.7F,1F},
            //51 always black
            new float[] {0.0F,-0.03F,-0.09F,1F},
            //52 always dark gray
            new float[] {0.25F,0.22F,0.16F,1F},
            //53 always light gray
            new float[] {0.65F,0.65F,0.65F,1F},
            //54 always off-white
            new float[] {0.9F,0.9F,0.85F,1F},
            //55 always bright white
            new float[] {1.25F,1.25F,0.95F,1F},
            //56 always gold
            new float[] {1.2F,1.16F,-0.1F,1F},
            //57 always silver
            new float[] {0.7F,0.77F,0.83F,1F},
            //58 total transparent
            new float[] {0F,0F,0F,0F},
            },

            new float[][] { //2 occultist
            //0 shoes
            new float[] {0.25F,0.25F,0.25F,1F},
            //1 pants
            new float[] {0.1F,0.1F,0.1F,1F},
            //2 shirt
            new float[] {0.42F,0.07F,0.02F,1F},
            //3 hair
            new float[] {0.4F,0.15F,0.05F,1F},
            //4 lips/ears
            new float[] {0.83F,0.49F,0.18F,1F},
            //5 skin
            new float[] {0.93F,0.74F,0.39F,1F},
            //6 eyes shine
            new float[] {1.4F,1.4F,1.4F,1F},
            //7 eyes
            new float[] {0.15F,0.1F,0.0F,1F},
            //8 metal shine
            new float[] {0.7F,0.85F,1.1F,1F},
            //9 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //10 flowing clothes
            new float[] {0.3F,0.05F,0.0F,1F},
            //11 flowing clothes pattern
            new float[] {0.5F,0.1F,0.05F,1F},
            //12 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //13 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //14 yellow fire
            new float[] {1.25F,1.1F,0.45F,borderless_alpha},
            //15 orange fire
            new float[] {1.25F,0.7F,0.3F,borderless_alpha},
            //16 sparks
            new float[] {1.3F,1.2F,0.85F,borderless_alpha},
            //17 flickering sparks
            new float[] {0.15F,0.8F,0.4F,borderless_alpha},
            //18 flickering sparks alternate
            new float[] {0.85F,0.1F,0.7F,borderless_alpha},
            //19 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //20 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //21 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //22 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //23 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //24 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //25 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //26 fuzz deepest
            new float[] {0.5F,0.0F,-0.05F,fuzz_alpha},
            //27 fuzz deep
            new float[] {0.55F,0.05F,-0.05F,fuzz_alpha},
            //28 fuzz mid-deep
            new float[] {0.6F,0.1F,0.0F,fuzz_alpha},
            //29 fuzz mid-light
            new float[] {0.65F,0.15F,0.05F,fuzz_alpha},
            //30 fuzz light
            new float[] {0.7F,0.2F,0.1F,fuzz_alpha},
            //31 fuzz lightest
            new float[] {0.8F,0.25F,0.15F,fuzz_alpha},
            //32 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //33 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //34 spinning objects pattern, even frames 
            new float[] {0.5F,0.5F,0.5F,spin_alpha_0},
            //35 spinning objects, even frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_0},
            //36 spinning objects pattern, odd frames
            new float[] {0.5F,0.5F,0.5F,spin_alpha_1},
            //37 spinning objects, odd frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_1},
            //38 dripping fluid frame 0
            new float[] {0.4F,0.6F,0.9F,1F},
            //39 dripping fluid frame 1
            new float[] {0.4F,0.6F,0.9F,0F},
            //40 dripping fluid frame 2
            new float[] {0.4F,0.6F,0.9F,0F},
            //41 dripping fluid frame 3
            new float[] {0.4F,0.6F,0.9F,0F},
            //42 always red
            new float[] {0.9F,0.05F,0.0F,1F},
            //43 always orange
            new float[] {0.95F,0.4F,-0.05F,1F},
            //44 always yellow
            new float[] {1.05F,1.05F,0.25F,1F},
            //45 always green
            new float[] {0.25F,0.55F,0.1F,1F},
            //46 always blue
            new float[] {0.05F,0.05F,0.85F,1F},
            //47 always violet
            new float[] {0.35F,0.1F,0.55F,1F},
            //48 always brown
            new float[] {0.55F,0.4F,0.25F,1F},
            //49 always tan
            new float[] {0.85F,0.7F,0.45F,1F},
            //50 always pink
            new float[] {0.95F,0.3F,0.7F,1F},
            //51 always black
            new float[] {0.0F,-0.03F,-0.09F,1F},
            //52 always dark gray
            new float[] {0.25F,0.22F,0.16F,1F},
            //53 always light gray
            new float[] {0.65F,0.65F,0.65F,1F},
            //54 always off-white
            new float[] {0.9F,0.9F,0.85F,1F},
            //55 always bright white
            new float[] {1.25F,1.25F,0.95F,1F},
            //56 always gold
            new float[] {1.2F,1.16F,-0.1F,1F},
            //57 always silver
            new float[] {0.7F,0.77F,0.83F,1F},
            //58 total transparent
            new float[] {0F,0F,0F,0F},
            },

        };
        public static float[][][] fleshTones = new float[][][]
        {
            new float[][] { //0 brown hair
            //3 hair
            new float[] {0.4F,0.15F,0.05F,1F},
            //4 lips/ears
            new float[] {0.83F,0.49F,0.18F,1F},
            //5 skin
            new float[] {0.93F,0.74F,0.39F,1F},
            //6 eyes shine
            new float[] {1.4F,1.4F,1.4F,1F},
            //7 eyes
            new float[] {0.15F,0.1F,0.0F,1F},
            },
            
            new float[][] { //1 blonde hair
            //3 hair
            new float[] {0.97F,0.89F,0.39F,1F},
            //4 lips/ears
            new float[] {0.82F,0.50F,0.16F,1F},
            //5 skin
            new float[] {0.97F,0.80F,0.42F,1F},
            //6 eyes shine
            new float[] {1.4F,1.4F,1.4F,1F},
            //7 eyes
            new float[] {0.02F,0.18F,0.29F,1F},
            },

            new float[][] { //2 red hair
            //3 hair
            new float[] {0.9F,0.4F,-0.05F,1F},
            //4 lips/ears
            new float[] {0.88F,0.41F,0.18F,1F},
            //5 skin
            new float[] {1.02F,0.84F,0.55F,1F},
            //6 eyes shine
            new float[] {1.4F,1.4F,1.4F,1F},
            //7 eyes
            new float[] {0.02F,0.18F,0.29F,1F},
            },
            
            new float[][] { //3 gold skin
            //3 hair
            new float[] {0.05F,0.0F,-0.05F,1F},
            //4 lips/ears
            new float[] {0.72F,0.53F,0.05F,1F},
            //5 skin
            new float[] {0.94F,0.79F,0.37F,1F},
            //6 eyes shine
            new float[] {1.4F,1.4F,1.4F,1F},
            //7 eyes
            new float[] {0.1F,0.1F,0.05F,1F},
            },
            
            new float[][] { //4 very dark skin
            //3 hair
            new float[] {0.05F,0.03F,0.03F,1F},
            //4 lips/ears
            new float[] {0.2F,0.1F,0.1F,1F},
            //5 skin
            new float[] {0.38F,0.22F,0.1F,1F},
            //6 eyes shine
            new float[] {1.4F,1.4F,1.4F,1F},
            //7 eyes
            new float[] {0.15F,0.05F,-0.1F,1F},
            },

            new float[][] { //4 medium-dark skin
            //3 hair
            new float[] {-0.05F,-0.05F,-0.05F,1F},
            //4 lips/ears
            new float[] {0.51F,0.3F,0.1F,1F},
            //5 skin
            new float[] {0.66F,0.42F,0.2F,1F},
            //6 eyes shine
            new float[] {1.4F,1.4F,1.4F,1F},
            //7 eyes
            new float[] {0.15F,0.1F,-0.03F,1F},
            },
            
            new float[][] { //5 red-brown skin
            //3 hair
            new float[] {0.03F,0.0F,-0.05F,1F},
            //4 lips/ears
            new float[] {0.4F,0.15F,0.0F,1F},
            //5 skin
            new float[] {0.65F,0.3F,0.15F,1F},
            //6 eyes shine
            new float[] {1.4F,1.4F,1.4F,1F},
            //7 eyes
            new float[] {0.15F,0.05F,-0.1F,1F},
            },
        };
        public static float[][][][] kolonizes = new float[][][][] { kolonize1, kolonize1.Replicate() };


        public static void Initialize()
        {
            VoxelLogic.kcolorcount = kolonizes[0][0].Length;
            VoxelLogic.clear = (byte)(253 - (VoxelLogic.kcolorcount - 1) * 4);
            for (int s = 0; s < kolonizes.Length; s++)
            {
                for (int p = 0; p < kolonizes[s].Length; p++)
                {
                    float mod = s * 0.18f;
                    for (int c = 0; c < kolonizes[s][p].Length; c++)
                    {
                        if (c >= 3 && c <= 7)
                            continue;
                        if (c >= 12 && c <= 23)
                            continue;
                        kolonizes[s][p][c][0] -= mod;
                        kolonizes[s][p][c][1] -= mod;
                        kolonizes[s][p][c][2] -= mod;
                    }
                }

            }
        }

    }
}
