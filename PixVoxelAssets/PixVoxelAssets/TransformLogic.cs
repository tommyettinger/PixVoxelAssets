using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsPV
{
    public class Bone
    {
        public const int Multiplier = OrthoSingle.multiplier, Bonus = OrthoSingle.bonus;
        public readonly byte[,,] Colors;
        public float Roll = 0f, Pitch = 0f, Yaw = 0f, StartRoll = 0f, StartPitch = 0f, StartYaw = 0f,
            MoveX = 0f, MoveY = 0f, MoveZ = 0f, StretchX = 1f, StretchY = 1f, StretchZ = 1f;

        /*
         * @param yaw the rotation around the y axis in radians
         * @param pitch the rotation around the x axis in radians
         * @param roll the rotation around the z axis in radians
         * @return this quaternion */
        private static Vector3 YawAxis = new Vector3(0f, 1f, 0f), PitchAxis = new Vector3(1f, 0f, 0f), RollAxis = new Vector3(0f, 0f, 1f);
        public byte[] SpreadColors = null;

        public Bone(byte[,,] c)
        {
            Colors = c;
        }
        public Bone(byte[,,] colors, float yaw, float pitch, float roll)
        {
            this.Colors = colors;
            this.Yaw = yaw;
            this.Pitch = pitch;
            this.Roll = roll;
        }
        public Bone InitSpread(byte[] spreadColors, float yaw_back, float pitch_back, float roll_back)
        {
            SpreadColors = spreadColors;
            StartYaw = yaw_back;
            StartPitch = pitch_back;
            StartRoll = roll_back;
            return this;
        }
        public Bone InitStretch(float stretchX, float stretchY, float stretchZ)
        {
            StretchX = stretchX;
            StretchY = stretchY;
            StretchZ = stretchZ;
            return this;
        }
        public Bone Translate(float moveX, float moveY, float moveZ)
        {
            MoveX += moveX;
            MoveY += moveY;
            MoveZ += moveZ;
            return this;
        }

        public Bone Reset()
        {
            Roll = 0f;
            Pitch = 0f;
            Yaw = 0f;
            StartRoll = 0f;
            StartPitch = 0f;
            StartYaw = 0f;
            MoveX = 0f;
            MoveY = 0f;
            MoveZ = 0f;
            StretchX = 1f;
            StretchY = 1f;
            StretchZ = 1f;
            SpreadColors = null;
            return this;
        }

        public byte[,,] FinalizeSimple(int xOffset, int yOffset)
        {
            //q.setEulerAngles(360 - Yaw, 360 - Pitch, 360 - Roll);
            //            q = q.idt().slerp(new Quaternion[] { new Quaternion(YawAxis, (360 - Pitch) % 360f), new Quaternion(PitchAxis, (360 - Roll) % 360f), new Quaternion(RollAxis, (360 - Yaw) % 360f) });
            Quaternion q = new Quaternion().mulLeft(new Quaternion(YawAxis, (360 - Pitch) % 360f)).mulLeft(new Quaternion(PitchAxis, (360 - Roll) % 360f)).mulLeft(new Quaternion(RollAxis, (360 - Yaw) % 360f));

            //dir = dir.fromAngles(360 - Yaw, 360 - Pitch, 360 - Roll);
            int xSize = Colors.GetLength(0), ySize = Colors.GetLength(1), zSize = Colors.GetLength(2),
                hxs = xSize / 2, hys = ySize / 2, hzs = zSize / 2;
            Vector3 v = new Vector3(0f, 0f, 0f);

            byte[,,] c2 = new byte[xSize, ySize, zSize];

            for(int z = 0; z < zSize; z++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int x = 0; x < xSize; x++)
                    {
                        if(Colors[x, y, z] > 0)
                        {
                            v.x = (x - hxs) * StretchX + xOffset;
                            v.y = (y - hys) * StretchY;
                            v.z = (z - hzs) * StretchZ;
                            q.transform(v);
                            int x2 = (int)(v.x + 0.5f + hxs - xOffset + MoveX), y2 = (int)(v.y + 0.5f + hys + MoveY), z2 = (int)(v.z + 0.5f + hzs + MoveZ);
                            if(x2 >= 0 && y2 >= 0 && z2 >= 0 && x2 < xSize && y2 < ySize && z2 < zSize)
                                c2[x2, y2, z2] = Colors[x, y, z];
                        }
                    }
                }
            }
            return c2;
        }

        public byte[,,] Finalize(int xOffset, int yOffset)
        {
            if(SpreadColors == null)
                return FinalizeSimple(xOffset, yOffset);

            //q.setEulerAngles(360 - Yaw, 360 - Pitch, 360 - Roll);
            Quaternion q = new Quaternion().mulLeft(new Quaternion(YawAxis, (360 - Pitch) % 360f)).mulLeft(new Quaternion(PitchAxis, (360 - Roll) % 360f)).mulLeft(new Quaternion(RollAxis, (360 - Yaw) % 360f));
            //q = q.idt().slerp(new Quaternion[] { new Quaternion(RollAxis, (360 - Roll) % 360f), new Quaternion(PitchAxis, (360 - Pitch) % 360f), new Quaternion(YawAxis, (360 - Yaw) % 360f) });

            //Quaternion q0 = new Quaternion().setEulerAngles((StartYaw - Yaw + 360) % 360, (StartPitch - Pitch + 360) % 360, (StartRoll - Roll + 360) % 360), q2;
            Quaternion q0 = q.cpy().mulLeft(new Quaternion(YawAxis, -StartPitch))
                .mulLeft(new Quaternion(PitchAxis, -StartRoll))
                .mulLeft(new Quaternion(RollAxis, -StartYaw)),
            q2;



            float distance = Math.Abs(Yaw + StartYaw) % 360 + Math.Abs(Pitch + StartPitch) % 360 + Math.Abs(Roll + StartRoll) % 360;

            float levels = Math.Max(distance / 2.5f, 1f);

            int xSize = Colors.GetLength(0), ySize = Colors.GetLength(1), zSize = Colors.GetLength(2),
                hxs = xSize / 2, hys = ySize / 2, hzs = zSize / 2;
            Vector3 v = new Vector3(0f, 0f, 0f);

            byte[,,] c2 = new byte[xSize, ySize, zSize];

            for(int z = 0; z < zSize; z++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int x = 0; x < xSize; x++)
                    {
                        if(Colors[x, y, z] > 0)
                        {
                            v.x = (x - hxs) * StretchX + xOffset;
                            v.y = (y - hys) * StretchY;
                            v.z = (z - hzs) * StretchZ;
                            // v.nor();
                            q.transform(v);
                            int x2 = (int)(v.x + 0.5f + hxs - xOffset + MoveX), y2 = (int)(v.y + 0.5f + hys + MoveY), z2 = (int)(v.z + 0.5f + hzs + MoveZ);

                            //int x2 = (int)(v.x + 0.5f), y2 = (int)(v.y + 0.5f), z2 = (int)(v.z + 0.5f);

                            if(x2 >= 0 && y2 >= 0 && z2 >= 0 && x2 < xSize && y2 < ySize && z2 < zSize)
                            {
                                c2[x2, y2, z2] = Colors[x, y, z];
                            }
                        }
                    }
                }
            }

            for(int z = 0; z < zSize; z++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int x = 0; x < xSize; x++)
                    {
                        for(int c = 0; c < SpreadColors.Length; c++)
                        {
                            if(Colors[x, y, z] == SpreadColors[c])
                            {
                                v.x = (x - hxs) * StretchX + xOffset;
                                v.y = (y - hys) * StretchY;
                                v.z = (z - hzs) * StretchZ;
                                //v.nor();
                                //Vector3 vOuter = q0.transform(v.cpy());

                                for(float a = 0.0f; a <= 1.0f; a += 1.0f / levels)
                                {
                                    q2 = q0.cpy().slerp(q, a);
                                    Vector3 v2 = q2.transform(v.cpy());
                                    
                                    int x2 = (int)(v2.x + 0.5f + hxs - xOffset + MoveX), y2 = (int)(v2.y + 0.5f + hys + MoveY), z2 = (int)(v2.z + 0.5f + hzs + MoveZ);
                                    
                                    if(x2 >= 0 && y2 >= 0 && z2 >= 0 && x2 < xSize && y2 < ySize && z2 < zSize && c2[x2, y2, z2] == 0)
                                        c2[x2, y2, z2] = Colors[x, y, z];
                                }
                                break;
                            }
                        }
                    }
                }
            }

            return c2;
        }
        public bool RoughEquals(Bone target)
        {
            return (Pitch == target.Pitch && Roll == target.Roll && Yaw == target.Yaw && StartPitch == target.StartPitch && StartRoll == target.StartRoll && StartYaw == target.StartYaw &&
                MoveX == target.MoveX && MoveY == target.MoveY && MoveZ == target.MoveZ && StretchX == target.StretchX && StretchY == target.StretchY && StretchZ == target.StretchZ);
        }
        public Bone Interpolate(Bone target, float alpha)
        {
            if(alpha <= 0.0)
                return this.Replicate();
            else if(alpha >= 1.0)
                return target.Replicate();
            else if(RoughEquals(target))
                return this.Replicate();
            Quaternion q = new Quaternion().mulLeft(new Quaternion(YawAxis, (360 - Pitch) % 360f)).mulLeft(new Quaternion(PitchAxis, (360 - Roll) % 360f)).mulLeft(new Quaternion(RollAxis, (360 - Yaw) % 360f));

            Quaternion q0 = new Quaternion().mulLeft(new Quaternion(YawAxis, (360 - target.Pitch) % 360f))
                .mulLeft(new Quaternion(PitchAxis, (360 - target.Roll) % 360f))
                .mulLeft(new Quaternion(RollAxis, (360 - target.Yaw) % 360f)),
            q2;
            Vector3 myStarts = new Vector3(StartYaw, StartPitch, StartRoll), tgtStarts = new Vector3(target.StartYaw, target.StartPitch, target.StartRoll),
                myStretches = new Vector3(StretchX, StretchY, StretchZ), tgtStretches = new Vector3(target.StretchX, target.StretchY, target.StretchZ);
            myStarts.lerp(tgtStarts, alpha);
            myStretches.lerp(tgtStretches, alpha);
            byte[] newSpread = null;
            if(SpreadColors != null)
            {
                newSpread = SpreadColors;
                if(target.SpreadColors != null)
                {
                    newSpread = new byte[SpreadColors.Length + target.SpreadColors.Length];
                    for(int i = 0; i < SpreadColors.Length; i++)
                    {
                        newSpread[i] = SpreadColors[i];
                    }
                    for(int i = 0; i < target.SpreadColors.Length; i++)
                    {
                        newSpread[SpreadColors.Length + i] = target.SpreadColors[i];
                    }
                }
            }
            else if(target.SpreadColors != null)
                newSpread = target.SpreadColors;
            

            q2 = q.cpy().slerp(q0, alpha);
            return new Bone(Colors, 360 - q2.getRoll(), 360 - q2.getYaw(), 360 - q2.getPitch())
                .InitSpread(newSpread, myStarts.x, myStarts.y, myStarts.z)
                .InitStretch(myStretches.x, myStretches.y, myStretches.z);
        }
        public Bone Replicate()
        {
            Bone b = new Bone(Colors, Yaw, Pitch, Roll);
            b.SpreadColors = SpreadColors.Replicate();
            b.StartYaw = StartYaw;
            b.StartPitch = StartPitch;
            b.StartRoll = StartRoll;
            b.StretchX = StretchX;
            b.StretchY = StretchY;
            b.StretchZ = StretchZ;
            return b;
        }

        public static Bone readBone(string file)
        {
            if(file == null)
                return new Bone(new byte[60 * Multiplier * Bonus, 60 * Multiplier * Bonus, 60 * Multiplier * Bonus]);
            BinaryReader bin = new BinaryReader(File.Open(VoxelLogic.voxFolder + file + "_W.vox", FileMode.Open));
            List<MagicaVoxelData> raw = VoxelLogic.FromMagicaRaw(bin);
            return new Bone(TransformLogic.TransformStartLarge(raw, Multiplier * Bonus));
        }
    }
    public struct Connector
    {
        public string plug;
        public string socket;
        public int matchColor;
        public Connector(string plug, string socket, int matchColor)
        {
            this.plug = plug;
            this.socket = socket;
            this.matchColor = matchColor;
        }
    }
    public class Model
    {
        public Dictionary<string, Bone> Bones = new Dictionary<string, Bone>(16);
        public Connector[] Anatomy = new Connector[0];
        public Model()
        {

        }
        public Model(Dictionary<string, Bone> bones)
        {
            Bones = bones;
        }
        
        public Model AddBone(string boneName, Bone bone)
        {
            Bones[boneName] = bone;
            return this;
        }

        public Model AddBone(string boneName, byte[,,] bone)
        {
            Bones[boneName] = new Bone(bone);
            return this;
        }

        public Model AddSpread(string boneName, float pitchy, float rolly, float yawy, params byte[] spreadColors)
        {
            Bones[boneName].InitSpread(spreadColors, rolly, pitchy, yawy);
            return this;
        }
        public Model AddYaw(float yaw, params string[] names)
        {
            if(names.Length == 0)
            {
                foreach(string nm in Bones.Keys)
                {
                    Bones[nm].Yaw += yaw;
                }
            }
            else
            {
                foreach(string nm in names)
                {
                    Bones[nm].Yaw += yaw;
                }
            }
            return this;
        }
        public Model AddPitch(float pitch, params string[] names)
        {
            if(names.Length == 0)
            {
                foreach(string nm in Bones.Keys)
                {
                    Bones[nm].Pitch += pitch;
                }
            }
            else
            {
                foreach(string nm in names)
                {
                    Bones[nm].Pitch += pitch;
                }
            }
            return this;
        }
        public Model AddRoll(float roll, params string[] names)
        {
            if(names.Length == 0)
            {
                foreach(string nm in Bones.Keys)
                {
                    Bones[nm].Roll += roll;
                }
            }
            else
            {
                foreach(string nm in names)
                {
                    Bones[nm].Roll += roll;
                }
            }
            return this;
        }
        public Model Translate(float x, float y, float z, params string[] names)
        {
            if(names.Length == 0)
            {
                foreach(string nm in Bones.Keys)
                {
                    Bones[nm].Translate(x, y, z);
                }
            }
            else
            {
                foreach(string nm in names)
                {
                    Bones[nm].Translate(x, y, z);
                }
            }
            return this;
        }
        public Model AddStretch(float x, float y, float z, params string[] names)
        {
            if(names.Length == 0)
            {
                foreach(string nm in Bones.Keys)
                {
                    Bones[nm].StretchX *= x;
                    Bones[nm].StretchY *= y;
                    Bones[nm].StretchZ *= z;
                }
            }
            else
            {
                foreach(string nm in names)
                {
                    Bones[nm].StretchX *= x;
                    Bones[nm].StretchY *= y;
                    Bones[nm].StretchZ *= z;
                }
            }
            return this;
        }
        public Model SetAnatomy(params Connector[] anatomy)
        {
            Anatomy = anatomy;
            return this;
        }
        public byte[,,] Finalize()
        {
            Dictionary<string, byte[,,]> finals = Bones.ToDictionary(kv => kv.Key, kv => kv.Value.Finalize(40, 40));
            string finished = "";
            foreach(Connector conn in Anatomy)
            {
                byte[,,] bs = TransformLogic.MergeVoxels(finals[conn.plug], finals[conn.socket], conn.matchColor);
                if(bs == finals[conn.socket])
                    Console.WriteLine("PLUG NOT FOUND: " + conn.plug + " connecting to " + conn.socket);
                else if(bs == finals[conn.plug])
                    Console.WriteLine("SOCKET NOT FOUND: " + conn.socket + " connecting to " + conn.plug);
                finals[conn.socket] = bs;
                finished = conn.socket;
            }
            return finals[finished];
        }
        public Model Interpolate(Model target, float alpha)
        {
            Model m = new Model();
            m.Anatomy = Anatomy;
            Dictionary<string, Bone> finals = Bones.ToDictionary(kv => kv.Key, kv => kv.Value.Interpolate(target.Bones[kv.Key], alpha));
            foreach(var kv in finals)
            {
                m.AddBone(kv.Key, kv.Value);
            }
            return m;

        }
        public Model Replicate()
        {
            Dictionary<string, Bone> bones = Bones.ToDictionary(kv => kv.Key, kv => kv.Value.Replicate());
            Model m = new Model(bones);
            m.Anatomy = Anatomy.Replicate();
            return m;
        }

        public static Model Humanoid(string right_leg = "Human_Male", string left_leg = "Human_Male", string torso = "Human_Male",
            string left_upper_arm = "Human_Male", string left_lower_arm = "Human_Male", string right_upper_arm = "Human_Male", string right_lower_arm = "Human_Male",
            string head = "Human_Male", string face = "Neutral", string left_weapon = null, string right_weapon = null)
        {
            Model model = new Model();
            model.AddBone("Left_Leg", Bone.readBone(left_leg + "_LLeg"));
            model.AddBone("Right_Leg", Bone.readBone(right_leg + "_RLeg"));
            model.AddBone("Torso", Bone.readBone(torso + "_Torso"));
            model.AddBone("Face", Bone.readBone(face + "_Face"));
            model.AddBone("Head", Bone.readBone(head + "_Head"));
            model.AddBone("Left_Upper_Arm", Bone.readBone(left_upper_arm + "_LUpperArm"));
            model.AddBone("Right_Upper_Arm", Bone.readBone(right_upper_arm + "_RUpperArm"));
            model.AddBone("Left_Lower_Arm", Bone.readBone(left_lower_arm + "_LLowerArm"));
            model.AddBone("Right_Lower_Arm", Bone.readBone(right_lower_arm + "_RLowerArm"));
            model.AddBone("Left_Weapon", Bone.readBone(left_weapon));
            model.AddBone("Right_Weapon", Bone.readBone(right_weapon));


            model.SetAnatomy(new Connector("Face", "Head", 9), new Connector("Head", "Torso", 8), new Connector("Right_Weapon", "Right_Lower_Arm", 6), new Connector("Left_Weapon", "Left_Lower_Arm", 6),
                new Connector("Right_Lower_Arm", "Right_Upper_Arm", 5), new Connector("Left_Lower_Arm", "Left_Upper_Arm", 4), new Connector("Right_Upper_Arm", "Torso", 3), new Connector("Left_Upper_Arm", "Torso", 2),
                new Connector("Right_Leg", "Torso", 1), new Connector("Torso", "Left_Leg", 0));
            return model;
        }
    }

    public delegate Model Pose(Model m);
    

    public class TransformLogic
    {
        public static byte[,,] MergeVoxels(byte[,,] plug, byte[,,] socket, int matchColor)
        {
            int plugX = -1, plugY = -1, plugZ = -1, socketX = -1, socketY = -1, socketZ = -1;
            if(socket == null)
                return plug;
            else if(plug == null)
                return socket;
            int xSize = socket.GetLength(0), ySize = socket.GetLength(1), zSize = socket.GetLength(2);

            for(int z = 0; z < zSize; z++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int x = 0; x < xSize; x++)
                    {
                        if(plug[x, y, z] > 257 - VoxelLogic.wcolorcount * 4 && (254 - plug[x, y, z]) == matchColor * 4)
                        {
                            plugX = x;
                            plugY = y;
                            plugZ = z;
                            goto END_PLUG;
                        }
                    }
                }
            }
            END_PLUG:
            if(plugX < 0)
                return socket;

            for(int z = 0; z < zSize; z++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int x = 0; x < xSize; x++)
                    {
                        if(socket[x, y, z] > 257 - VoxelLogic.wcolorcount * 4 && (254 - socket[x, y, z]) == matchColor * 4)
                        {
                            socketX = x;
                            socketY = y;
                            socketZ = z;
                            goto END_SOCKET;
                        }
                    }
                }
            }
            END_SOCKET:
            byte[,,] working = socket.Replicate();

            for(int z = 0; z < zSize; z++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int x = 0; x < xSize; x++)
                    {
                        if(plug[x, y, z] > 0 &&
                           x - plugX + socketX >= 0 && y - plugY + socketY >= 0 && z - plugZ + socketZ >= 0 &&
                           x - plugX + socketX < xSize && y - plugY + socketY < ySize && z - plugZ + socketZ < zSize)
                        {
                            working[(x - plugX + socketX),
                                    (y - plugY + socketY),
                                    (z - plugZ + socketZ)] = plug[x, y, z];
                        }
                    }
                }
            }
            return working;

        }
        public static byte[,,] MergeVoxels(byte[,,] plug, byte[,,] socket, int matchColor, int removeColor, int replaceColor)
        {
            int plugX = -1, plugY = -1, plugZ = -1, socketX = -1, socketY = -1, socketZ = -1;
            if(socket == null)
                return plug;
            else if(plug == null)
                return socket;
            int xSize = socket.GetLength(0), ySize = socket.GetLength(1), zSize = socket.GetLength(2);

            for(int z = 0; z < zSize; z++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int x = 0; x < xSize; x++)
                    {
                        if(plug[x, y, z] > 257 - VoxelLogic.wcolorcount * 4 && (254 - plug[x, y, z]) == matchColor * 4)
                        {
                            plugX = x;
                            plugY = y;
                            plugZ = z;
                            goto END_PLUG;
                        }
                    }
                }
            }
            END_PLUG:
            if(plugX < 0)
                return socket;

            for(int z = 0; z < zSize; z++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int x = 0; x < xSize; x++)
                    {
                        if(socket[x, y, z] > 257 - VoxelLogic.wcolorcount * 4 && (254 - socket[x, y, z]) == matchColor * 4)
                        {
                            socketX = x;
                            socketY = y;
                            socketZ = z;
                            goto END_SOCKET;
                        }
                    }
                }
            }
            END_SOCKET:
            byte[,,] working = socket.Replicate();

            for(int z = 0; z < zSize; z++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int x = 0; x < xSize; x++)
                    {
                        if(plug[x, y, z] > 0 &&
                           x - plugX + socketX >= 0 && y - plugY + socketY >= 0 && z - plugZ + socketZ >= 0 &&
                           x - plugX + socketX < xSize && y - plugY + socketY < ySize && z - plugZ + socketZ < zSize)
                        {
                            working[(x - plugX + socketX),
                                    (y - plugY + socketY),
                                    (z - plugZ + socketZ)] = plug[x, y, z];
                        }
                    }
                }
            }
            for(int z = 0; z < zSize; z++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int x = 0; x < xSize; x++)
                    {
                        if(254L - working[x, y, z] == removeColor * 4L)
                        {
                            working[x, y, z] = (byte)replaceColor;
                        }
                    }
                }
            }
            return working;

        }

        public static byte[,,] VoxListToArray(IEnumerable<MagicaVoxelData> voxelData, int xSize, int ySize, int zSize)
        {
            byte[,,] data = new byte[xSize, ySize, zSize];
            foreach(MagicaVoxelData mvd in voxelData)
            {
                data[mvd.x, mvd.y, mvd.z] = mvd.color;
            }
            return data;
        }
        public static List<MagicaVoxelData> VoxArrayToList(byte[,,] voxelData)
        {
            List<MagicaVoxelData> vlist = new List<MagicaVoxelData>(voxelData.Length);
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);

            for(byte x = 0; x < xSize; x++)
            {
                for(byte y = 0; y < ySize; y++)
                {
                    for(byte z = 0; z < zSize; z++)
                    {
                        if(voxelData[x, y, z] > 0)
                            vlist.Add(new MagicaVoxelData { x = x, y = y, z = z, color = voxelData[x, y, z] });
                    }
                }
            }
            return vlist;
        }

        public static byte[,,] VoxListToLargerArray(IEnumerable<MagicaVoxelData> voxelData, int multiplier, int xSize, int ySize, int zSize)
        {
            byte[,,] data = new byte[xSize * multiplier, ySize * multiplier, zSize * multiplier];
            foreach(MagicaVoxelData mvd in voxelData)
            {
                for(int x = 0; x < multiplier; x++)
                {
                    for(int y = 0; y < multiplier; y++)
                    {
                        for(int z = 0; z < multiplier; z++)
                        {
                            if(mvd.color != CURedux.emitter0 && mvd.color != CURedux.trail0 && mvd.color != CURedux.emitter1 && mvd.color != CURedux.trail1 && mvd.color != VoxelLogic.clear)
                                data[mvd.x * multiplier + x, mvd.y * multiplier + y, mvd.z * multiplier + z] = mvd.color;

                        }
                    }
                }
            }
            return data;
        }
        public static byte[,,] Translate(byte[,,] voxelData, int xMove, int yMove, int zMove)
        {
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);

            byte[,,] vs = new byte[xSize, ySize, zSize];

            int xmin = 0, ymin = 0, zmin = 0,
                xmax = xSize, ymax = ySize, zmax = zSize;
            if(xMove < 0)
                xmin -= xMove;
            else if(xMove > 0)
                xmax -= xMove;
            if(yMove < 0)
                ymin -= yMove;
            else if(yMove > 0)
                ymax -= yMove;
            if(zMove < 0)
                zmin -= zMove;
            else if(zMove > 0)
                zmax -= zMove;

            for(int x = xmin; x < xmax; x++)
            {
                for(int y = ymin; y < ymax; y++)
                {
                    for(int z = zmin; z < zmax; z++)
                    {
                        vs[x + xMove, y + yMove, z + zMove] = voxelData[x, y, z];
                    }
                }
            }

            return vs;

        }
        public static byte[,,] RunCA(byte[,,] voxelData, int smoothLevel)
        {
            if(smoothLevel <= 1)
                return voxelData;
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);
            Dictionary<byte, int> colorCount = new Dictionary<byte, int>();
            byte[][,,] vs = new byte[smoothLevel][,,];
            vs[0] = voxelData.Replicate();
            for(int v = 1; v < smoothLevel; v++)
            {
                vs[v] = new byte[xSize, ySize, zSize];
                for(int x = 0; x < xSize; x++)
                {
                    for(int y = 0; y < ySize; y++)
                    {
                        for(int z = 0; z < zSize; z++)
                        {
                            colorCount.Clear();
                            int emptyCount = 0;
                            if(x == 0 || y == 0 || z == 0 || x == xSize - 1 || y == ySize - 1 || z == zSize - 1
                                || (254 - vs[v - 1][x, y, z]) % 4 == 0)
                            {
                                colorCount[vs[v - 1][x, y, z]] = 100;
                            }
                            else
                            {
                                for(int xx = -1; xx < 2; xx++)
                                {
                                    for(int yy = -1; yy < 2; yy++)
                                    {
                                        for(int zz = -1; zz < 2; zz++)
                                        {
                                            byte smallColor = vs[v - 1][x + xx, y + yy, z + zz];
                                            if(smallColor == 0)
                                            {
                                                emptyCount++;
                                            }
                                            else if(colorCount.ContainsKey(smallColor))
                                            {
                                                colorCount[smallColor]++;
                                            }
                                            else
                                            {
                                                colorCount[smallColor] = 1;
                                            }
                                        }
                                    }
                                }
                            }
                            if(emptyCount > 17)
                                vs[v][x, y, z] = 0;
                            else
                                vs[v][x, y, z] = colorCount.OrderByDescending(kv => kv.Value).First().Key;
                        }
                    }
                }
            }
            return vs[smoothLevel - 1];
        }
        public static List<MagicaVoxelData> VoxLargerArrayToList(byte[,,] voxelData, int multiplier)
        {
            return VoxLargerArrayToList(voxelData, multiplier, multiplier, multiplier);
        }

        public static byte[,,] SealGaps(byte[,,] voxelData)
        {
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);
            byte[,,] vls = voxelData.Replicate();
            for(int z = 0; z < zSize - 1; z++)
            {
                for(int y = 1; y < ySize - 1; y++)
                {
                    for(int x = 1; x < xSize - 1; x++)
                    {
                        if(voxelData[x - 1, y, z] > 0 && voxelData[x, y, z + 1] > 0 && voxelData[x - 1, y, z + 1] == 0)
                            vls[x, y, z] = voxelData[x - 1, y, z];
                        else if(voxelData[x + 1, y, z] > 0 && voxelData[x, y, z + 1] > 0 && voxelData[x + 1, y, z + 1] == 0)
                            vls[x, y, z] = voxelData[x + 1, y, z];
                        else if(voxelData[x, y - 1, z] > 0 && voxelData[x, y, z + 1] > 0 && voxelData[x, y - 1, z + 1] == 0)
                            vls[x, y, z] = voxelData[x, y - 1, z];
                        else if(voxelData[x, y + 1, z] > 0 && voxelData[x, y, z + 1] > 0 && voxelData[x, y + 1, z + 1] == 0)
                            vls[x, y, z] = voxelData[x, y + 1, z];
                    }
                }
            }
            return vls;
        }


        public static byte[,,] Shrink(byte[,,] voxelData, int multiplier)
        {
            return Shrink(voxelData, multiplier, multiplier, multiplier);
        }
        public static byte[,,] Shrink(byte[,,] voxelData, int xmultiplier, int ymultiplier, int zmultiplier)
        {
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);
            Dictionary<byte, int> colorCount = new Dictionary<byte, int>();

            byte[,,] vfinal = new byte[xSize / xmultiplier, ySize / ymultiplier, zSize / zmultiplier];
            Dictionary<byte, MagicaVoxelData> specialColors = new Dictionary<byte, MagicaVoxelData>();
            for(int x = 0; x < xSize / xmultiplier; x++)
            {
                for(int y = 0; y < ySize / ymultiplier; y++)
                {
                    for(int z = 0; z < zSize / zmultiplier; z++)
                    {
                        colorCount = new Dictionary<byte, int>();
                        int fullCount = 0;
                        int emptyCount = 0;
                        int specialCount = 0;
                        for(int xx = 0; xx < xmultiplier; xx++)
                        {
                            for(int yy = 0; yy < ymultiplier; yy++)
                            {
                                for(int zz = 0; zz < zmultiplier; zz++)
                                {
                                    byte smallColor = voxelData[x * xmultiplier + xx, y * ymultiplier + yy, z * zmultiplier + zz];
                                    if((254 - smallColor) % 4 == 0)
                                    {
                                        specialCount++;
                                        if(specialColors.ContainsKey(smallColor))
                                        {
                                            if(specialColors[smallColor].color < specialCount)
                                            {
                                                specialColors[smallColor] = new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)z, color = (byte)specialCount };
                                            }
                                        }
                                        else
                                        {
                                            specialColors[smallColor] = new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)z, color = (byte)specialCount };
                                        }
                                    }
                                    if(smallColor > 0)
                                    {
                                        if(colorCount.ContainsKey(smallColor))
                                        {
                                            colorCount[smallColor] = colorCount[smallColor] + 16;
                                        }
                                        else
                                        {
                                            colorCount[smallColor] = 16;
                                        }
                                        fullCount += 16;
                                    }
                                    else
                                    {
                                        emptyCount += 5;
                                    }
                                }
                            }
                        }
                        byte best = 0;
                        if(fullCount >= emptyCount)
                            best = colorCount.OrderByDescending(kv => kv.Value).First().Key;
                        if(best > 0)
                        {
                            vfinal[x, y, z] = best;
                        }
                    }
                }
            }
            foreach(var kv in specialColors)
            {
                vfinal[kv.Value.x, kv.Value.y, kv.Value.z] = kv.Key;
            }
            return vfinal;
        }
        public static List<MagicaVoxelData> VoxLargerArrayToList(byte[,,] voxelData, int xmultiplier, int ymultiplier, int zmultiplier)
        {
            return VoxArrayToList(RunCA(voxelData, 3));
        }

        public static byte[,,] TransformStartLarge(IEnumerable<MagicaVoxelData> voxels)
        {
            return VoxListToLargerArray(voxels, 4, 60, 60, 60);
        }
        public static byte[,,] TransformStartHuge(IEnumerable<MagicaVoxelData> voxels)
        {
            return VoxListToLargerArray(voxels, 4, 120, 120, 80);
        }
        public static byte[,,] TransformStartLarge(IEnumerable<MagicaVoxelData> voxels, int multiplier)
        {
            return VoxListToLargerArray(voxels, multiplier, 60, 60, 60);
        }
        public static byte[,,] TransformStartHuge(IEnumerable<MagicaVoxelData> voxels, int multiplier)
        {
            return VoxListToLargerArray(voxels, multiplier, 120, 120, 80);
        }
        public static byte[,,] TransformStart(IEnumerable<MagicaVoxelData> voxels, int xSize, int ySize, int zSize)
        {
            return VoxListToLargerArray(voxels, 4, xSize, ySize, zSize);
        }
        public static List<MagicaVoxelData> TransformEnd(byte[,,] colors)
        {
            return VoxLargerArrayToList(colors, 4);
        }

        public static byte[,,] RotateYaw(byte[,,] colors, int amount)
        {
            int xSize = colors.GetLength(0), ySize = colors.GetLength(1), zSize = colors.GetLength(2);

            byte[,,] vls = new byte[xSize, ySize, zSize];
            switch(amount / 90)
            {
                case 0:
                    vls = colors.Replicate();
                    break;
                case 1:
                    for(int x = 0; x < xSize; x++)
                    {
                        for(int y = 0; y < ySize; y++)
                        {
                            for(int z = 0; z < zSize; z++)
                            {
                                vls[y, xSize - x - 1, z] = colors[x,y,z];
                            }
                        }
                    }
                    break;
                case 2:
                    for(int x = 0; x < xSize; x++)
                    {
                        for(int y = 0; y < ySize; y++)
                        {
                            for(int z = 0; z < zSize; z++)
                            {
                                vls[xSize - x - 1,  ySize - y - 1, z] = colors[x, y, z];
                            }
                        }
                    }
                    break;
                case 3:
                    for(int x = 0; x < xSize; x++)
                    {
                        for(int y = 0; y < ySize; y++)
                        {
                            for(int z = 0; z < zSize; z++)
                            {
                                vls[ySize - y - 1, x, z] = colors[x, y, z];
                            }
                        }
                    }
                    break;
            }
            return vls;
        }
        public static byte[,,] RotateYawPartial(byte[,,] colors, int degrees)
        {
            if(degrees % 90 == 0)
                return RotateYaw(colors, degrees);
            int xSize = colors.GetLength(0), ySize = colors.GetLength(1), zSize = colors.GetLength(2);

            byte[,,] vls = new byte[xSize, ySize, zSize];

            double angle = (Math.PI / 180) * ((degrees + 720) % 360);
            double sn = Math.Sin(angle), cs = Math.Cos(angle);

            for(int x = 0; x < xSize; x++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    int tempX = (x - (xSize / 2));
                    int tempY = (y - (ySize / 2));
                    int x2 = (int)Math.Round((cs * tempX) + (sn * tempY) + (xSize / 2));
                    int y2 = (int)Math.Round((-sn * tempX) + (cs * tempY) + (ySize / 2));

                    for(int z = 0; z < zSize; z++)
                    {
                        if(x2 >= 0 && y2 >= 0 && x2 < xSize && y2 < ySize && colors[x, y, z] > 0)
                            vls[x2, y2, z] = colors[x, y, z];
                    }
                }
            }

            return vls;
        }
        public static byte[,,] RotatePitchPartial(byte[,,] colors, int degrees)
        {
            int xSize = colors.GetLength(0), ySize = colors.GetLength(1), zSize = colors.GetLength(2);

            byte[,,] vls = new byte[xSize, ySize, zSize];

            double angle = (Math.PI / 180) * ((degrees + 720) % 360);
            double sn = Math.Sin(angle), cs = Math.Cos(angle);

            for(int x = 0; x < xSize; x++)
            {
                for(int z = 0; z < zSize; z++)
                {
                    int tempX = (x - (xSize / 2));
                    int tempZ = (z - (zSize / 2));
                    int x2 = (int)Math.Round((cs * tempX) + (sn * tempZ) + (xSize / 2));
                    int z2 = (int)Math.Round((-sn * tempX) + (cs * tempZ) + (zSize / 2));

                    for(int y = 0; y < ySize; y++)
                    {
                        if(x2 >= 0 && z2 >= 0 && x2 < xSize && z2 < zSize && colors[x, y, z] > 0)
                            vls[x2, y, z2] = colors[x, y, z];
                    }
                }
            }

            return vls;
        }
        public static byte[,,] RotatePitchPartialSpread(byte[,,] colors, int degrees, int effectStartDegrees, byte[] spreadColor)
        {
            int xSize = colors.GetLength(0), ySize = colors.GetLength(1), zSize = colors.GetLength(2);

            byte[,,] vls = new byte[xSize, ySize, zSize];
            double finalAngle = (Math.PI / 180) * ((degrees) % 360), effectStartAngle = (Math.PI / 180) * ((effectStartDegrees) % 360);
            double increm = Math.PI / 36;

            if((degrees - effectStartDegrees + 1440) % 360 < 180)
            {
                increm *= -1.0;
            }

            double sn = 0.0, cs = 0.0;

            sn = Math.Sin(finalAngle);
            cs = Math.Cos(finalAngle);
            for(int x = 0; x < xSize; x++)
            {
                for(int z = 0; z < zSize; z++)
                {
                    int tempX = (x - (xSize / 2));
                    int tempZ = (z - (zSize / 2));
                    int x2 = (int)Math.Round((cs * tempX) + (sn * tempZ) + (xSize / 2));
                    int z2 = (int)Math.Round((-sn * tempX) + (cs * tempZ) + (zSize / 2));

                    for(int y = 0; y < ySize; y++)
                    {
                        if(x2 >= 0 && z2 >= 0 && x2 < xSize && z2 < zSize && colors[x, y, z] > 0)
                            vls[x2, y, z2] = colors[x, y, z];
                    }
                }
            }
            int sclen = 1;
            if(spreadColor != null) sclen = spreadColor.Length;

            for(double angle = finalAngle; Math.Abs(effectStartAngle - angle) >= Math.Abs(increm); angle += increm)
            {
                sn = Math.Sin(angle);
                cs = Math.Cos(angle);
                for(int x = 0; x < xSize; x++)
                {
                    for(int z = 0; z < zSize; z++)
                    {
                        int tempX = (x - (xSize / 2));
                        int tempZ = (z - (zSize / 2));
                        int x2 = (int)Math.Round((cs * tempX) + (sn * tempZ) + (xSize / 2));
                        int z2 = (int)Math.Round((-sn * tempX) + (cs * tempZ) + (zSize / 2));

                        for(int y = 0; y < ySize; y++)
                        {
                            for(int it = 0; it < sclen; it++)
                            {
                                if((spreadColor == null || colors[x, y, z] == spreadColor[it]) && colors[x, y, z] > 0 && x2 >= 0 && z2 >= 0 && x2 < xSize && z2 < zSize && vls[x2, y, z2] == 0)
                                    vls[x2, y, z2] = colors[x, y, z];
                            }
                        }
                    }
                }
            }
            return vls;
        }
        public static byte[,,] RotateRollPartial(byte[,,] colors, int degrees)
        {
            int xSize = colors.GetLength(0), ySize = colors.GetLength(1), zSize = colors.GetLength(2);

            byte[,,] vls = new byte[xSize, ySize, zSize];
            double angle = (Math.PI / 180) * ((degrees + 720) % 360);
            double sn = Math.Sin(angle), cs = Math.Cos(angle);

            for(int z = 0; z < zSize; z++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    int tempY = (y - (ySize / 2));
                    int tempZ = (z - (zSize / 2));
                    int y2 = (int)Math.Round((-sn * tempZ) + (cs * tempY) + (ySize / 2));
                    int z2 = (int)Math.Round((cs * tempZ) + (sn * tempY) + (zSize / 2));

                    for(int x = 0; x < xSize; x++)
                    {
                        if(z2 >= 0 && y2 >= 0 && z2 < zSize && y2 < ySize && colors[x, y, z] > 0)
                            vls[x, y2, z2] = colors[x, y, z];
                    }
                }
            }

            return vls;
        }

        public static byte[,,] ScalePartial(byte[,,] colors, double xScale, double yScale, double zScale)
        {
            int xSize = colors.GetLength(0), ySize = colors.GetLength(1), zSize = colors.GetLength(2);

            if(xScale <= 0 || yScale <= 0 || zScale <= 0)
                return colors;
            byte[,,] vls = new byte[(int)(xSize * xScale), (int)(ySize * yScale), (int)(zSize * zScale)];

            for(int z = 0; z < zSize; z++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int x = 0; x < xSize; x++)
                    {
                        for(double xsc = 0.0; xsc < xScale; xsc += 1.0)
                        {
                            for(double ysc = 0.0; ysc < yScale; ysc += 1.0)
                            {
                                for(double zsc = 0.0; zsc < zScale; zsc += 1.0)
                                {
                                    int tempX = (x - (xSize / 2));
                                    int tempY = (y - (ySize / 2));
                                    int tempZ = (z - (zSize / 2));
                                    int x2 = (int)Math.Round((xScale * tempX) + (xSize / 2) + ((tempX < 0) ? xsc : -xsc));
                                    int y2 = (int)Math.Round((yScale * tempY) + (ySize / 2) + ((tempY < 0) ? ysc : -ysc));
                                    int z2 = (int)Math.Round((zScale * tempZ) + (zSize / 2) + ((tempZ < 0) ? zsc : -zsc));

                                    if(colors[x, y, z] > 0 && x2 >= 0 && y2 >= 0 && z2 >= 0 && x2 < xSize && y2 < ySize && z2 < zSize)
                                        vls[x2, y2, z2] = colors[x, y, z];
                                }
                            }
                        }
                    }
                }
            }
            return vls;
        }

    }
}
