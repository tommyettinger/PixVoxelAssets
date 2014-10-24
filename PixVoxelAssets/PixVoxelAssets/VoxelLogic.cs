using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsPV
{
    public struct MagicaVoxelData
    {
        public byte x;
        public byte y;
        public byte z;
        public byte color;

        public MagicaVoxelData(BinaryReader stream, bool subsample)
        {
            x = (byte)stream.ReadByte();//(subsample ? stream.ReadByte() / 2 : stream.ReadByte());
            y = (byte)stream.ReadByte();//(subsample ? stream.ReadByte() / 2 : stream.ReadByte());
            z = (byte)stream.ReadByte();//(subsample ? stream.ReadByte() / 2 : stream.ReadByte());
            color = stream.ReadByte();
        }
    }

    public enum MovementType
    {
        Foot, Treads, TreadsAmphi, Wheels, WheelsTraverse, Flight, Immobile
    }
    class VoxelLogic
    {
        public static string[] Terrains = new string[]
        {"Plains","Forest","Desert","Jungle","Hills"
        ,"Mountains","Ruins","Tundra","Road","River", "Basement"};

        public static string[] CurrentUnits = {
"Infantry", "Infantry_P", "Infantry_S", "Infantry_T",
"Artillery", "Artillery_P", "Artillery_S", "Artillery_T",
"Tank", "Tank_P", "Tank_S", "Tank_T",
"Plane", "Plane_P", "Plane_S", "Plane_T",
"Supply", "Supply_P", "Supply_S", "Supply_T",
"Copter", "Copter_P", "Copter_S", "Copter_T", 
"City", "Factory", "Airport", "Laboratory", "Castle", "Estate"};
        public static Dictionary<string, int> UnitLookup = new Dictionary<string, int>(30), TerrainLookup = new Dictionary<string, int>(10);
        public static Dictionary<MovementType, List<int>> MobilityToUnits = new Dictionary<MovementType, List<int>>(30), MobilityToTerrains = new Dictionary<MovementType, List<int>>();
        public static List<int>[] TerrainToUnits = new List<int>[30];
        public static Dictionary<int, List<MovementType>> TerrainToMobilities = new Dictionary<int, List<MovementType>>();
        public static int[] CurrentSpeeds = {
3, 3, 5, 3,
4, 3, 6, 4,
6, 4, 7, 6,
7, 5, 9, 8,
5, 5, 6, 6,
7, 5, 8, 7, 
0,0,0,0,0,0};
        public static int[][] CurrentWeapons = {
new int[] {1, -1}, new int[] {0, 5}, new int[] {1, -1}, new int[] {0, 0},
new int[] {-1, 4}, new int[] {3, -1}, new int[] {-1, 6}, new int[] {-1, 6},
new int[] {3, 1}, new int[] {3, 1}, new int[] {1, -1}, new int[] {1, 3},
new int[] {1, -1}, new int[] {-1, 7}, new int[] {5, -1}, new int[] {5, -1},
new int[] {-1, -1}, new int[] {-1, -1}, new int[] {-1, -1}, new int[] {-1, -1},
new int[] {-1, -1}, new int[] {1, 5}, new int[] {1, -1}, new int[] {-1, -1},
new int[] {-1, -1}, new int[] {-1, -1}, new int[] {-1, -1}, new int[] {-1, -1}, new int[] {-1, -1}, new int[] {-1, -1}
};
        public static int[][] CurrentWeaponReceptions = {
new int[] {1, -1}, new int[] {1, 3}, new int[] {2, -1}, new int[] {1, 1},
new int[] {-1, 3}, new int[] {4, -1}, new int[] {-1, 3}, new int[] {-1, 4},
new int[] {2, 1}, new int[] {4, 2}, new int[] {2, -1}, new int[] {1, 2},
new int[] {2, -1}, new int[] {-1, 4}, new int[] {1, -1}, new int[] {2, -1},
new int[] {-1, -1}, new int[] {-1, -1}, new int[] {-1, -1}, new int[] {-1, -1},
new int[] {-1, -1}, new int[] {2, 2}, new int[] {2, -1}, new int[] {-1, -1},
new int[] {-1, -1}, new int[] {-1, -1}, new int[] {-1, -1}, new int[] {-1, -1}, new int[] {-1, -1}, new int[] {-1, -1}
};
        public static MovementType[] CurrentMobilities = {
MovementType.Foot, MovementType.Foot, MovementType.WheelsTraverse, MovementType.Foot,
MovementType.Treads, MovementType.Treads, MovementType.Treads, MovementType.Wheels,
MovementType.Treads, MovementType.Treads, MovementType.Treads, MovementType.TreadsAmphi,
MovementType.Flight, MovementType.Flight, MovementType.Flight, MovementType.Flight,
MovementType.Wheels, MovementType.Treads, MovementType.TreadsAmphi, MovementType.Wheels,
MovementType.Flight, MovementType.Flight, MovementType.Flight, MovementType.Flight, 
MovementType.Immobile, MovementType.Immobile, MovementType.Immobile, MovementType.Immobile, MovementType.Immobile, MovementType.Immobile, 
                                                         };

        public static void Initialize()
        {
            MovementType[] values = (MovementType[])Enum.GetValues(typeof(MovementType));
            foreach (MovementType v in values)
            {
                MobilityToUnits[v] = new List<int>();
            }
            for (int t = 0; t < Terrains.Length; t++)
            {
                TerrainLookup[Terrains[t]] = t;
                TerrainToMobilities[t] = new List<MovementType>();
                TerrainToUnits[t] = new List<int>();
            }
            for (int i = 0; i < CurrentUnits.Length; i++)
            {
                UnitLookup[CurrentUnits[i]] = i;
                MobilityToUnits[CurrentMobilities[i]].Add(i);
            }
            MobilityToTerrains[MovementType.Flight] =
                new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            MobilityToTerrains[MovementType.Foot] =
                new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            MobilityToTerrains[MovementType.Treads] =
                new List<int>() { 0, 1, 2, 3, 6, 7, 8 };
            MobilityToTerrains[MovementType.TreadsAmphi] =
                new List<int>() { 0, 1, 2, 3, 6, 7, 8, 9 };
            MobilityToTerrains[MovementType.Wheels] =
                new List<int>() { 0, 2, 7, 8, };
            MobilityToTerrains[MovementType.WheelsTraverse] =
                new List<int>() { 0, 1, 2, 3, 6, 7, 8 };
            MobilityToTerrains[MovementType.Immobile] =
                new List<int>() { };

            foreach (var kv in MobilityToTerrains)
            {
                foreach (int t in kv.Value)
                {
                    TerrainToMobilities[t].Add(kv.Key);
                }
            }
            foreach (var kv in TerrainToMobilities)
            {
                foreach (MovementType m in kv.Value)
                    TerrainToUnits[kv.Key].AddRange(MobilityToUnits[m]);
                TerrainToUnits[kv.Key] = TerrainToUnits[kv.Key].Distinct().ToList();
            }
        }

        public static string[] final_units =
        {
            "Person",
        };

        private static Random r = new Random();
        public static float flat_alpha = 0.77F;
        public static float waver_alpha = 0.83F;
        public static float spin_alpha_0 = 0.85F;
        public static float spin_alpha_1 = 0.87F;
        public static float fuzz_alpha = 0.91F;
        public static float[][] xcolors = new float[][]
        {
            //0 tires, tread
            new float[] {0.23F,0.2F,0.2F,1F},
            new float[] {0.23F,0.2F,0.2F,1F},
            new float[] {0.23F,0.2F,0.2F,1F},
            new float[] {0.23F,0.2F,0.2F,1F},
            new float[] {0.23F,0.2F,0.2F,1F},
            new float[] {0.23F,0.2F,0.2F,1F},
            new float[] {0.23F,0.2F,0.2F,1F},
            new float[] {0.23F,0.2F,0.2F,1F},
//            new float[] {0.3F,0.3F,0.33F,1F},
            
            //8 mud, wood
            new float[] {0.4F,0.25F,0.15F,1F},
            new float[] {0.2F,0.4F,0.3F,1F},
            new float[] {0.4F,0.25F,0.15F,1F},
            new float[] {0.4F,0.25F,0.15F,1F},
            new float[] {0.4F,0.25F,0.15F,1F},
            new float[] {0.4F,0.25F,0.15F,1F},
            new float[] {0.4F,0.25F,0.15F,1F},
            new float[] {0.4F,0.25F,0.15F,1F},
            //new float[] {0.4F,0.3F,0.2F,1F},
            
            //16 gun barrel
            new float[] {0.4F,0.35F,0.5F,1F},
            new float[] {0.3F,0.35F,0.4F,1F},
            new float[] {0.3F,0.35F,0.4F,1F},
            new float[] {0.3F,0.35F,0.4F,1F},
            new float[] {0.3F,0.35F,0.4F,1F},
            new float[] {0.3F,0.35F,0.4F,1F},
            new float[] {0.3F,0.35F,0.4F,1F},
            new float[] {0.3F,0.35F,0.4F,1F},
            
            //24 gun peripheral (sights, trigger)
            new float[] {0.4F,0.5F,0.4F,1F},
            new float[] {0.6F,0.8F,1F,1F},
            new float[] {0.4F,0.5F,0.4F,1F},
            new float[] {0.4F,0.5F,0.4F,1F},
            new float[] {0.4F,0.5F,0.4F,1F},
            new float[] {0.4F,0.5F,0.4F,1F},
            new float[] {0.4F,0.5F,0.4F,1F},
            new float[] {0.4F,0.5F,0.4F,1F},
            
            //32 main paint
            new float[] {0.3F,0.3F,0.3F,1F},     //black
            new float[] {0.79F,0.76F,0.7F,1F},  //white
            new float[] {0.86F,0.19F,0.1F,1F},         //red
            new float[] {1.1F,0.45F,0F,1F},      //orange
            new float[] {0.85F,0.71F,0.1F,1F},         //yellow
            new float[] {0.2F,0.7F,0.1F,1F},    //green
            new float[] {0.15F,0.25F,0.8F,1F},       //blue
            new float[] {0.4F,0.1F,0.45F,1F},       //purple
            
            //40 doors
            new float[] {0.6F,0.05F,-0.1F,1F},         //black
            new float[] {0.5F,0.6F,0.7F,1F},         //white
            new float[] {0.7F,-0.05F,-0.1F,1F},       //red
            new float[] {0.7F,0.25F,0.05F,1F},       //orange
            new float[] {0.63F,0.51F,-0.15F,1F},   //yellow
            new float[] {0.0F,0.45F,-0.1F,1F},     //green
            new float[] {0.1F,0.1F,0.5F,1F},       //blue
            new float[] {0.65F,0.15F,0.6F,1F},       //purple
            
            //48 cockpit
            new float[] {0.5F,0.5F,0.4F,1F},     //black
            new float[] {0.63F,0.55F,0.8F,1F},   //white
            new float[] {0.55F,0.36F,0.2F,1F}, // {0.9F,0.5F,0.4F,1F},     //red
            new float[] {0.87F,0.5F,0.2F,1F},    //orange
            new float[] {0.68F,0.61F,0.3F,1F},     //yellow
            new float[] {0.45F,0.25F,0.0F,1F},     //green
            new float[] {0.4F,0.45F,0.7F,1F},     //blue
            new float[] {0.55F,0.4F,0.65F,1F},   //purple

            //56 helmet
            new float[] {0.2F,0.15F,0.1F,1F},     //black
            new float[] {0.69F,0.66F,0.6F,1F},       //white
            new float[] {1F,0.15F,0.05F,1F},     //red
            new float[] {0.95F,0.35F,0.05F,1F},       //orange
            new float[] {0.68F,0.55F,0.2F,1F},         //yellow
            new float[] {0.29F,0.5F,0.2F,1F},       //green
            new float[] {0.2F,0.25F,0.5F,1F},       //blue
            new float[] {0.5F,0.3F,0.5F,1F},       //purple
            
            //64 flesh
            new float[] {0.87F,0.65F,0.3F,1F},  //black
            new float[] {0.7F,0.9F,0.4F,1F},      //white
            new float[] {0.87F,0.65F,0.3F,1F},  //red
            new float[] {0.87F,0.65F,0.3F,1F},  //orange
            new float[] {0.87F,0.65F,0.3F,1F},  //yellow
            new float[] {0.87F,0.65F,0.3F,1F},  //green
            new float[] {0.87F,0.65F,0.3F,1F},  //blue
            new float[] {0.87F,0.65F,0.3F,1F},  //purple
            //OLD new float[] {1.1F,0.89F,0.55F,1F},  //normal
            //WEIRD new float[] {0.55F,0.8F,-0.3F,1F},      //white
            
            //72 exposed metal
            new float[] {0.69F,0.62F,0.56F,1F},     //black
            new float[] {0.75F,0.75F,0.85F,1F},     //white
            new float[] {0.69F,0.62F,0.56F,1F},     //red
            new float[] {0.69F,0.62F,0.56F,1F},     //orange
            new float[] {0.69F,0.62F,0.56F,1F},     //yellow
            new float[] {0.69F,0.62F,0.56F,1F},     //green
            new float[] {0.69F,0.62F,0.56F,1F},     //blue
            new float[] {0.69F,0.62F,0.56F,1F},     //purple

            //80 lights
            new float[] {1.0F,1.0F,0.5F,1F},        //black
            new float[] {0.5F,1.2F,0.6F,1F},         //white
            new float[] {1.1F,0.9F,0.5F,1F},        //red
            new float[] {1.1F,0.9F,0.5F,1F},        //orange
            new float[] {1.05F,0.65F,0.15F,1F},        //yellow
            new float[] {0.9F,1.1F,0.5F,1F},        //green
            new float[] {0.9F,0.8F,0.65F,1F},        //blue
            new float[] {0.95F,0.9F,0.45F,1F},        //purple

            //88 windows
            new float[] {0.3F,0.5F,0.5F,1F},
            new float[] {0.3F,1F,0.9F,1F},
            new float[] {0.45F,0.7F,0.7F,1F},
            new float[] {0.45F,0.7F,0.7F,1F},
            new float[] {0.35F,0.3F,0.25F,1F},
            new float[] {0.35F,0.3F,0.25F,1F},
            new float[] {0.45F,0.7F,0.7F,1F},
            new float[] {0.45F,0.7F,0.7F,1F},

            //96 shadow (FLAT ALPHA)
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            
            //104 whirling rotors FRAME 0(SPIN ALPHA FRAME 0)
            new float[] {0.65F,0.65F,0.65F,spin_alpha_0},     //black
            new float[] {0.65F,0.7F,0.7F,spin_alpha_0},  //white
            new float[] {0.8F,0.25F,0.25F,spin_alpha_0},         //red
            new float[] {0.8F,0.5F,0.1F,spin_alpha_0},      //orange
            new float[] {0.85F,0.85F,0.35F,spin_alpha_0},         //yellow
            new float[] {0.4F,0.6F,0.35F,spin_alpha_0},    //green
            new float[] {0.55F,0.5F,0.95F,spin_alpha_0},       //blue
            new float[] {0.65F,0.35F,0.65F,spin_alpha_0},       //purple

            //112 whirling rotors FRAME 1(SPIN ALPHA FRAME 1)
            new float[] {0.65F,0.65F,0.65F,spin_alpha_1},     //black
            new float[] {0.65F,0.7F,0.7F,spin_alpha_1},  //white
            new float[] {0.8F,0.25F,0.25F,spin_alpha_1},         //red
            new float[] {0.8F,0.5F,0.1F,spin_alpha_1},      //orange
            new float[] {0.85F,0.85F,0.35F,spin_alpha_1},         //yellow
            new float[] {0.4F,0.6F,0.35F,spin_alpha_1},    //green
            new float[] {0.55F,0.5F,0.95F,spin_alpha_1},       //blue
            new float[] {0.65F,0.35F,0.65F,spin_alpha_1},       //purple
            /*
            new float[] {0.6F,0.6F,0.6F,0.21F},     //black
            new float[] {1.25F,1.35F,1.45F,0.21F},  //white
            new float[] {1.3F,0.25F,0.3F,0.21F},         //red
            new float[] {1.2F,0.6F,0.3F,0.21F},      //orange
            new float[] {1.4F,1.4F,0.6F,0.21F},         //yellow
            new float[] {0.4F,1.5F,0.4F,0.21F},    //green
            new float[] {0.4F,0.4F,1.4F,0.21F},       //blue
            new float[] {1.1F,0.2F,1.1F,0.21F},       //purple
            */
            //120 inner shadow (NO ALPHA)
            new float[] {0.13F,0.10F,0.04F,1F},
            new float[] {0.13F,0.10F,0.04F,1F},
            new float[] {0.13F,0.10F,0.04F,1F},
            new float[] {0.13F,0.10F,0.04F,1F},
            new float[] {0.13F,0.10F,0.04F,1F},
            new float[] {0.13F,0.10F,0.04F,1F},
            new float[] {0.13F,0.10F,0.04F,1F},
            new float[] {0.13F,0.10F,0.04F,1F},
            
            //128 water splash (FLAT ALPHA)
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            
            //136 smoke (NO ALPHA)
            new float[] {0.12F,0.08F,-0.01F,1F},
            new float[] {0.01F,0.12F,0.16F,1F},
            new float[] {0.14F,0.14F,0.02F,1F},
            new float[] {0.14F,0.14F,0.02F,1F},
            new float[] {0.14F,0.14F,0.02F,1F},
            new float[] {0.14F,0.14F,0.02F,1F},
            new float[] {0.14F,0.14F,0.02F,1F},
            new float[] {0.14F,0.14F,0.02F,1F},

            //144 guts
            new float[] {0.67F,0.05F,-0.1F,1F},  //black
            new float[] {0.5F,0.8F,-0.2F,1F},      //white
            new float[] {0.67F,0.05F,-0.1F,1F},  //red
            new float[] {0.67F,0.05F,-0.1F,1F},  //orange
            new float[] {0.67F,0.05F,-0.1F,1F},  //yellow
            new float[] {0.67F,0.05F,-0.1F,1F},  //green
            new float[] {0.67F,0.05F,-0.1F,1F},  //blue
            new float[] {0.67F,0.05F,-0.1F,1F},  //purple
            
            //152 orange fire
            new float[] {1.25F,0.7F,0.3F,1F},
            new float[] {0.52F,1.1F,0.4F,1F},
            new float[] {1.25F,0.7F,0.3F,1F},
            new float[] {1.25F,0.7F,0.3F,1F},
            new float[] {1.25F,0.7F,0.3F,1F},
            new float[] {1.25F,0.7F,0.3F,1F},
            new float[] {1.25F,0.7F,0.3F,1F},
            new float[] {1.25F,0.7F,0.3F,1F},
            
            //160 yellow fire
            new float[] {1.25F,1.1F,0.45F,1F},
            new float[] {0.55F,1.3F,0.6F,1F},
            new float[] {1.25F,1.1F,0.45F,1F},
            new float[] {1.25F,1.1F,0.45F,1F},
            new float[] {1.25F,1.1F,0.45F,1F},
            new float[] {1.25F,1.1F,0.45F,1F},
            new float[] {1.25F,1.1F,0.45F,1F},
            new float[] {1.25F,1.1F,0.45F,1F},
            
            //168 total transparent
            new float[] {0F,0F,0F,0F},
            
            //169-176 markers
            new float[] {0F,0F,0F,0F},
            //169 Bomb Drop
            new float[] {0F,0F,0F,0F},
            //170 Arc Missile
            new float[] {0F,0F,0F,0F},
            //171 Rocket
            new float[] {0F,0F,0F,0F},
            //172 Artillery Cannon
            new float[] {0F,0F,0F,0F},
            //173 Cannon
            new float[] {0F,0F,0F,0F},
            //174 AA Gun
            new float[] {0F,0F,0F,0F},
            //175 Machine Gun
            new float[] {0F,0F,0F,0F},
            //176 Handgun

            //177 padding (7 elements)
            new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F},

            //junk, 184-255
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            //8 final garbage
/*            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},*/
        };

        public static float[][] wcolors = new float[][]
        {
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
            new float[] {0.35F,0.1F,-0.05F,1F},
            //7 hair
            new float[] {0.45F,0.2F,0.0F,1F},
            //8 skin contrast
            new float[] {0.77F,0.45F,0.05F,1F},
            //9 skin
            new float[] {0.87F,0.65F,0.3F,1F},
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
            new float[] {0.13F,0.10F,0.04F,1F},
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
            //41 placeholder
            new float[] {0F,0F,0F,0F},
            //42 placeholder
            new float[] {0F,0F,0F,0F},
            //43 placeholder
            new float[] {0F,0F,0F,0F},
            //44 placeholder
            new float[] {0F,0F,0F,0F},
            //45 placeholder
            new float[] {0F,0F,0F,0F},
            //46 placeholder
            new float[] {0F,0F,0F,0F},
            //47 total transparent
            new float[] {0F,0F,0F,0F},
        };

        public static float[][][] wpalettes = new float[][][]
            {
            new float[][] { //0 brown hair
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
            new float[] {0.77F,0.45F,0.05F,1F},
            //9 skin
            new float[] {0.87F,0.65F,0.3F,1F},
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
            new float[] {0.13F,0.10F,0.04F,1F},
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
            //41 placeholder
            new float[] {0F,0F,0F,0F},
            //42 placeholder
            new float[] {0F,0F,0F,0F},
            //43 placeholder
            new float[] {0F,0F,0F,0F},
            //44 placeholder
            new float[] {0F,0F,0F,0F},
            //45 placeholder
            new float[] {0F,0F,0F,0F},
            //46 placeholder
            new float[] {0F,0F,0F,0F},
            //47 total transparent
            new float[] {0F,0F,0F,0F},
            },

            new float[][] { //1 blonde hair
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
            new float[] {0.8F,0.65F,0.2F,1F},
            //7 hair
            new float[] {0.7F,0.65F,0.35F,1F},
            //8 skin contrast
            new float[] {0.82F,0.5F,0.1F,1F},
            //9 skin
            new float[] {0.89F,0.69F,0.32F,1F},
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
            new float[] {0.13F,0.10F,0.04F,1F},
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
            //41 placeholder
            new float[] {0F,0F,0F,0F},
            //42 placeholder
            new float[] {0F,0F,0F,0F},
            //43 placeholder
            new float[] {0F,0F,0F,0F},
            //44 placeholder
            new float[] {0F,0F,0F,0F},
            //45 placeholder
            new float[] {0F,0F,0F,0F},
            //46 placeholder
            new float[] {0F,0F,0F,0F},
            //47 total transparent
            new float[] {0F,0F,0F,0F},
            },

            new float[][] { //2 zombie
            //0 shoes, boots, brown leather contrast
            new float[] {0.3F,0.1F,0.0F,1F},
            //1 shoes, boots, brown leather
            new float[] {0.4F,0.18F,0.0F,1F},
            //2 pants, jeans contrast
            new float[] {0F,0F,0F,0F},
            //3 pants, jeans
            new float[] {0.6F,0.55F,0.3F,1F},
            //4 shirt contrast
            new float[] {0.35F,0.1F,0.0F,1F},
            //5 shirt
            new float[] {0.44F,0.36F,0.33F,1F},
            //6 hair contrast
            new float[] {0.1F,0.25F,0.05F,1F},
            //7 hair
            new float[] {0.0F,0.2F,0.0F,1F},
            //8 skin contrast
            new float[] {0.4F,0.05F,-0.1F,1F},
            //9 skin
            new float[] {0.45F,0.57F,0.35F,1F},
            //10 eyes shine
            new float[] {1.4F,0.6F,0.4F,1F},
            //11 eyes
            new float[] {0.8F,0.15F,0.0F,1F},
            //12 metal contrast
            new float[] {0.4F,0.7F,0.4F,1F},
            //13 metal
            new float[] {0.4F,0.4F,0.4F,1F},
            //14 flowing clothes contrast
            new float[] {0.35F,0.1F,0.0F,waver_alpha},
            //15 flowing clothes
            new float[] {0.44F,0.36F,0.33F,waver_alpha},
            //16 inner shadow
            new float[] {0.13F,0.10F,0.04F,1F},
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
            //41 placeholder
            new float[] {0F,0F,0F,0F},
            //42 placeholder
            new float[] {0F,0F,0F,0F},
            //43 placeholder
            new float[] {0F,0F,0F,0F},
            //44 placeholder
            new float[] {0F,0F,0F,0F},
            //45 placeholder
            new float[] {0F,0F,0F,0F},
            //46 placeholder
            new float[] {0F,0F,0F,0F},
            //47 total transparent
            new float[] {0F,0F,0F,0F},
            },

            new float[][] {//3 ninja
            //0 shoes, boots, brown leather contrast
            new float[] {0.2F,0.0F,0.0F,1F},
            //1 shoes, boots, brown leather
            new float[] {0.5F,0.5F,0.5F,1F},
            //2 pants, jeans contrast
            new float[] {0.65F,0.5F,0.2F,1F},
            //3 pants, jeans
            new float[] {0.75F,0.6F,0.35F,1F},
            //4 shirt contrast
            new float[] {0.2F,0.2F,0.2F,1F},
            //5 shirt
            new float[] {0.3F,0.3F,0.3F,1F},
            //6 hair contrast
            new float[] {0.7F,0.8F,1.0F,1F},
            //7 hair
            new float[] {0.0F,0.1F,0.2F,1F},
            //8 skin contrast
            new float[] {0.82F,0.6F,0.1F,1F},
            //9 skin
            new float[] {0.9F,0.7F,0.3F,1F},
            //10 eyes shine
            new float[] {1.4F,1.4F,1.4F,1F},
            //11 eyes
            new float[] {0.05F,0.0F,0.05F,1F},
            //12 metal contrast
            new float[] {0.5F,0.04F,0.04F,1F},
            //13 metal
            new float[] {0.7F,0.75F,0.85F,1F},
            //14 flowing clothes contrast
            new float[] {0.2F,0.2F,0.2F,waver_alpha},
            //15 flowing clothes
            new float[] {0.3F,0.3F,0.3F,waver_alpha},
            //16 inner shadow
            new float[] {0.13F,0.10F,0.04F,1F},
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
            //41 placeholder
            new float[] {0F,0F,0F,0F},
            //42 placeholder
            new float[] {0F,0F,0F,0F},
            //43 placeholder
            new float[] {0F,0F,0F,0F},
            //44 placeholder
            new float[] {0F,0F,0F,0F},
            //45 placeholder
            new float[] {0F,0F,0F,0F},
            //46 placeholder
            new float[] {0F,0F,0F,0F},
            //47 total transparent
            new float[] {0F,0F,0F,0F},
            },

            new float[][] {//4 shogun
            //0 white cloth
            new float[] {0.85F,0.8F,0.9F,1F},
            //1 shoes, boots, black socks
            new float[] {0.1F,0.05F,-0.05F,1F},
            //2 belt
            new float[] {0.5F,0.35F,0.15F,1F},
            //3 pants, jeans
            new float[] {0.15F,0.15F,0.15F,1F},
            //4 shirt contrast
            new float[] {0.4F,0.15F,0.6F,1F},
            //5 shirt
            new float[] {0.25F,0.2F,0.7F,1F},
            //6 hair contrast
            new float[] {0.85F,0.75F,0.3F,1F},
            //7 hair
            new float[] {-0.05F,-0.05F,-0.05F,1F},
            //8 skin contrast
            new float[] {0.7F,0.1F,0.15F,1F},
            //9 skin
            new float[] {0.8F,0.8F,0.82F,1F},
            //10 eyes shine
            new float[] {1.4F,1.4F,1.4F,1F},
            //11 eyes
            new float[] {0.15F,0.2F,0.2F,1F},
            //12 metal contrast
            new float[] {0.5F,0.04F,0.04F,1F},
            //13 metal
            new float[] {0.7F,0.75F,0.85F,1F},
            //14 flowing clothes contrast
            new float[] {0.4F,0.15F,0.6F,waver_alpha},
            //15 flowing clothes
            new float[] {0.25F,0.2F,0.7F,waver_alpha},
            //16 inner shadow
            new float[] {0.13F,0.10F,0.04F,1F},
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
            new float[] {0.62F,0.2F,0.1F,1F},
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
            //41 placeholder
            new float[] {0F,0F,0F,0F},
            //42 placeholder
            new float[] {0F,0F,0F,0F},
            //43 placeholder
            new float[] {0F,0F,0F,0F},
            //44 placeholder
            new float[] {0F,0F,0F,0F},
            //45 placeholder
            new float[] {0F,0F,0F,0F},
            //46 placeholder
            new float[] {0F,0F,0F,0F},
            //47 total transparent
            new float[] {0F,0F,0F,0F},
            },

            new float[][] {//5 samurai
            //0 white cloth
            new float[] {0.85F,0.8F,0.9F,1F},
            //1 shoes, boots, black socks
            new float[] {0.1F,0.05F,-0.05F,1F},
            //2 belt
            new float[] {0.3F,0.5F,0.2F,1F},
            //3 pants, jeans
            new float[] {0.15F,0.15F,0.15F,1F},
            //4 shirt contrast
            new float[] {0.1F,0.7F,0.4F,1F},
            //5 shirt
            new float[] {0.15F,0.6F,0.3F,1F},
            //6 hair contrast
            new float[] {0.7F,1.15F,0.6F,1F},
            //7 hair
            new float[] {-0.05F,-0.05F,-0.05F,1F},
            //8 skin contrast
            new float[] {0.7F,0.1F,0.15F,1F},
            //9 skin
            new float[] {0.88F,0.75F,0.7F,1F},
            //10 eyes shine
            new float[] {1.2F,1.2F,1.2F,1F},
            //11 eyes
            new float[] {0.15F,0.2F,0.2F,1F},
            //12 metal contrast
            new float[] {0.1F,0.3F,0.05F,1F},
            //13 metal
            new float[] {0.75F,0.8F,0.9F,1F},
            //14 flowing clothes contrast
            new float[] {0.1F,0.7F,0.4F,waver_alpha},
            //15 flowing clothes
            new float[] {0.15F,0.6F,0.3F,waver_alpha},
            //16 inner shadow
            new float[] {0.13F,0.10F,0.04F,1F},
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
            //41 placeholder
            new float[] {0F,0F,0F,0F},
            //42 placeholder
            new float[] {0F,0F,0F,0F},
            //43 placeholder
            new float[] {0F,0F,0F,0F},
            //44 placeholder
            new float[] {0F,0F,0F,0F},
            //45 placeholder
            new float[] {0F,0F,0F,0F},
            //46 placeholder
            new float[] {0F,0F,0F,0F},
            //47 total transparent
            new float[] {0F,0F,0F,0F},
            },
            
            new float[][] {//6 skeleton
            //0 bone highlight
            new float[] {0.5F,0.45F,0.35F,1F},
            //1 bones
            new float[] {0.85F,0.85F,0.7F,1F},
            //2 pants, jeans contrast
            new float[] {0F,0F,0F,0F},
            //3 pants, jeans
            new float[] {0.6F,0.55F,0.3F,1F},
            //4 shirt contrast
            new float[] {0F,0F,0F,0F},
            //5 shirt
            new float[] {0.44F,0.36F,0.33F,1F},
            //6 wood contrast
            new float[] {0.4F,0.15F,-0.05F,1F},
            //7 wood
            new float[] {0.55F,0.3F,0.05F,1F},
            //8 skin contrast
            new float[] {0.4F,0.05F,-0.1F,1F},
            //9 skin
            new float[] {0.45F,0.57F,0.35F,1F},
            //10 eyes shine
            new float[] {0.5F,0F,-0.05F,1F},
            //11 eyes
            new float[] {0.75F,0.15F,0.05F,1F},
            //12 metal contrast
            new float[] {0.3F,0.35F,0.35F,1F},
            //13 metal
            new float[] {0.45F,0.5F,0.5F,1F},
            //14 flowing clothes contrast
            new float[] {0.35F,0.1F,0.0F,waver_alpha},
            //15 flowing clothes
            new float[] {0.44F,0.36F,0.33F,waver_alpha},
            //16 inner shadow
            new float[] {0.13F,0.10F,0.04F,1F},
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
            //41 placeholder
            new float[] {0F,0F,0F,0F},
            //42 placeholder
            new float[] {0F,0F,0F,0F},
            //43 placeholder
            new float[] {0F,0F,0F,0F},
            //44 placeholder
            new float[] {0F,0F,0F,0F},
            //45 placeholder
            new float[] {0F,0F,0F,0F},
            //46 placeholder
            new float[] {0F,0F,0F,0F},
            //47 total transparent
            new float[] {0F,0F,0F,0F},
            },

            new float[][] {//7 spirit
            //0 white cloth
            new float[] {0.85F,0.8F,0.9F,1F},
            //1 shoes, boots, black socks
            new float[] {0.1F,0.05F,-0.05F,1F},
            //2 pants, jeans contrast
            new float[] {0F,0F,0F,1F},
            //3 pants, jeans
            new float[] {0.6F,0.55F,0.3F,1F},
            //4 shirt contrast
            new float[] {0.9F,0.9F,0.9F,1F},
            //5 shirt
            new float[] {1.1F,1.1F,1.1F,1F},
            //6 hair contrast
            new float[] {0.65F,0.55F,0.35F,1F},
            //7 hair
            new float[] {0.6F,0.6F,0.6F,1F},
            //8 skin contrast
            new float[] {0.5F,0.5F,0.5F,1F},
            //9 skin
            new float[] {0.75F,0.75F,0.75F,1F},
            //10 eyes shine
            new float[] {0.6F,0.8F,1.1F,1F},
            //11 eyes
            new float[] {0.5F,0.7F,1.0F,1F},
            //12 metal contrast
            new float[] {1.0F,0.9F,0.5F,1F},
            //13 metal
            new float[] {0.7F,0.8F,0.85F,1F},
            //14 flowing rags contrast
            new float[] {0.9F,0.9F,0.9F,waver_alpha},
            //15 flowing rags
            new float[] {1.1F,1.1F,1.1F,waver_alpha},
            //16 inner shadow
            new float[] {0.13F,0.10F,0.04F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {0.65F,1.1F,1.15F,1F},
            //19 orange fire
            new float[] {0.35F,1.2F,1.05F,1F},
            //20 sparks
            new float[] {0.8F,1.2F,1.25F,1F},
            //21 glow frame 0
            new float[] {0.55F,0.9F,0.95F,1F},
            //22 glow frame 1
            new float[] {0.75F,1.1F,1.15F,1F},
            //23 glow frame 2
            new float[] {0.55F,0.9F,0.95F,1F},
            //24 glow frame 3
            new float[] {0.35F,0.7F,0.75F,1F},
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
            new float[] {0.9F,1.0F,1.1F,1F},
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
            //41 placeholder
            new float[] {0F,0F,0F,0F},
            //42 placeholder
            new float[] {0F,0F,0F,0F},
            //43 placeholder
            new float[] {0F,0F,0F,0F},
            //44 placeholder
            new float[] {0F,0F,0F,0F},
            //45 placeholder
            new float[] {0F,0F,0F,0F},
            //46 placeholder
            new float[] {0F,0F,0F,0F},
            //47 total transparent
            new float[] {0F,0F,0F,0F},
            },

            new float[][] {//8 wraith
            //0 white cloth
            new float[] {0.85F,0.8F,0.9F,1F},
            //1 shoes, boots, black socks
            new float[] {0.1F,0.05F,-0.05F,1F},
            //2 pants, jeans contrast
            new float[] {0F,0F,0F,1F},
            //3 pants, jeans
            new float[] {0.6F,0.55F,0.3F,1F},
            //4 shirt contrast
            new float[] {0.15F,0.15F,0.15F,1F},
            //5 shirt
            new float[] {0.3F,0.3F,0.3F,1F},
            //6 hair contrast
            new float[] {0.7F,0.8F,0.9F,1F},
            //7 hair
            new float[] {0.6F,0.6F,0.6F,1F},
            //8 skin contrast
            new float[] {0.5F,0.5F,0.5F,1F},
            //9 skin
            new float[] {0.75F,0.75F,0.75F,1F},
            //10 eyes shine
            new float[] {0.6F,0.8F,1.1F,1F},
            //11 eyes
            new float[] {0.5F,0.7F,1.0F,1F},
            //12 metal contrast
            new float[] {0.5F,0.8F,0.85F,1F},
            //13 metal
            new float[] {0.4F,0.65F,0.7F,1F},
            //14 flowing rags contrast
            new float[] {0.15F,0.15F,0.15F,waver_alpha},
            //15 flowing rags
            new float[] {0.3F,0.3F,0.3F,waver_alpha},
            //16 inner shadow
            new float[] {0.13F,0.10F,0.04F,1F},
            //17 smoke
            new float[] {0.14F,0.14F,0.02F,waver_alpha},
            //18 yellow fire
            new float[] {0.85F,0.4F,0.95F,1F},
            //19 orange fire
            new float[] {0.7F,0.3F,0.85F,1F},
            //20 sparks
            new float[] {1.15F,0.6F,1.25F,1F},
            //21 glow frame 0
            new float[] {1.0F,0.75F,0.7F,1F},
            //22 glow frame 1
            new float[] {1.2F,0.95F,0.9F,1F},
            //23 glow frame 2
            new float[] {1.0F,0.75F,0.7F,1F},
            //24 glow frame 3
            new float[] {0.8F,0.55F,0.5F,1F},
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
            new float[] {0.57F,-0.05F,-0.1F,1F},
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
            //41 placeholder
            new float[] {0F,0F,0F,0F},
            //42 placeholder
            new float[] {0F,0F,0F,0F},
            //43 placeholder
            new float[] {0F,0F,0F,0F},
            //44 placeholder
            new float[] {0F,0F,0F,0F},
            //45 placeholder
            new float[] {0F,0F,0F,0F},
            //46 placeholder
            new float[] {0F,0F,0F,0F},
            //47 total transparent
            new float[] {0F,0F,0F,0F},
            },
            
            new float[][] { //9 cinder
            //0 bone highlight
            new float[] {0.1F,0.05F,0.05F,1F},
            //1 bones
            new float[] {0.5F,0.5F,0.35F,1F},
            //2 pants, jeans contrast
            new float[] {0.0F,-0.15F,-0.15F,1F},
            //3 pants, jeans
            new float[] {0.15F,-0.05F,-0.1F,1F},
            //4 shirt contrast
            new float[] {0.15F,0.0F,-0.1F,1F},
            //5 shirt
            new float[] {0.25F,0.05F,-0.05F,1F},
            //6 hair contrast
            new float[] {0.15F,0.05F,-0.05F,1F},
            //7 hair
            new float[] {0.15F,0.05F,-0.05F,1F},
            //8 skin contrast
            new float[] {0.2F,-0.05F,-0.1F,1F},
            //9 skin
            new float[] {0.6F,0.15F,0.0F,1F},
            //10 eyes shine
            new float[] {0.4F,0.85F,1.2F,1F},
            //11 eyes
            new float[] {0.25F,0.7F,1.05F,1F},
            //12 metal contrast
            new float[] {0.75F,0.45F,-0.1F,1F},
            //13 metal
            new float[] {0.75F,0.45F,-0.1F,1F},
            //14 flowing clothes contrast
            new float[] {0.2F,0.0F,-0.05F,waver_alpha},
            //15 flowing clothes
            new float[] {0.3F,0.05F,0.0F,waver_alpha},
            //16 inner shadow
            new float[] {0.13F,0.10F,0.04F,1F},
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
            new float[] {0.69F,0.09F,-0.08F,1F},
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
            //41 placeholder
            new float[] {0F,0F,0F,0F},
            //42 placeholder
            new float[] {0F,0F,0F,0F},
            //43 placeholder
            new float[] {0F,0F,0F,0F},
            //44 placeholder
            new float[] {0F,0F,0F,0F},
            //45 placeholder
            new float[] {0F,0F,0F,0F},
            //46 placeholder
            new float[] {0F,0F,0F,0F},
            //47 total transparent
            new float[] {0F,0F,0F,0F},
            },
            
            new float[][] { //10 nodebpe
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
            new float[] {0.13F,0.10F,0.04F,1F},
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
            //41 placeholder
            new float[] {0F,0F,0F,0F},
            //42 placeholder
            new float[] {0F,0F,0F,0F},
            //43 placeholder
            new float[] {0F,0F,0F,0F},
            //44 placeholder
            new float[] {0F,0F,0F,0F},
            //45 placeholder
            new float[] {0F,0F,0F,0F},
            //46 placeholder
            new float[] {0F,0F,0F,0F},
            //47 total transparent
            new float[] {0F,0F,0F,0F},
            },
            
            new float[][] { //11 tassar
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
            new float[] {0.05F,-0.05F,0.35F,1F},
            //12 metal contrast
            new float[] {0.5F,0.85F,0.95F,1F},
            //13 metal
            new float[] {0.75F,0.95F,1.15F,1F},
            //14 flowing clothes contrast
            new float[] {0.4F,0.1F,0.3F,waver_alpha},
            //15 flowing clothes
            new float[] {0.3F,0.15F,0.5F,waver_alpha},
            //16 inner shadow
            new float[] {0.13F,0.10F,0.04F,1F},
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
            //41 placeholder
            new float[] {0F,0F,0F,0F},
            //42 placeholder
            new float[] {0F,0F,0F,0F},
            //43 placeholder
            new float[] {0F,0F,0F,0F},
            //44 placeholder
            new float[] {0F,0F,0F,0F},
            //45 placeholder
            new float[] {0F,0F,0F,0F},
            //46 placeholder
            new float[] {0F,0F,0F,0F},
            //47 total transparent
            new float[] {0F,0F,0F,0F},
            },
            
            new float[][] { //12 ilapa
            //0 spike contrast
            new float[] {0.5F,0.5F,0.5F,1F},
            //1 spikes
            new float[] {0.65F,0.65F,0.65F,1F},
            //2 arms contrast
            new float[] {0.55F,0.55F,0.4F,1F},
            //3 arms
            new float[] {0.75F,0.75F,0.5F,1F},
            //4 fur contrast
            new float[] {1.05F,0.7F,0.5F,1F},
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
            new float[] {0.13F,0.10F,0.04F,1F},
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
            new float[] {0.15F,0.2F,0.7F,1F},
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
            //41 placeholder
            new float[] {0F,0F,0F,0F},
            //42 placeholder
            new float[] {0F,0F,0F,0F},
            //43 placeholder
            new float[] {0F,0F,0F,0F},
            //44 placeholder
            new float[] {0F,0F,0F,0F},
            //45 placeholder
            new float[] {0F,0F,0F,0F},
            //46 placeholder
            new float[] {0F,0F,0F,0F},
            //47 total transparent
            new float[] {0F,0F,0F,0F},
            },
            
            new float[][] { //13 kurguiv
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
            new float[] {0.13F,0.10F,0.04F,1F},
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
            //41 placeholder
            new float[] {0F,0F,0F,0F},
            //42 placeholder
            new float[] {0F,0F,0F,0F},
            //43 placeholder
            new float[] {0F,0F,0F,0F},
            //44 placeholder
            new float[] {0F,0F,0F,0F},
            //45 placeholder
            new float[] {0F,0F,0F,0F},
            //46 placeholder
            new float[] {0F,0F,0F,0F},
            //47 total transparent
            new float[] {0F,0F,0F,0F},
            },

            new float[][] { //14 erezdo
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
            new float[] {0.13F,0.10F,0.04F,1F},
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
            //41 placeholder
            new float[] {0F,0F,0F,0F},
            //42 placeholder
            new float[] {0F,0F,0F,0F},
            //43 placeholder
            new float[] {0F,0F,0F,0F},
            //44 placeholder
            new float[] {0F,0F,0F,0F},
            //45 placeholder
            new float[] {0F,0F,0F,0F},
            //46 placeholder
            new float[] {0F,0F,0F,0F},
            //47 total transparent
            new float[] {0F,0F,0F,0F},
            },
            
            new float[][] { //15 gold-skinned human
            //0 shoes, boots, tennis shoes contrast
            new float[] {0.5F,0.5F,0.5F,1F},
            //1 shoes, boots, tennis shoes
            new float[] {0.8F,0.8F,0.8F,1F},
            //2 pants, jeans contrast
            new float[] {0.5F,0.55F,0.5F,1F},
            //3 pants, jeans
            new float[] {0.65F,0.7F,0.65F,1F},
            //4 shirt contrast
            new float[] {0.5F,-0.05F,0.0F,1F},
            //5 shirt
            new float[] {0.8F,-0.05F,0.0F,1F},
            //6 hair contrast
            new float[] {0.0F,-0.05F,-0.1F,1F},
            //7 hair
            new float[] {0.05F,0.0F,-0.05F,1F},
            //8 skin contrast
            new float[] {0.7F,0.5F,0.0F,1F},
            //9 skin
            new float[] {0.91F,0.75F,0.35F,1F},
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
            new float[] {0.13F,0.10F,0.04F,1F},
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
            //41 placeholder
            new float[] {0F,0F,0F,0F},
            //42 placeholder
            new float[] {0F,0F,0F,0F},
            //43 placeholder
            new float[] {0F,0F,0F,0F},
            //44 placeholder
            new float[] {0F,0F,0F,0F},
            //45 placeholder
            new float[] {0F,0F,0F,0F},
            //46 placeholder
            new float[] {0F,0F,0F,0F},
            //47 total transparent
            new float[] {0F,0F,0F,0F},
            },
            
            new float[][] { //16 dark-skinned human
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
            new float[] {0.23F,0.12F,0.07F,1F},
            //9 skin
            new float[] {0.45F,0.22F,0.02F,1F},
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
            new float[] {0.13F,0.10F,0.04F,1F},
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
            //41 placeholder
            new float[] {0F,0F,0F,0F},
            //42 placeholder
            new float[] {0F,0F,0F,0F},
            //43 placeholder
            new float[] {0F,0F,0F,0F},
            //44 placeholder
            new float[] {0F,0F,0F,0F},
            //45 placeholder
            new float[] {0F,0F,0F,0F},
            //46 placeholder
            new float[] {0F,0F,0F,0F},
            //47 total transparent
            new float[] {0F,0F,0F,0F},
            },

            new float[][] { //17 brown-skinned human
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
            new float[] {0.13F,0.10F,0.04F,1F},
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
            //41 placeholder
            new float[] {0F,0F,0F,0F},
            //42 placeholder
            new float[] {0F,0F,0F,0F},
            //43 placeholder
            new float[] {0F,0F,0F,0F},
            //44 placeholder
            new float[] {0F,0F,0F,0F},
            //45 placeholder
            new float[] {0F,0F,0F,0F},
            //46 placeholder
            new float[] {0F,0F,0F,0F},
            //47 total transparent
            new float[] {0F,0F,0F,0F},
            },
            
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
            
            new float[][] { //18 cerpali
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
            ////4 pincer stripe
            //new float[] {0.7F,0.0F,-0.05F,1F},
            ////5 pincer
            //new float[] {0.95F,0.2F,0.15F,1F},
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
            new float[] {0.13F,0.10F,0.04F,1F},
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
            //41 placeholder
            new float[] {0F,0F,0F,0F},
            //42 placeholder
            new float[] {0F,0F,0F,0F},
            //43 placeholder
            new float[] {0F,0F,0F,0F},
            //44 placeholder
            new float[] {0F,0F,0F,0F},
            //45 placeholder
            new float[] {0F,0F,0F,0F},
            //46 placeholder
            new float[] {0F,0F,0F,0F},
            //47 total transparent
            new float[] {0F,0F,0F,0F},
            },

            new float[][] { //19 vashk
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
            new float[] {0.13F,0.10F,0.04F,1F},
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
            //41 placeholder
            new float[] {0F,0F,0F,0F},
            //42 placeholder
            new float[] {0F,0F,0F,0F},
            //43 placeholder
            new float[] {0F,0F,0F,0F},
            //44 placeholder
            new float[] {0F,0F,0F,0F},
            //45 placeholder
            new float[] {0F,0F,0F,0F},
            //46 placeholder
            new float[] {0F,0F,0F,0F},
            //47 total transparent
            new float[] {0F,0F,0F,0F},
            },
            
            new float[][] { //20 lomuk
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
            new float[] {0.13F,0.10F,0.04F,1F},
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
            //41 placeholder
            new float[] {0F,0F,0F,0F},
            //42 placeholder
            new float[] {0F,0F,0F,0F},
            //43 placeholder
            new float[] {0F,0F,0F,0F},
            //44 placeholder
            new float[] {0F,0F,0F,0F},
            //45 placeholder
            new float[] {0F,0F,0F,0F},
            //46 placeholder
            new float[] {0F,0F,0F,0F},
            //47 total transparent
            new float[] {0F,0F,0F,0F},
            },
            
            new float[][] { //21 glarosp
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
            new float[] {0.13F,0.10F,0.04F,1F},
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
            //41 placeholder
            new float[] {0F,0F,0F,0F},
            //42 placeholder
            new float[] {0F,0F,0F,0F},
            //43 placeholder
            new float[] {0F,0F,0F,0F},
            //44 placeholder
            new float[] {0F,0F,0F,0F},
            //45 placeholder
            new float[] {0F,0F,0F,0F},
            //46 placeholder
            new float[] {0F,0F,0F,0F},
            //47 total transparent
            new float[] {0F,0F,0F,0F},
            },
            
            new float[][] { //22 pelmir
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
            new float[] {0.13F,0.10F,0.04F,1F},
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
            //41 placeholder
            new float[] {0F,0F,0F,0F},
            //42 placeholder
            new float[] {0F,0F,0F,0F},
            //43 placeholder
            new float[] {0F,0F,0F,0F},
            //44 placeholder
            new float[] {0F,0F,0F,0F},
            //45 placeholder
            new float[] {0F,0F,0F,0F},
            //46 placeholder
            new float[] {0F,0F,0F,0F},
            //47 total transparent
            new float[] {0F,0F,0F,0F},
            },
            
            new float[][] { //23 uljir
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
            new float[] {0.13F,0.10F,0.04F,1F},
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
            //41 placeholder
            new float[] {0F,0F,0F,0F},
            //42 placeholder
            new float[] {0F,0F,0F,0F},
            //43 placeholder
            new float[] {0F,0F,0F,0F},
            //44 placeholder
            new float[] {0F,0F,0F,0F},
            //45 placeholder
            new float[] {0F,0F,0F,0F},
            //46 placeholder
            new float[] {0F,0F,0F,0F},
            //47 total transparent
            new float[] {0F,0F,0F,0F},
            },
            
            new float[][] { //24 sfyst
            //0 feet contrast
            new float[] {0.25F,0.2F,0.15F,1F},
            //1 feet
            new float[] {0.35F,0.3F,0.25F,1F},
            //2 clothing contrast
            new float[] {0.5F,0.25F,0.75F,1F},
            //3 clothing
            new float[] {0.4F,0.35F,0.85F,1F},
            //4 body, fins contrast
            new float[] {0.45F,0.4F,0.35F,1F},
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
            new float[] {0.45F,0.42F,0.37F,waver_alpha},
            //15 trunktacles
            new float[] {0.6F,0.57F,0.52F,waver_alpha},
            //16 inner shadow
            new float[] {0.13F,0.10F,0.04F,1F},
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
            //41 placeholder
            new float[] {0F,0F,0F,0F},
            //42 placeholder
            new float[] {0F,0F,0F,0F},
            //43 placeholder
            new float[] {0F,0F,0F,0F},
            //44 placeholder
            new float[] {0F,0F,0F,0F},
            //45 placeholder
            new float[] {0F,0F,0F,0F},
            //46 placeholder
            new float[] {0F,0F,0F,0F},
            //47 total transparent
            new float[] {0F,0F,0F,0F},
            },
            
        };
        public const int wcolorcount = 48, wpalettecount = 25;

        public static float[][] xcolours = new float[256][];
        public static byte[][] xrendered;
        public static byte[][] storeColorCubes()
        {
            byte[][] cubes = new byte[168 + 32][];

            Image image = new Bitmap("cube_soft.png");
            Image flat = new Bitmap("flat_soft.png");
            Image spin = new Bitmap("spin_soft.png");
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

            for (int current_color = 0; current_color < 168; current_color++)
            {
                Bitmap b =
                new Bitmap(4, 4, PixelFormat.Format32bppArgb);

                Graphics g = Graphics.FromImage((Image)b);

                if (current_color / 8 == 96 / 8)
                {
                    colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+xcolors[current_color][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+xcolors[current_color][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+xcolors[current_color][2],  0, 0},
   new float[] {0,  0,  0,  1, 0},
   new float[] {0, 0, 0, 0, 1F}});
                }
                else if (xcolors[current_color][3] == flat_alpha)
                {
                    colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+xcolors[current_color][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+xcolors[current_color][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+xcolors[current_color][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                }
                else if (current_color / 8 == 10) //lights
                {
                    float lightCalc = 0.06F;
                    colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+xcolors[current_color][0] + lightCalc,  0,  0,  0, 0},
   new float[] {0,  0.251F+xcolors[current_color][1] + lightCalc,  0,  0, 0},
   new float[] {0,  0,  0.31F+xcolors[current_color][2] + lightCalc,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                }
                else
                {
                    colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+xcolors[current_color][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+xcolors[current_color][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+xcolors[current_color][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                }
                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);
                g.DrawImage((current_color / 8 == 10 || current_color / 8 == 13 || current_color / 8 == 14) ? spin :
                   (xcolors[current_color][3] == 1F || current_color / 8 == 15 || current_color / 8 == 16) ? image :
                   (xcolors[current_color][3] == flat_alpha) ? flat : spin,
                   new Rectangle(0, 0,
                       width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
                cubes[current_color] = new byte[64];
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        try
                        {
                            Color c = b.GetPixel(i, j);
                            cubes[current_color][i * 4 + j * 16 + 0] = c.B;
                            cubes[current_color][i * 4 + j * 16 + 1] = c.G;
                            cubes[current_color][i * 4 + j * 16 + 2] = c.R;
                            cubes[current_color][i * 4 + j * 16 + 3] = c.A;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.StackTrace);
                            Console.WriteLine(e.InnerException);
                        }
                    }
                }

            }
            for (int current_color = 80; current_color < 88; current_color++)
            {
                for (int frame = 0; frame < 4; frame++)
                {
                    Bitmap b =
                    new Bitmap(4, 4, PixelFormat.Format32bppArgb);

                    Graphics g = Graphics.FromImage((Image)b);
                    float lightCalc = (0.5F - (((frame % 4) % 3) + ((frame % 4) / 3))) * 0.12F;
                    colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+xcolors[current_color][0] + lightCalc,  0,  0,  0, 0},
   new float[] {0,  0.251F+xcolors[current_color][1] + lightCalc,  0,  0, 0},
   new float[] {0,  0,  0.31F+xcolors[current_color][2] + lightCalc,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    g.DrawImage(spin,
                       new Rectangle(0, 0,
                           width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                       0, 0,        // upper-left corner of source rectangle 
                       width,       // width of source rectangle
                       height,      // height of source rectangle
                       GraphicsUnit.Pixel,
                       imageAttributes);

                    cubes[88 + current_color + (8 * frame)] = new byte[64];
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            Color c = b.GetPixel(i, j);
                            cubes[88 + current_color + (8 * frame)][i * 4 + j * 16 + 0] = c.B;
                            cubes[88 + current_color + (8 * frame)][i * 4 + j * 16 + 1] = c.G;
                            cubes[88 + current_color + (8 * frame)][i * 4 + j * 16 + 2] = c.R;
                            cubes[88 + current_color + (8 * frame)][i * 4 + j * 16 + 3] = c.A;
                        }
                    }
                }
            }
            return cubes;
        }
        public static byte[][][] wrendered;
        public static byte[][] wcurrent;
        private static byte[][][] storeColorCubesW()
        {
            byte[, ,] cubes = new byte[wpalettecount, wcolorcount, 64];

            Image image = new Bitmap("cube_soft.png");
            Image flat = new Bitmap("flat_soft.png");
            Image spin = new Bitmap("spin_soft.png");
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
            for (int p = 0; p < wpalettecount; p++)
            {
                for (int current_color = 0; current_color < wcolorcount; current_color++)
                {
                    Bitmap b =
                    new Bitmap(width, height, PixelFormat.Format32bppArgb);

                    Graphics g = Graphics.FromImage((Image)b);

                    if (current_color == 25)
                    {
                        colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+VoxelLogic.wpalettes[p][current_color][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+VoxelLogic.wpalettes[p][current_color][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+VoxelLogic.wpalettes[p][current_color][2],  0, 0},
   new float[] {0,  0,  0,  1, 0},
   new float[] {0, 0, 0, 0, 1F}});
                    }
                    else if (VoxelLogic.wpalettes[p][current_color][3] == 0F)
                    {
                        colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 1F}});
                    }
                    else if (VoxelLogic.wpalettes[p][current_color][3] == VoxelLogic.flat_alpha)
                    {
                        colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+VoxelLogic.wpalettes[p][current_color][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+VoxelLogic.wpalettes[p][current_color][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+VoxelLogic.wpalettes[p][current_color][2],  0, 0},
   new float[] {0,  0,  0,  VoxelLogic.flat_alpha, 0},
   new float[] {0, 0, 0, 0, 1F}});
                    }
                    else
                    {
                        colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+VoxelLogic.wpalettes[p][current_color][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+VoxelLogic.wpalettes[p][current_color][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+VoxelLogic.wpalettes[p][current_color][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                    }
                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    g.DrawImage((current_color >= 18 && current_color <= 24) ? spin :
                       (VoxelLogic.wpalettes[p][current_color][3] == 1F || VoxelLogic.wpalettes[p][current_color][3] == waver_alpha) ? image :
                       (VoxelLogic.wpalettes[p][current_color][3] == VoxelLogic.flat_alpha) ? flat : spin,
                       new Rectangle(0, 0,
                           width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                       0, 0,        // upper-left corner of source rectangle 
                       width,       // width of source rectangle
                       height,      // height of source rectangle
                       GraphicsUnit.Pixel,
                       imageAttributes);
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            Color c = b.GetPixel(i, j);
                            cubes[p, current_color, i * 4 + j * 16 + 0] = c.B;
                            cubes[p, current_color, i * 4 + j * 16 + 1] = c.G;
                            cubes[p, current_color, i * 4 + j * 16 + 2] = c.R;
                            cubes[p, current_color, i * 4 + j * 16 + 3] = c.A;
                        }
                    }
                }
                /*                for (int current_color = 80; current_color < 88; current_color++)
                                {
                                    for (int frame = 0; frame < 4; frame++)
                                    {
                                        Bitmap b =
                                        new Bitmap(2, 3, PixelFormat.Format32bppArgb);

                                        Graphics g = Graphics.FromImage((Image)b);
                                        float lightCalc = (0.5F - (((frame % 4) % 3) + ((frame % 4) / 3))) * 0.12F;
                                        colorMatrix = new ColorMatrix(new float[][]{ 
                   new float[] {0.22F+VoxelLogic.xcolors[current_color][0] + lightCalc,  0,  0,  0, 0},
                   new float[] {0,  0.251F+VoxelLogic.xcolors[current_color][1] + lightCalc,  0,  0, 0},
                   new float[] {0,  0,  0.31F+VoxelLogic.xcolors[current_color][2] + lightCalc,  0, 0},
                   new float[] {0,  0,  0,  1F, 0},
                   new float[] {0, 0, 0, 0, 1F}});

                                        imageAttributes.SetColorMatrix(
                                           colorMatrix,
                                           ColorMatrixFlag.Default,
                                           ColorAdjustType.Bitmap);
                                        g.DrawImage(spin,
                                           new Rectangle(0, 0,
                                               width, height),  // destination rectangle 
                                            //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                                           0, 0,        // upper-left corner of source rectangle 
                                           width,       // width of source rectangle
                                           height,      // height of source rectangle
                                           GraphicsUnit.Pixel,
                                           imageAttributes);

                                        cubes[88 + current_color + (8 * frame)] = new byte[24];
                                        for (int i = 0; i < 2; i++)
                                        {
                                            for (int j = 0; j < 3; j++)
                                            {
                                                Color c = b.GetPixel(i, j);
                                                cubes[88 + current_color + (8 * frame)][i * 4 + j * 8 + 0] = c.B;
                                                cubes[88 + current_color + (8 * frame)][i * 4 + j * 8 + 1] = c.G;
                                                cubes[88 + current_color + (8 * frame)][i * 4 + j * 8 + 2] = c.R;
                                                cubes[88 + current_color + (8 * frame)][i * 4 + j * 8 + 3] = c.A;
                                            }
                                        }
                                    }
                                }*/
            }
            byte[][][] cubes2 = new byte[wpalettecount][][];
            for (int i = 0; i < wpalettecount; i++)
            {
                cubes2[i] = new byte[wcolorcount][];
                for (int c = 0; c < wcolorcount; c++)
                {
                    cubes2[i][c] = new byte[64];
                    for (int j = 0; j < 64; j++)
                    {
                        cubes2[i][c][j] = cubes[i, c, j];
                    }
                }
            }
            return cubes2;
        }



        public static void InitializeXPalette()
        {
            xrendered = storeColorCubes();
        }

        public static void InitializeWPalette()
        {
            wrendered = storeColorCubesW();
        }


        public class Bresenham3D : IEnumerable<MagicaVoxelData>
        {
            MagicaVoxelData start;
            MagicaVoxelData end;
            float steps = 1;

            public Bresenham3D(MagicaVoxelData p_start, MagicaVoxelData p_end)
            {
                start = p_start;
                end = p_end;
                steps = 1;
            }

            public IEnumerator<MagicaVoxelData> GetEnumerator()
            {
                MagicaVoxelData result = new MagicaVoxelData { x = start.x, y = start.y, z = start.z, color = start.color };

                int xd, yd, zd;
                int x, y, z;
                int ax, ay, az;
                int sx, sy, sz;
                int dx, dy, dz;

                dx = (int)(end.x - start.x);
                dy = (int)(end.y - start.y);
                dz = (int)(end.z - start.z);

                ax = Math.Abs(dx) << 1;
                ay = Math.Abs(dy) << 1;
                az = Math.Abs(dz) << 1;

                sx = (int)Math.Sign((float)dx);
                sy = (int)Math.Sign((float)dy);
                sz = (int)Math.Sign((float)dz);

                x = (int)start.x;
                y = (int)start.y;
                z = (int)start.z;

                if (ax >= Math.Max(ay, az)) // x dominant
                {
                    yd = ay - (ax >> 1);
                    zd = az - (ax >> 1);
                    for (; ; )
                    {
                        result.x = (byte)(x / steps);
                        result.y = (byte)(y / steps);
                        result.z = (byte)(z / steps);
                        yield return result;

                        if (x == (int)end.x)
                            yield break;

                        if (yd >= 0)
                        {
                            y += sy;
                            yd -= ax;
                        }

                        if (zd >= 0)
                        {
                            z += sz;
                            zd -= ax;
                        }

                        x += sx;
                        yd += ay;
                        zd += az;
                    }
                }
                else if (ay >= Math.Max(ax, az)) // y dominant
                {
                    xd = ax - (ay >> 1);
                    zd = az - (ay >> 1);
                    for (; ; )
                    {
                        result.x = (byte)(x / steps);
                        result.y = (byte)(y / steps);
                        result.z = (byte)(z / steps);
                        yield return result;

                        if (y == (byte)end.y)
                            yield break;

                        if (xd >= 0)
                        {
                            x += sx;
                            xd -= ay;
                        }

                        if (zd >= 0)
                        {
                            z += sz;
                            zd -= ay;
                        }

                        y += sy;
                        xd += ax;
                        zd += az;
                    }
                }
                else if (az >= Math.Max(ax, ay)) // z dominant
                {
                    xd = ax - (az >> 1);
                    yd = ay - (az >> 1);
                    for (; ; )
                    {
                        result.x = (byte)(x / steps);
                        result.y = (byte)(y / steps);
                        result.z = (byte)(z / steps);
                        yield return result;

                        if (z == (byte)end.z)
                            yield break;

                        if (xd >= 0)
                        {
                            x += sx;
                            xd -= az;
                        }

                        if (yd >= 0)
                        {
                            y += sy;
                            yd -= az;
                        }

                        z += sz;
                        xd += ax;
                        yd += ay;
                    }
                }
            }
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        private static int sizex = 0, sizey = 0, sizez = 0;
        /// <summary>
        /// Load a MagicaVoxel .vox format file into a MagicaVoxelData[] that we use for voxel chunks.
        /// </summary>
        /// <param name="stream">An open BinaryReader stream that is the .vox file.</param>
        /// <returns>The voxel chunk data for the MagicaVoxel .vox file.</returns>
        public static MagicaVoxelData[] FromMagica(BinaryReader stream)
        {
            // check out http://voxel.codeplex.com/wikipage?title=VOX%20Format&referringTitle=Home for the file format used below

            List<MagicaVoxelData> voxelData = new List<MagicaVoxelData>();//, voxelsAltered = new List<MagicaVoxelData>();
            int[,] taken = new int[20, 20];
            string magic = new string(stream.ReadChars(4));
            int version = stream.ReadInt32();

            // a MagicaVoxel .vox file starts with a 'magic' 4 character 'VOX ' identifier
            if (magic == "VOX ")
            {
                bool subsample = false;

                while (stream.BaseStream.Position < stream.BaseStream.Length)
                {
                    // each chunk has an ID, size and child chunks
                    char[] chunkId = stream.ReadChars(4);
                    int chunkSize = stream.ReadInt32();
                    int childChunks = stream.ReadInt32();
                    string chunkName = new string(chunkId);

                    // there are only 2 chunks we only care about, and they are SIZE and XYZI
                    if (chunkName == "SIZE")
                    {
                        sizex = stream.ReadInt32();
                        sizey = stream.ReadInt32();
                        sizez = stream.ReadInt32();
                        taken = new int[sizex, sizey];
                        taken.Fill(-1);
                        if (sizex > 32 || sizey > 32) subsample = true;

                        stream.ReadBytes(chunkSize - 4 * 3);
                    }
                    else if (chunkName == "XYZI")
                    {
                        // XYZI contains n voxels
                        int numVoxels = stream.ReadInt32();
                        int div = (subsample ? 2 : 1);

                        // each voxel has x, y, z and color index values
                        for (int i = 0; i < numVoxels; i++)
                            voxelData.Add(new MagicaVoxelData(stream, subsample));
                    }
                    else if (chunkName == "RGBA")
                    {
                        //colors = new float[256][];

                        for (int i = 0; i < 256; i++)
                        {
                            byte r = stream.ReadByte();
                            byte g = stream.ReadByte();
                            byte b = stream.ReadByte();
                            byte a = stream.ReadByte();

                        }
                    }
                    else stream.ReadBytes(chunkSize);   // read any excess bytes
                }

                if (voxelData.Count == 0) return voxelData.ToArray(); // failed to read any valid voxel data

                // now push the voxel data into our voxel chunk structure

            }
            /*            taken.Fill(-1);
                        foreach (MagicaVoxelData mvd in voxelsAltered.FindAll(v => v.z == 0))
                        {
                            taken[mvd.x, mvd.y] = 2;
                        }
                        foreach (MagicaVoxelData mvd in voxelsAltered.FindAll(v => v.z >= 100))
                        {
                            if(taken[mvd.x, mvd.y] != 2)
                            {
                                MagicaVoxelData vox = new MagicaVoxelData();
                                vox.x = mvd.x;
                                vox.y = mvd.y;
                                vox.z = (byte)(0);
                                vox.color = 249 - 96;
                                voxelData.Add(vox);
                            }
                        }*/

            return PlaceShadows(voxelData).ToArray();
        }
        public static List<MagicaVoxelData> PlaceShadows(List<MagicaVoxelData> voxelData)
        {
            int[,] taken = new int[sizex, sizey];
            taken.Fill(-1);
            List<MagicaVoxelData> voxelsAltered = new List<MagicaVoxelData>(voxelData.Count * 9 / 8);
            for (int i = 0; i < voxelData.Count; i++)
            {
                if (voxelData[i].x >= sizex || voxelData[i].y >= sizey)
                {
                    continue;
                }
                voxelsAltered.Add(voxelData[i]);
                if (-1 == taken[voxelData[i].x, voxelData[i].y] && voxelData[i].color != 249 - 80 && voxelData[i].color != 249 - 104 && voxelData[i].color != 249 - 112
                     && voxelData[i].color != 249 - 96 && voxelData[i].color != 249 - 128 && voxelData[i].color != 249 - 136 && voxelData[i].color != 249 - 152 && voxelData[i].color != 249 - 160
                     && voxelData[i].color > 249 - 168)
                {
                    MagicaVoxelData vox = new MagicaVoxelData();
                    vox.x = voxelData[i].x;
                    vox.y = voxelData[i].y;
                    vox.z = (byte)(0);
                    vox.color = 249 - 96;
                    taken[vox.x, vox.y] = voxelsAltered.Count();
                    voxelsAltered.Add(vox);
                }
            }
            return voxelsAltered;
        }
        public static List<MagicaVoxelData> PlaceShadowsW(List<MagicaVoxelData> voxelData)
        {
            int[,] taken = new int[sizex, sizey];
            taken.Fill(-1);
            List<MagicaVoxelData> voxelsAltered = new List<MagicaVoxelData>(voxelData.Count * 9 / 8);
            for (int i = 0; i < voxelData.Count; i++)
            {
                if (voxelData[i].x >= sizex || voxelData[i].y >= sizey)
                {
                    continue;
                }
                voxelsAltered.Add(voxelData[i]);
                if (-1 == taken[voxelData[i].x, voxelData[i].y]
                     && (voxelData[i].color > 253 - 21 * 4 || voxelData[i].color < 253 - 24 * 4)
                     && voxelData[i].color != 253 - 25 * 4 && voxelData[i].color != 253 - 27 * 4
                     && voxelData[i].color != 253 - 17 * 4 && voxelData[i].color != 253 - 18 * 4 && voxelData[i].color != 253 - 19 * 4
                     && voxelData[i].color > 253 - 47 * 4)
                {
                    MagicaVoxelData vox = new MagicaVoxelData();
                    vox.x = voxelData[i].x;
                    vox.y = voxelData[i].y;
                    vox.z = (byte)(0);
                    vox.color = 249 - 96;
                    taken[vox.x, vox.y] = voxelsAltered.Count();
                    voxelsAltered.Add(vox);
                }
            }
            return voxelsAltered;
        }

        /// <summary>
        /// Load a MagicaVoxel .vox format file into a MagicaVoxelData[] that we use for voxel chunks.
        /// </summary>
        /// <param name="stream">An open BinaryReader stream that is the .vox file.</param>
        /// <returns>The voxel chunk data for the MagicaVoxel .vox file.</returns>
        public static List<MagicaVoxelData> FromMagicaRaw(BinaryReader stream)
        {
            // check out http://voxel.codeplex.com/wikipage?title=VOX%20Format&referringTitle=Home for the file format used below

            List<MagicaVoxelData> voxelData = new List<MagicaVoxelData>(), voxelsAltered = new List<MagicaVoxelData>();
            string magic = new string(stream.ReadChars(4));
            int version = stream.ReadInt32();

            // a MagicaVoxel .vox file starts with a 'magic' 4 character 'VOX ' identifier
            if (magic == "VOX ")
            {
                bool subsample = false;

                while (stream.BaseStream.Position < stream.BaseStream.Length)
                {
                    // each chunk has an ID, size and child chunks
                    char[] chunkId = stream.ReadChars(4);
                    int chunkSize = stream.ReadInt32();
                    int childChunks = stream.ReadInt32();
                    string chunkName = new string(chunkId);

                    // there are only 2 chunks we only care about, and they are SIZE and XYZI
                    if (chunkName == "SIZE")
                    {
                        sizex = stream.ReadInt32();
                        sizey = stream.ReadInt32();
                        sizez = stream.ReadInt32();
                        if (sizex > 32 || sizey > 32) subsample = true;

                        stream.ReadBytes(chunkSize - 4 * 3);
                    }
                    else if (chunkName == "XYZI")
                    {
                        // XYZI contains n voxels
                        int numVoxels = stream.ReadInt32();
                        int div = (subsample ? 2 : 1);

                        // each voxel has x, y, z and color index values
                        for (int i = 0; i < numVoxels; i++)
                            voxelData.Add(new MagicaVoxelData(stream, subsample));
                    }
                    else if (chunkName == "RGBA")
                    {
                        //colors = new float[256][];

                        for (int i = 0; i < 256; i++)
                        {
                            byte r = stream.ReadByte();
                            byte g = stream.ReadByte();
                            byte b = stream.ReadByte();
                            byte a = stream.ReadByte();

                        }
                    }
                    else stream.ReadBytes(chunkSize);   // read any excess bytes
                }

                if (voxelData.Count == 0) return voxelData; // failed to read any valid voxel data

                // now push the voxel data into our voxel chunk structure
                for (int i = 0; i < voxelData.Count; i++)
                {
                    // do not store this voxel if it lies out of range of the voxel chunk (32x128x32)
                    //if (voxelData[i].x > 31 || voxelData[i].y > 31 || voxelData[i].z > 127) continue;

                    voxelsAltered.Add(voxelData[i]);

                }
            }
            /*            taken.Fill(-1);
                        foreach (MagicaVoxelData mvd in voxelsAltered.FindAll(v => v.z == 0))
                        {
                            taken[mvd.x, mvd.y] = 2;
                        }
                        foreach (MagicaVoxelData mvd in voxelsAltered.FindAll(v => v.z >= 100))
                        {
                            if(taken[mvd.x, mvd.y] != 2)
                            {
                                MagicaVoxelData vox = new MagicaVoxelData();
                                vox.x = mvd.x;
                                vox.y = mvd.y;
                                vox.z = (byte)(0);
                                vox.color = 249 - 96;
                                voxelData.Add(vox);
                            }
                        }*/

            return voxelsAltered;
        }

        public static MagicaVoxelData[] AssembleHeadToBody(BinaryReader body, bool raw)
        {
            List<MagicaVoxelData> head = FromMagicaRaw(new BinaryReader(File.Open("Head_Part_X.vox", FileMode.Open))).ToList();
            List<MagicaVoxelData> bod = FromMagicaRaw(body).ToList();
            MagicaVoxelData bodyPlug = new MagicaVoxelData { color = 255 };
            MagicaVoxelData headPlug = new MagicaVoxelData { color = 255 };
            foreach (MagicaVoxelData mvd in bod)
            {
                if (mvd.color > 249 - 168 && (250 - mvd.color) % 8 == 0)
                {
                    bodyPlug = mvd;
                    bodyPlug.color--;
                    break;
                }
            }
            if (bodyPlug.color == 255)
                return ((raw) ? bod : PlaceShadows(bod)).ToArray();
            foreach (MagicaVoxelData mvd in head)
            {
                if (mvd.color > 249 - 168 && (250 - mvd.color) % 8 == 0)
                {
                    headPlug = mvd;
                    headPlug.color--;
                    break;
                }
            }
            List<MagicaVoxelData> working = new List<MagicaVoxelData>(head.Count + bod.Count);
            for (int i = 0; i < head.Count; i++)
            {
                MagicaVoxelData mvd = head[i];
                mvd.x += (byte)(bodyPlug.x - headPlug.x);
                mvd.y += (byte)(bodyPlug.y - headPlug.y);
                mvd.z += (byte)(bodyPlug.z - headPlug.z);
                if ((250 - mvd.color) % 8 == 0)
                    mvd.color--;
                working.Add(mvd);
            }
            bod.AddRange(working);
            if (raw)
            {
                return bod.ToArray();
            }
            return PlaceShadows(bod).ToArray();
        }


        public static void Madden()
        {
            for (int i = 0; i < 256; i++)
            {
                xcolours[i] = xcolors[i];
            }

            for (int i = 1; i < 23; i++)
            {
                float alpha = 1F;
                switch (i)
                {
                    case 22: alpha = 0F; break;

                    case 17: alpha = flat_alpha; break;
                    case 13: alpha = flat_alpha; break;
                    case 14: alpha = spin_alpha_0; break;
                    case 15: alpha = spin_alpha_1; break;
                }
                if (i == 22)
                {

                    xcolors[(i - 1) * 8 + 0] = new float[] { 1F, 1F, 1F, 0F };
                    xcolors[(i - 1) * 8 + 1] = new float[] { 1F, 1F, 1F, 0F };
                    xcolors[(i - 1) * 8 + 2] = new float[] { 1F, 1F, 1F, 0F };
                    xcolors[(i - 1) * 8 + 3] = new float[] { 1F, 1F, 1F, 0F };
                    xcolors[(i - 1) * 8 + 4] = new float[] { 1F, 1F, 1F, 0F };
                    xcolors[(i - 1) * 8 + 5] = new float[] { 1F, 1F, 1F, 0F };
                    xcolors[(i - 1) * 8 + 6] = new float[] { 1F, 1F, 1F, 0F };
                    xcolors[(i - 1) * 8 + 7] = new float[] { 1F, 1F, 1F, 0F };
                    xcolors[(i - 1) * 8 + 8] = new float[] { 1F, 1F, 1F, 0F };
                }
                else if (i == 11)
                {
                    xcolors[(i - 1) * 8 + 0] = new float[] { 1.0F, 0.5F, 1.0F, 1F };
                    xcolors[(i - 1) * 8 + 1] = new float[] { 1.0F, 0.5F, 1.0F, 1F };
                    xcolors[(i - 1) * 8 + 2] = new float[] { 1.0F, 0.5F, 1.0F, 1F };
                    xcolors[(i - 1) * 8 + 3] = new float[] { 1.0F, 0.5F, 1.0F, 1F };
                    xcolors[(i - 1) * 8 + 4] = new float[] { 1.0F, 0.5F, 1.0F, 1F };
                    xcolors[(i - 1) * 8 + 5] = new float[] { 1.0F, 0.5F, 1.0F, 1F };
                    xcolors[(i - 1) * 8 + 6] = new float[] { 1.0F, 0.5F, 1.0F, 1F };
                    xcolors[(i - 1) * 8 + 7] = new float[] { 1.0F, 0.5F, 1.0F, 1F };
                }
                else
                {
                    xcolors[(i - 1) * 8 + 0] = new float[] { 1 / i, 1F / i * 1.5F, 0.9F / (24 - i), alpha };
                    xcolors[(i - 1) * 8 + 1] = new float[] { 1 / i, 1F / i * 1.5F, 0.9F / (24 - i), alpha };
                    xcolors[(i - 1) * 8 + 2] = new float[] { 1 / i, 1F / i * 1.5F, 0.9F / (24 - i), alpha };
                    xcolors[(i - 1) * 8 + 3] = new float[] { 1 / i, 1F / i * 1.5F, 0.9F / (24 - i), alpha };
                    xcolors[(i - 1) * 8 + 4] = new float[] { 1 / i, 1F / i * 1.5F, 0.9F / (24 - i), alpha };
                    xcolors[(i - 1) * 8 + 5] = new float[] { 1 / i, 1F / i * 1.5F, 0.9F / (24 - i), alpha };
                    xcolors[(i - 1) * 8 + 6] = new float[] { 1 / i, 1F / i * 1.5F, 0.9F / (24 - i), alpha };
                    xcolors[(i - 1) * 8 + 7] = new float[] { 1 / i, 1F / i * 1.5F, 0.9F / (24 - i), alpha };
                }
            }
            for (int i = 177; i < 256; i++)
            {
                xcolors[i] = new float[] { 0, 0, 0, 0 };
            }
        }
        public static void Awaken()
        {
            for (int i = 0; i < 256; i++)
            {
                xcolors[i] = xcolours[i];
            }
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
            if (pos.z < 60)
                near.Add(new MagicaVoxelData { x = (byte)(pos.x), y = (byte)(pos.y), z = (byte)(pos.z + 1), color = (byte)(colors.RandomElement()), });
            if (pos.z > 0)
                near.Add(new MagicaVoxelData { x = (byte)(pos.x), y = (byte)(pos.y), z = (byte)(pos.z - 1), color = (byte)(colors.RandomElement()), });
            return near;

        }

        public static MagicaVoxelData[][] FieryExplosionDouble(MagicaVoxelData[] voxels, bool blowback)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[9][];
            voxelFrames[0] = new MagicaVoxelData[voxels.Length];
            voxels.CopyTo(voxelFrames[0], 0);
            for (int i = 0; i < voxels.Length; i++)
            {
                voxelFrames[0][i].x += 40;
                voxelFrames[0][i].y += 40;
            }
            for (int f = 1; f < 4; f++)
            {
                List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxels.Length);
                MagicaVoxelData[] vls = new MagicaVoxelData[voxelFrames[f - 1].Length], working = new MagicaVoxelData[voxelFrames[f - 1].Length]; //.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)
                voxelFrames[f - 1].CopyTo(vls, 0);
                voxelFrames[f - 1].CopyTo(working, 0);

                int[] minX = new int[40 * 2];
                int[] maxX = new int[40 * 2];
                float[] midX = new float[40 * 2];
                for (int level = 0; level < 40 * 2; level++)
                {
                    minX[level] = vls.Min(v => v.x * ((v.z != level || v.color == 249 - 96 || v.color == 249 - 128 ||
                        v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color <= 249 - 168) ? 2000 : 1));
                    maxX[level] = vls.Max(v => v.x * ((v.z != level || v.color == 249 - 96 || v.color == 249 - 128 ||
                        v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color <= 249 - 168) ? 0 : 1));
                    midX[level] = (maxX[level] + minX[level]) / 2F;
                }

                int[] minY = new int[40 * 2];
                int[] maxY = new int[40 * 2];
                float[] midY = new float[40 * 2];
                for (int level = 0; level < 40 * 2; level++)
                {
                    minY[level] = vls.Min(v => v.y * ((v.z != level || v.color == 249 - 96 || v.color == 249 - 128 ||
                        v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color <= 249 - 168) ? 2000 : 1));
                    maxY[level] = vls.Max(v => v.y * ((v.z != level || v.color == 249 - 96 || v.color == 249 - 128 ||
                        v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color <= 249 - 168) ? 0 : 1));
                    midY[level] = (maxY[level] + minY[level]) / 2F;
                }

                int minZ = vls.Min(v => v.z * ((v.color == 249 - 96 || v.color == 249 - 128 ||
                        v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color <= 249 - 168) ? 2000 : 1));
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
                        mvd.color = (byte)((r.Next(10) == 0) ? 249 - 152 : 249 - 144); //random transform to orange fire
                    else if (v.color <= 249 - 168) //clear and markers
                        mvd.color = (byte)249 - 168; //clear stays clear
                    else if (v.color == 249 - 120)
                        mvd.color = 249 - 168; //clear inner shadow
                    else if (v.color == 249 - 96)
                        mvd.color = 249 - 96; //shadow stays shadow
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
                        zMove = (f - 1) * 1.8f;
                        xMove = (float)(r.NextDouble() * 2.0 - 1.0);
                        yMove = (float)(r.NextDouble() * 2.0 - 1.0);
                    }
                    else
                    {
                        /*
                        if (v.x > midX[v.z])// && v.x > maxX - 5)
                            xMove = ((midX[v.z] - r.Next(3) - ((blowback) ? 5 : 0) - (maxX[v.z] - v.x)) * 0.6F * ((v.z - minZ + 3) / (maxZ - minZ + 1F)));// / 300F) * (v.z + 5); //5 -
                        else if (v.x < midX[v.z])// && v.x < minX + 5)
                            xMove = ((0 + (v.x - midX[v.z] + r.Next(3) - ((blowback) ? 8 : 0))) * 0.6F * ((v.z - minZ + 3) / (maxZ - minZ + 1F))); // / 300F) * ((v.z + 5)) * f; //-5 +
                        if (v.y > midY[v.z])// && v.y > maxY - 5)
                            yMove = ((0 + midY[v.z] - r.Next(3) - (maxY[v.z] - v.y)) * 0.6F * ((v.z - minZ + 3) / (maxZ - minZ + 1F))); // / 300F) * (v.z + 5);//5 -
                        else if (v.y < midY[v.z])// && v.y < minY + 5)
                            yMove = ((-10 + (v.y - midY[v.z] + r.Next(3))) * 0.6F * ((v.z - minZ + 3) / (maxZ - minZ + 1F))); // / 300F) * (v.z + 5); //-5 +
                        */

                        //for higher values: center - (between 9 and 11) - distance from current voxel x to edge x, times variable based on height
                        //                    60                                                  70         80 = 50
                        if (v.x > midX[v.z])
                            xMove = ((midX[v.z] - r.Next(3) - ((blowback) ? 9 : 0) - (maxX[v.z] - v.x)) * 0.6F * ((v.z - minZ + 3) / (maxZ - minZ + 1F)));
                        //for lower values: distance from current voxel x to center - (between 6 to 8), times variable based on height
                        else if (v.x < midX[v.z])
                            xMove = ((midX[v.z] + r.Next(3) - ((blowback) ? 8 : 0) - minX[v.z] + v.x) * -0.6F * ((v.z - minZ + 3) / (maxZ - minZ + 1F)));// / 300F) * (v.z + 5); //5 -
                        //           60                                             40        50
                        //xMove = ((0 + (v.x - midX[v.z] + r.Next(3) - ((blowback) ? 8 : 0))) * 0.8F * ((v.z - minZ + 3) / (maxZ - minZ + 1F))); // / 300F) * ((v.z + 5)) * f; //-5 +
                        if (v.y > midY[v.z])
                            yMove = ((midY[v.z] - r.Next(3) - (maxY[v.z] - v.y)) * 0.6F * ((v.z - minZ + 3) / (maxZ - minZ + 1F)));
                        else if (v.y < midY[v.z])
                            yMove = ((midY[v.z] + r.Next(3) - minY[v.z] + v.y) * -0.6F * ((v.z - minZ + 3) / (maxZ - minZ + 1F)));
                        //                            yMove = ((0 + (v.y - midY[v.z] + r.Next(3))) * 0.8F * ((v.z - minZ + 3) / (maxZ - minZ + 1F))); // / 300F) * (v.z + 5); //-5 +
                        if (minZ > 0)
                            zMove = ((v.z) * (1 - f) / 6F);
                        else
                            zMove = (v.z / ((maxZ + 1) * (0.3F))) * (4 - f) * 0.8F;
                    }
                    if (xMove > 0)
                    {
                        float nv = (v.x + (xMove * f / 7)) - Math.Abs((yMove * f / 15)) + (float)(r.NextDouble() * 2.0 - 1.0);
                        if (nv < 0) nv = 0;
                        if (nv > 119) nv = 119;
                        mvd.x = (byte)(Math.Floor(nv));
                    }
                    else if (xMove < 0)
                    {
                        float nv = (v.x + (xMove * f / 7)) + Math.Abs((yMove * f / 15)) + (float)(r.NextDouble() * 2.0 - 1.0);
                        if (nv < 0) nv = 0;
                        if (nv > 119) nv = 119;
                        mvd.x = (byte)(Math.Floor(nv));
                    }
                    else
                    {
                        if (v.x < 0) mvd.x = 0;
                        else if (v.x > 119) mvd.x = 119;
                        else mvd.x = v.x;
                    }
                    if (yMove > 0)
                    {
                        float nv = (v.y + (yMove * f / 8)) - Math.Abs((xMove * f / 15)) + (float)(r.NextDouble() * 2.0 - 1.0);
                        if (nv < 0) nv = 0;
                        if (nv > 119) nv = 119;
                        mvd.y = (byte)(Math.Floor(nv));
                    }
                    else if (yMove < 0)
                    {
                        float nv = (v.y + (yMove * f / 8)) + Math.Abs((xMove * f / 15)) + (float)(r.NextDouble() * 2.0 - 1.0);
                        if (nv < 0) nv = 0;
                        if (nv > 119) nv = 119;
                        mvd.y = (byte)(Math.Floor(nv));
                    }
                    else
                    {
                        if (v.y < 0) mvd.y = 0;
                        else if (v.y > 119) mvd.y = 119;
                        else mvd.y = v.y;
                    }
                    //if (xMove > 0)
                    //{
                    //    float nv = (v.x + (xMove * f / 4F));
                    //    if (nv < 0) nv = 0;
                    //    if (nv > 119) nv = 119;
                    //    mvd.x = (byte)(Math.Ceiling(nv));
                    //}
                    //else if (xMove < 0)
                    //{
                    //    float nv = (v.x + (xMove * f / 4F));
                    //    if (nv < 0) nv = 0;
                    //    if (nv > 119) nv = 119;
                    //    mvd.x = (byte)((blowback) ? Math.Floor(nv) : (Math.Ceiling(nv)));
                    //}
                    //else
                    //{
                    //    mvd.x = v.x;
                    //}

                    //if (yMove > 0)
                    //{
                    //    float nv = (v.y + (yMove * f / 4F));
                    //    if (nv < 0) nv = 0;
                    //    if (nv > 119) nv = 119;
                    //    mvd.y = (byte)(Math.Ceiling(nv));
                    //}
                    //else if (yMove < 0)
                    //{
                    //    float nv = (v.y + (yMove * f / 4F));
                    //    if (nv < 0) nv = 0;
                    //    if (nv > 119) nv = 119;
                    //    mvd.y = (byte)(Math.Floor(nv));

                    //}
                    //else
                    //{
                    //    if (v.y < 0) mvd.y = 0;
                    //    if (v.y > 119) mvd.y = 119;
                    //    else mvd.y = v.y;
                    //}

                    if (zMove != 0)
                    {
                        float nv = (v.z + (zMove * 1.5f / f));

                        if (nv <= 0 && f < 8) nv = r.Next(2); //bounce
                        else if (nv < 0) nv = 0;

                        if (nv > 79) nv = 79;
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

                int[] minX = new int[40 * 2];
                int[] maxX = new int[40 * 2];
                float[] midX = new float[40 * 2];
                for (int level = 0; level < 40 * 2; level++)
                {
                    minX[level] = vls.Min(v => v.x * ((v.z != level || v.color == 249 - 96 || v.color == 249 - 128 ||
                        v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color < 249 - 168) ? 100 : 1));
                    maxX[level] = vls.Max(v => v.x * ((v.z != level || v.color == 249 - 96 || v.color == 249 - 128 ||
                        v.color == 249 - 136 || v.color < 249 - 152 || v.color < 249 - 160 || v.color < 249 - 168) ? 0 : 1));
                    midX[level] = (maxX[level] + minX[level]) / 2F;
                }

                int[] minY = new int[40 * 2];
                int[] maxY = new int[40 * 2];
                float[] midY = new float[40 * 2];
                for (int level = 0; level < 40 * 2; level++)
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
                        mvd.color = (byte)((r.Next(8) == 0) ? 249 - 152 : 249 - 144); //random transform to orange fire
                    else if (v.color <= 249 - 168) //clear and markers
                        mvd.color = (byte)249 - 168; //clear stays clear
                    else if (v.color == 249 - 120)
                        mvd.color = 249 - 168; //clear inner shadow
                    else if (v.color == 249 - 96)
                        mvd.color = 249 - 96; //shadow stays shadow
                    else if (v.color == 249 - 80) //lights
                        mvd.color = 249 - 16; //cannon color for broken lights
                    else if (v.color == 249 - 88) //windows
                        mvd.color = (byte)((r.Next(3) == 0) ? 249 - 168 : 249 - 88); //random transform to clear
                    else if (v.color == 249 - 104) //rotors
                        mvd.color = 249 - 56; //helmet color for broken rotors
                    else if (v.color == 249 - 112)
                        mvd.color = 249 - 168; //clear non-active rotors
                    else if (v.color == 249 - 152) //orange fire
                        mvd.color = (byte)((r.Next(8) + 2 <= f) ? 249 - 136 : ((r.Next(3) <= 1) ? 249 - 160 : ((r.Next(3) == 0) ? 249 - 136 : 249 - 152))); //random transform to yellow fire or smoke
                    else if (v.color == 249 - 160) //yellow fire
                        mvd.color = (byte)((r.Next(8) + 1 <= f) ? 249 - 136 : ((r.Next(3) <= 1) ? 249 - 152 : ((r.Next(4) == 0) ? 249 - 136 : 249 - 160))); //random transform to orange fire or smoke
                    else if (v.color == 249 - 136) //smoke
                        mvd.color = (byte)((r.Next(8) + 3 <= f) ? 249 - 168 : 249 - 136); //random transform to clear
                    else
                        mvd.color = (byte)((r.Next(f * 3) <= 5) ? 249 - (152 + ((r.Next(4) == 0) ? 8 : 0)) : v.color); //random transform to orange or yellow fire //(f >= 6) ? 249 - 136 :
                    float xMove = 0, yMove = 0, zMove = 0;
                    if (mvd.color == 249 - 152 || mvd.color == 249 - 160 || mvd.color == 249 - 136)
                    {
                        zMove = f * 0.6f;
                        xMove = (float)(r.NextDouble() * 2.0 - 1.0);
                        yMove = (float)(r.NextDouble() * 2.0 - 1.0);
                    }
                    else
                    {
                        /*
                        if (v.x > midX[v.z])// && v.x > maxX - 5)
                            xMove = ((midX[v.z] - r.Next(4) - ((blowback) ? 7 : 0) - (maxX[v.z] - v.x)) * 0.5F * ((v.z - minZ + 1) / (maxZ - minZ + 1F))); //5 -
                        else if (v.x < midX[v.z])// && v.x < minX + 5)
                            xMove = ((0 + (v.x - midX[v.z] + r.Next(4) - ((blowback) ? 6 : 0))) * 0.5F * ((v.z - minZ + 1) / (maxZ - minZ + 1F))); //-5 +
                        if (v.y > midY[v.z])// && v.y > maxY - 5)
                            yMove = ((midY[v.z] - r.Next(4) - (maxY[v.z] - v.y)) * 0.5F * ((v.z - minZ + 1) / (maxZ - minZ + 1F)));//5 -
                        else if (v.y < midY[v.z])// && v.y < minY + 5)
                            yMove = ((0 + (v.y - midY[v.z] + r.Next(4))) * 0.5F * ((v.z - minZ + 1) / (maxZ - minZ + 1F))); //-5 +
*/

                        if (v.x > midX[v.z])
                            xMove = ((midX[v.z] - r.Next(4) - ((blowback) ? 7 : 0) - (maxX[v.z] - v.x)) * 0.5F * ((v.z - minZ + 3) / (maxZ - minZ + 1F)));
                        //for lower values: distance from current voxel x to center - (between 7 to 10), times variable based on height
                        else if (v.x < midX[v.z])
                            xMove = ((midX[v.z] + r.Next(4) - ((blowback) ? 6 : 0) - minX[v.z] + v.x) * -0.5F * ((v.z - minZ + 3) / (maxZ - minZ + 1F)));// / 300F) * (v.z + 5); //5 -
                        //           60                                             40        50
                        //xMove = ((0 + (v.x - midX[v.z] + r.Next(3) - ((blowback) ? 8 : 0))) * 0.8F * ((v.z - minZ + 3) / (maxZ - minZ + 1F))); // / 300F) * ((v.z + 5)) * f; //-5 +

                        if (v.y > midY[v.z])
                            yMove = ((midY[v.z] - r.Next(4) - (maxY[v.z] - v.y)) * 0.5F * ((v.z - minZ + 3) / (maxZ - minZ + 1F)));
                        else if (v.y < midY[v.z])
                            yMove = ((midY[v.z] + r.Next(4) - minY[v.z] + v.y) * -0.5F * ((v.z - minZ + 3) / (maxZ - minZ + 1F)));

                        if (f < 5 && minZ == 0)
                            zMove = (v.z / ((maxZ + 1) * (0.2F))) * (4 - f) * 0.6F;
                        else
                            zMove = (1 - f * 1.5F);

                    }

                    if (xMove > 0)
                    {
                        float nv = (v.x + (xMove / (0.1f * (f + 5)))) - Math.Abs((yMove / (0.15f * (f + 5)))) + (float)(r.NextDouble() * 2.0 - 1.0);
                        if (nv < 0) nv = 0;
                        if (nv > 119) nv = 119;
                        mvd.x = (byte)(Math.Floor(nv));
                    }
                    else if (xMove < 0)
                    {
                        float nv = (v.x + (xMove / (0.1f * (f + 5)))) + Math.Abs((yMove / (0.15f * (f + 5)))) + (float)(r.NextDouble() * 2.0 - 1.0);
                        if (nv < 0) nv = 0;
                        if (nv > 119) nv = 119;
                        mvd.x = (byte)(Math.Floor(nv));
                    }
                    else
                    {
                        if (v.x < 0) mvd.x = 0;
                        if (v.x > 119) mvd.x = 119;
                        else mvd.x = v.x;
                    }
                    if (yMove > 0)
                    {
                        float nv = (v.y + (yMove / (0.15f * (f + 5)))) - Math.Abs((xMove / (0.2f * (f + 5)))) + (float)(r.NextDouble() * 2.0 - 1.0);
                        if (nv < 0)
                        {
                            nv = 0;
                            mvd.color = 249 - 168;
                        }
                        if (nv > 119)
                        {
                            nv = 119;
                            mvd.color = 249 - 168;
                        }
                        mvd.y = (byte)(Math.Floor(nv));
                    }
                    else if (yMove < 0)
                    {
                        float nv = (v.y + (yMove / (0.15f * (f + 5)))) + Math.Abs((xMove / (0.2f * (f + 5)))) + (float)(r.NextDouble() * 2.0 - 1.0);
                        if (nv < 0)
                        {
                            nv = 0;
                            mvd.color = 249 - 168;
                        }
                        if (nv > 119)
                        {
                            nv = 119;
                            mvd.color = 249 - 168;
                        }
                        mvd.y = (byte)(Math.Ceiling(nv));
                    }
                    else
                    {
                        if (v.y < 0) mvd.y = 0;
                        if (v.y > 119) mvd.y = 119;
                        else mvd.y = v.y;
                    }

                    //if (xMove > 0)
                    //{
                    //    float nv = (v.x + (xMove * 1.5f / f));
                    //    if (nv < 0) nv = 0;
                    //    if (nv > 119) nv = 119;
                    //    mvd.x = (byte)(Math.Floor(nv));
                    //}
                    //else if (xMove < 0)
                    //{
                    //    float nv = (v.x + (xMove * 1.5f / f));
                    //    if (nv < 0) nv = 0;
                    //    if (nv > 119) nv = 119;
                    //    mvd.x = (byte)((blowback) ? Math.Floor(nv) : (Math.Ceiling(nv)));
                    //}
                    //else
                    //{
                    //    mvd.x = v.x;
                    //}
                    //if (yMove > 0)
                    //{
                    //    float nv = (v.y + (yMove * 1.5f / f));
                    //    if (nv < 0) nv = 0;
                    //    if (nv > 119) nv = 119;
                    //    mvd.y = (byte)(Math.Floor(nv));
                    //}
                    //else if (yMove < 0)
                    //{
                    //    float nv = (v.y + (yMove * 1.5f / f));
                    //    if (nv < 0) nv = 0;
                    //    if (nv > 119) nv = 119;
                    //    mvd.y = (byte)(Math.Ceiling(nv));
                    //}
                    //else
                    //{
                    //    mvd.y = v.y;
                    //}
                    if (zMove != 0)
                    {
                        float nv = (v.z + (zMove * 1.3f));

                        if (nv <= 0 && f < 8) nv = r.Next(2); //bounce
                        else if (nv < 0) nv = 0;

                        if (nv > 79)
                        {
                            nv = 79;
                            mvd.color = 249 - 168;
                        }
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
                int[,] taken = new int[120, 120];
                taken.Fill(-1);
                for (int i = 0; i < voxelFrames[f].Length; i++)
                {
                    // do not store this voxel if it lies out of range of the voxel chunk (30x30x30)
                    if (voxelFrames[f][i].x >= 120 || voxelFrames[f][i].y >= 120 || voxelFrames[f][i].z >= 80)
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
        public static MagicaVoxelData[][] FieryExplosionDoubleW(MagicaVoxelData[] voxels, bool blowback)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[13][];
            voxelFrames[0] = new MagicaVoxelData[voxels.Length];
            voxels.CopyTo(voxelFrames[0], 0);
            for (int i = 0; i < voxels.Length; i++)
            {
                voxelFrames[0][i].x += 40;
                voxelFrames[0][i].y += 40;
            }
            for (int f = 1; f < 5; f++)
            {
                List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxels.Length);
                MagicaVoxelData[] vls = new MagicaVoxelData[voxelFrames[f - 1].Length], working = new MagicaVoxelData[voxelFrames[f - 1].Length]; //.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)
                voxelFrames[f - 1].CopyTo(vls, 0);
                voxelFrames[f - 1].CopyTo(working, 0);

                int[] minX = new int[40 * 2];
                int[] maxX = new int[40 * 2];
                float[] midX = new float[40 * 2];
                for (int level = 0; level < 40 * 2; level++)
                {
                    minX[level] = vls.Min(v => v.x * ((v.z != level || v.color == 253 - 25 * 4 || v.color == 253 - 27 * 4 ||
                        v.color == 253 - 17 * 4 || v.color == 253 - 19 * 4 || v.color == 253 - 18 * 4 || v.color <= 253 - 47 * 4) ? 2000 : 1));
                    maxX[level] = vls.Max(v => v.x * ((v.z != level || v.color == 253 - 25 * 4 || v.color == 253 - 27 * 4 ||
                        v.color == 253 - 17 * 4 || v.color == 253 - 19 * 4 || v.color == 253 - 18 * 4 || v.color <= 253 - 47 * 4) ? 0 : 1));
                    midX[level] = (maxX[level] + minX[level]) / 2F;
                }

                int[] minY = new int[40 * 2];
                int[] maxY = new int[40 * 2];
                float[] midY = new float[40 * 2];
                for (int level = 0; level < 40 * 2; level++)
                {
                    minY[level] = vls.Min(v => v.y * ((v.z != level || v.color == 253 - 25 * 4 || v.color == 253 - 27 * 4 ||
                        v.color == 253 - 17 * 4 || v.color == 253 - 19 * 4 || v.color == 253 - 18 * 4 || v.color <= 253 - 47 * 4) ? 2000 : 1));
                    maxY[level] = vls.Max(v => v.y * ((v.z != level || v.color == 253 - 25 * 4 || v.color == 253 - 27 * 4 ||
                        v.color == 253 - 17 * 4 || v.color == 253 - 19 * 4 || v.color == 253 - 18 * 4 || v.color <= 253 - 47 * 4) ? 0 : 1));
                    midY[level] = (maxY[level] + minY[level]) / 2F;
                }

                int minZ = vls.Min(v => v.z * ((v.color == 253 - 25 * 4 || v.color == 253 - 27 * 4 ||
                        v.color == 253 - 17 * 4 || v.color == 253 - 19 * 4 || v.color == 253 - 18 * 4 || v.color <= 253 - 47 * 4) ? 2000 : 1));
                int maxZ = vls.Max(v => v.z * ((v.color == 253 - 25 * 4 || v.color == 253 - 27 * 4 ||
                        v.color == 253 - 17 * 4 || v.color == 253 - 19 * 4 || v.color == 253 - 18 * 4 || v.color <= 253 - 47 * 4) ? 0 : 1));
                float midZ = (maxZ + minZ) / 2F;

                int iter = 0;
                foreach (MagicaVoxelData v in vls)
                {
                    MagicaVoxelData mvd = new MagicaVoxelData();
                    int c = (253 - v.color) / 4;
                    if (c == 8 || c == 9) //flesh
                        mvd.color = (byte)((r.Next(1 + f) == 0) ? 253 - 34 * 4 : (r.Next(8) == 0) ? 253 - 19 * 4 : v.color); //random transform to guts
                    else if (c == 34) //guts
                        mvd.color = (byte)((r.Next(18) == 0) ? 253 - 19 * 4 : v.color); //random transform to orange fire
                    else if (c >= 47) //clear and markers
                        mvd.color = 253 - 47 * 4; //clear stays clear
                    else if (c == 16)
                        mvd.color = 253 - 47 * 4; //clear inner shadow
                    else if (c == 25)
                        mvd.color = 253 - 25 * 4; //shadow stays shadow
                    else if (c >= 21 && c <= 24) //lights
                        mvd.color = 253 - 12 * 4; //metal contrast color for broken lights
                    else if (c == 35) //windows
                        mvd.color = (byte)((r.Next(3) == 0) ? 253 - 47 * 4 : v.color); //random transform to clear
                    /*
                    else if (v.color == 253 - 104) //rotors
                        mvd.color = 253 - 56; //helmet color for broken rotors
                    else if (v.color == 253 - 112)
                        mvd.color = 253 - 47*4; //clear non-active rotors
                     */
                    else if (c == 19) //orange fire
                        mvd.color = (byte)((r.Next(3) <= 1) ? 253 - 18 * 4 : ((r.Next(5) == 0) ? 253 - 17 * 4 : v.color)); //random transform to yellow fire or smoke
                    else if (c == 18) //yellow fire
                        mvd.color = (byte)((r.Next(4) <= 1) ? 253 - 19 * 4 : ((r.Next(4) == 0) ? 253 - 20 * 4 : v.color)); //random transform to orange fire or sparks
                    else if (c == 20) //sparks
                        mvd.color = (byte)((r.Next(4) == 0) ? 253 - 47 * 4 : v.color); //random transform to clear
                    else if (c == 17)
                        mvd.color = v.color; // smoke stays smoke
                    else
                        mvd.color = (byte)((r.Next(9 - f) == 0) ? 253 - (19 * 4 - ((r.Next(4) == 0) ? 4 : 0)) : v.color); //random transform to orange or yellow fire
                    float xMove = 0, yMove = 0, zMove = 0;

                    if (mvd.color == 253 - 19 * 4 || mvd.color == 253 - 18 * 4 || mvd.color == 253 - 17 * 4)
                    {
                        zMove = (f - 1) * 1.8f;
                        xMove = (float)(r.NextDouble() * 2.0 - 1.0);
                        yMove = (float)(r.NextDouble() * 2.0 - 1.0);
                    }
                    else
                    {
                        //for higher values: center - (between 9 and 11) - distance from current voxel x to edge x, times variable based on height
                        if (v.x > midX[v.z])
                            xMove = ((midX[v.z] - r.Next(3) - ((blowback) ? 9 : 0) - (maxX[v.z] - v.x)) * 0.6F * ((v.z - minZ + 1) / (maxZ - minZ + 1F)));
                        //for lower values: distance from current voxel x to center - (between 6 to 8), times variable based on height
                        else if (v.x < midX[v.z])
                            xMove = ((midX[v.z] + r.Next(3) - ((blowback) ? 8 : 0) - minX[v.z] + v.x) * -0.6F * ((v.z - minZ + 1) / (maxZ - minZ + 1F)));// / 300F) * (v.z + 5); //5 -
                        if (v.y > midY[v.z])
                            yMove = ((midY[v.z] - r.Next(3) - (maxY[v.z] - v.y)) * 0.6F * ((v.z - minZ + 3) / (maxZ - minZ + 1F)));
                        else if (v.y < midY[v.z])
                            yMove = ((midY[v.z] + r.Next(3) - minY[v.z] + v.y) * -0.6F * ((v.z - minZ + 3) / (maxZ - minZ + 1F)));
                        if (mvd.color == 253 - 20 * 4)
                        {
                            zMove = 0.1f;
                            xMove *= 2;
                            yMove *= 2;
                        }
                        else if (minZ > 0)
                            zMove = ((v.z) * (1 - f) / 6F);
                        else
                            zMove = (v.z / ((maxZ + 1 + midZ) * (0.15F))) * (4 - f) * 1.1F;
                    }

                    if (xMove > 20) xMove = 20;
                    if (xMove < -20) xMove = -20;
                    if (yMove > 20) yMove = 20;
                    if (yMove < -20) yMove = -20;

                    if (xMove > 0)
                    {
                        float nv = (v.x + (xMove * f / 7)) - Math.Abs((yMove * f / 15)) + (float)(r.NextDouble() * 2.0 - 1.0);
                        if (nv < 0) nv = 0;
                        if (nv > 119) nv = 119;
                        mvd.x = (byte)(Math.Floor(nv));
                    }
                    else if (xMove < 0)
                    {
                        float nv = (v.x + (xMove * f / 7)) + Math.Abs((yMove * f / 15)) + (float)(r.NextDouble() * 2.0 - 1.0);
                        if (nv < 0) nv = 0;
                        if (nv > 119) nv = 119;
                        mvd.x = (byte)(Math.Floor(nv));
                    }
                    else
                    {
                        if (v.x < 0) mvd.x = 0;
                        else if (v.x > 119) mvd.x = 119;
                        else mvd.x = v.x;
                    }
                    if (yMove > 0)
                    {
                        float nv = (v.y + (yMove * f / 7)) - Math.Abs((xMove * f / 15)) + (float)(r.NextDouble() * 2.0 - 1.0);
                        if (nv < 0) nv = 0;
                        if (nv > 119) nv = 119;
                        mvd.y = (byte)(Math.Floor(nv));
                    }
                    else if (yMove < 0)
                    {
                        float nv = (v.y + (yMove * f / 7)) + Math.Abs((xMove * f / 15)) + (float)(r.NextDouble() * 2.0 - 1.0);
                        if (nv < 0) nv = 0;
                        if (nv > 119) nv = 119;
                        mvd.y = (byte)(Math.Floor(nv));
                    }
                    else
                    {
                        if (v.y < 0) mvd.y = 0;
                        else if (v.y > 119) mvd.y = 119;
                        else mvd.y = v.y;
                    }

                    if (zMove != 0)
                    {
                        float nv = (v.z + (zMove * 1.5f / f));

                        if (nv <= 0 && f < 8) nv = r.Next(2); //bounce
                        else if (nv < 0) nv = 0;

                        if (nv > 79) nv = 79;
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
            for (int f = 5; f < 13; f++)
            {
                List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxels.Length);
                MagicaVoxelData[] vls = new MagicaVoxelData[voxelFrames[f - 1].Length], working = new MagicaVoxelData[voxelFrames[f - 1].Length]; //.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)
                voxelFrames[f - 1].CopyTo(vls, 0);
                voxelFrames[f - 1].CopyTo(working, 0);

                int[] minX = new int[40 * 2];
                int[] maxX = new int[40 * 2];
                float[] midX = new float[40 * 2];
                for (int level = 0; level < 40 * 2; level++)
                {
                    minX[level] = vls.Min(v => v.x * ((v.z != level || v.color == 253 - 25 * 4 || v.color == 253 - 27 * 4 ||
                        v.color == 253 - 17 * 4 || v.color == 253 - 19 * 4 || v.color == 253 - 18 * 4 || v.color <= 253 - 47 * 4) ? 100 : 1));
                    maxX[level] = vls.Max(v => v.x * ((v.z != level || v.color == 253 - 25 * 4 || v.color == 253 - 27 * 4 ||
                        v.color == 253 - 17 * 4 || v.color == 253 - 19 * 4 || v.color == 253 - 18 * 4 || v.color <= 253 - 47 * 4) ? 0 : 1));
                    midX[level] = (maxX[level] + minX[level]) / 2F;
                }

                int[] minY = new int[40 * 2];
                int[] maxY = new int[40 * 2];
                float[] midY = new float[40 * 2];
                for (int level = 0; level < 40 * 2; level++)
                {
                    minY[level] = vls.Min(v => v.y * ((v.z != level || v.color == 253 - 25 * 4 || v.color == 253 - 27 * 4 ||
                        v.color == 253 - 17 * 4 || v.color == 253 - 19 * 4 || v.color == 253 - 18 * 4 || v.color <= 253 - 47 * 4) ? 100 : 1));
                    maxY[level] = vls.Max(v => v.y * ((v.z != level || v.color == 253 - 25 * 4 || v.color == 253 - 27 * 4 ||
                        v.color == 253 - 17 * 4 || v.color == 253 - 19 * 4 || v.color == 253 - 18 * 4 || v.color <= 253 - 47 * 4) ? 0 : 1));
                    midY[level] = (maxY[level] + minY[level]) / 2F;
                }

                int minZ = vls.Min(v => v.z * ((v.color == 253 - 25 * 4 || v.color == 253 - 27 * 4 ||
                        v.color == 253 - 17 * 4 || v.color == 253 - 19 * 4 || v.color == 253 - 18 * 4 || v.color <= 253 - 47 * 4) ? 100 : 1));
                int maxZ = vls.Max(v => v.z * ((v.color == 253 - 25 * 4 || v.color == 253 - 27 * 4 ||
                        v.color == 253 - 17 * 4 || v.color == 253 - 19 * 4 || v.color == 253 - 18 * 4 || v.color <= 253 - 47 * 4) ? 0 : 1));
                float midZ = (maxZ + minZ) / 2F;

                int iter = 0;
                foreach (MagicaVoxelData v in vls)
                {
                    MagicaVoxelData mvd = new MagicaVoxelData();
                    int c = (253 - v.color) / 4;
                    if (c == 8 || c == 9) //flesh
                        mvd.color = (byte)((r.Next(f) == 0) ? 253 - 34 * 4 : (r.Next(6) == 0 && f < 10) ? 253 - 19 * 4 : v.color); //random transform to guts
                    else if (c == 34) //guts
                        mvd.color = (byte)((r.Next(20) == 0 && f < 10) ? 253 - 19 * 4 : v.color); //random transform to orange fire
                    else if (c >= 47) //clear and markers
                        mvd.color = (byte)253 - 47 * 4; //clear stays clear
                    else if (c == 16)
                        mvd.color = 253 - 47 * 4; //clear inner shadow
                    else if (c == 25)
                        mvd.color = 253 - 25 * 4; //shadow stays shadow
                    else if (c >= 21 && c <= 24) //lights
                        mvd.color = 253 - 12 * 4; //metal contrast color for broken lights
                    else if (c == 35) //windows
                        mvd.color = (byte)((r.Next(3) == 0) ? 253 - 47 * 4 : v.color); //random transform to clear
                    /*
                    else if (v.color == 253 - 104) //rotors
                        mvd.color = 253 - 56; //helmet color for broken rotors
                    else if (v.color == 253 - 112)
                        mvd.color = 253 - 47*4; //clear non-active rotors
                     */
                    else if (c == 19) //orange fire
                        mvd.color = (byte)((r.Next(9) + 2 <= f) ? 253 - 17 * 4 : ((r.Next(3) <= 1) ? 253 - 18 * 4 : ((r.Next(3) == 0) ? 253 - 17 * 4 : v.color))); //random transform to yellow fire or smoke
                    else if (c == 18) //yellow fire
                        mvd.color = (byte)((r.Next(9) + 1 <= f) ? 253 - 17 * 4 : ((r.Next(3) <= 1) ? 253 - 19 * 4 : ((r.Next(4) == 0) ? 253 - 17 * 4 : ((r.Next(4) == 0) ? 253 - 20 * 4 : v.color)))); //random transform to orange fire, smoke, or sparks
                    else if (c == 20) //sparks
                        mvd.color = (byte)((r.Next(4) > 0 && r.Next(12) > f) ? v.color : 253 - 47 * 4); //random transform to clear
                    else if (c == 17) //smoke
                        mvd.color = (byte)((r.Next(10) + 3 <= f) ? 253 - 47 * 4 : 253 - 17 * 4); //random transform to clear
                    else
                        mvd.color = (byte)((r.Next(f * 4) <= 6) ? 253 - (152 + ((r.Next(4) == 0) ? 8 : 0)) : v.color); //random transform to orange or yellow fire //(f >= 6) ? 253 - 17*4 :

                    float xMove = 0, yMove = 0, zMove = 0;
                    if (mvd.color == 253 - 19 * 4 || mvd.color == 253 - 18 * 4 || mvd.color == 253 - 17 * 4)
                    {
                        zMove = f * 0.5f;
                        xMove = (float)(r.NextDouble() * 2.0 - 1.0);
                        yMove = (float)(r.NextDouble() * 2.0 - 1.0);
                    }
                    else
                    {
                        if (v.x > midX[v.z])
                            xMove = ((midX[v.z] - r.Next(4) - ((blowback) ? 7 : 0) - (maxX[v.z] - v.x)) * 0.5F * ((v.z - minZ + 1) / (maxZ - minZ + 1F)));
                        //for lower values: distance from current voxel x to center - (between 7 to 10), times variable based on height
                        else if (v.x < midX[v.z])
                            xMove = ((midX[v.z] + r.Next(4) - ((blowback) ? 6 : 0) - minX[v.z] + v.x) * -0.5F * ((v.z - minZ + 1) / (maxZ - minZ + 1F)));// / 300F) * (v.z + 5); //5 -
                        //           60                                             40        50

                        if (v.y > midY[v.z])
                            yMove = ((midY[v.z] - r.Next(4) - (maxY[v.z] - v.y)) * 0.5F * ((v.z - minZ + 3) / (maxZ - minZ + 1F)));
                        else if (v.y < midY[v.z])
                            yMove = ((midY[v.z] + r.Next(4) - minY[v.z] + v.y) * -0.5F * ((v.z - minZ + 3) / (maxZ - minZ + 1F)));

                        if (mvd.color == 253 - 20 * 4)
                        {
                            zMove = 0.1f;
                            xMove *= 2;
                            yMove *= 2;
                        }
                        else if (f < 6 && minZ == 0)
                            zMove = (v.z / ((maxZ + 1) * (0.2F))) * (5 - f) * 0.6F;
                        else
                            zMove = (1 - f * 1.85F);
                    }
                    if (v.z <= 1 && f >= 10)
                    {
                        xMove = 0;
                        yMove = 0;
                    }
                    if (xMove > 20) xMove = 20;
                    if (xMove < -20) xMove = -20;
                    if (yMove > 20) yMove = 20;
                    if (yMove < -20) yMove = -20;

                    if (xMove > 0)
                    {
                        float nv = (v.x + (xMove / (0.1f * (f + 5)))) - Math.Abs((yMove / (0.15f * (f + 5)))) + (float)(r.NextDouble() * 2.0 - 1.0);
                        if (nv < 0) nv = 0;
                        if (nv > 119) nv = 119;
                        mvd.x = (byte)(Math.Floor(nv));
                    }
                    else if (xMove < 0)
                    {
                        float nv = (v.x + (xMove / (0.1f * (f + 5)))) + Math.Abs((yMove / (0.15f * (f + 5)))) + (float)(r.NextDouble() * 2.0 - 1.0);
                        if (nv < 0) nv = 0;
                        if (nv > 119) nv = 119;
                        mvd.x = (byte)(Math.Floor(nv));
                    }
                    else
                    {
                        if (v.x < 0) mvd.x = 0;
                        if (v.x > 119) mvd.x = 119;
                        else mvd.x = v.x;
                    }
                    if (yMove > 0)
                    {
                        float nv = (v.y + (yMove / (0.15f * (f + 5)))) - Math.Abs((xMove / (0.2f * (f + 5)))) + (float)(r.NextDouble() * 2.0 - 1.0);
                        if (nv < 0)
                        {
                            nv = 0;
                            mvd.color = 253 - 47 * 4;
                        }
                        if (nv > 119)
                        {
                            nv = 119;
                            mvd.color = 253 - 47 * 4;
                        }
                        mvd.y = (byte)(Math.Floor(nv));
                    }
                    else if (yMove < 0)
                    {
                        float nv = (v.y + (yMove / (0.15f * (f + 5)))) + Math.Abs((xMove / (0.2f * (f + 5)))) + (float)(r.NextDouble() * 2.0 - 1.0);
                        if (nv < 0)
                        {
                            nv = 0;
                            mvd.color = 253 - 47 * 4;
                        }
                        if (nv > 119)
                        {
                            nv = 119;
                            mvd.color = 253 - 47 * 4;
                        }
                        mvd.y = (byte)(Math.Ceiling(nv));
                    }
                    else
                    {
                        if (v.y < 0) mvd.y = 0;
                        if (v.y > 119) mvd.y = 119;
                        else mvd.y = v.y;
                    }

                    if (zMove != 0)
                    {
                        float nv = (v.z + (zMove * 1.3f));

                        if (nv <= 0 && f < 8) nv = r.Next(2); //bounce
                        else if (nv < 0) nv = 0;

                        if (nv > 79)
                        {
                            nv = 79;
                            mvd.color = 253 - 47 * 4;
                        }
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
            for (int f = 1; f < 13; f++)
            {

                List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxels.Length);
                int[,] taken = new int[120, 120];
                taken.Fill(-1);
                for (int i = 0; i < voxelFrames[f].Length; i++)
                {
                    // do not store this voxel if it lies out of range of the voxel chunk (30x30x30)
                    if (voxelFrames[f][i].x >= 120 || voxelFrames[f][i].y >= 120 || voxelFrames[f][i].z >= 80)
                    {
                        Console.Write("Voxel out of bounds: " + voxelFrames[f][i].x + ", " + voxelFrames[f][i].y + ", " + voxelFrames[f][i].z);
                        continue;
                    }

                    altered.Add(voxelFrames[f][i]);
                    if (-1 == taken[voxelFrames[f][i].x, voxelFrames[f][i].y]
                         && (voxelFrames[f][i].color > 253 - 21 * 4 || voxelFrames[f][i].color < 253 - 24 * 4)
                         && voxelFrames[f][i].color != 253 - 25 * 4 && voxelFrames[f][i].color != 253 - 27 * 4
                         && voxelFrames[f][i].color != 253 - 17 * 4 && voxelFrames[f][i].color != 253 - 18 * 4 && voxelFrames[f][i].color != 253 - 19 * 4
                         && voxelFrames[f][i].color > 253 - 47 * 4)
                    {
                        MagicaVoxelData vox = new MagicaVoxelData();
                        vox.x = voxelFrames[f][i].x;
                        vox.y = voxelFrames[f][i].y;
                        vox.z = (byte)(0);
                        vox.color = 253 - 25 * 4;
                        taken[vox.x, vox.y] = altered.Count();
                        altered.Add(vox);
                    }
                }
                voxelFrames[f] = altered.ToArray();
            }

            MagicaVoxelData[][] frames = new MagicaVoxelData[12][];

            for (int f = 1; f < 13; f++)
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
        public static MagicaVoxelData[][] FieryExplosionQuadW(MagicaVoxelData[] voxels, bool blowback)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[13][];
            voxelFrames[0] = new MagicaVoxelData[voxels.Length];
            voxels.CopyTo(voxelFrames[0], 0);
            for (int i = 0; i < voxels.Length; i++)
            {
                voxelFrames[0][i].x += 40;
                voxelFrames[0][i].y += 40;
            }
            for (int f = 1; f < 5; f++)
            {
                List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxels.Length);
                MagicaVoxelData[] vls = new MagicaVoxelData[voxelFrames[f - 1].Length], working = new MagicaVoxelData[voxelFrames[f - 1].Length]; //.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)
                voxelFrames[f - 1].CopyTo(vls, 0);
                voxelFrames[f - 1].CopyTo(working, 0);

                int[] minX = new int[120];
                int[] maxX = new int[120];
                float[] midX = new float[120];
                for (int level = 0; level < 120; level++)
                {
                    minX[level] = vls.Where(v => v.z == level).DefaultIfEmpty(new MagicaVoxelData { x = 255, y = 255, z = 255, color = 153 }).Min(v => v.x * ((v.color == 253 - 25 * 4 || v.color == 253 - 27 * 4 ||
                        v.color == 253 - 17 * 4 || v.color == 253 - 19 * 4 || v.color == 253 - 18 * 4 || v.color <= 253 - 47 * 4) ? 2000 : 1));
                    maxX[level] = vls.Where(v => v.z == level).DefaultIfEmpty(new MagicaVoxelData { x = 0, y = 0, z = 0, color = 153 }).Max(v => v.x * ((v.color == 253 - 25 * 4 || v.color == 253 - 27 * 4 ||
                        v.color == 253 - 17 * 4 || v.color == 253 - 19 * 4 || v.color == 253 - 18 * 4 || v.color <= 253 - 47 * 4) ? 0 : 1));
                    midX[level] = (maxX[level] + minX[level]) / 2F;
                }

                int[] minY = new int[120];
                int[] maxY = new int[120];
                float[] midY = new float[120];
                for (int level = 0; level < 120; level++)
                {
                    minY[level] = vls.Where(v => v.z == level).DefaultIfEmpty(new MagicaVoxelData { x = 255, y = 255, z = 255, color = 153 }).Min(v => v.y * ((v.color == 253 - 25 * 4 || v.color == 253 - 27 * 4 ||
                        v.color == 253 - 17 * 4 || v.color == 253 - 19 * 4 || v.color == 253 - 18 * 4 || v.color <= 253 - 47 * 4) ? 10000 : 1));
                    maxY[level] = vls.Where(v => v.z == level).DefaultIfEmpty(new MagicaVoxelData { x = 0, y = 0, z = 0, color = 153 }).Max(v => v.y * ((v.color == 253 - 25 * 4 || v.color == 253 - 27 * 4 ||
                        v.color == 253 - 17 * 4 || v.color == 253 - 19 * 4 || v.color == 253 - 18 * 4 || v.color <= 253 - 47 * 4) ? 0 : 1));
                    midY[level] = (maxY[level] + minY[level]) / 2F;
                }

                int minZ = vls.Min(v => v.z * ((v.color == 253 - 25 * 4 || v.color == 253 - 27 * 4 ||
                        v.color == 253 - 17 * 4 || v.color == 253 - 19 * 4 || v.color == 253 - 18 * 4 || v.color <= 253 - 47 * 4) ? 2000 : 1));
                int maxZ = vls.Max(v => v.z * ((v.color == 253 - 25 * 4 || v.color == 253 - 27 * 4 ||
                        v.color == 253 - 17 * 4 || v.color == 253 - 19 * 4 || v.color == 253 - 18 * 4 || v.color <= 253 - 47 * 4) ? 0 : 1));
                float midZ = (maxZ + minZ) / 2F;

                int iter = 0;
                foreach (MagicaVoxelData v in vls)
                {
                    MagicaVoxelData mvd = new MagicaVoxelData();
                    int c = (253 - v.color) / 4;
                    if (c == 8 || c == 9) //flesh
                        mvd.color = (byte)((r.Next(1 + f) == 0) ? 253 - 34 * 4 : (r.Next(8) == 0) ? 253 - 19 * 4 : v.color); //random transform to guts
                    else if (c == 34) //guts
                        mvd.color = (byte)((r.Next(18) == 0) ? 253 - 19 * 4 : v.color); //random transform to orange fire
                    else if (c >= 47) //clear and markers
                        mvd.color = 253 - 47 * 4; //clear stays clear
                    else if (c == 16)
                        mvd.color = 253 - 47 * 4; //clear inner shadow
                    else if (c == 25)
                        mvd.color = 253 - 25 * 4; //shadow stays shadow
                    else if (c >= 21 && c <= 24) //lights
                        mvd.color = 253 - 12 * 4; //metal contrast color for broken lights
                    else if (c == 35) //windows
                        mvd.color = (byte)((r.Next(3) == 0) ? 253 - 47 * 4 : v.color); //random transform to clear
                    /*
                    else if (v.color == 253 - 104) //rotors
                        mvd.color = 253 - 56; //helmet color for broken rotors
                    else if (v.color == 253 - 112)
                        mvd.color = 253 - 47*4; //clear non-active rotors
                     */
                    else if (c == 19) //orange fire
                        mvd.color = (byte)((r.Next(3) <= 1) ? 253 - 18 * 4 : ((r.Next(5) == 0) ? 253 - 17 * 4 : v.color)); //random transform to yellow fire or smoke
                    else if (c == 18) //yellow fire
                        mvd.color = (byte)((r.Next(4) <= 1) ? 253 - 19 * 4 : ((r.Next(4) == 0) ? 253 - 20 * 4 : v.color)); //random transform to orange fire or sparks
                    else if (c == 20) //sparks
                        mvd.color = (byte)((r.Next(4) == 0) ? 253 - 47 * 4 : v.color); //random transform to clear
                    else if (c == 17)
                        mvd.color = v.color; // smoke stays smoke
                    else
                        mvd.color = (byte)((r.Next(9 - f) == 0) ? 253 - (19 * 4 - ((r.Next(4) == 0) ? 4 : 0)) : v.color); //random transform to orange or yellow fire
                    float xMove = 0, yMove = 0, zMove = 0;

                    if (mvd.color == 253 - 19 * 4 || mvd.color == 253 - 18 * 4 || mvd.color == 253 - 17 * 4)
                    {
                        zMove = (f - 1) * 1.8f;
                        xMove = (float)(r.NextDouble() * 2.0 - 1.0);
                        yMove = (float)(r.NextDouble() * 2.0 - 1.0);
                    }
                    else
                    {
                        //for higher values: center - (between 9 and 11) - distance from current voxel x to edge x, times variable based on height
                        if (v.x > midX[v.z])
                            xMove = ((midX[v.z] - r.Next(3) - ((blowback) ? 9 : 0) - (maxX[v.z] - v.x)) * 0.6F * ((v.z - minZ + 1) / (maxZ - minZ + 1F)));
                        //for lower values: distance from current voxel x to center - (between 6 to 8), times variable based on height
                        else if (v.x < midX[v.z])
                            xMove = ((midX[v.z] + r.Next(3) - ((blowback) ? 8 : 0) - minX[v.z] + v.x) * -0.6F * ((v.z - minZ + 1) / (maxZ - minZ + 1F)));// / 300F) * (v.z + 5); //5 -
                        if (v.y > midY[v.z])
                            yMove = ((midY[v.z] - r.Next(3) - (maxY[v.z] - v.y)) * 0.6F * ((v.z - minZ + 3) / (maxZ - minZ + 1F)));
                        else if (v.y < midY[v.z])
                            yMove = ((midY[v.z] + r.Next(3) - minY[v.z] + v.y) * -0.6F * ((v.z - minZ + 3) / (maxZ - minZ + 1F)));
                        if (mvd.color == 253 - 20 * 4)
                        {
                            zMove = 0.1f;
                            xMove *= 2;
                            yMove *= 2;
                        }
                        else if (minZ > 0)
                            zMove = ((v.z) * (1 - f) / 6F);
                        else
                            zMove = (v.z / ((maxZ + 1 + midZ) * (0.15F))) * (4 - f) * 1.1F;
                    }

                    if (xMove > 20) xMove = 20;
                    if (xMove < -20) xMove = -20;
                    if (yMove > 20) yMove = 20;
                    if (yMove < -20) yMove = -20;

                    if (xMove > 0)
                    {
                        float nv = (v.x + (xMove * f / 7)) - Math.Abs((yMove * f / 15)) + (float)(r.NextDouble() * 2.0 - 1.0);
                        if (nv < 0) nv = 0;
                        if (nv > 159)
                        {
                            nv = 159;
                        }
                        mvd.x = (byte)(Math.Floor(nv));
                    }
                    else if (xMove < 0)
                    {
                        float nv = (v.x + (xMove * f / 7)) + Math.Abs((yMove * f / 15)) + (float)(r.NextDouble() * 2.0 - 1.0);
                        if (nv < 0) nv = 0;
                        if (nv > 159)
                        {
                            nv = 159;
                        }
                        mvd.x = (byte)(Math.Floor(nv));
                    }
                    else
                    {
                        if (v.x < 0) mvd.x = 0;
                        else if (v.x > 159)
                        {
                            mvd.x = 159;
                        }
                        else mvd.x = v.x;
                    }
                    if (yMove > 0)
                    {
                        float nv = (v.y + (yMove * f / 8)) - Math.Abs((xMove * f / 15)) + (float)(r.NextDouble() * 2.0 - 1.0);
                        if (nv < 0) nv = 0;
                        if (nv > 159)
                        {
                            nv = 159;
                        }
                        mvd.y = (byte)(Math.Floor(nv));
                    }
                    else if (yMove < 0)
                    {
                        float nv = (v.y + (yMove * f / 8)) + Math.Abs((xMove * f / 15)) + (float)(r.NextDouble() * 2.0 - 1.0);
                        if (nv < 0) nv = 0;
                        if (nv > 159)
                        {
                            nv = 159;
                        }
                        mvd.y = (byte)(Math.Floor(nv));
                    }
                    else
                    {
                        if (v.y < 0) mvd.y = 0;
                        else if (v.y > 159)
                        {
                            mvd.y = 159;
                        }
                        else mvd.y = v.y;
                    }

                    if (zMove != 0)
                    {
                        float nv = (v.z + (zMove * 1.5f / f));

                        if (nv <= 0 && f < 8) nv = r.Next(2); //bounce
                        else if (nv < 0) nv = 0;

                        if (nv > 119) nv = 119;
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
            for (int f = 5; f < 13; f++)
            {
                List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxels.Length);
                MagicaVoxelData[] vls = new MagicaVoxelData[voxelFrames[f - 1].Length], working = new MagicaVoxelData[voxelFrames[f - 1].Length]; //.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)
                voxelFrames[f - 1].CopyTo(vls, 0);
                voxelFrames[f - 1].CopyTo(working, 0);

                int[] minX = new int[120];
                int[] maxX = new int[120];
                float[] midX = new float[120];
                for (int level = 0; level < 120; level++)
                {
                    minX[level] = vls.Min(v => v.x * ((v.z != level || v.color == 253 - 25 * 4 || v.color == 253 - 27 * 4 ||
                        v.color == 253 - 17 * 4 || v.color == 253 - 19 * 4 || v.color == 253 - 18 * 4 || v.color <= 253 - 47 * 4) ? 100 : 1));
                    maxX[level] = vls.Max(v => v.x * ((v.z != level || v.color == 253 - 25 * 4 || v.color == 253 - 27 * 4 ||
                        v.color == 253 - 17 * 4 || v.color == 253 - 19 * 4 || v.color == 253 - 18 * 4 || v.color <= 253 - 47 * 4) ? 0 : 1));
                    midX[level] = (maxX[level] + minX[level]) / 2F;
                }

                int[] minY = new int[120];
                int[] maxY = new int[120];
                float[] midY = new float[120];
                for (int level = 0; level < 120; level++)
                {
                    minY[level] = vls.Min(v => v.y * ((v.z != level || v.color == 253 - 25 * 4 || v.color == 253 - 27 * 4 ||
                        v.color == 253 - 17 * 4 || v.color == 253 - 19 * 4 || v.color == 253 - 18 * 4 || v.color <= 253 - 47 * 4) ? 100 : 1));
                    maxY[level] = vls.Max(v => v.y * ((v.z != level || v.color == 253 - 25 * 4 || v.color == 253 - 27 * 4 ||
                        v.color == 253 - 17 * 4 || v.color == 253 - 19 * 4 || v.color == 253 - 18 * 4 || v.color <= 253 - 47 * 4) ? 0 : 1));
                    midY[level] = (maxY[level] + minY[level]) / 2F;
                }
                int minZ = vls.Min(v => v.z * ((v.color == 253 - 25 * 4 || v.color == 253 - 27 * 4 ||
                    v.color == 253 - 17 * 4 || v.color == 253 - 19 * 4 || v.color == 253 - 18 * 4 || v.color <= 253 - 47 * 4) ? 100 : 1));
                int maxZ = vls.Max(v => v.z * ((v.color == 253 - 25 * 4 || v.color == 253 - 27 * 4 ||
                    v.color == 253 - 17 * 4 || v.color == 253 - 19 * 4 || v.color == 253 - 18 * 4 || v.color <= 253 - 47 * 4) ? 0 : 1));
                float midZ = (maxZ + minZ) / 2F;

                int iter = 0;
                foreach (MagicaVoxelData v in vls)
                {
                    MagicaVoxelData mvd = new MagicaVoxelData();
                    int c = (253 - v.color) / 4;
                    if (c == 8 || c == 9) //flesh
                        mvd.color = (byte)((r.Next(f) == 0) ? 253 - 34 * 4 : (r.Next(6) == 0 && f < 10) ? 253 - 19 * 4 : v.color); //random transform to guts
                    else if (c == 34) //guts
                        mvd.color = (byte)((r.Next(20) == 0 && f < 10) ? 253 - 19 * 4 : v.color); //random transform to orange fire
                    else if (c >= 47) //clear and markers
                        mvd.color = (byte)253 - 47 * 4; //clear stays clear
                    else if (c == 16)
                        mvd.color = 253 - 47 * 4; //clear inner shadow
                    else if (c == 25)
                        mvd.color = 253 - 25 * 4; //shadow stays shadow
                    else if (c >= 21 && c <= 24) //lights
                        mvd.color = 253 - 12 * 4; //metal contrast color for broken lights
                    else if (c == 35) //windows
                        mvd.color = (byte)((r.Next(3) == 0) ? 253 - 47 * 4 : v.color); //random transform to clear
                    /*
                    else if (v.color == 253 - 104) //rotors
                        mvd.color = 253 - 56; //helmet color for broken rotors
                    else if (v.color == 253 - 112)
                        mvd.color = 253 - 47*4; //clear non-active rotors
                     */
                    else if (c == 19) //orange fire
                        mvd.color = (byte)((r.Next(9) + 2 <= f) ? 253 - 17 * 4 : ((r.Next(3) <= 1) ? 253 - 18 * 4 : ((r.Next(3) == 0) ? 253 - 17 * 4 : v.color))); //random transform to yellow fire or smoke
                    else if (c == 18) //yellow fire
                        mvd.color = (byte)((r.Next(9) + 1 <= f) ? 253 - 17 * 4 : ((r.Next(3) <= 1) ? 253 - 19 * 4 : ((r.Next(4) == 0) ? 253 - 17 * 4 : ((r.Next(4) == 0) ? 253 - 20 * 4 : v.color)))); //random transform to orange fire, smoke, or sparks
                    else if (c == 20) //sparks
                        mvd.color = (byte)((r.Next(4) > 0 && r.Next(12) > f) ? v.color : 253 - 47 * 4); //random transform to clear
                    else if (c == 17) //smoke
                        mvd.color = (byte)((r.Next(10) + 3 <= f) ? 253 - 47 * 4 : 253 - 17 * 4); //random transform to clear
                    else
                        mvd.color = (byte)((r.Next(f * 4) <= 6) ? 253 - (152 + ((r.Next(4) == 0) ? 8 : 0)) : v.color); //random transform to orange or yellow fire //(f >= 6) ? 253 - 17*4 :

                    float xMove = 0, yMove = 0, zMove = 0;
                    if (mvd.color == 253 - 19 * 4 || mvd.color == 253 - 18 * 4 || mvd.color == 253 - 17 * 4)
                    {
                        zMove = f * 0.5f;
                        xMove = (float)(r.NextDouble() * 2.0 - 1.0);
                        yMove = (float)(r.NextDouble() * 2.0 - 1.0);
                    }
                    else
                    {
                        if (v.x > midX[v.z])
                            xMove = ((midX[v.z] - r.Next(4) - ((blowback) ? 7 : 0) - (maxX[v.z] - v.x)) * 0.5F * ((v.z - minZ + 1) / (maxZ - minZ + 1F)));
                        //for lower values: distance from current voxel x to center - (between 7 to 10), times variable based on height
                        else if (v.x < midX[v.z])
                            xMove = ((midX[v.z] + r.Next(4) - ((blowback) ? 6 : 0) - minX[v.z] + v.x) * -0.5F * ((v.z - minZ + 1) / (maxZ - minZ + 1F)));// / 300F) * (v.z + 5); //5 -
                        //           60                                             40        50

                        if (v.y > midY[v.z])
                            yMove = ((midY[v.z] - r.Next(4) - (maxY[v.z] - v.y)) * 0.5F * ((v.z - minZ + 3) / (maxZ - minZ + 1F)));
                        else if (v.y < midY[v.z])
                            yMove = ((midY[v.z] + r.Next(4) - minY[v.z] + v.y) * -0.5F * ((v.z - minZ + 3) / (maxZ - minZ + 1F)));

                        if (mvd.color == 253 - 20 * 4)
                        {
                            zMove = 0.1f;
                            xMove *= 2;
                            yMove *= 2;
                        }
                        else if (f < 6 && minZ == 0)
                            zMove = (v.z / ((maxZ + 1) * (0.2F))) * (5 - f) * 0.6F;
                        else
                            zMove = (1 - f * 1.85F);
                    }
                    if (v.z <= 1 && f >= 10)
                    {
                        xMove = 0;
                        yMove = 0;
                    }
                    if (xMove > 20) xMove = 20;
                    if (xMove < -20) xMove = -20;
                    if (yMove > 20) yMove = 20;
                    if (yMove < -20) yMove = -20;

                    if (xMove > 0)
                    {
                        float nv = (v.x + (xMove / (0.1f * (f + 5)))) - Math.Abs((yMove / (0.15f * (f + 5)))) + (float)(r.NextDouble() * 2.0 - 1.0);
                        if (nv < 0) nv = 0;
                        if (nv > 159) nv = 159;
                        mvd.x = (byte)(Math.Floor(nv));
                    }
                    else if (xMove < 0)
                    {
                        float nv = (v.x + (xMove / (0.1f * (f + 5)))) + Math.Abs((yMove / (0.15f * (f + 5)))) + (float)(r.NextDouble() * 2.0 - 1.0);
                        if (nv < 0) nv = 0;
                        if (nv > 159) nv = 159;
                        mvd.x = (byte)(Math.Floor(nv));
                    }
                    else
                    {
                        if (v.x < 0) mvd.x = 0;
                        if (v.x > 159) mvd.x = 159;
                        else mvd.x = v.x;
                    }
                    if (yMove > 0)
                    {
                        float nv = (v.y + (yMove / (0.15f * (f + 5)))) - Math.Abs((xMove / (0.2f * (f + 5)))) + (float)(r.NextDouble() * 2.0 - 1.0);
                        if (nv < 0)
                        {
                            nv = 0;
                            mvd.color = 253 - 47 * 4;
                        }
                        if (nv > 159)
                        {
                            nv = 159;
                            mvd.color = 253 - 47 * 4;
                        }
                        mvd.y = (byte)(Math.Floor(nv));
                    }
                    else if (yMove < 0)
                    {
                        float nv = (v.y + (yMove / (0.15f * (f + 5)))) + Math.Abs((xMove / (0.2f * (f + 5)))) + (float)(r.NextDouble() * 2.0 - 1.0);
                        if (nv < 0)
                        {
                            nv = 0;
                            mvd.color = 253 - 47 * 4;
                        }
                        if (nv > 159)
                        {
                            nv = 159;
                            mvd.color = 253 - 47 * 4;
                        }
                        mvd.y = (byte)(Math.Ceiling(nv));
                    }
                    else
                    {
                        if (v.y < 0) mvd.y = 0;
                        if (v.y > 159) mvd.y = 159;
                        else mvd.y = v.y;
                    }

                    if (zMove != 0)
                    {
                        float nv = (v.z + (zMove * 1.3f));

                        if (nv <= 0 && f < 10) nv = r.Next(2); //bounce
                        else if (nv < 0) nv = 0;

                        if (nv > 119)
                        {
                            nv = 119;
                            mvd.color = 253 - 47 * 4;
                        }
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
            for (int f = 1; f < 13; f++)
            {

                List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxels.Length);
                int[,] taken = new int[160, 160];
                taken.Fill(-1);
                for (int i = 0; i < voxelFrames[f].Length; i++)
                {
                    // do not store this voxel if it lies out of range of the voxel chunk (30x30x30)
                    if (voxelFrames[f][i].x >= 160 || voxelFrames[f][i].y >= 160 || voxelFrames[f][i].z >= 120)
                    {
                        Console.Write("Voxel out of bounds: " + voxelFrames[f][i].x + ", " + voxelFrames[f][i].y + ", " + voxelFrames[f][i].z);
                        continue;
                    }

                    altered.Add(voxelFrames[f][i]);
                    if (-1 == taken[voxelFrames[f][i].x, voxelFrames[f][i].y]
                         && (voxelFrames[f][i].color > 253 - 21 * 4 || voxelFrames[f][i].color < 253 - 24 * 4)
                         && voxelFrames[f][i].color != 253 - 25 * 4 && voxelFrames[f][i].color != 253 - 27 * 4
                         && voxelFrames[f][i].color != 253 - 17 * 4 && voxelFrames[f][i].color != 253 - 18 * 4 && voxelFrames[f][i].color != 253 - 19 * 4
                         && voxelFrames[f][i].color > 253 - 47 * 4)
                    {
                        MagicaVoxelData vox = new MagicaVoxelData();
                        vox.x = voxelFrames[f][i].x;
                        vox.y = voxelFrames[f][i].y;
                        vox.z = (byte)(0);
                        vox.color = 253 - 25 * 4;
                        taken[vox.x, vox.y] = altered.Count();
                        altered.Add(vox);
                    }
                }
                voxelFrames[f] = altered.ToArray();
            }

            MagicaVoxelData[][] frames = new MagicaVoxelData[12][];

            for (int f = 1; f < 13; f++)
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
        private static List<MagicaVoxelData> generateFatVoxel(MagicaVoxelData start, int color)
        {
            List<MagicaVoxelData> vox = new List<MagicaVoxelData>(8);
            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    for (int z = 0; z < 2; z++)
                    {
                        vox.Add(new MagicaVoxelData { x = (byte)(start.x + x), y = (byte)(start.y + y), z = (byte)(start.z + z), color = (byte)color });
                    }
                }
            }
            return vox;
        }
        private static List<MagicaVoxelData> generateCube(MagicaVoxelData start, int size, int color)
        {
            List<MagicaVoxelData> vox = new List<MagicaVoxelData>(8);
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    for (int z = 0; z < size; z++)
                    {
                        vox.Add(new MagicaVoxelData { x = (byte)(start.x + x), y = (byte)(start.y + y), z = (byte)(start.z + z), color = (byte)color });
                    }
                }
            }
            return vox;
        }
        private static List<MagicaVoxelData> generateBox(MagicaVoxelData start, int xsize, int ysize, int zsize, int color)
        {
            List<MagicaVoxelData> vox = new List<MagicaVoxelData>(8);
            for (int x = 0; x < xsize; x++)
            {
                for (int y = 0; y < ysize; y++)
                {
                    for (int z = 0; z < zsize; z++)
                    {
                        vox.Add(new MagicaVoxelData { x = (byte)(start.x + x), y = (byte)(start.y + y), z = (byte)(start.z + z), color = (byte)color });
                    }
                }
            }
            return vox;
        }
        private static List<MagicaVoxelData> generateStraightLine(MagicaVoxelData start, MagicaVoxelData end, int color)
        {
            List<MagicaVoxelData> vox = new List<MagicaVoxelData>(16);
            MagicaVoxelData newStart = new MagicaVoxelData { x = (start.x < end.x) ? start.x : end.x, y = (start.y < end.y) ? start.y : end.y, z = (start.z < end.z) ? start.z : end.z, color = (byte)color };
            MagicaVoxelData newEnd = new MagicaVoxelData { x = (start.x > end.x) ? start.x : end.x, y = (start.y > end.y) ? start.y : end.y, z = (start.z > end.z) ? start.z : end.z, color = (byte)color };
            Bresenham3D line = new Bresenham3D(newStart, newEnd);
            foreach (MagicaVoxelData l in line)
            {
                vox.Add(l);
            }
            /*            for (int x = newStart.x; x <= newEnd.x; x++)
                        {
                            for (int y = newStart.y; y <= newEnd.y; y++)
                            {
                                for (int z = newStart.z; z <= newEnd.z; z++)
                                {
                                    vox.Add(new MagicaVoxelData { x = (byte)(x), y = (byte)(y), z = (byte)(z), color = (byte)color });
                                }
                            }
                        }*/
            return vox;
        }
        private static List<MagicaVoxelData> generateFatStraightLine(MagicaVoxelData start, MagicaVoxelData end, int color)
        {
            List<MagicaVoxelData> vox = new List<MagicaVoxelData>(128);
            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    for (int z = 0; z < 2; z++)
                    {
                        vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + x), y = (byte)(start.y + y), z = (byte)(start.z + z), color = (byte)color },
                            new MagicaVoxelData { x = (byte)(end.x + x), y = (byte)(end.y + y), z = (byte)(end.z + z), color = (byte)color }, color));
                    }
                }
            }
            return vox;
        }

        public static MagicaVoxelData[][] SmokePlumeDouble(MagicaVoxelData start, int height, int effectDuration)
        {
            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[16];
            voxelFrames[0] = new List<MagicaVoxelData>(height * 8);
            //for (int i = 0; i < voxels.Length; i++)
            //{
            //    voxelFrames[0][i].x += 20;
            //    voxelFrames[0][i].y += 20;
            //}
            voxelFrames[0].Add(new MagicaVoxelData { x = start.x, y = start.y, z = 0, color = 249 - 160 });

            for (int i = 1; i < 16; i++)
            {
                voxelFrames[i] = new List<MagicaVoxelData>(height * 8);
            }
            for (int f = 1; f < 16 && f < effectDuration && f < height; f++)
            {
                for (int i = 0; i <= f; i++)
                {
                    voxelFrames[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(start.x - i / 2), y = start.y, z = (byte)i, color = 249 - 160 }, ((f < 2 && i == 0) ? 249 - 160 : start.color)));
                }
            }
            for (int f = Math.Min(height, effectDuration); f < 16 && f <= effectDuration && f < height * 2; f += 2)
            {
                voxelFrames[f].AddRange(generateFatStraightLine(new MagicaVoxelData { x = (byte)(start.x - (f - height) + r.Next(5) - 2), y = (byte)(start.y + r.Next(5) - 2), z = (byte)(f - height), color = start.color },
                    new MagicaVoxelData { x = (byte)(start.x - (height - 1) + r.Next(5) - 2), y = (byte)(start.y + r.Next(5) - 2), z = (byte)(height - 1), color = start.color },
                    start.color));
            }

            MagicaVoxelData[][] frames = new MagicaVoxelData[16][];

            for (int f = 0; f < 16; f++)
            {
                frames[f] = new MagicaVoxelData[voxelFrames[f].Count];
                voxelFrames[f].ToArray().CopyTo(frames[f], 0);
            }
            return frames;
        }
        public static MagicaVoxelData[][] BurstDouble(MagicaVoxelData start, int maxFrames, bool bigger)
        {
            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[maxFrames];
            voxelFrames[0] = new List<MagicaVoxelData>(10);
            //for (int i = 0; i < voxels.Length; i++)
            //{
            //    voxelFrames[0][i].x += 20;
            //    voxelFrames[0][i].y += 20;
            //}
            voxelFrames[0].Add(new MagicaVoxelData { x = start.x, y = start.y, z = start.z, color = 249 - 160 });

            for (int i = 1; i < maxFrames; i++)
            {
                voxelFrames[i] = new List<MagicaVoxelData>(80);
            }

            int rz = (r.Next(5) - 2);
            for (int i = 1; i < maxFrames; i++)
            {
                if (i <= 1)
                    voxelFrames[i].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(start.x), y = (byte)(start.y), z = start.z, color = (byte)(249 - 152) }, 249 - 152));
                if (i > 1)
                {
                    voxelFrames[i].AddRange(generateFatStraightLine(new MagicaVoxelData { x = (byte)(start.x + 2), y = (byte)(start.y), z = (byte)(start.z - 2 + rz * (i - 1)), color = (byte)(249 - 152) },
                        new MagicaVoxelData { x = (byte)(start.x + 2), y = (byte)(start.y), z = (byte)(start.z + 2 + rz * (i - 1)), color = (byte)(249 - 152) }, 249 - 152));
                    voxelFrames[i].AddRange(generateFatStraightLine(new MagicaVoxelData { x = (byte)(start.x + 2), y = (byte)(start.y - 2), z = (byte)(start.z + rz * (i - 1)), color = (byte)(249 - 152) },
                                            new MagicaVoxelData { x = (byte)(start.x + 2), y = (byte)(start.y + 2), z = (byte)(start.z + rz * (i - 1)), color = (byte)(249 - 152) }, 249 - 152));
                    if (bigger)
                    {
                        voxelFrames[i].AddRange(generateFatStraightLine(new MagicaVoxelData { x = (byte)(start.x + 4), y = (byte)(start.y), z = (byte)(start.z - 2 + rz * (i - 1)), color = (byte)(249 - 152) },
                            new MagicaVoxelData { x = (byte)(start.x + 4), y = (byte)(start.y), z = (byte)(start.z + 2 + rz * (i - 1)), color = (byte)(249 - 152) }, 249 - 152));
                        voxelFrames[i].AddRange(generateFatStraightLine(new MagicaVoxelData { x = (byte)(start.x + 4), y = (byte)(start.y - 2), z = (byte)(start.z + rz * (i - 1)), color = (byte)(249 - 152) },
                            new MagicaVoxelData { x = (byte)(start.x + 4), y = (byte)(start.y + 2), z = (byte)(start.z + rz * (i - 1)), color = (byte)(249 - 152) }, 249 - 152));
                        if (i > 2)
                        {
                            voxelFrames[i].AddRange(generateFatStraightLine(new MagicaVoxelData { x = (byte)(start.x + 6), y = (byte)(start.y), z = (byte)(start.z - 4 + rz * (i - 1)), color = (byte)(249 - 152) },
                            new MagicaVoxelData { x = (byte)(start.x + 6), y = (byte)(start.y), z = (byte)(start.z + 4 + rz * (i - 1)), color = (byte)(249 - 152) }, 249 - 152));
                            voxelFrames[i].AddRange(generateFatStraightLine(new MagicaVoxelData { x = (byte)(start.x + 6), y = (byte)(start.y - 4), z = (byte)(start.z + rz * (i - 1)), color = (byte)(249 - 152) },
                                new MagicaVoxelData { x = (byte)(start.x + 6), y = (byte)(start.y + 4), z = (byte)(start.z + rz * (i - 1)), color = (byte)(249 - 152) }, 249 - 152));

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
        public static MagicaVoxelData[][] SparksDouble(MagicaVoxelData start, int sweepDuration)
        {
            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[16];
            voxelFrames[0] = new List<MagicaVoxelData>(80);
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
                voxelFrames[i] = new List<MagicaVoxelData>(80);
            }
            bool sweepingPositive = true;
            int iter = 0;
            while (iter < 16)
            {
                for (int i = 0; i < sweepDuration && i < 16 && iter < 16; i++, iter++)
                {
                    int rx = (r.Next(3) - r.Next(2));
                    voxelFrames[iter].Add(new MagicaVoxelData { x = (byte)(start.x - rx), y = (byte)(start.y + ((sweepingPositive) ? i : sweepDuration - i) * 4), z = (byte)(0), color = (byte)(249 - 160) });
                    if (r.Next(2) == 0)
                    {
                        voxelFrames[iter].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(start.x - rx), y = (byte)(start.y + ((sweepingPositive) ? i : sweepDuration - i) * 4), z = 1, color = (byte)(249 - 160) }, 249 - 160));
                        voxelFrames[iter].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(start.x - rx - 2), y = (byte)(start.y + ((sweepingPositive) ? i : sweepDuration - i) * 4), z = 1, color = (byte)(249 - 160) }, 249 - 160));
                        voxelFrames[iter].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(start.x - rx - 2), y = (byte)(start.y + ((sweepingPositive) ? i : sweepDuration - i) * 4), z = 3, color = (byte)(249 - 160) }, 249 - 160));
                        if (r.Next(2) == 0)
                        {
                            voxelFrames[iter].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(start.x - rx - 4), y = (byte)(start.y + ((sweepingPositive) ? i : sweepDuration - i) * 4), z = 1, color = (byte)(249 - 160) }, 249 - 160));
                            voxelFrames[iter].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(start.x - rx - 4), y = (byte)(start.y + ((sweepingPositive) ? i : sweepDuration - i) * 4), z = 3, color = (byte)(249 - 160) }, 249 - 160));
                            voxelFrames[iter].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(start.x - rx - 4), y = (byte)(start.y + ((sweepingPositive) ? i : sweepDuration - i) * 4), z = 5, color = (byte)(249 - 160) }, 249 - 160));
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

        private static List<MagicaVoxelData> generateMissileDouble(MagicaVoxelData start, int length)
        {
            List<MagicaVoxelData> vox = new List<MagicaVoxelData>(128);
            for (int x = 0; x < 3; x++)
            {
                for (int z = 0; z < 3; z++)
                {
                    vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + x), y = (byte)(start.y), z = (byte)(start.z + z), color = (byte)(249 - 40) },
                        new MagicaVoxelData { x = (byte)(start.x + x), y = (byte)(start.y + 3), z = (byte)(start.z + z), color = (byte)(249 - 40) }, 249 - 40));
                }
            }

            vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + 3), y = (byte)(start.y + 1), z = (byte)(start.z), color = (byte)(249 - 40) },
                new MagicaVoxelData { x = (byte)(start.x + 3), y = (byte)(start.y + 1), z = (byte)(start.z + 3), color = (byte)(249 - 40) }, 249 - 40));
            vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + 3), y = (byte)(start.y + 2), z = (byte)(start.z), color = (byte)(249 - 40) },
                new MagicaVoxelData { x = (byte)(start.x + 3), y = (byte)(start.y + 2), z = (byte)(start.z + 3), color = (byte)(249 - 40) }, 249 - 40));
            vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + 2), y = (byte)(start.y + 1), z = (byte)(start.z + 3), color = (byte)(249 - 40) },
                new MagicaVoxelData { x = (byte)(start.x + 0), y = (byte)(start.y + 1), z = (byte)(start.z + 3), color = (byte)(249 - 40) }, 249 - 40));
            vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + 2), y = (byte)(start.y + 2), z = (byte)(start.z + 3), color = (byte)(249 - 40) },
                new MagicaVoxelData { x = (byte)(start.x + 0), y = (byte)(start.y + 2), z = (byte)(start.z + 3), color = (byte)(249 - 40) }, 249 - 40));

            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (length > 0 || x - 1 > length)
                        vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + x), y = (byte)(start.y + y), z = (byte)(start.z - 1), color = (byte)(249 - 72) },
                        new MagicaVoxelData { x = (byte)(start.x + x - 1 - length), y = (byte)(start.y + y), z = (byte)(start.z - 2 - length), color = (byte)(249 - 72) }, 249 - 72));
                }
            }

            for (int z = 0; z < 2; z++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (length > 0 || z - 1 > length)
                        vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - 1), y = (byte)(start.y + y), z = (byte)(start.z + z), color = (byte)(249 - 72) },
                        new MagicaVoxelData { x = (byte)(start.x - 2 - length), y = (byte)(start.y + y), z = (byte)(start.z + z - 1 - length), color = (byte)(249 - 72) }, 249 - 72));

                }
            }

            if (length > 0)
            {
                for (int y = 0; y < 4; y++)
                {
                    vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - 1), y = (byte)(start.y + y), z = (byte)(start.z - 1), color = (byte)(249 - 72) },
                    new MagicaVoxelData { x = (byte)(start.x - 2 - length), y = (byte)(start.y + y), z = (byte)(start.z - 2 - length), color = (byte)(249 - 72) }, 249 - 72));
                }
            }
            return vox;
        }
        private static List<MagicaVoxelData> generateMissileFieryTrailDouble(MagicaVoxelData start, int length)
        {
            List<MagicaVoxelData> vox = new List<MagicaVoxelData>(128);
            for (int x = 0; x < 3; x++)
            {
                for (int z = 0; z < 3; z++)
                {
                    vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + x), y = (byte)(start.y), z = (byte)(start.z + z), color = (byte)(249 - 40) },
                        new MagicaVoxelData { x = (byte)(start.x + x), y = (byte)(start.y + 3), z = (byte)(start.z + z), color = (byte)(249 - 40) }, 249 - 40));
                }
            }

            vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + 3), y = (byte)(start.y + 1), z = (byte)(start.z), color = (byte)(249 - 40) },
                new MagicaVoxelData { x = (byte)(start.x + 3), y = (byte)(start.y + 1), z = (byte)(start.z + 3), color = (byte)(249 - 40) }, 249 - 40));
            vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + 3), y = (byte)(start.y + 2), z = (byte)(start.z), color = (byte)(249 - 40) },
                new MagicaVoxelData { x = (byte)(start.x + 3), y = (byte)(start.y + 2), z = (byte)(start.z + 3), color = (byte)(249 - 40) }, 249 - 40));
            vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + 2), y = (byte)(start.y + 1), z = (byte)(start.z + 3), color = (byte)(249 - 40) },
                new MagicaVoxelData { x = (byte)(start.x + 0), y = (byte)(start.y + 1), z = (byte)(start.z + 3), color = (byte)(249 - 40) }, 249 - 40));
            vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + 2), y = (byte)(start.y + 2), z = (byte)(start.z + 3), color = (byte)(249 - 40) },
                new MagicaVoxelData { x = (byte)(start.x + 0), y = (byte)(start.y + 2), z = (byte)(start.z + 3), color = (byte)(249 - 40) }, 249 - 40));

            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (length > 0 || x - 1 > length)
                    {
                        vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + x), y = (byte)(start.y + y), z = (byte)(start.z - 1), color = (byte)(249 - 72) },
                        new MagicaVoxelData { x = (byte)(start.x + x - 1 - length), y = (byte)(start.y + y), z = (byte)(start.z - 2 - length), color = (byte)(249 - 72) }, 249 - 72));
                        if (x % 2 == 1)
                        {
                            vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + x - 1 - length), y = (byte)(start.y + y), z = (byte)(start.z - 2 - length), color = (byte)(249 - 72) },
                                new MagicaVoxelData { x = (byte)(start.x + x - ((y > 0 && y < 3 && x < 2) ? 10 : 6) - length), y = (byte)(start.y + y), z = (byte)(start.z - ((y > 0 && y < 3 && x < 2) ? 10 : 6) - length), color = (byte)(249 - 72) },
                                (y > 0 && y < 3 && x > 0) ? 249 - 160 : 249 - 152));
                        }
                    }
                }
            }

            for (int z = 0; z < 2; z++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (length > 0 || z - 1 > length)
                        vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - 1), y = (byte)(start.y + y), z = (byte)(start.z + z), color = (byte)(249 - 72) },
                        new MagicaVoxelData { x = (byte)(start.x - 2 - length), y = (byte)(start.y + y), z = (byte)(start.z + z - 1 - length), color = (byte)(249 - 72) }, 249 - 72));
                    if (z % 2 == 1)
                    {
                        vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - 2 - length), y = (byte)(start.y + y), z = (byte)(start.z + z - 1 - length), color = (byte)(249 - 72) },
                            new MagicaVoxelData { x = (byte)(start.x - ((y > 0 && y < 3 && z < 2) ? 10 : 6) - length), y = (byte)(start.y + y), z = (byte)(start.z + z - ((y > 0 && y < 3 && z < 2) ? 10 : 6) - length), color = (byte)(249 - 72) },
                            (y > 0 && y < 3 && z < 2) ? 249 - 160 : 249 - 152));
                    }
                }
            }
            if (length > 0)
            {
                for (int y = 0; y < 4; y++)
                {
                    vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - 1), y = (byte)(start.y + y), z = (byte)(start.z - 1), color = (byte)(249 - 72) },
                    new MagicaVoxelData { x = (byte)(start.x - 2 - length), y = (byte)(start.y + y), z = (byte)(start.z - 2 - length), color = (byte)(249 - 72) }, 249 - 72));

                    vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - 2 - length), y = (byte)(start.y + y), z = (byte)(start.z - 2 - length), color = (byte)(249 - 160) },
                        new MagicaVoxelData { x = (byte)(start.x - 2 - ((y > 0 && y < 3) ? 10 : 6) - length), y = (byte)(start.y + y), z = (byte)(start.z - ((y > 0 && y < 3) ? 10 : 6) - length), color = (byte)(249 - 160) },
                        249 - 160));
                }
            }
            return vox;
        }

        private static List<MagicaVoxelData> generateConeDouble(MagicaVoxelData start, int segments, int color)
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
        private static List<MagicaVoxelData> generateDownwardMissileDouble(MagicaVoxelData start, int length)
        {
            List<MagicaVoxelData> vox = new List<MagicaVoxelData>(128);
            for (int x = 0; x < 3; x++)
            {
                for (int z = 0; z < 3; z++)
                {
                    vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - x), y = (byte)(start.y), z = (byte)(start.z - z), color = (byte)(249 - 40) },
                        new MagicaVoxelData { x = (byte)(start.x - x), y = (byte)(start.y + 3), z = (byte)(start.z - z), color = (byte)(249 - 40) }, 249 - 40));
                }
            }

            vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - 3), y = (byte)(start.y + 1), z = (byte)(start.z), color = (byte)(249 - 40) },
                new MagicaVoxelData { x = (byte)(start.x - 3), y = (byte)(start.y + 1), z = (byte)(start.z - 3), color = (byte)(249 - 40) }, 249 - 40));
            vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - 3), y = (byte)(start.y + 2), z = (byte)(start.z), color = (byte)(249 - 40) },
                new MagicaVoxelData { x = (byte)(start.x - 3), y = (byte)(start.y + 2), z = (byte)(start.z - 3), color = (byte)(249 - 40) }, 249 - 40));
            vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - 2), y = (byte)(start.y + 1), z = (byte)(start.z - 3), color = (byte)(249 - 40) },
                new MagicaVoxelData { x = (byte)(start.x - 0), y = (byte)(start.y + 1), z = (byte)(start.z - 3), color = (byte)(249 - 40) }, 249 - 40));
            vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - 2), y = (byte)(start.y + 2), z = (byte)(start.z - 3), color = (byte)(249 - 40) },
                new MagicaVoxelData { x = (byte)(start.x - 0), y = (byte)(start.y + 2), z = (byte)(start.z - 3), color = (byte)(249 - 40) }, 249 - 40));

            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (start.z < 120 - 2 && (length > 0 || x - 1 > length))
                        vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - x), y = (byte)(start.y + y), z = (byte)(start.z + 1), color = (byte)(249 - 72) },
                        new MagicaVoxelData { x = (byte)(start.x - x + 1 + length), y = (byte)(start.y + y), z = (byte)(start.z + 2 + length), color = (byte)(249 - 72) }, 249 - 72));
                }
            }

            for (int z = 0; z < 2; z++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (length > 0 || z - 1 > length)
                        vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + 1), y = (byte)(start.y + y), z = (byte)(start.z - z), color = (byte)(249 - 72) },
                        new MagicaVoxelData { x = (byte)(start.x + 2 + length), y = (byte)(start.y + y), z = (byte)(start.z - z + 1 + length), color = (byte)(249 - 72) }, 249 - 72));

                }
            }

            if (length > 0)
            {
                for (int y = 0; y < 4; y++)
                {
                    vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + 1), y = (byte)(start.y + y), z = (byte)(start.z + 1), color = (byte)(249 - 72) },
                    new MagicaVoxelData { x = (byte)(start.x + 2 + length), y = (byte)(start.y + y), z = (byte)(start.z + 2 + length), color = (byte)(249 - 72) }, 249 - 72));
                }
            }
            return vox;
        }
        private static List<MagicaVoxelData> generateDownwardMissileFieryTrailDouble(MagicaVoxelData start, int length)
        {
            List<MagicaVoxelData> vox = new List<MagicaVoxelData>(128);
            for (int x = 0; x < 3; x++)
            {
                for (int z = 0; z < 3; z++)
                {
                    vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - x), y = (byte)(start.y), z = (byte)(start.z - z), color = (byte)(249 - 40) },
                        new MagicaVoxelData { x = (byte)(start.x - x), y = (byte)(start.y + 3), z = (byte)(start.z - z), color = (byte)(249 - 40) }, 249 - 40));
                }
            }

            vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - 3), y = (byte)(start.y + 1), z = (byte)(start.z), color = (byte)(249 - 40) },
                new MagicaVoxelData { x = (byte)(start.x - 3), y = (byte)(start.y + 1), z = (byte)(start.z - 3), color = (byte)(249 - 40) }, 249 - 40));
            vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - 3), y = (byte)(start.y + 2), z = (byte)(start.z), color = (byte)(249 - 40) },
                new MagicaVoxelData { x = (byte)(start.x - 3), y = (byte)(start.y + 2), z = (byte)(start.z - 3), color = (byte)(249 - 40) }, 249 - 40));
            vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - 2), y = (byte)(start.y + 1), z = (byte)(start.z - 3), color = (byte)(249 - 40) },
                new MagicaVoxelData { x = (byte)(start.x - 0), y = (byte)(start.y + 1), z = (byte)(start.z - 3), color = (byte)(249 - 40) }, 249 - 40));
            vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - 2), y = (byte)(start.y + 2), z = (byte)(start.z - 3), color = (byte)(249 - 40) },
                new MagicaVoxelData { x = (byte)(start.x - 0), y = (byte)(start.y + 2), z = (byte)(start.z - 3), color = (byte)(249 - 40) }, 249 - 40));

            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (length > 0 || x - 1 > length)
                    {
                        vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - x), y = (byte)(start.y + y), z = (byte)(start.z + 1), color = (byte)(249 - 72) },
                        new MagicaVoxelData { x = (byte)(start.x - x + 1 + length), y = (byte)(start.y + y), z = (byte)(start.z + 2 + length), color = (byte)(249 - 72) }, 249 - 72));
                        if (x % 2 == 1)
                        {
                            vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x - x + 1 + length), y = (byte)(start.y + y), z = (byte)(start.z + 2 + length), color = (byte)(249 - 72) },
                                new MagicaVoxelData { x = (byte)(start.x - x + ((y > 0 && y < 3 && x < 2) ? 10 : 6) + length), y = (byte)(start.y + y), z = (byte)(start.z + ((y > 0 && y < 3 && x < 2) ? 10 : 6) + length), color = (byte)(249 - 72) },
                                (y > 0 && y < 3 && x > 0) ? 249 - 160 : 249 - 152));
                        }
                    }
                }
            }

            for (int z = 0; z < 2; z++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (length > 0 || z - 1 > length)
                        vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + 1), y = (byte)(start.y + y), z = (byte)(start.z - z), color = (byte)(249 - 72) },
                        new MagicaVoxelData { x = (byte)(start.x + 2 + length), y = (byte)(start.y + y), z = (byte)(start.z - z + 1 + length), color = (byte)(249 - 72) }, 249 - 72));
                    if (z % 2 == 1)
                    {
                        vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + 2 + length), y = (byte)(start.y + y), z = (byte)(start.z - z + 1 + length), color = (byte)(249 - 72) },
                            new MagicaVoxelData { x = (byte)(start.x + ((y > 0 && y < 3 && z < 2) ? 10 : 6) + length), y = (byte)(start.y + y), z = (byte)(start.z - z + ((y > 0 && y < 3 && z < 2) ? 10 : 6) + length), color = (byte)(249 - 72) },
                            (y > 0 && y < 3 && z < 2) ? 249 - 160 : 249 - 152));
                    }
                }
            }
            if (length > 0)
            {
                for (int y = 0; y < 4; y++)
                {
                    vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + 1), y = (byte)(start.y + y), z = (byte)(start.z + 1), color = (byte)(249 - 72) },
                    new MagicaVoxelData { x = (byte)(start.x + 2 + length), y = (byte)(start.y + y), z = (byte)(start.z + 2 + length), color = (byte)(249 - 72) }, 249 - 72));

                    vox.AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(start.x + 2 + length), y = (byte)(start.y + y), z = (byte)(start.z + 2 + length), color = (byte)(249 - 160) },
                        new MagicaVoxelData { x = (byte)(start.x + 2 + ((y > 0 && y < 3) ? 10 : 6) + length), y = (byte)(start.y + y), z = (byte)(start.z + ((y > 0 && y < 3) ? 10 : 6) + length), color = (byte)(249 - 160) },
                        249 - 160));
                }
            }
            return vox;
        }

        private static List<MagicaVoxelData> generateDownwardConeDouble(MagicaVoxelData start, int segments, int color)
        {
            List<MagicaVoxelData> cone = new List<MagicaVoxelData>(40);
            for (int x = 0; x < segments; x++)
            {
                for (float y = 0; y <= x * 0.75f; y++)
                {
                    for (float z = 0; z <= x * 1.25f; z++)
                    {
                        if (Math.Floor(y) == y && Math.Floor(z) == z)
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
        public static MagicaVoxelData[][] HugeExplosionDouble(MagicaVoxelData[] voxels, int blowback, int maxFrames, int trimLevel)
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

                int[] minX = new int[60];
                int[] maxX = new int[60];
                float[] midX = new float[60];
                for (int level = 0; level < 60; level++)
                {
                    minX[level] = vls.Min(v => v.x * ((v.z != level || v.color < 249 - 168) ? 100 : 1));
                    maxX[level] = vls.Max(v => v.x * ((v.z != level || v.color < 249 - 168) ? 0 : 1));
                    midX[level] = (maxX[level] + minX[level]) / 2F;
                }

                int[] minY = new int[60];
                int[] maxY = new int[60];
                float[] midY = new float[60];
                for (int level = 0; level < 60; level++)
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
                        mvd.color = (byte)((r.Next(1 + f) == 0) ? 249 - 144 : (r.Next(8) == 0) ? 249 - 152 : 249 - 64); //random transform to guts
                    else if (v.color == 249 - 144) //guts
                        mvd.color = (byte)((r.Next(10) == 0) ? 249 - 152 : 249 - 144); //random transform to orange fire
                    else if (v.color <= 249 - 168) //clear and markers
                        mvd.color = (byte)249 - 168; //clear stays clear
                    else if (v.color == 249 - 120)
                        mvd.color = 249 - 168; //clear inner shadow
                    else if (v.color == 249 - 96)
                        mvd.color = 249 - 96; //shadow stays shadow
                    else if (v.color == 249 - 80) //lights
                        mvd.color = 249 - 16; //cannon color for broken lights
                    else if (v.color == 249 - 88) //windows
                        mvd.color = (byte)((r.Next(3) == 0) ? 249 - 168 : 249 - 88); //random transform to clear
                    else if (v.color == 249 - 104) //rotors
                        mvd.color = 249 - 56; //grayish paint color for broken rotors
                    else if (v.color == 249 - 112)
                        mvd.color = 249 - 168; //clear non-active rotors
                    else if (v.color == 249 - 152) //orange fire
                        mvd.color = (byte)((f > maxFrames / 2 && r.Next(maxFrames) <= f + 2) ? 249 - 136 : ((r.Next(3) <= 1) ? 249 - 160 : ((r.Next(3) == 0) ? 249 - 136 : 249 - 152))); //random transform to yellow fire or smoke
                    else if (v.color == 249 - 160) //yellow fire
                        mvd.color = (byte)((f > maxFrames / 2 && r.Next(maxFrames) <= f + 2) ? 249 - 136 : ((r.Next(3) <= 1) ? 249 - 152 : ((r.Next(4) == 0) ? 249 - 136 : 249 - 160))); //random transform to orange fire or smoke
                    else if (v.color == 249 - 136) //smoke
                        mvd.color = (byte)((f > maxFrames * 3 / 5 && r.Next(maxFrames) <= f) ? 249 - 168 : 249 - 136); //random transform to clear
                    else
                        mvd.color = (byte)((r.Next((f + 3) * 2) <= 2) ? 249 - (152 + ((r.Next(4) == 0) ? 8 : 0)) : v.color); //random transform to orange or yellow fire

                    float xMove = 0, yMove = 0, zMove = 0;
                    if (f > maxFrames / 2 && (mvd.color == 249 - 152 || mvd.color == 249 - 160 || mvd.color == 249 - 136))
                    {
                        zMove = f * (16F * 0.4F / maxFrames);
                    }
                    else
                    {
                        if (v.x > midX[v.z])
                            xMove = r.Next(10) + 5 + blowback;//((midX[v.z] - r.Next(4) - ((blowback) ? 7 : 0) - (maxX[v.z] - v.x)) * 0.6F * ((v.z - minZ + 1) / (maxZ - minZ + 1F))); //5 -
                        else if (v.x < midX[v.z])// && v.x < minX + 5)
                            xMove = blowback - r.Next(10) - 6;// ((0 + (v.x - midX[v.z] + r.Next(4) - ((blowback) ? 6 : 0))) * 0.6F * ((v.z - minZ + 1) / (maxZ - minZ + 1F))); //-5 +
                        if (v.y > midY[v.z])// && v.y > maxY - 5)
                            yMove = r.Next(10) + 5;//((midY[v.z] - r.Next(4) - (maxY[v.z] - v.y)) * 0.6F * ((v.z - minZ + 1) / (maxZ - minZ + 1F)));//5 -
                        else if (v.y < midY[v.z])// && v.y < minY + 5)
                            yMove = 0 - r.Next(10) - 6;//((0 + (v.y - midY[v.z] + r.Next(4))) * 0.6F * ((v.z - minZ + 1) / (maxZ - minZ + 1F))); //-5 +

                        if (mvd.color == 249 - 152 || mvd.color == 249 - 160 || mvd.color == 249 - 136)
                            zMove = f * 0.55F;
                        else if (f < (maxFrames - 3) && minZ <= 1)
                            zMove = (v.z / ((maxZ + 1) * (0.3F))) * ((maxFrames - 3) - f) * 0.8F;
                        else
                            zMove = (1 - f * 2.1F);
                    }
                    if (xMove > 0)
                    {
                        float nv = (v.x + (xMove / (0.2f * (f + 4)))) - Math.Abs((yMove / (0.5f * (f + 3))));
                        if (nv < 1) nv = 1;
                        if (nv > 118) nv = 118;
                        mvd.x = (byte)((blowback <= 0) ? Math.Floor(nv) : (Math.Ceiling(nv)));
                    }
                    else if (xMove < 0)
                    {
                        float nv = (v.x + (xMove / (0.2f * (f + 4)))) + Math.Abs((yMove / (0.5f * (f + 3))));
                        if (nv < 1) nv = 1;
                        if (nv > 118) nv = 118;
                        mvd.x = (byte)((blowback > 0) ? Math.Floor(nv) : (Math.Ceiling(nv)));
                    }
                    else
                    {
                        if (v.x < 1) mvd.x = 1;
                        if (v.x > 118) mvd.x = 118;
                        else mvd.x = v.x;
                    }
                    if (yMove > 0)
                    {
                        float nv = (v.y + (yMove / (0.2f * (f + 4)))) - Math.Abs((xMove / (0.5f * (f + 3))));
                        if (nv < 1) nv = 1;
                        if (nv > 118) nv = 118;
                        mvd.y = (byte)(Math.Floor(nv));
                    }
                    else if (yMove < 0)
                    {
                        float nv = (v.y + (yMove / (0.2f * (f + 4)))) + Math.Abs((xMove / (0.5f * (f + 3))));
                        if (nv < 1) nv = 1;
                        if (nv > 118) nv = 118;
                        mvd.y = (byte)(Math.Ceiling(nv));
                    }
                    else
                    {
                        mvd.y = v.y;
                    }
                    if (zMove != 0)
                    {
                        float nv = (v.z + (zMove / (0.2f * (f + 3))));

                        if (nv <= 0 && f < maxFrames && !(mvd.color == 249 - 152 || mvd.color == 249 - 160 || mvd.color == 249 - 136)) nv = r.Next(2); //bounce
                        else if (nv < 0) nv = 0;

                        if (nv > 45) nv = 45;
                        mvd.z = (byte)Math.Round(nv);
                    }
                    else
                    {
                        mvd.z = v.z;
                    }
                    working.Add(mvd);
                    if (r.Next(maxFrames) > f + maxFrames / 6 && r.Next(maxFrames) > f + 2) working.AddRange(adjacent(mvd, new int[] { 249 - 152, 249 - 160, 249 - 152, 249 - 160, 249 - 136 }));
                }
                working = working.Where(_ => r.Next(7) < 8 - trimLevel).ToList();
                voxelFrames[f] = new MagicaVoxelData[working.Count];
                working.ToArray().CopyTo(voxelFrames[f], 0);
            }
            MagicaVoxelData[][] frames = new MagicaVoxelData[maxFrames][];

            for (int f = 1; f <= maxFrames; f++)
            {
                frames[f - 1] = new MagicaVoxelData[voxelFrames[f].Length];
                voxelFrames[f].ToArray().CopyTo(frames[f - 1], 0);
            }
            return frames;
        }

        public static MagicaVoxelData[][] HandgunAnimationDouble(MagicaVoxelData[][] parsedFrames, int unit)
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
                    extra[f].AddRange(generateBox(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x), y = (byte)(launchers[currentlyFiring].y), z = (byte)(launchers[currentlyFiring].z), color = 249 - 152 }, 8, 2, 2, 249 - 160));
                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x + 2), y = (byte)(launchers[currentlyFiring].y + 2), z = (byte)(launchers[currentlyFiring].z), color = 249 - 160 }, 249 - 152));
                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x + 2), y = (byte)(launchers[currentlyFiring].y - 2), z = (byte)(launchers[currentlyFiring].z), color = 249 - 160 }, 249 - 152));
                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x + 2), y = (byte)(launchers[currentlyFiring].y), z = (byte)(launchers[currentlyFiring].z + 2), color = 249 - 160 }, 249 - 152));
                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring].x + 2), y = (byte)(launchers[currentlyFiring].y), z = (byte)(launchers[currentlyFiring].z - 2), color = 249 - 160 }, 249 - 152));
                }
                if (currentlyFiring <= launchers.Count && currentlyFiring > 0)
                {
                    extra[f].AddRange(generateBox(new MagicaVoxelData { x = (byte)(launchers[currentlyFiring - 1].x + 4), y = (byte)(launchers[currentlyFiring - 1].y), z = (byte)(launchers[currentlyFiring - 1].z), color = 249 - 152 }, 6, 2, 2, 249 - 160));
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
        public static MagicaVoxelData[][] HandgunReceiveAnimationDouble(MagicaVoxelData[][] parsedFrames, int strength)
        {
            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[parsedFrames.Length];
            MagicaVoxelData[][] finalFrames = new MagicaVoxelData[parsedFrames.Length][];

            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length];
            MagicaVoxelData[][][] plumes = new MagicaVoxelData[strength * 2][][];
            MagicaVoxelData[][][] bursts = new MagicaVoxelData[strength * 2][][];
            for (int s = 0; s < 2 * strength; s++)
            {
                plumes[s] = SmokePlumeDouble(new MagicaVoxelData
                {
                    x = (byte)((s % 2 == 0) ? 90 - r.Next(20) : r.Next(20) + 30),
                    y = (byte)((s % 2 == 0) ? (r.Next(30) + 20) : (100 - r.Next(30))),
                    z = 0,
                    color = 249 - 136
                }, 8, 7);
                bursts[s] = BurstDouble(new MagicaVoxelData
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
        public static MagicaVoxelData[][] MachineGunAnimationDouble(MagicaVoxelData[][] parsedFrames, int unit)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[parsedFrames.Length][];
            voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
            voxelFrames[parsedFrames.Length - 1] = new MagicaVoxelData[parsedFrames[parsedFrames.Length - 1].Length];
            parsedFrames[0].CopyTo(voxelFrames[0], 0);
            parsedFrames[parsedFrames.Length - 1].CopyTo(voxelFrames[parsedFrames.Length - 1], 0);
            List<MagicaVoxelData> launchers = new List<MagicaVoxelData>(40);
            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length - 2];

            List<int> known = new List<int>(30);
            foreach (MagicaVoxelData mvd in voxelFrames[0])
            {
                if (mvd.color == 249 - 175)
                {
                    launchers.Add(mvd);
                }
            }

            for (int f = 0; f < voxelFrames.Length - 2; f++) //going only through the middle
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
                if (currentlyFiring % 2 == 0)
                {

                    foreach (MagicaVoxelData launcher in launchers)
                    {
                        if (currentlyFiring != 0)
                        {
                            currentlyFiring = (currentlyFiring + 1) % (launchers.Count / 4 + 1);
                            continue;
                        }
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 7), y = launcher.y, z = launcher.z, color = (byte)(249 - 160) }, 249 - 160));

                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 5), y = launcher.y, z = (byte)(launcher.z + 2), color = 249 - 160 }, 249 - 152));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 5), y = launcher.y, z = (byte)(launcher.z - 2), color = 249 - 160 }, 249 - 152));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 2), z = launcher.z, color = 249 - 160 }, 249 - 152));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 2), z = launcher.z, color = 249 - 160 }, 249 - 152));

                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 2), color = 249 - 160 }, 249 - 152));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 2), color = 249 - 160 }, 249 - 152));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 2), z = (byte)(launcher.z - 2), color = 249 - 160 }, 249 - 152));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 2), z = (byte)(launcher.z - 2), color = 249 - 160 }, 249 - 152));

                    }
                    extra[f] = extra[f].Where(v => r.Next(10) > 1).ToList();

                }
                else
                {
                    foreach (MagicaVoxelData launcher in launchers)
                    {
                        if (currentlyFiring < 2 && launchers.Count > 8)
                        {
                            currentlyFiring = (currentlyFiring + 1) % (launchers.Count / 4 + 1);
                            continue;
                        }
                        extra[f].AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = launcher.z, color = (byte)(249 - 152) }
                            , new MagicaVoxelData { x = (byte)(launcher.x + 9), y = launcher.y, z = launcher.z, color = (byte)(249 - 152) }, 249 - 152));

                        currentlyFiring = (currentlyFiring + 1) % (launchers.Count / 4 + 1);
                    }
                    extra[f] = extra[f].Where(v => r.Next(10) > 2).ToList();

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
        public static MagicaVoxelData[][] MachineGunReceiveAnimationDouble(MagicaVoxelData[][] parsedFrames, int strength)
        {
            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[parsedFrames.Length];
            MagicaVoxelData[][] finalFrames = new MagicaVoxelData[parsedFrames.Length][];

            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length];
            MagicaVoxelData[][][] sparks = new MagicaVoxelData[strength][][];
            for (int s = 0; s < strength; s++)
            {
                sparks[s] = SparksDouble(new MagicaVoxelData
                {
                    x = (byte)(66 + r.Next(10)),
                    y = (byte)(30 + s * 12),
                    z = 0,
                    color = 249 - 160
                }, 2 * (3 + strength));
            }
            for (int f = 0; f < voxelFrames.Length; f++)
            {
                extra[f] = new List<MagicaVoxelData>(160);
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

        public static MagicaVoxelData[][] CannonAnimationDouble(MagicaVoxelData[][] parsedFrames, int unit)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[parsedFrames.Length][];
            voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
            voxelFrames[parsedFrames.Length - 1] = new MagicaVoxelData[parsedFrames[parsedFrames.Length - 1].Length];
            parsedFrames[0].CopyTo(voxelFrames[0], 0);
            parsedFrames[parsedFrames.Length - 1].CopyTo(voxelFrames[parsedFrames.Length - 1], 0);
            List<MagicaVoxelData> launchers = new List<MagicaVoxelData>(40);
            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length - 2];

            List<int> known = new List<int>(30);
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
            //            List<MagicaVoxelData>[] halves = { launchers.Where(mvd => (mvd.y <= midY)).ToList(), launchers.Where(mvd => (mvd.y > midY)).ToList() };
            List<MagicaVoxelData>[] halves = { launchers.ToList(), launchers.ToList() };

            for (int f = 0; f < voxelFrames.Length - 2; f++) //going only through the middle
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
                if (f == 0 || f == 1)
                {
                    foreach (MagicaVoxelData launcher in halves[0])
                    {
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 12), y = launcher.y, z = launcher.z, color = (byte)(249 - 160) }, (249 - 160 + (r.Next(2) * 8))));

                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = launcher.y, z = (byte)(launcher.z + 8), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = launcher.y, z = (byte)(launcher.z - 8), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = (byte)(launcher.y + 8), z = launcher.z, color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = (byte)(launcher.y - 8), z = launcher.z, color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));

                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = (byte)(launcher.y + 5), z = (byte)(launcher.z + 5), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = (byte)(launcher.y + 5), z = (byte)(launcher.z - 5), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = (byte)(launcher.y - 5), z = (byte)(launcher.z + 5), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = (byte)(launcher.y - 5), z = (byte)(launcher.z - 5), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));

                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = (byte)(launcher.y + 3), z = (byte)(launcher.z + 5), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = (byte)(launcher.y + 3), z = (byte)(launcher.z - 5), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = (byte)(launcher.y - 3), z = (byte)(launcher.z + 5), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = (byte)(launcher.y - 3), z = (byte)(launcher.z - 5), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));

                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = (byte)(launcher.y + 5), z = (byte)(launcher.z + 3), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = (byte)(launcher.y + 5), z = (byte)(launcher.z - 3), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = (byte)(launcher.y - 5), z = (byte)(launcher.z + 3), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = (byte)(launcher.y - 5), z = (byte)(launcher.z - 3), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));

                        extra[f] = extra[f].Where(v => r.Next(10) > 0).ToList();

                    }
                }
                else if (f == 1 || f == 2)
                {

                    foreach (MagicaVoxelData launcher in halves[1])
                    {

                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 10), y = launcher.y, z = launcher.z, color = (byte)(249 - 160) }, (249 - 160 + (r.Next(2) * 8))));

                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = launcher.y, z = (byte)(launcher.z + 6), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = launcher.y, z = (byte)(launcher.z - 6), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y + 6), z = launcher.z, color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y - 6), z = launcher.z, color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));

                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y + 4), z = (byte)(launcher.z + 4), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y + 4), z = (byte)(launcher.z - 4), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y - 4), z = (byte)(launcher.z + 4), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y - 4), z = (byte)(launcher.z - 4), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));

                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 4), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y + 2), z = (byte)(launcher.z - 4), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 4), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y - 2), z = (byte)(launcher.z - 4), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));

                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y + 4), z = (byte)(launcher.z + 2), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y + 4), z = (byte)(launcher.z - 2), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y - 4), z = (byte)(launcher.z + 2), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y - 4), z = (byte)(launcher.z - 2), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));

                        extra[f] = extra[f].Where(v => r.Next(10) > 0).ToList();

                    }
                }
                else if (f == 3)
                {
                    foreach (MagicaVoxelData launcher in launchers)
                    {
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 5), y = launcher.y, z = launcher.z, color = (byte)(249 - 160) }, 249 - 136));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 2), color = 249 - 160 }, 249 - 136));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 2), z = launcher.z, color = 249 - 160 }, 249 - 136));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 2), color = 249 - 160 }, 249 - 136));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 2), z = launcher.z, color = 249 - 160 }, 249 - 136));
                    }
                    extra[f] = extra[f].Where(v => r.Next(6) > 2).ToList();

                }
                else if (f == 4)
                {
                    foreach (MagicaVoxelData launcher in launchers)
                    {
                        extra[f].AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 2), color = (byte)(249 - 136) },
                            new MagicaVoxelData { x = (byte)(launcher.x + 5), y = launcher.y, z = launcher.z, color = (byte)(249 - 160) }, 249 - 136));

                        extra[f].AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 2), color = (byte)(249 - 136) },
                            new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 4), color = (byte)(249 - 160) }, 249 - 136));
                        extra[f].AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 2), color = (byte)(249 - 136) },
                            new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 2), color = (byte)(249 - 160) }, 249 - 136));
                        extra[f].AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 2), color = (byte)(249 - 136) },
                            new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 4), color = (byte)(249 - 160) }, 249 - 136));
                        extra[f].AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 2), color = (byte)(249 - 136) },
                            new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 2), color = (byte)(249 - 160) }, 249 - 136));

                    }
                    extra[f] = extra[f].Where(v => r.Next(6) > 2 && r.Next(6) > 1).ToList();

                }
                else if (f == 5)
                {
                    foreach (MagicaVoxelData launcher in launchers)
                    {
                        extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 5), z = (byte)(launcher.z + 5), color = 249 - 136 }, 249 - 136));
                        extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 5), z = (byte)(launcher.z + 5), color = 249 - 136 }, 249 - 136));
                        extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 5), z = (byte)(launcher.z + 5), color = 249 - 136 }, 249 - 136));
                        extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 5), z = (byte)(launcher.z + 5), color = 249 - 136 }, 249 - 136));
                    }
                    extra[f] = extra[f].Where(v => r.Next(6) > 3 && r.Next(6) > 2).ToList();

                }
                else if (f == 6)
                {
                    foreach (MagicaVoxelData launcher in launchers)
                    {

                        extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 7), z = (byte)(launcher.z + 5), color = 249 - 136 }, 249 - 136));
                        extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 7), z = (byte)(launcher.z + 5), color = 249 - 136 }, 249 - 136));
                        extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 3 + r.Next(6)), z = (byte)(launcher.z + 9), color = 249 - 136 }, 249 - 136));
                        extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 3 - r.Next(6)), z = (byte)(launcher.z + 9), color = 249 - 136 }, 249 - 136));
                    }
                    extra[f] = extra[f].Where(v => r.Next(6) > 4 && r.Next(6) > 3).ToList();

                }
                else if (f == 7)
                {
                    foreach (MagicaVoxelData launcher in launchers)
                    {

                        extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 7), z = (byte)(launcher.z + 6), color = 249 - 136 }, 249 - 136));
                        extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 7), z = (byte)(launcher.z + 6), color = 249 - 136 }, 249 - 136));
                        extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 3 + r.Next(6)), z = (byte)(launcher.z + 10), color = 249 - 136 }, 249 - 136));
                        extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 3 - r.Next(6)), z = (byte)(launcher.z + 10), color = 249 - 136 }, 249 - 136));
                    }
                    extra[f] = extra[f].Where(v => r.Next(7) > 5 && r.Next(6) > 4).ToList();

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
        public static MagicaVoxelData[][] CannonReceiveAnimationDouble(MagicaVoxelData[][] parsedFrames, int strength)
        {
            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[parsedFrames.Length];
            MagicaVoxelData[][] finalFrames = new MagicaVoxelData[parsedFrames.Length][];

            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length];
            MagicaVoxelData[][][] explosions = new MagicaVoxelData[strength][][];
            for (int s = 0; s < strength; s++)
            {
                explosions[s] = HugeExplosionDouble(randomFill(75, 56 + s, 16 + s, 4, 4, 4, new int[] { 249 - 152, 249 - 152, 249 - 160, 249 - 160, 249 - 136 }).ToArray(), 3, 6 + strength, 2);
            }
            for (int f = 0; f < voxelFrames.Length; f++)
            {
                extra[f] = new List<MagicaVoxelData>(400);
            }

            for (int i = 0; i < 6 + strength; i++)
            {
                for (int sp = 0; sp < explosions.Length; sp++)
                    extra[3 + i].AddRange(explosions[sp][i]);
            }

            for (int f = 0; f < voxelFrames.Length; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(400);
                working.AddRange(extra[f]);
                finalFrames[f] = working.ToArray();
            }
            return finalFrames;
        }

        public static MagicaVoxelData[][] LongCannonAnimationDouble(MagicaVoxelData[][] parsedFrames, int unit)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[parsedFrames.Length][];
            voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
            voxelFrames[parsedFrames.Length - 1] = new MagicaVoxelData[parsedFrames[parsedFrames.Length - 1].Length];
            parsedFrames[0].CopyTo(voxelFrames[0], 0);
            parsedFrames[parsedFrames.Length - 1].CopyTo(voxelFrames[parsedFrames.Length - 1], 0);
            List<MagicaVoxelData> launchers = new List<MagicaVoxelData>(40);
            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length - 2];

            List<int> known = new List<int>(30);
            foreach (MagicaVoxelData mvd in voxelFrames[0])
            {
                if (mvd.color == 249 - 172)
                {
                    launchers.Add(mvd);
                }
            }

            //            List<MagicaVoxelData>[] halves = { launchers.Where(mvd => (mvd.y <= midY)).ToList(), launchers.Where(mvd => (mvd.y > midY)).ToList() };

            for (int f = 0; f < voxelFrames.Length - 2; f++) //going only through the middle
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
                if (f == 0 || f == 1 || f == 2)
                {
                    foreach (MagicaVoxelData launcher in launchers)
                    {
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 12), y = (byte)(launcher.y + 0), z = (byte)(launcher.z + 12), color = (byte)(249 - 160) }, (249 - 160 + (r.Next(2) * 8))));

                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y + 0), z = (byte)(launcher.z + 12), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y + 0), z = (byte)(launcher.z + 4), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y + 4), z = (byte)(launcher.z + 8), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y - 4), z = (byte)(launcher.z + 8), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));

                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y + 5), z = (byte)(launcher.z + 4), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y + 5), z = (byte)(launcher.z + 12), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y - 5), z = (byte)(launcher.z + 4), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y - 5), z = (byte)(launcher.z + 12), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));

                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y + 4), z = (byte)(launcher.z + 4), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y + 4), z = (byte)(launcher.z + 12), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y - 4), z = (byte)(launcher.z + 4), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 8), y = (byte)(launcher.y - 4), z = (byte)(launcher.z + 12), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));

                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 6), y = (byte)(launcher.y + 6), z = (byte)(launcher.z + 4), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 6), y = (byte)(launcher.y + 6), z = (byte)(launcher.z + 8), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 6), y = (byte)(launcher.y - 6), z = (byte)(launcher.z + 4), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 6), y = (byte)(launcher.y - 6), z = (byte)(launcher.z + 8), color = 249 - 160 }, (249 - 160 + (r.Next(2) * 8))));

                        extra[f] = extra[f].Where(v => r.Next(9) > f).ToList();

                    }
                }
                else if (f == 3)
                {
                    foreach (MagicaVoxelData launcher in launchers)
                    {
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 5), y = launcher.y, z = launcher.z, color = (byte)(249 - 160) }, 249 - 136));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 1), color = 249 - 160 }, 249 - 136));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 3), color = 249 - 160 }, 249 - 136));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 1), color = 249 - 160 }, 249 - 136));
                        extra[f].AddRange(generateStraightLine(launcher, new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 3), color = 249 - 160 }, 249 - 136));
                    }
                    extra[f] = extra[f].Where(v => r.Next(6) > 2).ToList();

                }
                else if (f == 4)
                {
                    foreach (MagicaVoxelData launcher in launchers)
                    {
                        extra[f].AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 1), color = (byte)(249 - 136) },
                            new MagicaVoxelData { x = (byte)(launcher.x + 5), y = launcher.y, z = (byte)(launcher.z + 3), color = (byte)(249 - 160) }, 249 - 136));
                        extra[f].AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 3), color = (byte)(249 - 136) },
                            new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 5), color = (byte)(249 - 160) }, 249 - 136));
                        extra[f].AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 2), color = (byte)(249 - 136) },
                            new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 2), z = (byte)(launcher.z + 4), color = (byte)(249 - 160) }, 249 - 136));
                        extra[f].AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 3), color = (byte)(249 - 136) },
                            new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 5), color = (byte)(249 - 160) }, 249 - 136));
                        extra[f].AddRange(generateStraightLine(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z + 2), color = (byte)(249 - 136) },
                            new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 2), z = (byte)(launcher.z + 4), color = (byte)(249 - 160) }, 249 - 136));

                    }
                    extra[f] = extra[f].Where(v => r.Next(6) > 2 && r.Next(6) > 1).ToList();

                }
                else if (f == 5)
                {
                    foreach (MagicaVoxelData launcher in launchers)
                    {
                        extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y + 5), z = (byte)(launcher.z + 7), color = 249 - 136 }, 249 - 136));
                        extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 3), y = (byte)(launcher.y - 5), z = (byte)(launcher.z + 7), color = 249 - 136 }, 249 - 136));
                        extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 5), z = (byte)(launcher.z + 7), color = 249 - 136 }, 249 - 136));
                        extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 5), z = (byte)(launcher.z + 7), color = 249 - 136 }, 249 - 136));
                    }
                    extra[f] = extra[f].Where(v => r.Next(6) > 3 && r.Next(6) > 2).ToList();

                }
                else if (f == 6)
                {
                    foreach (MagicaVoxelData launcher in launchers)
                    {

                        extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 7), z = (byte)(launcher.z + 7), color = 249 - 136 }, 249 - 136));
                        extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 7), z = (byte)(launcher.z + 7), color = 249 - 136 }, 249 - 136));
                        extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 3 + r.Next(6)), z = (byte)(launcher.z + 10), color = 249 - 136 }, 249 - 136));
                        extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 3 - r.Next(6)), z = (byte)(launcher.z + 10), color = 249 - 136 }, 249 - 136));
                    }
                    extra[f] = extra[f].Where(v => r.Next(6) > 4 && r.Next(6) > 3).ToList();

                }
                else if (f == 7)
                {
                    foreach (MagicaVoxelData launcher in launchers)
                    {

                        extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 7), z = (byte)(launcher.z + 8), color = 249 - 136 }, 249 - 136));
                        extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 7), z = (byte)(launcher.z + 8), color = 249 - 136 }, 249 - 136));
                        extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y + 3 + r.Next(6)), z = (byte)(launcher.z + 12), color = 249 - 136 }, 249 - 136));
                        extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 5), y = (byte)(launcher.y - 3 - r.Next(6)), z = (byte)(launcher.z + 12), color = 249 - 136 }, 249 - 136));
                    }
                    extra[f] = extra[f].Where(v => r.Next(7) > 5 && r.Next(6) > 4).ToList();

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

        public static MagicaVoxelData[][] LongCannonReceiveAnimationDouble(MagicaVoxelData[][] parsedFrames, int strength)
        {
            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[parsedFrames.Length];
            MagicaVoxelData[][] finalFrames = new MagicaVoxelData[parsedFrames.Length][];

            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length];
            MagicaVoxelData[][][] explosions = new MagicaVoxelData[strength][][];
            for (int s = 0; s < strength; s++)
            {
                explosions[s] = HugeExplosionDouble(randomFill(75, 56 + s, 0, 4, 4, 8, new int[] { 249 - 136, 249 - 152, 249 - 136, 249 - 152, 249 - 160 }).ToArray(), 1, 5 + strength, 2);
            }
            for (int f = 0; f < voxelFrames.Length; f++)
            {
                extra[f] = new List<MagicaVoxelData>(600);
            }

            for (int i = 0; i < 5 + strength; i++)
            {
                for (int sp = 0; sp < explosions.Length; sp++)
                    extra[4 + i].AddRange(explosions[sp][i]);
            }
            for (int f = 0; f < voxelFrames.Length; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(600);
                working.AddRange(extra[f]);
                finalFrames[f] = working.ToArray();
            }
            return finalFrames;
        }

        public static MagicaVoxelData[][] RocketAnimationDouble(MagicaVoxelData[][] parsedFrames, int unit)
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
                            x = (byte)(missile[f - 1][i].x + 8),
                            y = (byte)(missile[f - 1][i].y),
                            z = missile[f - 1][i].z,
                            color = missile[f - 1][i].color
                        });
                    }
                }
                if (f == 0)
                {
                    missile[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x), y = launcher.y, z = (byte)(launcher.z), color = 249 - 40 }, 249 - 40));
                }
                if (f == 1)
                {
                    missile[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 6), y = launcher.y, z = (byte)(launcher.z), color = 249 - 40 }, 249 - 40));
                    missile[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = launcher.y, z = (byte)(launcher.z), color = 249 - 40 }, 249 - 72));
                    missile[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z), color = 249 - 40 }, 249 - 72));
                    missile[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 0), y = launcher.y, z = (byte)(launcher.z), color = 249 - 40 }, 249 - 72));

                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 2), y = trail.y, z = (byte)(trail.z), color = 249 - 160 }, 249 - 160));
                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 4), y = (byte)(trail.y), z = (byte)(trail.z), color = 249 - 160 }, 249 - 160));

                    extra[f].AddRange(generateBox(new MagicaVoxelData { x = (byte)(trail.x - 3), y = (byte)(trail.y - 1), z = (byte)(trail.z - 1), color = 249 - 152 }, 3, 4, 4, 249 - 152));

                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 2), y = (byte)(trail.y + 2), z = (byte)(trail.z), color = 249 - 160 }, 249 - 152));
                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 2), y = (byte)(trail.y - 2), z = (byte)(trail.z), color = 249 - 160 }, 249 - 152));
                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 2), y = (byte)(trail.y), z = (byte)(trail.z + 2), color = 249 - 160 }, 249 - 152));
                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 2), y = (byte)(trail.y), z = (byte)(trail.z - 2), color = 249 - 160 }, 249 - 152));
                }
                else if (f == 2)
                {
                    missile[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 6), y = launcher.y, z = (byte)(launcher.z), color = 249 - 40 }, 249 - 72));
                    missile[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = launcher.y, z = (byte)(launcher.z), color = 249 - 40 }, 249 - 72));
                    missile[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z), color = 249 - 40 }, 249 - 72));
                    missile[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x), y = launcher.y, z = (byte)(launcher.z), color = 249 - 40 }, 249 - 72));

                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 2), y = trail.y, z = (byte)(trail.z), color = 249 - 160 }, 249 - 160));
                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 4), y = trail.y, z = (byte)(trail.z), color = 249 - 160 }, 249 - 160));

                    extra[f].AddRange(generateBox(new MagicaVoxelData { x = (byte)(trail.x - 3), y = (byte)(trail.y - 1), z = (byte)(trail.z - 1), color = 249 - 152 }, 3, 4, 4, 249 - 152));

                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 2), y = (byte)(trail.y + 2), z = (byte)(trail.z), color = 249 - 160 }, 249 - 152));
                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 2), y = (byte)(trail.y - 2), z = (byte)(trail.z), color = 249 - 160 }, 249 - 152));
                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 2), y = (byte)(trail.y), z = (byte)(trail.z + 2), color = 249 - 160 }, 249 - 152));
                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 2), y = (byte)(trail.y), z = (byte)(trail.z - 2), color = 249 - 160 }, 249 - 152));

                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 4), y = (byte)(trail.y + 2), z = (byte)(trail.z), color = 249 - 136 }, 249 - 136));
                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 4), y = (byte)(trail.y - 2), z = (byte)(trail.z), color = 249 - 136 }, 249 - 136));
                    extra[f].AddRange(generateBox(new MagicaVoxelData { x = (byte)(trail.x - 6), y = (byte)(trail.y - 2), z = (byte)(trail.z), color = 249 - 152 }, 2, 6, 2, 249 - 136));

                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 2), y = (byte)(trail.y + 2), z = (byte)(trail.z + 2), color = 249 - 136 }, 249 - 136));
                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 2), y = (byte)(trail.y - 2), z = (byte)(trail.z + 2), color = 249 - 136 }, 249 - 136));

                    extra[f].AddRange(generateBox(new MagicaVoxelData { x = (byte)(trail.x - 6), y = (byte)(trail.y - 2), z = (byte)(trail.z + 2), color = 249 - 152 }, 4, 6, 2, 249 - 136));
                }
                else if (f == 3)
                {

                    missile[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 6), y = launcher.y, z = (byte)(launcher.z), color = 249 - 40 }, 249 - 160));
                    missile[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = launcher.y, z = (byte)(launcher.z), color = 249 - 40 }, 249 - 160));
                    missile[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(launcher.x + 2), y = launcher.y, z = (byte)(launcher.z), color = 249 - 40 }, 249 - 160));

                    extra[f].AddRange(generateBox(new MagicaVoxelData { x = (byte)(trail.x + 5), y = (byte)(trail.y - 1), z = (byte)(trail.z - 1), color = 249 - 152 }, 3, 4, 4, 249 - 152));
                    extra[f].AddRange(generateBox(new MagicaVoxelData { x = (byte)(trail.x + 6), y = (byte)(trail.y - 2), z = (byte)(trail.z + 0), color = 249 - 152 }, 2, 6, 2, 249 - 152));
                    extra[f].AddRange(generateBox(new MagicaVoxelData { x = (byte)(trail.x + 6), y = (byte)(trail.y + 0), z = (byte)(trail.z - 2), color = 249 - 152 }, 2, 2, 6, 249 - 152));

                    extra[f].AddRange(generateBox(new MagicaVoxelData { x = (byte)(trail.x - 8), y = (byte)(trail.y - 2), z = (byte)(trail.z + 0), color = 249 - 152 }, 6, 6, 4, 249 - 136));

                    extra[f] = extra[f].Where(v => r.Next(5) > 0).ToList();

                }
                else if (f == 4)
                {
                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 6), y = (byte)(trail.y + 2 + (r.Next(5) - 2)), z = (byte)(trail.z + 0), color = 249 - 160 }, 249 - 136));
                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 6), y = (byte)(trail.y - 2 + (r.Next(5) - 2)), z = (byte)(trail.z + 0), color = 249 - 160 }, 249 - 136));
                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 6), y = (byte)(trail.y + 2 + (r.Next(5) - 2)), z = (byte)(trail.z + 2), color = 249 - 160 }, 249 - 136));
                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 6), y = (byte)(trail.y - 2 + (r.Next(5) - 2)), z = (byte)(trail.z + 2), color = 249 - 160 }, 249 - 136));

                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 8), y = (byte)(trail.y + 2 + (r.Next(5) - 2)), z = (byte)(trail.z + 0), color = 249 - 160 }, 249 - 136));
                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 8), y = (byte)(trail.y - 2 + (r.Next(5) - 2)), z = (byte)(trail.z + 0), color = 249 - 160 }, 249 - 136));
                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 8), y = (byte)(trail.y + 2 + (r.Next(5) - 2)), z = (byte)(trail.z + 2), color = 249 - 160 }, 249 - 136));
                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 8), y = (byte)(trail.y - 2 + (r.Next(5) - 2)), z = (byte)(trail.z + 2), color = 249 - 160 }, 249 - 136));

                    extra[f] = extra[f].Where(v => r.Next(4) > 0).ToList();

                }
                else if (f == 5)
                {

                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 8), y = (byte)(trail.y + 4 + (r.Next(5) - 2)), z = (byte)(trail.z + 2), color = 249 - 160 }, 249 - 136));
                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 8), y = (byte)(trail.y - 4 + (r.Next(5) - 2)), z = (byte)(trail.z + 2), color = 249 - 160 }, 249 - 136));
                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 10), y = (byte)(trail.y + 4 + (r.Next(5) - 2)), z = (byte)(trail.z + 2), color = 249 - 160 }, 249 - 136));
                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 10), y = (byte)(trail.y - 4 + (r.Next(5) - 2)), z = (byte)(trail.z + 2), color = 249 - 160 }, 249 - 136));
                    extra[f] = extra[f].Where(v => r.Next(4) > 0).ToList();

                }
                else if (f == 6)
                {
                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 10), y = (byte)(trail.y + 4 + (r.Next(7) - 3)), z = (byte)(trail.z + 2), color = 249 - 160 }, 249 - 136));
                    extra[f].AddRange(generateFatVoxel(new MagicaVoxelData { x = (byte)(trail.x - 10), y = (byte)(trail.y - 4 + (r.Next(7) - 3)), z = (byte)(trail.z + 2), color = 249 - 160 }, 249 - 136));
                    extra[f] = extra[f].Where(v => r.Next(4) > 0).ToList();

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
        public static MagicaVoxelData[][] RocketReceiveAnimationDouble(MagicaVoxelData[][] parsedFrames, int distance)
        {
            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[parsedFrames.Length];
            MagicaVoxelData[][] finalFrames = new MagicaVoxelData[parsedFrames.Length][];

            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length];
            MagicaVoxelData[][] explosion = HugeExplosionDouble(randomFill(70, 48, 20, 8, 5, 5, new int[] { 249 - 136, 249 - 152, 249 - 160, 249 - 152, 249 - 160 }).ToArray(), -2, 7, 2);

            for (int f = 0; f < voxelFrames.Length; f++)
            {
                extra[f] = new List<MagicaVoxelData>(20);
            }
            for (int i = 0; i < 7; i++)
            {
                extra[3 + distance + i].AddRange(explosion[i]);
            }

            for (int f = 0; f < voxelFrames.Length; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(20);
                working.AddRange(extra[f]);
                finalFrames[f] = working.ToArray();
            }
            return finalFrames;
        }

        public static MagicaVoxelData[][] ArcMissileAnimationDouble(MagicaVoxelData[][] parsedFrames, int unit)
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
                if (f == 0)
                {
                    missile[f].AddRange(generateMissileDouble(launcher, 0));
                }
                if (f == 1)
                {
                    missile[f].AddRange(generateMissileDouble(new MagicaVoxelData { x = (byte)(launcher.x + 4), y = launcher.y, z = (byte)(launcher.z + 4), color = 249 - 40 }, 4));
                }
                else if (f == 2)
                {
                    missile[f].AddRange(generateMissileDouble(new MagicaVoxelData { x = (byte)(launcher.x + 12), y = launcher.y, z = (byte)(launcher.z + 12), color = 249 - 40 }, 12));
                }
                else if (f == 3)
                {
                    missile[f].AddRange(generateMissileFieryTrailDouble(new MagicaVoxelData { x = (byte)(launcher.x + 20), y = launcher.y, z = (byte)(launcher.z + 20), color = 249 - 40 }, 12));
                }
                else if (f == 4)
                {
                    missile[f].AddRange(generateMissileFieryTrailDouble(new MagicaVoxelData { x = (byte)(launcher.x + 28), y = launcher.y, z = (byte)(launcher.z + 28), color = 249 - 40 }, 12));

                    extra[f].AddRange(generateCube(new MagicaVoxelData { x = launcher.x, y = (byte)(launcher.y - 1), z = launcher.z, color = 249 - 136 }, 6, 249 - 136));

                    extra[f] = extra[f].Where(v => r.Next(5) == 0).ToList();


                }
                else if (f == 5)
                {
                    missile[f].AddRange(generateMissileFieryTrailDouble(new MagicaVoxelData { x = (byte)(launcher.x + 36), y = launcher.y, z = (byte)(launcher.z + 36), color = 249 - 40 }, 12));

                    extra[f].AddRange(generateCube(new MagicaVoxelData { x = launcher.x, y = (byte)(launcher.y - 1), z = launcher.z, color = 249 - 136 }, 6, 249 - 136));

                    extra[f] = extra[f].Where(v => r.Next(7) == 0).ToList();
                    //extra[f].AddRange(generateCone(new MagicaVoxelData { x = (byte)(launcher.x + 6), y = (byte)(launcher.y), z = (byte)(launcher.z + 6), color = 249 - 136 }, 4, 249 - 120));

                }
                else if (f == 6)
                {
                    missile[f].AddRange(generateMissileFieryTrailDouble(new MagicaVoxelData { x = (byte)(launcher.x + 44), y = launcher.y, z = (byte)(launcher.z + 44), color = 249 - 40 }, 12));

                    extra[f].AddRange(generateCube(new MagicaVoxelData { x = launcher.x, y = (byte)(launcher.y - 2), z = launcher.z, color = 249 - 136 }, 8, 249 - 136));

                    extra[f] = extra[f].Where(v => r.Next(20) == 0).ToList();

                }
                else if (f > 6)
                {
                    missile[f].AddRange(generateMissileFieryTrailDouble(new MagicaVoxelData { x = (byte)(launcher.x + 8 + f * 8), y = launcher.y, z = (byte)(launcher.z + 8 + f * 8), color = 249 - 40 }, 12));
                }
                if (f >= 4)
                {
                    extra[f].AddRange(generateConeDouble(new MagicaVoxelData { x = (byte)(launcher.x + 3 * (f - 3)), y = (byte)(launcher.y), z = (byte)(launcher.z + 3 * (f - 3)), color = 249 - 136 }, (f * 3) / 2, 249 - 136).
                        Where(v => r.Next(15) > f && r.Next(15) > f));
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
        public static MagicaVoxelData[][] ArcMissileReceiveAnimationDouble(MagicaVoxelData[][] parsedFrames, int strength)
        {

            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[parsedFrames.Length];
            MagicaVoxelData[][] finalFrames = new MagicaVoxelData[parsedFrames.Length][];

            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length], missile = new List<MagicaVoxelData>[voxelFrames.Length];
            MagicaVoxelData[][] explosion = new MagicaVoxelData[16][];

            MagicaVoxelData launcher = new MagicaVoxelData { x = 112, y = 58, z = 60 };
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

                extra[f] = new List<MagicaVoxelData>(160);
                missile[f] = new List<MagicaVoxelData>(160);

                if (f > 0)
                {
                    for (int i = 0; i < missile[f - 1].Count; i++)
                    {
                        if (missile[f - 1][i].x - 4 < 64)
                        {
                            isExploding = true;
                            explosion = HugeExplosionDouble(randomFill(50, 55, missile[f - 1][i].z, 8, 10, 7, new int[] { 249 - 136, 249 - 152, 249 - 160 }).Concat(missile[f - 1]).ToArray(), 0, 14 - f, 2);
                            missile[f].Clear();
                            firstBurst = f;
                            break;
                        }
                    }
                }
                if (f == 0 && !isExploding)
                {
                    missile[f].AddRange(generateDownwardMissileDouble(launcher, 0));

                    /*                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 2), y = (byte)(launcher.y), z = (byte)(launcher.z - 2), color = 249 - 40 });
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
                                        */

                }
                if (f == 1 && !isExploding)
                {
                    missile[f].AddRange(generateDownwardMissileDouble(new MagicaVoxelData { x = (byte)(launcher.x - 8), y = launcher.y, z = (byte)(launcher.z - 8), color = 249 - 40 }, 8));

                    /*
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
                    missile[f].Add(new MagicaVoxelData { x = (byte)(launcher.x - 1), y = (byte)(launcher.y + 1), z = (byte)(launcher.z - 1), color = 249 - 72 });*/




                }
                else if (f >= 2 && !isExploding)
                {
                    missile[f].AddRange(generateDownwardMissileFieryTrailDouble(new MagicaVoxelData { x = (byte)(launcher.x - 8 * f), y = launcher.y, z = (byte)(launcher.z - 8 * f), color = 249 - 40 }, 12));
                    /*
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
                     * */
                }
                if (f >= 4)
                {
                    extra[f].AddRange(generateDownwardConeDouble(new MagicaVoxelData { x = (byte)(launcher.x - 8 * (f - 3)), y = (byte)(launcher.y), z = (byte)(launcher.z - 8 * (f - 3)), color = 249 - 136 },
                        8, 249 - 136).Where(v => r.Next(95) > (f + 1) * (f + 1) && r.Next(95) > (f + 1) * f));
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

        public static MagicaVoxelData[][] BombAnimationDouble(MagicaVoxelData[][] parsedFrames, int unit)
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
                        double drop = f * (r.NextDouble() * 1.3 + 1.0);
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
                    if (f <= 1)
                    {
                        missiles[m][f].AddRange(generateCube(new MagicaVoxelData { x = (byte)(launcher.x + 0), y = (byte)(launcher.y), z = (byte)(launcher.z - 1), color = 249 - 40 }, 4, 249 - 40));
                    }

                    if (exploding[m] > -1)
                    {
                        if (exploding[m] == 0)
                        {
                            explosions[m] = HugeExplosionDouble(randomFill(centers[m].x - 3, centers[m].y - 3, 0, 10, 10, 8, new int[] { 249 - 136, 249 - 152, 249 - 160 }).ToArray(), 0, voxelFrames.Length - 2 - f, 4);
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

        public static MagicaVoxelData[][] BombReceiveAnimationDouble(MagicaVoxelData[][] parsedFrames, int strength)
        {
            List<MagicaVoxelData>[] voxelFrames = new List<MagicaVoxelData>[parsedFrames.Length];
            MagicaVoxelData[][] finalFrames = new MagicaVoxelData[parsedFrames.Length][];

            List<MagicaVoxelData>[] extra = new List<MagicaVoxelData>[voxelFrames.Length];
            MagicaVoxelData[][][] explosions = new MagicaVoxelData[strength][][];
            for (int s = 0; s < strength; s++)
            {
                explosions[s] = HugeExplosionDouble(randomFill(84, 50 + r.Next(11), r.Next(5), 8, 6, 6, new int[] { 249 - 152, 249 - 160, 249 - 136 }).ToArray(), -3, 9, 2);
            }
            for (int f = 0; f < voxelFrames.Length; f++)
            {
                extra[f] = new List<MagicaVoxelData>(800);
            }
            for (int i = 0; i < 9; i++)
            {
                for (int sp = 0; sp < explosions.Length; sp++)
                    extra[5 + i].AddRange(explosions[sp][i]);
            }

            for (int f = 0; f < voxelFrames.Length; f++)
            {
                List<MagicaVoxelData> working = new List<MagicaVoxelData>(800);
                working.AddRange(extra[f]);
                finalFrames[f] = working.ToArray();
            }
            return finalFrames;
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

        public static AnimationGenerator[] weaponAnimationsDouble = { HandgunAnimationDouble, MachineGunAnimationDouble, MachineGunAnimationDouble, CannonAnimationDouble,
                                                                       LongCannonAnimationDouble, RocketAnimationDouble, ArcMissileAnimationDouble, BombAnimationDouble };
        private static AnimationGenerator[] receiveAnimations = { HandgunReceiveAnimationDouble, MachineGunReceiveAnimationDouble, MachineGunReceiveAnimationDouble,
                                                                    CannonReceiveAnimationDouble, LongCannonReceiveAnimationDouble,
                                                                    RocketReceiveAnimationDouble, 
                                                                    ArcMissileReceiveAnimationDouble, 
                                                                    BombReceiveAnimationDouble
                                                                };


        public static MagicaVoxelData[][] makeFiringAnimationDouble(MagicaVoxelData[] parsed, int unit, int weapon)
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
                voxelFrames[0][i].x += 40;
                voxelFrames[0][i].y += 40;
            }

            if (CurrentWeapons[unit][weapon] == -1)
            {
                return new MagicaVoxelData[0][];
            }
            else
            {
                voxelFrames = weaponAnimationsDouble[CurrentWeapons[unit][weapon]](voxelFrames, unit);
            }
            sizex = 120;
            sizey = 120;
            sizez = 120;
            for (int f = 0; f < parsedFrames.Length; f++)
            {
                voxelFrames[f] = PlaceShadows(voxelFrames[f].ToList()).ToArray();
            }

            return voxelFrames;
        }

        public static MagicaVoxelData[][] makeReceiveAnimationDouble(int weaponType, int strength)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[16][];
            //voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];

            voxelFrames = receiveAnimations[weaponType](voxelFrames, strength);

            for (int f = 0; f < voxelFrames.Length; f++)
            {
                sizex = 120;
                sizey = 120;
                voxelFrames[f] = PlaceShadows(voxelFrames[f].ToList()).ToArray();
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
                voxelFrames[0][i].x += 40;
                voxelFrames[0][i].y += 40;
            }
            for (int f = 1; f <= 8; f++)
            {
                voxelFrames[f] = new MagicaVoxelData[voxelFrames[f - 1].Length]; //.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)
                voxelFrames[f - 1].CopyTo(voxelFrames[f], 0);

                for (int i = 0; i < voxelFrames[f].Length; i++)
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
            for (int f = 1; f <= 8; f++)
            {

                List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxelFrames[f].Length);
                int[,] taken = new int[120, 120];
                taken.Fill(-1);

                int minX;
                int maxX;
                float midX;

                minX = voxelFrames[f].Min(v => v.x * ((v.color == 249 - 80 || v.color == 249 - 104 || v.color == 249 - 112 ||
                    v.color == 249 - 96 || v.color == 249 - 128 || v.color == 249 - 136 || v.color == 249 - 152 ||
                    v.color == 249 - 160 && v.color > 249 - 168) ? 100 : 1));
                maxX = voxelFrames[f].Max(v => v.x * ((v.color == 249 - 80 || v.color == 249 - 104 || v.color == 249 - 112 ||
                    v.color == 249 - 96 || v.color == 249 - 128 || v.color == 249 - 136 || v.color == 249 - 152 ||
                    v.color == 249 - 160 && v.color > 249 - 168) ? 0 : 1));
                midX = (maxX + minX) / 2F;

                int minY;
                int maxY;
                float midY;
                minY = voxelFrames[f].Min(v => v.y * ((v.color == 249 - 80 || v.color == 249 - 104 || v.color == 249 - 112 ||
                    v.color == 249 - 96 || v.color == 249 - 128 || v.color == 249 - 136 || v.color == 249 - 152 ||
                    v.color == 249 - 160 && v.color > 249 - 168) ? 100 : 1));
                maxY = voxelFrames[f].Max(v => v.y * ((v.color == 249 - 80 || v.color == 249 - 104 || v.color == 249 - 112 ||
                    v.color == 249 - 96 || v.color == 249 - 128 || v.color == 249 - 136 || v.color == 249 - 152 ||
                    v.color == 249 - 160 && v.color > 249 - 168) ? 0 : 1));
                midY = (maxY + minY) / 2F;

                for (int i = 0; i < voxelFrames[f].Length; i++)
                {
                    // do not store this voxel if it lies out of range of the voxel chunk (120x120x120)
                    if (voxelFrames[f][i].x >= 120 || voxelFrames[f][i].y >= 120 || voxelFrames[f][i].z >= 120)
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
                        else
                        {
                            if (voxelFrames[f][i].x > midX)
                            {
                                vox.x = (byte)(voxelFrames[f][i].x - f * (r.NextDouble() + 0.7));
                                if (vox.x < midX)
                                    vox.color = 249 - 168;
                            }
                            else
                            {
                                vox.x = (byte)(voxelFrames[f][i].x + f * (r.NextDouble() + 0.7));
                                if (vox.x > midX)
                                    vox.color = 249 - 168;
                            }
                            if (voxelFrames[f][i].y > midY)
                            {
                                vox.y = (byte)(voxelFrames[f][i].y - f * (r.NextDouble() + 0.7));
                                if (vox.y < midY)
                                    vox.color = 249 - 168;
                            }
                            else
                            {
                                vox.y = (byte)(voxelFrames[f][i].y + f * (r.NextDouble() + 0.7));
                                if (vox.y > midY)
                                    vox.color = 249 - 168;
                            }
                            vox.z = (byte)(0);
                        }
                        taken[vox.x, vox.y] = altered.Count();
                        altered.Add(vox);
                    }
                }
                voxelFrames[f] = altered.ToArray();
            }

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
    }
}
