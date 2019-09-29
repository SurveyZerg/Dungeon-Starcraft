using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon_Starcraft
{
    class Map
    {
        private object[] ObjectOnMap = new object[0];
        public object this[int i]
        {
            get => ObjectOnMap[i];
            set => ObjectOnMap[i] = value;
        }
        private int size = 1;
        public static int MapSizeForClasses = 1;
        public int Size
        {
            get
            {
                return size;
            }
            set
            {
                if (value > 0 && value >= size)
                {
                    size = value;
                    Array.Resize(ref ObjectOnMap, size);
                    MapSizeForClasses = value;
                }
            }
        }
    }
}
