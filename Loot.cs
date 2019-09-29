using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon_Starcraft
{
    class Loot
    {
        private int location;
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
        protected Loot (int location)
        {
            Location = location;
        }
    }
    class Chest : Loot
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
        public Chest(int location) : base(location)
        {
            var rnd = new Random();
            Gold = rnd.Next(20, 101);
        }
        public Chest (int gold, int location) : base(location)
        {
            Gold = gold;
        }
    }
    class Aura : Loot
    {
        protected Aura (int location) : base (location)
        {
            Location = location;
        }
    }
    class AttackAura : Aura
    {
        private int attackBuff;
        public int AttackBuff
        {
            get => attackBuff;
            set
            {
                if (value > 0)
                    attackBuff = value;
                else
                    throw new ArgumentException("Аура атаки должна баффать минимум на +1");
            }
        }
        public AttackAura (int location) : base (location)
        {
            var rnd = new Random();
            AttackBuff = rnd.Next(1, 7);
        }
        public AttackAura(int attackBuff, int location) : base (location)
        {
            AttackBuff = attackBuff;
        }
    }
}
