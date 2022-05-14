using Server.Common;
using Server.Common.Data;
using System;
using System.Collections.Generic;
using System.IO;

using System.Text;
using Server.Common.IO;

namespace Server.Ghost.Provider
{
    public static class ItemFactory
    {
        private static string openPath = Application.LaunchPath + @"\table\item.itm";

        //武器資料
        //物品編號(int), 物品名稱(string), 職業(byte), 功力(byte), 類型(short), 攻擊距離(short), 攻擊速度(byte), 購買價格(int)
        public static Dictionary<int, ItemData> weaponData = new Dictionary<int, ItemData>();
        //衣服資料
        //物品編號(int), 物品名稱(string), 職業(byte), 功力(byte), 購買價格(int)
        public static Dictionary<int, ItemData> topData = new Dictionary<int, ItemData>();
        //服裝資料
        //物品編號(int), 物品名稱(string), 職業(byte), 功力(byte), 購買價格(int)
        public static Dictionary<int, ItemData> clothingData = new Dictionary<int, ItemData>();
        //戒指資料
        //物品編號(int), 物品名稱(string), 職業(byte), 功力(byte), 購買價格(int)
        public static Dictionary<int, ItemData> ringData = new Dictionary<int, ItemData>();
        //項鍊資料
        //物品編號(int), 物品名稱(string), 職業(byte), 功力(byte), 購買價格(int)
        public static Dictionary<int, ItemData> necklaceData = new Dictionary<int, ItemData>();
        //披風資料
        //物品編號(int), 物品名稱(string), 職業(byte), 功力(byte), 購買價格(int)
        public static Dictionary<int, ItemData> capeData = new Dictionary<int, ItemData>();
        //消耗品資料
        //物品編號(int), 物品名稱(string), 購買價格(int)
        public static Dictionary<int, ItemData> useData = new Dictionary<int, ItemData>();
        //封印箱資料
        //物品編號(int), 物品名稱(string), 購買價格(int)
        public static Dictionary<int, ItemData> soulData = new Dictionary<int, ItemData>();
        //帽子資料
        //物品編號(int), 物品名稱(string), 購買價格(int)
        public static Dictionary<int, ItemData> hatData = new Dictionary<int, ItemData>();
        //髮型資料
        //物品編號(int), 物品名稱(string), 購買價格(int)
        public static Dictionary<int, ItemData> hairData = new Dictionary<int, ItemData>();
        //眼睛資料
        //物品編號(int), 物品名稱(string), 購買價格(int)
        public static Dictionary<int, ItemData> eyesData = new Dictionary<int, ItemData>();
        //面具資料
        //物品編號(int), 物品名稱(string), 購買價格(int)
        public static Dictionary<int, ItemData> maskData = new Dictionary<int, ItemData>();
        //鬍子資料
        //物品編號(int), 物品名稱(string), 購買價格(int)
        public static Dictionary<int, ItemData> beardData = new Dictionary<int, ItemData>();
        //其他資料
        //物品編號(int), 物品名稱(string), 購買價格(int)
        public static Dictionary<int, ItemData> etcData = new Dictionary<int, ItemData>();
        //寵物資料
        //物品編號(int), 物品名稱(string), 購買價格(int)
        public static Dictionary<int, ItemData> petData = new Dictionary<int, ItemData>();
        //領巾資料
        //物品編號(int), 物品名稱(string), 購買價格(int)
        public static Dictionary<int, ItemData> scarfData = new Dictionary<int, ItemData>();
        //未知資料
        //物品編號(int), 物品名稱(string), 購買價格(int)
        public static Dictionary<int, ItemData> unknownData = new Dictionary<int, ItemData>();
        //拼圖資料
        //物品編號(int), 物品名稱(string), 購買價格(int)
        public static Dictionary<int, ItemData> jigsawData = new Dictionary<int, ItemData>();
        //耳環資料
        //物品編號(int), 物品名稱(string), 購買價格(int)
        public static Dictionary<int, ItemData> earringData = new Dictionary<int, ItemData>();

        public static List<Dictionary<int, ItemData>> all = new List<Dictionary<int, ItemData>>();

