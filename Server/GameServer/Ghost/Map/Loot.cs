namespace Server.Ghost
{
    public class Loot
    {
        public int ID { get; set; }
        public int MobID { get; set; }
        public int ItemID { get; set; }
        public int MinimumQuantity { get; private set; }
        public int MaximumQuantity { get; private set; }
        public int QuestID { get; private set; }
        public int Chance { get; private set; }
        public int MobHp { get; set; }
        //经验
        public int MobExp { get; set; }
        //攻击方式
        public int AttckType { get; set; }
        //移动方式
        public int MoveType { get; set; }

        public int MobAtt1 { get; set; }

        public int MobAtt2 { get; set; }
        //碰撞攻击
        public int MobCrashAtt { get; set; }
        //防御
        public int MobDefence { get; set; }

        public int AddEffect { get; set; }

        public Loot(int id ,int mobId , int mobHp ,
            int mobExp , int attckType , int moveType ,
            int mobAtt1 , int mobAtt2 , int mobCrashAtt ,
            int mobDefence , int addEff)
        {
            this.ID = id;
            this.MobID = mobId;
            this.MobHp = mobHp;
            this.MobExp = mobExp;
            this.AttckType = attckType;
            this.MoveType = moveType;
            this.MobAtt1 = mobAtt1;
            this.MobAtt2 = mobAtt2;
            this.MobCrashAtt = mobCrashAtt;
            this.MobDefence = mobDefence;
            this.AddEffect = addEff;
        }
 

        public Loot(dynamic datum, bool fromMob = true)
        {
            this.MobID = datum.mobID;
            this.ItemID = datum.itemID;

            if (fromMob)
            {
                this.MinimumQuantity = datum.min_quantity;
                this.MaximumQuantity = datum.max_quantity;
            }

            this.QuestID = datum.questID;
            this.Chance = datum.chance;
        }

    }
}
