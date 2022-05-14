using Server.Common.Data;
using Server.Common.IO;
using System.Collections.Generic;
using System.Collections;

namespace Server.Ghost.Provider
{
    public static class MobFactory
    {
        //掉落清单（
        public static List<Loot> Drop_Data = new List<Loot>();

        //包含怪物
        public static Dictionary<int, Loot> Mob_Map = new Dictionary<int, Loot>();

        public static int MonsterMaxHP(int MonsterID)
        {
            int MonsterHP = short.MaxValue;
            foreach (KeyValuePair<int, Loot> kvp in Mob_Map)
            {
                if (kvp.Key == MonsterID)
                {
                    return kvp.Value.MobHp; ;
                }
            }
            return MonsterHP;
        }

        public static int MonsterExp(int MonsterID)
        {
            int MonsterExp = 0;
            foreach (KeyValuePair<int, Loot> kvp in Mob_Map)
            {
                if(kvp.Key == MonsterID)
                {
                    return kvp.Value.MobExp; ;
                }
            }
            return MonsterExp;
        }

        public static byte MoveType(int MonsterID)
        {
            byte MoveType = 1;
            foreach (KeyValuePair<int, Loot> kvp in Mob_Map)
            {
                if (kvp.Key == MonsterID)
                {
                    return ((byte)kvp.Value.MoveType);
                }
            }
            return MoveType;
        }

        public static byte AttackType(int MonsterID)
        {
            byte AttackType = 1;
            foreach (KeyValuePair<int, Loot> kvp in Mob_Map)
            {
                if (kvp.Key == MonsterID)
                {
                    return ((byte)kvp.Value.AttckType);
                }
            }
            return AttackType;
        }

        public static int Attack1(int MonsterID)
        {
            int Attack = short.MaxValue;
            foreach (KeyValuePair<int, Loot> kvp in Mob_Map)
            {
                if (kvp.Key == MonsterID)
                {
                    return kvp.Value.MobAtt1;
                }
            }
            return Attack;
        }

        public static int Attack2(int MonsterID)
        {
            int Attack = short.MaxValue;
            foreach (KeyValuePair<int, Loot> kvp in Mob_Map)
            {
                if (kvp.Key == MonsterID)
                {
                    return kvp.Value.MobAtt2;
                }
            }
            return Attack;
        }

        public static int CrashAttack(int MonsterID)
        {
            int Attack = short.MaxValue;
            foreach (KeyValuePair<int, Loot> kvp in Mob_Map)
            {
                if (kvp.Key == MonsterID)
                {
                    return kvp.Value.MobCrashAtt;
                }
            }
            return Attack;
        }

        public static int Defense(int MonsterID)
        {
            int Defense = short.MaxValue;
            foreach (KeyValuePair<int, Loot> kvp in Mob_Map)
            {
                if (kvp.Key == MonsterID)
                {
                    return kvp.Value.MobDefence;
                }
            }
            return Defense;
        }

        public static byte AddEffect(int MonsterID)
        {
            byte Effect = 0;
            foreach (KeyValuePair<int, Loot> kvp in Mob_Map)
            {
                if (kvp.Key == MonsterID)
                {
                    return (byte)kvp.Value.AddEffect;
                }
            }
            return Effect;
        }

        public static void InitializeMonsterDrop()
        {
            using (Log.Load("Loading Drops"))
            {

                Log.Inform("=====读取数据库怪物信息====");
                foreach (dynamic datum in new Datums("cn_mob_list").Populate())
                {
                    
                    Loot mob = new Loot(datum.id, datum.MobID, datum.MobHp,
                                datum.MobExp , datum.AttckType , datum.MoveType,
                                datum.MobAtt1 , datum.MobAtt2 , datum.MobCrashAtt ,
                                datum.MobDefence, datum.AddEffect);
                    Mob_Map.Add(datum.MobID, mob);
                }

                Log.Inform("=====读取数据库掉落物品信息====");
                foreach (dynamic datum in new Datums("drop_data").Populate())
                {
                    Loot newLoot = new Loot(datum);
                    Drop_Data.Add(newLoot);
                }

                
            }
        }
    }
}
