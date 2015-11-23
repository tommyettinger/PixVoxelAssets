using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsPV
{
    class CURedux
    {
        public static Random r = new Random(0x1337BEEF);

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

        public const byte shadow = 253 - 25 * 4, smoke = 253 - 17 * 4, yellow_fire = 253 - 18 * 4, orange_fire = 253 - 19 * 4,
            sparks = 253 - 20 * 4, bold_paint = 253 - 31 * 4, metal = 253 - 13 * 4,
            emitter0 = 253 - 57 * 4, trail0 = 253 - 58 * 4, emitter1= 253 - 59 * 4, trail1 = 253 - 60 * 4;

        public static float[][][] wpalettes = new float[][][]
        {
        new float[][] { // 0 dark
            //0 tires, treads contrast
            new float[] {0.31F,0.28F,0.28F,1F},
            //1 tires, treads
            new float[] {0.37F,0.35F,0.34F,1F},
            //2 doors paint (dark) contrast
            new float[] {0.45F,-0.02F,-0.05F,1F},
            //3 doors paint (dark)
            new float[] {0.6F,0.05F,0.05F,1F},
            //4 main paint (mid) contrast
            new float[] {0.15F,0.15F,0.15F,1F},
            //5 main paint (mid)
            new float[] {0.3F,0.3F,0.3F,1F},
            //6 hair contrast
            new float[] {0.5F,0.2F,0.08F,1F},
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
            new float[] {0.3F,0.3F,0.29F,1F},
            //17 smoke
            new float[] {0.27F,0.17F,0.07F,waver_alpha},
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
            new float[] {0.19F,0.19F,0.19F,flat_alpha},
            //26 mud, wood
            new float[] {0.4F,0.25F,0.15F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 cockpit paint (gray) contrast
            new float[] {0.27F,0.27F,0.2F,1F},
            //29 cockpit paint (gray)
            new float[] {0.42F,0.42F,0.35F,1F},
            //30 helmet paint (bold) contrast
            new float[] {0.2F,0.15F,0.1F,1F},
            //31 helmet paint (bold)
            new float[] {0.3F,0.25F,0.2F,1F},
            //32 alternate paint contrast
            new float[] {0.65F,0.0F,0.05F,1F},
            //33 alternate paint
            new float[] {0.8F,0.1F,0.18F,1F},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.3F,0.5F,0.5F,1F},
            //36 rotor frame 0 contrast
            new float[] {0.75F,0.1F,0.15F,spin_alpha_0},
            //37 rotor frame 0
            new float[] {0.65F,0.65F,0.65F,spin_alpha_0},
            //38 rotor frame 1 contrast
            new float[] {0.75F,0.1F,0.15F,spin_alpha_1},
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
            //57 weapon 0 emitter
            new float[] {0F,0F,0F,0F},
            //58 weapon 0 trail
            new float[] {0F,0F,0F,0F},
            //59 weapon 1 emitter
            new float[] {0F,0F,0F,0F},
            //60 weapon 1 trail
            new float[] {0F,0F,0F,0F},
            //61 total transparent
            new float[] {0F,0F,0F,0F},
            },

        new float[][] { // 1 pale
            //0 tires, treads contrast
            new float[] {0.31F,0.28F,0.28F,1F},
            //1 tires, treads
            new float[] {0.37F,0.35F,0.34F,1F},
            //2 doors paint (dark) contrast
            new float[] {0.45F,0.55F,0.68F,1F},
            //3 doors paint (dark)
            new float[] {0.6F,0.7F,0.83F,1F},
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
            new float[] {0.3F,0.3F,0.29F,1F},
            //17 smoke
            new float[] {0.15F,0.27F,0.33F,waver_alpha},
            //18 yellow fire
            new float[] {0.55F,1.3F,0.6F,1F},
            //19 orange fire
            new float[] {0.52F,1.1F,0.4F,1F},
            //20 sparks
            new float[] {0.8F,1.3F,1.15F,1F},
            //21 glow frame 0
            new float[] {0.3F,1.1F,0.55F,1F},
            //22 glow frame 1
            new float[] {0.45F,1.2F,0.7F,1F},
            //23 glow frame 2
            new float[] {0.3F,1.1F,0.55F,1F},
            //24 glow frame 3
            new float[] {0.2F,0.95F,0.45F,1F},
            //25 shadow
            new float[] {0.19F,0.19F,0.19F,flat_alpha},
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
            //57 weapon 0 emitter
            new float[] {0F,0F,0F,0F},
            //58 weapon 0 trail
            new float[] {0F,0F,0F,0F},
            //59 weapon 1 emitter
            new float[] {0F,0F,0F,0F},
            //60 weapon 1 trail
            new float[] {0F,0F,0F,0F},
            //61 total transparent
            new float[] {0F,0F,0F,0F},
            },

        new float[][] { // 2 red
            //0 tires, treads contrast
            new float[] {0.31F,0.28F,0.28F,1F},
            //1 tires, treads
            new float[] {0.37F,0.35F,0.34F,1F},
            //2 doors paint (dark) contrast
            new float[] {0.37F,-0.1F,-0.1F,1F},
            //3 doors paint (dark)
            new float[] {0.55F,-0.05F,-0.1F,1F},
            //4 main paint (mid) contrast
            new float[] {0.73F,0.07F,0.0F,1F},
            //5 main paint (mid)
            new float[] {0.86F,0.19F,0.1F,1F},
            //6 hair contrast
            new float[] {0.5F,0.2F,0.08F,1F},
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
            new float[] {0.3F,0.3F,0.29F,1F},
            //17 smoke
            new float[] {0.29F,0.25F,0.15F,waver_alpha},
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
            new float[] {0.19F,0.19F,0.19F,flat_alpha},
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
            //57 weapon 0 emitter
            new float[] {0F,0F,0F,0F},
            //58 weapon 0 trail
            new float[] {0F,0F,0F,0F},
            //59 weapon 1 emitter
            new float[] {0F,0F,0F,0F},
            //60 weapon 1 trail
            new float[] {0F,0F,0F,0F},
            //61 total transparent
            new float[] {0F,0F,0F,0F},
            },

        new float[][] { // 3 orange
            //0 tires, treads contrast
            new float[] {0.31F,0.28F,0.28F,1F},
            //1 tires, treads
            new float[] {0.37F,0.35F,0.34F,1F},
            //2 doors paint (dark) contrast
            new float[] {0.42F,0.09F,-0.1F,1F},
            //3 doors paint (dark)
            new float[] {0.6F,0.2F,0.0F,1F},
            //4 main paint (mid) contrast
            new float[] {0.85F,0.28F,-0.1F,1F},
            //5 main paint (mid)
            new float[] {1.0F,0.4F,-0.02F,1F},
            //6 hair contrast
            new float[] {0.5F,0.2F,0.08F,1F},
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
            new float[] {0.3F,0.3F,0.29F,1F},
            //17 smoke
            new float[] {0.29F,0.25F,0.15F,waver_alpha},
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
            new float[] {0.19F,0.19F,0.19F,flat_alpha},
            //26 mud, wood
            new float[] {0.4F,0.25F,0.15F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 cockpit paint (gray) contrast
            new float[] {0.62F,0.33F,0.05F,1F},
            //29 cockpit paint (gray)
            new float[] {0.77F,0.45F,0.15F,1F},
            //30 helmet paint (bold) contrast
            new float[] {0.82F,0.21F,-0.08F,1F},
            //31 helmet paint (bold)
            new float[] {0.95F,0.3F,0.05F,1F},
            //32 alternate paint contrast
            new float[] {0.95F,0.9F,0.28F,1F},
            //33 alternate paint
            new float[] {1.05F,1.0F,0.4F,1F},
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
            //57 weapon 0 emitter
            new float[] {0F,0F,0F,0F},
            //58 weapon 0 trail
            new float[] {0F,0F,0F,0F},
            //59 weapon 1 emitter
            new float[] {0F,0F,0F,0F},
            //60 weapon 1 trail
            new float[] {0F,0F,0F,0F},
            //61 total transparent
            new float[] {0F,0F,0F,0F},
            },

        new float[][] { // 4 yellow
            //0 tires, treads contrast
            new float[] {0.31F,0.28F,0.28F,1F},
            //1 tires, treads
            new float[] {0.37F,0.35F,0.34F,1F},
            //2 doors paint (dark) contrast
            new float[] {0.5F,0.4F,-0.15F,1F},
            //3 doors paint (dark)
            new float[] {0.63F,0.51F,-0.15F,1F},
            //4 main paint (mid) contrast
            new float[] {0.65F,0.6F,0.1F,1F},
            //5 main paint (mid)
            new float[] {0.8F,0.75F,0.2F,1F},
            //6 hair contrast
            new float[] {0.5F,0.2F,0.08F,1F},
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
            new float[] {0.3F,0.3F,0.29F,1F},
            //17 smoke
            new float[] {0.29F,0.25F,0.15F,waver_alpha},
            //18 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            //19 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            //20 sparks
            new float[] {1.3F,1.2F,0.85F,1F},
            //21 glow frame 0
            new float[] {0.9F,0.75F,0.25F,1F},
            //22 glow frame 1
            new float[] {1.0F,0.85F,0.3F,1F},
            //23 glow frame 2
            new float[] {0.9F,0.75F,0.25F,1F},
            //24 glow frame 3
            new float[] {0.8F,0.65F,0.1F,1F},
            //25 shadow
            new float[] {0.19F,0.19F,0.19F,flat_alpha},
            //26 mud, wood
            new float[] {0.4F,0.25F,0.15F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 cockpit paint (gray) contrast
            new float[] {0.5F,0.45F,0.2F,1F},
            //29 cockpit paint (gray)
            new float[] {0.6F,0.55F,0.3F,1F},
            //30 helmet paint (bold) contrast
            new float[] {0.62F,0.55F,0.0F,1F},
            //31 helmet paint (bold)
            new float[] {0.73F,0.65F,0.15F,1F},
            //32 alternate paint contrast
            new float[] {-0.1F,-0.1F,0.45F,1F},
            //33 alternate paint
            new float[] {0.0F,0.0F,0.6F,1F},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.35F,0.3F,0.25F,1F},
            //36 rotor frame 0 contrast
            new float[] {0.0F,0.0F,0.5F,spin_alpha_0},
            //37 rotor frame 0
            new float[] {0.85F,0.85F,0.35F,spin_alpha_0},
            //38 rotor frame 1 contrast
            new float[] {0.0F,0.0F,0.5F,spin_alpha_1},
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
            //57 weapon 0 emitter
            new float[] {0F,0F,0F,0F},
            //58 weapon 0 trail
            new float[] {0F,0F,0F,0F},
            //59 weapon 1 emitter
            new float[] {0F,0F,0F,0F},
            //60 weapon 1 trail
            new float[] {0F,0F,0F,0F},
            //61 total transparent
            new float[] {0F,0F,0F,0F},
            },

        new float[][] { // 5 green
            //0 tires, treads contrast
            new float[] {0.31F,0.28F,0.28F,1F},
            //1 tires, treads
            new float[] {0.37F,0.35F,0.34F,1F},
            //2 doors paint (dark) contrast
            new float[] {-0.05F,0.22F,-0.1F,1F},
            //3 doors paint (dark)
            new float[] {0.0F,0.35F,-0.1F,1F},
            //4 main paint (mid) contrast
            new float[] {0.0F,0.45F,0.0F,1F},
            //5 main paint (mid)
            new float[] {0.1F,0.55F,0.1F,1F},
            //6 hair contrast
            new float[] {0.5F,0.2F,0.08F,1F},
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
            new float[] {0.3F,0.3F,0.29F,1F},
            //17 smoke
            new float[] {0.29F,0.25F,0.15F,waver_alpha},
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
            new float[] {0.19F,0.19F,0.19F,flat_alpha},
            //26 mud, wood
            new float[] {0.4F,0.25F,0.15F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 cockpit paint (gray) contrast
            new float[] {0.3F,0.17F,0.0F,1F},
            //29 cockpit paint (gray)
            new float[] {0.45F,0.25F,0.0F,1F},
            //30 helmet paint (bold) contrast
            new float[] {0.15F,0.55F,-0.05F,1F},
            //31 helmet paint (bold)
            new float[] {0.25F,0.65F,0.05F,1F},
            //32 alternate paint contrast
            new float[] {0.53F,0.4F,0.3F,1F},
            //33 alternate paint
            new float[] {0.65F,0.5F,0.4F,1F},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.54F,0.5F,0.47F,1F},
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
            //57 weapon 0 emitter
            new float[] {0F,0F,0F,0F},
            //58 weapon 0 trail
            new float[] {0F,0F,0F,0F},
            //59 weapon 1 emitter
            new float[] {0F,0F,0F,0F},
            //60 weapon 1 trail
            new float[] {0F,0F,0F,0F},
            //61 total transparent
            new float[] {0F,0F,0F,0F},
            },

        new float[][] { // 6 blue
            //0 tires, treads contrast
            new float[] {0.31F,0.28F,0.28F,1F},
            //1 tires, treads
            new float[] {0.37F,0.35F,0.34F,1F},
            //2 doors paint (dark) contrast
            new float[] {0.14F,0.14F,0.33F,1F},
            //3 doors paint (dark)
            new float[] {0.25F,0.25F,0.45F,1F},
            //4 main paint (mid) contrast
            new float[] {0.12F,0.15F,0.87F,1F},
            //5 main paint (mid)
            new float[] {0.25F,0.28F,1.05F,1F},
            //6 hair contrast
            new float[] {0.5F,0.2F,0.08F,1F},
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
            new float[] {0.3F,0.3F,0.29F,1F},
            //17 smoke
            new float[] {0.29F,0.25F,0.15F,waver_alpha},
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
            new float[] {0.19F,0.19F,0.19F,flat_alpha},
            //26 mud, wood
            new float[] {0.4F,0.25F,0.15F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 cockpit paint (gray) contrast
            new float[] {0.18F,0.18F,0.5F,1F},
            //29 cockpit paint (gray)
            new float[] {0.3F,0.3F,0.63F,1F},
            //30 helmet paint (bold) contrast
            new float[] {0.1F,0.1F,0.4F,1F},
            //31 helmet paint (bold)
            new float[] {0.2F,0.2F,0.5F,1F},
            //32 alternate paint contrast
            new float[] {0.68F,0.57F,-0.05F,1F},
            //33 alternate paint
            new float[] {0.8F,0.7F,0.05F,1F},
            //34 gore
            new float[] {0.67F,0.05F,-0.1F,1F},
            //35 glass
            new float[] {0.45F,0.7F,0.7F,1F},
            //36 rotor frame 0 contrast
            new float[] {0.75F,0.65F,0.05F,spin_alpha_0},
            //37 rotor frame 0
            new float[] {0.55F,0.5F,0.95F,spin_alpha_0},
            //38 rotor frame 1 contrast
            new float[] {0.75F,0.65F,0.05F,spin_alpha_1},
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
            //57 weapon 0 emitter
            new float[] {0F,0F,0F,0F},
            //58 weapon 0 trail
            new float[] {0F,0F,0F,0F},
            //59 weapon 1 emitter
            new float[] {0F,0F,0F,0F},
            //60 weapon 1 trail
            new float[] {0F,0F,0F,0F},
            //61 total transparent
            new float[] {0F,0F,0F,0F},
            },

        new float[][] { // 7 purple
            //0 tires, treads contrast
            new float[] {0.31F,0.28F,0.28F,1F},
            //1 tires, treads
            new float[] {0.37F,0.35F,0.34F,1F},
            //2 doors paint (dark) contrast
            new float[] {0.18F,-0.02F,0.15F,1F},
            //3 doors paint (dark)
            new float[] {0.3F,0.05F,0.25F,1F},
            //4 main paint (mid) contrast
            new float[] {0.38F,0.01F,0.43F,1F},
            //5 main paint (mid)
            new float[] {0.5F,0.1F,0.55F,1F},
            //6 hair contrast
            new float[] {0.5F,0.2F,0.08F,1F},
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
            new float[] {0.3F,0.3F,0.29F,1F},
            //17 smoke
            new float[] {0.29F,0.25F,0.15F,waver_alpha},
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
            new float[] {0.19F,0.19F,0.19F,flat_alpha},
            //26 mud, wood
            new float[] {0.4F,0.25F,0.15F,1F},
            //27 water
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            //28 cockpit paint (gray) contrast
            new float[] {0.4F,0.25F,0.5F,1F},
            //29 cockpit paint (gray)
            new float[] {0.55F,0.33F,0.65F,1F},
            //30 helmet paint (bold) contrast
            new float[] {0.6F,0.12F,0.6F,1F},
            //31 helmet paint (bold)
            new float[] {0.73F,0.28F,0.73F,1F},
            //32 alternate paint contrast
            new float[] {0.3F,0.3F,0.3F,1F},
            //33 alternate paint
            new float[] {0.42F,0.42F,0.42F,1F},
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
            //57 weapon 0 emitter
            new float[] {0F,0F,0F,0F},
            //58 weapon 0 trail
            new float[] {0F,0F,0F,0F},
            //59 weapon 1 emitter
            new float[] {0F,0F,0F,0F},
            //60 weapon 1 trail
            new float[] {0F,0F,0F,0F},
            //61 total transparent
            new float[] {0F,0F,0F,0F},
            },
        };

        public static float[][][] wterrains = new float[][][]
        {
            new float[][] { //0 (50) plains
            //terrain dark
            new float[] {0.67F,0.96F,0.3F,1F},
            //terrain mid
            new float[] {0.67F,0.96F,0.3F,1F},
            //terrain light
            new float[] {0.67F,0.96F,0.3F,1F},
            //terrain highlight
            new float[] {0.67F,0.96F,0.3F,1F},
            },
            new float[][] { //1 (51) forest
            //terrain dark
            new float[] {0.25F,0.8F,0.1F,1F},
            //terrain mid
            new float[] {0.25F,0.8F,0.1F,1F},
            //terrain light
            new float[] {0.25F,0.8F,0.1F,1F},
            //terrain highlight
            new float[] {0.25F,0.8F,0.1F,1F},
            },
            new float[][] { //2 (52) desert
            //terrain dark
            new float[] {1.05F,0.9F,0.4F,1F},
            //terrain mid
            new float[] {1.05F,0.9F,0.4F,1F},
            //terrain light
            new float[] {1.05F,0.9F,0.4F,1F},
            //terrain highlight
            new float[] {1.05F,0.9F,0.4F,1F},
            },
            new float[][] { //3 (53) jungle
            //terrain dark
            new float[] {0.0F,0.65F,0.4F,1F},
            //terrain mid
            new float[] {0.0F,0.65F,0.4F,1F},
            //terrain light
            new float[] {0.0F,0.65F,0.4F,1F},
            //terrain highlight
            new float[] {0.0F,0.65F,0.4F,1F},
            },
            new float[][] { //4 (54) hills
            //terrain dark
            new float[] {0.95F,0.7F,0.35F,1F},
            //terrain mid
            new float[] {0.95F,0.7F,0.35F,1F},
            //terrain light
            new float[] {0.95F,0.7F,0.35F,1F},
            //terrain highlight
            new float[] {0.95F,0.7F,0.35F,1F},
            },
            new float[][] { //5 (55) mountains
            //terrain dark
            new float[] {0.7F,0.8F,0.83F,1F},
            //terrain mid
            new float[] {0.7F,0.8F,0.83F,1F},
            //terrain light
            new float[] {0.7F,0.8F,0.83F,1F},
            //terrain highlight
            new float[] {0.7F,0.8F,0.83F,1F},
            },
            new float[][] { //6 (56) ruins
            //terrain dark
            new float[] {0.7F,0.3F,0.65F,1F},
            //terrain mid
            new float[] {0.7F,0.3F,0.65F,1F},
            //terrain light
            new float[] {0.7F,0.3F,0.65F,1F},
            //terrain highlight
            new float[] {0.7F,0.3F,0.65F,1F},
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
            new float[] {0.65F,0.65F,0.65F,1F},
            //terrain mid
            new float[] {0.65F,0.65F,0.65F,1F},
            //terrain light
            new float[] {0.65F,0.65F,0.65F,1F},
            //terrain highlight
            new float[] {0.65F,0.65F,0.65F,1F},
            },
            new float[][] { //9 (59) river
            //terrain dark
            new float[] {0.2F,0.35F,1.0F,1F},
            //terrain mid
            new float[] {0.2F,0.35F,1.0F,1F},
            //terrain light
            new float[] {0.2F,0.35F,1.0F,1F},
            //terrain highlight
            new float[] {0.2F,0.35F,1.0F,1F},
            },
            new float[][] { //10 (60) basement
            //terrain dark
            new float[] {0.35F,0.35F,0.35F,1F},
            //terrain mid
            new float[] {0.35F,0.35F,0.35F,1F},
            //terrain light
            new float[] {0.35F,0.35F,0.35F,1F},
            //terrain highlight
            new float[] {0.35F,0.35F,0.35F,1F},
            },
            new float[][] { //11 (61) sea
            //terrain dark
            new float[] {0.05F,0.45F,0.8F,1F},
            //terrain mid
            new float[] {0.05F,0.45F,0.8F,1F},
            //terrain light
            new float[] {0.05F,0.45F,0.8F,1F},
            //terrain highlight
            new float[] {0.05F,0.45F,0.8F,1F},
            },
            new float[][] { //12 (62) custom 0
            //terrain dark
            new float[] {1.0F,0.2F,-0.1F,1F},
            //terrain mid
            new float[] {1.0F,0.2F,-0.1F,1F},
            //terrain light
            new float[] {1.0F,0.2F,-0.1F,1F},
            //terrain highlight
            new float[] {1.0F,0.2F,-0.1F,1F},
            },
            new float[][] { //13 (63) custom 1
            //terrain dark
            new float[] {1.05F,0.15F,0.95F,1F},
            //terrain mid
            new float[] {1.05F,0.15F,0.95F,1F},
            //terrain light
            new float[] {1.05F,0.15F,0.95F,1F},
            //terrain highlight
            new float[] {1.05F,0.15F,0.95F,1F},
            },
            new float[][] { //14 (64) custom 2
            //terrain dark
            new float[] {1.15F,1.1F,-0.1F,1F},
            //terrain mid
            new float[] {1.15F,1.1F,-0.1F,1F},
            //terrain light
            new float[] {1.15F,1.1F,-0.1F,1F},
            //terrain highlight
            new float[] {1.15F,1.1F,-0.1F,1F},
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

        public static string[] Terrains = new string[]
        {"Plains","Forest","Desert","Jungle","Hills"
        ,"Mountains","Ruins","Tundra","Road","River", "Basement", "Sea"
        ,"Custom0","Custom1","Custom2"};

        public static byte[][][,,] Explosions, SuperExplosions;
        public const int maxExplosionFrames = 6;

        public static void Initialize(bool disableGore)
        {
            VoxelLogic.wpalettecount = wpalettes.Length;
            int wpc = VoxelLogic.wpalettecount;

            if(disableGore)
            {
                for(int p = 0; p < wpc; p++)
                {
                    wpalettes[p][34] = wpalettes[p][9].Replicate();
                }
            }

            VoxelLogic.terrainPalettes = new int[] { wpc };
            VoxelLogic.subtlePalettes = new int[] { wpc };
            VoxelLogic.wcolorcount = wpalettes[0].Length;
            VoxelLogic.wcolors = wpalettes[0].Replicate();
            wpalettes = wpalettes.Concat(new float[][][] { wpalettes[0].Replicate()}).ToArray();
            for(int p = 0; p < wterrains.Length; p++)
            {
                wpalettes[wpc][0 + p*4] = wterrains[p][0];
                wpalettes[wpc][0 + p * 4][0] -= 0.4f;
                wpalettes[wpc][0 + p * 4][1] -= 0.4f;
                wpalettes[wpc][0 + p * 4][2] -= 0.4f;
                wpalettes[wpc][0 + p * 4][3] = 1F;

                wpalettes[wpc][1 + p * 4] = wterrains[p][1];
                wpalettes[wpc][1 + p * 4][0] -= 0.25f;
                wpalettes[wpc][1 + p * 4][1] -= 0.25f;
                wpalettes[wpc][1 + p * 4][2] -= 0.25f;
                wpalettes[wpc][1 + p * 4][3] = 1F;

                wpalettes[wpc][2 + p * 4] = wterrains[p][2];
                wpalettes[wpc][2 + p * 4][0] -= 0.1f;
                wpalettes[wpc][2 + p * 4][1] -= 0.1f;
                wpalettes[wpc][2 + p * 4][2] -= 0.1f;
                wpalettes[wpc][2 + p * 4][3] = 1F;

                wpalettes[wpc][3 + p * 4] = wterrains[p][3];
                wpalettes[wpc][3 + p * 4][0] += 0.05f;
                wpalettes[wpc][3 + p * 4][1] += 0.05f;
                wpalettes[wpc][3 + p * 4][2] += 0.05f;
                wpalettes[wpc][3 + p * 4][3] = 1F;

            }

            VoxelLogic.wpalettecount = wpalettes.Length;

            VoxelLogic.clear = (byte)(253 - (VoxelLogic.wcolorcount - 1) * 4);
            VoxelLogic.wcolorcount = wpalettes[0].Length;

            for(int p = 0; p < VoxelLogic.wpalettecount; p++)
            {
                float[] drip = wpalettes[p][27].ToArray(), transp = wpalettes[p][VoxelLogic.wcolorcount - 1];
                drip[3] = 1F;
                float[] zap = wpalettes[p][40].ToArray();
                zap[3] = spin_alpha_1;
                wpalettes[p] = wpalettes[p].Concat(new float[][] { drip, transp, transp, transp, drip, zap }).ToArray();
            }

            VoxelLogic.wpalettes = wpalettes;
            TransformLogic.dismiss = new byte[] {
                0, VoxelLogic.clear, 253 - 17 * 4, 253 - 18 * 4, 253 - 19 * 4, 253 - 20 * 4, 253 - 25 * 4, CURedux.emitter0, CURedux.trail0, CURedux.emitter1, CURedux.trail1
            };


        Explosions = new byte[10][][,,];
            for(int e = 0; e < 10; e++)
            {
                MagicaVoxelData[][] expl = FireballSwitchable(randomFill(60 - (e + 1) * 2, 60 - (e + 1) * 2, 0, (e + 1) * 4, (e + 1) * 4, (e + 1) * 3,
                    new int[] { orange_fire, orange_fire, smoke, yellow_fire }).ToArray(), 0, maxExplosionFrames, 3, 120, 120, 80);
                Explosions[e] = new byte[maxExplosionFrames][,,];
                for(int f = 0; f < maxExplosionFrames; f++)
                {
                    Explosions[e][f] = TransformLogic.VoxListToArray(expl[f], 120, 120, 80);
                }
            }
            SuperExplosions = new byte[10][][,,];
            for(int e = 0; e < 10; e++)
            {
                MagicaVoxelData[][] expl = FireballSwitchable(randomFill(80 - (e + 1) * 3, 80 - (e + 1) * 3, 0, (e + 1) * 6, (e + 1) * 6, (e + 1) * 5,
                    new int[] { orange_fire, orange_fire, smoke, yellow_fire }).ToArray(), 0, maxExplosionFrames, 3, 160, 160, 120);
                SuperExplosions[e] = new byte[maxExplosionFrames][,,];
                for(int f = 0; f < maxExplosionFrames; f++)
                {
                    SuperExplosions[e][f] = TransformLogic.VoxListToArray(expl[f], 160, 160, 120);
                }
            }
        }



        public static MagicaVoxelData[][] SmokePlumeLarge(MagicaVoxelData start, int height, int effectDuration)
        {
            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[16];
            voxelFrames[0] = new List<MagicaVoxelData>(height * 8);
            //for (int i = 0; i < voxels.Length; i++)
            //{
            //    voxelFrames[0][i].x += 20;
            //    voxelFrames[0][i].y += 20;
            //}
            voxelFrames[0].Add(new MagicaVoxelData { x = start.x, y = start.y, z = 0, color = yellow_fire });

            for(int i = 1; i < 16; i++)
            {
                voxelFrames[i] = new List<MagicaVoxelData>(height * 8);
            }
            for(int f = 1; f < 16 && f < effectDuration && f < height; f++)
            {
                for(int i = 0; i <= f; i++)
                {
                    voxelFrames[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(start.x - i / 2), y = start.y, z = (byte)i, color = yellow_fire }, ((f < 2 && i == 0) ? yellow_fire : start.color)));
                }
            }
            for(int f = Math.Min(height, effectDuration); f < 16 && f <= effectDuration && f < height * 2; f += 2)
            {
                voxelFrames[f].AddRange(VoxelLogic.generateFatStraightLine(new MagicaVoxelData { x = (byte)(start.x - (f - height) + r.Next(5) - 2), y = (byte)(start.y + r.Next(5) - 2), z = (byte)(f - height), color = start.color },
                    new MagicaVoxelData { x = (byte)(start.x - (height - 1) + r.Next(5) - 2), y = (byte)(start.y + r.Next(5) - 2), z = (byte)(height - 1), color = start.color },
                    start.color));
            }

            MagicaVoxelData[][] frames = new MagicaVoxelData[16][];

            for(int f = 0; f < 16; f++)
            {
                frames[f] = new MagicaVoxelData[voxelFrames[f].Count];
                voxelFrames[f].CopyTo(frames[f], 0);
            }
            return frames;
        }
        public static MagicaVoxelData[][] BurstLarge(MagicaVoxelData start, int maxFrames, bool bigger)
        {
            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[maxFrames];
            voxelFrames[0] = new List<MagicaVoxelData>(10);
            //for (int i = 0; i < voxels.Length; i++)
            //{
            //    voxelFrames[0][i].x += 20;
            //    voxelFrames[0][i].y += 20;
            //}
            voxelFrames[0].Add(new MagicaVoxelData { x = start.x, y = start.y, z = start.z, color = yellow_fire });

            for(int i = 1; i < maxFrames; i++)
            {
                voxelFrames[i] = new List<MagicaVoxelData>(80);
            }

            int rz = (r.Next(5) - 2);
            for(int i = 1; i < maxFrames; i++)
            {
                if(i <= 1)
                    voxelFrames[i].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(start.x), y = (byte)(start.y), z = start.z, color = orange_fire }, orange_fire));
                if(i > 1)
                {
                    voxelFrames[i].AddRange(VoxelLogic.generateFatStraightLine(new MagicaVoxelData { x = (byte)(start.x + 2), y = (byte)(start.y), z = (byte)(start.z - 2 + rz * (i - 1)), color = orange_fire },
                        new MagicaVoxelData { x = (byte)(start.x + 2), y = (byte)(start.y), z = (byte)(start.z + 2 + rz * (i - 1)), color = orange_fire }, orange_fire));
                    voxelFrames[i].AddRange(VoxelLogic.generateFatStraightLine(new MagicaVoxelData { x = (byte)(start.x + 2), y = (byte)(start.y - 2), z = (byte)(start.z + rz * (i - 1)), color = orange_fire },
                                            new MagicaVoxelData { x = (byte)(start.x + 2), y = (byte)(start.y + 2), z = (byte)(start.z + rz * (i - 1)), color = orange_fire }, orange_fire));
                    if(bigger)
                    {
                        voxelFrames[i].AddRange(VoxelLogic.generateFatStraightLine(new MagicaVoxelData { x = (byte)(start.x + 4), y = (byte)(start.y), z = (byte)(start.z - 2 + rz * (i - 1)), color = orange_fire },
                            new MagicaVoxelData { x = (byte)(start.x + 4), y = (byte)(start.y), z = (byte)(start.z + 2 + rz * (i - 1)), color = orange_fire }, orange_fire));
                        voxelFrames[i].AddRange(VoxelLogic.generateFatStraightLine(new MagicaVoxelData { x = (byte)(start.x + 4), y = (byte)(start.y - 2), z = (byte)(start.z + rz * (i - 1)), color = orange_fire },
                            new MagicaVoxelData { x = (byte)(start.x + 4), y = (byte)(start.y + 2), z = (byte)(start.z + rz * (i - 1)), color = orange_fire }, orange_fire));
                        if(i > 2)
                        {
                            voxelFrames[i].AddRange(VoxelLogic.generateFatStraightLine(new MagicaVoxelData { x = (byte)(start.x + 6), y = (byte)(start.y), z = (byte)(start.z - 4 + rz * (i - 1)), color = orange_fire },
                            new MagicaVoxelData { x = (byte)(start.x + 6), y = (byte)(start.y), z = (byte)(start.z + 4 + rz * (i - 1)), color = orange_fire }, orange_fire));
                            voxelFrames[i].AddRange(VoxelLogic.generateFatStraightLine(new MagicaVoxelData { x = (byte)(start.x + 6), y = (byte)(start.y - 4), z = (byte)(start.z + rz * (i - 1)), color = orange_fire },
                                new MagicaVoxelData { x = (byte)(start.x + 6), y = (byte)(start.y + 4), z = (byte)(start.z + rz * (i - 1)), color = orange_fire }, orange_fire));

                        }
                    }
                }

            }

            MagicaVoxelData[][] frames = new MagicaVoxelData[16][];

            for(int f = 0; f < maxFrames; f++)
            {
                frames[f] = new MagicaVoxelData[voxelFrames[f].Count];
                voxelFrames[f].CopyTo(frames[f], 0);
            }
            return frames;
        }
        public static MagicaVoxelData[][] SparksLarge(MagicaVoxelData start, int sweepDuration)
        {
            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[16];
            voxelFrames[0] = new List<MagicaVoxelData>(80);
            //for (int i = 0; i < voxels.Length; i++)
            //{
            //    voxelFrames[0][i].x += 20;
            //    voxelFrames[0][i].y += 20;
            //}
            for(int i = 0; i < 1; i++)
            {
                voxelFrames[0].Add(new MagicaVoxelData { x = start.x, y = start.y, z = (byte)i, color = sparks });
            }
            for(int i = 1; i < 16; i++)
            {
                voxelFrames[i] = new List<MagicaVoxelData>(80);
            }
            bool sweepingPositive = true;
            int iter = 0;
            while(iter < 16)
            {
                for(int i = 0; i < sweepDuration && i < 16 && iter < 16; i++, iter++)
                {
                    int rx = (r.Next(3) - r.Next(2));
                    voxelFrames[iter].Add(new MagicaVoxelData { x = (byte)(start.x - rx), y = (byte)(start.y + ((sweepingPositive) ? i : sweepDuration - i) * 4), z = 0, color = sparks });

                    voxelFrames[iter].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(start.x - rx), y = (byte)(start.y + ((sweepingPositive) ? i : sweepDuration - i) * 4), z = 1, color = sparks }, sparks));
                    voxelFrames[iter].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(start.x - rx - 2), y = (byte)(start.y + ((sweepingPositive) ? i : sweepDuration - i) * 4), z = 1, color = sparks }, sparks));
                    voxelFrames[iter].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(start.x - rx - 2), y = (byte)(start.y + ((sweepingPositive) ? i : sweepDuration - i) * 4), z = 3, color = sparks }, sparks));
                    if(r.Next(2) == 0)
                    {
                        voxelFrames[iter].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(start.x - rx - 4), y = (byte)(start.y + ((sweepingPositive) ? i : sweepDuration - i) * 4), z = 1, color = sparks }, sparks));
                        voxelFrames[iter].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(start.x - rx - 4), y = (byte)(start.y + ((sweepingPositive) ? i : sweepDuration - i) * 4), z = 3, color = sparks }, sparks));
                        voxelFrames[iter].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(start.x - rx - 4), y = (byte)(start.y + ((sweepingPositive) ? i : sweepDuration - i) * 4), z = 5, color = sparks }, sparks));
                    }
                }
                
                sweepingPositive = !sweepingPositive;
            }

            MagicaVoxelData[][] frames = new MagicaVoxelData[16][];

            for(int f = 0; f < 16; f++)
            {
                frames[f] = new MagicaVoxelData[voxelFrames[f].Count];
                voxelFrames[f].CopyTo(frames[f], 0);
            }
            return frames;
        }

        private static List<MagicaVoxelData> generateMissileLarge(MagicaVoxelData start, int length)
        {
            List<MagicaVoxelData> vox = new List<MagicaVoxelData>(128);
            for(int x = 0; x < 3; x++)
            {
                for(int z = 0; z < 3; z++)
                {
                    vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + x), y = (byte)(start.y), z = (byte)(start.z + z), color = bold_paint },
                        new MagicaVoxelData { x = (byte)(start.x + x), y = (byte)(start.y + 3), z = (byte)(start.z + z), color = bold_paint }, bold_paint));
                }
            }

            vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + 3), y = (byte)(start.y + 1), z = (byte)(start.z), color = bold_paint },
                new MagicaVoxelData { x = (byte)(start.x + 3), y = (byte)(start.y + 1), z = (byte)(start.z + 3), color = bold_paint }, bold_paint));
            vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + 3), y = (byte)(start.y + 2), z = (byte)(start.z), color = bold_paint },
                new MagicaVoxelData { x = (byte)(start.x + 3), y = (byte)(start.y + 2), z = (byte)(start.z + 3), color = bold_paint }, bold_paint));
            vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + 2), y = (byte)(start.y + 1), z = (byte)(start.z + 3), color = bold_paint },
                new MagicaVoxelData { x = (byte)(start.x + 0), y = (byte)(start.y + 1), z = (byte)(start.z + 3), color = bold_paint }, bold_paint));
            vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + 2), y = (byte)(start.y + 2), z = (byte)(start.z + 3), color = bold_paint },
                new MagicaVoxelData { x = (byte)(start.x + 0), y = (byte)(start.y + 2), z = (byte)(start.z + 3), color = bold_paint }, bold_paint));

            for(int x = 0; x < 2; x++)
            {
                for(int y = 0; y < 4; y++)
                {
                    if(length > 0 || x - 1 > length)
                        vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + x), y = (byte)(start.y + y), z = (byte)(start.z - 1), color = metal },
                        new MagicaVoxelData { x = (byte)(start.x + x - 1 - length), y = (byte)(start.y + y), z = (byte)((start.z - 2 - length < 0) ? 0 : start.z - 2 - length), color = metal }, metal));
                }
            }

            for(int z = 0; z < 2; z++)
            {
                for(int y = 0; y < 4; y++)
                {
                    if(length > 0 || z - 1 > length)
                        vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - 1), y = (byte)(start.y + y), z = (byte)(start.z + z), color = metal },
                            new MagicaVoxelData { x = (byte)(start.x - 2 - length), y = (byte)(start.y + y), z = (byte)((start.z + z - 1 - length < 0) ? 0 : start.z + z - 1 - length), color = metal }, metal));

                }
            }

            if(length > 0)
            {
                for(int y = 0; y < 4; y++)
                {
                    vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - 1), y = (byte)(start.y + y), z = (byte)(start.z - 1), color = metal },
                    new MagicaVoxelData { x = (byte)(start.x - 2 - length), y = (byte)(start.y + y), z = (byte)((start.z - 2 - length < 0) ? 0 : start.z - 2 - length), color = metal }, metal));
                }
            }
            return vox;
        }
        private static List<MagicaVoxelData> generateMissileFieryTrailLarge(MagicaVoxelData start, int length)
        {
            List<MagicaVoxelData> vox = new List<MagicaVoxelData>(128);
            for(int x = 0; x < 3; x++)
            {
                for(int z = 0; z < 3; z++)
                {
                    vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + x), y = (byte)(start.y), z = (byte)(start.z + z), color = bold_paint },
                        new MagicaVoxelData { x = (byte)(start.x + x), y = (byte)(start.y + 3), z = (byte)(start.z + z), color = bold_paint }, bold_paint));
                }
            }

            vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + 3), y = (byte)(start.y + 1), z = (byte)(start.z), color = bold_paint },
                new MagicaVoxelData { x = (byte)(start.x + 3), y = (byte)(start.y + 1), z = (byte)(start.z + 3), color = bold_paint }, bold_paint));
            vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + 3), y = (byte)(start.y + 2), z = (byte)(start.z), color = bold_paint },
                new MagicaVoxelData { x = (byte)(start.x + 3), y = (byte)(start.y + 2), z = (byte)(start.z + 3), color = bold_paint }, bold_paint));
            vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + 2), y = (byte)(start.y + 1), z = (byte)(start.z + 3), color = bold_paint },
                new MagicaVoxelData { x = (byte)(start.x + 0), y = (byte)(start.y + 1), z = (byte)(start.z + 3), color = bold_paint }, bold_paint));
            vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + 2), y = (byte)(start.y + 2), z = (byte)(start.z + 3), color = bold_paint },
                new MagicaVoxelData { x = (byte)(start.x + 0), y = (byte)(start.y + 2), z = (byte)(start.z + 3), color = bold_paint }, bold_paint));

            for(int x = 0; x < 2; x++)
            {
                for(int y = 0; y < 4; y++)
                {
                    if(length > 0 || x - 1 > length)
                    {
                        vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + x), y = (byte)(start.y + y), z = (byte)(start.z - 1), color = metal },
                        new MagicaVoxelData { x = (byte)(start.x + x - 1 - length), y = (byte)(start.y + y), z = (byte)((start.z - 2 - length < 0) ? 0 : start.z - 2 - length), color = metal }, metal));
                        if(x % 2 == 1)
                        {
                            vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + x - 1 - length), y = (byte)(start.y + y), z = (byte)((start.z - 2 - length < 0) ? 0 : start.z - 2 - length), color = metal },
                                new MagicaVoxelData
                                {
                                    x = (byte)(start.x + x - ((y > 0 && y < 3 && x < 2) ? 10 : 6) - length),
                                    y = (byte)(start.y + y),
                                    z = (byte)((start.z - ((y > 0 && y < 3 && x < 2) ? 10 : 6) - length < 0) ? 0 : (start.z - ((y > 0 && y < 3 && x < 2) ? 10 : 6) - length)),
                                    color = metal
                                },
                                (y > 0 && y < 3 && x > 0) ? yellow_fire : orange_fire));
                        }
                    }
                }
            }

            for(int z = 0; z < 2; z++)
            {
                for(int y = 0; y < 4; y++)
                {
                    if(length > 0 || z - 1 > length)
                        vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - 1), y = (byte)(start.y + y), z = (byte)(start.z + z), color = metal },
                        new MagicaVoxelData { x = (byte)(start.x - 2 - length), y = (byte)(start.y + y), z = (byte)((start.z + z - 1 - length < 0) ? 0 : start.z + z - 1 - length), color = metal }, metal));
                    if(z % 2 == 1)
                    {
                        vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - 2 - length), y = (byte)(start.y + y), z = (byte)((start.z + z - 1 - length < 0) ? 0 : start.z + z - 1 - length), color = metal },
                            new MagicaVoxelData
                            {
                                x = (byte)(start.x - ((y > 0 && y < 3 && z < 2) ? 10 : 6) - length),
                                y = (byte)(start.y + y),
                                z = (byte)((start.z + z - ((y > 0 && y < 3 && z < 2) ? 10 : 6) - length < 0) ? 0 : (start.z + z - ((y > 0 && y < 3 && z < 2) ? 10 : 6) - length)),
                                color = metal
                            },
                            (y > 0 && y < 3 && z < 2) ? yellow_fire : orange_fire));
                    }
                }
            }
            if(length > 0)
            {
                for(int y = 0; y < 4; y++)
                {
                    vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - 1), y = (byte)(start.y + y), z = (byte)(start.z - 1), color = metal },
                    new MagicaVoxelData { x = (byte)(start.x - 2 - length), y = (byte)(start.y + y), z = (byte)((start.z - 2 - length < 0) ? 0 : start.z - 2 - length), color = metal }, metal));

                    vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - 2 - length), y = (byte)(start.y + y), z = (byte)((start.z - 2 - length < 0) ? 0 : start.z - 2 - length), color = (byte)(yellow_fire) },
                        new MagicaVoxelData
                        {
                            x = (byte)(start.x - 2 - ((y > 0 && y < 3) ? 10 : 6) - length),
                            y = (byte)(start.y + y),
                            z = (byte)((start.z - ((y > 0 && y < 3) ? 10 : 6) - length < 0) ? 0 : (start.z - ((y > 0 && y < 3) ? 10 : 6) - length)),
                            color = (byte)(yellow_fire)
                        },
                        yellow_fire));
                }
            }
            return vox;
        }

        private static List<MagicaVoxelData> generateConeLarge(MagicaVoxelData start, int segments, int color)
        {
            List<MagicaVoxelData> cone = new List<MagicaVoxelData>(40);
            for(int x = 0; x < segments; x++)
            {
                for(float y = 0; y <= x * 0.75f; y++)
                {
                    for(float z = 0; z <= x * 0.75f; z++)
                    {
                        if(Math.Floor(y) == y && Math.Floor(z) == z)
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
        private static List<MagicaVoxelData> generateDownwardMissileLarge(MagicaVoxelData start, int length)
        {
            List<MagicaVoxelData> vox = new List<MagicaVoxelData>(128);
            for(int x = 0; x < 3; x++)
            {
                for(int z = 0; z < 3; z++)
                {
                    vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - x), y = (byte)(start.y), z = (byte)(start.z - z), color = bold_paint },
                        new MagicaVoxelData { x = (byte)(start.x - x), y = (byte)(start.y + 3), z = (byte)(start.z - z), color = bold_paint }, bold_paint));
                }
            }

            vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - 3), y = (byte)(start.y + 1), z = (byte)(start.z), color = bold_paint },
                new MagicaVoxelData { x = (byte)(start.x - 3), y = (byte)(start.y + 1), z = (byte)(start.z - 3), color = bold_paint }, bold_paint));
            vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - 3), y = (byte)(start.y + 2), z = (byte)(start.z), color = bold_paint },
                new MagicaVoxelData { x = (byte)(start.x - 3), y = (byte)(start.y + 2), z = (byte)(start.z - 3), color = bold_paint }, bold_paint));
            vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - 2), y = (byte)(start.y + 1), z = (byte)(start.z - 3), color = bold_paint },
                new MagicaVoxelData { x = (byte)(start.x - 0), y = (byte)(start.y + 1), z = (byte)(start.z - 3), color = bold_paint }, bold_paint));
            vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - 2), y = (byte)(start.y + 2), z = (byte)(start.z - 3), color = bold_paint },
                new MagicaVoxelData { x = (byte)(start.x - 0), y = (byte)(start.y + 2), z = (byte)(start.z - 3), color = bold_paint }, bold_paint));

            for(int x = 0; x < 2; x++)
            {
                for(int y = 0; y < 4; y++)
                {
                    if(start.z < 120 - 2 && (length > 0 || x - 1 > length))
                        vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - x), y = (byte)(start.y + y), z = (byte)(start.z + 1), color = metal },
                        new MagicaVoxelData { x = (byte)(start.x - x + 1 + length), y = (byte)(start.y + y), z = (byte)(start.z + 2 + length), color = metal }, metal));
                }
            }

            for(int z = 0; z < 2; z++)
            {
                for(int y = 0; y < 4; y++)
                {
                    if(length > 0 || z - 1 > length)
                        vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + 1), y = (byte)(start.y + y), z = (byte)(start.z - z), color = metal },
                        new MagicaVoxelData { x = (byte)(start.x + 2 + length), y = (byte)(start.y + y), z = (byte)(start.z - z + 1 + length), color = metal }, metal));

                }
            }

            if(length > 0)
            {
                for(int y = 0; y < 4; y++)
                {
                    vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + 1), y = (byte)(start.y + y), z = (byte)(start.z + 1), color = metal },
                    new MagicaVoxelData { x = (byte)(start.x + 2 + length), y = (byte)(start.y + y), z = (byte)(start.z + 2 + length), color = metal }, metal));
                }
            }
            return vox;
        }
        private static List<MagicaVoxelData> generateDownwardMissileFieryTrailLarge(MagicaVoxelData start, int length)
        {
            List<MagicaVoxelData> vox = new List<MagicaVoxelData>(128);
            for(int x = 0; x < 3; x++)
            {
                for(int z = 0; z < 3; z++)
                {
                    vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - x), y = (byte)(start.y), z = (byte)(start.z - z), color = bold_paint },
                        new MagicaVoxelData { x = (byte)(start.x - x), y = (byte)(start.y + 3), z = (byte)(start.z - z), color = bold_paint }, bold_paint));
                }
            }

            vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - 3), y = (byte)(start.y + 1), z = (byte)(start.z), color = bold_paint },
                new MagicaVoxelData { x = (byte)(start.x - 3), y = (byte)(start.y + 1), z = (byte)(start.z - 3), color = bold_paint }, bold_paint));
            vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - 3), y = (byte)(start.y + 2), z = (byte)(start.z), color = bold_paint },
                new MagicaVoxelData { x = (byte)(start.x - 3), y = (byte)(start.y + 2), z = (byte)(start.z - 3), color = bold_paint }, bold_paint));
            vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - 2), y = (byte)(start.y + 1), z = (byte)(start.z - 3), color = bold_paint },
                new MagicaVoxelData { x = (byte)(start.x - 0), y = (byte)(start.y + 1), z = (byte)(start.z - 3), color = bold_paint }, bold_paint));
            vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - 2), y = (byte)(start.y + 2), z = (byte)(start.z - 3), color = bold_paint },
                new MagicaVoxelData { x = (byte)(start.x - 0), y = (byte)(start.y + 2), z = (byte)(start.z - 3), color = bold_paint }, bold_paint));

            for(int x = 0; x < 2; x++)
            {
                for(int y = 0; y < 4; y++)
                {
                    if(length > 0 || x - 1 > length)
                    {
                        vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - x), y = (byte)(start.y + y), z = (byte)(start.z + 1), color = metal },
                        new MagicaVoxelData { x = (byte)(start.x - x + 1 + length), y = (byte)(start.y + y), z = (byte)(start.z + 2 + length), color = metal }, metal));
                        if(x % 2 == 1)
                        {
                            vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - x + 1 + length), y = (byte)(start.y + y), z = (byte)(start.z + 2 + length), color = metal },
                                new MagicaVoxelData { x = (byte)(start.x - x + ((y > 0 && y < 3 && x < 2) ? 10 : 6) + length), y = (byte)(start.y + y), z = (byte)(start.z + ((y > 0 && y < 3 && x < 2) ? 10 : 6) + length), color = metal },
                                (y > 0 && y < 3 && x > 0) ? yellow_fire : orange_fire));
                        }
                    }
                }
            }

            for(int z = 0; z < 2; z++)
            {
                for(int y = 0; y < 4; y++)
                {
                    if(length > 0 || z - 1 > length)
                        vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + 1), y = (byte)(start.y + y), z = (byte)(start.z - z), color = metal },
                        new MagicaVoxelData { x = (byte)(start.x + 2 + length), y = (byte)(start.y + y), z = (byte)(start.z - z + 1 + length), color = metal }, metal));
                    if(z % 2 == 1)
                    {
                        vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + 2 + length), y = (byte)(start.y + y), z = (byte)(start.z - z + 1 + length), color = metal },
                            new MagicaVoxelData { x = (byte)(start.x + ((y > 0 && y < 3 && z < 2) ? 10 : 6) + length), y = (byte)(start.y + y), z = (byte)(start.z - z + ((y > 0 && y < 3 && z < 2) ? 10 : 6) + length), color = metal },
                            (y > 0 && y < 3 && z < 2) ? yellow_fire : orange_fire));
                    }
                }
            }
            if(length > 0)
            {
                for(int y = 0; y < 4; y++)
                {
                    vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + 1), y = (byte)(start.y + y), z = (byte)(start.z + 1), color = metal },
                    new MagicaVoxelData { x = (byte)(start.x + 2 + length), y = (byte)(start.y + y), z = (byte)(start.z + 2 + length), color = metal }, metal));

                    vox.AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + 2 + length), y = (byte)(start.y + y), z = (byte)(start.z + 2 + length), color = (byte)(yellow_fire) },
                        new MagicaVoxelData { x = (byte)(start.x + 2 + ((y > 0 && y < 3) ? 10 : 6) + length), y = (byte)(start.y + y), z = (byte)(start.z + ((y > 0 && y < 3) ? 10 : 6) + length), color = (byte)(yellow_fire) },
                        yellow_fire));
                }
            }
            return vox;
        }

        private static List<MagicaVoxelData> generateDownwardConeLarge(MagicaVoxelData start, int segments, int color)
        {
            List<MagicaVoxelData> cone = new List<MagicaVoxelData>(40);
            for(int x = 0; x < segments; x++)
            {
                for(float y = 0; y <= x * 0.75f; y++)
                {
                    for(float z = 0; z <= x * 1.25f; z++)
                    {
                        if(Math.Floor(y) == y && Math.Floor(z) == z)
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
            }
            return cone;
        }
        private static List<MagicaVoxelData> randomFill(int xStart, int yStart, int zStart, int xSize, int ySize, int zSize, int[] colors)
        {
            List<MagicaVoxelData> box = new List<MagicaVoxelData>(xSize * ySize * zSize);
            for(int x = 0; x < xSize; x++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int z = 0; z < zSize; z++)
                    {
                        if(zStart + z < 0) //x + y == xSize + ySize - 2 || x + z == xSize + zSize - 2 || z + y == zSize + ySize - 2 || 
                            continue;
                        box.Add(new MagicaVoxelData { x = (byte)(xStart + x), y = (byte)(yStart + y), z = (byte)(zStart + z), color = (byte)colors.RandomElement() });
                    }
                }
            }
            return box;

        }

        public static MagicaVoxelData[][] FireballLarge(MagicaVoxelData[] voxels, int blowback, int maxFrames, int trimLevel)
        {
            return FireballSwitchable(voxels, blowback, maxFrames, trimLevel, 120, 120, 80);
        }
        public static MagicaVoxelData[][] FireballSwitchable(MagicaVoxelData[] voxels, int blowback, int maxFrames, int trimLevel, int xSize, int ySize, int zSize)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[maxFrames + 1][];
            voxelFrames[0] = new MagicaVoxelData[voxels.Length];
            voxels.CopyTo(voxelFrames[0], 0);

            int xLimitLow = voxels.Min(v => v.x * (v.color <= emitter0 ? 1000 : 1)),
                xLimitHigh = voxels.Max(v => v.x * (v.color <= emitter0 ? 0 : 1)),
                xMiddle = (xLimitHigh + xLimitLow) / 2,
                xRange = xLimitHigh - xLimitLow,
                yLimitLow = voxels.Min(v => v.y * (v.color <= emitter0 ? 1000 : 1)),
                yLimitHigh = voxels.Max(v => v.y * (v.color <= emitter0 ? 0 : 1)),
                yMiddle = (yLimitHigh + yLimitLow) / 2,
                yRange = yLimitHigh - yLimitLow,
                zLimitLow = voxels.Min(v => v.z * (v.color <= emitter0 ? 1000 : 1)),
                zLimitHigh = voxels.Max(v => v.z * (v.color <= emitter0 ? 0 : 1)),
                zMiddle = (zLimitHigh + zLimitLow) / 2,
                zRange = zLimitHigh - zLimitLow;

            for(int f = 1; f <= maxFrames; f++)
            {
                List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxels.Length), working = new List<MagicaVoxelData>(voxelFrames[f - 1].Length * 2);
                MagicaVoxelData[] vls = new MagicaVoxelData[voxelFrames[f - 1].Length]; //.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)
                voxelFrames[f - 1].CopyTo(vls, 0);
                if(vls.Count() == 0)
                {
                    voxelFrames[f] = new MagicaVoxelData[0];
                    continue;
                }
                int[] minX = new int[zSize];
                int[] maxX = new int[zSize];
                float[] midX = new float[zSize];
                for(int level = 0; level < zSize; level++)
                {
                    minX[level] = vls.Min(v => v.x * ((v.z != level || v.color < VoxelLogic.clear) ? 1000 : 1));
                    maxX[level] = vls.Max(v => v.x * ((v.z != level || v.color < VoxelLogic.clear) ? 0 : 1));
                    midX[level] = (maxX[level] + minX[level]) / 2F;
                }

                int[] minY = new int[zSize];
                int[] maxY = new int[zSize];
                float[] midY = new float[zSize];
                for(int level = 0; level < zSize; level++)
                {
                    minY[level] = vls.Min(v => v.y * ((v.z != level || v.color < VoxelLogic.clear) ? 1000 : 1));
                    maxY[level] = vls.Max(v => v.y * ((v.z != level || v.color < VoxelLogic.clear) ? 0 : 1));
                    midY[level] = (maxY[level] + minY[level]) / 2F;
                }

                int minZ = vls.Min(v => v.z * ((v.color < VoxelLogic.clear) ? 1000 : 1));
                int maxZ = vls.Max(v => v.z * ((v.color < VoxelLogic.clear) ? 0 : 1));
                float midZ = (maxZ + minZ) / 2F;

                foreach(MagicaVoxelData v in vls)
                {
                    MagicaVoxelData mvd = new MagicaVoxelData();
                    int c = ((255 - v.color) % 4 == 0) ? (255 - v.color) / 4 + VoxelLogic.wcolorcount : (253 - v.color) / 4;
                    if(c == 8 || c == 9) //flesh
                        mvd.color = (byte)((r.Next(f) == 0) ? 253 - 34 * 4 : (r.Next(6) == 0 && f < 10) ? 253 - 19 * 4 : v.color); //random transform to guts
                    else if(c == 34) //guts
                        mvd.color = (byte)((r.Next(20) == 0 && f < 10) ? 253 - 19 * 4 : v.color); //random transform to orange fire
                    else if(c == VoxelLogic.wcolorcount - 1) //clear and markers
                        mvd.color = (byte)VoxelLogic.clear; //clear stays clear
                    else if(c == 16)
                        mvd.color = VoxelLogic.clear; //clear inner shadow
                    else if(c == 25)
                        mvd.color = 253 - 25 * 4; //shadow stays shadow
                    else if(c == 27)
                        mvd.color = 253 - 27 * 4; //water stays water
                    else if(c >= VoxelLogic.wcolorcount && c < VoxelLogic.wcolorcount + 5)
                        mvd.color = (byte)(255 - (c - VoxelLogic.wcolorcount) * 4); // falling water stays falling water
                    else if(c == 40)
                        mvd.color = 253 - 20 * 4; //flickering sparks become normal sparks
                    else if(c >= 21 && c <= 24) //lights
                        mvd.color = 253 - 35 * 4; //glass color for broken lights
                    else if(c == 35) //windows
                        mvd.color = (byte)((r.Next(3) == 0) ? VoxelLogic.clear : v.color); //random transform to clear
                    else if(c == 36) //rotor contrast
                        mvd.color = 253 - 0 * 4; //"foot contrast" color for broken rotors contrast
                    else if(c == 37) //rotor
                        mvd.color = 253 - 1 * 4; //"foot" color for broken rotors
                    else if(c == 38 || c == 39)
                        mvd.color = VoxelLogic.clear; //clear non-active rotors
                    else if(c == 19) //orange fire
                        mvd.color = (byte)((r.Next(9) + 2 <= f) ? 253 - 17 * 4 : ((r.Next(3) <= 1) ? 253 - 18 * 4 : ((r.Next(3) == 0) ? 253 - 17 * 4 : v.color))); //random transform to yellow fire or smoke
                    else if(c == 18) //yellow fire
                        mvd.color = (byte)((r.Next(9) + 1 <= f) ? 253 - 17 * 4 : ((r.Next(3) <= 1) ? 253 - 19 * 4 : ((r.Next(4) == 0) ? 253 - 17 * 4 : ((r.Next(4) == 0) ? 253 - 20 * 4 : v.color)))); //random transform to orange fire, smoke, or sparks
                    else if(c == 20) //sparks
                        mvd.color = (byte)((r.Next(4) > 0 && r.Next(12) > f) ? v.color : VoxelLogic.clear); //random transform to clear
                    else if(c == 17) //smoke
                        mvd.color = (byte)((r.Next(10) + 3 <= f) ? VoxelLogic.clear : 253 - 17 * 4); //random transform to clear
                    else
                        mvd.color = (byte)((r.Next(f * 4) <= 6) ? 253 - ((r.Next(4) == 0) ? 18 * 4 : 19 * 4) : v.color); //random transform to orange or yellow fire

                    float xMove = 0, yMove = 0, zMove = 0;
                    if(mvd.color == orange_fire || mvd.color == yellow_fire || mvd.color == smoke)
                    {
                        zMove = f * 0.8f;
                        xMove = (float)(r.NextDouble() * 2.0 - 1.0);
                        yMove = (float)(r.NextDouble() * 2.0 - 1.0);
                    }
                    else
                    {
                        if(v.x > midX[v.z])
                            xMove = ((blowback * 0.3f - r.Next(3) + (v.x - midX[v.z])) / (f + 8) * 25F * ((v.z - minZ + 1) / (maxZ - minZ + 1F)));
                        else if(v.x < midX[v.z])
                            xMove = ((blowback * 0.3f + r.Next(3) - midX[v.z] + v.x) / (f + 8) * 25F * ((v.z - minZ + 1) / (maxZ - minZ + 1F)));
                        if(v.y > midY[v.z])
                            yMove = ((0 - r.Next(3) + (v.y - midY[v.z])) / (f + 8) * 25F * ((v.z - minZ + 1) / (maxZ - minZ + 1F)));
                        else if(v.y < midY[v.z])
                            yMove = ((0 + r.Next(3) - midY[v.z] + v.y) / (f + 8) * 25F * ((v.z - minZ + 1) / (maxZ - minZ + 1F)));

                        if(mvd.color == 253 - 20 * 4)
                        {
                            zMove = 0.1f;
                            xMove *= 2;
                            yMove *= 2;
                        }
                        else if(mvd.color == orange_fire || mvd.color == yellow_fire || mvd.color == smoke)
                            zMove = f * 0.55F;
                        else if(f < (maxFrames - 4) && minZ <= 1)
                            zMove = (v.z / ((maxZ + 1) * (0.3F))) * ((maxFrames - 3) - f) * 0.8F;
                        else
                            zMove = (1 - f * 2.1F);
                    }
                    float magnitude = (float)Math.Sqrt(xMove * xMove + yMove * yMove);

                    if(xMove > 0)
                    {
                        //float nv = (v.x + (xMove / (0.2f * (f + 4)))) - Math.Abs((yMove / (0.5f * (f + 3))));
                        float nv = v.x + (float)r.NextDouble() * ((xMove / magnitude) * 35F / f) + (float)(r.NextDouble() * 8.0 - 4.0);
                        if(nv < 1) nv = 1;
                        if(nv > xSize - 2) nv = xSize - 2;
                        mvd.x = (byte)((blowback <= 0) ? Math.Floor(nv) : (Math.Ceiling(nv)));
                    }
                    else if(xMove < 0)
                    {
                        //float nv = (v.x + (xMove / (0.2f * (f + 4)))) + Math.Abs((yMove / (0.5f * (f + 3))));
                        float nv = v.x - (float)r.NextDouble() * ((xMove / magnitude) * -35F / f) + (float)(r.NextDouble() * 8.0 - 4.0);

                        if(nv < 1) nv = 1;
                        if(nv > xSize - 2) nv = xSize - 2;
                        mvd.x = (byte)((blowback > 0) ? Math.Floor(nv) : (Math.Ceiling(nv)));
                    }
                    else
                    {
                        if(v.x < 1) mvd.x = 1;
                        if(v.x > xSize - 2) mvd.x = (byte)(xSize - 2);
                        else mvd.x = v.x;
                    }
                    if(yMove > 0)
                    {
                        //float nv = (v.y + (yMove / (0.2f * (f + 4)))) - Math.Abs((xMove / (0.5f * (f + 3))));
                        float nv = v.y + (float)r.NextDouble() * ((yMove / magnitude) * 35F / f) + (float)(r.NextDouble() * 8.0 - 4.0);

                        if(nv < 1) nv = 1;
                        if(nv > ySize - 2) nv = ySize - 2;
                        mvd.y = (byte)(Math.Floor(nv));
                    }
                    else if(yMove < 0)
                    {
                        //float nv = (v.y + (yMove / (0.2f * (f + 4)))) + Math.Abs((xMove / (0.5f * (f + 3))));
                        float nv = v.y - (float)r.NextDouble() * ((yMove / magnitude) * -35F / f) + (float)(r.NextDouble() * 8.0 - 4.0);

                        if(nv < 1) nv = 1;
                        if(nv > ySize - 2) nv = ySize - 2;
                        mvd.y = (byte)(Math.Ceiling(nv));
                    }
                    else
                    {
                        mvd.y = v.y;
                    }
                    if(zMove != 0)
                    {
                        float nv = (v.z + (zMove / (0.35f + 0.14f * (f + 3))));

                        if(nv <= 0 && f < maxFrames && !(mvd.color == orange_fire || mvd.color == yellow_fire || mvd.color == smoke)) nv = r.Next(2); //bounce
                        else if(nv < 0) nv = 0;

                        if(nv > zSize - 1)
                        {
                            nv = zSize - 1;
                            mvd.color = VoxelLogic.clear;
                        }
                        mvd.z = (byte)Math.Round(nv);
                    }
                    else
                    {
                        mvd.z = v.z;
                    }
                    working.Add(mvd);
                    if(r.Next(maxFrames) > f + maxFrames / 6 && r.Next(maxFrames) > f + 2) working.AddRange(VoxelLogic.Adjacent(mvd, new int[] { orange_fire, yellow_fire, orange_fire, yellow_fire, smoke }));
                }
                working = working.Where(mvd => r.Next(7) < 7f - trimLevel
                - Math.Abs(mvd.x - xMiddle) * 1.6f / xRange
                - Math.Abs(mvd.y - yMiddle) * 1.6f / yRange
                - Math.Abs(mvd.z - zMiddle) * 1.6f / zRange).ToList();
                voxelFrames[f] = new MagicaVoxelData[working.Count];
                working.CopyTo(voxelFrames[f], 0);
            }
            MagicaVoxelData[][] frames = new MagicaVoxelData[maxFrames][];

            for(int f = 1; f <= maxFrames; f++)
            {
                frames[f - 1] = new MagicaVoxelData[voxelFrames[f].Length];
                voxelFrames[f].CopyTo(frames[f - 1], 0);
            }
            return frames;
        }

        public static MagicaVoxelData[][] HandgunAnimationLarge(MagicaVoxelData[][] parsedFrames, int unit, int which)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[parsedFrames.Length][];
            voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
            voxelFrames[parsedFrames.Length - 1] = new MagicaVoxelData[parsedFrames[parsedFrames.Length - 1].Length];
            parsedFrames[0].CopyTo(voxelFrames[0], 0);
            parsedFrames[parsedFrames.Length - 1].CopyTo(voxelFrames[parsedFrames.Length - 1], 0);
            List<MagicaVoxelData> launchers = new List<MagicaVoxelData>(4);
            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length - 2];
            foreach(MagicaVoxelData mvd in voxelFrames[0])
            {
                if(mvd.color == emitter0 - which * 8)
                {
                    launchers.Add(mvd);
                }
            }
            for(int f = 0; f < voxelFrames.Length - 2; f++) //going only through the middle
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
                if(currentlyFiring < launchers.Count)
                {
                    extra[f].AddRange(VoxelLogic.generateBox(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x), y = (byte)(launchers[currentlyFiring].y), z = (byte)(launchers[currentlyFiring].z), color = orange_fire }, 8, 2, 2, yellow_fire));
                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x + 2), y = (byte)(launchers[currentlyFiring].y + 2), z = (byte)(launchers[currentlyFiring].z), color = yellow_fire }, orange_fire));
                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x + 2), y = (byte)(launchers[currentlyFiring].y - 2), z = (byte)(launchers[currentlyFiring].z), color = yellow_fire }, orange_fire));
                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x + 2), y = (byte)(launchers[currentlyFiring].y), z = (byte)(launchers[currentlyFiring].z + 2), color = yellow_fire }, orange_fire));
                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x + 2), y = (byte)(launchers[currentlyFiring].y), z = (byte)(launchers[currentlyFiring].z - 2), color = yellow_fire }, orange_fire));
                }
                if(currentlyFiring <= launchers.Count && currentlyFiring > 0)
                {
                    extra[f].AddRange(VoxelLogic.generateBox(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring - 1].x + 4), y = (byte)(launchers[currentlyFiring - 1].y), z = (byte)(launchers[currentlyFiring - 1].z), color = orange_fire }, 6, 2, 2, yellow_fire));
                }
            }
            for(int f = 1; f < voxelFrames.Length - 1; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(parsedFrames[f]);
                working.AddRange(extra[f - 1]);
                voxelFrames[f] = working.ToArray();
            }
            return voxelFrames;
        }
        public static MagicaVoxelData[][] HandgunReceiveAnimationLarge(MagicaVoxelData[][] parsedFrames, int strength)
        {
            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[parsedFrames.Length];
            MagicaVoxelData[][] finalFrames = new MagicaVoxelData[parsedFrames.Length][];

            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length];
            MagicaVoxelData[][][] plumes = new MagicaVoxelData[strength * 2][][];
            MagicaVoxelData[][][] bursts = new MagicaVoxelData[strength * 2][][];
            for(int s = 0; s < 2 * strength; s++)
            {
                plumes[s] = SmokePlumeLarge(new MagicaVoxelData
                {
                    x = (byte)((s % 2 == 0) ? 90 - r.Next(20) : r.Next(20) + 30),
                    y = (byte)((s % 2 == 0) ? (r.Next(30) + 20) : (100 - r.Next(30))),
                    z = 0,
                    color = smoke
                }, 8, 7);
                bursts[s] = BurstLarge(new MagicaVoxelData
                {
                    x = (byte)(35 + r.Next(3)),
                    y = (byte)(32 - r.Next(4)),
                    z = (byte)(7 + r.Next(3)),
                    color = yellow_fire
                }, 3, s >= strength);
            }
            for(int f = 0; f < voxelFrames.Length; f++)
            {
                extra[f] = new List<MagicaVoxelData>(20);
            }
            int secondMiss = 0, secondHit = 0;
            for(int f = 0; f < voxelFrames.Length - 2; f++)
            {
                int currentlyMissing = f, currentlyHitting = f + 4;
                if(currentlyMissing % 8 < f)
                {
                    currentlyMissing %= 8;
                    secondMiss ^= 1;
                }
                if(currentlyHitting % 8 < f)
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
                if(currentlyMissing < strength)
                {
                    for(int p = 0; p < 7 && f + p < parsedFrames.Length; p++)
                    {
                        extra[f + p].AddRange(plumes[currentlyMissing + strength * secondMiss][p]);
                    }
                }
                if(currentlyHitting < strength)
                {
                    for(int b = 0; b < 3 && f + b < parsedFrames.Length; b++)
                    {
                        extra[f + b].AddRange(bursts[currentlyHitting + strength * secondHit][b]);
                    }
                }
            }
            for(int f = 0; f < voxelFrames.Length; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(20);
                working.AddRange(extra[f]);
                finalFrames[f] = working.ToArray();
            }
            return finalFrames;
        }
        public static MagicaVoxelData[][] MachineGunAnimationLarge(MagicaVoxelData[][] parsedFrames, int unit, int which)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[parsedFrames.Length][];
            voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
            voxelFrames[parsedFrames.Length - 1] = new MagicaVoxelData[parsedFrames[parsedFrames.Length - 1].Length];
            parsedFrames[0].CopyTo(voxelFrames[0], 0);
            parsedFrames[parsedFrames.Length - 1].CopyTo(voxelFrames[parsedFrames.Length - 1], 0);
            List<MagicaVoxelData> launchers = new List<MagicaVoxelData>(40);
            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length - 2];

            List<int> known = new List<int>(30);
            foreach(MagicaVoxelData mvd in voxelFrames[0])
            {
                if(mvd.color == emitter0 - which * 8)
                {
                    launchers.Add(mvd);
                }
            }

            for(int f = 0; f < voxelFrames.Length - 2; f++) //going only through the middle
            {

                int currentlyFiring = f % (launchers.Count / 4 + 1);
                extra[f] = new List<MagicaVoxelData>(1024);
                //if (f > 0)
                //{
                //    for (int i = 0; i < extra[f-1].Count; i++)
                //    {
                //        extra[f].Add(new MagicaVoxelData { x = (byte)(extra[f-1][i].x + 2), y = (byte)(extra[f-1][i].y + Math.Round(r.NextDouble() * 1.1 - 0.55)),
                //            z = extra[f-1][i].z, color = extra[f-1][i].color });
                //    }
                //}
                if(currentlyFiring % 2 == 0)
                {

                    foreach(MagicaVoxelData launcher in launchers)
                    {
                        if(currentlyFiring != 0)
                        {
                            currentlyFiring = (currentlyFiring + 1) % (launchers.Count / 4 + 1);
                            continue;
                        }
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 7), y = launcher.y, z = launcher.z, color = (byte)(yellow_fire) }, yellow_fire));

                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 5), y = launcher.y, z = (byte)(launcher.z + 2), color = yellow_fire }, orange_fire));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 5), y = launcher.y, z = (byte)(launcher.z - 2), color = yellow_fire }, orange_fire));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 2), z = launcher.z, color = yellow_fire }, orange_fire));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 2), z = launcher.z, color = yellow_fire }, orange_fire));

                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 2), color = yellow_fire }, orange_fire));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 2), color = yellow_fire }, orange_fire));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z - 2), color = yellow_fire }, orange_fire));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 2), z = (byte)(launcher.z - 2), color = yellow_fire }, orange_fire));

                    }
                    extra[f] = extra[f].Where(v => r.Next(10) > 1).ToList();

                }
                else
                {
                    foreach(MagicaVoxelData launcher in launchers)
                    {
                        if(currentlyFiring < 2 && launchers.Count > 8)
                        {
                            currentlyFiring = (currentlyFiring + 1) % (launchers.Count / 4 + 1);
                            continue;
                        }
                        extra[f].AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = launcher.z, color = orange_fire }
                            , new MagicaVoxelData { x = (byte)(launcher.x + 9), y = launcher.y, z = launcher.z, color = orange_fire }, orange_fire));

                        currentlyFiring = (currentlyFiring + 1) % (launchers.Count / 4 + 1);
                    }
                    extra[f] = extra[f].Where(v => r.Next(10) > 2).ToList();

                }

            }
            for(int f = 1; f < voxelFrames.Length - 1; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(parsedFrames[f]);
                working.AddRange(extra[f - 1]);
                voxelFrames[f] = working.ToArray();
            }
            return voxelFrames;
        }
        public static MagicaVoxelData[][] MachineGunReceiveAnimationLarge(MagicaVoxelData[][] parsedFrames, int strength)
        {
            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[parsedFrames.Length];
            MagicaVoxelData[][] finalFrames = new MagicaVoxelData[parsedFrames.Length][];

            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length];
            MagicaVoxelData[][][] sparks = new MagicaVoxelData[strength][][];
            for(int s = 0; s < strength; s++)
            {
                sparks[s] = SparksLarge(new MagicaVoxelData
                {
                    x = (byte)(66 + r.Next(10)),
                    y = (byte)(27 + s * 12),
                    z = 0,
                    color = yellow_fire
                }, 2 * (3 + strength));
            }
            for(int f = 0; f < voxelFrames.Length; f++)
            {
                extra[f] = new List<MagicaVoxelData>(160);
            }
            for(int f = 0; f < voxelFrames.Length - 1; f++)
            {
                //if (f > 0)
                //{
                //    for (int i = 0; i < extra[f-1].Count; i++)
                //    {
                //        extra[f].Add(new MagicaVoxelData { x = (byte)(extra[f-1][i].x + 2), y = (byte)(extra[f-1][i].y + Math.Round(r.NextDouble() * 1.1 - 0.55)),
                //            z = extra[f-1][i].z, color = extra[f-1][i].color });
                //    }
                //}
                for(int sp = 0; sp < sparks.Length; sp++)
                    extra[f + 1].AddRange(sparks[sp][f]);

            }
            for(int f = 0; f < voxelFrames.Length; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(20);
                working.AddRange(extra[f]);
                finalFrames[f] = working.ToArray();
            }
            return finalFrames;
        }

        public static MagicaVoxelData[][] CannonAnimationLarge(MagicaVoxelData[][] parsedFrames, int unit, int which)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[parsedFrames.Length][];
            voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
            voxelFrames[parsedFrames.Length - 1] = new MagicaVoxelData[parsedFrames[parsedFrames.Length - 1].Length];
            parsedFrames[0].CopyTo(voxelFrames[0], 0);
            parsedFrames[parsedFrames.Length - 1].CopyTo(voxelFrames[parsedFrames.Length - 1], 0);
            List<MagicaVoxelData> launchers = new List<MagicaVoxelData>(40);
            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length - 2];

            List<int> known = new List<int>(30);
            foreach(MagicaVoxelData mvd in voxelFrames[0])
            {
                if(mvd.color == emitter0 - which * 8)
                {
                    launchers.Add(mvd);
                }
            }
            int maxY = launchers.Max(v => v.y);
            int minY = launchers.Max(v => v.y);
            float midY = (maxY + minY) / 2F;
            //            List<MagicaVoxelData>[] halves = { launchers.Where(mvd => (mvd.y <= midY)).ToList(), launchers.Where(mvd => (mvd.y > midY)).ToList() };
            List<MagicaVoxelData>[] halves = { launchers.ToList(), launchers.ToList() };

            for(int f = 0; f < voxelFrames.Length - 2; f++) //going only through the middle
            {
                extra[f] = new List<MagicaVoxelData>(1024);

                //if (f > 0)
                //{
                //    for (int i = 0; i < extra[f-1].Count; i++)
                //    {
                //        extra[f].Add(new MagicaVoxelData { x = (byte)(extra[f-1][i].x + 2), y = (byte)(extra[f-1][i].y + Math.Round(r.NextDouble() * 1.1 - 0.55)),
                //            z = extra[f-1][i].z, color = extra[f-1][i].color });
                //    }
                //}
                if(f == 0 || f == 1)
                {
                    foreach(MagicaVoxelData launcher in halves[0])
                    {
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 12), y = launcher.y, z = launcher.z, color = (byte)(yellow_fire) }, (yellow_fire - (r.Next(2) * 4))));

                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = launcher.y, z = (byte)(launcher.z + 8), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = launcher.y, z = (byte)(launcher.z - 8), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = (byte)(launcher.y + 8), z = launcher.z, color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = (byte)(launcher.y - 8), z = launcher.z, color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));

                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = (byte)(launcher.y + 5), z = (byte)(launcher.z + 5), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = (byte)(launcher.y + 5), z = (byte)(launcher.z - 5), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = (byte)(launcher.y - 5), z = (byte)(launcher.z + 5), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = (byte)(launcher.y - 5), z = (byte)(launcher.z - 5), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));

                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = (byte)(launcher.y + 3), z = (byte)(launcher.z + 5), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = (byte)(launcher.y + 3), z = (byte)(launcher.z - 5), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = (byte)(launcher.y - 3), z = (byte)(launcher.z + 5), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = (byte)(launcher.y - 3), z = (byte)(launcher.z - 5), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));

                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = (byte)(launcher.y + 5), z = (byte)(launcher.z + 3), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = (byte)(launcher.y + 5), z = (byte)(launcher.z - 3), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = (byte)(launcher.y - 5), z = (byte)(launcher.z + 3), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = (byte)(launcher.y - 5), z = (byte)(launcher.z - 3), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));

                        extra[f] = extra[f].Where(v => r.Next(10) > 0).ToList();

                    }
                }
                else if(f == 1 || f == 2)
                {

                    foreach(MagicaVoxelData launcher in halves[1])
                    {

                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = launcher.y, z = launcher.z, color = (byte)(yellow_fire) }, (yellow_fire - (r.Next(2) * 4))));

                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = launcher.y, z = (byte)(launcher.z + 6), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = launcher.y, z = (byte)(launcher.z - 6), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y + 6), z = launcher.z, color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y - 6), z = launcher.z, color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));

                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y + 4), z = (byte)(launcher.z + 4), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y + 4), z = (byte)(launcher.z - 4), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y - 4), z = (byte)(launcher.z + 4), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y - 4), z = (byte)(launcher.z - 4), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));

                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 4), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y + 2), z = (byte)(launcher.z - 4), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 4), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y - 2), z = (byte)(launcher.z - 4), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));

                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y + 4), z = (byte)(launcher.z + 2), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y + 4), z = (byte)(launcher.z - 2), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y - 4), z = (byte)(launcher.z + 2), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y - 4), z = (byte)(launcher.z - 2), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));

                        extra[f] = extra[f].Where(v => r.Next(10) > 0).ToList();

                    }
                }
                else if(f == 3)
                {
                    foreach(MagicaVoxelData launcher in launchers)
                    {
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 5), y = launcher.y, z = launcher.z, color = (byte)(yellow_fire) }, smoke));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 2), color = yellow_fire }, smoke));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 2), z = launcher.z, color = yellow_fire }, smoke));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 2), color = yellow_fire }, smoke));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 2), z = launcher.z, color = yellow_fire }, smoke));
                    }
                    extra[f] = extra[f].Where(v => r.Next(6) > 2).ToList();

                }
                else if(f == 4)
                {
                    foreach(MagicaVoxelData launcher in launchers)
                    {
                        extra[f].AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 2), color = (byte)(smoke) },
                            new MagicaVoxelData { x = (byte)(launcher.x + 5), y = launcher.y, z = launcher.z, color = (byte)(yellow_fire) }, smoke));

                        extra[f].AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 2), color = (byte)(smoke) },
                            new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 4), color = (byte)(yellow_fire) }, smoke));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 2), color = (byte)(smoke) },
                            new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 2), color = (byte)(yellow_fire) }, smoke));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 2), color = (byte)(smoke) },
                            new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 4), color = (byte)(yellow_fire) }, smoke));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 2), color = (byte)(smoke) },
                            new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 2), color = (byte)(yellow_fire) }, smoke));

                    }
                    extra[f] = extra[f].Where(v => r.Next(6) > 2 && r.Next(6) > 1).ToList();

                }
                else if(f == 5)
                {
                    foreach(MagicaVoxelData launcher in launchers)
                    {
                        extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 5), z = (byte)(launcher.z + 5), color = smoke }, smoke));
                        extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 5), z = (byte)(launcher.z + 5), color = smoke }, smoke));
                        extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 5), z = (byte)(launcher.z + 5), color = smoke }, smoke));
                        extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 5), z = (byte)(launcher.z + 5), color = smoke }, smoke));
                    }
                    extra[f] = extra[f].Where(v => r.Next(6) > 3 && r.Next(6) > 2).ToList();

                }
                else if(f == 6)
                {
                    foreach(MagicaVoxelData launcher in launchers)
                    {

                        extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 7), z = (byte)(launcher.z + 5), color = smoke }, smoke));
                        extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 7), z = (byte)(launcher.z + 5), color = smoke }, smoke));
                        extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 3 + r.Next(6)), z = (byte)(launcher.z + 9), color = smoke }, smoke));
                        extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 3 - r.Next(6)), z = (byte)(launcher.z + 9), color = smoke }, smoke));
                    }
                    extra[f] = extra[f].Where(v => r.Next(6) > 4 && r.Next(6) > 3).ToList();

                }
                else if(f == 7)
                {
                    foreach(MagicaVoxelData launcher in launchers)
                    {

                        extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 7), z = (byte)(launcher.z + 6), color = smoke }, smoke));
                        extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 7), z = (byte)(launcher.z + 6), color = smoke }, smoke));
                        extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 3 + r.Next(6)), z = (byte)(launcher.z + 10), color = smoke }, smoke));
                        extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 3 - r.Next(6)), z = (byte)(launcher.z + 10), color = smoke }, smoke));
                    }
                    extra[f] = extra[f].Where(v => r.Next(7) > 5 && r.Next(6) > 4).ToList();

                }
            }
            for(int f = 1; f < voxelFrames.Length - 1; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(parsedFrames[f]);
                if(f == 2 || f == 4)
                {
                    for(int i = 0; i < working.Count; i++)
                    {
                        working[i] = new MagicaVoxelData { x = (byte)(working[i].x - 1), y = working[i].y, z = working[i].z, color = working[i].color };
                    }
                }
                else if(f == 3)
                {
                    for(int i = 0; i < working.Count; i++)
                    {
                        working[i] = new MagicaVoxelData { x = (byte)(working[i].x - 2), y = working[i].y, z = working[i].z, color = working[i].color };
                    }
                }
                working.AddRange(extra[f - 1]);
                voxelFrames[f] = working.ToArray();
            }
            return voxelFrames;
        }
        public static MagicaVoxelData[][] CannonReceiveAnimationLarge(MagicaVoxelData[][] parsedFrames, int strength)
        {
            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[parsedFrames.Length];
            MagicaVoxelData[][] finalFrames = new MagicaVoxelData[parsedFrames.Length][];

            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length];
            MagicaVoxelData[][][] explosions = new MagicaVoxelData[strength][][];
            for(int s = 0; s < strength; s++)
            {
                explosions[s] = FireballLarge(randomFill(75, 56 + s, 16 + s, 4, 4, 4, new int[] { orange_fire, orange_fire, yellow_fire, yellow_fire, smoke }).ToArray(), 3, 6 + strength, 2);
            }
            for(int f = 0; f < voxelFrames.Length; f++)
            {
                extra[f] = new List<MagicaVoxelData>(400);
            }

            for(int i = 0; i < 6 + strength; i++)
            {
                for(int sp = 0; sp < explosions.Length; sp++)
                    extra[3 + i].AddRange(explosions[sp][i]);
            }

            for(int f = 0; f < voxelFrames.Length; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(400);
                working.AddRange(extra[f]);
                finalFrames[f] = working.ToArray();
            }
            return finalFrames;
        }

        public static MagicaVoxelData[][] LongCannonAnimationLarge(MagicaVoxelData[][] parsedFrames, int unit, int which)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[parsedFrames.Length][];
            voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
            voxelFrames[parsedFrames.Length - 1] = new MagicaVoxelData[parsedFrames[parsedFrames.Length - 1].Length];
            parsedFrames[0].CopyTo(voxelFrames[0], 0);
            parsedFrames[parsedFrames.Length - 1].CopyTo(voxelFrames[parsedFrames.Length - 1], 0);
            List<MagicaVoxelData> launchers = new List<MagicaVoxelData>(40);
            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length - 2];

            List<int> known = new List<int>(30);
            foreach(MagicaVoxelData mvd in voxelFrames[0])
            {
                if(mvd.color == emitter0 - which * 8)
                {
                    launchers.Add(mvd);
                }
            }

            //            List<MagicaVoxelData>[] halves = { launchers.Where(mvd => (mvd.y <= midY)).ToList(), launchers.Where(mvd => (mvd.y > midY)).ToList() };

            for(int f = 0; f < voxelFrames.Length - 2; f++) //going only through the middle
            {
                extra[f] = new List<MagicaVoxelData>(1024);

                //if (f > 0)
                //{
                //    for (int i = 0; i < extra[f-1].Count; i++)
                //    {
                //        extra[f].Add(new MagicaVoxelData { x = (byte)(extra[f-1][i].x + 2), y = (byte)(extra[f-1][i].y + Math.Round(r.NextDouble() * 1.1 - 0.55)),
                //            z = extra[f-1][i].z, color = extra[f-1][i].color });
                //    }
                //}
                if(f == 0 || f == 1 || f == 2)
                {
                    foreach(MagicaVoxelData launcher in launchers)
                    {
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 12), y = (byte)(launcher.y + 0), z = (byte)(launcher.z + 12), color = (byte)(yellow_fire) }, (yellow_fire - (r.Next(2) * 4))));

                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y + 0), z = (byte)(launcher.z + 12), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y + 0), z = (byte)(launcher.z + 4), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y + 4), z = (byte)(launcher.z + 8), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y - 4), z = (byte)(launcher.z + 8), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));

                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y + 5), z = (byte)(launcher.z + 4), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y + 5), z = (byte)(launcher.z + 12), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y - 5), z = (byte)(launcher.z + 4), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y - 5), z = (byte)(launcher.z + 12), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));

                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y + 4), z = (byte)(launcher.z + 4), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y + 4), z = (byte)(launcher.z + 12), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y - 4), z = (byte)(launcher.z + 4), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y - 4), z = (byte)(launcher.z + 12), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));

                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 6), y = (byte)(launcher.y + 6), z = (byte)(launcher.z + 4), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 6), y = (byte)(launcher.y + 6), z = (byte)(launcher.z + 8), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 6), y = (byte)(launcher.y - 6), z = (byte)(launcher.z + 4), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 6), y = (byte)(launcher.y - 6), z = (byte)(launcher.z + 8), color = yellow_fire }, (yellow_fire - (r.Next(2) * 4))));

                    }
                    extra[f] = extra[f].GroupBy(mvd => mvd.x + mvd.y * 256 + mvd.z * 256 * 256).Select(g => g.First()).Where(v => r.Next(6) > f && r.Next(5) > 1).ToList();

                }
                else if(f == 3)
                {
                    foreach(MagicaVoxelData launcher in launchers)
                    {
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 5), y = launcher.y, z = launcher.z, color = (byte)(yellow_fire) }, smoke));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 1), color = yellow_fire }, smoke));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 3), color = yellow_fire }, smoke));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 1), color = yellow_fire }, smoke));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 3), color = yellow_fire }, smoke));
                    }
                    extra[f] = extra[f].GroupBy(mvd => mvd.x + mvd.y * 256 + mvd.z * 256 * 256).Select(g => g.First()).Where(v => r.Next(6) > 2).ToList();

                }
                else if(f == 4)
                {
                    foreach(MagicaVoxelData launcher in launchers)
                    {
                        extra[f].AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 1), color = (byte)(smoke) },
                            new MagicaVoxelData { x = (byte)(launcher.x + 5), y = launcher.y, z = (byte)(launcher.z + 3), color = (byte)(yellow_fire) }, smoke));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 3), color = (byte)(smoke) },
                            new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 5), color = (byte)(yellow_fire) }, smoke));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 2), color = (byte)(smoke) },
                            new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 4), color = (byte)(yellow_fire) }, smoke));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 3), color = (byte)(smoke) },
                            new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 5), color = (byte)(yellow_fire) }, smoke));
                        extra[f].AddRange(VoxelLogic.generateStraightLine(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 2), color = (byte)(smoke) },
                            new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 4), color = (byte)(yellow_fire) }, smoke));

                    }
                    extra[f] = extra[f].Where(v => r.Next(6) > 2 && r.Next(6) > 1).ToList();

                }
                else if(f == 5)
                {
                    foreach(MagicaVoxelData launcher in launchers)
                    {
                        extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 5), z = (byte)(launcher.z + 7), color = smoke }, smoke));
                        extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 5), z = (byte)(launcher.z + 7), color = smoke }, smoke));
                        extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 5), z = (byte)(launcher.z + 7), color = smoke }, smoke));
                        extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 5), z = (byte)(launcher.z + 7), color = smoke }, smoke));
                    }
                    extra[f] = extra[f].Where(v => r.Next(6) > 3 && r.Next(6) > 2).ToList();

                }
                else if(f == 6)
                {
                    foreach(MagicaVoxelData launcher in launchers)
                    {

                        extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 7), z = (byte)(launcher.z + 7), color = smoke }, smoke));
                        extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 7), z = (byte)(launcher.z + 7), color = smoke }, smoke));
                        extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 3 + r.Next(6)), z = (byte)(launcher.z + 10), color = smoke }, smoke));
                        extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 3 - r.Next(6)), z = (byte)(launcher.z + 10), color = smoke }, smoke));
                    }
                    extra[f] = extra[f].Where(v => r.Next(6) > 4 && r.Next(6) > 3).ToList();

                }
                else if(f == 7)
                {
                    foreach(MagicaVoxelData launcher in launchers)
                    {

                        extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 7), z = (byte)(launcher.z + 8), color = smoke }, smoke));
                        extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 7), z = (byte)(launcher.z + 8), color = smoke }, smoke));
                        extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 3 + r.Next(6)), z = (byte)(launcher.z + 12), color = smoke }, smoke));
                        extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 3 - r.Next(6)), z = (byte)(launcher.z + 12), color = smoke }, smoke));
                    }
                    extra[f] = extra[f].Where(v => r.Next(7) > 5 && r.Next(6) > 4).ToList();

                }
            }
            for(int f = 1; f < voxelFrames.Length - 1; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(parsedFrames[f]);
                if(f == 2 || f == 4)
                {
                    for(int i = 0; i < working.Count; i++)
                    {
                        working[i] = new MagicaVoxelData { x = (byte)(working[i].x - 1), y = working[i].y, z = working[i].z, color = working[i].color };
                    }
                }
                else if(f == 3)
                {
                    for(int i = 0; i < working.Count; i++)
                    {
                        working[i] = new MagicaVoxelData { x = (byte)(working[i].x - 2), y = working[i].y, z = working[i].z, color = working[i].color };
                    }
                }
                working.AddRange(extra[f - 1]);
                voxelFrames[f] = working.ToArray();
            }
            return voxelFrames;
        }

        public static MagicaVoxelData[][] LongCannonReceiveAnimationLarge(MagicaVoxelData[][] parsedFrames, int strength)
        {
            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[parsedFrames.Length];
            MagicaVoxelData[][] finalFrames = new MagicaVoxelData[parsedFrames.Length][];

            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length];
            MagicaVoxelData[][][] explosions = new MagicaVoxelData[strength][][];
            for(int s = 0; s < strength; s++)
            {
                explosions[s] = FireballLarge(randomFill(75, 56 + s, 0, 4, 4, 8, new int[] { smoke, orange_fire, smoke, orange_fire, yellow_fire }).ToArray(), 1, 5 + strength, 2);
            }
            for(int f = 0; f < voxelFrames.Length; f++)
            {
                extra[f] = new List<MagicaVoxelData>(600);
            }

            for(int i = 0; i < 5 + strength; i++)
            {
                for(int sp = 0; sp < explosions.Length; sp++)
                    extra[4 + i].AddRange(explosions[sp][i]);
            }
            for(int f = 0; f < voxelFrames.Length; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(600);
                working.AddRange(extra[f]);
                finalFrames[f] = working.ToArray();
            }
            return finalFrames;
        }

        public static MagicaVoxelData[][] RocketAnimationLarge(MagicaVoxelData[][] parsedFrames, int unit, int which)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[parsedFrames.Length][];
            voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
            voxelFrames[parsedFrames.Length - 1] = new MagicaVoxelData[parsedFrames[parsedFrames.Length - 1].Length];
            parsedFrames[0].CopyTo(voxelFrames[0], 0);
            parsedFrames[parsedFrames.Length - 1].CopyTo(voxelFrames[parsedFrames.Length - 1], 0);
            List<MagicaVoxelData> launchers = new List<MagicaVoxelData>(4), trails = new List<MagicaVoxelData>(4);
            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length - 2], missile = new List<MagicaVoxelData>[voxelFrames.Length - 2];
            foreach(MagicaVoxelData mvd in voxelFrames[0])
            {
                if(mvd.color == emitter0 - which * 8)
                {
                    launchers.Add(mvd);
                }
                else if(mvd.color == trail0 - which * 8)
                {
                    trails.Add(mvd);
                }
            }
            int maxY = launchers.Max(v => v.y);
            int minY = launchers.Min(v => v.y);
            int midY = (maxY + minY) / 2;
            MagicaVoxelData launcher = launchers.OrderBy(mvd => mvd.z * 3 + mvd.x + mvd.y).First();
            launcher.y = (byte)midY;
            MagicaVoxelData trail = trails.OrderBy(mvd => mvd.z * 3 + mvd.x + mvd.y).First();
            for(int f = 0; f < voxelFrames.Length - 2; f++) //going only through the middle
            {
                extra[f] = new List<MagicaVoxelData>(20);
                missile[f] = new List<MagicaVoxelData>(20);

                if(f > 1)
                {
                    for(int i = 0; i < missile[f - 1].Count; i++)
                    {
                        missile[f].Add(new MagicaVoxelData
                        {
                            x = (byte)(missile[f - 1][i].x + 8),
                            y = (byte)(missile[f - 1][i].y),
                            z = missile[f - 1][i].z,
                            color = missile[f - 1][i].color
                        });
                    }
                }
                if(f == 0)
                {
                    missile[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x), y = launcher.y, z = (byte)(launcher.z), color = bold_paint }, bold_paint));
                }
                if(f == 1)
                {
                    missile[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 6), y = launcher.y, z = (byte)(launcher.z), color = bold_paint }, bold_paint));
                    missile[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = launcher.y, z = (byte)(launcher.z), color = bold_paint }, metal));
                    missile[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z), color = bold_paint }, metal));
                    missile[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 0), y = launcher.y, z = (byte)(launcher.z), color = bold_paint }, metal));

                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 2), y = trail.y, z = (byte)(trail.z), color = yellow_fire }, yellow_fire));
                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 4), y = (byte)(trail.y), z = (byte)(trail.z), color = yellow_fire }, yellow_fire));

                    extra[f].AddRange(VoxelLogic.generateBox(new MagicaVoxelData { x = (byte)(trail.x - 3), y = (byte)(trail.y - 1), z = (byte)(trail.z - 1), color = orange_fire }, 3, 4, 4, orange_fire));

                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 2), y = (byte)(trail.y + 2), z = (byte)(trail.z), color = yellow_fire }, orange_fire));
                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 2), y = (byte)(trail.y - 2), z = (byte)(trail.z), color = yellow_fire }, orange_fire));
                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 2), y = (byte)(trail.y), z = (byte)(trail.z + 2), color = yellow_fire }, orange_fire));
                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 2), y = (byte)(trail.y), z = (byte)(trail.z - 2), color = yellow_fire }, orange_fire));
                }
                else if(f == 2)
                {
                    missile[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 6), y = launcher.y, z = (byte)(launcher.z), color = bold_paint }, metal));
                    missile[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = launcher.y, z = (byte)(launcher.z), color = bold_paint }, metal));
                    missile[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z), color = bold_paint }, metal));
                    missile[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x), y = launcher.y, z = (byte)(launcher.z), color = bold_paint }, metal));

                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 2), y = trail.y, z = (byte)(trail.z), color = yellow_fire }, yellow_fire));
                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 4), y = trail.y, z = (byte)(trail.z), color = yellow_fire }, yellow_fire));

                    extra[f].AddRange(VoxelLogic.generateBox(new MagicaVoxelData { x = (byte)(trail.x - 3), y = (byte)(trail.y - 1), z = (byte)(trail.z - 1), color = orange_fire }, 3, 4, 4, orange_fire));

                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 2), y = (byte)(trail.y + 2), z = (byte)(trail.z), color = yellow_fire }, orange_fire));
                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 2), y = (byte)(trail.y - 2), z = (byte)(trail.z), color = yellow_fire }, orange_fire));
                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 2), y = (byte)(trail.y), z = (byte)(trail.z + 2), color = yellow_fire }, orange_fire));
                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 2), y = (byte)(trail.y), z = (byte)(trail.z - 2), color = yellow_fire }, orange_fire));

                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 4), y = (byte)(trail.y + 2), z = (byte)(trail.z), color = smoke }, smoke));
                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 4), y = (byte)(trail.y - 2), z = (byte)(trail.z), color = smoke }, smoke));
                    extra[f].AddRange(VoxelLogic.generateBox(new MagicaVoxelData { x = (byte)(trail.x - 6), y = (byte)(trail.y - 2), z = (byte)(trail.z), color = orange_fire }, 2, 6, 2, smoke));

                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 2), y = (byte)(trail.y + 2), z = (byte)(trail.z + 2), color = smoke }, smoke));
                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 2), y = (byte)(trail.y - 2), z = (byte)(trail.z + 2), color = smoke }, smoke));

                    extra[f].AddRange(VoxelLogic.generateBox(new MagicaVoxelData { x = (byte)(trail.x - 6), y = (byte)(trail.y - 2), z = (byte)(trail.z + 2), color = orange_fire }, 4, 6, 2, smoke));
                }
                else if(f == 3)
                {

                    missile[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 6), y = launcher.y, z = (byte)(launcher.z), color = bold_paint }, yellow_fire));
                    missile[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = launcher.y, z = (byte)(launcher.z), color = bold_paint }, yellow_fire));
                    missile[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z), color = bold_paint }, yellow_fire));

                    extra[f].AddRange(VoxelLogic.generateBox(new MagicaVoxelData { x = (byte)(trail.x + 5), y = (byte)(trail.y - 1), z = (byte)(trail.z - 1), color = orange_fire }, 3, 4, 4, orange_fire));
                    extra[f].AddRange(VoxelLogic.generateBox(new MagicaVoxelData { x = (byte)(trail.x + 6), y = (byte)(trail.y - 2), z = (byte)(trail.z + 0), color = orange_fire }, 2, 6, 2, orange_fire));
                    extra[f].AddRange(VoxelLogic.generateBox(new MagicaVoxelData { x = (byte)(trail.x + 6), y = (byte)(trail.y + 0), z = (byte)(trail.z - 2), color = orange_fire }, 2, 2, 6, orange_fire));

                    extra[f].AddRange(VoxelLogic.generateBox(new MagicaVoxelData { x = (byte)(trail.x - 8), y = (byte)(trail.y - 2), z = (byte)(trail.z + 0), color = orange_fire }, 6, 6, 4, smoke));

                    extra[f] = extra[f].Where(v => r.Next(5) > 0).ToList();

                }
                else if(f == 4)
                {
                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 6), y = (byte)(trail.y + 2 + (r.Next(5) - 2)), z = (byte)(trail.z + 0), color = yellow_fire }, smoke));
                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 6), y = (byte)(trail.y - 2 + (r.Next(5) - 2)), z = (byte)(trail.z + 0), color = yellow_fire }, smoke));
                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 6), y = (byte)(trail.y + 2 + (r.Next(5) - 2)), z = (byte)(trail.z + 2), color = yellow_fire }, smoke));
                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 6), y = (byte)(trail.y - 2 + (r.Next(5) - 2)), z = (byte)(trail.z + 2), color = yellow_fire }, smoke));

                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 8), y = (byte)(trail.y + 2 + (r.Next(5) - 2)), z = (byte)(trail.z + 0), color = yellow_fire }, smoke));
                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 8), y = (byte)(trail.y - 2 + (r.Next(5) - 2)), z = (byte)(trail.z + 0), color = yellow_fire }, smoke));
                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 8), y = (byte)(trail.y + 2 + (r.Next(5) - 2)), z = (byte)(trail.z + 2), color = yellow_fire }, smoke));
                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 8), y = (byte)(trail.y - 2 + (r.Next(5) - 2)), z = (byte)(trail.z + 2), color = yellow_fire }, smoke));

                    extra[f] = extra[f].Where(v => r.Next(4) > 0).ToList();

                }
                else if(f == 5)
                {

                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 8), y = (byte)(trail.y + 4 + (r.Next(5) - 2)), z = (byte)(trail.z + 2), color = yellow_fire }, smoke));
                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 8), y = (byte)(trail.y - 4 + (r.Next(5) - 2)), z = (byte)(trail.z + 2), color = yellow_fire }, smoke));
                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 10), y = (byte)(trail.y + 4 + (r.Next(5) - 2)), z = (byte)(trail.z + 2), color = yellow_fire }, smoke));
                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 10), y = (byte)(trail.y - 4 + (r.Next(5) - 2)), z = (byte)(trail.z + 2), color = yellow_fire }, smoke));
                    extra[f] = extra[f].Where(v => r.Next(4) > 0).ToList();

                }
                else if(f == 6)
                {
                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 10), y = (byte)(trail.y + 4 + (r.Next(7) - 3)), z = (byte)(trail.z + 2), color = yellow_fire }, smoke));
                    extra[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 10), y = (byte)(trail.y - 4 + (r.Next(7) - 3)), z = (byte)(trail.z + 2), color = yellow_fire }, smoke));
                    extra[f] = extra[f].Where(v => r.Next(4) > 0).ToList();

                }
            }
            for(int f = 1; f < voxelFrames.Length - 1; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(parsedFrames[f]);
                working.AddRange(missile[f - 1]);
                working.AddRange(extra[f - 1]);
                voxelFrames[f] = working.ToArray();
            }
            return voxelFrames;
        }
        public static MagicaVoxelData[][] RocketReceiveAnimationLarge(MagicaVoxelData[][] parsedFrames, int distance)
        {
            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[parsedFrames.Length];
            MagicaVoxelData[][] finalFrames = new MagicaVoxelData[parsedFrames.Length][];

            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length];
            MagicaVoxelData[][] explosion = FireballLarge(randomFill(70, 48, 20, 8, 5, 5, new int[] { smoke, orange_fire, yellow_fire, orange_fire, yellow_fire }).ToArray(), -2, 7, 2);

            for(int f = 0; f < voxelFrames.Length; f++)
            {
                extra[f] = new List<MagicaVoxelData>(20);
            }
            for(int i = 0; i < 7; i++)
            {
                extra[3 + distance + i].AddRange(explosion[i]);
            }

            for(int f = 0; f < voxelFrames.Length; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(20);
                working.AddRange(extra[f]);
                finalFrames[f] = working.ToArray();
            }
            return finalFrames;
        }

        public static MagicaVoxelData[][] ArcMissileAnimationLarge(MagicaVoxelData[][] parsedFrames, int unit, int which)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[parsedFrames.Length][];
            voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
            voxelFrames[parsedFrames.Length - 1] = new MagicaVoxelData[parsedFrames[parsedFrames.Length - 1].Length];
            parsedFrames[0].CopyTo(voxelFrames[0], 0);
            parsedFrames[parsedFrames.Length - 1].CopyTo(voxelFrames[parsedFrames.Length - 1], 0);
            List<MagicaVoxelData> launchers = new List<MagicaVoxelData>(4), trails = new List<MagicaVoxelData>(4);
            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length - 2], missile = new List<MagicaVoxelData>[voxelFrames.Length - 2];
            foreach(MagicaVoxelData mvd in voxelFrames[0])
            {
                if(mvd.color == emitter0 - which * 8)
                {
                    launchers.Add(mvd);
                }
            }
            int maxY = launchers.Max(v => v.y);
            int minY = launchers.Min(v => v.y);
            int midY = (maxY + minY) / 2;
            MagicaVoxelData launcher = launchers.OrderBy(mvd => mvd.z * 3 + mvd.x + mvd.y).First();
            launcher.y = (byte)midY;
            for(int f = 0; f < voxelFrames.Length - 2; f++) //going only through the middle
            {
                extra[f] = new List<MagicaVoxelData>(160);
                missile[f] = new List<MagicaVoxelData>(160);
                /*
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
                }*/
                if(f == 0)
                {
                    missile[f].AddRange(generateMissileLarge(launcher, 0));
                }
                if(f == 1)
                {
                    missile[f].AddRange(generateMissileLarge(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = launcher.y, z = (byte)(launcher.z + 4), color = bold_paint }, 4));
                }
                else if(f == 2)
                {
                    missile[f].AddRange(generateMissileLarge(new MagicaVoxelData { x = (byte)(launcher.x + 12), y = launcher.y, z = (byte)(launcher.z + 12), color = bold_paint }, 12));
                }
                else if(f == 3)
                {
                    missile[f].AddRange(generateMissileFieryTrailLarge(new MagicaVoxelData { x = (byte)(launcher.x + 20), y = launcher.y, z = (byte)(launcher.z + 20), color = bold_paint }, 12));
                }
                else if(f == 4)
                {
                    missile[f].AddRange(generateMissileFieryTrailLarge(new MagicaVoxelData { x = (byte)(launcher.x + 28), y = launcher.y, z = (byte)(launcher.z + 28), color = bold_paint }, 12));

                    extra[f].AddRange(VoxelLogic.generateCube(new MagicaVoxelData { x = launcher.x, y = (byte)(launcher.y - 1), z = launcher.z, color = smoke }, 6, smoke));

                    extra[f] = extra[f].Where(v => r.Next(5) == 0).ToList();


                }
                else if(f == 5)
                {
                    missile[f].AddRange(generateMissileFieryTrailLarge(new MagicaVoxelData { x = (byte)(launcher.x + 36), y = launcher.y, z = (byte)(launcher.z + 36), color = bold_paint }, 12));

                    extra[f].AddRange(VoxelLogic.generateCube(new MagicaVoxelData { x = launcher.x, y = (byte)(launcher.y - 1), z = launcher.z, color = smoke }, 6, smoke));

                    extra[f] = extra[f].Where(v => r.Next(7) == 0).ToList();
                    //extra[f].AddRange(generateCone(new MagicaVoxelData { x = (byte)(launcher.x + 6), y = (byte)(launcher.y), z = (byte)(launcher.z + 6), color = smoke }, 4, 249 - 120));

                }
                else if(f == 6)
                {
                    missile[f].AddRange(generateMissileFieryTrailLarge(new MagicaVoxelData { x = (byte)(launcher.x + 44), y = launcher.y, z = (byte)(launcher.z + 44), color = bold_paint }, 12));

                    extra[f].AddRange(VoxelLogic.generateCube(new MagicaVoxelData { x = launcher.x, y = (byte)(launcher.y - 2), z = launcher.z, color = smoke }, 8, smoke));

                    extra[f] = extra[f].Where(v => r.Next(20) == 0).ToList();

                }
                else if(f > 6)
                {
                    missile[f].AddRange(generateMissileFieryTrailLarge(new MagicaVoxelData { x = (byte)(launcher.x + 8 + f * 8), y = launcher.y, z = (byte)(launcher.z + 8 + f * 8), color = bold_paint }, 12));
                }
                if(f >= 4)
                {
                    extra[f].AddRange(generateConeLarge(new MagicaVoxelData { x = (byte)(launcher.x + 3 * (f - 3)), y = (byte)(launcher.y), z = (byte)(launcher.z + 3 * (f - 3)), color = smoke }, (f * 3) / 2, smoke).
                        Where(v => r.Next(15) > f && r.Next(15) > f));
                }
            }
            for(int f = 1; f < voxelFrames.Length - 1; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(parsedFrames[f]);
                working.AddRange(missile[f - 1]);
                working.AddRange(extra[f - 1]);
                voxelFrames[f] = working.ToArray();
            }
            return voxelFrames;
        }
        public static MagicaVoxelData[][] ArcMissileReceiveAnimationLarge(MagicaVoxelData[][] parsedFrames, int strength)
        {

            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[parsedFrames.Length];
            MagicaVoxelData[][] finalFrames = new MagicaVoxelData[parsedFrames.Length][];

            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length], missile = new List<MagicaVoxelData>[voxelFrames.Length];
            MagicaVoxelData[][] explosion = new MagicaVoxelData[16][];

            MagicaVoxelData launcher = new MagicaVoxelData { x = 112, y = 58, z = 60 };
            bool isExploding = false;
            int firstBurst = 0;
            for(int f = 0; f < voxelFrames.Length; f++)
            {
                //if (f > 0)
                //{
                //    for (int i = 0; i < extra[f-1].Count; i++)
                //    {
                //        extra[f].Add(new MagicaVoxelData { x = (byte)(extra[f-1][i].x + 2), y = (byte)(extra[f-1][i].y + Math.Round(r.NextDouble() * 1.1 - 0.55)),
                //            z = extra[f-1][i].z, color = extra[f-1][i].color });
                //    }
                //}

                extra[f] = new List<MagicaVoxelData>(160);
                missile[f] = new List<MagicaVoxelData>(160);

                if(f > 0)
                {
                    for(int i = 0; i < missile[f - 1].Count; i++)
                    {
                        if(missile[f - 1][i].x - 4 < 64)
                        {
                            isExploding = true;
                            explosion = FireballLarge(randomFill(50, 55, missile[f - 1][i].z, 8, 10, 7, new int[] { smoke, orange_fire, yellow_fire }).Concat(missile[f - 1]).ToArray(), 0, 14 - f, 2);
                            missile[f].Clear();
                            firstBurst = f;
                            break;
                        }
                    }
                }
                if(f == 0 && !isExploding)
                {
                    missile[f].AddRange(generateDownwardMissileLarge(launcher, 0));

                    /*                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y), z = (byte)(launcher.z - 2), color = bold_paint });
                                        missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y), z = (byte)(launcher.z - 2), color = bold_paint });
                                        missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 2), color = bold_paint });
                                        missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y), z = (byte)(launcher.z - 3), color = bold_paint });
                                        missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 2), color = bold_paint });
                                        missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 3), color = bold_paint });
                                        missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y), z = (byte)(launcher.z - 3), color = bold_paint });
                                        missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 3), color = bold_paint });

                                        missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = launcher.y, z = (byte)(launcher.z - 2), color = metal });
                                        missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 2), color = metal });
                                        missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = launcher.y, z = (byte)(launcher.z - 1), color = metal });
                                        missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = metal });

                                        missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y), z = (byte)(launcher.z), color = metal });
                                        missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y), z = (byte)(launcher.z), color = metal });
                                        missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 1), z = (byte)(launcher.z), color = metal });
                                        missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = metal });
                                        missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z), color = metal });
                                        missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = metal });
                                        missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = metal });
                                        missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = metal });
                                        */

                }
                if(f == 1 && !isExploding)
                {
                    missile[f].AddRange(generateDownwardMissileLarge(new MagicaVoxelData { x = (byte)(launcher.x - 8), y = launcher.y, z = (byte)(launcher.z - 8), color = bold_paint }, 8));

                    /*
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y), z = (byte)(launcher.z - 3), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 3), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = launcher.y, z = (byte)(launcher.z - 4), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 4), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 4), y = launcher.y, z = (byte)(launcher.z - 3), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 4), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 3), color = metal });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y), z = (byte)(launcher.z - 2), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 2), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = launcher.y, z = (byte)(launcher.z - 3), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 3), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = launcher.y, z = (byte)(launcher.z - 2), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 2), color = metal });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = launcher.y, z = (byte)(launcher.z - 2), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 2), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = launcher.y, z = (byte)(launcher.z - 1), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = metal });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y), z = (byte)(launcher.z), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y), z = (byte)(launcher.z), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 1), z = (byte)(launcher.z), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = metal });*/




                }
                else if(f >= 2 && !isExploding)
                {
                    missile[f].AddRange(generateDownwardMissileFieryTrailLarge(new MagicaVoxelData { x = (byte)(launcher.x - 8 * f), y = launcher.y, z = (byte)(launcher.z - 8 * f), color = bold_paint }, 12));
                    /*
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y), z = (byte)(launcher.z - 3), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 3), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = launcher.y, z = (byte)(launcher.z - 4), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 4), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 4), y = launcher.y, z = (byte)(launcher.z - 3), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 4), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 3), color = metal });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y), z = (byte)(launcher.z - 2), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 2), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = launcher.y, z = (byte)(launcher.z - 3), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 3), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = launcher.y, z = (byte)(launcher.z - 2), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 3), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 2), color = metal });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = launcher.y, z = (byte)(launcher.z - 2), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 2), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = launcher.y, z = (byte)(launcher.z - 1), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = metal });

                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y), z = (byte)(launcher.z), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y), z = (byte)(launcher.z), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 1), z = (byte)(launcher.z), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = metal });
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = metal });
                     * */
                }
                if(f >= 4)
                {
                    extra[f].AddRange(generateDownwardConeLarge(new MagicaVoxelData { x = (byte)(launcher.x - 8 * (f - 3)), y = (byte)(launcher.y), z = (byte)(launcher.z - 8 * (f - 3)), color = smoke },
                        8, smoke).Where(v => r.Next(95) > (f + 1) * (f + 1) && r.Next(95) > (f + 1) * f));
                }
                if(f < 14 && isExploding)
                {
                    extra[f].AddRange(explosion[f - firstBurst]);
                }
            }
            for(int f = 0; f < voxelFrames.Length; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(20);
                working.AddRange(missile[f]);
                working.AddRange(extra[f]);
                finalFrames[f] = working.ToArray();
            }
            return finalFrames; ;
        }

        public static MagicaVoxelData[][] BombAnimationLarge(MagicaVoxelData[][] parsedFrames, int unit, int which)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[parsedFrames.Length][];
            voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
            voxelFrames[parsedFrames.Length - 1] = new MagicaVoxelData[parsedFrames[parsedFrames.Length - 1].Length];
            parsedFrames[0].CopyTo(voxelFrames[0], 0);
            parsedFrames[parsedFrames.Length - 1].CopyTo(voxelFrames[parsedFrames.Length - 1], 0);
            List<MagicaVoxelData> launchers = new List<MagicaVoxelData>(4);
            // List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length - 2];
            foreach(MagicaVoxelData mvd in voxelFrames[0])
            {
                if(mvd.color == emitter0 - which * 8)
                {
                    launchers.Add(mvd);
                }
            }
            List<MagicaVoxelData>[][] missiles = new List<MagicaVoxelData>[launchers.Count][];
            MagicaVoxelData[][][] explosions = new MagicaVoxelData[launchers.Count][][];
            MagicaVoxelData[] centers = new MagicaVoxelData[launchers.Count];
            int[] exploding = new int[launchers.Count];
            for(int i = 0; i < launchers.Count; i++)
            {
                missiles[i] = new List<MagicaVoxelData>[voxelFrames.Length - 2];
                explosions[i] = new MagicaVoxelData[voxelFrames.Length - 2][];
                exploding[i] = -1;
            }
            for(int f = 0; f < voxelFrames.Length - 2; f++) //going only through the middle
            {
                //extra[f] = new List<MagicaVoxelData>(100);
                for(int m = 0; m < missiles.Length; m++)
                {

                    launchers = new List<MagicaVoxelData>(4);
                    foreach(MagicaVoxelData mvd in voxelFrames[0])
                    {
                        if(mvd.color == emitter0 - which * 8)
                        {
                            launchers.Add(mvd);
                        }
                    }
                    MagicaVoxelData launcher = launchers[m];
                    missiles[m][f] = new List<MagicaVoxelData>(40);

                    if(f > 0)
                    {
                        double drop = f * (r.NextDouble() * 1.3 + 1.0);
                        foreach(MagicaVoxelData missile in missiles[m][f - 1].OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128))
                        {
                            if(missile.z - drop < 1)
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
                    if(f <= 1)
                    {
                        missiles[m][f].AddRange(VoxelLogic.generateCube(new MagicaVoxelData { x = (byte)(launcher.x + 0), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = bold_paint }, 4, bold_paint));
                    }

                    if(exploding[m] > -1)
                    {
                        if(exploding[m] == 0)
                        {
                            explosions[m] = FireballLarge(randomFill(centers[m].x - 5, centers[m].y - 5, 0, 12, 12, 8, new int[] { smoke, orange_fire, yellow_fire }).ToArray(), 0, voxelFrames.Length - 2 - f, 0);
                        }
                        exploding[m]++;
                    }
                }
            }
            for(int f = 1; f < voxelFrames.Length - 1; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(parsedFrames[f]);
                for(int i = 0; i < launchers.Count; i++)
                {
                    working.AddRange(missiles[i][f - 1]);
                    if(f + 1 - voxelFrames.Length + exploding[i] >= 0 && f + 1 - voxelFrames.Length + exploding[i] < explosions[i].Length)
                        working.AddRange(explosions[i][f + 1 - voxelFrames.Length + exploding[i]]);
                }
                //working.AddRange(extra[f - 1]);
                voxelFrames[f] = working.ToArray();
            }

            return voxelFrames;
        }

        public static MagicaVoxelData[][] BombReceiveAnimationLarge(MagicaVoxelData[][] parsedFrames, int strength)
        {
            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[parsedFrames.Length];
            MagicaVoxelData[][] finalFrames = new MagicaVoxelData[parsedFrames.Length][];

            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length];
            MagicaVoxelData[][][] explosions = new MagicaVoxelData[strength][][];
            for(int s = 0; s < strength; s++)
            {
                explosions[s] = FireballLarge(randomFill(84, 50 + r.Next(11), r.Next(5), 8, 6, 6, new int[] { orange_fire, yellow_fire, smoke }).ToArray(), -3, 9, 2);
            }
            for(int f = 0; f < voxelFrames.Length; f++)
            {
                extra[f] = new List<MagicaVoxelData>(800);
            }
            for(int i = 0; i < 9; i++)
            {
                for(int sp = 0; sp < explosions.Length; sp++)
                    extra[5 + i].AddRange(explosions[sp][i]);
            }

            for(int f = 0; f < voxelFrames.Length; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(800);
                working.AddRange(extra[f]);
                finalFrames[f] = working.ToArray();
            }
            return finalFrames;
        }

        public static MagicaVoxelData[][] TorpedoAnimationLarge(MagicaVoxelData[][] parsedFrames, int unit, int which)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[parsedFrames.Length][];
            voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
            voxelFrames[parsedFrames.Length - 1] = new MagicaVoxelData[parsedFrames[parsedFrames.Length - 1].Length];
            parsedFrames[0].CopyTo(voxelFrames[0], 0);
            parsedFrames[parsedFrames.Length - 1].CopyTo(voxelFrames[parsedFrames.Length - 1], 0);
            List<MagicaVoxelData> launchers = new List<MagicaVoxelData>(4);//, trails = new List<MagicaVoxelData>(4);
            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length - 2], missile = new List<MagicaVoxelData>[voxelFrames.Length - 2];
            foreach(MagicaVoxelData mvd in voxelFrames[0])
            {
                if(mvd.color == emitter0 - which * 8)
                {
                    launchers.Add(mvd);
                }
            }

            int maxY = launchers.Max(v => v.y);
            int minY = launchers.Min(v => v.y);
            int midY = (maxY + minY) / 2;
            MagicaVoxelData launcher = launchers.OrderBy(mvd => mvd.z * 3 + mvd.x + mvd.y).First();
            launcher.y = (byte)midY;

            //            MagicaVoxelData trail = trails.RandomElement();
            for(int f = 0; f < voxelFrames.Length - 2; f++) //going only through the middle
            {
                extra[f] = new List<MagicaVoxelData>(20);
                missile[f] = new List<MagicaVoxelData>(20);

                if(f > 1)
                {
                    for(int i = 0; i < missile[f - 1].Count; i++)
                    {
                        missile[f].Add(new MagicaVoxelData
                        {
                            x = (byte)(missile[f - 1][i].x + 8),
                            y = (byte)(missile[f - 1][i].y),
                            z = missile[f - 1][i].z,
                            color = missile[f - 1][i].color
                        });
                    }
                }
                if(f == 0)
                {
                    missile[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x), y = launcher.y, z = (byte)(launcher.z), color = bold_paint }, shadow));
                }
                if(f == 1)
                {
                    missile[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 6), y = launcher.y, z = (byte)(launcher.z), color = bold_paint }, shadow));
                    missile[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = launcher.y, z = (byte)(launcher.z), color = bold_paint }, shadow));
                    missile[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z), color = bold_paint }, shadow));
                    missile[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 0), y = launcher.y, z = (byte)(launcher.z), color = bold_paint }, shadow));

                }
                else if(f == 2)
                {
                    missile[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 6), y = launcher.y, z = (byte)(launcher.z), color = bold_paint }, shadow));
                    missile[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = launcher.y, z = (byte)(launcher.z), color = bold_paint }, shadow));
                    missile[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z), color = bold_paint }, shadow));
                    missile[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 0), y = launcher.y, z = (byte)(launcher.z), color = bold_paint }, shadow));
                }
                else if(f == 3)
                {

                    missile[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 6), y = launcher.y, z = (byte)(launcher.z), color = bold_paint }, shadow));
                    missile[f].AddRange(VoxelLogic.generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = launcher.y, z = (byte)(launcher.z), color = bold_paint }, shadow));
                }
            }
            for(int f = 1; f < voxelFrames.Length - 1; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(parsedFrames[f]);
                working.AddRange(missile[f - 1]);
                voxelFrames[f] = working.ToArray();
            }
            return voxelFrames;
        }
        public static MagicaVoxelData[][] TorpedoReceiveAnimationLarge(MagicaVoxelData[][] parsedFrames, int distance)
        {
            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[parsedFrames.Length];
            MagicaVoxelData[][] finalFrames = new MagicaVoxelData[parsedFrames.Length][];

            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length];
            MagicaVoxelData[][] explosion = FireballLarge(randomFill(62, 47, 0, 8, 8, 7, new int[] { 253 - 27 * 4, 253 - 27 * 4, 253 - 27 * 4, orange_fire, yellow_fire }).ToArray(), -2, 7, 2);

            for(int f = 0; f < voxelFrames.Length; f++)
            {
                extra[f] = new List<MagicaVoxelData>(20);
            }
            for(int i = 0; i < 7; i++)
            {
                extra[3 + distance + i].AddRange(explosion[i].Select(mvd => { mvd.z = (byte)((i * i <= mvd.z) ? mvd.z - i * i : 0); mvd.color = (byte)((mvd.color == smoke) ? VoxelLogic.clear : 253 - 27 * 4); return mvd; }));
            }

            for(int f = 0; f < voxelFrames.Length; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(20);
                working.AddRange(extra[f]);
                finalFrames[f] = working.ToArray();
            }
            return finalFrames;
        }


        public delegate MagicaVoxelData[][] AnimationGenerator(MagicaVoxelData[][] parsedFrames, int prop, int whichEmitter);
        public delegate MagicaVoxelData[][] ReceiveAnimationGenerator(MagicaVoxelData[][] parsedFrames, int prop);

        //169 Bomb Drop
        //170 Arc Missile
        //171 Rocket
        //172 Long Cannon
        //173 Cannon
        //174 AA Gun
        //175 Machine Gun
        //176 Handgun
        public static string[] WeaponTypes = { "Handgun", "Machine_Gun", "AA_Gun", "Cannon", "Long_Cannon", "Rocket", "Arc_Missile", "Bomb" };

        public static AnimationGenerator[] weaponAnimationsLarge = { HandgunAnimationLarge, MachineGunAnimationLarge, TorpedoAnimationLarge, CannonAnimationLarge,
                                                                       LongCannonAnimationLarge, RocketAnimationLarge, ArcMissileAnimationLarge, BombAnimationLarge };
        private static ReceiveAnimationGenerator[] receiveAnimations = { HandgunReceiveAnimationLarge, MachineGunReceiveAnimationLarge, TorpedoReceiveAnimationLarge,
                                                                    CannonReceiveAnimationLarge, LongCannonReceiveAnimationLarge,
                                                                    RocketReceiveAnimationLarge,
                                                                    ArcMissileReceiveAnimationLarge,
                                                                    BombReceiveAnimationLarge
                                                                };


        public static MagicaVoxelData[][] makeFiringAnimationLarge(MagicaVoxelData[] parsed, int unit, int weapon)
        {
            MagicaVoxelData[][] parsedFrames = new MagicaVoxelData[][] {
                parsed, parsed, parsed, parsed,
                parsed, parsed, parsed, parsed,
                parsed, parsed, parsed, parsed,
                parsed, parsed, parsed, parsed, };
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[parsedFrames.Length][];
            //voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
            parsedFrames.CopyTo(voxelFrames, 0);

            VoxelLogic.sizex = 120;
            VoxelLogic.sizey = 120;
            VoxelLogic.sizez = 120;
            for(int i = 0; i < parsedFrames[0].Length; i++)
            {
                voxelFrames[0][i].x += 40;
                voxelFrames[0][i].y += 40;
            }

            if(VoxelLogic.CurrentWeapons[unit][weapon] == -1)
            {
                return new MagicaVoxelData[0][];
            }
            else
            {
                voxelFrames = weaponAnimationsLarge[VoxelLogic.CurrentWeapons[unit][weapon]](voxelFrames, unit, weapon);
            }

            return voxelFrames;
        }
        public static List<MagicaVoxelData>[] makeFiringAnimationSuper(MagicaVoxelData[] parsed, int unit, int weapon)
        {
            MagicaVoxelData[][] parsedFrames = new MagicaVoxelData[][] {
                parsed, parsed, parsed, parsed,
                parsed, parsed, parsed, parsed,
                parsed, parsed, parsed, parsed,
                parsed, parsed, parsed, parsed, };
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[parsedFrames.Length][];
            List<MagicaVoxelData>[] vf2 = new List<MagicaVoxelData>[parsedFrames.Length];
            //voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
            parsedFrames.CopyTo(voxelFrames, 0);
            for(int i = 0; i < parsedFrames[0].Length; i++)
            {
                voxelFrames[0][i].x += 20;
                voxelFrames[0][i].y += 20;
            }

            if(VoxelLogic.CurrentWeapons[unit][weapon] == -1)
            {
                return new List<MagicaVoxelData>[0];
            }
            else
            {
                voxelFrames = weaponAnimationsLarge[VoxelLogic.CurrentWeapons[unit][weapon]](voxelFrames, unit, weapon);
            }
            VoxelLogic.sizex = 80;
            VoxelLogic.sizey = 80;
            VoxelLogic.sizez = 60;
            for(int i = 0; i < voxelFrames.Length; i++)
            {
                vf2[i] = new List<MagicaVoxelData>();
                for(int j = 0; j < voxelFrames[i].Length; j++)
                {
                    if(!(voxelFrames[i][j].x >= 80 || voxelFrames[i][j].y >= 80 || voxelFrames[i][j].z >= 60))
                        vf2[i].Add(voxelFrames[i][j]);
                }

            }



            return vf2;
        }

        public static List<MagicaVoxelData>[] makeFiringAnimationSuper(MagicaVoxelData[][] parsed, int unit, int weapon)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[parsed.Length][];
            List<MagicaVoxelData>[] vf2 = new List<MagicaVoxelData>[parsed.Length];
            //voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
            for(int f = 0; f < parsed.Length; f++)
            {
                voxelFrames[f] = new MagicaVoxelData[parsed[f].Length];
                parsed[f].CopyTo(voxelFrames[f], 0);

                for(int i = 0; i < parsed[f].Length; i++)
                {
                    voxelFrames[f][i].x += 20;
                    voxelFrames[f][i].y += 20;
                }
            }
            if(VoxelLogic.CurrentWeapons[unit][weapon] == -1)
            {
                return new List<MagicaVoxelData>[0];
            }
            else
            {
                voxelFrames = weaponAnimationsLarge[VoxelLogic.CurrentWeapons[unit][weapon]](voxelFrames, unit, weapon);
            }
            VoxelLogic.sizex = 80;
            VoxelLogic.sizey = 80;
            VoxelLogic.sizez = 60;
            for(int i = 0; i < voxelFrames.Length; i++)
            {
                vf2[i] = new List<MagicaVoxelData>();
                for(int j = 0; j < voxelFrames[i].Length; j++)
                {
                    if(!(voxelFrames[i][j].x >= 80 || voxelFrames[i][j].y >= 80 || voxelFrames[i][j].z >= 60))
                        vf2[i].Add(voxelFrames[i][j]);
                }

            }



            return vf2;
        }

        public static MagicaVoxelData[][] makeReceiveAnimationLarge(int weaponType, int strength)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[16][];
            //voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];

            voxelFrames = receiveAnimations[weaponType](voxelFrames, strength);

            VoxelLogic.sizex = 120;
            VoxelLogic.sizey = 120;

            return voxelFrames;
        }
        public static List<MagicaVoxelData>[] makeReceiveAnimationSuper(int weaponType, int strength)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[16][];
            List<MagicaVoxelData>[] vf2 = new List<MagicaVoxelData>[16];
            //voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];

            voxelFrames = receiveAnimations[weaponType](voxelFrames, strength);
            for(int i = 0; i < 16; i++)
            {
                HashSet<MagicaVoxelData> vlist = new HashSet<MagicaVoxelData>();
                for(int j = 0; j < voxelFrames[i].Length; j++)
                {
                    MagicaVoxelData m = new MagicaVoxelData { x = (byte)(voxelFrames[i][j].x * 0.65), y = (byte)(voxelFrames[i][j].y * 0.65), z = (byte)(voxelFrames[i][j].z * 0.65), color = voxelFrames[i][j].color };
                    vlist.Add(m);
                    vlist.Add(VoxelLogic.AlterVoxel(m, 0, 0, 1, m.color));
                    vlist.Add(VoxelLogic.AlterVoxel(m, 0, 0, -1, m.color));
                    vlist.Add(VoxelLogic.AlterVoxel(m, 0, 1, 0, m.color));
                    vlist.Add(VoxelLogic.AlterVoxel(m, 0, -1, 0, m.color));
                    vlist.Add(VoxelLogic.AlterVoxel(m, 1, 0, 0, m.color));
                    vlist.Add(VoxelLogic.AlterVoxel(m, -1, 0, 0, m.color));
                }
                vf2[i] = vlist.ToList();
            }
            VoxelLogic.sizex = 80;
            VoxelLogic.sizey = 80;

            return vf2;
        }

        public static MagicaVoxelData[][] Flyover(MagicaVoxelData[] voxels)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[17][];
            voxelFrames[0] = new MagicaVoxelData[voxels.Length];
            voxels.CopyTo(voxelFrames[0], 0);
            for(int i = 0; i < voxels.Length; i++)
            {
                voxelFrames[0][i].x += 40;
                voxelFrames[0][i].y += 40;
            }
            for(int f = 1; f <= 8; f++)
            {
                voxelFrames[f] = new MagicaVoxelData[voxelFrames[f - 1].Length]; //.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)
                voxelFrames[f - 1].CopyTo(voxelFrames[f], 0);

                for(int i = 0; i < voxelFrames[f].Length; i++)
                {
                    voxelFrames[f][i].z += 2;
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
            for(int f = 1; f <= 8; f++)
            {

                List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxelFrames[f].Length);

                for(int i = 0; i < voxelFrames[f].Length; i++)
                {
                    // do not store this voxel if it lies out of range of the voxel chunk (120x120x120)
                    if(voxelFrames[f][i].x >= 120 || voxelFrames[f][i].y >= 120 || voxelFrames[f][i].z >= 120)
                    {
                        continue;
                    }

                    altered.Add(voxelFrames[f][i]);
                    //-1 == taken[voxelFrames[f][i].x, voxelFrames[f][i].y] && 
                }
                voxelFrames[f] = altered.ToArray();
            }

            MagicaVoxelData[][] frames = new MagicaVoxelData[16][];

            for(int f = 1; f < 9; f++)
            {
                frames[f - 1] = new MagicaVoxelData[voxelFrames[f].Length];
                frames[16 - f] = new MagicaVoxelData[voxelFrames[f].Length];
                voxelFrames[f].CopyTo(frames[f - 1], 0);
                voxelFrames[f].CopyTo(frames[16 - f], 0);
            }
            return frames;
        }
        public static MagicaVoxelData[][] FlyoverSuper(MagicaVoxelData[] voxels)
        {

            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[16][];
            voxelFrames[0] = new MagicaVoxelData[voxels.Length];
            voxels.CopyTo(voxelFrames[0], 0);
            
            for(int f = 1; f <= 8; f++)
            {
                voxelFrames[f] = new MagicaVoxelData[voxelFrames[f - 1].Length]; //.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)
                voxelFrames[f - 1].CopyTo(voxelFrames[f], 0);

                for(int i = 0; i < voxelFrames[f].Length; i++)
                {
                    voxelFrames[f][i].z += 2;
                }
            }
            for(int f = 1; f <= 8; f++)
            {

                List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxelFrames[f].Length);

                for(int i = 0; i < voxelFrames[f].Length; i++)
                {
                    // do not store this voxel if it lies out of range of the voxel chunk (120x120x120)
                    if(voxelFrames[f][i].x >= 120 || voxelFrames[f][i].y >= 120 || voxelFrames[f][i].z >= 120)
                    {
                        continue;
                    }

                    altered.Add(voxelFrames[f][i]);
                    //-1 == taken[voxelFrames[f][i].x, voxelFrames[f][i].y] && 
                }
                voxelFrames[f - 1] = altered.ToArray();
                voxelFrames[16 - f] = voxelFrames[f - 1];
            }
            
            return voxelFrames;
        }

        private static byte[] dismiss = TransformLogic.dismiss;

        public static byte[][,,] FieryDeathW(String u, Model m)
        {
            byte[,,] colors = m.Finalize();
            int xSize = colors.GetLength(0), ySize = colors.GetLength(1), zSize = colors.GetLength(2);
            byte[][,,] voxelFrames = new byte[13][,,];
            voxelFrames[0] = colors;

            int initialMinZ = colors.MinZ(dismiss);

            if(VoxelLogic.CurrentMobilities[VoxelLogic.UnitLookup[u]] == MovementType.Immobile)
            {
                return TransformLogic.FieryExplosionW(colors, false, false);
            }
            else if(VoxelLogic.CurrentMobilities[VoxelLogic.UnitLookup[u]] == MovementType.Foot)
            {
                return FieryHaymakerW(m, voxelFrames, xSize, ySize, zSize);
            }
            else if(initialMinZ > 1)
            {
                return FieryDeathFallW(m, voxelFrames, xSize, ySize, zSize);
            }
            else if(VoxelLogic.CurrentMobilities[VoxelLogic.UnitLookup[u]] == MovementType.Naval)
            {
                return SinkW(m, voxelFrames, xSize, ySize, zSize);
            }
            else
            {
                return FieryFlipW(m, voxelFrames, xSize, ySize, zSize);
            }
        }

        public static byte[][,,] FieryDeathFallW(Model m, byte[][,,] voxelFrames, int xSize, int ySize, int zSize)
        {
            int[] minZ = new int[m.Bones.Count];
            int[] maxZ = new int[m.Bones.Count];
            int[] midZ = new int[m.Bones.Count];
            int[] minX = new int[m.Bones.Count];
            int[] maxX = new int[m.Bones.Count];
            int[] midX = new int[m.Bones.Count];
            int[] minY = new int[m.Bones.Count];
            int[] maxY = new int[m.Bones.Count];
            int[] midY = new int[m.Bones.Count];
            long[] sizes = new long[m.Bones.Count];
            Bone[] bones = m.Bones.Values.ToArray(), crashes = new Bone[bones.Length], wrecks = new Bone[bones.Length];
            Bone[][] boneFrames = new Bone[bones.Length][], crashFrames = new Bone[bones.Length][], wreckFrames = new Bone[bones.Length][];

            byte[][,,] finals = bones.Select(b => b.Finalize(10 * Bone.Multiplier, 0)).ToArray();
            for(int i = 0; i < bones.Length; i++)
            {
                minZ[i] = finals[i].MinZ(dismiss);
                maxZ[i] = finals[i].MaxZ(dismiss);
                midZ[i] = (maxZ[i] + minZ[i]) / 2;

                minX[i] = finals[i].MinXAtZ(midZ[i], dismiss);
                maxX[i] = finals[i].MaxXAtZ(midZ[i], dismiss);
                midX[i] = (maxX[i] + minX[i]) / 2;

                minY[i] = finals[i].MinYAtZ(midZ[i], dismiss);
                maxY[i] = finals[i].MaxYAtZ(midZ[i], dismiss);
                midY[i] = (maxY[i] + minY[i]) / 2;

                sizes[i] = (maxX[i] - minX[i]) * (maxY[i] - minY[i]) * (maxZ[i] - minZ[i]);
                if(sizes[i] <= 0)
                    sizes[i] = -1;
                else
                    sizes[i] = (int)Math.Log10(sizes[i] * sizes[i] * 0.1);
                if(sizes[i] > 9)
                    sizes[i] = 9;


                crashes[i] = bones[i].Replicate();
                crashes[i].Pitch = 360 - 50;
                crashes[i].MoveZ = -midZ[i];
                crashes[i].MoveX = xSize / 8f;
                crashes[i].Roll = 0;// r.Next(40) - 20f;
                crashes[i].Yaw = 0;// r.Next(120) - 60f;
                crashes[i].InitFierySpreadCU(-crashes[i].Yaw / 1.5f, 40, -crashes[i].Roll / 1.5f, 0.15f);


                wrecks[i] = bones[i].Replicate();
                wrecks[i].Pitch = 360 - 30;
                wrecks[i].MoveZ = -(midZ[i] + maxZ[i]) / 2;
                wrecks[i].MoveX = xSize / 6f;
                wrecks[i].Roll = crashes[i].Roll;
                wrecks[i].Yaw = crashes[i].Yaw;
                wrecks[i].InitFierySpreadCU(0, 10, 0, 0.09f);


                boneFrames[i] = new Bone[12];
                crashFrames[i] = new Bone[12];
                wreckFrames[i] = new Bone[12];

                boneFrames[i][0] = bones[i].Replicate();
                crashFrames[i][0] = crashes[i].Replicate();
                wreckFrames[i][0] = wrecks[i].Replicate();

                TransformLogic.MorphInPlace(ref boneFrames[i][0].Colors, new byte[] { orange_fire, orange_fire, smoke, yellow_fire }, 0.06f);
                TransformLogic.MorphInPlace(ref crashFrames[i][0].Colors, new byte[] { orange_fire, orange_fire, smoke, yellow_fire }, 0.06f);
                TransformLogic.MorphInPlace(ref wreckFrames[i][0].Colors, new byte[] { orange_fire, orange_fire, smoke, yellow_fire }, 0.06f);

                for(int f = 1; f < 12; f++)
                {
                    boneFrames[i][f] = boneFrames[i][f - 1].Replicate();
                    crashFrames[i][f] = crashFrames[i][f - 1].Replicate();
                    wreckFrames[i][f] = wreckFrames[i][f - 1].Replicate();

                    TransformLogic.MorphInPlace(ref boneFrames[i][f].Colors, new byte[] { orange_fire, orange_fire, smoke, yellow_fire, orange_fire, orange_fire, smoke, yellow_fire, 0 }, 0.0f + 0.035f * f);
                    TransformLogic.MorphInPlace(ref crashFrames[i][f].Colors, new byte[] { orange_fire, orange_fire, smoke, yellow_fire, orange_fire, orange_fire, smoke, yellow_fire, 0 }, 0.0f + 0.035f * f);
                    TransformLogic.MorphInPlace(ref wreckFrames[i][f].Colors, new byte[] { orange_fire, orange_fire, smoke, yellow_fire, orange_fire, orange_fire, smoke, yellow_fire, 0 }, 0.0f + 0.035f * f);
                }
            }

            for(int f = 1; f < 9; f++)
            {
                voxelFrames[f] = boneFrames[0][f - 1].Interpolate(crashFrames[0][f - 1], f * 0.125f).FinalizeScatter(10 * Bone.Multiplier, 0);
                for(int b = 1; b < bones.Length; b++)
                {
                    voxelFrames[f] = TransformLogic.Overlap(voxelFrames[f], boneFrames[b][f - 1].Interpolate(crashFrames[b][f - 1], f * 0.125f).FinalizeScatter(10 * Bone.Multiplier, 0));
                }
            }
            for(int f = 9; f < 13; f++)
            {
                voxelFrames[f] = crashFrames[0][f - 1].Interpolate(wreckFrames[0][f - 1], (f - 8) * 0.25f).FinalizeScatter(10 * Bone.Multiplier, 0);
                for(int b = 1; b < bones.Length; b++)
                {
                    voxelFrames[f] = TransformLogic.Overlap(voxelFrames[f], crashFrames[b][f - 1].Interpolate(wreckFrames[b][f - 1], (f - 8) * 0.25f).FinalizeScatter(10 * Bone.Multiplier, 0));
                }
            }
            for(int f = 6, e = 0; f < 12 && e < maxExplosionFrames; f++, e++)
            {
                for(int b = 0; b < bones.Length; b++)
                {
                    if(sizes[b] >= 0)
                    {
                        if(xSize > 120)
                            voxelFrames[f] = TransformLogic.Overlap(voxelFrames[f], TransformLogic.Translate(SuperExplosions[sizes[b]][e], midX[b] - 80, midY[b] - 80, 0));
                        else
                            voxelFrames[f] = TransformLogic.Overlap(voxelFrames[f], TransformLogic.Translate(Explosions[sizes[b]][e], midX[b] - 60, midY[b] - 60, 0));
                    }
                }
            }

            byte[][,,] frames = new byte[12][,,];

            for(int f = 1; f < 13; f++)
            {
                frames[f - 1] = voxelFrames[f];
            }
            return frames;
        }
        public static byte[][,,] SinkW(Model m, byte[][,,] voxelFrames, int xSize, int ySize, int zSize)
        {
            int[] minZ = new int[m.Bones.Count];
            int[] maxZ = new int[m.Bones.Count];
            int[] midZ = new int[m.Bones.Count];
            int[] minX = new int[m.Bones.Count];
            int[] maxX = new int[m.Bones.Count];
            int[] midX = new int[m.Bones.Count];
            int[] minY = new int[m.Bones.Count];
            int[] maxY = new int[m.Bones.Count];
            int[] midY = new int[m.Bones.Count];
            int[] sizes = new int[m.Bones.Count];
            Bone[] bones = m.Bones.Values.ToArray(), sunks = new Bone[bones.Length];
            Bone[][] boneFrames = new Bone[bones.Length][], sunkFrames = new Bone[bones.Length][];

            byte[][,,] finals = bones.Select(b => b.Finalize(10 * Bone.Multiplier, 0)).ToArray();
            for(int i = 0; i < bones.Length; i++)
            {
                minZ[i] = finals[i].MinZ(dismiss);
                maxZ[i] = finals[i].MaxZ(dismiss);
                midZ[i] = (maxZ[i] + minZ[i]) / 2;

                minX[i] = finals[i].MinXAtZ(midZ[i], dismiss);
                maxX[i] = finals[i].MaxXAtZ(midZ[i], dismiss);
                midX[i] = (maxX[i] + minX[i]) / 2;

                minY[i] = finals[i].MinYAtZ(midZ[i], dismiss);
                maxY[i] = finals[i].MaxYAtZ(midZ[i], dismiss);
                midY[i] = (maxY[i] + minY[i]) / 2;

                sizes[i] = (maxX[i] - minX[i]) * (maxY[i] - minY[i]) * (maxZ[i] - minZ[i]);
                if(sizes[i] <= 0)
                    sizes[i] = -1;
                else
                    sizes[i] = (int)Math.Log10(sizes[i] * sizes[i] * 0.05);
                if(sizes[i] > 9)
                    sizes[i] = 9;


                sunks[i] = bones[i].Replicate();
                sunks[i].Pitch = 0;
                sunks[i].ZeroOut(0, 0, maxZ[i] + 10);
                sunks[i].MoveX = xSize / -7f;
                sunks[i].Roll = 0;// r.Next(40) - 20f;
                sunks[i].Yaw = 0;// r.Next(120) - 60f;
                
                boneFrames[i] = new Bone[12];
                sunkFrames[i] = new Bone[12];

                boneFrames[i][0] = bones[i].Replicate();
                sunkFrames[i][0] = sunks[i].Replicate();

                TransformLogic.MorphInPlace(ref boneFrames[i][0].Colors, new byte[] { orange_fire, orange_fire, smoke, yellow_fire }, 0.05f);
                TransformLogic.MorphInPlace(ref sunkFrames[i][0].Colors, new byte[] { orange_fire, orange_fire, smoke, yellow_fire }, 0.05f);

                for(int f = 1; f < 12; f++)
                {
                    boneFrames[i][f] = boneFrames[i][f - 1].Replicate();
                    sunkFrames[i][f] = sunkFrames[i][f - 1].Replicate();

                    TransformLogic.MorphInPlace(ref boneFrames[i][f].Colors, new byte[] { orange_fire, orange_fire, smoke, yellow_fire, smoke, smoke, smoke, 0, 0 }, 0.05f + 0.035f * f);
                    TransformLogic.MorphInPlace(ref sunkFrames[i][f].Colors, new byte[] { orange_fire, orange_fire, smoke, yellow_fire, smoke, smoke, smoke, 0, 0 }, 0.05f + 0.035f * f);
                }
            }
            for(int f = 1; f < 13; f++)
            {
                voxelFrames[f] = boneFrames[0][f - 1].Interpolate(sunkFrames[0][f - 1], f / 12f).Finalize(10 * Bone.Multiplier, 0);
                for(int b = 1; b < bones.Length; b++)
                {
                    voxelFrames[f] = TransformLogic.Overlap(voxelFrames[f], boneFrames[b][f - 1].Interpolate(sunkFrames[b][f - 1], f / 12f).Finalize(10 * Bone.Multiplier, 0));
                }
            }
            for(int f = 1, e = 0; f < 7 && e < maxExplosionFrames; f++, e++)
            {
                for(int b = 0; b < bones.Length; b++)
                {
                    if(sizes[b] >= 0)
                    {
                        if(xSize > 120)
                            voxelFrames[f] = TransformLogic.Overlap(voxelFrames[f], TransformLogic.Translate(SuperExplosions[sizes[b]][e], midX[b] - 80, midY[b] - 80, midZ[b] / 2));
                        else
                            voxelFrames[f] = TransformLogic.Overlap(voxelFrames[f], TransformLogic.Translate(Explosions[sizes[b]][e], midX[b] - 60, midY[b] - 60, midZ[b] / 2));
                    }
                }
            }

            byte[][,,] frames = new byte[12][,,];

            for(int f = 1; f < 13; f++)
            {
                frames[f - 1] = voxelFrames[f];
            }
            return frames;
        }
        public static byte[][,,] FieryHaymakerW(Model m, byte[][,,] voxelFrames, int xSize, int ySize, int zSize)
        {
            int[] minZ = new int[m.Bones.Count];
            int[] maxZ = new int[m.Bones.Count];
            int[] midZ = new int[m.Bones.Count];
            int[] minX = new int[m.Bones.Count];
            int[] maxX = new int[m.Bones.Count];
            int[] midX = new int[m.Bones.Count];
            int[] minY = new int[m.Bones.Count];
            int[] maxY = new int[m.Bones.Count];
            int[] midY = new int[m.Bones.Count];
            int[] sizes = new int[m.Bones.Count];
            Bone[] bones = m.Bones.Values.ToArray(), aerial = new Bone[bones.Length], wasted = new Bone[bones.Length];
            Bone[][] boneFrames = new Bone[bones.Length][], aerialFrames = new Bone[bones.Length][], wastedFrames = new Bone[bones.Length][];

            byte[][,,] finals = bones.Select(b => b.Finalize(10 * Bone.Multiplier, 0)).ToArray();
            for(int i = 0; i < bones.Length; i++)
            {
                minZ[i] = finals[i].MinZ(dismiss);
                maxZ[i] = finals[i].MaxZ(dismiss);
                midZ[i] = (maxZ[i] + minZ[i]) / 2;

                minX[i] = finals[i].MinX(dismiss);
                maxX[i] = finals[i].MaxX(dismiss);
                midX[i] = (maxX[i] + minX[i]) / 2;

                minY[i] = finals[i].MinY(dismiss);
                maxY[i] = finals[i].MaxY(dismiss);
                midY[i] = (maxY[i] + minY[i]) / 2;

                sizes[i] = (maxX[i] - minX[i]) * (maxY[i] - minY[i]) * (maxZ[i] - minZ[i]);
                if(sizes[i] <= 0)
                    sizes[i] = -1;
                else
                    sizes[i] = (int)Math.Log10(sizes[i] * sizes[i] * 0.2);
                if(sizes[i] > 9)
                    sizes[i] = 9;


                aerial[i] = bones[i].Replicate();
                aerial[i].Pitch = 80 + 2 * r.Next(4);
                aerial[i].MoveZ = zSize / 7.5f;
                aerial[i].MoveX -= xSize / 3.5f - minZ[i] * 0.6f;
                aerial[i].MoveY = ((r.Next(3) + 1) * (r.Next(2) == 0 ? 1 : -1)) * minZ[i] * 0.8f;
                aerial[i].Roll = 0;// (i + 1) * 0.2f * (r.Next(20) - 10f);
                aerial[i].Yaw = 0;// (i + 0.5f) * 0.2f * (r.Next(10) - 5f);
                aerial[i].InitFierySpreadCU(0, 30, 0, 0.2f);


                wasted[i] = bones[i].Replicate();
                wasted[i].Pitch = 90;
                //wasted[i].ZeroOut(midZ[i], midY[i], midX[i]);//.ZeroOutZ = (midX[i] + minX[i]) * 0.5f;
                wasted[i].MoveZ = -midX[i] * 0.8f - minZ[i] * 0.45f; // + minZ[i] * 0.25f
                wasted[i].MoveX -= xSize / 2f - minZ[i] * 2f;
                wasted[i].MoveY = aerial[i].MoveY * 1.5f;
                wasted[i].Roll = 0;// aerial[i].Roll * 1.4f + r.Next(2, 6) / 10f * (r.Next(2) == 1 ? -1 : 1);
                wasted[i].Yaw = 0;// aerial[i].Yaw * 2 + r.Next(4, 11) / 10f * (r.Next(2) == 1 ? -1 : 1);
                wasted[i].InitFierySpreadCU(0, -5, 0, 0.05f);

                boneFrames[i] = new Bone[12];
                aerialFrames[i] = new Bone[12];
                wastedFrames[i] = new Bone[12];

                boneFrames[i][0] = bones[i].Replicate();
                aerialFrames[i][0] = aerial[i].Replicate();
                wastedFrames[i][0] = wasted[i].Replicate();

                TransformLogic.MorphInPlace(ref boneFrames[i][0].Colors, new byte[] { orange_fire, orange_fire, smoke, yellow_fire }, 0.06f);
                TransformLogic.MorphInPlace(ref aerialFrames[i][0].Colors, new byte[] { orange_fire, orange_fire, smoke, yellow_fire }, 0.06f);
                TransformLogic.MorphInPlace(ref wastedFrames[i][0].Colors, new byte[] { orange_fire, orange_fire, smoke, yellow_fire }, 0.06f);

                for(int f = 1; f < 12; f++)
                {
                    boneFrames[i][f] = boneFrames[i][f - 1].Replicate();
                    aerialFrames[i][f] = aerialFrames[i][f - 1].Replicate();
                    wastedFrames[i][f] = wastedFrames[i][f - 1].Replicate();
                    if(sizes[i] >= 0)
                    {
                        TransformLogic.MorphInPlace(ref boneFrames[i][f].Colors, new byte[] { orange_fire, orange_fire, smoke, yellow_fire, orange_fire, orange_fire, smoke, yellow_fire, 0 }, 0.0f + 0.01f * f * sizes[i]);
                        TransformLogic.MorphInPlace(ref aerialFrames[i][f].Colors, new byte[] { orange_fire, orange_fire, smoke, yellow_fire, orange_fire, orange_fire, smoke, yellow_fire, 0 }, 0.0f + 0.01f * f * sizes[i]);
                        TransformLogic.MorphInPlace(ref wastedFrames[i][f].Colors, new byte[] { orange_fire, orange_fire, smoke, yellow_fire, orange_fire, orange_fire, smoke, yellow_fire, 0 }, 0.0f + 0.01f * f * sizes[i]);
                    }
                }

            }

            for(int f = 1; f < 4; f++)
            {
                voxelFrames[f] = boneFrames[0][f - 1].Interpolate(aerialFrames[0][f - 1], f * 1f / 3f).Finalize(10 * Bone.Multiplier, 0);
                for(int b = 1; b < bones.Length; b++)
                {
                    voxelFrames[f] = TransformLogic.Overlap(voxelFrames[f], boneFrames[b][f - 1].Interpolate(aerialFrames[b][f - 1], f * 1f / 3f).Finalize(10 * Bone.Multiplier, 0));
                }
            }
            for(int f = 4; f < 7; f++)
            {
                voxelFrames[f] = aerialFrames[0][f - 1].Interpolate(wastedFrames[0][f - 1], (f - 3) * 1f / 3f).Finalize(10 * Bone.Multiplier, 0);
                for(int b = 1; b < bones.Length; b++)
                {
                    voxelFrames[f] = TransformLogic.Overlap(voxelFrames[f], aerialFrames[b][f - 1].Interpolate(wastedFrames[b][f - 1], (f - 3) * 1f / 3f).Finalize(10 * Bone.Multiplier, 0));
                }
            }
            for(int f = 7; f < 13; f++)
            {
                voxelFrames[f] = wastedFrames[0][f - 1].Finalize(10 * Bone.Multiplier, 0);
                for(int b = 1; b < bones.Length; b++)
                {
                    voxelFrames[f] = TransformLogic.Overlap(voxelFrames[f], wastedFrames[b][f - 1].Finalize(10 * Bone.Multiplier, 0));
                }
            }
            for(int f = 1, e = 0; f < 7 && e < maxExplosionFrames; f++, e++)
            {
                for(int b = 0; b < bones.Length; b++)
                {
                    if(sizes[b] >= 0)
                    {
                        if(xSize > 120)
                            voxelFrames[f] = TransformLogic.Overlap(voxelFrames[f], TransformLogic.Translate(SuperExplosions[sizes[b]][e], midX[b] - 80, midY[b] - 80, 0));
                        else
                            voxelFrames[f] = TransformLogic.Overlap(voxelFrames[f], TransformLogic.Translate(Explosions[sizes[b]][e], midX[b] - 60, midY[b] - 60, 0));
                    }
                }
            }

            byte[][,,] frames = new byte[12][,,];

            for(int f = 1; f < 13; f++)
            {
                frames[f - 1] = voxelFrames[f];
            }
            return frames;
        }

        public static byte[][,,] FieryFlipW(Model m, byte[][,,] voxelFrames, int xSize, int ySize, int zSize)
        {
            int[] minZ = new int[m.Bones.Count];
            int[] maxZ = new int[m.Bones.Count];
            int[] midZ = new int[m.Bones.Count];
            int[] minX = new int[m.Bones.Count];
            int[] maxX = new int[m.Bones.Count];
            int[] midX = new int[m.Bones.Count];
            int[] minY = new int[m.Bones.Count];
            int[] maxY = new int[m.Bones.Count];
            int[] midY = new int[m.Bones.Count];
            int[] sizes = new int[m.Bones.Count];
            Bone[] bones = m.Bones.Values.ToArray(), aerial = new Bone[bones.Length], wasted = new Bone[bones.Length];
            Bone[][] boneFrames = new Bone[bones.Length][], aerialFrames = new Bone[bones.Length][], wastedFrames = new Bone[bones.Length][];

            byte[][,,] finals = bones.Select(b => b.Finalize(10 * Bone.Multiplier, 0)).ToArray();
            for(int i = 0; i < bones.Length; i++)
            {
                minZ[i] = finals[i].MinZ(dismiss);
                maxZ[i] = finals[i].MaxZ(dismiss);
                midZ[i] = (maxZ[i] + minZ[i]) / 2;

                minX[i] = finals[i].MinX(dismiss);
                maxX[i] = finals[i].MaxX(dismiss);
                midX[i] = (maxX[i] + minX[i]) / 2;

                minY[i] = finals[i].MinY(dismiss);
                maxY[i] = finals[i].MaxY(dismiss);
                midY[i] = (maxY[i] + minY[i]) / 2;

                sizes[i] = (maxX[i] - minX[i]) * (maxY[i] - minY[i]) * (maxZ[i] - minZ[i]);
                if(sizes[i] <= 0)
                    sizes[i] = -1;
                else
                    sizes[i] = (int)Math.Log10(sizes[i] * sizes[i] * 0.2);
                if(sizes[i] > 9)
                    sizes[i] = 9;


                aerial[i] = bones[i].Replicate();
                aerial[i].Pitch = 120 + (i + r.Next(3)) * (r.Next(3) + 1);
                aerial[i].MoveZ = zSize / 9.5f;
//                aerial[i].ZeroOut(midX[i], midY[i], -(midZ[i]));
                aerial[i].MoveX = xSize / -11f;// -15 - r.Next(10);
                aerial[i].MoveY = (r.Next(5) - 2) * (i + 0.4f) * 0.7f;
                aerial[i].Roll = 0;// (i + 1) * 0.2f * (r.Next(20) - 10f);
                aerial[i].Yaw = 0;// (i + 0.5f) * 0.2f * (r.Next(10) - 5f);
                aerial[i].InitFierySpreadCU(0, 40, 0, 0.2f);


                wasted[i] = bones[i].Replicate();
                wasted[i].Pitch = 180;
                wasted[i].ZeroOut(0, 0, midZ[i]);
                wasted[i].MoveX = xSize / -8f;// aerial[i].MoveX + midX[i] * 2f - 10 - r.Next(10);
                wasted[i].MoveY = aerial[i].MoveY * 1.4f;
                wasted[i].Roll = 0;// aerial[i].Roll * 1.4f + r.Next(2, 6) / 10f * (r.Next(2) == 1 ? -1 : 1);
                wasted[i].Yaw = 0;// aerial[i].Yaw * 2 + r.Next(4, 11) / 10f * (r.Next(2) == 1 ? -1 : 1);
                wasted[i].InitFierySpreadCU(0, -15, 0, 0.1f);

                boneFrames[i] = new Bone[12];
                aerialFrames[i] = new Bone[12];
                wastedFrames[i] = new Bone[12];

                boneFrames[i][0] = bones[i].Replicate();
                aerialFrames[i][0] = aerial[i].Replicate();
                wastedFrames[i][0] = wasted[i].Replicate();

                TransformLogic.MorphInPlace(ref boneFrames[i][0].Colors, new byte[] { orange_fire, orange_fire, smoke, yellow_fire }, 0.03f);
                TransformLogic.MorphInPlace(ref aerialFrames[i][0].Colors, new byte[] { orange_fire, orange_fire, smoke, yellow_fire }, 0.03f);
                TransformLogic.MorphInPlace(ref wastedFrames[i][0].Colors, new byte[] { orange_fire, orange_fire, smoke, yellow_fire }, 0.03f);

                for(int f = 1; f < 12; f++)
                {
                    boneFrames[i][f] = boneFrames[i][f - 1].Replicate();
                    aerialFrames[i][f] = aerialFrames[i][f - 1].Replicate();
                    wastedFrames[i][f] = wastedFrames[i][f - 1].Replicate();

                    TransformLogic.MorphInPlace(ref boneFrames[i][f].Colors, new byte[] { orange_fire, orange_fire, smoke, yellow_fire, orange_fire, orange_fire, smoke, yellow_fire, 0, 0, 0 }, 0.03f + 0.015f * f);
                    TransformLogic.MorphInPlace(ref aerialFrames[i][f].Colors, new byte[] { orange_fire, orange_fire, smoke, yellow_fire, orange_fire, orange_fire, smoke, yellow_fire, 0, 0, 0 }, 0.03f + 0.015f * f);
                    TransformLogic.MorphInPlace(ref wastedFrames[i][f].Colors, new byte[] { orange_fire, orange_fire, smoke, yellow_fire, orange_fire, orange_fire, smoke, yellow_fire, 0, 0, 0 }, 0.03f + 0.015f * f);
                }

            }

            for(int f = 1; f < 5; f++)
            {
                voxelFrames[f] = boneFrames[0][f - 1].Interpolate(aerialFrames[0][f - 1], f * 1f / 5f).Finalize(10 * Bone.Multiplier, 0);
                for(int b = 1; b < bones.Length; b++)
                {
                    voxelFrames[f] = TransformLogic.Overlap(voxelFrames[f], boneFrames[b][f - 1].Interpolate(aerialFrames[b][f - 1], f * 1f / 5f).FinalizeScatter(10 * Bone.Multiplier, 0));
                }
            }
            for(int f = 5; f < 10; f++)
            {
                voxelFrames[f] = aerialFrames[0][f - 1].Interpolate(wastedFrames[0][f - 1], (f - 5) * 1f / 4f).Finalize(10 * Bone.Multiplier, 0);
                for(int b = 1; b < bones.Length; b++)
                {
                    voxelFrames[f] = TransformLogic.Overlap(voxelFrames[f], aerialFrames[b][f - 1].Interpolate(wastedFrames[b][f - 1], (f - 5) * 1f / 4f).FinalizeScatter(10 * Bone.Multiplier, 0));
                }
            }
            for(int f = 10; f < 13; f++)
            {
                voxelFrames[f] = wastedFrames[0][f - 1].Finalize(10 * Bone.Multiplier, 0);
                for(int b = 1; b < bones.Length; b++)
                {
                    voxelFrames[f] = TransformLogic.Overlap(voxelFrames[f], wastedFrames[b][f - 1].FinalizeScatter(10 * Bone.Multiplier, 0));
                }
            }
            for(int f = 1, e = 0; f < 7 && e < maxExplosionFrames; f++, e++)
            {
                for(int b = 0; b < bones.Length; b++)
                {
                    if(sizes[b] >= 0)
                    {
                        if(xSize > 120)
                            voxelFrames[f] = TransformLogic.Overlap(voxelFrames[f], TransformLogic.Translate(SuperExplosions[sizes[b]][e], midX[b] - 80, midY[b] - 80, 0));
                        else
                            voxelFrames[f] = TransformLogic.Overlap(voxelFrames[f], TransformLogic.Translate(Explosions[sizes[b]][e], midX[b] - 60, midY[b] - 60, 0));
                    }
                }
            }

            byte[][,,] frames = new byte[12][,,];

            for(int f = 1; f < 13; f++)
            {
                frames[f - 1] = voxelFrames[f];
            }
            return frames;
        }



        public static byte ColorConversion(byte original)
        {
            if((249 - original) > 168)
                return (byte)(emitter0 - 4 * ((249 - original) % 4));
            else if((249 - original) % 8 != 0)
                return original;
            switch(249 - original)
            {
                case 0: return original;
                case 8: return 253 - 26 * 4;
                case 16: return 253 - 15 * 4;
                case 24: return 253 - 14 * 4;
                case 32: return 253 - 5 * 4;
                case 40: return 253 - 3 * 4;
                case 48: return 253 - 29 * 4;
                case 56: return 253 - 31 * 4;
                case 64:
                case 144: return 253 - 9 * 4;
                case 72: return 253 - 13 * 4;
                case 80: return 253 - 21 * 4;
                case 88: return 253 - 35 * 4;
                case 96: return 253 - 25 * 4;
                case 104: return 253 - 37 * 4;
                case 112: return 253 - 39 * 4;
                case 120: return 253 - 16 * 4;
                case 128: return 253 - 27 * 4;
                case 136: return 253 - 17 * 4;
                case 152: return 253 - 19 * 4;
                case 160: return 253 - 18 * 4;
                default: return original;
            }
        }
        public static void ConvertXW(string u)
        {
            BinaryReader bin = new BinaryReader(File.Open("CU1/" + u + "_Part_X.vox", FileMode.Open));
            List<MagicaVoxelData> raw = VoxelLogic.FromMagicaRaw(bin), next = new List<MagicaVoxelData>(raw.Count);

            foreach(MagicaVoxelData mvd in raw)
            {
                next.Add(new MagicaVoxelData {x = mvd.x, y = mvd.y, z = mvd.z, color = ColorConversion(mvd.color) });
            }
            VoxelLogic.WriteVOX("CU2/" + u + "_Large_W.vox", next, "W_EXACT", 5, 40, 40, 40);
        }
    }
}
