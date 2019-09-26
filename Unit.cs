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
        private int money = 0;
        private int location;

        #region properties
        public string Name { get => name; set => name = value; }
        public int HP
        {
            get => hp;
            set
            {
                if (value > 0)
                    hp = value;
                else
                    hp = 0;
            }

        }
        public int Mana
        {
            get => mana;
            set
            {
                if (value > 0)
                    mana = value;
                else
                    mana = 0;
            }

        }
        public int Armor
        {
            get => armor;
            set
            {
                if (value > 0)
                    armor = value;
                else
                    armor = 0;
            }

        }
        public int Damage
        {
            get => damage;
            set
            {
                if (value > 0)
                    damage = value;
                else
                    damage = 0;
            }

        }
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
        #endregion

        public Unit(string name, int hp, int mana, int armor, int damage)
        {
            Name = name;
            HP = hp;
            Mana = mana;
            Armor = armor;
            Damage = damage;
        }
        public void ShowStatus(bool showGold = false)
        {
            if (showGold)
                Console.Write($"{Name}: HP = {HP}   Mana = {Mana}\nArmor = {Armor}  Damage = {Damage}\nGold = {Money}\n");
            else
                Console.Write($"{Name}: HP = {HP}   Mana = {Mana}\nArmor = {Armor}  Damage = {Damage}\n");
        }
    }
}
