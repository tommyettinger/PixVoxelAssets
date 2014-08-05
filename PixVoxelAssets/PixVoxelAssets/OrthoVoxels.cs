using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace AssetsCU
{

    public class OrthoVoxels
    {

        public enum MovementType
        {
            Foot, Treads, TreadsAmphi, Wheels, WheelsTraverse, Flight, Immobile
        }
        public static StringBuilder log = new StringBuilder();
        public static string[] Terrains = new string[]
        {"Plains","Forest","Desert","Jungle","Hills"
        ,"Mountains","Ruins","Tundra","Road","River", "Basement"};
        public static string[] Directions = { "SE", "SW", "NW", "NE" };
        //foot 0-0, treads 1-5, wheels 6-8, flight 9-10
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

        /*{ "Infantry", //foot 0 0
                               "Tank", "Artillery", "Artillery_P", "Artillery_S", "Supply_P", //treads 1 5
                               "Artillery_T", "Supply", "Supply_T", //wheels 6 8
                               "Helicopter", "Plane", //flight 9 10
                               "City", "Factory", "Castle", "Capital" //facility
                             };*/
        public static string[] final_units =
        {
            "Infantry",
"Bazooka",
"Bike",
"Sniper",
"Light Artillery",
"Defensive Artillery",
"AA Artillery",
"Stealth Artillery",
"Light Tank",
"Heavy Tank",
"AA Tank",
"Recon Tank",
"Prop Plane",
"Ground Bomber",
"Fighter Jet",
"Stealth Bomber",
"Supply Truck",
"Rig",
"Amphi Transport",
"Jammer",
"Transport Copter",
"Gunship Copter",
"Blitz Copter",
"Comm Copter",
};

        public class UnitInfo
        {
            public int unit;
            public string name;
            public int color;
            public int facing;

            public int speed;
            public MovementType mobility;
            public int x;
            public int y;
            public UnitInfo()
            {
                unit = 0;
                name = "Infantry";
                speed = 3;
                mobility = MovementType.Foot;
                color = 1;
                facing = 0;
                x = 3;
                y = 3;
            }
            public UnitInfo(string name, int color, int facing, int x, int y)
            {
                this.name = name;
                this.x = x;
                this.y = y;
                //                this.unit = index_matches[unit];
                this.unit = UnitLookup[name];
                this.color = color;
                this.facing = facing;
                this.speed = CurrentSpeeds[this.unit];
                this.mobility = CurrentMobilities[this.unit];
            }
            public UnitInfo(int unit, int color, int facing, int x, int y)
            {
                this.name = CurrentUnits[unit];
                this.x = x;
                this.y = y;
                //                this.unit = index_matches[unit];
                this.unit = unit;
                this.color = color;
                this.facing = facing;
                this.speed = CurrentSpeeds[this.unit];
                this.mobility = CurrentMobilities[this.unit];
            }
        }
        private static Random r = new Random(PlusPaletteDraw.r.Next());
        private static float flat_alpha = 0.9991F;
        private static float spin_alpha_0 = 0.9993F;
        private static float spin_alpha_1 = 0.9995F;
        public static float[][] colors = new float[][]
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
            new float[] {0.7F,1.2F,-0.1F,1F},      //white
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
            
            //136 smoke (FLAT ALPHA)
            new float[] {0.12F,0.08F,-0.01F,flat_alpha},
            new float[] {0.01F,0.12F,0.16F,flat_alpha},
            new float[] {0.14F,0.14F,0.02F,flat_alpha},
            new float[] {0.14F,0.14F,0.02F,flat_alpha},
            new float[] {0.14F,0.14F,0.02F,flat_alpha},
            new float[] {0.14F,0.14F,0.02F,flat_alpha},
            new float[] {0.14F,0.14F,0.02F,flat_alpha},
            new float[] {0.14F,0.14F,0.02F,flat_alpha},

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
        public static float[][] colours = new float[256][];
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
        private static int sizex = 0, sizey = 0, sizez = 0;
        /// <summary>
        /// Load a MagicaVoxel .vox format file into a MagicaVoxelData[] that we use for voxel chunks.
        /// </summary>
        /// <param name="stream">An open BinaryReader stream that is the .vox file.</param>
        /// <returns>The voxel chunk data for the MagicaVoxel .vox file.</returns>
        private static MagicaVoxelData[] FromMagica(BinaryReader stream)
        {
            // check out http://voxel.codeplex.com/wikipage?title=VOX%20Format&referringTitle=Home for the file format used below
            // we're going to return a voxel chunk worth of data
            ushort[] data = new ushort[32 * 128 * 32];

            List<MagicaVoxelData> voxelData = new List<MagicaVoxelData>(), voxelsAltered = new List<MagicaVoxelData>();
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
                for (int i = 0; i < voxelData.Count; i++)
                {
                    /*
                    // do not store this voxel if it lies out of range of the voxel chunk (32x128x32)
                    if (subsample)
                        if (voxelData[i].x > 63 || voxelData[i].y > 63 || voxelData[i].z > 127) continue;
                    else
                        if (voxelData[i].x > 31 || voxelData[i].y > 31 || voxelData[i].z > 127) continue;
                    */
                    voxelsAltered.Add(voxelData[i]);
                    if (-1 == taken[voxelData[i].x, voxelData[i].y] && voxelData[i].color != 249 - 80 && voxelData[i].color != 249 - 104 && voxelData[i].color != 249 - 112
                         && voxelData[i].color != 249 - 96 && voxelData[i].color != 249 - 128 && voxelData[i].color != 249 - 136 && voxelData[i].color != 249 - 152 && voxelData[i].color != 249 - 160)
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

            return voxelsAltered.ToArray();
        }

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
        public static void Madden()
        {
            for (int i = 0; i < 256; i++)
            {
                colours[i] = colors[i];
            }

            for (int i = 1; i < 23; i++)
            {
                float alpha = 1F;
                switch (i)
                {
                    case 22: alpha = 0F; break;
                    case 18: alpha = flat_alpha; break;
                    case 17: alpha = flat_alpha; break;
                    case 13: alpha = flat_alpha; break;
                    case 14: alpha = spin_alpha_0; break;
                    case 15: alpha = spin_alpha_1; break;
                }
                if (i == 22)
                {

                    colors[(i - 1) * 8 + 0] = new float[] { 1F, 1F, 1F, 0F };
                    colors[(i - 1) * 8 + 1] = new float[] { 1F, 1F, 1F, 0F };
                    colors[(i - 1) * 8 + 2] = new float[] { 1F, 1F, 1F, 0F };
                    colors[(i - 1) * 8 + 3] = new float[] { 1F, 1F, 1F, 0F };
                    colors[(i - 1) * 8 + 4] = new float[] { 1F, 1F, 1F, 0F };
                    colors[(i - 1) * 8 + 5] = new float[] { 1F, 1F, 1F, 0F };
                    colors[(i - 1) * 8 + 6] = new float[] { 1F, 1F, 1F, 0F };
                    colors[(i - 1) * 8 + 7] = new float[] { 1F, 1F, 1F, 0F };
                    colors[(i - 1) * 8 + 8] = new float[] { 1F, 1F, 1F, 0F };
                }
                else if (i == 11)
                {
                    colors[(i - 1) * 8 + 0] = new float[] { 1.0F, 0.5F, 1.0F, 1F };
                    colors[(i - 1) * 8 + 1] = new float[] { 1.0F, 0.5F, 1.0F, 1F };
                    colors[(i - 1) * 8 + 2] = new float[] { 1.0F, 0.5F, 1.0F, 1F };
                    colors[(i - 1) * 8 + 3] = new float[] { 1.0F, 0.5F, 1.0F, 1F };
                    colors[(i - 1) * 8 + 4] = new float[] { 1.0F, 0.5F, 1.0F, 1F };
                    colors[(i - 1) * 8 + 5] = new float[] { 1.0F, 0.5F, 1.0F, 1F };
                    colors[(i - 1) * 8 + 6] = new float[] { 1.0F, 0.5F, 1.0F, 1F };
                    colors[(i - 1) * 8 + 7] = new float[] { 1.0F, 0.5F, 1.0F, 1F };
                }
                else
                {
                    colors[(i - 1) * 8 + 0] = new float[] { 1 / i, 1F / i * 1.5F, 0.9F / (24 - i), alpha };
                    colors[(i - 1) * 8 + 1] = new float[] { 1 / i, 1F / i * 1.5F, 0.9F / (24 - i), alpha };
                    colors[(i - 1) * 8 + 2] = new float[] { 1 / i, 1F / i * 1.5F, 0.9F / (24 - i), alpha };
                    colors[(i - 1) * 8 + 3] = new float[] { 1 / i, 1F / i * 1.5F, 0.9F / (24 - i), alpha };
                    colors[(i - 1) * 8 + 4] = new float[] { 1 / i, 1F / i * 1.5F, 0.9F / (24 - i), alpha };
                    colors[(i - 1) * 8 + 5] = new float[] { 1 / i, 1F / i * 1.5F, 0.9F / (24 - i), alpha };
                    colors[(i - 1) * 8 + 6] = new float[] { 1 / i, 1F / i * 1.5F, 0.9F / (24 - i), alpha };
                    colors[(i - 1) * 8 + 7] = new float[] { 1 / i, 1F / i * 1.5F, 0.9F / (24 - i), alpha };
                }
            }
            for (int i = 177; i < 256; i++)
            {
                colors[i] = new float[] { 0, 0, 0, 0 };
            }
        }
        public static void Awaken()
        {
            for (int i = 0; i < 256; i++)
            {
                colors[i] = colours[i];
            }
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

        private static MagicaVoxelData[] FromMagicaRaw(BinaryReader stream)
        {
            // check out http://voxel.codeplex.com/wikipage?title=VOX%20Format&referringTitle=Home for the file format used below
            // we're going to return a voxel chunk worth of data
            ushort[] data = new ushort[32 * 128 * 32];

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

                if (voxelData.Count == 0) return voxelData.ToArray(); // failed to read any valid voxel data

                // now push the voxel data into our voxel chunk structure
                for (int i = 0; i < voxelData.Count; i++)
                {
                    // do not store this voxel if it lies out of range of the voxel chunk (32x128x32)
                    if (voxelData[i].x > 31 || voxelData[i].y > 31 || voxelData[i].z > 127) continue;

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

            return voxelsAltered.ToArray();
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
                for (int y = 0; y <= x; y++)
                {
                    for (int z = 0; z <= segments; z++)
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
            return cone;

        }
        private static List<MagicaVoxelData> generateDownwardCone(MagicaVoxelData start, int segments, int color)
        {
            List<MagicaVoxelData> cone = new List<MagicaVoxelData>(40);
            for (int x = 0; x < segments; x++)
            {
                for (int y = 0; y <= x; y++)
                {
                    for (int z = 0; z <= segments; z++)
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

        public static MagicaVoxelData[][] makeFiringAnimation(MagicaVoxelData[][] parsedFrames, int unit, int weapon)
        {
            MagicaVoxelData[][] voxelFrames = new MagicaVoxelData[parsedFrames.Length][];
            //voxelFrames[0] = new MagicaVoxelData[parsedFrames[0].Length];
            parsedFrames.CopyTo(voxelFrames, 0);
            /*for (int i = 0; i < parsedFrames[0].Length; i++)
            {
                voxelFrames[0][i].x += 5;
                voxelFrames[0][i].y += 5;
            }*/
            Console.WriteLine("X: " + voxelFrames[0].Min(mvd => mvd.x) + ", Y: " + voxelFrames[0].Min(mvd => mvd.y));
            if (CurrentWeapons[unit][weapon] == -1)
            {
                return new MagicaVoxelData[0][];
            }
            else
            {
                voxelFrames = weaponAnimations[CurrentWeapons[unit][weapon]](voxelFrames, unit);
            }
            for (int f = 0; f < parsedFrames.Length; f++)
            {

                List<MagicaVoxelData> altered = new List<MagicaVoxelData>(voxelFrames[f].Length);
                int[,] taken = new int[30, 30];
                taken.Fill(-1);
                for (int i = 0; i < voxelFrames[f].Length; i++)
                {
                    // do not store this voxel if it lies out of range of the voxel chunk (60x60x40)
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

            if (CurrentWeapons[unit][weapon] == -1)
            {
                return new MagicaVoxelData[0][];
            }
            else
            {
                voxelFrames = weaponAnimations[CurrentWeapons[unit][weapon]](voxelFrames, unit);
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

        private static Bitmap drawPixelsS(MagicaVoxelData[] voxels, int idx)
        {
            Bitmap b = new Bitmap(40, 60, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            Image image = new Bitmap("cube_ortho.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 2;
            int height = 3;

            float[][] colorMatrixElements = { 
   new float[] {1F,  0,  0,  0, 0},
   new float[] {0,  1F,  0,  0, 0},
   new float[] {0,  0,  1F,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            foreach (MagicaVoxelData vx in voxels.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;
                if (current_color > 112)
                    current_color = 24;
                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+colors[current_color + idx][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color + idx][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color + idx][2],  0, 0},
   new float[] {0,  0,  0,  colors[current_color + idx][3], 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(
                   image,
                   new Rectangle(vx.y * 2, 60 - 20 - 3 + vx.x - vx.z * 2, width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }

        private static Bitmap[] render(MagicaVoxelData[] voxels, int facing, int faction, int frame, int maxFrames)
        {   
            Bitmap[] b = {
            new Bitmap(44, 64, PixelFormat.Format32bppArgb),
            new Bitmap(44, 64, PixelFormat.Format32bppArgb),};

            Graphics g = Graphics.FromImage((Image)b[0]);
            Graphics gsh = Graphics.FromImage((Image)b[1]);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_ortho.png");
            Image flat = new Bitmap("flat_ortho.png");
            Image spin = new Bitmap("spin_ortho.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 2;
            int height = 3;
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
                case 0: //South
                    vls = voxels;
                    break;
                case 1: //West
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempY) + (ySize / 2));
                        vls[i].y = (byte)((tempX * -1) + (xSize / 2) - 1); // - 1
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;
                case 2: //North
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempX * -1) + (xSize / 2) - 1); // - 1
                        vls[i].y = (byte)((tempY * -1) + (ySize / 2) - 1); // - 1
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;
                case 3: //East
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempY * -1) + (ySize / 2) - 1); // - 1
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
                if ((frame % 2 != 0) && colors[current_color + faction][3] == spin_alpha_0)
                    continue;
                else if ((frame % 2 != 1) && colors[current_color + faction][3] == spin_alpha_1)
                    continue;
                else if (current_color == 168)
                    continue;
                else if (current_color == 96)
                {

                    colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+colors[current_color + faction][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color + faction][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color + faction][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    //vx.y * 2, 60 - 20 - 3 + vx.x - vx.z * 2
                    gsh.DrawImage(flat,
                       new Rectangle(vx.y * 2 + 2, 62 - 20 - 3 + vx.x - vx.z * 2
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
   new float[] {0.22F+colors[current_color + faction][0] + lightCalc,  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color + faction][1] + lightCalc,  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color + faction][2] + lightCalc,  0, 0},
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
   new float[] {0.22F+colors[current_color + faction][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color + faction][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color + faction][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                    }
                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    g.DrawImage((current_color == 80) ? spin :
                       (colors[current_color + faction][3] == 1F) ? image : (colors[current_color + faction][3] == flat_alpha) ? flat : spin,
                       new Rectangle(vx.y * 2 + 2 + ((current_color == 136) ? jitter - 1 : 0),
                           62 - 20 - 3 + vx.x - vx.z * 2 - ((colors[current_color + faction][3] == flat_alpha) ? -2 : jitter),
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
                if ((frame % 2 != 0) && colors[current_color + faction][3] == spin_alpha_0)
                    continue;
                else if ((frame % 2 != 1) && colors[current_color + faction][3] == spin_alpha_1)
                    continue;
                else if (current_color == 168)
                    continue;
                else if (current_color == 96)
                {

                    colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+colors[current_color + faction][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color + faction][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color + faction][2],  0, 0},
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
   new float[] {0.22F+colors[current_color + faction][0] + lightCalc,  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color + faction][1] + lightCalc,  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color + faction][2] + lightCalc,  0, 0},
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
   new float[] {0.22F+colors[current_color + faction][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color + faction][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color + faction][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                    }
                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    g.DrawImage((current_color == 80) ? spin :
                       (colors[current_color + faction][3] == 1F) ? image : (colors[current_color + faction][3] == flat_alpha) ? flat : spin,
                       new Rectangle((vx.x + vx.y) * 2 + 4 * 2 + ((current_color == 136) ? jitter - 1 * 2 : 0),
                           100 * 2 - 20 * 2 - vx.y + vx.x - vx.z * 3 - ((colors[current_color + faction][3] == flat_alpha) ? -2 * 2 : jitter),
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
            Bitmap b = new Bitmap(44, 64, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            Image image = new Bitmap("black_outline_ortho.png");
            //            Image flat = new Bitmap("flat_soft.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 5;

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
                case 0: //South
                    vls = voxels;
                    break;
                case 1: //West
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempY) + (ySize / 2));
                        vls[i].y = (byte)((tempX * -1) + (xSize / 2) - 1); // - 1
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;
                case 2: //North
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempX * -1) + (xSize / 2) - 1); // - 1
                        vls[i].y = (byte)((tempY * -1) + (ySize / 2) - 1); // - 1
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;
                case 3: //East
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempY * -1) + (ySize / 2) - 1); // - 1
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

                if ((frame % 2 != 0) && colors[current_color + faction][3] == spin_alpha_0)
                    continue;
                else if ((frame % 2 != 1) && colors[current_color + faction][3] == spin_alpha_1)
                    continue;
                else if (current_color == 120 || current_color == 152 || current_color == 160 || current_color == 136 || current_color >= 168)
                    continue;
                if ((colors[current_color + faction][3] != flat_alpha))
                {
                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    g.DrawImage(image, //((colors[current_color + faction][3] == flat_alpha) ? flat : image)
                        //vx.y * 2 + 1, 66 - 3 - 20 + vx.x - vx.z*2
                    new Rectangle(vx.y * 2 + 1, //if flat use + 4
                        64 - 20 - 3 - 3 + vx.x - vx.z * 2 - jitter, //((colors[current_color + faction][3] == flat_alpha) ? -4 : jitter
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

                if ((frame % 2 != 0) && colors[current_color + faction][3] == spin_alpha_0)
                    continue;
                else if ((frame % 2 != 1) && colors[current_color + faction][3] == spin_alpha_1)
                    continue;
                else if (current_color == 120 || current_color == 152 || current_color == 160 || current_color == 136 || current_color >= 168)
                    continue;
                if ((colors[current_color + faction][3] != flat_alpha))
                {
                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    g.DrawImage(image, //((colors[current_color + faction][3] == flat_alpha) ? flat : image)
                    new Rectangle((vx.x + vx.y) * 2 + 4 * 2, //if flat use + 4
                        100 * 2 - 20 * 2 - 2 - vx.y + vx.x - vx.z * 3 - jitter, //((colors[current_color + faction][3] == flat_alpha) ? -4 : jitter
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


        private static Bitmap[] renderLarge(MagicaVoxelData[] voxels, int facing, int faction, int frame)
        {
            Bitmap[] b = {
            new Bitmap(124, 184, PixelFormat.Format32bppArgb),
            new Bitmap(124, 184, PixelFormat.Format32bppArgb),};

            Graphics g = Graphics.FromImage((Image)b[0]);
            Graphics gsh = Graphics.FromImage((Image)b[1]);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_ortho.png");
            Image flat = new Bitmap("flat_ortho.png");
            Image spin = new Bitmap("spin_ortho.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 2;
            int height = 3;
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
                case 0: //South
                    vls = voxels;
                    break;
                case 1: //West
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempY) + (ySize / 2));
                        vls[i].y = (byte)((tempX * -1) + (xSize / 2) - 1); // - 1
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;
                case 2: //North
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempX * -1) + (xSize / 2) - 1); // - 1
                        vls[i].y = (byte)((tempY * -1) + (ySize / 2) - 1); // - 1
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;
                case 3: //East
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempY * -1) + (ySize / 2) - 1); // - 1
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
                if ((frame % 2 != 0) && colors[current_color + faction][3] == spin_alpha_0)
                    continue;
                else if ((frame % 2 != 1) && colors[current_color + faction][3] == spin_alpha_1)
                    continue;
                else if (current_color >= 168)
                {
                    //Console.Write(frame);
                    continue;
                }
                else if (current_color == 96)
                {

                    colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+colors[current_color + faction][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color + faction][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color + faction][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    gsh.DrawImage(flat, //vx.y * 2, 60 - 20 - 3 + vx.x - vx.z * 2
                       new Rectangle(vx.y * 2 + 2, 182 - 60 - 3 + vx.x - vx.z * 2
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
   new float[] {0.22F+colors[current_color + faction][0] + lightCalc,  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color + faction][1] + lightCalc,  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color + faction][2] + lightCalc,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                    }
                    else
                    {
                        colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+colors[current_color + faction][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color + faction][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color + faction][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                    }
                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    g.DrawImage((current_color == 80) ? spin :
                       (colors[current_color + faction][3] == 1F) ? image : (colors[current_color + faction][3] == flat_alpha) ? flat : spin,
                       new Rectangle(
                           vx.y * 2 + 2 + ((current_color == 136) ? jitter - 1 : 0),
                           182 - 60 - 3 + vx.x - vx.z * 2 - ((colors[current_color + faction][3] == flat_alpha) ? -2 : 0),
                           //(vx.x + vx.y) * 2 + 4 + ((current_color == 136) ? jitter - 1 : 0),
                           //300 - 60 - vx.y + vx.x - vx.z * 3, // - ((colors[current_color + faction][3] == flat_alpha) ? -2 : 0),
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
            Bitmap b = new Bitmap(124, 184, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            Image image = new Bitmap("black_outline_ortho.png");
            //            Image flat = new Bitmap("flat_soft.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 5;

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

                if ((frame % 2 != 0) && colors[current_color + faction][3] == spin_alpha_0)
                    continue;
                else if ((frame % 2 != 1) && colors[current_color + faction][3] == spin_alpha_1)
                    continue;
                else if (current_color == 120 || current_color == 152 || current_color == 160 || current_color == 136 || current_color >= 168)
                    continue;
                if (colors[current_color + faction][3] != flat_alpha && current_color != 120)
                {
                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    g.DrawImage(image, //((colors[current_color + faction][3] == flat_alpha) ? flat : image)
                    new Rectangle(vx.y * 2 + 1, //if flat use + 4
                        184 - 60 - 3 - 3 + vx.x - vx.z * 2,// - jitter, //((colors[current_color + faction][3] == flat_alpha) ? -4 : jitter
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


        private static Bitmap renderOnlyTerrainColors()
        {
            Bitmap b = new Bitmap(128, 4, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            Image image = new Bitmap("cube_soft.png");
            //            Image gray = new Bitmap("cube_gray_soft.png");
            //Image reversed = new Bitmap("cube_reversed.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 4;
            float[][] flatcolors = PlusPaletteDraw.flatcolors;
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
        private static Bitmap renderOnlyTerrainColors(int faction)
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
               new float[] {0.55F*(0.22F+colors[32 + faction][0]), 0.55F*(0.251F+colors[32 + faction][1]), 0.55F*(0.31F+colors[32 + faction][2]), 0, 1F}}),
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
        private static Bitmap renderOnlyColors(int faction)
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
   new float[] {0.22F+colors[current_color + faction][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color + faction][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color + faction][2],  0, 0},
   new float[] {0,  0,  0, colors[current_color + faction][3], 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);
                g.DrawImage(
                   (colors[current_color + faction][3] == 1F) ? image : (colors[current_color + faction][3] == flat_alpha) ? flat : spin,
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
   new float[] {0.22F+colors[80 + faction][0] + lightCalcs[i],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[80 + faction][1] + lightCalcs[i],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[80 + faction][2] + lightCalcs[i],  0, 0},
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
   new float[] {0.22F+colors[current_color + faction][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color + faction][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color + faction][2],  0, 0},
   new float[] {0,  0,  0, colors[current_color + faction][3], 0},
   new float[] {0, 0, 0, 0, 1F}});
                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);
                g.DrawImage(
                   (colors[current_color + faction][3] == 1F) ? image : (colors[current_color + faction][3] == flat_alpha) ? flat : spin,
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



        private static Bitmap drawPixelsSE(MagicaVoxelData[] voxels, int faction, int frame)
        {
            Bitmap b = new Bitmap(80, 100, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_soft.png");
            Image flat = new Bitmap("flat_soft.png");
            Image spin = new Bitmap("spin_soft.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 4;

            float[][] colorMatrixElements = { 
   new float[] {1F,  0,  0,  0, 0},
   new float[] {0,  1F,  0,  0, 0},
   new float[] {0,  0,  1F,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            foreach (MagicaVoxelData vx in voxels.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;

                if (frame != 0 && colors[current_color + faction][3] == spin_alpha_0)
                    continue;
                else if (frame != 1 && colors[current_color + faction][3] == spin_alpha_1)
                    continue;
                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+colors[current_color + faction][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color + faction][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color + faction][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(
                   (colors[current_color + faction][3] == 1F) ? image : (colors[current_color + faction][3] == flat_alpha) ? flat : spin,
                   new Rectangle((vx.x + vx.y) * 2, 100 - 24 - vx.y + vx.x - vx.z * 3 - ((colors[current_color + faction][3] == flat_alpha) ? -2 : frame * 2)
                       , width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }

        private static Bitmap drawPixelsSW(MagicaVoxelData[] voxels, int faction, int frame)
        {
            Bitmap b = new Bitmap(80, 100, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube_large.png");
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
            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            for (int i = 0; i < voxels.Length; i++)
            {
                byte tempX = (byte)(voxels[i].x - 10);
                byte tempY = (byte)(voxels[i].y - 10);
                vls[i].x = (byte)((tempY) + 10);
                vls[i].y = (byte)((tempX * -1) + 10 - 1);
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;

            }
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;

                if (frame != 0 && colors[current_color + faction][3] == spin_alpha_0)
                    continue;
                else if (frame != 1 && colors[current_color + faction][3] == spin_alpha_1)
                    continue;
                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+colors[current_color + faction][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color + faction][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color + faction][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(
                   (colors[current_color + faction][3] == 1F) ? image : (colors[current_color + faction][3] == flat_alpha) ? flat : spin,
                   new Rectangle((vx.x + vx.y) * 2, 100 - 24 - vx.y + vx.x - vx.z * 3 - ((colors[current_color + faction][3] == flat_alpha) ? -2 : frame * 2)
                       , width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }

        private static Bitmap drawPixelsNE(MagicaVoxelData[] voxels, int faction, int frame)
        {
            Bitmap b = new Bitmap(80, 100, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube_large.png");
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
            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            for (int i = 0; i < voxels.Length; i++)
            {
                byte tempX = (byte)(voxels[i].x - 10);
                byte tempY = (byte)(voxels[i].y - 10);
                vls[i].x = (byte)((tempY * -1) + 10 - 1);
                vls[i].y = (byte)(tempX + 10);
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;

            }
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;

                if (frame != 0 && colors[current_color + faction][3] == spin_alpha_0)
                    continue;
                else if (frame != 1 && colors[current_color + faction][3] == spin_alpha_1)
                    continue;
                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+colors[current_color + faction][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color + faction][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color + faction][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(
                   (colors[current_color + faction][3] == 1F) ? image : (colors[current_color + faction][3] == flat_alpha) ? flat : spin,
                   new Rectangle((vx.x + vx.y) * 2, 100 - 24 - vx.y + vx.x - vx.z * 3 - ((colors[current_color + faction][3] == flat_alpha) ? -2 : frame * 2)
                       , width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }

        private static Bitmap drawPixelsNW(MagicaVoxelData[] voxels, int faction, int frame)
        {
            Bitmap b = new Bitmap(80, 100, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube_large.png");
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
            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            for (int i = 0; i < voxels.Length; i++)
            {
                byte tempX = (byte)(voxels[i].x - 10);
                byte tempY = (byte)(voxels[i].y - 10);
                vls[i].x = (byte)((tempX * -1) + 10 - 1);
                vls[i].y = (byte)((tempY * -1) + 10 - 1);
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;

            }
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;

                if (frame != 0 && colors[current_color + faction][3] == spin_alpha_0)
                    continue;
                else if (frame != 1 && colors[current_color + faction][3] == spin_alpha_1)
                    continue;
                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+colors[current_color + faction][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color + faction][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color + faction][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(
                   (colors[current_color + faction][3] == 1F) ? image : (colors[current_color + faction][3] == flat_alpha) ? flat : spin,
                   new Rectangle((vx.x + vx.y) * 2, 100 - 24 - vx.y + vx.x - vx.z * 3 - ((colors[current_color + faction][3] == flat_alpha) ? -2 : frame * 2)
                       , width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }







        private static Bitmap drawOutlineSE(MagicaVoxelData[] voxels, int faction, int frame)
        {
            Bitmap b = new Bitmap(88, 108, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            Image image = new Bitmap("black_outline_soft.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 8;
            int height = 8;

            float[][] colorMatrixElements = { 
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            foreach (MagicaVoxelData vx in voxels.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;
                if (frame != 0 && colors[current_color + faction][3] == spin_alpha_0)
                    continue;
                else if (frame != 1 && colors[current_color + faction][3] == spin_alpha_1)
                    continue;
                if (colors[current_color + faction][3] != flat_alpha)
                {
                    /*colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+colors[56 + idx][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[56 + idx][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[56 + idx][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});*/

                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    g.DrawImage(
                       image,
                       new Rectangle((vx.x + vx.y) * 2 + 2, 100 - 20 - 2 - vx.y + vx.x - vx.z * 3 - ((colors[current_color + faction][3] == flat_alpha) ? -2 : frame * 2)
                           , width, height),  // destination rectangle 
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
        private static Bitmap drawOutlineSW(MagicaVoxelData[] voxels, int faction, int frame)
        {
            Bitmap b = new Bitmap(88, 108, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            Image image = new Bitmap("black_outline_soft.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 8;
            int height = 8;

            float[][] colorMatrixElements = { 
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            for (int i = 0; i < voxels.Length; i++)
            {
                byte tempX = (byte)(voxels[i].x - 10);
                byte tempY = (byte)(voxels[i].y - 10);
                vls[i].x = (byte)((tempY) + 10);
                vls[i].y = (byte)((tempX * -1) + 10 - 1);
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;

            } foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;

                if (frame != 0 && colors[current_color + faction][3] == spin_alpha_0)
                    continue;
                else if (frame != 1 && colors[current_color + faction][3] == spin_alpha_1)
                    continue;
                if (colors[current_color + faction][3] != flat_alpha)
                {
                    /*colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+colors[56 + idx][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[56 + idx][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[56 + idx][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});*/

                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    g.DrawImage(
                       image,
                       new Rectangle((vx.x + vx.y) * 2 + 2, 100 - 20 - 2 - vx.y + vx.x - vx.z * 3 - ((colors[current_color + faction][3] == flat_alpha) ? -2 : frame * 2)
                           , width, height),  // destination rectangle 
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

        private static Bitmap drawOutlineNE(MagicaVoxelData[] voxels, int faction, int frame)
        {
            Bitmap b = new Bitmap(88, 108, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            Image image = new Bitmap("black_outline_soft.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 8;
            int height = 8;

            float[][] colorMatrixElements = { 
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            for (int i = 0; i < voxels.Length; i++)
            {
                byte tempX = (byte)(voxels[i].x - 10);
                byte tempY = (byte)(voxels[i].y - 10);
                vls[i].x = (byte)((tempY * -1) + 10 - 1);
                vls[i].y = (byte)(tempX + 10);
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;

            } foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;

                if (frame != 0 && colors[current_color + faction][3] == spin_alpha_0)
                    continue;
                else if (frame != 1 && colors[current_color + faction][3] == spin_alpha_1)
                    continue;
                if (colors[current_color + faction][3] != flat_alpha)
                {
                    /*colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+colors[56 + idx][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[56 + idx][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[56 + idx][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});*/

                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    g.DrawImage(
                       image,
                       new Rectangle((vx.x + vx.y) * 2 + 2, 100 - 20 - 2 - vx.y + vx.x - vx.z * 3 - ((colors[current_color + faction][3] == flat_alpha) ? -2 : frame * 2)
                           , width, height),  // destination rectangle 
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


        private static Bitmap drawOutlineNW(MagicaVoxelData[] voxels, int faction, int frame)
        {
            Bitmap b = new Bitmap(88, 108, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            Image image = new Bitmap("black_outline_soft.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 8;
            int height = 8;

            float[][] colorMatrixElements = { 
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            for (int i = 0; i < voxels.Length; i++)
            {
                byte tempX = (byte)(voxels[i].x - 10);
                byte tempY = (byte)(voxels[i].y - 10);
                vls[i].x = (byte)((tempX * -1) + 10 - 1);
                vls[i].y = (byte)((tempY * -1) + 10 - 1);
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;

            }
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;

                if (frame != 0 && colors[current_color + faction][3] == spin_alpha_0)
                    continue;
                else if (frame != 1 && colors[current_color + faction][3] == spin_alpha_1)
                    continue;
                if (colors[current_color + faction][3] != flat_alpha)
                {
                    /*colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+colors[56 + idx][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[56 + idx][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[56 + idx][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});*/

                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    g.DrawImage(
                       image,
                       new Rectangle((vx.x + vx.y) * 2 + 2, 100 - 20 - 2 - vx.y + vx.x - vx.z * 3 - ((colors[current_color + faction][3] == flat_alpha) ? -2 : frame * 2), width, height),  // destination rectangle 
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


        private static void processUnitBasic(string u)
        {
            BinaryReader bin = new BinaryReader(File.Open(u + "_X.vox", FileMode.Open));
            MagicaVoxelData[] parsed = FromMagica(bin);

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
        private static Bitmap processSingleOutlined(MagicaVoxelData[] parsed, int color, string dir, int frame, int maxFrames)
        {
            Graphics g;
            Bitmap[] b;
            Bitmap o, n = new Bitmap(44, 64, PixelFormat.Format32bppArgb);
            int d = 0;
            switch (dir)
            {
                case "S":
                    break;
                case "W": d = 1;
                    break;
                case "N": d = 2;
                    break;
                case "E": d = 3;
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

        private static Bitmap processSingleOutlinedDouble(MagicaVoxelData[] parsed, int color, string dir, int frame, int maxFrames)
        {
            Graphics g;
            Bitmap[] b;
            Bitmap o, n = new Bitmap(88 * 2, 108 * 2, PixelFormat.Format32bppArgb), b2 = new Bitmap(88, 108, PixelFormat.Format32bppArgb);
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

            b = renderLarge(parsed, d, color, frame);
            o = renderOutlineLarge(parsed, d, color, frame);
            g = Graphics.FromImage(b[1]);

            g.DrawImage(o, 0, 0);
            ImageAttributes attr = new ImageAttributes();
            g.DrawImage(b[0], 0, 0);//, new Rectangle(0, 0, 88, 108), 0, 0, 88, 108, GraphicsUnit.Pixel, attr
            //attr.SetColorKey(Color.FromArgb(255, 16, 16, 16), Color.White);
            //g = Graphics.FromImage(n);
            //g.DrawImage(b[1], new Rectangle(0, 0, 88, 108), 0, 0, 88, 108, GraphicsUnit.Pixel, attr);

            Graphics g2 = Graphics.FromImage(b2);
            g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g2.DrawImage(b[1].Clone(new Rectangle(32, 46 + 32, 88 * 2, 108 * 2), b[1].PixelFormat), 0, 0, 88, 108);
            return b2;
        }

        private static void processExplosion(string u)
        {
            Console.WriteLine("Processing: " + u);
            BinaryReader bin = new BinaryReader(File.Open(u + "_X.vox", FileMode.Open));
            MagicaVoxelData[] parsed = FromMagicaRaw(bin);
            //renderLarge(parsed, 0, 0, 0)[0].Save("junk_" + u + ".png");

            MagicaVoxelData[][] explode = FieryExplosion(parsed, ((CurrentMobilities[UnitLookup[u]] == MovementType.Immobile) ? false : true));
            string folder = ("ortho/frames");
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

            System.IO.Directory.CreateDirectory("ortho/gifs");
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
            startInfo.Arguments = "-dispose background -delay 11 -loop 0 " + s + " ortho/gifs/" + u + "_explosion_animated.gif";
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
            string folder = ("ortho/frames");

            for (int w = 0; w < 2; w++)
            {
                if ((w == 0 && u == "Infantry" || u == "Tank_S") || (w == 1 && (u == "Infantry_P" || u == "Infantry_T")))
                {
                    filename = u + "_Firing_X.vox";
                }
                if (CurrentWeapons[UnitLookup[u]][w] == 7)
                {
                    bin = new BinaryReader(File.Open(filename, FileMode.Open));
                    parsed = FromMagicaRaw(bin);
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

                    voxelFrames = weaponAnimations[CurrentWeapons[UnitLookup[u]][w]](voxelFrames, UnitLookup[u]);

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
                else if (CurrentWeapons[UnitLookup[u]][w] != -1)
                {
                    bin = new BinaryReader(File.Open(filename, FileMode.Open));
                    parsed = FromMagicaRaw(bin);
                    MagicaVoxelData[][] firing = makeFiringAnimation(parsed, UnitLookup[u], w);
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

                System.IO.Directory.CreateDirectory("ortho/gifs");
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
                startInfo.Arguments = "-dispose background -delay 11 -loop 0 " + s + " ortho/gifs/" + u + "_attack_" + w + "_animated.gif";
                Console.WriteLine("Running convert.exe ...");
                Process.Start(startInfo).WaitForExit();
            }

        }
        private static void processReceiving()
        {
            string folder = ("ortho/frames");

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

                System.IO.Directory.CreateDirectory("ortho/gifs");
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
                    startInfo.Arguments = "-dispose background -delay 11 -loop 0 " + st + " ortho/gifs/" + "color" + color + "_" + WeaponTypes[i] + "_animated.gif";
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
                    parsed = FromMagicaRaw(bin);
                    for (int f = 0; f < 4; f++)
                    { //"color" + i + "/"
                        CreateChannelBitmap(processSingleOutlined(parsed, 7, "S", f, 4), "indexed/" + u + "_Firing_face0" + "_" + f + ".png");
                        CreateChannelBitmap(processSingleOutlined(parsed, 7, "W", f, 4), "indexed/" + u + "_Firing_face1" + "_" + f + ".png");
                        CreateChannelBitmap(processSingleOutlined(parsed, 7, "N", f, 4), "indexed/" + u + "_Firing_face2" + "_" + f + ".png");
                        CreateChannelBitmap(processSingleOutlined(parsed, 7, "E", f, 4), "indexed/" + u + "_Firing_face3" + "_" + f + ".png");
                    }
                    bin.Close();
                }
                if (CurrentWeapons[UnitLookup[u]][w] == 7)
                {
                    bin = new BinaryReader(File.Open(filename, FileMode.Open));
                    parsed = FromMagicaRaw(bin);
                    MagicaVoxelData[][] flying = Flyover(parsed);
                    MagicaVoxelData[][] receive = makeReceiveAnimation(7, CurrentWeaponReceptions[UnitLookup[u]][w]);
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

                    voxelFrames = weaponAnimations[CurrentWeapons[UnitLookup[u]][w]](voxelFrames, UnitLookup[u]);

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
                else if (CurrentWeapons[UnitLookup[u]][w] != -1)
                {
                    bin = new BinaryReader(File.Open(filename, FileMode.Open));
                    parsed = FromMagicaRaw(bin);
                    MagicaVoxelData[][] firing = makeFiringAnimation(parsed, UnitLookup[u], w);
                    MagicaVoxelData[][] receive = makeReceiveAnimation(CurrentWeapons[UnitLookup[u]][w], CurrentWeaponReceptions[UnitLookup[u]][w]);

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
            MagicaVoxelData[] parsed = FromMagicaRaw(bin);
            MagicaVoxelData[][] explode = FieryExplosion(parsed, ((CurrentMobilities[UnitLookup[u]] == MovementType.Immobile) ? false : true));
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
                parsed = FromMagicaRaw(bin);
                explode = FieryExplosion(parsed, ((CurrentMobilities[UnitLookup[u]] == MovementType.Immobile) ? false : true));

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

        private static void processUnitOutlined(string u)
        {

            Console.WriteLine("Processing: " + u);
            BinaryReader bin = new BinaryReader(File.Open(u + "_X.vox", FileMode.Open));
            MagicaVoxelData[] parsed = FromMagica(bin);
            int framelimit = 4;
            if (!UnitLookup.ContainsKey(u))
            {
                framelimit = 4;
                Bitmap b = new Bitmap(56, 108);
                Graphics g = Graphics.FromImage(b);
                for (int i = 0; i < 8; i++)
                {
                    for (int f = 0; f < framelimit; f++)
                    {
                        g.DrawImage(processSingleOutlined(parsed, i, "S", f, framelimit), -16, (f - 2) * 26);
                    }
                    b.Save("color" + i + "_" + u + ".png", ImageFormat.Png);
                }
                bin.Close();
                return;
            }
            else if (CurrentMobilities[UnitLookup[u]] == MovementType.Immobile)
            {
                framelimit = 2;
            }

            for (int i = 0; i < 8; i++)
            {
                string folder = ("ortho/color" + i);//"color" + i;
                System.IO.Directory.CreateDirectory(folder); //("color" + i);
                for (int f = 0; f < framelimit; f++)
                { //"color" + i + "/"

                    processSingleOutlined(parsed, i, "S", f, framelimit).Save(folder + "/" + u + "_face0" + "_" + f + ".png", ImageFormat.Png); //se
                    processSingleOutlined(parsed, i, "W", f, framelimit).Save(folder + "/" + u + "_face1" + "_" + f + ".png", ImageFormat.Png); //sw
                    processSingleOutlined(parsed, i, "N", f, framelimit).Save(folder + "/" + u + "_face2" + "_" + f + ".png", ImageFormat.Png); //nw
                    processSingleOutlined(parsed, i, "E", f, framelimit).Save(folder + "/" + u + "_face3" + "_" + f + ".png", ImageFormat.Png); //ne

                }

            }

            System.IO.Directory.CreateDirectory("ortho/gifs");
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            string s = "";
            for (int i = 0; i < 8; i++)
                s += "ortho/color" + i + "/" + u + "_face* ";
            startInfo.Arguments = "-dispose background -delay 25 -loop 0 " + s + " ortho/gifs/" + u + "_animated.gif";
            Process.Start(startInfo).WaitForExit();

            bin.Close();

            processExplosion(u);

            processFiring(u);
        }

        private static void processUnitOutlinedDouble(string u)
        {

            Console.WriteLine("Processing: " + u);
            BinaryReader bin = new BinaryReader(File.Open(u + "_Large_X.vox", FileMode.Open));
            MagicaVoxelData[] parsed = FromMagica(bin);
            for (int i = 0; i < parsed.Length; i++)
            {
                parsed[i].x += 10;
                parsed[i].y += 10;
            }
            int framelimit = 4;
            if (!UnitLookup.ContainsKey(u))
            {
                framelimit = 4;
                Bitmap b = new Bitmap(56 * 2, 108 * 2);
                Graphics g = Graphics.FromImage(b);
                for (int i = 0; i < 8; i++)
                {
                    for (int f = 0; f < framelimit; f++)
                    {
                        g.DrawImage(processSingleOutlinedDouble(parsed, i, "SE", f, framelimit), -16, (f - 2) * 26);
                    }
                    b.Save("color" + i + "_" + u + ".png", ImageFormat.Png);
                }
                bin.Close();
                return;
            }
            else if (CurrentMobilities[UnitLookup[u]] == MovementType.Immobile)
            {
                framelimit = 2;
            }

            for (int i = 0; i < 8; i++)
            {
                string folder = ("color" + i);//"color" + i;
                System.IO.Directory.CreateDirectory(folder); //("color" + i);
                for (int f = 0; f < framelimit; f++)
                { //"color" + i + "/"

                    processSingleOutlinedDouble(parsed, i, "SE", f, framelimit).Save(folder + "/" + u + "_Large_face0" + "_" + f + ".png", ImageFormat.Png); //se
                    processSingleOutlinedDouble(parsed, i, "SW", f, framelimit).Save(folder + "/" + u + "_Large_face1" + "_" + f + ".png", ImageFormat.Png); //sw
                    processSingleOutlinedDouble(parsed, i, "NW", f, framelimit).Save(folder + "/" + u + "_Large_face2" + "_" + f + ".png", ImageFormat.Png); //nw
                    processSingleOutlinedDouble(parsed, i, "NE", f, framelimit).Save(folder + "/" + u + "_Large_face3" + "_" + f + ".png", ImageFormat.Png); //ne

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

            //processExplosion(u);

            //processFiring(u);
        }
        public static void processTerrainChannel()
        {
            for (int i = 0; i < 11; i++)
            {
                PlusPaletteDraw.drawPixelsFlat(i);

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
            MagicaVoxelData[] parsed = FromMagica(bin);
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
            MagicaVoxelData[] parsed = FromMagica(bin);
            int framelimit = 4;
            if (CurrentMobilities[UnitLookup[u]] == MovementType.Immobile)
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
                parsed = FromMagicaRaw(bin);
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
        static Bitmap makeTiling()
        {
            Bitmap[] tilings = new Bitmap[10];
            for (int i = 0; i < 10; i++)
            {
                tilings[i] = OrthoPaletteDraw.drawPixelsFlat(i);
            }
            Bitmap b = new Bitmap(64 * 20, 32 * 20);
            Graphics tiling = Graphics.FromImage(b);

            LocalMap lm = new LocalMap(20, 20);
            for (int j = 0; j < 20; j++)
            {
                for (int i = 0; i < 20; i++)
                {
                    tiling.DrawImageUnscaled(tilings[lm.Land[i, j]], (64 * i), (32 * j) - 16 + 6);
                }
            }

            return b;
        }
        
        static Bitmap makeFlatTiling()
        {
            Bitmap b = new Bitmap(128 * 16, 32 * 32);
            Graphics g = Graphics.FromImage(b);

            Bitmap[] tilings = new Bitmap[10];
            for (int i = 0; i < 10; i++)
            {
                tilings[i] = PlusPaletteDraw.drawPixelsFlat(i);
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
                    g.DrawImageUnscaled(tilings[grid[i, j]], (128 * i) - ((j % 2 == 0) ? 0 : 64), (32 * j) - 35 - 32);
                }
            }
            return b;
        }

        static float[,] dijkstra(UnitInfo self, int[,] grid, UnitInfo[,] placing, int goalX, int goalY)
        {
            /*
            (defn find-cells [^doubles a cell-kind]
                (persistent! (areduce ^doubles a i ret (transient {})
                                      (if (= (hiphip/aget ^doubles a i) cell-kind) (assoc! ret i cell-kind) ret))))

            (defn find-goals [^doubles a]
              (find-cells a GOAL))

            (defn find-walls [^doubles a]
                (persistent! (areduce ^doubles a i ret (transient {})
                                      (if (>= (hiphip/aget ^doubles a i) (double wall)) (assoc! ret i wall) ret))))

            (defn find-floors [^doubles a]
              (find-cells a floor))

            (defn find-lowest [^doubles a]
              (let [low-val (hiphip/amin a)]
                (find-cells a low-val)))

            (defn find-monsters [m]
                (into {} (for [mp (map #(:pos @%) m)] [mp 1.0])))

            (defn dijkstra
              ([a]
                 (dijkstra a (find-walls a) (find-lowest a)))
              ([dun _]
                 (dijkstra (:dungeon dun) (merge (find-walls (:dungeon dun)) (find-monsters @(:monsters dun))) (find-lowest (:dungeon dun))))
              ([a closed open-cells]
                 (loop [open open-cells]
                   (when (seq open)
                     (recur (reduce (fn [newly-open [^long i ^double v]]
                                      (reduce (fn [acc dir]
                                                (if (or (closed dir) (open dir)
                                                        (>= (+ 1.0 v) (hiphip/aget ^doubles a dir)))
                                                  acc
                                                  (do (hiphip/aset ^doubles a dir (+ 1.0 v))
                                                      (assoc acc dir (+ 1.0 v)))))
                                              newly-open, [(- i wide2)
                                                           (+ i wide2)
                                                           (- i 2)
                                                           (+ i 2)]))
                                    {}, open))))
                 a))*/
            int width = grid.GetLength(0);
            int height = grid.GetLength(1);
            float wall = 222;
            float unexplored = 111;
            float goal = 0;
            float[] d = new float[width * height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                    d[i + width * j] = unexplored;
            }
            for (int i = 0; i < width; i++)
            {
                d[i] = wall;
                d[i + width * (height - 1)] = wall;
            }
            for (int j = 1; j < height - 1; j += 2)
            {
                d[j * width] = wall;
                d[(j + 1) * width + (width - 1)] = wall;
            }
            d[goalX + width * goalY] = goal;
            int[] ability =
            new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, };
            //plains forest desert jungle hills mountains ruins tundra road river
            Dictionary<MovementType, bool> pass = new Dictionary<MovementType, bool>
            {
                {MovementType.Foot, true},
                {MovementType.Treads, true},
                {MovementType.Wheels, true},
                {MovementType.TreadsAmphi, true},
                {MovementType.WheelsTraverse, true},
                {MovementType.Flight, true},
                {MovementType.Immobile, false},
            };
            switch (self.mobility)
            {
                case MovementType.Foot:
                    ability =
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, };
                    pass = new Dictionary<MovementType, bool>
            {
                {MovementType.Foot, false},
                {MovementType.Treads, true},
                {MovementType.Wheels, false},
                {MovementType.TreadsAmphi, true},
                {MovementType.WheelsTraverse, false},
                {MovementType.Flight, true},
                {MovementType.Immobile, false},
            };
                    break;
                case MovementType.Treads:
                    ability =
            new int[] { 1, 1, 1, 1, 0, 0, 1, 1, 1, 0, };
                    pass = new Dictionary<MovementType, bool>
            {
                {MovementType.Foot, false},
                {MovementType.Treads, false},
                {MovementType.Wheels, false},
                {MovementType.TreadsAmphi, false},
                {MovementType.WheelsTraverse, false},
                {MovementType.Flight, true},
                {MovementType.Immobile, false},
            };
                    break;
                case MovementType.Wheels:
                    ability =
            new int[] { 1, 0, 1, 0, 0, 0, 0, 1, 1, 0, };
                    pass = new Dictionary<MovementType, bool>
            {
                {MovementType.Foot, false},
                {MovementType.Treads, false},
                {MovementType.Wheels, false},
                {MovementType.TreadsAmphi, false},
                {MovementType.WheelsTraverse, false},
                {MovementType.Flight, true},
                {MovementType.Immobile, false},
            };
                    break;
                case MovementType.TreadsAmphi:
                    ability =
            new int[] { 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, };
                    pass = new Dictionary<MovementType, bool>
            {
                {MovementType.Foot, false},
                {MovementType.Treads, false},
                {MovementType.Wheels, false},
                {MovementType.TreadsAmphi, false},
                {MovementType.WheelsTraverse, false},
                {MovementType.Flight, true},
                {MovementType.Immobile, false},
            };
                    break;
                case MovementType.WheelsTraverse:
                    ability =
            new int[] { 1, 1, 1, 1, 0, 0, 1, 1, 1, 0, };
                    pass = new Dictionary<MovementType, bool>
            {
                {MovementType.Foot, false},
                {MovementType.Treads, false},
                {MovementType.Wheels, false},
                {MovementType.TreadsAmphi, false},
                {MovementType.WheelsTraverse, false},
                {MovementType.Flight, true},
                {MovementType.Immobile, false},
            };
                    break;
                case MovementType.Flight:
                    ability =
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, };
                    pass = new Dictionary<MovementType, bool>
            {
                {MovementType.Foot, true},
                {MovementType.Treads, true},
                {MovementType.Wheels, true},
                {MovementType.TreadsAmphi, true},
                {MovementType.WheelsTraverse, true},
                {MovementType.Flight, false},
                {MovementType.Immobile, false},
            };
                    break;
            }
            Dictionary<int, int> open = new Dictionary<int, int> { { (goalX + width * goalY), 0 } },
                fringe = new Dictionary<int, int>(),
                closed = new Dictionary<int, int>();
            int[] oddmoves = { -1 + width, width, -1 + -width, -width };
            int[] evenmoves = { 1 + width, width, 1 + -width, -width };
            while (open.Count > 0)
            {
                foreach (var idx_dijk in open)
                {
                    foreach (int mov in ((idx_dijk.Key / width) % 2 == 0) ? evenmoves : oddmoves)
                        if (open.ContainsKey(idx_dijk.Key + mov) ||
                            closed.ContainsKey(idx_dijk.Key + mov) ||
                            d[idx_dijk.Key + mov] == wall ||
                            d[idx_dijk.Key + mov] <= idx_dijk.Value + 1)
                        {

                        }
                        else if (
                            ability[grid[(idx_dijk.Key + mov) % width, (idx_dijk.Key + mov) / width]] == 1 &&
                            (placing[(idx_dijk.Key + mov) % width, (idx_dijk.Key + mov) / width] == null ||
                            pass[placing[(idx_dijk.Key + mov) % width, (idx_dijk.Key + mov) / width].mobility] ||
                            (placing[(idx_dijk.Key + mov) % width, (idx_dijk.Key + mov) / width].color == self.color &&
                            placing[(idx_dijk.Key + mov) % width, (idx_dijk.Key + mov) / width].mobility != MovementType.Immobile)
                            //self.x + self.y * width == idx_dijk.Key + mov
                            ))
                        {
                            fringe[idx_dijk.Key + mov] = (idx_dijk.Value + 1);
                            d[idx_dijk.Key + mov] = idx_dijk.Value + 1;
                        }
                }
                foreach (var kv in open)
                {
                    closed[kv.Key] = (kv.Value);
                }
                open.Clear();
                foreach (var kv in fringe)
                {
                    open[kv.Key] = (kv.Value);
                }
                fringe.Clear();

            }
            /*
([a closed open-cells]
    (loop [open open-cells]
        (when (seq open)
            (recur
               (reduce
                   (fn [newly-open [^long i ^double v]]
                       (reduce
                           (fn [acc dir]
                               (if (or (closed dir) (open dir) (>= (+ 1.0 v) (hiphip/aget ^doubles a dir)))
                                   acc
                                   (do (hiphip/aset ^doubles a dir (+ 1.0 v))
                                       (assoc acc dir (+ 1.0 v))
                                   )
                                )
                            )
                            newly-open
                            [(- i wide2)
                             (+ i wide2)
                             (- i 2)
                             (+ i 2)]
                        )
                    )
                    {}
                    open))))
     a)*/
            d[goalX + width * goalY] = wall;
            float[,] n = new float[width, height];

            for (int j = 0; j < height; j++)
            {
                if (j % 2 == 0)
                    Console.Write("  ");
                for (int i = 0; i < width; i++)
                {
                    n[i, j] = d[i + j * width];
                    Console.Write(string.Format("{0,4}", n[i, j]));
                }
                Console.WriteLine();
            }
            return n;
        }
        static List<Tuple<int, int, int>> getDijkstraPath(UnitInfo active, int[,] grid, UnitInfo[,] placing, int targetX, int targetY)
        {
            int width = grid.GetLength(0);
            float[,] d_inv = dijkstra(active, grid, placing, targetX, targetY);
            List<Tuple<int, int, int>> path = new List<Tuple<int, int, int>>();
            int currentX = active.x, currentY = active.y, currentFacing = active.facing;

            for (int f = 0; f < active.speed; f++)
            {
                Dictionary<int, float> near = new Dictionary<int, float>();
                near[currentX + width * (currentY)] = d_inv[currentX, currentY];
                near[currentX + width * (currentY + 1)] = d_inv[currentX, currentY + 1];
                near[currentX + width * (currentY - 1)] = d_inv[currentX, currentY - 1];
                if (currentY % 2 == 0)
                {
                    near[currentX + 1 + width * (currentY + 1)] = d_inv[currentX + 1, currentY + 1];
                    near[currentX + 1 + width * (currentY - 1)] = d_inv[currentX + 1, currentY - 1];
                }
                else
                {
                    near[currentX - 1 + width * (currentY + 1)] = d_inv[currentX - 1, currentY + 1];
                    near[currentX - 1 + width * (currentY - 1)] = d_inv[currentX - 1, currentY - 1];
                }
                var ordered = near.OrderBy(kv => kv.Value); //.First().Key;;
                int newpos = ordered.TakeWhile(kv => kv.Value == ordered.First().Value).RandomElement().Key;
                if (near.All(e => e.Value == near[newpos]))
                {
                    return new List<Tuple<int, int, int>>();
                }
                int newX = newpos % width, newY = newpos / width;
                if (!(newX == currentX && newY == currentY))
                {
                    if (newY % 2 == 0)
                    {
                        if (newY > currentY)
                        {
                            //south
                            if (newX < currentX)
                                currentFacing = 1;
                            else
                                currentFacing = 0;
                        }
                        else
                        {
                            //north
                            if (newX < currentX)
                                currentFacing = 2;
                            else
                                currentFacing = 3;
                        }
                    }
                    else
                        if (newY > currentY)
                        {
                            //south
                            if (newX == currentX)
                                currentFacing = 1;
                            else
                                currentFacing = 0;
                        }
                        else
                        {
                            //north
                            if (newX == currentX)
                                currentFacing = 2;
                            else
                                currentFacing = 3;
                        }

                    currentX = newX;
                    currentY = newY;
                    /*if (placing[newX, newY] != null && newX == targetX && newY == targetY)
                    {
                        break;
                    }
                    if (d_inv[newX, newY] <= 1.1F && placing[newX, newY] == null)
                        break;
                    */
                }
                path.Add(new Tuple<int, int, int>(currentX, currentY, currentFacing));
            }
            while (path.Count > 0 && placing[path.Last().Item1, path.Last().Item2] != null)
            {
                path.RemoveAt(path.Count - 1);
            }
            return path;
        }
        
        /// <summary>
        /// This will take a long time to run.  It should produce a ton of assets and an animated gif preview.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Initialize();

            System.IO.Directory.CreateDirectory("Palettes");
            System.IO.Directory.CreateDirectory("indexed");
            System.IO.Directory.CreateDirectory("ortho");
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
            InitializePalettes();
            //            Madden();
            //processTerrainChannel();
            makeTiling().Save("tiling_ortho_flat.png", ImageFormat.Png);

           // processReceiving();
            //processExplosion("Plane");


            
            /*
            processFiring("Copter");
            processFiring("Copter_P");
            processFiring("Copter_S");
            processFiring("Copter_T");

            processFiring("Plane");
            processFiring("Plane_P");
            processFiring("Plane_S");
            processFiring("Plane_T");

            processFiring("Supply");
            processFiring("Supply_P");
            processFiring("Supply_S");
            processFiring("Supply_T");

            processFiring("Artillery");
            processFiring("Artillery_S");
            processFiring("Artillery_P");
            processFiring("Artillery_T");

            processFiring("Infantry_P");
            processFiring("Infantry");
            processFiring("Infantry_S");
            processFiring("Infantry_T");

            processFiring("Tank");
            processFiring("Tank_P");
            processFiring("Tank_S");
            processFiring("Tank_T");
            */
            /*for (int c = 0; c < 8; c++)
            {
                List<string> ls = new List<string>(17);
                for (int i = 0; i < 17; i++)
                {
                    ls.Add(colors[i * 8 + c][0] + ", " + colors[i * 8 + c][1] + ", " + colors[i * 8 + c][2] + "\n");
                }
                File.WriteAllLines("Palettes/color" + c + ".txt", ls);
            }*/

            /*
            processChannelExplosion("Artillery");
            processChannelExplosion("Artillery_P");
            processChannelExplosion("Artillery_S");
            processChannelExplosion("Artillery_T");

            processChannelExplosion("Airport");
            processChannelExplosion("City");
            processChannelExplosion("Factory");
            processChannelExplosion("Laboratory");
            processChannelExplosion("Castle");

            processChannelExplosion("Estate");

            processChannelExplosion("Infantry");
            processChannelExplosion("Infantry_P");

            processChannelExplosion("Infantry_S");
            processChannelExplosion("Infantry_T");


            processChannelExplosion("Plane");

            processChannelExplosion("Plane_P");
            processChannelExplosion("Plane_S");
            processChannelExplosion("Plane_T");

            processChannelExplosion("Copter");
            processChannelExplosion("Copter_P");
            processChannelExplosion("Copter_S");
            processChannelExplosion("Copter_T");

            processChannelExplosion("Tank");
            processChannelExplosion("Tank_P");
            processChannelExplosion("Tank_S");
            processChannelExplosion("Tank_T");

            processChannelExplosion("Supply");
            processChannelExplosion("Supply_P");
            processChannelExplosion("Supply_S");
            processChannelExplosion("Supply_T");
            */

            /*
            processExplosion("Artillery");
            processExplosion("Artillery_P");
            processExplosion("Artillery_S");
            processExplosion("Artillery_T");
           
            processExplosion("Airport");
            processExplosion("City");
            processExplosion("Factory");
            processExplosion("Laboratory");
            processExplosion("Castle");
            
            processExplosion("Estate");
            
            processExplosion("Infantry");
            processExplosion("Infantry_P");
            
            processExplosion("Infantry_S");
            processExplosion("Infantry_T");
            

            processExplosion("Plane");
            
            processExplosion("Plane_P");
            processExplosion("Plane_S");
            processExplosion("Plane_T");

            processExplosion("Copter");
            processExplosion("Copter_P");
            processExplosion("Copter_S");
            processExplosion("Copter_T");

            processExplosion("Tank");
            processExplosion("Tank_P");
            processExplosion("Tank_S");
            processExplosion("Tank_T");

            processExplosion("Supply");
            processExplosion("Supply_P");
            processExplosion("Supply_S");
            processExplosion("Supply_T");
            */


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
*/
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