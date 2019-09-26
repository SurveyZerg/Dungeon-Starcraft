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
        public static bool End = false;
        public BattleEventArgs BattleEventArgs = new BattleEventArgs();

        public event EventHandler<BattleEventArgs> OnBattleEvent;
        //public event EventHandler OnTalkingEvent;
        public Map Map = new Map();
        public void Start()
        {
            int size = 10; //Нужно считывать это значение с файла
            Map.Size = size;
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
    }
}
