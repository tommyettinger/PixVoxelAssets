using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsPV
{
    class CUPalettes
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
        new float[][] { // 0 dark
            //0 tires, treads contrast
            new float[] {0.13F,0.1F,0.1F,1F},
            //1 tires, treads
            new float[] {0.23F,0.2F,0.2F,1F},
            //2 doors paint (dark) contrast
            new float[] {0.4F,0.0F,-0.1F,1F},
            //3 doors paint (dark)
            new float[] {0.6F,0.05F,-0.1F,1F},
            //4 main paint (mid) contrast
            new float[] {0.15F,0.15F,0.15F,1F},
            //5 main paint (mid)
            new float[] {0.3F,0.3F,0.3F,1F},
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
            //12 exposed metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 exposed metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 gun peripheral
            new float[] {0.4F,0.5F,0.4F,1F},
            //15 gun barrel
            new float[] {0.4F,0.35F,0.5F,1F},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.13F,0.07F,-0.04F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {1.0F,1.0F,0.5F,1F},
            //22 glow frame 1
            new float[] {1.15F,1.15F,0.65F,1F},
            //23 glow frame 2
            new float[] {1.0F,1.0F,0.5F,1F},
            //24 glow frame 3
            new float[] {0.9F,0.9F,0.4F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud, wood
            new float[] {0.4F,0.25F,0.15F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 cockpit paint (gray) contrast
            new float[] {0.35F,0.35F,0.25F,1F},
            //29 cockpit paint (gray)
            new float[] {0.5F,0.5F,0.4F,1F},
            //30 helmet paint (bold) contrast
            new float[] {0.1F,0.05F,0.0F,1F},
            //31 helmet paint (bold)
            new float[] {0.2F,0.15F,0.1F,1F},
            //32 alternate paint contrast
            new float[] {0.38F,0.3F,0.05F,1F},
            //33 alternate paint
            new float[] {0.48F,0.4F,0.15F,1F},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.3F,0.5F,0.5F,1F},
            //36 rotor frame 0 contrast
            new float[] {0.85F,0.4F,0.4F,spin_alpha_0},
            //37 rotor frame 0
            new float[] {0.65F,0.65F,0.65F,spin_alpha_0},
            //38 rotor frame 1 contrast
            new float[] {0.85F,0.4F,0.4F,spin_alpha_1},
            //39 rotor frame 1
            new float[] {0.65F,0.65F,0.65F,spin_alpha_1},
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
            //57 weapon 1 emitter
            new float[] {0F,0F,0F,0F},
            //58 weapon 1 trail
            new float[] {0F,0F,0F,0F},
            //59 weapon 2 emitter
            new float[] {0F,0F,0F,0F},
            //60 weapon 2 trail
            new float[] {0F,0F,0F,0F},
            //61 total transparent
            new float[] {0F,0F,0F,0F},
            },

        new float[][] { // 1 pale
            //0 tires, treads contrast
            new float[] {0.13F,0.1F,0.1F,1F},
            //1 tires, treads
            new float[] {0.23F,0.2F,0.2F,1F},
            //2 doors paint (dark) contrast
            new float[] {0.5F,0.6F,0.7F,1F},
            //3 doors paint (dark)
            new float[] {0.6F,0.7F,0.8F,1F},
            //4 main paint (mid) contrast
            new float[] {0.79F,0.76F,0.7F,1F},
            //5 main paint (mid)
            new float[] {0.89F,0.86F,0.8F,1F},
            //6 hair contrast
            new float[] {0.7F,0.9F,0.4F,1F},
            //7 hair
            new float[] {0.7F,0.9F,0.4F,1F},
            //8 skin contrast
            new float[] {0.4F,0.8F,0.35F,1F},
            //9 skin
            new float[] {0.7F,0.9F,0.4F,1F},
            //10 eyes shine
            new float[] {1.0F,1.1F,1.1F,1F},
            //11 eyes
            new float[] {0.05F,0.08F,0.12F,1F},
            //12 exposed metal contrast
            new float[] {0.85F,1.05F,1.0F,1F},
            //13 exposed metal
            new float[] {0.7F,0.9F,0.85F,1F},
            //14 gun peripheral
            new float[] {0.6F,0.8F,1F,1F},
            //15 gun barrel
            new float[] {0.3F,0.35F,0.4F,1F},
            //16 inner shadow
            new float[] {0.1F,0.1F,0.09F,1F},
            //17 smoke
            new float[] {0.01F,0.12F,0.16F,waver_alpha},
            //18 yellow fire
            new float[] {0.55F,1.3F,0.6F,1F},
            //19 orange fire
            new float[] {0.52F,1.1F,0.4F,1F},
            //20 sparks
            new float[] {0.8F,1.3F,1.15F,1F},
            //21 glow frame 0
            new float[] {0.5F,1.2F,0.6F,1F},
            //22 glow frame 1
            new float[] {0.65F,1.3F,0.8F,1F},
            //23 glow frame 2
            new float[] {0.5F,1.2F,0.6F,1F},
            //24 glow frame 3
            new float[] {0.4F,1.05F,0.45F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud, wood
            new float[] {0.4F,0.25F,0.15F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 cockpit paint (gray) contrast
            new float[] {0.53F,0.45F,0.7F,1F},
            //29 cockpit paint (gray)
            new float[] {0.68F,0.6F,0.85F,1F},
            //30 helmet paint (bold) contrast
            new float[] {0.59F,0.56F,0.5F,1F},
            //31 helmet paint (bold)
            new float[] {0.74F,0.71F,0.65F,1F},
            //32 alternate paint contrast
            new float[] {0.15F,0.75F,0.6F,1F},
            //33 alternate paint
            new float[] {0.3F,0.9F,0.75F,1F},
            //34 gore
            new float[] {0.5F,0.8F,-0.2F,1F},
            //35 glass
            new float[] {0.3F,1F,0.9F,1F},
            //36 rotor frame 0 contrast
            new float[] {0.7F,1.0F,1.0F,spin_alpha_0},
            //37 rotor frame 0
            new float[] {0.75F,0.8F,0.8F,spin_alpha_0},
            //38 rotor frame 1 contrast
            new float[] {0.7F,1.0F,1.0F,spin_alpha_1},
            //39 rotor frame 1
            new float[] {0.75F,0.8F,0.8F,spin_alpha_1},
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
            //57 weapon 1 emitter
            new float[] {0F,0F,0F,0F},
            //58 weapon 1 trail
            new float[] {0F,0F,0F,0F},
            //59 weapon 2 emitter
            new float[] {0F,0F,0F,0F},
            //60 weapon 2 trail
            new float[] {0F,0F,0F,0F},
            //61 total transparent
            new float[] {0F,0F,0F,0F},
            },

        new float[][] { // 2 red
            //0 tires, treads contrast
            new float[] {0.13F,0.1F,0.1F,1F},
            //1 tires, treads
            new float[] {0.23F,0.2F,0.2F,1F},
            //2 doors paint (dark) contrast
            new float[] {0.52F,-0.1F,-0.1F,1F},
            //3 doors paint (dark)
            new float[] {0.7F,-0.05F,-0.1F,1F},
            //4 main paint (mid) contrast
            new float[] {0.73F,0.07F,0.0F,1F},
            //5 main paint (mid)
            new float[] {0.86F,0.19F,0.1F,1F},
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
            //12 exposed metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 exposed metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 gun peripheral
            new float[] {0.4F,0.5F,0.4F,1F},
            //15 gun barrel
            new float[] {0.3F,0.35F,0.4F,1F},
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
            new float[] {1.1F,0.9F,0.5F,1F},
            //22 glow frame 1
            new float[] {1.25F,1.0F,0.65F,1F},
            //23 glow frame 2
            new float[] {1.1F,0.9F,0.5F,1F},
            //24 glow frame 3
            new float[] {1.0F,0.8F,0.4F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud, wood
            new float[] {0.4F,0.25F,0.15F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 cockpit paint (gray) contrast
            new float[] {0.4F,0.23F,0.08F,1F},
            //29 cockpit paint (gray)
            new float[] {0.55F,0.36F,0.2F,1F},
            //30 helmet paint (bold) contrast
            new float[] {0.83F,0.03F,-0.05F,1F},
            //31 helmet paint (bold)
            new float[] {1.0F,0.15F,0.05F,1F},
            //32 alternate paint contrast
            new float[] {0.95F,0.8F,0.5F,1F},
            //33 alternate paint
            new float[] {1.05F,0.95F,0.6F,1F},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.45F,0.7F,0.7F,1F},
            //36 rotor frame 0 contrast
            new float[] {0.8F,0.65F,0.25F,spin_alpha_0},
            //37 rotor frame 0
            new float[] {0.8F,0.25F,0.25F,spin_alpha_0},
            //38 rotor frame 1 contrast
            new float[] {0.8F,0.65F,0.25F,spin_alpha_1},
            //39 rotor frame 1
            new float[] {0.8F,0.25F,0.25F,spin_alpha_1},
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
            //57 weapon 1 emitter
            new float[] {0F,0F,0F,0F},
            //58 weapon 1 trail
            new float[] {0F,0F,0F,0F},
            //59 weapon 2 emitter
            new float[] {0F,0F,0F,0F},
            //60 weapon 2 trail
            new float[] {0F,0F,0F,0F},
            //61 total transparent
            new float[] {0F,0F,0F,0F},
            },

        new float[][] { // 3 orange
            //0 tires, treads contrast
            new float[] {0.13F,0.1F,0.1F,1F},
            //1 tires, treads
            new float[] {0.23F,0.2F,0.2F,1F},
            //2 doors paint (dark) contrast
            new float[] {0.55F,0.1F,-0.1F,1F},
            //3 doors paint (dark)
            new float[] {0.7F,0.25F,0.05F,1F},
            //4 main paint (mid) contrast
            new float[] {0.95F,0.3F,-0.1F,1F},
            //5 main paint (mid)
            new float[] {1.1F,0.45F,0F,1F},
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
            //12 exposed metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 exposed metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 gun peripheral
            new float[] {0.4F,0.5F,0.4F,1F},
            //15 gun barrel
            new float[] {0.3F,0.35F,0.4F,1F},
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
            new float[] {1.1F,0.9F,0.5F,1F},
            //22 glow frame 1
            new float[] {1.25F,1.0F,0.65F,1F},
            //23 glow frame 2
            new float[] {1.1F,0.9F,0.5F,1F},
            //24 glow frame 3
            new float[] {1.0F,0.8F,0.4F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud, wood
            new float[] {0.4F,0.25F,0.15F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 cockpit paint (gray) contrast
            new float[] {0.72F,0.38F,0.1F,1F},
            //29 cockpit paint (gray)
            new float[] {0.87F,0.5F,0.2F,1F},
            //30 helmet paint (bold) contrast
            new float[] {0.8F,0.25F,-0.08F,1F},
            //31 helmet paint (bold)
            new float[] {0.95F,0.35F,0.05F,1F},
            //32 alternate paint contrast
            new float[] {0.95F,0.8F,0.5F,1F},
            //33 alternate paint
            new float[] {1.05F,0.95F,0.6F,1F},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.45F,0.7F,0.7F,1F},
            //36 rotor frame 0 contrast
            new float[] {0.6F,0.6F,0.6F,spin_alpha_0},
            //37 rotor frame 0
            new float[] {0.8F,0.5F,0.1F,spin_alpha_0},
            //38 rotor frame 1 contrast
            new float[] {0.6F,0.6F,0.6F,spin_alpha_1},
            //39 rotor frame 1
            new float[] {0.8F,0.5F,0.1F,spin_alpha_1},
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
            //57 weapon 1 emitter
            new float[] {0F,0F,0F,0F},
            //58 weapon 1 trail
            new float[] {0F,0F,0F,0F},
            //59 weapon 2 emitter
            new float[] {0F,0F,0F,0F},
            //60 weapon 2 trail
            new float[] {0F,0F,0F,0F},
            //61 total transparent
            new float[] {0F,0F,0F,0F},
            },

        new float[][] { // 4 yellow
            //0 tires, treads contrast
            new float[] {0.13F,0.1F,0.1F,1F},
            //1 tires, treads
            new float[] {0.23F,0.2F,0.2F,1F},
            //2 doors paint (dark) contrast
            new float[] {0.5F,0.4F,-0.15F,1F},
            //3 doors paint (dark)
            new float[] {0.63F,0.51F,-0.15F,1F},
            //4 main paint (mid) contrast
            new float[] {0.7F,0.65F,0.1F,1F},
            //5 main paint (mid)
            new float[] {0.85F,0.75F,0.15F,1F},
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
            //12 exposed metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 exposed metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 gun peripheral
            new float[] {0.4F,0.5F,0.4F,1F},
            //15 gun barrel
            new float[] {0.3F,0.35F,0.4F,1F},
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
            new float[] {1.05F,0.65F,0.15F,1F},
            //22 glow frame 1
            new float[] {1.1F,0.85F,0.3F,1F},
            //23 glow frame 2
            new float[] {1.05F,0.65F,0.15F,1F},
            //24 glow frame 3
            new float[] {0.95F,0.55F,0.0F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud, wood
            new float[] {0.4F,0.25F,0.15F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 cockpit paint (gray) contrast
            new float[] {0.55F,0.5F,0.2F,1F},
            //29 cockpit paint (gray)
            new float[] {0.68F,0.61F,0.3F,1F},
            //30 helmet paint (bold) contrast
            new float[] {0.55F,0.45F,0.1F,1F},
            //31 helmet paint (bold)
            new float[] {0.68F,0.55F,0.2F,1F},
            //32 alternate paint contrast
            new float[] {-0.1F,-0.1F,0.25F,1F},
            //33 alternate paint
            new float[] {0.0F,0.0F,0.4F,1F},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.35F,0.3F,0.25F,1F},
            //36 rotor frame 0 contrast
            new float[] {0.0F,0.0F,0.4F,spin_alpha_0},
            //37 rotor frame 0
            new float[] {0.85F,0.85F,0.35F,spin_alpha_0},
            //38 rotor frame 1 contrast
            new float[] {0.0F,0.0F,0.4F,spin_alpha_1},
            //39 rotor frame 1
            new float[] {0.85F,0.85F,0.35F,spin_alpha_1},
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
            //57 weapon 1 emitter
            new float[] {0F,0F,0F,0F},
            //58 weapon 1 trail
            new float[] {0F,0F,0F,0F},
            //59 weapon 2 emitter
            new float[] {0F,0F,0F,0F},
            //60 weapon 2 trail
            new float[] {0F,0F,0F,0F},
            //61 total transparent
            new float[] {0F,0F,0F,0F},
            },

        new float[][] { // 5 green
            //0 tires, treads contrast
            new float[] {0.13F,0.1F,0.1F,1F},
            //1 tires, treads
            new float[] {0.23F,0.2F,0.2F,1F},
            //2 doors paint (dark) contrast
            new float[] {-0.05F,0.32F,-0.1F,1F},
            //3 doors paint (dark)
            new float[] {0.0F,0.45F,-0.1F,1F},
            //4 main paint (mid) contrast
            new float[] {0.07F,0.59F,-0.05F,1F},
            //5 main paint (mid)
            new float[] {0.2F,0.7F,0.1F,1F},
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
            //12 exposed metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 exposed metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 gun peripheral
            new float[] {0.4F,0.5F,0.4F,1F},
            //15 gun barrel
            new float[] {0.3F,0.35F,0.4F,1F},
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
            new float[] {0.9F,1.1F,0.5F,1F},
            //22 glow frame 1
            new float[] {1.05F,1.25F,0.65F,1F},
            //23 glow frame 2
            new float[] {0.9F,1.1F,0.5F,1F},
            //24 glow frame 3
            new float[] {0.77F,1.0F,0.38F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud, wood
            new float[] {0.4F,0.25F,0.15F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 cockpit paint (gray) contrast
            new float[] {0.3F,0.17F,0.0F,1F},
            //29 cockpit paint (gray)
            new float[] {0.45F,0.25F,0.0F,1F},
            //30 helmet paint (bold) contrast
            new float[] {0.2F,0.55F,-0.05F,1F},
            //31 helmet paint (bold)
            new float[] {0.3F,0.65F,0.05F,1F},
            //32 alternate paint contrast
            new float[] {0.53F,0.45F,0.3F,1F},
            //33 alternate paint
            new float[] {0.65F,0.55F,0.4F,1F},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.35F,0.3F,0.25F,1F},
            //36 rotor frame 0 contrast
            new float[] {0.6F,0.5F,0.35F,spin_alpha_0},
            //37 rotor frame 0
            new float[] {0.4F,0.6F,0.35F,spin_alpha_0},
            //38 rotor frame 1 contrast
            new float[] {0.6F,0.5F,0.35F,spin_alpha_1},
            //39 rotor frame 1
            new float[] {0.4F,0.6F,0.35F,spin_alpha_1},
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
            //57 weapon 1 emitter
            new float[] {0F,0F,0F,0F},
            //58 weapon 1 trail
            new float[] {0F,0F,0F,0F},
            //59 weapon 2 emitter
            new float[] {0F,0F,0F,0F},
            //60 weapon 2 trail
            new float[] {0F,0F,0F,0F},
            //61 total transparent
            new float[] {0F,0F,0F,0F},
            },

        new float[][] { // 6 blue
            //0 tires, treads contrast
            new float[] {0.13F,0.1F,0.1F,1F},
            //1 tires, treads
            new float[] {0.23F,0.2F,0.2F,1F},
            //2 doors paint (dark) contrast
            new float[] {0.0F,0.0F,0.35F,1F},
            //3 doors paint (dark)
            new float[] {0.1F,0.1F,0.5F,1F},
            //4 main paint (mid) contrast
            new float[] {0.03F,0.12F,0.7F,1F},
            //5 main paint (mid)
            new float[] {0.15F,0.25F,0.8F,1F},
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
            //12 exposed metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 exposed metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 gun peripheral
            new float[] {0.4F,0.5F,0.4F,1F},
            //15 gun barrel
            new float[] {0.3F,0.35F,0.4F,1F},
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
            new float[] {0.9F,0.8F,0.65F,1F},
            //22 glow frame 1
            new float[] {1.05F,0.95F,0.8F,1F},
            //23 glow frame 2
            new float[] {0.9F,0.8F,0.65F,1F},
            //24 glow frame 3
            new float[] {0.78F,0.7F,0.55F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud, wood
            new float[] {0.4F,0.25F,0.15F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 cockpit paint (gray) contrast
            new float[] {0.28F,0.31F,0.6F,1F},
            //29 cockpit paint (gray)
            new float[] {0.4F,0.45F,0.7F,1F},
            //30 helmet paint (bold) contrast
            new float[] {0.1F,0.15F,0.4F,1F},
            //31 helmet paint (bold)
            new float[] {0.2F,0.25F,0.5F,1F},
            //32 alternate paint contrast
            new float[] {0.86F,0.77F,0.05F,1F},
            //33 alternate paint
            new float[] {0.95F,0.85F,0.15F,1F},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.45F,0.7F,0.7F,1F},
            //36 rotor frame 0 contrast
            new float[] {0.9F,0.8F,0.1F,spin_alpha_0},
            //37 rotor frame 0
            new float[] {0.55F,0.5F,0.95F,spin_alpha_0},
            //38 rotor frame 1 contrast
            new float[] {0.9F,0.8F,0.1F,spin_alpha_1},
            //39 rotor frame 1
            new float[] {0.55F,0.5F,0.95F,spin_alpha_1},
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
            //57 weapon 1 emitter
            new float[] {0F,0F,0F,0F},
            //58 weapon 1 trail
            new float[] {0F,0F,0F,0F},
            //59 weapon 2 emitter
            new float[] {0F,0F,0F,0F},
            //60 weapon 2 trail
            new float[] {0F,0F,0F,0F},
            //61 total transparent
            new float[] {0F,0F,0F,0F},
            },

        new float[][] { // 7 purple
            //0 tires, treads contrast
            new float[] {0.13F,0.1F,0.1F,1F},
            //1 tires, treads
            new float[] {0.23F,0.2F,0.2F,1F},
            //2 doors paint (dark) contrast
            new float[] {0.52F,0.1F,0.47F,1F},
            //3 doors paint (dark)
            new float[] {0.65F,0.15F,0.6F,1F},
            //4 main paint (mid) contrast
            new float[] {0.3F,-0.06F,0.35F,1F},
            //5 main paint (mid)
            new float[] {0.4F,0.1F,0.45F,1F},
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
            //12 exposed metal contrast
            new float[] {0.7F,0.85F,1.1F,1F},
            //13 exposed metal
            new float[] {0.6F,0.65F,0.75F,1F},
            //14 gun peripheral
            new float[] {0.4F,0.5F,0.4F,1F},
            //15 gun barrel
            new float[] {0.3F,0.35F,0.4F,1F},
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
            new float[] {1.1F,1.05F,0.6F,1F},
            //23 glow frame 2
            new float[] {0.95F,0.9F,0.45F,1F},
            //24 glow frame 3
            new float[] {0.85F,0.8F,0.35F,1F},
            //25 shadow
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            //26 mud, wood
            new float[] {0.4F,0.25F,0.15F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 cockpit paint (gray) contrast
            new float[] {0.4F,0.3F,0.5F,1F},
            //29 cockpit paint (gray)
            new float[] {0.55F,0.4F,0.65F,1F},
            //30 helmet paint (bold) contrast
            new float[] {0.45F,0.12F,0.45F,1F},
            //31 helmet paint (bold)
            new float[] {0.55F,0.25F,0.55F,1F},
            //32 alternate paint contrast
            new float[] {0.65F,0.8F,0.9F,1F},
            //33 alternate paint
            new float[] {0.8F,0.9F,1.0F,1F},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.45F,0.7F,0.7F,1F},
            //36 rotor frame 0 contrast
            new float[] {0.7F,0.8F,0.9F,spin_alpha_0},
            //37 rotor frame 0
            new float[] {0.65F,0.35F,0.65F,spin_alpha_0},
            //38 rotor frame 1 contrast
            new float[] {0.7F,0.8F,0.9F,spin_alpha_1},
            //39 rotor frame 1
            new float[] {0.65F,0.35F,0.65F,spin_alpha_1},
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
            //57 weapon 1 emitter
            new float[] {0F,0F,0F,0F},
            //58 weapon 1 trail
            new float[] {0F,0F,0F,0F},
            //59 weapon 2 emitter
            new float[] {0F,0F,0F,0F},
            //60 weapon 2 trail
            new float[] {0F,0F,0F,0F},
            //61 total transparent
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

        public static float[][][] wterrainsgray = new float[][][]
            {
            new float[][] { //0 (50) plains
            //terrain dark
            new float[] {0.8F,0.8F,0.8F,1F},
            //terrain mid
            new float[] {0.8F,0.8F,0.8F,1F},
            //terrain light
            new float[] {0.8F,0.8F,0.8F,1F},
            //terrain highlight
            new float[] {0.8F,0.8F,0.8F,1F},
            },
            new float[][] { //1 (51) forest
            //terrain dark
            new float[] {0.5F,0.5F,0.5F,1F},
            //terrain mid
            new float[] {0.5F,0.5F,0.5F,1F},
            //terrain light
            new float[] {0.5F,0.5F,0.5F,1F},
            //terrain highlight
            new float[] {0.5F,0.5F,0.5F,1F},
            },
            new float[][] { //2 (52) desert
            //terrain dark
            new float[] {0.9F,0.9F,0.9F,1F},
            //terrain mid
            new float[] {0.9F,0.9F,0.9F,1F},
            //terrain light
            new float[] {0.9F,0.9F,0.9F,1F},
            //terrain highlight
            new float[] {0.9F,0.9F,0.9F,1F},
            },
            new float[][] { //3 (53) jungle
            //terrain dark
            new float[] {0.35F,0.35F,0.35F,1F},
            //terrain mid
            new float[] {0.35F,0.35F,0.35F,1F},
            //terrain light
            new float[] {0.35F,0.35F,0.35F,1F},
            //terrain highlight
            new float[] {0.35F,0.35F,0.35F,1F},
            },
            new float[][] { //4 (54) hills
            //terrain dark
            new float[] {0.6F,0.6F,0.6F,1F},
            //terrain mid
            new float[] {0.6F,0.6F,0.6F,1F},
            //terrain light
            new float[] {0.6F,0.6F,0.6F,1F},
            //terrain highlight
            new float[] {0.6F,0.6F,0.6F,1F},
            },
            new float[][] { //5 (55) mountains
            //terrain dark
            new float[] {1.05F,1.05F,1.05F,1F},
            //terrain mid
            new float[] {1.05F,1.05F,1.05F,1F},
            //terrain light
            new float[] {1.05F,1.05F,1.05F,1F},
            //terrain highlight
            new float[] {1.05F,1.05F,1.05F,1F},
            },
            new float[][] { //6 (56) ruins
            //terrain dark
            new float[] {0.25F,0.25F,0.25F,1F},
            //terrain mid
            new float[] {0.25F,0.25F,0.25F,1F},
            //terrain light
            new float[] {0.25F,0.25F,0.25F,1F},
            //terrain highlight
            new float[] {0.25F,0.25F,0.25F,1F},
            },
            new float[][] { //7 (57) tundra
            //terrain dark
            new float[] {1.15F,1.15F,1.15F,1F},
            //terrain mid
            new float[] {1.15F,1.15F,1.15F,1F},
            //terrain light
            new float[] {1.15F,1.15F,1.15F,1F},
            //terrain highlight
            new float[] {1.15F,1.15F,1.15F,1F},
            },
            new float[][] { //8 (58) road
            //terrain dark
            new float[] {0.3F,0.3F,0.3F,1F},
            //terrain mid
            new float[] {0.3F,0.3F,0.3F,1F},
            //terrain light
            new float[] {0.3F,0.3F,0.3F,1F},
            //terrain highlight
            new float[] {0.3F,0.3F,0.3F,1F},
            },
            new float[][] { //9 (59) river
            //terrain dark
            new float[] {0.4F,0.4F,0.4F,1F},
            //terrain mid
            new float[] {0.4F,0.4F,0.4F,1F},
            //terrain light
            new float[] {0.4F,0.4F,0.4F,1F},
            //terrain highlight
            new float[] {0.4F,0.4F,0.4F,1F},
            },
            new float[][] { //10 (60) sea
            //terrain dark
            new float[] {0.2F,0.2F,0.2F,1F},
            //terrain mid
            new float[] {0.2F,0.2F,0.2F,1F},
            //terrain light
            new float[] {0.2F,0.2F,0.2F,1F},
            //terrain highlight
            new float[] {0.2F,0.2F,0.2F,1F},
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
                
                for(int c = 4; c < wpalettes[0].Length; c++)
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
        
    }
}
