using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon_Starcraft
{
    class Game
    {
        public Map Map = new Map();
        public void Start()
        {
            
        }
        public void Load(string saveName)
        {
            int size = 100; //Нужно считывать это значение с файла
            Map.Size = size;
        }
        public void Save(string saveName)
        {

        }
    }
}
