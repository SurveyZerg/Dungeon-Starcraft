using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon_Starcraft
{
    class Unit
    {
        private string name;
        private int hp;
        private int mana;
        private int armor;
        private int damage;

        #region properties
        public string Name { get => name; set => name = value; }
        public int HP
        {
            get => hp;
            set
            {
                if (value > 0)
                    hp = value;
            }

        }
        public int Mana
        {
            get => mana;
            set
            {
                if (value > 0)
                    mana = value;
            }

        }
        public int Armor
        {
            get => armor;
            set
            {
                if (value > 0)
                    armor = value;
            }

        }
        public int Damage
        {
            get => damage;
            set
            {
                if (value > 0)
                    damage = value;
            }

        }
        #endregion


    }
}
