using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsPV
{
    public class DungeonPalettes
    {
        public const float flat_alpha = VoxelLogic.flat_alpha;
        public const float fuzz_alpha = VoxelLogic.fuzz_alpha;
        public const float waver_alpha = VoxelLogic.waver_alpha;
        public const float yver_alpha = VoxelLogic.yver_alpha;
        public const float bordered_alpha = VoxelLogic.bordered_alpha;
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

        public static float[][][] dungeon_palettes = new float[][][]
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
                new float[] {0.9F,0.95F,1.1F,1F},
                //9 metal
                new float[] {0.63F,0.66F,0.7F,1F},
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
                new float[] {0.4F,0.6F,0.9F,borderless_alpha},
                //26 fuzz contrast
                new float[] {0.5F,0.0F,-0.05F,fuzz_alpha},
                //27 fuzz
                new float[] {0.55F,0.05F,-0.05F,fuzz_alpha},
                //28 faded deep
                new float[] {0.6F,0.6F,0.6F,fade_alpha},
                //29 faded dark
                new float[] {0.66F,0.66F,0.66F,fade_alpha},
                //30 faded light            
                new float[] {0.72F,0.72F,0.72F,fade_alpha},
                //31 faded lightest
                new float[] {0.78F,0.78F,0.78F,fade_alpha},
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
                new float[] {0.9F,0.95F,1.1F,1F},
                //9 metal
                new float[] {0.63F,0.66F,0.7F,1F},
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
                new float[] {0.4F,0.6F,0.9F,borderless_alpha},
                //26 fuzz contrast
                new float[] {0.5F,0.0F,-0.05F,fuzz_alpha},
                //27 fuzz
                new float[] {0.55F,0.05F,-0.05F,fuzz_alpha},
                //28 faded deep
                new float[] {0.54F,0.54F,0.54F,fade_alpha},
                //29 faded dark
                new float[] {0.6F,0.6F,0.6F,fade_alpha},
                //30 faded light            
                new float[] {0.66F,0.66F,0.66F,fade_alpha},
                //31 faded lightest
                new float[] {0.72F,0.72F,0.72F,fade_alpha},
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
                new float[] {0.9F,0.95F,1.1F,1F},
                //9 metal
                new float[] {0.63F,0.66F,0.7F,1F},
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
                new float[] {0.4F,0.6F,0.9F,borderless_alpha},
                //26 fuzz contrast
                new float[] {0.5F,0.0F,-0.05F,fuzz_alpha},
                //27 fuzz
                new float[] {0.55F,0.05F,-0.05F,fuzz_alpha},
                //28 faded deep
                new float[] {0.54F,0.54F,0.54F,fade_alpha},
                //29 faded dark
                new float[] {0.6F,0.6F,0.6F,fade_alpha},
                //30 faded light            
                new float[] {0.66F,0.66F,0.66F,fade_alpha},
                //31 faded lightest
                new float[] {0.72F,0.72F,0.72F,fade_alpha},
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

            new float[][] { //3 wolf
                //0 claws
                new float[] {0.8F,0.7F,0.45F,1F},
                //1 dark parts of skin
                new float[] {0.05F,0.0F,-0.05F,1F},
                //2 shirt
                new float[] {0.42F,0.07F,0.02F,1F},
                //3 hair
                new float[] {0.4F,0.15F,0.05F,1F},
                //4 lips/ears
                new float[] {0.4F,0.15F,0.0F,1F},
                //5 skin
                new float[] {0.85F,0.5F,0.6F,1F},
                //6 eyes shine
                new float[] {1.4F,1.4F,1.4F,1F},
                //7 eyes
                new float[] {0.0F,0.1F,0.15F,1F},
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
                new float[] {0.4F,0.6F,0.9F,borderless_alpha},
                //26 fuzz contrast
                new float[] {0.5F,0.5F,0.5F,fuzz_alpha},
                //27 fuzz
                new float[] {0.6F,0.6F,0.6F,fuzz_alpha},
                //28 faded deep
                new float[] {0.54F,0.54F,0.54F,fade_alpha},
                //29 faded dark
                new float[] {0.6F,0.6F,0.6F,fade_alpha},
                //30 faded light            
                new float[] {0.66F,0.66F,0.66F,fade_alpha},
                //31 faded lightest
                new float[] {0.72F,0.72F,0.72F,fade_alpha},
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

            new float[][] { //4 stone dungeon fixtures
                            //0 shoes, boots, brown leather
                new float[] {0.45F,0.25F,0.1F,1F},
                //1 wood
                new float[] {0.39F,0.27F,0.06F,grain_some_alpha},
                //2 cloth
                new float[] {0.8F,0.75F,0.1F,1F},
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
                //10 waving banners
                new float[] {0.8F,0.75F,0.1F,waver_alpha},
                //11 waving banners pattern
                new float[] {1.1F,0.1F,0.0F,waver_alpha},
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
                new float[] {0.9F,0.85F,0.7F,1F},
                //20 glow frame 1
                new float[] {1.05F,1.0F,0.8F,1F},
                //21 glow frame 2
                new float[] {0.9F,0.85F,0.7F,1F},
                //22 glow frame 3
                new float[] {0.75F,0.7F,0.6F,1F},
                //23 shadow
                new float[] {0.1F,0.1F,0.1F,flat_alpha},
                //24 mud
                new float[] {0.2F,0.4F,0.3F,1F},
                //25 water
                new float[] {0.6F,0.75F,0.95F,borderless_alpha},
                //26 fuzz contrast
                new float[] {0.5F,0.0F,-0.05F,fuzz_alpha},
                //27 fuzz
                new float[] {0.55F,0.05F,-0.05F,fuzz_alpha},
                //28 faded deep
                new float[] {0.54F,0.54F,0.54F,fade_alpha},
                //29 faded dark
                new float[] {0.6F,0.6F,0.6F,fade_alpha},
                //30 faded light            
                new float[] {0.66F,0.66F,0.66F,fade_alpha},
                //31 faded lightest
                new float[] {0.72F,0.72F,0.72F,fade_alpha},
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

            new float[][] { //5 ant
                //0 claws
                new float[] {0.2F,0.14F,0.07F,1F},
                //1 body
                new float[] {0.32F,0.29F,0.25F,1F},
                //2 jaws
                new float[] {0.25F,0.21F,0.15F,1F},
                //3 hair
                new float[] {0.4F,0.15F,0.05F,1F},
                //4 stinger
                new float[] {0.3F,0.5F,0.2F,1F},
                //5 skin
                new float[] {0.85F,0.5F,0.6F,1F},
                //6 eyes shine
                new float[] {1.4F,1.4F,1.4F,1F},
                //7 eyes
                new float[] {0.15F,0.1F,0.02F,gloss_alpha},
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
                new float[] {0.4F,0.6F,0.9F,borderless_alpha},
                //26 fuzz contrast
                new float[] {0.5F,0.5F,0.5F,fuzz_alpha},
                //27 fuzz
                new float[] {0.6F,0.6F,0.6F,fuzz_alpha},
                //28 faded deep
                new float[] {0.54F,0.54F,0.54F,fade_alpha},
                //29 faded dark
                new float[] {0.6F,0.6F,0.6F,fade_alpha},
                //30 faded light            
                new float[] {0.66F,0.66F,0.66F,fade_alpha},
                //31 faded lightest
                new float[] {0.72F,0.72F,0.72F,fade_alpha},
                //32 gore
                new float[] {0.08F,0.04F,-0.1F,1F},
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


            new float[][] { //6 linnorm
                //0 claws
                new float[] {0.85F,0.68F,0.5F,1F},
                //1 body
                new float[] {0.1F,0.4F,-0.05F,1F},
                //2 shirt
                new float[] {0.42F,0.07F,0.02F,1F},
                //3 nose
                new float[] {0.25F,0.1F,0.03F,1F},
                //4 mouth
                new float[] {0.4F,0.15F,0.0F,1F},
                //5 skin
                new float[] {0.85F,0.5F,0.6F,1F},
                //6 eyes shine
                new float[] {1.4F,1.4F,1.4F,1F},
                //7 eyes
                new float[] {0.0F,0.1F,0.15F,1F},
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
                new float[] {0.4F,0.6F,0.9F,borderless_alpha},
                //26 fuzz contrast
                new float[] {0.5F,0.5F,0.5F,fuzz_alpha},
                //27 fuzz
                new float[] {0.6F,0.6F,0.6F,fuzz_alpha},
                //28 faded deep
                new float[] {0.54F,0.54F,0.54F,fade_alpha},
                //29 faded dark
                new float[] {0.6F,0.6F,0.6F,fade_alpha},
                //30 faded light            
                new float[] {0.66F,0.66F,0.66F,fade_alpha},
                //31 faded lightest
                new float[] {0.72F,0.72F,0.72F,fade_alpha},
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


            new float[][] { //7 armored human
                //0 shoes, boots, brown leather
                new float[] {0.35F,0.18F,0.08F,1F},
                //1 pants
                new float[] {0.4F,0.4F,0.4F,1F},
                //2 shirt
                new float[] {0.6F,0.6F,0.6F,1F},
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
                new float[] {0.9F,0.95F,1.1F,1F},
                //9 metal
                new float[] {0.63F,0.66F,0.7F,1F},
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
                new float[] {0.4F,0.6F,0.9F,borderless_alpha},
                //26 fuzz contrast
                new float[] {0.5F,0.0F,-0.05F,fuzz_alpha},
                //27 fuzz
                new float[] {0.55F,0.05F,-0.05F,fuzz_alpha},
                //28 faded deep
                new float[] {0.6F,0.6F,0.6F,fade_alpha},
                //29 faded dark
                new float[] {0.66F,0.66F,0.66F,fade_alpha},
                //30 faded light            
                new float[] {0.72F,0.72F,0.72F,fade_alpha},
                //31 faded lightest
                new float[] {0.78F,0.78F,0.78F,fade_alpha},
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

            new float[][] { //8 flashy mage
                //0 shoes, boots, black leather
                new float[] {0.1F,0.06F,-0.01F,1F},
                //1 pants
                new float[] {0.9F,0.82F,0.7F,1F},
                //2 shirt
                new float[] {0.7F,0.0F,0.1F,1F},
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
                new float[] {0.9F,0.95F,1.1F,1F},
                //9 metal
                new float[] {0.63F,0.66F,0.7F,1F},
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
                new float[] {0.05F,0.1F,0.95F,grain_hard_alpha},
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
                new float[] {0.4F,0.6F,0.9F,borderless_alpha},
                //26 fuzz contrast
                new float[] {0.5F,0.0F,-0.05F,fuzz_alpha},
                //27 fuzz
                new float[] {0.55F,0.05F,-0.05F,fuzz_alpha},
                //28 faded deep
                new float[] {0.6F,0.6F,0.6F,fade_alpha},
                //29 faded dark
                new float[] {0.66F,0.66F,0.66F,fade_alpha},
                //30 faded light            
                new float[] {0.72F,0.72F,0.72F,fade_alpha},
                //31 faded lightest
                new float[] {0.78F,0.78F,0.78F,fade_alpha},
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

            new float[][] { //9 fancy clothes
                //0 shoes, boots, black leather
                new float[] {0.1F,0.06F,-0.01F,1F},
                //1 pants, purse
                new float[] {1.05F,1.0F,0.65F,1F},
                //2 shirt, dress
                new float[] {0.55F,-0.03F,0.05F,1F},
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
                new float[] {0.9F,0.95F,1.1F,1F},
                //9 metal
                new float[] {0.63F,0.66F,0.7F,1F},
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
                new float[] {0.4F,0.6F,0.9F,borderless_alpha},
                //26 fuzz contrast
                new float[] {0.5F,0.0F,-0.05F,fuzz_alpha},
                //27 fuzz
                new float[] {0.55F,0.05F,-0.05F,fuzz_alpha},
                //28 faded deep
                new float[] {0.6F,0.6F,0.6F,fade_alpha},
                //29 faded dark
                new float[] {0.66F,0.66F,0.66F,fade_alpha},
                //30 faded light            
                new float[] {0.72F,0.72F,0.72F,fade_alpha},
                //31 faded lightest
                new float[] {0.78F,0.78F,0.78F,fade_alpha},
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

            new float[][] { //10 light clothes
                //0 shoes, boots, tanned leather
                new float[] {0.69F,0.57F,0.38F,1F},
                //1 pants, purse
                new float[] {1.15F,0.95F,0.85F,1F},
                //2 shirt, dress
                new float[] {1.1F,1.1F,1.1F,1F},
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
                new float[] {0.9F,0.95F,1.1F,1F},
                //9 metal
                new float[] {0.63F,0.66F,0.7F,1F},
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
                new float[] {0.4F,0.6F,0.9F,borderless_alpha},
                //26 fuzz contrast
                new float[] {0.5F,0.0F,-0.05F,fuzz_alpha},
                //27 fuzz
                new float[] {0.55F,0.05F,-0.05F,fuzz_alpha},
                //28 faded deep
                new float[] {0.6F,0.6F,0.6F,fade_alpha},
                //29 faded dark
                new float[] {0.66F,0.66F,0.66F,fade_alpha},
                //30 faded light            
                new float[] {0.72F,0.72F,0.72F,fade_alpha},
                //31 faded lightest
                new float[] {0.78F,0.78F,0.78F,fade_alpha},
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

        };

        public static float[][][] mecha_palettes = new float[][][]
        {

            new float[][] { //blue steel
                            //0 guns
                new float[] {0.5F,0.47F,0.48F,1F},
                //1 highlight paint
                new float[] {0.65F,0.7F,0.8F,1F},
                //2 base paint
                new float[] {0.55F,0.65F,0.7F,1F},
                //3 FILTHY HUMAN hair
                new float[] {0.4F,0.15F,0.05F,1F},
                //4 FILTHY HUMAN lips/ears
                new float[] {0.83F,0.49F,0.18F,1F},
                //5 FILTHY HUMAN skin
                new float[] {0.93F,0.74F,0.39F,1F},
                //6 FILTHY HUMAN eyes shine
                new float[] {1.4F,1.4F,1.4F,1F},
                //7 FILTHY HUMAN eyes
                new float[] {0.15F,0.1F,0.0F,1F},
                //8 chrome shine
                new float[] {0.95F,0.95F,1.1F,1F},
                //9 chrome
                new float[] {0.8F,0.8F,0.85F,1F},
                //10 non-euclidean ectoplasm (x)
                new float[] {0.45F,0.75F,0.6F,waver_alpha},
                //11 non-euclidean ectoplasm (y)
                new float[] {0.45F,0.75F,0.6F,yver_alpha},
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
                //17 flickering sparks electric
                new float[] {1.0F,0.75F,1.1F,borderless_alpha},
                //18 flickering sparks plasma
                new float[] {1.3F,1.3F,0.9F,borderless_alpha},
                //19 glow frame 0
                new float[] {1.1F,1.0F,0.65F,borderless_alpha},
                //20 glow frame 1
                new float[] {1.25F,1.15F,0.8F,borderless_alpha},
                //21 glow frame 2
                new float[] {1.1F,1.0F,0.65F,borderless_alpha},
                //22 glow frame 3
                new float[] {0.95F,0.85F,0.5F,borderless_alpha},
                //23 shadow
                new float[] {0.1F,0.1F,0.1F,flat_alpha},
                //24 mud
                new float[] {0.2F,0.4F,0.3F,1F},
                //25 water
                new float[] {0.6F,0.65F,0.9F,borderless_alpha},
                //26 fuzz contrast
                new float[] {0.55F,0.05F,-0.05F,fuzz_alpha},
                //27 fuzz
                new float[] {0.6F,0.1F,0.0F,fuzz_alpha},
                //28 faded deep
                new float[] {0.54F,0.54F,0.54F,fade_alpha},
                //29 faded dark
                new float[] {0.6F,0.6F,0.6F,fade_alpha},
                //30 faded light            
                new float[] {0.66F,0.66F,0.66F,fade_alpha},
                //31 faded lightest
                new float[] {0.72F,0.72F,0.72F,fade_alpha},
                //32 oil
                new float[] {0.2F,0.17F,-0.1F,1F},
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
                //52 always dark gray, used for tinted lenses
                new float[] {0.25F,0.22F,0.16F,1F},
                //53 always light gray, used for raw unpainted metal
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


            /*
            new float[][] { //0 bold red mecha
            //0 raw metal contrast -> 53.5
            new float[] {0.45F,0.48F,0.5F,1F},
            //1 raw metal -> 53
            new float[] {0.6F,0.63F,0.65F,1F},
            //2 highlight paint contrast -> 1.5
            new float[] {0.5F,0.1F,-0.05F,1F},
            //3 highlight paint -> 1
            new float[] {0.7F,0.2F,0.05F,1F},
            //4 base paint contrast -> 2.5
            new float[] {0.3F,-0.15F,-0.15F,1F},
            //5 base paint -> 2
            new float[] {0.45F,0.0F,0.0F,1F},
            //6 guns contrast -> 0.5
            new float[] {0.28F,0.23F,0.2F,1F},
            //7 guns -> 0
            new float[] {0.43F,0.38F,0.35F,1F},
            //8 flesh contrast
            new float[] {0.4F,0.15F,0.0F,1F},
            //9 flesh
            new float[] {0.88F,0.6F,0.42F,1F},
            //10 tinted lenses shine -> 55
            new float[] {1.15F,1.15F,1.2F,1F},
            //11 tinted lenses -> 52
            new float[] {0.18F,0.18F,0.2F,1F},
            //12 chrome contrast -> 8
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 chrome -> 9
            new float[] {0.6F,0.75F,0.95F,1F},
            //14 flowing cloth contrast -> 10
            new float[] {0.0F,0.0F,0.0F,waver_alpha},
            //15 flowing cloth -> 11
            new float[] {0.1F,0.1F,0.1F,waver_alpha},
            //16 inner shadow -> 12
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke -> 13
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire -> 14
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire -> 15
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks -> 16
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0 -> 19
            new float[] {1.1F,1.0F,0.65F,borderless_alpha},
            //22 glow frame 1 -> 20
            new float[] {1.25F,1.15F,0.8F,borderless_alpha},
            //23 glow frame 2 -> 21
            new float[] {1.1F,1.0F,0.65F,borderless_alpha},
            //24 glow frame 3 -> 22
            new float[] {0.95F,0.85F,0.5F,borderless_alpha},
            //25 shadow -> 23
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 oil -> 32
            new float[] {0.0F,-0.02F,-0.1F,1F},
            //27 water -> 25
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.6F,0.6F,0.1F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.7F,0.7F,0.15F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.8F,0.8F,0.2F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.9F,0.9F,0.3F,fuzz_alpha},
            //32 fuzz light
            new float[] {1.0F,1.0F,0.4F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {1.1F,1.1F,0.55F,fuzz_alpha},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 normal glass -> 33
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 spinning objects contrast, even frames 
            new float[] {0.5F,0.5F,0.5F,spin_alpha_0},
            //37 spinning objects, even frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_0},
            //38 spinning objects, odd frames
            new float[] {0.5F,0.5F,0.5F,spin_alpha_1},
            //39 spinning objects, odd frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_1},
            //40 flickering sparks
            new float[] {1.3F,1.3F,0.9F,borderless_alpha},
            //41 always green contrast
            new float[] {0.12F,0.35F,0.0F,1F},
            //42 always green
            new float[] {0.25F,0.55F,0.1F,1F},
            //43 always brown contrast
            new float[] {0.4F,0.25F,0.1F,1F},
            //44 always brown
            new float[] {0.55F,0.4F,0.25F,1F},
            //45 always tan contrast
            new float[] {0.7F,0.55F,0.3F,1F},
            //46 always tan
            new float[] {0.85F,0.7F,0.45F,1F},
            //47 always black contrast
            new float[] {0.0F,-0.03F,-0.09F,1F},
            //48 always black
            new float[] {0.15F,0.12F,0.06F,1F},
            //49 always white contrast
            new float[] {1.25F,1.25F,0.75F,1F},
            //50 always white
            new float[] {0.9F,0.9F,0.9F,1F},
            //51 always red contrast
            new float[] {0.85F,0.0F,-0.05F,1F},
            //52 always red
            new float[] {0.9F,0.05F,0.0F,1F},
            //53 always violet contrast
            new float[] {0.4F,0.1F,0.3F,1F},
            //54 always violet
            new float[] {0.3F,0.15F,0.5F,1F},
            //55 always gold
            new float[] {0.92F,0.85F,0.4F,1F},
            //56 always silver
            new float[] {0.7F,0.77F,0.83F,1F},
            //57 eraser
            new float[] {0F,0F,0F,eraser_alpha},
            //58 total transparent
            new float[] {0F,0F,0F,0F},
            },
            */
        };


        public static float[][][] mythos_palettes = new float[][][]
        {

            new float[][] { //green shoggoth
                            //0 claws, fangs
                new float[] {0.95F,0.9F,0.35F,1F},
                //1 non-euclidean form (x)
                new float[] {0.4F,0.6F,0.45F,waver_alpha},
                //2 non-euclidian form (y)
                new float[] {0.4F,0.6F,0.45F,yver_alpha},
                //3 FILTHY HUMAN hair
                new float[] {0.4F,0.15F,0.05F,1F},
                //4 FILTHY HUMAN lips/ears
                new float[] {0.83F,0.49F,0.18F,1F},
                //5 FILTHY HUMAN skin
                new float[] {0.93F,0.74F,0.39F,1F},
                //6 FILTHY HUMAN eyes shine
                new float[] {1.4F,1.4F,1.4F,1F},
                //7 FILTHY HUMAN eyes
                new float[] {0.15F,0.1F,0.0F,1F},
                //8 eyes shine
                new float[] {1.4F,1.4F,1.4F,waver_alpha},
                //9 eyes
                new float[] {0.15F,0.0F,0.0F,waver_alpha},
                //10 non-euclidean ectoplasm (x)
                new float[] {0.45F,0.75F,0.6F,waver_alpha},
                //11 non-euclidean ectoplasm (y)
                new float[] {0.45F,0.75F,0.6F,yver_alpha},
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
                //17 flickering sparks electric
                new float[] {1.0F,0.75F,1.1F,borderless_alpha},
                //18 flickering sparks plasma
                new float[] {1.3F,1.3F,0.9F,borderless_alpha},
                //19 glow frame 0
                new float[] {0.65F,1.05F,0.65F,waver_alpha},
                //20 glow frame 1
                new float[] {0.8F,1.2F,0.8F,waver_alpha},
                //21 glow frame 2
                new float[] {0.65F,1.05F,0.65F,waver_alpha},
                //22 glow frame 3
                new float[] {0.5F,0.9F,0.5F,waver_alpha},
                //23 shadow
                new float[] {0.1F,0.1F,0.1F,flat_alpha},
                //24 mouth
                new float[] {0.5F,0.1F,0.05F,1F},
                //25 water
                new float[] {0.4F,0.6F,0.9F,borderless_alpha},
                //26 fuzz contrast
                new float[] {0.5F,0.0F,-0.05F,fuzz_alpha},
                //27 fuzz deep
                new float[] {0.55F,0.05F,-0.05F,fuzz_alpha},
                //28 faded deep
                new float[] {0.54F,0.54F,0.54F,fade_alpha},
                //29 faded dark
                new float[] {0.6F,0.6F,0.6F,fade_alpha},
                //30 faded light            
                new float[] {0.66F,0.66F,0.66F,fade_alpha},
                //31 faded lightest
                new float[] {0.72F,0.72F,0.72F,fade_alpha},
                //32 green blood
                new float[] {0.25F,1.05F,0.2F,1F},
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
                new float[] {0.6F,0.85F,0.4F,1F},
                //39 dripping fluid frame 1
                new float[] {0.6F,0.85F,0.4F,0F},
                //40 dripping fluid frame 2
                new float[] {0.6F,0.85F,0.4F,0F},
                //41 dripping fluid frame 3
                new float[] {0.6F,0.85F,0.4F,0F},
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

            new float[][] { //yellow shoggoth
                            //0 claws, fangs
                new float[] {0.98F,0.92F,0.65F,1F},
                //1 non-euclidean form (x)
                new float[] {0.73F,0.7F,0.25F,waver_alpha},
                //2 non-euclidian form (y)
                new float[] {0.73F,0.7F,0.25F,yver_alpha},
                //3 FILTHY HUMAN hair
                new float[] {0.4F,0.15F,0.05F,1F},
                //4 FILTHY HUMAN lips/ears
                new float[] {0.83F,0.49F,0.18F,1F},
                //5 FILTHY HUMAN skin
                new float[] {0.93F,0.74F,0.39F,1F},
                //6 FILTHY HUMAN eyes shine
                new float[] {1.4F,1.4F,1.4F,1F},
                //7 FILTHY HUMAN eyes
                new float[] {0.15F,0.1F,0.0F,1F},
                //8 eyes shine
                new float[] {1.4F,1.4F,1.4F,waver_alpha},
                //9 eyes
                new float[] {0.1F,0.0F,0.05F,waver_alpha},
                //10 non-euclidean ectoplasm (x)
                new float[] {0.45F,0.75F,0.6F,waver_alpha},
                //11 non-euclidean ectoplasm (y)
                new float[] {0.45F,0.75F,0.6F,yver_alpha},
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
                //17 flickering sparks electric
                new float[] {1.0F,0.75F,1.1F,borderless_alpha},
                //18 flickering sparks plasma
                new float[] {1.3F,1.3F,0.9F,borderless_alpha},
                //19 glow frame 0
                new float[] {1.05F,1.05F,0.25F,waver_alpha},
                //20 glow frame 1
                new float[] {1.15F,1.15F,0.4F,waver_alpha},
                //21 glow frame 2
                new float[] {1.05F,1.05F,0.25F,waver_alpha},
                //22 glow frame 3
                new float[] {0.95F,0.95F,0.1F,waver_alpha},
                //23 shadow
                new float[] {0.1F,0.1F,0.1F,flat_alpha},
                //24 mouth
                new float[] {0.5F,0.1F,0.05F,1F},
                //25 water
                new float[] {0.4F,0.6F,0.9F,borderless_alpha},
                //26 fuzz contrast
                new float[] {0.5F,0.0F,-0.05F,fuzz_alpha},
                //27 fuzz
                new float[] {0.55F,0.05F,-0.05F,fuzz_alpha},
                //28 faded deep
                new float[] {0.54F,0.54F,0.54F,fade_alpha},
                //29 faded dark
                new float[] {0.6F,0.6F,0.6F,fade_alpha},
                //30 faded light            
                new float[] {0.66F,0.66F,0.66F,fade_alpha},
                //31 faded lightest
                new float[] {0.72F,0.72F,0.72F,fade_alpha},
                //32 yellow blood
                new float[] {0.58F,0.55F,0.1F,1F},
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
                new float[] {1.13F,1.1F,0.2F,1F},
                //39 dripping fluid frame 1
                new float[] {0.55F,0.8F,0.35F,0F},
                //40 dripping fluid frame 2
                new float[] {0.55F,0.8F,0.35F,0F},
                //41 dripping fluid frame 3
                new float[] {0.55F,0.8F,0.35F,0F},
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

            new float[][] { //purple shoggoth
                            //0 claws, fangs
                new float[] {0.75F,0.65F,0.45F,1F},
                //1 non-euclidean form (x)
                new float[] {0.5F,0.35F,0.55F,waver_alpha},
                //2 non-euclidian form (y)
                new float[] {0.5F,0.35F,0.55F,yver_alpha},
                //3 FILTHY HUMAN hair
                new float[] {0.4F,0.15F,0.05F,1F},
                //4 FILTHY HUMAN lips/ears
                new float[] {0.83F,0.49F,0.18F,1F},
                //5 FILTHY HUMAN skin
                new float[] {0.93F,0.74F,0.39F,1F},
                //6 FILTHY HUMAN eyes shine
                new float[] {1.4F,1.4F,1.4F,1F},
                //7 FILTHY HUMAN eyes
                new float[] {0.15F,0.1F,0.0F,1F},
                //8 eyes shine
                new float[] {1.4F,1.4F,1.4F,waver_alpha},
                //9 eyes
                new float[] {0.0F,0.0F,0.0F,waver_alpha},
                //10 non-euclidean ectoplasm (x)
                new float[] {0.45F,0.75F,0.6F,waver_alpha},
                //11 non-euclidean ectoplasm (y)
                new float[] {0.45F,0.75F,0.6F,yver_alpha},
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
                //17 flickering sparks electric
                new float[] {1.0F,0.75F,1.1F,borderless_alpha},
                //18 flickering sparks plasma
                new float[] {1.3F,1.3F,0.9F,borderless_alpha},
                //19 glow frame 0
                new float[] {0.95F,0.55F,0.95F,waver_alpha},
                //20 glow frame 1
                new float[] {1.05F,0.7F,1.05F,waver_alpha},
                //21 glow frame 2
                new float[] {0.95F,0.55F,0.95F,waver_alpha},
                //22 glow frame 3
                new float[] {0.85F,0.4F,0.85F,waver_alpha},
                //23 shadow
                new float[] {0.1F,0.1F,0.1F,flat_alpha},
                //24 mouth
                new float[] {0.4F,-0.05F,0.2F,1F},
                //25 water
                new float[] {0.4F,0.6F,0.9F,borderless_alpha},
                //26 fuzz contrast
                new float[] {0.5F,0.0F,-0.05F,fuzz_alpha},
                //27 fuzz
                new float[] {0.55F,0.05F,-0.05F,fuzz_alpha},
                //28 faded deep
                new float[] {0.54F,0.54F,0.54F,fade_alpha},
                //29 faded dark
                new float[] {0.6F,0.6F,0.6F,fade_alpha},
                //30 faded light            
                new float[] {0.66F,0.66F,0.66F,fade_alpha},
                //31 faded lightest
                new float[] {0.72F,0.72F,0.72F,fade_alpha},
                //32 purple blood
                new float[] {0.35F,0.1F,0.4F,1F},
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
                new float[] {0.85F,0.5F,0.75F,1F},
                //39 dripping fluid frame 1
                new float[] {0.85F,0.8F,0.75F,0F},
                //40 dripping fluid frame 2
                new float[] {0.55F,0.8F,0.35F,0F},
                //41 dripping fluid frame 3
                new float[] {0.55F,0.8F,0.35F,0F},
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
                new float[] {0.4F,0.15F,0.05F,fuzz_alpha},
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
                new float[] {0.97F,0.85F,0.36F,fuzz_alpha},
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
                new float[] {0.9F,0.37F,-0.03F,fuzz_alpha},
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
                new float[] {0.05F,0.0F,-0.05F,fuzz_alpha},
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
                new float[] {0.05F,0.03F,0.03F,fuzz_alpha},
                //4 lips/ears
                new float[] {0.2F,0.1F,0.1F,1F},
                //5 skin
                new float[] {0.38F,0.22F,0.1F,1F},
                //6 eyes shine
                new float[] {1.4F,1.4F,1.4F,1F},
                //7 eyes
                new float[] {0.15F,0.05F,-0.1F,1F},
            },

            new float[][] { //4 olive-dark skin
                            //3 hair
                new float[] {-0.05F,-0.05F,-0.05F,fuzz_alpha},
                //4 lips/ears
                new float[] {0.51F,0.3F,0.1F,1F},
                //5 skin
                new float[] {0.66F,0.44F,0.22F,1F},
                //6 eyes shine
                new float[] {1.4F,1.4F,1.4F,1F},
                //7 eyes
                new float[] {0.15F,0.1F,-0.03F,1F},
            },

            new float[][] { //5 red-brown skin
                            //3 hair
                new float[] {0.03F,0.0F,-0.05F,fuzz_alpha},
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
        public static float[][][][] kdungeon = new float[][][][] { dungeon_palettes, dungeon_palettes.Replicate() };
        public static float[][][][] kmecha = new float[][][][] { mecha_palettes, mecha_palettes.Replicate() };
        public static float[][][][] kmythos = new float[][][][] { mythos_palettes, mythos_palettes.Replicate() };


        public static void Initialize()
        {
            VoxelLogic.kcolorcount = kdungeon[0][0].Length;
            VoxelLogic.clear = (byte)(253 - (VoxelLogic.kcolorcount - 1) * 4);
            for (int s = 0; s < kdungeon.Length; s++)
            {
                for (int p = 0; p < kdungeon[s].Length; p++)
                {
                    float mod = s * 0.18f;
                    for (int c = 0; c < kdungeon[s][p].Length; c++)
                    {
                        if (c >= 3 && c <= 7)
                            continue;
                        if (c >= 12 && c <= 23)
                            continue;
                        kdungeon[s][p][c][0] -= mod;
                        kdungeon[s][p][c][1] -= mod;
                        kdungeon[s][p][c][2] -= mod;
                    }
                }
            }
            for (int s = 0; s < kmecha.Length; s++)
            {
                for (int p = 0; p < kmecha[s].Length; p++)
                {
                    float mod = s * 0.18f;
                    for (int c = 0; c < kmecha[s][p].Length; c++)
                    {
                        if (c >= 3 && c <= 7)
                            continue;
                        if (c >= 12 && c <= 23)
                            continue;
                        kmecha[s][p][c][0] -= mod;
                        kmecha[s][p][c][1] -= mod;
                        kmecha[s][p][c][2] -= mod;
                    }
                }

            }
            for (int s = 0; s < kmythos.Length; s++)
            {
                for (int p = 0; p < kmythos[s].Length; p++)
                {
                    float mod = s * 0.18f;
                    for (int c = 0; c < kmythos[s][p].Length; c++)
                    {
                        if (c >= 3 && c <= 7)
                            continue;
                        if (c >= 12 && c <= 23)
                            continue;
                        kmythos[s][p][c][0] -= mod;
                        kmythos[s][p][c][1] -= mod;
                        kmythos[s][p][c][2] -= mod;
                    }
                }

            }
        }

    }
}
