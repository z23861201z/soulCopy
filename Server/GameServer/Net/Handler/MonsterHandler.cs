using Server.Common.Constants;
using Server.Common.IO;
using Server.Common.IO.Packet;
using Server.Common.Threading;
using Server.Common.Utilities;
using Server.Ghost;
using Server.Ghost.Characters;
using Server.Ghost.Provider;
using Server.Net;
using Server.Packet;
using System;

namespace Server.Handler
{
    /***
     * 怪物相关请求处理
     */
    public static class MonsterHandler
    {
        public static void AttackMonster_Req(InPacket lea, Client gc)
        {
            //人物ID-怪物ID-坐标-攻击-技能编号
            short CharacterID = lea.ReadShort();
            short OriginalID = lea.ReadShort();
            lea.ReadShort();
            short Damage = lea.ReadShort();
            short HitX = lea.ReadShort();
            short HitY = lea.ReadShort();
            short SkillID = lea.ReadShort();
            var chr = gc.Character;
            Map Map = MapFactory.GetMap(chr.MapX, chr.MapY);
            //找不到怪物不管
            Monster Monster = Map.getMonsterByOriginalID(OriginalID);
            if (Monster == null)
                return;
            //1、怪物扣血
            Monster.HP -= Damage;

            //2、针对技能对怪物加上debuff
            switch (SkillID)
            {
                case 10108: // 點穴定身
                    if (Randomizer.Next(0, 2) == 0)
                        Monster.Effect = 1;
                    break;
                case 10204: // 餵毒術
                    if (Randomizer.Next(0, 2) == 0)
                        Monster.Effect = 2;
                    break;
                case 10304: // 玄冰擊
                case 10305: // 冰凍大地
                    if (Randomizer.Next(0, 2) == 0)
                        Monster.Effect = 5;
                    break;
                case 10306: // 矇蔽蝕眼
                    if (Randomizer.Next(0, 2) == 0)
                        Monster.Effect = 3;
                    break;
                default:
                    //Log.Inform("[Attack Monster] SkillID = {0}", SkillID);
                    break;
            }

            //3、怪物死亡
            if (Monster.HP <= 0)
            {
                monsterDeath(gc, Map, Monster);
            }
            else
            {
                Monster.State = 7;
                if (chr.PlayerX < HitX && Monster.Direction == 1)
                    Monster.Direction = 0xFF;
                else if (chr.PlayerX > HitX && Monster.Direction == 0xFF)
                    Monster.Direction = 1;
            }

            //告知地图所有人怪物状态
            foreach (Character All in Map.Characters)
            {
                MonsterPacket.spawnMonster(All.Client, Monster, CharacterID, Damage, HitX, HitY);
            }

          
        }
    
        /**
         *怪物死亡逻辑
         */
        public static void monsterDeath(Client gc , Map Map , Monster Monster)
        {
            var chr = gc.Character;

            Monster.State = 9;
            Monster.Effect = 0;
            //map.Monster.Remove(Monster);
            Monster.IsAlive = false;

            //获得打怪经验
            chr.Exp += Monster.Exp;
            //经验满了升级
            if (chr.Exp >= GameConstants.getExpNeededForLevel(chr.Level))
            {
                chr.LevelUp();
            }
            //修改经验
            StatusPacket.UpdateExp(gc);

            // 加入要掉落物品
            int Max_Count = 2; // 設定最大物品掉落數
            int Current_Count = 0;
            foreach (Loot loot in MobFactory.Drop_Data)
            {
                if (Current_Count == Max_Count)
                    break;

                if (loot.MobID == Monster.MonsterID)
                {
                    if ((Randomizer.Next(999999) / GameServer.Rates.Loot) < loot.Chance)
                    {
                        Monster.Drops.Add(new Drop(0, loot.ItemID, (short)Randomizer.Next(loot.MinimumQuantity, loot.MaximumQuantity)));
                        Current_Count++;
                    }
                }
            }

            // 加入要掉落靈魂
            //【藍色鬼魂】能恢復20%的鬼力值
            //【綠色鬼魂】能恢復40%的鬼力值
            //【紅色鬼魂】累積憤怒計量值用，當憤怒計滿之後能轉為憤怒狀態，攻防都會*1.2倍
            //【紫色鬼魂】能吸收到封印裝備，蒐集越多越能增加封印物合成的成功機率

            // 無 : 1%
            // 9900001 : 20%
            // 9900002 : 19%
            // 9900003 : 20%
            // 9900004 : 40%

            int[] Soul = { 0, 9900001, 9900002, 9900003, 9900004 };
            int rnd = Randomizer.Next(0, 4);
            if (rnd != 0)
            {
                Monster.Drops.Add(new Drop(0, Soul[rnd], 20));
            }

            // 掉落钱
            short rndMoney = (short)(Monster.Exp / 2 + Randomizer.Next(6));
            if (rndMoney != 0 && Monster.MonsterID != 1010002)
            {
                Monster.Drops.Add(new Drop(0, InventoryType.getMoneyStyle(rndMoney), rndMoney)); // 錢
            }

            // 掉落物品信息
            for (int i = 0; i < Monster.Drops.Count; i++)
            {
                Monster.Drops[i].PositionX = Monster.PositionX;
                Monster.Drops[i].PositionY = Monster.PositionY - 50;
                //Item it = new Item(Monster.Drops[i].ItemID, 0x63, 0x63, Monster.Drops[i].Quantity);
                Monster.Drops[i].ID = Map.ObjectID;
                Map.Item.Add(Map.ObjectID, new Drop(Map.ObjectID, Monster.Drops[i].ItemID, Monster.Drops[i].Quantity));
                Map.ObjectID++;
            }

            //告知地图全部人员物品掉落
            foreach (Character All in Map.Characters)
            {
                MapPacket.MonsterDrop(All.Client, Monster);
            }
            //清空怪物信息
            Monster.Drops.Clear();

            //怪物死亡停止计时器
            if (Monster.State == 9 && Monster.tmr1 != null)
            {
                Monster.tmr1.Cancel();
                Monster.tmr1 = null;
                return;
            }

            if (Monster.State == 9 && Monster.tmr2 != null)
            {
                Monster.tmr2.Cancel();
                Monster.tmr2 = null;
                return;
            }

            if (Monster.State == 9 && Monster.tmr3 != null)
            {
                Monster.tmr3.Cancel();
                Monster.tmr3 = null;
                return;
            }
        }
    