        public static bool insertItemFlag = false;
        public static void Initialize()
        {
            FileStream itemFile = File.Open(openPath, FileMode.Open, FileAccess.ReadWrite);
            BinaryReader item = new BinaryReader(itemFile, Encoding.GetEncoding("UTF-16LE"));
            //==============================================================================
            item.ReadBytes(120); // 未知
            //==============================================================================
            // 武器類型開始
            int weaponCount = item.ReadInt32(); // 武器數量

            Console.WriteLine("\n");
            // 武器写入数据库
            dynamic dbWeapon = new Datum("item_weapon");
            for (int i = 0; i < weaponCount; i++)
            {
                int itemId = item.ReadInt32(); // 物品編號
                byte[] itemNameByteArray = item.ReadBytes(62); // 物品名稱 (Byte[])
                string itemNameString = Encoding.GetEncoding("UTF-16LE").GetString(itemNameByteArray); // 物品名稱 (Byte[] => String)
                int v1 = item.ReadByte();
                byte job = item.ReadByte(); // 職業
                byte level = item.ReadByte(); // 功力
                short attack = item.ReadInt16(); // 類型
                short magicAttack = item.ReadInt16();
                short attackRange = item.ReadInt16(); // 攻擊距離
                int v2 = item.ReadInt16();
                byte speed = item.ReadByte(); // 攻擊速度
                int v3 = item.ReadInt16();
                int v4 = item.ReadInt16();
                int v5 = item.ReadInt16();
                int v6 = item.ReadInt16();
                int v7 = item.ReadInt16();
                int v8 = item.ReadInt16();
                int v9 = item.ReadInt16();
                int price = item.ReadInt32(); // 購買價格
                byte fusion = item.ReadByte();
                int v10 = item.ReadInt16();
                int v11 = item.ReadInt16();
                String v12 = Encoding.GetEncoding("UTF-16LE").GetString(item.ReadBytes(16));
                String v13 = Encoding.GetEncoding("UTF-16LE").GetString(item.ReadBytes(20));
                // 判断加载项是否重复

                //写入数据库
                if (insertItemFlag && !dbWeapon.existById("itemId" , itemId))
                {
                    dbWeapon.itemId = itemId;
                    dbWeapon.itemName = itemNameString;
                    dbWeapon.v1 = v1;
                    dbWeapon.job = job;
                    dbWeapon.level = level;
                    dbWeapon.attack = attack;
                    dbWeapon.magicAttack = magicAttack;
                    dbWeapon.v2 = v2;
                    dbWeapon.speed = speed;
                    dbWeapon.v3 = v3;
                    dbWeapon.v4 = v4;
                    dbWeapon.v5 = v5;
                    dbWeapon.v6 = v6;
                    dbWeapon.v7 = v7;
                    dbWeapon.v8 = v8;
                    dbWeapon.v9 = v9;
                    dbWeapon.price = price;
                    dbWeapon.fusion = fusion;
                    dbWeapon.v10 = v10;
                    dbWeapon.v11 = v11;
                    dbWeapon.v12 = v12;
                    dbWeapon.v13 = v13;
                    dbWeapon.Insert();
                }
                ItemData itemEntity = new ItemData(itemId, itemNameString, job, level, attack, magicAttack, attackRange, speed, price, fusion);
                if (weaponData.ContainsKey(itemId))
                {
                    Log.Inform("武器重复:" + itemId);
                }
                else
                {
                    weaponData.Add(itemId, new ItemData(itemId, itemNameString, job, level, attack, magicAttack, attackRange, speed, price, fusion));
                }
              
            }
            all.Add(weaponData);
            //==============================================================================
            // 衣服類型開始
            int topCount = item.ReadInt32(); // 衣服數量
            dynamic dbClothing = new Datum("item_clothing");
            for (int i = 0; i < topCount; i++)
            {
                int itemId = item.ReadInt32(); // 物品編號
                byte[] itemNameByteArray = item.ReadBytes(62); // 物品名稱 (Byte[])
                string itemNameString = Encoding.GetEncoding("UTF-16LE").GetString(itemNameByteArray); // 物品名稱 (Byte[] => String)
                int v1 = item.ReadByte();
                byte job = item.ReadByte();
                byte level = item.ReadByte();
                short defense = item.ReadInt16();
                int v2 = item.ReadInt16();
                int v3 = item.ReadInt16();
                int v4 = item.ReadInt16();
                int v5 = item.ReadInt16();
                int v6 = item.ReadInt16();
                int v7 = item.ReadInt16();
                int v8 = item.ReadInt16();
                int price = item.ReadInt32(); // 購買價格
                byte fusion = item.ReadByte();
                int v9 = item.ReadInt16();
                int v10 = item.ReadInt16();
                String v11 = Encoding.GetEncoding("UTF-16LE").GetString(item.ReadBytes(16));
                String v12 = Encoding.GetEncoding("UTF-16LE").GetString(item.ReadBytes(20));

                //写入数据库
                if (insertItemFlag && !dbClothing.existById("itemId", itemId))
                {
                    dbClothing.itemId = itemId;
                    dbClothing.itemName = itemNameString;
                    dbClothing.v1 = v1;
                    dbClothing.job = job;
                    dbClothing.level = level;
                    dbClothing.defense = defense;
                    dbClothing.price = price;
                    dbClothing.fusion = fusion;
                    dbClothing.v2 = v2;
                    dbClothing.v3 = v3;
                    dbClothing.v4 = v4;
                    dbClothing.v5 = v5;
                    dbClothing.v6 = v6;
                    dbClothing.v7 = v7;
                    dbClothing.v8 = v8;
                    dbClothing.v9 = v9;
                    dbClothing.v10 = v10;
                    dbClothing.v11 = v11;
                    dbClothing.v12 = v12;
                    dbClothing.Insert();
                }


                // 判断加载项是否重复
                ItemData itemEntity = new ItemData(itemId, itemNameString, job, level, defense, price, fusion);
                if (topData.ContainsKey(itemId))
                {
                    Log.Inform("衣服重复:" + itemId);
                }
                else
                {
                    topData.Add(itemId, new ItemData(itemId, itemNameString, job, level, defense, price, fusion));
                }
               
            }
            all.Add(topData);
            //==============================================================================
            // 服裝類型開始
            int clothingCount = item.ReadInt32(); // 服裝數量
            dynamic dbFashion = new Datum("item_fashion");
            for (int i = 0; i < clothingCount; i++)
            {
                int itemId = item.ReadInt32(); // 物品編號
                byte[] itemNameByteArray = item.ReadBytes(62); // 物品名稱 (Byte[])
                string itemNameString = Encoding.GetEncoding("UTF-16LE").GetString(itemNameByteArray); // 物品名稱 (Byte[] => String)
                byte gender = item.ReadByte();
                byte job = item.ReadByte();
                byte level = item.ReadByte();
                short defense = item.ReadInt16();
                short str = item.ReadInt16();
                short dex = item.ReadInt16();
                short vit = item.ReadInt16();
                short iint = item.ReadInt16();
                int v1 = item.ReadInt16();
                int v2 = item.ReadInt16();
                int v3 = item.ReadInt16();
                int price = item.ReadInt32();
                int v4 = item.ReadInt16();
                int v5 = item.ReadInt16();
                byte fusion = item.ReadByte();
                String v6 = Encoding.GetEncoding("UTF-16LE").GetString(item.ReadBytes(3));
                String v7 = Encoding.GetEncoding("UTF-16LE").GetString(item.ReadBytes(12));


                //写入数据库
                if (insertItemFlag && !dbFashion.existById("itemId", itemId))
                {
                    dbFashion.itemId = itemId;
                    dbFashion.itemName = itemNameString;
                    dbFashion.v1 = v1;
                    dbFashion.job = job;
                    dbFashion.level = level;
                    dbFashion.defense = defense;
                    dbFashion.price = price;
                    dbFashion.fusion = fusion;
                    dbFashion.str = str;
                    dbFashion.dex = dex;
                    dbFashion.vit = vit;
                    dbFashion.iint = iint;
                    dbFashion.gender = gender;
                    dbFashion.v2 = v2;
                    dbFashion.v3 = v3;
                    dbFashion.v4 = v4;
                    dbFashion.v5 = v5;
                    dbFashion.v6 = v6;
                    dbFashion.v7 = v7;
                    dbFashion.Insert();
                }

                // 判断加载项是否重复
                ItemData itemEntity = new ItemData(itemId, itemNameString, job, level, defense, str, dex, vit, iint, price, fusion);
                if (clothingData.ContainsKey(itemId))
                {
                    Log.Inform("时装重复:" + itemId);
                }
                else
                {
                    clothingData.Add(itemId, new ItemData(itemId, itemNameString, job, level, defense, str, dex, vit, iint, price, fusion));
                }
             
            }
            all.Add(clothingData);
            //==============================================================================
            // 戒指類型開始
            int ringCount = item.ReadInt32(); // 戒指數量
            dynamic dbRing = new Datum("item_ring");
            for (int i = 0; i < ringCount; i++)
            {
                int itemId = item.ReadInt32(); // 物品編號
                byte[] itemNameByteArray = item.ReadBytes(62); // 物品名稱 (Byte[])
                string itemNameString = Encoding.GetEncoding("UTF-16LE").GetString(itemNameByteArray); // 物品名稱 (Byte[] => String)
                byte job = item.ReadByte();
                byte level = item.ReadByte();
                short str = item.ReadInt16();
                short dex = item.ReadInt16();
                short vit = item.ReadInt16();
                short iint = item.ReadInt16();
                short magic = item.ReadInt16();
                short avoid = item.ReadInt16();
                short attack = item.ReadInt16();
                short defense = item.ReadInt16();
                short hp = item.ReadInt16();
                short mp = item.ReadInt16();
                int price = item.ReadInt32();
                int v1 = item.ReadInt16();
                int v2 = item.ReadInt16();
                byte refining = item.ReadByte();

                String v3 = Encoding.GetEncoding("UTF-16LE").GetString(item.ReadBytes(10));
                String v4 = Encoding.GetEncoding("UTF-16LE").GetString(item.ReadBytes(5));
              
                byte[] itemDescriptionByteArray = item.ReadBytes(256); // 物品敘述 (Byte[])
                string itemDescriptionString = Encoding.GetEncoding("UTF-16LE").GetString(itemDescriptionByteArray); // 物品敘述 (Byte[] => String)


                //写入数据库
                if (insertItemFlag && !dbRing.existById("itemId", itemId))
                {
                    dbRing.itemId = itemId;
                    dbRing.itemName = itemNameString;
                    dbRing.job = job;
                    dbRing.level = level;
                    dbRing.str = str;
                    dbRing.dex = dex;
                    dbRing.vit = vit;
                    dbRing.iint = iint;
                    dbRing.magic = magic;
                    dbRing.avoid = avoid;
                    dbRing.attack = attack;
                    dbRing.defense = defense;
                    dbRing.hp = hp;
                    dbRing.mp = mp;
                    dbRing.price = price;
                    dbRing.v1 = v1;
                    dbRing.v2 = v2;
                    dbRing.refining = refining;
                    dbRing.v3 = v3;
                    dbRing.v4 = v4;
                    dbRing.itemDesc = itemDescriptionString;
                    dbRing.Insert();
                }


                // 判断加载项是否重复
                ItemData itemEntity = new ItemData(itemId, itemNameString, job, level, str, dex, vit, iint, magic, avoid, attack, defense, hp, mp, refining, price);
                if (ringData.ContainsKey(itemId))
                {
                    Log.Inform("戒指重复:" + itemId);
                }
                else
                {
                    ringData.Add(itemId, new ItemData(itemId, itemNameString, job, level, str, dex, vit, iint, magic, avoid, attack, defense, hp, mp, refining, price));
                }
            }
            all.Add(ringData);
            //==============================================================================
            // 項鍊類型開始
            dynamic dbNecklace = new Datum("item_necklace");
            int necklaceCount = item.ReadInt32(); // 項鍊數量
            for (int i = 0; i < necklaceCount; i++)
            {
                int itemId = item.ReadInt32(); // 物品編號
                byte[] ItemNameByteArray = item.ReadBytes(62); // 物品名稱 (Byte[])
                string itemNameString = Encoding.GetEncoding("UTF-16LE").GetString(ItemNameByteArray); // 物品名稱 (Byte[] => String)
                byte job = item.ReadByte();
                byte level = item.ReadByte();
                short defense = item.ReadInt16();
                int v1 = item.ReadInt16();
                int v2 = item.ReadInt16();
                int v3 = item.ReadInt16();
                int v4 = item.ReadInt16();
                short hp = item.ReadInt16();
                short mp = item.ReadInt16();
                int price = item.ReadInt32();
                int v5 = item.ReadInt16();
                int v6 = item.ReadInt16();
                int v7 = item.ReadByte();

                String v8 = Encoding.GetEncoding("UTF-16LE").GetString(item.ReadBytes(10));
                String v9 = Encoding.GetEncoding("UTF-16LE").GetString(item.ReadBytes(5));
                byte[] itemDescriptionByteArray = item.ReadBytes(256); // 物品敘述 (Byte[])
                string itemDescriptionString = Encoding.GetEncoding("UTF-16LE").GetString(itemDescriptionByteArray); // 物品敘述 (Byte[] => String)



                //写入数据库
                if (insertItemFlag && !dbNecklace.existById("itemId", itemId))
                {
                    dbNecklace.itemId = itemId;
                    dbNecklace.itemName = itemNameString;
                    dbNecklace.level = level;
                    dbNecklace.defense = defense;
                    dbNecklace.v1 = v1;
                    dbNecklace.v2 = v2;
                    dbNecklace.v3 = v3;
                    dbNecklace.v4 = v4;
                    dbNecklace.hp = hp;
                    dbNecklace.mp = mp;
                    dbNecklace.price = price;
                    dbNecklace.v5 = v5;
                    dbNecklace.v6 = v6;
                    dbNecklace.v7 = v7;
                    dbNecklace.v8 = v8;
                    dbNecklace.v9 = v9;
                    dbNecklace.itemDesc = itemDescriptionString;

                    dbNecklace.Insert();
                }

                // 判断加载项是否重复
                ItemData itemEntity = new ItemData(itemId, itemNameString, job, level, defense, hp, mp, price);
                if (necklaceData.ContainsKey(itemId))
                {
                    Log.Inform("项链重复:" + itemId);
                }
                else
                {
                    necklaceData.Add(itemId, new ItemData(itemId, itemNameString, job, level, defense, hp, mp, price));
                }
            }
            all.Add(necklaceData);
            //==============================================================================
            // 披風類型開始
            dynamic dbCape = new Datum("item_cape");
            int capeCount = item.ReadInt32(); // 披風數量
            for (int i = 0; i < capeCount; i++)
            {
                int itemId = item.ReadInt32(); // 物品編號
                byte[] itemNameByteArray = item.ReadBytes(62); // 物品名稱 (Byte[])
                string itemNameString = Encoding.GetEncoding("UTF-16LE").GetString(itemNameByteArray); // 物品名稱 (Byte[] => String)
                byte job = item.ReadByte();
                byte level = item.ReadByte();
                short str = item.ReadInt16();
                short dex = item.ReadInt16();
                short vit = item.ReadInt16();
                short iint = item.ReadInt16();
                int v1 = item.ReadInt16();
                int v2 = item.ReadInt16();
                int v3 = item.ReadInt16();
                int v4 = item.ReadInt16();
                int v5 = item.ReadInt16();
                int v6 = item.ReadInt16();
                int price = item.ReadInt32();
                int v7 = item.ReadInt16();
                int v8 = item.ReadInt16();
                int v9 = item.ReadByte();
                String v10 = Encoding.GetEncoding("UTF-16LE").GetString(item.ReadBytes(3));
                String v11 = Encoding.GetEncoding("UTF-16LE").GetString(item.ReadBytes(12));

                //写入数据库
                if (insertItemFlag && !dbCape.existById("itemId", itemId))
                {
                    dbCape.itemId = itemId;
                    dbCape.itemName = itemNameString;
                    dbCape.job = job;
                    dbCape.level = level;
                    dbCape.str = str;
                    dbCape.dex = dex;
                    dbCape.vit = vit;
                    dbCape.iint = iint;
                    dbCape.v1 = v1;
                    dbCape.v2 = v2;
                    dbCape.v3 = v3;
                    dbCape.v4 = v4;
                    dbCape.v5 = v5;
                    dbCape.v6 = v6;
                    dbCape.price = price;
                    dbCape.v7 = v7;
                    dbCape.v8 = v8;
                    dbCape.v9 = v9;
                    dbCape.v10 = v10;
                    dbCape.v11 = v11;
                    dbCape.Insert();
                }

                // 判断加载项是否重复
                 ItemData itemEntity = new ItemData(itemId, itemNameString, job, level, str, dex, vit, iint, price);
                if (capeData.ContainsKey(itemId))
                {
                    Log.Inform("披风重复:" + itemId);
                }
                else
                {
                    capeData.Add(itemId, itemEntity);
                }
            }
            all.Add(capeData);
            //==============================================================================
            // 消耗類型開始
            int useCount = item.ReadInt32(); // 消耗道具數量
            dynamic dbConsume = new Datum("item_consume");
            for (int i = 0; i < useCount; i++)
            {
                int itemId = item.ReadInt32(); // 物品編號
                byte[] itemNameByteArray = item.ReadBytes(62); // 物品名稱 (Byte[])
                string itemNameString = Encoding.GetEncoding("UTF-16LE").GetString(itemNameByteArray); // 物品名稱 (Byte[] => String)
                int v1 = item.ReadByte();
                // Type 0 : 恢復鬼力
                // Type 1 : 恢復體力(%)
                // Type 2 : 恢復體力
                // Type 3 : 恢復體力(%)
                // Type 4 : 解除異常
                int type = item.ReadInt32();
                int recover = item.ReadInt32();
                int v2 = item.ReadInt32();
                int price = item.ReadInt32();
                int v3 = item.ReadInt16();
                int v4 = item.ReadInt16();
                String v5 = Encoding.GetEncoding("UTF-16LE").GetString(item.ReadBytes(16));
                int v6 = item.ReadByte();
                byte[] itemDescriptionByteArray = item.ReadBytes(256); // 物品敘述 (Byte[])
                string itemDescriptionString = Encoding.GetEncoding("UTF-16LE").GetString(itemDescriptionByteArray); // 物品敘述 (Byte[] => String)
                 
                //写入数据库
                if(insertItemFlag && !dbConsume.existById("itemId", itemId))
                {
                    dbConsume.itemId = itemId;
                    dbConsume.itemName = itemNameString.Replace("'","");
                    dbConsume.v1 = v1;
                    dbConsume.type = type;
                    dbConsume.recover = recover;
                    dbConsume.v2 = v2;
                    dbConsume.price = price;
                    dbConsume.v3 = v3;
                    dbConsume.v4 = v4;
                    dbConsume.v5 = v5;
                    dbConsume.v6 = v6;
                    dbConsume.itemDesc = itemDescriptionString;
                    dbConsume.Insert();
                }

                // 判断加载项是否重复
                ItemData itemEntity = new ItemData(itemId, itemNameString, type, recover, price);
                if (useData.ContainsKey(itemId))
                {
                    Log.Inform("消耗重复:" + itemId);
                }
                else
                {
                    useData.Add(itemId, itemEntity);
                }
            }
            all.Add(useData);
            //==============================================================================
            // 封印箱類型開始
            dynamic dbSoul = new Datum("item_soul");
            int soulCount = item.ReadInt32(); // 封印箱數量
            for (int i = 0; i < soulCount; i++)
            {
                int itemId = item.ReadInt32(); // 物品編號
                byte[] itemNameByteArray = item.ReadBytes(62); // 物品名稱 (Byte[])
                string itemNameString = Encoding.GetEncoding("UTF-16LE").GetString(itemNameByteArray); // 物品名稱 (Byte[] => String)
                int v1 = item.ReadByte();
                int spirit = item.ReadInt32();
                int v2 = item.ReadInt32();
                int price = item.ReadInt32();
                int v3= item.ReadInt16();
                int v4 = item.ReadInt16();
                String v5 = Encoding.GetEncoding("UTF-16LE").GetString(item.ReadBytes(16));

                //写入数据库
                if (insertItemFlag && !dbSoul.existById("itemId", itemId))
                {
                    dbSoul.itemId = itemId;
                    dbSoul.itemName = itemNameString; 
                    dbSoul.v1 = v1;
                    dbSoul.spirit = spirit; 
                    dbSoul.v2 = v2;
                    dbSoul.price = price;
                    dbSoul.v3 = v3;
                    dbSoul.v4 = v4;
                    dbSoul.v5 = v5;
                    dbSoul.Insert();
                }

                // 判断加载项是否重复
                ItemData itemEntity = new ItemData(itemId, itemNameString, spirit, price);
                if (soulData.ContainsKey(itemId))
                {
                    Log.Inform("封印重复:" + itemId);
                }
                else
                {
                    soulData.Add(itemId, itemEntity);
                }
                
            }
            all.Add(soulData);
            //==============================================================================
            // 帽子類型開始
            int hatCount = item.ReadInt32(); // 帽子數量
            dynamic dbHat = new Datum("item_hat");
            for (int i = 0; i < hatCount; i++)
            {
                int itemId = item.ReadInt32(); // 物品編號
                byte[] itemNameByteArray = item.ReadBytes(62); // 物品名稱 (Byte[])
                string itemNameString = Encoding.GetEncoding("UTF-16LE").GetString(itemNameByteArray); // 物品名稱 (Byte[] => String)
                int v0 = item.ReadByte();
                short str = item.ReadInt16();
                short dex = item.ReadInt16();
                short vit = item.ReadInt16();
                short iint = item.ReadInt16();
                int v1 = item.ReadInt16();
                short hp = item.ReadInt16();
                short mp = item.ReadInt16();
                int price = item.ReadInt32();
                int v2= item.ReadInt16();
                int v3 = item.ReadInt16();
                int v4 = item.ReadByte();
                string v5 = Encoding.GetEncoding("UTF-16LE").GetString(item.ReadBytes(3));
                String v6 = Encoding.GetEncoding("UTF-16LE").GetString(item.ReadBytes(12));

                if (insertItemFlag && !dbHat.existById("itemId", itemId))
                {
                    dbHat.itemId = itemId;
                    dbHat.itemName = itemNameString;
                    dbHat.v0 = v0;
                    dbHat.str = str;
                    dbHat.dex = dex;
                    dbHat.vit = vit;
                    dbHat.iint = iint;
                    dbHat.v1 = v1;
                    dbHat.hp = hp;
                    dbHat.mp = mp;
                    dbHat.price = price;
                    dbHat.v2 = v2;
                    dbHat.v3 = v3;
                    dbHat.v4 = v4;
                    dbHat.v5 = v5;
                    dbHat.v6 = v6;
                    dbHat.Insert();
                    
                }

                    // 判断加载项是否重复
                    ItemData itemEntity = new ItemData(itemId, itemNameString, str, dex, vit, iint, hp, mp, price);
                if (hatData.ContainsKey(itemId))
                {
                    Log.Inform("帽子重复:" + itemId);
                }
                else
                {
                    hatData.Add(itemId, itemEntity);
                }
            }
            all.Add(hatData);
            //==============================================================================
            // 髮型類型開始
            dynamic dbHair = new Datum("item_hair");
            int hairCount = item.ReadInt32(); // 髮型數量
            for (int i = 0; i < hairCount; i++)
            {
                int itemId = item.ReadInt32(); // 物品編號
                byte[] ItemNameByteArray = item.ReadBytes(62); // 物品名稱 (Byte[])
                string itemNameString = Encoding.GetEncoding("UTF-16LE").GetString(ItemNameByteArray); // 物品名稱 (Byte[] => String)
                byte gender = item.ReadByte();
                int price = item.ReadInt32();
                int v1 = item.ReadInt16();
                int v2 = item.ReadInt16();
                String v3 = Encoding.GetEncoding("UTF-16LE").GetString(item.ReadBytes(16));
                //写入数据库
                if (insertItemFlag && !dbHair.existById("itemId", itemId))
                {
                    dbHair.itemId = itemId;
                    dbHair.itemName = itemNameString;
                    dbHair.gender = gender;
                    dbHair.price = price;
                    dbHair.v1 = v1;
                    dbHair.v2 = v2;
                    dbHair.v3 = v3;

                    dbHair.Insert();
                }

                    // 判断加载项是否重复
                    ItemData itemEntity = new ItemData(itemId, itemNameString, price);
                if (hairData.ContainsKey(itemId))
                {
                    Log.Inform("发型重复:" + itemId);
                }
                else
                {
                    hairData.Add(itemId, itemEntity);
                }
            }
            all.Add(hairData);
            //==============================================================================
            // 眼睛類型開始
            int eyesCount = item.ReadInt32(); // 眼睛數量
            dynamic dbEyes = new Datum("item_eyes");
            for (int i = 0; i < eyesCount; i++)
            {
                int itemId = item.ReadInt32(); // 物品編號
                byte[] ItemNameByteArray = item.ReadBytes(62); // 物品名稱 (Byte[])
                string itemNameString = Encoding.GetEncoding("UTF-16LE").GetString(ItemNameByteArray); // 物品名稱 (Byte[] => String)
                byte gender = item.ReadByte();
                int price = item.ReadInt32();
                int v1 = item.ReadInt16();
                int v2 = item.ReadInt16();
                string v3 = Encoding.GetEncoding("UTF-16LE").GetString(item.ReadBytes(16));

                //写入数据库
                if (insertItemFlag && !dbEyes.existById("itemId", itemId))
                {
                    dbEyes.itemId = itemId;
                    dbEyes.itemName = itemNameString;
                    dbEyes.gender = gender;
                    dbEyes.price = price;
                    dbEyes.v1 = v1;
                    dbEyes.v2 = v2;
                    dbEyes.v3 = v3;

                    dbEyes.Insert();
                }

                // 判断加载项是否重复
                ItemData itemEntity = new ItemData(itemId, itemNameString, price);
                if (eyesData.ContainsKey(itemId))
                {
                    Log.Inform("眼睛重复:" + itemId);
                }
                else
                {
                    eyesData.Add(itemId, itemEntity);
                }

            }
            all.Add(eyesData);
            //==============================================================================
            // 面具類型開始
            int maskCount = item.ReadInt32(); // 面具數量
            dynamic dbMask = new Datum("item_mask");
            for (int i = 0; i < maskCount; i++)
            {
                int itemId = item.ReadInt32(); // 物品編號
                byte[] itemNameByteArray = item.ReadBytes(62); // 物品名稱 (Byte[])
                string itemNameString = Encoding.GetEncoding("UTF-16LE").GetString(itemNameByteArray); // 物品名稱 (Byte[] => String)
                int v1 = item.ReadByte();
                int price = item.ReadInt32();
                int v2 = item.ReadInt16();
                int v3 = item.ReadInt16();
                string v4 = Encoding.GetEncoding("UTF-16LE").GetString(item.ReadBytes(16));

                //写入数据库
                if (insertItemFlag && !dbMask.existById("itemId", itemId))
                {
                    dbMask.itemId = itemId;
                    dbMask.itemName = itemNameString;
                    dbMask.v1 = v1;
                    dbMask.price = price;
                    dbMask.v2 = v2;
                    dbMask.v3 = v3;
                    dbMask.v4 = v4;

                    dbMask.Insert();
                }


                // 判断加载项是否重复
                ItemData itemEntity = new ItemData(itemId, itemNameString, price);
                if (maskData.ContainsKey(itemId))
                {
                    Log.Inform("面具重复:" + itemId);
                }
                else
                {
                    maskData.Add(itemId, itemEntity);
                }

            }
            all.Add(maskData);
            //==============================================================================
            // 鬍子類型開始
            int beardCount = item.ReadInt32(); // 鬍子數量
            dynamic dbBeard = new Datum("item_beard");
            for (int i = 0; i < beardCount; i++)
            {
                int itemId = item.ReadInt32(); // 物品編號
                byte[] itemNameByteArray = item.ReadBytes(62); // 物品名稱 (Byte[])
                string itemNameString = Encoding.GetEncoding("UTF-16LE").GetString(itemNameByteArray); // 物品名稱 (Byte[] => String)
                int v1 = item.ReadByte();
                int price = item.ReadInt32();
                int v2= item.ReadInt16();
                int v3 = item.ReadInt16();
                string v4 = Encoding.GetEncoding("UTF-16LE").GetString(item.ReadBytes(16));

                //写入数据库
                if (insertItemFlag && !dbBeard.existById("itemId", itemId))
                {
                    dbBeard.itemId = itemId;
                    dbBeard.itemName = itemNameString;
                    dbBeard.v1 = v1;
                    dbBeard.price = price;
                    dbBeard.v2 = v2;
                    dbBeard.v3 = v3;
                    dbBeard.v4 = v4;

                    dbBeard.Insert();
                }

                // 判断加载项是否重复
                ItemData itemEntity = new ItemData(itemId, itemNameString, price);
                if (maskData.ContainsKey(itemId))
                {
                    Log.Inform("面具重复:" + itemId);
                }
                else
                {
                    maskData.Add(itemId, itemEntity);
                }
                beardData.Add(itemId, new ItemData(itemId, itemNameString, price));
            }
            all.Add(beardData);
            //==============================================================================
            // 其他類型開始
            int etcCount = item.ReadInt32(); // 其他數量
            dynamic dbEtc = new Datum("item_etc");
            for (int i = 0; i < etcCount; i++)
            {
                int itemId = item.ReadInt32(); // 物品編號
                byte[] itemNameByteArray = item.ReadBytes(62); // 物品名稱 (Byte[])
                string itemNameString = Encoding.GetEncoding("UTF-16LE").GetString(itemNameByteArray); // 物品名稱 (Byte[] => String)
                int v1 = item.ReadByte();
                int price = item.ReadInt32();
                int v2 =item.ReadInt16();
                int v3 = item.ReadInt16();
                string v4 = Encoding.GetEncoding("UTF-16LE").GetString(item.ReadBytes(16));

                byte[] itemDescriptionByteArray = item.ReadBytes(256); // 物品敘述 (Byte[])
                string itemDescriptionString = Encoding.GetEncoding("UTF-16LE").GetString(itemDescriptionByteArray); // 物品敘述 (Byte[] => String)

                //写入数据库
                if (insertItemFlag && !dbEtc.existById("itemId", itemId))
                {
                    dbEtc.itemId = itemId;
                    dbEtc.itemName = itemNameString;
                    dbEtc.v1 = v1;
                    dbEtc.price = price;
                    dbEtc.v2 = v2;
                    dbEtc.v3 = v3;
                    dbEtc.v4 = v4;
                    dbEtc.itemDesc = itemDescriptionString;

                    dbEtc.Insert();
                }

                etcData.Add(itemId, new ItemData(itemId, itemNameString, price));
            }
            all.Add(etcData);
            //==============================================================================
            // 寵物類型開始
            int petCount = item.ReadInt32(); // 寵物類型數量
            dynamic dbPet = new Datum("item_pet");
            for (int i = 0; i < petCount; i++)
            {
                int itemId = item.ReadInt32(); // 物品編號
                byte[] itemNameByteArray = item.ReadBytes(62); // 物品名稱 (Byte[])
                string itemNameString = Encoding.GetEncoding("UTF-16LE").GetString(itemNameByteArray); // 物品名稱 (Byte[] => String)
                int v1 = item.ReadByte();
                int v2 = item.ReadInt32();
                int v3 = item.ReadInt32();
                int v4 = item.ReadInt32();
                int price = item.ReadInt32();
                int v5 = item.ReadInt16();
                int v6 = item.ReadInt16();
                String v7 = Encoding.GetEncoding("UTF-16LE").GetString(item.ReadBytes(16));
                int v8 = item.ReadByte();
                byte[] itemDescriptionByteArray = item.ReadBytes(256); // 物品敘述 (Byte[])
                string itemDescriptionString = Encoding.GetEncoding("UTF-16LE").GetString(itemDescriptionByteArray); // 物品敘述 (Byte[] => String)

                //写入数据库
                if (insertItemFlag && !dbPet.existById("itemId", itemId))
                {
                    dbPet.itemId = itemId;
                    dbPet.itemName = itemNameString;
                    dbPet.v1 = v1;
                    dbPet.price = price;
                    dbPet.v2 = v2;
                    dbPet.v3 = v3;
                    dbPet.v4 = v4;
                    dbPet.price = price;
                    dbPet.v5 = v5;
                    dbPet.v6 = v6;
                    dbPet.v7 = v7;
                    dbPet.v8 = v8;
                    dbPet.itemDesc = itemDescriptionString;

                    dbPet.Insert();
                }


                petData.Add(itemId, new ItemData(itemId, itemNameString, price));
            }
            all.Add(petData);
            //==============================================================================
            // 精灵類型開始
            dynamic dbScarf = new Datum("item_scarf");
            int scarfCount = item.ReadInt32(); // 精灵類型數量
            for (int i = 0; i < scarfCount; i++)
            {
                int itemId = item.ReadInt32(); // 物品編號
                byte[] itemNameByteArray = item.ReadBytes(62); // 物品名稱 (Byte[])
                string itemNameString = Encoding.GetEncoding("UTF-16LE").GetString(itemNameByteArray); // 物品名稱 (Byte[] => String)
                int v1 = item.ReadByte();
                int v2 = item.ReadInt16();
                int v3 = item.ReadInt16();
                int v4 = item.ReadInt16();
                int price = item.ReadInt32();
                int v5 = item.ReadInt16();
                int v6 = item.ReadInt16();
                String v7 = Encoding.GetEncoding("UTF-16LE").GetString(item.ReadBytes(16));
                byte[] itemDescriptionByteArray = item.ReadBytes(256); // 物品敘述 (Byte[])
                string itemDescriptionString = Encoding.GetEncoding("UTF-16LE").GetString(itemDescriptionByteArray); // 物品敘述 (Byte[] => String)

                //写入数据库
                if (insertItemFlag && !dbScarf.existById("itemId", itemId))
                {
                    dbScarf.itemId = itemId;
                    dbScarf.itemName = itemNameString;
                    dbScarf.v1 = v1;
                    dbScarf.price = price;
                    dbScarf.v2 = v2;
                    dbScarf.v3 = v3;
                    dbScarf.v4 = v4;
                    dbScarf.price = price;
                    dbScarf.v5 = v5;
                    dbScarf.v6 = v6;
                    dbScarf.v7 = v7;
                    dbScarf.itemDesc = itemDescriptionString;

                    dbScarf.Insert();
                }

                scarfData.Add(itemId, new ItemData(itemId, itemNameString, price));
            }
            all.Add(scarfData);
            //==============================================================================
            // 未知類型開始
            int unknownCount = item.ReadInt32(); // 未知數量
            dynamic dbUnknown = new Datum("item_unknown");
            for (int i = 0; i < unknownCount; i++)
            {
                int itemId = item.ReadInt32(); // 物品編號
                byte[] itemNameByteArray = item.ReadBytes(62); // 物品名稱 (Byte[])
                string itemNameString = Encoding.GetEncoding("UTF-16LE").GetString(itemNameByteArray); // 物品名稱 (Byte[] => String)
                int v1= item.ReadByte();
                int v2= item.ReadByte();
                int v3= item.ReadByte();
                int v4= item.ReadInt16();
                int v5= item.ReadInt16();
                int v6= item.ReadInt16();
                int v7= item.ReadInt16();
                int v8= item.ReadInt16();
                int v9= item.ReadInt16();
                int v10= item.ReadInt16();
                int v11= item.ReadInt16();
                int v12= item.ReadInt16();
                int v13= item.ReadInt16();
                int v14 = item.ReadByte();
                int price = item.ReadInt32();
                int v15 = item.ReadInt16();
                int v16 = item.ReadInt16();
                String v17 = Encoding.GetEncoding("UTF-16LE").GetString(item.ReadBytes(16));
                String v18 = Encoding.GetEncoding("UTF-16LE").GetString(item.ReadBytes(6));


                //写入数据库
                if (insertItemFlag && !dbUnknown.existById("itemId", itemId))
                {
                    dbUnknown.itemId = itemId;
                    dbUnknown.itemName = itemNameString;
                    dbUnknown.v1 = v1;
                    dbUnknown.v2 = v2;
                    dbUnknown.v3 = v3;
                    dbUnknown.v4 = v4;
                    dbUnknown.v5 = v5;
                    dbUnknown.v6 = v6;
                    dbUnknown.v7 = v7;
                    dbUnknown.v8 = v8;
                    dbUnknown.v9 = v9;
                    dbUnknown.v10 = v10;
                    dbUnknown.v11 = v11;
                    dbUnknown.v12 = v12;
                    dbUnknown.v13 = v13;
                    dbUnknown.v14 = v14;
                    dbUnknown.price = price;
                    dbUnknown.v15 = v15;
                    dbUnknown.v16 = v16;
                    dbUnknown.v17 = v17;
                    dbUnknown.v18 = v18;

                    dbUnknown.Insert();
                }


                unknownData.Add(itemId, new ItemData(itemId, itemNameString, price));
            }
            all.Add(unknownData);
            //==============================================================================
            // 拼圖類型開始
            int jigsawCount = item.ReadInt32(); // 拼圖數量
            dynamic dbJigsaw = new Datum("item_jigsaw");
            for (int i = 0; i < jigsawCount; i++)
            {
                int itemId = item.ReadInt32(); // 物品編號
                byte[] itemNameByteArray = item.ReadBytes(62); // 物品名稱 (Byte[])
                string itemNameString = Encoding.GetEncoding("UTF-16LE").GetString(itemNameByteArray); // 物品名稱 (Byte[] => String)
                int price = item.ReadInt32();
                int v1 = item.ReadInt16();
                int v2 = item.ReadInt16();
                int v3 = item.ReadByte();

                //写入数据库
                if (insertItemFlag && !dbJigsaw.existById("itemId", itemId))
                {
                    dbJigsaw.itemId = itemId;
                    dbJigsaw.itemName = itemNameString;
                    dbJigsaw.price = price;
                    dbJigsaw.v1 = v1;
                    dbJigsaw.v2 = v2;
                    dbJigsaw.v3 = v3;

                    dbJigsaw.Insert();
                }



                jigsawData.Add(itemId, new ItemData(itemId, itemNameString, price));
            }
            all.Add(jigsawData);
            //==============================================================================
            // 耳環類型開始
            int earringCount = item.ReadInt32(); // 耳環數量
            dynamic dbEarring = new Datum("item_earring");
            for (int i = 0; i < earringCount; i++)
            {
                int itemId = item.ReadInt32(); // 物品編號
                byte[] itemNameByteArray = item.ReadBytes(62); // 物品名稱 (Byte[])
                string itemNameString = Encoding.GetEncoding("UTF-16LE").GetString(itemNameByteArray); // 物品名稱 (Byte[] => String)
                int v1 = item.ReadByte();
                int v2 = item.ReadInt16();
                int v3 = item.ReadInt16();
                int v4 = item.ReadInt16();
                int v5 = item.ReadInt16();
                int v6 = item.ReadInt16();
                int v7 = item.ReadInt16();
                int v8 = item.ReadInt16();
                int v9 = item.ReadInt16();
                int v10 = item.ReadInt16();
                int v11 = item.ReadInt16();
                int v12 = item.ReadByte();
                int price = item.ReadInt32();
                int v13 = item.ReadInt16();
                int v14 = item.ReadInt16();
                int v15 = item.ReadByte();
                byte[] itemDescriptionByteArray = item.ReadBytes(256); // 物品敘述 (Byte[])
                string itemDescriptionString = Encoding.GetEncoding("UTF-16LE").GetString(itemDescriptionByteArray); // 物品敘述 (Byte[] => String)

                //写入数据库
                if (insertItemFlag && !dbEarring.existById("itemId", itemId))
                {
                    dbEarring.itemId = itemId;
                    dbEarring.itemName = itemNameString;
                    dbEarring.v1 = v1;
                    dbEarring.v2 = v2;
                    dbEarring.v3 = v3;
                    dbEarring.v4 = v4;
                    dbEarring.v5 = v5;
                    dbEarring.v6 = v6;
                    dbEarring.v7 = v7;
                    dbEarring.v8 = v8;
                    dbEarring.v9 = v9;
                    dbEarring.v10 = v10;
                    dbEarring.v11 = v11;
                    dbEarring.v12 = v12;
                    dbEarring.v13 = v13;
                    dbEarring.v14 = v14;
                    dbEarring.price = price;
                    dbEarring.v15 = v15;
                    dbEarring.itemDesc = itemDescriptionString;

                    dbEarring.Insert();
                }

                earringData.Add(itemId, new ItemData(itemId, itemNameString, price));
            }
            all.Add(earringData);
            itemFile.Close();
            item.Close();
        }

        public static ItemData GetItemData(int itemID)
        {
            foreach (Dictionary<int, ItemData> idata in all)
            {
                if (idata.ContainsKey(itemID))
                {
                    return idata[itemID];
                }
            }
            return null;
        }
    }
}
