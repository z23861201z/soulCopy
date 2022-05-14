﻿namespace Server.Common.Constants
{
    public class GameConstants
    {
        private static int[] exp = { 10,25,65,150,290,500,795,1185,1690,2660,3430,4350,5440,6720,8190,9870,11770,
            13910,16310,22230,25170,28400,31950,35810,40010,44560,49470,54770,60450,69200,78500,88400,99000,110200,
            122100,134700,148000,162100,251200,273600,297300,322100,348200,375400,404000,433700,464800,497300,630600,
            671800,714600,759200,805500,873600,944400,1018800,1095600,1174800,1467300,1586400,1708800,1836000,1966800,
            2102400,2241600,2385600,2534400,2688000,3315800,3524400,3739200,3960000,4186800,4419600,4659600,4905600,5158800,
            5418000,6634700,6968400,7310400,7660800,8019600,8388000,8764800,9151200,9546000,9950400,12119100,12619200,13130400,
            13652400,14186400,14732400,15289200,15858000,16438800,17031600,20670800,20884600 };

        public static int getExpNeededForLevel(byte level)
        {
            if (level > 100)
            {
                return 20884600;
            }
            return exp[level - 1];
        }
    }
}