        /***
         * 怪物被攻击后的效果
         */
        public static void monsterBeHit(Monster Monster , Map Map, short CharacterID, short HitX, short HitY)
        {
            // 怪物被攻击后的效果
            int r = Randomizer.Next(0, 2);
            if (r == 0 && Monster.Effect == 0 && Monster.State == 7 && Monster.AttackType != 0 && Monster.tmr1 == null)
            {
                Monster.tmr1 = new Delay(600, false, () =>
                {
                    if (Monster.State == 9)
                    {
                        Monster.tmr1.Cancel();
                        Monster.tmr2.Cancel();
                        Monster.tmr3.Cancel();
                        Monster.tmr1 = null;
                        Monster.tmr2 = null;
                        Monster.tmr3 = null;
                        return;
                    }
                    Monster.State = 3;
                    foreach (Character All in Map.Characters)
                        MonsterPacket.spawnMonster(All.Client, Monster, CharacterID, 0, HitX, HitY);
                    Monster.tmr1 = null;

                    if (Monster.State == 3 && Monster.tmr2 == null)
                    {
                        Monster.tmr2 = new Delay(500, false, () =>
                        {
                            if (Monster.State == 9)
                            {
                                Monster.tmr1.Cancel();
                                Monster.tmr2.Cancel();
                                Monster.tmr3.Cancel();
                                Monster.tmr1 = null;
                                Monster.tmr2 = null;
                                Monster.tmr3 = null;
                                return;
                            }
                            Monster.State = (Monster.MoveType == 0 ? (byte)0 : (byte)1);
                            foreach (Character All in Map.Characters)
                                MonsterPacket.spawnMonster(All.Client, Monster, 0, 0, 0, 0);
                            Monster.tmr2 = null;
                        });
                        Monster.tmr2.Execute();
                    }

                });
                Monster.tmr1.Execute();
            }

            if ((r == 1 && Monster.Effect == 0 && Monster.State != 9) || (Monster.State != 9 && Monster.Effect == 0 && Monster.AttackType == 0))
            {
                Monster.tmr2 = new Delay(500, false, () =>
                {
                    Monster.State = (Monster.MoveType == 0 ? (byte)0 : (byte)1);
                    foreach (Character All in Map.Characters)
                        MonsterPacket.spawnMonster(All.Client, Monster, 0, 0, 0, 0);
                    Monster.tmr2 = null;
                });
                Monster.tmr2.Execute();
            }

            if (Monster.Effect != 0)
            {
                Monster.tmr3 = new Delay(6000, false, () =>
                {
                    Monster.Effect = 0;
                    Monster.State = (Monster.MoveType == 0 ? (byte)0 : (byte)1);
                    foreach (Character All in Map.Characters)
                        MonsterPacket.spawnMonster(All.Client, Monster, 0, 0, 0, 0);
                    Monster.tmr3 = null;
                });
                Monster.tmr3.Execute();
            }
        }
    }
}
