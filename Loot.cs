using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon_Starcraft
{
    class Loot
    {
        private int money;
        private int location;
        public int Money
        {
            get => money;
            set
            {
                if (value > 0)
                    money = value;
            }
        }
        public int Location
        {
            get => location;
            set
            {
                if (value >= 0)
                    location = value;
            }

        }
        public Loot (int money)
        {
            Money = money;
        }
    }
}
