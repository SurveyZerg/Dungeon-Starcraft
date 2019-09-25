using System;

namespace Dungeon_Starcraft
{
    class Program
    {
        static void Main(string[] args)
        {
            Game Game = new Game();
            Game.Start();
            Unit MainHero = new Unit();
            //Нужно вывести список сейвов и дать выбор пользователю
            Game.Load("Save#1");

        }
    }
}
