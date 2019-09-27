using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon_Starcraft
{
    class BattleEventArgs : EventArgs
    {
        public Unit MainHero;
        public Unit Enemy;
    }
    class Game
    {
        public static bool End {get;set;}
        public BattleEventArgs BattleEventArgs = new BattleEventArgs();

        public event EventHandler<BattleEventArgs> OnBattleEvent;
        //public event EventHandler OnTalkingEvent;
        public Map Map = new Map();
        public void Start(Unit mainHero, Unit boss)
        {
            var rnd = new Random();
            Map.Size = rnd.Next(10,20);

            Map[0] = mainHero;
            mainHero.Location = 0;
            Map[Map.Size - 1] = boss;
            boss.Location = Map.Size - 1;

            NewGame();
        }
        public void Load(string saveName)
        {
            
        }
        public void Save(string saveName)
        {

        }
        public void Meeting(Unit A,BattleEventArgs e)
        {
            //Здесь можно сделать условие на дружественный\нет (При дружеском токинг ивент)
            OnBattleEvent?.Invoke(this,e);
        }

        private void NewGame()
        {
            List<Unit> unitsList = new List<Unit>();
            List<Loot> lootList = new List<Loot>();

            var rnd = new Random();

            bool loot;

            for (int i = 1; i < Map.Size - 1; i++)
            {
                loot = Convert.ToBoolean(rnd.Next(0, 2));
                if (loot)
                {
                    lootList.Add(new Loot(rnd.Next(0, 501)));
                    Map[i] = lootList[lootList.Count - 1];
                    lootList[lootList.Count - 1].Location = i;
                }
                else
                {
                    unitsList.Add(new Unit($"Enemy #{unitsList.Count + 1}", rnd.Next(5, 21), 0, rnd.Next(0, 3), rnd.Next(3, 8)));
                    Map[i] = unitsList[unitsList.Count - 1];
                    unitsList[unitsList.Count - 1].Location = i;
                }
            }
        }
        //public int FindObjectOnMap(object a)
        //{
        //    int x;
        //    for (x = 0; x < game.Map.Size; x++)
        //    {
        //        if (game.Map[x] == a)
        //            break;
        //    }
        //    return x;
        //}
    }
}
