using System;
using System.Collections.Generic;
using System.IO;

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

        public void Start(ref Unit mainHero,ref Unit boss)
        {
            var rnd = new Random();
            Map.Size = rnd.Next(10,20);

            Map[0] = mainHero;
            mainHero.Location = 0;
            Map[Map.Size - 1] = boss;
            boss.Location = Map.Size - 1;

            NewGame();
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

        public void Load(string savename,ref Unit mainHero,ref Unit boss)
        {
            using (StreamReader sr = new StreamReader($"saves/{savename}"))
            {
                //Поиск размера
                string buff = sr.ReadLine();
                buff = buff.Remove(0, (buff.IndexOf("-") + 2));
                Map.Size = Convert.ToInt32(buff);
                //

                //Поиск всего, что лежит на карте
                for (int i = 0; !sr.EndOfStream; i++)
                {
                    buff = sr.ReadLine();
                    if (buff.Contains("null"))
                    {
                        Map[i] = null;
                    }
                    else
                    {
                        string typeOfObject = buff.Substring(0, buff.IndexOf(":"));
                        Unit a = (Unit)Map[i];
                        Loot b = (Loot)Map[i];
                        List<string> attributes = new List<string>();
                        for (int j = 0; buff != ""; j++)
                        {
                            buff = buff.Remove(0, (buff.IndexOf("-") + 2));
                            attributes.Add(buff.Substring(0, buff.IndexOf(",")));
                            buff = buff.Remove(0, (buff.IndexOf(",") + 2));
                        }


                        if (typeOfObject == "MainHero" || typeOfObject == "Boss" || typeOfObject == "Unit")
                        {
                            a = new Unit(attributes[0], Convert.ToInt32(attributes[1]), Convert.ToInt32(attributes[2]), Convert.ToInt32(attributes[3]), Convert.ToInt32(attributes[4]));
                            a.Money = Convert.ToInt32(attributes[5]);
                            a.Location = Convert.ToInt32(attributes[6]);
                            Map[i] = a;
                            //Костыль для босса и героя
                            if (typeOfObject == "MainHero")
                                mainHero = a;
                            else if (typeOfObject == "Boss")
                                boss = a;
                        }
                        else if (typeOfObject == "Loot")
                        {
                            b = new Loot(Convert.ToInt32(attributes[0]));
                            b.Location = Convert.ToInt32(attributes[1]);
                            Map[i] = b;
                        }
                    }
                }
            }
        }
        public static string GetSaveName (int saveNumber)
        {
            using (StreamReader sr = new StreamReader("saves/savelog.txt"))
            {
                if (sr.ReadLine() == null)
                {
                    throw new ArgumentException("Сохранений не существует!\nНельзя позволять пользователю загружать игру, если он ни разу не сохранялся\n");
                }
                else
                {
                    sr.DiscardBufferedData();
                    sr.BaseStream.Seek(0, SeekOrigin.Begin);
                    for (int i = 1; i < saveNumber; i++)
                    {
                        sr.ReadLine();
                    }
                    string savename = sr.ReadLine();
                    return savename;
                }
            }
        }

        public void Save(string savename, Unit MainHero, Unit Boss)
        {
            using (StreamWriter sw = new StreamWriter($"saves/savelog.txt", true))
            {
                sw.WriteLine($"{savename}.txt");
            }
            using (StreamWriter sw = new StreamWriter($"saves/{savename}.txt"))
            {
                sw.WriteLine($"Map Size - {Map.Size}");
                for (int i = 0; i < MainHero.Location; i++)
                {
                    sw.WriteLine($"null: Location - {i}");
                }
                for (int i = MainHero.Location; i < Map.Size; i++)
                {
                    if (Map[i] is Unit)
                    {
                        Unit temp = (Unit)Map[i];
                        string type;
                        if (Map[i] == MainHero)
                            type = "MainHero";
                        else if (Map[i] == Boss)
                            type = "Boss";
                        else
                            type = "Unit";
                        sw.WriteLine($"{type}: Name - {temp.Name}, HP - {temp.HP}, Mana - {temp.Mana}, Armor - {temp.Armor}, Damage - {temp.Damage}, Gold - {temp.Money}, Location - {temp.Location}, ");
                    }
                    else if (Map[i] is Loot)
                    {
                        Loot temp = (Loot)Map[i];
                        sw.WriteLine($"Loot: Gold - {temp.Money}, Location - {temp.Location}, ");
                    }
                }
            }
        }

        public void Meeting(Unit A,BattleEventArgs e)
        {
            //Здесь можно сделать условие на дружественный\нет (При дружеском токинг ивент)
            OnBattleEvent?.Invoke(this,e);
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
