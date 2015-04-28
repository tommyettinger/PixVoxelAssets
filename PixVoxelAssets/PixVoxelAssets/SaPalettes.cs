using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsPV
{
    class SaPalettes
    {
        public const float flat_alpha = VoxelLogic.flat_alpha;
        public const float fuzz_alpha = VoxelLogic.fuzz_alpha;
        public const float waver_alpha = VoxelLogic.waver_alpha;
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

        public static float[][][] wpalettes = new float[][][]
            {
            new float[][] { //0 (18) axarik
            //0 hands contrast
            new float[] {0.5F,0.5F,0.55F,1F},
            //1 hands
            new float[] {0.7F,0.7F,0.75F,1F},
            //2 fur stripe
            new float[] {0.3F,0.35F,0.35F,1F},
            //3 fur
            new float[] {0.6F,0.7F,0.7F,1F},
            //4 pincer stripe
            new float[] {0.63F,0.75F,0.53F,1F},
            //5 pincer
            new float[] {0.8F,0.95F,0.6F,1F},
            //6 hair contrast
            new float[] {0.15F,0.05F,-0.05F,1F},
            //7 hair
            new float[] {0.15F,0.05F,-0.05F,1F},
            //8 skin contrast
            new float[] {0.85F,0.2F,0.8F,1F},
            //9 snout
            new float[] {0.6F,0.2F,0.5F,1F},
            //10 eyes shine
            new float[] {0.35F,0.05F,0.4F,1F},
            //11 eyes
            new float[] {0.3F,0.0F,0.35F,1F},
            //12 metal contrast
            new float[] {0.65F,0.75F,1.1F,1F},
            //13 metal
            new float[] {0.5F,0.6F,0.75F,1F},
            //14 waving tail contrast
            new float[] {0.15F,0.25F,0.25F,waver_alpha},
            //15 waving tail
            new float[] {0.55F,0.65F,0.65F,waver_alpha},
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
            new float[] {0.95F,0.9F,0.45F,1F},
            //22 glow frame 1
            new float[] {1.15F,1.1F,0.65F,1F},
            //23 glow frame 2
            new float[] {0.95F,0.9F,0.45F,1F},
            //24 glow frame 3
            new float[] {0.75F,0.7F,0.25F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.74F,0.3F,-0.05F,fuzz_alpha},
            //29 fuzz lowlight
            new float[] {0.5F,0.2F,-0.09F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.79F,0.34F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.83F,0.38F,0.05F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.9F,0.45F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {1.05F,0.65F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.6F,0.2F,0.5F,1F},
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
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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

            new float[][] { //1 (61) ceglia
            //0 claws contrast
            new float[] {0.85F,0.75F,0.45F,1F},
            //1 claws, teeth, hands, legs, chitin
            new float[] {1.0F,0.9F,0.6F,1F},
            //2 shell contrast
            new float[] {1.1F,0.45F,-0.1F,1F},
            //3 shell
            new float[] {1.3F,0.75F,0.4F,1F},
            //4 arms contrast
            new float[] {0.3F,0.0F,-0.1F,1F},
            //5 arms
            new float[] {0.4F,0.1F,0.0F,1F},
            //6 hand
            new float[] {0.8F,0.45F,0.2F,1F},
            //7 eyestalk
            new float[] {0.15F,0.15F,0.05F,1F},
            //8 flesh contrast
            new float[] {0.4F,0.05F,-0.05F,1F},
            //9 flesh
            new float[] {0.5F,0.15F,0.05F,1F},
            //10 eyes shine
            new float[] {1.4F,0.85F,0.6F,1F},
            //11 eyes
            new float[] {0.2F,0.1F,-0.1F,1F},
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
            new float[] {0.75F,0.3F,-0.05F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,0.95F,0.75F,1F},
            //19 orange fire
            new float[] {1.3F,0.85F,0.5F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.95F,1F},
            //21 glow frame 0
            new float[] {1.1F,0.75F,0.4F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.9F,0.55F,1F},
            //23 glow frame 2
            new float[] {1.1F,0.75F,0.4F,1F},
            //24 glow frame 3
            new float[] {0.95F,0.6F,0.25F,1F},
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
            new float[] {0.4F,0.12F,-0.05F,1F},
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
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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

            new float[][] { //2 (14) erezdo
            //0 armor contrast
            new float[] {0.8F,-0.05F,0.05F,1F},
            //1 armor
            new float[] {0.15F,0.15F,0.15F,1F},
            //2 stone/crystal contrast
            new float[] {0.5F,0.17F,0.2F,1F},
            //3 stone/crystal
            new float[] {0.65F,0.25F,0.3F,1F},
            //4 clothes contrast
            new float[] {0.5F,-0.05F,0.0F,1F},
            //5 clothes
            new float[] {0.8F,-0.05F,0.05F,1F},
            //6 fur contrast
            new float[] {0.7F,0.4F,0.05F,1F},
            //7 fur
            new float[] {0.8F,0.55F,0.1F,1F},
            //8 skin contrast
            new float[] {0.6F,0.35F,0.15F,1F},
            //9 skin
            new float[] {0.75F,0.55F,0.25F,1F},
            //10 eyes shine
            new float[] {1.3F,1.3F,1.1F,1F},
            //11 eyes
            new float[] {0.22F,0.1F,-0.09F,1F},
            //12 metal contrast
            new float[] {0.85F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.8F,0.8F,0.9F,1F},
            //14 cape contrast
            new float[] {0.9F,0.72F,0.15F,waver_alpha},
            //15 cape
            new float[] {0.7F,0.0F,0.1F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.85F,0.25F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,1.0F,1F},
            //21 glow frame 0
            new float[] {1.1F,1.1F,0.9F,1F},
            //22 glow frame 1
            new float[] {1.3F,1.3F,1.1F,1F},
            //23 glow frame 2
            new float[] {1.1F,1.1F,0.9F,1F},
            //24 glow frame 3
            new float[] {0.9F,0.9F,0.7F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.6F,0.3F,-0.05F,fuzz_alpha},
            //29 fuzz lowlight
            new float[] {0.65F,0.35F,0.0F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.7F,0.4F,0.05F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.8F,0.55F,0.1F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.88F,0.7F,0.15F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.95F,0.85F,0.3F,fuzz_alpha},
            //34 gore
            new float[] {0.62F,0.00F,0.05F,1F},
            //35 crystal shine
            new float[] {1.15F,0.65F,0.75F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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
            
            new float[][] { //3 (21) glarosp
            //0 bone contrast
            new float[] {0.6F,0.6F,0.4F,1F},
            //1 spikes
            new float[] {0.8F,0.8F,0.55F,1F},
            //2 scales contrast
            new float[] {0.0F,0.01F,-0.09F,1F},
            //3 scales
            new float[] {0.2F,0.25F,0.15F,1F},
            //4 clothing contrast
            new float[] {0.2F,0.1F,0.05F,1F},
            //5 clothing
            new float[] {0.35F,0.25F,0.2F,1F},
            //6 eyestalk contrast
            new float[] {1.1F,0.5F,0.75F,1F},
            //7 eyestalks
            new float[] {0.65F,0.75F,0.55F,1F},
            //8 mouth inside
            new float[] {0.5F,0.1F,0.0F,1F},
            //9 skin
            new float[] {0.4F,0.5F,0.3F,1F},
            //10 eyes shine
            new float[] {0.7F,0.85F,0.35F,1F},
            //11 eyes
            new float[] {0.05F,0.1F,-0.05F,1F},
            //12 metal contrast
            new float[] {0.7F,1.15F,0.7F,1F},
            //13 metal
            new float[] {0.8F,0.9F,0.8F,1F},
            //14 flowing clothes contrast
            new float[] {0.2F,0.1F,0.05F,waver_alpha},
            //15 flowing clothes
            new float[] {0.35F,0.25F,0.2F,waver_alpha},
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
            new float[] {0.95F,0.9F,0.45F,1F},
            //22 glow frame 1
            new float[] {1.15F,1.1F,0.65F,1F},
            //23 glow frame 2
            new float[] {0.95F,0.9F,0.45F,1F},
            //24 glow frame 3
            new float[] {0.75F,0.7F,0.25F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.64F,0.14F,-0.05F,fuzz_alpha},
            //29 fuzz lowlight
            new float[] {0.4F,0.0F,-0.09F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.69F,0.2F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.73F,0.23F,0.05F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.8F,0.3F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.95F,0.75F,0.25F,fuzz_alpha},
            //34 gore
            new float[] {0.75F,0.09F,-0.05F,1F},
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
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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
            










            new float[][] { //4 (0) brown hair human
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
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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

            new float[][] { //5 (1) blonde hair human
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
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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
            
            new float[][] { //6 (15) gold-skinned human
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
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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
            
            new float[][] { //7 (16) dark-skinned human
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
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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

            new float[][] { //8 (17) brown-skinned human
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
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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
            
            new float[][] { //9 (new) olive skin human
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
            new float[] {-0.05F,-0.05F,-0.05F,1F},
            //7 hair
            new float[] {0.0F,0.0F,0.0F,1F},
            //8 skin contrast
            new float[] {0.51F,0.3F,0.1F,1F},
            //9 skin
            new float[] {0.66F,0.42F,0.2F,1F},
            //10 eyes shine
            new float[] {1.4F,1.4F,1.4F,1F},
            //11 eyes
            new float[] {0.15F,0.1F,-0.03F,1F},
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

            new float[][] { //10 (new) red hair human
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
            new float[] {0.8F,0.33F,0.0F,1F},
            //7 hair
            new float[] {0.9F,0.4F,-0.05F,1F},
            //8 skin contrast
            new float[] {0.88F,0.41F,0.18F,1F},
            //9 skin
            new float[] {1.02F,0.84F,0.55F,1F},
            //10 eyes shine
            new float[] {1.4F,1.4F,1.4F,1F},
            //11 eyes
            new float[] {0.02F,0.18F,0.29F,1F},
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

            new float[][] { //11 (12) ilapa
            //0 spike contrast
            new float[] {0.5F,0.5F,0.5F,1F},
            //1 spikes
            new float[] {0.65F,0.65F,0.65F,1F},
            //2 arms contrast
            new float[] {0.55F,0.55F,0.4F,1F},
            //3 arms
            new float[] {0.75F,0.75F,0.5F,1F},
            //4 fur contrast
            new float[] {0.96F,0.9F,0.55F,1F},
            //5 fur
            new float[] {1.05F,1.0F,0.65F,1F},
            //6 hair contrast
            new float[] {1.1F,0.5F,0.75F,1F},
            //7 hair
            new float[] {1.25F,0.6F,0.85F,1F},
            //8 skin contrast
            new float[] {0.5F,0.25F,0.3F,1F},
            //9 skin
            new float[] {0.9F,0.85F,0.7F,1F},
            //10 eyes shine
            new float[] {0.9F,0.65F,1.3F,1F},
            //11 eyes
            new float[] {0.05F,-0.05F,0.1F,1F},
            //12 metal contrast
            new float[] {0.7F,0.7F,0.8F,1F},
            //13 metal
            new float[] {0.8F,0.85F,0.95F,1F},
            //14 flowing clothes contrast
            new float[] {0.92F,0.7F,0.3F,waver_alpha},
            //15 flowing clothes
            new float[] {1.05F,1.0F,0.65F,waver_alpha},
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
            new float[] {0.95F,0.9F,0.45F,1F},
            //22 glow frame 1
            new float[] {1.15F,1.1F,0.65F,1F},
            //23 glow frame 2
            new float[] {0.95F,0.9F,0.45F,1F},
            //24 glow frame 3
            new float[] {0.75F,0.7F,0.25F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.64F,0.14F,-0.05F,fuzz_alpha},
            //29 fuzz lowlight
            new float[] {0.4F,0.0F,-0.09F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.69F,0.2F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.73F,0.23F,0.05F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.8F,0.3F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.95F,0.75F,0.25F,fuzz_alpha},
            //34 gore
            new float[] {0.85F,0.5F,0.75F,1F},
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
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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
            
            new float[][] { //12 (13) kurguiv
            //0 bone contrast
            new float[] {0.5F,0.45F,0.35F,1F},
            //1 bones
            new float[] {0.85F,0.85F,0.7F,1F},
            //2 insectoid legs contrast
            new float[] {0.55F,0.05F,-0.05F,1F},
            //3 insectoid legs
            new float[] {0.75F,0.15F,0.05F,1F},
            //4 feathers contrast
            new float[] {0.15F,0.15F,-0.05F,1F},
            //5 feathers
            new float[] {0.8F,0.8F,0.35F,1F},
            //6 crest contrast
            new float[] {0.85F,0.8F,-0.05F,1F},
            //7 crest
            new float[] {1.15F,1.1F,0.35F,1F},
            //8 beak
            new float[] {0.7F,0.4F,0.1F,1F},
            //9 skin
            new float[] {0.5F,0.35F,0.1F,1F},
            //10 eyes shine
            new float[] {1.3F,1.3F,1.0F,1F},
            //11 eyes
            new float[] {0.05F,0.1F,-0.05F,1F},
            //12 metal contrast
            new float[] {0.45F,0.0F,0.1F,1F},
            //13 metal
            new float[] {0.1F,0.1F,0.1F,1F},
            //14 flowing clothes contrast
            new float[] {0.4F,0.7F,0.2F,waver_alpha},
            //15 flowing clothes
            new float[] {0.55F,0.8F,0.35F,waver_alpha},
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
            new float[] {0.9F,0.95F,0.9F,1F},
            //22 glow frame 1
            new float[] {1.1F,1.15F,1.1F,1F},
            //23 glow frame 2
            new float[] {0.95F,0.9F,0.45F,1F},
            //24 glow frame 3
            new float[] {0.7F,0.75F,0.7F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.64F,0.14F,-0.05F,fuzz_alpha},
            //29 fuzz lowlight
            new float[] {0.4F,0.0F,-0.09F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.69F,0.2F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.73F,0.23F,0.05F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.8F,0.3F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.95F,0.75F,0.25F,fuzz_alpha},
            //34 gore
            new float[] {0.9F,0.5F,0.0F,1F},
            //35 glass
            new float[] {0.4F,0.4F,0.3F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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

            new float[][] { //13 (20) lomuk
            //0 bone highlight
            new float[] {0.5F,0.45F,0.35F,1F},
            //1 bones
            new float[] {0.85F,0.85F,0.7F,1F},
            //2 legs contrast
            new float[] {0.65F,0.35F,-0.05F,1F},
            //3 legs
            new float[] {0.85F,0.65F,0.05F,1F},
            //4 unmoving fur contrast
            new float[] {0.0F,0.0F,0.5F,1F},
            //5 unmoving fur
            new float[] {0.0F,0.0F,0.65F,1F},
            //6 hair contrast
            new float[] {0.0F,0.05F,0.15F,1F},
            //7 hair
            new float[] {0.05F,0.1F,0.25F,1F},
            //8 skin contrast
            new float[] {0.0F,0.0F,0.1F,1F},
            //9 skin
            new float[] {0F,0.8F,0.9F,1F},
            //10 eyes shine
            new float[] {0.75F,1.1F,1.0F,1F},
            //11 eyes
            new float[] {0.05F,0.1F,0.1F,1F},
            //12 metal contrast
            new float[] {0.6F,1.0F,1.2F,1F},
            //13 metal
            new float[] {0.4F,0.7F,0.85F,1F},
            //14 wiggly parts contrast
            new float[] {0.0F,0.0F,0.5F,waver_alpha},
            //15 wiggly parts
            new float[] {0.0F,0.0F,0.65F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {0.5F,1.3F,1.2F,1F},
            //19 orange fire
            new float[] {0.7F,1.15F,0.9F,1F},
            //20 sparks
            new float[] {0.9F,1.2F,1.3F,1F},
            //21 glow frame 0
            new float[] {0.25F,0.7F,0.65F,1F},
            //22 glow frame 1
            new float[] {0.35F,0.8F,0.75F,1F},
            //23 glow frame 2
            new float[] {0.25F,0.7F,0.65F,1F},
            //24 glow frame 3
            new float[] {0.15F,0.6F,0.55F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.0F,0.0F,0.35F,fuzz_alpha},
            //29 fuzz lowlight
            new float[] {0.0F,0.0F,0.45F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.04F,0.04F,0.55F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.08F,0.08F,0.65F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.15F,0.15F,0.75F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.25F,0.25F,0.95F,fuzz_alpha},
            //34 gore
            new float[] {0.0F,0.0F,0.1F,1F},
            //35 glass
            new float[] {0.65F,0.1F,0.85F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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
            
            new float[][] { //14 (10) nodebpe
            //0 bone highlight
            new float[] {0.5F,0.45F,0.35F,1F},
            //1 bones
            new float[] {0.85F,0.85F,0.7F,1F},
            //2 legs contrast
            new float[] {0.65F,0.35F,-0.05F,1F},
            //3 legs
            new float[] {0.85F,0.65F,0.05F,1F},
            //4 fur contrast
            new float[] {0.5F,0.15F,-0.09F,1F},
            //5 fur
            new float[] {0.6F,0.2F,-0.05F,1F},
            //6 hair contrast
            new float[] {0.15F,0.05F,-0.05F,1F},
            //7 hair
            new float[] {0.15F,0.05F,-0.05F,1F},
            //8 skin contrast
            new float[] {0.6F,0.1F,0.15F,1F},
            //9 skin
            new float[] {1.1F,0.75F,0.3F,1F},
            //10 eyes shine
            new float[] {0.9F,1.2F,0.9F,1F},
            //11 eyes
            new float[] {0.3F,0.75F,0.25F,1F},
            //12 metal contrast
            new float[] {0.75F,0.45F,-0.1F,1F},
            //13 metal
            new float[] {0.75F,0.45F,-0.1F,1F},
            //14 flowing clothes contrast
            new float[] {0.2F,0.0F,-0.05F,waver_alpha},
            //15 flowing clothes
            new float[] {0.3F,0.05F,0.0F,waver_alpha},
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
            new float[] {0.95F,0.9F,0.45F,1F},
            //22 glow frame 1
            new float[] {1.15F,1.1F,0.65F,1F},
            //23 glow frame 2
            new float[] {0.95F,0.9F,0.45F,1F},
            //24 glow frame 3
            new float[] {0.75F,0.7F,0.25F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.64F,0.14F,-0.05F,fuzz_alpha},
            //29 fuzz lowlight
            new float[] {0.4F,0.0F,-0.09F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.69F,0.2F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.73F,0.23F,0.05F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.8F,0.3F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.95F,0.75F,0.25F,fuzz_alpha},
            //34 gore
            new float[] {0.4F,0.6F,0.15F,1F},
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
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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
            
            new float[][] { //15 (62) oah
            //0 obsidian contrast
            new float[] {0.1F,0.07F,0.03F,1F},
            //1 obsidian
            new float[] {0.25F,0.22F,0.18F,1F},
            //2 cannon contrast
            new float[] {0.12F,0.3F,0.3F,1F},
            //3 cannon
            new float[] {0.25F,0.45F,0.45F,1F},
            //4 arms contrast
            new float[] {0.3F,0.0F,-0.1F,1F},
            //5 arms
            new float[] {0.4F,0.1F,0.0F,1F},
            //6 hand
            new float[] {0.8F,0.45F,0.2F,1F},
            //7 eyestalk
            new float[] {0.15F,0.15F,0.05F,1F},
            //8 flesh contrast
            new float[] {0.4F,0.05F,-0.05F,1F},
            //9 flesh
            new float[] {0.5F,0.15F,0.05F,1F},
            //10 eyes shine
            new float[] {1.1F,1.1F,1.4F,1F},
            //11 eyes
            new float[] {0.1F,0.1F,0.15F,1F},
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
            new float[] {1.1F,0.75F,0.4F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.9F,0.55F,1F},
            //23 glow frame 2
            new float[] {1.1F,0.75F,0.4F,1F},
            //24 glow frame 3
            new float[] {0.95F,0.6F,0.25F,1F},
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
            new float[] {0.4F,0.12F,-0.05F,1F},
            //35 glass
            new float[] {0.5F,0.6F,0.75F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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
            
            new float[][] { //16 (24) sfyst
            //0 feet contrast
            new float[] {0.25F,0.2F,0.15F,1F},
            //1 feet
            new float[] {0.35F,0.3F,0.25F,1F},
            //2 clothing contrast
            new float[] {0.5F,0.25F,0.75F,1F},
            //3 clothing
            new float[] {0.4F,0.35F,0.85F,1F},
            //4 body, fins contrast
            new float[] {0.42F,0.32F,0.27F,1F},
            //5 body, fins
            new float[] {0.6F,0.55F,0.5F,1F},
            //6 hair contrast
            new float[] {0.15F,0.05F,-0.05F,1F},
            //7 hair
            new float[] {0.15F,0.05F,-0.05F,1F},
            //8 skin contrast
            new float[] {0.35F,0.0F,0.05F,1F},
            //9 skin
            new float[] {0.6F,0.5F,0.65F,1F},
            //10 eyes shine
            new float[] {0.7F,0.9F,0.95F,1F},
            //11 eyes
            new float[] {0.05F,0.1F,0.1F,1F},
            //12 metal contrast
            new float[] {0.9F,1.05F,1.1F,1F},
            //13 metal
            new float[] {0.7F,0.85F,0.9F,1F},
            //14 trunktacles contrast
            new float[] {0.42F,0.37F,0.32F,waver_alpha},
            //15 trunktacles
            new float[] {0.6F,0.57F,0.52F,waver_alpha},
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
            new float[] {0.95F,0.9F,0.45F,1F},
            //22 glow frame 1
            new float[] {1.15F,1.1F,0.65F,1F},
            //23 glow frame 2
            new float[] {0.95F,0.9F,0.45F,1F},
            //24 glow frame 3
            new float[] {0.75F,0.7F,0.25F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.74F,0.3F,-0.05F,fuzz_alpha},
            //29 fuzz lowlight
            new float[] {0.5F,0.2F,-0.09F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.79F,0.34F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.83F,0.38F,0.05F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.9F,0.45F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {1.05F,0.65F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.4F,0.55F,0.6F,1F},
            //35 glass
            new float[] {0.5F,0.9F,0.9F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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
            
            new float[][] { //17 (11) tassar
            //0 bone highlight
            new float[] {0.5F,0.45F,0.35F,1F},
            //1 bones
            new float[] {0.85F,0.85F,0.7F,1F},
            //2 hat contrast
            new float[] {0.2F,0.05F,0.4F,1F},
            //3 hat
            new float[] {0.25F,0.05F,0.45F,1F},
            //4 robes contrast
            new float[] {0.4F,0.1F,0.3F,1F},
            //5 robes
            new float[] {0.3F,0.15F,0.5F,1F},
            //6 hair contrast
            new float[] {0.15F,0.05F,-0.05F,1F},
            //7 hair
            new float[] {0.15F,0.05F,-0.05F,1F},
            //8 skin contrast
            new float[] {0.6F,0.1F,0.15F,1F},
            //9 skin
            new float[] {1.1F,0.75F,0.3F,1F},
            //10 eyes shine
            new float[] {1.05F,0.8F,1.2F,1F},
            //11 eyes
            new float[] {0.0F,-0.08F,0.1F,gloss_alpha},
            //12 metal contrast
            new float[] {0.5F,0.85F,0.95F,1F},
            //13 metal
            new float[] {0.75F,0.95F,1.15F,1F},
            //14 flowing clothes contrast
            new float[] {0.4F,0.1F,0.3F,waver_alpha},
            //15 flowing clothes
            new float[] {0.3F,0.15F,0.5F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {0.6F,1.25F,0.95F,1F},
            //19 orange fire
            new float[] {0.5F,0.9F,1.0F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {0.95F,0.9F,0.45F,1F},
            //22 glow frame 1
            new float[] {1.15F,1.1F,0.65F,1F},
            //23 glow frame 2
            new float[] {0.95F,0.9F,0.45F,1F},
            //24 glow frame 3
            new float[] {0.75F,0.7F,0.25F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.64F,0.14F,-0.05F,fuzz_alpha},
            //29 fuzz lowlight
            new float[] {0.4F,0.0F,-0.09F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.69F,0.2F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.73F,0.23F,0.05F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.8F,0.3F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.95F,0.75F,0.25F,fuzz_alpha},
            //34 gore
            new float[] {0.5F,0.15F,0.4F,1F},
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
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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
            
            new float[][] { //18 (19) vashk
            //0 talons, claws, horns highlight
            new float[] {0.4F,0.15F,0.05F,1F},
            //1 talons, claws, horns
            new float[] {0.65F,0.55F,0.3F,1F},
            //2 scales contrast
            new float[] {0.05F,0.25F,0.0F,1F},
            //3 scales
            new float[] {0.2F,0.35F,0.15F,1F},
            //4 crest contrast
            new float[] {0.75F,0.0F,-0.09F,1F},
            //5 crest
            new float[] {1.1F,0.7F,0.1F,1F},
            //6 hair contrast
            new float[] {0.15F,0.05F,-0.05F,1F},
            //7 hair
            new float[] {0.15F,0.05F,-0.05F,1F},
            //8 skin contrast
            new float[] {0.5F,0.15F,0.1F,1F},
            //9 skin
            new float[] {0.85F,1.0F,0.7F,1F},
            //10 eyes shine
            new float[] {1.4F,0.15F,0.15F,1F},
            //11 eyes
            new float[] {0.15F,0.08F,0.0F,1F},
            //12 metal contrast
            new float[] {1.1F,1.1F,1.3F,1F},
            //13 metal
            new float[] {0.65F,0.75F,0.9F,1F},
            //14 flowing clothes contrast
            new float[] {0.0F,0.0F,0.25F,waver_alpha},
            //15 flowing clothes
            new float[] {0.2F,0.2F,0.45F,waver_alpha},
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
            new float[] {0.95F,0.9F,0.45F,1F},
            //22 glow frame 1
            new float[] {1.15F,1.1F,0.65F,1F},
            //23 glow frame 2
            new float[] {0.95F,0.9F,0.45F,1F},
            //24 glow frame 3
            new float[] {0.75F,0.7F,0.25F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.74F,0.3F,-0.05F,fuzz_alpha},
            //29 fuzz lowlight
            new float[] {0.5F,0.2F,-0.09F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.79F,0.34F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.83F,0.38F,0.05F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.9F,0.45F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {1.05F,0.65F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.65F,0.2F,0.1F,1F},
            //35 glass
            new float[] {0.65F,0.6F,0.55F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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
            
            

            new float[][] { //19 (22) pelmir
            //0 feet contrast
            new float[] {0.05F,0.25F,-0.05F,1F},
            //1 feet
            new float[] {0.1F,0.4F,0.0F,1F},
            //2 flower contrast
            new float[] {1.1F,0.25F,0.1F,1F},
            //3 flower
            new float[] {0.9F,0.8F,0.3F,1F},
            //4 bark contrast
            new float[] {0.2F,0.45F,0.1F,1F},
            //5 bark
            new float[] {0.27F,0.6F,0.15F,1F},
            //6 tendrilstalks contrast
            new float[] {1.2F,0.3F,0.05F,1F},
            //7 tendrilstalks
            new float[] {0.55F,0.05F,0.05F,1F},
            //8 skin contrast
            new float[] {0.35F,0.05F,-0.05F,1F},
            //9 skin
            new float[] {0.9F,0.6F,0.25F,1F},
            //10 eyes shine
            new float[] {1.4F,1.4F,1.4F,1F},
            //11 eyes
            new float[] {0.05F,0.15F,-0.1F,1F},
            //12 metal contrast
            new float[] {0.5F,0.35F,0.1F,1F},
            //13 metal
            new float[] {0.75F,0.5F,0.2F,1F},
            //14 tail contrast
            new float[] {0.2F,0.5F,0.1F,waver_alpha},
            //15 tail
            new float[] {0.27F,0.65F,0.15F,waver_alpha},
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
            new float[] {0.7F,0.11F,-0.05F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.74F,0.16F,-0.05F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.79F,0.19F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.83F,0.22F,0.05F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.88F,0.25F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.95F,0.75F,0.25F,fuzz_alpha},
            //34 gore
            new float[] {0.87F,0.55F,0.15F,1F},
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
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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
            
            new float[][] { //20 (23) uljir
            //0 talons, barbs, horns highlight
            new float[] {0.4F,0.3F,0.2F,1F},
            //1 talons, claws, horns
            new float[] {0.75F,0.65F,0.55F,1F},
            //2 pants contrast
            new float[] {0.0F,-0.05F,0.0F,1F},
            //3 pants
            new float[] {0.15F,0.1F,0.15F,1F},
            //4 shirt contrast
            new float[] {0.15F,0.15F,0.3F,1F},
            //5 shirt
            new float[] {0.2F,0.2F,0.4F,1F},
            //6 hair contrast
            new float[] {0.15F,0.05F,-0.05F,1F},
            //7 hair
            new float[] {0.15F,0.05F,-0.05F,1F},
            //8 skin contrast
            new float[] {0.35F,0.0F,0.05F,1F},
            //9 skin
            new float[] {0.6F,0.5F,0.65F,1F},
            //10 eyes shine
            new float[] {0.7F,0.7F,0.7F,1F},
            //11 eyes
            new float[] {0.05F,0.0F,0.05F,1F},
            //12 metal contrast
            new float[] {0.7F,0.55F,0.8F,1F},
            //13 metal
            new float[] {0.55F,0.4F,0.65F,1F},
            //14 flowing clothes contrast
            new float[] {0.65F,0.0F,0.1F,waver_alpha},
            //15 flowing clothes
            new float[] {0.25F,0.1F,0.12F,waver_alpha},
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
            new float[] {0.95F,0.9F,0.45F,1F},
            //22 glow frame 1
            new float[] {1.15F,1.1F,0.65F,1F},
            //23 glow frame 2
            new float[] {0.95F,0.9F,0.45F,1F},
            //24 glow frame 3
            new float[] {0.75F,0.7F,0.25F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.74F,0.3F,-0.05F,fuzz_alpha},
            //29 fuzz lowlight
            new float[] {0.5F,0.2F,-0.09F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.79F,0.34F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.83F,0.38F,0.05F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.9F,0.45F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {1.05F,0.65F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.05F,-0.05F,0.05F,1F},
            //35 glass
            new float[] {0.5F,0.4F,0.55F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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
            







            new float[][] { //21 eidolon (light)
            //0 feet contrast
            new float[] {0.25F,0.2F,0.15F,1F},
            //1 feet
            new float[] {0.35F,0.3F,0.25F,1F},
            //2 clothing contrast
            new float[] {0.5F,0.25F,0.75F,1F},
            //3 clothing
            new float[] {0.4F,0.35F,0.85F,1F},
            //4 body, fins contrast
            new float[] {0.42F,0.32F,0.27F,1F},
            //5 body, fins
            new float[] {0.6F,0.55F,0.5F,1F},
            //6 hair contrast
            new float[] {0.15F,0.05F,-0.05F,1F},
            //7 hair
            new float[] {0.15F,0.05F,-0.05F,1F},
            //8 skin contrast
            new float[] {0.35F,0.0F,0.05F,1F},
            //9 skin
            new float[] {0.6F,0.5F,0.65F,1F},
            //10 eyes shine
            new float[] {0.7F,0.9F,0.95F,1F},
            //11 eyes
            new float[] {0.05F,0.1F,0.1F,1F},
            //12 metal contrast
            new float[] {0.9F,1.05F,1.1F,1F},
            //13 metal
            new float[] {0.7F,0.85F,0.9F,1F},
            //14 wavering solids contrast
            new float[] {0.42F,0.37F,0.32F,waver_alpha},
            //15 wavering solids
            new float[] {0.6F,0.57F,0.52F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.95F,0.77F,0.9F,bordered_alpha},
            //18 yellow fire
            new float[] {0.92F,0.92F,0.5F,bordered_alpha},
            //19 orange fire
            new float[] {0.75F,0.95F,0.7F,bordered_alpha},
            //20 sparks
            new float[] {0.95F,0.95F,0.75F,bordered_alpha},
            //21 glow frame 0
            new float[] {1.4F,0.85F,0.65F,bordered_alpha},
            //22 glow frame 1
            new float[] {0.8F,1.4F,0.7F,bordered_alpha},
            //23 glow frame 2
            new float[] {0.8F,0.8F,1.4F,bordered_alpha},
            //24 glow frame 3
            new float[] {1.1F,1.1F,1.1F,bordered_alpha},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.74F,0.3F,-0.05F,fuzz_alpha},
            //29 fuzz lowlight
            new float[] {0.5F,0.2F,-0.09F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.79F,0.34F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.83F,0.38F,0.05F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.9F,0.45F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {1.05F,0.65F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.4F,0.55F,0.6F,1F},
            //35 glass
            new float[] {0.75F,0.75F,0.65F,bordered_alpha},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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
            
            new float[][] { //22 eidolon (dark)
            //0 feet contrast
            new float[] {0.25F,0.2F,0.15F,1F},
            //1 feet
            new float[] {0.35F,0.3F,0.25F,1F},
            //2 clothing contrast
            new float[] {0.5F,0.25F,0.75F,1F},
            //3 clothing
            new float[] {0.4F,0.35F,0.85F,1F},
            //4 body, fins contrast
            new float[] {0.42F,0.32F,0.27F,1F},
            //5 body, fins
            new float[] {0.6F,0.55F,0.5F,1F},
            //6 hair contrast
            new float[] {0.15F,0.05F,-0.05F,1F},
            //7 hair
            new float[] {0.15F,0.05F,-0.05F,1F},
            //8 skin contrast
            new float[] {0.35F,0.0F,0.05F,1F},
            //9 skin
            new float[] {0.6F,0.5F,0.65F,1F},
            //10 eyes shine
            new float[] {0.7F,0.9F,0.95F,1F},
            //11 eyes
            new float[] {0.05F,0.1F,0.1F,1F},
            //12 metal contrast
            new float[] {0.09F,0.06F,0.0F,1F},
            //13 metal
            new float[] {0.13F,0.1F,0.04F,1F},
            //14 wavering solids contrast
            new float[] {0.42F,0.37F,0.32F,waver_alpha},
            //15 wavering solids
            new float[] {0.6F,0.57F,0.52F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.08F,0.02F,0.1F,bordered_alpha},
            //18 yellow fire
            new float[] {0.1F,-0.05F,0.15F,bordered_alpha},
            //19 orange fire
            new float[] {0.2F,0.05F,0.35F,bordered_alpha},
            //20 sparks
            new float[] {0.28F,0.25F,0.3F,bordered_alpha},
            //21 glow frame 0
            new float[] {0.1F,0.1F,0.1F,bordered_alpha},
            //22 glow frame 1
            new float[] {0.2F,0.2F,0.2F,bordered_alpha},
            //23 glow frame 2
            new float[] {0.1F,0.1F,0.1F,bordered_alpha},
            //24 glow frame 3
            new float[] {0.0F,0.0F,0.0F,bordered_alpha},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.74F,0.3F,-0.05F,fuzz_alpha},
            //29 fuzz lowlight
            new float[] {0.5F,0.2F,-0.09F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.79F,0.34F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.83F,0.38F,0.05F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.9F,0.45F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {1.05F,0.65F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.4F,0.55F,0.6F,1F},
            //35 glass
            new float[] {0.18F,0.13F,0.2F,bordered_alpha},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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
            
            new float[][] { //23 eidolon (atomic)
            //0 feet contrast
            new float[] {0.25F,0.2F,0.15F,1F},
            //1 feet
            new float[] {0.35F,0.3F,0.25F,1F},
            //2 clothing contrast
            new float[] {0.5F,0.25F,0.75F,1F},
            //3 clothing
            new float[] {0.4F,0.35F,0.85F,1F},
            //4 body, fins contrast
            new float[] {0.42F,0.32F,0.27F,1F},
            //5 body, fins
            new float[] {0.6F,0.55F,0.5F,1F},
            //6 hair contrast
            new float[] {0.15F,0.05F,-0.05F,1F},
            //7 hair
            new float[] {0.15F,0.05F,-0.05F,1F},
            //8 skin contrast
            new float[] {0.35F,0.0F,0.05F,1F},
            //9 skin
            new float[] {0.6F,0.5F,0.65F,1F},
            //10 eyes shine
            new float[] {0.7F,0.9F,0.95F,1F},
            //11 eyes
            new float[] {0.05F,0.1F,0.1F,1F},
            //12 metal contrast
            new float[] {0.9F,1.05F,1.1F,1F},
            //13 metal
            new float[] {0.7F,0.85F,0.9F,1F},
            //14 wavering solids contrast
            new float[] {0.42F,0.37F,0.32F,waver_alpha},
            //15 wavering solids
            new float[] {0.6F,0.57F,0.52F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.95F,0.6F,0.9F,bordered_alpha},
            //18 yellow fire
            new float[] {0.88F,0.45F,0.82F,bordered_alpha},
            //19 orange fire
            new float[] {0.73F,0.35F,0.9F,bordered_alpha},
            //20 sparks
            new float[] {0.85F,0.85F,0.6F,bordered_alpha},
            //21 glow frame 0
            new float[] {0.9F,0.3F,0.9F,bordered_alpha},
            //22 glow frame 1
            new float[] {1.05F,0.4F,1.05F,bordered_alpha},
            //23 glow frame 2
            new float[] {0.9F,0.3F,0.9F,bordered_alpha},
            //24 glow frame 3
            new float[] {0.75F,0.2F,0.75F,bordered_alpha},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.74F,0.3F,-0.05F,fuzz_alpha},
            //29 fuzz lowlight
            new float[] {0.5F,0.2F,-0.09F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.79F,0.34F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.83F,0.38F,0.05F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.9F,0.45F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {1.05F,0.65F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.4F,0.55F,0.6F,1F},
            //35 glass
            new float[] {0.6F,0.4F,0.6F,bordered_alpha},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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
            
            new float[][] { //24 eidolon (kinetic)
            //0 still debris contrast
            new float[] {0.7F,0.2F,0.2F,1F},
            //1 still debris
            new float[] {0.65F,0.65F,0.65F,1F},
            //2 clothing contrast
            new float[] {0.5F,0.25F,0.75F,1F},
            //3 clothing
            new float[] {0.4F,0.35F,0.85F,1F},
            //4 body, fins contrast
            new float[] {0.42F,0.32F,0.27F,1F},
            //5 body, fins
            new float[] {0.6F,0.55F,0.5F,1F},
            //6 hair contrast
            new float[] {0.15F,0.05F,-0.05F,1F},
            //7 hair
            new float[] {0.15F,0.05F,-0.05F,1F},
            //8 skin contrast
            new float[] {0.35F,0.0F,0.05F,1F},
            //9 skin
            new float[] {0.6F,0.5F,0.65F,1F},
            //10 eyes shine
            new float[] {0.7F,0.9F,0.95F,1F},
            //11 eyes
            new float[] {0.05F,0.1F,0.1F,1F},
            //12 metal contrast
            new float[] {0.9F,1.05F,1.1F,1F},
            //13 metal
            new float[] {0.7F,0.85F,0.9F,1F},
            //14 wavering solids contrast
            new float[] {0.3F,0.3F,0.3F,waver_alpha},
            //15 wavering solids
            new float[] {0.45F,0.45F,0.45F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.35F,0.35F,0.5F,bordered_alpha},
            //18 yellow fire
            new float[] {0.4F,0.45F,0.9F,bordered_alpha},
            //19 orange fire
            new float[] {0.3F,0.95F,0.5F,bordered_alpha},
            //20 sparks
            new float[] {0.95F,0.95F,0.95F,bordered_alpha},
            //21 glow frame 0
            new float[] {0.6F,0.7F,0.7F,bordered_alpha},
            //22 glow frame 1
            new float[] {0.75F,0.85F,0.85F,bordered_alpha},
            //23 glow frame 2
            new float[] {0.6F,0.7F,0.7F,bordered_alpha},
            //24 glow frame 3
            new float[] {0.45F,0.55F,0.55F,bordered_alpha},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.74F,0.3F,-0.05F,fuzz_alpha},
            //29 fuzz lowlight
            new float[] {0.5F,0.2F,-0.09F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.79F,0.34F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.83F,0.38F,0.05F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.9F,0.45F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {1.05F,0.65F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.4F,0.55F,0.6F,1F},
            //35 glass
            new float[] {0.18F,0.13F,0.2F,bordered_alpha},
            //36 moving debris contrast, even frames 
            new float[] {0.7F,0.2F,0.2F,spin_alpha_0},
            //37 moving debris, even frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_0},
            //38 moving debris contrast, odd frames
            new float[] {0.7F,0.2F,0.2F,spin_alpha_1},
            //39 moving debris, odd frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_1},
            //40 flickering phantom
            new float[] {0.4F,0.95F,0.0F,spin_alpha_0},
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
            
            new float[][] { //25 eidolon (fire)
            //0 feet contrast
            new float[] {0.25F,0.2F,0.15F,1F},
            //1 feet
            new float[] {0.35F,0.3F,0.25F,1F},
            //2 clothing contrast
            new float[] {0.5F,0.25F,0.75F,1F},
            //3 clothing
            new float[] {0.4F,0.35F,0.85F,1F},
            //4 body, fins contrast
            new float[] {0.42F,0.32F,0.27F,1F},
            //5 body, fins
            new float[] {0.6F,0.55F,0.5F,1F},
            //6 hair contrast
            new float[] {0.15F,0.05F,-0.05F,1F},
            //7 hair
            new float[] {0.15F,0.05F,-0.05F,1F},
            //8 skin contrast
            new float[] {0.35F,0.0F,0.05F,1F},
            //9 skin
            new float[] {0.6F,0.5F,0.65F,1F},
            //10 eyes shine
            new float[] {0.7F,0.9F,0.95F,1F},
            //11 eyes
            new float[] {0.05F,0.1F,0.1F,1F},
            //12 metal contrast
            new float[] {0.9F,1.05F,1.1F,1F},
            //13 metal
            new float[] {0.7F,0.85F,0.9F,1F},
            //14 wavering solids contrast
            new float[] {0.3F,0.3F,0.3F,waver_alpha},
            //15 wavering solids
            new float[] {0.45F,0.45F,0.45F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,bordered_alpha},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,bordered_alpha},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,bordered_alpha},
            //21 glow frame 0
            new float[] {0.85F,0.35F,0.05F,bordered_alpha},
            //22 glow frame 1
            new float[] {1.1F,0.55F,0F,bordered_alpha},
            //23 glow frame 2
            new float[] {0.85F,0.35F,0.05F,bordered_alpha},
            //24 glow frame 3
            new float[] {0.75F,0.2F,-0.05F,bordered_alpha},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.74F,0.3F,-0.05F,fuzz_alpha},
            //29 fuzz lowlight
            new float[] {0.5F,0.2F,-0.09F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.79F,0.34F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.83F,0.38F,0.05F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.9F,0.45F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {1.05F,0.65F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.4F,0.55F,0.6F,1F},
            //35 glass
            new float[] {0.18F,0.13F,0.2F,bordered_alpha},
            //36 moving debris contrast, even frames 
            new float[] {0.7F,0.2F,0.2F,spin_alpha_0},
            //37 moving debris, even frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_0},
            //38 moving debris contrast, odd frames
            new float[] {0.7F,0.2F,0.2F,spin_alpha_1},
            //39 moving debris, odd frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_1},
            //40 flickering phantom            
            new float[] {0.7F,0.8F,0.8F,spin_alpha_0},
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
            
            new float[][] { //26 eidolon (cold)
            //0 feet contrast
            new float[] {0.25F,0.2F,0.15F,1F},
            //1 feet
            new float[] {0.35F,0.3F,0.25F,1F},
            //2 clothing contrast
            new float[] {0.5F,0.25F,0.75F,1F},
            //3 clothing
            new float[] {0.4F,0.35F,0.85F,1F},
            //4 body, fins contrast
            new float[] {0.42F,0.32F,0.27F,1F},
            //5 body, fins
            new float[] {0.6F,0.55F,0.5F,1F},
            //6 hair contrast
            new float[] {0.15F,0.05F,-0.05F,1F},
            //7 hair
            new float[] {0.15F,0.05F,-0.05F,1F},
            //8 skin contrast
            new float[] {0.35F,0.0F,0.05F,1F},
            //9 skin
            new float[] {0.6F,0.5F,0.65F,1F},
            //10 eyes shine
            new float[] {0.7F,0.9F,0.95F,1F},
            //11 eyes
            new float[] {0.05F,0.1F,0.1F,1F},
            //12 metal contrast
            new float[] {0.9F,1.05F,1.1F,1F},
            //13 metal
            new float[] {0.7F,0.85F,0.9F,1F},
            //14 wavering solids contrast
            new float[] {0.8F,0.95F,1.05F,waver_alpha},
            //15 wavering solids
            new float[] {0.9F,1.1F,1.2F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 snow
            new float[] {0.9F,1.1F,1.2F,bordered_alpha},
            //18 yellow fire
            new float[] {0.65F,0.9F,1.3F,bordered_alpha},
            //19 orange fire
            new float[] {0.55F,0.8F,0.9F,bordered_alpha},
            //20 sparks
            new float[] {0.85F,1.2F,1.25F,bordered_alpha},
            //21 glow frame 0
            new float[] {0.6F,0.8F,0.85F,bordered_alpha},
            //22 glow frame 1
            new float[] {0.75F,0.95F,1.0F,bordered_alpha},
            //23 glow frame 2
            new float[] {0.6F,0.8F,0.85F,bordered_alpha},
            //24 glow frame 3
            new float[] {0.45F,0.65F,0.7F,bordered_alpha},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.74F,0.3F,-0.05F,fuzz_alpha},
            //29 fuzz lowlight
            new float[] {0.5F,0.2F,-0.09F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.79F,0.34F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.83F,0.38F,0.05F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.9F,0.45F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {1.05F,0.65F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.4F,0.55F,0.6F,1F},
            //35 glass
            new float[] {0.5F,0.65F,0.7F,bordered_alpha},
            //36 moving debris contrast, even frames 
            new float[] {0.7F,0.2F,0.2F,spin_alpha_0},
            //37 moving debris, even frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_0},
            //38 moving debris contrast, odd frames
            new float[] {0.7F,0.2F,0.2F,spin_alpha_1},
            //39 moving debris, odd frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_1},
            //40 flickering phantom            
            new float[] {0.7F,0.8F,0.8F,spin_alpha_0},
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
            
            new float[][] { //27 eidolon (electric)
            //0 feet contrast
            new float[] {0.25F,0.2F,0.15F,1F},
            //1 feet
            new float[] {0.35F,0.3F,0.25F,1F},
            //2 clothing contrast
            new float[] {0.5F,0.25F,0.75F,1F},
            //3 clothing
            new float[] {0.4F,0.35F,0.85F,1F},
            //4 body, fins contrast
            new float[] {0.42F,0.32F,0.27F,1F},
            //5 body, fins
            new float[] {0.6F,0.55F,0.5F,1F},
            //6 hair contrast
            new float[] {0.15F,0.05F,-0.05F,1F},
            //7 hair
            new float[] {0.15F,0.05F,-0.05F,1F},
            //8 skin contrast
            new float[] {0.35F,0.0F,0.05F,1F},
            //9 skin
            new float[] {0.6F,0.5F,0.65F,1F},
            //10 eyes shine
            new float[] {0.7F,0.9F,0.95F,1F},
            //11 eyes
            new float[] {0.05F,0.1F,0.1F,1F},
            //12 metal contrast
            new float[] {0.9F,1.05F,1.1F,1F},
            //13 metal
            new float[] {0.7F,0.85F,0.9F,1F},
            //14 wavering solids contrast
            new float[] {0.8F,0.95F,1.05F,waver_alpha},
            //15 wavering solids
            new float[] {0.9F,1.1F,1.2F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.8F,0.8F,0.6F,bordered_alpha},
            //18 yellow fire
            new float[] {0.95F,0.92F,0.2F,bordered_alpha},
            //19 orange fire
            new float[] {0.85F,0.95F,0.35F,bordered_alpha},
            //20 sparks
            new float[] {0.95F,0.95F,0.7F,bordered_alpha},
            //21 glow frame 0
            new float[] {0.95F,0.95F,0.45F,bordered_alpha},
            //22 glow frame 1
            new float[] {0.8F,0.8F,0.8F,bordered_alpha},
            //23 glow frame 2
            new float[] {0.85F,0.5F,0.95F,bordered_alpha},
            //24 glow frame 3
            new float[] {0.8F,0.8F,0.8F,bordered_alpha},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,bordered_alpha},
            //28 fuzz deepest
            new float[] {0.74F,0.3F,-0.05F,fuzz_alpha},
            //29 fuzz lowlight
            new float[] {0.5F,0.2F,-0.09F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.79F,0.34F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.83F,0.38F,0.05F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.9F,0.45F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {1.05F,0.65F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.4F,0.55F,0.6F,1F},
            //35 glass
            new float[] {0.8F,0.8F,0.55F,bordered_alpha},
            //36 moving debris contrast, even frames 
            new float[] {0.7F,0.2F,0.2F,spin_alpha_0},
            //37 moving debris, even frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_0},
            //38 moving debris contrast, odd frames
            new float[] {0.7F,0.2F,0.2F,spin_alpha_1},
            //39 moving debris, odd frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_1},
            //40 flickering sparks
            new float[] {0.95F,0.95F,0.7F,spin_alpha_0},
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
            
            new float[][] { //28 eidolon (water)
            //0 feet contrast
            new float[] {0.25F,0.2F,0.15F,1F},
            //1 feet
            new float[] {0.35F,0.3F,0.25F,1F},
            //2 clothing contrast
            new float[] {0.5F,0.25F,0.75F,1F},
            //3 clothing
            new float[] {0.4F,0.35F,0.85F,1F},
            //4 body, fins contrast
            new float[] {0.42F,0.32F,0.27F,1F},
            //5 body, fins
            new float[] {0.6F,0.55F,0.5F,1F},
            //6 hair contrast
            new float[] {0.15F,0.05F,-0.05F,1F},
            //7 hair
            new float[] {0.15F,0.05F,-0.05F,1F},
            //8 skin contrast
            new float[] {0.35F,0.0F,0.05F,1F},
            //9 skin
            new float[] {0.6F,0.5F,0.65F,1F},
            //10 eyes shine
            new float[] {0.7F,0.9F,0.95F,1F},
            //11 eyes
            new float[] {0.05F,0.1F,0.1F,1F},
            //12 metal contrast
            new float[] {0.9F,1.05F,1.1F,1F},
            //13 metal
            new float[] {0.7F,0.85F,0.9F,1F},
            //14 wavering solids contrast
            new float[] {0.8F,0.95F,1.05F,waver_alpha},
            //15 wavering solids
            new float[] {0.9F,1.1F,1.2F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.2F,0.55F,0.7F,bordered_alpha},
            //18 yellow fire
            new float[] {0.25F,0.65F,0.8F,bordered_alpha},
            //19 orange fire
            new float[] {0.2F,0.8F,0.75F,bordered_alpha},
            //20 sparks
            new float[] {0.6F,0.85F,0.9F,bordered_alpha},
            //21 glow frame 0
            new float[] {0.15F,0.4F,0.65F,bordered_alpha},
            //22 glow frame 1
            new float[] {0.25F,0.55F,0.8F,bordered_alpha},
            //23 glow frame 2
            new float[] {0.15F,0.4F,0.65F,bordered_alpha},
            //24 glow frame 3
            new float[] {0.05F,0.25F,0.5F,bordered_alpha},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,bordered_alpha},
            //28 fuzz deepest
            new float[] {0.74F,0.3F,-0.05F,fuzz_alpha},
            //29 fuzz lowlight
            new float[] {0.5F,0.2F,-0.09F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.79F,0.34F,0.0F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.83F,0.38F,0.05F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.9F,0.45F,0.1F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {1.05F,0.65F,0.15F,fuzz_alpha},
            //34 gore
            new float[] {0.4F,0.55F,0.6F,1F},
            //35 glass
            new float[] {0.15F,0.45F,0.6F,bordered_alpha},
            //36 moving debris contrast, even frames 
            new float[] {0.7F,0.2F,0.2F,spin_alpha_0},
            //37 moving debris, even frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_0},
            //38 moving debris contrast, odd frames
            new float[] {0.7F,0.2F,0.2F,spin_alpha_1},
            //39 moving debris, odd frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_1},
            //40 flickering phantom            
            new float[] {0.7F,0.8F,0.8F,spin_alpha_0},
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
            
            new float[][] { //29 eidolon (air)
            //0 feet contrast
            new float[] {0.25F,0.2F,0.15F,1F},
            //1 feet
            new float[] {0.35F,0.3F,0.25F,1F},
            //2 clothing contrast
            new float[] {0.5F,0.25F,0.75F,1F},
            //3 clothing
            new float[] {0.4F,0.35F,0.85F,1F},
            //4 body, fins contrast
            new float[] {0.42F,0.32F,0.27F,1F},
            //5 body, fins
            new float[] {0.6F,0.55F,0.5F,1F},
            //6 hair contrast
            new float[] {0.15F,0.05F,-0.05F,1F},
            //7 hair
            new float[] {0.15F,0.05F,-0.05F,1F},
            //8 skin contrast
            new float[] {0.35F,0.0F,0.05F,1F},
            //9 skin
            new float[] {0.6F,0.5F,0.65F,1F},
            //10 eyes shine
            new float[] {0.7F,0.9F,0.95F,1F},
            //11 eyes
            new float[] {0.05F,0.1F,0.1F,1F},
            //12 metal contrast
            new float[] {0.9F,1.05F,1.1F,1F},
            //13 metal
            new float[] {0.7F,0.85F,0.9F,1F},
            //14 wavering solids contrast
            new float[] {0.8F,0.95F,1.05F,waver_alpha},
            //15 wavering solids
            new float[] {0.9F,1.1F,1.2F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.7F,0.75F,0.8F,bordered_alpha},
            //18 yellow fire
            new float[] {0.4F,0.65F,0.9F,bordered_alpha},
            //19 orange fire
            new float[] {0.6F,0.7F,0.8F,bordered_alpha},
            //20 sparks
            new float[] {0.6F,0.65F,0.7F,bordered_alpha},
            //21 glow frame 0
            new float[] {0.35F,0.5F,0.8F,bordered_alpha},
            //22 glow frame 1
            new float[] {0.5F,0.65F,0.95F,bordered_alpha},
            //23 glow frame 2
            new float[] {0.35F,0.5F,0.8F,bordered_alpha},
            //24 glow frame 3
            new float[] {0.2F,0.35F,0.65F,bordered_alpha},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.45F,0.25F,0.15F,bordered_flat_alpha},
            //27 water
            new float[] {0.4F,0.6F,0.9F,bordered_alpha},
            //28 earth deepest
            new float[] {0.3F,0.1F,0.0F,fuzz_alpha},
            //29 earth lowlight
            new float[] {0.4F,0.2F,0.07F,fuzz_alpha},
            //30 earth mid-deep
            new float[] {0.5F,0.32F,0.14F,fuzz_alpha},
            //31 earth mid-light
            new float[] {0.6F,0.44F,0.22F,fuzz_alpha},
            //32 earth light
            new float[] {0.7F,0.56F,0.3F,fuzz_alpha},
            //33 earth lightest
            new float[] {0.85F,0.7F,0.41F,fuzz_alpha},
            //34 gore
            new float[] {0.4F,0.55F,0.6F,1F},
            //35 glass
            new float[] {0.25F,0.45F,0.2F,bordered_alpha},
            //36 moving debris contrast, even frames 
            new float[] {0.15F,0.2F,0.2F,spin_alpha_0},
            //37 moving debris, even frames
            new float[] {0.45F,0.55F,0.55F,spin_alpha_0},
            //38 moving debris contrast, odd frames
            new float[] {0.15F,0.2F,0.2F,spin_alpha_1},
            //39 moving debris, odd frames
            new float[] {0.45F,0.55F,0.55F,spin_alpha_1},
            //40 flickering phantom
            new float[] {0.6F,0.65F,0.7F,bordered_alpha},
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
            
            new float[][] { //30 eidolon (earth)
            //0 feet contrast
            new float[] {0.25F,0.2F,0.15F,1F},
            //1 feet
            new float[] {0.35F,0.3F,0.25F,1F},
            //2 clothing contrast
            new float[] {0.5F,0.25F,0.75F,1F},
            //3 clothing
            new float[] {0.4F,0.35F,0.85F,1F},
            //4 body, fins contrast
            new float[] {0.42F,0.32F,0.27F,1F},
            //5 body, fins
            new float[] {0.6F,0.55F,0.5F,1F},
            //6 hair contrast
            new float[] {0.15F,0.05F,-0.05F,1F},
            //7 hair
            new float[] {0.15F,0.05F,-0.05F,1F},
            //8 skin contrast
            new float[] {0.35F,0.0F,0.05F,1F},
            //9 skin
            new float[] {0.6F,0.5F,0.65F,1F},
            //10 eyes shine
            new float[] {0.7F,0.9F,0.95F,1F},
            //11 eyes
            new float[] {0.05F,0.1F,0.1F,1F},
            //12 metal contrast
            new float[] {0.9F,1.05F,1.1F,1F},
            //13 metal
            new float[] {0.7F,0.85F,0.9F,1F},
            //14 wavering solids contrast
            new float[] {0.8F,0.95F,1.05F,waver_alpha},
            //15 wavering solids
            new float[] {0.9F,1.1F,1.2F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.17F,0.1F,-0.03F,bordered_alpha},
            //18 yellow fire
            new float[] {0.3F,0.2F,0.08F,bordered_alpha},
            //19 orange fire
            new float[] {0.4F,0.33F,0.2F,bordered_alpha},
            //20 sparks
            new float[] {0.56F,0.48F,0.35F,bordered_alpha},
            //21 glow frame 0
            new float[] {0.35F,0.8F,0.3F,bordered_alpha},
            //22 glow frame 1
            new float[] {0.45F,0.95F,0.35F,bordered_alpha},
            //23 glow frame 2
            new float[] {0.35F,0.8F,0.3F,bordered_alpha},
            //24 glow frame 3
            new float[] {0.25F,0.65F,0.25F,bordered_alpha},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.45F,0.25F,0.15F,bordered_flat_alpha},
            //27 water
            new float[] {0.4F,0.6F,0.9F,bordered_alpha},
            //28 earth deepest
            new float[] {0.3F,0.1F,0.0F,fuzz_alpha},
            //29 earth lowlight
            new float[] {0.4F,0.2F,0.07F,fuzz_alpha},
            //30 earth mid-deep
            new float[] {0.5F,0.32F,0.14F,fuzz_alpha},
            //31 earth mid-light
            new float[] {0.6F,0.44F,0.22F,fuzz_alpha},
            //32 earth light
            new float[] {0.7F,0.56F,0.3F,fuzz_alpha},
            //33 earth lightest
            new float[] {0.85F,0.7F,0.41F,fuzz_alpha},
            //34 gore
            new float[] {0.4F,0.55F,0.6F,1F},
            //35 glass
            new float[] {0.25F,0.45F,0.2F,bordered_alpha},
            //36 moving debris contrast, even frames 
            new float[] {0.7F,0.2F,0.2F,spin_alpha_0},
            //37 moving debris, even frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_0},
            //38 moving debris contrast, odd frames
            new float[] {0.7F,0.2F,0.2F,spin_alpha_1},
            //39 moving debris, odd frames
            new float[] {0.65F,0.65F,0.65F,spin_alpha_1},
            //40 flickering phantom            
            new float[] {0.7F,0.8F,0.8F,spin_alpha_0},
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
            
            new float[][] { //31 eidolon (time)
            //0 feet contrast
            new float[] {0.25F,0.2F,0.15F,1F},
            //1 feet
            new float[] {0.35F,0.3F,0.25F,1F},
            //2 clothing contrast
            new float[] {0.5F,0.25F,0.75F,1F},
            //3 clothing
            new float[] {0.4F,0.35F,0.85F,1F},
            //4 body, fins contrast
            new float[] {0.42F,0.32F,0.27F,1F},
            //5 body, fins
            new float[] {0.6F,0.55F,0.5F,1F},
            //6 hair contrast
            new float[] {0.15F,0.05F,-0.05F,1F},
            //7 hair
            new float[] {0.15F,0.05F,-0.05F,1F},
            //8 skin contrast
            new float[] {0.35F,0.0F,0.05F,1F},
            //9 skin
            new float[] {0.6F,0.5F,0.65F,1F},
            //10 eyes shine
            new float[] {0.7F,0.9F,0.95F,1F},
            //11 eyes
            new float[] {0.05F,0.1F,0.1F,1F},
            //12 metal contrast
            new float[] {0.9F,1.05F,1.1F,1F},
            //13 metal
            new float[] {0.7F,0.85F,0.9F,1F},
            //14 wavering solids contrast
            new float[] {0.8F,0.95F,1.05F,waver_alpha},
            //15 wavering solids
            new float[] {0.9F,1.1F,1.2F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.92F,0.86F,0.75F,bordered_alpha},
            //18 yellow fire
            new float[] {0.95F,0.9F,0.65F,bordered_alpha},
            //19 orange fire
            new float[] {0.8F,0.8F,0.8F,bordered_alpha},
            //20 sparks
            new float[] {1.0F,0.84F,0.54F,bordered_alpha},
            //21 glow frame 0
            new float[] {0.85F,0.8F,0.55F,bordered_alpha},
            //22 glow frame 1
            new float[] {1.0F,0.95F,0.6F,bordered_alpha},
            //23 glow frame 2
            new float[] {0.85F,0.8F,0.55F,bordered_alpha},
            //24 glow frame 3
            new float[] {0.7F,0.65F,0.5F,bordered_alpha},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.45F,0.25F,0.15F,bordered_flat_alpha},
            //27 water
            new float[] {0.4F,0.6F,0.9F,bordered_alpha},
            //28 earth deepest
            new float[] {0.3F,0.1F,0.0F,fuzz_alpha},
            //29 earth lowlight
            new float[] {0.4F,0.2F,0.07F,fuzz_alpha},
            //30 earth mid-deep
            new float[] {0.5F,0.32F,0.14F,fuzz_alpha},
            //31 earth mid-light
            new float[] {0.6F,0.44F,0.22F,fuzz_alpha},
            //32 earth light
            new float[] {0.7F,0.56F,0.3F,fuzz_alpha},
            //33 earth lightest
            new float[] {0.85F,0.7F,0.41F,fuzz_alpha},
            //34 gore
            new float[] {0.4F,0.55F,0.6F,1F},
            //35 glass
            new float[] {0.75F,0.7F,0.5F,bordered_alpha},
            //36 moving debris contrast, even frames 
            new float[] {0.15F,0.2F,0.2F,spin_alpha_0},
            //37 moving debris, even frames
            new float[] {0.45F,0.55F,0.55F,spin_alpha_0},
            //38 moving debris contrast, odd frames
            new float[] {0.15F,0.2F,0.2F,spin_alpha_1},
            //39 moving debris, odd frames
            new float[] {0.45F,0.55F,0.55F,spin_alpha_1},
            //40 flickering phantom
            new float[] {0.98F,0.82F,0.5F,bordered_alpha},
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
            
            new float[][] { //32 eidolon (space)
            //0 feet contrast
            new float[] {0.25F,0.2F,0.15F,1F},
            //1 feet
            new float[] {0.35F,0.3F,0.25F,1F},
            //2 clothing contrast
            new float[] {0.5F,0.25F,0.75F,1F},
            //3 clothing
            new float[] {0.4F,0.35F,0.85F,1F},
            //4 body, fins contrast
            new float[] {0.42F,0.32F,0.27F,1F},
            //5 body, fins
            new float[] {0.6F,0.55F,0.5F,1F},
            //6 hair contrast
            new float[] {0.15F,0.05F,-0.05F,1F},
            //7 hair
            new float[] {0.15F,0.05F,-0.05F,1F},
            //8 skin contrast
            new float[] {0.35F,0.0F,0.05F,1F},
            //9 skin
            new float[] {0.6F,0.5F,0.65F,1F},
            //10 eyes shine
            new float[] {0.7F,0.9F,0.95F,1F},
            //11 eyes
            new float[] {0.05F,0.1F,0.1F,1F},
            //12 metal contrast
            new float[] {0.9F,1.05F,1.1F,1F},
            //13 metal
            new float[] {0.7F,0.85F,0.9F,1F},
            //14 wavering solids contrast
            new float[] {0.8F,0.95F,1.05F,waver_alpha},
            //15 wavering solids
            new float[] {0.9F,1.1F,1.2F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.0F,-0.03F,-0.09F,bordered_alpha},
            //18 yellow fire
            new float[] {0.15F,0.15F,0.15F,bordered_alpha},
            //19 orange fire
            new float[] {0.0F,0.0F,0.0F,bordered_alpha},
            //20 sparks
            new float[] {0.75F,0.2F,0.9F,bordered_alpha},
            //21 glow frame 0
            new float[] {0.7F,0.7F,0.7F,bordered_alpha},
            //22 glow frame 1
            new float[] {1.1F,1.1F,1.1F,bordered_alpha},
            //23 glow frame 2
            new float[] {0.7F,0.7F,0.7F,bordered_alpha},
            //24 glow frame 3
            new float[] {0.3F,0.3F,0.3F,bordered_alpha},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.45F,0.25F,0.15F,bordered_flat_alpha},
            //27 water
            new float[] {0.1F,0.1F,0.1F,bordered_alpha},
            //28 earth deepest
            new float[] {0.3F,0.1F,0.0F,fuzz_alpha},
            //29 earth lowlight
            new float[] {0.4F,0.2F,0.07F,fuzz_alpha},
            //30 earth mid-deep
            new float[] {0.5F,0.32F,0.14F,fuzz_alpha},
            //31 earth mid-light
            new float[] {0.6F,0.44F,0.22F,fuzz_alpha},
            //32 earth light
            new float[] {0.7F,0.56F,0.3F,fuzz_alpha},
            //33 earth lightest
            new float[] {0.85F,0.7F,0.41F,fuzz_alpha},
            //34 gore
            new float[] {0.4F,0.55F,0.6F,1F},
            //35 glass
            new float[] {0.05F,0.05F,0.05F,bordered_alpha},
            //36 moving debris contrast, even frames 
            new float[] {0.15F,0.2F,0.2F,spin_alpha_0},
            //37 moving debris, even frames
            new float[] {0.45F,0.55F,0.55F,spin_alpha_0},
            //38 moving debris contrast, odd frames
            new float[] {0.15F,0.2F,0.2F,spin_alpha_1},
            //39 moving debris, odd frames
            new float[] {0.45F,0.55F,0.55F,spin_alpha_1},
            //40 flickering stars
            new float[] {1.2F,1.1F,0.85F,bordered_alpha},
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
            
            new float[][] { //33 robot stealth paint
            //0 shoes, boots, brown leather contrast
            new float[] {0.22F,0.05F,-0.05F,1F},
            //1 shoes, boots, brown leather
            new float[] {0.28F,0.12F,0.0F,1F},
            //2 pants, jeans contrast
            new float[] {0F,0F,0F,1F},
            //3 pants, jeans
            new float[] {0.2F,0.2F,0.2F,1F},
            //4 shirt contrast
            new float[] {0.1F,0.1F,0.15F,1F},
            //5 shirt
            new float[] {0.25F,0.25F,0.3F,1F},
            //6 wires contrast
            new float[] {0.6F,0.52F,0.25F,1F},
            //7 wires
            new float[] {0.8F,0.7F,0.45F,1F},
            //8 skin contrast
            new float[] {0.82F,0.5F,0.1F,1F},
            //9 skin
            new float[] {0.89F,0.69F,0.32F,1F},
            //10 eyes shine
            new float[] {1.4F,0.3F,0.3F,1F},
            //11 eyes
            new float[] {0.9F,0.1F,0.1F,1F},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.1F,0.1F,0.15F,waver_alpha},
            //15 flowing clothes
            new float[] {0.25F,0.25F,0.3F,waver_alpha},
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
            new float[] {0.8F,0.2F,0.2F,1F},
            //22 glow frame 1
            new float[] {1.1F,0.35F,0.35F,1F},
            //23 glow frame 2
            new float[] {0.8F,0.2F,0.2F,1F},
            //24 glow frame 3
            new float[] {0.5F,0.05F,0.05F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 bright color
            new float[] {1.1F,0.3F,0.2F,1F},
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
            new float[] {0.65F,0.1F,0.0F,1F},
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
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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

            new float[][] { //34 robot construction paint
            //0 tires, treads contrast
            new float[] {0.2F,0.2F,0.15F,1F},
            //1 tires, treads
            new float[] {0.3F,0.3F,0.25F,1F},
            //2 main paint contrast
            new float[] {0.65F,0.5F,0.1F,1F},
            //3 main paint
            new float[] {0.8F,0.65F,0.2F,1F},
            //4 odd paint contrast
            new float[] {0.8F,0.3F,0.0F,1F},
            //5 odd paint
            new float[] {0.9F,0.45F,0.05F,1F},
            //6 wires contrast
            new float[] {0.6F,0.52F,0.25F,1F},
            //7 wires
            new float[] {0.8F,0.7F,0.45F,1F},
            //8 skin contrast
            new float[] {0.82F,0.5F,0.1F,1F},
            //9 skin
            new float[] {0.89F,0.69F,0.32F,1F},
            //10 eyes shine
            new float[] {1.1F,1.1F,1.4F,1F},
            //11 eyes
            new float[] {0.25F,0.25F,0.35F,1F},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 flowing clothes contrast
            new float[] {0.8F,0.3F,0.0F,waver_alpha},
            //15 flowing clothes
            new float[] {0.9F,0.45F,0.05F,waver_alpha},
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
            new float[] {1.3F,0.6F,0.2F,1F},
            //22 glow frame 1
            new float[] {1.5F,0.75F,0.35F,1F},
            //23 glow frame 2
            new float[] {1.3F,0.6F,0.2F,1F},
            //24 glow frame 3
            new float[] {1.1F,0.45F,0.05F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 bright color
            new float[] {1.1F,0.3F,0.2F,1F},
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
            new float[] {0.65F,0.1F,0.0F,1F},
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
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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
            
            new float[][] { //35 (47) grass and plants
            //0 dry bark contrast
            new float[] {0.25F,0.13F,0.03F,grain_some_alpha},
            //1 dry bark
            new float[] {0.45F,0.33F,0.15F,grain_some_alpha},
            //2 green leaves contrast
            new float[] {0.05F,0.35F,0.0F,1F},
            //3 green leaves
            new float[] {0.15F,0.55F,0.1F,1F},
            //4 grassy ground contrast
            new float[] {0.22F,0.6F,0.05F,1F},
            //5 grassy ground
            new float[] {0.35F,0.75F,0.15F,1F},
            //6 dead leaves contrast
            new float[] {0.7F,0.57F,0.45F,1F},
            //7 dead leaves
            new float[] {0.78F,0.7F,0.55F,1F},
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
            new float[] {0.0F,0.25F,-0.05F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.05F,0.35F,0.0F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.1F,0.45F,0.05F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.15F,0.55F,0.1F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.2F,0.65F,0.15F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.25F,0.75F,0.2F,fuzz_alpha},
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
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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

            new float[][] { //36 (48) stones
            //0 stone contrast
            new float[] {0.45F,0.4F,0.35F,grain_mild_alpha},
            //1 stone
            new float[] {0.65F,0.6F,0.55F,grain_mild_alpha},
            //2 mud/dirt contrast
            new float[] {0.55F,0.43F,0.32F,1F},
            //3 mud/dirt
            new float[] {0.7F,0.55F,0.45F,1F},
            //4 grassy ground contrast
            new float[] {0.22F,0.6F,0.05F,1F},
            //5 grassy ground
            new float[] {0.35F,0.75F,0.15F,1F},
            //6 dead leaves contrast
            new float[] {0.7F,0.57F,0.45F,1F},
            //7 dead leaves
            new float[] {0.78F,0.7F,0.55F,1F},
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
            new float[] {0.0F,0.25F,-0.05F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.05F,0.35F,0.0F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.1F,0.45F,0.05F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.15F,0.55F,0.1F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.2F,0.65F,0.15F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.25F,0.75F,0.2F,fuzz_alpha},
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
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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
            
            new float[][] { //37 (49) primitive houses
            //0 wooden wall contrast
            new float[] {0.25F,0.15F,0.05F,grain_some_alpha},
            //1 wooden wall
            new float[] {0.45F,0.35F,0.15F,grain_some_alpha},
            //2 new lumber contrast
            new float[] {0.85F,0.65F,0.3F,grain_some_alpha},
            //3 new lumber
            new float[] {0.95F,0.8F,0.5F,grain_some_alpha},
            //4 grassy ground contrast
            new float[] {0.22F,0.6F,0.05F,1F},
            //5 grassy ground
            new float[] {0.35F,0.75F,0.15F,1F},
            //6 dead leaves contrast
            new float[] {0.7F,0.57F,0.45F,1F},
            //7 dead leaves
            new float[] {0.78F,0.7F,0.55F,1F},
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
            new float[] {0.5F,0.45F,0.3F,fuzz_alpha},
            //29 fuzz deep            
            new float[] {0.6F,0.55F,0.39F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.7F,0.65F,0.43F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.8F,0.75F,0.47F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.9F,0.85F,0.51F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.95F,0.9F,0.55F,fuzz_alpha},
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
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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
            

            new float[][] { //38 biohazard barrel
            //0 rims
            new float[] {0.5F,0.5F,0.4F,1F},
            //1 contents
            new float[] {0.3F,1.15F,0.05F,1F},
            //2 markings
            new float[] {-0.05F,-0.05F,-0.05F,1F},
            //3 bright hazard paint
            new float[] {1.1F,1.1F,0.1F,1F},
            //4 grassy ground contrast
            new float[] {0.22F,0.6F,0.05F,1F},
            //5 grassy ground
            new float[] {0.35F,0.75F,0.15F,1F},
            //6 dead leaves contrast
            new float[] {0.7F,0.57F,0.45F,1F},
            //7 dead leaves
            new float[] {0.78F,0.7F,0.55F,1F},
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
            new float[] {0.5F,0.45F,0.3F,fuzz_alpha},
            //29 fuzz deep            
            new float[] {0.6F,0.55F,0.39F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.7F,0.65F,0.43F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.8F,0.75F,0.47F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.9F,0.85F,0.51F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.95F,0.9F,0.55F,fuzz_alpha},
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
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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
            
            new float[][] { //39 wooden desk (screens on)
            //0 wood contrast
            new float[] {0.25F,0.15F,0.05F,grain_mild_alpha},
            //1 wood
            new float[] {0.45F,0.35F,0.15F,grain_mild_alpha},
            //2 stained wood contrast
            new float[] {0.2F,0.08F,-0.02F,grain_some_alpha},
            //3 stained wood
            new float[] {0.35F,0.23F,0.08F,grain_some_alpha},
            //4 plastic equipment contrast
            new float[] {0.05F,0.05F,0.05F,1F},
            //5 plastic equipment
            new float[] {0.15F,0.15F,0.15F,1F},
            //6 inert organic stuff contrast
            new float[] {0.7F,0.57F,0.45F,1F},
            //7 inert organic stuff
            new float[] {0.78F,0.7F,0.55F,1F},
            //8 organic matter contrast
            new float[] {0.8F,0.5F,0.12F,1F},
            //9 organic matter
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
            new float[] {0.95F,0.95F,0.9F,1F},
            //22 glow frame 1
            new float[] {1.1F,1.1F,1.05F,1F},
            //23 glow frame 2
            new float[] {0.95F,0.95F,0.9F,1F},
            //24 glow frame 3
            new float[] {0.85F,0.85F,0.81F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.0F,0.25F,-0.05F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.05F,0.35F,0.0F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.1F,0.45F,0.05F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.15F,0.55F,0.1F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.2F,0.65F,0.15F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.25F,0.75F,0.2F,fuzz_alpha},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.2F,0.2F,0.2F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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
            
            new float[][] { //40 wooden desk (screens off)
            //0 wood contrast
            new float[] {0.25F,0.15F,0.05F,grain_mild_alpha},
            //1 wood
            new float[] {0.45F,0.35F,0.15F,grain_mild_alpha},
            //2 stained wood contrast
            new float[] {0.2F,0.08F,-0.02F,grain_some_alpha},
            //3 stained wood
            new float[] {0.35F,0.23F,0.08F,grain_some_alpha},
            //4 plastic equipment contrast
            new float[] {0.05F,0.05F,0.05F,1F},
            //5 plastic equipment
            new float[] {0.15F,0.15F,0.15F,1F},
            //6 inert organic stuff contrast
            new float[] {0.7F,0.57F,0.45F,1F},
            //7 inert organic stuff
            new float[] {0.78F,0.7F,0.55F,1F},
            //8 organic matter contrast
            new float[] {0.8F,0.5F,0.12F,1F},
            //9 organic matter
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
            new float[] {0.2F,0.2F,0.2F,1F},
            //22 glow frame 1
            new float[] {0.2F,0.2F,0.2F,1F},
            //23 glow frame 2
            new float[] {0.2F,0.2F,0.2F,1F},
            //24 glow frame 3
            new float[] {0.2F,0.2F,0.2F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.0F,0.25F,-0.05F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.05F,0.35F,0.0F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.1F,0.45F,0.05F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.15F,0.55F,0.1F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.2F,0.65F,0.15F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.25F,0.75F,0.2F,fuzz_alpha},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.2F,0.2F,0.2F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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
            
            new float[][] { //41 office desk (screens on)
            //0 dark plastic contrast
            new float[] {0.2F,0.2F,0.2F,1F},
            //1 dark plastic
            new float[] {0.35F,0.35F,0.35F,1F},
            //2 light plastic contrast
            new float[] {0.7F,0.7F,0.7F,1F},
            //3 light plastic
            new float[] {0.8F,0.8F,0.8F,1F},
            //4 plastic equipment contrast
            new float[] {0.6F,0.6F,0.65F,1F},
            //5 plastic equipment
            new float[] {0.7F,0.7F,0.75F,1F},
            //6 inert organic stuff contrast
            new float[] {0.7F,0.57F,0.45F,1F},
            //7 inert organic stuff
            new float[] {0.78F,0.7F,0.55F,1F},
            //8 organic matter contrast
            new float[] {0.8F,0.5F,0.12F,1F},
            //9 organic matter
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
            new float[] {0.95F,0.95F,0.9F,1F},
            //22 glow frame 1
            new float[] {1.1F,1.1F,1.05F,1F},
            //23 glow frame 2
            new float[] {0.95F,0.95F,0.9F,1F},
            //24 glow frame 3
            new float[] {0.85F,0.85F,0.81F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.0F,0.25F,-0.05F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.05F,0.35F,0.0F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.1F,0.45F,0.05F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.15F,0.55F,0.1F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.2F,0.65F,0.15F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.25F,0.75F,0.2F,fuzz_alpha},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.2F,0.2F,0.2F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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
            
            new float[][] { //42 office desk (screens off)
            //0 dark plastic contrast
            new float[] {0.2F,0.2F,0.2F,1F},
            //1 dark plastic
            new float[] {0.35F,0.35F,0.35F,1F},
            //2 light plastic contrast
            new float[] {0.7F,0.7F,0.7F,1F},
            //3 light plastic
            new float[] {0.8F,0.8F,0.8F,1F},
            //4 plastic equipment contrast
            new float[] {0.6F,0.6F,0.65F,1F},
            //5 plastic equipment
            new float[] {0.7F,0.7F,0.75F,1F},
            //6 inert organic stuff contrast
            new float[] {0.7F,0.57F,0.45F,1F},
            //7 inert organic stuff
            new float[] {0.78F,0.7F,0.55F,1F},
            //8 organic matter contrast
            new float[] {0.8F,0.5F,0.12F,1F},
            //9 organic matter
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
            new float[] {0.2F,0.2F,0.2F,1F},
            //22 glow frame 1
            new float[] {0.2F,0.2F,0.2F,1F},
            //23 glow frame 2
            new float[] {0.2F,0.2F,0.2F,1F},
            //24 glow frame 3
            new float[] {0.2F,0.2F,0.2F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud
            new float[] {0.2F,0.4F,0.3F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 fuzz deepest
            new float[] {0.0F,0.25F,-0.05F,fuzz_alpha},
            //29 fuzz deep
            new float[] {0.05F,0.35F,0.0F,fuzz_alpha},
            //30 fuzz mid-deep
            new float[] {0.1F,0.45F,0.05F,fuzz_alpha},
            //31 fuzz mid-light
            new float[] {0.15F,0.55F,0.1F,fuzz_alpha},
            //32 fuzz light
            new float[] {0.2F,0.65F,0.15F,fuzz_alpha},
            //33 fuzz lightest
            new float[] {0.25F,0.75F,0.2F,fuzz_alpha},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.2F,0.2F,0.2F,1F},
            //36 placeholder
            new float[] {0F,0F,0F,0F},
            //37 placeholder
            new float[] {0F,0F,0F,0F},
            //38 placeholder
            new float[] {0F,0F,0F,0F},
            //39 placeholder
            new float[] {0F,0F,0F,0F},
            //40 placeholder
            new float[] {0F,0F,0F,0F},
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
            
            new float[][] { //43 vih
            //0 claws contrast
            new float[] {0.85F,0.75F,0.45F,1F},
            //1 claws, teeth, hands, legs, chitin
            new float[] {1.0F,0.9F,0.6F,1F},
            //2 veins contrast
            new float[] {0.75F,0.55F,0.7F,1F},
            //3 veins
            new float[] {0.9F,0.7F,0.85F,1F},
            //4 skin contrast
            new float[] {1.3F,0.75F,0.45F,1F},
            //5 skin
            new float[] {1.3F,0.85F,0.6F,1F},
            //6 hand
            new float[] {0.8F,0.45F,0.2F,1F},
            //7 eyestalk
            new float[] {0.15F,0.15F,0.05F,1F},
            //8 flesh contrast
            new float[] {1.3F,0.75F,0.45F,1F},
            //9 flesh
            new float[] {1.3F,0.85F,0.6F,1F},
            //10 eyes shine
            new float[] {0.95F,0.95F,1.1F,1F},
            //11 eyes
            new float[] {0.05F,0.05F,0.15F,1F},
            //12 metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 tentacles contrast
            new float[] {1.3F,0.75F,0.45F,waver_alpha},
            //15 tentacles
            new float[] {1.3F,0.85F,0.6F,waver_alpha},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {-0.05F,-0.05F,0.4F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,0.75F,0.9F,1F},
            //19 orange fire
            new float[] {1.1F,0.55F,1.0F,1F},
            //20 sparks
            new float[] {0.8F,1.1F,0.8F,flash_alpha_0},
            //21 glow frame 0
            new float[] {1.1F,0.75F,0.4F,1F},
            //22 glow frame 1
            new float[] {1.25F,0.9F,0.55F,1F},
            //23 glow frame 2
            new float[] {1.1F,0.75F,0.4F,1F},
            //24 glow frame 3
            new float[] {0.95F,0.6F,0.25F,1F},
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
            new float[] {1.0F,0.55F,0.3F,1F},
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
            //40 flickering magic
            new float[] {0.8F,1.1F,0.8F,flash_alpha_0},
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

        public static float[][][] wterrains = new float[][][]
        {
            new float[][] { //0 (50) plains
            //terrain dark
            new float[] {0.63F,0.92F,0.3F,1F},
            //terrain mid
            new float[] {0.63F,0.92F,0.3F,1F},
            //terrain light
            new float[] {0.63F,0.92F,0.3F,1F},
            //terrain highlight
            new float[] {0.63F,0.92F,0.3F,1F},
            },
            new float[][] { //1 (51) forest
            //terrain dark
            new float[] {0.2F,0.7F,0.15F,1F},
            //terrain mid
            new float[] {0.2F,0.7F,0.15F,1F},
            //terrain light
            new float[] {0.2F,0.7F,0.15F,1F},
            //terrain highlight
            new float[] {0.2F,0.7F,0.15F,1F},
            },
            new float[][] { //2 (52) desert
            //terrain dark
            new float[] {1.05F,0.9F,0.3F,1F},
            //terrain mid
            new float[] {1.05F,0.9F,0.3F,1F},
            //terrain light
            new float[] {1.05F,0.9F,0.3F,1F},
            //terrain highlight
            new float[] {1.05F,0.9F,0.3F,1F},
            },
            new float[][] { //3 (53) jungle
            //terrain dark
            new float[] {0F,0.55F,0.35F,1F},
            //terrain mid
            new float[] {0F,0.55F,0.35F,1F},
            //terrain light
            new float[] {0F,0.55F,0.35F,1F},
            //terrain highlight
            new float[] {0F,0.55F,0.35F,1F},
            },
            new float[][] { //4 (54) hills
            //terrain dark
            new float[] {0.95F,0.7F,0.4F,1F},
            //terrain mid
            new float[] {0.95F,0.7F,0.4F,1F},
            //terrain light
            new float[] {0.95F,0.7F,0.4F,1F},
            //terrain highlight
            new float[] {0.95F,0.7F,0.4F,1F},
            },
            new float[][] { //5 (55) mountains
            //terrain dark
            new float[] {0.8F,0.83F,0.86F,1F},
            //terrain mid
            new float[] {0.8F,0.83F,0.86F,1F},
            //terrain light
            new float[] {0.8F,0.83F,0.86F,1F},
            //terrain highlight
            new float[] {0.8F,0.83F,0.86F,1F},
            },
            new float[][] { //6 (56) ruins
            //terrain dark
            new float[] {0.8F,0.45F,0.75F,1F},
            //terrain mid
            new float[] {0.8F,0.45F,0.75F,1F},
            //terrain light
            new float[] {0.8F,0.45F,0.75F,1F},
            //terrain highlight
            new float[] {0.8F,0.45F,0.75F,1F},
            },
            new float[][] { //7 (57) tundra
            //terrain dark
            new float[] {0.8F,1F,1F,1F},
            //terrain mid
            new float[] {0.8F,1F,1F,1F},
            //terrain light
            new float[] {0.8F,1F,1F,1F},
            //terrain highlight
            new float[] {0.8F,1F,1F,1F},
            },
            new float[][] { //8 (58) road
            //terrain dark
            new float[] {0.5F,0.5F,0.5F,1F},
            //terrain mid
            new float[] {0.5F,0.5F,0.5F,1F},
            //terrain light
            new float[] {0.5F,0.5F,0.5F,1F},
            //terrain highlight
            new float[] {0.5F,0.5F,0.5F,1F},
            },
            new float[][] { //9 (59) river
            //terrain dark
            new float[] {0.2F,0.4F,0.95F,1F},
            //terrain mid
            new float[] {0.2F,0.4F,0.95F,1F},
            //terrain light
            new float[] {0.2F,0.4F,0.95F,1F},
            //terrain highlight
            new float[] {0.2F,0.4F,0.95F,1F},
            },
            new float[][] { //10 (60) sea
            //terrain dark
            new float[] {0F,0.3F,0.7F,1F},
            //terrain mid
            new float[] {0F,0.3F,0.7F,1F},
            //terrain light
            new float[] {0F,0.3F,0.7F,1F},
            //terrain highlight
            new float[] {0F,0.3F,0.7F,1F},
            },            

        };

        public static void Initialize()
        {
            VoxelLogic.wpalettecount = wpalettes.Length;
            int wpc = VoxelLogic.wpalettecount;
            VoxelLogic.subtlePalettes = new int[] { 35, 36, wpc, wpc + 1, wpc + 2, wpc + 3, wpc + 4, wpc + 5, wpc + 6, wpc + 7, wpc + 8, wpc + 9, wpc + 10 };
            VoxelLogic.wcolorcount = wpalettes[0].Length;
            VoxelLogic.wcolors = wpalettes[0].Replicate();
            wpalettes = wpalettes.Concat(wpalettes[0].Repeat(wterrains.Length)).ToArray();
            for (int p = 0; p < wterrains.Length; p++)
            {
                float[][] temp = new float[wpalettes[0].Length][];
                temp[0] = wterrains[p][0];
                temp[0][0] -= 0.4f;
                temp[0][1] -= 0.4f;
                temp[0][2] -= 0.4f;

                temp[1] = wterrains[p][1];
                temp[1][0] -= 0.25f;
                temp[1][1] -= 0.25f;
                temp[1][2] -= 0.25f;
                temp[2] = wterrains[p][2];
                temp[2][0] -= 0.1f;
                temp[2][1] -= 0.1f;
                temp[2][2] -= 0.1f;

                temp[3] = wterrains[p][3];
                temp[3][0] += 0.05f;
                temp[3][1] += 0.05f;
                temp[3][2] += 0.05f;
                for (int c = 4; c < wpalettes[0].Length; c++)
                {
                    temp[c] = wpalettes[0][c];
                }
                wpalettes[VoxelLogic.wpalettecount + p] = temp;
            }

            VoxelLogic.wpalettecount = wpalettes.Length;

            VoxelLogic.clear = (byte)(253 - (VoxelLogic.wcolorcount - 1) * 4);
            for (int p = 0; p < VoxelLogic.wpalettecount; p++)
            {
                float[] drip = wpalettes[p][27].ToArray(), transp = VoxelLogic.wpalettes[p][VoxelLogic.wcolorcount - 1];
                drip[3] = 1F;
                float[] zap = wpalettes[p][40].ToArray();
                zap[3] = spin_alpha_1;
                wpalettes[p] = wpalettes[p].Concat(new float[][] { drip, transp, transp, transp, drip, zap }).ToArray();
            }
            VoxelLogic.wpalettes = wpalettes;
        }






        //new float[][] { //18 cerpali0 (crustacean)
        ////0 lower legs highlight
        //new float[] {0.45F,0.35F,0.28F,1F},
        ////1 lower legs
        //new float[] {0.55F,0.45F,0.25F,1F},
        ////2 carapace contrast
        //new float[] {0.6F,0.5F,0.35F,1F},
        ////3 carapace
        //new float[] {0.9F,0.8F,0.65F,1F},
        ////4 antennae contrast
        //new float[] {0.97F,0.88F,0.8F,1F},
        ////5 antennae
        //new float[] {0.95F,0.95F,0.95F,1F},
        ////6 hair contrast
        //new float[] {0.15F,0.05F,-0.05F,1F},
        ////7 hair
        //new float[] {0.15F,0.05F,-0.05F,1F},
        ////8 skin contrast
        //new float[] {0.55F,0.25F,-0.05F,1F},
        ////9 skin
        //new float[] {0.9F,0.55F,0.2F,1F},
        ////10 eyes shine
        //new float[] {0.8F,1.2F,0.85F,1F},
        ////11 eyes
        //new float[] {0.15F,0.6F,0.2F,1F},
        ////12 metal contrast
        //new float[] {0.65F,0.75F,1.1F,1F},
        ////13 metal
        //new float[] {0.5F,0.6F,0.75F,1F},
        ////14 flowing clothes contrast
        //new float[] {0.2F,0.0F,-0.05F,waver_alpha},
        ////15 flowing clothes
        //new float[] {0.3F,0.05F,0.0F,waver_alpha},
        ////16 inner shadow
        //new float[] {0.13F,0.10F,0.04F,1F},
        ////17 smoke
        //new float[] {0.14F,0.14F,0.02F,waver_alpha},
        ////18 yellow fire
        //new float[] {1.25F,1.1F,0.45F,1F},
        ////19 orange fire
        //new float[] {1.25F,0.7F,0.3F,1F},
        ////20 sparks
        //new float[] {1.3F,1.2F,0.85F,1F},
        ////21 glow frame 0
        //new float[] {0.95F,0.9F,0.45F,1F},
        ////22 glow frame 1
        //new float[] {1.15F,1.1F,0.65F,1F},
        ////23 glow frame 2
        //new float[] {0.95F,0.9F,0.45F,1F},
        ////24 glow frame 3
        //new float[] {0.75F,0.7F,0.25F,1F},
        ////25 shadow
        //new float[] {0.1F,0.1F,0.1F,flat_alpha},
        ////26 mud
        //new float[] {0.2F,0.4F,0.3F,1F},
        ////27 water
        //new float[] {0.4F,0.6F,0.9F,flat_alpha},
        ////28 fuzz deepest
        //new float[] {0.74F,0.3F,-0.05F,fuzz_alpha},
        ////29 fuzz lowlight
        //new float[] {0.5F,0.2F,-0.09F,fuzz_alpha},
        ////30 fuzz mid-deep
        //new float[] {0.79F,0.34F,0.0F,fuzz_alpha},
        ////31 fuzz mid-light
        //new float[] {0.83F,0.38F,0.05F,fuzz_alpha},
        ////32 fuzz light
        //new float[] {0.9F,0.45F,0.1F,fuzz_alpha},
        ////33 fuzz lightest
        //new float[] {1.05F,0.65F,0.15F,fuzz_alpha},
        ////34 gore
        //new float[] {0.9F,0.55F,0.1F,1F},
        ////35 glass
        //new float[] {0.5F,0.8F,1.1F,1F},
        ////36 placeholder
        //new float[] {0F,0F,0F,0F},
        ////37 placeholder
        //new float[] {0F,0F,0F,0F},
        ////38 placeholder
        //new float[] {0F,0F,0F,0F},
        ////39 placeholder
        //new float[] {0F,0F,0F,0F},
        ////40 placeholder
        //new float[] {0F,0F,0F,0F},
        ////41 placeholder
        //new float[] {0F,0F,0F,0F},
        ////42 placeholder
        //new float[] {0F,0F,0F,0F},
        ////43 placeholder
        //new float[] {0F,0F,0F,0F},
        ////44 placeholder
        //new float[] {0F,0F,0F,0F},
        ////45 placeholder
        //new float[] {0F,0F,0F,0F},
        ////46 placeholder
        //new float[] {0F,0F,0F,0F},
        ////47 total transparent
        //new float[] {0F,0F,0F,0F},
        //},

    }
}
