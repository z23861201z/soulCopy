using Server.Common;
using Server.Common.Data;
using Server.Common.IO;
using Server.Ghost.Characters;
using Server.Handler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Server.Ghost.Provider
{
    public static class MapFactory
    {
        private static List<Map> Maps { get; set; }
        public static List<Character> AllCharacters = new List<Character>();

        public static void Initialize()
        {
            Maps = new List<Map>();


            Log.Inform("=====读取数据库地图信息======");
            foreach (dynamic datum in new Datums("cn_map").Populate())
            {
                Maps.Add(new Map((short)datum.MapX, (short)datum.MapY));
            }

            foreach (Map Map in Maps)
            {
                //if ((Map.MapX == 1 && Map.MapY == 53) || (Map.MapX == 1 && Map.MapY == 54) || (Map.MapX == 1 && Map.MapY == 55))
                //    continue;
                try
                {
                    LoadMapPexelsData(Map);
                    ParsePrjFile(Map);
                }
                catch (IOException ex)
                {
                    //暂时屏蔽缺失地图提示
                  //  Log.Inform("缺失地图X="+Map.MapX +"Y=" + Map.MapY);
                }
                
                
            }
            Maps.Add(new Map(1, 53));
            Maps.Add(new Map(1, 54));
            Maps.Add(new Map(1, 55));
            Maps.Add(new Map(10, 62));
            Maps.Add(new Map(10, 64));
            Map M1 = GetMap(1, 53);
            Map M2 = GetMap(1, 54);
            Map M3 = GetMap(1, 55);
            Map M4 = GetMap(10, 62);
            Map M5 = GetMap(10, 64);
            M1 = GetMap(1, 52);
            M1 = GetMap(1, 52);
            M1 = GetMap(1, 52);
            M4 = GetMap(10, 61);
            M5 = GetMap(10, 63);
        }

        public static Map GetMap(short mapX, short mapY)
        {
            Map map = Maps.Find(i => (i.MapX == mapX && i.MapY == mapY));
            return map;
        }

        public static void LoadMapPexelsData(Map map)
        {
            string openPath = Application.LaunchPath + @"\Data\Map\t" + map.MapX + "_s" + map.MapY + ".map";

            FileStream file = File.Open(openPath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(file);

            sbyte Index = 0;
            sbyte Data;
            int Width = br.ReadInt32();
            int Height = br.ReadInt32();
            map.SetMapHeightWidth(Height, Width);
            for (int f = 0; f < Width; f++)
            {
                for (int i = 0; i < Height; i++)
                {
                    br.ReadByte();
                    br.ReadByte();
                    Index = br.ReadSByte();
                    map.SetMapPexel(f * 32 + f, i, Index);
                    for (int j = 0; j < 32; j++)
                    {
                        Data = br.ReadSByte();
                        map.SetMapPexel(f * 32 + f + 1 + j, i, Data);
                    }
                    br.ReadByte();
                }
            }

            ////////////////////////
            for (int f = 0; f < Width; f++)
            {
                for (int i = 0; i < Height; i++)
                {
                    br.ReadByte();
                    br.ReadByte();
                    Index = br.ReadSByte();
                    if (Index != -1)
                        map.SetMapPexel(f * 32 + f, i, Index);
                    for (int j = 0; j < 32; j++)
                    {
                        Data = br.ReadSByte();
                        if (Data != -1)
                            map.SetMapPexel(f * 32 + f + 1 + j, i, Data);
                    }
                    br.ReadByte();
                }
            }
            file.Close();
            br.Close();
        }

        public static void ParsePrjFile(Map Map)
        {
            string FileName = Application.LaunchPath + @"\Data\Project\t" + Map.MapX + "_s" + Map.MapY + ".prj";
            using (FileStream stream = new FileStream(FileName, FileMode.Open))
            {
                BinaryReader reader = new BinaryReader(stream);
                try
                {
                    Dictionary<int, string> prjStr = new Dictionary<int, string>();
                    ReadString str = new ReadString();
                    str.FiersRead = reader.ReadBytes(0xA0);
                    str.Decode();
                    string name = str.Name;
                    int val1 = reader.ReadInt32();
                    int val2 = reader.ReadInt32();
                    int val3 = reader.ReadInt32();

                    //=========================================sub_655150
                    int strCount = reader.ReadInt32();

                    if (strCount > 0)
                    {
                        for (int i = 0; i < strCount; i++)
                        {
                            str.FiersRead = reader.ReadBytes(0x100);
                            str.Decode();

                        }
                    }
                    str.FiersRead = reader.ReadBytes(0x100);
                    str.Decode();

                    str.FiersRead = reader.ReadBytes(0x100);
                    str.Decode();


                    strCount = reader.ReadInt32();
                    if (strCount > 0)
                    {
                        for (int i = 0; i < strCount; i++)
                        {
                            str.FiersRead = reader.ReadBytes(0x100);
                            str.Decode();

                        }
                    }
                    strCount = reader.ReadInt32();
                    if (strCount > 0)
                    {
                        for (int i = 0; i < strCount; i++)
                        {
                            str.FiersRead = reader.ReadBytes(0x100);
                            str.Decode();

                        }
                    }

                    //=========================================sub_6531D0
                    int val5 = reader.ReadInt32();
                    int val6 = reader.ReadInt32();
                    int val7 = reader.ReadInt32();
                    str.FiersRead = reader.ReadBytes(0x100);
                    str.Decode();

                    str.FiersRead = reader.ReadBytes(0x100);
                    str.Decode();

                    strCount = reader.ReadInt32();
                    if (strCount > 0)
                    {
                        for (int i = 0; i < strCount; i++)
                        {
                            str.FiersRead = reader.ReadBytes(0x100);
                            str.Decode();

                        }
                    }
                    //=========================================(1)sub_652FA0
                    strCount = reader.ReadInt32();
                    if (strCount > 0)
                    {
                        for (int i = 0; i < strCount; i++)
                        {
                            str.FiersRead = reader.ReadBytes(0x100);
                            str.Decode();

                            reader.ReadBytes(0x10);
                            reader.ReadInt32();
                            reader.ReadInt32();
                            reader.ReadInt32();
                            reader.ReadInt32();
                        }
                    }
                    //=========================================(2)
                    strCount = reader.ReadInt32();
                    if (strCount > 0)
                    {
                        for (int i = 0; i < strCount; i++)
                        {
                            reader.ReadInt32();
                            reader.ReadInt32();
                            reader.ReadInt32();
                            reader.ReadInt32();
                        }
                    }
                    //=========================================(3)sub_653460
                    //地圖物件
                    strCount = reader.ReadInt32();
                    if (strCount > 0)
                    {
                        for (int i = 0; i < strCount; i++)
                        {
                            str.FiersRead = reader.ReadBytes(0x100);
                            str.Decode();

                            reader.ReadInt32();
                            reader.ReadInt32();
                            str.FiersRead = reader.ReadBytes(0x100);
                            str.Decode();

                            str.FiersRead = reader.ReadBytes(0x100);
                            str.Decode();

                            reader.ReadInt32();
                            reader.ReadInt32();
                            reader.ReadInt32();
                            reader.ReadByte();
                            reader.ReadBytes(0x14);
                        }
                    }
                    //=========================================(4)sub_653750
                    //怪物
                    strCount = reader.ReadInt32();
                    if (strCount > 0)
                    {
                        for (int i = 0; i < strCount; i++)
                        {
                            str.FiersRead = reader.ReadBytes(0x100);
                            str.Decode();

                            int mv1 = reader.ReadInt32();
                            int mv2 = reader.ReadInt32();
                            str.FiersRead = reader.ReadBytes(0x100);
                            str.Decode();

                            str.FiersRead = reader.ReadBytes(0x100);
                            str.Decode();

                            int mv3 = reader.ReadInt32();
                            int mv4 = reader.ReadInt32();
                            int mv5 = reader.ReadInt32();
                            int mv6 = reader.ReadInt32();
                            int mv7 = reader.ReadByte();
                            int mv8 = reader.ReadInt32();
                            int mv9 = reader.ReadInt32();
                            reader.ReadBytes(0x14);
                        }
                    }
                    //=========================================(5)sub_653A20
                    //讀取NPC資訊
                    strCount = reader.ReadInt32();
                    if (strCount > 0)
                    {
                        for (int i = 0; i < strCount; i++)
                        {
                            str.FiersRead = reader.ReadBytes(0x100);
                            str.Decode();

                            reader.ReadInt32();
                            reader.ReadInt32();
                            str.FiersRead = reader.ReadBytes(0x100);
                            str.Decode();

                            str.FiersRead = reader.ReadBytes(0x100);
                            str.Decode();
                            reader.ReadInt32();
                            reader.ReadByte();
                            reader.ReadInt16();
                            reader.ReadInt16();
                            reader.ReadInt32();
                            reader.ReadBytes(0xC);
                        }
                    }
                    //=========================================(6)sub_653CF0
                    strCount = reader.ReadInt32();
                    if (strCount > 0)
                    {
                        for (int i = 0; i < strCount; i++)
                        {
                            str.FiersRead = reader.ReadBytes(0x100);
                            str.Decode();
                            reader.ReadInt32();
                            reader.ReadInt32();
                            str.FiersRead = reader.ReadBytes(0x100);
                            str.Decode();
                            str.FiersRead = reader.ReadBytes(0x100);
                            str.Decode();
                            reader.ReadInt32();
                            reader.ReadInt32();
                            reader.ReadInt32();
                            reader.ReadInt32();
                            reader.ReadByte();
                            reader.ReadBytes(0x14);
                        }
                    }
                    //=========================================(7)sub_653E70
                    strCount = reader.ReadInt32();
                    reader.ReadInt32();
                    reader.ReadInt32();
                    if (strCount > 0)
                    {
                        for (int i = 0; i < strCount; i++)
                        {
                            str.FiersRead = reader.ReadBytes(0x28);
                            str.Decode();
                            reader.ReadInt32();
                            reader.ReadByte();
                            reader.ReadInt32();
                            reader.ReadInt32();
                            reader.ReadInt32();
                        }
                    }
                    //=========================================(8)sub_653E70
                    strCount = reader.ReadInt32();
                    reader.ReadInt32();
                    reader.ReadInt32();
                    if (strCount > 0)
                    {
                        for (int i = 0; i < strCount; i++)
                        {
                            str.FiersRead = reader.ReadBytes(0x28);
                            str.Decode();
                            reader.ReadInt32();
                            reader.ReadByte();
                            reader.ReadInt32();
                            reader.ReadInt32();
                            reader.ReadInt32();
                        }
                    }
                    //=========================================(9)sub_653E70
                    strCount = reader.ReadInt32();
                    reader.ReadInt32();
                    reader.ReadInt32();
                    if (strCount > 0)
                    {
                        for (int i = 0; i < strCount; i++)
                        {
                            str.FiersRead = reader.ReadBytes(0x28);
                            str.Decode();
                            reader.ReadInt32();
                            reader.ReadByte();
                            reader.ReadInt32();
                            reader.ReadInt32();
                            reader.ReadInt32();
                        }
                    }
                    //=========================================(10)sub_653E70
                    strCount = reader.ReadInt32();
                    if (strCount > 0)
                    {
                        for (int i = 0; i < strCount; i++)
                        {
                            str.FiersRead = reader.ReadBytes(0x28);
                            str.Decode();
                            reader.ReadInt32();
                            reader.ReadByte();
                            reader.ReadInt32();
                            reader.ReadInt32();
                            reader.ReadInt32();
                        }
                    }
                    //=========================================(11)sub_653E70
                    strCount = reader.ReadInt32();
                    if (strCount > 0)
                    {
                        for (int i = 0; i < strCount; i++)
                        {
                            str.FiersRead = reader.ReadBytes(0x28);
                            str.Decode();
                            reader.ReadInt32();
                            reader.ReadByte();
                            reader.ReadInt32();
                            reader.ReadInt32();
                            reader.ReadInt32();
                        }
                    }
                    //=========================================(12)sub_653E70
                    strCount = reader.ReadInt32();
                    if (strCount > 0)
                    {
                        for (int i = 0; i < strCount; i++)
                        {
                            str.FiersRead = reader.ReadBytes(0x28);
                            str.Decode();
                            reader.ReadInt32();
                            reader.ReadByte();
                            reader.ReadInt32();
                            reader.ReadInt32();
                            reader.ReadInt32();
                        }
                    }
                    //=========================================(13)sub_67B180
                    //怪物資訊-- todo 2022年4月28日 不知道为什么不生效
                    strCount = reader.ReadInt32();
                    if (strCount > 0)
                    {
                        for (int i = 0; i < strCount; i++)
                        {
                            str.FiersRead = reader.ReadBytes(0x28);
                            str.Decode();
                            var MonsterID = reader.ReadInt32();
                            var Direction = reader.ReadByte();
                            var Speed = BitConverter.ToSingle(reader.ReadBytes(4), 0);
                            var PosX = reader.ReadInt32();
                            var PosY = reader.ReadInt32();
                            int ss = reader.ReadInt32();
                            int ss2 = 0;
                            do
                            {
                                reader.ReadInt32();
                                reader.ReadInt32();
                                ++ss2;
                            } while (ss2 < ss);
                            //
                            char[] Value = MonsterID.ToString().ToCharArray();
                            char[] MonsterLevel = new char[4];
                            MonsterLevel[0] = Value[1];
                            MonsterLevel[1] = Value[2];
                            MonsterLevel[2] = Value[3];
                            MonsterLevel[3] = Value[4];
                            //
                            int Level = int.Parse(new string(MonsterLevel));
                            int MaxHP = MobFactory.MonsterMaxHP(MonsterID);
                            int Exp = MobFactory.MonsterExp(MonsterID);
                            byte MoveType = MobFactory.MoveType(MonsterID);
                            byte AttackType = MobFactory.AttackType(MonsterID);
                            int Attack1 = MobFactory.Attack1(MonsterID);
                            int Attack2 = MobFactory.Attack2(MonsterID);
                            int CrashAttack = MobFactory.CrashAttack(MonsterID);
                            int Defense = MobFactory.Defense(MonsterID);
                            byte AddEffect = MobFactory.AddEffect(MonsterID);
                            Monster Monster = new Monster(i, MonsterID, Level, MaxHP, MaxHP, 0, Exp, MoveType == 0 ? 0 : Speed, Direction, MoveType, AttackType, Attack1, Attack2, CrashAttack, Defense, MoveType == 0 ? (byte)0 : (byte)1, 0, AddEffect, PosX, PosY, true);
                            Map.Monster.Add(Monster);

                            // 修正怪物座標
                            sbyte NextPosition = Map.GetMapPexel(Monster.PositionX, Monster.PositionY);
                            while (NextPosition == -1)
                            {
                                Monster.PositionY = (Monster.PositionY / 32);
                                Monster.PositionY += 1;
                                Monster.PositionY *= 32;
                                sbyte Next = Map.GetMapPexel(Monster.PositionX, Monster.PositionY);
                                if (Next != -1)
                                    break;
                            }
                        }
                    }
                    for (int j = Map.Monster.Count; j < 50; j++)
                        Map.Monster.Add(null);
                    //=========================================(14)sub_653F60
                    strCount = reader.ReadInt32();
                    if (strCount > 0)
                    {
                        for (int i = 0; i < strCount; i++)
                        {
                            str.FiersRead = reader.ReadBytes(0x28);
                            str.Decode();
                            reader.ReadInt32();
                            reader.ReadByte();
                            reader.ReadInt32();
                            reader.ReadInt32();
                            reader.ReadInt32();
                        }
                    }
                    //=========================================(15)sub_654050
                    strCount = reader.ReadInt32();
                    if (strCount > 0)
                    {
                        for (int i = 0; i < strCount; i++)
                        {
                            str.FiersRead = reader.ReadBytes(0x28);
                            str.Decode();
                            reader.ReadInt32();
                            reader.ReadByte();
                            reader.ReadInt32();
                            reader.ReadInt32();
                            reader.ReadInt32();
                        }
                    }
                    //=========================================(16)sub_653E70
                    strCount = reader.ReadInt32();
                    if (strCount > 0)
                    {
                        for (int i = 0; i < strCount; i++)
                        {
                            str.FiersRead = reader.ReadBytes(0x28);
                            str.Decode();
                            reader.ReadInt32();
                            reader.ReadByte();
                            reader.ReadInt32();
                            reader.ReadInt32();
                            reader.ReadInt32();
                        }
                    }
                    //=========================================(17)sub_653E70
                    strCount = reader.ReadInt32();
                    reader.ReadInt32();
                    reader.ReadInt32();
                    if (strCount > 0)
                    {
                        for (int i = 0; i < strCount; i++)
                        {
                            str.FiersRead = reader.ReadBytes(0x28);
                            str.Decode();
                            reader.ReadInt32();
                            reader.ReadByte();
                            reader.ReadInt32();
                            reader.ReadInt32();
                            reader.ReadInt32();
                        }
                    }
                    //=========================================

                }
                catch (Exception)
                {
                }
            }
        }

        private class ReadString
        {
            public byte[] FiersRead = new byte[0x100];
            public byte[] SecondRead = new byte[0x100];
            public string Descrption;
            public string Name;

            public void Decode()
            {
                int index = 0;
                index = 0;
                while (index < 0x100)
                {
                    if (FiersRead[index + 2] == 0 && FiersRead[index] == 0 && FiersRead[index + 1] == 0)
                    {
                        index++;
                        break;
                    }
                    index++;
                }
                string str = Encoding.Unicode.GetString(FiersRead, 0, index);
                Name = str;
                str = "";
                index = 0;
                while (index < 0x100)
                {
                    if (SecondRead[index + 2] == 0 && SecondRead[index] == 0 && SecondRead[index + 1] == 0)
                    {
                        index++;
                        break;
                    }
                    index++;
                }
                str = Encoding.Unicode.GetString(SecondRead, 0, index);
                Descrption = str;
            }
        }
    }
}
