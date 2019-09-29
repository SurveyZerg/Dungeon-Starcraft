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
        public int Location
        {
            get => location;
            set
            {
                if (value >= 0 && value < Map.MapSizeForClasses)
                    location = value;
                else
                    throw new ArgumentException($"Положение объекта {this.GetType()} вышло за пределы массива");
            }

        }
        #endregion
        protected Unit (int location)
        {
            Location = location;
        }
        protected Unit(string name, int hp, int mana, int armor, int damage, int location)
        {
            Name = name;
            HP = hp;
            Mana = mana;
            Armor = armor;
            Damage = damage;
            Location = location;
        }
        public virtual void ShowStatus()
        {
            Console.Write($"{Name}: HP = {HP}   Mana = {Mana}\nArmor = {Armor}  Damage = {Damage}   ");
        }
    }
    class MainHero : Unit
    {
        private int gold;
        public int Gold
        {
            get => gold;
            set
            {
                if (value >= 0)
                    gold = value;
                else
                    throw new ArgumentException("Голда не может быть отрицательна, долги в подземелья еще не вводили");
            }

        }
        public MainHero(string name, int location) : base (location)
        {
            var rnd = new Random();
            Name = name;
            HP = rnd.Next(60,121);
            Mana = rnd.Next(0,1);
            Armor = rnd.Next(2,5);
            Damage = rnd.Next(7,15);
        }
        public MainHero(string name, int hp, int mana, int armor, int damage, int gold, int location) : base(name, hp, mana, armor, damage, location)
        {
            Gold = gold;
        }
        public override void ShowStatus()
        {
            base.ShowStatus();
            Console.WriteLine($"Gold = {Gold}");
        }
    }
    class Boss : Unit
    {
        public Boss(string name, int location) : base(location)
        {
            var rnd = new Random();
            Name = name;
            HP = rnd.Next(60, 121);
            Mana = rnd.Next(0, 1);
            Armor = rnd.Next(2, 3);
            Damage = rnd.Next(7, 15);
        }
        public Boss(string name, int hp, int mana, int armor, int damage, int location) : base(name, hp, mana, armor, damage, location)
        {

        }
        public override void ShowStatus()
        {
            base.ShowStatus();
            Console.WriteLine($"");
        }
    }
    class Goblin : Unit
    {
        private int gold;
        public int Gold
        {
            get => gold;
            set
            {
                if (value >= 0)
                    gold = value;
                else
                    throw new ArgumentException("Голда не может быть отрицательна, долги в подземелья еще не вводили");
            }

        }
        public Goblin(int location) : base(location)
        {
            var rnd = new Random();
            Name = "Mr Goblin";
            HP = rnd.Next(10, 21);
            Mana = rnd.Next(0, 1);
            Armor = rnd.Next(0, 2);
            Damage = rnd.Next(5, 8);
        }
        public Goblin(string name, int location) : base(location)
        {
            var rnd = new Random();
            Name = name;
            HP = rnd.Next(10, 21);
            Mana = rnd.Next(0, 1);
            Armor = rnd.Next(0, 2);
            Damage = rnd.Next(5, 8);
        }
        public Goblin(string name, int hp, int mana, int armor, int damage, int gold, int location) : base(name, hp, mana, armor, damage, location)
        {
            Gold = gold;
        }
        public override void ShowStatus()
        {
            base.ShowStatus();
            Console.WriteLine($"Gold = {Gold}");
        }
    }
    class Spider : Unit
    {
        //ДОБАВИТЬ ОТРАВЛЕНИЕ ПАУКУ
        public Spider(int location) : base(location)
        {
            var rnd = new Random();
            int temp = rnd.Next(0, 3);
            if (temp == 0)
                Name = "Spider Pizza Man";
            else if (temp == 1)
                Name = "Spider Gay";
            else if (temp == 2)
                Name = "Spider boii";
            HP = rnd.Next(20, 31);
            Mana = rnd.Next(0, 1);
            Armor = rnd.Next(1, 3);
            Damage = rnd.Next(5, 8);
        }
        public Spider(string name, int location) : base(location)
        {
            var rnd = new Random();
            Name = name;
            HP = rnd.Next(20, 31);
            Mana = rnd.Next(0, 1);
            Armor = rnd.Next(1, 3);
            Damage = rnd.Next(5, 8);
        }
        public Spider(string name, int hp, int mana, int armor, int damage, int location) : base(name, hp, mana, armor, damage, location)
        {

        }
        public override void ShowStatus()
        {
            base.ShowStatus();
            Console.WriteLine($"");
        }
    }
}
