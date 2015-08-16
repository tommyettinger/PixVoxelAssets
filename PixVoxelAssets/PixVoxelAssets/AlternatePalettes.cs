using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsPV
{
    class AlternatePalettes
    {
        public const float flat_alpha = VoxelLogic.flat_alpha;
        public const float fuzz_alpha = VoxelLogic.fuzz_alpha;
        public const float waver_alpha = VoxelLogic.waver_alpha;
        public const float bordered_alpha = VoxelLogic.bordered_alpha;
        public const float borderless_alpha = VoxelLogic.borderless_alpha;
        public const float eraser_alpha = VoxelLogic.eraser_alpha;
        public const float spin_alpha_0 = VoxelLogic.spin_alpha_0;
        public const float spin_alpha_1 = VoxelLogic.spin_alpha_1;
        public const float flash_alpha = VoxelLogic.flash_alpha;
        public const float flash_alpha_0 = VoxelLogic.flash_alpha_0;
        public const float flash_alpha_1 = VoxelLogic.flash_alpha_1;
        public const float gloss_alpha = VoxelLogic.gloss_alpha;
        public const float grain_hard_alpha = VoxelLogic.grain_hard_alpha;
        public const float grain_some_alpha = VoxelLogic.grain_some_alpha;
        public const float grain_mild_alpha = VoxelLogic.grain_mild_alpha;

        public static float[][][] scheme0 = new float[][][]
            {
            new float[][] { //0 brown hair human
            //0 shoes, boots, brown leather contrast
            new float[] {0.35F,0.15F,0.0F,1F},
            //1 shoes, boots, brown leather
            new float[] {0.45F,0.25F,0.1F,1F},
            //2 pants, jeans contrast
            new float[] {0.25F,0.35F,0.55F,1F},
            //3 pants, jeans
            new float[] {0.5F,0.65F,0.95F,1F},
            //4 shirt contrast
            new float[] {0.15F,0.45F,0.1F,1F},
            //5 shirt
            new float[] {0.3F,0.55F,0.3F,1F},
            //6 hair contrast
            new float[] {0.3F,0.05F,0.0F,1F},
            //7 hair
            new float[] {0.4F,0.15F,0.05F,1F},
            //8 skin contrast
            new float[] {0.8F,0.5F,0.12F,1F},
            //9 skin
            new float[] {0.93F,0.74F,0.39F,1F},
            //10 eyes shine
            new float[] {1.4F,1.4F,1.4F,1F},
            //11 eyes
            new float[] {0.15F,0.1F,0.0F,1F},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.15F,0.45F,0.1F,waver_alpha},
            //15 flowing clothes
            new float[] {0.3F,0.55F,0.3F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.5F,0.0F,-0.05F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.55F,0.05F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.6F,0.1F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.65F,0.15F,0.05F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.7F,0.2F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.8F,0.25F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            new float[][] { //1 blonde hair human
            //0 shoes, boots, brown leather contrast
            new float[] {0.3F,0.12F,0.0F,1F},
            //1 shoes, boots, brown leather
            new float[] {0.45F,0.25F,0.1F,1F},
            //2 pants, jeans contrast
            new float[] {0.77F,0.68F,0.45F,1F},
            //3 pants, jeans
            new float[] {0.9F,0.83F,0.65F,1F},
            //4 shirt contrast
            new float[] {0.35F,0.45F,0.6F,1F},
            //5 shirt
            new float[] {0.5F,0.6F,0.7F,1F},
            //6 hair contrast
            new float[] {0.65F,0.55F,0.25F,1F},
            //7 hair
            new float[] {0.82F,0.72F,0.35F,1F},
            //8 skin contrast
            new float[] {0.77F,0.58F,0.21F,1F},
            //9 skin
            new float[] {0.9F,0.77F,0.44F,1F},
            //10 eyes shine
            new float[] {1.4F,1.4F,1.4F,1F},
            //11 eyes
            new float[] {0.0F,0.15F,0.25F,1F},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.35F,0.45F,0.6F,waver_alpha},
            //15 flowing clothes
            new float[] {0.5F,0.6F,0.7F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.5F,0.0F,-0.05F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.55F,0.05F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.6F,0.1F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.65F,0.15F,0.05F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.7F,0.2F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.8F,0.25F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            new float[][] { //2 gold-skinned human
            //0 shoes, boots, tennis shoes contrast
            new float[] {0.5F,0.5F,0.5F,1F},
            //1 shoes, boots, tennis shoes
            new float[] {0.8F,0.8F,0.8F,1F},
            //2 pants, jeans contrast
            new float[] {0.5F,0.55F,0.5F,1F},
            //3 pants, jeans
            new float[] {0.65F,0.7F,0.65F,1F},
            //4 shirt contrast
            new float[] {0.5F,-0.03F,0.0F,1F},
            //5 shirt
            new float[] {0.8F,0.0F,0.0F,1F},
            //6 hair contrast
            new float[] {0.0F,-0.03F,-0.09F,1F},
            //7 hair
            new float[] {0.05F,0.0F,-0.05F,1F},
            //8 skin contrast
            new float[] {0.72F,0.53F,0.05F,1F},
            //9 skin
            new float[] {0.94F,0.79F,0.37F,1F},
            //10 eyes shine
            new float[] {1.4F,1.4F,1.4F,1F},
            //11 eyes
            new float[] {0.1F,0.1F,0.05F,1F},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.5F,-0.05F,0.0F,waver_alpha},
            //15 flowing clothes
            new float[] {0.8F,-0.05F,0.0F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.5F,0.0F,-0.05F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.55F,0.05F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.6F,0.1F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.65F,0.15F,0.05F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.7F,0.2F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.8F,0.25F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            new float[][] { //3 dark-skinned human
            //0 shoes, boots contrast
            new float[] {0.0F,-0.05F,-0.1F,1F},
            //1 shoes, boots
            new float[] {0.2F,0.15F,0.1F,1F},
            //2 pants, jeans contrast
            new float[] {0.35F,0.35F,0.35F,1F},
            //3 pants, jeans
            new float[] {0.5F,0.5F,0.5F,1F},
            //4 shirt contrast
            new float[] {0.8F,0.8F,0.8F,1F},
            //5 shirt
            new float[] {0.95F,0.95F,0.95F,1F},
            //6 hair contrast
            new float[] {0.2F,0.15F,0.1F,1F},
            //7 hair
            new float[] {0.05F,0.0F,-0.05F,1F},
            //8 skin contrast
            new float[] {0.2F,0.1F,0.1F,1F},
            //9 skin
            new float[] {0.38F,0.22F,0.1F,1F},
            //10 eyes shine
            new float[] {1.4F,1.4F,1.4F,1F},
            //11 eyes
            new float[] {0.15F,0.05F,-0.1F,1F},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.9F,0.9F,0.9F,waver_alpha},
            //15 flowing clothes
            new float[] {1.1F,1.1F,1.1F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.5F,0.0F,-0.05F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.55F,0.05F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.6F,0.1F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.65F,0.15F,0.05F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.7F,0.2F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.8F,0.25F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            new float[][] { //4 brown-skinned human
            //0 shoes, boots contrast
            new float[] {0.2F,0.1F,-0.1F,1F},
            //1 shoes, boots
            new float[] {0.35F,0.2F,0.0F,1F},
            //2 pants, jeans contrast
            new float[] {0.0F,0.1F,0.35F,1F},
            //3 pants, jeans
            new float[] {0.1F,0.25F,0.55F,1F},
            //4 shirt contrast
            new float[] {0.0F,0.55F,0.1F,1F},
            //5 shirt
            new float[] {0.15F,0.7F,0.25F,1F},
            //6 hair contrast
            new float[] {0.12F,0.05F,0.0F,1F},
            //7 hair
            new float[] {0.05F,0.0F,-0.05F,1F},
            //8 skin contrast
            new float[] {0.4F,0.15F,0.0F,1F},
            //9 skin
            new float[] {0.65F,0.3F,0.15F,1F},
            //10 eyes shine
            new float[] {1.4F,1.4F,1.4F,1F},
            //11 eyes
            new float[] {0.15F,0.05F,-0.1F,1F},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.2F,0.6F,0.3F,waver_alpha},
            //15 flowing clothes
            new float[] {0.35F,0.75F,0.45F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.5F,0.0F,-0.05F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.55F,0.05F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.6F,0.1F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.65F,0.15F,0.05F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.7F,0.2F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.8F,0.25F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            new float[][] { //5 wolf
            //0 paws, nose contrast
            new float[] {0.05F,0.0F,-0.05F,1F},
            //1 claws
            new float[] {0.8F,0.7F,0.45F,1F},
            //2 pants, jeans contrast
            new float[] {0.0F,0.1F,0.35F,1F},
            //3 pants, jeans
            new float[] {0.1F,0.25F,0.55F,1F},
            //4 shirt contrast
            new float[] {0.0F,0.55F,0.1F,1F},
            //5 shirt
            new float[] {0.15F,0.7F,0.25F,1F},
            //6 hair contrast
            new float[] {0.12F,0.05F,0.0F,1F},
            //7 hair
            new float[] {0.05F,0.0F,-0.05F,1F},
            //8 mouth contrast
            new float[] {0.4F,0.15F,0.0F,1F},
            //9 pink skin
            new float[] {0.85F,0.5F,0.6F,1F},
            //10 eyes shine
            new float[] {1.4F,1.4F,1.4F,1F},
            //11 eyes
            new float[] {0.0F,0.1F,0.15F,1F},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.2F,0.6F,0.3F,waver_alpha},
            //15 flowing clothes
            new float[] {0.35F,0.75F,0.45F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.2F,0.2F,0.2F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.3F,0.3F,0.3F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.4F,0.4F,0.4F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.5F,0.5F,0.5F,fuzz_alpha},
            //32 fuzz light            
            new float[] {0.6F,0.6F,0.6F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.7F,0.7F,0.7F,fuzz_alpha},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            new float[][] { //6 dragon
            //0 paws, nose contrast
            new float[] {0.15F,0.03F,-0.05F,1F},
            //1 claws, teeth
            new float[] {0.8F,0.8F,0.6F,1F},
            //2 alternate scales contrast
            new float[] {0.0F,0.1F,0.35F,1F},
            //3 alternate scales
            new float[] {0.1F,0.25F,0.55F,1F},
            //4 scales contrast
            new float[] {0.1F,0.4F,0.0F,1F},
            //5 scales
            new float[] {0.25F,0.55F,0.15F,1F},
            //6 hair contrast
            new float[] {0.12F,0.05F,0.0F,1F},
            //7 hair
            new float[] {0.05F,0.0F,-0.05F,1F},
            //8 mouth contrast
            new float[] {0.4F,0.15F,0.0F,1F},
            //9 pink skin
            new float[] {0.85F,0.5F,0.6F,1F},
            //10 eyes shine
            new float[] {1.4F,1.1F,1.0F,1F},
            //11 eyes
            new float[] {0.25F,0.02F,-0.05F,1F},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.2F,0.6F,0.3F,waver_alpha},
            //15 flowing clothes
            new float[] {0.35F,0.75F,0.45F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.4F,0.0F,-0.09F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.5F,0.1F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.6F,0.2F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.7F,0.3F,0.05F,fuzz_alpha},
            //32 fuzz light            
            new float[] {0.8F,0.4F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.9F,0.5F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            new float[][] { //7 beetle
            //0 claws contrast
            new float[] {0.02F,-0.05F,0.02F,1F},
            //1 claws, teeth, seams
            new float[] {0.12F,0.07F,0.12F,1F},
            //2 body contrast
            new float[] {0.25F,0.0F,0.3F,1F},
            //3 body
            new float[] {0.4F,0.1F,0.45F,1F},
            //4 head contrast
            new float[] {0.2F,0.1F,0.2F,1F},
            //5 head
            new float[] {0.35F,0.25F,0.35F,1F},
            //6 antlers contrast
            new float[] {0.0F,-0.05F,0.0F,1F},
            //7 antlers, dots
            new float[] {0.15F,0.1F,0.15F,1F},
            //8 mouth contrast
            new float[] {0.4F,0.15F,0.0F,1F},
            //9 pink skin
            new float[] {0.85F,0.5F,0.6F,1F},
            //10 eyes shine
            new float[] {1.1F,1.3F,1.1F,1F},
            //11 eyes
            new float[] {0.0F,0.05F,-0.05F,gloss_alpha},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.2F,0.6F,0.3F,waver_alpha},
            //15 flowing clothes
            new float[] {0.35F,0.75F,0.45F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.4F,0.0F,-0.09F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.5F,0.1F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.6F,0.2F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.7F,0.3F,0.05F,fuzz_alpha},
            //32 fuzz light            
            new float[] {0.8F,0.4F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.9F,0.5F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            new float[][] { //8 hawk
            //0 claws contrast
            new float[] {0.75F,0.35F,0.0F,1F},
            //1 claws, beak
            new float[] {0.9F,0.6F,0.1F,1F},
            //2 markings contrast
            new float[] {0.15F,0.45F,0.0F,1F},
            //3 markings
            new float[] {0.65F,0.15F,0.05F,1F},
            //4 body contrast
            new float[] {0.55F,0.4F,0.15F,1F},
            //5 body
            new float[] {0.7F,0.58F,0.3F,1F},
            //6 hair contrast
            new float[] {0.2F,0.4F,0.1F,1F},
            //7 hair
            new float[] {0.35F,0.55F,0.25F,1F},
            //8 mouth contrast
            new float[] {0.4F,0.15F,0.0F,1F},
            //9 pink skin
            new float[] {0.85F,0.5F,0.6F,1F},
            //10 eyes shine
            new float[] {1.4F,1.15F,0.65F,1F},
            //11 eyes
            new float[] {0.15F,0.1F,-0.05F,1F},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.2F,0.6F,0.3F,waver_alpha},
            //15 flowing clothes
            new float[] {0.35F,0.75F,0.45F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.4F,0.0F,-0.09F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.5F,0.1F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.6F,0.2F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.7F,0.3F,0.05F,fuzz_alpha},
            //32 fuzz light            
            new float[] {0.8F,0.4F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.9F,0.5F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            new float[][] { //9 crab
            //0 legs contrast
            new float[] {0.85F,0.7F,0.3F,1F},
            //1 legs
            new float[] {1.0F,0.85F,0.45F,1F},
            //2 body contrast
            new float[] {0.75F,0.55F,0.05F,1F},
            //3 body
            new float[] {0.9F,0.7F,0.2F,1F},
            //4 pincers contrast
            new float[] {0.85F,0.5F,0.05F,1F},
            //5 pincers
            new float[] {1.0F,0.65F,0.15F,1F},
            //6 markings contrast
            new float[] {0.65F,0.5F,0.2F,1F},
            //7 markings
            new float[] {0.7F,0.05F,0.05F,1F},
            //8 mouth
            new float[] {0.4F,0.2F,0.05F,1F},
            //9 pink skin
            new float[] {0.85F,0.5F,0.6F,1F},
            //10 eyes shine
            new float[] {1.3F,1.1F,1.1F,1F},
            //11 eyes
            new float[] {0.95F,0.45F,0.15F,1F},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.2F,0.6F,0.3F,waver_alpha},
            //15 flowing clothes
            new float[] {0.35F,0.75F,0.45F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.4F,0.0F,-0.09F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.5F,0.1F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.6F,0.2F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.7F,0.3F,0.05F,fuzz_alpha},
            //32 fuzz light            
            new float[] {0.8F,0.4F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.9F,0.5F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            
            new float[][] { //10 goblin
            //0 shoes contrast
            new float[] {0.35F,0.0F,-0.05F,1F},
            //1 shoes
            new float[] {0.5F,0.1F,0.05F,1F},
            //2 pants, jeans contrast
            new float[] {0.3F,0.17F,0.05F,1F},
            //3 pants, jeans
            new float[] {0.45F,0.3F,0.15F,1F},
            //4 shirt contrast
            new float[] {0.45F,0.4F,0.1F,1F},
            //5 shirt
            new float[] {0.55F,0.5F,0.2F,1F},
            //6 hair contrast
            new float[] {0.83F,0.7F,0.35F,1F},
            //7 hair
            new float[] {0.0F,0.03F,-0.05F,1F},
            //8 mouth contrast
            new float[] {0.4F,0.15F,0.0F,1F},
            //9 flesh
            new float[] {0.2F,0.55F,0.1F,1F},
            //10 eyes shine
            new float[] {1.1F,1.4F,0.9F,1F},
            //11 eyes
            new float[] {0.12F,0.05F,0.0F,1F},
            //12 metal contrast
            new float[] {0.7F,0.85F,0.95F,1F},
            //13 metal
            new float[] {0.5F,0.55F,0.65F,1F},
            //14 flowing clothes contrast
            new float[] {0.2F,0.6F,0.3F,waver_alpha},
            //15 flowing clothes
            new float[] {0.35F,0.75F,0.45F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.2F,0.2F,0.2F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.3F,0.3F,0.3F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.4F,0.4F,0.4F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.5F,0.5F,0.5F,fuzz_alpha},
            //32 fuzz light            
            new float[] {0.6F,0.6F,0.6F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.7F,0.7F,0.7F,fuzz_alpha},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            new float[][] { //11 ant
            //0 claws contrast
            new float[] {0.02F,-0.02F,-0.08F,1F},
            //1 claws, teeth
            new float[] {0.1F,0.06F,0.0F,1F},
            //2 body contrast
            new float[] {0.2F,0.17F,0.13F,1F},
            //3 body
            new float[] {0.32F,0.29F,0.25F,1F},
            //4 stinger contrast
            new float[] {0.5F,0.65F,0.3F,1F},
            //5 stinger
            new float[] {0.65F,0.8F,0.45F,1F},
            //6 jaws contrast
            new float[] {0.15F,0.11F,0.05F,1F},
            //7 jaws
            new float[] {0.25F,0.21F,0.15F,1F},
            //8 mouth contrast
            new float[] {0.4F,0.15F,0.0F,1F},
            //9 pink skin
            new float[] {0.85F,0.5F,0.6F,1F},
            //10 eyes shine
            new float[] {1.15F,1.0F,0.85F,1F},
            //11 eyes
            new float[] {0.15F,0.1F,0.02F,gloss_alpha},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.2F,0.6F,0.3F,waver_alpha},
            //15 flowing clothes
            new float[] {0.35F,0.75F,0.45F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.4F,0.0F,-0.09F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.5F,0.1F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.6F,0.2F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.7F,0.3F,0.05F,fuzz_alpha},
            //32 fuzz light            
            new float[] {0.8F,0.4F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.9F,0.5F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.08F,0.04F,-0.1F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            new float[][] { //12 bee
            //0 legs contrast
            new float[] {0.55F,0.55F,0.1F,1F},
            //1 legs, teeth, stinger
            new float[] {0.7F,0.7F,0.25F,1F},
            //2 stripes contrast
            new float[] {0.85F,0.85F,0.15F,1F},
            //3 stripes
            new float[] {1.0F,1.0F,0.3F,1F},
            //4 body contrast
            new float[] {0.0F,0.0F,-0.05F,1F},
            //5 body
            new float[] {0.1F,0.1F,0.05F,1F},
            //6 antennae contrast
            new float[] {0.6F,0.6F,0.3F,1F},
            //7 antennae
            new float[] {0.75F,0.75F,0.45F,1F},
            //8 mouth contrast
            new float[] {0.4F,0.15F,0.0F,1F},
            //9 pink skin
            new float[] {0.85F,0.5F,0.6F,1F},
            //10 eyes shine
            new float[] {1.4F,0.85F,0.8F,1F},
            //11 eyes
            new float[] {-0.05F,-0.05F,-0.05F,gloss_alpha},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.2F,0.6F,0.3F,waver_alpha},
            //15 flowing clothes
            new float[] {0.35F,0.75F,0.45F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.4F,0.0F,-0.09F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.5F,0.1F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.6F,0.2F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.7F,0.3F,0.05F,fuzz_alpha},
            //32 fuzz light            
            new float[] {0.8F,0.4F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.9F,0.5F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.0F,0.1F,-0.1F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 flapping wings contrast, even frames 
            new float[] {0.8F,0.8F,0.6F,spin_alpha_0},
            //37 flapping wings, even frames
            new float[] {0.6F,0.6F,0.45F,spin_alpha_0},
            //38 flapping wings contrast, odd frames
            new float[] {0.8F,0.8F,0.6F,spin_alpha_1},
            //39 flapping wings, odd frames
            new float[] {0.6F,0.6F,0.45F,spin_alpha_1},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            new float[][] { //13 eye tyrant
            //0 teeth contrast
            new float[] {0.75F,0.7F,0.4F,1F},
            //1 teeth
            new float[] {0.95F,0.9F,0.7F,1F},
            //2 eyes on stalks shine
            new float[] {1.3F,1.0F,1.0F,waver_alpha},
            //3 eyes on stalks
            new float[] {0.13F,0.05F,-0.05F,waver_alpha},
            //4 body contrast
            new float[] {0.35F,0.1F,0.15F,1F},
            //5 body
            new float[] {0.5F,0.22F,0.3F,1F},
            //6 hair contrast
            new float[] {0.65F,0.5F,0.2F,1F},
            //7 hair
            new float[] {0.7F,0.05F,0.05F,1F},
            //8 mouth
            new float[] {0.6F,0.25F,0.1F,1F},
            //9 eye white
            new float[] {0.9F,0.8F,0.77F,1F},
            //10 eyes shine
            new float[] {1.3F,0.9F,1.0F,1F},
            //11 eyes
            new float[] {0.13F,0.05F,-0.05F,1F},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 eyestalks contrast
            new float[] {0.35F,0.1F,0.15F,waver_alpha},
            //15 eyestalks
            new float[] {0.5F,0.22F,0.3F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.4F,0.0F,-0.09F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.5F,0.1F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.6F,0.2F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.7F,0.3F,0.05F,fuzz_alpha},
            //32 fuzz light            
            new float[] {0.8F,0.4F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.9F,0.5F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.62F,-0.05F,0.1F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            new float[][] { //14 centipede
            //0 legs
            new float[] {0.31F,0.29F,0.33F,1F},
            //1 antennae, teeth
            new float[] {0.23F,0.2F,0.25F,1F},
            //2 body contrast
            new float[] {0.02F,0.0F,0.05F,1F},
            //3 body
            new float[] {0.12F,0.1F,0.15F,1F},
            //4 stinger contrast
            new float[] {0.55F,0.2F,0.6F,1F},
            //5 stinger
            new float[] {0.7F,0.35F,0.75F,1F},
            //6 jaws contrast
            new float[] {0.0F,-0.02F,0.02F,1F},
            //7 jaws
            new float[] {0.12F,0.1F,0.15F,1F},
            //8 mouth contrast
            new float[] {0.4F,0.15F,0.0F,1F},
            //9 pink skin
            new float[] {0.85F,0.5F,0.6F,1F},
            //10 eyes shine
            new float[] {1.4F,0.85F,0.8F,1F},
            //11 eyes
            new float[] {0.05F,0.03F,0.08F,gloss_alpha},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.2F,0.6F,0.3F,waver_alpha},
            //15 flowing clothes
            new float[] {0.35F,0.75F,0.45F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.4F,0.0F,-0.09F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.5F,0.1F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.6F,0.2F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.7F,0.3F,0.05F,fuzz_alpha},
            //32 fuzz light            
            new float[] {0.8F,0.4F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.9F,0.5F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.07F,0.05F,0.08F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 flapping wings contrast, even frames 
            new float[] {0.7F,0.2F,0.2F,spin_alpha_0},
            //37 flapping wings, even frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_0},
            //38 flapping wings contrast, odd frames
            new float[] {0.7F,0.2F,0.2F,spin_alpha_1},
            //39 flapping wings, odd frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_1},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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

            new float[][] { //15 sandworm
            //0 legs
            new float[] {0.31F,0.29F,0.33F,1F},
            //1 teeth
            new float[] {0.7F,0.65F,0.45F,1F},
            //2 body contrast
            new float[] {0.25F,0.1F,0.35F,1F},
            //3 body
            new float[] {0.4F,0.2F,0.5F,1F},
            //4 stinger contrast
            new float[] {0.65F,0.2F,0.55F,1F},
            //5 stinger
            new float[] {0.8F,0.35F,0.7F,1F},
            //6 jaws contrast
            new float[] {0.0F,-0.02F,0.02F,1F},
            //7 jaws
            new float[] {0.12F,0.1F,0.15F,1F},
            //8 mouth
            new float[] {0.35F,0.12F,0.0F,1F},
            //9 pink skin
            new float[] {0.85F,0.5F,0.6F,1F},
            //10 eyes shine
            new float[] {1.4F,0.85F,0.8F,1F},
            //11 eyes
            new float[] {0.05F,0.03F,0.08F,gloss_alpha},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.2F,0.6F,0.3F,waver_alpha},
            //15 flowing clothes
            new float[] {0.35F,0.75F,0.45F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.4F,0.0F,-0.09F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.5F,0.1F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.6F,0.2F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.7F,0.3F,0.05F,fuzz_alpha},
            //32 fuzz light            
            new float[] {0.8F,0.4F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.9F,0.5F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.35F,0.03F,0.25F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 flapping wings contrast, even frames 
            new float[] {0.7F,0.2F,0.2F,spin_alpha_0},
            //37 flapping wings, even frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_0},
            //38 flapping wings contrast, odd frames
            new float[] {0.7F,0.2F,0.2F,spin_alpha_1},
            //39 flapping wings, odd frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_1},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            };





        public static float[][][] scheme1 = new float[][][]
            {
            new float[][] { //0 brown hair human
            //0 shoes, boots, brown leather contrast
            new float[] {0.2F,0.08F,-0.02F,1F},
            //1 shoes, boots, brown leather
            new float[] {0.35F,0.2F,0.08F,1F},
            //2 pants, jeans contrast
            new float[] {0.05F,0.05F,0.05F,1F},
            //3 pants, jeans
            new float[] {0.2F,0.2F,0.2F,1F},
            //4 shirt contrast
            new float[] {0.4F,0.0F,0.05F,1F},
            //5 shirt
            new float[] {0.55F,0.1F,0.2F,1F},
            //6 hair contrast
            new float[] {0.3F,0.05F,0.0F,1F},
            //7 hair
            new float[] {0.4F,0.15F,0.05F,1F},
            //8 skin contrast
            new float[] {0.8F,0.5F,0.12F,1F},
            //9 skin
            new float[] {0.93F,0.74F,0.39F,1F},
            //10 eyes shine
            new float[] {1.4F,1.4F,1.4F,1F},
            //11 eyes
            new float[] {0.15F,0.1F,0.0F,1F},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.15F,0.45F,0.1F,waver_alpha},
            //15 flowing clothes
            new float[] {0.3F,0.55F,0.3F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.5F,0.0F,-0.05F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.55F,0.05F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.6F,0.1F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.65F,0.15F,0.05F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.7F,0.2F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.8F,0.25F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            new float[][] { //1 blonde hair human
            //0 shoes, boots, brown leather contrast
            new float[] {0.0F,0.0F,0.0F,1F},
            //1 shoes, boots, brown leather
            new float[] {0.1F,0.1F,0.1F,1F},
            //2 pants, jeans contrast
            new float[] {0.25F,0.25F,0.25F,1F},
            //3 pants, jeans
            new float[] {0.4F,0.4F,0.4F,1F},
            //4 shirt contrast
            new float[] {0.55F,0.55F,0.55F,1F},
            //5 shirt
            new float[] {0.7F,0.7F,0.7F,1F},
            //6 hair contrast
            new float[] {0.65F,0.55F,0.25F,1F},
            //7 hair
            new float[] {0.82F,0.72F,0.35F,1F},
            //8 skin contrast
            new float[] {0.77F,0.58F,0.21F,1F},
            //9 skin
            new float[] {0.9F,0.77F,0.44F,1F},
            //10 eyes shine
            new float[] {1.4F,1.4F,1.4F,1F},
            //11 eyes
            new float[] {0.0F,0.15F,0.25F,1F},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.35F,0.45F,0.6F,waver_alpha},
            //15 flowing clothes
            new float[] {0.5F,0.6F,0.7F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.5F,0.0F,-0.05F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.55F,0.05F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.6F,0.1F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.65F,0.15F,0.05F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.7F,0.2F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.8F,0.25F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            new float[][] { //2 gold-skinned human
            //0 shoes, boots, brown leather contrast
            new float[] {0.0F,0.0F,0.0F,1F},
            //1 shoes, boots, brown leather
            new float[] {0.1F,0.1F,0.1F,1F},
            //2 pants, jeans contrast
            new float[] {0.1F,0.1F,0.1F,1F},
            //2 pants, jeans
            new float[] {0.2F,0.2F,0.2F,1F},
            //4 shirt contrast
            new float[] {0.2F,0.2F,0.2F,1F},
            //5 shirt
            new float[] {0.3F,0.3F,0.3F,1F},
            //6 hair contrast
            new float[] {0.0F,-0.03F,-0.09F,1F},
            //7 hair
            new float[] {0.05F,0.0F,-0.05F,1F},
            //8 skin contrast
            new float[] {0.72F,0.53F,0.05F,1F},
            //9 skin
            new float[] {0.94F,0.79F,0.37F,1F},
            //10 eyes shine
            new float[] {1.4F,1.4F,1.4F,1F},
            //11 eyes
            new float[] {0.1F,0.1F,0.05F,1F},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.5F,-0.05F,0.0F,waver_alpha},
            //15 flowing clothes
            new float[] {0.8F,-0.05F,0.0F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.5F,0.0F,-0.05F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.55F,0.05F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.6F,0.1F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.65F,0.15F,0.05F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.7F,0.2F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.8F,0.25F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            new float[][] { //3 dark-skinned human
            //0 shoes, boots contrast
            new float[] {0.0F,-0.05F,-0.1F,1F},
            //1 shoes, boots
            new float[] {0.2F,0.15F,0.1F,1F},
            //2 pants, jeans contrast
            new float[] {0.67F,0.55F,0.35F,1F},
            //3 pants, jeans
            new float[] {0.8F,0.7F,0.55F,1F},
            //4 shirt contrast
            new float[] {0.2F,0.2F,0.45F,1F},
            //5 shirt
            new float[] {0.35F,0.35F,0.55F,1F},
            //6 hair contrast
            new float[] {0.2F,0.15F,0.1F,1F},
            //7 hair
            new float[] {0.05F,0.0F,-0.05F,1F},
            //8 skin contrast
            new float[] {0.2F,0.1F,0.1F,1F},
            //9 skin
            new float[] {0.38F,0.22F,0.1F,1F},
            //10 eyes shine
            new float[] {1.4F,1.4F,1.4F,1F},
            //11 eyes
            new float[] {0.15F,0.05F,-0.1F,1F},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.9F,0.9F,0.9F,waver_alpha},
            //15 flowing clothes
            new float[] {1.1F,1.1F,1.1F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.5F,0.0F,-0.05F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.55F,0.05F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.6F,0.1F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.65F,0.15F,0.05F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.7F,0.2F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.8F,0.25F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            new float[][] { //4 brown-skinned human
            //0 shoes, boots contrast
            new float[] {0.0F,0.0F,0.0F,1F},
            //1 shoes, boots
            new float[] {0.15F,0.15F,0.15F,1F},
            //2 pants, jeans contrast
            new float[] {0.0F,0.05F,0.2F,1F},
            //3 pants, jeans
            new float[] {0.1F,0.2F,0.35F,1F},
            //4 shirt contrast
            new float[] {0.4F,0.4F,0.4F,1F},
            //5 shirt
            new float[] {0.55F,0.55F,0.55F,1F},
            //6 hair contrast
            new float[] {0.12F,0.05F,0.0F,1F},
            //7 hair
            new float[] {0.05F,0.0F,-0.05F,1F},
            //8 skin contrast
            new float[] {0.4F,0.15F,0.0F,1F},
            //9 skin
            new float[] {0.65F,0.3F,0.15F,1F},
            //10 eyes shine
            new float[] {1.4F,1.4F,1.4F,1F},
            //11 eyes
            new float[] {0.15F,0.05F,-0.1F,1F},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.2F,0.6F,0.3F,waver_alpha},
            //15 flowing clothes
            new float[] {0.35F,0.75F,0.45F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.5F,0.0F,-0.05F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.55F,0.05F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.6F,0.1F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.65F,0.15F,0.05F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.7F,0.2F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.8F,0.25F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            new float[][] { //5 wolf
            //0 paws, nose contrast
            new float[] {0.15F,0.03F,-0.05F,1F},
            //1 claws, teeth
            new float[] {0.8F,0.7F,0.45F,1F},
            //2 pants, jeans contrast
            new float[] {0.0F,0.1F,0.35F,1F},
            //3 pants, jeans
            new float[] {0.1F,0.25F,0.55F,1F},
            //4 shirt contrast
            new float[] {0.0F,0.55F,0.1F,1F},
            //5 shirt
            new float[] {0.15F,0.7F,0.25F,1F},
            //6 hair contrast
            new float[] {0.12F,0.05F,0.0F,1F},
            //7 hair
            new float[] {0.05F,0.0F,-0.05F,1F},
            //8 mouth contrast
            new float[] {0.4F,0.15F,0.0F,1F},
            //9 pink skin
            new float[] {0.85F,0.5F,0.6F,1F},
            //10 eyes shine
            new float[] {1.4F,1.4F,1.4F,1F},
            //11 eyes
            new float[] {0.0F,0.1F,0.15F,1F},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.2F,0.6F,0.3F,waver_alpha},
            //15 flowing clothes
            new float[] {0.35F,0.75F,0.45F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.4F,0.0F,-0.09F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.5F,0.1F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.6F,0.2F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.7F,0.3F,0.05F,fuzz_alpha},
            //32 fuzz light            
            new float[] {0.8F,0.4F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.9F,0.5F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            new float[][] { //6 dragon
            //0 paws, nose contrast
            new float[] {0.15F,0.03F,-0.05F,1F},
            //1 claws, teeth
            new float[] {0.8F,0.8F,0.6F,1F},
            //2 alternate scales contrast
            new float[] {0.0F,0.1F,0.35F,1F},
            //3 alternate scales
            new float[] {0.1F,0.25F,0.55F,1F},
            //4 scales contrast
            new float[] {0.45F,0.0F,0.0F,1F},
            //5 scales
            new float[] {0.6F,0.2F,0.15F,1F},
            //6 hair contrast
            new float[] {0.12F,0.05F,0.0F,1F},
            //7 hair
            new float[] {0.05F,0.0F,-0.05F,1F},
            //8 mouth contrast
            new float[] {0.4F,0.15F,0.0F,1F},
            //9 pink skin
            new float[] {0.85F,0.5F,0.6F,1F},
            //10 eyes shine
            new float[] {1.2F,0.2F,0.2F,1F},
            //11 eyes
            new float[] {0.0F,0.0F,0.0F,1F},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.2F,0.6F,0.3F,waver_alpha},
            //15 flowing clothes
            new float[] {0.35F,0.75F,0.45F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.4F,0.0F,-0.09F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.5F,0.1F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.6F,0.2F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.7F,0.3F,0.05F,fuzz_alpha},
            //32 fuzz light            
            new float[] {0.8F,0.4F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.9F,0.5F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            new float[][] { //7 beetle
            //0 claws contrast
            new float[] {0.05F,0.1F,0.0F,1F},
            //1 claws, teeth, seams
            new float[] {0.15F,0.25F,0.1F,1F},
            //2 body contrast
            new float[] {0.15F,0.45F,0.0F,1F},
            //3 body
            new float[] {0.3F,0.65F,0.1F,1F},
            //4 head contrast
            new float[] {0.25F,0.4F,0.1F,1F},
            //5 head
            new float[] {0.4F,0.55F,0.25F,1F},
            //6 antlers contrast, dots
            new float[] {0.2F,0.4F,0.1F,1F},
            //7 antlers
            new float[] {0.35F,0.55F,0.25F,1F},
            //8 mouth contrast
            new float[] {0.4F,0.15F,0.0F,1F},
            //9 pink skin
            new float[] {0.85F,0.5F,0.6F,1F},
            //10 eyes shine
            new float[] {1.25F,1.2F,1.1F,1F},
            //11 eyes
            new float[] {0.05F,0.0F,-0.05F,gloss_alpha},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.2F,0.6F,0.3F,waver_alpha},
            //15 flowing clothes
            new float[] {0.35F,0.75F,0.45F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.4F,0.0F,-0.09F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.5F,0.1F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.6F,0.2F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.7F,0.3F,0.05F,fuzz_alpha},
            //32 fuzz light            
            new float[] {0.8F,0.4F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.9F,0.5F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            new float[][] { //8 hawk
            //0 claws contrast
            new float[] {0.65F,0.6F,0.25F,1F},
            //1 claws, beak
            new float[] {0.8F,0.75F,0.35F,1F},
            //2 markings contrast
            new float[] {0.15F,0.45F,0.0F,1F},
            //3 markings
            new float[] {0.5F,0.05F,0.05F,1F},
            //4 body contrast
            new float[] {0.55F,0.55F,0.55F,1F},
            //5 body
            new float[] {0.7F,0.7F,0.7F,1F},
            //6 hair contrast
            new float[] {0.2F,0.4F,0.1F,1F},
            //7 hair
            new float[] {0.35F,0.55F,0.25F,1F},
            //8 mouth contrast
            new float[] {0.4F,0.15F,0.0F,1F},
            //9 pink skin
            new float[] {0.85F,0.5F,0.6F,1F},
            //10 eyes shine
            new float[] {1.5F,1.0F,0.9F,1F},
            //11 eyes
            new float[] {0.15F,0.0F,-0.05F,1F},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.2F,0.6F,0.3F,waver_alpha},
            //15 flowing clothes
            new float[] {0.35F,0.75F,0.45F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.4F,0.0F,-0.09F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.5F,0.1F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.6F,0.2F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.7F,0.3F,0.05F,fuzz_alpha},
            //32 fuzz light            
            new float[] {0.8F,0.4F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.9F,0.5F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            new float[][] { //9 crab
            //0 legs contrast
            new float[] {0.8F,0.8F,0.8F,1F},
            //1 legs
            new float[] {0.95F,0.95F,0.95F,1F},
            //2 body contrast
            new float[] {0.75F,0.75F,0.75F,1F},
            //3 body
            new float[] {0.9F,0.9F,0.9F,1F},
            //4 pincers contrast
            new float[] {0.75F,0.75F,0.75F,1F},
            //5 pincers
            new float[] {0.8F,0.8F,0.8F,1F},
            //6 markings contrast
            new float[] {0.6F,0.6F,0.6F,1F},
            //7 markings
            new float[] {0.6F,0.5F,0.35F,1F},
            //8 mouth
            new float[] {0.2F,0.1F,0.05F,1F},
            //9 pink skin
            new float[] {0.85F,0.5F,0.6F,1F},
            //10 eyes shine
            new float[] {1.1F,1.1F,1.3F,1F},
            //11 eyes
            new float[] {0.45F,0.75F,0.95F,1F},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.2F,0.6F,0.3F,waver_alpha},
            //15 flowing clothes
            new float[] {0.35F,0.75F,0.45F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.4F,0.0F,-0.09F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.5F,0.1F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.6F,0.2F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.7F,0.3F,0.05F,fuzz_alpha},
            //32 fuzz light            
            new float[] {0.8F,0.4F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.9F,0.5F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            new float[][] { //10 goblin
            //0 shoes contrast
            new float[] {0.3F,0.3F,0.35F,1F},
            //1 shoes
            new float[] {0.45F,0.45F,0.5F,1F},
            //2 pants, jeans contrast
            new float[] {0.15F,0.15F,0.15F,1F},
            //3 pants, jeans
            new float[] {0.3F,0.3F,0.3F,1F},
            //4 shirt contrast
            new float[] {0.65F,0.65F,0.65F,1F},
            //5 shirt
            new float[] {0.8F,0.8F,0.8F,1F},
            //6 hair contrast
            new float[] {0.5F,1.05F,1.1F,1F},
            //7 hair
            new float[] {0.2F,0.22F,0.25F,1F},
            //8 mouth contrast
            new float[] {0.25F,0.13F,0.08F,1F},
            //9 flesh
            new float[] {0.08F,0.12F,0.5F,1F},
            //10 eyes shine
            new float[] {1.1F,1.1F,1.4F,1F},
            //11 eyes
            new float[] {0.0F,0.12F,0.05F,1F},
            //12 metal contrast
            new float[] {0.7F,0.85F,0.95F,1F},
            //13 metal
            new float[] {0.5F,0.55F,0.65F,1F},
            //14 flowing clothes contrast
            new float[] {0.2F,0.6F,0.3F,waver_alpha},
            //15 flowing clothes
            new float[] {0.35F,0.75F,0.45F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.2F,0.2F,0.2F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.3F,0.3F,0.3F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.4F,0.4F,0.4F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.5F,0.5F,0.5F,fuzz_alpha},
            //32 fuzz light            
            new float[] {0.6F,0.6F,0.6F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.7F,0.7F,0.7F,fuzz_alpha},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            new float[][] { //11 ant
            //0 claws contrast
            new float[] {0.55F,0.22F,0.02F,1F},
            //1 claws, teeth
            new float[] {0.65F,0.3F,0.1F,1F},
            //2 body contrast
            new float[] {0.66F,0.3F,0.08F,1F},
            //3 body
            new float[] {0.78F,0.4F,0.15F,1F},
            //4 stinger contrast
            new float[] {0.65F,0.3F,0.5F,1F},
            //5 stinger
            new float[] {0.8F,0.45F,0.65F,1F},
            //6 jaws contrast
            new float[] {0.66F,0.25F,0.15F,1F},
            //7 jaws
            new float[] {0.78F,0.35F,0.25F,1F},
            //8 mouth contrast
            new float[] {0.4F,0.15F,0.0F,1F},
            //9 pink skin
            new float[] {0.85F,0.5F,0.6F,1F},
            //10 eyes shine
            new float[] {1.4F,0.85F,0.8F,1F},
            //11 eyes
            new float[] {0.65F,0.6F,0.15F,gloss_alpha},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.2F,0.6F,0.3F,waver_alpha},
            //15 flowing clothes
            new float[] {0.35F,0.75F,0.45F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.4F,0.0F,-0.09F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.5F,0.1F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.6F,0.2F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.7F,0.3F,0.05F,fuzz_alpha},
            //32 fuzz light            
            new float[] {0.8F,0.4F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.9F,0.5F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.85F,0.2F,0.12F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 flapping wings contrast, even frames 
            new float[] {0.7F,0.2F,0.2F,spin_alpha_0},
            //37 flapping wings, even frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_0},
            //38 flapping wings contrast, odd frames
            new float[] {0.7F,0.2F,0.2F,spin_alpha_1},
            //39 flapping wings, odd frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_1},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            new float[][] { //12 bee
            //0 legs contrast
            new float[] {0.25F,0.2F,0.05F,1F},
            //1 legs, teeth, stinger
            new float[] {0.35F,0.3F,0.15F,1F},
            //2 stripes contrast
            new float[] {0.1F,-0.05F,-0.15F,1F},
            //3 stripes
            new float[] {0.2F,0.05F,-0.05F,1F},
            //4 body contrast
            new float[] {0.05F,0.0F,-0.1F,1F},
            //5 body
            new float[] {0.15F,0.1F,-0.05F,1F},
            //6 antennae contrast
            new float[] {0.25F,0.15F,0.0F,1F},
            //7 antennae
            new float[] {0.35F,0.25F,0.1F,1F},
            //8 mouth contrast
            new float[] {0.4F,0.15F,0.0F,1F},
            //9 pink skin
            new float[] {0.85F,0.5F,0.6F,1F},
            //10 eyes shine
            new float[] {1.4F,0.85F,0.8F,1F},
            //11 eyes
            new float[] {-0.1F,-0.1F,-0.1F,gloss_alpha},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.2F,0.6F,0.3F,waver_alpha},
            //15 flowing clothes
            new float[] {0.35F,0.75F,0.45F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.4F,0.0F,-0.09F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.5F,0.1F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.6F,0.2F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.7F,0.3F,0.05F,fuzz_alpha},
            //32 fuzz light            
            new float[] {0.8F,0.4F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.9F,0.5F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {-0.1F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 flapping wings contrast, even frames 
            new float[] {0.3F,0.25F,0.1F,spin_alpha_0},
            //37 flapping wings, even frames
            new float[] {0.4F,0.35F,0.2F,spin_alpha_0},
            //38 flapping wings contrast, odd frames
            new float[] {0.3F,0.25F,0.1F,spin_alpha_1},
            //39 flapping wings, odd frames
            new float[] {0.4F,0.35F,0.2F,spin_alpha_1},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            new float[][] { //13 eye tyrant
            //0 teeth contrast
            new float[] {0.55F,0.5F,0.25F,1F},
            //1 teeth
            new float[] {0.9F,0.85F,0.65F,1F},
            //2 eyes on stalks shine
            new float[] {1.15F,1.0F,1.2F,waver_alpha},
            //3 eyes on stalks
            new float[] {0.08F,-0.05F,0.1F,waver_alpha},
            //4 body contrast
            new float[] {0.15F,0.08F,0.2F,1F},
            //5 body
            new float[] {0.3F,0.18F,0.35F,1F},
            //6 hair contrast
            new float[] {0.65F,0.5F,0.2F,1F},
            //7 hair
            new float[] {0.7F,0.05F,0.05F,1F},
            //8 mouth
            new float[] {0.35F,0.15F,0.1F,1F},
            //9 eye white
            new float[] {0.85F,0.7F,0.73F,1F},
            //10 eyes shine
            new float[] {1.15F,1.0F,1.2F,1F},
            //11 eyes
            new float[] {0.08F,-0.05F,0.1F,1F},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 eyestalks contrast
            new float[] {0.15F,0.08F,0.2F,waver_alpha},
            //15 eyestalks
            new float[] {0.3F,0.18F,0.35F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.4F,0.0F,-0.09F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.5F,0.1F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.6F,0.2F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.7F,0.3F,0.05F,fuzz_alpha},
            //32 fuzz light            
            new float[] {0.8F,0.4F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.9F,0.5F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.3F,0.1F,0.35F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            new float[][] { //14 centipede
            //0 legs
            new float[] {0.4F,0.18F,0.05F,1F},
            //1 antennae, teeth
            new float[] {0.25F,0.2F,0.12F,1F},
            //2 body contrast
            new float[] {0.4F,0.05F,-0.08F,1F},
            //3 body
            new float[] {0.55F,0.15F,0.02F,1F},
            //4 stinger contrast
            new float[] {0.55F,0.2F,0.4F,1F},
            //5 stinger
            new float[] {0.7F,0.35F,0.55F,1F},
            //6 jaws contrast
            new float[] {0.25F,0.02F,-0.05F,1F},
            //7 jaws
            new float[] {0.38F,0.08F,0.02F,1F},
            //8 mouth contrast
            new float[] {0.4F,0.15F,0.0F,1F},
            //9 pink skin
            new float[] {0.85F,0.5F,0.6F,1F},
            //10 eyes shine
            new float[] {1.4F,0.85F,0.8F,1F},
            //11 eyes
            new float[] {0.15F,0.1F,0.05F,gloss_alpha},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.2F,0.6F,0.3F,waver_alpha},
            //15 flowing clothes
            new float[] {0.35F,0.75F,0.45F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.4F,0.0F,-0.09F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.5F,0.1F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.6F,0.2F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.7F,0.3F,0.05F,fuzz_alpha},
            //32 fuzz light            
            new float[] {0.8F,0.4F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.9F,0.5F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.5F,0.15F,0.03F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 flapping wings contrast, even frames 
            new float[] {0.7F,0.2F,0.2F,spin_alpha_0},
            //37 flapping wings, even frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_0},
            //38 flapping wings contrast, odd frames
            new float[] {0.7F,0.2F,0.2F,spin_alpha_1},
            //39 flapping wings, odd frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_1},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            new float[][] { //15 sandworm
            //0 legs
            new float[] {0.31F,0.29F,0.33F,1F},
            //1 teeth
            new float[] {0.75F,0.7F,0.45F,1F},
            //2 body contrast
            new float[] {0.5F,0.35F,0.0F,1F},
            //3 body
            new float[] {0.65F,0.5F,0.1F,1F},
            //4 stinger contrast
            new float[] {0.65F,0.2F,0.55F,1F},
            //5 stinger
            new float[] {0.8F,0.35F,0.7F,1F},
            //6 jaws contrast
            new float[] {0.0F,-0.02F,0.02F,1F},
            //7 jaws
            new float[] {0.12F,0.1F,0.15F,1F},
            //8 mouth
            new float[] {0.45F,0.2F,0.03F,1F},
            //9 pink skin
            new float[] {0.85F,0.5F,0.6F,1F},
            //10 eyes shine
            new float[] {1.4F,0.85F,0.8F,1F},
            //11 eyes
            new float[] {0.05F,0.03F,0.08F,gloss_alpha},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.2F,0.6F,0.3F,waver_alpha},
            //15 flowing clothes
            new float[] {0.35F,0.75F,0.45F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.4F,0.6F,0.75F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.3F,1.25F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.25F,1.25F,1F},
            //24 glow frame 3
            new float[] {1.25F,1.25F,0.3F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.4F,0.0F,-0.09F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.5F,0.1F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.6F,0.2F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.7F,0.3F,0.05F,fuzz_alpha},
            //32 fuzz light            
            new float[] {0.8F,0.4F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.9F,0.5F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.5F,0.25F,0.0F,1F},
            //35 glass
            new float[] {0.5F,0.8F,1.1F,1F},
            //36 flapping wings contrast, even frames 
            new float[] {0.7F,0.2F,0.2F,spin_alpha_0},
            //37 flapping wings, even frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_0},
            //38 flapping wings contrast, odd frames
            new float[] {0.7F,0.2F,0.2F,spin_alpha_1},
            //39 flapping wings, odd frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_1},
            //40 flickering sparks
            new float[] {0.93F,0.83F,1.4F,flash_alpha_0},
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
            
            };

        public static float[][][][] schemes = new float[][][][] { scheme0, scheme1 };

        public static float[][][] mecha_palettes = new float[][][]
        {
            new float[][] { //0 bold red mecha
            //0 raw metal contrast
            new float[] {0.45F,0.48F,0.5F,1F},
            //1 raw metal
            new float[] {0.6F,0.63F,0.65F,1F},
            //2 highlight paint contrast
            new float[] {0.5F,0.1F,-0.05F,1F},
            //3 highlight paint
            new float[] {0.7F,0.2F,0.05F,1F},
            //4 base paint contrast
            new float[] {0.3F,-0.15F,-0.15F,1F},
            //5 base paint
            new float[] {0.45F,0.0F,0.0F,1F},
            //6 guns contrast
            new float[] {0.28F,0.23F,0.2F,1F},
            //7 guns
            new float[] {0.43F,0.38F,0.35F,1F},
            //8 flesh contrast
            new float[] {0.4F,0.15F,0.0F,1F},
            //9 flesh
            new float[] {0.88F,0.6F,0.42F,1F},
            //10 tinted lenses shine
            new float[] {1.15F,1.15F,1.2F,1F},
            //11 tinted lenses
            new float[] {0.18F,0.18F,0.2F,1F},
            //12 chrome contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 chrome
            new float[] {0.6F,0.75F,0.95F,1F},
            //14 flowing cloth contrast
            new float[] {0.0F,0.0F,0.0F,waver_alpha},
            //15 flowing cloth
            new float[] {0.1F,0.1F,0.1F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.1F,1.0F,0.65F,flash_alpha},
            //22 glow frame 1
            new float[] {1.25F,1.15F,0.8F,flash_alpha},
            //23 glow frame 2
            new float[] {1.1F,1.0F,0.65F,flash_alpha},
            //24 glow frame 3
            new float[] {0.95F,0.85F,0.5F,flash_alpha},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 oil
            new float[] {0.0F,-0.02F,-0.1F,1F},
            //27 water
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
            //35 normal glass
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
            
        };

        static AlternatePalettes()
        {
            VoxelLogic.wcolorcount = schemes[0][0].Length;
            for (int s = 0; s < schemes.Length; s++)
            {
                for (int p = 0; p < schemes[s].Length; p++)
                {
                    if (p >= 50 && p <= 60)
                        continue;
                    float[] drip = schemes[s][p][27].ToArray(), transp = schemes[s][p][VoxelLogic.wcolorcount - 1];
                    drip[3] = 1F;
                    float[] zap = schemes[s][p][40].ToArray();
                    zap[3] = flash_alpha_1;
                    float[] dark = new float[] { 0.0f + s * 0.20f, -0.03f + s * 0.17f, -0.09f + s * 0.22f, 1f }
                        , violet = new float[] { 0.55f, 0.05f, 0.65f, 1f }
                        , deepviolet = new float[] { 0.3f, 0.05f, 0.4f, 1f }
                        , shiny = new float[] { 1.3f, 1.3f, 0.8f, borderless_alpha }
                        , windy0 = new float[] { 0.95f, 0.85f, 0.65f, flash_alpha_0 }
                        , windy1 = new float[] { 0.95f, 0.85f, 0.65f, flash_alpha_1 }
                        , flurry = new float[] { 0.99f, 0.9f, 0.7f, borderless_alpha };
                    schemes[s][p] = schemes[s][p].Concat(new float[][] {
                        drip, //0 moving water
                        transp, //1
                        transp, //2
                        transp, //3
                        drip, //4 lots of water
                        zap, //5 flickering zap
                        deepviolet, //6 moving darkblob
                        transp, //7
                        transp, //8
                        transp, //9
                        deepviolet, //10 darkblob on the ground, still
                        violet, //11 violet, will move with unit
                        dark, //12 darkness on the unit, will move with unit
                        deepviolet, //13 deep violet, will move with unit
                        shiny, //14 bright light bit
                        transp, //15
                        transp, //16
                        transp, //17
                        windy0, //18 spinning wind frame 0
                        windy1, //19 spinning wind frame 1
                        flurry, //20 randomly appearing lighter dust on ground
                        flurry, //21 randomly appearing lighter mist inside something

                    }).ToArray();
                }

                if (s == 0)
                {
                    for (int p = 0; p < mecha_palettes.Length; p++)
                    {
                        float[] drip = mecha_palettes[p][27].ToArray(), transp = mecha_palettes[p][VoxelLogic.wcolorcount - 1];
                        drip[3] = 1F;
                        float[] zap = new float[] { 0.93F, 0.83F, 1.4F, flash_alpha };
                        float[] dark = new float[] { 0.0f + s * 0.20f, -0.03f + s * 0.17f, -0.09f + s * 0.22f, 1f }
                            , violet = new float[] { 0.55f, 0.05f, 0.65f, 1f }
                            , deepviolet = new float[] { 0.3f, 0.05f, 0.4f, 1f }
                            , shiny = new float[] { 1.3f, 1.3f, 0.8f, borderless_alpha }
                            , windy0 = new float[] { 0.95f, 0.85f, 0.65f, flash_alpha_0 }
                            , windy1 = new float[] { 0.95f, 0.85f, 0.65f, flash_alpha_1 }
                            , flurry = new float[] { 0.99f, 0.9f, 0.7f, borderless_alpha };
                        mecha_palettes[p] = mecha_palettes[p].Concat(new float[][] {
                        drip, //0 moving water
                        transp, //1
                        transp, //2
                        transp, //3
                        drip, //4 lots of water
                        zap, //5 flickering zap
                        deepviolet, //6 moving darkblob
                        transp, //7
                        transp, //8
                        transp, //9
                        deepviolet, //10 darkblob on the ground, still
                        violet, //11 violet, will move with unit
                        dark, //12 darkness on the unit, will move with unit
                        deepviolet, //13 deep violet, will move with unit
                        shiny, //14 bright light bit
                        transp, //15
                        transp, //16
                        transp, //17
                        windy0, //18 spinning wind frame 0
                        windy1, //19 spinning wind frame 1
                        flurry, //20 randomly appearing lighter dust on ground
                        flurry, //21 randomly appearing lighter mist inside something
                        }).ToArray();
                    }
                }
            }
        
        
        
        }


    }
}
